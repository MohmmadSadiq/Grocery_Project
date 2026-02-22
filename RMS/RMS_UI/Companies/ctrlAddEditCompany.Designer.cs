namespace RMS_UI.Companies
{
    partial class ctrlAddEditCompany
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

        // Section: Basic Information
        private System.Windows.Forms.Label          _lblSectionBasic;
        private System.Windows.Forms.Panel          _pnlSep1;
        private System.Windows.Forms.Label          _lblCompanyName;
        private System.Windows.Forms.TextBox        _txtCompanyName;
        private System.Windows.Forms.Label          _lblCommercialNumber;
        private System.Windows.Forms.TextBox        _txtCommercialNumber;

        // Section: Contact Details
        private System.Windows.Forms.Panel          _pnlSep2;
        private System.Windows.Forms.Label          _lblSectionContact;
        private System.Windows.Forms.Label          _lblPhone;
        private System.Windows.Forms.TextBox        _txtPhone;
        private System.Windows.Forms.Label          _lblEmail;
        private System.Windows.Forms.TextBox        _txtEmail;
        private System.Windows.Forms.Label          _lblContactPerson;
        private System.Windows.Forms.TextBox        _txtContactPersonID;
        private System.Windows.Forms.TextBox        _txtContactPersonName;
        private System.Windows.Forms.Button         _btnFindPerson;

        // Section: Location
        private System.Windows.Forms.Panel          _pnlSep3;
        private System.Windows.Forms.Label          _lblSectionLocation;
        private System.Windows.Forms.Label          _lblCountry;
        private System.Windows.Forms.ComboBox       _cmbCountry;
        private System.Windows.Forms.Label          _lblAddress;
        private System.Windows.Forms.TextBox        _txtAddress;

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
            button1 = new Button();
            _lblSectionBasic = new Label();
            _pnlSep1 = new Panel();
            _lblCompanyName = new Label();
            _txtCompanyName = new TextBox();
            _lblCommercialNumber = new Label();
            _txtCommercialNumber = new TextBox();
            _pnlSep2 = new Panel();
            _lblSectionContact = new Label();
            _lblPhone = new Label();
            _txtPhone = new TextBox();
            _lblEmail = new Label();
            _txtEmail = new TextBox();
            _lblContactPerson = new Label();
            _txtContactPersonID = new TextBox();
            _txtContactPersonName = new TextBox();
            _btnFindPerson = new Button();
            _pnlSep3 = new Panel();
            _lblSectionLocation = new Label();
            _lblCountry = new Label();
            _cmbCountry = new ComboBox();
            _lblAddress = new Label();
            _txtAddress = new TextBox();
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
            _pnlHeader.Size = new Size(548, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(24, 12);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(256, 30);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "🏢  Add New Company";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new Font("Segoe UI", 9F);
            _lblMode.ForeColor = Color.FromArgb(100, 116, 139);
            _lblMode.Location = new Point(26, 44);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new Size(273, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Fill in the details below to register a new company.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = DockStyle.Top;
            _notification.Location = new Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new Size(548, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _pnlButtons
            // 
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = DockStyle.Bottom;
            _pnlButtons.Location = new Point(0, 735);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new Padding(0, 14, 24, 14);
            _pnlButtons.Size = new Size(548, 65);
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
            _btnSave.Location = new Point(402, 14);
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
            _btnCancel.Location = new Point(272, 14);
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
            _pnlContent.Size = new Size(548, 663);
            _pnlContent.TabIndex = 2;
            // 
            // _pnlCard
            // 
            _pnlCard.BackColor = Color.White;
            _pnlCard.Controls.Add(_lblID);
            _pnlCard.Controls.Add(button1);
            _pnlCard.Controls.Add(_lblSectionBasic);
            _pnlCard.Controls.Add(_pnlSep1);
            _pnlCard.Controls.Add(_lblCompanyName);
            _pnlCard.Controls.Add(_txtCompanyName);
            _pnlCard.Controls.Add(_lblCommercialNumber);
            _pnlCard.Controls.Add(_txtCommercialNumber);
            _pnlCard.Controls.Add(_pnlSep2);
            _pnlCard.Controls.Add(_lblSectionContact);
            _pnlCard.Controls.Add(_lblPhone);
            _pnlCard.Controls.Add(_txtPhone);
            _pnlCard.Controls.Add(_lblEmail);
            _pnlCard.Controls.Add(_txtEmail);
            _pnlCard.Controls.Add(_lblContactPerson);
            _pnlCard.Controls.Add(_txtContactPersonID);
            _pnlCard.Controls.Add(_txtContactPersonName);
            _pnlCard.Controls.Add(_btnFindPerson);
            _pnlCard.Controls.Add(_pnlSep3);
            _pnlCard.Controls.Add(_lblSectionLocation);
            _pnlCard.Controls.Add(_lblCountry);
            _pnlCard.Controls.Add(_cmbCountry);
            _pnlCard.Controls.Add(_lblAddress);
            _pnlCard.Controls.Add(_txtAddress);
            _pnlCard.Dock = DockStyle.Fill;
            _pnlCard.Location = new Point(20, 20);
            _pnlCard.Name = "_pnlCard";
            _pnlCard.Padding = new Padding(24, 20, 24, 24);
            _pnlCard.Size = new Size(508, 623);
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
            _lblID.TabIndex = 22;
            _lblID.Text = "ID: N/A";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(59, 130, 246);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(430, 360);
            button1.Name = "button1";
            button1.Size = new Size(54, 31);
            button1.TabIndex = 2;
            button1.Text = "Clear";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // _lblSectionBasic
            // 
            _lblSectionBasic.AutoSize = true;
            _lblSectionBasic.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionBasic.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionBasic.Location = new Point(24, 29);
            _lblSectionBasic.Name = "_lblSectionBasic";
            _lblSectionBasic.Size = new Size(154, 19);
            _lblSectionBasic.TabIndex = 0;
            _lblSectionBasic.Text = "📋  Basic Information";
            // 
            // _pnlSep1
            // 
            _pnlSep1.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new Point(24, 53);
            _pnlSep1.Name = "_pnlSep1";
            _pnlSep1.Size = new Size(608, 1);
            _pnlSep1.TabIndex = 1;
            // 
            // _lblCompanyName
            // 
            _lblCompanyName.AutoSize = true;
            _lblCompanyName.Font = new Font("Segoe UI", 9F);
            _lblCompanyName.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCompanyName.Location = new Point(24, 65);
            _lblCompanyName.Name = "_lblCompanyName";
            _lblCompanyName.Size = new Size(102, 15);
            _lblCompanyName.TabIndex = 2;
            _lblCompanyName.Text = "Company Name *";
            // 
            // _txtCompanyName
            // 
            _txtCompanyName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtCompanyName.BorderStyle = BorderStyle.FixedSingle;
            _txtCompanyName.Font = new Font("Segoe UI", 10F);
            _txtCompanyName.Location = new Point(24, 85);
            _txtCompanyName.MaxLength = 100;
            _txtCompanyName.Name = "_txtCompanyName";
            _txtCompanyName.PlaceholderText = "Enter company name...";
            _txtCompanyName.Size = new Size(460, 25);
            _txtCompanyName.TabIndex = 3;
            // 
            // _lblCommercialNumber
            // 
            _lblCommercialNumber.AutoSize = true;
            _lblCommercialNumber.Font = new Font("Segoe UI", 9F);
            _lblCommercialNumber.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCommercialNumber.Location = new Point(24, 123);
            _lblCommercialNumber.Name = "_lblCommercialNumber";
            _lblCommercialNumber.Size = new Size(119, 15);
            _lblCommercialNumber.TabIndex = 4;
            _lblCommercialNumber.Text = "Commercial Number";
            // 
            // _txtCommercialNumber
            // 
            _txtCommercialNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtCommercialNumber.BorderStyle = BorderStyle.FixedSingle;
            _txtCommercialNumber.Font = new Font("Segoe UI", 10F);
            _txtCommercialNumber.Location = new Point(24, 143);
            _txtCommercialNumber.Name = "_txtCommercialNumber";
            _txtCommercialNumber.PlaceholderText = "e.g. CR-123456";
            _txtCommercialNumber.Size = new Size(460, 25);
            _txtCommercialNumber.TabIndex = 5;
            // 
            // _pnlSep2
            // 
            _pnlSep2.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep2.Location = new Point(24, 187);
            _pnlSep2.Name = "_pnlSep2";
            _pnlSep2.Size = new Size(608, 1);
            _pnlSep2.TabIndex = 6;
            // 
            // _lblSectionContact
            // 
            _lblSectionContact.AutoSize = true;
            _lblSectionContact.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionContact.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionContact.Location = new Point(24, 197);
            _lblSectionContact.Name = "_lblSectionContact";
            _lblSectionContact.Size = new Size(137, 19);
            _lblSectionContact.TabIndex = 7;
            _lblSectionContact.Text = "📞  Contact Details";
            // 
            // _lblPhone
            // 
            _lblPhone.AutoSize = true;
            _lblPhone.Font = new Font("Segoe UI", 9F);
            _lblPhone.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPhone.Location = new Point(24, 229);
            _lblPhone.Name = "_lblPhone";
            _lblPhone.Size = new Size(41, 15);
            _lblPhone.TabIndex = 8;
            _lblPhone.Text = "Phone";
            // 
            // _txtPhone
            // 
            _txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtPhone.BorderStyle = BorderStyle.FixedSingle;
            _txtPhone.Font = new Font("Segoe UI", 10F);
            _txtPhone.Location = new Point(24, 249);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.PlaceholderText = "e.g. +962 799 000 000";
            _txtPhone.Size = new Size(460, 25);
            _txtPhone.TabIndex = 9;
            // 
            // _lblEmail
            // 
            _lblEmail.AutoSize = true;
            _lblEmail.Font = new Font("Segoe UI", 9F);
            _lblEmail.ForeColor = Color.FromArgb(100, 116, 139);
            _lblEmail.Location = new Point(24, 287);
            _lblEmail.Name = "_lblEmail";
            _lblEmail.Size = new Size(36, 15);
            _lblEmail.TabIndex = 10;
            _lblEmail.Text = "Email";
            // 
            // _txtEmail
            // 
            _txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtEmail.BorderStyle = BorderStyle.FixedSingle;
            _txtEmail.Font = new Font("Segoe UI", 10F);
            _txtEmail.Location = new Point(24, 307);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.PlaceholderText = "e.g. company@example.com";
            _txtEmail.Size = new Size(460, 25);
            _txtEmail.TabIndex = 11;
            // 
            // _lblContactPerson
            // 
            _lblContactPerson.AutoSize = true;
            _lblContactPerson.Font = new Font("Segoe UI", 9F);
            _lblContactPerson.ForeColor = Color.FromArgb(100, 116, 139);
            _lblContactPerson.Location = new Point(24, 345);
            _lblContactPerson.Name = "_lblContactPerson";
            _lblContactPerson.Size = new Size(88, 15);
            _lblContactPerson.TabIndex = 12;
            _lblContactPerson.Text = "Contact Person";
            // 
            // _txtContactPersonID
            // 
            _txtContactPersonID.BackColor = Color.FromArgb(245, 247, 250);
            _txtContactPersonID.BorderStyle = BorderStyle.FixedSingle;
            _txtContactPersonID.Enabled = false;
            _txtContactPersonID.Font = new Font("Segoe UI", 10F);
            _txtContactPersonID.Location = new Point(24, 365);
            _txtContactPersonID.Name = "_txtContactPersonID";
            _txtContactPersonID.PlaceholderText = "ID";
            _txtContactPersonID.ReadOnly = true;
            _txtContactPersonID.Size = new Size(72, 25);
            _txtContactPersonID.TabIndex = 13;
            _txtContactPersonID.TabStop = false;
            // 
            // _txtContactPersonName
            // 
            _txtContactPersonName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtContactPersonName.BackColor = Color.FromArgb(245, 247, 250);
            _txtContactPersonName.BorderStyle = BorderStyle.FixedSingle;
            _txtContactPersonName.Enabled = false;
            _txtContactPersonName.Font = new Font("Segoe UI", 10F);
            _txtContactPersonName.Location = new Point(104, 365);
            _txtContactPersonName.Name = "_txtContactPersonName";
            _txtContactPersonName.PlaceholderText = "Select a person using the Find button →";
            _txtContactPersonName.ReadOnly = true;
            _txtContactPersonName.Size = new Size(268, 25);
            _txtContactPersonName.TabIndex = 14;
            _txtContactPersonName.TabStop = false;
            // 
            // _btnFindPerson
            // 
            _btnFindPerson.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnFindPerson.BackColor = Color.FromArgb(219, 234, 254);
            _btnFindPerson.Cursor = Cursors.Hand;
            _btnFindPerson.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
            _btnFindPerson.FlatAppearance.MouseOverBackColor = Color.FromArgb(191, 219, 254);
            _btnFindPerson.FlatStyle = FlatStyle.Flat;
            _btnFindPerson.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnFindPerson.ForeColor = Color.FromArgb(37, 99, 235);
            _btnFindPerson.Location = new Point(378, 360);
            _btnFindPerson.Name = "_btnFindPerson";
            _btnFindPerson.Size = new Size(46, 31);
            _btnFindPerson.TabIndex = 15;
            _btnFindPerson.Text = "Add";
            _btnFindPerson.UseVisualStyleBackColor = false;
            _btnFindPerson.Click += _btnFindPerson_Click;
            // 
            // _pnlSep3
            // 
            _pnlSep3.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep3.Location = new Point(24, 411);
            _pnlSep3.Name = "_pnlSep3";
            _pnlSep3.Size = new Size(608, 1);
            _pnlSep3.TabIndex = 16;
            // 
            // _lblSectionLocation
            // 
            _lblSectionLocation.AutoSize = true;
            _lblSectionLocation.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionLocation.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionLocation.Location = new Point(24, 421);
            _lblSectionLocation.Name = "_lblSectionLocation";
            _lblSectionLocation.Size = new Size(94, 19);
            _lblSectionLocation.TabIndex = 17;
            _lblSectionLocation.Text = "📍  Location";
            // 
            // _lblCountry
            // 
            _lblCountry.AutoSize = true;
            _lblCountry.Font = new Font("Segoe UI", 9F);
            _lblCountry.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCountry.Location = new Point(24, 453);
            _lblCountry.Name = "_lblCountry";
            _lblCountry.Size = new Size(50, 15);
            _lblCountry.TabIndex = 18;
            _lblCountry.Text = "Country";
            // 
            // _cmbCountry
            // 
            _cmbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCountry.FlatStyle = FlatStyle.Flat;
            _cmbCountry.Font = new Font("Segoe UI", 10F);
            _cmbCountry.Location = new Point(24, 473);
            _cmbCountry.Name = "_cmbCountry";
            _cmbCountry.Size = new Size(300, 25);
            _cmbCountry.TabIndex = 19;
            // 
            // _lblAddress
            // 
            _lblAddress.AutoSize = true;
            _lblAddress.Font = new Font("Segoe UI", 9F);
            _lblAddress.ForeColor = Color.FromArgb(100, 116, 139);
            _lblAddress.Location = new Point(24, 513);
            _lblAddress.Name = "_lblAddress";
            _lblAddress.Size = new Size(49, 15);
            _lblAddress.TabIndex = 20;
            _lblAddress.Text = "Address";
            // 
            // _txtAddress
            // 
            _txtAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtAddress.BorderStyle = BorderStyle.FixedSingle;
            _txtAddress.Font = new Font("Segoe UI", 10F);
            _txtAddress.Location = new Point(24, 533);
            _txtAddress.Multiline = true;
            _txtAddress.Name = "_txtAddress";
            _txtAddress.PlaceholderText = "Street, City, Postal Code...";
            _txtAddress.ScrollBars = ScrollBars.Vertical;
            _txtAddress.Size = new Size(460, 68);
            _txtAddress.TabIndex = 21;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditCompany
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlContent);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Controls.Add(_pnlButtons);
            Name = "ctrlAddEditCompany";
            Size = new Size(548, 800);
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

        private Button button1;
        private Label _lblID;
    }
}
