using System;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class frmAddEditUser : Form
    {
        public clsUser? SavedUser { get; private set; }

        private readonly ctrlAddEditUser _ctrl;

        public frmAddEditUser()
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditUser();
            Bootstrap();
        }

        public frmAddEditUser(int userId)
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditUser(userId);
            Bootstrap();
        }

        private void Bootstrap()
        {
            _ctrl.Dock = DockStyle.Fill;
            Controls.Add(_ctrl);

            _ctrl.UserSaved += (_, user) =>
            {
                SavedUser = user;
                DialogResult = DialogResult.OK;
                Close();
            };

            _ctrl.CancelClicked += (_, _) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            ThemeManager.ThemeChanged += OnThemeChanged;
            Load += (_, _) => ApplyTheme();
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

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Colors.FormBackground;
            _ctrl.ApplyTheme();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
    }
}
