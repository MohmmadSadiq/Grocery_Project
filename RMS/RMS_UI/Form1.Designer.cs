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
            ctrlAddEditCompany1 = new Companies.ctrlAddEditCompany();
            SuspendLayout();
            // 
            // ctrlAddEditCompany1
            // 
            ctrlAddEditCompany1.BackColor = Color.FromArgb(245, 247, 250);
            ctrlAddEditCompany1.Location = new Point(179, 12);
            ctrlAddEditCompany1.Name = "ctrlAddEditCompany1";
            ctrlAddEditCompany1.Size = new Size(548, 800);
            ctrlAddEditCompany1.TabIndex = 0;
            ctrlAddEditCompany1.Load += ctrlAddEditCompany1_Load;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 822);
            Controls.Add(ctrlAddEditCompany1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Companies.ctrlAddEditCompany ctrlAddEditCompany1;
    }
}