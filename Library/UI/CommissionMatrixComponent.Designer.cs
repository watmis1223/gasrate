namespace CalculationOilPrice.Library.UI
{
    partial class CommissionMatrixComponent
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
            this.commissionMatrixGridControl = new DevExpress.XtraGrid.GridControl();
            this.commissionMatrixGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // commissionMatrixGridControl
            // 
            this.commissionMatrixGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commissionMatrixGridControl.Location = new System.Drawing.Point(0, 0);
            this.commissionMatrixGridControl.MainView = this.commissionMatrixGridView;
            this.commissionMatrixGridControl.Name = "commissionMatrixGridControl";
            this.commissionMatrixGridControl.Size = new System.Drawing.Size(321, 272);
            this.commissionMatrixGridControl.TabIndex = 0;
            this.commissionMatrixGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.commissionMatrixGridView});
            // 
            // commissionMatrixGridView
            // 
            this.commissionMatrixGridView.GridControl = this.commissionMatrixGridControl;
            this.commissionMatrixGridView.Name = "commissionMatrixGridView";
            this.commissionMatrixGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.commissionMatrixGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.commissionMatrixGridView.OptionsCustomization.AllowColumnMoving = false;
            this.commissionMatrixGridView.OptionsCustomization.AllowColumnResizing = false;
            this.commissionMatrixGridView.OptionsCustomization.AllowFilter = false;
            this.commissionMatrixGridView.OptionsCustomization.AllowGroup = false;
            this.commissionMatrixGridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.commissionMatrixGridView.OptionsCustomization.AllowSort = false;
            this.commissionMatrixGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.commissionMatrixGridView.OptionsView.ShowDetailButtons = false;
            this.commissionMatrixGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.commissionMatrixGridView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.commissionMatrixGridView.OptionsView.ShowGroupPanel = false;
            this.commissionMatrixGridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.commissionMatrixGridView_InitNewRow);
            this.commissionMatrixGridView.Click += new System.EventHandler(this.commissionMatrixGridView_Click);
            // 
            // CommissionMatrixComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.commissionMatrixGridControl);
            this.Name = "CommissionMatrixComponent";
            this.Size = new System.Drawing.Size(321, 272);
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl commissionMatrixGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView commissionMatrixGridView;

    }
}
