namespace RMS_UI.Companies
{
    partial class frmFindCompany
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
            ctrlFindCompany1 = new ctrlFindCompany();
            _btnClose = new Button();
            btnAddCompany = new Button();
            SuspendLayout();
            // 
            // ctrlFindCompany1
            // 
            ctrlFindCompany1.BackColor = Color.FromArgb(245, 247, 250);
            ctrlFindCompany1.Dock = DockStyle.Top;
            ctrlFindCompany1.Location = new Point(0, 0);
            ctrlFindCompany1.Name = "ctrlFindCompany1";
            ctrlFindCompany1.Size = new Size(410, 484);
            ctrlFindCompany1.TabIndex = 0;
            // 
            // _btnClose
            // 
            _btnClose.BackColor = Color.Red;
            _btnClose.Cursor = Cursors.Hand;
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
            _btnClose.FlatStyle = FlatStyle.Flat;
            _btnClose.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnClose.ForeColor = Color.White;
            _btnClose.Location = new Point(310, 490);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(88, 30);
            _btnClose.TabIndex = 6;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = false;
            _btnClose.Click += _btnClose_Click;
            // 
            // btnAddCompany
            // 
            btnAddCompany.BackColor = Color.FromArgb(0, 192, 192);
            btnAddCompany.Cursor = Cursors.Hand;
            btnAddCompany.FlatAppearance.BorderSize = 0;
            btnAddCompany.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnAddCompany.FlatStyle = FlatStyle.Flat;
            btnAddCompany.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAddCompany.ForeColor = Color.White;
            btnAddCompany.Location = new Point(222, 12);
            btnAddCompany.Name = "btnAddCompany";
            btnAddCompany.Size = new Size(168, 30);
            btnAddCompany.TabIndex = 7;
            btnAddCompany.Text = "➕ Add New Company";
            btnAddCompany.UseVisualStyleBackColor = false;
            btnAddCompany.Click += btnAddCompany_Click;
            // 
            // frmFindCompany
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(410, 532);
            Controls.Add(btnAddCompany);
            Controls.Add(_btnClose);
            Controls.Add(ctrlFindCompany1);
            Font = new Font("Segoe UI", 9F);
            Name = "frmFindCompany";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🔍 Find Company";
            Load += frmFindCompany_Load;
            ResumeLayout(false);
        }

        #endregion

        private ctrlFindCompany ctrlFindCompany1;
        private Button _btnClose;
        private Button btnAddCompany;
    }
}