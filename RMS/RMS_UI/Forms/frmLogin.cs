using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            Bootstrap();
        }

        private void Bootstrap()
        {
            ThemeManager.ThemeChanged += OnThemeChanged;

            btnLogin.Click += (_, _) => TryLogin();
            btnCancel.Click += (_, _) => CancelLogin();
            chkShowPassword.CheckedChanged += (_, _) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

            txtUserName.KeyDown += HandleEnterKey;
            txtPassword.KeyDown += HandleEnterKey;

            LoadRememberedCredentials();
            ApplyTheme();
        }

        private void HandleEnterKey(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TryLogin();
            }
        }

        private void TryLogin()
        {
            HideError();

            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Enter username and password.");
                return;
            }

            clsUser? user = null;

            try
            {
                user = clsUser.FindByUserName(userName);
            }
            catch
            {
                ShowError("Login service is temporarily unavailable.");
                return;
            }

            if (user == null)
            {
                ShowError("Invalid username or password.");
                return;
            }

            if (!user.IsActive)
            {
                ShowError("Your account is inactive. Contact administrator.");
                return;
            }

            if (!PasswordSecurity.Verify(password, user.PasswordHash, user.PasswordSalt))
            {
                ShowError("Invalid username or password.");
                return;
            }

            clsGlobalUser.CurrentUser = user;
            LoginCredentialStore.Save(userName, password, chkRememberMe.Checked);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelLogin()
        {
            clsGlobalUser.Logout();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LoadRememberedCredentials()
        {
            if (!LoginCredentialStore.TryLoad(out string userName, out string password, out bool rememberMe))
            {
                txtUserName.Focus();
                return;
            }

            chkRememberMe.Checked = rememberMe;

            if (!rememberMe)
            {
                txtUserName.Focus();
                return;
            }

            txtUserName.Text = userName;
            txtPassword.Text = password;

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                btnLogin.Focus();
            }
            else
            {
                txtPassword.Focus();
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void HideError()
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            ApplyTheme();
            panelBranding.Invalidate();
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;
            panelShell.BackColor = colors.FormBackground;
            panelCardHost.BackColor = colors.FormBackground;

            // Keep a solid fallback so white branding text is always readable.
            panelBranding.BackColor = colors.Primary;

            panelCard.BackColor = colors.ContentBackground;
            panelCardBorder.BackColor = colors.BorderColor;

            lblWelcome.ForeColor = colors.TitleText;
            lblSubTitle.ForeColor = colors.SecondaryText;
            lblUserName.ForeColor = colors.PrimaryText;
            lblPassword.ForeColor = colors.PrimaryText;
            chkRememberMe.ForeColor = colors.PrimaryText;
            chkShowPassword.ForeColor = colors.SecondaryText;

            txtUserName.BackColor = colors.TitleBarBackground;
            txtUserName.ForeColor = colors.PrimaryText;
            txtPassword.BackColor = colors.TitleBarBackground;
            txtPassword.ForeColor = colors.PrimaryText;

            btnLogin.BackColor = colors.Primary;
            btnLogin.FlatAppearance.BorderColor = colors.Primary;
            btnLogin.ForeColor = Color.White;

            btnCancel.BackColor = colors.TitleBarBackground;
            btnCancel.FlatAppearance.BorderColor = colors.BorderColor;
            btnCancel.ForeColor = colors.PrimaryText;

            lblError.ForeColor = Color.FromArgb(220, 38, 38);

            lblBrandName.ForeColor = Color.White;
            lblBrandTagline.ForeColor = Color.FromArgb(235, 245, 255);
            lblCapabilityTitle.ForeColor = Color.White;
            lblCapabilityBody.ForeColor = Color.FromArgb(219, 234, 254);

            lblBrandName.BackColor = Color.Transparent;
            lblBrandTagline.BackColor = Color.Transparent;
            lblCapabilityTitle.BackColor = Color.Transparent;
            lblCapabilityBody.BackColor = Color.Transparent;

            // Force repaint to apply the gradient immediately after theme changes.
            panelBranding.Invalidate();
        }

        private void panelBranding_Paint(object? sender, PaintEventArgs e)
        {
            var colors = ThemeManager.Colors;
            Rectangle rect = panelBranding.ClientRectangle;

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            using LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                colors.Primary,
                colors.PrimaryHover,
                130f);

            e.Graphics.FillRectangle(brush, rect);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using SolidBrush bubbleBrush = new SolidBrush(Color.FromArgb(35, 255, 255, 255));
            e.Graphics.FillEllipse(bubbleBrush, rect.Width - 220, -80, 280, 280);
            e.Graphics.FillEllipse(bubbleBrush, -120, rect.Height - 220, 260, 260);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
    }
}
