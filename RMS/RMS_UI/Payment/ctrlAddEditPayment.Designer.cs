namespace RMS_UI.Payment
{
    partial class ctrlAddEditPayment
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            _pnlHeader = new Panel();
            _lblTitle = new Label();
            _lblMode = new Label();
            _btnSave = new Button();
            _btnCancel = new Button();

            _notification = new RMS_UI.Controls.NotificationControl();

            _pnlContent = new Panel();
            _lblSectionPayment = new Label();
            _pnlSep1 = new Panel();

            _lblPaymentMethod = new Label();
            _cmbPaymentMethod = new ComboBox();
            _lblPaymentAmount = new Label();
            _numPaymentAmount = new NumericUpDown();
            _lblPaymentDate = new Label();
            _dtpPaymentDate = new DateTimePicker();
            _lblNotes = new Label();
            _txtNotes = new TextBox();

            _pnlAmountInfo = new Panel();
            _lblMaxAmountLabel = new Label();
            _lblMaxAmountValue = new Label();

            _errorProvider = new ErrorProvider(components);

            _pnlHeader.SuspendLayout();
            _pnlContent.SuspendLayout();
            _pnlAmountInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numPaymentAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();

            // ─────────────────────────────────────────────────────────────────
            // _pnlHeader
            // ─────────────────────────────────────────────────────────────────
            _pnlHeader.BackColor = Color.White;
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Size = new Size(450, 72);
            _pnlHeader.Controls.Add(_btnCancel);
            _pnlHeader.Controls.Add(_btnSave);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Controls.Add(_lblTitle);

            // _lblTitle
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(20, 12);
            _lblTitle.Text = "💳  Add Payment";

            // _lblMode
            _lblMode.AutoSize = true;
            _lblMode.Font = new Font("Segoe UI", 9F);
            _lblMode.ForeColor = Color.FromArgb(100, 116, 139);
            _lblMode.Location = new Point(22, 46);
            _lblMode.Text = "Enter payment details below.";

            // _btnSave
            _btnSave.BackColor = Color.FromArgb(59, 130, 246);
            _btnSave.Cursor = Cursors.Hand;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatStyle = FlatStyle.Flat;
            _btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSave.ForeColor = Color.White;
            _btnSave.Location = new Point(240, 16);
            _btnSave.Size = new Size(100, 40);
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = false;
            _btnSave.Click += _btnSave_Click;

            // _btnCancel
            _btnCancel.BackColor = Color.FromArgb(245, 247, 250);
            _btnCancel.Cursor = Cursors.Hand;
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnCancel.FlatAppearance.BorderSize = 1;
            _btnCancel.FlatStyle = FlatStyle.Flat;
            _btnCancel.Font = new Font("Segoe UI", 10F);
            _btnCancel.ForeColor = Color.FromArgb(100, 116, 139);
            _btnCancel.Location = new Point(348, 16);
            _btnCancel.Size = new Size(90, 40);
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = false;
            _btnCancel.Click += _btnCancel_Click;

            // ─────────────────────────────────────────────────────────────────
            // _notification
            // ─────────────────────────────────────────────────────────────────
            _notification.Dock = DockStyle.Top;
            _notification.Size = new Size(450, 0);

            // ─────────────────────────────────────────────────────────────────
            // _pnlContent
            // ─────────────────────────────────────────────────────────────────
            _pnlContent.BackColor = Color.White;
            _pnlContent.Dock = DockStyle.Fill;
            _pnlContent.Padding = new Padding(20, 10, 20, 10);
            _pnlContent.Controls.Add(_txtNotes);
            _pnlContent.Controls.Add(_lblNotes);
            _pnlContent.Controls.Add(_pnlAmountInfo);
            _pnlContent.Controls.Add(_numPaymentAmount);
            _pnlContent.Controls.Add(_lblPaymentAmount);
            _pnlContent.Controls.Add(_dtpPaymentDate);
            _pnlContent.Controls.Add(_lblPaymentDate);
            _pnlContent.Controls.Add(_cmbPaymentMethod);
            _pnlContent.Controls.Add(_lblPaymentMethod);
            _pnlContent.Controls.Add(_pnlSep1);
            _pnlContent.Controls.Add(_lblSectionPayment);

            // _lblSectionPayment
            _lblSectionPayment.AutoSize = true;
            _lblSectionPayment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionPayment.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionPayment.Location = new Point(20, 14);
            _lblSectionPayment.Text = "Payment Details";

            // _pnlSep1
            _pnlSep1.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new Point(20, 38);
            _pnlSep1.Size = new Size(400, 1);

            // _lblPaymentMethod
            _lblPaymentMethod.AutoSize = true;
            _lblPaymentMethod.Font = new Font("Segoe UI", 9.5F);
            _lblPaymentMethod.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPaymentMethod.Location = new Point(20, 52);
            _lblPaymentMethod.Text = "Payment Method *";

            // _cmbPaymentMethod
            _cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbPaymentMethod.Font = new Font("Segoe UI", 10F);
            _cmbPaymentMethod.Location = new Point(20, 74);
            _cmbPaymentMethod.Size = new Size(400, 25);

            // _lblPaymentAmount
            _lblPaymentAmount.AutoSize = true;
            _lblPaymentAmount.Font = new Font("Segoe UI", 9.5F);
            _lblPaymentAmount.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPaymentAmount.Location = new Point(20, 110);
            _lblPaymentAmount.Text = "Amount *";

            // _numPaymentAmount
            _numPaymentAmount.DecimalPlaces = 2;
            _numPaymentAmount.Font = new Font("Segoe UI", 11F);
            _numPaymentAmount.Location = new Point(20, 132);
            _numPaymentAmount.Maximum = 999999999m;
            _numPaymentAmount.Minimum = 0.01m;
            _numPaymentAmount.Size = new Size(200, 27);
            _numPaymentAmount.Value = 0.01m;
            _numPaymentAmount.ThousandsSeparator = true;

            // _pnlAmountInfo
            _pnlAmountInfo.BackColor = Color.FromArgb(240, 253, 244);
            _pnlAmountInfo.Location = new Point(230, 125);
            _pnlAmountInfo.Size = new Size(190, 38);
            _pnlAmountInfo.Controls.Add(_lblMaxAmountValue);
            _pnlAmountInfo.Controls.Add(_lblMaxAmountLabel);

            // _lblMaxAmountLabel
            _lblMaxAmountLabel.AutoSize = true;
            _lblMaxAmountLabel.Font = new Font("Segoe UI", 8.5F);
            _lblMaxAmountLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblMaxAmountLabel.Location = new Point(8, 3);
            _lblMaxAmountLabel.Text = "Max allowed:";

            // _lblMaxAmountValue
            _lblMaxAmountValue.AutoSize = true;
            _lblMaxAmountValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblMaxAmountValue.ForeColor = Color.FromArgb(22, 163, 74);
            _lblMaxAmountValue.Location = new Point(90, 0);
            _lblMaxAmountValue.Text = "0.00";

            // _lblPaymentDate
            _lblPaymentDate.AutoSize = true;
            _lblPaymentDate.Font = new Font("Segoe UI", 9.5F);
            _lblPaymentDate.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPaymentDate.Location = new Point(20, 172);
            _lblPaymentDate.Text = "Payment Date";

            // _dtpPaymentDate
            _dtpPaymentDate.Font = new Font("Segoe UI", 10F);
            _dtpPaymentDate.Format = DateTimePickerFormat.Short;
            _dtpPaymentDate.Location = new Point(20, 194);
            _dtpPaymentDate.Size = new Size(200, 25);

            // _lblNotes
            _lblNotes.AutoSize = true;
            _lblNotes.Font = new Font("Segoe UI", 9.5F);
            _lblNotes.ForeColor = Color.FromArgb(100, 116, 139);
            _lblNotes.Location = new Point(20, 230);
            _lblNotes.Text = "Notes";

            // _txtNotes
            _txtNotes.Font = new Font("Segoe UI", 10F);
            _txtNotes.Location = new Point(20, 252);
            _txtNotes.Multiline = true;
            _txtNotes.ScrollBars = ScrollBars.Vertical;
            _txtNotes.Size = new Size(400, 70);

            // _errorProvider
            _errorProvider.ContainerControl = this;

            // ─────────────────────────────────────────────────────────────────
            // ctrlAddEditPayment
            // ─────────────────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlContent);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Name = "ctrlAddEditPayment";
            Size = new Size(450, 400);

            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlContent.ResumeLayout(false);
            _pnlContent.PerformLayout();
            _pnlAmountInfo.ResumeLayout(false);
            _pnlAmountInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numPaymentAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlHeader;
        private Label _lblTitle;
        private Label _lblMode;
        private Button _btnSave;
        private Button _btnCancel;

        private RMS_UI.Controls.NotificationControl _notification;

        private Panel _pnlContent;
        private Label _lblSectionPayment;
        private Panel _pnlSep1;
        private Label _lblPaymentMethod;
        private ComboBox _cmbPaymentMethod;
        private Label _lblPaymentAmount;
        private NumericUpDown _numPaymentAmount;
        private Panel _pnlAmountInfo;
        private Label _lblMaxAmountLabel;
        private Label _lblMaxAmountValue;
        private Label _lblPaymentDate;
        private DateTimePicker _dtpPaymentDate;
        private Label _lblNotes;
        private TextBox _txtNotes;

        private ErrorProvider _errorProvider;
    }
}
