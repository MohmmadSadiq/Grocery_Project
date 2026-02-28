using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using RMS_Business;
using RMS_UI.Payment;
using RMS_UI.Products;
using RMS_UI.Utilities;

namespace RMS_UI.Suppliers_Purchase
{
    public partial class ctrlAddEditPurchase : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────────
        public event EventHandler<clsPurchase>? PurchaseSaved;
#pragma warning disable CS0067 // Will be used when Edit mode is invoked from a form
        public event EventHandler? CancelClicked;
#pragma warning restore CS0067

        // ── State ─────────────────────────────────────────────────────────────────
        private clsPurchase _purchase;
        private List<clsBatch> _batchList = new List<clsBatch>();
        private int _editingBatchIndex = -1;
        private string? _attachedFilePath = null; // temp path before save
        private WebView2? _webViewPdf = null;     // created at runtime for PDF preview

        // ── Mode ──────────────────────────────────────────────────────────────────
        enum enMode { AddNew = 1, Edit = 2 }
        private enMode Mode { get; set; }

        // ── Invoice storage folder ────────────────────────────────────────────────
        private static readonly string InvoiceFolder =
            Path.Combine(Application.StartupPath, "Invoices");

        // ── Constructors ──────────────────────────────────────────────────────────

        /// <summary>Add New mode.</summary>
        public ctrlAddEditPurchase()
        {
            Mode = enMode.AddNew;
            _purchase = new clsPurchase();
            InitializeComponent();
            _InitControl();
        }

        /// <summary>Edit mode – loads existing purchase by ID.</summary>
        public ctrlAddEditPurchase(int purchaseID)
        {
            _purchase = clsPurchase.Find(purchaseID) ?? new clsPurchase();
            Mode = _purchase.PurchaseID == -1 ? enMode.AddNew : enMode.Edit;
            InitializeComponent();
            _InitControl();
        }

        // ── Initialization ────────────────────────────────────────────────────────

        private void _InitControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _PopulateTransactionStatus();
            _PopulateSuppliers();
            _PopulateEmployees();
            _SetupBatchGridColumns();

            // Payment panel defaults
            _ctrlPaymentPanel.IsPendingMode = true;
            _ctrlPaymentPanel.TotalAmount = 0;

            if (Mode == enMode.Edit)
                _LoadPurchaseData();
            else
            {
                _lblTitle.Text = "🧾  Add New Purchase";
                _lblMode.Text = "Enter purchase details and add batch items below.";
                _btnSave.Text = "💾  Save";

                // defaults
                if (_cmbTransactionStatus.Items.Count > 0)
                    _cmbTransactionStatus.SelectedIndex = 0; // InProgress
            }

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Populate ComboBoxes ───────────────────────────────────────────────────

        private void _PopulateTransactionStatus()
        {
            _cmbTransactionStatus.Items.Clear();
            _cmbTransactionStatus.Items.Add(new ComboBoxItem("In Progress", (byte)clsTransaction.enTransactionStatus.InProgress));
            _cmbTransactionStatus.Items.Add(new ComboBoxItem("Completed", (byte)clsTransaction.enTransactionStatus.Completed));
            _cmbTransactionStatus.Items.Add(new ComboBoxItem("Cancelled", (byte)clsTransaction.enTransactionStatus.Canceld));
            _cmbTransactionStatus.DisplayMember = "Text";
        }

        private void _PopulateSuppliers()
        {
            try
            {
                DataTable dt = clsSupplier.GetAllSupplier();
                _cmbSupplier.Items.Clear();
                _cmbSupplier.Items.Add(new ComboBoxItem("-- Select Supplier --", -1));

                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["SupplierID"]);
                    // Try to build a display name from known columns
                    string name = row.Table.Columns.Contains("SupplierName")
                        ? row["SupplierName"]?.ToString() ?? $"Supplier #{id}"
                        : $"Supplier #{id}";
                    _cmbSupplier.Items.Add(new ComboBoxItem(name, id));
                }

                _cmbSupplier.DisplayMember = "Text";
                _cmbSupplier.SelectedIndex = 0;
            }
            catch { /* supplier list unavailable */ }
        }

        private void _PopulateEmployees()
        {
            try
            {
                DataTable dt = clsEmployee.GetAllEmployee();
                _cmbEmployee.Items.Clear();
                _cmbEmployee.Items.Add(new ComboBoxItem("-- None --", -1));

                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["EmployeeID"]);
                    string name = row.Table.Columns.Contains("EmployeeName")
                        ? row["EmployeeName"]?.ToString() ?? $"Employee #{id}"
                        : $"Employee #{id}";
                    _cmbEmployee.Items.Add(new ComboBoxItem(name, id));
                }

                _cmbEmployee.DisplayMember = "Text";
                _cmbEmployee.SelectedIndex = 0;
            }
            catch { /* employee list unavailable */ }
        }

        // ── DataGridView columns ──────────────────────────────────────────────────

        private void _SetupBatchGridColumns()
        {
            _dgvBatches.Columns.Clear();
            _dgvBatches.AutoGenerateColumns = false;

            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNo",         HeaderText = "#",              Width = 40,  ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductUnit", HeaderText = "Product Unit",   Width = 220, ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty",         HeaderText = "Quantity",       Width = 90,  ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCost",        HeaderText = "Unit Cost",      Width = 100, ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubtotal",    HeaderText = "Subtotal",       Width = 110, ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProdDate",    HeaderText = "Prod. Date",     Width = 100, ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colExpDate",     HeaderText = "Exp. Date",      Width = 100, ReadOnly = true });
            _dgvBatches.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBatchNo",     HeaderText = "Batch #",        Width = 100, ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        // ── Load existing purchase (Edit mode) ───────────────────────────────────

        private void _LoadPurchaseData()
        {
            _lblTitle.Text = "🧾  Edit Purchase";
            _lblMode.Text = $"Editing purchase ID: {_purchase.PurchaseID}";
            _lblID.Text = $"ID: {_purchase.PurchaseID}";
            _btnSave.Text = "💾  Update";

            // Invoice info
            _txtInvoiceNumber.Text = _purchase.InvoiceNumber ?? "";
            _dtpTransactionDate.Value = _purchase.TransactionDate;

            // Status
            for (int i = 0; i < _cmbTransactionStatus.Items.Count; i++)
            {
                if (_cmbTransactionStatus.Items[i] is ComboBoxItem ci &&
                    ci.Value is byte val && val == (byte)_purchase.TransactionStatus)
                {
                    _cmbTransactionStatus.SelectedIndex = i;
                    break;
                }
            }

            // Supplier
            if (_purchase.SupplierID.HasValue)
                _SelectComboByValue(_cmbSupplier, _purchase.SupplierID.Value);

            // Employee
            if (_purchase.PurchasedByEmployeeID.HasValue)
                _SelectComboByValue(_cmbEmployee, _purchase.PurchasedByEmployeeID.Value);

            // Batches
            if (_purchase.PurchaseBatches != null)
            {
                _batchList = new List<clsBatch>(_purchase.PurchaseBatches);
                _RefreshBatchGrid();
            }

            // Notes
            _txtNotes.Text = _purchase.Nots ?? "";

            // Payment panel — switch to persisted mode
            _ctrlPaymentPanel.TransactionID = _purchase.TransactionID;
            _ctrlPaymentPanel.TotalAmount = _batchList.Sum(b => b.TotalQuantity * b.UniteCostPrice);
            _ctrlPaymentPanel.LoadPayments();

            // Invoice document
            if (!string.IsNullOrWhiteSpace(_purchase.InvoiceDocumentPath) &&
                File.Exists(_purchase.InvoiceDocumentPath))
            {
                _attachedFilePath = _purchase.InvoiceDocumentPath;
                _ShowFileInfo(_attachedFilePath);
                _PreviewDocument(_attachedFilePath);
            }
        }

        // ── Batch Management ──────────────────────────────────────────────────────

        private void _btnAddBatch_Click(object sender, EventArgs e)
        {
            if (!_ValidateBatchInput()) return;

            var batch = _BuildBatchFromInputs();

            if (_editingBatchIndex >= 0 && _editingBatchIndex < _batchList.Count)
            {
                // Updating existing batch
                _batchList[_editingBatchIndex] = batch;
                _editingBatchIndex = -1;
                _btnAddBatch.Text = "➕ Add";
            }
            else
            {
                _batchList.Add(batch);
            }

            _RefreshBatchGrid();
            _ClearBatchInputs();
        }

        private void _btnEditBatch_Click(object sender, EventArgs e)
        {
            if (_dgvBatches.CurrentRow == null) return;

            int idx = _dgvBatches.CurrentRow.Index;
            if (idx < 0 || idx >= _batchList.Count) return;

            _editingBatchIndex = idx;
            var batch = _batchList[idx];

            // Fill inputs from selected batch
            _ctrlProductFinder.SetProductUnitByID(batch.ProductUnitID);
            _numQuantity.Value = batch.TotalQuantity > 0 ? batch.TotalQuantity : _numQuantity.Minimum;
            _numUnitCost.Value = batch.UniteCostPrice > 0 ? batch.UniteCostPrice : _numUnitCost.Minimum;

            _dtpProductionDate.Checked = batch.ProductionDate.HasValue;
            if (batch.ProductionDate.HasValue) _dtpProductionDate.Value = batch.ProductionDate.Value;

            _dtpExpiryDate.Checked = batch.ExpiryDate.HasValue;
            if (batch.ExpiryDate.HasValue) _dtpExpiryDate.Value = batch.ExpiryDate.Value;

            _txtBatchNumber.Text = batch.BatchNumber ?? "";

            _btnAddBatch.Text = "✅ Update";
        }

        private void _btnRemoveBatch_Click(object sender, EventArgs e)
        {
            if (_dgvBatches.CurrentRow == null) return;

            int idx = _dgvBatches.CurrentRow.Index;
            if (idx < 0 || idx >= _batchList.Count) return;

            _batchList.RemoveAt(idx);

            if (_editingBatchIndex == idx)
            {
                _editingBatchIndex = -1;
                _btnAddBatch.Text = "➕ Add";
                _ClearBatchInputs();
            }

            _RefreshBatchGrid();
        }

        private clsBatch _BuildBatchFromInputs()
        {
            var batch = new clsBatch();
            batch.ProductUnitID = _ctrlProductFinder.SelectedProductUnit?.ProductUnitID ?? -1;
            batch.TotalQuantity = _numQuantity.Value;
            batch.UniteCostPrice = _numUnitCost.Value;
            batch.ProductionDate = _dtpProductionDate.Checked ? _dtpProductionDate.Value : (DateTime?)null;
            batch.ExpiryDate = _dtpExpiryDate.Checked ? _dtpExpiryDate.Value : (DateTime?)null;
            batch.BatchNumber = string.IsNullOrWhiteSpace(_txtBatchNumber.Text) ? null : _txtBatchNumber.Text.Trim();
            return batch;
        }

        private void _ClearBatchInputs()
        {
            _ctrlProductFinder.ResetAll();
            _numQuantity.Value = _numQuantity.Minimum;
            _numUnitCost.Value = _numUnitCost.Minimum;
            _dtpProductionDate.Checked = false;
            _dtpExpiryDate.Checked = false;
            _txtBatchNumber.Text = "";
        }

        /// <summary>
        /// Resets the control back to Add-New mode, clearing all fields and batches.
        /// </summary>
        public void BackToAddNewMode()
        {
            // Reset mode and purchase object
            Mode = enMode.AddNew;
            _purchase = new clsPurchase();

            // Reset header labels and button
            _lblTitle.Text = "🧾  Add New Purchase";
            _lblMode.Text = "Enter purchase details and add batch items below.";
            _btnSave.Text = "💾  Save";

            // Reset combo boxes to defaults
            if (_cmbTransactionStatus.Items.Count > 0)
                _cmbTransactionStatus.SelectedIndex = 0; // InProgress

            if (_cmbSupplier.Items.Count > 0)
                _cmbSupplier.SelectedIndex = -1;

            if (_cmbEmployee.Items.Count > 0)
                _cmbEmployee.SelectedIndex = -1;

            // Clear notes
            _txtNotes.Text = "";

            // Clear batches
            _batchList.Clear();
            _editingBatchIndex = -1;
            _dgvBatches.Rows.Clear();

            // Clear batch inputs
            _ClearBatchInputs();

            // Clear attached file
            _attachedFilePath = null;
            _lblFileName.Text = "No file attached";
            _lblFileSize.Text = "";
            _btnRemoveFile.Visible = false;

            _picInvoice.Image?.Dispose();
            _picInvoice.Image = null;
            _picInvoice.Visible = false;

            if (_webViewPdf != null) _webViewPdf.Visible = false;
            _pnlNoFile.Visible = true;

            // Reset payment panel
            _ctrlPaymentPanel.ClearPendingPayments();
            _ctrlPaymentPanel.IsPendingMode = true;
            _ctrlPaymentPanel.TransactionID = -1;
            _ctrlPaymentPanel.TotalAmount = 0;
        }

        private void _RefreshBatchGrid()
        {
            _dgvBatches.Rows.Clear();

            for (int i = 0; i < _batchList.Count; i++)
            {
                var b = _batchList[i];
                decimal subtotal = b.TotalQuantity * b.UniteCostPrice;

                // Find product unit name
                string productName = $"PU#{b.ProductUnitID}";
                var pu = clsProductUnit.Find(b.ProductUnitID);
                if (pu != null)
                {
                    string pName = pu.ProductInfo?.ProductName ?? "";
                    string uName = pu.UnitInfo?.UnitName ?? "";
                    productName = string.IsNullOrEmpty(uName) ? pName : $"{pName} ({uName})";
                }

                _dgvBatches.Rows.Add(
                    (i + 1).ToString(),
                    productName,
                    b.TotalQuantity.ToString("N2"),
                    b.UniteCostPrice.ToString("N2"),
                    subtotal.ToString("N2"),
                    b.ProductionDate?.ToString("yyyy-MM-dd") ?? "—",
                    b.ExpiryDate?.ToString("yyyy-MM-dd") ?? "—",
                    b.BatchNumber ?? "—"
                );
            }

            _UpdateTotalAmount();
        }

        private void _UpdateTotalAmount()
        {
            decimal total = _batchList.Sum(b => b.TotalQuantity * b.UniteCostPrice);
            _lblTotalAmount.Text = total.ToString("N2");
            _ctrlPaymentPanel.TotalAmount = total;
        }

        // ── Invoice Document ──────────────────────────────────────────────────────

        private void _btnBrowseFile_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Title = "Select Invoice Document";
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|PDF Files|*.pdf|All Files|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _attachedFilePath = dlg.FileName;
                _ShowFileInfo(_attachedFilePath);
                _PreviewDocument(_attachedFilePath);
            }
        }

        private void _btnRemoveFile_Click(object sender, EventArgs e)
        {
            _attachedFilePath = null;
            _lblFileName.Text = "No file attached";
            _lblFileSize.Text = "";
            _btnRemoveFile.Visible = false;

            _picInvoice.Image?.Dispose();
            _picInvoice.Image = null;
            _picInvoice.Visible = false;

            if (_webViewPdf != null) _webViewPdf.Visible = false;
            _pnlNoFile.Visible = true;
        }

        private void _ShowFileInfo(string path)
        {
            var fi = new FileInfo(path);
            _lblFileName.Text = fi.Name;
            double sizeKB = fi.Length / 1024.0;
            _lblFileSize.Text = sizeKB >= 1024
                ? $"({sizeKB / 1024.0:N1} MB)"
                : $"({sizeKB:N0} KB)";
            _btnRemoveFile.Visible = true;
        }

        private void _PreviewDocument(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext == ".pdf")
            {
                _picInvoice.Visible = false;
                _pnlNoFile.Visible = false;
                _EnsureWebView2();
                if (_webViewPdf != null)
                {
                    _webViewPdf.Visible = true;
                    _webViewPdf.Source = new Uri(path);
                }
            }
            else // image
            {
                if (_webViewPdf != null) _webViewPdf.Visible = false;
                _pnlNoFile.Visible = false;
                try
                {
                    // Load without locking the file
                    using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    _picInvoice.Image?.Dispose();
                    _picInvoice.Image = Image.FromStream(stream);
                    _picInvoice.Visible = true;
                }
                catch
                {
                    _picInvoice.Visible = false;
                    _pnlNoFile.Visible = true;
                    _notification.ShowError("Failed to load image preview.");
                }
            }
        }

        private void _EnsureWebView2()
        {
            if (_webViewPdf != null) return;

            try
            {
                _webViewPdf = new WebView2();
                _webViewPdf.Dock = DockStyle.Fill;
                _webViewPdf.Visible = false;
                _pnlDocPreview.Controls.Add(_webViewPdf);
                _webViewPdf.BringToFront();
            }
            catch
            {
                _notification.ShowWarning("WebView2 runtime is not installed. PDF preview is unavailable.");
            }
        }

        private string? _SaveInvoiceFile()
        {
            if (string.IsNullOrEmpty(_attachedFilePath) || !File.Exists(_attachedFilePath))
                return null;

            // If the file is already in the Invoices folder (Edit mode), keep it
            if (_attachedFilePath.StartsWith(InvoiceFolder, StringComparison.OrdinalIgnoreCase))
                return _attachedFilePath;

            // Create folder if needed
            if (!Directory.Exists(InvoiceFolder))
                Directory.CreateDirectory(InvoiceFolder);

            string ext = Path.GetExtension(_attachedFilePath);
            string destName = $"Purchase_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString("N")[..8]}{ext}";
            string destPath = Path.Combine(InvoiceFolder, destName);

            File.Copy(_attachedFilePath, destPath, true);
            return destPath;
        }

        // ── Validation ────────────────────────────────────────────────────────────

        private bool _ValidateBatchInput()
        {
            _errorProvider.Clear();
            bool valid = true;

            var selectedProduct = _ctrlProductFinder.SelectedProductUnit;
            if (selectedProduct == null)
            {
                _errorProvider.SetError(_ctrlProductFinder, "Please search and select a product unit.");
                valid = false;
            }

            if (_numUnitCost.Value <= 0)
            {
                _errorProvider.SetError(_numUnitCost, "Unit cost must be greater than zero.");
                valid = false;
            }

            if (!valid)
                _notification.ShowWarning("Please fill in batch item fields correctly.");

            return valid;
        }

        private bool _ValidateInput()
        {
            bool isValid = true;
            _errorProvider.Clear();
            _notification.HideImmediately();

            // Supplier is required
            var selectedSupplier = _cmbSupplier.SelectedItem as ComboBoxItem;
            if (selectedSupplier == null || (selectedSupplier.Value is int sv && sv == -1))
            {
                _errorProvider.SetError(_cmbSupplier, "Please select a supplier.");
                isValid = false;
            }

            // At least one batch required
            if (_batchList.Count == 0)
            {
                _errorProvider.SetError(_dgvBatches, "Please add at least one batch item.");
                isValid = false;
            }

            // Status is required
            if (_cmbTransactionStatus.SelectedItem == null)
            {
                _errorProvider.SetError(_cmbTransactionStatus, "Please select a status.");
                isValid = false;
            }

            if (!isValid)
                _notification.ShowWarning("Please fix the highlighted fields before saving.");

            return isValid;
        }

        // ── Save ──────────────────────────────────────────────────────────────────

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInput()) return;

            // ── Map fields → _purchase ───────────────────────────────────────
            _purchase.TransactionType = clsTransaction.enTransactionType.Purchase;
            _purchase.TransactionDate = _dtpTransactionDate.Value;
            _purchase.InvoiceNumber = string.IsNullOrWhiteSpace(_txtInvoiceNumber.Text) ? null : _txtInvoiceNumber.Text.Trim();

            // Status
            if (_cmbTransactionStatus.SelectedItem is ComboBoxItem statusItem && statusItem.Value is byte statusVal)
                _purchase.TransactionStatus = (clsTransaction.enTransactionStatus)statusVal;

            // Supplier
            if (_cmbSupplier.SelectedItem is ComboBoxItem supItem && supItem.Value is int supId && supId > 0)
                _purchase.SupplierID = supId;
            else
                _purchase.SupplierID = null;

            // Employee
            if (_cmbEmployee.SelectedItem is ComboBoxItem empItem && empItem.Value is int empId && empId > 0)
                _purchase.PurchasedByEmployeeID = empId;
            else
                _purchase.PurchasedByEmployeeID = null;

            // Total
            _purchase.TotalAmount = _batchList.Sum(b => b.TotalQuantity * b.UniteCostPrice);

            // Batches
            _purchase.PurchaseBatches = _batchList;

            // Notes
            _purchase.Nots = string.IsNullOrWhiteSpace(_txtNotes.Text) ? null : _txtNotes.Text.Trim();

            // Invoice document
            string? savedDocPath = _SaveInvoiceFile();
            _purchase.InvoiceDocumentPath = savedDocPath;

            // User tracking
            int? currentUserID = clsGlobalUser.CurrentUser?.UserID;
            if (Mode == enMode.AddNew)
                _purchase.CreatedByUserID = currentUserID;
            else
                _purchase.UpdatedByUserID = currentUserID;

            // ── Persist ──────────────────────────────────────────────────────
            bool saved = _purchase.Save();

            if (saved)
            {
                string msg = Mode == enMode.AddNew
                    ? "Purchase added successfully!"
                    : "Purchase updated successfully!";

                // Persist any pending payments now that we have a TransactionID
                var pendingPayments = _ctrlPaymentPanel.GetPendingPayments();
                foreach (var payment in pendingPayments)
                {
                    payment.Allocations.Clear();
                    payment.Allocations.Add(new clsPaymentAllocation
                    {
                        TransactionID = _purchase.TransactionID,
                        Amount = payment.PaymentAmount
                    });
                    payment.Save();
                }
                _ctrlPaymentPanel.ClearPendingPayments();

                Mode = enMode.Edit;
                _LoadPurchaseData();

                _notification.ShowSuccess(msg);
                PurchaseSaved?.Invoke(this, _purchase);
            }
            else
            {
                _notification.ShowError("Failed to save the purchase. Please try again.");
            }
        }

        // ── Reset ─────────────────────────────────────────────────────────────────

        private void _btnReset_Click(object sender, EventArgs e)
        {
            _purchase = new clsPurchase();
            Mode = enMode.AddNew;
            _batchList.Clear();
            _editingBatchIndex = -1;

            _lblTitle.Text = "🧾  Add New Purchase";
            _lblMode.Text = "Enter purchase details and add batch items below.";
            _lblID.Text = "ID: N/A";
            _btnSave.Text = "💾  Save";
            _btnReset.Visible = true;

            _txtInvoiceNumber.Text = "";
            _dtpTransactionDate.Value = DateTime.Now;
            if (_cmbTransactionStatus.Items.Count > 0) _cmbTransactionStatus.SelectedIndex = 0;
            if (_cmbSupplier.Items.Count > 0) _cmbSupplier.SelectedIndex = 0;
            if (_cmbEmployee.Items.Count > 0) _cmbEmployee.SelectedIndex = 0;
            _txtNotes.Text = "";

            _ClearBatchInputs();
            _RefreshBatchGrid();
            _btnRemoveFile_Click(this, EventArgs.Empty); // clear attached file

            // Reset payment panel
            _ctrlPaymentPanel.ClearPendingPayments();
            _ctrlPaymentPanel.IsPendingMode = true;
            _ctrlPaymentPanel.TransactionID = -1;
            _ctrlPaymentPanel.TotalAmount = 0;

            _errorProvider.Clear();
            _notification.HideImmediately();
        }

        // ── Theme ─────────────────────────────────────────────────────────────────

        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;

            // Root
            BackColor = c.FormBackground;

            // Header
            _pnlHeader.BackColor = c.ContentBackground;
            _lblTitle.ForeColor = c.TitleText;
            _lblMode.ForeColor = c.SecondaryText;

            // Save
            _btnSave.BackColor = c.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = c.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Reset / Cancel
            _btnReset.BackColor = c.FormBackground;
            _btnReset.ForeColor = c.SecondaryText;
            _btnReset.FlatAppearance.BorderColor = c.BorderColor;
            _btnReset.FlatAppearance.MouseOverBackColor = c.ButtonHover;

            // Inner tabs
            _tabInner.BackColor = c.FormBackground;

            // Card panels (invoice info, supplier info, batch section, footer)
            foreach (var pnl in new[] { _pnlInvoiceInfo, _pnlSupplierInfo, _pnlBatchSection, _pnlFooter })
            {
                pnl.BackColor = c.ContentBackground;
            }

            // Section labels
            foreach (var lbl in new[] { _lblSectionInvoice, _lblSectionSupplier, _lblSectionBatch, _lblSectionNotes })
            {
                lbl.ForeColor = c.Primary;
            }

            // Separators
            foreach (var sep in new[] { _pnlSep1, _pnlSep2, _pnlSep3, _pnlSep4 })
            {
                sep.BackColor = c.BorderColor;
            }

            // Field labels
            foreach (var lbl in new[] { _lblInvoiceNumber, _lblTransactionDate, _lblTransactionStatus,
                                         _lblSupplier, _lblEmployee, _lblNotes,
                                         _lblQuantity, _lblUnitCost,
                                         _lblProductionDate, _lblExpiryDate, _lblBatchNumber })
            {
                lbl.ForeColor = c.SecondaryText;
            }

            _lblID.ForeColor = c.SecondaryText;
            _lblTotalLabel.ForeColor = c.SecondaryText;

            // TextBoxes
            foreach (var txt in new[] { _txtInvoiceNumber, _txtBatchNumber, _txtNotes })
            {
                txt.BackColor = c.ContentBackground;
                txt.ForeColor = c.PrimaryText;
            }

            // ComboBoxes
            foreach (var cmb in new[] { _cmbTransactionStatus, _cmbSupplier, _cmbEmployee })
            {
                cmb.BackColor = c.ContentBackground;
                cmb.ForeColor = c.PrimaryText;
            }

            // Batch input panel
            _pnlBatchInput.BackColor = c.FormBackground;

            // DataGridView
            _dgvBatches.BackgroundColor = c.ContentBackground;
            _dgvBatches.GridColor = c.BorderColor;
            _dgvBatches.DefaultCellStyle.BackColor = c.ContentBackground;
            _dgvBatches.DefaultCellStyle.ForeColor = c.PrimaryText;
            _dgvBatches.DefaultCellStyle.SelectionBackColor = c.PrimaryLight;
            _dgvBatches.DefaultCellStyle.SelectionForeColor = c.PrimaryText;
            _dgvBatches.ColumnHeadersDefaultCellStyle.BackColor = c.FormBackground;
            _dgvBatches.ColumnHeadersDefaultCellStyle.ForeColor = c.SecondaryText;
            _dgvBatches.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _dgvBatches.AlternatingRowsDefaultCellStyle.BackColor = c.FormBackground;

            // Total panel
            _pnlTotal.BackColor = Color.FromArgb(240, 253, 244);

            // Doc tab toolbar
            _pnlDocToolbar.BackColor = c.ContentBackground;
            _lblFileName.ForeColor = c.SecondaryText;
            _lblFileSize.ForeColor = Color.FromArgb(148, 163, 184);
            _pnlDocPreview.BackColor = c.FormBackground;
            _pnlNoFile.BackColor = c.FormBackground;
            _lblNoFile.ForeColor = Color.FromArgb(148, 163, 184);

            // Browse button
            _btnBrowseFile.BackColor = c.Primary;
            _btnBrowseFile.ForeColor = Color.White;
            _btnBrowseFile.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Add batch button
            _btnAddBatch.BackColor = c.Primary;
            _btnAddBatch.ForeColor = Color.White;
            _btnAddBatch.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Edit batch button
            _btnEditBatch.BackColor = c.PrimaryLight;
            _btnEditBatch.ForeColor = c.Primary;
            _btnEditBatch.FlatAppearance.BorderColor = c.BorderAccent;
            _btnEditBatch.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            // Payment panel
            _ctrlPaymentPanel.ApplyTheme();

            Invalidate();
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void _pnlCard_Paint(object sender, PaintEventArgs e)
        {
            // Optional: draw subtle border around card panels  
            if (sender is Panel pnl)
            {
                var c = ThemeManager.Colors;
                using var pen = new Pen(c.BorderColor, 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        // ── ComboBoxItem (reusable helper) ────────────────────────────────────────

        private class ComboBoxItem
        {
            public string Text { get; }
            public object Value { get; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
