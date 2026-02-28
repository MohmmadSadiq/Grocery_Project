using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.PaymentMethods
{
    public partial class ctrlAddEditPaymentMethod : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────
        public event EventHandler<clsPaymentMethod>? PaymentMethodSaved;
        public event EventHandler? CancelClicked;

        // ── State ─────────────────────────────────────────────────────────────
        private clsPaymentMethod _paymentMethod;

        // ── Mode ──────────────────────────────────────────────────────────────
        enum enMode { AddNew = 1, Edit = 2 }
        private enMode Mode { get; set; }

        // ── Constructors ──────────────────────────────────────────────────────

        /// <summary>Add New mode.</summary>
        public ctrlAddEditPaymentMethod()
        {
            Mode = enMode.AddNew;
            _paymentMethod = new clsPaymentMethod();
            InitializeComponent();
            _InitControl();
        }

        /// <summary>Edit mode – loads existing payment method by ID.</summary>
        public ctrlAddEditPaymentMethod(int paymentMethodID)
        {
            _paymentMethod = clsPaymentMethod.Find(paymentMethodID) ?? new clsPaymentMethod();
            Mode = _paymentMethod.PaymentMethodID == -1 ? enMode.AddNew : enMode.Edit;
            InitializeComponent();
            _InitControl();
        }

        // ── Initialization ────────────────────────────────────────────────────

        private void _InitControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            if (Mode == enMode.Edit)
                _LoadPaymentMethodData();
            else
            {
                _lblTitle.Text = "💳  Add Payment Method";
                _lblMode.Text = "Fill in the details below to add a new payment method.";
            }

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Load existing data into fields ────────────────────────────────────

        private void _LoadPaymentMethodData()
        {
            _lblTitle.Text = "💳  Edit Payment Method";
            _lblMode.Text = $"Editing:  {_paymentMethod.MethodName}";

            _txtMethodName.Text = _paymentMethod.MethodName;
            _txtDescription.Text = _paymentMethod.Description ?? string.Empty;
            _chkIsActiveForSales.Checked = _paymentMethod.IsActiveForSales;
            _chkIsActiveForPurchases.Checked = _paymentMethod.IsActiveForPurchases;
        }

        // ── Validation ────────────────────────────────────────────────────────

        private bool _ValidateInput()
        {
            bool isValid = true;
            _errorProvider.Clear();
            _notification.HideImmediately();

            if (string.IsNullOrWhiteSpace(_txtMethodName.Text))
            {
                _errorProvider.SetError(_txtMethodName, "Method name is required.");
                isValid = false;
            }
            else if (_txtMethodName.Text.Trim().Length > 100)
            {
                _errorProvider.SetError(_txtMethodName, "Method name must not exceed 100 characters.");
                isValid = false;
            }

            if (!isValid)
                _notification.ShowWarning("Please fix the highlighted fields before saving.");

            return isValid;
        }

        // ── Button event handlers ─────────────────────────────────────────────

        private void _btnSave_Click(object? sender, EventArgs e)
        {
            if (!_ValidateInput()) return;

            _paymentMethod.MethodName = _txtMethodName.Text.Trim();
            _paymentMethod.Description = string.IsNullOrWhiteSpace(_txtDescription.Text)
                ? null
                : _txtDescription.Text.Trim();
            _paymentMethod.IsActiveForSales = _chkIsActiveForSales.Checked;
            _paymentMethod.IsActiveForPurchases = _chkIsActiveForPurchases.Checked;

            bool saved = _paymentMethod.Save();

            if (saved)
            {
                string msg = Mode == enMode.AddNew
                    ? "Payment method added successfully!"
                    : "Payment method updated successfully!";

                Mode = enMode.Edit;
                _LoadPaymentMethodData();
                _notification.ShowSuccess(msg);
                PaymentMethodSaved?.Invoke(this, _paymentMethod);
            }
            else
            {
                _notification.ShowError("Failed to save the payment method. Please try again.");
            }
        }

        private void _btnCancel_Click(object? sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        // ── Theme ─────────────────────────────────────────────────────────────

        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;
            _pnlHeader.BackColor = c.ContentBackground;
            _pnlContent.BackColor = c.FormBackground;
            _pnlCard.BackColor = c.ContentBackground;
            _pnlButtons.BackColor = c.ContentBackground;

            _lblTitle.ForeColor = c.TitleText;
            _lblMode.ForeColor = c.SecondaryText;

            _pnlSep1.BackColor = c.BorderColor;

            _lblMethodName.ForeColor = c.SecondaryText;
            _lblDescription.ForeColor = c.SecondaryText;

            _txtMethodName.BackColor = c.ContentBackground;
            _txtMethodName.ForeColor = c.PrimaryText;
            _txtMethodName.BorderStyle = BorderStyle.FixedSingle;

            _txtDescription.BackColor = c.ContentBackground;
            _txtDescription.ForeColor = c.PrimaryText;
            _txtDescription.BorderStyle = BorderStyle.FixedSingle;

            _chkIsActiveForSales.ForeColor = c.PrimaryText;
            _chkIsActiveForPurchases.ForeColor = c.PrimaryText;

            _btnSave.BackColor = c.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = c.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnCancel.BackColor = c.FormBackground;
            _btnCancel.ForeColor = c.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = c.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = c.ButtonHover;

            Invalidate();
        }
    }
}
