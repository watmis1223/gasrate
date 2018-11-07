namespace CalculationOilPrice.Library.UI
{
    partial class CommissionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommissionControl));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.mainLayoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.commissionMatrixControl = new CalculationOilPrice.Library.UI.CommissionMatrixControl();
            this.solutionLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.purchasePriceButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.orderQuantityButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.mainLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.orderQuantityLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.purchasePriceLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.solutionLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.commissionMatrixLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).BeginInit();
            this.mainLayoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.purchasePriceButtonEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderQuantityButtonEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderQuantityLayoutItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchasePriceLayoutItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionLayoutItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixLayoutItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayoutControl
            // 
            this.mainLayoutControl.Appearance.DisabledLayoutGroupCaption.ForeColor = System.Drawing.SystemColors.GrayText;
            this.mainLayoutControl.Appearance.DisabledLayoutGroupCaption.Options.UseForeColor = true;
            this.mainLayoutControl.Appearance.DisabledLayoutItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.mainLayoutControl.Appearance.DisabledLayoutItem.Options.UseForeColor = true;
            this.mainLayoutControl.Controls.Add(this.commissionMatrixControl);
            this.mainLayoutControl.Controls.Add(this.solutionLabelControl);
            this.mainLayoutControl.Controls.Add(this.purchasePriceButtonEdit);
            this.mainLayoutControl.Controls.Add(this.orderQuantityButtonEdit);
            this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutControl.Name = "mainLayoutControl";
            this.mainLayoutControl.Root = this.mainLayoutGroup;
            this.mainLayoutControl.Size = new System.Drawing.Size(699, 522);
            this.mainLayoutControl.TabIndex = 0;
            this.mainLayoutControl.Text = "layoutControl1";
            // 
            // commissionMatrixControl
            // 
            this.commissionMatrixControl.Location = new System.Drawing.Point(6, 123);
            this.commissionMatrixControl.Name = "commissionMatrixControl";
            this.commissionMatrixControl.Size = new System.Drawing.Size(688, 191);
            this.commissionMatrixControl.TabIndex = 7;
            // 
            // solutionLabelControl
            // 
            this.solutionLabelControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.solutionLabelControl.Location = new System.Drawing.Point(6, 86);
            this.solutionLabelControl.Name = "solutionLabelControl";
            this.solutionLabelControl.Size = new System.Drawing.Size(688, 26);
            this.solutionLabelControl.TabIndex = 6;
            this.solutionLabelControl.Text = resources.GetString("solutionLabelControl.Text");
            // 
            // purchasePriceButtonEdit
            // 
            this.purchasePriceButtonEdit.Location = new System.Drawing.Point(116, 37);
            this.purchasePriceButtonEdit.Name = "purchasePriceButtonEdit";
            this.purchasePriceButtonEdit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.purchasePriceButtonEdit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.purchasePriceButtonEdit.Properties.Appearance.Options.UseBackColor = true;
            this.purchasePriceButtonEdit.Properties.Appearance.Options.UseForeColor = true;
            this.purchasePriceButtonEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.purchasePriceButtonEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            serializableAppearanceObject1.ForeColor = System.Drawing.Color.Black;
            serializableAppearanceObject1.Options.UseForeColor = true;
            this.purchasePriceButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "CHF / 100 kg", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null)});
            this.purchasePriceButtonEdit.Properties.Mask.EditMask = "n2";
            this.purchasePriceButtonEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.purchasePriceButtonEdit.Properties.ReadOnly = true;
            this.purchasePriceButtonEdit.Size = new System.Drawing.Size(133, 20);
            this.purchasePriceButtonEdit.TabIndex = 5;
            // 
            // orderQuantityButtonEdit
            // 
            this.orderQuantityButtonEdit.Location = new System.Drawing.Point(116, 6);
            this.orderQuantityButtonEdit.Name = "orderQuantityButtonEdit";
            this.orderQuantityButtonEdit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.orderQuantityButtonEdit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.orderQuantityButtonEdit.Properties.Appearance.Options.UseBackColor = true;
            this.orderQuantityButtonEdit.Properties.Appearance.Options.UseForeColor = true;
            this.orderQuantityButtonEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.orderQuantityButtonEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            serializableAppearanceObject2.ForeColor = System.Drawing.Color.Black;
            serializableAppearanceObject2.Options.UseForeColor = true;
            this.orderQuantityButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "KG", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null)});
            this.orderQuantityButtonEdit.Properties.Mask.EditMask = "n2";
            this.orderQuantityButtonEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.orderQuantityButtonEdit.Properties.ReadOnly = true;
            this.orderQuantityButtonEdit.Size = new System.Drawing.Size(133, 20);
            this.orderQuantityButtonEdit.TabIndex = 4;
            // 
            // mainLayoutGroup
            // 
            this.mainLayoutGroup.CustomizationFormText = "mainLayoutGroup";
            this.mainLayoutGroup.GroupBordersVisible = false;
            this.mainLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.orderQuantityLayoutItem,
            this.purchasePriceLayoutItem,
            this.solutionLayoutItem,
            this.commissionMatrixLayoutItem,
            this.emptySpaceItem1,
            this.emptySpaceItem2});
            this.mainLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutGroup.Name = "mainLayoutGroup";
            this.mainLayoutGroup.Size = new System.Drawing.Size(699, 522);
            this.mainLayoutGroup.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.mainLayoutGroup.Text = "mainLayoutGroup";
            this.mainLayoutGroup.TextVisible = false;
            // 
            // orderQuantityLayoutItem
            // 
            this.orderQuantityLayoutItem.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
            this.orderQuantityLayoutItem.AppearanceItemCaption.Options.UseForeColor = true;
            this.orderQuantityLayoutItem.Control = this.orderQuantityButtonEdit;
            this.orderQuantityLayoutItem.CustomizationFormText = "layoutControlItem1";
            this.orderQuantityLayoutItem.Location = new System.Drawing.Point(0, 0);
            this.orderQuantityLayoutItem.Name = "orderQuantityLayoutItem";
            this.orderQuantityLayoutItem.Size = new System.Drawing.Size(254, 31);
            this.orderQuantityLayoutItem.Text = "Bestellmenge";
            this.orderQuantityLayoutItem.TextLocation = DevExpress.Utils.Locations.Left;
            this.orderQuantityLayoutItem.TextSize = new System.Drawing.Size(105, 13);
            // 
            // purchasePriceLayoutItem
            // 
            this.purchasePriceLayoutItem.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
            this.purchasePriceLayoutItem.AppearanceItemCaption.Options.UseForeColor = true;
            this.purchasePriceLayoutItem.Control = this.purchasePriceButtonEdit;
            this.purchasePriceLayoutItem.CustomizationFormText = "layoutControlItem2";
            this.purchasePriceLayoutItem.Location = new System.Drawing.Point(0, 31);
            this.purchasePriceLayoutItem.MaxSize = new System.Drawing.Size(254, 31);
            this.purchasePriceLayoutItem.MinSize = new System.Drawing.Size(254, 31);
            this.purchasePriceLayoutItem.Name = "purchasePriceLayoutItem";
            this.purchasePriceLayoutItem.Size = new System.Drawing.Size(254, 31);
            this.purchasePriceLayoutItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.purchasePriceLayoutItem.Text = "Einkaufspreis";
            this.purchasePriceLayoutItem.TextLocation = DevExpress.Utils.Locations.Left;
            this.purchasePriceLayoutItem.TextSize = new System.Drawing.Size(105, 13);
            // 
            // solutionLayoutItem
            // 
            this.solutionLayoutItem.Control = this.solutionLabelControl;
            this.solutionLayoutItem.CustomizationFormText = "solutionLayoutItem";
            this.solutionLayoutItem.Location = new System.Drawing.Point(0, 62);
            this.solutionLayoutItem.Name = "solutionLayoutItem";
            this.solutionLayoutItem.Size = new System.Drawing.Size(699, 55);
            this.solutionLayoutItem.Text = "Provisionsberechnung";
            this.solutionLayoutItem.TextLocation = DevExpress.Utils.Locations.Top;
            this.solutionLayoutItem.TextSize = new System.Drawing.Size(105, 13);
            // 
            // commissionMatrixLayoutItem
            // 
            this.commissionMatrixLayoutItem.Control = this.commissionMatrixControl;
            this.commissionMatrixLayoutItem.CustomizationFormText = "commissionMatrixLayoutItem";
            this.commissionMatrixLayoutItem.Location = new System.Drawing.Point(0, 117);
            this.commissionMatrixLayoutItem.MaxSize = new System.Drawing.Size(0, 202);
            this.commissionMatrixLayoutItem.MinSize = new System.Drawing.Size(111, 202);
            this.commissionMatrixLayoutItem.Name = "commissionMatrixLayoutItem";
            this.commissionMatrixLayoutItem.Size = new System.Drawing.Size(699, 202);
            this.commissionMatrixLayoutItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.commissionMatrixLayoutItem.Text = "commissionMatrixLayoutItem";
            this.commissionMatrixLayoutItem.TextLocation = DevExpress.Utils.Locations.Top;
            this.commissionMatrixLayoutItem.TextSize = new System.Drawing.Size(0, 0);
            this.commissionMatrixLayoutItem.TextToControlDistance = 0;
            this.commissionMatrixLayoutItem.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 319);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(699, 203);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(254, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(445, 62);
            this.emptySpaceItem2.Text = "emptySpaceItem2";
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // CommissionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutControl);
            this.Name = "CommissionControl";
            this.Size = new System.Drawing.Size(699, 522);
            this.Load += new System.EventHandler(this.CommissionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).EndInit();
            this.mainLayoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.purchasePriceButtonEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderQuantityButtonEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderQuantityLayoutItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchasePriceLayoutItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionLayoutItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionMatrixLayoutItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl mainLayoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup mainLayoutGroup;
        private DevExpress.XtraEditors.ButtonEdit purchasePriceButtonEdit;
        private DevExpress.XtraEditors.ButtonEdit orderQuantityButtonEdit;
        private DevExpress.XtraLayout.LayoutControlItem orderQuantityLayoutItem;
        private DevExpress.XtraLayout.LayoutControlItem purchasePriceLayoutItem;
        private DevExpress.XtraEditors.LabelControl solutionLabelControl;
        private DevExpress.XtraLayout.LayoutControlItem solutionLayoutItem;
        private CommissionMatrixControl commissionMatrixControl;
        private DevExpress.XtraLayout.LayoutControlItem commissionMatrixLayoutItem;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;

    }
}
