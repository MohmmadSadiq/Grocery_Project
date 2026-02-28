using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Payment
{
    public partial class ctrlAddEditPayment : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────
        public event EventHandler<clsPayment>? PaymentSaved;
        public event EventHandler? CancelClicked;

        // ── State ─────────────────────────────────────────────────────────────
        private clsPayment _payment;
        private decimal _maxAllowedAmount;

        enum enMode { AddNew = 1, Edit = 2 }
        private enMode Mode { get; set; }

        // ── Constructors ──────────────────────────────────────────────────────

        /// <summary>Add New mode. maxAllowedAmount limits the payment amount.</summary>
        public ctrlAddEditPayment(decimal maxAllowedAmount)
        {
            Mode = enMode.AddNew;
            _payment = new clsPayment();
            _maxAllowedAmount = maxAllowedAmount;
            InitializeComponent();
            _InitControl();
        }

        /// <summary>Edit mode – loads existing payment.</summary>
        public ctrlAddEditPayment(int paymentID, decimal maxAllowedAmount)
        {
            _payment = clsPayment.Find(paymentID) ?? new clsPayment();
            Mode = _payment.PaymentID == -1 ? enMode.AddNew : enMode.Edit;
            // In edit mode, add back the current payment's amount to the allowed max
            _maxAllowedAmount = Mode == enMode.Edit
                ? maxAllowedAmount + _payment.PaymentAmount
                : maxAllowedAmount;
            InitializeComponent();
            _InitControl();
        }

        // ── Init ──────────────────────────────────────────────────────────────

        private void _InitControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _PopulatePaymentMethods();

            // Allow any value — overpayment handled on save
            _numPaymentAmount.Maximum = 999999999;
            _numPaymentAmount.Value = _numPaymentAmount.Minimum;
            _lblMaxAmountValue.Text = _maxAllowedAmount.ToString("N2");

            if (Mode == enMode.Edit)
                _LoadPaymentData();
            else
            {
                _lblTitle.Text = "💳  Add Payment";
                _lblMode.Text = "Enter payment details below.";
                _btnSave.Text = "💾  Save";
                _dtpPaymentDate.Value = DateTime.Now;
            }

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Populate ──────────────────────────────────────────────────────────

        private void _PopulatePaymentMethods()
        {
            try
            {
                DataTable dt = clsPaymentMethod.GetAllPaymentMethod();
                _cmbPaymentMethod.Items.Clear();
                _cmbPaymentMethod.Items.Add(new ComboBoxItem("-- Select Method --", -1));

                foreach (DataRow row in dt.Rows)
                {
                    // Filter: only methods active for purchases
                    bool isActiveForPurchases = row.Table.Columns.Contains("IsActiveForPurchases")
                        && row["IsActiveForPurchases"] != DBNull.Value
                        && Convert.ToBoolean(row["IsActiveForPurchases"]);

                    if (!isActiveForPurchases) continue;

                    int id = Convert.ToInt32(row["PaymentMethodID"]);
                    string name = row["MethodName"]?.ToString() ?? $"Method #{id}";
                    _cmbPaymentMethod.Items.Add(new ComboBoxItem(name, id));
                }

                _cmbPaymentMethod.DisplayMember = "Text";
                _cmbPaymentMethod.SelectedIndex = 0;
            }
            catch { /* unavailable */ }
        }

        // ── Load (Edit mode) ──────────────────────────────────────────────────

        private void _LoadPaymentData()
        {
            _lblTitle.Text = "💳  Edit Payment";
            _lblMode.Text = $"Editing payment ID: {_payment.PaymentID}";
            _btnSave.Text = "💾  Update";

            _SelectComboByValue(_cmbPaymentMethod, _payment.PaymentMethodID);
            _numPaymentAmount.Value = _payment.PaymentAmount;
            _dtpPaymentDate.Value = _payment.PaymentDate != DateTime.MinValue
                ? _payment.PaymentDate : DateTime.Now;
            _txtNotes.Text = _payment.Notes ?? "";
        }

        // ── Validation ────────────────────────────────────────────────────────

        private bool _ValidateInput()
        {
            _errorProvider.Clear();
            _notification.HideImmediately();
            bool valid = true;

            // Payment method required
            var selected = _cmbPaymentMethod.SelectedItem as ComboBoxItem;
            if (selected == null || (selected.Value is int v && v == -1))
            {
                _errorProvider.SetError(_cmbPaymentMethod, "Please select a payment method.");
                valid = false;
            }

            // Amount > 0
            if (_numPaymentAmount.Value <= 0)
            {
                _errorProvider.SetError(_numPaymentAmount, "Amount must be greater than zero.");
                valid = false;
            }

            if (!valid)
                _notification.ShowWarning("Please fix the highlighted fields.");

            return valid;
        }

        // ── Save ──────────────────────────────────────────────────────────────

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInput()) return;

            // ── Check overpayment ────────────────────────────────────────
            if (_numPaymentAmount.Value > _maxAllowedAmount)
            {
                var result = MessageBox.Show(
                    $"The entered amount ({_numPaymentAmount.Value:N2}) exceeds the maximum allowed ({_maxAllowedAmount:N2}).\n\n" +
                    $"Would you like to adjust the amount to the remaining balance ({_maxAllowedAmount:N2})?",
                    "Amount Exceeds Limit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    _numPaymentAmount.Value = _maxAllowedAmount;
                }
                else
                {
                    return; // cancel the save
                }
            }

            // Map fields
            _payment.PaymentDate = _dtpPaymentDate.Value;
            _payment.PaymentAmount = _numPaymentAmount.Value;
            _payment.Notes = string.IsNullOrWhiteSpace(_txtNotes.Text) ? null : _txtNotes.Text.Trim();

            if (_cmbPaymentMethod.SelectedItem is ComboBoxItem mi && mi.Value is int methodId && methodId > 0)
                _payment.PaymentMethodID = methodId;

            int? currentUserID = clsGlobalUser.CurrentUser?.UserID;
            if (Mode == enMode.AddNew)
                _payment.CreatedByUserID = currentUserID;
            else
                _payment.UpdatedByUserID = currentUserID;

            // Don't persist to DB here — the caller (ctrlPaymentPanel) handles that.
            // Just fire the event with the payment object.
            _notification.ShowSuccess("Payment ready.");
            PaymentSaved?.Invoke(this, _payment);
        }

        private void _btnCancel_Click(object sender, EventArgs e)
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
            _lblTitle.ForeColor = c.TitleText;
            _lblMode.ForeColor = c.SecondaryText;

            _btnSave.BackColor = c.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = c.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnCancel.BackColor = c.FormBackground;
            _btnCancel.ForeColor = c.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = c.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = c.ButtonHover;

            _pnlContent.BackColor = c.ContentBackground;
            _lblSectionPayment.ForeColor = c.Primary;
            _pnlSep1.BackColor = c.BorderColor;

            foreach (var lbl in new[] { _lblPaymentMethod, _lblPaymentAmount, _lblPaymentDate, _lblNotes, _lblMaxAmountLabel })
                lbl.ForeColor = c.SecondaryText;

            _cmbPaymentMethod.BackColor = c.ContentBackground;
            _cmbPaymentMethod.ForeColor = c.PrimaryText;
            _numPaymentAmount.BackColor = c.ContentBackground;
            _numPaymentAmount.ForeColor = c.PrimaryText;
            _txtNotes.BackColor = c.ContentBackground;
            _txtNotes.ForeColor = c.PrimaryText;

            _pnlAmountInfo.BackColor = Color.FromArgb(240, 253, 244);

            Invalidate();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void _SelectComboByValue(ComboBox cmb, int value)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i] is ComboBoxItem item && item.Value is int v && v == value)
                {
                    cmb.SelectedIndex = i;
                    return;
                }
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; }
            public object Value { get; }
            public ComboBoxItem(string text, object value) { Text = text; Value = value; }
            public override string ToString() => Text;
        }
    }
}
