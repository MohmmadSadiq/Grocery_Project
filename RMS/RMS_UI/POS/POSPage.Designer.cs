namespace RMS_UI.POS
{
    partial class POSPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            _pnlCartSidebar = new Panel();
            _pnlActions = new Panel();
            _btnNewSale = new Button();
            _btnCompleteSale = new Button();
            _ctrlPaymentPanel = new Payment.ctrlPaymentPanel();
            _pnlSummary = new Panel();
            _pnlSummaryLine = new Panel();
            _lblTotalLabel = new Label();
            _lblTotalAmount = new Label();
            _lblItemsCount = new Label();
            _dgvCart = new DataGridView();
            _pnlCustomer = new Panel();
            _lblCustomerLabel = new Label();
            _cmbCustomer = new ComboBox();
            _pnlCartHeader = new Panel();
            _lblCartTitle = new Label();
            _lblCartItemCount = new Label();
            _btnClearCart = new Button();
            _pnlSeparator = new Panel();
            _pnlProducts = new Panel();
            _pnlSearchBar = new Panel();
            _txtSearch = new TextBox();
            _flpProducts = new FlowLayoutPanel();
            _pnlCartSidebar.SuspendLayout();
            _pnlActions.SuspendLayout();
            _pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvCart).BeginInit();
            _pnlCustomer.SuspendLayout();
            _pnlCartHeader.SuspendLayout();
            _pnlProducts.SuspendLayout();
            _pnlSearchBar.SuspendLayout();
            SuspendLayout();

            // ─────────────────────────────────────────────────────
            // _pnlCartSidebar  (right panel — cart + payment)
            // ─────────────────────────────────────────────────────
            _pnlCartSidebar.Controls.Add(_dgvCart);
            _pnlCartSidebar.Controls.Add(_pnlCustomer);
            _pnlCartSidebar.Controls.Add(_pnlCartHeader);
            _pnlCartSidebar.Controls.Add(_pnlActions);
            _pnlCartSidebar.Controls.Add(_ctrlPaymentPanel);
            _pnlCartSidebar.Controls.Add(_pnlSummary);
            _pnlCartSidebar.Dock = DockStyle.Right;
            _pnlCartSidebar.Location = new Point(820, 0);
            _pnlCartSidebar.Name = "_pnlCartSidebar";
            _pnlCartSidebar.Size = new Size(380, 683);
            _pnlCartSidebar.TabIndex = 0;

            // ─────────────────────────────────────────────────────
            // _pnlCartHeader  (cart title bar)
            // ─────────────────────────────────────────────────────
            _pnlCartHeader.Controls.Add(_btnClearCart);
            _pnlCartHeader.Controls.Add(_lblCartItemCount);
            _pnlCartHeader.Controls.Add(_lblCartTitle);
            _pnlCartHeader.Dock = DockStyle.Top;
            _pnlCartHeader.Location = new Point(0, 0);
            _pnlCartHeader.Name = "_pnlCartHeader";
            _pnlCartHeader.Padding = new Padding(16, 0, 8, 0);
            _pnlCartHeader.Size = new Size(380, 55);
            _pnlCartHeader.TabIndex = 0;

            // _lblCartTitle
            _lblCartTitle.AutoSize = true;
            _lblCartTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblCartTitle.Location = new Point(16, 14);
            _lblCartTitle.Name = "_lblCartTitle";
            _lblCartTitle.Size = new Size(120, 25);
            _lblCartTitle.TabIndex = 0;
            _lblCartTitle.Text = "🛒 Sales Cart";

            // _lblCartItemCount
            _lblCartItemCount.AutoSize = true;
            _lblCartItemCount.Font = new Font("Segoe UI", 9F);
            _lblCartItemCount.Location = new Point(170, 19);
            _lblCartItemCount.Name = "_lblCartItemCount";
            _lblCartItemCount.Size = new Size(45, 15);
            _lblCartItemCount.TabIndex = 1;
            _lblCartItemCount.Text = "0 items";

            // _btnClearCart
            _btnClearCart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnClearCart.Cursor = Cursors.Hand;
            _btnClearCart.FlatAppearance.BorderSize = 0;
            _btnClearCart.FlatStyle = FlatStyle.Flat;
            _btnClearCart.Font = new Font("Segoe UI", 9F);
            _btnClearCart.Location = new Point(295, 12);
            _btnClearCart.Name = "_btnClearCart";
            _btnClearCart.Size = new Size(75, 30);
            _btnClearCart.TabIndex = 2;
            _btnClearCart.Text = "🗑 Clear";
            _btnClearCart.UseVisualStyleBackColor = false;

            // ─────────────────────────────────────────────────────
            // _pnlCustomer  (optional customer selector)
            // ─────────────────────────────────────────────────────
            _pnlCustomer.Controls.Add(_cmbCustomer);
            _pnlCustomer.Controls.Add(_lblCustomerLabel);
            _pnlCustomer.Dock = DockStyle.Top;
            _pnlCustomer.Location = new Point(0, 55);
            _pnlCustomer.Name = "_pnlCustomer";
            _pnlCustomer.Padding = new Padding(16, 6, 16, 6);
            _pnlCustomer.Size = new Size(380, 44);
            _pnlCustomer.TabIndex = 1;

            // _lblCustomerLabel
            _lblCustomerLabel.AutoSize = true;
            _lblCustomerLabel.Font = new Font("Segoe UI", 9F);
            _lblCustomerLabel.Location = new Point(16, 13);
            _lblCustomerLabel.Name = "_lblCustomerLabel";
            _lblCustomerLabel.Size = new Size(64, 15);
            _lblCustomerLabel.TabIndex = 0;
            _lblCustomerLabel.Text = "Customer:";

            // _cmbCustomer
            _cmbCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCustomer.Font = new Font("Segoe UI", 9F);
            _cmbCustomer.Location = new Point(90, 9);
            _cmbCustomer.Name = "_cmbCustomer";
            _cmbCustomer.Size = new Size(274, 23);
            _cmbCustomer.TabIndex = 1;

            // ─────────────────────────────────────────────────────
            // _dgvCart  (cart items grid)
            // ─────────────────────────────────────────────────────
            _dgvCart.AllowUserToAddRows = false;
            _dgvCart.AllowUserToDeleteRows = false;
            _dgvCart.AllowUserToResizeRows = false;
            _dgvCart.BackgroundColor = SystemColors.Window;
            _dgvCart.BorderStyle = BorderStyle.None;
            _dgvCart.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _dgvCart.ColumnHeadersHeight = 35;
            _dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgvCart.Dock = DockStyle.Fill;
            _dgvCart.Location = new Point(0, 99);
            _dgvCart.MultiSelect = false;
            _dgvCart.Name = "_dgvCart";
            _dgvCart.RowHeadersVisible = false;
            _dgvCart.RowTemplate.Height = 35;
            _dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvCart.Size = new Size(380, 200);
            _dgvCart.TabIndex = 2;

            // ─────────────────────────────────────────────────────
            // _pnlSummary  (totals bar above payment panel)
            // ─────────────────────────────────────────────────────
            _pnlSummary.Controls.Add(_lblTotalAmount);
            _pnlSummary.Controls.Add(_lblTotalLabel);
            _pnlSummary.Controls.Add(_lblItemsCount);
            _pnlSummary.Controls.Add(_pnlSummaryLine);
            _pnlSummary.Dock = DockStyle.Bottom;
            _pnlSummary.Location = new Point(0, 300);
            _pnlSummary.Name = "_pnlSummary";
            _pnlSummary.Padding = new Padding(16, 8, 16, 8);
            _pnlSummary.Size = new Size(380, 75);
            _pnlSummary.TabIndex = 3;

            // _pnlSummaryLine (separator)
            _pnlSummaryLine.Dock = DockStyle.Top;
            _pnlSummaryLine.Location = new Point(16, 0);
            _pnlSummaryLine.Name = "_pnlSummaryLine";
            _pnlSummaryLine.Size = new Size(348, 1);
            _pnlSummaryLine.TabIndex = 0;

            // _lblItemsCount
            _lblItemsCount.AutoSize = true;
            _lblItemsCount.Font = new Font("Segoe UI", 9F);
            _lblItemsCount.Location = new Point(16, 10);
            _lblItemsCount.Name = "_lblItemsCount";
            _lblItemsCount.Size = new Size(45, 15);
            _lblItemsCount.TabIndex = 1;
            _lblItemsCount.Text = "0 items";

            // _lblTotalLabel
            _lblTotalLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblTotalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblTotalLabel.Location = new Point(200, 10);
            _lblTotalLabel.Name = "_lblTotalLabel";
            _lblTotalLabel.Size = new Size(50, 19);
            _lblTotalLabel.TabIndex = 2;
            _lblTotalLabel.Text = "Total:";
            _lblTotalLabel.TextAlign = ContentAlignment.MiddleRight;

            // _lblTotalAmount
            _lblTotalAmount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblTotalAmount.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            _lblTotalAmount.Location = new Point(180, 28);
            _lblTotalAmount.Name = "_lblTotalAmount";
            _lblTotalAmount.Size = new Size(184, 40);
            _lblTotalAmount.TabIndex = 3;
            _lblTotalAmount.Text = "0.00";
            _lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;

            // ─────────────────────────────────────────────────────
            // _ctrlPaymentPanel  (pending-mode payment list)
            // ─────────────────────────────────────────────────────
            _ctrlPaymentPanel.Dock = DockStyle.Bottom;
            _ctrlPaymentPanel.Location = new Point(0, 375);
            _ctrlPaymentPanel.Name = "_ctrlPaymentPanel";
            _ctrlPaymentPanel.Size = new Size(380, 248);
            _ctrlPaymentPanel.TabIndex = 4;

            // ─────────────────────────────────────────────────────
            // _pnlActions  (Complete / New buttons)
            // ─────────────────────────────────────────────────────
            _pnlActions.Controls.Add(_btnCompleteSale);
            _pnlActions.Controls.Add(_btnNewSale);
            _pnlActions.Dock = DockStyle.Bottom;
            _pnlActions.Location = new Point(0, 623);
            _pnlActions.Name = "_pnlActions";
            _pnlActions.Padding = new Padding(12, 8, 12, 8);
            _pnlActions.Size = new Size(380, 60);
            _pnlActions.TabIndex = 5;

            // _btnCompleteSale
            _btnCompleteSale.Cursor = Cursors.Hand;
            _btnCompleteSale.Dock = DockStyle.Fill;
            _btnCompleteSale.FlatAppearance.BorderSize = 0;
            _btnCompleteSale.FlatStyle = FlatStyle.Flat;
            _btnCompleteSale.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _btnCompleteSale.ForeColor = Color.White;
            _btnCompleteSale.Name = "_btnCompleteSale";
            _btnCompleteSale.Size = new Size(260, 44);
            _btnCompleteSale.TabIndex = 0;
            _btnCompleteSale.Text = "✅ Complete Sale";
            _btnCompleteSale.UseVisualStyleBackColor = false;

            // _btnNewSale
            _btnNewSale.Cursor = Cursors.Hand;
            _btnNewSale.Dock = DockStyle.Right;
            _btnNewSale.FlatAppearance.BorderSize = 0;
            _btnNewSale.FlatStyle = FlatStyle.Flat;
            _btnNewSale.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnNewSale.Name = "_btnNewSale";
            _btnNewSale.Size = new Size(90, 44);
            _btnNewSale.TabIndex = 1;
            _btnNewSale.Text = "🔄 New";
            _btnNewSale.UseVisualStyleBackColor = false;

            // ─────────────────────────────────────────────────────
            // _pnlSeparator  (vertical divider line)
            // ─────────────────────────────────────────────────────
            _pnlSeparator.Dock = DockStyle.Right;
            _pnlSeparator.Location = new Point(819, 0);
            _pnlSeparator.Name = "_pnlSeparator";
            _pnlSeparator.Size = new Size(1, 683);
            _pnlSeparator.TabIndex = 1;

            // ─────────────────────────────────────────────────────
            // _pnlProducts  (left panel — search + card grid)
            // ─────────────────────────────────────────────────────
            _pnlProducts.Controls.Add(_flpProducts);
            _pnlProducts.Controls.Add(_pnlSearchBar);
            _pnlProducts.Dock = DockStyle.Fill;
            _pnlProducts.Location = new Point(0, 0);
            _pnlProducts.Name = "_pnlProducts";
            _pnlProducts.Size = new Size(819, 683);
            _pnlProducts.TabIndex = 2;

            // ─────────────────────────────────────────────────────
            // _pnlSearchBar  (top search bar)
            // ─────────────────────────────────────────────────────
            _pnlSearchBar.Controls.Add(_txtSearch);
            _pnlSearchBar.Dock = DockStyle.Top;
            _pnlSearchBar.Location = new Point(0, 0);
            _pnlSearchBar.Name = "_pnlSearchBar";
            _pnlSearchBar.Padding = new Padding(16, 12, 16, 8);
            _pnlSearchBar.Size = new Size(819, 55);
            _pnlSearchBar.TabIndex = 0;

            // _txtSearch
            _txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtSearch.BorderStyle = BorderStyle.None;
            _txtSearch.Font = new Font("Segoe UI", 12F);
            _txtSearch.Location = new Point(28, 14);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.PlaceholderText = "🔍  Search by name or scan barcode...";
            _txtSearch.Size = new Size(763, 22);
            _txtSearch.TabIndex = 0;

            // ─────────────────────────────────────────────────────
            // _flpProducts  (product card grid)
            // ─────────────────────────────────────────────────────
            _flpProducts.AutoScroll = true;
            _flpProducts.Dock = DockStyle.Fill;
            _flpProducts.Location = new Point(0, 55);
            _flpProducts.Name = "_flpProducts";
            _flpProducts.Padding = new Padding(12);
            _flpProducts.Size = new Size(819, 628);
            _flpProducts.TabIndex = 1;
            _flpProducts.WrapContents = true;

            // ─────────────────────────────────────────────────────
            // POSPage
            // ─────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlProducts);
            Controls.Add(_pnlSeparator);
            Controls.Add(_pnlCartSidebar);
            Name = "POSPage";
            Size = new Size(1200, 683);
            _pnlCartSidebar.ResumeLayout(false);
            _pnlActions.ResumeLayout(false);
            _pnlSummary.ResumeLayout(false);
            _pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvCart).EndInit();
            _pnlCustomer.ResumeLayout(false);
            _pnlCustomer.PerformLayout();
            _pnlCartHeader.ResumeLayout(false);
            _pnlCartHeader.PerformLayout();
            _pnlProducts.ResumeLayout(false);
            _pnlSearchBar.ResumeLayout(false);
            _pnlSearchBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlCartSidebar;
        private Panel _pnlCartHeader;
        private Label _lblCartTitle;
        private Label _lblCartItemCount;
        private Button _btnClearCart;
        private Panel _pnlCustomer;
        private Label _lblCustomerLabel;
        private ComboBox _cmbCustomer;
        private DataGridView _dgvCart;
        private Panel _pnlSummary;
        private Panel _pnlSummaryLine;
        private Label _lblItemsCount;
        private Label _lblTotalLabel;
        private Label _lblTotalAmount;
        private Payment.ctrlPaymentPanel _ctrlPaymentPanel;
        private Panel _pnlActions;
        private Button _btnCompleteSale;
        private Button _btnNewSale;
        private Panel _pnlSeparator;
        private Panel _pnlProducts;
        private Panel _pnlSearchBar;
        private TextBox _txtSearch;
        private FlowLayoutPanel _flpProducts;
    }
}
