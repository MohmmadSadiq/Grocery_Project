namespace RMS_UI.Controls
{
    partial class ctrlAddEditUser
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblMode;
        private RMS_UI.Controls.NotificationControl _notification;
        private System.Windows.Forms.Panel _pnlButtons;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.TableLayoutPanel _splitLayout;
        private RMS_UI.Peoples.ctrlPersonCardWithConfig _personCard;
        private System.Windows.Forms.Panel _pnlUserCard;
        private System.Windows.Forms.Label _lblUserSection;
        private System.Windows.Forms.Label _lblUserName;
        private System.Windows.Forms.TextBox _txtUserName;
        private System.Windows.Forms.CheckBox _chkIsActive;
        private System.Windows.Forms.CheckBox _chkChangePassword;
        private System.Windows.Forms.Label _lblPassword;
        private System.Windows.Forms.TextBox _txtPassword;
        private System.Windows.Forms.Label _lblConfirmPassword;
        private System.Windows.Forms.TextBox _txtConfirmPassword;
        private System.Windows.Forms.CheckBox _chkShowPassword;
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
            _splitLayout = new System.Windows.Forms.TableLayoutPanel();
            _personCard = new RMS_UI.Peoples.ctrlPersonCardWithConfig();
            _pnlUserCard = new System.Windows.Forms.Panel();
            _lblUserSection = new System.Windows.Forms.Label();
            _lblUserName = new System.Windows.Forms.Label();
            _txtUserName = new System.Windows.Forms.TextBox();
            _chkIsActive = new System.Windows.Forms.CheckBox();
            _chkChangePassword = new System.Windows.Forms.CheckBox();
            _lblPassword = new System.Windows.Forms.Label();
            _txtPassword = new System.Windows.Forms.TextBox();
            _lblConfirmPassword = new System.Windows.Forms.Label();
            _txtConfirmPassword = new System.Windows.Forms.TextBox();
            _chkShowPassword = new System.Windows.Forms.CheckBox();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);
            _pnlHeader.SuspendLayout();
            _pnlButtons.SuspendLayout();
            _splitLayout.SuspendLayout();
            _pnlUserCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlHeader.Location = new System.Drawing.Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            _pnlHeader.Size = new System.Drawing.Size(1120, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            _lblTitle.Location = new System.Drawing.Point(24, 8);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(192, 28);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "👤  Add New User";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblMode.Location = new System.Drawing.Point(26, 39);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new System.Drawing.Size(356, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Link a person on the left and complete user account info on the right.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = System.Windows.Forms.DockStyle.Top;
            _notification.Location = new System.Drawing.Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new System.Drawing.Size(1120, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _pnlButtons
            // 
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            _pnlButtons.Location = new System.Drawing.Point(0, 635);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new System.Windows.Forms.Padding(0, 14, 24, 14);
            _pnlButtons.Size = new System.Drawing.Size(1120, 65);
            _pnlButtons.TabIndex = 3;
            // 
            // _btnSave
            // 
            _btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnSave.Location = new System.Drawing.Point(974, 14);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new System.Drawing.Size(122, 38);
            _btnSave.TabIndex = 1;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = true;
            _btnSave.Click += _btnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            _btnCancel.Location = new System.Drawing.Point(844, 14);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(120, 38);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            _btnCancel.Click += _btnCancel_Click;
            // 
            // _splitLayout
            // 
            _splitLayout.ColumnCount = 2;
            _splitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            _splitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            _splitLayout.Controls.Add(_personCard, 0, 0);
            _splitLayout.Controls.Add(_pnlUserCard, 1, 0);
            _splitLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            _splitLayout.Location = new System.Drawing.Point(0, 72);
            _splitLayout.Name = "_splitLayout";
            _splitLayout.Padding = new System.Windows.Forms.Padding(20);
            _splitLayout.RowCount = 1;
            _splitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _splitLayout.Size = new System.Drawing.Size(1120, 563);
            _splitLayout.TabIndex = 2;
            // 
            // _personCard
            // 
            _personCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _personCard.Location = new System.Drawing.Point(23, 23);
            _personCard.Name = "_personCard";
            _personCard.Size = new System.Drawing.Size(577, 517);
            _personCard.TabIndex = 0;
            // 
            // _pnlUserCard
            // 
            _pnlUserCard.Controls.Add(_txtConfirmPassword);
            _pnlUserCard.Controls.Add(_lblConfirmPassword);
            _pnlUserCard.Controls.Add(_chkShowPassword);
            _pnlUserCard.Controls.Add(_txtPassword);
            _pnlUserCard.Controls.Add(_lblPassword);
            _pnlUserCard.Controls.Add(_chkChangePassword);
            _pnlUserCard.Controls.Add(_chkIsActive);
            _pnlUserCard.Controls.Add(_txtUserName);
            _pnlUserCard.Controls.Add(_lblUserName);
            _pnlUserCard.Controls.Add(_lblUserSection);
            _pnlUserCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlUserCard.Location = new System.Drawing.Point(606, 23);
            _pnlUserCard.Name = "_pnlUserCard";
            _pnlUserCard.Padding = new System.Windows.Forms.Padding(20);
            _pnlUserCard.Size = new System.Drawing.Size(491, 517);
            _pnlUserCard.TabIndex = 1;
            _pnlUserCard.Paint += _pnlUserCard_Paint;
            // 
            // _lblUserSection
            // 
            _lblUserSection.AutoSize = true;
            _lblUserSection.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            _lblUserSection.Location = new System.Drawing.Point(20, 20);
            _lblUserSection.Name = "_lblUserSection";
            _lblUserSection.Size = new System.Drawing.Size(149, 21);
            _lblUserSection.TabIndex = 0;
            _lblUserSection.Text = "Account Settings";
            // 
            // _lblUserName
            // 
            _lblUserName.AutoSize = true;
            _lblUserName.Location = new System.Drawing.Point(20, 66);
            _lblUserName.Name = "_lblUserName";
            _lblUserName.Size = new System.Drawing.Size(63, 15);
            _lblUserName.TabIndex = 1;
            _lblUserName.Text = "Username";
            // 
            // _txtUserName
            // 
            _txtUserName.Location = new System.Drawing.Point(20, 84);
            _txtUserName.MaxLength = 50;
            _txtUserName.Name = "_txtUserName";
            _txtUserName.Size = new System.Drawing.Size(445, 23);
            _txtUserName.TabIndex = 2;
            // 
            // _chkIsActive
            // 
            _chkIsActive.AutoSize = true;
            _chkIsActive.Checked = true;
            _chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkIsActive.Location = new System.Drawing.Point(20, 122);
            _chkIsActive.Name = "_chkIsActive";
            _chkIsActive.Size = new System.Drawing.Size(69, 19);
            _chkIsActive.TabIndex = 3;
            _chkIsActive.Text = "Is Active";
            _chkIsActive.UseVisualStyleBackColor = true;
            // 
            // _chkChangePassword
            // 
            _chkChangePassword.AutoSize = true;
            _chkChangePassword.Location = new System.Drawing.Point(20, 157);
            _chkChangePassword.Name = "_chkChangePassword";
            _chkChangePassword.Size = new System.Drawing.Size(120, 19);
            _chkChangePassword.TabIndex = 4;
            _chkChangePassword.Text = "Change Password";
            _chkChangePassword.UseVisualStyleBackColor = true;
            _chkChangePassword.CheckedChanged += _chkChangePassword_CheckedChanged;
            // 
            // _lblPassword
            // 
            _lblPassword.AutoSize = true;
            _lblPassword.Location = new System.Drawing.Point(20, 186);
            _lblPassword.Name = "_lblPassword";
            _lblPassword.Size = new System.Drawing.Size(57, 15);
            _lblPassword.TabIndex = 5;
            _lblPassword.Text = "Password";
            // 
            // _txtPassword
            // 
            _txtPassword.Location = new System.Drawing.Point(20, 204);
            _txtPassword.MaxLength = 100;
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Size = new System.Drawing.Size(445, 23);
            _txtPassword.TabIndex = 6;
            _txtPassword.UseSystemPasswordChar = true;
            // 
            // _lblConfirmPassword
            // 
            _lblConfirmPassword.AutoSize = true;
            _lblConfirmPassword.Location = new System.Drawing.Point(20, 240);
            _lblConfirmPassword.Name = "_lblConfirmPassword";
            _lblConfirmPassword.Size = new System.Drawing.Size(102, 15);
            _lblConfirmPassword.TabIndex = 7;
            _lblConfirmPassword.Text = "Confirm Password";
            // 
            // _txtConfirmPassword
            // 
            _txtConfirmPassword.Location = new System.Drawing.Point(20, 258);
            _txtConfirmPassword.MaxLength = 100;
            _txtConfirmPassword.Name = "_txtConfirmPassword";
            _txtConfirmPassword.Size = new System.Drawing.Size(445, 23);
            _txtConfirmPassword.TabIndex = 8;
            _txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // _chkShowPassword
            // 
            _chkShowPassword.AutoSize = true;
            _chkShowPassword.Location = new System.Drawing.Point(20, 287);
            _chkShowPassword.Name = "_chkShowPassword";
            _chkShowPassword.Size = new System.Drawing.Size(108, 19);
            _chkShowPassword.TabIndex = 9;
            _chkShowPassword.Text = "Show Password";
            _chkShowPassword.UseVisualStyleBackColor = true;
            _chkShowPassword.CheckedChanged += _chkShowPassword_CheckedChanged;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditUser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_splitLayout);
            Controls.Add(_pnlButtons);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Name = "ctrlAddEditUser";
            Size = new System.Drawing.Size(1120, 700);
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlButtons.ResumeLayout(false);
            _splitLayout.ResumeLayout(false);
            _pnlUserCard.ResumeLayout(false);
            _pnlUserCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }
    }
}
