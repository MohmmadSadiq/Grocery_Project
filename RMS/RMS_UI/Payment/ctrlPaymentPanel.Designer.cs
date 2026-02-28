namespace RMS_UI.Payment
{
    partial class ctrlPaymentPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support – do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _pnlPaymentSection = new Panel();
            _lblSectionPayments = new Label();
            _pnlSepPayments = new Panel();
            _pnlPaymentButtons = new Panel();
            _btnAddPayment = new Button();
            _btnEditPayment = new Button();
            _btnDeletePayment = new Button();
            _dgvPayments = new DataGridView();
            _pnlSummary = new Panel();
            _lblTotalLabel = new Label();
            _lblTotalValue = new Label();
            _lblPaidLabel = new Label();
            _lblPaidValue = new Label();
            _lblRemainingLabel = new Label();
            _lblRemainingValue = new Label();
            _pnlSepSummary = new Panel();

            _pnlPaymentSection.SuspendLayout();
            _pnlPaymentButtons.SuspendLayout();
            _pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvPayments).BeginInit();
            SuspendLayout();

            // ── _pnlPaymentSection ──────────────────────────────────────────
            _pnlPaymentSection.Dock = DockStyle.Fill;
            _pnlPaymentSection.Padding = new Padding(16, 12, 16, 12);
            _pnlPaymentSection.BackColor = Color.White;
            _pnlPaymentSection.Controls.Add(_dgvPayments);
            _pnlPaymentSection.Controls.Add(_pnlSummary);
            _pnlPaymentSection.Controls.Add(_pnlPaymentButtons);
            _pnlPaymentSection.Controls.Add(_pnlSepPayments);
            _pnlPaymentSection.Controls.Add(_lblSectionPayments);

            // ── _lblSectionPayments ─────────────────────────────────────────
            _lblSectionPayments.Dock = DockStyle.Top;
            _lblSectionPayments.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            _lblSectionPayments.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionPayments.Text = "💰  Payments";
            _lblSectionPayments.Height = 30;
            _lblSectionPayments.Padding = new Padding(0, 4, 0, 0);

            // ── _pnlSepPayments ─────────────────────────────────────────────
            _pnlSepPayments.Dock = DockStyle.Top;
            _pnlSepPayments.Height = 1;
            _pnlSepPayments.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSepPayments.Margin = new Padding(0, 0, 0, 8);

            // ── _pnlPaymentButtons ──────────────────────────────────────────
            _pnlPaymentButtons.Dock = DockStyle.Top;
            _pnlPaymentButtons.Height = 42;
            _pnlPaymentButtons.Padding = new Padding(0, 6, 0, 6);

            // _btnAddPayment
            _btnAddPayment.Text = "➕ Add Payment";
            _btnAddPayment.Size = new Size(130, 30);
            _btnAddPayment.Location = new Point(0, 6);
            _btnAddPayment.FlatStyle = FlatStyle.Flat;
            _btnAddPayment.FlatAppearance.BorderSize = 0;
            _btnAddPayment.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnAddPayment.BackColor = Color.FromArgb(59, 130, 246);
            _btnAddPayment.ForeColor = Color.White;
            _btnAddPayment.Cursor = Cursors.Hand;
            _btnAddPayment.Click += _btnAddPayment_Click;

            // _btnEditPayment
            _btnEditPayment.Text = "✏️ Edit";
            _btnEditPayment.Size = new Size(80, 30);
            _btnEditPayment.Location = new Point(136, 6);
            _btnEditPayment.FlatStyle = FlatStyle.Flat;
            _btnEditPayment.Font = new Font("Segoe UI", 9F);
            _btnEditPayment.BackColor = Color.FromArgb(219, 234, 254);
            _btnEditPayment.ForeColor = Color.FromArgb(59, 130, 246);
            _btnEditPayment.FlatAppearance.BorderColor = Color.FromArgb(147, 197, 253);
            _btnEditPayment.Cursor = Cursors.Hand;
            _btnEditPayment.Click += _btnEditPayment_Click;

            // _btnDeletePayment
            _btnDeletePayment.Text = "🗑️ Delete";
            _btnDeletePayment.Size = new Size(90, 30);
            _btnDeletePayment.Location = new Point(222, 6);
            _btnDeletePayment.FlatStyle = FlatStyle.Flat;
            _btnDeletePayment.Font = new Font("Segoe UI", 9F);
            _btnDeletePayment.BackColor = Color.FromArgb(254, 226, 226);
            _btnDeletePayment.ForeColor = Color.FromArgb(220, 38, 38);
            _btnDeletePayment.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);
            _btnDeletePayment.Cursor = Cursors.Hand;
            _btnDeletePayment.Click += _btnDeletePayment_Click;

            _pnlPaymentButtons.Controls.Add(_btnDeletePayment);
            _pnlPaymentButtons.Controls.Add(_btnEditPayment);
            _pnlPaymentButtons.Controls.Add(_btnAddPayment);

            // ── _pnlSummary ─────────────────────────────────────────────────
            _pnlSummary.Dock = DockStyle.Bottom;
            _pnlSummary.Height = 70;
            _pnlSummary.Padding = new Padding(0, 6, 0, 0);

            // _pnlSepSummary (top border of summary)
            _pnlSepSummary.Dock = DockStyle.Top;
            _pnlSepSummary.Height = 1;
            _pnlSepSummary.BackColor = Color.FromArgb(226, 232, 240);

            // Total label & value
            _lblTotalLabel.Text = "Total Amount:";
            _lblTotalLabel.Font = new Font("Segoe UI", 9F);
            _lblTotalLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblTotalLabel.Location = new Point(0, 14);
            _lblTotalLabel.AutoSize = true;

            _lblTotalValue.Text = "0.00";
            _lblTotalValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblTotalValue.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTotalValue.Location = new Point(110, 12);
            _lblTotalValue.AutoSize = true;

            // Paid label & value
            _lblPaidLabel.Text = "Paid:";
            _lblPaidLabel.Font = new Font("Segoe UI", 9F);
            _lblPaidLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPaidLabel.Location = new Point(250, 14);
            _lblPaidLabel.AutoSize = true;

            _lblPaidValue.Text = "0.00";
            _lblPaidValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblPaidValue.ForeColor = Color.FromArgb(22, 163, 74);
            _lblPaidValue.Location = new Point(290, 12);
            _lblPaidValue.AutoSize = true;

            // Remaining label & value
            _lblRemainingLabel.Text = "Remaining:";
            _lblRemainingLabel.Font = new Font("Segoe UI", 9F);
            _lblRemainingLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblRemainingLabel.Location = new Point(430, 14);
            _lblRemainingLabel.AutoSize = true;

            _lblRemainingValue.Text = "0.00";
            _lblRemainingValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblRemainingValue.ForeColor = Color.FromArgb(234, 88, 12);
            _lblRemainingValue.Location = new Point(510, 12);
            _lblRemainingValue.AutoSize = true;

            _pnlSummary.Controls.Add(_lblRemainingValue);
            _pnlSummary.Controls.Add(_lblRemainingLabel);
            _pnlSummary.Controls.Add(_lblPaidValue);
            _pnlSummary.Controls.Add(_lblPaidLabel);
            _pnlSummary.Controls.Add(_lblTotalValue);
            _pnlSummary.Controls.Add(_lblTotalLabel);
            _pnlSummary.Controls.Add(_pnlSepSummary);

            // ── _dgvPayments ────────────────────────────────────────────────
            _dgvPayments.Dock = DockStyle.Fill;
            _dgvPayments.AllowUserToAddRows = false;
            _dgvPayments.AllowUserToDeleteRows = false;
            _dgvPayments.AllowUserToResizeRows = false;
            _dgvPayments.ReadOnly = true;
            _dgvPayments.MultiSelect = false;
            _dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvPayments.RowHeadersVisible = false;
            _dgvPayments.BorderStyle = BorderStyle.None;
            _dgvPayments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _dgvPayments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgvPayments.EnableHeadersVisualStyles = false;
            _dgvPayments.ColumnHeadersHeight = 36;
            _dgvPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvPayments.RowTemplate.Height = 32;
            _dgvPayments.AutoGenerateColumns = false;
            _dgvPayments.BackgroundColor = Color.White;
            _dgvPayments.GridColor = Color.FromArgb(226, 232, 240);

            // ── this ─────────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlPaymentSection);
            Name = "ctrlPaymentPanel";
            Size = new Size(900, 280);

            _pnlPaymentSection.ResumeLayout(false);
            _pnlPaymentButtons.ResumeLayout(false);
            _pnlSummary.ResumeLayout(false);
            _pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvPayments).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlPaymentSection;
        private Label _lblSectionPayments;
        private Panel _pnlSepPayments;
        private Panel _pnlPaymentButtons;
        private Button _btnAddPayment;
        private Button _btnEditPayment;
        private Button _btnDeletePayment;
        private DataGridView _dgvPayments;
        private Panel _pnlSummary;
        private Label _lblTotalLabel;
        private Label _lblTotalValue;
        private Label _lblPaidLabel;
        private Label _lblPaidValue;
        private Label _lblRemainingLabel;
        private Label _lblRemainingValue;
        private Panel _pnlSepSummary;
    }
}
