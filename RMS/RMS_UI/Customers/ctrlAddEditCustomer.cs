using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Suppliers_Purchase;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class ctrlAddEditCustomer : UserControl
    {
        public event EventHandler<clsCustomer>? CustomerSaved;
        public event EventHandler? CancelClicked;

        private enum enMode { AddNew = 1, Edit = 2 }

        private clsCustomer _customer;
        private enMode _mode;

        public ctrlAddEditCustomer()
        {
            _mode = enMode.AddNew;
            _customer = new clsCustomer();
            InitializeComponent();
            InitializeControl();
        }

        public ctrlAddEditCustomer(int customerID)
        {
            _customer = clsCustomer.Find(customerID) ?? new clsCustomer();
            _mode = _customer.CustomerID > 0 ? enMode.Edit : enMode.AddNew;
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _ctrlBusinessPartners.TogglingWithPartnerChanging = false;
            _ctrlBusinessPartners.Partner = null;

            if (_mode == enMode.Edit)
                LoadCustomerData();
            else
            {
                _lblTitle.Text = "👥  Add New Customer";
                _lblMode.Text = "Fill in the details below to register a new customer.";
                _chkIsActive.Checked = true;
            }

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void LoadCustomerData()
        {
            _lblTitle.Text = "👥  Edit Customer";
            _lblMode.Text = $"Editing customer ID: {_customer.CustomerID}";
            _lblID.Text = $"ID: {_customer.CustomerID}";

            _txtAccountID.Text = _customer.AccountID?.ToString() ?? string.Empty;
            _chkIsActive.Checked = _customer.IsActive;

            switch (_customer.CustomerType)
            {
                case clsCustomer.enCustomer.Person:
                    _rbPerson.Checked = true;
                    _ctrlBusinessPartners.LoadPerson(_customer.PersonID);
                    break;
                case clsCustomer.enCustomer.Company:
                    _rbCompany.Checked = true;
                    _ctrlBusinessPartners.LoadCompany(_customer.CompanyID);
                    break;
                default:
                    _rbPerson.Checked = false;
                    _rbCompany.Checked = false;
                    _ctrlBusinessPartners.Partner = null;
                    break;
            }
        }

        private bool ValidateInput()
        {
            _errorProvider.Clear();
            _notification.HideImmediately();

            bool isValid = true;

            if (!_rbPerson.Checked && !_rbCompany.Checked)
            {
                _errorProvider.SetError(_rbPerson, "Please select a partner type (Person or Company).");
                isValid = false;
            }

            if (_rbPerson.Checked && _ctrlBusinessPartners.Person == null)
            {
                _errorProvider.SetError(_rbPerson, "Please fill in the person information.");
                isValid = false;
            }

            if (_rbCompany.Checked && _ctrlBusinessPartners.Company == null)
            {
                _errorProvider.SetError(_rbCompany, "Please fill in the company information.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(_txtAccountID.Text.Trim())
                && !(int.TryParse(_txtAccountID.Text.Trim(), out int accountId) && accountId > 0))
            {
                _errorProvider.SetError(_txtAccountID, "AccountID must be a positive number.");
                isValid = false;
            }

            if (!isValid)
                _notification.ShowWarning("Please fix the highlighted fields before saving.");

            return isValid;
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            bool wasAddNew = _mode == enMode.AddNew;

            if (_rbPerson.Checked)
            {
                _customer.PersonID = _ctrlBusinessPartners.Person?.PersonID;
                _customer.CompanyID = null;
            }
            else if (_rbCompany.Checked)
            {
                _customer.CompanyID = _ctrlBusinessPartners.Company?.CompanyID;
                _customer.PersonID = null;
            }

            _customer.AccountID = TryGetPositiveInt(_txtAccountID.Text);
            _customer.IsActive = _chkIsActive.Checked;

            int? currentUserId = clsGlobalUser.CurrentUser?.UserID;
            if (_mode == enMode.AddNew)
                _customer.CreatedByUserID = currentUserId;
            else
                _customer.UpdatedByUserID = currentUserId;

            bool saved = _customer.Save();
            if (!saved)
            {
                _notification.ShowError("Failed to save customer. Please try again.");
                return;
            }

            _mode = enMode.Edit;
            _customer = clsCustomer.Find(_customer.CustomerID) ?? _customer;
            LoadCustomerData();
            _notification.ShowSuccess(wasAddNew
                ? "Customer added successfully!"
                : "Customer updated successfully!");
            CustomerSaved?.Invoke(this, _customer);
        }

        private static int? TryGetPositiveInt(string? text)
        {
            if (int.TryParse((text ?? string.Empty).Trim(), out int value) && value > 0)
                return value;
            return null;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        private void _rbPerson_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbPerson.Checked)
                _ctrlBusinessPartners.Partner = ctrlBusinessPartners.enBusinessPartners.Person;
        }

        private void _rbCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbCompany.Checked)
                _ctrlBusinessPartners.Partner = ctrlBusinessPartners.enBusinessPartners.Company;
        }

        private void _btnFindAccount_Click(object sender, EventArgs e)
        {
            _notification.ShowInfo("Account lookup is not yet available.");
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
            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;
            _pnlHeader.BackColor = c.ContentBackground;
            _pnlContent.BackColor = c.FormBackground;
            _pnlCard.BackColor = c.ContentBackground;
            _pnlButtons.BackColor = c.ContentBackground;

            _lblTitle.ForeColor = c.TitleText;
            _lblMode.ForeColor = c.SecondaryText;
            _lblID.ForeColor = c.SecondaryText;

            _lblSectionPartner.ForeColor = c.Primary;
            _lblSectionStatus.ForeColor = c.Primary;
            _lblSectionAccount.ForeColor = c.Primary;
            _pnlSep1.BackColor = c.BorderColor;

            _rbPerson.ForeColor = c.PrimaryText;
            _rbPerson.BackColor = c.ContentBackground;
            _rbCompany.ForeColor = c.PrimaryText;
            _rbCompany.BackColor = c.ContentBackground;

            _lblAccountID.ForeColor = c.SecondaryText;
            _chkIsActive.ForeColor = c.PrimaryText;
            _chkIsActive.BackColor = c.ContentBackground;

            _txtAccountID.BackColor = c.ContentBackground;
            _txtAccountID.ForeColor = c.PrimaryText;

            _btnFindAccount.BackColor = c.PrimaryLight;
            _btnFindAccount.ForeColor = c.Primary;
            _btnFindAccount.FlatAppearance.BorderColor = c.BorderAccent;
            _btnFindAccount.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnSave.BackColor = c.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnCancel.BackColor = c.FormBackground;
            _btnCancel.ForeColor = c.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = c.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = c.ButtonHover;

            _ctrlBusinessPartners.ApplyTheme();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnHandleDestroyed(e);
        }
    }
}
