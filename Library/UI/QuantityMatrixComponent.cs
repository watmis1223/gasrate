using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.XtraGrid.Columns;

namespace CalculationOilPrice.Library.UI
{
    public partial class QuantityMatrixComponent : UserControl
    {
        public delegate void SelectWIRChangedCallback(decimal WIR, decimal wirValue);
        public event SelectWIRChangedCallback SelectWIRChanged;

        GridColumn selectedWIRColumn;
        decimal selectedWIRValue;
        DataTable matrixDt;        

        public QuantityMatrixComponent()
        {
            InitializeComponent();
        }

        public void Reload(bool bindingData, bool enableEdit)
        {
            DataColumn[] orderFields = { new DataColumn("QuantityKG") };
            matrixDt = Storage.StorageOperator.LoadTable("OilMatrix", null, null, orderFields);

            if (matrixDt != null)
            {
                matrixDt.Columns["QuantityKG"].Caption = "Menge";
                matrixDt.Columns["QuantityKG"].ReadOnly = !enableEdit;                

                for (int i = 1; i <= matrixDt.Columns.Count - 1; i++)
                {
                    matrixDt.Columns[i].Caption = string.Format("{0}%", matrixDt.Columns[i].ColumnName);
                }
            }            

            if (bindingData)
            {
                wirMatrixGridControl.DataSource = matrixDt;
                wirMatrixGridView.RefreshData();
                wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatString = "n0";
                wirMatrixGridView.Columns["QuantityKG"].Width = 80;
                wirMatrixGridView.Columns["QuantityKG"].OptionsColumn.AllowFocus = false;
                wirMatrixGridView.Columns["QuantityKG"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            selectedWIRColumn = null;            
        }

        public void AllowAddNewItem()
        {
            wirMatrixGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            wirMatrixGridView.Columns["QuantityKG"].OptionsColumn.AllowFocus = true;
        }

        public void SetWIR(decimal partOfWIR)
        {
            GridColumn col = null;

            for(int i =0; i<= wirMatrixGridView.Columns.Count -1;i ++)
            {
                if (wirMatrixGridView.Columns[i].FieldName == partOfWIR.ToString("N0"))
                {
                    col = wirMatrixGridView.Columns[i];
                    break;
                }
            }

            if (col != null)
            {
                wirMatrixGridView.FocusedColumn = col;
                selectedWIRColumn = col;
                selectedWIRValue = 0;

                if (wirMatrixGridView.GetDataRow(0) != null)
                {
                    selectedWIRValue = Convert.ToDecimal(wirMatrixGridView.GetDataRow(0)[col.FieldName]);
                }

                if (SelectWIRChanged != null)
                {
                    SelectWIRChanged(Convert.ToDecimal(selectedWIRColumn.FieldName), selectedWIRValue);
                }
            }            
        }

        public void Filter(decimal quantityKgValue)
        {
            if (matrixDt == null)
            {
                return;
            }

            matrixDt.DefaultView.RowFilter = string.Format("QuantityKG={0}", quantityKgValue);

            wirMatrixGridControl.DataSource = matrixDt.DefaultView;
            wirMatrixGridView.RefreshData();
            wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatString = "n0";
            wirMatrixGridView.Columns["QuantityKG"].Width = 100;
            wirMatrixGridView.Columns["QuantityKG"].OptionsColumn.AllowFocus = false;
            wirMatrixGridView.Columns["QuantityKG"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
            wirMatrixGridView.OptionsBehavior.Editable = false;
            wirMatrixGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
            wirMatrixGridView.Appearance.FocusedCell.BackColor = Color.Orange;
            wirMatrixGridView.OptionsView.ShowIndicator = false;

            if (SelectWIRChanged != null)
            {
                if (selectedWIRColumn == null)
                {
                    selectedWIRColumn = wirMatrixGridView.Columns["10"];                 
                }

                wirMatrixGridView.FocusedColumn = selectedWIRColumn;
                selectedWIRValue = Convert.ToDecimal(wirMatrixGridView.GetRowCellValue(0, selectedWIRColumn));
                SelectWIRChanged(Convert.ToDecimal(selectedWIRColumn.FieldName), selectedWIRValue);
            }                        
        }

        public void FilterInRange(decimal quantityKgValue)
        {
            if (matrixDt == null)
            {
                return;
            }

            object objMaxValue = matrixDt.Compute("max(QuantityKG)", "QuantityKG<=" + quantityKgValue + "");
            int maxValue = 0;

            if (objMaxValue != DBNull.Value)
            {
                maxValue = Convert.ToInt32(objMaxValue);
            }             

            matrixDt.DefaultView.RowFilter = string.Format("QuantityKG={0}", maxValue);

            wirMatrixGridControl.DataSource = matrixDt.DefaultView;
            wirMatrixGridView.RefreshData();
            wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            wirMatrixGridView.Columns["QuantityKG"].DisplayFormat.FormatString = "n0";
            wirMatrixGridView.Columns["QuantityKG"].Width = 100;
            wirMatrixGridView.Columns["QuantityKG"].OptionsColumn.AllowFocus = false;
            wirMatrixGridView.Columns["QuantityKG"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            wirMatrixGridView.OptionsBehavior.Editable = false;
            wirMatrixGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
            wirMatrixGridView.Appearance.FocusedCell.BackColor = Color.Orange;
            wirMatrixGridView.OptionsView.ShowIndicator = false;

            if (SelectWIRChanged != null)
            {
                if (selectedWIRColumn == null)
                {
                    selectedWIRColumn = wirMatrixGridView.Columns["10"];
                }

                wirMatrixGridView.FocusedColumn = selectedWIRColumn;
                selectedWIRValue = Convert.ToDecimal(wirMatrixGridView.GetRowCellValue(0, selectedWIRColumn));
                SelectWIRChanged(Convert.ToDecimal(selectedWIRColumn.FieldName), selectedWIRValue);
            }
        }

        public void Save()
        {
            Storage.StorageOperator.SaveTable(matrixDt, matrixDt.Columns["QuantityKG"], null);
        }

        private void wirMatrixGridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "QuantityKG")
            {
                selectedWIRColumn = wirMatrixGridView.Columns["10"];                
                selectedWIRValue = Convert.ToDecimal(wirMatrixGridView.GetRowCellValue(e.RowHandle, "10"));
            }
            else
            {                
                selectedWIRColumn = e.Column;
                selectedWIRValue = Convert.ToDecimal(e.CellValue);
            }

            if (SelectWIRChanged != null)
            {
                SelectWIRChanged(Convert.ToDecimal(selectedWIRColumn.FieldName), selectedWIRValue);
            }
        }

        private void wirMatrixGridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            
        }

        private void wirMatrixGridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DataRow row = wirMatrixGridView.GetDataRow(e.RowHandle);
            //row["MatrixID"] = Guid.NewGuid();
        }
    }
}
