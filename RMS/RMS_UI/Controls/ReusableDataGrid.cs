using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    /// <summary>
    /// Extended grid with tabs for filtering, search functionality, empty state, and context menu for bulk actions.
    /// </summary>
    [DesignerCategory("UserControl")]
    public partial class ReusableDataGrid : UserControl
    {
        #region Private Fields
        private string _currentTab = "All";
        private string _searchText = "";
        private string _searchField = "ProductName";
        #endregion

        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ModernDataGridView GridView => _gridView;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DataGridView DataGridView => _gridView.DataGridView;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string CurrentTab => _currentTab;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string SearchText => _searchText;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string SearchField => _searchField;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int CurrentPage
        {
            get => _gridView.CurrentPage;
            set => _gridView.CurrentPage = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int PageSize
        {
            get => _gridView.PageSize;
            set => _gridView.PageSize = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int TotalRecords
        {
            get => _gridView.TotalRecords;
            set => _gridView.TotalRecords = value;
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowTabs { get; set; } = true;

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowSearch { get; set; } = true;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ContextMenuStrip BulkActionsMenu => _contextMenu;
        #endregion

        #region Events
        public event EventHandler<TabChangedEventArgs>? TabChanged;
        public event EventHandler<SearchEventArgs>? SearchRequested;
        public event EventHandler<PageChangedEventArgs>? PageChanged;
        public event EventHandler<DataGridViewCellEventArgs>? CellDoubleClicked;
        public event EventHandler? SelectionChanged;
        public event EventHandler? ClearFiltersClicked;

        // Bulk action events
        public event EventHandler? ActivateSelected;
        public event EventHandler? DeactivateSelected;
        public event EventHandler? MoveToCategorySelected;
        public event EventHandler? ExportToExcelSelected;
        public event EventHandler? DeleteSelected;
        #endregion

        public ReusableDataGrid()
        {
            InitializeComponent();
            CreateTabButtons();
            CreateSearchControls();
            CreateEmptyStatePanel();
            CreateContextMenu();
            _gridView.AddCheckboxColumn();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        #region Designer Event Handlers
        private void ReusableDataGrid_Load(object sender, EventArgs e)
        {
            // Initial setup
        }

        private void GridView_PageChanged(object sender, PageChangedEventArgs e)
        {
            PageChanged?.Invoke(this, e);
        }

        private void GridView_CellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            CellDoubleClicked?.Invoke(this, e);
        }

        private void GridView_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnClearFilters_Click(object? sender, EventArgs e)
        {
            ClearSearch();
            _currentTab = "All";
            UpdateTabSelection("All");
            ClearFiltersClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Tab Buttons
        private void CreateTabButtons()
        {
            var tabFont = new Font("Segoe UI Semibold", 9.5F);

            _btnAll = CreateTabButton("All", "All", tabFont);
            _btnActive = CreateTabButton("Active", "Active", tabFont);
            _btnInactive = CreateTabButton("Inactive", "Inactive", tabFont);

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0)
            };

            flowPanel.Controls.Add(_btnAll);
            flowPanel.Controls.Add(_btnActive);
            flowPanel.Controls.Add(_btnInactive);

            _tabPanel.Controls.Add(flowPanel);

            // Set initial selection
            UpdateTabSelection("All");
        }

        private Button CreateTabButton(string text, string tag, Font font)
        {
            var btn = new Button
            {
                Text = text,
                Tag = tag,
                FlatStyle = FlatStyle.Flat,
                Font = font,
                Size = new Size(100, 35),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 5, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += TabButton_Click;
            return btn;
        }

        private void TabButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tab)
            {
                _currentTab = tab;
                UpdateTabSelection(tab);
                TabChanged?.Invoke(this, new TabChangedEventArgs(tab));
            }
        }

        private void UpdateTabSelection(string selectedTab)
        {
            var colors = ThemeManager.Colors;

            foreach (var btn in new[] { _btnAll, _btnActive, _btnInactive })
            {
                if (btn.Tag?.ToString() == selectedTab)
                {
                    btn.BackColor = colors.Primary;
                    btn.ForeColor = Color.White;
                }
                else
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = colors.SecondaryText;
                }
            }
        }
        #endregion

        #region Search Controls
        private void CreateSearchControls()
        {
            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false
            };

            // Search Field ComboBox
            _cmbSearchField = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Width = 140,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 10, 0)
            };
            _cmbSearchField.Items.Add(new ComboBoxItem("Product Name", "ProductName"));
            _cmbSearchField.Items.Add(new ComboBoxItem("Product ID", "ProductID"));
            _cmbSearchField.DisplayMember = "Text";
            _cmbSearchField.ValueMember = "Value";
            _cmbSearchField.SelectedIndex = 0;
            _cmbSearchField.SelectedIndexChanged += (s, e) =>
            {
                if (_cmbSearchField.SelectedItem is ComboBoxItem item)
                    _searchField = item.Value;
            };

            // Search TextBox
            _txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Width = 250,
                Height = 32,
                Margin = new Padding(0, 0, 10, 0)
            };
            _txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    PerformSearch();
                }
            };

            // Search Button
            _btnSearch = new Button
            {
                Text = "🔍 Search",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(80, 32),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 5, 0)
            };
            _btnSearch.FlatAppearance.BorderSize = 1;
            _btnSearch.Click += (s, e) => PerformSearch();

            // Clear Search Button
            _btnClearSearch = new Button
            {
                Text = "✕ Clear",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(70, 32),
                Cursor = Cursors.Hand
            };
            _btnClearSearch.FlatAppearance.BorderSize = 1;
            _btnClearSearch.Click += (s, e) => ClearSearch();

            flowPanel.Controls.Add(_cmbSearchField);
            flowPanel.Controls.Add(_txtSearch);
            flowPanel.Controls.Add(_btnSearch);
            flowPanel.Controls.Add(_btnClearSearch);

            _searchPanel.Controls.Add(flowPanel);
        }

        private void PerformSearch()
        {
            _searchText = _txtSearch.Text.Trim();
            _gridView.ResetToFirstPage();
            SearchRequested?.Invoke(this, new SearchEventArgs(_searchText, _searchField));
        }

        private void ClearSearch()
        {
            _txtSearch.Text = "";
            _searchText = "";
            _gridView.ResetToFirstPage();
            SearchRequested?.Invoke(this, new SearchEventArgs("", _searchField));
        }
        #endregion

        #region Empty State
        private void CreateEmptyStatePanel()
        {
            // Clear existing controls from designer-created panel and reconfigure
            _emptyStatePanel.Controls.Clear();
            _emptyStatePanel.Dock = DockStyle.Fill;
            _emptyStatePanel.BackColor = Color.White;
            _emptyStatePanel.Visible = false;

            var centerPanel = new Panel
            {
                Size = new Size(300, 200),
                BackColor = Color.Transparent
            };

            _emptyIcon = new Label
            {
                Text = "📦",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _emptyMessage = new Label
            {
                Text = "No results found",
                Font = new Font("Segoe UI", 14F),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _btnClearFilters = new Button
            {
                Text = "Clear Filters",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(120, 35),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White
            };
            _btnClearFilters.FlatAppearance.BorderSize = 0;
            _btnClearFilters.Click += BtnClearFilters_Click;

            centerPanel.Controls.Add(_emptyIcon);
            centerPanel.Controls.Add(_emptyMessage);
            centerPanel.Controls.Add(_btnClearFilters);

            _emptyStatePanel.Controls.Add(centerPanel);

            _emptyStatePanel.Resize += (s, e) =>
            {
                centerPanel.Location = new Point(
                    (_emptyStatePanel.Width - centerPanel.Width) / 2,
                    (_emptyStatePanel.Height - centerPanel.Height) / 2
                );

                _emptyIcon.Location = new Point(
                    (centerPanel.Width - _emptyIcon.Width) / 2, 0
                );
                _emptyMessage.Location = new Point(
                    (centerPanel.Width - _emptyMessage.Width) / 2,
                    _emptyIcon.Bottom + 10
                );
                _btnClearFilters.Location = new Point(
                    (centerPanel.Width - _btnClearFilters.Width) / 2,
                    _emptyMessage.Bottom + 20
                );
            };

            // Add to controls if not already there and bring to front
            if (!this.Controls.Contains(_emptyStatePanel))
            {
                this.Controls.Add(_emptyStatePanel);
            }
        }

        /// <summary>
        /// Shows or hides the empty state panel
        /// </summary>
        public void ShowEmptyState(bool show, string? message = null)
        {
            if (message != null)
                _emptyMessage.Text = message;

            _emptyStatePanel.Visible = show;
            _gridView.Visible = !show;

            if (show)
                _emptyStatePanel.BringToFront();
        }
        #endregion

        #region Context Menu
        private void CreateContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Font = new Font("Segoe UI", 9.5F);

            // Change Status submenu
            var changeStatusItem = new ToolStripMenuItem("Change Status");
            var activateItem = new ToolStripMenuItem("Activate", null, (s, e) => ActivateSelected?.Invoke(this, EventArgs.Empty));
            var deactivateItem = new ToolStripMenuItem("Deactivate", null, (s, e) => DeactivateSelected?.Invoke(this, EventArgs.Empty));
            changeStatusItem.DropDownItems.AddRange(new ToolStripItem[] { activateItem, deactivateItem });

            // Move to Category
            var moveToCategoryItem = new ToolStripMenuItem("Move to Category", null, (s, e) => MoveToCategorySelected?.Invoke(this, EventArgs.Empty));

            // Export to Excel
            var exportItem = new ToolStripMenuItem("Export to Excel", null, (s, e) => ExportToExcelSelected?.Invoke(this, EventArgs.Empty));

            // Delete
            var deleteItem = new ToolStripMenuItem("Delete Selected", null, (s, e) => DeleteSelected?.Invoke(this, EventArgs.Empty));
            deleteItem.ForeColor = Color.FromArgb(239, 68, 68);

            _contextMenu.Items.AddRange(new ToolStripItem[]
            {
                changeStatusItem,
                moveToCategoryItem,
                new ToolStripSeparator(),
                exportItem,
                new ToolStripSeparator(),
                deleteItem
            });

            // Opening event - check if rows are selected
            _contextMenu.Opening += (s, e) =>
            {
                var checkedRows = _gridView.GetCheckedRows();
                if (checkedRows.Count == 0)
                {
                    e.Cancel = true;
                }
            };

            _gridView.DataGridView.ContextMenuStrip = _contextMenu;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the data source for the grid
        /// </summary>
        public void SetDataSource(object? dataSource, int? totalRecords = null)
        {
            _gridView.SetDataSource(dataSource, totalRecords);

            // Show empty state if no data
            bool isEmpty = false;
            if (dataSource is DataTable dt)
                isEmpty = dt.Rows.Count == 0;
            else if (dataSource == null)
                isEmpty = true;

            ShowEmptyState(isEmpty);
        }

        /// <summary>
        /// Configures a column
        /// </summary>
        public void ConfigureColumn(string columnName, string headerText, int width = -1, bool visible = true,
            DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft,
            string? fontFamily = null, float fontSize = 9.5f)
        {
            _gridView.ConfigureColumn(columnName, headerText, width, visible, alignment, fontFamily, fontSize);
        }

        /// <summary>
        /// Gets checked rows
        /// </summary>
        public List<DataGridViewRow> GetCheckedRows() => _gridView.GetCheckedRows();

        /// <summary>
        /// Select/deselect all checkboxes
        /// </summary>
        public void SelectAllCheckboxes(bool select) => _gridView.SelectAllCheckboxes(select);

        /// <summary>
        /// Adds a search field option to the combo box
        /// </summary>
        public void AddSearchField(string displayText, string fieldName)
        {
            _cmbSearchField.Items.Add(new ComboBoxItem(displayText, fieldName));
        }

        /// <summary>
        /// Clears search field options and adds new ones
        /// </summary>
        public void SetSearchFields(params (string displayText, string fieldName)[] fields)
        {
            _cmbSearchField.Items.Clear();
            foreach (var (displayText, fieldName) in fields)
            {
                _cmbSearchField.Items.Add(new ComboBoxItem(displayText, fieldName));
            }
            if (_cmbSearchField.Items.Count > 0)
                _cmbSearchField.SelectedIndex = 0;
        }

        /// <summary>
        /// Sets tab visibility
        /// </summary>
        public void SetTabVisibility(bool visible)
        {
            _tabPanel.Visible = visible;
            _topPanel.Height = visible ? 100 : 55;
        }

        /// <summary>
        /// Refreshes the current page
        /// </summary>
        public void RefreshCurrentPage() => _gridView.RefreshCurrentPage();

        /// <summary>
        /// Resets to first page
        /// </summary>
        public void ResetToFirstPage() => _gridView.ResetToFirstPage();
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.ContentBackground;

            if (_topPanel != null)
                _topPanel.BackColor = colors.ContentBackground;

            if (_tabPanel != null)
                _tabPanel.BackColor = colors.ContentBackground;

            if (_searchPanel != null)
                _searchPanel.BackColor = colors.ContentBackground;

            // Update tab buttons
            UpdateTabSelection(_currentTab);

            // Search controls
            if (_cmbSearchField != null)
            {
                _cmbSearchField.BackColor = colors.ContentBackground;
                _cmbSearchField.ForeColor = colors.PrimaryText;
            }

            if (_txtSearch != null)
            {
                _txtSearch.BackColor = colors.ContentBackground;
                _txtSearch.ForeColor = colors.PrimaryText;
            }

            if (_btnSearch != null)
            {
                _btnSearch.BackColor = colors.Primary;
                _btnSearch.ForeColor = Color.White;
                _btnSearch.FlatAppearance.BorderColor = colors.Primary;
            }

            if (_btnClearSearch != null)
            {
                _btnClearSearch.BackColor = colors.ContentBackground;
                _btnClearSearch.ForeColor = colors.SecondaryText;
                _btnClearSearch.FlatAppearance.BorderColor = colors.BorderColor;
            }

            // Empty state
            if (_emptyStatePanel != null)
            {
                _emptyStatePanel.BackColor = colors.ContentBackground;
            }

            if (_emptyMessage != null)
            {
                _emptyMessage.ForeColor = colors.SecondaryText;
            }

            if (_btnClearFilters != null)
            {
                _btnClearFilters.BackColor = colors.Primary;
            }

            // Context Menu
            if (_contextMenu != null)
            {
                _contextMenu.BackColor = colors.ContentBackground;
                _contextMenu.ForeColor = colors.PrimaryText;

                foreach (ToolStripItem item in _contextMenu.Items)
                {
                    if (item is ToolStripMenuItem menuItem)
                    {
                        if (menuItem.Text == "Delete Selected")
                            menuItem.ForeColor = Color.FromArgb(239, 68, 68);
                        else
                            menuItem.ForeColor = colors.PrimaryText;
                    }
                }
            }

            // Apply theme to grid
            _gridView?.ApplyTheme();

            Invalidate(true);
        }
        #endregion

        #region Helper Classes
        private class ComboBoxItem
        {
            public string Text { get; }
            public string Value { get; }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
        #endregion
    }

    #region Event Args
    public class TabChangedEventArgs : EventArgs
    {
        public string Tab { get; }

        public TabChangedEventArgs(string tab)
        {
            Tab = tab;
        }
    }

    public class SearchEventArgs : EventArgs
    {
        public string SearchText { get; }
        public string SearchField { get; }

        public SearchEventArgs(string searchText, string searchField)
        {
            SearchText = searchText;
            SearchField = searchField;
        }
    }
    #endregion
}
