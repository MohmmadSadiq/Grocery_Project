using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Peoples;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class ctrlAddEditUser : UserControl
    {
        public event EventHandler<clsUser>? UserSaved;
        public event EventHandler? CancelClicked;

        private enum enMode { AddNew = 1, Edit = 2 }

        private readonly clsUser _user;
        private enMode _mode;
        private int? _selectedPersonID;

        public ctrlAddEditUser()
        {
            _mode = enMode.AddNew;
            _user = new clsUser();
            InitializeComponent();
            InitializeControl();
        }

        public ctrlAddEditUser(int userId)
        {
            _user = clsUser.Find(userId) ?? new clsUser();
            _mode = _user.UserID > 0 ? enMode.Edit : enMode.AddNew;
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            _personCard.PersonAdded += PersonCard_PersonAdded;
            _personCard.PersonEdited += PersonCard_PersonEdited;
            _personCard.PersonCleared += PersonCard_PersonCleared;

            if (_mode == enMode.Edit)
            {
                LoadUserData();
            }
            else
            {
                _chkChangePassword.Checked = true;
            }

            _txtUserName.TextChanged += (_, _) => UpdateSaveEnabled();
            _txtPassword.TextChanged += (_, _) => UpdateSaveEnabled();
            _txtConfirmPassword.TextChanged += (_, _) => UpdateSaveEnabled();

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();
            ApplyModeUi();
            UpdateSaveEnabled();
        }

        private void LoadUserData()
        {
            _lblTitle.Text = "👤  Edit User";
            _lblMode.Text = "Update account data and optionally change password.";

            _txtUserName.Text = _user.UserName;
            _chkIsActive.Checked = _user.IsActive;

            if (_user.PersonInfo != null)
            {
                _selectedPersonID = _user.PersonInfo.PersonID;
                _personCard.LoadPerson(_user.PersonInfo);
            }
        }

        private void ApplyModeUi()
        {
            bool isEditMode = _mode == enMode.Edit;

            _chkChangePassword.Visible = isEditMode;
            if (!isEditMode)
            {
                _chkChangePassword.Checked = true;
            }

            bool enablePasswordFields = !isEditMode || _chkChangePassword.Checked;
            _lblPassword.Enabled = enablePasswordFields;
            _txtPassword.Enabled = enablePasswordFields;
            _lblConfirmPassword.Enabled = enablePasswordFields;
            _txtConfirmPassword.Enabled = enablePasswordFields;
            _chkShowPassword.Enabled = enablePasswordFields;

            if (!enablePasswordFields)
            {
                _txtPassword.Clear();
                _txtConfirmPassword.Clear();
                _chkShowPassword.Checked = false;
            }
        }

        private void UpdateSaveEnabled()
        {
            _btnSave.Enabled = IsSaveReady();
        }

        private bool IsSaveReady()
        {
            if (string.IsNullOrWhiteSpace(_txtUserName.Text) || !_selectedPersonID.HasValue)
            {
                return false;
            }

            bool requirePassword = _mode == enMode.AddNew || _chkChangePassword.Checked;
            if (!requirePassword)
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(_txtPassword.Text)
                && !string.IsNullOrWhiteSpace(_txtConfirmPassword.Text);
        }

        private void PersonCard_PersonAdded(object? sender, PersonEventArgs e)
        {
            _selectedPersonID = e.Person?.PersonID;
            UpdateSaveEnabled();
        }

        private void PersonCard_PersonEdited(object? sender, PersonEventArgs e)
        {
            _selectedPersonID = e.Person?.PersonID;
            UpdateSaveEnabled();
        }

        private void PersonCard_PersonCleared(object? sender, EventArgs e)
        {
            _selectedPersonID = null;
            UpdateSaveEnabled();
        }

        private void _chkChangePassword_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyModeUi();
            UpdateSaveEnabled();
        }

        private void _chkShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            bool showPlainText = _chkShowPassword.Checked;
            _txtPassword.UseSystemPasswordChar = !showPlainText;
            _txtConfirmPassword.UseSystemPasswordChar = !showPlainText;
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            _user.PersonID = _selectedPersonID!.Value;
            _user.UserName = _txtUserName.Text.Trim();
            _user.IsActive = _chkIsActive.Checked;

            int? currentUserId = clsGlobalUser.CurrentUser?.UserID;
            if (_mode == enMode.AddNew)
            {
                _user.CreatedByUserID = currentUserId;
            }
            else
            {
                _user.UpdatedByUserID = currentUserId;
            }

            bool shouldChangePassword = _mode == enMode.AddNew || _chkChangePassword.Checked;
            if (shouldChangePassword)
            {
                var passwordResult = PasswordSecurity.CreateHashAndSalt(_txtPassword.Text.Trim());
                _user.PasswordHash = passwordResult.Hash;
                _user.PasswordSalt = passwordResult.Salt;
            }

            if (_user.Save())
            {
                _mode = enMode.Edit;
                _notification.ShowSuccess("User saved successfully.");
                UserSaved?.Invoke(this, _user);
                _lblTitle.Text = "👤  Edit User";
                _lblMode.Text = "Update account data and optionally change password.";
                _chkChangePassword.Checked = false;
                ApplyModeUi();
                return;
            }

            _notification.ShowError("Failed to save user. Please try again.");
        }

        private bool ValidateInput()
        {
            _errorProvider.Clear();
            _notification.HideImmediately();

            bool isValid = true;
            string userName = _txtUserName.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName))
            {
                _errorProvider.SetError(_txtUserName, "Username is required.");
                isValid = false;
            }

            if (!_selectedPersonID.HasValue)
            {
                _notification.ShowWarning("Please select a person from the left card before saving.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                int? excludeUserId = _mode == enMode.Edit ? _user.UserID : null;
                if (clsUser.IsUserNameExists(userName, excludeUserId))
                {
                    _errorProvider.SetError(_txtUserName, "This username already exists.");
                    isValid = false;
                }
            }

            bool requirePassword = _mode == enMode.AddNew || _chkChangePassword.Checked;
            if (requirePassword)
            {
                if (string.IsNullOrWhiteSpace(_txtPassword.Text))
                {
                    _errorProvider.SetError(_txtPassword, "Password is required.");
                    isValid = false;
                }
                else if (_txtPassword.Text.Trim().Length < 8)
                {
                    _errorProvider.SetError(_txtPassword, "Password must be at least 8 characters.");
                    isValid = false;
                }

                if (_txtPassword.Text != _txtConfirmPassword.Text)
                {
                    _errorProvider.SetError(_txtConfirmPassword, "Password confirmation does not match.");
                    isValid = false;
                }
            }

            if (!isValid && _notification.Visible == false)
            {
                _notification.ShowWarning("Please fix validation errors before saving.");
            }

            return isValid;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;
            _pnlHeader.BackColor = colors.ContentBackground;
            _splitLayout.BackColor = colors.FormBackground;
            _pnlButtons.BackColor = colors.ContentBackground;
            _pnlUserCard.BackColor = colors.ContentBackground;

            _lblTitle.ForeColor = colors.TitleText;
            _lblMode.ForeColor = colors.SecondaryText;
            _lblUserSection.ForeColor = colors.Primary;
            _lblUserName.ForeColor = colors.SecondaryText;
            _lblPassword.ForeColor = colors.SecondaryText;
            _lblConfirmPassword.ForeColor = colors.SecondaryText;

            _txtUserName.BackColor = colors.ContentBackground;
            _txtUserName.ForeColor = colors.PrimaryText;
            _txtPassword.BackColor = colors.ContentBackground;
            _txtPassword.ForeColor = colors.PrimaryText;
            _txtConfirmPassword.BackColor = colors.ContentBackground;
            _txtConfirmPassword.ForeColor = colors.PrimaryText;

            _chkIsActive.ForeColor = colors.PrimaryText;
            _chkChangePassword.ForeColor = colors.PrimaryText;
            _chkShowPassword.ForeColor = colors.PrimaryText;

            _btnSave.BackColor = colors.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = colors.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;

            _btnCancel.BackColor = colors.ContentBackground;
            _btnCancel.ForeColor = colors.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = colors.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = colors.FormBackground;

            _personCard.BackColor = colors.ContentBackground;
            _personCard.Invalidate();
            _pnlUserCard.Invalidate();
        }

        private void _pnlUserCard_Paint(object sender, PaintEventArgs e)
        {
            var colors = ThemeManager.Colors;
            using var pen = new Pen(colors.BorderColor, 1.5f);
            var rect = new Rectangle(0, 0, _pnlUserCard.Width - 1, _pnlUserCard.Height - 1);
            e.Graphics.DrawRectangle(pen, rect);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnHandleDestroyed(e);
        }
    }
}
