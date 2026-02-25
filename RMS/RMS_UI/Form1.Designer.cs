namespace RMS_UI
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            autoComplete1 = new Syncfusion.Windows.Forms.Tools.AutoComplete(components);
            ctrlAddEditSupplier1 = new Suppliers_Purchase.ctrlAddEditSupplier();
            ((System.ComponentModel.ISupportInitialize)autoComplete1).BeginInit();
            SuspendLayout();
            // 
            // autoComplete1
            // 
            autoComplete1.HeaderFont = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.World);
            autoComplete1.ItemFont = new Font("Segoe UI", 8.25F);
            autoComplete1.MetroColor = Color.FromArgb(17, 158, 218);
            autoComplete1.ParentForm = this;
            // 
            // ctrlAddEditSupplier1
            // 
            ctrlAddEditSupplier1.BackColor = Color.FromArgb(245, 247, 250);
            ctrlAddEditSupplier1.Location = new Point(191, 12);
            ctrlAddEditSupplier1.Name = "ctrlAddEditSupplier1";
            ctrlAddEditSupplier1.Size = new Size(560, 900);
            ctrlAddEditSupplier1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 958);
            Controls.Add(ctrlAddEditSupplier1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)autoComplete1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoComplete autoComplete1;
        private Suppliers_Purchase.ctrlAddEditSupplier ctrlAddEditSupplier1;
    }
}