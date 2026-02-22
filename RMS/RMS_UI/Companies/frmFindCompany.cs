using System;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Companies
{
    public partial class frmFindCompany : Form
    {
        public event EventHandler<clsCompany>? CompanyFound;

        public frmFindCompany()
        {
            InitializeComponent();
        }

        private void frmFindCompany_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // Subscribe to CompanyFound event
            ctrlFindCompany1.CompanyFound += CtrlFindCompany_CompanyFound;

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void CtrlFindCompany_CompanyFound(object? sender, clsCompany company)
        {
            // When a company is found, update the form title
            this.Text = $"Find Company - Found: {company.CompanyName} (ID: {company.CompanyID})";

            CompanyFound?.Invoke(this, company);
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;
            BackColor = colors.FormBackground;
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            frmAddEditCompany frmAddEditCompany1 = new frmAddEditCompany();
            if (frmAddEditCompany1.ShowDialog() == DialogResult.OK && frmAddEditCompany1.SavedCompany != null)
            {
                ctrlFindCompany1.LoadCompany(frmAddEditCompany1.SavedCompany);
                CompanyFound?.Invoke(this, frmAddEditCompany1.SavedCompany);
            }
        }
    }
}
