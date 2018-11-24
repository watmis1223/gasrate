﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CalculationOilPrice.Library.Entity;
using CalculationOilPrice.Library.Storage;
using CalculationOilPrice.Library.UI;
using CalculationOilPrice.Library.UI.PriceCalculation;
using CalculationOilPrice.Library.Global;

namespace CalculationOilPrice
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        OilCtrlMain _OilModule = new OilCtrlMain();
        PriceCtrlMain _PriceModule = new PriceCtrlMain();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _OilModule.Dock = DockStyle.Fill;
            _PriceModule.Dock = DockStyle.Fill;

            brBtnOil_ItemClick(this, null);
        }

        public void ShowModule(ApplicationModules module, params string[] arguments)
        {
            pnlMain.Controls.Clear();

            switch (module)
            {
                case ApplicationModules.OilModule:
                    pnlMain.Controls.Add(_OilModule);
                    break;
                case ApplicationModules.PriceModuleCalculation:
                    _PriceModule.SetArguments(arguments);
                    _PriceModule.ModuleCalculationMode();
                    pnlMain.Controls.Add(_PriceModule);
                    break;
                case ApplicationModules.PriceModuleSetting:
                    _PriceModule.SetArguments(arguments);
                    _PriceModule.ModuleSettingMode();
                    pnlMain.Controls.Add(_PriceModule);
                    break;
                    //default:
                    //    break;

            }
        }

        private void brBtnOil_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.OilModule);
        }

        private void brBtnPriceCalculation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleCalculation);
        }

        private void brBtnPriceSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowModule(ApplicationModules.PriceModuleSetting);
        }
    }
}