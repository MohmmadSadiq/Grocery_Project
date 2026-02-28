namespace RMS_UI.Suppliers_Purchase
{
    partial class ctrlAddEditPurchase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        // ─── Header ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel          _pnlHeader;
        private System.Windows.Forms.Label          _lblTitle;
        private System.Windows.Forms.Label          _lblMode;
        private System.Windows.Forms.Button         _btnSave;
        private System.Windows.Forms.Button         _btnReset;

        // ─── Notification ───────────────────────────────────────────────────────
        private RMS_UI.Controls.NotificationControl _notification;

        // ─── Inner Tabs ─────────────────────────────────────────────────────────
        private System.Windows.Forms.TabControl     _tabInner;
        private System.Windows.Forms.TabPage        _tabDetails;
        private System.Windows.Forms.TabPage        _tabInvoiceDoc;

        // ═══ Tab 1: Purchase Details ════════════════════════════════════════════
        private System.Windows.Forms.Panel          _pnlDetailsScroll;

        // Section: Invoice Information
        private System.Windows.Forms.Panel          _pnlInvoiceInfo;
        private System.Windows.Forms.Label          _lblSectionInvoice;
        private System.Windows.Forms.Panel          _pnlSep1;
        private System.Windows.Forms.Label          _lblID;
        private System.Windows.Forms.Label          _lblInvoiceNumber;
        private System.Windows.Forms.TextBox        _txtInvoiceNumber;
        private System.Windows.Forms.Label          _lblTransactionDate;
        private System.Windows.Forms.DateTimePicker _dtpTransactionDate;
        private System.Windows.Forms.Label          _lblTransactionStatus;
        private System.Windows.Forms.ComboBox       _cmbTransactionStatus;

        // Section: Supplier & Employee
        private System.Windows.Forms.Panel          _pnlSupplierInfo;
        private System.Windows.Forms.Label          _lblSectionSupplier;
        private System.Windows.Forms.Panel          _pnlSep2;
        private System.Windows.Forms.Label          _lblSupplier;
        private System.Windows.Forms.ComboBox       _cmbSupplier;
        private System.Windows.Forms.Label          _lblEmployee;
        private System.Windows.Forms.ComboBox       _cmbEmployee;

        // Section: Batch Items
        private System.Windows.Forms.Panel          _pnlBatchSection;
        private System.Windows.Forms.Label          _lblSectionBatch;
        private System.Windows.Forms.Panel          _pnlSep3;
        private System.Windows.Forms.Panel          _pnlBatchInput;
        private Products.ctrlProductFinder          _ctrlProductFinder;
        private System.Windows.Forms.Label          _lblQuantity;
        private System.Windows.Forms.NumericUpDown   _numQuantity;
        private System.Windows.Forms.Label          _lblUnitCost;
        private System.Windows.Forms.NumericUpDown   _numUnitCost;
        private System.Windows.Forms.Label          _lblProductionDate;
        private System.Windows.Forms.DateTimePicker _dtpProductionDate;
        private System.Windows.Forms.Label          _lblExpiryDate;
        private System.Windows.Forms.DateTimePicker _dtpExpiryDate;
        private System.Windows.Forms.Label          _lblBatchNumber;
        private System.Windows.Forms.TextBox        _txtBatchNumber;
        private System.Windows.Forms.Button         _btnAddBatch;
        private System.Windows.Forms.Button         _btnEditBatch;
        private System.Windows.Forms.Button         _btnRemoveBatch;
        private System.Windows.Forms.DataGridView   _dgvBatches;

        // Section: Notes & Total
        private System.Windows.Forms.Panel          _pnlFooter;
        private System.Windows.Forms.Label          _lblSectionNotes;
        private System.Windows.Forms.Panel          _pnlSep4;
        private System.Windows.Forms.Label          _lblNotes;
        private System.Windows.Forms.TextBox        _txtNotes;
        private System.Windows.Forms.Panel          _pnlTotal;
        private System.Windows.Forms.Label          _lblTotalLabel;
        private System.Windows.Forms.Label          _lblTotalAmount;

        // Section: Payments
        private Payment.ctrlPaymentPanel            _ctrlPaymentPanel;

        // ═══ Tab 2: Invoice Document ════════════════════════════════════════════
        private System.Windows.Forms.Panel          _pnlDocToolbar;
        private System.Windows.Forms.Button         _btnBrowseFile;
        private System.Windows.Forms.Button         _btnRemoveFile;
        private System.Windows.Forms.Label          _lblFileName;
        private System.Windows.Forms.Label          _lblFileSize;
        private System.Windows.Forms.Panel          _pnlDocPreview;
        private System.Windows.Forms.PictureBox     _picInvoice;
        private System.Windows.Forms.Panel          _pnlNoFile;
        private System.Windows.Forms.Label          _lblNoFile;

        // Validation
        private System.Windows.Forms.ErrorProvider  _errorProvider;
        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _pnlHeader = new Panel();
            _btnReset = new Button();
            _btnSave = new Button();
            _lblTitle = new Label();
            _lblMode = new Label();
            _notification = new Controls.NotificationControl();
            _tabInner = new TabControl();
            _tabDetails = new TabPage();
            _pnlDetailsScroll = new Panel();
            _pnlFooter = new Panel();
            _lblSectionNotes = new Label();
            _pnlSep4 = new Panel();
            _lblNotes = new Label();
            _txtNotes = new TextBox();
            _pnlTotal = new Panel();
            _lblTotalLabel = new Label();
            _lblTotalAmount = new Label();
            _pnlBatchSection = new Panel();
            _lblSectionBatch = new Label();
            _pnlSep3 = new Panel();
            _pnlBatchInput = new Panel();
            _ctrlProductFinder = new Products.ctrlProductFinder();
            _lblQuantity = new Label();
            _numQuantity = new NumericUpDown();
            _lblUnitCost = new Label();
            _numUnitCost = new NumericUpDown();
            _lblProductionDate = new Label();
            _dtpProductionDate = new DateTimePicker();
            _lblExpiryDate = new Label();
            _dtpExpiryDate = new DateTimePicker();
            _lblBatchNumber = new Label();
            _txtBatchNumber = new TextBox();
            _btnAddBatch = new Button();
            _btnEditBatch = new Button();
            _btnRemoveBatch = new Button();
            _dgvBatches = new DataGridView();
            _pnlSupplierInfo = new Panel();
            _lblSectionSupplier = new Label();
            _pnlSep2 = new Panel();
            _lblSupplier = new Label();
            _cmbSupplier = new ComboBox();
            _lblEmployee = new Label();
            _cmbEmployee = new ComboBox();
            _pnlInvoiceInfo = new Panel();
            _lblSectionInvoice = new Label();
            _pnlSep1 = new Panel();
            _lblID = new Label();
            _lblInvoiceNumber = new Label();
            _txtInvoiceNumber = new TextBox();
            _lblTransactionDate = new Label();
            _dtpTransactionDate = new DateTimePicker();
            _lblTransactionStatus = new Label();
            _cmbTransactionStatus = new ComboBox();
            _tabInvoiceDoc = new TabPage();
            _pnlDocPreview = new Panel();
            _picInvoice = new PictureBox();
            _pnlNoFile = new Panel();
            _lblNoFile = new Label();
            _pnlDocToolbar = new Panel();
            _btnBrowseFile = new Button();
            _btnRemoveFile = new Button();
            _lblFileName = new Label();
            _lblFileSize = new Label();
            _ctrlPaymentPanel = new Payment.ctrlPaymentPanel();
            _errorProvider = new ErrorProvider(components);
            _pnlHeader.SuspendLayout();
            _tabInner.SuspendLayout();
            _tabDetails.SuspendLayout();
            _pnlDetailsScroll.SuspendLayout();
            _pnlFooter.SuspendLayout();
            _pnlTotal.SuspendLayout();
            _pnlBatchSection.SuspendLayout();
            _pnlBatchInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numUnitCost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_dgvBatches).BeginInit();
            _pnlSupplierInfo.SuspendLayout();
            _pnlInvoiceInfo.SuspendLayout();
            _tabInvoiceDoc.SuspendLayout();
            _pnlDocPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picInvoice).BeginInit();
            _pnlNoFile.SuspendLayout();
            _pnlDocToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.BackColor = Color.White;
            _pnlHeader.Controls.Add(_btnReset);
            _pnlHeader.Controls.Add(_btnSave);
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Location = new Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new Padding(24, 0, 24, 0);
            _pnlHeader.Size = new Size(950, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _btnReset
            // 
            _btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnReset.Cursor = Cursors.Hand;
            _btnReset.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnReset.FlatStyle = FlatStyle.Flat;
            _btnReset.Font = new Font("Segoe UI", 10F);
            _btnReset.ForeColor = Color.FromArgb(100, 116, 139);
            _btnReset.Location = new Point(656, 17);
            _btnReset.Name = "_btnReset";
            _btnReset.Size = new Size(130, 40);
            _btnReset.TabIndex = 3;
            _btnReset.Text = "🔄  Reset";
            _btnReset.UseVisualStyleBackColor = false;
            _btnReset.Click += _btnReset_Click;
            // 
            // _btnSave
            // 
            _btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnSave.BackColor = Color.FromArgb(59, 130, 246);
            _btnSave.Cursor = Cursors.Hand;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnSave.FlatStyle = FlatStyle.Flat;
            _btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSave.ForeColor = Color.White;
            _btnSave.Location = new Point(796, 17);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(130, 40);
            _btnSave.TabIndex = 2;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = false;
            _btnSave.Click += _btnSave_Click;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(24, 12);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(252, 30);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "\U0001f9fe  Add New Purchase";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new Font("Segoe UI", 9F);
            _lblMode.ForeColor = Color.FromArgb(100, 116, 139);
            _lblMode.Location = new Point(26, 44);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new Size(271, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Enter purchase details and add batch items below.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = DockStyle.Top;
            _notification.Location = new Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new Size(950, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _tabInner
            // 
            _tabInner.Controls.Add(_tabDetails);
            _tabInner.Controls.Add(_tabInvoiceDoc);
            _tabInner.Dock = DockStyle.Fill;
            _tabInner.Font = new Font("Segoe UI", 11F);
            _tabInner.Location = new Point(0, 72);
            _tabInner.Name = "_tabInner";
            _tabInner.SelectedIndex = 0;
            _tabInner.Size = new Size(950, 628);
            _tabInner.TabIndex = 2;
            // 
            // _tabDetails
            // 
            _tabDetails.Controls.Add(_pnlDetailsScroll);
            _tabDetails.Location = new Point(4, 29);
            _tabDetails.Name = "_tabDetails";
            _tabDetails.Padding = new Padding(3);
            _tabDetails.Size = new Size(942, 595);
            _tabDetails.TabIndex = 0;
            _tabDetails.Text = "📋 Purchase Details";
            _tabDetails.UseVisualStyleBackColor = true;
            // 
            // _pnlDetailsScroll
            // 
            _pnlDetailsScroll.AutoScroll = true;
            _pnlDetailsScroll.Controls.Add(_ctrlPaymentPanel);
            _pnlDetailsScroll.Controls.Add(_pnlFooter);
            _pnlDetailsScroll.Controls.Add(_pnlBatchSection);
            _pnlDetailsScroll.Controls.Add(_pnlSupplierInfo);
            _pnlDetailsScroll.Controls.Add(_pnlInvoiceInfo);
            _pnlDetailsScroll.Dock = DockStyle.Fill;
            _pnlDetailsScroll.Location = new Point(3, 3);
            _pnlDetailsScroll.Name = "_pnlDetailsScroll";
            _pnlDetailsScroll.Padding = new Padding(16);
            _pnlDetailsScroll.Size = new Size(936, 589);
            _pnlDetailsScroll.TabIndex = 0;
            // 
            // _ctrlPaymentPanel
            // 
            _ctrlPaymentPanel.Dock = DockStyle.Top;
            _ctrlPaymentPanel.Size = new Size(900, 280);
            _ctrlPaymentPanel.Name = "_ctrlPaymentPanel";
            _ctrlPaymentPanel.IsPendingMode = true;
            // 
            // _pnlFooter
            // 
            _pnlFooter.BackColor = Color.White;
            _pnlFooter.Controls.Add(_lblSectionNotes);
            _pnlFooter.Controls.Add(_pnlSep4);
            _pnlFooter.Controls.Add(_lblNotes);
            _pnlFooter.Controls.Add(_txtNotes);
            _pnlFooter.Controls.Add(_pnlTotal);
            _pnlFooter.Dock = DockStyle.Top;
            _pnlFooter.Location = new Point(16, 656);
            _pnlFooter.Name = "_pnlFooter";
            _pnlFooter.Padding = new Padding(20);
            _pnlFooter.Size = new Size(887, 140);
            _pnlFooter.TabIndex = 3;
            _pnlFooter.Paint += _pnlCard_Paint;
            // 
            // _lblSectionNotes
            // 
            _lblSectionNotes.AutoSize = true;
            _lblSectionNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionNotes.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionNotes.Location = new Point(20, 10);
            _lblSectionNotes.Name = "_lblSectionNotes";
            _lblSectionNotes.Size = new Size(117, 19);
            _lblSectionNotes.TabIndex = 0;
            _lblSectionNotes.Text = "📝  Notes & Total";
            // 
            // _pnlSep4
            // 
            _pnlSep4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSep4.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep4.Location = new Point(20, 34);
            _pnlSep4.Name = "_pnlSep4";
            _pnlSep4.Size = new Size(847, 1);
            _pnlSep4.TabIndex = 1;
            // 
            // _lblNotes
            // 
            _lblNotes.AutoSize = true;
            _lblNotes.Font = new Font("Segoe UI", 9F);
            _lblNotes.ForeColor = Color.FromArgb(100, 116, 139);
            _lblNotes.Location = new Point(20, 44);
            _lblNotes.Name = "_lblNotes";
            _lblNotes.Size = new Size(38, 15);
            _lblNotes.TabIndex = 2;
            _lblNotes.Text = "Notes";
            // 
            // _txtNotes
            // 
            _txtNotes.BorderStyle = BorderStyle.FixedSingle;
            _txtNotes.Font = new Font("Segoe UI", 10F);
            _txtNotes.Location = new Point(20, 62);
            _txtNotes.Multiline = true;
            _txtNotes.Name = "_txtNotes";
            _txtNotes.PlaceholderText = "Optional notes...";
            _txtNotes.ScrollBars = ScrollBars.Vertical;
            _txtNotes.Size = new Size(480, 60);
            _txtNotes.TabIndex = 3;
            // 
            // _pnlTotal
            // 
            _pnlTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _pnlTotal.BackColor = Color.FromArgb(240, 253, 244);
            _pnlTotal.Controls.Add(_lblTotalLabel);
            _pnlTotal.Controls.Add(_lblTotalAmount);
            _pnlTotal.Location = new Point(667, 44);
            _pnlTotal.Name = "_pnlTotal";
            _pnlTotal.Padding = new Padding(12);
            _pnlTotal.Size = new Size(200, 78);
            _pnlTotal.TabIndex = 4;
            // 
            // _lblTotalLabel
            // 
            _lblTotalLabel.AutoSize = true;
            _lblTotalLabel.Font = new Font("Segoe UI", 10F);
            _lblTotalLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblTotalLabel.Location = new Point(12, 12);
            _lblTotalLabel.Name = "_lblTotalLabel";
            _lblTotalLabel.Size = new Size(95, 19);
            _lblTotalLabel.TabIndex = 0;
            _lblTotalLabel.Text = "Total Amount:";
            // 
            // _lblTotalAmount
            // 
            _lblTotalAmount.AutoSize = true;
            _lblTotalAmount.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            _lblTotalAmount.ForeColor = Color.FromArgb(22, 163, 74);
            _lblTotalAmount.Location = new Point(12, 36);
            _lblTotalAmount.Name = "_lblTotalAmount";
            _lblTotalAmount.Size = new Size(72, 37);
            _lblTotalAmount.TabIndex = 1;
            _lblTotalAmount.Text = "0.00";
            // 
            // _pnlBatchSection
            // 
            _pnlBatchSection.BackColor = Color.White;
            _pnlBatchSection.Controls.Add(_lblSectionBatch);
            _pnlBatchSection.Controls.Add(_pnlSep3);
            _pnlBatchSection.Controls.Add(_ctrlProductFinder);
            _pnlBatchSection.Controls.Add(_pnlBatchInput);
            _pnlBatchSection.Controls.Add(_dgvBatches);
            _pnlBatchSection.Dock = DockStyle.Top;
            _pnlBatchSection.Location = new Point(16, 236);
            _pnlBatchSection.Name = "_pnlBatchSection";
            _pnlBatchSection.Padding = new Padding(20);
            _pnlBatchSection.Size = new Size(887, 606);
            _pnlBatchSection.TabIndex = 2;
            _pnlBatchSection.Paint += _pnlCard_Paint;
            // 
            // _lblSectionBatch
            // 
            _lblSectionBatch.AutoSize = true;
            _lblSectionBatch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionBatch.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionBatch.Location = new Point(20, 10);
            _lblSectionBatch.Name = "_lblSectionBatch";
            _lblSectionBatch.Size = new Size(114, 19);
            _lblSectionBatch.TabIndex = 0;
            _lblSectionBatch.Text = "📦  Batch Items";
            // 
            // _pnlSep3
            // 
            _pnlSep3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSep3.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep3.Location = new Point(20, 34);
            _pnlSep3.Name = "_pnlSep3";
            _pnlSep3.Size = new Size(847, 1);
            _pnlSep3.TabIndex = 1;
            // 
            // _pnlBatchInput
            // 
            _pnlBatchInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlBatchInput.BackColor = Color.FromArgb(248, 250, 252);
            _pnlBatchInput.Controls.Add(_lblQuantity);
            _pnlBatchInput.Controls.Add(_numQuantity);
            _pnlBatchInput.Controls.Add(_lblUnitCost);
            _pnlBatchInput.Controls.Add(_numUnitCost);
            _pnlBatchInput.Controls.Add(_lblProductionDate);
            _pnlBatchInput.Controls.Add(_dtpProductionDate);
            _pnlBatchInput.Controls.Add(_lblExpiryDate);
            _pnlBatchInput.Controls.Add(_dtpExpiryDate);
            _pnlBatchInput.Controls.Add(_lblBatchNumber);
            _pnlBatchInput.Controls.Add(_txtBatchNumber);
            _pnlBatchInput.Controls.Add(_btnAddBatch);
            _pnlBatchInput.Controls.Add(_btnEditBatch);
            _pnlBatchInput.Controls.Add(_btnRemoveBatch);
            _pnlBatchInput.Location = new Point(20, 230);
            _pnlBatchInput.Name = "_pnlBatchInput";
            _pnlBatchInput.Padding = new Padding(10);
            _pnlBatchInput.Size = new Size(847, 110);
            _pnlBatchInput.TabIndex = 2;
            // 
            // _ctrlProductFinder
            // 
            _ctrlProductFinder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _ctrlProductFinder.Location = new Point(20, 42);
            _ctrlProductFinder.Name = "_ctrlProductFinder";
            _ctrlProductFinder.Size = new Size(847, 180);
            _ctrlProductFinder.TabIndex = 0;
            // 
            // _lblQuantity
            // 
            _lblQuantity.AutoSize = true;
            _lblQuantity.Font = new Font("Segoe UI", 8.5F);
            _lblQuantity.ForeColor = Color.FromArgb(100, 116, 139);
            _lblQuantity.Location = new Point(10, 8);
            _lblQuantity.Name = "_lblQuantity";
            _lblQuantity.Size = new Size(53, 15);
            _lblQuantity.TabIndex = 2;
            _lblQuantity.Text = "Quantity";
            // 
            // _numQuantity
            // 
            _numQuantity.DecimalPlaces = 2;
            _numQuantity.Font = new Font("Segoe UI", 9.5F);
            _numQuantity.Location = new Point(10, 26);
            _numQuantity.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            _numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            _numQuantity.Name = "_numQuantity";
            _numQuantity.Size = new Size(110, 24);
            _numQuantity.TabIndex = 3;
            _numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // _lblUnitCost
            // 
            _lblUnitCost.AutoSize = true;
            _lblUnitCost.Font = new Font("Segoe UI", 8.5F);
            _lblUnitCost.ForeColor = Color.FromArgb(100, 116, 139);
            _lblUnitCost.Location = new Point(130, 8);
            _lblUnitCost.Name = "_lblUnitCost";
            _lblUnitCost.Size = new Size(56, 15);
            _lblUnitCost.TabIndex = 4;
            _lblUnitCost.Text = "Unit Cost";
            // 
            // _numUnitCost
            // 
            _numUnitCost.DecimalPlaces = 2;
            _numUnitCost.Font = new Font("Segoe UI", 9.5F);
            _numUnitCost.Location = new Point(130, 26);
            _numUnitCost.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            _numUnitCost.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            _numUnitCost.Name = "_numUnitCost";
            _numUnitCost.Size = new Size(120, 24);
            _numUnitCost.TabIndex = 5;
            _numUnitCost.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // _lblProductionDate
            // 
            _lblProductionDate.AutoSize = true;
            _lblProductionDate.Font = new Font("Segoe UI", 8.5F);
            _lblProductionDate.ForeColor = Color.FromArgb(100, 116, 139);
            _lblProductionDate.Location = new Point(10, 58);
            _lblProductionDate.Name = "_lblProductionDate";
            _lblProductionDate.Size = new Size(93, 15);
            _lblProductionDate.TabIndex = 7;
            _lblProductionDate.Text = "Production Date";
            // 
            // _dtpProductionDate
            // 
            _dtpProductionDate.Checked = false;
            _dtpProductionDate.Font = new Font("Segoe UI", 9F);
            _dtpProductionDate.Format = DateTimePickerFormat.Short;
            _dtpProductionDate.Location = new Point(10, 76);
            _dtpProductionDate.Name = "_dtpProductionDate";
            _dtpProductionDate.ShowCheckBox = true;
            _dtpProductionDate.Size = new Size(170, 23);
            _dtpProductionDate.TabIndex = 8;
            // 
            // _lblExpiryDate
            // 
            _lblExpiryDate.AutoSize = true;
            _lblExpiryDate.Font = new Font("Segoe UI", 8.5F);
            _lblExpiryDate.ForeColor = Color.FromArgb(100, 116, 139);
            _lblExpiryDate.Location = new Point(195, 58);
            _lblExpiryDate.Name = "_lblExpiryDate";
            _lblExpiryDate.Size = new Size(66, 15);
            _lblExpiryDate.TabIndex = 9;
            _lblExpiryDate.Text = "Expiry Date";
            // 
            // _dtpExpiryDate
            // 
            _dtpExpiryDate.Checked = false;
            _dtpExpiryDate.Font = new Font("Segoe UI", 9F);
            _dtpExpiryDate.Format = DateTimePickerFormat.Short;
            _dtpExpiryDate.Location = new Point(195, 76);
            _dtpExpiryDate.Name = "_dtpExpiryDate";
            _dtpExpiryDate.ShowCheckBox = true;
            _dtpExpiryDate.Size = new Size(170, 23);
            _dtpExpiryDate.TabIndex = 10;
            // 
            // _lblBatchNumber
            // 
            _lblBatchNumber.AutoSize = true;
            _lblBatchNumber.Font = new Font("Segoe UI", 8.5F);
            _lblBatchNumber.ForeColor = Color.FromArgb(100, 116, 139);
            _lblBatchNumber.Location = new Point(380, 58);
            _lblBatchNumber.Name = "_lblBatchNumber";
            _lblBatchNumber.Size = new Size(84, 15);
            _lblBatchNumber.TabIndex = 11;
            _lblBatchNumber.Text = "Batch Number";
            // 
            // _txtBatchNumber
            // 
            _txtBatchNumber.BorderStyle = BorderStyle.FixedSingle;
            _txtBatchNumber.Font = new Font("Segoe UI", 9F);
            _txtBatchNumber.Location = new Point(380, 76);
            _txtBatchNumber.Name = "_txtBatchNumber";
            _txtBatchNumber.PlaceholderText = "Optional";
            _txtBatchNumber.Size = new Size(140, 23);
            _txtBatchNumber.TabIndex = 12;
            // 
            // _btnAddBatch
            // 
            _btnAddBatch.BackColor = Color.FromArgb(59, 130, 246);
            _btnAddBatch.Cursor = Cursors.Hand;
            _btnAddBatch.FlatAppearance.BorderSize = 0;
            _btnAddBatch.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnAddBatch.FlatStyle = FlatStyle.Flat;
            _btnAddBatch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnAddBatch.ForeColor = Color.White;
            _btnAddBatch.Location = new Point(629, 22);
            _btnAddBatch.Name = "_btnAddBatch";
            _btnAddBatch.Size = new Size(90, 30);
            _btnAddBatch.TabIndex = 6;
            _btnAddBatch.Text = "➕ Add";
            _btnAddBatch.UseVisualStyleBackColor = false;
            _btnAddBatch.Click += _btnAddBatch_Click;
            // 
            // _btnEditBatch
            // 
            _btnEditBatch.BackColor = Color.FromArgb(219, 234, 254);
            _btnEditBatch.Cursor = Cursors.Hand;
            _btnEditBatch.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
            _btnEditBatch.FlatAppearance.MouseOverBackColor = Color.FromArgb(191, 219, 254);
            _btnEditBatch.FlatStyle = FlatStyle.Flat;
            _btnEditBatch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnEditBatch.ForeColor = Color.FromArgb(37, 99, 235);
            _btnEditBatch.Location = new Point(629, 58);
            _btnEditBatch.Name = "_btnEditBatch";
            _btnEditBatch.Size = new Size(90, 28);
            _btnEditBatch.TabIndex = 13;
            _btnEditBatch.Text = "✏️ Edit";
            _btnEditBatch.UseVisualStyleBackColor = false;
            _btnEditBatch.Click += _btnEditBatch_Click;
            // 
            // _btnRemoveBatch
            // 
            _btnRemoveBatch.BackColor = Color.FromArgb(254, 226, 226);
            _btnRemoveBatch.Cursor = Cursors.Hand;
            _btnRemoveBatch.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);
            _btnRemoveBatch.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);
            _btnRemoveBatch.FlatStyle = FlatStyle.Flat;
            _btnRemoveBatch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnRemoveBatch.ForeColor = Color.FromArgb(220, 38, 38);
            _btnRemoveBatch.Location = new Point(729, 58);
            _btnRemoveBatch.Name = "_btnRemoveBatch";
            _btnRemoveBatch.Size = new Size(90, 28);
            _btnRemoveBatch.TabIndex = 14;
            _btnRemoveBatch.Text = "🗑️ Del";
            _btnRemoveBatch.UseVisualStyleBackColor = false;
            _btnRemoveBatch.Click += _btnRemoveBatch_Click;
            // 
            // _dgvBatches
            // 
            _dgvBatches.AllowUserToAddRows = false;
            _dgvBatches.AllowUserToDeleteRows = false;
            _dgvBatches.AllowUserToResizeRows = false;
            _dgvBatches.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _dgvBatches.BackgroundColor = Color.White;
            _dgvBatches.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _dgvBatches.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _dgvBatches.ColumnHeadersHeight = 35;
            _dgvBatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvBatches.EnableHeadersVisualStyles = false;
            _dgvBatches.GridColor = Color.FromArgb(241, 245, 249);
            _dgvBatches.Location = new Point(20, 350);
            _dgvBatches.MultiSelect = false;
            _dgvBatches.Name = "_dgvBatches";
            _dgvBatches.ReadOnly = true;
            _dgvBatches.RowHeadersVisible = false;
            _dgvBatches.RowTemplate.Height = 30;
            _dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvBatches.Size = new Size(847, 236);
            _dgvBatches.TabIndex = 3;
            // 
            // _pnlSupplierInfo
            // 
            _pnlSupplierInfo.BackColor = Color.White;
            _pnlSupplierInfo.Controls.Add(_lblSectionSupplier);
            _pnlSupplierInfo.Controls.Add(_pnlSep2);
            _pnlSupplierInfo.Controls.Add(_lblSupplier);
            _pnlSupplierInfo.Controls.Add(_cmbSupplier);
            _pnlSupplierInfo.Controls.Add(_lblEmployee);
            _pnlSupplierInfo.Controls.Add(_cmbEmployee);
            _pnlSupplierInfo.Dock = DockStyle.Top;
            _pnlSupplierInfo.Location = new Point(16, 136);
            _pnlSupplierInfo.Name = "_pnlSupplierInfo";
            _pnlSupplierInfo.Padding = new Padding(20);
            _pnlSupplierInfo.Size = new Size(887, 100);
            _pnlSupplierInfo.TabIndex = 1;
            _pnlSupplierInfo.Paint += _pnlCard_Paint;
            // 
            // _lblSectionSupplier
            // 
            _lblSectionSupplier.AutoSize = true;
            _lblSectionSupplier.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionSupplier.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionSupplier.Location = new Point(20, 10);
            _lblSectionSupplier.Name = "_lblSectionSupplier";
            _lblSectionSupplier.Size = new Size(167, 19);
            _lblSectionSupplier.TabIndex = 0;
            _lblSectionSupplier.Text = "🏢  Supplier & Employee";
            // 
            // _pnlSep2
            // 
            _pnlSep2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSep2.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep2.Location = new Point(20, 34);
            _pnlSep2.Name = "_pnlSep2";
            _pnlSep2.Size = new Size(847, 1);
            _pnlSep2.TabIndex = 1;
            // 
            // _lblSupplier
            // 
            _lblSupplier.AutoSize = true;
            _lblSupplier.Font = new Font("Segoe UI", 9F);
            _lblSupplier.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSupplier.Location = new Point(20, 44);
            _lblSupplier.Name = "_lblSupplier";
            _lblSupplier.Size = new Size(50, 15);
            _lblSupplier.TabIndex = 2;
            _lblSupplier.Text = "Supplier";
            // 
            // _cmbSupplier
            // 
            _cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbSupplier.Font = new Font("Segoe UI", 10F);
            _cmbSupplier.FormattingEnabled = true;
            _cmbSupplier.Location = new Point(20, 62);
            _cmbSupplier.Name = "_cmbSupplier";
            _cmbSupplier.Size = new Size(350, 25);
            _cmbSupplier.TabIndex = 3;
            // 
            // _lblEmployee
            // 
            _lblEmployee.AutoSize = true;
            _lblEmployee.Font = new Font("Segoe UI", 9F);
            _lblEmployee.ForeColor = Color.FromArgb(100, 116, 139);
            _lblEmployee.Location = new Point(400, 44);
            _lblEmployee.Name = "_lblEmployee";
            _lblEmployee.Size = new Size(113, 15);
            _lblEmployee.TabIndex = 4;
            _lblEmployee.Text = "Purchased By (Emp)";
            // 
            // _cmbEmployee
            // 
            _cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbEmployee.Font = new Font("Segoe UI", 10F);
            _cmbEmployee.FormattingEnabled = true;
            _cmbEmployee.Location = new Point(400, 62);
            _cmbEmployee.Name = "_cmbEmployee";
            _cmbEmployee.Size = new Size(310, 25);
            _cmbEmployee.TabIndex = 5;
            // 
            // _pnlInvoiceInfo
            // 
            _pnlInvoiceInfo.BackColor = Color.White;
            _pnlInvoiceInfo.Controls.Add(_lblSectionInvoice);
            _pnlInvoiceInfo.Controls.Add(_pnlSep1);
            _pnlInvoiceInfo.Controls.Add(_lblID);
            _pnlInvoiceInfo.Controls.Add(_lblInvoiceNumber);
            _pnlInvoiceInfo.Controls.Add(_txtInvoiceNumber);
            _pnlInvoiceInfo.Controls.Add(_lblTransactionDate);
            _pnlInvoiceInfo.Controls.Add(_dtpTransactionDate);
            _pnlInvoiceInfo.Controls.Add(_lblTransactionStatus);
            _pnlInvoiceInfo.Controls.Add(_cmbTransactionStatus);
            _pnlInvoiceInfo.Dock = DockStyle.Top;
            _pnlInvoiceInfo.Location = new Point(16, 16);
            _pnlInvoiceInfo.Margin = new Padding(0, 0, 0, 8);
            _pnlInvoiceInfo.Name = "_pnlInvoiceInfo";
            _pnlInvoiceInfo.Padding = new Padding(20);
            _pnlInvoiceInfo.Size = new Size(887, 120);
            _pnlInvoiceInfo.TabIndex = 0;
            _pnlInvoiceInfo.Paint += _pnlCard_Paint;
            // 
            // _lblSectionInvoice
            // 
            _lblSectionInvoice.AutoSize = true;
            _lblSectionInvoice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionInvoice.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionInvoice.Location = new Point(20, 10);
            _lblSectionInvoice.Name = "_lblSectionInvoice";
            _lblSectionInvoice.Size = new Size(168, 19);
            _lblSectionInvoice.TabIndex = 0;
            _lblSectionInvoice.Text = "📋  Invoice Information";
            // 
            // _pnlSep1
            // 
            _pnlSep1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSep1.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new Point(20, 34);
            _pnlSep1.Name = "_pnlSep1";
            _pnlSep1.Size = new Size(847, 1);
            _pnlSep1.TabIndex = 1;
            // 
            // _lblID
            // 
            _lblID.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblID.AutoSize = true;
            _lblID.BackColor = Color.Transparent;
            _lblID.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            _lblID.ForeColor = Color.FromArgb(100, 116, 139);
            _lblID.Location = new Point(733, 8);
            _lblID.Name = "_lblID";
            _lblID.Size = new Size(57, 20);
            _lblID.TabIndex = 2;
            _lblID.Text = "ID: N/A";
            // 
            // _lblInvoiceNumber
            // 
            _lblInvoiceNumber.AutoSize = true;
            _lblInvoiceNumber.Font = new Font("Segoe UI", 9F);
            _lblInvoiceNumber.ForeColor = Color.FromArgb(100, 116, 139);
            _lblInvoiceNumber.Location = new Point(20, 44);
            _lblInvoiceNumber.Name = "_lblInvoiceNumber";
            _lblInvoiceNumber.Size = new Size(92, 15);
            _lblInvoiceNumber.TabIndex = 3;
            _lblInvoiceNumber.Text = "Invoice Number";
            // 
            // _txtInvoiceNumber
            // 
            _txtInvoiceNumber.BorderStyle = BorderStyle.FixedSingle;
            _txtInvoiceNumber.Font = new Font("Segoe UI", 10F);
            _txtInvoiceNumber.Location = new Point(20, 62);
            _txtInvoiceNumber.Name = "_txtInvoiceNumber";
            _txtInvoiceNumber.PlaceholderText = "e.g. INV-001";
            _txtInvoiceNumber.Size = new Size(200, 25);
            _txtInvoiceNumber.TabIndex = 4;
            // 
            // _lblTransactionDate
            // 
            _lblTransactionDate.AutoSize = true;
            _lblTransactionDate.Font = new Font("Segoe UI", 9F);
            _lblTransactionDate.ForeColor = Color.FromArgb(100, 116, 139);
            _lblTransactionDate.Location = new Point(240, 44);
            _lblTransactionDate.Name = "_lblTransactionDate";
            _lblTransactionDate.Size = new Size(82, 15);
            _lblTransactionDate.TabIndex = 5;
            _lblTransactionDate.Text = "Purchase Date";
            // 
            // _dtpTransactionDate
            // 
            _dtpTransactionDate.Font = new Font("Segoe UI", 10F);
            _dtpTransactionDate.Format = DateTimePickerFormat.Short;
            _dtpTransactionDate.Location = new Point(240, 62);
            _dtpTransactionDate.Name = "_dtpTransactionDate";
            _dtpTransactionDate.Size = new Size(200, 25);
            _dtpTransactionDate.TabIndex = 6;
            // 
            // _lblTransactionStatus
            // 
            _lblTransactionStatus.AutoSize = true;
            _lblTransactionStatus.Font = new Font("Segoe UI", 9F);
            _lblTransactionStatus.ForeColor = Color.FromArgb(100, 116, 139);
            _lblTransactionStatus.Location = new Point(460, 44);
            _lblTransactionStatus.Name = "_lblTransactionStatus";
            _lblTransactionStatus.Size = new Size(39, 15);
            _lblTransactionStatus.TabIndex = 7;
            _lblTransactionStatus.Text = "Status";
            // 
            // _cmbTransactionStatus
            // 
            _cmbTransactionStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTransactionStatus.Font = new Font("Segoe UI", 10F);
            _cmbTransactionStatus.FormattingEnabled = true;
            _cmbTransactionStatus.Location = new Point(460, 62);
            _cmbTransactionStatus.Name = "_cmbTransactionStatus";
            _cmbTransactionStatus.Size = new Size(180, 25);
            _cmbTransactionStatus.TabIndex = 8;
            // 
            // _tabInvoiceDoc
            // 
            _tabInvoiceDoc.Controls.Add(_pnlDocPreview);
            _tabInvoiceDoc.Controls.Add(_pnlDocToolbar);
            _tabInvoiceDoc.Location = new Point(4, 29);
            _tabInvoiceDoc.Name = "_tabInvoiceDoc";
            _tabInvoiceDoc.Padding = new Padding(3);
            _tabInvoiceDoc.Size = new Size(942, 595);
            _tabInvoiceDoc.TabIndex = 1;
            _tabInvoiceDoc.Text = "📎 Invoice Document";
            _tabInvoiceDoc.UseVisualStyleBackColor = true;
            // 
            // _pnlDocPreview
            // 
            _pnlDocPreview.BackColor = Color.FromArgb(241, 245, 249);
            _pnlDocPreview.Controls.Add(_picInvoice);
            _pnlDocPreview.Controls.Add(_pnlNoFile);
            _pnlDocPreview.Dock = DockStyle.Fill;
            _pnlDocPreview.Location = new Point(3, 58);
            _pnlDocPreview.Name = "_pnlDocPreview";
            _pnlDocPreview.Size = new Size(936, 534);
            _pnlDocPreview.TabIndex = 1;
            // 
            // _picInvoice
            // 
            _picInvoice.Dock = DockStyle.Fill;
            _picInvoice.Location = new Point(0, 0);
            _picInvoice.Name = "_picInvoice";
            _picInvoice.Size = new Size(936, 534);
            _picInvoice.SizeMode = PictureBoxSizeMode.Zoom;
            _picInvoice.TabIndex = 0;
            _picInvoice.TabStop = false;
            _picInvoice.Visible = false;
            // 
            // _pnlNoFile
            // 
            _pnlNoFile.Controls.Add(_lblNoFile);
            _pnlNoFile.Dock = DockStyle.Fill;
            _pnlNoFile.Location = new Point(0, 0);
            _pnlNoFile.Name = "_pnlNoFile";
            _pnlNoFile.Size = new Size(936, 534);
            _pnlNoFile.TabIndex = 1;
            // 
            // _lblNoFile
            // 
            _lblNoFile.Dock = DockStyle.Fill;
            _lblNoFile.Font = new Font("Segoe UI", 14F);
            _lblNoFile.ForeColor = Color.FromArgb(148, 163, 184);
            _lblNoFile.Location = new Point(0, 0);
            _lblNoFile.Name = "_lblNoFile";
            _lblNoFile.Size = new Size(936, 534);
            _lblNoFile.TabIndex = 0;
            _lblNoFile.Text = "📎  No invoice document attached.\r\nClick \"Browse File\" to attach an image or PDF.";
            _lblNoFile.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _pnlDocToolbar
            // 
            _pnlDocToolbar.BackColor = Color.White;
            _pnlDocToolbar.Controls.Add(_btnBrowseFile);
            _pnlDocToolbar.Controls.Add(_btnRemoveFile);
            _pnlDocToolbar.Controls.Add(_lblFileName);
            _pnlDocToolbar.Controls.Add(_lblFileSize);
            _pnlDocToolbar.Dock = DockStyle.Top;
            _pnlDocToolbar.Location = new Point(3, 3);
            _pnlDocToolbar.Name = "_pnlDocToolbar";
            _pnlDocToolbar.Padding = new Padding(12);
            _pnlDocToolbar.Size = new Size(936, 55);
            _pnlDocToolbar.TabIndex = 0;
            // 
            // _btnBrowseFile
            // 
            _btnBrowseFile.BackColor = Color.FromArgb(59, 130, 246);
            _btnBrowseFile.Cursor = Cursors.Hand;
            _btnBrowseFile.FlatAppearance.BorderSize = 0;
            _btnBrowseFile.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnBrowseFile.FlatStyle = FlatStyle.Flat;
            _btnBrowseFile.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnBrowseFile.ForeColor = Color.White;
            _btnBrowseFile.Location = new Point(12, 10);
            _btnBrowseFile.Name = "_btnBrowseFile";
            _btnBrowseFile.Size = new Size(130, 35);
            _btnBrowseFile.TabIndex = 0;
            _btnBrowseFile.Text = "📁 Browse File";
            _btnBrowseFile.UseVisualStyleBackColor = false;
            _btnBrowseFile.Click += _btnBrowseFile_Click;
            // 
            // _btnRemoveFile
            // 
            _btnRemoveFile.BackColor = Color.FromArgb(254, 226, 226);
            _btnRemoveFile.Cursor = Cursors.Hand;
            _btnRemoveFile.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);
            _btnRemoveFile.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);
            _btnRemoveFile.FlatStyle = FlatStyle.Flat;
            _btnRemoveFile.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnRemoveFile.ForeColor = Color.FromArgb(220, 38, 38);
            _btnRemoveFile.Location = new Point(152, 10);
            _btnRemoveFile.Name = "_btnRemoveFile";
            _btnRemoveFile.Size = new Size(120, 35);
            _btnRemoveFile.TabIndex = 1;
            _btnRemoveFile.Text = "🗑️ Remove";
            _btnRemoveFile.UseVisualStyleBackColor = false;
            _btnRemoveFile.Visible = false;
            _btnRemoveFile.Click += _btnRemoveFile_Click;
            // 
            // _lblFileName
            // 
            _lblFileName.AutoSize = true;
            _lblFileName.Font = new Font("Segoe UI", 10F);
            _lblFileName.ForeColor = Color.FromArgb(100, 116, 139);
            _lblFileName.Location = new Point(290, 18);
            _lblFileName.Name = "_lblFileName";
            _lblFileName.Size = new Size(105, 19);
            _lblFileName.TabIndex = 2;
            _lblFileName.Text = "No file attached";
            // 
            // _lblFileSize
            // 
            _lblFileSize.AutoSize = true;
            _lblFileSize.Font = new Font("Segoe UI", 9F);
            _lblFileSize.ForeColor = Color.FromArgb(148, 163, 184);
            _lblFileSize.Location = new Point(420, 20);
            _lblFileSize.Name = "_lblFileSize";
            _lblFileSize.Size = new Size(0, 15);
            _lblFileSize.TabIndex = 3;
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditPurchase
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_tabInner);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Name = "ctrlAddEditPurchase";
            Size = new Size(950, 700);
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _tabInner.ResumeLayout(false);
            _tabDetails.ResumeLayout(false);
            _pnlDetailsScroll.ResumeLayout(false);
            _pnlFooter.ResumeLayout(false);
            _pnlFooter.PerformLayout();
            _pnlTotal.ResumeLayout(false);
            _pnlTotal.PerformLayout();
            _pnlBatchSection.ResumeLayout(false);
            _pnlBatchSection.PerformLayout();
            _pnlBatchInput.ResumeLayout(false);
            _pnlBatchInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numUnitCost).EndInit();
            ((System.ComponentModel.ISupportInitialize)_dgvBatches).EndInit();
            _pnlSupplierInfo.ResumeLayout(false);
            _pnlSupplierInfo.PerformLayout();
            _pnlInvoiceInfo.ResumeLayout(false);
            _pnlInvoiceInfo.PerformLayout();
            _tabInvoiceDoc.ResumeLayout(false);
            _pnlDocPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picInvoice).EndInit();
            _pnlNoFile.ResumeLayout(false);
            _pnlDocToolbar.ResumeLayout(false);
            _pnlDocToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
