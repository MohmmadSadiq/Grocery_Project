using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.PaymentMethods
{
    /// <summary>
    /// Dialog for managing Payment Methods with a ModernDataGridView.
    /// Pattern: same as ProductSettingsDialog but single entity (no tabs).
    /// </summary>
    public partial class PaymentMethodSettingsDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private Panel _toolbar = null!;
        private ModernDataGridView _grid = null!;
        private Button _btnAdd = null!;
        private Button _btnEdit = null!;
        private Button _btnDelete = null!;
        private Button _btnClose = null!;
        private NotificationControl _notification = null!;
        #endregion

        #region Constructor
        public PaymentMethodSettingsDialog()
        {
            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void PaymentMethodSettingsDialog_Load(object? sender, EventArgs e)
        {
            LoadPaymentMethods();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            this.Text = "Payment Methods";
            this.Size = new Size(800, 500);
            this.MinimumSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Font = new Font("Segoe UI", 10F);

            // Notification
            _notification = new NotificationControl
            {
                Dock = DockStyle.Top
            };

            // Header Panel
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(59, 130, 246),
                Padding = new Padding(20, 0, 20, 0)
            };

            _titleLabel = new Label
            {
                Text = "💳 Payment Methods",
                Font = new Font("Segoe UI Semibold", 16F),
                ForeColor = Color.White,
                AutoSize = true
            };
            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Resize += (s, e) =>
            {
                _titleLabel.Location = new Point(20, (_headerPanel.Height - _titleLabel.Height) / 2);
            };

            // Toolbar
            _toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(15, 5, 15, 5)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnAdd = CreateButton("➕ Add Method", Color.FromArgb(34, 197, 94));
            _btnAdd.Click += BtnAdd_Click;

            _btnEdit = CreateButton("✏️ Edit", Color.FromArgb(59, 130, 246));
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = CreateButton("🗑️ Delete", Color.FromArgb(239, 68, 68));
            _btnDelete.Click += BtnDelete_Click;

            flowPanel.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete });
            _toolbar.Controls.Add(flowPanel);

            // Grid
            _grid = new ModernDataGridView
            {
                Dock = DockStyle.Fill,
                ShowPagination = false
            };
            _grid.CellDoubleClicked += Grid_CellDoubleClicked;

            // Bottom Panel with Close button
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(15)
            };

            _btnClose = new Button
            {
                Text = "Close",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(100, 38),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Right
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.Click += (s, e) => this.Close();

            bottomPanel.Controls.Add(_btnClose);
            bottomPanel.Resize += (s, e) =>
            {
                _btnClose.Location = new Point(
                    bottomPanel.Width - _btnClose.Width - 15,
                    (bottomPanel.Height - _btnClose.Height) / 2);
            };

            // Add controls (reverse dock order)
            this.Controls.Add(_grid);
            this.Controls.Add(_toolbar);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(_headerPanel);
            this.Controls.Add(_notification);

            this.ResumeLayout(false);
        }

        private Button CreateButton(string text, Color bgColor)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(130, 35),
                Cursor = Cursors.Hand,
                BackColor = bgColor,
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 10, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
        #endregion

        #region Data Loading
        private void LoadPaymentMethods()
        {
            try
            {
                var dt = clsPaymentMethod.GetAllPaymentMethod();
                _grid.SetDataSource(dt);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading payment methods: {ex.Message}");
            }
        }

        private void ConfigureGridColumns()
        {
            _grid.ConfigureColumn("PaymentMethodID", "ID", 60, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _grid.ConfigureColumn("MethodName", "Method Name", 180);
            _grid.ConfigureColumn("Description", "Description", 250);
            _grid.ConfigureColumn("IsActiveForSales", "Active for Sales", 120,
                true, DataGridViewContentAlignment.MiddleCenter);
            _grid.ConfigureColumn("IsActiveForPurchases", "Active for Purchases", 140,
                true, DataGridViewContentAlignment.MiddleCenter);
        }
        #endregion

        #region CRUD
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (var frm = new frmAddEditPaymentMethod())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadPaymentMethods();
                    _notification.ShowSuccess("Payment method added successfully");
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            EditSelectedPaymentMethod();
        }

        private void Grid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedPaymentMethod();
        }

        private void EditSelectedPaymentMethod()
        {
            if (_grid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a payment method to edit");
                return;
            }

            var row = _grid.DataGridView.SelectedRows[0];
            if (row.Cells["PaymentMethodID"]?.Value is int id)
            {
                using (var frm = new frmAddEditPaymentMethod(id))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadPaymentMethods();
                        _notification.ShowSuccess("Payment method updated successfully");
                    }
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_grid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a payment method to delete");
                return;
            }

            var row = _grid.DataGridView.SelectedRows[0];
            if (row.Cells["PaymentMethodID"]?.Value is int id)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this payment method?\n\nNote: Payment methods linked to transactions cannot be deleted.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (clsPaymentMethod.DeletePaymentMethod(id))
                        {
                            LoadPaymentMethods();
                            _notification.ShowSuccess("Payment method deleted successfully");
                        }
                        else
                        {
                            _notification.ShowError("Failed to delete payment method. It may be linked to transactions.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _notification.ShowError($"Error: {ex.Message}");
                    }
                }
            }
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;

            if (_headerPanel != null)
                _headerPanel.BackColor = colors.Primary;

            if (_toolbar != null)
                _toolbar.BackColor = colors.ContentBackground;

            _grid?.ApplyTheme();

            Invalidate(true);
        }
        #endregion
    }
}
