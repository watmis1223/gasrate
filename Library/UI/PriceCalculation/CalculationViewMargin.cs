using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class CalculationBasicCtrl
    {
        void BindMarginCalculationView()
        {
            if (_Model.CalculationMarginViewItems == null)
            {
                return;
            }

            //show margin gridcontrol if needed
            if (_Model.GeneralSetting.Options != null && _Model.GeneralSetting.Options.Contains("M"))
            {
                //bind data
                gridControl2.DataSource = _Model.CalculationMarginViewItems;

                //Setup columns
                //set vertical gap
                gridView2.RowSeparatorHeight = 2;

                //show footer
                gridView2.OptionsView.ShowFooter = true;

                //this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;                

                gridView2.Columns[TempColumnNames.Sign.ToString()].Width = 20;
                gridView2.Columns[TempColumnNames.Sign.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridView2.Columns[TempColumnNames.Sign.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Sign.ToString()].Caption = " ";
                gridView2.Columns[TempColumnNames.Sign.ToString()].OptionsColumn.FixedWidth = true;

                gridView2.Columns[TempColumnNames.Description.ToString()].Width = 200;
                gridView2.Columns[TempColumnNames.Description.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Description.ToString()].Caption = "Kostenanteil";
                gridView2.Columns[TempColumnNames.Description.ToString()].OptionsColumn.FixedWidth = true;

                gridView2.Columns[TempColumnNames.AmountPercent.ToString()].ColumnEdit = this.repositoryItemTextEdit1;
                gridView2.Columns[TempColumnNames.AmountPercent.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView2.Columns[TempColumnNames.AmountPercent.ToString()].Caption = "Fixkosten %";

                gridView2.Columns[TempColumnNames.AmountFix.ToString()].ColumnEdit = this.repositoryItemTextEdit1;
                gridView2.Columns[TempColumnNames.AmountFix.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView2.Columns[TempColumnNames.AmountFix.ToString()].Caption = "Fixkosten";

                gridView2.Columns[TempColumnNames.Total.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView2.Columns[TempColumnNames.Total.ToString()].ColumnEdit = this.repositoryItemTextEdit2;
                gridView2.Columns[TempColumnNames.Total.ToString()].Caption = "Vollkosten";
                gridView2.Columns[TempColumnNames.Total.ToString()].VisibleIndex = 2;

                gridView2.Columns[TempColumnNames.VariableTotal.ToString()].ColumnEdit = this.repositoryItemTextEdit1;
                gridView2.Columns[TempColumnNames.VariableTotal.ToString()].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView2.Columns[TempColumnNames.VariableTotal.ToString()].Caption = "Variable K";

                gridView2.Columns[TempColumnNames.EditedField.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.EditedField.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.Tag.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Tag.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.Group.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Group.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.ItemOrder.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.ItemOrder.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.IsSummary.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.IsSummary.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.Convert.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Convert.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.Currency.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Currency.ToString()].Visible = false;

                gridView2.Columns[TempColumnNames.CostCalculatonGroup.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.CostCalculatonGroup.ToString()].Visible = false;

                //add footer column1
                if (gridView2.Columns[TempColumnNames.Description.ToString()].Summary.Count == 0)
                {
                    gridView2.Columns[TempColumnNames.Description.ToString()].Summary.Add(DevExpress.Data.SummaryItemType.Custom,
                        "Col1Row1",
                        "Der Deckungsbeitrag beträgt {0:n4}% vom Bruttoverkaufspreis");
                }
            }
        }

        private void RefreshGridMargin()
        {
            gridView2.RefreshData();
        }
    }
}
