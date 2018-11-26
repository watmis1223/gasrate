using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using DevExpress.XtraEditors.ViewInfo;
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
        void BindBasicCalculationView()
        {
            //bind data
            gridControl1.DataSource = _Model.CalculationViewItems;

            //Setup columns
            //set vertical gap
            gridView1.RowSeparatorHeight = 2;

            gridView1.Columns[TempColumnNames.Sign.ToString()].Width = 15;
            gridView1.Columns[TempColumnNames.Sign.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Columns[TempColumnNames.Sign.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Sign.ToString()].Caption = " ";
            gridView1.Columns[TempColumnNames.Sign.ToString()].OptionsColumn.FixedWidth = true;

            gridView1.Columns[TempColumnNames.Description.ToString()].Width = 200;
            gridView1.Columns[TempColumnNames.Description.ToString()].ColumnEdit = this.repositoryItemTextEdit3;
            gridView1.Columns[TempColumnNames.Description.ToString()].Caption = "Kostenanteil";
            gridView1.Columns[TempColumnNames.Description.ToString()].OptionsColumn.FixedWidth = true;

            gridView1.Columns[TempColumnNames.AmountPercent.ToString()].ColumnEdit = this.repositoryItemTextEdit1;
            gridView1.Columns[TempColumnNames.AmountPercent.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns[TempColumnNames.AmountPercent.ToString()].Caption = "%";

            gridView1.Columns[TempColumnNames.AmountFix.ToString()].ColumnEdit = this.repositoryItemTextEdit1;
            gridView1.Columns[TempColumnNames.AmountFix.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns[TempColumnNames.AmountFix.ToString()].Caption = "Fix";
            if (!_AddedEmptyColumn)
            {
                AddEmptyColumn(gridView1.Columns[TempColumnNames.AmountFix.ToString()]);
                _AddedEmptyColumn = true;
            }

            gridView1.Columns[TempColumnNames.Total.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns[TempColumnNames.Total.ToString()].ColumnEdit = this.repositoryItemTextEdit2;
            gridView1.Columns[TempColumnNames.Total.ToString()].Caption = "CHF";

            gridView1.Columns[TempColumnNames.Tag.ToString()].Width = 80;
            gridView1.Columns[TempColumnNames.Tag.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Tag.ToString()].Caption = "Kürzel";
            gridView1.Columns[TempColumnNames.Tag.ToString()].OptionsColumn.FixedWidth = true;

            gridView1.Columns[TempColumnNames.Group.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Group.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.ItemOrder.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.ItemOrder.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.IsSummary.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.IsSummary.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.VariableTotal.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.VariableTotal.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.EditedField.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.EditedField.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.CostCalculatonGroup.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.CostCalculatonGroup.ToString()].Visible = false;

            // set visible for unit converter button
            // A is off, E is on           
            if (_Model.GeneralSetting.Convert.Mode == "A")
            {
                gridView1.Columns[TempColumnNames.Convert.ToString()].OptionsColumn.AllowEdit = false;
                gridView1.Columns[TempColumnNames.Convert.ToString()].Visible = false;
            }
            else
            {
                gridView1.Columns[TempColumnNames.Convert.ToString()].ColumnEdit = this.myRepositoryItemButtonEdit1;
                gridView1.Columns[TempColumnNames.Convert.ToString()].Caption = " ";
            }


            // set visible for custom currency button
            // A is off, E is on           
            if (_Model.GeneralSetting.Currency.Mode == "A")
            {
                gridView1.Columns[TempColumnNames.Currency.ToString()].OptionsColumn.AllowEdit = false;
                gridView1.Columns[TempColumnNames.Currency.ToString()].Visible = false;
            }
            else
            {
                gridView1.Columns[TempColumnNames.Currency.ToString()].ColumnEdit = this.myRepositoryItemButtonEdit2;
                gridView1.Columns[TempColumnNames.Currency.ToString()].Caption = " ";
            }
        }

        private void RefreshGrid()
        {
            //set filter
            gridView1.ActiveFilter.Clear();

            //remove scale items if needed
            _Model.CalculationViewItems.RemoveAll(item => item.Group > 3);

            //append first scale calculation items to basic calculation on init
            if (_Model.GeneralSetting.PriceScale.Scale == 1)
            {
                _Model.CalculationViewItems.AddRange(_Model.CalculationNotes[1].CalculationItems);
            }
            else
            {

                //hide scale-number input
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                if (cboPriceScales.ItemIndex > 0)
                {
                    // add price-scale calculation item to gridview
                    _Model.CalculationViewItems.AddRange(_Model.CalculationNotes[cboPriceScales.ItemIndex].CalculationItems);
                    txtScaleNumber.EditValue = _Model.CalculationNotes[cboPriceScales.ItemIndex].Quantity;
                    _Calculation.UpdateGroupAmountAll(_Model, false);

                    gridView1.ActiveFilter.NonColumnFilter = "[Group] > 3";

                    //display scale-number input
                    this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
                }
            }

            gridView1.RefreshData();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //calculation here  
            if (e.RowHandle > -1)
            {
                bool isSpecial = false;
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                int iRowOrder = (int)gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.ItemOrder.ToString());

                //if master amount
                if (sTag == "BEK" || sTag == "VK(brutto)")
                {
                    //_Model.MasterAmount = Convert.ToDecimal(e.Value);
                    _Calculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                    //_Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
                }
                else
                {
                    //calculate non master amount                   
                    if (sTag == "SKT" || sTag == "PV" || sTag == "RBT")
                    {
                        isSpecial = true;
                    }

                    if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                    {
                        _Calculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), true, isSpecial, true);
                        //_Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "");
                    }
                    else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                    {
                        _Calculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                        //_Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
                    }
                }

                _Calculation.UpdateGroupAmountAll(_Model, false);
                _MarginCalculation.UpdateBaseAmountAll(_Model);
            }

            gridView1.RefreshData();
            gridView2.RefreshData();
        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //validate value for SKT, PV, RBT
            //valid value is not less than 100
            if (gridView1.FocusedRowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString();
                if (sTag == "SKT" || sTag == "PV" || sTag == "RBT")
                {
                    if (Convert.ToDecimal(e.Value) >= 100)
                    {
                        e.Valid = false;
                    }
                }
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                switch (sTag)
                {
                    case "ESTP":
                    case "VVK":
                    case "SK 1":
                    case "SK 2":
                        e.Appearance.BackColor = Color.Gainsboro;
                        break;
                    case "VK(bar)":
                        e.Appearance.BackColor = Color.Lavender;
                        break;
                    case "VK(ziel)":
                        e.Appearance.BackColor = Color.PaleTurquoise;
                        break;
                    case "VK(liste)":
                        e.Appearance.BackColor = Color.MediumAquamarine;
                        break;
                    case "VK(brutto)":
                        e.Appearance.BackColor = Color.SandyBrown;
                        break;
                }
            }
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //disable editor for particular row's cell
            if (e.RowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                int iItemOrder = Convert.ToInt32(gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.ItemOrder.ToString()).ToString());
                switch (sTag)
                {
                    case "BEK":
                        if (_Model.GeneralSetting.CostType == "P")
                        {
                            //if cost calculation
                            if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString() ||
                                e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                            else if (e.Column.FieldName == TempColumnNames.Convert.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                        }
                        else
                        {
                            //if basic calculation
                            if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                            else if (e.Column.FieldName == TempColumnNames.Convert.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                        }

                        break;
                    case "SKT":
                    case "PV":
                    case "RBT":
                        if (_Model.GeneralSetting.CostType != "P")
                        {
                            if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                            else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                            {
                                //null editor item
                                if (_Model.GeneralSetting.Currency.Mode == "E")
                                {
                                    e.RepositoryItem = this.repositoryItemButtonEdit1;
                                }
                            }
                        }
                        break;
                    case "ESTP":
                    case "VVK":
                    case "SK 1":
                    case "SK 2":
                    case "VK(bar)":
                    case "VK(ziel)":
                    case "VK(liste)":
                        if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString() ||
                            e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                        {
                            //null editor item
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                        }
                        else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                        {
                            //null editor item
                            if (_Model.GeneralSetting.Currency.Mode == "E")
                            {
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                        }
                        break;
                    case "VK(brutto)":
                        if (_Model.GeneralSetting.CostType == "P")
                        {
                            //if cost calculation
                            if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                            else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                            {
                                //null editor item
                                if (_Model.GeneralSetting.Currency.Mode == "E")
                                {
                                    e.RepositoryItem = this.repositoryItemButtonEdit1;
                                }
                            }
                        }
                        else
                        {
                            if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString() ||
                                e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                            {
                                //null editor item
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                            else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                            {
                                //null editor item
                                if (_Model.GeneralSetting.Currency.Mode == "E")
                                {
                                    e.RepositoryItem = this.repositoryItemButtonEdit1;
                                }
                            }
                        }

                        break;
                    default:
                        if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                        {
                            //null editor item
                            if (_Model.GeneralSetting.Currency.Mode == "E" &&
                                String.IsNullOrWhiteSpace(_Calculation.GetCalculationRowCurrencyFieldEditable(_Model, iItemOrder)))
                            {
                                e.RepositoryItem = this.repositoryItemButtonEdit1;
                            }
                        }
                        break;
                }
            }
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView1.FocusedRowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString();

                //allow edit description for BEN(s) items only
                if (gridView1.FocusedColumn.FieldName == TempColumnNames.Description.ToString())
                {
                    if (!sTag.StartsWith("BEN"))
                    {
                        e.Cancel = true;
                    }
                }
                else if (gridView1.FocusedColumn.FieldName == TempColumnNames.Currency.ToString())
                {
                    //show currency editor for master amount
                    if (_Model.GeneralSetting.Currency.Mode == "E")
                    {
                        if (String.IsNullOrWhiteSpace(_Calculation.GetCalculationRowCurrencyFieldEditable(
                            _Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle))))
                        {
                            e.Cancel = true;
                        }
                    }
                }
                else if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountPercent.ToString() ||
                    gridView1.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString() ||
                    gridView1.FocusedColumn.FieldName == TempColumnNames.Total.ToString())
                {

                    switch (sTag)
                    {
                        case "BEK":
                            if (_Model.GeneralSetting.CostType == "P")
                            {
                                e.Cancel = true;
                            }
                            break;
                        case "VK(bar)":
                            gridView1.Appearance.FocusedCell.BackColor = Color.Lavender;
                            e.Cancel = true;
                            break;
                        case "VK(ziel)":
                            gridView1.Appearance.FocusedCell.BackColor = Color.PaleTurquoise;
                            e.Cancel = true;
                            break;
                        case "VK(liste)":
                            gridView1.Appearance.FocusedCell.BackColor = Color.MediumAquamarine;
                            e.Cancel = true;
                            break;
                        case "VK(brutto)":
                            if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString() &&
                                _Model.GeneralSetting.CostType != "P")
                            {
                                gridView1.Appearance.FocusedCell.BackColor = Color.SandyBrown;
                                e.Cancel = true;
                            }
                            else if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountPercent.ToString() ||
                                gridView1.FocusedColumn.FieldName == TempColumnNames.Total.ToString())
                            {
                                gridView1.Appearance.FocusedCell.BackColor = Color.SandyBrown;
                                e.Cancel = true;
                            }
                            break;
                        case "ESTP":
                        case "VVK":
                        case "SK 1":
                        case "SK 2":
                            gridView1.Appearance.FocusedCell.BackColor = Color.Gainsboro;
                            e.Cancel = true;
                            break;
                        default:
                            if (gridView1.FocusedColumn.FieldName == TempColumnNames.Total.ToString())
                            {
                                e.Cancel = true;
                            }

                            break;
                    }
                }
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //for button edit controls, set button's caption
            if (e.RowHandle > -1)
            {
                if (e.Column.FieldName == TempColumnNames.Convert.ToString())
                {
                    ButtonEditViewInfo editInfo = (ButtonEditViewInfo)((GridCellInfo)e.Cell).ViewInfo;
                    if (editInfo.RightButtons.Count > 0)
                    {
                        editInfo.RightButtons[0].Button.Caption = _Calculation.GetCalculationRowUnitValue(_Model, gridView1.GetDataSourceRowIndex(e.RowHandle));
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                {
                    ButtonEditViewInfo editInfo = (ButtonEditViewInfo)((GridCellInfo)e.Cell).ViewInfo;
                    if (editInfo.RightButtons.Count > 0)
                    {
                        editInfo.RightButtons[0].Button.Caption = _Calculation.GetCalculationRowCurrencyValue(_Model, gridView1.GetDataSourceRowIndex(e.RowHandle));
                    }
                }
            }
        }

        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            ////change active cell editor background color
            //if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountPercent.ToString())
            //{
            //    gridView1.ActiveEditor.BackColor = Color.Wheat;
            //}
            //else if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString())
            //{
            //    gridView1.ActiveEditor.BackColor = Color.Wheat;
            //}
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //change edited cell background color
            if (e.RowHandle > -1)
            {
                string sEditedField = String.Concat(gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.EditedField.ToString()));
                string sSign = String.Concat(gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Sign.ToString()));

                if (sSign == "+")
                {
                    if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
                    {
                        e.Appearance.BackColor = Color.White;
                        //set edited cell color                    
                        if (sEditedField == "P")
                        {
                            e.Appearance.BackColor = Color.Wheat;
                        }
                    }
                    else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                    {
                        e.Appearance.BackColor = Color.White;
                        //set edited cell color                    
                        if (sEditedField == "F")
                        {
                            e.Appearance.BackColor = Color.Wheat;
                        }
                    }
                }
            }
        }
    }
}
