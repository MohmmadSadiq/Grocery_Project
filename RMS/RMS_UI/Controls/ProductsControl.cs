using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    /// <summary>
    /// Products management control with single-screen layout.
    /// All add/edit operations happen through ProductDialog.
    /// </summary>
    [DesignerCategory("UserControl")]
    public partial class ProductsControl : UserControl
    {
        #region Constructor
        public ProductsControl()
        {
            InitializeComponent();
            CreateHeaderPanel();
            ConfigureDataGrid();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void ProductsControl_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void ConfigureDataGrid()
        {
            // Configure tabs
            _dataGrid.SetTabs(
                new TabDefinition("All", "All"),
                new TabDefinition("Active", "Active"),
                new TabDefinition("Inactive", "Inactive")
            );

            // Configure search fields
            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("Product Name", "ProductName"),
                new SearchFieldDefinition("Product ID", "ProductID")
            );

            // Configure filter
            _dataGrid.SetFilter(new FilterDefinition(
                displayName: "Category",
                filterKey: "CategoryID",
                dataSource: new Func<DataTable>(() => clsCategory.GetAllCategory()),
                valueColumn: "CategoryID",
                displayColumn: "CategoryName",
                allItemsText: "All Categories"
            ));

            // Wire up filter changed event
            _dataGrid.FilterChanged += DataGrid_FilterChanged;

            // Enable checkbox column and context menu
            _dataGrid.ShowCheckboxColumn = true;
            _dataGrid.ShowContextMenu = true;

            // Add context menu items
            _dataGrid.AddStandardStatusMenuItems(hasActivate: true, hasDeactivate: true, hasDelete: true, hasExport: true);
            _dataGrid.AddContextMenuSeparator();
            _dataGrid.AddContextMenuItem("📁 Move to Category", (s, e) => DataGrid_MoveToCategorySelected(s!, e));

            // Wire up clear search event
            _dataGrid.ClearSearchClicked += DataGrid_ClearSearchClicked;

            // Finalize setup (adds checkbox column if enabled)
            _dataGrid.FinalizeSetup();
        }
        #endregion

        #region Header Panel
        private void CreateHeaderPanel()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White,
                Padding = new Padding(30, 20, 30, 20)
            };

            // Title
            _lblTitle = new Label
            {
                Text = "Product Management",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Subtitle
            _lblSubtitle = new Label
            {
                Text = "Manage your catalog, track inventory, and update pricing.",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(30, 58)
            };

            // New Product Button
            _btnNewProduct = new Button
            {
                Text = "+ New Product",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 42),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnNewProduct.FlatAppearance.BorderSize = 0;
            _btnNewProduct.Click += BtnNewProduct_Click;

            // Settings Button
            _btnSettings = new Button
            {
                Text = "⚙ Settings",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(110, 42),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnSettings.FlatAppearance.BorderSize = 0;
            _btnSettings.Click += BtnSettings_Click;

            // Position buttons
            PositionHeaderButtons();

            _headerPanel.Resize += (s, e) => PositionHeaderButtons();

            _headerPanel.Controls.AddRange(new Control[]
            {
                _lblTitle,
                _lblSubtitle,
                _btnNewProduct,
                _btnSettings
            });
        }

        private void PositionHeaderButtons()
        {
            _btnSettings.Location = new Point(_headerPanel.Width - _btnSettings.Width - 30, 29);
            _btnNewProduct.Location = new Point(_btnSettings.Left - _btnNewProduct.Width - 10, 29);
        }
        #endregion

        #region Data Loading
        private void LoadProducts()
        {
            // Get IsActive filter from current tab
            bool? isActiveFilter = _dataGrid.CurrentTab switch
            {
                "Active" => true,
                "Inactive" => false,
                _ => null // "All" tab
            };

            // Map SearchField to SearchBy parameter
            string searchBy = _dataGrid.SearchField switch
            {
                "ProductName" => "Name",
                "ProductID" => "ID",
                _ => "Name"
            };

            clsProduct.ProductSearchCriteria searchCriteria = new clsProduct.ProductSearchCriteria
            {
                SearchText = _dataGrid.SearchText,
                SearchBy = searchBy,
                CategoryId = (int?)_dataGrid.SelectedFilterValue,
                IsActive = isActiveFilter,
                PageNumber = _dataGrid.CurrentPage,
                PageSize = _dataGrid.PageSize,
                SortBy = "ID"
            };

            try
            {
                var filteredData = clsProduct.SearchProductsPages(searchCriteria);
                
                // Get TotalCount from first row if available
                int totalCount = 0;
                if (filteredData.Rows.Count > 0 && filteredData.Columns.Contains("TotalCount"))
                {
                    totalCount = Convert.ToInt32(filteredData.Rows[0]["TotalCount"]);
                }

                _dataGrid.SetDataSource(filteredData, totalCount);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading products: {ex.Message}");
            }
        }

        /* Removed - Filtering now done in Database via sp_SearchProductsPages
        private DataTable FilterData(DataTable? dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt ?? new DataTable();

            var filteredDt = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                bool include = true;

                // Filter by Category using the new filter
                var selectedCategoryId = _dataGrid.SelectedFilterValue;
                if (selectedCategoryId != null)
                {
                    int rowCategoryId = row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : -1;
                    include = rowCategoryId == Convert.ToInt32(selectedCategoryId);
                }

                // Filter by IsActive tab
                if (include)
                {
                    var isActiveFilter = GetIsActiveFilter();
                    if (isActiveFilter.HasValue)
                    {
                        bool rowIsActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"];
                        include = rowIsActive == isActiveFilter.Value;
                    }
                }

                // Filter by search text
                if (include && !string.IsNullOrWhiteSpace(_dataGrid.SearchText))
                {
                    string searchText = _dataGrid.SearchText.ToLower();
                    string searchField = _dataGrid.SearchField;

                    if (searchField == "ProductName")
                    {
                        string productName = row["ProductName"]?.ToString()?.ToLower() ?? "";
                        include = productName.Contains(searchText);
                    }
                    else if (searchField == "ProductID")
                    {
                        string productId = row["ProductID"]?.ToString() ?? "";
                        include = productId.Contains(searchText);
                    }
                }

                if (include)
                    filteredDt.ImportRow(row);
            }

            return filteredDt;
        }
        */

        private void ConfigureGridColumns()
        {
            _dataGrid.ConfigureColumn("ProductID", "ID", 70, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("ProductName", "Product Name", 200, true,
                DataGridViewContentAlignment.MiddleLeft);
            _dataGrid.ConfigureColumn("CategoryName", "Category", 130);
            _dataGrid.ConfigureColumn("BrandName", "Brand", 130);
            _dataGrid.ConfigureColumn("CompanyName", "Company", 130);
            _dataGrid.ConfigureColumn("ReorderLevel", "Reorder Lvl", 90,
                true, DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("IsActive", "Status", 80,
                true, DataGridViewContentAlignment.MiddleCenter);

            // Hide unnecessary columns
            _dataGrid.ConfigureColumn("ImagePath", "", 0, false);
            _dataGrid.ConfigureColumn("TotalCount", "", 0, false);

            // Make last column fill remaining space
            _dataGrid.FillLastColumn();

            // Enable sorting
            foreach (DataGridViewColumn col in _dataGrid.DataGridView.Columns)
            {
                if (col.Name != "SelectCheckbox")
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }
        #endregion

        #region Grid Events
        private void DataGrid_TabChanged(object sender, TabChangedEventArgs e)
        {
            LoadProducts();
        }

        private void DataGrid_SearchRequested(object sender, SearchEventArgs e)
        {
            LoadProducts();
        }

        private void DataGrid_PageChanged(object sender, PageChangedEventArgs e)
        {
            LoadProducts();
        }

        private void DataGrid_CellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = _dataGrid.DataGridView.Rows[e.RowIndex];
                if (row.Cells["ProductID"]?.Value is int productId)
                {
                    OpenProductDialog(productId);
                }
            }
        }

        private void DataGrid_ClearFiltersClicked(object sender, EventArgs e)
        {
            _dataGrid.ClearAll();
            LoadProducts();
        }

        private void DataGrid_ClearSearchClicked(object? sender, EventArgs e)
        {
            _dataGrid.ClearAll();
            _dataGrid.ClearSearch();
            LoadProducts();
        }

        private void DataGrid_FilterChanged(object? sender, FilterChangedEventArgs e)
        {
            LoadProducts();
        }

        private void DataGrid_ActivateSelected(object sender, EventArgs e)
        {
            BulkUpdateStatus(true);
        }

        private void DataGrid_DeactivateSelected(object sender, EventArgs e)
        {
            BulkUpdateStatus(false);
        }

        private void DataGrid_DeleteSelected(object sender, EventArgs e)
        {
            BulkDelete();
        }

        private void DataGrid_ExportToExcelSelected(object sender, EventArgs e)
        {
            _notification.ShowInfo("Export to Excel feature coming soon");
        }

        private void DataGrid_MoveToCategorySelected(object sender, EventArgs e)
        {
            _notification.ShowInfo("Move to Category feature coming soon");
        }
        #endregion

        #region Button Handlers
        private void BtnNewProduct_Click(object? sender, EventArgs e)
        {
            OpenProductDialog(-1);

        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var dialog = new ProductSettingsDialog())
                {
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error opening settings: {ex.Message}");
            }
        }
        #endregion

        #region Dialog Operations
        private void OpenProductDialog(int productId)
        {
            try
            {
                using (var dialog = new ProductDialog(productId))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadProducts();
                        _notification.ShowSuccess(productId == -1 ?
                            "Product added successfully" :
                            "Product updated successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error: {ex.Message}");
            }
        }
        #endregion

        #region Bulk Operations
        private void BulkUpdateStatus(bool isActive)
        {
            var checkedRows = _dataGrid.GetCheckedRows();
            if (checkedRows.Count == 0)
            {
                _notification.ShowWarning("Please select products first");
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to {(isActive ? "activate" : "deactivate")} {checkedRows.Count} product(s)?",
                "Confirm Status Change",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int successCount = 0;
                foreach (var row in checkedRows)
                {
                    if (row.Cells["ProductID"]?.Value is int productId)
                    {
                        var product = clsProduct.Find(productId);
                        if (product != null)
                        {
                            product.IsActive = isActive;
                            if (product.Save())
                                successCount++;
                        }
                    }
                }

                _notification.ShowSuccess($"{successCount} product(s) updated successfully");
                LoadProducts();
            }
        }

        private void BulkDelete()
        {
            var checkedRows = _dataGrid.GetCheckedRows();
            if (checkedRows.Count == 0)
            {
                _notification.ShowWarning("Please select products to delete");
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {checkedRows.Count} product(s)?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                int successCount = 0;
                foreach (var row in checkedRows)
                {
                    if (row.Cells["ProductID"]?.Value is int productId)
                    {
                        if (clsProduct.DeleteProduct(productId))
                            successCount++;
                    }
                }

                _notification.ShowSuccess($"{successCount} product(s) deleted successfully");
                LoadProducts();
            }
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.ContentBackground;

            if (_headerPanel != null)
                _headerPanel.BackColor = colors.ContentBackground;

            if (_lblTitle != null)
                _lblTitle.ForeColor = colors.PrimaryText;

            if (_lblSubtitle != null)
                _lblSubtitle.ForeColor = colors.SecondaryText;

            _dataGrid?.ApplyTheme();

            Invalidate(true);
        }
        #endregion
    }
}
