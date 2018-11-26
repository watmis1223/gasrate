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

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class PriceCtrlMain : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SettingSaveChangedCallback();
        public event SettingSaveChangedCallback SettingSaveChanged;

        PriceCalculationSetting _PriceCalculationSetting;

        List<ComboboxItemModel> _CalculationList = new List<ComboboxItemModel>();

        bool isCalculationTabVisible = false;

        public PriceCtrlMain()
        {
            InitializeComponent();
        }

        private void PriceCtrlMain_Load(object sender, EventArgs e)
        {
            mainTabControl.SelectedTabPage = generalTabPage;

            ReloadPriceCalculationSetting(true);

            //set text line to general control
            generalCtrl1.SetTextLine(_PriceCalculationSetting.TextSetting);

            lstCalculation.DataSource = _CalculationList;
        }

        public void ModuleSettingMode()
        {
            settingTabPage.PageVisible = true;
            generalTabPage.PageVisible = false;
            calculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = settingTabPage;
        }

        public void ModuleCalculationMode()
        {
            settingTabPage.PageVisible = false;
            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = isCalculationTabVisible;
            mainTabControl.SelectedTabPage = generalTabPage;
        }

        void AddCalculationListItem(CalculationModel model)
        {
            //add to tree list
            if (model == null)
            {
                return;
            }

            _CalculationList.Add(new ComboboxItemModel()
            {
                Caption = model.GeneralSetting.Remark,
                Value = model.ID,
                Model = model
            });

            lstCalculation.Refresh();
        }

        public void ModuleCalculationByProffixMode(string[] arguments)
        {
            settingTabPage.PageVisible = false;
            generalTabPage.PageVisible = true;
            calculationTabPage.PageVisible = false;
            mainTabControl.SelectedTabPage = generalTabPage;

            //if call from proffix, arguments should not null
            ProffixModel oProffix = new ProffixModel();
            oProffix.SetModel(arguments);
            generalCtrl1.SetProffixParam(oProffix, _PriceCalculationSetting.ProffixConnection);

            if (oProffix.IsLoad)
            {
                calculationTabPage.PageVisible = true;
            }
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
            isCalculationTabVisible = true;
            calculationTabPage.PageVisible = true;
            mainTabControl.SelectedTabPage = calculationTabPage;

            //reload calculation control           
            calculationBasicCtrl1.NewCalculation(generalCtrl1.GetModel(), _PriceCalculationSetting);
            AddCalculationListItem(calculationBasicCtrl1.GetModel());            
        }

        private void MainTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            
        }
    }
}
