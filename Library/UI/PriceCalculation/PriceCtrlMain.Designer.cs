namespace CalculationOilPrice.Library.UI.PriceCalculation
{
    partial class PriceCtrlMain
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
            this.mainTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.generalTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.lstCalculation = new DevExpress.XtraEditors.ListBoxControl();
            this.generalCtrl1 = new CalculationOilPrice.Library.UI.PriceCalculation.GeneralCtrl();
            this.settingTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.settingCtrl1 = new CalculationOilPrice.Library.UI.PriceCalculation.SettingCtrl();
            this.calculationTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.calculationBasicCtrl1 = new CalculationOilPrice.Library.UI.PriceCalculation.CalculationBasicCtrl();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar1 = new DevExpress.XtraBars.Bar();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstCalculation)).BeginInit();
            this.settingTabPage.SuspendLayout();
            this.calculationTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedTabPage = this.generalTabPage;
            this.mainTabControl.Size = new System.Drawing.Size(1532, 1142);
            this.mainTabControl.TabIndex = 0;
            this.mainTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.settingTabPage,
            this.generalTabPage,
            this.calculationTabPage});
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.splitContainerControl1);
            this.generalTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new System.Drawing.Size(1520, 1087);
            this.generalTabPage.Text = "Allgemein";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Collapsed = true;
            this.splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.lstCalculation);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.generalCtrl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1520, 1087);
            this.splitContainerControl1.SplitterPosition = 378;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // lstCalculation
            // 
            this.lstCalculation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCalculation.Location = new System.Drawing.Point(0, 0);
            this.lstCalculation.Name = "lstCalculation";
            this.lstCalculation.Size = new System.Drawing.Size(0, 0);
            this.lstCalculation.TabIndex = 0;
            // 
            // generalCtrl1
            // 
            this.generalCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalCtrl1.Location = new System.Drawing.Point(0, 0);
            this.generalCtrl1.Margin = new System.Windows.Forms.Padding(6);
            this.generalCtrl1.Name = "generalCtrl1";
            this.generalCtrl1.Size = new System.Drawing.Size(1510, 1087);
            this.generalCtrl1.TabIndex = 0;
            this.generalCtrl1.NewButtonClick += new CalculationOilPrice.Library.UI.PriceCalculation.GeneralCtrl.NewButtonClickCallback(this.generalCtrl1_NewButtonClick);
            // 
            // settingTabPage
            // 
            this.settingTabPage.Controls.Add(this.settingCtrl1);
            this.settingTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.settingTabPage.Name = "settingTabPage";
            this.settingTabPage.Size = new System.Drawing.Size(1520, 1087);
            this.settingTabPage.Text = "Einstellungen";
            // 
            // settingCtrl1
            // 
            this.settingCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingCtrl1.Location = new System.Drawing.Point(0, 0);
            this.settingCtrl1.Margin = new System.Windows.Forms.Padding(6);
            this.settingCtrl1.Name = "settingCtrl1";
            this.settingCtrl1.Size = new System.Drawing.Size(1520, 1087);
            this.settingCtrl1.TabIndex = 0;
            // 
            // calculationTabPage
            // 
            this.calculationTabPage.Controls.Add(this.calculationBasicCtrl1);
            this.calculationTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.calculationTabPage.Name = "calculationTabPage";
            this.calculationTabPage.PageVisible = false;
            this.calculationTabPage.Size = new System.Drawing.Size(1520, 1087);
            this.calculationTabPage.Text = "Kalkulation";
            // 
            // calculationBasicCtrl1
            // 
            this.calculationBasicCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calculationBasicCtrl1.Location = new System.Drawing.Point(0, 0);
            this.calculationBasicCtrl1.Margin = new System.Windows.Forms.Padding(6);
            this.calculationBasicCtrl1.Name = "calculationBasicCtrl1";
            this.calculationBasicCtrl1.Size = new System.Drawing.Size(1520, 1087);
            this.calculationBasicCtrl1.TabIndex = 0;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.DockCol = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.Text = "Status bar";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.Text = "Main menu";
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.Text = "Tools";
            // 
            // PriceCtrlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PriceCtrlMain";
            this.Size = new System.Drawing.Size(1532, 1142);
            this.Load += new System.EventHandler(this.PriceCtrlMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstCalculation)).EndInit();
            this.settingTabPage.ResumeLayout(false);
            this.calculationTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl mainTabControl;
        private DevExpress.XtraTab.XtraTabPage settingTabPage;
        private DevExpress.XtraTab.XtraTabPage calculationTabPage;
        private DevExpress.XtraTab.XtraTabPage generalTabPage;
        private SettingCtrl settingCtrl1;
        private GeneralCtrl generalCtrl1;
        private CalculationBasicCtrl calculationBasicCtrl1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.ListBoxControl lstCalculation;
    }
}
