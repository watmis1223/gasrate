namespace CalculationOilPrice.Library.UI
{
    partial class QuantityMatrixComponent
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
            this.wirMatrixGridControl = new DevExpress.XtraGrid.GridControl();
            this.wirMatrixGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.wirMatrixGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wirMatrixGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // wirMatrixGridControl
            // 
            this.wirMatrixGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wirMatrixGridControl.Location = new System.Drawing.Point(0, 0);
            this.wirMatrixGridControl.MainView = this.wirMatrixGridView;
            this.wirMatrixGridControl.Name = "wirMatrixGridControl";
            this.wirMatrixGridControl.Size = new System.Drawing.Size(341, 192);
            this.wirMatrixGridControl.TabIndex = 0;
            this.wirMatrixGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.wirMatrixGridView});
            // 
            // wirMatrixGridView
            // 
            this.wirMatrixGridView.GridControl = this.wirMatrixGridControl;
            this.wirMatrixGridView.Name = "wirMatrixGridView";
            this.wirMatrixGridView.OptionsCustomization.AllowColumnMoving = false;
            this.wirMatrixGridView.OptionsCustomization.AllowColumnResizing = false;
            this.wirMatrixGridView.OptionsCustomization.AllowFilter = false;
            this.wirMatrixGridView.OptionsCustomization.AllowGroup = false;
            this.wirMatrixGridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.wirMatrixGridView.OptionsCustomization.AllowSort = false;
            this.wirMatrixGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.wirMatrixGridView.OptionsView.ShowDetailButtons = false;
            this.wirMatrixGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.wirMatrixGridView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.wirMatrixGridView.OptionsView.ShowGroupPanel = false;
            this.wirMatrixGridView.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.wirMatrixGridView_FocusedColumnChanged);
            this.wirMatrixGridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.wirMatrixGridView_InitNewRow);
            this.wirMatrixGridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.wirMatrixGridView_RowCellClick);
            // 
            // QuantityMatrixComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wirMatrixGridControl);
            this.Name = "QuantityMatrixComponent";
            this.Size = new System.Drawing.Size(341, 192);
            ((System.ComponentModel.ISupportInitialize)(this.wirMatrixGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wirMatrixGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl wirMatrixGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView wirMatrixGridView;
    }
}
