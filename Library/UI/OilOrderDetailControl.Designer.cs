namespace CalculationOilPrice.Library.UI
{
    partial class OilOrderDetailControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.orderDetailGridControl = new DevExpress.XtraGrid.GridControl();
            this.orderDetailGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // orderDetailGridControl
            // 
            this.orderDetailGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderDetailGridControl.Location = new System.Drawing.Point(0, 0);
            this.orderDetailGridControl.MainView = this.orderDetailGridView;
            this.orderDetailGridControl.Name = "orderDetailGridControl";
            this.orderDetailGridControl.Size = new System.Drawing.Size(581, 268);
            this.orderDetailGridControl.TabIndex = 0;
            this.orderDetailGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.orderDetailGridView});
            // 
            // orderDetailGridView
            // 
            this.orderDetailGridView.GridControl = this.orderDetailGridControl;
            this.orderDetailGridView.Name = "orderDetailGridView";
            this.orderDetailGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.orderDetailGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.orderDetailGridView.OptionsCustomization.AllowColumnMoving = false;
            this.orderDetailGridView.OptionsCustomization.AllowColumnResizing = false;
            this.orderDetailGridView.OptionsCustomization.AllowFilter = false;
            this.orderDetailGridView.OptionsCustomization.AllowGroup = false;
            this.orderDetailGridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.orderDetailGridView.OptionsCustomization.AllowSort = false;
            this.orderDetailGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.orderDetailGridView.OptionsView.ShowDetailButtons = false;
            this.orderDetailGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.orderDetailGridView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.orderDetailGridView.OptionsView.ShowGroupPanel = false;
            //this.orderDetailGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.orderDetailGridView_CustomDrawFooterCell);
            this.orderDetailGridView.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.orderDetailGridView_CustomRowFilter);
            this.orderDetailGridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.orderDetailGridView_CellValueChanged);
            this.orderDetailGridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.orderDetailGridView_InitNewRow);
            this.orderDetailGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.orderDetailGridView_KeyDown);
            this.orderDetailGridView.CustomSummaryCalculate += new DevExpress.Data.CustomSummaryEventHandler(this.orderDetailGridView_CustomSummaryCalculate);
            // 
            // OilOrderDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orderDetailGridControl);
            this.Name = "OilOrderDetailControl";
            this.Size = new System.Drawing.Size(581, 268);
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDetailGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl orderDetailGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView orderDetailGridView;
    }
}
