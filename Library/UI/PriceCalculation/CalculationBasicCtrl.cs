using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Xml;
using CalculationOilPrice.Library.Storage;
using CalculationOilPrice.Library.Entity.Setting;
using System.Globalization;
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraEditors.Repository;
using CalculationOilPrice.Library.Entity.Setting.PriceCalculation.Models;
using System.Linq;
using System.Collections;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using CalculationOilPrice.Library.Business.PriceCalculation;

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class CalculationBasicCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SaveChangedCallback();
        public event SaveChangedCallback SaveChanged;

        ICalculation _Calculation = new BasicCalculation();
        MarginCalculation _MarginCalculation = new MarginCalculation();


        CalculationModel _Model;

        List<GridColumn> emptyColumns = new List<GridColumn>();
        bool _AddedEmptyColumn = false;

        enum TempColumnNames
        {
            Sign,
            Description,
            AmountPercent,
            AmountFix,
            Currency,
            Total,
            Tag,
            Group,
            //IsSummaryGroup,
            //IsSummaryGroupPlusTotalAmount,
            Order,
            IsSummary,
            SummaryGroups,
            Convert,
            UpdatedAmountField,
            VariableTotal,
            EditedField,
            CostCalculatonGroup
        }

        public CalculationBasicCtrl()
        {
            InitializeComponent();

            gridView1.GridControl.Paint += GridControl_Paint;
        }
        private void SettingCtrl_Load(object sender, EventArgs e)
        {
            //text edit for non editable cell
            this.repositoryItemButtonEdit1.Buttons.Clear();

            //hide scale dropdown
            this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //scale unit layout
            this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            // margin grid layout
            this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            // margin dropdown layout
            this.layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        void GridControl_Paint(object sender, PaintEventArgs e)
        {
            //paint empty vertical column(s)
            GridViewInfo vi = gridView1.GetViewInfo() as GridViewInfo;
            using (SolidBrush brush = new SolidBrush(vi.PaintAppearance.Empty.BackColor))
            {
                foreach (GridColumn col in emptyColumns)
                {
                    GridColumnInfoArgs info = vi.ColumnsInfo[col];
                    Rectangle rect = info.Bounds;
                    rect.Height = vi.ClientBounds.Height - 2;
                    rect.Width -= 1;
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
        }

        public void AddEmptyColumn(GridColumn col)
        {
            //add vertical gap columns
            GridColumn newCol = gridView1.Columns.Add();
            newCol.VisibleIndex = col.VisibleIndex + 1;
            emptyColumns.Add(newCol);
            newCol.OptionsColumn.AllowFocus = false;
            newCol.MinWidth = 1;
            newCol.Width = 5;
            newCol.OptionsColumn.AllowSize = false;
        }

        public void New(GeneralSettingModel generalSettingModel, PriceCalculationSetting priceCalculationSetting)
        {
            _Model = new CalculationModel()
            {
                //set new id
                ID = priceCalculationSetting.CalculationCounter + 1,
                GeneralSetting = generalSettingModel,
                PriceSetting = priceCalculationSetting.PriceSetting,
                CalculationNotes = new List<CalculationNoteModel>(),
                CalculationViewItems = new List<CalculationItemModel>(),
                CalculationMarginViewItems = new List<CalculationItemModel>()
                //BasicCalculationItems = new List<CalculationItemModel>(),
                //ScaleCalculationItems = new List<CalculationScaleModel>()
            };

            //set calculation method first
            SetCalculationMethod();

            //setup data
            InitData();

            //refresh gridview
            RefreshGrid();

            //refresh margin gridview 
            RefreshGridMargin();
        }

        void SetCalculationMethod()
        {
            if (_Model.GeneralSetting.CostType == "P")
            {
                _Calculation = new CostCalculation();
            }
        }

        void SetScaleCombobox()
        {
            //setup price scales combobox
            //if price scale more than 1
            if (_Model.GeneralSetting.PriceScale.Scale > 1)
            {
                List<ComboboxItemModel> oItems = new List<ComboboxItemModel>();

                //add basic calculation first
                oItems.Add(new ComboboxItemModel() { Value = 0, Caption = "Grundberechnung" });

                //setup price scale dropdown items
                for (int i = 1; i <= _Model.GeneralSetting.PriceScale.Scale; i++)
                {
                    oItems.Add(new ComboboxItemModel() { Value = i, Caption = String.Format("Staffel {0}", i) });
                }

                cboPriceScales.Properties.DataSource = oItems;
                cboPriceScales.Properties.ValueMember = "Value";
                cboPriceScales.Properties.DisplayMember = "Caption";
                cboPriceScales.ItemIndex = 0;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
            }
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

        private void RefreshGridMargin()
        {
            gridView2.RefreshData();
        }

        private void InitData()
        {
            int itemOrder = 0;
            SetBasicCalculationData(ref itemOrder);
            SetPriceScaleCalculationData(itemOrder);

            //add view items
            if (_Model.CalculationViewItems.Count == 0)
            {
                _Model.CalculationViewItems.AddRange(_Model.CalculationNotes[0].CalculationItems);
            }

            //predefine scale amount
            SetPredefineProfitAmount();

            //setup price scales combobox if needed
            SetScaleCombobox();

            //bind basic calculation gridview
            BindBasicCalculationView();



            /////// margin data /////////////////

            //set margin data
            SetMarginCalculationData();

            //set margin combobox
            SetMarginCombobox();


            //add view items
            if (_Model.CalculationMarginViewItems.Count == 0)
            {
                _Model.CalculationMarginViewItems.AddRange(_Model.CalculationNotes[0].CalculationMarginItems);
            }

            //bind margin calculation gridview if needed
            BindMarginCalculationView();
        }

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

            gridView1.Columns[TempColumnNames.Order.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Order.ToString()].Visible = false;

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

        void BindMarginCalculationView()
        {
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

                gridView2.Columns[TempColumnNames.Order.ToString()].OptionsColumn.AllowEdit = false;
                gridView2.Columns[TempColumnNames.Order.ToString()].Visible = false;

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


        void SetBasicCalculationData(ref int order)
        {
            //always start with zero
            _Model.CalculationNotes.Add(new CalculationNoteModel()
            {
                ID = 0,
                CalculationItems = new List<CalculationItemModel>(),
                CalculationMarginItems = new List<CalculationItemModel>()
            });


            //group1
            //int order = 0;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "",
                Description = "Bareinkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BEK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2, 1 },
                    SummaryGroups = new List<int> { 3 },
                },
                Group = 0,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Bezugskosten",
                Tag = "BZK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            if (_Model.GeneralSetting.TextLines != null)
            {
                int count = 0;
                foreach (string item in _Model.GeneralSetting.TextLines)
                {
                    count += 1;
                    order += 1;
                    _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                    {
                        Sign = "+",
                        Description = item,
                        Tag = String.Format("BEN {0}", count),
                        Currency = new CurrencyModel() { Currency = "CHF" },
                        Group = 1,
                        Order = order,
                        Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                    });
                }
            }

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                Tag = "ESTP",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2 },
                    SummaryGroups = new List<int> { 3 },
                },
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group2
            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verwaltungsgemeinkosten",
                Tag = "OGK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Vertriebsgemeinkosten",
                Tag = "VGK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Sondereinzelkosten des Vertriebs",
                Tag = "VSK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Verwaltungs- und Vertriebskosten",
                Tag = "VVK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 2 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 2 },
                    SummaryGroups = new List<int>(),
                },
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                Tag = "ESTP",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3, 2 },
                    SummaryGroups = new List<int> { 3 },
                },
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 1",
                Tag = "SK 1",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 3 },
                    SummaryGroups = new List<int> { 3 },
                },
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group3
            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Lagerhaltungskosten",
                Tag = "LHK",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verpackungsanteil",
                Tag = "VPA",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Transportanteil",
                Tag = "TRA",
                Currency = new CurrencyModel() { Currency = "CHF" },
                BaseCalculationGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 2",
                Tag = "SK 2",
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 3,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                CostCalculatonGroup = new CostCalculatonGroupModel()
                {
                    BaseCalculationGroupRows = new List<int>() { 4 },
                    SummaryGroups = new List<int> { 4 },
                },
                Order = order,
                Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });
        }

        void SetPriceScaleCalculationData(int order)
        {
            //price scale id start by 1
            for (int i = 1; i <= _Model.GeneralSetting.PriceScale.Scale; i++)
            {
                int iOrder = order;

                _Model.CalculationNotes.Add(new CalculationNoteModel()
                {
                    ID = i,
                    CalculationItems = new List<CalculationItemModel>(),
                    CalculationMarginItems = new List<CalculationItemModel>()
                });

                //group4   
                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Gewinnaufschlag",
                    Tag = "GA",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3 },
                    Group = 4,
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Barverkaufspreis",
                    Tag = "VK(bar)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 4,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 5 },
                        SummaryGroups = new List<int> { 5 },
                    },
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group5
                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenskonto",
                    AmountPercent = _Model.PriceSetting.CashDiscount,
                    Tag = "SKT",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Verhandlungsspielraum",
                    AmountPercent = _Model.PriceSetting.SalesBonus,
                    Tag = "PV",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Zielverkaufspreis",
                    Tag = "VK(ziel)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 5,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 6 },
                        SummaryGroups = new List<int> { 6 },
                    },
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group6
                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenrabatt",
                    AmountPercent = _Model.PriceSetting.CustomerDiscount,
                    Tag = "RBT",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    Group = 6,
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Nettoverkaufspreis",
                    Tag = "VK(liste)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 6,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    CostCalculatonGroup = new CostCalculatonGroupModel()
                    {
                        BaseCalculationGroupRows = new List<int>() { 7 },
                        SummaryGroups = new List<int> { 7 },
                    },
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group7
                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Mehrwertsteuer",
                    AmountPercent = _Model.PriceSetting.VatTaxes,
                    Tag = "MWST",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    BaseCalculationGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Group = 7,
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                _Model.CalculationNotes.Last().CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Bruttoverkaufspreis",
                    Tag = "VK(brutto)",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 7,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 },
                    Order = iOrder,
                    Convert = _Model.GeneralSetting.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });
            }
        }

        void SetMarginCalculationData()
        {
            //add margin items for each note
            foreach (CalculationNoteModel item in _Model.CalculationNotes)
            {
                int order = 0;

                //master amount
                //int order = 0;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Bareinkaufspreis",
                    Tag = "BEK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 0,
                    Order = order
                });


                //group1
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Bezugskosten",
                    Tag = "BZK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    Order = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Beschaffungsteilkosten",
                    Tag = "BEN",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    Order = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Einstandspreis",
                    Tag = "ESTP",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 1,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1 },
                    Order = order
                });


                //group2
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Verwaltungsgemeinkosten",
                    Tag = "OGK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    Order = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Vertriebsgemeinkosten",
                    Tag = "VGK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    Order = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Sondereinzelkosten des Vertriebs",
                    Tag = "VSK",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //CalculationBaseGroupRows = new List<int>() { 0, 1 },
                    Group = 2,
                    Order = order
                });

                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Verwaltungs- und Vertriebskosten",
                    Tag = "VVK",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 2,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 2 },
                    Order = order
                });


                //group3
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Summe fix/variabel",
                    Tag = "SK 1",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 3,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2 },
                    Order = order
                });


                //group4                  
                order += 1;
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Gewinnaufschlag",
                    Tag = "GA",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    //IsSummary = true,
                    //CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                    Group = 4,
                    Order = order
                });


                //group5
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Deckungsbeitrag",
                    Tag = "VK",
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 5,
                    //CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3 },
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 4 },
                    Order = order
                });


                //group6
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Barverkaufspreis",
                    Tag = "VK(bar)",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 6,
                    //IsSummary = true,
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    Order = order
                });


                //group7
                item.CalculationMarginItems.Add(new CalculationItemModel()
                {
                    Sign = "",
                    Description = "Bruttoverkaufspreis",
                    Tag = "VK(brutto)",
                    AmountPercent = 100,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 7,
                    //IsSummary = true,
                    //SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Order = order
                });

            }
        }

        void SetPredefineProfitAmount()
        {
            //set predefined profit amount for each scale
            int iScaleCount = _Model.CalculationNotes.Count(item => item.ID > 0);

            if (iScaleCount > 1)
            {
                if (_Model.GeneralSetting.PriceScale.MinProfit > 0 && _Model.GeneralSetting.PriceScale.MaxProfit > 0)
                {
                    //everage amount excluded first item
                    decimal iEverageAmount = (_Model.GeneralSetting.PriceScale.MaxProfit -
                        _Model.GeneralSetting.PriceScale.MinProfit) / (iScaleCount - 1);

                    //scale item start from ID 1
                    for (int i = 0; i < iScaleCount; i++)
                    {
                        decimal iProfitAmount = 0;
                        if (i == 0)
                        {
                            // set first predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MaxProfit;
                        }
                        else if (i == (iScaleCount - 1))
                        {
                            // set last predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MinProfit;
                        }
                        else
                        {
                            // set rest predefined
                            //iProfitAmount = iEverageAmount * ((_Model.CalculationNotes.Count - 1) - i);
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MaxProfit - (iEverageAmount * i);
                        }

                        //update amount to calculation model
                        //if percent or fix predefined
                        if (_Model.GeneralSetting.PriceScale.MarkUp == "P")
                        {
                            _Calculation.UpdateRowAmountPercent(_Model, _Model.CalculationNotes[i + 1].CalculationItems[0], iProfitAmount, skipBaseGroupRows: true);
                        }
                        else
                        {
                            _Calculation.UpdateRowAmountFix(_Model, _Model.CalculationNotes[i + 1].CalculationItems[0], iProfitAmount, skipBaseGroupRows: true);
                        }
                    }
                }
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //calculation here  
            if (e.RowHandle > -1)
            {
                bool isSpecial = false;
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                int iRowOrder = (int)gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Order.ToString());

                //if master amount
                if (sTag == "BEK" || sTag == "VK(brutto)")
                {
                    //_Model.MasterAmount = Convert.ToDecimal(e.Value);
                    _Calculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                    _Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
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
                        _Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "");
                    }
                    else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                    {
                        _Calculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                        _Calculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
                    }
                }

                _Calculation.UpdateGroupAmountAll(_Model, false);
                _MarginCalculation.UpdateBaseAmountAll(_Model);
            }

            gridView1.RefreshData();
            gridView2.RefreshData();
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //calculation here  
            if (e.RowHandle > -1)
            {
                //bool isSpecial = false;
                //string sTag = gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                int iRowOrder = (int)gridView2.GetRowCellValue(e.RowHandle, TempColumnNames.Order.ToString());
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveChanged != null)
            {
                SaveChanged();
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

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //disable editor for particular row's cell
            if (e.RowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
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
                            gridView2.Appearance.FocusedCell.BackColor = Color.Lavender;
                            e.Cancel = true;
                            break;
                        case "VK(ziel)":
                            gridView2.Appearance.FocusedCell.BackColor = Color.PaleTurquoise;
                            e.Cancel = true;
                            break;
                        case "VK(liste)":
                            gridView2.Appearance.FocusedCell.BackColor = Color.MediumAquamarine;
                            e.Cancel = true;
                            break;
                        case "VK(brutto)":
                            if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString() &&
                                _Model.GeneralSetting.CostType != "P")
                            {
                                gridView2.Appearance.FocusedCell.BackColor = Color.SandyBrown;
                                e.Cancel = true;
                            }
                            break;
                        case "ESTP":
                        case "VVK":
                        case "SK 1":
                        case "SK 2":
                            gridView2.Appearance.FocusedCell.BackColor = Color.Gainsboro;
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

        private void CboPriceScales_EditValueChanged(object sender, EventArgs e)
        {
            RefreshGrid();

            //if margin enabled
            if (_Model.GeneralSetting.Options.Contains("M"))
            {
                if (cboPriceScales.ItemIndex == 0)
                {
                    //margin grid layout
                    this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else
                {
                    //margin grid layout
                    this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    //bind margin calculation gridview if needed
                    BindMarginCalculationView();

                    //calculate margin
                    _MarginCalculation.UpdateBaseAmountAll(_Model);

                    gridView2.RefreshData();
                }
            }
        }

        private void CboMargin_EditValueChanged(object sender, EventArgs e)
        {
            if (cboMargin.ItemIndex == 0)
            {
                //basic grid layout
                this.layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                //margin grid layout
                this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                //basic grid layout
                this.layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                //margin grid layout
                this.layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                //bind margin calculation gridview if needed
                BindMarginCalculationView();

                gridView2.RefreshData();
            }
        }

        private void MyRepositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //unit button click
            if (gridView1.FocusedRowHandle > -1)
            {
                string sValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Convert.ToString()).ToString();
                ButtonEdit ed = (ButtonEdit)gridView1.ActiveEditor;
                if (sValue == "EE")
                {
                    ed.Properties.Buttons[0].Caption = "VE";
                    _Calculation.UpdateCalculationRowUnit(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "VE");
                }
                else
                {
                    ed.Properties.Buttons[0].Caption = "EE";
                    _Calculation.UpdateCalculationRowUnit(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "EE");
                }

                _Calculation.UpdateGroupAmountAll(_Model, false);
            }

            gridView1.RefreshData();
        }

        private void MyRepositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //currency button click
            if (gridView1.FocusedRowHandle > -1)
            {
                string sValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Currency.ToString()).ToString();
                ButtonEdit ed = (ButtonEdit)gridView1.ActiveEditor;
                if (sValue == "CHF")
                {
                    ed.Properties.Buttons[0].Caption = _Model.GeneralSetting.Currency.Currency;
                    _Calculation.UpdateCalculationRowCurrency(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), _Model.GeneralSetting.Currency.Currency);
                }
                else
                {
                    ed.Properties.Buttons[0].Caption = "CHF";
                    _Calculation.UpdateCalculationRowCurrency(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "CHF");
                }

                _Calculation.UpdateGroupAmountAll(_Model, false);
            }

            gridView1.RefreshData();
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

        private void TxtScaleNumber_EditValueChanged(object sender, System.EventArgs e)
        {
            //after button edit control click, update value to calculation model
            if (cboPriceScales.ItemIndex > 0)
            {
                _Model.CalculationNotes[cboPriceScales.ItemIndex].Quantity = Convert.ToDecimal(txtScaleNumber.Text);
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
