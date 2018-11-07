namespace CalculationOilPrice.Library.UI
{
    partial class SearchDialog
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
            this.orderSearchControl1 = new CalculationOilPrice.Library.UI.OrderSearchControl();
            this.SuspendLayout();
            // 
            // orderSearchControl1
            // 
            this.orderSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.orderSearchControl1.Name = "orderSearchControl1";
            this.orderSearchControl1.Size = new System.Drawing.Size(864, 642);
            this.orderSearchControl1.TabIndex = 0;
            this.orderSearchControl1.OnSelectedOrder += new CalculationOilPrice.Library.UI.OrderSearchControl.OnSelectedOrderCallback(this.orderSearchControl1_OnSelectedOrder);
            // 
            // SearchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 642);
            this.Controls.Add(this.orderSearchControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SearchDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private OrderSearchControl orderSearchControl1;
    }
}