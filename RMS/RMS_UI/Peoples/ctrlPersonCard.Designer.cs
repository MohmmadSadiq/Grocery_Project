namespace RMS_UI.Controls
{
    partial class ctrlPersonCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _pnlContainer = new Panel();
            _pnlImageSection = new Panel();
            _picPersonImage = new PictureBox();
            _pnlMainInfo = new Panel();
            _lblPersonID = new Label();
            _lblFullName = new Label();
            _lblNationalNo = new Label();
            _pnlContactSection = new Panel();
            _lblAddressLabel = new Label();
            _lblAddress = new Label();
            _lblCountryLabel = new Label();
            _lblCountry = new Label();
            _lblEmailLabel = new Label();
            _lblEmail = new Label();
            _lblPhoneLabel = new Label();
            _lblPhone = new Label();
            _pnlPersonalInfo = new Panel();
            _lblGenderLabel = new Label();
            _lblGender = new Label();
            _lblDateLabel = new Label();
            _lblAge = new Label();
            _lblDateOfBirth = new Label();
            _pnlAuditInfo = new Panel();
            _lblCreatedDateLabel = new Label();
            _lblCreatedDate = new Label();
            _lblCreatedByLabel = new Label();
            _lblCreatedBy = new Label();
            _lblUpdatedDateLabel = new Label();
            _lblUpdatedDate = new Label();
            _lblUpdatedByLabel = new Label();
            _lblUpdatedBy = new Label();
            _pnlContainer.SuspendLayout();
            _pnlImageSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picPersonImage).BeginInit();
            _pnlMainInfo.SuspendLayout();
            _pnlContactSection.SuspendLayout();
            _pnlPersonalInfo.SuspendLayout();
            _pnlAuditInfo.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlContainer
            // 
            _pnlContainer.Controls.Add(_pnlImageSection);
            _pnlContainer.Controls.Add(_pnlMainInfo);
            _pnlContainer.Controls.Add(_pnlContactSection);
            _pnlContainer.Controls.Add(_pnlPersonalInfo);
            _pnlContainer.Controls.Add(_pnlAuditInfo);
            _pnlContainer.Dock = DockStyle.Fill;
            _pnlContainer.Location = new Point(0, 0);
            _pnlContainer.Margin = new Padding(4, 3, 4, 3);
            _pnlContainer.Name = "_pnlContainer";
            _pnlContainer.Padding = new Padding(18, 17, 18, 17);
            _pnlContainer.Size = new Size(467, 553);
            _pnlContainer.TabIndex = 0;
            // 
            // _pnlImageSection
            // 
            _pnlImageSection.Controls.Add(_picPersonImage);
            _pnlImageSection.Dock = DockStyle.Top;
            _pnlImageSection.Location = new Point(18, 109);
            _pnlImageSection.Margin = new Padding(4, 3, 4, 3);
            _pnlImageSection.Name = "_pnlImageSection";
            _pnlImageSection.Size = new Size(431, 173);
            _pnlImageSection.TabIndex = 0;
            // 
            // _picPersonImage
            // 
            _picPersonImage.BackgroundImageLayout = ImageLayout.Zoom;
            _picPersonImage.Dock = DockStyle.Fill;
            _picPersonImage.Location = new Point(0, 0);
            _picPersonImage.Margin = new Padding(4, 3, 4, 3);
            _picPersonImage.Name = "_picPersonImage";
            _picPersonImage.Size = new Size(431, 173);
            _picPersonImage.SizeMode = PictureBoxSizeMode.CenterImage;
            _picPersonImage.TabIndex = 0;
            _picPersonImage.TabStop = false;
            // 
            // _pnlMainInfo
            // 
            _pnlMainInfo.Controls.Add(_lblPersonID);
            _pnlMainInfo.Controls.Add(_lblFullName);
            _pnlMainInfo.Controls.Add(_lblNationalNo);
            _pnlMainInfo.Dock = DockStyle.Top;
            _pnlMainInfo.Location = new Point(18, 17);
            _pnlMainInfo.Margin = new Padding(4, 3, 4, 3);
            _pnlMainInfo.Name = "_pnlMainInfo";
            _pnlMainInfo.Padding = new Padding(0, 17, 0, 12);
            _pnlMainInfo.Size = new Size(431, 92);
            _pnlMainInfo.TabIndex = 1;
            // 
            // _lblPersonID
            // 
            _lblPersonID.AutoSize = true;
            _lblPersonID.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            _lblPersonID.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPersonID.Location = new Point(0, 0);
            _lblPersonID.Margin = new Padding(4, 0, 4, 0);
            _lblPersonID.Name = "_lblPersonID";
            _lblPersonID.Size = new Size(47, 15);
            _lblPersonID.TabIndex = 0;
            _lblPersonID.Text = "ID: N/A";
            // 
            // _lblFullName
            // 
            _lblFullName.AutoEllipsis = true;
            _lblFullName.AutoSize = true;
            _lblFullName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblFullName.ForeColor = Color.FromArgb(30, 41, 59);
            _lblFullName.Location = new Point(0, 21);
            _lblFullName.Margin = new Padding(4, 0, 4, 0);
            _lblFullName.Name = "_lblFullName";
            _lblFullName.Size = new Size(116, 30);
            _lblFullName.TabIndex = 1;
            _lblFullName.Text = "Full Name";
            // 
            // _lblNationalNo
            // 
            _lblNationalNo.AutoEllipsis = true;
            _lblNationalNo.AutoSize = true;
            _lblNationalNo.Font = new Font("Segoe UI", 9F);
            _lblNationalNo.ForeColor = Color.FromArgb(100, 116, 139);
            _lblNationalNo.Location = new Point(0, 58);
            _lblNationalNo.Margin = new Padding(4, 0, 4, 0);
            _lblNationalNo.Name = "_lblNationalNo";
            _lblNationalNo.Size = new Size(94, 15);
            _lblNationalNo.TabIndex = 2;
            _lblNationalNo.Text = "National ID: N/A";
            // 
            // _pnlContactSection
            // 
            _pnlContactSection.Controls.Add(_lblAddressLabel);
            _pnlContactSection.Controls.Add(_lblAddress);
            _pnlContactSection.Controls.Add(_lblCountryLabel);
            _pnlContactSection.Controls.Add(_lblCountry);
            _pnlContactSection.Controls.Add(_lblEmailLabel);
            _pnlContactSection.Controls.Add(_lblEmail);
            _pnlContactSection.Controls.Add(_lblPhoneLabel);
            _pnlContactSection.Controls.Add(_lblPhone);
            _pnlContactSection.Dock = DockStyle.Bottom;
            _pnlContactSection.Location = new Point(18, 293);
            _pnlContactSection.Margin = new Padding(4, 3, 4, 3);
            _pnlContactSection.Name = "_pnlContactSection";
            _pnlContactSection.Padding = new Padding(0, 12, 0, 12);
            _pnlContactSection.Size = new Size(431, 94);
            _pnlContactSection.TabIndex = 2;
            // 
            // _lblAddressLabel
            // 
            _lblAddressLabel.AutoSize = true;
            _lblAddressLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblAddressLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblAddressLabel.Location = new Point(233, 48);
            _lblAddressLabel.Margin = new Padding(4, 0, 4, 0);
            _lblAddressLabel.Name = "_lblAddressLabel";
            _lblAddressLabel.Size = new Size(54, 15);
            _lblAddressLabel.TabIndex = 4;
            _lblAddressLabel.Text = "Address:";
            // 
            // _lblAddress
            // 
            _lblAddress.AutoEllipsis = true;
            _lblAddress.AutoSize = true;
            _lblAddress.Font = new Font("Segoe UI", 9F);
            _lblAddress.ForeColor = Color.FromArgb(51, 65, 85);
            _lblAddress.Location = new Point(233, 68);
            _lblAddress.Margin = new Padding(4, 0, 4, 0);
            _lblAddress.Name = "_lblAddress";
            _lblAddress.Size = new Size(116, 15);
            _lblAddress.TabIndex = 5;
            _lblAddress.Text = "No address provided";
            // 
            // _lblCountryLabel
            // 
            _lblCountryLabel.AutoSize = true;
            _lblCountryLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblCountryLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCountryLabel.Location = new Point(233, 6);
            _lblCountryLabel.Margin = new Padding(4, 0, 4, 0);
            _lblCountryLabel.Name = "_lblCountryLabel";
            _lblCountryLabel.Size = new Size(54, 15);
            _lblCountryLabel.TabIndex = 6;
            _lblCountryLabel.Text = "Country:";
            // 
            // _lblCountry
            // 
            _lblCountry.AutoEllipsis = true;
            _lblCountry.AutoSize = true;
            _lblCountry.Font = new Font("Segoe UI", 9F);
            _lblCountry.ForeColor = Color.FromArgb(51, 65, 85);
            _lblCountry.Location = new Point(233, 26);
            _lblCountry.Margin = new Padding(4, 0, 4, 0);
            _lblCountry.Name = "_lblCountry";
            _lblCountry.Size = new Size(77, 15);
            _lblCountry.TabIndex = 7;
            _lblCountry.Text = "Not specified";
            // 
            // _lblEmailLabel
            // 
            _lblEmailLabel.AutoSize = true;
            _lblEmailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblEmailLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblEmailLabel.Location = new Point(0, 48);
            _lblEmailLabel.Margin = new Padding(4, 0, 4, 0);
            _lblEmailLabel.Name = "_lblEmailLabel";
            _lblEmailLabel.Size = new Size(39, 15);
            _lblEmailLabel.TabIndex = 2;
            _lblEmailLabel.Text = "Email:";
            // 
            // _lblEmail
            // 
            _lblEmail.AutoEllipsis = true;
            _lblEmail.AutoSize = true;
            _lblEmail.Font = new Font("Segoe UI", 9F);
            _lblEmail.ForeColor = Color.FromArgb(51, 65, 85);
            _lblEmail.Location = new Point(0, 68);
            _lblEmail.Margin = new Padding(4, 0, 4, 0);
            _lblEmail.Name = "_lblEmail";
            _lblEmail.Size = new Size(105, 15);
            _lblEmail.TabIndex = 3;
            _lblEmail.Text = "No email provided";
            // 
            // _lblPhoneLabel
            // 
            _lblPhoneLabel.AutoSize = true;
            _lblPhoneLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblPhoneLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPhoneLabel.Location = new Point(0, 6);
            _lblPhoneLabel.Margin = new Padding(4, 0, 4, 0);
            _lblPhoneLabel.Name = "_lblPhoneLabel";
            _lblPhoneLabel.Size = new Size(45, 15);
            _lblPhoneLabel.TabIndex = 0;
            _lblPhoneLabel.Text = "Phone:";
            // 
            // _lblPhone
            // 
            _lblPhone.AutoEllipsis = true;
            _lblPhone.AutoSize = true;
            _lblPhone.Font = new Font("Segoe UI", 9F);
            _lblPhone.ForeColor = Color.FromArgb(51, 65, 85);
            _lblPhone.Location = new Point(0, 25);
            _lblPhone.Margin = new Padding(4, 0, 4, 0);
            _lblPhone.Name = "_lblPhone";
            _lblPhone.Size = new Size(110, 15);
            _lblPhone.TabIndex = 1;
            _lblPhone.Text = "No phone provided";
            // 
            // _pnlPersonalInfo
            // 
            _pnlPersonalInfo.Controls.Add(_lblGenderLabel);
            _pnlPersonalInfo.Controls.Add(_lblGender);
            _pnlPersonalInfo.Controls.Add(_lblDateLabel);
            _pnlPersonalInfo.Controls.Add(_lblAge);
            _pnlPersonalInfo.Controls.Add(_lblDateOfBirth);
            _pnlPersonalInfo.Dock = DockStyle.Bottom;
            _pnlPersonalInfo.Location = new Point(18, 387);
            _pnlPersonalInfo.Margin = new Padding(4, 3, 4, 3);
            _pnlPersonalInfo.Name = "_pnlPersonalInfo";
            _pnlPersonalInfo.Padding = new Padding(0, 12, 0, 0);
            _pnlPersonalInfo.Size = new Size(431, 48);
            _pnlPersonalInfo.TabIndex = 3;
            // 
            // _lblGenderLabel
            // 
            _lblGenderLabel.AutoSize = true;
            _lblGenderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblGenderLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblGenderLabel.Location = new Point(233, 3);
            _lblGenderLabel.Margin = new Padding(4, 0, 4, 0);
            _lblGenderLabel.Name = "_lblGenderLabel";
            _lblGenderLabel.Size = new Size(52, 15);
            _lblGenderLabel.TabIndex = 3;
            _lblGenderLabel.Text = "Gender:";
            // 
            // _lblGender
            // 
            _lblGender.AutoSize = true;
            _lblGender.Font = new Font("Segoe UI", 9F);
            _lblGender.ForeColor = Color.FromArgb(51, 65, 85);
            _lblGender.Location = new Point(233, 23);
            _lblGender.Margin = new Padding(4, 0, 4, 0);
            _lblGender.Name = "_lblGender";
            _lblGender.Size = new Size(77, 15);
            _lblGender.TabIndex = 4;
            _lblGender.Text = "Not specified";
            // 
            // _lblDateLabel
            // 
            _lblDateLabel.AutoSize = true;
            _lblDateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblDateLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblDateLabel.Location = new Point(0, 6);
            _lblDateLabel.Margin = new Padding(4, 0, 4, 0);
            _lblDateLabel.Name = "_lblDateLabel";
            _lblDateLabel.Size = new Size(83, 15);
            _lblDateLabel.TabIndex = 0;
            _lblDateLabel.Text = "Date of Birth:";
            // 
            // _lblAge
            // 
            _lblAge.AutoSize = true;
            _lblAge.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            _lblAge.ForeColor = Color.FromArgb(100, 116, 139);
            _lblAge.Location = new Point(99, 25);
            _lblAge.Margin = new Padding(4, 0, 4, 0);
            _lblAge.Name = "_lblAge";
            _lblAge.Size = new Size(52, 15);
            _lblAge.TabIndex = 2;
            _lblAge.Text = "(0 years)";
            // 
            // _lblDateOfBirth
            // 
            _lblDateOfBirth.AutoSize = true;
            _lblDateOfBirth.Font = new Font("Segoe UI", 9F);
            _lblDateOfBirth.ForeColor = Color.FromArgb(51, 65, 85);
            _lblDateOfBirth.Location = new Point(0, 25);
            _lblDateOfBirth.Margin = new Padding(4, 0, 4, 0);
            _lblDateOfBirth.Name = "_lblDateOfBirth";
            _lblDateOfBirth.Size = new Size(29, 15);
            _lblDateOfBirth.TabIndex = 1;
            _lblDateOfBirth.Text = "N/A";
            // 
            // _pnlAuditInfo
            // 
            _pnlAuditInfo.Controls.Add(_lblCreatedDateLabel);
            _pnlAuditInfo.Controls.Add(_lblCreatedDate);
            _pnlAuditInfo.Controls.Add(_lblCreatedByLabel);
            _pnlAuditInfo.Controls.Add(_lblCreatedBy);
            _pnlAuditInfo.Controls.Add(_lblUpdatedDateLabel);
            _pnlAuditInfo.Controls.Add(_lblUpdatedDate);
            _pnlAuditInfo.Controls.Add(_lblUpdatedByLabel);
            _pnlAuditInfo.Controls.Add(_lblUpdatedBy);
            _pnlAuditInfo.Dock = DockStyle.Bottom;
            _pnlAuditInfo.Location = new Point(18, 435);
            _pnlAuditInfo.Margin = new Padding(4, 3, 4, 3);
            _pnlAuditInfo.Name = "_pnlAuditInfo";
            _pnlAuditInfo.Padding = new Padding(0, 12, 0, 0);
            _pnlAuditInfo.Size = new Size(431, 101);
            _pnlAuditInfo.TabIndex = 4;
            // 
            // _lblCreatedDateLabel
            // 
            _lblCreatedDateLabel.AutoSize = true;
            _lblCreatedDateLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblCreatedDateLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCreatedDateLabel.Location = new Point(0, 6);
            _lblCreatedDateLabel.Margin = new Padding(4, 0, 4, 0);
            _lblCreatedDateLabel.Name = "_lblCreatedDateLabel";
            _lblCreatedDateLabel.Size = new Size(84, 15);
            _lblCreatedDateLabel.TabIndex = 0;
            _lblCreatedDateLabel.Text = "Created Date:";
            // 
            // _lblCreatedDate
            // 
            _lblCreatedDate.AutoSize = true;
            _lblCreatedDate.Font = new Font("Segoe UI", 8.5F);
            _lblCreatedDate.ForeColor = Color.FromArgb(51, 65, 85);
            _lblCreatedDate.Location = new Point(0, 23);
            _lblCreatedDate.Margin = new Padding(4, 0, 4, 0);
            _lblCreatedDate.Name = "_lblCreatedDate";
            _lblCreatedDate.Size = new Size(29, 15);
            _lblCreatedDate.TabIndex = 1;
            _lblCreatedDate.Text = "N/A";
            // 
            // _lblCreatedByLabel
            // 
            _lblCreatedByLabel.AutoSize = true;
            _lblCreatedByLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblCreatedByLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblCreatedByLabel.Location = new Point(233, 6);
            _lblCreatedByLabel.Margin = new Padding(4, 0, 4, 0);
            _lblCreatedByLabel.Name = "_lblCreatedByLabel";
            _lblCreatedByLabel.Size = new Size(71, 15);
            _lblCreatedByLabel.TabIndex = 2;
            _lblCreatedByLabel.Text = "Created By:";
            // 
            // _lblCreatedBy
            // 
            _lblCreatedBy.AutoSize = true;
            _lblCreatedBy.Font = new Font("Segoe UI", 8.5F);
            _lblCreatedBy.ForeColor = Color.FromArgb(51, 65, 85);
            _lblCreatedBy.Location = new Point(233, 23);
            _lblCreatedBy.Margin = new Padding(4, 0, 4, 0);
            _lblCreatedBy.Name = "_lblCreatedBy";
            _lblCreatedBy.Size = new Size(29, 15);
            _lblCreatedBy.TabIndex = 3;
            _lblCreatedBy.Text = "N/A";
            // 
            // _lblUpdatedDateLabel
            // 
            _lblUpdatedDateLabel.AutoSize = true;
            _lblUpdatedDateLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblUpdatedDateLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblUpdatedDateLabel.Location = new Point(0, 58);
            _lblUpdatedDateLabel.Margin = new Padding(4, 0, 4, 0);
            _lblUpdatedDateLabel.Name = "_lblUpdatedDateLabel";
            _lblUpdatedDateLabel.Size = new Size(88, 15);
            _lblUpdatedDateLabel.TabIndex = 4;
            _lblUpdatedDateLabel.Text = "Updated Date:";
            // 
            // _lblUpdatedDate
            // 
            _lblUpdatedDate.AutoSize = true;
            _lblUpdatedDate.Font = new Font("Segoe UI", 8.5F);
            _lblUpdatedDate.ForeColor = Color.FromArgb(51, 65, 85);
            _lblUpdatedDate.Location = new Point(0, 75);
            _lblUpdatedDate.Margin = new Padding(4, 0, 4, 0);
            _lblUpdatedDate.Name = "_lblUpdatedDate";
            _lblUpdatedDate.Size = new Size(29, 15);
            _lblUpdatedDate.TabIndex = 5;
            _lblUpdatedDate.Text = "N/A";
            // 
            // _lblUpdatedByLabel
            // 
            _lblUpdatedByLabel.AutoSize = true;
            _lblUpdatedByLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblUpdatedByLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblUpdatedByLabel.Location = new Point(233, 58);
            _lblUpdatedByLabel.Margin = new Padding(4, 0, 4, 0);
            _lblUpdatedByLabel.Name = "_lblUpdatedByLabel";
            _lblUpdatedByLabel.Size = new Size(75, 15);
            _lblUpdatedByLabel.TabIndex = 6;
            _lblUpdatedByLabel.Text = "Updated By:";
            // 
            // _lblUpdatedBy
            // 
            _lblUpdatedBy.AutoSize = true;
            _lblUpdatedBy.Font = new Font("Segoe UI", 8.5F);
            _lblUpdatedBy.ForeColor = Color.FromArgb(51, 65, 85);
            _lblUpdatedBy.Location = new Point(233, 75);
            _lblUpdatedBy.Margin = new Padding(4, 0, 4, 0);
            _lblUpdatedBy.Name = "_lblUpdatedBy";
            _lblUpdatedBy.Size = new Size(29, 15);
            _lblUpdatedBy.TabIndex = 7;
            _lblUpdatedBy.Text = "N/A";
            // 
            // ctrlPersonCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(_pnlContainer);
            Margin = new Padding(12);
            Name = "ctrlPersonCard";
            Size = new Size(467, 553);
            _pnlContainer.ResumeLayout(false);
            _pnlImageSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picPersonImage).EndInit();
            _pnlMainInfo.ResumeLayout(false);
            _pnlMainInfo.PerformLayout();
            _pnlContactSection.ResumeLayout(false);
            _pnlContactSection.PerformLayout();
            _pnlPersonalInfo.ResumeLayout(false);
            _pnlPersonalInfo.PerformLayout();
            _pnlAuditInfo.ResumeLayout(false);
            _pnlAuditInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _pnlContainer;
        private System.Windows.Forms.Panel _pnlImageSection;
        private System.Windows.Forms.PictureBox _picPersonImage;
        private System.Windows.Forms.Panel _pnlMainInfo;
        private System.Windows.Forms.Label _lblFullName;
        private System.Windows.Forms.Label _lblNationalNo;
        private System.Windows.Forms.Label _lblPersonID;
        private System.Windows.Forms.Panel _pnlContactSection;
        private System.Windows.Forms.Label _lblPhoneLabel;
        private System.Windows.Forms.Label _lblPhone;
        private System.Windows.Forms.Label _lblEmailLabel;
        private System.Windows.Forms.Label _lblEmail;
        private System.Windows.Forms.Label _lblAddressLabel;
        private System.Windows.Forms.Label _lblAddress;
        private System.Windows.Forms.Label _lblCountryLabel;
        private System.Windows.Forms.Label _lblCountry;
        private System.Windows.Forms.Panel _pnlPersonalInfo;
        private System.Windows.Forms.Label _lblDateLabel;
        private System.Windows.Forms.Label _lblDateOfBirth;
        private System.Windows.Forms.Label _lblAge;
        private System.Windows.Forms.Label _lblGenderLabel;
        private System.Windows.Forms.Label _lblGender;
        private System.Windows.Forms.Panel _pnlAuditInfo;
        private System.Windows.Forms.Label _lblCreatedDateLabel;
        private System.Windows.Forms.Label _lblCreatedDate;
        private System.Windows.Forms.Label _lblCreatedByLabel;
        private System.Windows.Forms.Label _lblCreatedBy;
        private System.Windows.Forms.Label _lblUpdatedDateLabel;
        private System.Windows.Forms.Label _lblUpdatedDate;
        private System.Windows.Forms.Label _lblUpdatedByLabel;
        private System.Windows.Forms.Label _lblUpdatedBy;
    }
}
