using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Xml;
using CalculationOilPrice.Library.Storage;
using CalculationOilPrice.Library.Entity.Setting;
using System.Globalization;
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation;
using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class GeneralCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void NewButtonClickCallback();
        public event NewButtonClickCallback NewButtonClick;

        GeneralSettingModel _Model;
        ProffixModel _ProffixModel;

        public GeneralSettingModel GetModel()
        {
            return _Model;
        }

        public void SetProffixParam(ProffixModel model, string connectionString)
        {
            //if proffix model needed
            _ProffixModel = model;

            //load proffix product
            ProffixLAGArtikelModel oLAGArtikel =
                StorageOperator.GetProffixLAGArtikelModel(model.LAGDokumenteArtikelNrLAG, connectionString);
            if (oLAGArtikel != null)
            {
                string[] oLines = new string[6];
                oLines[0] = !String.IsNullOrWhiteSpace(oLAGArtikel.ArtikelNrLAG) ? oLAGArtikel.ArtikelNrLAG : "-";
                oLines[1] = !String.IsNullOrWhiteSpace(oLAGArtikel.Bezeichnung1) ? oLAGArtikel.Bezeichnung1 : "-";
                oLines[2] = !String.IsNullOrWhiteSpace(oLAGArtikel.Bezeichnung2) ? oLAGArtikel.Bezeichnung2 : "-";
                oLines[3] = !String.IsNullOrWhiteSpace(oLAGArtikel.Bezeichnung3) ? oLAGArtikel.Bezeichnung3 : "-";
                oLines[4] = !String.IsNullOrWhiteSpace(oLAGArtikel.Bezeichnung4) ? oLAGArtikel.Bezeichnung4 : "-";
                oLines[5] = !String.IsNullOrWhiteSpace(oLAGArtikel.Bezeichnung5) ? oLAGArtikel.Bezeichnung5 : "-";
                txtProductDesc.Lines = oLines;
            }


            //load proffix supplier
            List<ProffixLAGLieferantenModel> oLAGLieferantenList =
                StorageOperator.GetProffixLAGLieferantenModelList(model.LAGDokumenteArtikelNrLAG, connectionString);
            if (oLAGLieferantenList != null)
            {
                ddSupplier.Properties.Items.Clear();
                //ddSupplier.Properties.Items.Add(new ComboboxItemModel() { Caption = "-", Value = 0 });
                foreach (ProffixLAGLieferantenModel item in oLAGLieferantenList)
                {
                    ddSupplier.Properties.Items.Add(new ComboboxItemModel() { Caption = item.Name, Value = item.LaufNr });
                }
            }

            if (model.IsNew)
            {
                //load proffix document
                ProffixLAGDokumente oProffixLAGDokumente = StorageOperator.GetProffixLAGDokumente(model.LAGDokumenteArtikelNrLAG, model.CalculationID, connectionString);
                if (oProffixLAGDokumente != null)
                {
                    txtRemark.Text = oProffixLAGDokumente.Bemerkungen;
                }

                //new from proffix
                dtCreate.EditValue = DateTime.Now;
                btnNew.Enabled = true;
                btnReset.Enabled = false;
            }
            else
            {
                //load from proffix
                btnNew.Enabled = false;
                btnReset.Enabled = true;

                //load cal setting from db
                CalculationModel oCal = StorageOperator.CalPriceLoadByID(Convert.ToInt64(model.CalculationID));

                if (oCal != null)
                {
                    rdoCostTypeList.EditValue = oCal.GeneralSetting.CostType;
                    rdoCostTypeList.Enabled = true;

                    // M R A
                    chkOptionList.Items["M"].Enabled = false;
                    chkOptionList.Items["M"].CheckState = oCal.GeneralSetting.Options.Contains("M") ? CheckState.Checked : CheckState.Unchecked;
                    chkOptionList.Items["R"].CheckState = oCal.GeneralSetting.Options.Contains("R") ? CheckState.Checked : CheckState.Unchecked;
                    chkOptionList.Items["A"].CheckState = oCal.GeneralSetting.Options.Contains("A") ? CheckState.Checked : CheckState.Unchecked;

                    //text line group
                    layoutControlGroup3.Enabled = false;

                    //price scale group
                    layoutControlGroup5.Enabled = false;
                    rdoAmountMarkupList.EditValue = oCal.GeneralSetting.PriceScale.MarkUp;
                    numPriceScale.Value = oCal.GeneralSetting.PriceScale.Scale;
                    txtMinProfit.Text = oCal.GeneralSetting.PriceScale.MinProfit.ToString();
                    txtMaxProfit.Text = oCal.GeneralSetting.PriceScale.MaxProfit.ToString();

                    //convert
                    rdoUnitList.EditValue = oCal.GeneralSetting.Convert.Mode;
                    txtSaleUnit.Text = oCal.GeneralSetting.Convert.SaleUnit;
                    txtShopUnit.Text = oCal.GeneralSetting.Convert.ShopUnit;
                    txtUnitNumber.Text = oCal.GeneralSetting.Convert.UnitNumber.ToString();

                    //currency
                    rdoCurrencyList.EditValue = oCal.GeneralSetting.Currency.Mode;
                    txtConvertCurrency.Text = oCal.GeneralSetting.Currency.Currency != "CHF" ? oCal.GeneralSetting.Currency.Currency : txtConvertCurrency.Text;
                    txtExchangeRate.Text = oCal.GeneralSetting.Currency.Rate.ToString();

                    //txtRemark.Text = oCal.GeneralSetting.Remark;
                    txtInfo.Text = oCal.GeneralSetting.Info;
                    txtEmployee.Text = oCal.GeneralSetting.Employee;
                    dtCreate.EditValue = oCal.GeneralSetting.CreateDate;

                    try
                    {
                        for (int i = 0; i < ddSupplier.Properties.Items.Count; i++)
                        {
                            if (((ComboboxItemModel)ddSupplier.Properties.Items[i]).Caption == oCal.GeneralSetting.Supplier)
                            {
                                ddSupplier.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public void UINewCalculationMode()
        {
            if (rdoCostTypeList.SelectedIndex == 0)
            {
                UINewBasicCalculationMode();
            }
            else
            {
                UINewPurchaseCalculationMode();
            }
        }

        private void UINewBasicCalculationMode()
        {
            rdoCostTypeList.Enabled = false;
            chkOptionList.Items[0].Enabled = false;

            //text line group
            layoutControlGroup3.Enabled = false;

            //price scale group
            layoutControlGroup5.Enabled = false;
        }

        private void UINewPurchaseCalculationMode()
        {
            rdoCostTypeList.Enabled = false;
            chkOptionList.Items[0].Enabled = false;
        }

        public GeneralCtrl()
        {
            InitializeComponent();
        }

        internal void SetTextLine(TextSetting setting)
        {
            chkTextList.Items[0].Description = setting.Text1;
            chkTextList.Items[0].Value = setting.Text1;

            chkTextList.Items[1].Description = setting.Text2;
            chkTextList.Items[1].Value = setting.Text2;

            chkTextList.Items[2].Description = setting.Text3;
            chkTextList.Items[2].Value = setting.Text3;

            chkTextList.Items[3].Description = setting.Text4;
            chkTextList.Items[3].Value = setting.Text4;

            chkTextList.Items[4].Description = setting.Text5;
            chkTextList.Items[4].Value = setting.Text5;

            chkTextList.Items[5].Description = setting.Text6;
            chkTextList.Items[5].Value = setting.Text6;

            chkTextList.Items[6].Description = setting.Text7;
            chkTextList.Items[6].Value = setting.Text7;

            chkTextList.Items[7].Description = setting.Text8;
            chkTextList.Items[7].Value = setting.Text8;

            chkTextList.Items[8].Description = setting.Text9;
            chkTextList.Items[8].Value = setting.Text9;

            chkTextList.Items[9].Description = setting.Text10;
            chkTextList.Items[9].Value = setting.Text10;
        }

        public void SetSettingModel()
        {
            _Model = new GeneralSettingModel()
            {
                CostType = rdoCostTypeList.EditValue.ToString(),
                Remark = txtRemark.Text,
                Supplier = ddSupplier.SelectedItem.ToString(),
                Info = txtInfo.Text,
                Employee = txtEmployee.Text,
                CreateDate = dtCreate.DateTime.ToString("yyyy-MM-dd", new CultureInfo("en-US")),
                PriceScale = new GeneralPriceScale()
                {
                    MarkUp = rdoAmountMarkupList.EditValue.ToString(),
                    Scale = numPriceScale.Value,
                    MinProfit = Convert.ToDecimal(txtMinProfit.Text),
                    MaxProfit = Convert.ToDecimal(txtMaxProfit.Text)
                },
                Convert = new GeneralConvert()
                {
                    Mode = rdoUnitList.EditValue.ToString(),
                    SaleUnit = txtSaleUnit.Text,
                    ShopUnit = txtShopUnit.Text,
                    UnitNumber = Convert.ToDecimal(txtUnitNumber.Text)
                },
                Currency = new GeneralCurrency()
                {
                    Mode = rdoCurrencyList.EditValue.ToString(),
                    Currency = rdoCurrencyList.EditValue.ToString() == "A" ? "CHF" : txtConvertCurrency.Text,
                    Rate = Convert.ToDecimal(txtExchangeRate.Text)
                },
                Options = getSelectedOption(),
                TextLines = getSelectedTextLine(),
                ProductDesc = getProductDesc(),
                ProffixModel = _ProffixModel
            };

            try
            {
                _Model.Convert.EEUnitNumber = Convert.ToDecimal(new string(_Model.Convert.ShopUnit.Where(item => Char.IsDigit(item)).ToArray()));
            }
            catch
            {
                _Model.Convert.EEUnitNumber = 1;
            }

            try
            {
                _Model.Convert.VEUnitNumber = Convert.ToDecimal(new string(_Model.Convert.SaleUnit.Where(item => Char.IsDigit(item)).ToArray()));
            }
            catch
            {
                _Model.Convert.EEUnitNumber = 1;
            }
        }

        private GeneralProductDesc getProductDesc()
        {
            GeneralProductDesc oData = new GeneralProductDesc();

            if (txtProductDesc.Lines != null && txtProductDesc.Lines.Length == 6)
            {
                oData.Line1 = txtProductDesc.Lines[0];
                oData.Line2 = txtProductDesc.Lines[1];
                oData.Line3 = txtProductDesc.Lines[2];
                oData.Line4 = txtProductDesc.Lines[3];
                oData.Line5 = txtProductDesc.Lines[4];
                oData.Line6 = txtProductDesc.Lines[5];
            }

            return oData;
        }

        private List<string> getSelectedOption()
        {
            List<string> oList = new List<string>();

            foreach (CheckedListBoxItem item in chkOptionList.CheckedItems)
            {
                oList.Add(item.Value.ToString());
            }

            return oList;
        }

        private List<string> getSelectedTextLine()
        {
            List<string> oList = new List<string>();

            foreach (var item in chkTextList.CheckedItems)
            {
                oList.Add(item.ToString());
            }

            return oList;
        }

        private void Ctrl_Load(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (NewButtonClick != null)
            {
                //disable some controls
                UINewCalculationMode();

                //set model
                SetSettingModel();

                //invoke event
                NewButtonClick();
            }

            btnNew.Enabled = false;
            btnReset.Enabled = true;
        }

        private void rdoCostTypeList_EditValueChanged(object sender, EventArgs e)
        {
            if (rdoCostTypeList.EditValue.ToString() == "P")
            {
                chkOptionList.Items["M"].Enabled = false;
                layoutControlGroup3.Enabled = false;
                layoutControlGroup5.Enabled = false;
                layoutControlGroup7.Enabled = false;
                layoutControlGroup9.Enabled = false;
            }
            else
            {
                chkOptionList.Items["M"].Enabled = true;
                layoutControlGroup3.Enabled = true;
                layoutControlGroup5.Enabled = true;
                layoutControlGroup7.Enabled = true;
                layoutControlGroup9.Enabled = true;
            }
        }

        private void rdoUnitList_EditValueChanged(object sender, EventArgs e)
        {
            if (rdoUnitList.EditValue.ToString() == "A")
            {
                txtShopUnit.ReadOnly = true;
                txtSaleUnit.ReadOnly = true;
                txtUnitNumber.ReadOnly = true;
            }
            else
            {
                txtShopUnit.ReadOnly = false;
                txtSaleUnit.ReadOnly = false;
                txtUnitNumber.ReadOnly = false;
            }
        }

        private void txtShopUnit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            txtUnitNumber.Properties.Buttons[0].Caption = e.NewValue.ToString();
        }
        private void txtSaleUnit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            txtUnitNumber.Properties.Buttons[1].Caption = e.NewValue.ToString();
        }

        private void rdoCurrencyList_EditValueChanged(object sender, EventArgs e)
        {
            if (rdoCurrencyList.EditValue.ToString() == "A")
            {
                txtConvertCurrency.ReadOnly = true;
                txtExchangeRate.ReadOnly = true;
            }
            else
            {
                txtConvertCurrency.ReadOnly = false;
                txtExchangeRate.ReadOnly = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset calculation?", "Reset calculation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _Model = null;

                btnNew.Enabled = true;
                btnReset.Enabled = false;
            }
        }
    }
}
