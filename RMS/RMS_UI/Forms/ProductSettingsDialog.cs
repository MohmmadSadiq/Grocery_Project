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
    /// Dialog for managing general settings including Units and Categories.
    /// </summary>
    public partial class ProductSettingsDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private TabControl _tabControl = null!;
        
        // Units Tab
        private TabPage _tabUnits = null!;
        private ModernDataGridView _unitsGrid = null!;
        private Panel _unitsToolbar = null!;
        private Button _btnAddUnit = null!;
        private Button _btnEditUnit = null!;
        private Button _btnDeleteUnit = null!;
        
        // Categories Tab
        private TabPage _tabCategories = null!;
        private ModernDataGridView _categoriesGrid = null!;
        private Panel _categoriesToolbar = null!;
        private Button _btnAddCategory = null!;
        private Button _btnEditCategory = null!;
        private Button _btnDeleteCategory = null!;
        
        private Button _btnClose = null!;
        private NotificationControl _notification = null!;
        #endregion

        #region Constructor
        public ProductSettingsDialog()
        {
            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void ProductSettingsDialog_Load(object sender, EventArgs e)
        {
            LoadUnits();
            LoadCategories();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Product Settings";
            this.Size = new Size(800, 600);
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
                Text = "⚙️ Product Settings",
                Font = new Font("Segoe UI Semibold", 16F),
                ForeColor = Color.White,
                AutoSize = true
            };
            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Resize += (s, e) =>
            {
                _titleLabel.Location = new Point(20, (_headerPanel.Height - _titleLabel.Height) / 2);
            };

            // Tab Control
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                Padding = new Point(15, 8)
            };

            // Units Tab
            _tabUnits = new TabPage
            {
                Text = "📦 Units",
                Padding = new Padding(15)
            };

            CreateUnitsTab();
            _tabControl.TabPages.Add(_tabUnits);

            // Categories Tab
            _tabCategories = new TabPage
            {
                Text = "📁 Categories",
                Padding = new Padding(15)
            };

            CreateCategoriesTab();
            _tabControl.TabPages.Add(_tabCategories);

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
                _btnClose.Location = new Point(bottomPanel.Width - _btnClose.Width - 15, (bottomPanel.Height - _btnClose.Height) / 2);
            };

            // Add controls
            this.Controls.Add(_tabControl);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(_headerPanel);
            this.Controls.Add(_notification);

            this.ResumeLayout(false);
        }

        private void CreateUnitsTab()
        {
            // Toolbar
            _unitsToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(0, 5, 0, 5)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnAddUnit = CreateButton("➕ Add Unit", Color.FromArgb(34, 197, 94));
            _btnAddUnit.Click += BtnAddUnit_Click;

            _btnEditUnit = CreateButton("✏️ Edit", Color.FromArgb(59, 130, 246));
            _btnEditUnit.Click += BtnEditUnit_Click;

            _btnDeleteUnit = CreateButton("🗑️ Delete", Color.FromArgb(239, 68, 68));
            _btnDeleteUnit.Click += BtnDeleteUnit_Click;

            flowPanel.Controls.AddRange(new Control[] { _btnAddUnit, _btnEditUnit, _btnDeleteUnit });
            _unitsToolbar.Controls.Add(flowPanel);

            // Grid
            _unitsGrid = new ModernDataGridView
            {
                Dock = DockStyle.Fill,
                ShowPagination = false
            };
            _unitsGrid.CellDoubleClicked += UnitsGrid_CellDoubleClicked;

            _tabUnits.Controls.Add(_unitsGrid);
            _tabUnits.Controls.Add(_unitsToolbar);
        }

        private void CreateCategoriesTab()
        {
            // Toolbar
            _categoriesToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(0, 5, 0, 5)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnAddCategory = CreateButton("➕ Add Category", Color.FromArgb(34, 197, 94));
            _btnAddCategory.Click += BtnAddCategory_Click;

            _btnEditCategory = CreateButton("✏️ Edit", Color.FromArgb(59, 130, 246));
            _btnEditCategory.Click += BtnEditCategory_Click;

            _btnDeleteCategory = CreateButton("🗑️ Delete", Color.FromArgb(239, 68, 68));
            _btnDeleteCategory.Click += BtnDeleteCategory_Click;

            flowPanel.Controls.AddRange(new Control[] { _btnAddCategory, _btnEditCategory, _btnDeleteCategory });
            _categoriesToolbar.Controls.Add(flowPanel);

            // Grid
            _categoriesGrid = new ModernDataGridView
            {
                Dock = DockStyle.Fill,
                ShowPagination = false
            };
            _categoriesGrid.CellDoubleClicked += CategoriesGrid_CellDoubleClicked;

            _tabCategories.Controls.Add(_categoriesGrid);
            _tabCategories.Controls.Add(_categoriesToolbar);
        }

        private void CategoriesGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedCategory();
        }

        private void UnitsGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedUnit();
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
        private void LoadUnits()
        {
            try
            {
                var dt = clsUnit.GetAllUnit();
                _unitsGrid.SetDataSource(dt);

                ConfigureUnitsGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading units: {ex.Message}");
            }
        }

        private void ConfigureUnitsGridColumns()
        {
            _unitsGrid.ConfigureColumn("UnitID", "Unit ID", 90, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _unitsGrid.ConfigureColumn("UnitName", "Unit Name", 150);
            _unitsGrid.ConfigureColumn("Description", "Description", 250);
            _unitsGrid.ConfigureColumn("IsActive", "Status", 90,
                true, DataGridViewContentAlignment.MiddleCenter);
            _unitsGrid.ConfigureColumn("CreatedDate", "Created Date", 150);
            _unitsGrid.ConfigureColumn("CreatedByUserID", "", 0, false);
        }

        private void LoadCategories()
        {
            try
            {
                var dt = clsCategory.GetAllCategory();
                _categoriesGrid.SetDataSource(dt);

                ConfigureCategoriesGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading categories: {ex.Message}");
            }
        }

        private void ConfigureCategoriesGridColumns()
        {
            _categoriesGrid.ConfigureColumn("CategoryID", "Category ID", 100, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _categoriesGrid.ConfigureColumn("CategoryName", "Category Name", 180);
            _categoriesGrid.ConfigureColumn("Description", "Description", 280);
            _categoriesGrid.ConfigureColumn("CreatedDate", "Created Date", 150);
            _categoriesGrid.ConfigureColumn("CreatedByUserID", "", 0, false);
        }
        #endregion

        #region Unit CRUD
        private void BtnAddUnit_Click(object? sender, EventArgs e)
        {
            using (var dialog = new UnitDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUnits();
                    _notification.ShowSuccess("Unit added successfully");
                }
            }
        }

        private void BtnEditUnit_Click(object? sender, EventArgs e)
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
            if (row.Cells["UnitID"]?.Value is int unitId)
            {
                using (var dialog = new UnitDialog(unitId))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadUnits();
                        _notification.ShowSuccess("Unit updated successfully");
                    }
                }
            }
        }

        private void BtnDeleteUnit_Click(object? sender, EventArgs e)
        {
            if (_unitsGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a unit to delete");
                return;
            }

            var row = _unitsGrid.DataGridView.SelectedRows[0];
            if (row.Cells["UnitID"]?.Value is int unitId)
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
                        if (clsUnit.DeleteUnit(unitId))
                        {
                            LoadUnits();
                            _notification.ShowSuccess("Unit deleted successfully");
                        }
                        else
                        {
                            _notification.ShowError("Failed to delete unit. It may be linked to products.");
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

        #region Category CRUD
        private void BtnAddCategory_Click(object? sender, EventArgs e)
        {
            using (var dialog = new CategoryDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCategories();
                    _notification.ShowSuccess("Category added successfully");
                }
            }
        }

        private void BtnEditCategory_Click(object? sender, EventArgs e)
        {
            EditSelectedCategory();
        }

        private void EditSelectedCategory()
        {
            if (_categoriesGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a category to edit");
                return;
            }

            var row = _categoriesGrid.DataGridView.SelectedRows[0];
            if (row.Cells["CategoryID"]?.Value is int categoryId)
            {
                using (var dialog = new CategoryDialog(categoryId))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadCategories();
                        _notification.ShowSuccess("Category updated successfully");
                    }
                }
            }
        }

        private void BtnDeleteCategory_Click(object? sender, EventArgs e)
        {
            if (_categoriesGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a category to delete");
                return;
            }

            var row = _categoriesGrid.DataGridView.SelectedRows[0];
            if (row.Cells["CategoryID"]?.Value is int categoryId)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this category?\n\nNote: Categories linked to products cannot be deleted.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (clsCategory.DeleteCategory(categoryId))
                        {
                            LoadCategories();
                            _notification.ShowSuccess("Category deleted successfully");
                        }
                        else
                        {
                            _notification.ShowError("Failed to delete category. It may be linked to products.");
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

            if (_tabControl != null)
            {
                _tabControl.BackColor = colors.ContentBackground;
                foreach (TabPage tab in _tabControl.TabPages)
                {
                    tab.BackColor = colors.ContentBackground;
                }
            }

            _unitsGrid?.ApplyTheme();
            _categoriesGrid?.ApplyTheme();

            Invalidate(true);
        }
        #endregion
    }
}
