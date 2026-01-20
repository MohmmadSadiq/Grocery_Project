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
            reusableDataGrid1 = new Controls.ReusableDataGrid();
            SuspendLayout();
            // 
            // reusableDataGrid1
            // 
            reusableDataGrid1.BackColor = Color.FromArgb(255, 255, 255);
            reusableDataGrid1.Location = new Point(12, 12);
            reusableDataGrid1.Name = "reusableDataGrid1";
            reusableDataGrid1.Size = new Size(900, 600);
            reusableDataGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 629);
            Controls.Add(reusableDataGrid1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Controls.ReusableDataGrid reusableDataGrid1;
    }
}