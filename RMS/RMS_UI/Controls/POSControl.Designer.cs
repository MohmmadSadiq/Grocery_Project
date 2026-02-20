using System.Drawing;
using System.Windows.Forms;

namespace RMS_UI.Controls
{
    partial class POSControl
    {
        private System.ComponentModel.IContainer components = null;
        
        // Left Panel Controls
        private Panel pnlLeft;
        private Panel pnlTopBar;
        private TextBox txtSearch;
        private Label lblScanIcon;
        private Panel pnlQuickAccess;
        private Panel pnlCategories;
        private Panel pnlProductGrid;
        
        // Right Panel Controls
        private Panel pnlRight;
        private Panel pnlOrderHeader;
        private Label lblOrderNumber;
        private Label lblCustomer;
        private Label lblTimer;
        private DataGridView dgvCart;
        private Panel pnlTotals;
        private Label lblSubtotal;
        private Label lblSubtotalValue;
        private Label lblTax;
        private Label lblTaxValue;
        private Label lblDiscount;
        private Label lblDiscountValue;
        private Label lblTotal;
        private Label lblTotalValue;
        private Button btnCharge;
        private Panel pnlActions;
        private Button btnHold;
        private Button btnVoid;
        private Button btnPrint;

        // DataGridView Columns
        private DataGridViewTextBoxColumn colItem;
        private DataGridViewTextBoxColumn colQty;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewButtonColumn colPlus;
        private DataGridViewButtonColumn colMinus;
        private DataGridViewButtonColumn colDelete;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main Control
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Size = new Size(1200, 700);

            // ========== LEFT PANEL ==========
            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // Top Bar (Search & Scan)
            pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            lblScanIcon = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 20),
                Location = new Point(10, 15),
                Size = new Size(40, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            txtSearch = new TextBox
            {
                Location = new Point(60, 15),
                Width = 400,
                Height = 30,
                Font = new Font("Segoe UI", 14),
                PlaceholderText = "Search products or scan barcode..."
            };
            txtSearch.TextChanged += txtSearch_TextChanged;

            pnlTopBar.Controls.AddRange(new Control[] { lblScanIcon, txtSearch });

            // Quick Access Bar (Hot Items)
            pnlQuickAccess = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(5)
            };

            Label lblQuick = new Label
            {
                Text = "Quick Access:",
                Location = new Point(10, 15),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            pnlQuickAccess.Controls.Add(lblQuick);

            // Add hot items buttons
            string[] hotItems = { "Bread", "Milk", "Eggs", "Water", "Sugar" };
            for (int i = 0; i < hotItems.Length; i++)
            {
                Button btnQuick = new Button
                {
                    Text = hotItems[i],
                    Location = new Point(120 + i * 90, 10),
                    Size = new Size(85, 30),
                    BackColor = Color.FromArgb(255, 193, 7),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnQuick.FlatAppearance.BorderSize = 0;
                btnQuick.Click += btnQuickItem_Click;
                pnlQuickAccess.Controls.Add(btnQuick);
            }

            // Categories Panel
            pnlCategories = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            string[] categories = { "All", "Beverages", "Food", "Dessert", "Snacks" };
            for (int i = 0; i < categories.Length; i++)
            {
                Button btnCat = new Button
                {
                    Text = categories[i],
                    Location = new Point(10 + i * 120, 10),
                    Size = new Size(110, 35),
                    BackColor = i == 0 ? Color.FromArgb(0, 122, 204) : Color.White,
                    ForeColor = i == 0 ? Color.White : Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = categories[i]
                };
                btnCat.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
                btnCat.Click += btnCategory_Click;
                pnlCategories.Controls.Add(btnCat);
            }

            // Product Grid
            pnlProductGrid = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(5)
            };

            pnlLeft.Controls.AddRange(new Control[] { pnlProductGrid, pnlCategories, pnlQuickAccess, pnlTopBar });

            // ========== RIGHT PANEL ==========
            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 400,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };

            // Order Header
            pnlOrderHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(10)
            };

            lblOrderNumber = new Label
            {
                Text = "Order #1000",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };

            lblCustomer = new Label
            {
                Text = "Walk-in Customer",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(10, 35),
                Size = new Size(200, 20)
            };

            lblTimer = new Label
            {
                Text = "00:00:00",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(280, 20),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlOrderHeader.Controls.AddRange(new Control[] { lblOrderNumber, lblCustomer, lblTimer });

            // Cart DataGridView
            dgvCart = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = false,
                Font = new Font("Segoe UI", 10),
                ColumnHeadersHeight = 35
            };
            dgvCart.CellClick += dgvCart_CellClick;

            // Define columns
            colItem = new DataGridViewTextBoxColumn
            {
                Name = "colItem",
                HeaderText = "Item",
                ReadOnly = true,
                FillWeight = 40
            };

            colQty = new DataGridViewTextBoxColumn
            {
                Name = "colQty",
                HeaderText = "Qty",
                ReadOnly = true,
                FillWeight = 15
            };

            colPrice = new DataGridViewTextBoxColumn
            {
                Name = "colPrice",
                HeaderText = "Price",
                ReadOnly = true,
                FillWeight = 20
            };

            colTotal = new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "Total",
                ReadOnly = true,
                FillWeight = 20
            };

            colPlus = new DataGridViewButtonColumn
            {
                Name = "colPlus",
                HeaderText = "",
                Text = "+",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };

            colMinus = new DataGridViewButtonColumn
            {
                Name = "colMinus",
                HeaderText = "",
                Text = "-",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };

            colDelete = new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };

            dgvCart.Columns.AddRange(new DataGridViewColumn[] { 
                colItem, colQty, colPrice, colTotal, colPlus, colMinus, colDelete 
            });

            // Totals Panel
            pnlTotals = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            int yPos = 10;
            
            // Subtotal
            lblSubtotal = new Label { Text = "Subtotal:", Location = new Point(15, yPos), Size = new Size(150, 20), Font = new Font("Segoe UI", 10) };
            lblSubtotalValue = new Label { Text = "$0.00", Location = new Point(250, yPos), Size = new Size(120, 20), Font = new Font("Segoe UI", 10), TextAlign = ContentAlignment.MiddleRight };
            yPos += 25;

            // Tax
            lblTax = new Label { Text = "Tax (15%):", Location = new Point(15, yPos), Size = new Size(150, 20), Font = new Font("Segoe UI", 10) };
            lblTaxValue = new Label { Text = "$0.00", Location = new Point(250, yPos), Size = new Size(120, 20), Font = new Font("Segoe UI", 10), TextAlign = ContentAlignment.MiddleRight };
            yPos += 25;

            // Discount
            lblDiscount = new Label { Text = "Discount:", Location = new Point(15, yPos), Size = new Size(150, 20), Font = new Font("Segoe UI", 10) };
            lblDiscountValue = new Label { Text = "-$0.00", Location = new Point(250, yPos), Size = new Size(120, 20), Font = new Font("Segoe UI", 10), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight };
            yPos += 30;

            // Total
            lblTotal = new Label { Text = "TOTAL:", Location = new Point(15, yPos), Size = new Size(150, 25), Font = new Font("Segoe UI", 14, FontStyle.Bold) };
            lblTotalValue = new Label { Text = "$0.00", Location = new Point(200, yPos), Size = new Size(170, 25), Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(0, 122, 204), TextAlign = ContentAlignment.MiddleRight };
            yPos += 40;

            // Charge Button
            btnCharge = new Button
            {
                Text = "CHARGE $0.00",
                Location = new Point(15, yPos),
                Size = new Size(355, 50),
                BackColor = Color.FromArgb(0, 192, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCharge.FlatAppearance.BorderSize = 0;
            btnCharge.Click += btnCharge_Click;

            pnlTotals.Controls.AddRange(new Control[] { 
                lblSubtotal, lblSubtotalValue,
                lblTax, lblTaxValue,
                lblDiscount, lblDiscountValue,
                lblTotal, lblTotalValue,
                btnCharge
            });

            // Actions Panel
            pnlActions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10, 5, 10, 5)
            };

            btnHold = new Button
            {
                Text = "Hold (F1)",
                Location = new Point(10, 5),
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHold.FlatAppearance.BorderSize = 0;
            btnHold.Click += btnHold_Click;

            btnVoid = new Button
            {
                Text = "Void (F2)",
                Location = new Point(130, 5),
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVoid.FlatAppearance.BorderSize = 0;
            btnVoid.Click += btnVoid_Click;

            btnPrint = new Button
            {
                Text = "Print",
                Location = new Point(250, 5),
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;

            pnlActions.Controls.AddRange(new Control[] { btnHold, btnVoid, btnPrint });

            // Add to right panel
            pnlRight.Controls.AddRange(new Control[] { pnlTotals, pnlActions, dgvCart, pnlOrderHeader });

            // Add main panels to control
            this.Controls.AddRange(new Control[] { pnlLeft, pnlRight });

            this.ResumeLayout(false);
        }
    }
}
