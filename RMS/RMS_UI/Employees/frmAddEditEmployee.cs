using System;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class frmAddEditEmployee : Form
    {
        public clsEmployee? SavedEmployee { get; private set; }

        private readonly ctrlAddEditEmployee _ctrl;

        public frmAddEditEmployee()
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditEmployee();
            Bootstrap();
        }

        public frmAddEditEmployee(int employeeId)
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditEmployee(employeeId);
            Bootstrap();
        }

        private void Bootstrap()
        {
            _ctrl.Dock = DockStyle.Fill;
            Controls.Add(_ctrl);

            _ctrl.EmployeeSaved += (_, employee) =>
            {
                SavedEmployee = employee;
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
