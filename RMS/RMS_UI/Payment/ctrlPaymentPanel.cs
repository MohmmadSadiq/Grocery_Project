using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Payment
{
    /// <summary>
    /// Reusable payment list panel. Shows a grid of payments with Add / Edit / Delete
    /// buttons and a Total / Paid / Remaining summary.
    /// Supports two modes:
    ///   • Pending mode  – payments are held in memory until the parent saves.
    ///   • Persisted mode – payments are saved/loaded from the database.
    /// </summary>
    public partial class ctrlPaymentPanel : UserControl
    {
        // ── Public properties ─────────────────────────────────────────────────
        /// <summary>The TransactionID to link payments to. Set to -1 while still in Add-New (pending) mode.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TransactionID { get; set; } = -1;

        /// <summary>The invoice total amount — used to cap how much can be paid.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal TotalAmount
        {
            get => _totalAmount;
            set { _totalAmount = value; RefreshSummary(); }
        }
        private decimal _totalAmount;

        /// <summary>When true, payments are kept in-memory only (not persisted).
        /// The parent form should call <see cref="GetPendingPayments"/> after saving the purchase.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPendingMode { get; set; } = true;

        // ── Internal state ────────────────────────────────────────────────────
        private List<clsPayment> _pendingPayments = new List<clsPayment>();
        private List<PaymentRow> _displayRows = new List<PaymentRow>();

        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>Raised whenever the paid amount changes (pending or real).</summary>
        public event EventHandler? PaidAmountChanged;

        // ── Constructor ───────────────────────────────────────────────────────
        public ctrlPaymentPanel()
        {
            InitializeComponent();
            _SetupGridColumns();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Grid columns ──────────────────────────────────────────────────────
        private void _SetupGridColumns()
        {
            _dgvPayments.Columns.Clear();
            _dgvPayments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNo",     HeaderText = "#",       Width = 40 });
            _dgvPayments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate",    HeaderText = "Date",    Width = 110 });
            _dgvPayments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMethod",  HeaderText = "Method",  Width = 140 });
            _dgvPayments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAmount",  HeaderText = "Amount",  Width = 120 });
            _dgvPayments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNotes",   HeaderText = "Notes",   AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        // ── Load from DB (Persisted mode) ─────────────────────────────────────
        /// <summary>Switch to persisted mode and load payments from the database.</summary>
        public void LoadPayments()
        {
            IsPendingMode = false;
            _displayRows.Clear();

            try
            {
                DataTable dt = clsPaymentAllocation.GetAllocationsByTransactionID(TransactionID);
                foreach (DataRow row in dt.Rows)
                {
                    _displayRows.Add(new PaymentRow
                    {
                        PaymentID = Convert.ToInt32(row["PaymentID"]),
                        AllocationID = Convert.ToInt32(row["AllocationID"]),
                        Date = Convert.ToDateTime(row["PaymentDate"]),
                        MethodName = row["MethodName"]?.ToString() ?? "",
                        Amount = Convert.ToDecimal(row["Amount"]),
                        Notes = row["PaymentNotes"]?.ToString() ?? ""
                    });
                }
            }
            catch { /* table not available yet */ }

            _RefreshGrid();
            RefreshSummary();
        }

        // ── Pending helpers ───────────────────────────────────────────────────

        /// <summary>Returns the list of payments that have not yet been persisted.</summary>
        public List<clsPayment> GetPendingPayments() => _pendingPayments;

        /// <summary>Clears all pending (in-memory) payments.</summary>
        public void ClearPendingPayments()
        {
            _pendingPayments.Clear();
            if (IsPendingMode)
            {
                _displayRows.Clear();
                _RefreshGrid();
                RefreshSummary();
            }
        }

        /// <summary>Returns total paid so far (pending + persisted).</summary>
        public decimal GetTotalPaid()
        {
            if (IsPendingMode)
                return _pendingPayments.Sum(p => p.PaymentAmount);
            else
                return _displayRows.Sum(r => r.Amount);
        }

        // ── Button handlers ───────────────────────────────────────────────────

        private void _btnAddPayment_Click(object sender, EventArgs e)
        {
            decimal remaining = TotalAmount - GetTotalPaid();
            if (remaining <= 0)
            {
                MessageBox.Show("The invoice is fully paid. No more payments can be added.",
                    "Fully Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frm = new frmAddEditPayment(remaining);
            if (frm.ShowDialog() == DialogResult.OK && frm.SavedPayment != null)
            {
                var payment = frm.SavedPayment;

                if (IsPendingMode)
                {
                    _pendingPayments.Add(payment);
                    _displayRows.Add(new PaymentRow
                    {
                        PaymentID = -1,
                        AllocationID = -1,
                        Date = payment.PaymentDate,
                        MethodName = _GetMethodName(payment.PaymentMethodID),
                        Amount = payment.PaymentAmount,
                        Notes = payment.Notes ?? ""
                    });
                    _RefreshGrid();
                    RefreshSummary();
                }
                else
                {
                    // Persisted mode — save to DB immediately
                    payment.Allocations.Clear();
                    payment.Allocations.Add(new clsPaymentAllocation
                    {
                        TransactionID = TransactionID,
                        Amount = payment.PaymentAmount
                    });

                    if (payment.Save())
                    {
                        LoadPayments(); // reload from DB
                    }
                    else
                    {
                        MessageBox.Show("Failed to save the payment.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                PaidAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void _btnEditPayment_Click(object sender, EventArgs e)
        {
            if (_dgvPayments.CurrentRow == null) return;
            int idx = _dgvPayments.CurrentRow.Index;
            if (idx < 0 || idx >= _displayRows.Count) return;

            var row = _displayRows[idx];
            decimal otherPaid = GetTotalPaid() - row.Amount;
            decimal maxAllowed = TotalAmount - otherPaid;

            if (IsPendingMode)
            {
                // Edit the pending payment via dialog
                using var frm = new frmAddEditPayment(maxAllowed);
                if (frm.ShowDialog() == DialogResult.OK && frm.SavedPayment != null)
                {
                    var updated = frm.SavedPayment;
                    // Update pending list
                    if (idx < _pendingPayments.Count)
                    {
                        _pendingPayments[idx] = updated;
                        _displayRows[idx] = new PaymentRow
                        {
                            PaymentID = -1,
                            AllocationID = -1,
                            Date = updated.PaymentDate,
                            MethodName = _GetMethodName(updated.PaymentMethodID),
                            Amount = updated.PaymentAmount,
                            Notes = updated.Notes ?? ""
                        };
                    }
                    _RefreshGrid();
                    RefreshSummary();
                    PaidAmountChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                // Edit persisted payment
                if (row.PaymentID <= 0) return;

                using var frm = new frmAddEditPayment(row.PaymentID, maxAllowed);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadPayments(); // reload from DB
                    PaidAmountChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void _btnDeletePayment_Click(object sender, EventArgs e)
        {
            if (_dgvPayments.CurrentRow == null) return;
            int idx = _dgvPayments.CurrentRow.Index;
            if (idx < 0 || idx >= _displayRows.Count) return;

            var confirm = MessageBox.Show("Are you sure you want to delete this payment?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            if (IsPendingMode)
            {
                if (idx < _pendingPayments.Count)
                    _pendingPayments.RemoveAt(idx);
                _displayRows.RemoveAt(idx);
                _RefreshGrid();
                RefreshSummary();
            }
            else
            {
                var row = _displayRows[idx];
                if (row.PaymentID > 0)
                {
                    int? currentUser = clsGlobalUser.CurrentUser?.UserID;
                    if (clsPayment.DeletePayment(row.PaymentID, currentUser))
                        LoadPayments();
                    else
                    {
                        MessageBox.Show("Failed to delete the payment.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            PaidAmountChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── Grid refresh ──────────────────────────────────────────────────────

        private void _RefreshGrid()
        {
            _dgvPayments.Rows.Clear();
            for (int i = 0; i < _displayRows.Count; i++)
            {
                var r = _displayRows[i];
                _dgvPayments.Rows.Add(
                    (i + 1).ToString(),
                    r.Date.ToString("yyyy-MM-dd"),
                    r.MethodName,
                    r.Amount.ToString("N2"),
                    r.Notes
                );
            }
        }

        // ── Summary ───────────────────────────────────────────────────────────

        public void RefreshSummary()
        {
            decimal paid = GetTotalPaid();
            decimal remaining = TotalAmount - paid;

            _lblTotalValue.Text = TotalAmount.ToString("N2");
            _lblPaidValue.Text = paid.ToString("N2");
            _lblRemainingValue.Text = remaining.ToString("N2");

            // Color the remaining label orange if > 0, green if 0
            _lblRemainingValue.ForeColor = remaining > 0
                ? Color.FromArgb(234, 88, 12)   // orange
                : Color.FromArgb(22, 163, 74);   // green
        }

        // ── Theme ─────────────────────────────────────────────────────────────

        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;
            BackColor = c.ContentBackground;
            _pnlPaymentSection.BackColor = c.ContentBackground;

            _lblSectionPayments.ForeColor = c.Primary;
            _pnlSepPayments.BackColor = c.BorderColor;
            _pnlSepSummary.BackColor = c.BorderColor;

            // Buttons
            _btnAddPayment.BackColor = c.Primary;
            _btnAddPayment.ForeColor = Color.White;
            _btnAddPayment.FlatAppearance.BorderSize = 0;
            _btnAddPayment.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnEditPayment.BackColor = c.PrimaryLight;
            _btnEditPayment.ForeColor = c.Primary;
            _btnEditPayment.FlatAppearance.BorderColor = c.BorderAccent;
            _btnEditPayment.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnDeletePayment.BackColor = Color.FromArgb(254, 226, 226);
            _btnDeletePayment.ForeColor = Color.FromArgb(220, 38, 38);
            _btnDeletePayment.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);

            // DataGridView
            _dgvPayments.BackgroundColor = c.ContentBackground;
            _dgvPayments.GridColor = c.BorderColor;
            _dgvPayments.DefaultCellStyle.BackColor = c.ContentBackground;
            _dgvPayments.DefaultCellStyle.ForeColor = c.PrimaryText;
            _dgvPayments.DefaultCellStyle.SelectionBackColor = c.PrimaryLight;
            _dgvPayments.DefaultCellStyle.SelectionForeColor = c.PrimaryText;
            _dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = c.FormBackground;
            _dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = c.SecondaryText;
            _dgvPayments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _dgvPayments.AlternatingRowsDefaultCellStyle.BackColor = c.FormBackground;

            // Summary labels
            _lblTotalLabel.ForeColor = c.SecondaryText;
            _lblTotalValue.ForeColor = c.PrimaryText;
            _lblPaidLabel.ForeColor = c.SecondaryText;
            _lblPaidValue.ForeColor = Color.FromArgb(22, 163, 74);
            _lblRemainingLabel.ForeColor = c.SecondaryText;
            _pnlSummary.BackColor = c.ContentBackground;
            _pnlPaymentButtons.BackColor = c.ContentBackground;

            Invalidate();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private string _GetMethodName(int methodID)
        {
            try
            {
                DataTable dt = clsPaymentMethod.GetAllPaymentMethod();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["PaymentMethodID"]) == methodID)
                        return row["MethodName"]?.ToString() ?? $"Method #{methodID}";
                }
            }
            catch { }
            return $"Method #{methodID}";
        }

        // ── Display DTO ───────────────────────────────────────────────────────
        private class PaymentRow
        {
            public int PaymentID { get; set; }
            public int AllocationID { get; set; }
            public DateTime Date { get; set; }
            public string MethodName { get; set; } = "";
            public decimal Amount { get; set; }
            public string Notes { get; set; } = "";
        }
    }
}
