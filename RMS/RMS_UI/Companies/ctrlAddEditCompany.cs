using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Peoples;
using RMS_UI.Utilities;

namespace RMS_UI.Companies
{
    public partial class ctrlAddEditCompany : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────────
        public event EventHandler<clsCompany>? CompanySaved;
        public event EventHandler? CancelClicked;

        // ── State ─────────────────────────────────────────────────────────────────
        private clsCompany _company;
        private int? _selectedPersonID;

        // ── Mode ─────────────────────────────────────────────────────────────────
        enum enMode { AddNew = 1, Edit = 2 }

        private enMode Mode { get; set; }

        // ── Constructors ──────────────────────────────────────────────────────────

        /// <summary>Add New mode.</summary>
        public ctrlAddEditCompany()
        {
            Mode = enMode.AddNew;
            _company = new clsCompany();
            InitializeComponent();
            _InitControl();
        }

        /// <summary>Edit mode – loads existing company by ID.</summary>
        public ctrlAddEditCompany(int companyID)
        {

            _company = clsCompany.Find(companyID) ?? new clsCompany();
            Mode = _company.CompanyID == -1 ? enMode.AddNew : enMode.Edit;
            InitializeComponent();
            _InitControl();
        }

        // ── Initialization ────────────────────────────────────────────────────────

        private void _InitControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _PopulateCountries();

            if (this.Mode == enMode.Edit)
                _LoadCompanyData();
            else
            {
                _lblTitle.Text = "🏢  Add New Company";
                _lblMode.Text = "Fill in the details below to register a new company.";
            }

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Load existing company into fields ─────────────────────────────────────

        private void _LoadCompanyData()
        {
            _lblTitle.Text = "🏢  Edit Company";
            _lblMode.Text = $"Editing:  {_company.CompanyName}";
            _lblID.Text = $"ID: {_company.CompanyID}";

            _txtCompanyName.Text = _company.CompanyName;
            _txtCommercialNumber.Text = _company.CommercialNumber ?? string.Empty;
            _txtPhone.Text = _company.Phone ?? string.Empty;
            _txtEmail.Text = _company.Email ?? string.Empty;
            _txtAddress.Text = _company.Address ?? string.Empty;

            if (_company.ContactPersonID.HasValue)
            {
                _selectedPersonID = _company.ContactPersonID;
                _txtContactPersonID.Text = _company.ContactPersonID.ToString();
                _txtContactPersonName.Text = _company.ContactPerson?.FullName ?? string.Empty;
            }

            if (_company.CountryID.HasValue)
            {
                foreach (CountryItem item in _cmbCountry.Items)
                {
                    if (item.ID == _company.CountryID.Value)
                    {
                        _cmbCountry.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // ── Populate country ComboBox ─────────────────────────────────────────────

        private void _PopulateCountries()
        {
            _cmbCountry.Items.Clear();
            _cmbCountry.Items.Add(new CountryItem(0, "— Select Country —"));

            try
            {
                DataTable dt = clsCountry.GetAllCountries();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        _cmbCountry.Items.Add(new CountryItem(
                            Convert.ToInt32(row["CountryID"]),
                            row["CountryName"].ToString() ?? string.Empty));
                    }
                }
            }
            catch { /* silently skip if DB unavailable at design-time */ }

            _cmbCountry.SelectedIndex = 0;
        }

        // ── Validation ────────────────────────────────────────────────────────────

        private bool _ValidateInput()
        {
            bool isValid = true;
            _errorProvider.Clear();
            _notification.HideImmediately();

            // Company Name – required
            if (string.IsNullOrWhiteSpace(_txtCompanyName.Text))
            {
                _errorProvider.SetError(_txtCompanyName, "Company name is required.");
                isValid = false;
            }
            else if (_txtCompanyName.Text.Trim().Length > 100)
            {
                _errorProvider.SetError(_txtCompanyName, "Company name must not exceed 100 characters.");
                isValid = false;
            }

            // Email – optional but must be valid format if provided
            if (!string.IsNullOrWhiteSpace(_txtEmail.Text))
            {
                if (!Regex.IsMatch(_txtEmail.Text.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                {
                    _errorProvider.SetError(_txtEmail, "Please enter a valid email address.");
                    isValid = false;
                }
            }

            // Phone – optional but digits/spaces/+-()/. only
            if (!string.IsNullOrWhiteSpace(_txtPhone.Text))
            {
                if (!Regex.IsMatch(_txtPhone.Text.Trim(), @"^[\d\s\+\-\(\)\.]+$"))
                {
                    _errorProvider.SetError(_txtPhone, "Phone may only contain digits, spaces and + - ( ).");
                    isValid = false;
                }
            }

            if (!isValid)
                _notification.ShowWarning("Please fix the highlighted fields before saving.");

            return isValid;
        }

        // ── Button event handlers ─────────────────────────────────────────────────

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInput()) return;

            // Map fields → _company object
            _company.CompanyName = _txtCompanyName.Text.Trim();
            _company.CommercialNumber = _NullIfEmpty(_txtCommercialNumber.Text);
            _company.Phone = _NullIfEmpty(_txtPhone.Text);
            _company.Email = _NullIfEmpty(_txtEmail.Text);
            _company.Address = _NullIfEmpty(_txtAddress.Text);
            _company.ContactPersonID = _selectedPersonID;

            var selectedCountry = _cmbCountry.SelectedItem as CountryItem;
            _company.CountryID = (selectedCountry != null && selectedCountry.ID > 0)
                ? selectedCountry.ID
                : (int?)null;

            int? currentUserID = clsGlobalUser.CurrentUser?.UserID;
            if (this.Mode == enMode.AddNew)
                _company.CreatedByUserID = currentUserID;
            else
                _company.UpdatedByUserID = currentUserID;

            // Persist
            bool saved = _company.Save();

            if (saved)
            {
                string msg = this.Mode == enMode.AddNew
                    ? "Company added successfully!"
                    : "Company updated successfully!";

                this.Mode = enMode.Edit;

                if (this.Mode == enMode.Edit)
                    _LoadCompanyData();
                else
                {
                    _lblTitle.Text = "🏢  Add New Company";
                    _lblMode.Text = "Fill in the details below to register a new company.";
                }

                _notification.ShowSuccess(msg);
                _lblMode.Text = $"Editing:  {_company.CompanyName}";
                CompanySaved?.Invoke(this, _company);
            }
            else
            {
                _notification.ShowError("Failed to save the company. Please try again.");
            }
        }

        private void _btnFindPerson_Click(object sender, EventArgs e)
        {
            using var frm = new frmFindPerson();

            frm.PersonFound += (s, person) =>
            {
                _selectedPersonID = person.PersonID;
                _txtContactPersonID.Text = person.PersonID.ToString();
                _txtContactPersonName.Text = person.FullName;
                MessageBox.Show("Person Added Successfully");

            };

            frm.ShowDialog(this);
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        // ── Theme ─────────────────────────────────────────────────────────────────

        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;

            // Containers
            BackColor = c.FormBackground;
            _pnlHeader.BackColor = c.ContentBackground;
            _pnlContent.BackColor = c.FormBackground;
            _pnlCard.BackColor = c.ContentBackground;
            _pnlButtons.BackColor = c.ContentBackground;

            // Header text
            _lblTitle.ForeColor = c.TitleText;
            _lblMode.ForeColor = c.SecondaryText;

            // Section headings
            _lblSectionBasic.ForeColor = c.Primary;
            _lblSectionContact.ForeColor = c.Primary;
            _lblSectionLocation.ForeColor = c.Primary;

            // Separators
            _pnlSep1.BackColor = c.BorderColor;
            _pnlSep2.BackColor = c.BorderColor;
            _pnlSep3.BackColor = c.BorderColor;

            // Field labels
            foreach (Label lbl in new[] {
                _lblCompanyName, _lblCommercialNumber,
                _lblPhone, _lblEmail, _lblContactPerson,
                _lblCountry, _lblAddress })
            {
                lbl.ForeColor = c.SecondaryText;
            }

            // Editable textboxes
            foreach (TextBox txt in new[] {
                _txtCompanyName, _txtCommercialNumber,
                _txtPhone, _txtEmail, _txtAddress })
            {
                txt.BackColor = c.ContentBackground;
                txt.ForeColor = c.PrimaryText;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // Read-only person textboxes
            foreach (TextBox txt in new[] { _txtContactPersonID, _txtContactPersonName })
            {
                txt.BackColor = c.FormBackground;
                txt.ForeColor = c.SecondaryText;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // Country combo
            _cmbCountry.BackColor = c.ContentBackground;
            _cmbCountry.ForeColor = c.PrimaryText;

            // Find-person button
            _btnFindPerson.BackColor = c.PrimaryLight;
            _btnFindPerson.ForeColor = c.Primary;
            _btnFindPerson.FlatAppearance.BorderColor = c.BorderAccent;
            _btnFindPerson.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Save button
            _btnSave.BackColor = c.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = c.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Cancel button
            _btnCancel.BackColor = c.FormBackground;
            _btnCancel.ForeColor = c.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = c.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = c.ButtonHover;

            Invalidate();
        }

        // ── Override OnPaint – subtle card shadow ─────────────────────────────────

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string? _NullIfEmpty(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        // Lightweight wrapper so CountryID stays with the display name in the ComboBox
        private sealed class CountryItem
        {
            public int ID { get; }
            public string Name { get; }
            public CountryItem(int id, string name) { ID = id; Name = name; }
            public override string ToString() => Name;
        }

        private void _pnlCard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _selectedPersonID = null;
            _company.ContactPersonID = null;
            _txtContactPersonID.Text = string.Empty;
            _txtContactPersonName.Text = string.Empty;
        }
    }
}
