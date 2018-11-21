using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class CalculationBasicCtrl
    {
        void SetOptionMargin()
        {
            //set margin combobox
            SetMarginCombobox();
        }

        void SetMarginCombobox()
        {
            //setup margin combobox
            //if price scale equals 1
            if (_Model.GeneralSetting.Options.Contains("M"))
            {
                if (_Model.GeneralSetting.PriceScale.Scale == 1)
                {
                    List<ComboboxItemModel> oItems = new List<ComboboxItemModel>();

                    //add basic calculation first
                    oItems.Add(new ComboboxItemModel() { Value = 0, Caption = "Brechnung VK" });

                    //add margin
                    oItems.Add(new ComboboxItemModel() { Value = 1, Caption = "Deckungsbeitrag" });

                    cboMargin.Properties.DataSource = oItems;
                    cboMargin.Properties.ValueMember = "Value";
                    cboMargin.Properties.DisplayMember = "Caption";
                    cboMargin.ItemIndex = 0;
                    this.layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
                }
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //calculation here  
            if (e.RowHandle > -1)
            {
                //bool isSpecial = false;
                //string sTag = gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                int iRowOrder = (int)gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.ItemOrder.ToString());
                decimal iBaseTotal = _MarginCalculation.GetBaseTotal(_Model, iRowOrder);
                decimal iValue = Convert.ToDecimal(e.Value);

                if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                {
                    if (iValue < 0 || iValue > 100)
                    {
                        _MarginCalculation.UpdateMarginRowAmountPercent(_Model, iRowOrder, (decimal)gridView2.ActiveEditor.OldEditValue);
                    }
                    else
                    {
                        //_MarginCalculation.UpdateMarginRowAmountPercent(_Model, iRowOrder, iValue);
                        _MarginCalculation.UpdateMarginRowEditedField(_Model, iRowOrder, "P");
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                {
                    if (iValue > iBaseTotal)
                    {
                        _MarginCalculation.UpdateMarginRowAmountFix(_Model, iRowOrder, (decimal)gridView2.ActiveEditor.OldEditValue);
                    }
                    else
                    {
                        //_MarginCalculation.UpdateMarginRowAmountFix(_Model, iRowOrder, iValue);
                        _MarginCalculation.UpdateMarginRowEditedField(_Model, iRowOrder, "F");
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.VariableTotal.ToString())
                {
                    if (iValue > iBaseTotal)
                    {
                        _MarginCalculation.UpdateMarginRowAmountVariable(_Model, iRowOrder, (decimal)gridView2.ActiveEditor.OldEditValue);
                    }
                    else
                    {
                        //_MarginCalculation.UpdateMarginRowAmountVariable(_Model, iRowOrder, iValue);
                        _MarginCalculation.UpdateMarginRowEditedField(_Model, iRowOrder, "V");
                    }
                }

                _MarginCalculation.UpdateBaseAmountAll(_Model);
            }

            gridView2.RefreshData();
        }

        private void gridView2_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                string sTag = gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();

                //switch (sTag)
                //{
                //    case "BEK": //Bareinkaufspreis
                //    case "ESTP": //Einstandspreis
                //    case "VVK": //Verwaltungs- und Vertriebskosten
                //    case "SK 1": //Summe fix/variabel
                //    case "GA": //Gewinnaufschlag
                //    case "VK": //Deckungsbeitrag
                //    case "VK(bar)": //Barverkaufspreis
                //    case "VK(brutto)": //Bruttoverkaufspreis
                //        break;
                //}

                switch (sTag)
                {
                    case "BEK":
                    case "ESTP":
                    case "VVK":
                    case "SK 1":
                    case "GA": //Gewinnaufschlag
                    case "VK": //Deckungsbeitrag
                    case "VK(bar)": //Barverkaufspreis
                    case "VK(brutto)": //Bruttoverkaufspreis
                        e.Appearance.BackColor = Color.Gainsboro;
                        break;
                }
            }
        }

        private void gridView2_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //disable editor for particular row's cell
            if (e.RowHandle > -1)
            {
                string sTag = gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();

                //switch (sTag)
                //{
                //    case "BEK": //Bareinkaufspreis
                //    case "ESTP": //Einstandspreis
                //    case "VVK": //Verwaltungs- und Vertriebskosten
                //    case "SK 1": //Summe fix/variabel
                //    case "GA": //Gewinnaufschlag
                //    case "VK": //Deckungsbeitrag
                //    case "VK(bar)": //Barverkaufspreis
                //    case "VK(brutto)": //Bruttoverkaufspreis
                //        break;
                //}

                if (e.Column.FieldName == TempColumnNames.Total.ToString())
                {
                    switch (sTag)
                    {
                        case "ESTP": //Einstandspreis                        
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            //null editor item
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                            break;
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK": //Bareinkaufspreis
                        case "ESTP": //Einstandspreis       
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            //null editor item
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                            break;
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK": //Bareinkaufspreis
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            //null editor item
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                            break;
                        case "ESTP": //Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                            e.RepositoryItem = this.repositoryItemTextEdit2;
                            break;

                    }
                }
                else if (e.Column.FieldName == TempColumnNames.VariableTotal.ToString())
                {
                    switch (sTag)
                    {
                        case "GA": //Gewinnaufschlag
                            //null editor item
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                            break;
                        case "BEK": //Bareinkaufspreis
                        case "ESTP": //Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            e.RepositoryItem = this.repositoryItemTextEdit2;
                            break;
                    }
                }
            }
        }

        private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView2.FocusedRowHandle > -1)
            {
                string sTag = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString();

                //allow edit description for BEN(s) items only
                if (gridView2.FocusedColumn.FieldName == TempColumnNames.Total.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK":
                        case "ESTP": ////Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            gridView2.Appearance.FocusedCell.BackColor = Color.Gainsboro;
                            e.Cancel = true;
                            break;
                    }
                }
                else if (gridView2.FocusedColumn.FieldName == TempColumnNames.AmountPercent.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK":
                        case "ESTP": ////Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            gridView2.Appearance.FocusedCell.BackColor = Color.Gainsboro;
                            e.Cancel = true;
                            break;
                    }
                }
                else if (gridView2.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK":
                        case "ESTP": ////Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            gridView2.Appearance.FocusedCell.BackColor = Color.Gainsboro;
                            e.Cancel = true;
                            break;
                    }
                }
                else if (gridView2.FocusedColumn.FieldName == TempColumnNames.VariableTotal.ToString())
                {
                    switch (sTag)
                    {
                        case "BEK":
                        case "ESTP": ////Einstandspreis
                        case "VVK": //Verwaltungs- und Vertriebskosten
                        case "SK 1": //Summe fix/variabel
                        case "GA": //Gewinnaufschlag
                        case "VK": //Deckungsbeitrag
                        case "VK(bar)": //Barverkaufspreis
                        case "VK(brutto)": //Bruttoverkaufspreis
                            gridView2.Appearance.FocusedCell.BackColor = Color.Gainsboro;
                            e.Cancel = true;
                            break;
                    }
                }
            }
        }

        private void gridView2_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridColumnSummaryItem customSummaryItem = e.Item as GridColumnSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                //summary column 1
                if (customSummaryItem.FieldName == "Col1Row1")
                {
                    e.TotalValue = _MarginCalculation.GetMarginSummarize(_Model);
                }
            }
        }

        private void gridView2_CustomDrawFooter(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            //GridView view = sender as GridView;
            //Rectangle r1 = (view.GetViewInfo() as GridViewInfo).ColumnsInfo[1].Bounds;
            //Rectangle r2 = (view.GetViewInfo() as GridViewInfo).ColumnsInfo[2].Bounds;
            //Rectangle r11 = (view.GetViewInfo() as GridViewInfo).FooterInfo.Cells[0].Bounds;
            //Rectangle r12 = (view.GetViewInfo() as GridViewInfo).FooterInfo.Cells[1].Bounds;
        }

        private void gridView2_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.Caption == "Kostenanteil")
            {
                Rectangle r = e.Info.Bounds;
                Rectangle r11 = (view.GetViewInfo() as GridViewInfo).ColumnsInfo[view.Columns[TempColumnNames.Description.ToString()]].Bounds;

                e.Bounds.Inflate(-5, -5);
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                e.Info.Bounds = new Rectangle(r11.Left, r.Top, r11.Width + 200, r.Height);
                e.DefaultDraw();


                //Rectangle r = e.Info.Bounds;
                //string text = e.Info.DisplayText;
                //Rectangle r11 = (view.GetViewInfo() as GridViewInfo).ColumnsInfo[view.Columns[TempColumnNames.Description.ToString()]].Bounds;
                ////Rectangle r12 = (view.GetViewInfo() as GridViewInfo).ColumnsInfo[view.Columns[TempColumnNames.Total.ToString()]].Bounds;
                //e.Info.Bounds = new Rectangle(r11.Left, r.Top, r11.Width + 200, r.Height);
                //e.Painter.DrawObject(e.Info);
                //e.Info.Bounds = r;
                e.Handled = true;
            }
        }
    }
}
