namespace RMS_UI.Companies
{
    partial class ctrlFindCompany
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel              _pnlTop;
        private System.Windows.Forms.Label              _lblTitle;
        private System.Windows.Forms.Label              _lblSearchBy;
        private System.Windows.Forms.ComboBox           _cmbSearchBy;
        private System.Windows.Forms.Label              _lblSearchValue;
        private System.Windows.Forms.TextBox            _txtSearchValue;
        private System.Windows.Forms.Button             _btnSearch;
        private RMS_UI.Controls.NotificationControl     _notification;
        private RMS_UI.Controls.ctrlCompanyCard         _ctrlCompanyCard;
        #endregion

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _pnlTop = new Panel();
            _btnSearch = new Button();
            _txtSearchValue = new TextBox();
            _lblSearchValue = new Label();
            _cmbSearchBy = new ComboBox();
            _lblSearchBy = new Label();
            _lblTitle = new Label();
            _notification = new Controls.NotificationControl();
            _ctrlCompanyCard = new Controls.ctrlCompanyCard();
            _pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlTop
            // 
            _pnlTop.Controls.Add(_btnSearch);
            _pnlTop.Controls.Add(_txtSearchValue);
            _pnlTop.Controls.Add(_lblSearchValue);
            _pnlTop.Controls.Add(_cmbSearchBy);
            _pnlTop.Controls.Add(_lblSearchBy);
            _pnlTop.Controls.Add(_lblTitle);
            _pnlTop.Dock = DockStyle.Top;
            _pnlTop.Location = new Point(0, 0);
            _pnlTop.Name = "_pnlTop";
            _pnlTop.Padding = new Padding(20, 18, 20, 16);
            _pnlTop.Size = new Size(394, 128);
            _pnlTop.TabIndex = 0;
            _pnlTop.Paint += _pnlTop_Paint;
            // 
            // _btnSearch
            // 
            _btnSearch.BackColor = Color.FromArgb(59, 130, 246);
            _btnSearch.Cursor = Cursors.Hand;
            _btnSearch.FlatAppearance.BorderSize = 0;
            _btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnSearch.FlatStyle = FlatStyle.Flat;
            _btnSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnSearch.ForeColor = Color.White;
            _btnSearch.Location = new Point(298, 88);
            _btnSearch.Name = "_btnSearch";
            _btnSearch.Size = new Size(88, 30);
            _btnSearch.TabIndex = 5;
            _btnSearch.Text = "Search";
            _btnSearch.UseVisualStyleBackColor = false;
            _btnSearch.Click += _btnSearch_Click;
            // 
            // _txtSearchValue
            // 
            _txtSearchValue.BorderStyle = BorderStyle.FixedSingle;
            _txtSearchValue.Font = new Font("Segoe UI", 9.5F);
            _txtSearchValue.Location = new Point(102, 94);
            _txtSearchValue.Name = "_txtSearchValue";
            _txtSearchValue.Size = new Size(190, 24);
            _txtSearchValue.TabIndex = 4;
            // 
            // _lblSearchValue
            // 
            _lblSearchValue.AutoSize = true;
            _lblSearchValue.Font = new Font("Segoe UI", 9F);
            _lblSearchValue.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSearchValue.Location = new Point(20, 98);
            _lblSearchValue.Name = "_lblSearchValue";
            _lblSearchValue.Size = new Size(38, 15);
            _lblSearchValue.TabIndex = 3;
            _lblSearchValue.Text = "Value:";
            // 
            // _cmbSearchBy
            // 
            _cmbSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbSearchBy.FlatStyle = FlatStyle.Flat;
            _cmbSearchBy.Font = new Font("Segoe UI", 9.5F);
            _cmbSearchBy.Location = new Point(102, 60);
            _cmbSearchBy.Name = "_cmbSearchBy";
            _cmbSearchBy.Size = new Size(148, 25);
            _cmbSearchBy.TabIndex = 2;
            // 
            // _lblSearchBy
            // 
            _lblSearchBy.AutoSize = true;
            _lblSearchBy.Font = new Font("Segoe UI", 9F);
            _lblSearchBy.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSearchBy.Location = new Point(20, 64);
            _lblSearchBy.Name = "_lblSearchBy";
            _lblSearchBy.Size = new Size(61, 15);
            _lblSearchBy.TabIndex = 1;
            _lblSearchBy.Text = "Search by:";
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(20, 18);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(173, 25);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "🔍  Find Company";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = DockStyle.Top;
            _notification.Location = new Point(0, 128);
            _notification.Name = "_notification";
            _notification.Size = new Size(394, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _ctrlCompanyCard
            // 
            _ctrlCompanyCard.BackColor = Color.FromArgb(255, 255, 255);
            _ctrlCompanyCard.Dock = DockStyle.Fill;
            _ctrlCompanyCard.Location = new Point(0, 128);
            _ctrlCompanyCard.Margin = new Padding(0);
            _ctrlCompanyCard.Name = "_ctrlCompanyCard";
            _ctrlCompanyCard.Size = new Size(394, 348);
            _ctrlCompanyCard.TabIndex = 2;
            // 
            // ctrlFindCompany
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_ctrlCompanyCard);
            Controls.Add(_notification);
            Controls.Add(_pnlTop);
            Name = "ctrlFindCompany";
            Size = new Size(394, 476);
            _pnlTop.ResumeLayout(false);
            _pnlTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
