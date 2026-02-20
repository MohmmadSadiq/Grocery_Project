namespace RMS_UI.Forms
{
    partial class frmCompanyCard
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
            ctrlCompanyCard1 = new Controls.ctrlCompanyCard();
            SuspendLayout();
            // 
            // ctrlCompanyCard1
            // 
            ctrlCompanyCard1.BackColor = Color.FromArgb(255, 255, 255);
            ctrlCompanyCard1.Location = new Point(-5, 19);
            ctrlCompanyCard1.Margin = new Padding(10);
            ctrlCompanyCard1.Name = "ctrlCompanyCard1";
            ctrlCompanyCard1.Size = new Size(400, 438);
            ctrlCompanyCard1.TabIndex = 0;
            // 
            // frmCompanyCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 450);
            Controls.Add(ctrlCompanyCard1);
            Name = "frmCompanyCard";
            Text = "frmCompanyCard";
            Load += frmCompanyCard_Load;
            ResumeLayout(false);
        }

        #endregion

        private Controls.ctrlCompanyCard ctrlCompanyCard1;
    }
}