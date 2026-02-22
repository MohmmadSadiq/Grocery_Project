using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Companies
{
    public partial class ctrlFindCompany : UserControl
    {
        // -----------------------------------------------------------------------
        // Event: fired only when a company is successfully found
        // -----------------------------------------------------------------------
        public event EventHandler<clsCompany>? CompanyFound;

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------
        public ctrlFindCompany()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // Populate search-type options
            _cmbSearchBy.Items.AddRange(new object[]
            {
                "By ID",
                "By Company Name",
                "By Commercial No",
                "By Phone",
                "By Email"
            });
            _cmbSearchBy.SelectedIndex = 0;

            // Keyboard shortcut: Enter triggers search
            _txtSearchValue.KeyDown += (sender, e) => _txtSearchValue_KeyDown(sender!, e);

            // Theme
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        public ctrlFindCompany(int companyID) : this()
        {
            _pnlTop.Enabled = false;
            _ctrlCompanyCard.LoadCompany(companyID);
        }

        // -----------------------------------------------------------------------
        // Search Logic
        // -----------------------------------------------------------------------
        private void _btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void _txtSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PerformSearch();
        }

        private void PerformSearch()
        {
            string searchValue = _txtSearchValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                _notification.ShowWarning("Please enter a search value.");
                return;
            }

            clsCompany? company = null;

            try
            {
                switch (_cmbSearchBy.SelectedIndex)
                {
                    case 0: // By ID
                        if (int.TryParse(searchValue, out int id))
                            company = clsCompany.Find(id);
                        else
                        {
                            _notification.ShowWarning("ID must be a valid number.");
                            return;
                        }
                        break;

                    case 1: // By Company Name
                        company = clsCompany.FindByCompanyName(searchValue);
                        break;

                    case 2: // By Commercial No
                        company = clsCompany.FindByCommercialNumber(searchValue);
                        break;

                    case 3: // By Phone
                        company = clsCompany.FindByPhone(searchValue);
                        break;

                    case 4: // By Email
                        company = clsCompany.FindByEmail(searchValue);
                        break;
                }
            }
            catch (Exception)
            {
                _notification.ShowError("An error occurred while searching.");
                return;
            }

            if (company != null)
            {
                _ctrlCompanyCard.LoadCompany(company);
                _notification.HideImmediately();
                CompanyFound?.Invoke(this, company);
            }
            else
            {
                _notification.ShowError($"No company found with the given {_cmbSearchBy.Text.Replace("By ", "")}.");
            }
        }

        public void LoadCompany(clsCompany company)
        {
            _ctrlCompanyCard.LoadCompany(company);
            _pnlTop.Enabled = false;
        }

        // -----------------------------------------------------------------------
        // Theme
        // -----------------------------------------------------------------------
        private void ApplyTheme()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;
            _pnlTop.BackColor = colors.ContentBackground;
            _lblTitle.ForeColor = colors.TitleText;

            _lblSearchBy.ForeColor = colors.SecondaryText;
            _cmbSearchBy.BackColor = colors.ContentBackground;
            _cmbSearchBy.ForeColor = colors.PrimaryText;

            _lblSearchValue.ForeColor = colors.SecondaryText;
            _txtSearchValue.BackColor = colors.ContentBackground;
            _txtSearchValue.ForeColor = colors.PrimaryText;
            _txtSearchValue.BorderStyle = BorderStyle.FixedSingle;

            _btnSearch.BackColor = colors.Primary;
            _btnSearch.ForeColor = Color.White;
            _btnSearch.FlatAppearance.BorderColor = colors.Primary;
            _btnSearch.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;

            Invalidate();
        }

        private void _pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
