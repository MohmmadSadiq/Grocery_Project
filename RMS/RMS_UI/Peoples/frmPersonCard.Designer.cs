namespace RMS_UI.Forms
{
    partial class frmPersonCard
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
            _btnClose = new Button();
            ctrlPersonCard1 = new Controls.ctrlPersonCard();
            SuspendLayout();
            // 
            // _btnClose
            // 
            _btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _btnClose.BackColor = Color.FromArgb(239, 68, 68);
            _btnClose.Cursor = Cursors.Hand;
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 38, 38);
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 62, 62);
            _btnClose.FlatStyle = FlatStyle.Flat;
            _btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnClose.ForeColor = Color.White;
            _btnClose.Location = new Point(387, 729);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(87, 35);
            _btnClose.TabIndex = 1;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = false;
            _btnClose.Click += _btnClose_Click;
            // 
            // ctrlPersonCard1
            // 
            ctrlPersonCard1.BackColor = Color.FromArgb(255, 255, 255);
            ctrlPersonCard1.Location = new Point(4, 6);
            ctrlPersonCard1.Margin = new Padding(12);
            ctrlPersonCard1.Name = "ctrlPersonCard1";
            ctrlPersonCard1.Size = new Size(467, 713);
            ctrlPersonCard1.TabIndex = 2;
            // 
            // frmPersonCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(486, 776);
            Controls.Add(ctrlPersonCard1);
            Controls.Add(_btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPersonCard";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Person Details";
            Load += frmPersonCard_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button _btnClose;
        private Controls.ctrlPersonCard ctrlPersonCard1;
    }
}