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
    public partial class PriceCtrlMain : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SettingSaveChangedCallback();
        public event SettingSaveChangedCallback SettingSaveChanged;

        PriceCalculationSetting _PriceCalculationSetting;

        public PriceCtrlMain()
        {
            InitializeComponent();
        }

        private void PriceCtrlMain_Load(object sender, EventArgs e)
        {
            ReloadPriceCalculationSetting(true);

            //set text line to general control
            generalCtrl1.SetTextLine(_PriceCalculationSetting.TextSetting);
        }

        void ReloadPriceCalculationSetting(bool refresh)
        {
            //load module settings
            if (refresh)
            {
                _PriceCalculationSetting = ApplicationOperator.GetPriceCalculationSetting();
            }
        }

        private void generalCtrl1_NewButtonClick()
        {
            calculationTabPage.PageVisible = true;
            mainTabControl.SelectedTabPage = calculationTabPage;

            //reload calculation control           
            calculationBasicCtrl1.NewCalculation(generalCtrl1.GetModel(), _PriceCalculationSetting.PriceSetting);
        }      
    }
}
