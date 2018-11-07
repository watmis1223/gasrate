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

namespace CalculationOilPrice.Library.UI
{
    public partial class OilCtrlMain : DevExpress.XtraEditors.XtraUserControl
    {
        public OilCtrlMain()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            calculationControl1.ReloadData();            
        }
        
        private void mainTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == commissionTabPage)
            {
                commissionControl1.CalculateCommission(calculationControl1.Order);
            }
        }

        void settingControl1_SettingSaveChanged()
        {
            calculationControl1.ReloadSettingValue();
        }

        private void calculationControl1_PrintInvoice()
        {
            Invoice invoice = new Invoice();
            commissionControl1.CalculateCommission(calculationControl1.Order);
            invoice.CreateInvoice(calculationControl1.Order);
        }

        void calculationControl1_SavedCalculation(Order order)
        {
            OrderInfo orderInfo = new OrderInfo(order);
            orderInfo.SaveOrderInfo();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ApplicationOperator.SaveGeneralSetting(generalSetting);
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }
    }
}