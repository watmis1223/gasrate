using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CalculationOilPrice.Library.Entity;

namespace CalculationOilPrice.Library.UI
{
    public partial class SearchDialog : DevExpress.XtraEditors.XtraForm
    {
        public Order SelectedOrder;

        public SearchDialog()
        {
            InitializeComponent();
        }

        public void ReloadData()
        {
            orderSearchControl1.ReloadData();
        }

        private void orderSearchControl1_OnSelectedOrder(CalculationOilPrice.Library.Entity.Order order)
        {
            SelectedOrder = order;
            DialogResult = DialogResult.OK;
        }
    }
}