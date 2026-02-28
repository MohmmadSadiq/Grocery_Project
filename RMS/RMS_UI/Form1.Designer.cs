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
            ctrlProductFinder1 = new Products.ctrlProductFinder();
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
            // ctrlProductFinder1
            // 
            ctrlProductFinder1.Location = new Point(237, 63);
            ctrlProductFinder1.Name = "ctrlProductFinder1";
            ctrlProductFinder1.Size = new Size(400, 170);
            ctrlProductFinder1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 958);
            Controls.Add(ctrlProductFinder1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)autoComplete1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoComplete autoComplete1;
        private Products.ctrlProductFinder ctrlProductFinder1;
    }
}