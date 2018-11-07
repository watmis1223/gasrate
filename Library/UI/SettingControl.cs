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

namespace CalculationOilPrice.Library.UI
{
    public partial class SettingControl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SettingSaveChangedCallback();
        public event SettingSaveChangedCallback SettingSaveChanged;

        GeneralSetting generalSetting;

        public SettingControl()
        {
            InitializeComponent();
            generalSetting = ApplicationOperator.GetGeneralSetting();
        }

        private void SettingControl_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        public void ReloadData()
        {
            try
            {
                isCHFFixPriceCheckEdit.CheckState = CheckState.Unchecked;

                fixCostUnloadPlaceTextEdit.Text = generalSetting.OilPriceSetting.FixCostForUnloadPlace.ToString();
                densityOfHeatingOilTextEdit.Text = generalSetting.OilPriceSetting.DensityOfHeatOil.ToString();
                currentCurrencyRateTextEdit.Text = generalSetting.OilPriceSetting.CurrentCurrencyRate.ToString();
                extraLightOilUSPriceTextEdit.Text = generalSetting.OilPriceSetting.ExtraLightOilUSPrice.ToString();
                ecoOilUSPriceTextEdit.Text = generalSetting.OilPriceSetting.EcoOilUSPrice.ToString();
                commissionFactorTextEdit.Text = generalSetting.OilPriceSetting.CommissionFactor.ToString();
                reportPathButtonEdit.Text = generalSetting.ReportPathSetting.ReportPath;

                if (generalSetting.OilPriceSetting.IsFixCHFPrices)
                {
                    isCHFFixPriceCheckEdit.CheckState = CheckState.Checked;
                }

                UpdateOilCHFPrice();

                wirMatrixGridComponent.Reload(true, true);
                wirMatrixGridComponent.AllowAddNewItem();
                commissionInfoComponent.Reload();
            }
            catch { }            
        }

        void UpdateOilCHFPrice()
        {
            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Unchecked)
            {
                extraLightOilCHFPriceTextEdit.Text = (generalSetting.OilPriceSetting.ExtraLightOilUSPrice * generalSetting.OilPriceSetting.CurrentCurrencyRate).ToString("n2");
                ecoOilCHFPriceTextEdit.Text = (generalSetting.OilPriceSetting.EcoOilUSPrice * generalSetting.OilPriceSetting.CurrentCurrencyRate).ToString("n2");
            }

            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Checked)
            {
                extraLightOilCHFPriceTextEdit.Text = generalSetting.OilPriceSetting.FixExtraLightOilCHFPrice.ToString("n2");
                ecoOilCHFPriceTextEdit.Text = generalSetting.OilPriceSetting.FixEcoOilCHFPrice.ToString("n2");
            }
        }

        decimal GetDoubleValueFromString(string stringValue)
        {
            decimal returnValue = 0;

            if (stringValue.StartsWith("."))
            {
                returnValue = 0;
            }

            if (string.IsNullOrEmpty(stringValue))
            {
                returnValue = 0;
            }
            else
            {
                returnValue = decimal.Parse(stringValue);
            }

            return returnValue;
        }

        private void extraLightOilUSPriceTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.ExtraLightOilUSPrice = GetDoubleValueFromString(extraLightOilUSPriceTextEdit.Text);
            UpdateOilCHFPrice();
        }

        private void ecoOilUSPriceTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.EcoOilUSPrice = GetDoubleValueFromString(ecoOilUSPriceTextEdit.Text);
            UpdateOilCHFPrice();
        }

        private void extraLightOilCHFPriceTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Checked)
            {
                generalSetting.OilPriceSetting.FixExtraLightOilCHFPrice = GetDoubleValueFromString(extraLightOilCHFPriceTextEdit.Text);
            }
        }

        private void ecoOilCHFPriceTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Checked)
            {
                generalSetting.OilPriceSetting.FixEcoOilCHFPrice = GetDoubleValueFromString(ecoOilCHFPriceTextEdit.Text);
            }
        } 

        private void currentCurrencyRateTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.CurrentCurrencyRate = GetDoubleValueFromString(currentCurrencyRateTextEdit.Text);
            UpdateOilCHFPrice();
        }

        private void fixCostUnloadPlaceTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.FixCostForUnloadPlace = GetDoubleValueFromString(fixCostUnloadPlaceTextEdit.Text);
        }

        private void densityOfHeatingOilTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.DensityOfHeatOil = GetDoubleValueFromString(densityOfHeatingOilTextEdit.Text);
        }

        private void commissionFactorTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.OilPriceSetting.CommissionFactor = GetDoubleValueFromString(commissionFactorTextEdit.Text);
        }

        private void reportPathButtonEdit_EditValueChanged(object sender, EventArgs e)
        {
            generalSetting.ReportPathSetting.ReportPath = reportPathButtonEdit.Text;
        }

        private void reportPathButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                reportPathButtonEdit.Text = fd.SelectedPath;
                generalSetting.ReportPathSetting.ReportPath = reportPathButtonEdit.Text;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            wirMatrixGridComponent.Save();
            commissionInfoComponent.Save();
            ApplicationOperator.SaveGeneralSetting(generalSetting);

            if (SettingSaveChanged != null)
            {
                SettingSaveChanged();
            }
        }

        private void isCHFFixPriceCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Checked)
            {
                generalSetting.OilPriceSetting.IsFixCHFPrices = true;
                extraLightOilCHFPriceTextEdit.Properties.ReadOnly = false;
                ecoOilCHFPriceTextEdit.Properties.ReadOnly = false;
            }

            if (isCHFFixPriceCheckEdit.CheckState == CheckState.Unchecked)
            {
                generalSetting.OilPriceSetting.IsFixCHFPrices = false;
                extraLightOilCHFPriceTextEdit.Properties.ReadOnly = true;
                ecoOilCHFPriceTextEdit.Properties.ReadOnly = true;                
            }

            UpdateOilCHFPrice();
        }                       
    }
}
