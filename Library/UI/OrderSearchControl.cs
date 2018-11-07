using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using CalculationOilPrice.Library.Entity;

namespace CalculationOilPrice.Library.UI
{
    public partial class OrderSearchControl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void OnSelectedOrderCallback(Order order);
        public event OnSelectedOrderCallback OnSelectedOrder;

        protected DataTable dtSearch;

        public OrderSearchControl()
        {
            InitializeComponent();
        }

        private void OrderSearchControl_Load(object sender, EventArgs e)
        {
            
        }

        public void ReloadData()
        {
            GetData();
            ResetCustomFilter();
        }

        void GetData()
        {
            try
            {
                DataColumn[] orderFields = { new DataColumn("OrderNumber") };

                dtSearch = new DataTable();
                dtSearch = Storage.StorageOperator.LoadTable("OilOrder", null, null, orderFields);

                gridControl1.DataSource = dtSearch;

                foreach (GridColumn col in gridView1.Columns)
                {
                    col.Visible = false;
                    col.OptionsFilter.AllowFilter = false;
                }

                gridView1.Columns["OrderDate"].Visible = true;
                gridView1.Columns["CustomerName"].Visible = true;
                gridView1.Columns["CustomerSurname"].Visible = true;                
                gridView1.Columns["OrderNumber"].Visible = true;

                //gridView1.Columns["OrderDate"].OptionsFilter.AllowFilter = true;
                //gridView1.Columns["CustomerName"].OptionsFilter.AllowFilter = true;
                //gridView1.Columns["CustomerSurname"].OptionsFilter.AllowFilter = true;
                //gridView1.Columns["OrderNumber"].OptionsFilter.AllowFilter = true;

                //gridView1.Columns["OrderDate"].DisplayFormat.Format = new base                
                gridView1.Columns["OrderDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                gridView1.Columns["OrderDate"].DisplayFormat.FormatString = "d/M/yyyy";

                gridView1.Columns["OrderNumber"].VisibleIndex = 0;
                gridView1.Columns["OrderDate"].VisibleIndex = 1;
                gridView1.Columns["CustomerName"].VisibleIndex = 2;
                gridView1.Columns["CustomerSurname"].VisibleIndex = 3;

                gridView1.Columns["OrderNumber"].BestFit();
                gridView1.Columns["OrderDate"].BestFit();
                gridView1.Columns["CustomerName"].BestFit();
                gridView1.Columns["CustomerSurname"].BestFit();
                
            }
            finally
            {

            }                


            //SetWaitDialogCaption("Creating Data...");
            //const int RowCount = 100000;
            //Random random = new Random();
            //DateTime dateTime = DateTime.Now;
            //this.dataTable.BeginLoadData();
            //try
            //{
            //    for (int i = 0; i < RowCount; i++)
            //    {
            //        DataRow row = this.dataTable.NewRow();
            //        row[this.dataColumnId] = i + 1;
            //        row[this.dataColumnText] = "Text " + (i % 100 + 1).ToString();
            //        row[this.dataColumnBool] = random.Next(1000) % 2 == 1;
            //        row[this.dataColumnDate] = dateTime.AddDays(random.Next(100));
            //        row[this.dataColumnCurrency] = Math.Round(random.NextDouble() * 10000000.0) / 1000;
            //        this.dataTable.Rows.Add(row);
            //    }
            //}
            //finally
            //{
            //    this.dataTable.EndLoadData();
            //}
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {            
            gridView1.ActiveFilterString = filterControl1.FilterString; 
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            ResetCustomFilter();
        }

        void ResetCustomFilter()
        {
            filterControl1.FilterString = null;
            //filterControl1.FilterString = gridView1.ActiveFilterString;

            filterControl1.FilterColumns.Add(new UnboundFilterColumn("Order Number", "OrderNumber", typeof(int),
                new RepositoryItemSpinEdit(), FilterColumnClauseClass.Generic));

            //filterControl1.FilterColumns.Add(new UnboundFilterColumn("Order Date", "OrderDate", typeof(DateTime),
            //    new RepositoryItemDateEdit(), FilterColumnClauseClass.DateTime));

            filterControl1.FilterColumns.Add(new UnboundFilterColumn("Name", "CustomerName", typeof(string),
                new RepositoryItemTextEdit(), FilterColumnClauseClass.String));

            filterControl1.FilterColumns.Add(new UnboundFilterColumn("Surname", "CustomerSurname", typeof(string),
                new RepositoryItemTextEdit(), FilterColumnClauseClass.String));

            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);            
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);

            if (info.InRow || info.InRowCell)
            {
                DataRow row = gridView1.GetDataRow(info.RowHandle);
                Order order = new Order();
                order.OrderID =  (Guid)row["OrderID"];
                //order.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString();
                order.OilQuantity =  Convert.ToDecimal(row["OilQuantity"]);
                order.UnloadPlace = Convert.ToDecimal(row["UnloadPlace"]);
                order.UnloadPlaceNumber = 1;
                order.PartOfWIR = Convert.ToDecimal(row["PartOfWIR"]);                
                order.CustomerName = Convert.ToString(row["CustomerName"]);
                order.CustomerSurname = Convert.ToString(row["CustomerSurname"]);
                order.CustomerAddress = Convert.ToString(row["CustomerAddress"]);
                order.CustomerZipcode = Convert.ToString(row["CustomerZipCode"]);
                order.CustomerPlace = Convert.ToString(row["CustomerPlace"]);                
                order.OilQuality = (int)row["OilQualityIndex"];

                if (OnSelectedOrder != null)
                {
                    OnSelectedOrder(order);
                }

                //order.UnloadPlace = (int)row["WirFactor"];
                //order.UnloadPlace = (int)row["OilQualityPrice"];
                //order.UnloadPlace = (int)row["CurrencyRate"];
                //order.UnloadPlace = (int)row["OrderNumber"];
                //order.UnloadPlace = (int)row["ReportName"];                

                //string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                //MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
            }

        }
    }
}
