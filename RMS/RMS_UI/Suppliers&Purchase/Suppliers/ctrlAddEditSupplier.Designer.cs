namespace RMS_UI.Suppliers_Purchase
{
    partial class ctrlAddEditSupplier
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        // ─── Header ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel          _pnlHeader;
        private System.Windows.Forms.Label          _lblTitle;
        private System.Windows.Forms.Label          _lblMode;

        // ─── Notification ───────────────────────────────────────────────────────
        private RMS_UI.Controls.NotificationControl _notification;

        // ─── Buttons Bar ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel          _pnlButtons;
        private System.Windows.Forms.Button         _btnSave;
        private System.Windows.Forms.Button         _btnCancel;

        // ─── Content / Card ─────────────────────────────────────────────────────
        private System.Windows.Forms.Panel          _pnlContent;
        private System.Windows.Forms.Panel          _pnlCard;
        private System.Windows.Forms.Label          _lblID;

        // Section: Business Partner
        private System.Windows.Forms.Label          _lblSectionPartner;
        private System.Windows.Forms.Panel          _pnlSep1;
        private System.Windows.Forms.RadioButton    _rbPerson;
        private System.Windows.Forms.RadioButton    _rbCompany;
        private ctrlBusinessPartners                _ctrlBusinessPartners;

        // Section: Supplier Status
        private System.Windows.Forms.Label          _lblSectionStatus;
        private System.Windows.Forms.CheckBox       _chkIsActive;

        // Section: Account
        private System.Windows.Forms.Label          _lblSectionAccount;
        private System.Windows.Forms.Label          _lblAccount;
        private System.Windows.Forms.TextBox        _txtAccountID;
        private System.Windows.Forms.Button         _btnFindAccount;

        // Validation
        private System.Windows.Forms.ErrorProvider  _errorProvider;
        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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
            components = new System.ComponentModel.Container();
            _pnlHeader = new Panel();
            _lblTitle = new Label();
            _lblMode = new Label();
            _notification = new Controls.NotificationControl();
            _pnlButtons = new Panel();
            _btnSave = new Button();
            _btnCancel = new Button();
            _pnlContent = new Panel();
            _pnlCard = new Panel();
            _lblID = new Label();
            _lblSectionPartner = new Label();
            _pnlSep1 = new Panel();
            _rbPerson = new RadioButton();
            _rbCompany = new RadioButton();
            _ctrlBusinessPartners = new ctrlBusinessPartners();
            _lblSectionStatus = new Label();
            _chkIsActive = new CheckBox();
            _lblSectionAccount = new Label();
            _lblAccount = new Label();
            _txtAccountID = new TextBox();
            _btnFindAccount = new Button();
            _errorProvider = new ErrorProvider(components);
            _pnlHeader.SuspendLayout();
            _pnlButtons.SuspendLayout();
            _pnlContent.SuspendLayout();
            _pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.BackColor = Color.White;
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Location = new Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new Padding(24, 0, 24, 0);
            _pnlHeader.Size = new Size(679, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(24, 12);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(244, 30);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "📦  Add New Supplier";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new Font("Segoe UI", 9F);
            _lblMode.ForeColor = Color.FromArgb(100, 116, 139);
            _lblMode.Location = new Point(26, 44);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new Size(265, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Fill in the details below to register a new supplier.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = DockStyle.Top;
            _notification.Location = new Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new Size(679, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _pnlButtons
            // 
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = DockStyle.Bottom;
            _pnlButtons.Location = new Point(0, 787);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new Padding(0, 14, 24, 14);
            _pnlButtons.Size = new Size(679, 65);
            _pnlButtons.TabIndex = 3;
            // 
            // _btnSave
            // 
            _btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnSave.BackColor = Color.FromArgb(59, 130, 246);
            _btnSave.Cursor = Cursors.Hand;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnSave.FlatStyle = FlatStyle.Flat;
            _btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSave.ForeColor = Color.White;
            _btnSave.Location = new Point(533, 14);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(122, 38);
            _btnSave.TabIndex = 1;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = false;
            _btnSave.Click += _btnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnCancel.Cursor = Cursors.Hand;
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnCancel.FlatStyle = FlatStyle.Flat;
            _btnCancel.Font = new Font("Segoe UI", 10F);
            _btnCancel.ForeColor = Color.FromArgb(100, 116, 139);
            _btnCancel.Location = new Point(403, 14);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(120, 38);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = false;
            _btnCancel.Click += _btnCancel_Click;
            // 
            // _pnlContent
            // 
            _pnlContent.AutoScroll = true;
            _pnlContent.Controls.Add(_pnlCard);
            _pnlContent.Dock = DockStyle.Fill;
            _pnlContent.Location = new Point(0, 72);
            _pnlContent.Name = "_pnlContent";
            _pnlContent.Padding = new Padding(20);
            _pnlContent.Size = new Size(679, 715);
            _pnlContent.TabIndex = 2;
            // 
            // _pnlCard
            // 
            _pnlCard.BackColor = Color.White;
            _pnlCard.Controls.Add(_lblID);
            _pnlCard.Controls.Add(_lblSectionPartner);
            _pnlCard.Controls.Add(_pnlSep1);
            _pnlCard.Controls.Add(_rbPerson);
            _pnlCard.Controls.Add(_rbCompany);
            _pnlCard.Controls.Add(_ctrlBusinessPartners);
            _pnlCard.Controls.Add(_lblSectionStatus);
            _pnlCard.Controls.Add(_chkIsActive);
            _pnlCard.Controls.Add(_lblSectionAccount);
            _pnlCard.Controls.Add(_lblAccount);
            _pnlCard.Controls.Add(_txtAccountID);
            _pnlCard.Controls.Add(_btnFindAccount);
            _pnlCard.Dock = DockStyle.Fill;
            _pnlCard.Location = new Point(20, 20);
            _pnlCard.Name = "_pnlCard";
            _pnlCard.Padding = new Padding(24, 20, 24, 24);
            _pnlCard.Size = new Size(639, 675);
            _pnlCard.TabIndex = 0;
            _pnlCard.Paint += _pnlCard_Paint;
            // 
            // _lblID
            // 
            _lblID.AutoSize = true;
            _lblID.BackColor = Color.Transparent;
            _lblID.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            _lblID.ForeColor = Color.FromArgb(100, 116, 139);
            _lblID.Location = new Point(6, 2);
            _lblID.Name = "_lblID";
            _lblID.Size = new Size(57, 20);
            _lblID.TabIndex = 0;
            _lblID.Text = "ID: N/A";
            // 
            // _lblSectionPartner
            // 
            _lblSectionPartner.AutoSize = true;
            _lblSectionPartner.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionPartner.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionPartner.Location = new Point(24, 29);
            _lblSectionPartner.Name = "_lblSectionPartner";
            _lblSectionPartner.Size = new Size(146, 19);
            _lblSectionPartner.TabIndex = 1;
            _lblSectionPartner.Text = "\U0001f91d  Business Partner";
            // 
            // _pnlSep1
            // 
            _pnlSep1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSep1.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new Point(24, 53);
            _pnlSep1.Name = "_pnlSep1";
            _pnlSep1.Size = new Size(591, 1);
            _pnlSep1.TabIndex = 2;
            // 
            // _rbPerson
            // 
            _rbPerson.AutoSize = true;
            _rbPerson.Font = new Font("Segoe UI", 10F);
            _rbPerson.ForeColor = Color.FromArgb(51, 65, 85);
            _rbPerson.Location = new Point(24, 65);
            _rbPerson.Name = "_rbPerson";
            _rbPerson.Size = new Size(95, 23);
            _rbPerson.TabIndex = 3;
            _rbPerson.Text = "👤  Person";
            _rbPerson.UseVisualStyleBackColor = true;
            _rbPerson.CheckedChanged += _rbPerson_CheckedChanged;
            // 
            // _rbCompany
            // 
            _rbCompany.AutoSize = true;
            _rbCompany.Font = new Font("Segoe UI", 10F);
            _rbCompany.ForeColor = Color.FromArgb(51, 65, 85);
            _rbCompany.Location = new Point(160, 65);
            _rbCompany.Name = "_rbCompany";
            _rbCompany.Size = new Size(113, 23);
            _rbCompany.TabIndex = 4;
            _rbCompany.Text = "🏢  Company";
            _rbCompany.UseVisualStyleBackColor = true;
            _rbCompany.CheckedChanged += _rbCompany_CheckedChanged;
            // 
            // _ctrlBusinessPartners
            // 
            _ctrlBusinessPartners.AllowTabToggling = false;
            _ctrlBusinessPartners.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _ctrlBusinessPartners.BackColor = Color.FromArgb(245, 247, 250);
            _ctrlBusinessPartners.Location = new Point(24, 98);
            _ctrlBusinessPartners.Name = "_ctrlBusinessPartners";
            _ctrlBusinessPartners.Size = new Size(452, 563);
            _ctrlBusinessPartners.TabIndex = 5;
            // 
            // _lblSectionStatus
            // 
            _lblSectionStatus.AutoSize = true;
            _lblSectionStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionStatus.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionStatus.Location = new Point(279, 29);
            _lblSectionStatus.Name = "_lblSectionStatus";
            _lblSectionStatus.Size = new Size(137, 19);
            _lblSectionStatus.TabIndex = 6;
            _lblSectionStatus.Text = "✅  Supplier Status";
            // 
            // _chkIsActive
            // 
            _chkIsActive.AutoSize = true;
            _chkIsActive.Checked = true;
            _chkIsActive.CheckState = CheckState.Checked;
            _chkIsActive.Font = new Font("Segoe UI", 10F);
            _chkIsActive.ForeColor = Color.FromArgb(51, 65, 85);
            _chkIsActive.Location = new Point(279, 60);
            _chkIsActive.Name = "_chkIsActive";
            _chkIsActive.Size = new Size(118, 23);
            _chkIsActive.TabIndex = 8;
            _chkIsActive.Text = "Active Supplier";
            _chkIsActive.UseVisualStyleBackColor = true;
            // 
            // _lblSectionAccount
            // 
            _lblSectionAccount.AutoSize = true;
            _lblSectionAccount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionAccount.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionAccount.Location = new Point(501, 29);
            _lblSectionAccount.Name = "_lblSectionAccount";
            _lblSectionAccount.Size = new Size(91, 19);
            _lblSectionAccount.TabIndex = 9;
            _lblSectionAccount.Text = "💳  Account";
            // 
            // _lblAccount
            // 
            _lblAccount.AutoSize = true;
            _lblAccount.Font = new Font("Segoe UI", 9F);
            _lblAccount.ForeColor = Color.FromArgb(100, 116, 139);
            _lblAccount.Location = new Point(501, 68);
            _lblAccount.Name = "_lblAccount";
            _lblAccount.Size = new Size(66, 15);
            _lblAccount.TabIndex = 11;
            _lblAccount.Text = "Account ID";
            // 
            // _txtAccountID
            // 
            _txtAccountID.BackColor = Color.FromArgb(245, 247, 250);
            _txtAccountID.BorderStyle = BorderStyle.FixedSingle;
            _txtAccountID.Enabled = false;
            _txtAccountID.Font = new Font("Segoe UI", 10F);
            _txtAccountID.Location = new Point(501, 98);
            _txtAccountID.Name = "_txtAccountID";
            _txtAccountID.PlaceholderText = "ID";
            _txtAccountID.ReadOnly = true;
            _txtAccountID.Size = new Size(100, 25);
            _txtAccountID.TabIndex = 12;
            _txtAccountID.TabStop = false;
            // 
            // _btnFindAccount
            // 
            _btnFindAccount.BackColor = Color.FromArgb(219, 234, 254);
            _btnFindAccount.Cursor = Cursors.Hand;
            _btnFindAccount.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
            _btnFindAccount.FlatAppearance.MouseOverBackColor = Color.FromArgb(191, 219, 254);
            _btnFindAccount.FlatStyle = FlatStyle.Flat;
            _btnFindAccount.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnFindAccount.ForeColor = Color.FromArgb(37, 99, 235);
            _btnFindAccount.Location = new Point(501, 138);
            _btnFindAccount.Name = "_btnFindAccount";
            _btnFindAccount.Size = new Size(70, 31);
            _btnFindAccount.TabIndex = 13;
            _btnFindAccount.Text = "Find";
            _btnFindAccount.UseVisualStyleBackColor = false;
            _btnFindAccount.Click += _btnFindAccount_Click;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditSupplier
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlContent);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Controls.Add(_pnlButtons);
            Name = "ctrlAddEditSupplier";
            Size = new Size(679, 852);
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlButtons.ResumeLayout(false);
            _pnlContent.ResumeLayout(false);
            _pnlCard.ResumeLayout(false);
            _pnlCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
