using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Suppliers_Purchase
{
    public partial class ctrlAddEditSupplier: UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────────
        public event EventHandler<clsSupplier>? SupplierSaved;
        public event EventHandler? CancelClicked;

        // ── State ─────────────────────────────────────────────────────────────────
        private clsSupplier _supplier;

        // ── Mode ──────────────────────────────────────────────────────────────────
        enum enMode { AddNew = 1, Edit = 2 }
        private enMode Mode { get; set; }

        // ── Constructors ──────────────────────────────────────────────────────────

        /// <summary>Add New mode.</summary>
        public ctrlAddEditSupplier()
        {
            Mode = enMode.AddNew;
            _supplier = new clsSupplier();
            InitializeComponent();
            _InitControl();
        }

        /// <summary>Edit mode – loads existing supplier by ID.</summary>
        public ctrlAddEditSupplier(int supplierID)
        {
            _supplier = clsSupplier.Find(supplierID) ?? new clsSupplier();
            Mode = _supplier.SupplierID == -1 ? enMode.AddNew : enMode.Edit;
            InitializeComponent();
            _InitControl();
        }

        // ── Initialization ────────────────────────────────────────────────────────

        private void _InitControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // Configure business partners control
            _ctrlBusinessPartners.TogglingWithPartnerChanging = false;
            _ctrlBusinessPartners.Partner = null;

            if (Mode == enMode.Edit)
                _LoadSupplierData();
            else
            {
                _lblTitle.Text = "📦  Add New Supplier";
                _lblMode.Text = "Fill in the details below to register a new supplier.";
                _chkIsActive.Checked = true;
            }

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Load existing supplier into fields ────────────────────────────────────

        private void _LoadSupplierData()
        {
            _lblTitle.Text = "📦  Edit Supplier";
            _lblMode.Text = $"Editing supplier ID:  {_supplier.SupplierID}";
            _lblID.Text = $"ID: {_supplier.SupplierID}";

            _chkIsActive.Checked = _supplier.IsActive;

            if (_supplier.AccountID.HasValue)
                _txtAccountID.Text = _supplier.AccountID.ToString();

            // Set partner type based on supplier type
            switch (_supplier.SupplierType)
            {
                case clsSupplier.enSupplier.Person:
                    _rbPerson.Checked = true;
                    _ctrlBusinessPartners.LoadPerson(_supplier.PersonID);
                    break;
                case clsSupplier.enSupplier.Company:
                    _rbCompany.Checked = true;
                    _ctrlBusinessPartners.LoadCompany(_supplier.CompanyID);
                    break;
                default:
                    _rbPerson.Checked = false;
                    _rbCompany.Checked = false;
                    _ctrlBusinessPartners.Partner = null;
                    break;
            }
        }

        // ── Radio button handlers ─────────────────────────────────────────────────

        private void _rbPerson_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbPerson.Checked)
            {
                _ctrlBusinessPartners.Partner = ctrlBusinessPartners.enBusinessPartners.Person;
            }
        }

        private void _rbCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbCompany.Checked)
            {
                _ctrlBusinessPartners.Partner = ctrlBusinessPartners.enBusinessPartners.Company;
            }
        }

        // ── Validation ────────────────────────────────────────────────────────────

        private bool _ValidateInput()
        {
            bool isValid = true;
            _errorProvider.Clear();
            _notification.HideImmediately();

            // Must select a partner type
            if (!_rbPerson.Checked && !_rbCompany.Checked)
            {
                _errorProvider.SetError(_rbPerson, "Please select a partner type (Person or Company).");
                isValid = false;
            }

            // If Person is selected, verify a person is chosen
            if (_rbPerson.Checked && _ctrlBusinessPartners.Person == null)
            {
                _errorProvider.SetError(_rbPerson, "Please fill in the person information.");
                isValid = false;
            }

            // If Company is selected, verify a company is chosen
            if (_rbCompany.Checked && _ctrlBusinessPartners.Company == null)
            {
                _errorProvider.SetError(_rbCompany, "Please fill in the company information.");
                isValid = false;
            }

            if (!isValid)
                _notification.ShowWarning("Please fix the highlighted fields before saving.");

            return isValid;
        }

        // ── Button event handlers ─────────────────────────────────────────────────

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInput()) return;

            // Map fields → _supplier object
            if (_rbPerson.Checked)
            {
                _supplier.PersonID = _ctrlBusinessPartners.Person?.PersonID;
                _supplier.CompanyID = null;
            }
            else if (_rbCompany.Checked)
            {
                _supplier.CompanyID = _ctrlBusinessPartners.Company?.CompanyID;
                _supplier.PersonID = null;
            }

            _supplier.IsActive = _chkIsActive.Checked;

            if (int.TryParse(_txtAccountID.Text.Trim(), out int accountID) && accountID > 0)
                _supplier.AccountID = accountID;
            else
                _supplier.AccountID = null;

            int? currentUserID = clsGlobalUser.CurrentUser?.UserID;
            if (Mode == enMode.AddNew)
                _supplier.CreatedByUserID = currentUserID;
            else
                _supplier.UpdatedByUserID = currentUserID;

            // Persist
            bool saved = _supplier.Save();

            if (saved)
            {
                string msg = Mode == enMode.AddNew
                    ? "Supplier added successfully!"
                    : "Supplier updated successfully!";

                Mode = enMode.Edit;
                _LoadSupplierData();

                _notification.ShowSuccess(msg);
                SupplierSaved?.Invoke(this, _supplier);
            }
            else
            {
                _notification.ShowError("Failed to save the supplier. Please try again.");
            }
        }

        private void _btnFindAccount_Click(object sender, EventArgs e)
        {
            // Placeholder — implement account lookup when clsAccount is available
            _notification.ShowInfo("Account lookup is not yet available.");
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

            // ID label
            _lblID.ForeColor = c.SecondaryText;

            // Section headings
            _lblSectionPartner.ForeColor = c.Primary;
            _lblSectionStatus.ForeColor = c.Primary;
            _lblSectionAccount.ForeColor = c.Primary;

            // Separators
            _pnlSep1.BackColor = c.BorderColor;
            

            // Radio buttons
            _rbPerson.ForeColor = c.PrimaryText;
            _rbPerson.BackColor = c.ContentBackground;
            _rbCompany.ForeColor = c.PrimaryText;
            _rbCompany.BackColor = c.ContentBackground;

            // Checkbox
            _chkIsActive.ForeColor = c.PrimaryText;
            _chkIsActive.BackColor = c.ContentBackground;

            // Field labels
            _lblAccount.ForeColor = c.SecondaryText;

            // Read-only textbox
            _txtAccountID.BackColor = c.FormBackground;
            _txtAccountID.ForeColor = c.SecondaryText;
            _txtAccountID.BorderStyle = BorderStyle.FixedSingle;

            // Find Account button
            _btnFindAccount.BackColor = c.PrimaryLight;
            _btnFindAccount.ForeColor = c.Primary;
            _btnFindAccount.FlatAppearance.BorderColor = c.BorderAccent;
            _btnFindAccount.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

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

            // Business partners control
            _ctrlBusinessPartners.ApplyTheme();

            Invalidate();
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void _pnlCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
