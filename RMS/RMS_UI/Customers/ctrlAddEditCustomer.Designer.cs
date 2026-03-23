namespace RMS_UI.Controls
{
    partial class ctrlAddEditCustomer
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblMode;
        private RMS_UI.Controls.NotificationControl _notification;
        private System.Windows.Forms.Panel _pnlButtons;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Panel _pnlContent;
        private System.Windows.Forms.Panel _pnlCard;
        private System.Windows.Forms.Label _lblID;
        private System.Windows.Forms.Label _lblSectionPartner;
        private System.Windows.Forms.Panel _pnlSep1;
        private System.Windows.Forms.RadioButton _rbPerson;
        private System.Windows.Forms.RadioButton _rbCompany;
        private RMS_UI.Suppliers_Purchase.ctrlBusinessPartners _ctrlBusinessPartners;
        private System.Windows.Forms.Label _lblSectionStatus;
        private System.Windows.Forms.CheckBox _chkIsActive;
        private System.Windows.Forms.Label _lblSectionAccount;
        private System.Windows.Forms.Label _lblAccountID;
        private System.Windows.Forms.TextBox _txtAccountID;
        private System.Windows.Forms.Button _btnFindAccount;
        private System.Windows.Forms.ErrorProvider _errorProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _pnlHeader = new System.Windows.Forms.Panel();
            _lblTitle = new System.Windows.Forms.Label();
            _lblMode = new System.Windows.Forms.Label();
            _notification = new RMS_UI.Controls.NotificationControl();
            _pnlButtons = new System.Windows.Forms.Panel();
            _btnSave = new System.Windows.Forms.Button();
            _btnCancel = new System.Windows.Forms.Button();
            _pnlContent = new System.Windows.Forms.Panel();
            _pnlCard = new System.Windows.Forms.Panel();
            _lblID = new System.Windows.Forms.Label();
            _lblSectionPartner = new System.Windows.Forms.Label();
            _pnlSep1 = new System.Windows.Forms.Panel();
            _rbPerson = new System.Windows.Forms.RadioButton();
            _rbCompany = new System.Windows.Forms.RadioButton();
            _ctrlBusinessPartners = new RMS_UI.Suppliers_Purchase.ctrlBusinessPartners();
            _lblSectionStatus = new System.Windows.Forms.Label();
            _chkIsActive = new System.Windows.Forms.CheckBox();
            _lblSectionAccount = new System.Windows.Forms.Label();
            _lblAccountID = new System.Windows.Forms.Label();
            _txtAccountID = new System.Windows.Forms.TextBox();
            _btnFindAccount = new System.Windows.Forms.Button();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);
            _pnlHeader.SuspendLayout();
            _pnlButtons.SuspendLayout();
            _pnlContent.SuspendLayout();
            _pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.BackColor = System.Drawing.Color.White;
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlHeader.Location = new System.Drawing.Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            _pnlHeader.Size = new System.Drawing.Size(679, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            _lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new System.Drawing.Point(24, 12);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(252, 30);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "👥  Add New Customer";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblMode.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblMode.Location = new System.Drawing.Point(26, 44);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new System.Drawing.Size(266, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Fill in the details below to register a new customer.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = System.Windows.Forms.DockStyle.Top;
            _notification.Location = new System.Drawing.Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new System.Drawing.Size(679, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _pnlButtons
            // 
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            _pnlButtons.Location = new System.Drawing.Point(0, 787);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new System.Windows.Forms.Padding(0, 14, 24, 14);
            _pnlButtons.Size = new System.Drawing.Size(679, 65);
            _pnlButtons.TabIndex = 3;
            // 
            // _btnSave
            // 
            _btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnSave.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnSave.ForeColor = System.Drawing.Color.White;
            _btnSave.Location = new System.Drawing.Point(533, 14);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new System.Drawing.Size(122, 38);
            _btnSave.TabIndex = 1;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = false;
            _btnSave.Click += _btnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            _btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            _btnCancel.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _btnCancel.Location = new System.Drawing.Point(403, 14);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(120, 38);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = false;
            _btnCancel.Click += _btnCancel_Click;
            // 
            // _pnlContent
            // 
            _pnlContent.AutoScroll = true;
            _pnlContent.Controls.Add(_pnlCard);
            _pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlContent.Location = new System.Drawing.Point(0, 72);
            _pnlContent.Name = "_pnlContent";
            _pnlContent.Padding = new System.Windows.Forms.Padding(20);
            _pnlContent.Size = new System.Drawing.Size(679, 715);
            _pnlContent.TabIndex = 2;
            // 
            // _pnlCard
            // 
            _pnlCard.BackColor = System.Drawing.Color.White;
            _pnlCard.Controls.Add(_lblID);
            _pnlCard.Controls.Add(_lblSectionPartner);
            _pnlCard.Controls.Add(_pnlSep1);
            _pnlCard.Controls.Add(_rbPerson);
            _pnlCard.Controls.Add(_rbCompany);
            _pnlCard.Controls.Add(_ctrlBusinessPartners);
            _pnlCard.Controls.Add(_lblSectionStatus);
            _pnlCard.Controls.Add(_chkIsActive);
            _pnlCard.Controls.Add(_lblSectionAccount);
            _pnlCard.Controls.Add(_lblAccountID);
            _pnlCard.Controls.Add(_txtAccountID);
            _pnlCard.Controls.Add(_btnFindAccount);
            _pnlCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlCard.Location = new System.Drawing.Point(20, 20);
            _pnlCard.Name = "_pnlCard";
            _pnlCard.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            _pnlCard.Size = new System.Drawing.Size(639, 675);
            _pnlCard.TabIndex = 0;
            // 
            // _lblID
            // 
            _lblID.AutoSize = true;
            _lblID.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Italic);
            _lblID.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblID.Location = new System.Drawing.Point(6, 2);
            _lblID.Name = "_lblID";
            _lblID.Size = new System.Drawing.Size(57, 20);
            _lblID.TabIndex = 0;
            _lblID.Text = "ID: N/A";
            // 
            // _lblSectionPartner
            // 
            _lblSectionPartner.AutoSize = true;
            _lblSectionPartner.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _lblSectionPartner.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _lblSectionPartner.Location = new System.Drawing.Point(24, 29);
            _lblSectionPartner.Name = "_lblSectionPartner";
            _lblSectionPartner.Size = new System.Drawing.Size(146, 19);
            _lblSectionPartner.TabIndex = 1;
            _lblSectionPartner.Text = "🤝  Business Partner";
            // 
            // _pnlSep1
            // 
            _pnlSep1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _pnlSep1.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new System.Drawing.Point(24, 53);
            _pnlSep1.Name = "_pnlSep1";
            _pnlSep1.Size = new System.Drawing.Size(591, 1);
            _pnlSep1.TabIndex = 2;
            // 
            // _rbPerson
            // 
            _rbPerson.AutoSize = true;
            _rbPerson.Font = new System.Drawing.Font("Segoe UI", 10F);
            _rbPerson.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            _rbPerson.Location = new System.Drawing.Point(24, 65);
            _rbPerson.Name = "_rbPerson";
            _rbPerson.Size = new System.Drawing.Size(95, 23);
            _rbPerson.TabIndex = 3;
            _rbPerson.Text = "👤  Person";
            _rbPerson.UseVisualStyleBackColor = true;
            _rbPerson.CheckedChanged += _rbPerson_CheckedChanged;
            // 
            // _rbCompany
            // 
            _rbCompany.AutoSize = true;
            _rbCompany.Font = new System.Drawing.Font("Segoe UI", 10F);
            _rbCompany.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            _rbCompany.Location = new System.Drawing.Point(160, 65);
            _rbCompany.Name = "_rbCompany";
            _rbCompany.Size = new System.Drawing.Size(113, 23);
            _rbCompany.TabIndex = 4;
            _rbCompany.Text = "🏢  Company";
            _rbCompany.UseVisualStyleBackColor = true;
            _rbCompany.CheckedChanged += _rbCompany_CheckedChanged;
            // 
            // _ctrlBusinessPartners
            // 
            _ctrlBusinessPartners.AllowTabToggling = false;
            _ctrlBusinessPartners.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _ctrlBusinessPartners.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            _ctrlBusinessPartners.Location = new System.Drawing.Point(24, 98);
            _ctrlBusinessPartners.Name = "_ctrlBusinessPartners";
            _ctrlBusinessPartners.Size = new System.Drawing.Size(452, 563);
            _ctrlBusinessPartners.TabIndex = 5;
            // 
            // _lblSectionStatus
            // 
            _lblSectionStatus.AutoSize = true;
            _lblSectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _lblSectionStatus.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _lblSectionStatus.Location = new System.Drawing.Point(279, 29);
            _lblSectionStatus.Name = "_lblSectionStatus";
            _lblSectionStatus.Size = new System.Drawing.Size(143, 19);
            _lblSectionStatus.TabIndex = 7;
            _lblSectionStatus.Text = "✅  Customer Status";
            // 
            // _chkIsActive
            // 
            _chkIsActive.AutoSize = true;
            _chkIsActive.Checked = true;
            _chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkIsActive.Font = new System.Drawing.Font("Segoe UI", 10F);
            _chkIsActive.Location = new System.Drawing.Point(279, 60);
            _chkIsActive.Name = "_chkIsActive";
            _chkIsActive.Size = new System.Drawing.Size(122, 23);
            _chkIsActive.TabIndex = 8;
            _chkIsActive.Text = "Active Customer";
            _chkIsActive.UseVisualStyleBackColor = true;
            // 
            // _lblSectionAccount
            // 
            _lblSectionAccount.AutoSize = true;
            _lblSectionAccount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _lblSectionAccount.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _lblSectionAccount.Location = new System.Drawing.Point(501, 29);
            _lblSectionAccount.Name = "_lblSectionAccount";
            _lblSectionAccount.Size = new System.Drawing.Size(91, 19);
            _lblSectionAccount.TabIndex = 9;
            _lblSectionAccount.Text = "💳  Account";
            // 
            // _lblAccountID
            // 
            _lblAccountID.AutoSize = true;
            _lblAccountID.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblAccountID.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblAccountID.Location = new System.Drawing.Point(501, 68);
            _lblAccountID.Name = "_lblAccountID";
            _lblAccountID.Size = new System.Drawing.Size(62, 15);
            _lblAccountID.TabIndex = 10;
            _lblAccountID.Text = "AccountID";
            // 
            // _txtAccountID
            // 
            _txtAccountID.Font = new System.Drawing.Font("Segoe UI", 10F);
            _txtAccountID.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            _txtAccountID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _txtAccountID.Location = new System.Drawing.Point(501, 98);
            _txtAccountID.Name = "_txtAccountID";
            _txtAccountID.PlaceholderText = "ID";
            _txtAccountID.Size = new System.Drawing.Size(100, 25);
            _txtAccountID.TabIndex = 11;
            // 
            // _btnFindAccount
            // 
            _btnFindAccount.BackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            _btnFindAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnFindAccount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(191, 219, 254);
            _btnFindAccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(191, 219, 254);
            _btnFindAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnFindAccount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            _btnFindAccount.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            _btnFindAccount.Location = new System.Drawing.Point(501, 138);
            _btnFindAccount.Name = "_btnFindAccount";
            _btnFindAccount.Size = new System.Drawing.Size(70, 31);
            _btnFindAccount.TabIndex = 12;
            _btnFindAccount.Text = "Find";
            _btnFindAccount.UseVisualStyleBackColor = false;
            _btnFindAccount.Click += _btnFindAccount_Click;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditCustomer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(_pnlContent);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Controls.Add(_pnlButtons);
            Name = "ctrlAddEditCustomer";
            Size = new System.Drawing.Size(679, 852);
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlButtons.ResumeLayout(false);
            _pnlContent.ResumeLayout(false);
            _pnlCard.ResumeLayout(false);
            _pnlCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }
    }
}
