namespace RMS_UI.PaymentMethods
{
    partial class PaymentMethodSettingsDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // PaymentMethodSettingsDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 500);
            Name = "PaymentMethodSettingsDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Payment Methods";
            Load += PaymentMethodSettingsDialog_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}
