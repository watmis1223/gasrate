using System;
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

namespace CalculationOilPrice
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        OilCtrlMain ctrl1 = new OilCtrlMain();
        PriceCtrlMain ctrl2 = new PriceCtrlMain();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ctrl1.Dock = DockStyle.Fill;
            ctrl2.Dock = DockStyle.Fill;

            navBarItem3_LinkClicked(null, null);
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl2.Controls.Clear();            
            panelControl2.Controls.Add(ctrl1);
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl2.Controls.Clear();
            panelControl2.Controls.Add(ctrl2);
        }
    }
}