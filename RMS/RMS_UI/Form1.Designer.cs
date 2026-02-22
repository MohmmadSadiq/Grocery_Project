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
            ctrlPersonCardWithConfig1 = new Peoples.ctrlPersonCardWithConfig();
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
            // ctrlPersonCardWithConfig1
            // 
            ctrlPersonCardWithConfig1.BackColor = Color.Transparent;
            ctrlPersonCardWithConfig1.Location = new Point(197, 12);
            ctrlPersonCardWithConfig1.Name = "ctrlPersonCardWithConfig1";
            ctrlPersonCardWithConfig1.Size = new Size(467, 713);
            ctrlPersonCardWithConfig1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 822);
            Controls.Add(ctrlPersonCardWithConfig1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)autoComplete1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.AutoComplete autoComplete1;
        private Peoples.ctrlPersonCardWithConfig ctrlPersonCardWithConfig1;
    }
}