using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CalculationOilPrice.Library.Entity.Setting;
using CalculationOilPrice.Library.Storage;
using CalculationOilPrice.Library.Entity;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Controls;

namespace CalculationOilPrice.Library.UI
{
    public partial class CalculationControl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void PrintInvoiceCallback();
        public event PrintInvoiceCallback PrintInvoice;

        public delegate void SavedCalculationCallback(Order order);
        public event SavedCalculationCallback SavedCalculation;

        GeneralSetting generalSetting;
        DataTable orderTable;        
        DataTable quantityLookupTable;
        DataTable unloadPlaceTable;
        //bool allowFilter = false;
        bool allowIncreaseCounter = false;

        public Order Order;

        public CalculationControl()
        {
            InitializeComponent();
            ReloadSetting();
            allowIncreaseCounter = true;
            InitValidationRules();
            holdCustomerInfoCheckEdit.CheckState = CheckState.Unchecked;

            orderQuantityKGLookupComboBoxEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            orderQuantityKGLookupComboBoxEdit.Properties.Mask.EditMask = @"\d*";
        }

        void ReloadSetting()
        {
            generalSetting = ApplicationOperator.GetGeneralSetting();
        }

        public void ReloadData()
        {
            if (allowIncreaseCounter)
            {
                generalSetting.CalculationCounter += 1;
                allowIncreaseCounter = false;
            }

            wirMatrixGridComponent.Reload(false, false);

            //orderID = Guid.NewGuid();
            if (Order == null)
            {
                Order = new Order();
                Order.OrderID = Guid.NewGuid();
            }

            InitQuantityLookupTable();
            InitUnloadPlaceTable();

            orderQuantityKGLookupComboBoxEdit.SelectedIndex = 0;
            qualityComboBoxEdit.SelectedIndex = 0;
            unloadPlaceSpinEdit.EditValue = 1;
            dateDateEdit.EditValue = DateTime.Now;

            if (holdCustomerInfoCheckEdit.CheckState == CheckState.Unchecked)
            {
                nameTextEdit.EditValue = string.Empty;
                surnamTextEdit.EditValue = string.Empty;
                addressMemoEdit.EditValue = string.Empty;
                zipCodeTextEdit.EditValue = string.Empty;
                placeTextEdit.EditValue = string.Empty;
            }

            customWirCheckEdit.CheckState = CheckState.Unchecked;
            calculationNumberTextEdit.Text = generalSetting.CalculationCounter.ToString();

            unloadPlaceNumberLookUpEdit.Properties.DataSource = unloadPlaceTable;
            unloadPlaceNumberLookUpEdit.Properties.PopulateColumns();
            unloadPlaceNumberLookUpEdit.Properties.DisplayMember = "UnloadPlaceNumber";
            unloadPlaceNumberLookUpEdit.Properties.ValueMember = "UnloadPlaceNumber";
            //allowFilter = false;
            unloadPlaceNumberLookUpEdit.EditValue = unloadPlaceTable.Rows[0]["UnloadPlaceNumber"];
            //allowFilter = true;

            ////Order.OrderID = Guid.NewGuid();
            //Order.SetOrderDateTimeInfo(DateTime.Parse(dateDateEdit.EditValue.ToString()));

            ////Order.OrderDate = DateTime.Parse(dateDateEdit.EditValue.ToString());
            
            //Order.OilQuality = qualityComboBoxEdit.SelectedIndex;
            //Order.OilQuantity = Convert.ToDecimal(orderQuantityKGLookupComboBoxEdit.SelectedItem);
            //Order.UnloadPlace = Convert.ToDecimal(unloadPlaceSpinEdit.EditValue);
            //Order.UnloadCost = generalSetting.OilPriceSetting.FixCostForUnloadPlace;
            //Order.CommissionFactor = generalSetting.OilPriceSetting.CommissionFactor;
            //Order.UnloadPlaceNumber = Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue);            
            //Order.OrderNumber = generalSetting.CalculationCounter;
            
            //Order.CustomerName = nameTextEdit.EditValue.ToString();
            //Order.CustomerSurname = surnamTextEdit.EditValue.ToString();
            //Order.CustomerAddress = addressMemoEdit.EditValue.ToString();
            //Order.CustomerZipcode = zipCodeTextEdit.EditValue.ToString();
            //Order.CustomerPlace = placeTextEdit.EditValue.ToString();

            BindNewOrder();
            InitNewTable("OilOrder", orderTable);
            BindOrderDetail(true, Guid.NewGuid());
            
            //oilOrderDetailControl.CreateOrderDetail(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));            
        }

        void BindOrderDetail(bool isNewOrder, Guid searchID)
        {
            oilOrderDetailControl.CreateOrderDetail(Order, searchID, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));

            //if (isNewOrder)
            //{
            //    //Guid newId = this.Order.OrderID;
            //    //this.Order.OrderID = Guid.Empty;
            //    oilOrderDetailControl.CreateOrderDetail(Order, searchID, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
            //    //this.Order.OrderID = newId;
            //}
            //else
            //{
            //    oilOrderDetailControl.CreateOrderDetail(Order, searchID, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue)); 
            //}            
        }

        void BindNewOrder()
        {
            //Order.OrderID = Guid.NewGuid();
            Order.SetOrderDateTimeInfo(DateTime.Parse(dateDateEdit.EditValue.ToString()));

            //Order.OrderDate = DateTime.Parse(dateDateEdit.EditValue.ToString());

            Order.OilQuality = qualityComboBoxEdit.SelectedIndex;

            Order.OilQuantity = 0;
            if (orderQuantityKGLookupComboBoxEdit.SelectedItem.ToString() != "")
            {
                Order.OilQuantity = Convert.ToDecimal(orderQuantityKGLookupComboBoxEdit.SelectedItem);
            }
            
            Order.UnloadPlace = Convert.ToDecimal(unloadPlaceSpinEdit.EditValue);
            Order.UnloadCost = generalSetting.OilPriceSetting.FixCostForUnloadPlace;
            Order.CommissionFactor = generalSetting.OilPriceSetting.CommissionFactor;
            Order.UnloadPlaceNumber = Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue);
            Order.OrderNumber = generalSetting.CalculationCounter;

            Order.CustomerName = nameTextEdit.EditValue.ToString();
            Order.CustomerSurname = surnamTextEdit.EditValue.ToString();
            Order.CustomerAddress = addressMemoEdit.EditValue.ToString();
            Order.CustomerZipcode = zipCodeTextEdit.EditValue.ToString();
            Order.CustomerPlace = placeTextEdit.EditValue.ToString();
        }

        void BindOrderView()
        {
            Order.UnloadCost = generalSetting.OilPriceSetting.FixCostForUnloadPlace;
            Order.CommissionFactor = generalSetting.OilPriceSetting.CommissionFactor;

            qualityComboBoxEdit.SelectedIndex = -1;
            qualityComboBoxEdit.SelectedIndex = this.Order.OilQuality;
            nameTextEdit.EditValue = Order.CustomerName;
            surnamTextEdit.EditValue = Order.CustomerSurname;
            addressMemoEdit.EditValue = Order.CustomerAddress;
            zipCodeTextEdit.EditValue = Order.CustomerZipcode;
            placeTextEdit.EditValue = Order.CustomerPlace;

            decimal unloadPlaceTmp = Order.UnloadPlace;
            unloadPlaceSpinEdit.EditValue = 0;
            unloadPlaceSpinEdit.EditValue = unloadPlaceTmp;
            Order.UnloadPlace = unloadPlaceTmp;

            decimal partOfWir = this.Order.PartOfWIR;
            int selectedQuantity = -1;
            orderQuantityKGLookupComboBoxEdit.SelectedIndex = selectedQuantity;
            for (int i = 0; i <= orderQuantityKGLookupComboBoxEdit.Properties.Items.Count - 1; i++)
            {
                if (orderQuantityKGLookupComboBoxEdit.Properties.Items[i].ToString() != "" 
                    && Convert.ToDecimal(orderQuantityKGLookupComboBoxEdit.Properties.Items[i]) == Order.OilQuantity)
                {
                    selectedQuantity = i;
                    break;
                }
            }

            partOfWir = this.Order.PartOfWIR;
            
            orderQuantityKGLookupComboBoxEdit.SelectedIndex = selectedQuantity;

            if (selectedQuantity == -1)
            {
                orderQuantityKGLookupComboBoxEdit.EditValue = Order.OilQuantity;
            }

            this.Order.PartOfWIR = partOfWir;
            wirMatrixGridComponent.SetWIR(this.Order.PartOfWIR);
        }

        void InitValidationRules()
        {
            // <notEmptyTextEdit> 
            ConditionValidationRule notEmptyValidationRule = new ConditionValidationRule();
            notEmptyValidationRule.ConditionOperator = ConditionOperator.IsNotBlank;
            notEmptyValidationRule.ErrorText = "Please enter a Name";
            // </notEmptyTextEdit>

            // <notEmptyTextEdit>
            dxValidationProvider.SetValidationRule(nameTextEdit, notEmptyValidationRule);
            //dxValidationProvider.SetValidationRule(surnamTextEdit, notEmptyValidationRule);
        }        
        
        public void ReloadSettingValue()
        {
            ReloadSetting();

            wirMatrixGridComponent.Reload(false, false);
            Order.UnloadCost = generalSetting.OilPriceSetting.FixCostForUnloadPlace;
            Order.CommissionFactor = generalSetting.OilPriceSetting.CommissionFactor;

            UpdateOilPriceCHF(qualityComboBoxEdit.SelectedIndex);
            UpdateWirRatePrice();
            oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
        }

        void InitNewTable(string tableName, DataTable table)
        {            
            DataColumn dc = new DataColumn("0");
            dc.DefaultValue = 1;
            orderTable = Storage.StorageOperator.LoadTable(tableName, null, new DataColumn[] { dc }, null);
        }

        void InitQuantityLookupTable()
        {
            DataColumn dc = new DataColumn("QuantityKG");
            quantityLookupTable = Storage.StorageOperator.LoadTable("OilMatrix", new DataColumn[] { dc }, null, null);

            orderQuantityKGLookupComboBoxEdit.Properties.Items.Clear();

            orderQuantityKGLookupComboBoxEdit.Properties.Items.Add("");

            if (quantityLookupTable != null)
            {
                foreach (DataRow dr in quantityLookupTable.Rows)
                {
                    orderQuantityKGLookupComboBoxEdit.Properties.Items.Add(dr["QuantityKG"]);
                }
            }            
        }

        void InitUnloadPlaceTable()
        {            
            unloadPlaceTable = new DataTable("UnloadPlace");

            DataColumn dc = new DataColumn("UnloadPlaceNumber");
            dc.DataType = typeof(Int32);
            unloadPlaceTable.Columns.Add(dc);

            DataRow dr = unloadPlaceTable.NewRow();            
            dr["UnloadPlaceNumber"] = 1;
            unloadPlaceTable.Rows.Add(dr);
        }

        void UpdateOilPriceCHF(int oilQuality)
        {
            switch (oilQuality)
            {
                case 0:
                    ///ExtraLightOilUSPrice
                    if (generalSetting.OilPriceSetting.IsFixCHFPrices)
                    {
                        Order.OilPrice = generalSetting.OilPriceSetting.FixExtraLightOilCHFPrice;
                    }
                    else
                    {
                        Order.OilPrice = (generalSetting.OilPriceSetting.ExtraLightOilUSPrice * generalSetting.OilPriceSetting.CurrentCurrencyRate);                        
                    }

                    Order.OilPrice = Math.Round(Order.OilPrice, 2, MidpointRounding.AwayFromZero);
                    priceFromQuantityKGTextEdit.Text = Order.OilPrice.ToString("n2");
                    break;
                case 1:
                    ///EcoOilUSPrice
                    if (generalSetting.OilPriceSetting.IsFixCHFPrices)
                    {
                        Order.OilPrice = generalSetting.OilPriceSetting.FixEcoOilCHFPrice;
                    }
                    else
                    {
                        Order.OilPrice = (generalSetting.OilPriceSetting.EcoOilUSPrice * generalSetting.OilPriceSetting.CurrentCurrencyRate);                        
                    }
                    
                    Order.OilPrice = Math.Round(Order.OilPrice, 2, MidpointRounding.AwayFromZero);
                    priceFromQuantityKGTextEdit.Text = Order.OilPrice.ToString("n2");
                    break;
            }
        }

        void SaveOrder()
        {            
            DataRow dr = orderTable.NewRow();

            dr["OrderID"] = Order.OrderID;
            
            //dr["OrderDate"] = dateDateEdit.EditValue.ToString();
            //dr["OilQuantity"] = orderQuantityKGLookupComboBoxEdit.EditValue;
            //dr["UnloadPlace"] = unloadPlaceSpinEdit.Value;
            //dr["PartOfWIR"] = wirTextEdit.Text;
            //dr["OilQualityIndex"] = qualityComboBoxEdit.SelectedIndex;

            dr["OrderDate"] = string.Format("{0} {1}", Order.OrderDate, Order.OrderTime);
            dr["OilQuantity"] = Order.OilQuantity;
            dr["UnloadPlace"] = Order.UnloadPlace;
            dr["PartOfWIR"] = Order.PartOfWIR;
            dr["OilQualityIndex"] = Order.OilQuality;


            dr["CustomerName"] = Order.CustomerName;
            dr["CustomerSurname"] = Order.CustomerSurname;
            dr["CustomerAddress"] = Order.CustomerAddress;
            dr["CustomerZipCode"] = Order.CustomerZipcode;
            dr["CustomerPlace"] = Order.CustomerPlace;

            
            dr["OilQualityPrice"] = Order.OilPrice;
            dr["WirFactor"] = Order.WIRFactor;
            dr["CurrencyRate"] = generalSetting.OilPriceSetting.CurrentCurrencyRate;

            dr["OrderNumber"] = Order.OrderNumber;
            dr["ReportName"] = Order.GetReportName();

            orderTable.Rows.Add(dr);
            Storage.StorageOperator.SaveTable(orderTable, orderTable.Columns["OrderID"], null);
            oilOrderDetailControl.SaveOrderDetail();            
        }

        void UpdateWirRatePrice()
        {            
            priceMultiplyRateTextEdit.Text = (Order.OilPrice * Order.WIRFactor).ToString("n2");
        }

        private void newOrderSimpleButton_Click(object sender, EventArgs e)
        {
            isSelectWIRChangedInvoked = false;
            orderQuantityKGLookupComboBoxEdit.SelectedIndex = -1;
            qualityComboBoxEdit.SelectedIndex = -1;
            wirRateTextEdit.EditValue = 0;
            
            ReloadData();            
        }

        private void saveOrderSimpleButton_Click(object sender, EventArgs e)
        {
            if (dxValidationProvider.Validate())
            {
                SaveOrder();
                ApplicationOperator.SaveGeneralSetting(generalSetting);
                allowIncreaseCounter = true;

                if (SavedCalculation != null)
                {
                    SavedCalculation(Order);                                        
                }
            }
        }

        private void reportSimpleButton_Click(object sender, EventArgs e)
        {
            if (dxValidationProvider.Validate())
            {
                if (PrintInvoice != null)
                {
                    PrintInvoice();
                }
            }
            //Invoice invoice = new Invoice();
            //invoice.CreateInvoice(Order);
        }

        private void dateDateEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void qualityComboBoxEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (((ComboBoxEdit)sender).EditValue == null)
            {
                return;
            }

            Order.OilQualityName = ((ComboBoxEdit)sender).EditValue.ToString();
            UpdateOilPriceCHF(((ComboBoxEdit)sender).SelectedIndex);
            UpdateWirRatePrice();
            oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
        }

        private void orderQuantityKGLookupComboBoxEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (((ComboBoxEdit)sender).EditValue == null)
            {
                return;
            }

            decimal quantityKG = 0;

            if (((ComboBoxEdit)sender).EditValue.ToString() != "")
            {
                quantityKG = Convert.ToDecimal(((ComboBoxEdit)sender).EditValue);
            }
                       
            orderQuantityLTextEdit.Text = Math.Round((quantityKG / generalSetting.OilPriceSetting.DensityOfHeatOil)).ToString("n2");

            //wirMatrixGridComponent.Filter(quantityKG);
            wirMatrixGridComponent.FilterInRange(quantityKG);
            Order.OilQuantity = quantityKG;
            oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
        }

        private void unloadPlaceSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            Order.UnloadPlace = Convert.ToDecimal(unloadPlaceSpinEdit.EditValue);
            oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));            
        }

        private void unloadPlaceSpinEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Convert.ToDecimal(e.NewValue) < 0)
            {
                unloadPlaceSpinEdit.EditValue = 0;
                e.Cancel = true;
            }
        }

        private void wirTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void nameTextEdit_EditValueChanged(object sender, EventArgs e)
        {            
            Order.CustomerName = nameTextEdit.EditValue.ToString();
        }

        private void surnamTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            Order.CustomerSurname = surnamTextEdit.EditValue.ToString();
        }

        private void addressMemoEdit_EditValueChanged(object sender, EventArgs e)
        {
            Order.CustomerAddress = addressMemoEdit.EditValue.ToString();
        }

        private void zipCodeTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            Order.CustomerZipcode = zipCodeTextEdit.EditValue.ToString();
        }

        private void placeTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            Order.CustomerPlace = placeTextEdit.EditValue.ToString();
        }

        bool isSelectWIRChangedInvoked = false;
        decimal tempWirFactor;
        decimal tempWir;

        private void wirMatrixGridComponent_SelectWIRChanged(decimal WIR, decimal wirValue)
        {
            isSelectWIRChangedInvoked = true;

            wirTextEdit.Text = string.Empty;

            if (wirValue > 0)
            {
                wirTextEdit.Text = WIR.ToString();
            }

            tempWir = WIR;
            tempWirFactor = wirValue;

            if (customWirCheckEdit.CheckState == CheckState.Unchecked)
            {
                wirRateTextEdit.Text = wirValue.ToString();
            }

            UpdateOilPriceCHF(qualityComboBoxEdit.SelectedIndex);
            UpdateWirRatePrice();

            Order.PartOfWIR = WIR;
            oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
            isSelectWIRChangedInvoked = false;
        }

        private void priceFromQuantityKGTextEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void wirRateTextEdit_EditValueChanged(object sender, EventArgs e)
        {            
            Order.WIRFactor = Convert.ToDecimal(wirRateTextEdit.EditValue);            

            if (!isSelectWIRChangedInvoked)
            {                
                UpdateOilPriceCHF(qualityComboBoxEdit.SelectedIndex);
                UpdateWirRatePrice();
                oilOrderDetailControl.UpdateOrder(Order, Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue));
            }
        }

        private void wirRateTextEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //if (Convert.ToDecimal(e.NewValue) <= 0)
            //{
            //    wirRateTextEdit.EditValue = 1;
            //    e.Cancel = true;
            //}
        }

        private void customWirCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (customWirCheckEdit.CheckState == CheckState.Unchecked)
            {
                wirMatrixGridComponent_SelectWIRChanged(tempWir, tempWirFactor);
                wirRateTextEdit.Properties.ReadOnly = true;
            }
            else
            {
                wirRateTextEdit.Properties.ReadOnly = false;
            }
        }
        
        private void unloadPlaceNumberLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //switch (e.Button.Kind)
            //{
            //    case DevExpress.XtraEditors.Controls.ButtonPredefines.Plus:
            //        int lastNumber = Convert.ToInt32(unloadPlaceTable.Rows[unloadPlaceTable.Rows.Count-1]["UnloadPlaceNumber"]);
            //        DataRow dr = unloadPlaceTable.NewRow();
            //        dr["UnloadPlaceNumber"] = lastNumber +1;
            //        unloadPlaceTable.Rows.Add(dr);
            //        unloadPlaceNumberLookUpEdit.EditValue = unloadPlaceTable.Rows[unloadPlaceTable.Rows.Count - 1]["UnloadPlaceNumber"];

            //        if (Convert.ToInt32(unloadPlaceSpinEdit.EditValue) < Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue))
            //        {
            //            unloadPlaceSpinEdit.EditValue = unloadPlaceNumberLookUpEdit.EditValue;
            //        }

            //        break;
            //}
        }

        private void unloadPlaceNumberLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            //Order.UnloadPlaceNumber = Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue);
            //oilOrderDetailControl.UpdateOrder(Order, Order.UnloadPlaceNumber);

            //if (allowFilter)
            //{
            //    Order.UnloadPlaceNumber = Convert.ToInt32(unloadPlaceNumberLookUpEdit.EditValue);
            //    oilOrderDetailControl.UpdateOrder(Order, Order.UnloadPlaceNumber);
            //}
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            isSelectWIRChangedInvoked = false;
            orderQuantityKGLookupComboBoxEdit.SelectedIndex = -1;
            qualityComboBoxEdit.SelectedIndex = -1;
            wirRateTextEdit.EditValue = 0;

            //ReloadData(); 
            Guid currentId = this.Order.OrderID;
            Guid searchId = Guid.Empty;
            SearchDialog dlg = new SearchDialog();
            
            dlg.ReloadData();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string orderDate = this.Order.OrderDate;
                int orderNumber = Order.OrderNumber;
                
                this.Order = dlg.SelectedOrder;
                searchId = this.Order.OrderID;
                this.Order.OrderID = currentId;
                this.Order.OrderDate = orderDate;
                this.Order.OrderNumber = orderNumber;
                                
            }

            //BindOrderDetail(false);
            BindOrderView();
            BindOrderDetail(false, searchId);
            
        }        
    }
}
