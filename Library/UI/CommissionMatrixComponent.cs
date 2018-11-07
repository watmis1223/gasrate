using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CalculationOilPrice.Library.UI
{
    public partial class CommissionMatrixComponent : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable matrixDt;

        public CommissionMatrixComponent()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            DataColumn[] orderFields = { new DataColumn("MarginMin") };
            matrixDt = Storage.StorageOperator.LoadTable("CommissionMatrix", null, null, orderFields);

            if (matrixDt != null)
            {
                matrixDt.Columns["PercentOfCommission"].Caption = "Prozent %";
                matrixDt.Columns["MarginMin"].Caption = "Marge von";
                matrixDt.Columns["MarginMax"].Caption = "Marge bis";
            }

            commissionMatrixGridControl.DataSource = matrixDt;
            commissionMatrixGridView.RefreshData();
            commissionMatrixGridView.Columns["MatrixID"].Visible = false;
            commissionMatrixGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
        }

        public void Save()
        {
            Storage.StorageOperator.SaveTable(matrixDt, matrixDt.Columns["MatrixID"], null);
        }

        private void commissionMatrixGridView_Click(object sender, EventArgs e)
        {

        }

        private void commissionMatrixGridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DataRow row = commissionMatrixGridView.GetDataRow(e.RowHandle);
            row["MatrixID"] = Guid.NewGuid();
            //row["PercentOfCommission"] = 0;
            //row["MarginMin"] = 0;
            //row["MarginMax"] = 99999;
        }
    }
}
