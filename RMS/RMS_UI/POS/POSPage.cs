using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Payment;
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

        private List<clsProductUnit> _allProductUnits = new();
        private readonly List<CartItem> _cartItems = new();
        private NotificationControl _notification = null!;

        // Debounce timer for search
        private System.Windows.Forms.Timer _searchTimer = null!;

        #endregion

        #region Nested Types

        /// <summary>
        /// Wraps a <see cref="clsProductSale"/> with cached display info
        /// so the grid doesn't re-trigger lazy loads on every refresh.
        /// </summary>
        private class CartItem
        {
            public clsProductSale SaleItem { get; set; } = null!;
            public string ProductName { get; set; } = "";
            public string UnitName { get; set; } = "";
        }

        #endregion

        #region Constructor

        public POSPage()
        {
            InitializeComponent();

            SetupNotification();
            SetupCartGrid();
            SetupCustomerCombo();
            SetupSearchBox();
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

        private void SetupCartGrid()
        {
            _dgvCart.Columns.Clear();

            _dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNo",
                HeaderText = "#",
                Width = 35,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            _dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProduct",
                HeaderText = "Product",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            _dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colQty",
                HeaderText = "Qty",
                Width = 50,
                ReadOnly = false,   // editable for quantity changes
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            _dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrice",
                HeaderText = "Price",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" },
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            _dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubtotal",
                HeaderText = "Subtotal",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" },
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            var btnCol = new DataGridViewButtonColumn
            {
                Name = "colRemove",
                HeaderText = "",
                Text = "✕",
                UseColumnTextForButtonValue = true,
                Width = 36,
                FlatStyle = FlatStyle.Flat
            };
            _dgvCart.Columns.Add(btnCol);

            _dgvCart.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            _dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            _dgvCart.EnableHeadersVisualStyles = false;
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
                FilterProducts();
            };
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

            _btnClearCart.Click += BtnClearCart_Click;
            _btnCompleteSale.Click += BtnCompleteSale_Click;
            _btnNewSale.Click += BtnNewSale_Click;

            _dgvCart.CellContentClick += DgvCart_CellContentClick;
            _dgvCart.CellEndEdit += DgvCart_CellEndEdit;
            _dgvCart.EditingControlShowing += DgvCart_EditingControlShowing;
        }

        #endregion

        #region Product Loading

        private void LoadProducts()
        {
            _flpProducts.SuspendLayout();
            _flpProducts.Controls.Clear();

            try
            {
                _allProductUnits = clsProductUnit.GetAllActiveProductUnitList();
            }
            catch
            {
                _notification.ShowError("Failed to load products.");
                _allProductUnits = new List<clsProductUnit>();
            }

            foreach (var pu in _allProductUnits)
            {
                var card = CreateProductCard(pu);
                _flpProducts.Controls.Add(card);
            }

            _flpProducts.ResumeLayout(true);
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
                // Just filter — no barcode found
                _searchTimer.Stop();
                FilterProducts();
            }
        }

        private void FilterProducts()
        {
            string searchText = _txtSearch.Text.Trim().ToLowerInvariant();

            _flpProducts.SuspendLayout();

            foreach (Control ctrl in _flpProducts.Controls)
            {
                if (ctrl is ctrlProductCard card && card.ProductUnit != null)
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        card.Visible = true;
                        continue;
                    }

                    string productName = card.ProductUnit.ProductInfo?.ProductName?.ToLowerInvariant() ?? "";
                    string barcode = card.ProductUnit.Barcode?.ToLowerInvariant() ?? "";
                    string unitName = card.ProductUnit.UnitInfo?.UnitName?.ToLowerInvariant() ?? "";

                    card.Visible = productName.Contains(searchText)
                                || barcode.Contains(searchText)
                                || unitName.Contains(searchText);
                }
            }

            _flpProducts.ResumeLayout(true);
        }

        #endregion

        #region Cart Operations

        private void OnProductCardClicked(object? sender, clsProductUnit pu)
        {
            AddToCart(pu);
        }

        private void AddToCart(clsProductUnit pu)
        {
            // Look for existing item with the same ProductUnitID
            var existing = _cartItems.FirstOrDefault(
                ci => ci.SaleItem.ProductUnitID == pu.ProductUnitID);

            if (existing != null)
            {
                // Increment quantity
                existing.SaleItem.Quantity += 1;
            }
            else
            {
                // Cache display strings from lazy-loaded properties
                string productName = pu.ProductInfo?.ProductName ?? "Unknown";
                string unitName = pu.UnitInfo?.UnitName ?? "";
                string displayName = string.IsNullOrEmpty(unitName)
                    ? productName
                    : $"{productName} ({unitName})";

                _cartItems.Add(new CartItem
                {
                    SaleItem = new clsProductSale
                    {
                        ProductUnitID = pu.ProductUnitID,
                        Quantity = 1,
                        UnitPrice = pu.SalePrice ?? 0
                    },
                    ProductName = displayName,
                    UnitName = unitName
                });
            }

            RefreshCart();
        }

        private void RefreshCart()
        {
            _dgvCart.SuspendLayout();
            _dgvCart.Rows.Clear();

            for (int i = 0; i < _cartItems.Count; i++)
            {
                var ci = _cartItems[i];
                decimal subtotal = ci.SaleItem.Quantity * ci.SaleItem.UnitPrice;

                _dgvCart.Rows.Add(
                    i + 1,
                    ci.ProductName,
                    ci.SaleItem.Quantity,
                    ci.SaleItem.UnitPrice,
                    subtotal,
                    "✕"
                );
            }

            _dgvCart.ResumeLayout(true);
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            int totalItems = _cartItems.Sum(ci => (int)ci.SaleItem.Quantity);
            decimal totalAmount = _cartItems.Sum(ci => ci.SaleItem.Quantity * ci.SaleItem.UnitPrice);

            _lblCartItemCount.Text = $"{totalItems} item{(totalItems != 1 ? "s" : "")}";
            _lblItemsCount.Text = $"{_cartItems.Count} line{(_cartItems.Count != 1 ? "s" : "")} · {totalItems} item{(totalItems != 1 ? "s" : "")}";
            _lblTotalAmount.Text = totalAmount.ToString("N2");

            _ctrlPaymentPanel.TotalAmount = totalAmount;
        }

        #endregion

        #region Cart Grid Events

        private void DgvCart_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Handle remove button click
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count) return;
            if (e.ColumnIndex != _dgvCart.Columns["colRemove"]!.Index) return;

            _cartItems.RemoveAt(e.RowIndex);
            RefreshCart();
        }

        private void DgvCart_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count) return;
            if (e.ColumnIndex != _dgvCart.Columns["colQty"]!.Index) return;

            var cellValue = _dgvCart.Rows[e.RowIndex].Cells["colQty"].Value;

            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out decimal newQty))
            {
                if (newQty <= 0)
                {
                    // Remove item if quantity is 0 or negative
                    _cartItems.RemoveAt(e.RowIndex);
                }
                else
                {
                    _cartItems[e.RowIndex].SaleItem.Quantity = newQty;
                }
            }
            else
            {
                // Invalid input — revert to 1
                _cartItems[e.RowIndex].SaleItem.Quantity = 1;
            }

            RefreshCart();
        }

        /// <summary>
        /// Only allow numeric input in the Qty column.
        /// </summary>
        private void DgvCart_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (_dgvCart.CurrentCell?.ColumnIndex == _dgvCart.Columns["colQty"]!.Index)
            {
                e.Control.KeyPress -= QtyCell_KeyPress;
                e.Control.KeyPress += QtyCell_KeyPress;
            }
        }

        private void QtyCell_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region Sale Actions

        private void BtnCompleteSale_Click(object? sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                _notification.ShowWarning("Cart is empty. Add products before completing the sale.");
                return;
            }

            try
            {
                decimal totalAmount = _cartItems.Sum(ci => ci.SaleItem.Quantity * ci.SaleItem.UnitPrice);

                // Build sale object
                var sale = new clsSales
                {
                    TransactionType = clsTransaction.enTransactionType.Sale,
                    TransactionDate = DateTime.Now,
                    TransactionStatus = clsTransaction.enTransactionStatus.Completed,
                    TotalAmount = totalAmount,
                    CustomerID = GetSelectedCustomerID(),
                    CreatedByUserID = clsGlobalUser.CurrentUser?.UserID,
                    SaleItems = _cartItems.Select(ci => ci.SaleItem).ToList()
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
            if (_cartItems.Count > 0)
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
            if (_cartItems.Count == 0) return;

            var result = MessageBox.Show(
                "Clear all items from the cart?",
                "Clear Cart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _cartItems.Clear();
            _ctrlPaymentPanel.ClearPendingPayments();
            RefreshCart();
        }

        private void ResetForNewSale()
        {
            _cartItems.Clear();
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

            // Cart grid
            _dgvCart.BackgroundColor = c.ContentBackground;
            _dgvCart.GridColor = c.BorderColor;
            _dgvCart.DefaultCellStyle.BackColor = c.ContentBackground;
            _dgvCart.DefaultCellStyle.ForeColor = c.PrimaryText;
            _dgvCart.DefaultCellStyle.SelectionBackColor = c.PrimaryLight;
            _dgvCart.DefaultCellStyle.SelectionForeColor = c.PrimaryText;
            _dgvCart.ColumnHeadersDefaultCellStyle.BackColor = c.FormBackground;
            _dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = c.SecondaryText;

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
