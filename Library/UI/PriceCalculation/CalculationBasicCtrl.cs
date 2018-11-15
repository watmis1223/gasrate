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

        BasicCalculation _BasicCalculation = new BasicCalculation();


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
            UpdatedAmountField
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

            //hide dropdown
            this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        void GridControl_Paint(object sender, PaintEventArgs e)
        {
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

        public void Init(GeneralSettingModel generalSettingModel, PriceSetting priceSetting)
        {
            _Model = new CalculationModel()
            {
                GeneralSetting = generalSettingModel,
                PriceSetting = priceSetting,
                BasicCalculationItems = new List<CalculationItemModel>(),
                ScaleCalculationItems = new List<CalculationScaleModel>()
            };

            //setup data
            InitData();

            //setup price scales combobox
            //if price scale more than 1
            if (generalSettingModel.PriceScale.Scale > 1)
            {
                //_Model.CalculationScaleItems = new List<CalculationScaleModel>();

                List<PriceScaleComboboxItem> oItems = new List<PriceScaleComboboxItem>();

                //add basic calculation first
                oItems.Add(new PriceScaleComboboxItem() { Value = 0, Caption = "Grundberechnung" });

                //setup price scale dropdown items
                for (int i = 1; i <= generalSettingModel.PriceScale.Scale; i++)
                {
                    oItems.Add(new PriceScaleComboboxItem() { Value = i, Caption = String.Format("Staffel {0}", i) });
                }

                cboPriceScales.Properties.DataSource = oItems;
                cboPriceScales.Properties.ValueMember = "Value";
                cboPriceScales.Properties.DisplayMember = "Caption";
                cboPriceScales.ItemIndex = 0;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;
            }

            //refresh gridview
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            //set filter
            gridView1.ActiveFilter.Clear();
            _Model.BasicCalculationItems.RemoveAll(item => item.Group > 3);

            //append first scale calculation items to basic calculation on init
            if (_Model.GeneralSetting.PriceScale.Scale == 1)
            {
                _Model.BasicCalculationItems.AddRange(_Model.ScaleCalculationItems[0].CalculationItems);
            }
            else
            {

                //hide scale-number input
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                if (cboPriceScales.ItemIndex > 0)
                {
                    // add price-scale calculation item to gridview
                    _Model.BasicCalculationItems.AddRange(_Model.ScaleCalculationItems[cboPriceScales.ItemIndex - 1].CalculationItems);
                    txtScaleNumber.EditValue = _Model.ScaleCalculationItems[cboPriceScales.ItemIndex - 1].Scale;
                    _BasicCalculation.UpdateGroupAmountAll(_Model, false);

                    gridView1.ActiveFilter.NonColumnFilter = "[Group] > 3";

                    //display scale-number input
                    this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInRuntime;


                    //if (cboPriceScales.ItemIndex == 0)
                    //{
                    //    gridView1.ActiveFilter.NonColumnFilter = "[Group] < 4";
                    //}
                    //else
                    //{
                    //    gridView1.ActiveFilter.NonColumnFilter = "[Group] > 3";
                    //}
                }
            }

            gridView1.RefreshData();
        }

        private void InitData()
        {
            int itemOrder = 0;
            SetBasicCalculationData(_Model.GeneralSetting, _Model.PriceSetting, ref itemOrder);
            SetPriceScaleCalculationData(_Model.GeneralSetting, _Model.PriceSetting, itemOrder);
            SetPredefineProfitAmount();

            //bind data
            gridControl1.DataSource = _Model.BasicCalculationItems;

            //Setup columns
            gridView1.RowSeparatorHeight = 2;

            gridView1.Columns[TempColumnNames.Sign.ToString()].Width = 15;
            gridView1.Columns[TempColumnNames.Sign.ToString()].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Columns[TempColumnNames.Sign.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Sign.ToString()].Caption = " ";

            gridView1.Columns[TempColumnNames.Description.ToString()].ColumnEdit = this.repositoryItemTextEdit3;
            gridView1.Columns[TempColumnNames.Description.ToString()].Caption = "Kostenanteil";

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

            gridView1.Columns[TempColumnNames.Tag.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Tag.ToString()].Caption = "Kürzel";

            gridView1.Columns[TempColumnNames.Group.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Group.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.Order.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Order.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.IsSummary.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.IsSummary.ToString()].Visible = false;

            //gridView1.Columns[TempColumnNames.UpdatedAmountField.ToString()].OptionsColumn.AllowEdit = false;
            //gridView1.Columns[TempColumnNames.UpdatedAmountField.ToString()].Visible = false;

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

        void SetBasicCalculationData(GeneralSettingModel generalSettingModel, PriceSetting priceSetting, ref int order)
        {
            //group1
            //int order = 0;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "",
                Description = "Bareinkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BEK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 0,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Bezugskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BZK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            if (_Model.GeneralSetting.TextLines != null)
            {
                int count = 0;
                foreach (string item in _Model.GeneralSetting.TextLines)
                {
                    count += 1;
                    order += 1;
                    _Model.BasicCalculationItems.Add(new CalculationItemModel()
                    {
                        Sign = "+",
                        Description = item,
                        AmountPercent = 0,
                        AmountFix = 0,
                        Total = 0,
                        Tag = String.Format("BEN {0}", count),
                        //Currency = generalSettingModel.Currency.Currency,
                        Currency = new CurrencyModel() { Currency = "CHF" },
                        Group = 1,
                        Order = order,
                        Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                    });
                }
            }

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "ESTP",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 1,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group2
            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verwaltungsgemeinkosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "OGK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Vertriebsgemeinkosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VGK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Sondereinzelkosten des Vertriebs",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VSK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Verwaltungs- und Vertriebskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VVK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 2 },
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "ESTP",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 1",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "SK 1",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2 },
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });


            //group3
            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Lagerhaltungskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "LHK",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verpackungsanteil",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VPA",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Transportanteil",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "TRA",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });

            order += 1;
            _Model.BasicCalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 2",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "SK 2",
                //Currency = generalSettingModel.Currency.Currency,
                Currency = new CurrencyModel() { Currency = "CHF" },
                Group = 3,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                Order = order,
                Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
            });
        }

        void SetPriceScaleCalculationData(GeneralSettingModel generalSettingModel, PriceSetting priceSetting, int order)
        {
            for (int i = 0; i < generalSettingModel.PriceScale.Scale; i++)
            {
                int iOrder = order;

                CalculationScaleModel oScale = new CalculationScaleModel()
                {
                    ID = i,
                    Scale = 0,
                    CalculationItems = new List<CalculationItemModel>()
                };

                //group4   
                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Gewinnaufschlag",
                    AmountPercent = 0,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "GA",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3 },
                    Group = 4,
                    //Group = 0,
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Barverkaufspreis",
                    AmountPercent = 0,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "VK(bar)",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 4,
                    //Group = 0,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4 },
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group5
                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenskonto",
                    AmountPercent = priceSetting.CashDiscount,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "SKT",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    //Group = 1,
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Verhandlungsspielraum",
                    AmountPercent = priceSetting.SalesBonus,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "PV",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                    Group = 5,
                    //Group = 1,
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Zielverkaufspreis",
                    AmountPercent = 0,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "VK(ziel)",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 5,
                    //Group = 1,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group6
                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Kundenrabatt",
                    AmountPercent = priceSetting.CustomerDiscount,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "RBT",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5 },
                    Group = 6,
                    //Group = 2,
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Nettoverkaufspreis",
                    AmountPercent = 0,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "VK(liste)",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 6,
                    //Group = 2,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });


                //group7
                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "+",
                    Description = "Mehrwertsteuer",
                    AmountPercent = priceSetting.VatTaxes,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "MWST",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                    Group = 7,
                    //Group = 3,
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                iOrder += 1;
                oScale.CalculationItems.Add(new CalculationItemModel()
                {
                    Sign = "=",
                    Description = "Bruttoverkaufspreis",
                    AmountPercent = 0,
                    AmountFix = 0,
                    Total = 0,
                    Tag = "VK(brutto)",
                    //Currency = generalSettingModel.Currency.Currency,
                    Currency = new CurrencyModel() { Currency = "CHF" },
                    Group = 7,
                    //Group = 3,
                    IsSummary = true,
                    SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 },
                    Order = iOrder,
                    Convert = generalSettingModel.Convert.Mode == "E" ? new ConvertModel() { Unit = "EE" } : null
                });

                _Model.ScaleCalculationItems.Add(oScale);
            }
        }

        void SetPredefineProfitAmount()
        {
            //set predefined profit amount            
            if (_Model.ScaleCalculationItems.Count > 1)
            {
                if (_Model.GeneralSetting.PriceScale.MinProfit > 0 && _Model.GeneralSetting.PriceScale.MaxProfit > 0)
                {
                    //everage amount excluded first item
                    decimal iEverageAmount = (_Model.GeneralSetting.PriceScale.MaxProfit -
                        _Model.GeneralSetting.PriceScale.MinProfit) / (_Model.ScaleCalculationItems.Count - 1);

                    for (int i = 0; i < _Model.ScaleCalculationItems.Count; i++)
                    {
                        decimal iProfitAmount = 0;
                        if (i == 0)
                        {
                            // set first predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MaxProfit;
                        }
                        else if (i == _Model.ScaleCalculationItems.Count - 1)
                        {
                            // set last predefined
                            iProfitAmount = _Model.GeneralSetting.PriceScale.MinProfit;
                        }
                        else
                        {
                            // set rest predefined
                            iProfitAmount = iEverageAmount * ((_Model.ScaleCalculationItems.Count - 1) - i);
                        }

                        //update amount to calculation model
                        //if percent or fix predefined
                        if (_Model.GeneralSetting.PriceScale.MarkUp == "P")
                        {
                            _BasicCalculation.UpdateRowAmountPercent(_Model, _Model.ScaleCalculationItems[i].CalculationItems[0], iProfitAmount, skipBaseGroupRows: true);
                        }
                        else
                        {
                            _BasicCalculation.UpdateRowAmountFix(_Model, _Model.ScaleCalculationItems[i].CalculationItems[0], iProfitAmount, skipBaseGroupRows: true);
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
                if (sTag == "BEK")
                {
                    //_Model.MasterAmount = Convert.ToDecimal(e.Value);
                    _BasicCalculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                    _BasicCalculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
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
                        _BasicCalculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), true, isSpecial, true);
                        _BasicCalculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "");
                    }
                    else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                    {
                        _BasicCalculation.UpdateCalculationRowAmount(_Model, iRowOrder, Convert.ToDecimal(e.Value), false, isSpecial, true);
                        _BasicCalculation.UpdateCalculationRowCurrencyField(_Model, iRowOrder, "F");
                    }
                }

                _BasicCalculation.UpdateGroupAmountAll(_Model, false);
            }

            gridView1.RefreshData();
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

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //disable editor for particular row's cell
            if (e.RowHandle > -1)
            {
                string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
                switch (sTag)
                {
                    case "BEK":
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
                        break;
                    case "SKT":
                    case "PV":
                    case "RBT":
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
                        break;
                    case "ESTP":
                    case "VVK":
                    case "SK 1":
                    case "SK 2":
                    case "VK(bar)":
                    case "VK(ziel)":
                    case "VK(liste)":
                    case "VK(brutto)":
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
                }
            }
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView1.FocusedRowHandle > -1)
            {
                //allow edit description for BEN(s) items only
                if (gridView1.FocusedColumn.FieldName == TempColumnNames.Description.ToString())
                {
                    if (!gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString().StartsWith("BEN"))
                    {
                        e.Cancel = true;
                    }
                }
                else if (gridView1.FocusedColumn.FieldName == TempColumnNames.Currency.ToString())
                {
                    //show currency editor for master amount
                    if (_Model.GeneralSetting.Currency.Mode == "E")
                    {
                        if (String.IsNullOrWhiteSpace(_BasicCalculation.GetCalculationRowCurrencyFieldEditable(
                            _Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle))))
                        {
                            e.Cancel = true;
                        }
                    }

                    //if (_Model.GeneralSetting.Currency.Mode == "E")
                    //{

                    //    if (!gridView1.GetRowCellValue(
                    //        gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString().StartsWith("BEK"))
                    //    {                            
                    //        e.Cancel = true;

                    //        ////if GA and both custom currency and convertion, allow editor
                    //        //if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString().StartsWith("GA"))
                    //        //{
                    //        //    if (_Model.GeneralSetting.Convert.Mode != "E")
                    //        //    {
                    //        //        e.Cancel = true;
                    //        //    }
                    //        //}
                    //        //else
                    //        //{
                    //        //    e.Cancel = true;
                    //        //}
                    //    }
                    //}
                }
                //else if (gridView1.FocusedColumn.FieldName == TempColumnNames.AmountFix.ToString())
                //{
                //    //disable amount fix editor if use both custom currency and convertion 
                //    if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString().StartsWith("GA"))
                //    {
                //        if (_Model.GeneralSetting.Currency.Mode == "E" && _Model.GeneralSetting.Convert.Mode == "E")
                //        {
                //            e.Cancel = true;
                //        }
                //    }
                //}
            }
        }

        private void CboPriceScales_EditValueChanged(object sender, EventArgs e)
        {
            RefreshGrid();
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
                    _BasicCalculation.UpdateCalculationRowUnit(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "VE");
                }
                else
                {
                    ed.Properties.Buttons[0].Caption = "EE";
                    _BasicCalculation.UpdateCalculationRowUnit(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "EE");
                }

                _BasicCalculation.UpdateGroupAmountAll(_Model, false);
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
                    _BasicCalculation.UpdateCalculationRowCurrency(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), _Model.GeneralSetting.Currency.Currency);
                }
                else
                {
                    ed.Properties.Buttons[0].Caption = "CHF";
                    _BasicCalculation.UpdateCalculationRowCurrency(_Model, gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle), "CHF");
                }

                _BasicCalculation.UpdateGroupAmountAll(_Model, false);
            }

            gridView1.RefreshData();
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                if (e.Column.FieldName == TempColumnNames.Convert.ToString())
                {
                    ButtonEditViewInfo editInfo = (ButtonEditViewInfo)((GridCellInfo)e.Cell).ViewInfo;
                    if (editInfo.RightButtons.Count > 0)
                    {
                        editInfo.RightButtons[0].Button.Caption = _BasicCalculation.GetCalculationRowUnitValue(_Model, gridView1.GetDataSourceRowIndex(e.RowHandle));
                    }
                }
                else if (e.Column.FieldName == TempColumnNames.Currency.ToString())
                {
                    ButtonEditViewInfo editInfo = (ButtonEditViewInfo)((GridCellInfo)e.Cell).ViewInfo;
                    if (editInfo.RightButtons.Count > 0)
                    {
                        editInfo.RightButtons[0].Button.Caption = _BasicCalculation.GetCalculationRowCurrencyValue(_Model, gridView1.GetDataSourceRowIndex(e.RowHandle));
                    }
                }
            }
        }

        private void TxtScaleNumber_EditValueChanged(object sender, System.EventArgs e)
        {
            if (cboPriceScales.ItemIndex > 0)
            {
                _Model.ScaleCalculationItems[cboPriceScales.ItemIndex - 1].Scale = Convert.ToDecimal(txtScaleNumber.Text);
            }
        }
    }
}
