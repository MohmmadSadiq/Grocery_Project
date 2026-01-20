using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    /// <summary>
    /// Dialog for managing product-specific units (ProductUnits).
    /// </summary>
    public partial class ProductUnitsDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private ModernDataGridView _unitsGrid = null!;
        private Panel _toolbarPanel = null!;
        private Button _btnAdd = null!;
        private Button _btnEdit = null!;
        private Button _btnDelete = null!;
        private Button _btnClose = null!;
        private NotificationControl _notification = null!;

        private int _productId;
        private string _productName;
        #endregion

        #region Constructor
        public ProductUnitsDialog(int productId, string productName)
        {
            _productId = productId;
            _productName = productName;

            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void ProductUnitsDialog_Load(object sender, EventArgs e)
        {
            LoadProductUnits();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = $"Product Units: {_productName}";
            this.Size = new Size(900, 550);
            this.MinimumSize = new Size(700, 400);
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
                BackColor = Color.FromArgb(139, 92, 246), // Purple
                Padding = new Padding(20, 0, 20, 0)
            };

            _titleLabel = new Label
            {
                Text = $"📦 Product Units: {_productName}",
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = Color.White,
                AutoSize = true
            };
            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Resize += (s, e) =>
            {
                _titleLabel.Location = new Point(20, (_headerPanel.Height - _titleLabel.Height) / 2);
            };

            // Toolbar Panel
            _toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                Padding = new Padding(15, 10, 15, 10)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnAdd = CreateButton("➕ Add Unit", Color.FromArgb(34, 197, 94));
            _btnAdd.Click += BtnAdd_Click;

            _btnEdit = CreateButton("✏️ Edit", Color.FromArgb(59, 130, 246));
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = CreateButton("🗑️ Delete", Color.FromArgb(239, 68, 68));
            _btnDelete.Click += BtnDelete_Click;

            flowPanel.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete });
            _toolbarPanel.Controls.Add(flowPanel);

            // Grid
            _unitsGrid = new ModernDataGridView
            {
                Dock = DockStyle.Fill,
                ShowPagination = false
            };
            _unitsGrid.CellDoubleClicked += UnitsGrid_CellDoubleClicked;

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
            _btnClose.Click += BtnClose_Click;

            bottomPanel.Controls.Add(_btnClose);
            bottomPanel.Resize += BottomPanel_Resize;

            // Add controls
            this.Controls.Add(_unitsGrid);
            this.Controls.Add(_toolbarPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(_headerPanel);
            this.Controls.Add(_notification);

            this.ResumeLayout(false);
        }

        private void UnitsGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedUnit();
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void BottomPanel_Resize(object? sender, EventArgs e)
        {
            if (sender is Panel panel)
                _btnClose.Location = new Point(panel.Width - _btnClose.Width - 15, (panel.Height - _btnClose.Height) / 2);
        }
        private Button CreateButton(string text, Color bgColor)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(120, 35),
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
        private void LoadProductUnits()
        {
            try
            {
                var dt = clsProductUnit.GetProductUnitsByProductID(_productId);
                
                // Add UnitName column if not exists (from join with Units table)
                if (!dt.Columns.Contains("UnitName"))
                {
                    dt.Columns.Add("UnitName", typeof(string));
                    
                    // Populate UnitName from Unit table
                    var unitsTable = clsUnit.GetAllUnit();
                    foreach (DataRow row in dt.Rows)
                    {
                        int unitId = (int)row["UnitID"];
                        var unitRows = unitsTable.Select($"UnitID = {unitId}");
                        if (unitRows.Length > 0)
                        {
                            row["UnitName"] = unitRows[0]["UnitName"];
                        }
                    }
                }

                _unitsGrid.SetDataSource(dt);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading units: {ex.Message}");
            }
        }

        private void ConfigureGridColumns()
        {
            _unitsGrid.ConfigureColumn("ProductUnitID", "ID", 70, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _unitsGrid.ConfigureColumn("UnitName", "Unit Name", 120);
            _unitsGrid.ConfigureColumn("ConversionFactor", "Conversion Factor", 100,
                true, DataGridViewContentAlignment.MiddleCenter);
            _unitsGrid.ConfigureColumn("SalePrice", "Sale Price", 100,
                true, DataGridViewContentAlignment.MiddleCenter);
            _unitsGrid.ConfigureColumn("Barcode", "Barcode", 140,
                true, DataGridViewContentAlignment.MiddleCenter, "Consolas", 9f);
            _unitsGrid.ConfigureColumn("Description", "Description", 150);
            _unitsGrid.ConfigureColumn("IsActive", "Status", 80,
                true, DataGridViewContentAlignment.MiddleCenter);

            // Hide internal columns
            _unitsGrid.ConfigureColumn("ProductID", "", 0, false);
            _unitsGrid.ConfigureColumn("UnitID", "", 0, false);
            _unitsGrid.ConfigureColumn("CreatedDate", "", 0, false);
            _unitsGrid.ConfigureColumn("CreatedByUserID", "", 0, false);
            _unitsGrid.ConfigureColumn("UpdatedDate", "", 0, false);
            _unitsGrid.ConfigureColumn("UpdatedByUserID", "", 0, false);
        }
        #endregion

        #region CRUD Operations
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (var dialog = new ProductUnitDialog(_productId))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProductUnits();
                    _notification.ShowSuccess("Unit added successfully");
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            EditSelectedUnit();
        }

        private void EditSelectedUnit()
        {
            if (_unitsGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a unit to edit");
                return;
            }

            var row = _unitsGrid.DataGridView.SelectedRows[0];
            if (row.Cells["ProductUnitID"]?.Value is int productUnitId)
            {
                using (var dialog = new ProductUnitDialog(_productId, productUnitId))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadProductUnits();
                        _notification.ShowSuccess("Unit updated successfully");
                    }
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_unitsGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a unit to delete");
                return;
            }

            var row = _unitsGrid.DataGridView.SelectedRows[0];
            if (row.Cells["ProductUnitID"]?.Value is int productUnitId)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this unit?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (clsProductUnit.DeleteProductUnit(productUnitId))
                        {
                            LoadProductUnits();
                            _notification.ShowSuccess("Unit deleted successfully");
                        }
                        else
                        {
                            _notification.ShowError("Failed to delete unit");
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
                _headerPanel.BackColor = Color.FromArgb(139, 92, 246); // Keep purple

            if (_toolbarPanel != null)
                _toolbarPanel.BackColor = colors.ContentBackground;

            _unitsGrid?.ApplyTheme();

            Invalidate(true);
        }
        #endregion
    }
}
