using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using CalculationOilPrice.Library.Entity;
using CalculationOilPrice.Library.Global;

namespace CalculationOilPrice.Library.UI
{
    public partial class OilOrderDetailControl : UserControl
    {
        DataTable orderDetailDt;
        Order currentOrder;
        decimal vp100kgInWIR;
        decimal grandTotal;
        int currentUnloadPlaceNumber;
        int itemCount;

        enum TempColumnNames
        {
            AdditionalCosts,
            UnloadPlaceCost,
            Vp100kgInWIR,
            Vp100kgInCHF,
            Total1,
            Total2,
            GrandTotal
        }

        public OilOrderDetailControl()
        {
            InitializeComponent();
        }

        void Reload(Guid orderID, Guid searchID)
        {
            DataColumn dc = null;
            DataTable dtLoad = new DataTable();

            if (searchID == Guid.Empty)
            {
                dc = new DataColumn("0");
                dc.DefaultValue = 1;
                dtLoad = Storage.StorageOperator.LoadTable("OilOrderDetail", null, new DataColumn[] { dc }, null);
            }
            else
            {
                dc = new DataColumn("OrderID");
                dc.DefaultValue = searchID;

                DataColumn orderCol = new DataColumn("ItemNumber");
                dtLoad = Storage.StorageOperator.LoadTable("OilOrderDetail", null, new DataColumn[] { dc },
                    new DataColumn[] { orderCol });
            }

            if (orderDetailDt == null)
            {
                orderDetailDt = new DataTable();

                dc = new DataColumn("0");
                dc.DefaultValue = 1;
                orderDetailDt = Storage.StorageOperator.LoadTable("OilOrderDetail", null, new DataColumn[] { dc }, null);

                if (orderDetailDt != null)
                {
                    orderDetailDt.Columns["ItemDescription"].Caption = "Description";
                    orderDetailDt.Columns["ItemCostPercent"].Caption = "%";
                    orderDetailDt.Columns["ItemCostCash"].Caption = "Betrag";

                    dc = new DataColumn(TempColumnNames.AdditionalCosts.ToString());
                    dc.DataType = typeof(decimal);
                    dc.Caption = "";
                    orderDetailDt.Columns.Add(dc);

                    dc = new DataColumn(TempColumnNames.GrandTotal.ToString());
                    dc.DataType = typeof(decimal);
                    dc.Caption = "";
                    orderDetailDt.Columns.Add(dc);
                }
            }


            orderDetailGridView.BeginDataUpdate();
            //orderDetailGridView.BeginUpdate();
            //orderDetailDt.BeginLoadData();
            try
            {
                orderDetailDt.Rows.Clear();
                foreach (DataRow loadRow in dtLoad.Rows)
                {
                    orderDetailDt.Rows.Add(loadRow.ItemArray);
                    orderDetailDt.Rows[orderDetailDt.Rows.Count - 1]["OilOrderDetailID"] = Guid.NewGuid();
                    orderDetailDt.Rows[orderDetailDt.Rows.Count - 1]["OrderID"] = orderID;
                }

                UpdateRowAmount();
                //orderDetailGridControl.DataSource = orderDetailDt;
                //orderDetailGridView.RefreshData();
                //orderDetailGridView.UpdateSummary();
            }
            catch { }
            finally
            {
                orderDetailGridView.EndDataUpdate();
                //dtLoad.EndLoadData();
                //orderDetailGridView.EndUpdate();
            }

            if (orderDetailDt != null)
            {
                orderDetailGridControl.DataSource = orderDetailDt;
                orderDetailGridView.RefreshData();

                orderDetailGridView.OptionsView.ShowFooter = true;
                orderDetailGridView.FooterPanelHeight = 85;

                // add detail columns
                orderDetailGridView.Columns["OrderID"].Visible = false;
                orderDetailGridView.Columns["UnloadPlaceNumber"].Visible = false;
                orderDetailGridView.Columns["ItemNumber"].Visible = false;
                orderDetailGridView.Columns["ItemDescription"].Width = 200;
                orderDetailGridView.Columns["ItemCostPercent"].Width = 30;
                orderDetailGridView.Columns["ItemCostPercent"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                orderDetailGridView.Columns["ItemCostCash"].Width = 30;
                orderDetailGridView.Columns["ItemCostCash"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Width = 80;
                orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Width = 80;
                orderDetailGridView.Columns["OilOrderDetailID"].Visible = false;

                ///Setup temp columns
                orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].OptionsColumn.AllowEdit = false;
                orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].OptionsColumn.AllowEdit = false;

                orderDetailGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;

                orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].OptionsColumn.AllowFocus = false;
                orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].OptionsColumn.AllowEdit = false;


                //add footer column1
                if (orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Summary.Count == 0)
                {
                    orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col1Row1.Key, OilOrderDetailSummaryColumnCategory.Col1Row1.Mask);
                    orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col1Row2.Key, OilOrderDetailSummaryColumnCategory.Col1Row2.Mask);
                    orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col1Row3.Key, OilOrderDetailSummaryColumnCategory.Col1Row3.Mask);
                    orderDetailGridView.Columns[TempColumnNames.AdditionalCosts.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col1Row4.Key, OilOrderDetailSummaryColumnCategory.Col1Row4.Mask);
                }

                //add footer column2
                if (orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Summary.Count == 0)
                {
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].OptionsColumn.AllowFocus = false;
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].OptionsColumn.AllowEdit = false;
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col2Row1.Key, OilOrderDetailSummaryColumnCategory.Col2Row1.Mask);
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col2Row2.Key, OilOrderDetailSummaryColumnCategory.Col2Row2.Mask);
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col2Row3.Key, OilOrderDetailSummaryColumnCategory.Col2Row3.Mask);
                    orderDetailGridView.Columns[TempColumnNames.GrandTotal.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom, OilOrderDetailSummaryColumnCategory.Col2Row4.Key, OilOrderDetailSummaryColumnCategory.Col2Row4.Mask);
                }
            }

            currentOrder.OrderDetailDt = orderDetailDt;
        }

        public void CreateOrderDetail(Order order, Guid searchID, int unloadPlaceNumber)
        {
            itemCount = 0;
            currentUnloadPlaceNumber = unloadPlaceNumber;
            currentOrder = order;
            Reload(order.OrderID, searchID);
            orderDetailGridView.UpdateSummary();
        }

        public void UpdateOrder(Order order, int unloadPlaceNumber)
        {
            currentUnloadPlaceNumber = unloadPlaceNumber;
            currentOrder = order;

            //orderDetailGridView.RefreshData();

            UpdateRowAmount();
            orderDetailGridView.UpdateSummary();
        }

        public void SaveOrderDetail()
        {
            //Rearrange position.
            int i = 1;
            foreach (DataRow dr in orderDetailDt.Rows)
            {
                dr["ItemNumber"] = i;
                i += 1;
            }

            Storage.StorageOperator.SaveTable(orderDetailDt, orderDetailDt.Columns["OilOrderDetailID"]
                , new DataColumn[] { orderDetailDt.Columns[TempColumnNames.AdditionalCosts.ToString()],
                orderDetailDt.Columns[TempColumnNames.GrandTotal.ToString()]});
        }

        void FilterData(int unloadPlaceNumber)
        {
            //FilteredDataView
        }

        void UpdateRowAmount()
        {
            decimal itemCost = 0;
            int rowCount = 0;

            if (orderDetailDt == null)
            {
                return;
            }

            foreach (DataRow dr in orderDetailDt.Rows)
            {
                if (dr["ItemCostPercent"].ToString() != "0")
                {
                    itemCost = Convert.ToDecimal(dr["ItemCostPercent"]);
                    itemCost = (currentOrder.OilPrice * (itemCost / 100));
                    itemCost = itemCost * (currentOrder.OilQuantity / 100);
                    //itemCost = currentOrder.OilPrice * (itemCost / 100);
                    itemCost = Math.Round(itemCost, 2, MidpointRounding.AwayFromZero);
                    SetAmountCost(rowCount, itemCost);
                }

                if (dr["ItemCostCash"].ToString() != "0")
                {
                    itemCost = Convert.ToDecimal(dr["ItemCostCash"]);
                    SetAmountCost(rowCount, itemCost);
                }

                rowCount++;
            }
        }

        decimal GetOrderDetailsPrice()
        {
            decimal orderDetailPrice = 0;

            foreach (DataRow dr in orderDetailDt.Rows)
            {
                if (dr[TempColumnNames.AdditionalCosts.ToString()] == DBNull.Value)
                {
                    dr[TempColumnNames.AdditionalCosts.ToString()] = 0;
                }

                orderDetailPrice += Convert.ToDecimal(dr[TempColumnNames.AdditionalCosts.ToString()]);
            }

            orderDetailPrice = Math.Round(orderDetailPrice, 2, MidpointRounding.AwayFromZero);
            return orderDetailPrice;
        }

        decimal GetUnloadPlaceCost()
        {
            decimal cost = 0;
            cost = currentOrder.UnloadPlace * currentOrder.UnloadCost;
            cost = Math.Round(cost, 2, MidpointRounding.AwayFromZero);

            return cost;
        }

        decimal GetTotalIPrice()
        {
            decimal cost = 0;
            cost = (vp100kgInWIR * (currentOrder.OilQuantity / 100)) * (currentOrder.PartOfWIR / 100);
            cost = Math.Round(cost, 2, MidpointRounding.AwayFromZero);

            return cost;
        }

        decimal GetVpPerHundredKgWIRPrice()
        {
            decimal cost = 0;
            cost = GetTotalIPrice() * (currentOrder.PartOfWIR / 100);
            cost = Math.Round(cost, 2, MidpointRounding.AwayFromZero);

            return cost;
        }

        decimal GetTotalIIPrice()
        {
            decimal cost = 0;
            decimal quantityDifference = 0;
            decimal oilQuantity = 0;

            oilQuantity = (currentOrder.OilQuantity / 100);
            quantityDifference = oilQuantity * (currentOrder.PartOfWIR / 100);
            quantityDifference = oilQuantity - quantityDifference;
            cost = currentOrder.OilPrice * quantityDifference;
            cost = Math.Round(cost, 2, MidpointRounding.AwayFromZero);

            return cost;
        }

        private void orderDetailGridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            itemCount++;

            DataRow row = orderDetailGridView.GetDataRow(e.RowHandle);
            row["OrderID"] = currentOrder.OrderID;
            row["OilOrderDetailID"] = Guid.NewGuid();
            row["ItemCostPercent"] = 0;
            row["ItemCostCash"] = 0;
            row["ItemNumber"] = itemCount;
            row["UnloadPlaceNumber"] = currentUnloadPlaceNumber;
            row[TempColumnNames.AdditionalCosts.ToString()] = 0;
        }

        private void SetAmountCost(int orderDetailRowIndex, decimal amountCost)
        {
            DataRow dr = null;

            if (orderDetailRowIndex < 0)
            {
                ///Negative index means current new row.
                dr = orderDetailGridView.GetDataRow(orderDetailRowIndex);
            }
            else
            {
                dr = orderDetailDt.Rows[orderDetailRowIndex];
            }
            dr[TempColumnNames.AdditionalCosts.ToString()] = amountCost;
        }

        private void orderDetailGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ItemCostPercent")
            {
                decimal itemCost = Convert.ToDecimal(e.Value);
                itemCost = currentOrder.OilPrice * (itemCost / 100);
                itemCost = itemCost * (currentOrder.OilQuantity / 100);
                itemCost = Math.Round(itemCost, 2, MidpointRounding.AwayFromZero);
                orderDetailGridView.GetDataRow(e.RowHandle).SetField("ItemCostCash", 0);
                SetAmountCost(e.RowHandle, itemCost);
            }

            if (e.Column.FieldName == "ItemCostCash")
            {
                decimal itemCost = Convert.ToDecimal(e.Value);
                orderDetailGridView.GetDataRow(e.RowHandle).SetField("ItemCostPercent", 0);
                SetAmountCost(e.RowHandle, itemCost);
            }

            orderDetailGridView.UpdateSummary();
        }

        //AppearanceDefault amountCostTempSumAppr = new AppearanceDefault(Color.Black, Color.Empty, new Font(AppearanceObject.DefaultFont, FontStyle.Bold));
        //Rectangle amountCostSummaryBound;
        //string amountCostSummaryText;

        //private void orderDetailGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        //{
        //}

        private void orderDetailGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridColumnSummaryItem customSummaryItem = e.Item as GridColumnSummaryItem;

            vp100kgInWIR = currentOrder.OilPrice * currentOrder.WIRFactor;
            vp100kgInWIR = Math.Round(vp100kgInWIR, 2, MidpointRounding.AwayFromZero);
            //vp100kgInWIR = vp100kgInWIR + GetOrderDetailsPrice();

            //if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            //{
            //    //amountCostTemp = parentOrder.OilPrice + GetOrderDetailPrice();
            //}

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                //summary column 1
                if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col1Row1.Key)
                {
                    currentOrder.AdditionalCosts = Convert.ToDecimal(e.TotalValue);
                    currentOrder.Total2CashPrice = currentOrder.Total2CashPrice + currentOrder.AdditionalCosts + +currentOrder.UnloadCost;

                    e.TotalValue = GetOrderDetailsPrice();
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col1Row2.Key)
                {
                    e.TotalValue = GetUnloadPlaceCost();
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col1Row3.Key)
                {
                    e.TotalValue = vp100kgInWIR;
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col1Row4.Key)
                {
                    e.TotalValue = currentOrder.OilPrice;
                }

                //summary column2
                if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col2Row1.Key)
                {
                    currentOrder.Total1Price = GetTotalIPrice();
                    currentOrder.Total2CashPrice = GetTotalIIPrice() + GetUnloadPlaceCost() + currentOrder.AdditionalCosts;

                    e.TotalValue = currentOrder.Total1Price + currentOrder.Total2CashPrice;
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col2Row2.Key)
                {
                    e.TotalValue = (GetTotalIPrice() + (GetTotalIIPrice() + GetUnloadPlaceCost() + currentOrder.AdditionalCosts)) / 2;
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col2Row3.Key)
                {
                    e.TotalValue = GetTotalIIPrice() + GetUnloadPlaceCost() + currentOrder.AdditionalCosts;
                }
                else if (customSummaryItem.FieldName == OilOrderDetailSummaryColumnCategory.Col2Row4.Key)
                {
                    e.TotalValue = GetTotalIPrice();
                }
            }
        }

        private void orderDetailGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                orderDetailGridView.DeleteSelectedRows();
                orderDetailGridView.UpdateSummary();
            }
        }

        private void orderDetailGridView_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            DataRow row = orderDetailDt.Rows[e.ListSourceRow];

            if (Convert.ToInt32(row["UnloadPlaceNumber"]) != currentUnloadPlaceNumber)
            {
                e.Visible = false;
            }

            e.Handled = true;
        }
    }
}
