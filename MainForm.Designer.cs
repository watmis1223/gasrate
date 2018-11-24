namespace CalculationOilPrice
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.brBrnPrice = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.brBtnPriceCalculation = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnPriceSetting = new DevExpress.XtraBars.BarButtonItem();
            this.brBtnOil = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.brBrnPrice,
            this.brBtnOil,
            this.brBtnPriceCalculation,
            this.brBtnPriceSetting});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 10;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1748, 281);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // brBrnPrice
            // 
            this.brBrnPrice.ActAsDropDown = true;
            this.brBrnPrice.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.brBrnPrice.Caption = "Price";
            this.brBrnPrice.DropDownControl = this.popupMenu1;
            this.brBrnPrice.Id = 4;
            this.brBrnPrice.ImageOptions.Image = global::CalculationOilPrice.Properties.Resources.tag_16x16;
            this.brBrnPrice.ImageOptions.LargeImage = global::CalculationOilPrice.Properties.Resources.tag_32x32;
            this.brBrnPrice.Name = "brBrnPrice";
            this.brBrnPrice.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // popupMenu1
            // 
            this.popupMenu1.ItemLinks.Add(this.brBtnPriceCalculation);
            this.popupMenu1.ItemLinks.Add(this.brBtnPriceSetting);
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.Ribbon = this.ribbonControl1;
            // 
            // brBtnPriceCalculation
            // 
            this.brBtnPriceCalculation.Caption = "Kalkulation";
            this.brBtnPriceCalculation.Id = 8;
            this.brBtnPriceCalculation.Name = "brBtnPriceCalculation";
            this.brBtnPriceCalculation.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnPriceCalculation_ItemClick);
            // 
            // brBtnPriceSetting
            // 
            this.brBtnPriceSetting.Caption = "Settings";
            this.brBtnPriceSetting.Id = 9;
            this.brBtnPriceSetting.Name = "brBtnPriceSetting";
            this.brBtnPriceSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnPriceSetting_ItemClick);
            // 
            // brBtnOil
            // 
            this.brBtnOil.Caption = "Oil";
            this.brBtnOil.Id = 7;
            this.brBtnOil.ImageOptions.Image = global::CalculationOilPrice.Properties.Resources.database_16x161;
            this.brBtnOil.ImageOptions.LargeImage = global::CalculationOilPrice.Properties.Resources.database_32x321;
            this.brBtnOil.Name = "brBtnOil";
            this.brBtnOil.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.brBtnOil.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brBtnOil_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Modules";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.brBtnOil);
            this.ribbonPageGroup1.ItemLinks.Add(this.brBrnPrice);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 281);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1748, 1036);
            this.pnlMain.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1748, 1317);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculation Oil Price";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }                

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem brBrnPrice;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem brBtnOil;        
        private DevExpress.XtraBars.BarButtonItem brBtnPriceCalculation;
        private DevExpress.XtraBars.BarButtonItem brBtnPriceSetting;
        private DevExpress.XtraEditors.PanelControl pnlMain;
    }
}