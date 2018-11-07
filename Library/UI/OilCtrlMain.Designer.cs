namespace CalculationOilPrice.Library.UI
{
    partial class OilCtrlMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.calculationTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.calculationControl1 = new CalculationOilPrice.Library.UI.CalculationControl();
            this.commissionTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.commissionControl1 = new CalculationOilPrice.Library.UI.CommissionControl();
            this.settingTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.settingControl1 = new CalculationOilPrice.Library.UI.SettingControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.calculationTabPage.SuspendLayout();
            this.commissionTabPage.SuspendLayout();
            this.settingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(6);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedTabPage = this.calculationTabPage;
            this.mainTabControl.Size = new System.Drawing.Size(1748, 1317);
            this.mainTabControl.TabIndex = 0;
            this.mainTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.calculationTabPage,
            this.commissionTabPage,
            this.settingTabPage});
            this.mainTabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.mainTabControl_SelectedPageChanged);
            // 
            // calculationTabPage
            // 
            this.calculationTabPage.Controls.Add(this.calculationControl1);
            this.calculationTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.calculationTabPage.Name = "calculationTabPage";
            this.calculationTabPage.Size = new System.Drawing.Size(1736, 1262);
            this.calculationTabPage.Text = "Kalkulation";
            // 
            // calculationControl1
            // 
            this.calculationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calculationControl1.Location = new System.Drawing.Point(0, 0);
            this.calculationControl1.Margin = new System.Windows.Forms.Padding(12);
            this.calculationControl1.Name = "calculationControl1";
            this.calculationControl1.Size = new System.Drawing.Size(1736, 1262);
            this.calculationControl1.TabIndex = 0;
            this.calculationControl1.PrintInvoice += new CalculationOilPrice.Library.UI.CalculationControl.PrintInvoiceCallback(this.calculationControl1_PrintInvoice);
            this.calculationControl1.SavedCalculation += new CalculationOilPrice.Library.UI.CalculationControl.SavedCalculationCallback(this.calculationControl1_SavedCalculation);
            // 
            // commissionTabPage
            // 
            this.commissionTabPage.Controls.Add(this.commissionControl1);
            this.commissionTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.commissionTabPage.Name = "commissionTabPage";
            this.commissionTabPage.Size = new System.Drawing.Size(1290, 396);
            this.commissionTabPage.Text = "Provision";
            // 
            // commissionControl1
            // 
            this.commissionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commissionControl1.Location = new System.Drawing.Point(0, 0);
            this.commissionControl1.Margin = new System.Windows.Forms.Padding(12);
            this.commissionControl1.Name = "commissionControl1";
            this.commissionControl1.Size = new System.Drawing.Size(1290, 396);
            this.commissionControl1.TabIndex = 0;
            // 
            // settingTabPage
            // 
            this.settingTabPage.Controls.Add(this.settingControl1);
            this.settingTabPage.Margin = new System.Windows.Forms.Padding(6);
            this.settingTabPage.Name = "settingTabPage";
            this.settingTabPage.Size = new System.Drawing.Size(1290, 396);
            this.settingTabPage.Text = "Einstellungen";
            // 
            // settingControl1
            // 
            this.settingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingControl1.Location = new System.Drawing.Point(0, 0);
            this.settingControl1.Margin = new System.Windows.Forms.Padding(12);
            this.settingControl1.Name = "settingControl1";
            this.settingControl1.Size = new System.Drawing.Size(1290, 396);
            this.settingControl1.TabIndex = 0;
            this.settingControl1.SettingSaveChanged += new CalculationOilPrice.Library.UI.SettingControl.SettingSaveChangedCallback(this.settingControl1_SettingSaveChanged);
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "navBarGroup1";
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem1),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem2)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "navBarItem1";
            this.navBarItem1.Name = "navBarItem1";
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "navBarItem2";
            this.navBarItem2.Name = "navBarItem2";
            // 
            // OilCtrlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "OilCtrlMain";
            this.Size = new System.Drawing.Size(1748, 1317);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainTabControl)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.calculationTabPage.ResumeLayout(false);
            this.commissionTabPage.ResumeLayout(false);
            this.settingTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }                

        #endregion

        private DevExpress.XtraTab.XtraTabControl mainTabControl;
        private DevExpress.XtraTab.XtraTabPage calculationTabPage;
        private DevExpress.XtraTab.XtraTabPage commissionTabPage;
        private DevExpress.XtraTab.XtraTabPage settingTabPage;
        private CalculationOilPrice.Library.UI.CalculationControl calculationControl1;
        private CalculationOilPrice.Library.UI.SettingControl settingControl1;
        private CalculationOilPrice.Library.UI.CommissionControl commissionControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
    }
}