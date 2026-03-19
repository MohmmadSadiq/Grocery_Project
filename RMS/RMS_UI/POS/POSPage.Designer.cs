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
            _ctrlSaleCart = new Cart.ctrlSaleCart();
            _pnlCustomer = new Panel();
            _cmbCustomer = new ComboBox();
            _lblCustomerLabel = new Label();
            _pnlCartHeader = new Panel();
            _btnClearCart = new Button();
            _lblCartItemCount = new Label();
            _lblCartTitle = new Label();
            _pnlActions = new Panel();
            _btnCompleteSale = new Button();
            _btnNewSale = new Button();
            _ctrlPaymentPanel = new Payment.ctrlPaymentPanel();
            _pnlSummary = new Panel();
            _lblTotalAmount = new Label();
            _lblTotalLabel = new Label();
            _lblItemsCount = new Label();
            _pnlSummaryLine = new Panel();
            _pnlSeparator = new Panel();
            _pnlProducts = new Panel();
            _flpProducts = new FlowLayoutPanel();
            _pnlFinderActions = new Panel();
            _btnAddFinderSelection = new Button();
            _ctrlProductFinder = new Products.ctrlProductFinder();
            _pnlPaging = new Panel();
            _btnPrevPage = new Button();
            _lblPageInfo = new Label();
            _btnNextPage = new Button();
            _lblPageSize = new Label();
            _cmbPageSize = new ComboBox();
            _pnlSearchBar = new Panel();
            _txtSearch = new TextBox();
            _pnlCartSidebar.SuspendLayout();
            _pnlCustomer.SuspendLayout();
            _pnlCartHeader.SuspendLayout();
            _pnlActions.SuspendLayout();
            _pnlSummary.SuspendLayout();
            _pnlProducts.SuspendLayout();
            _pnlPaging.SuspendLayout();
            _pnlSearchBar.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlCartSidebar
            // 
            _pnlCartSidebar.Controls.Add(_ctrlSaleCart);
            _pnlCartSidebar.Controls.Add(_pnlCustomer);
            _pnlCartSidebar.Controls.Add(_pnlCartHeader);
            _pnlCartSidebar.Controls.Add(_pnlActions);
            _pnlCartSidebar.Controls.Add(_ctrlPaymentPanel);
            _pnlCartSidebar.Controls.Add(_pnlSummary);
            _pnlCartSidebar.Dock = DockStyle.Right;
            _pnlCartSidebar.Location = new Point(766, 0);
            _pnlCartSidebar.Name = "_pnlCartSidebar";
            _pnlCartSidebar.Size = new Size(434, 683);
            _pnlCartSidebar.TabIndex = 0;
            // 
            // _ctrlSaleCart
            // 
            _ctrlSaleCart.BackColor = Color.FromArgb(245, 247, 250);
            _ctrlSaleCart.Dock = DockStyle.Fill;
            _ctrlSaleCart.Location = new Point(0, 99);
            _ctrlSaleCart.Name = "_ctrlSaleCart";
            _ctrlSaleCart.Size = new Size(434, 201);
            _ctrlSaleCart.TabIndex = 2;
            // 
            // _pnlCustomer
            // 
            _pnlCustomer.Controls.Add(_cmbCustomer);
            _pnlCustomer.Controls.Add(_lblCustomerLabel);
            _pnlCustomer.Dock = DockStyle.Top;
            _pnlCustomer.Location = new Point(0, 55);
            _pnlCustomer.Name = "_pnlCustomer";
            _pnlCustomer.Padding = new Padding(16, 6, 16, 6);
            _pnlCustomer.Size = new Size(434, 44);
            _pnlCustomer.TabIndex = 1;
            // 
            // _cmbCustomer
            // 
            _cmbCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCustomer.Font = new Font("Segoe UI", 9F);
            _cmbCustomer.Location = new Point(90, 9);
            _cmbCustomer.Name = "_cmbCustomer";
            _cmbCustomer.Size = new Size(328, 23);
            _cmbCustomer.TabIndex = 1;
            // 
            // _lblCustomerLabel
            // 
            _lblCustomerLabel.AutoSize = true;
            _lblCustomerLabel.Font = new Font("Segoe UI", 9F);
            _lblCustomerLabel.Location = new Point(16, 13);
            _lblCustomerLabel.Name = "_lblCustomerLabel";
            _lblCustomerLabel.Size = new Size(62, 15);
            _lblCustomerLabel.TabIndex = 0;
            _lblCustomerLabel.Text = "Customer:";
            // 
            // _pnlCartHeader
            // 
            _pnlCartHeader.Controls.Add(_btnClearCart);
            _pnlCartHeader.Controls.Add(_lblCartItemCount);
            _pnlCartHeader.Controls.Add(_lblCartTitle);
            _pnlCartHeader.Dock = DockStyle.Top;
            _pnlCartHeader.Location = new Point(0, 0);
            _pnlCartHeader.Name = "_pnlCartHeader";
            _pnlCartHeader.Padding = new Padding(16, 0, 8, 0);
            _pnlCartHeader.Size = new Size(434, 55);
            _pnlCartHeader.TabIndex = 0;
            // 
            // _btnClearCart
            // 
            _btnClearCart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnClearCart.Cursor = Cursors.Hand;
            _btnClearCart.FlatAppearance.BorderSize = 0;
            _btnClearCart.FlatStyle = FlatStyle.Flat;
            _btnClearCart.Font = new Font("Segoe UI", 9F);
            _btnClearCart.Location = new Point(349, 12);
            _btnClearCart.Name = "_btnClearCart";
            _btnClearCart.Size = new Size(75, 30);
            _btnClearCart.TabIndex = 2;
            _btnClearCart.Text = "🗑 Clear";
            _btnClearCart.UseVisualStyleBackColor = false;
            // 
            // _lblCartItemCount
            // 
            _lblCartItemCount.AutoSize = true;
            _lblCartItemCount.Font = new Font("Segoe UI", 9F);
            _lblCartItemCount.Location = new Point(170, 19);
            _lblCartItemCount.Name = "_lblCartItemCount";
            _lblCartItemCount.Size = new Size(45, 15);
            _lblCartItemCount.TabIndex = 1;
            _lblCartItemCount.Text = "0 items";
            // 
            // _lblCartTitle
            // 
            _lblCartTitle.AutoSize = true;
            _lblCartTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblCartTitle.Location = new Point(16, 14);
            _lblCartTitle.Name = "_lblCartTitle";
            _lblCartTitle.Size = new Size(126, 25);
            _lblCartTitle.TabIndex = 0;
            _lblCartTitle.Text = "\U0001f6d2 Sales Cart";
            // 
            // _pnlActions
            // 
            _pnlActions.Controls.Add(_btnCompleteSale);
            _pnlActions.Controls.Add(_btnNewSale);
            _pnlActions.Dock = DockStyle.Bottom;
            _pnlActions.Location = new Point(0, 300);
            _pnlActions.Name = "_pnlActions";
            _pnlActions.Padding = new Padding(12, 8, 12, 8);
            _pnlActions.Size = new Size(434, 60);
            _pnlActions.TabIndex = 5;
            // 
            // _btnCompleteSale
            // 
            _btnCompleteSale.Cursor = Cursors.Hand;
            _btnCompleteSale.Dock = DockStyle.Fill;
            _btnCompleteSale.FlatAppearance.BorderSize = 0;
            _btnCompleteSale.FlatStyle = FlatStyle.Flat;
            _btnCompleteSale.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _btnCompleteSale.ForeColor = Color.White;
            _btnCompleteSale.Location = new Point(12, 8);
            _btnCompleteSale.Name = "_btnCompleteSale";
            _btnCompleteSale.Size = new Size(320, 44);
            _btnCompleteSale.TabIndex = 0;
            _btnCompleteSale.Text = "✅ Complete Sale";
            _btnCompleteSale.UseVisualStyleBackColor = false;
            // 
            // _btnNewSale
            // 
            _btnNewSale.Cursor = Cursors.Hand;
            _btnNewSale.Dock = DockStyle.Right;
            _btnNewSale.FlatAppearance.BorderSize = 0;
            _btnNewSale.FlatStyle = FlatStyle.Flat;
            _btnNewSale.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnNewSale.Location = new Point(332, 8);
            _btnNewSale.Name = "_btnNewSale";
            _btnNewSale.Size = new Size(90, 44);
            _btnNewSale.TabIndex = 1;
            _btnNewSale.Text = "🔄 New";
            _btnNewSale.UseVisualStyleBackColor = false;
            // 
            // _ctrlPaymentPanel
            // 
            _ctrlPaymentPanel.BackColor = Color.FromArgb(255, 255, 255);
            _ctrlPaymentPanel.Dock = DockStyle.Bottom;
            _ctrlPaymentPanel.Location = new Point(0, 360);
            _ctrlPaymentPanel.Name = "_ctrlPaymentPanel";
            _ctrlPaymentPanel.Size = new Size(434, 248);
            _ctrlPaymentPanel.TabIndex = 4;
            // 
            // _pnlSummary
            // 
            _pnlSummary.Controls.Add(_lblTotalAmount);
            _pnlSummary.Controls.Add(_lblTotalLabel);
            _pnlSummary.Controls.Add(_lblItemsCount);
            _pnlSummary.Controls.Add(_pnlSummaryLine);
            _pnlSummary.Dock = DockStyle.Bottom;
            _pnlSummary.Location = new Point(0, 608);
            _pnlSummary.Name = "_pnlSummary";
            _pnlSummary.Padding = new Padding(16, 8, 16, 8);
            _pnlSummary.Size = new Size(434, 75);
            _pnlSummary.TabIndex = 3;
            // 
            // _lblTotalAmount
            // 
            _lblTotalAmount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblTotalAmount.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            _lblTotalAmount.Location = new Point(234, 28);
            _lblTotalAmount.Name = "_lblTotalAmount";
            _lblTotalAmount.Size = new Size(184, 40);
            _lblTotalAmount.TabIndex = 3;
            _lblTotalAmount.Text = "0.00";
            _lblTotalAmount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _lblTotalLabel
            // 
            _lblTotalLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblTotalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblTotalLabel.Location = new Point(254, 10);
            _lblTotalLabel.Name = "_lblTotalLabel";
            _lblTotalLabel.Size = new Size(50, 19);
            _lblTotalLabel.TabIndex = 2;
            _lblTotalLabel.Text = "Total:";
            _lblTotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _lblItemsCount
            // 
            _lblItemsCount.AutoSize = true;
            _lblItemsCount.Font = new Font("Segoe UI", 9F);
            _lblItemsCount.Location = new Point(16, 10);
            _lblItemsCount.Name = "_lblItemsCount";
            _lblItemsCount.Size = new Size(45, 15);
            _lblItemsCount.TabIndex = 1;
            _lblItemsCount.Text = "0 items";
            // 
            // _pnlSummaryLine
            // 
            _pnlSummaryLine.Dock = DockStyle.Top;
            _pnlSummaryLine.Location = new Point(16, 8);
            _pnlSummaryLine.Name = "_pnlSummaryLine";
            _pnlSummaryLine.Size = new Size(402, 1);
            _pnlSummaryLine.TabIndex = 0;
            // 
            // _pnlSeparator
            // 
            _pnlSeparator.Dock = DockStyle.Right;
            _pnlSeparator.Location = new Point(765, 0);
            _pnlSeparator.Name = "_pnlSeparator";
            _pnlSeparator.Size = new Size(1, 683);
            _pnlSeparator.TabIndex = 1;
            // 
            // _pnlProducts
            // 
            _pnlProducts.Controls.Add(_flpProducts);
            _pnlProducts.Controls.Add(_pnlFinderActions);
            _pnlProducts.Controls.Add(_ctrlProductFinder);
            _pnlProducts.Controls.Add(_pnlPaging);
            _pnlProducts.Controls.Add(_pnlSearchBar);
            _pnlProducts.Dock = DockStyle.Fill;
            _pnlProducts.Location = new Point(0, 0);
            _pnlProducts.Name = "_pnlProducts";
            _pnlProducts.Size = new Size(765, 683);
            _pnlProducts.TabIndex = 2;
            // 
            // _flpProducts
            // 
            _flpProducts.AutoScroll = true;
            _flpProducts.Dock = DockStyle.Fill;
            _flpProducts.Location = new Point(0, 91);
            _flpProducts.Name = "_flpProducts";
            _flpProducts.Padding = new Padding(12);
            _flpProducts.Size = new Size(765, 550);
            _flpProducts.TabIndex = 1;
            // 
            // _pnlFinderActions
            // 
            _pnlFinderActions.Controls.Add(_btnAddFinderSelection);
            _pnlFinderActions.Dock = DockStyle.Bottom;
            _pnlFinderActions.Location = new Point(0, 641);
            _pnlFinderActions.Name = "_pnlFinderActions";
            _pnlFinderActions.Padding = new Padding(16, 6, 16, 6);
            _pnlFinderActions.Size = new Size(765, 42);
            _pnlFinderActions.TabIndex = 4;
            // 
            // _btnAddFinderSelection
            // 
            _btnAddFinderSelection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnAddFinderSelection.Cursor = Cursors.Hand;
            _btnAddFinderSelection.FlatAppearance.BorderSize = 0;
            _btnAddFinderSelection.FlatStyle = FlatStyle.Flat;
            _btnAddFinderSelection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnAddFinderSelection.Location = new Point(575, 6);
            _btnAddFinderSelection.Name = "_btnAddFinderSelection";
            _btnAddFinderSelection.Size = new Size(174, 30);
            _btnAddFinderSelection.TabIndex = 0;
            _btnAddFinderSelection.Text = "➕ Add Selected To Cart";
            _btnAddFinderSelection.UseVisualStyleBackColor = true;
            // 
            // _ctrlProductFinder
            // 
            _ctrlProductFinder.Dock = DockStyle.Bottom;
            _ctrlProductFinder.Location = new Point(0, 461);
            _ctrlProductFinder.Name = "_ctrlProductFinder";
            _ctrlProductFinder.Size = new Size(765, 180);
            _ctrlProductFinder.TabIndex = 3;
            // 
            // _pnlPaging
            // 
            _pnlPaging.Controls.Add(_cmbPageSize);
            _pnlPaging.Controls.Add(_lblPageSize);
            _pnlPaging.Controls.Add(_btnNextPage);
            _pnlPaging.Controls.Add(_lblPageInfo);
            _pnlPaging.Controls.Add(_btnPrevPage);
            _pnlPaging.Dock = DockStyle.Top;
            _pnlPaging.Location = new Point(0, 55);
            _pnlPaging.Name = "_pnlPaging";
            _pnlPaging.Padding = new Padding(16, 4, 16, 4);
            _pnlPaging.Size = new Size(765, 36);
            _pnlPaging.TabIndex = 2;
            // 
            // _btnPrevPage
            // 
            _btnPrevPage.Cursor = Cursors.Hand;
            _btnPrevPage.FlatAppearance.BorderSize = 0;
            _btnPrevPage.FlatStyle = FlatStyle.Flat;
            _btnPrevPage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnPrevPage.Location = new Point(16, 4);
            _btnPrevPage.Name = "_btnPrevPage";
            _btnPrevPage.Size = new Size(64, 26);
            _btnPrevPage.TabIndex = 0;
            _btnPrevPage.Text = "Prev";
            _btnPrevPage.UseVisualStyleBackColor = true;
            // 
            // _lblPageInfo
            // 
            _lblPageInfo.AutoSize = true;
            _lblPageInfo.Font = new Font("Segoe UI", 9F);
            _lblPageInfo.Location = new Point(90, 9);
            _lblPageInfo.Name = "_lblPageInfo";
            _lblPageInfo.Size = new Size(91, 15);
            _lblPageInfo.TabIndex = 1;
            _lblPageInfo.Text = "Page 1 of 1 (0)";
            // 
            // _btnNextPage
            // 
            _btnNextPage.Cursor = Cursors.Hand;
            _btnNextPage.FlatAppearance.BorderSize = 0;
            _btnNextPage.FlatStyle = FlatStyle.Flat;
            _btnNextPage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnNextPage.Location = new Point(200, 4);
            _btnNextPage.Name = "_btnNextPage";
            _btnNextPage.Size = new Size(64, 26);
            _btnNextPage.TabIndex = 2;
            _btnNextPage.Text = "Next";
            _btnNextPage.UseVisualStyleBackColor = true;
            // 
            // _lblPageSize
            // 
            _lblPageSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblPageSize.AutoSize = true;
            _lblPageSize.Font = new Font("Segoe UI", 9F);
            _lblPageSize.Location = new Point(618, 9);
            _lblPageSize.Name = "_lblPageSize";
            _lblPageSize.Size = new Size(54, 15);
            _lblPageSize.TabIndex = 3;
            _lblPageSize.Text = "Page size";
            // 
            // _cmbPageSize
            // 
            _cmbPageSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbPageSize.Font = new Font("Segoe UI", 9F);
            _cmbPageSize.FormattingEnabled = true;
            _cmbPageSize.Location = new Point(678, 5);
            _cmbPageSize.Name = "_cmbPageSize";
            _cmbPageSize.Size = new Size(71, 23);
            _cmbPageSize.TabIndex = 4;
            // 
            // _pnlSearchBar
            // 
            _pnlSearchBar.Controls.Add(_txtSearch);
            _pnlSearchBar.Dock = DockStyle.Top;
            _pnlSearchBar.Location = new Point(0, 0);
            _pnlSearchBar.Name = "_pnlSearchBar";
            _pnlSearchBar.Padding = new Padding(16, 12, 16, 8);
            _pnlSearchBar.Size = new Size(765, 55);
            _pnlSearchBar.TabIndex = 0;
            // 
            // _txtSearch
            // 
            _txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _txtSearch.BorderStyle = BorderStyle.None;
            _txtSearch.Font = new Font("Segoe UI", 12F);
            _txtSearch.Location = new Point(28, 14);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.PlaceholderText = "🔍  Search by name or scan barcode...";
            _txtSearch.Size = new Size(709, 22);
            _txtSearch.TabIndex = 0;
            // 
            // POSPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_pnlProducts);
            Controls.Add(_pnlSeparator);
            Controls.Add(_pnlCartSidebar);
            Name = "POSPage";
            Size = new Size(1200, 683);
            _pnlCartSidebar.ResumeLayout(false);
            _pnlCustomer.ResumeLayout(false);
            _pnlCustomer.PerformLayout();
            _pnlCartHeader.ResumeLayout(false);
            _pnlCartHeader.PerformLayout();
            _pnlActions.ResumeLayout(false);
            _pnlSummary.ResumeLayout(false);
            _pnlSummary.PerformLayout();
            _pnlProducts.ResumeLayout(false);
            _pnlFinderActions.ResumeLayout(false);
            _pnlPaging.ResumeLayout(false);
            _pnlPaging.PerformLayout();
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
        private Cart.ctrlSaleCart _ctrlSaleCart;
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
        private Panel _pnlFinderActions;
        private Button _btnAddFinderSelection;
        private Products.ctrlProductFinder _ctrlProductFinder;
        private Panel _pnlPaging;
        private Button _btnPrevPage;
        private Label _lblPageInfo;
        private Button _btnNextPage;
        private Label _lblPageSize;
        private ComboBox _cmbPageSize;
        private Panel _pnlSearchBar;
        private TextBox _txtSearch;
        private FlowLayoutPanel _flpProducts;
    }
}
