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

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class SettingCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SettingSaveChangedCallback();
        public event SettingSaveChangedCallback SettingSaveChanged;

        PriceCalculationSetting _Setting;

        public SettingCtrl()
        {
            InitializeComponent();
            _Setting = ApplicationOperator.GetPriceCalculationSetting();
        }

        private void SettingCtrl_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        public void ReloadData()
        {
            try
            {
                txtCashDiscount.EditValue = _Setting.PriceSetting.CashDiscount;
                txtSaleBonus.EditValue = _Setting.PriceSetting.SalesBonus;
                txtCustomerDiscount.EditValue = _Setting.PriceSetting.CustomerDiscount;
                txtVatTax.EditValue = _Setting.PriceSetting.VatTaxes;

                txtText1.EditValue = _Setting.TextSetting.Text1;
                txtText2.EditValue = _Setting.TextSetting.Text2;
                txtText3.EditValue = _Setting.TextSetting.Text3;
                txtText4.EditValue = _Setting.TextSetting.Text4;
                txtText5.EditValue = _Setting.TextSetting.Text5;
                txtText6.EditValue = _Setting.TextSetting.Text6;
                txtText7.EditValue = _Setting.TextSetting.Text7;
                txtText8.EditValue = _Setting.TextSetting.Text8;
                txtText9.EditValue = _Setting.TextSetting.Text9;
                txtText10.EditValue = _Setting.TextSetting.Text10;

                chkPrintPreview.CheckState = _Setting.ReportPathSetting.PrintPreview ? CheckState.Checked : CheckState.Unchecked;

                txtProffixConnection.EditValue = _Setting.ProffixConnection;
            }
            catch { }
        }

        void SetValues()
        {
            _Setting.PriceSetting.CashDiscount = Convert.ToDecimal(txtCashDiscount.Text);
            _Setting.PriceSetting.SalesBonus = Convert.ToDecimal(txtSaleBonus.Text);
            _Setting.PriceSetting.CustomerDiscount = Convert.ToDecimal(txtCustomerDiscount.Text);
            _Setting.PriceSetting.VatTaxes = Convert.ToDecimal(txtVatTax.Text);

            _Setting.TextSetting.Text1 = txtText1.Text;
            _Setting.TextSetting.Text2 = txtText2.Text;
            _Setting.TextSetting.Text3 = txtText3.Text;
            _Setting.TextSetting.Text4 = txtText4.Text;
            _Setting.TextSetting.Text5 = txtText5.Text;
            _Setting.TextSetting.Text6 = txtText6.Text;
            _Setting.TextSetting.Text7 = txtText7.Text;
            _Setting.TextSetting.Text8 = txtText8.Text;
            _Setting.TextSetting.Text9 = txtText9.Text;
            _Setting.TextSetting.Text10 = txtText10.Text;

            _Setting.ReportPathSetting.PrintPreview = chkPrintPreview.CheckState == CheckState.Checked;

            _Setting.ProffixConnection = txtProffixConnection.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetValues();
            ApplicationOperator.SavePriceCalculationSetting(_Setting);

            if (SettingSaveChanged != null)
            {
                SettingSaveChanged();
            }
        }
    }
}
