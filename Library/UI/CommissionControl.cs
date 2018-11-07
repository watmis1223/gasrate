using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CalculationOilPrice.Library.Entity;

namespace CalculationOilPrice.Library.UI
{
    public partial class CommissionControl : UserControl
    {
        public Order currentOrder;

        public CommissionControl()
        {
            InitializeComponent();
        }

        private void CommissionControl_Load(object sender, EventArgs e)
        {
            
        }

        public void CalculateCommission(Order order)
        {
            currentOrder = order;            
            orderQuantityButtonEdit.Text = currentOrder.OilQuantity.ToString("n2");
            purchasePriceButtonEdit.Text = currentOrder.OilPrice.ToString("n2");
            commissionMatrixControl.CalculateCommission(currentOrder);
        }
    }
}
