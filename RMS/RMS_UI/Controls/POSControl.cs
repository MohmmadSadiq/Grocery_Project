using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RMS_UI.Controls
{
    public partial class POSControl : UserControl
    {
        // Cart Items
        private List<CartItem> _cartItems = new List<CartItem>();
        private decimal _subtotal = 0;
        private decimal _tax = 0;
        private decimal _discount = 0;
        private decimal _total = 0;
        private int _orderNumber = 1000;
        private DateTime _orderStartTime;

        // Sample product data
        private List<Product> _allProducts = new List<Product>();
        private List<Product> _filteredProducts = new List<Product>();
        private string _currentCategory = "All";

        public POSControl()
        {
            InitializeComponent();
            InitializeSampleData();
            // Keyboard shortcuts handled by parent Form if needed
            LoadProducts();
            UpdateOrderHeader();
            _orderStartTime = DateTime.Now;
        }

        private void InitializeSampleData()
        {
            // Sample products
            _allProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Latte Large", Price = 4.50m, Category = "Beverages", Stock = 50, IsHotItem = true },
                new Product { Id = 2, Name = "Cappuccino", Price = 4.00m, Category = "Beverages", Stock = 45, IsHotItem = true },
                new Product { Id = 3, Name = "Espresso", Price = 3.50m, Category = "Beverages", Stock = 60 },
                new Product { Id = 4, Name = "Burger Deluxe", Price = 8.00m, Category = "Food", Stock = 30, IsHotItem = true },
                new Product { Id = 5, Name = "Cheesecake", Price = 5.00m, Category = "Dessert", Stock = 15 },
                new Product { Id = 6, Name = "Water Bottle", Price = 1.00m, Category = "Beverages", Stock = 100, IsHotItem = true },
                new Product { Id = 7, Name = "Fries", Price = 3.00m, Category = "Food", Stock = 50 },
                new Product { Id = 8, Name = "Ice Cream", Price = 4.50m, Category = "Dessert", Stock = 25 },
                new Product { Id = 9, Name = "Sandwich", Price = 6.50m, Category = "Food", Stock = 20, IsHotItem = true },
                new Product { Id = 10, Name = "Orange Juice", Price = 3.50m, Category = "Beverages", Stock = 35 },
                new Product { Id = 11, Name = "Salad", Price = 7.00m, Category = "Food", Stock = 18 },
                new Product { Id = 12, Name = "Cookie", Price = 2.00m, Category = "Dessert", Stock = 60 },
            };
        }

        private void SetupKeyboardShortcuts()
        {
            // KeyPreview is not available for UserControl. Keyboard shortcuts should be handled by the parent Form.
        }

        private void POSControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    HoldOrder();
                    break;
                case Keys.F2:
                    VoidTransaction();
                    break;
                case Keys.F3:
                    OpenDrawer();
                    break;
                case Keys.F4:
                    ApplyDiscount();
                    break;
                case Keys.Enter:
                    if (!txtSearch.Focused)
                        ProcessPayment();
                    break;
                case Keys.Escape:
                    ClearCurrentOrder();
                    break;
            }
        }

        private void LoadProducts(string category = "All", string searchText = "")
        {
            _currentCategory = category;
            
            _filteredProducts = _allProducts.Where(p =>
                (category == "All" || p.Category == category) &&
                (string.IsNullOrEmpty(searchText) || p.Name.ToLower().Contains(searchText.ToLower()))
            ).ToList();

            RefreshProductGrid();
        }

        private void RefreshProductGrid()
        {
            pnlProductGrid.Controls.Clear();
            
            int x = 10, y = 10;
            int cardWidth = 150;
            int cardHeight = 180;
            int spacing = 10;
            int cardsPerRow = (pnlProductGrid.Width - 20) / (cardWidth + spacing);

            for (int i = 0; i < _filteredProducts.Count; i++)
            {
                var product = _filteredProducts[i];
                var card = CreateProductCard(product);
                
                int col = i % cardsPerRow;
                int row = i / cardsPerRow;
                
                card.Location = new Point(x + col * (cardWidth + spacing), y + row * (cardHeight + spacing));
                card.Size = new Size(cardWidth, cardHeight);
                
                pnlProductGrid.Controls.Add(card);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = product
            };

            // Product Image Placeholder
            Panel imgPanel = new Panel
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Size = new Size(130, 100),
                Location = new Point(10, 10)
            };
            Label imgLabel = new Label
            {
                Text = "📦",
                Font = new Font("Segoe UI", 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            imgPanel.Controls.Add(imgLabel);
            card.Controls.Add(imgPanel);

            // Product Name
            Label lblName = new Label
            {
                Text = product.Name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 115),
                Size = new Size(130, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblName);

            // Price
            Label lblPrice = new Label
            {
                Text = $"${product.Price:F2}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(10, 138),
                Size = new Size(130, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblPrice);

            // Stock indicator
            if (product.Stock < 20)
            {
                Label lblStock = new Label
                {
                    Text = $"{product.Stock} in stock",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = product.Stock < 10 ? Color.Red : Color.Orange,
                    Location = new Point(10, 160),
                    Size = new Size(130, 15),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                card.Controls.Add(lblStock);
            }

            // Click event
            card.Click += (s, e) => AddToCart(product);
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => AddToCart(product);
            }

            return card;
        }

        private void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            RefreshCart();
            ShowNotification($"{product.Name} added to cart");
        }

        private void RefreshCart()
        {
            dgvCart.Rows.Clear();

            // تحقق من وجود الأعمدة المطلوبة
            bool hasColItem = dgvCart.Columns.Contains("colItem");
            bool hasColQty = dgvCart.Columns.Contains("colQty");
            bool hasColPrice = dgvCart.Columns.Contains("colPrice");
            bool hasColTotal = dgvCart.Columns.Contains("colTotal");

            foreach (var item in _cartItems)
            {
                int rowIndex = dgvCart.Rows.Add();
                var row = dgvCart.Rows[rowIndex];

                if (hasColItem) row.Cells["colItem"].Value = item.ProductName;
                if (hasColQty) row.Cells["colQty"].Value = item.Quantity;
                if (hasColPrice) row.Cells["colPrice"].Value = $"${item.Price:F2}";
                if (hasColTotal) row.Cells["colTotal"].Value = $"${item.Total:F2}";
                row.Tag = item;
            }

            CalculateTotals();
        }

        private void CalculateTotals()
        {
            _subtotal = _cartItems.Sum(i => i.Total);
            _tax = _subtotal * 0.15m; // 15% tax
            _total = _subtotal + _tax - _discount;

            lblSubtotalValue.Text = $"${_subtotal:F2}";
            lblTaxValue.Text = $"${_tax:F2}";
            lblDiscountValue.Text = $"-${_discount:F2}";
            lblTotalValue.Text = $"${_total:F2}";
            btnCharge.Text = $"CHARGE ${_total:F2}";
        }

        private void UpdateOrderHeader()
        {
            _orderNumber++;
            lblOrderNumber.Text = $"Order #{_orderNumber}";
            lblCustomer.Text = "Walk-in Customer";
        }

        private void ShowNotification(string message)
        {
            // Simple notification - you can enhance this
            ToolTip tooltip = new ToolTip();
            tooltip.Show(message, this, this.Width / 2, 50, 1500);
        }

        // Action Methods
        private void HoldOrder()
        {
            if (_cartItems.Count == 0) return;
            
            MessageBox.Show("Order held successfully", "Hold Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearCurrentOrder();
        }

        private void VoidTransaction()
        {
            if (_cartItems.Count == 0) return;

            var result = MessageBox.Show("Are you sure you want to void this transaction?", 
                "Void Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (result == DialogResult.Yes)
            {
                ClearCurrentOrder();
            }
        }

        private void OpenDrawer()
        {
            MessageBox.Show("Cash drawer opened", "Open Drawer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ApplyDiscount()
        {
            using (var form = new Form())
            {
                form.Text = "Apply Discount";
                form.Size = new Size(300, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                
                Label lbl = new Label { Text = "Discount Amount:", Location = new Point(20, 20) };
                TextBox txt = new TextBox { Location = new Point(20, 45), Width = 240 };
                Button btnOk = new Button { Text = "Apply", Location = new Point(100, 75), DialogResult = DialogResult.OK };
                
                form.Controls.AddRange(new Control[] { lbl, txt, btnOk });
                form.AcceptButton = btnOk;
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (decimal.TryParse(txt.Text, out decimal discount))
                    {
                        _discount = discount;
                        CalculateTotals();
                    }
                }
            }
        }

        private void ProcessPayment()
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var paymentForm = new PaymentForm(_total))
            {
                if (paymentForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Payment successful!\n\nThank you for your purchase.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearCurrentOrder();
                    UpdateOrderHeader();
                }
            }
        }

        private void ClearCurrentOrder()
        {
            _cartItems.Clear();
            _discount = 0;
            RefreshCart();
            _orderStartTime = DateTime.Now;
        }

        // Event Handlers
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts(_currentCategory, txtSearch.Text);
        }

        private void btnCategory_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                LoadProducts(btn.Tag?.ToString() ?? "All", txtSearch.Text);
                
                // Update button styles
                foreach (Control ctrl in pnlCategories.Controls)
                {
                    if (ctrl is Button b)
                    {
                        b.BackColor = b == btn ? Color.FromArgb(0, 122, 204) : Color.White;
                        b.ForeColor = b == btn ? Color.White : Color.Black;
                    }
                }
            }
        }

        private void btnQuickItem_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                AddToCart(product);
            }
        }

        private void dgvCart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvCart.Rows.Count) return;
                if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvCart.Columns.Count) return;

                var row = dgvCart.Rows[e.RowIndex];
                if (row == null || row.Tag == null) return;
                
                var item = row.Tag as CartItem;
                if (item == null) return;

                var colName = dgvCart.Columns[e.ColumnIndex].Name;
                if (string.IsNullOrEmpty(colName)) return;

                // Plus button
                if (colName == "colPlus")
                {
                    item.Quantity++;
                    RefreshCart();
                }
                // Minus button
                else if (colName == "colMinus")
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        RefreshCart();
                    }
                }
                // Delete button
                else if (colName == "colDelete")
                {
                    _cartItems.Remove(item);
                    RefreshCart();
                }
            }
            catch (Exception ex)
            {
                // منع أي استثناءات من إيقاف البرنامج
                MessageBox.Show($"Error processing cart action: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            ProcessPayment();
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            HoldOrder();
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            VoidTransaction();
        }
    }

    // Helper Classes
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsHotItem { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }

    // Payment Form
    public class PaymentForm : Form
    {
        private decimal _totalDue;
        private TextBox txtCash = null!;
        private TextBox txtCard = null!;
        private Label lblChange = null!;

        public PaymentForm(decimal totalDue)
        {
            _totalDue = totalDue;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Payment";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Total Due
            Label lblTotalDue = new Label
            {
                Text = $"Total Due: ${_totalDue:F2}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTotalDue);

            // Cash
            Label lblCash = new Label { Text = "Cash:", Location = new Point(20, 70), Size = new Size(100, 20) };
            txtCash = new TextBox { Location = new Point(130, 70), Width = 230 };
            txtCash.TextChanged += CalculateChange;
            this.Controls.AddRange(new Control[] { lblCash, txtCash });

            // Card
            Label lblCard = new Label { Text = "Card:", Location = new Point(20, 100), Size = new Size(100, 20) };
            txtCard = new TextBox { Location = new Point(130, 100), Width = 230 };
            txtCard.TextChanged += CalculateChange;
            this.Controls.AddRange(new Control[] { lblCard, txtCard });

            // Quick amounts
            Panel pnlQuick = new Panel { Location = new Point(20, 140), Size = new Size(350, 40) };
            int[] amounts = { 50, 100, 200 };
            for (int i = 0; i < amounts.Length; i++)
            {
                Button btn = new Button
                {
                    Text = $"${amounts[i]}",
                    Location = new Point(i * 90, 0),
                    Size = new Size(80, 35),
                    Tag = amounts[i]
                };
                btn.Click += (s, e) =>
                {
                    txtCash.Text = ((Button)s!).Tag?.ToString() ?? "0";
                };
                pnlQuick.Controls.Add(btn);
            }
            this.Controls.Add(pnlQuick);

            // Change
            lblChange = new Label
            {
                Text = "Change: $0.00",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 200),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Green
            };
            this.Controls.Add(lblChange);

            // Complete button
            Button btnComplete = new Button
            {
                Text = "Complete Payment",
                Location = new Point(100, 250),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(0, 192, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnComplete.Click += (s, e) =>
            {
                decimal cash = decimal.TryParse(txtCash.Text, out decimal c) ? c : 0;
                decimal card = decimal.TryParse(txtCard.Text, out decimal cc) ? cc : 0;

                if (cash + card >= _totalDue)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Insufficient payment amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            this.Controls.Add(btnComplete);
        }

        private void CalculateChange(object? sender, EventArgs e)
        {
            decimal cash = decimal.TryParse(txtCash.Text, out decimal c) ? c : 0;
            decimal card = decimal.TryParse(txtCard.Text, out decimal cc) ? cc : 0;
            decimal change = (cash + card) - _totalDue;

            lblChange.Text = change >= 0 ? $"Change: ${change:F2}" : $"Remaining: ${Math.Abs(change):F2}";
            lblChange.ForeColor = change >= 0 ? Color.Green : Color.Red;
        }
    }
}
