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

namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    public partial class CalculationBasicCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void SaveChangedCallback();
        public event SaveChangedCallback SaveChanged;

        DataTable _Dt;
        CalculationModel _Model;
        string _NumFormat = "{0:n4}";
        string _NumFormatColumn = "n4";

        //DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit;

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
            SummaryGroups
        }

        public CalculationBasicCtrl()
        {
            InitializeComponent();

            gridView1.GridControl.Paint += GridControl_Paint;
        }
        private void SettingCtrl_Load(object sender, EventArgs e)
        {
            this.repositoryItemButtonEdit1.Buttons.Clear();
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
            GridColumn newCol = gridView1.Columns.Add();
            newCol.VisibleIndex = col.VisibleIndex + 1;
            emptyColumns.Add(newCol);
            newCol.OptionsColumn.AllowFocus = false;
            newCol.MinWidth = 1;
            newCol.Width = 5;
            newCol.OptionsColumn.AllowSize = false;
        }

        internal void Reload(GeneralSettingModel generalSettingModel, PriceSetting priceSetting)
        {
            InitModel(generalSettingModel, priceSetting);

            //InitDataSource(generalSettingModel.TextLines, priceSetting);

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

            gridView1.Columns[TempColumnNames.Currency.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Currency.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.Group.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Group.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.Order.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.Order.ToString()].Visible = false;

            gridView1.Columns[TempColumnNames.IsSummary.ToString()].OptionsColumn.AllowEdit = false;
            gridView1.Columns[TempColumnNames.IsSummary.ToString()].Visible = false;
        }

        void InitModel(GeneralSettingModel generalSettingModel, PriceSetting priceSetting)
        {
            _Model = new CalculationModel()
            {
                GeneralSetting = generalSettingModel,        
                PriceSetting = priceSetting,
                CalculationItems = new List<CalculationItemModel>()
            };

            //group1
            int order = 0;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "",
                Description = "Bareinkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BEK",
                Currency = generalSettingModel.Currency.Currency,
                Group = 0,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Bezugskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "BZK",
                Currency = generalSettingModel.Currency.Currency,
                Group = 1,
                Order = order
            });

            if (_Model.GeneralSetting.TextLines != null)
            {
                int count = 0;
                foreach (string item in _Model.GeneralSetting.TextLines)
                {
                    count += 1;
                    order += 1;
                    _Model.CalculationItems.Add(new CalculationItemModel()
                    {
                        Sign = "+",
                        Description = item,
                        AmountPercent = 0,
                        AmountFix = 0,
                        Total = 0,
                        Tag = String.Format("BEN {0}", count),
                        Currency = generalSettingModel.Currency.Currency,
                        Group = 1,
                        Order = order
                    });
                }
            }

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "ESTP",
                Currency = generalSettingModel.Currency.Currency,
                Group = 1,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                Order = order
            });


            //group2
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verwaltungsgemeinkosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "OGK",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Vertriebsgemeinkosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VGK",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Sondereinzelkosten des Vertriebs",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VSK",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1 },
                Group = 2,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Verwaltungs- und Vertriebskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VVK",
                Currency = generalSettingModel.Currency.Currency,
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 2 },
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Einstandspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "ESTP",
                Currency = generalSettingModel.Currency.Currency,
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1 },
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 1",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "SK 1",
                Currency = generalSettingModel.Currency.Currency,
                Group = 2,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2 },
                Order = order
            });


            //group3
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Lagerhaltungskosten",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "LHK",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verpackungsanteil",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VPA",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Transportanteil",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "TRA",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2 },
                Group = 3,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Selbstkosten 2",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "SK 2",
                Currency = generalSettingModel.Currency.Currency,
                Group = 3,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3 },
                Order = order
            });


            //group4
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Gewinnaufschlag",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "GA",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3 },
                Group = 4,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Barverkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VK(bar)",
                Currency = generalSettingModel.Currency.Currency,
                Group = 4,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3, 4 },
                Order = order
            });


            //group5
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Kundenskonto",
                AmountPercent = priceSetting.CashDiscount,
                AmountFix = 0,
                Total = 0,
                Tag = "SKT",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                Group = 5,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Verhandlungsspielraum",
                AmountPercent = priceSetting.SalesBonus,
                AmountFix = 0,
                Total = 0,
                Tag = "PV",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4 },
                Group = 5,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Zielverkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VK(ziel)",
                Currency = generalSettingModel.Currency.Currency,
                Group = 5,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5 },
                Order = order
            });


            //group6
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Kundenrabatt",
                AmountPercent = priceSetting.CustomerDiscount,
                AmountFix = 0,
                Total = 0,
                Tag = "RBT",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5 },
                Group = 6,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Nettoverkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VK(liste)",
                Currency = generalSettingModel.Currency.Currency,
                Group = 6,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                Order = order
            });


            //group7
            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "+",
                Description = "Mehrwertsteuer",
                AmountPercent = priceSetting.VatTaxes,
                AmountFix = 0,
                Total = 0,
                Tag = "MWST",
                Currency = generalSettingModel.Currency.Currency,
                CalculationBaseGroupRows = new List<int>() { 0, 1, 2, 3, 4, 5, 6 },
                Group = 7,
                Order = order
            });

            order += 1;
            _Model.CalculationItems.Add(new CalculationItemModel()
            {
                Sign = "=",
                Description = "Bruttoverkaufspreis",
                AmountPercent = 0,
                AmountFix = 0,
                Total = 0,
                Tag = "VK(brutto)",
                Currency = generalSettingModel.Currency.Currency,
                Group = 7,
                IsSummary = true,
                SummaryGroups = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 },
                Order = order
            });


            gridControl1.DataSource = _Model.CalculationItems;
            gridView1.RefreshData();
        }

        void InitDataSource(List<string> textLine, PriceSetting priceSetting)
        {
            _Dt = new DataTable("Calculation");
            DataColumn dc = null;

            dc = new DataColumn(TempColumnNames.Sign.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "";
            _Dt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Description.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "Kostenanteil";
            _Dt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.AmountPercent.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "%";
            _Dt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.AmountFix.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "Fix";
            _Dt.Columns.Add(dc);

            //dc = new DataColumn(TempColumnNames.Amount.ToString());
            //dc.DataType = typeof(decimal);
            //dc.Caption = "SFr";
            //_Dt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Total.ToString());
            dc.DataType = typeof(decimal);
            dc.Caption = "SFr";
            _Dt.Columns.Add(dc);

            dc = new DataColumn(TempColumnNames.Tag.ToString());
            dc.DataType = typeof(string);
            dc.Caption = "Kürzel";
            _Dt.Columns.Add(dc);

            //dc = new DataColumn(TempColumnNames.Group.ToString());
            //dc.DataType = typeof(string);            
            //_Dt.Columns.Add(dc);


            //set lines
            DataRow dr = null;

            //group1
            //set price
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "";
            dr[TempColumnNames.Description.ToString()] = "Bareinkaufspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "BEK";
            //dr[TempColumnNames.Group.ToString()] = "GRP1";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Bezugskosten";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "BZK";
            //dr[TempColumnNames.Group.ToString()] = "GRP1";
            _Dt.Rows.Add(dr);

            //set text line
            if (textLine != null)
            {
                int count = 0;
                foreach (string item in textLine)
                {
                    count += 1;
                    dr = _Dt.NewRow();
                    dr[TempColumnNames.Sign.ToString()] = "+";
                    dr[TempColumnNames.Description.ToString()] = item;
                    dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
                    dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
                    dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
                    dr[TempColumnNames.Tag.ToString()] = String.Format("BEN {0}", count);
                    //dr[TempColumnNames.Group.ToString()] = "GRP1";
                    _Dt.Rows.Add(dr);
                }
            }



            //subtotal group1
            //Einstandspreis
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Einstandspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "ESTP";
            _Dt.Rows.Add(dr);



            //group2
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Verwaltungsgemeinkosten";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "OGK";
            //dr[TempColumnNames.Group.ToString()] = "GRP2";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Vertriebsgemeinkosten";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VGK";
            //dr[TempColumnNames.Group.ToString()] = "GRP2";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Sondereinzelkosten des Vertriebs";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VSK";
            //dr[TempColumnNames.Group.ToString()] = "GRP2";
            _Dt.Rows.Add(dr);



            //subtotal group2
            //Verwaltungs- und Vertriebskosten
            //Einstandspreis
            //Selbstkosten 1
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Verwaltungs- und Vertriebskosten";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VVK";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Einstandspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "ESTP";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Selbstkosten 1";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "SK 1";
            _Dt.Rows.Add(dr);



            //group3
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Lagerhaltungskosten";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "LHK";
            //dr[TempColumnNames.Group.ToString()] = "GRP3";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Verpackungsanteil";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VPA";
            //dr[TempColumnNames.Group.ToString()] = "GRP3";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Transportanteil";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "TRA";
            //dr[TempColumnNames.Group.ToString()] = "GRP3";
            _Dt.Rows.Add(dr);


            //subtotal group3
            //Selbstkosten 2
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Selbstkosten 2";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "SK 2";
            _Dt.Rows.Add(dr);



            //group4
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Gewinnaufschlag";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "GA";
            //dr[TempColumnNames.Group.ToString()] = "GRP4";            
            _Dt.Rows.Add(dr);


            //subtotal group4
            //Barverkaufspreis
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Barverkaufspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VK(bar)";
            _Dt.Rows.Add(dr);



            //group5
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Kundenskonto";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, (priceSetting != null ? priceSetting.CashDiscount : 0));
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "SKT";
            //dr[TempColumnNames.Group.ToString()] = "GRP5";
            _Dt.Rows.Add(dr);

            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Verhandlungsspielraum";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, (priceSetting != null ? priceSetting.SalesBonus : 0));
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "PV";
            //dr[TempColumnNames.Group.ToString()] = "GRP5";
            _Dt.Rows.Add(dr);


            //subtotal group5
            //Zielverkaufspreis
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Zielverkaufspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VK(ziel)";
            _Dt.Rows.Add(dr);



            //group6
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Kundenrabatt";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, (priceSetting != null ? priceSetting.CustomerDiscount : 0));
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "RBT";
            //dr[TempColumnNames.Group.ToString()] = "GRP6";
            _Dt.Rows.Add(dr);


            //subtotal group6
            //Nettoverkaufspreis
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Nettoverkaufspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VK(liste)";
            _Dt.Rows.Add(dr);



            //group7
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "+";
            dr[TempColumnNames.Description.ToString()] = "Mehrwertsteuer";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, (priceSetting != null ? priceSetting.VatTaxes : 0));
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "MWST";
            //dr[TempColumnNames.Group.ToString()] = "GRP7";
            _Dt.Rows.Add(dr);


            //subtotal group7
            //Bruttoverkaufspreis
            dr = _Dt.NewRow();
            dr[TempColumnNames.Sign.ToString()] = "=";
            dr[TempColumnNames.Description.ToString()] = "Bruttoverkaufspreis";
            dr[TempColumnNames.AmountPercent.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.AmountFix.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Total.ToString()] = String.Format(_NumFormat, 0.00);
            dr[TempColumnNames.Tag.ToString()] = "VK(brutto)";
            _Dt.Rows.Add(dr);


            gridControl1.DataSource = _Dt;
            gridView1.RefreshData();
        }

        void UpdateCalculationRowAmount(int rowID, decimal value, bool isPercent, bool specialCalculation)
        {
            CalculationItemModel oCalRow = _Model.CalculationItems[rowID];

            if (oCalRow != null)
            {
                //if not master amount row
                //master row's group is 0
                if (oCalRow.Group != 0)
                {
                    if (isPercent)
                    {
                        if (specialCalculation)
                        {
                            UpdateRowAmountPercentSpecial(oCalRow, value);
                        }
                        else
                        {
                            UpdateRowAmountPercent(oCalRow, value);
                        }
                    }
                    else
                    {
                        UpdateRowAmountFix(oCalRow, value);
                    }
                }
                else
                {
                    //set row's total amount
                    //master row
                    oCalRow.Total = value;
                }
            }
        }

        void UpdateRowAmountPercentSpecial(CalculationItemModel calRow, decimal value)
        {
            decimal iBaseAmount = _Model.MasterAmount;
            if (calRow.CalculationBaseGroupRows != null)
            {
                iBaseAmount = GetCalculationBaseSummaryGroups(calRow.CalculationBaseGroupRows);
            }

            //formular for SKT, PV, RBT(maximum input is 99.99 %
            // if edit one cell only (SKT or PV or RBT)
            // [90 % +10 % from input = 100 %]    [10 % from input]
            // (((100 / 90) * 108.131) *          (10 / 100))        = 12.0145555556

            //if edit multiple cell (SKT, PV)
            // get multiple summary percent first           
            // if SKT = 4, PV = 6 so sum is 10
            // so if SKT = 1
            // [90 % +10 % from input = 100 %]    [10 % from input]
            // (((100 / 90) * 108.131) *          (10 / 100))        = 12.0145555556
            // SKT = 12.0145555556 * (4/10) = 4.80582222224 (40%)
            // PV = 12.0145555556 * (6/10) = 7.20873333336 (60%)

            decimal iDiffer = 100 - value;
            calRow.AmountPercent = value;
            calRow.AmountFix = (((100 / iDiffer) * iBaseAmount) * (value / 100));

            if (calRow.Tag == "SKT" || calRow.Tag == "PV")
            {
                decimal iSummaryPercent = value;
                var oCalculationRows = _Model.CalculationItems.FindAll(item => !item.IsSummary && item.Group == calRow.Group);
                iSummaryPercent = oCalculationRows.Sum(item => item.AmountPercent);

                if (iSummaryPercent > 0 && iSummaryPercent < 100)
                {
                    iDiffer = 100 - iSummaryPercent;
                    calRow.AmountFix = (((100 / iDiffer) * iBaseAmount) * (iSummaryPercent / 100)) * (value / iSummaryPercent);
                }
            }

            calRow.Total = calRow.AmountFix;
        }

        void UpdateRowAmountPercent(CalculationItemModel calRow, decimal value)
        {
            decimal iBaseAmount = _Model.MasterAmount;
            if (calRow.CalculationBaseGroupRows != null)
            {
                iBaseAmount = GetCalculationBaseSummaryGroups(calRow.CalculationBaseGroupRows);
            }
            calRow.AmountPercent = value;
            calRow.AmountFix = iBaseAmount * (calRow.AmountPercent / 100);
            calRow.Total = calRow.AmountFix;
        }

        void UpdateRowAmountFix(CalculationItemModel calRow, decimal value)
        {
            decimal iBaseAmount = _Model.MasterAmount;
            if (calRow.CalculationBaseGroupRows != null)
            {
                iBaseAmount = GetCalculationBaseSummaryGroups(calRow.CalculationBaseGroupRows);
            }
            calRow.AmountPercent = (value / iBaseAmount) * 100;
            calRow.AmountFix = value;
            calRow.Total = calRow.AmountFix;
        }

        decimal GetCalculationBaseSummaryGroups(List<int> calculationGroups)
        {
            decimal iSumCalRow = 0;

            foreach (int i in calculationGroups)
            {
                var oCalculationRows = _Model.CalculationItems.FindAll(item => !item.IsSummary && calculationGroups.Contains(item.Group));

                if (oCalculationRows != null)
                {
                    iSumCalRow = (from calRow in oCalculationRows select calRow.Total).Sum();
                }
            }

            return iSumCalRow;
        }

        void UpdateGroupAmountAll(bool updateGroupOnly)
        {
            var oModels = _Model.CalculationItems.FindAll(item => item.IsSummary);

            foreach (CalculationItemModel model in oModels)
            {
                UpdateGroupAmount(model.Group, model.Order, updateGroupOnly);
            }
        }

        void UpdateGroupAmount(int group, int groupID, bool updateGroupOnly)
        {
            CalculationItemModel oGroup = _Model.CalculationItems.Find(item => item.Group == group && item.Order == groupID && item.IsSummary);

            if (oGroup != null)
            {
                oGroup.Total = 0;

                //update items in group list first
                foreach (int i in oGroup.SummaryGroups)
                {
                    //get items per group
                    var oModels = _Model.CalculationItems.FindAll(item => item.Group == i && !item.IsSummary);

                    if (!updateGroupOnly)
                    {
                        foreach (CalculationItemModel model in oModels)
                        {
                            bool isSpecial = false;
                            if (model.Group == 0)
                            {
                                UpdateCalculationRowAmount(model.Order, model.AmountFix, false, isSpecial);
                            }
                            else
                            {
                                if (model.Tag == "SKT" || model.Tag == "PV" || model.Tag == "RBT")
                                {
                                    isSpecial = true;
                                }
                                UpdateCalculationRowAmount(model.Order, model.AmountPercent, true, isSpecial);
                            }
                        }
                    }

                    //update group total only
                    oGroup.Total += (from item in oModels select item.Total).Sum();
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

                //if master amount
                if (sTag == "BEK")
                {
                    _Model.MasterAmount = Convert.ToDecimal(e.Value);
                    UpdateCalculationRowAmount(e.RowHandle, _Model.MasterAmount, false, isSpecial);
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
                        UpdateCalculationRowAmount(e.RowHandle, Convert.ToDecimal(e.Value), true, isSpecial);
                    }
                    else if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                    {
                        UpdateCalculationRowAmount(e.RowHandle, Convert.ToDecimal(e.Value), false, isSpecial);
                    }
                }

                UpdateGroupAmountAll(false);
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

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.RowHandle > -1)
            //{
            //    string sTag = gridView1.GetRowCellValue(e.RowHandle, TempColumnNames.Tag.ToString()).ToString();
            //    if (sTag == "BEK")
            //    {
            //        if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
            //        {
            //            e.Appearance.BackColor = Color.LemonChiffon;
            //        }                    
            //    }
            //    else if (sTag.StartsWith("BEN"))
            //    {
            //        if (e.Column.FieldName == TempColumnNames.AmountPercent.ToString())
            //        {

            //            e.Appearance.BackColor = Color.LemonChiffon;
            //        }                    
            //    }
            //}
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
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
                        }
                        break;
                    case "SKT":
                    case "PV":
                    case "RBT":
                        if (e.Column.FieldName == TempColumnNames.AmountFix.ToString())
                        {
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
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
                            e.RepositoryItem = this.repositoryItemButtonEdit1;
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
                if (gridView1.FocusedColumn.FieldName == TempColumnNames.Description.ToString() &&
                    !gridView1.GetRowCellValue(gridView1.FocusedRowHandle, TempColumnNames.Tag.ToString()).ToString().StartsWith("BEN"))
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
