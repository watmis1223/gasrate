﻿using System;
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
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class GeneralCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void NewButtonClickCallback();
        public event NewButtonClickCallback NewButtonClick;

        GeneralSettingModel _Model;

        public GeneralSettingModel GetModel()
        {
            return _Model;
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
                Unit = new GeneralUnit()
                {
                    Mode = rdoUnitList.EditValue.ToString(),
                    SaleUnit = txtSaleUnit.Text,
                    ShopUnit = txtShopUnit.Text,
                    UnitNumber = (int)Convert.ToDecimal(txtUnitNumber.Text)
                },
                Currency = new GeneralCurrency()
                {
                    Mode = rdoCurrencyList.EditValue.ToString(),
                    Currency = txtConvertCurrency.Text,
                    Rate = Convert.ToDecimal(txtExchangeRate.Text)
                },
                Options = getSelectedOption(),
                TextLines = getSelectedTextLine()
            };
        }

        private List<string> getSelectedOption()
        {
            List<string> oList = new List<string>();

            foreach (var item in chkOptionList.SelectedItems)
            {
                oList.Add(item.ToString());
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

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (NewButtonClick != null)
            {
                //set model
                SetSettingModel();

                NewButtonClick();
            }
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
    }
}
