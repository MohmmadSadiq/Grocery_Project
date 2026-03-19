using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Payment;
using RMS_UI.POS.Cart;
using RMS_UI.Products;
using RMS_UI.Utilities;

namespace RMS_UI.POS
{
    /// <summary>
    /// Point-of-Sale page — displays a grid of product cards on the left and
    /// a cart + payment panel on the right. Designed for fast cashier workflows.
    /// Loaded via <c>MainPage.LoadContent(new POSPage())</c>.
    /// </summary>
    public partial class POSPage : UserControl
    {
        #region Fields

        private int _productSearchPageSize = 60;
        private int _currentSearchPage = 1;
        private int _totalProductSearchCount = 0;
        private string _currentSearchText = string.Empty;
        private bool _isUpdatingPageSize = false;
        private List<clsProductUnit> _allProductUnits = new();
        private NotificationControl _notification = null!;

        // Debounce timer for search
        private System.Windows.Forms.Timer _searchTimer = null!;

        #endregion

        #region Constructor

        public POSPage()
        {
            InitializeComponent();

            SetupNotification();
            SetupCustomerCombo();
            SetupSearchBox();
            SetupPagingControls();
            SetupProductFinder();
            SetupPaymentPanel();
            WireEvents();

            // Theme
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();

            // Load products on first show to avoid slow constructor
            this.Load += (s, e) => LoadProducts();
        }

        #endregion

        #region Initialization

        private void SetupNotification()
        {
            _notification = new NotificationControl
            {
                Dock = DockStyle.Top,
                AutoHide = true,
                AutoHideDuration = 3000,
                Visible = false
            };
            _pnlCartSidebar.Controls.Add(_notification);
            _notification.BringToFront();
        }

        private void SetupCustomerCombo()
        {
            // Placeholder item — fully functional customer search to be wired
            //   when clsCustomer business class is built.
            _cmbCustomer.Items.Clear();
            _cmbCustomer.Items.Add("Walk-in Customer");
            _cmbCustomer.SelectedIndex = 0;
        }

        private void SetupSearchBox()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                SearchProducts();
            };
        }

        private void SetupPagingControls()
        {
            _isUpdatingPageSize = true;
            _cmbPageSize.Items.Clear();
            _cmbPageSize.Items.AddRange(new object[] { 20, 40, 60, 100 });
            _cmbPageSize.SelectedItem = _productSearchPageSize;
            _isUpdatingPageSize = false;

            UpdatePagingUi();
        }

        private void SetupProductFinder()
        {
            _ctrlProductFinder.BrowseButtonEnabled = false;
            _ctrlProductFinder.UnitConfirmedByEnter += CtrlProductFinder_UnitConfirmedByEnter;
        }

        private void SetupPaymentPanel()
        {
            _ctrlPaymentPanel.IsPendingMode = true;
            _ctrlPaymentPanel.TotalAmount = 0;
            _ctrlPaymentPanel.TransactionID = -1;
        }

        private void WireEvents()
        {
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchTimer.Stop();
                _searchTimer.Start();
            };

            _txtSearch.KeyDown += TxtSearch_KeyDown;
            _btnPrevPage.Click += BtnPrevPage_Click;
            _btnNextPage.Click += BtnNextPage_Click;
            _cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;
            _btnAddFinderSelection.Click += BtnAddFinderSelection_Click;

            _btnClearCart.Click += BtnClearCart_Click;
            _btnCompleteSale.Click += BtnCompleteSale_Click;
            _btnNewSale.Click += BtnNewSale_Click;

            _ctrlSaleCart.SalesChanged += (s, e) => UpdateSummary();
            _ctrlSaleCart.TotalChanged += (s, e) => UpdateSummary();
        }

        #endregion

        #region Product Loading

        private void LoadProducts()
        {
            LoadProductsFromServer(string.Empty, _currentSearchPage);
        }

        private void LoadProductsFromServer(string? searchText, int pageNumber)
        {
            _flpProducts.SuspendLayout();
            _flpProducts.Controls.Clear();

            try
            {
                _currentSearchText = searchText?.Trim() ?? string.Empty;
                _currentSearchPage = pageNumber < 1 ? 1 : pageNumber;

                _allProductUnits = clsProductUnit.SearchActiveWithProductPaged(
                    _currentSearchText,
                    _currentSearchPage,
                    _productSearchPageSize,
                    out int totalCount);

                _totalProductSearchCount = totalCount;
            }
            catch
            {
                _notification.ShowError("Failed to load products.");
                _allProductUnits = new List<clsProductUnit>();
                _totalProductSearchCount = 0;
            }

            foreach (var pu in _allProductUnits)
            {
                var card = CreateProductCard(pu);
                _flpProducts.Controls.Add(card);
            }

            _flpProducts.ResumeLayout(true);
            UpdatePagingUi();
        }

        private ctrlProductCard CreateProductCard(clsProductUnit pu)
        {
            var card = new ctrlProductCard
            {
                Margin = new Padding(8),
                Tag = pu   // store for search filtering
            };
            card.LoadProductUnit(pu);
            card.ProductUnitClicked += OnProductCardClicked;
            return card;
        }

        #endregion

        #region Search & Filter

        private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            string text = _txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            // Try exact barcode match first
            var pu = clsProductUnit.FindByBarcode(text);
            if (pu != null)
            {
                AddToCart(pu);
                _txtSearch.Clear();
                _txtSearch.Focus();
            }
            else
            {
                // No exact barcode match, run server search.
                _searchTimer.Stop();
                SearchProducts();
            }
        }

        private void SearchProducts()
        {
            string searchText = _txtSearch.Text.Trim();
            _currentSearchPage = 1;
            LoadProductsFromServer(searchText, _currentSearchPage);
        }

        private void CtrlProductFinder_UnitConfirmedByEnter(object? sender, EventArgs e)
        {
            AddFinderSelectionToCart();
        }

        private void BtnAddFinderSelection_Click(object? sender, EventArgs e)
        {
            AddFinderSelectionToCart();
        }

        private void AddFinderSelectionToCart()
        {
            clsProductUnit? selected = _ctrlProductFinder.SelectedProductUnit;
            if (selected == null)
            {
                _notification.ShowWarning("No matching product unit selected.");
                return;
            }

            AddToCart(selected);
            _ctrlProductFinder.ResetAll();
        }

        private void BtnPrevPage_Click(object? sender, EventArgs e)
        {
            if (_currentSearchPage <= 1)
                return;

            _currentSearchPage--;
            LoadProductsFromServer(_currentSearchText, _currentSearchPage);
        }

        private void BtnNextPage_Click(object? sender, EventArgs e)
        {
            int totalPages = GetTotalPages();
            if (_currentSearchPage >= totalPages)
                return;

            _currentSearchPage++;
            LoadProductsFromServer(_currentSearchText, _currentSearchPage);
        }

        private void CmbPageSize_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingPageSize)
                return;

            if (_cmbPageSize.SelectedItem is int pageSize)
            {
                _productSearchPageSize = pageSize;
                _currentSearchPage = 1;
                LoadProductsFromServer(_currentSearchText, _currentSearchPage);
            }
        }

        private int GetTotalPages()
        {
            if (_productSearchPageSize <= 0)
                return 1;

            return Math.Max(1, (int)Math.Ceiling((double)_totalProductSearchCount / _productSearchPageSize));
        }

        private void UpdatePagingUi()
        {
            int totalPages = GetTotalPages();

            if (_currentSearchPage > totalPages)
                _currentSearchPage = totalPages;

            _btnPrevPage.Enabled = _currentSearchPage > 1;
            _btnNextPage.Enabled = _currentSearchPage < totalPages;
            _lblPageInfo.Text = $"Page {_currentSearchPage} of {totalPages} ({_totalProductSearchCount})";
        }

        #endregion

        #region Cart Operations

        private void OnProductCardClicked(object? sender, clsProductUnit pu)
        {
            AddToCart(pu);
        }

        private void AddToCart(clsProductUnit pu)
        {
            var existing = _ctrlSaleCart.Sales.FirstOrDefault(s => s.ProductUnitID == pu.ProductUnitID);
            if (existing != null)
            {
                _ctrlSaleCart.UpdateQuantity(existing, existing.Quantity + 1);
                return;
            }

            var saleItem = new clsProductSale
            {
                ProductUnitID = pu.ProductUnitID,
                Quantity = 1,
                UnitPrice = pu.SalePrice ?? 0
            };

            _ctrlSaleCart.AddSale(saleItem);
        }

        private void RefreshCart()
        {
            _ctrlSaleCart.ShowSales(_ctrlSaleCart.Sales);
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            int totalItems = _ctrlSaleCart.Sales.Sum(s => (int)s.Quantity);
            decimal totalAmount = _ctrlSaleCart.Sales.Sum(s => s.Quantity * s.UnitPrice);
            int lineCount = _ctrlSaleCart.Sales.Count;

            _lblCartItemCount.Text = $"{totalItems} item{(totalItems != 1 ? "s" : "")}";
            _lblItemsCount.Text = $"{lineCount} line{(lineCount != 1 ? "s" : "")} · {totalItems} item{(totalItems != 1 ? "s" : "")}";
            _lblTotalAmount.Text = totalAmount.ToString("N2");

            if (_ctrlPaymentPanel != null && !_ctrlPaymentPanel.IsDisposed)
                _ctrlPaymentPanel.TotalAmount = totalAmount;
        }

        #endregion

        #region Sale Actions

        private void BtnCompleteSale_Click(object? sender, EventArgs e)
        {
            if (_ctrlSaleCart.Sales.Count == 0)
            {
                _notification.ShowWarning("Cart is empty. Add products before completing the sale.");
                return;
            }

            try
            {
                decimal totalAmount = _ctrlSaleCart.Sales.Sum(s => s.Quantity * s.UnitPrice);

                // Build sale object
                var sale = new clsSales
                {
                    TransactionType = clsTransaction.enTransactionType.Sale,
                    TransactionDate = DateTime.Now,
                    TransactionStatus = clsTransaction.enTransactionStatus.Completed,
                    TotalAmount = totalAmount,
                    CustomerID = GetSelectedCustomerID(),
                    CreatedByUserID = clsGlobalUser.CurrentUser?.UserID,
                    SaleItems = _ctrlSaleCart.Sales.ToList()
                };

                if (!sale.Save())
                {
                    _notification.ShowError("Failed to save the sale. Please try again.");
                    return;
                }

                // Persist pending payments
                var pendingPayments = _ctrlPaymentPanel.GetPendingPayments();
                foreach (var payment in pendingPayments)
                {
                    payment.Allocations.Add(new clsPaymentAllocation
                    {
                        TransactionID = sale.TransactionID,
                        Amount = payment.PaymentAmount
                    });
                    payment.Save();
                }

                _notification.ShowSuccess("Sale completed successfully!");
                ResetForNewSale();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error: {ex.Message}");
            }
        }

        private void BtnNewSale_Click(object? sender, EventArgs e)
        {
            if (_ctrlSaleCart.Sales.Count > 0)
            {
                var result = MessageBox.Show(
                    "Discard current cart and start a new sale?",
                    "New Sale",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;
            }

            ResetForNewSale();
        }

        private void BtnClearCart_Click(object? sender, EventArgs e)
        {
            if (_ctrlSaleCart.Sales.Count == 0) return;

            var result = MessageBox.Show(
                "Clear all items from the cart?",
                "Clear Cart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _ctrlSaleCart.Clear();
            _ctrlPaymentPanel.ClearPendingPayments();
            RefreshCart();
        }

        private void ResetForNewSale()
        {
            _ctrlSaleCart.Clear();
            _ctrlPaymentPanel.ClearPendingPayments();
            _cmbCustomer.SelectedIndex = 0;
            _txtSearch.Clear();
            RefreshCart();
            _txtSearch.Focus();
        }

        private int? GetSelectedCustomerID()
        {
            // Currently no customer business class exists.
            // "Walk-in Customer" maps to null (no customer record).
            // When clsCustomer is implemented, this should read
            // the selected customer's ID from _cmbCustomer.
            if (_cmbCustomer.SelectedIndex <= 0) return null;
            return null;
        }

        #endregion

        #region Theme

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            // Main backgrounds
            BackColor = c.FormBackground;
            _pnlProducts.BackColor = c.FormBackground;
            _pnlCartSidebar.BackColor = c.ContentBackground;
            _pnlSeparator.BackColor = c.BorderColor;

            // Search bar
            _pnlSearchBar.BackColor = c.FormBackground;
            _txtSearch.BackColor = c.ContentBackground;
            _txtSearch.ForeColor = c.PrimaryText;

            _pnlPaging.BackColor = c.FormBackground;
            _lblPageInfo.ForeColor = c.SecondaryText;
            _lblPageSize.ForeColor = c.SecondaryText;

            _btnPrevPage.BackColor = c.PrimaryLight;
            _btnPrevPage.ForeColor = c.Primary;
            _btnPrevPage.FlatAppearance.MouseOverBackColor = c.BorderColor;

            _btnNextPage.BackColor = c.PrimaryLight;
            _btnNextPage.ForeColor = c.Primary;
            _btnNextPage.FlatAppearance.MouseOverBackColor = c.BorderColor;

            _pnlFinderActions.BackColor = c.FormBackground;
            _btnAddFinderSelection.BackColor = c.Primary;
            _btnAddFinderSelection.ForeColor = Color.White;
            _btnAddFinderSelection.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _cmbPageSize.BackColor = c.ContentBackground;
            _cmbPageSize.ForeColor = c.PrimaryText;

            // Cart header
            _pnlCartHeader.BackColor = c.ContentBackground;
            _lblCartTitle.ForeColor = c.TitleText;
            _lblCartItemCount.ForeColor = c.SecondaryText;

            // Clear cart button (danger style)
            _btnClearCart.BackColor = Color.FromArgb(254, 226, 226);
            _btnClearCart.ForeColor = Color.FromArgb(220, 38, 38);
            _btnClearCart.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);

            // Customer panel
            _pnlCustomer.BackColor = c.ContentBackground;
            _lblCustomerLabel.ForeColor = c.SecondaryText;
            _cmbCustomer.BackColor = c.ContentBackground;
            _cmbCustomer.ForeColor = c.PrimaryText;

            // Sale cart control themes itself

            // Summary
            _pnlSummary.BackColor = c.ContentBackground;
            _pnlSummaryLine.BackColor = c.BorderColor;
            _lblItemsCount.ForeColor = c.SecondaryText;
            _lblTotalLabel.ForeColor = c.TitleText;
            _lblTotalAmount.ForeColor = c.Primary;

            // Payment panel inherits its own theme

            // Actions
            _pnlActions.BackColor = c.ContentBackground;
            _btnCompleteSale.BackColor = c.Primary;
            _btnCompleteSale.ForeColor = Color.White;
            _btnCompleteSale.FlatAppearance.MouseOverBackColor = c.PrimaryHover;

            _btnNewSale.BackColor = c.PrimaryLight;
            _btnNewSale.ForeColor = c.Primary;
            _btnNewSale.FlatAppearance.MouseOverBackColor = c.BorderColor;

            // Product cards auto-theme via ThemeManager event

            Invalidate();
        }

        #endregion

        #region Custom Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw a subtle rounded border around the search box
            var c = ThemeManager.Colors;
            var rect = new Rectangle(
                _pnlSearchBar.Left + 16,
                _pnlSearchBar.Top + 8,
                _pnlSearchBar.Width - 32,
                _pnlSearchBar.Height - 16);

            using var pen = new Pen(c.BorderColor, 1);
            using var path = RoundedRectangle(rect, 8);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(pen, path);
        }

        /// <summary>Creates a rounded-rectangle GraphicsPath (same helper used across the project).</summary>
        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion
    }
}
