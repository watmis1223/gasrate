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
using CalculationOilPrice.Library.Entity.PriceCalculation.Models;
using System.Linq;
using System.Collections;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using CalculationOilPrice.Library.Business.PriceCalculation;
using CalculationOilPrice.Library.Entity.PriceCalculation.Extensions;

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
            ItemOrder,
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

        public CalculationModel GetModel()
        {
            return _Model;
        }

        public void LoadProffixCalculation(GeneralSettingModel generalSettingModel, PriceCalculationSetting moduleSetting, string[] arguments)
        {
            //ProffixResult oResult = new ProffixResult(arguments);

            //if (arguments != null)
            //{
            //    if (arguments.Length == 2)
            //    {
            //        if (arguments[1].TrimStart().TrimEnd().Trim().StartsWith("opencal"))
            //        {
            //            //new or load
            //            string[] sSubParam = arguments[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //            if (sSubParam.Length == 2)
            //            {
            //                //new
            //                NewCalculation(generalSettingModel, moduleSetting, sSubParam[1]);
            //            }
            //            else if (sSubParam.Length > 2)
            //            {
            //                //load
            //            }
            //        }
            //    }
            //}
        }

        public void LoadCalculation(CalculationModel model, PriceCalculationSetting moduleSetting)
        {
            model.ProffixConnection = moduleSetting.ProffixConnection;
            
            _Model = model;

            //setup everythings
            SetUpCalculation();
        }

        public void NewCalculation(GeneralSettingModel generalSettingModel, PriceCalculationSetting moduleSetting)
        {
            _Model = new CalculationModel()
            {
                //set new id is zero
                ID = 0,
                GeneralSetting = generalSettingModel,
                ProffixConnection = moduleSetting.ProffixConnection,
                CalculationNotes = new List<CalculationNoteModel>(),
                CalculationViewItems = new List<CalculationItemModel>(),
                ProffixModel = generalSettingModel.ProffixModel
            };

            //setup new calculation
            _Model.SetBasicCalculationNote();
            _Model.SetScaleCalculationNote(moduleSetting.PriceSetting, _Model.CalculationNotes.First().CalculationItems.Last().ItemOrder);
            _Model.SetMarginCalculationNote();

            //setup everythings
            SetUpCalculation();
        }

        void SetUpCalculation()
        {
            //setup everythings here

            //if needed
            //setup list of data that binding to gridview
            _Model.SetCalculationViewData();
            //_Model.SetCalculationMarginViewData();


            //set calculation method first
            //basic calculation or reverse calculation
            if (_Model.GeneralSetting.CostType == "P")
            {
                _Calculation = new CostCalculation();
            }

            //set scale option if needed
            SetOptionScale();

            //set margin option if needed
            SetOptionMargin();


            //bind basic calculation gridview
            BindBasicCalculationView();
            //bind margin calculation gridview if needed
            BindMarginCalculationView();


            //refresh gridview
            RefreshGrid();

            //refresh margin gridview 
            RefreshGridMargin();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //save model to db
            StorageOperator.SaveModel(_Model);

            if (SaveChanged != null)
            {
                SaveChanged();
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

                    //load margin data into data list of gridview
                    _Model.SetCalculationMarginViewData(cboPriceScales.ItemIndex);

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

                //load margin data into data list of gridview
                _Model.SetCalculationMarginViewData(cboMargin.ItemIndex);

                //bind margin calculation gridview if needed
                BindMarginCalculationView();

                //calculate margin
                _MarginCalculation.UpdateBaseAmountAll(_Model);

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

        private void TxtScaleNumber_EditValueChanged(object sender, System.EventArgs e)
        {
            //after button edit control click, update value to calculation model
            if (cboPriceScales.ItemIndex > 0)
            {
                _Model.CalculationNotes[cboPriceScales.ItemIndex].Quantity = Convert.ToDecimal(txtScaleNumber.Text);
            }
        }
    }
}
