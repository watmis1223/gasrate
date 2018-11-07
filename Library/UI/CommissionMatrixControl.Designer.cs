namespace CalculationOilPrice.Library.UI
{
    partial class CommissionMatrixControl
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
            this.mainLayoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.commissionGridControl = new DevExpress.XtraGrid.GridControl();
            this.commissionGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.mainLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.commissionLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).BeginInit();
            this.mainLayoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commissionGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionLayoutItem)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayoutControl
            // 
            this.mainLayoutControl.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.mainLayoutControl.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.mainLayoutControl.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.mainLayoutControl.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.mainLayoutControl.Controls.Add(this.commissionGridControl);
            this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutControl.Name = "mainLayoutControl";
            this.mainLayoutControl.Root = this.mainLayoutGroup;
            this.mainLayoutControl.Size = new System.Drawing.Size(610, 450);
            this.mainLayoutControl.TabIndex = 0;
            this.mainLayoutControl.Text = "layoutControl1";
            // 
            // commissionGridControl
            // 
            this.commissionGridControl.Location = new System.Drawing.Point(6, 6);
            this.commissionGridControl.MainView = this.commissionGridView;
            this.commissionGridControl.Name = "commissionGridControl";
            this.commissionGridControl.Size = new System.Drawing.Size(599, 439);
            this.commissionGridControl.TabIndex = 4;
            this.commissionGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.commissionGridView});
            // 
            // commissionGridView
            // 
            this.commissionGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.commissionGridView.GridControl = this.commissionGridControl;
            this.commissionGridView.Name = "commissionGridView";
            this.commissionGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.commissionGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.commissionGridView.OptionsCustomization.AllowColumnMoving = false;
            this.commissionGridView.OptionsCustomization.AllowColumnResizing = false;
            this.commissionGridView.OptionsCustomization.AllowFilter = false;
            this.commissionGridView.OptionsCustomization.AllowGroup = false;
            this.commissionGridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.commissionGridView.OptionsCustomization.AllowSort = false;
            this.commissionGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.commissionGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.commissionGridView.OptionsView.AllowCellMerge = true;
            this.commissionGridView.OptionsView.ShowDetailButtons = false;
            this.commissionGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.commissionGridView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.commissionGridView.OptionsView.ShowGroupPanel = false;
            this.commissionGridView.OptionsView.ShowIndicator = false;
            // 
            // mainLayoutGroup
            // 
            this.mainLayoutGroup.CustomizationFormText = "mainLayoutControlGroup";
            this.mainLayoutGroup.GroupBordersVisible = false;
            this.mainLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.commissionLayoutItem});
            this.mainLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutGroup.Name = "mainLayoutGroup";
            this.mainLayoutGroup.Size = new System.Drawing.Size(610, 450);
            this.mainLayoutGroup.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.mainLayoutGroup.Text = "mainLayoutGroup";
            this.mainLayoutGroup.TextVisible = false;
            // 
            // commissionLayoutItem
            // 
            this.commissionLayoutItem.Control = this.commissionGridControl;
            this.commissionLayoutItem.CustomizationFormText = "Order Items";
            this.commissionLayoutItem.Location = new System.Drawing.Point(0, 0);
            this.commissionLayoutItem.Name = "commissionLayoutItem";
            this.commissionLayoutItem.Size = new System.Drawing.Size(610, 450);
            this.commissionLayoutItem.Text = "Order Items";
            this.commissionLayoutItem.TextLocation = DevExpress.Utils.Locations.Left;
            this.commissionLayoutItem.TextSize = new System.Drawing.Size(0, 0);
            this.commissionLayoutItem.TextToControlDistance = 0;
            this.commissionLayoutItem.TextVisible = false;
            // 
            // CommissionMatrixControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutControl);
            this.Name = "CommissionMatrixControl";
            this.Size = new System.Drawing.Size(610, 450);
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).EndInit();
            this.mainLayoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.commissionGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionLayoutItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl mainLayoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup mainLayoutGroup;
        private DevExpress.XtraGrid.GridControl commissionGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView commissionGridView;
        private DevExpress.XtraLayout.LayoutControlItem commissionLayoutItem;
    }
}
