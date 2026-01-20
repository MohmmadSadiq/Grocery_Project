using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    /// <summary>
    /// Extended grid with tabs for filtering, search functionality, empty state, and context menu for bulk actions.
    /// Generic and reusable for any data type - configure tabs, search fields, and context menu via API.
    /// </summary>
    [DesignerCategory("UserControl")]
    public partial class ReusableDataGrid : UserControl
    {
        #region Private Fields
        private string _currentTab = "All";
        private string _searchText = "";
        private string _searchField = "";
        private List<TabDefinition> _tabDefinitions = new List<TabDefinition>();
        private List<SearchFieldDefinition> _searchFieldDefinitions = new List<SearchFieldDefinition>();
        private bool _checkboxColumnAdded = false;
        
        // Filter fields
        private FilterDefinition? _currentFilter;
        private Label? _filterLabel;
        private ComboBox? _filterCombo;
        private object? _selectedFilterValue;
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

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Set to true to show checkbox column for multi-select. Call before setting data source.")]
        public bool ShowCheckboxColumn { get; set; } = false;

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Set to true to enable context menu for bulk actions.")]
        public bool ShowContextMenu { get; set; } = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Description("The currently selected filter value. Returns null for 'All' selection.")]
        public object? SelectedFilterValue => _selectedFilterValue;

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
        public event EventHandler? ClearSearchClicked;
        public event EventHandler<FilterChangedEventArgs>? FilterChanged;

        // Standard bulk action events (used by AddStandardStatusMenuItems)
        public event EventHandler? ActivateSelected;
        public event EventHandler? DeactivateSelected;
        public event EventHandler? ExportToExcelSelected;
        public event EventHandler? DeleteSelected;
        #endregion

        public ReusableDataGrid()
        {
            InitializeComponent();
            // Don't create default tabs/search - let calling code configure via SetTabs/SetSearchFields
            CreateEmptyStatePanel();
            CreateContextMenu();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        /// <summary>
        /// Call after configuration is complete to finalize grid setup.
        /// Adds checkbox column if ShowCheckboxColumn is true.
        /// </summary>
        public void FinalizeSetup()
        {
            if (ShowCheckboxColumn && !_checkboxColumnAdded)
            {
                _gridView.AddCheckboxColumn();
                _checkboxColumnAdded = true;
            }
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
            ClearAll();
            _currentTab = _tabDefinitions.Count > 0 ? _tabDefinitions[0].Tag : "All";
            UpdateTabSelection(_currentTab);
            ClearFiltersClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Tab Buttons
        private void RecreateTabButtons()
        {
            _tabPanel.Controls.Clear();

            if (_tabDefinitions.Count == 0)
            {
                _tabPanel.Visible = false;
                return;
            }

            _tabPanel.Visible = true;
            var tabFont = new Font("Segoe UI Semibold", 9.5F);

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0)
            };

            foreach (var tabDef in _tabDefinitions)
            {
                var btn = CreateTabButton(tabDef.Text, tabDef.Tag, tabFont);
                flowPanel.Controls.Add(btn);
            }

            _tabPanel.Controls.Add(flowPanel);

            // Set initial selection
            if (_tabDefinitions.Count > 0)
            {
                _currentTab = _tabDefinitions[0].Tag;
                UpdateTabSelection(_currentTab);
            }
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

            if (_tabPanel.Controls.Count > 0 && _tabPanel.Controls[0] is FlowLayoutPanel flowPanel)
            {
                foreach (Control ctrl in flowPanel.Controls)
                {
                    if (ctrl is Button btn)
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
            }
        }
        #endregion

        #region Search Controls
        private void RecreateSearchControls()
        {
            _searchPanel.Controls.Clear();

            if (_searchFieldDefinitions.Count == 0)
            {
                _searchPanel.Visible = false;
                return;
            }

            _searchPanel.Visible = true;

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
            
            foreach (var fieldDef in _searchFieldDefinitions)
            {
                _cmbSearchField.Items.Add(new ComboBoxItem(fieldDef.DisplayName, fieldDef.FieldName));
            }
            
            _cmbSearchField.DisplayMember = "Text";
            _cmbSearchField.ValueMember = "Value";
            _cmbSearchField.SelectedIndex = 0;
            _cmbSearchField.SelectedIndexChanged += (s, e) =>
            {
                if (_cmbSearchField.SelectedItem is ComboBoxItem item)
                    _searchField = item.Value;
            };

            // Set initial search field
            if (_searchFieldDefinitions.Count > 0)
                _searchField = _searchFieldDefinitions[0].FieldName;

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
            _btnClearSearch.Click += (s, e) => ClearSearchClicked?.Invoke(this, EventArgs.Empty);

            flowPanel.Controls.Add(_cmbSearchField);
            flowPanel.Controls.Add(_txtSearch);
            flowPanel.Controls.Add(_btnSearch);
            flowPanel.Controls.Add(_btnClearSearch);

            // Add filter controls if filter is defined
            if (_currentFilter != null)
            {
                CreateFilterControls(flowPanel);
            }

            _searchPanel.Controls.Add(flowPanel);
            
            // Apply theme to new controls
            ApplyThemeToSearchControls();
        }

        private void ApplyThemeToSearchControls()
        {
            var colors = ThemeManager.Colors;
            
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

            // Filter controls theme
            if (_filterLabel != null)
            {
                _filterLabel.ForeColor = colors.SecondaryText;
            }

            if (_filterCombo != null)
            {
                _filterCombo.BackColor = colors.ContentBackground;
                _filterCombo.ForeColor = colors.PrimaryText;
            }
        }

        private void CreateFilterControls(FlowLayoutPanel flowPanel)
        {
            if (_currentFilter == null) return;

            // Add spacing/separator
            var spacer = new Panel
            {
                Width = 30,
                Height = 32,
                Margin = new Padding(0)
            };
            flowPanel.Controls.Add(spacer);

            // Filter Label - shown as label when single filter (not as combo)
            _filterLabel = new Label
            {
                Text = _currentFilter.DisplayName + ":",
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 8, 8, 0)
            };
            flowPanel.Controls.Add(_filterLabel);

            // Filter ComboBox
            _filterCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Width = 160,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 0, 0)
            };

            // Load filter data
            LoadFilterData();

            _filterCombo.SelectedIndexChanged += (s, e) =>
            {
                if (_filterCombo.SelectedItem is FilterComboItem item)
                {
                    _selectedFilterValue = item.IsAllItem ? null : item.Value;
                    FilterChanged?.Invoke(this, new FilterChangedEventArgs(
                        _currentFilter.FilterKey, 
                        _selectedFilterValue
                    ));
                }
            };

            flowPanel.Controls.Add(_filterCombo);

            // Apply theme
            var colors = ThemeManager.Colors;
            _filterLabel.ForeColor = colors.SecondaryText;
            _filterCombo.BackColor = colors.ContentBackground;
            _filterCombo.ForeColor = colors.PrimaryText;
        }

        private void LoadFilterData()
        {
            if (_filterCombo == null || _currentFilter == null) return;

            _filterCombo.Items.Clear();

            // Add "All" item first
            _filterCombo.Items.Add(new FilterComboItem(
                _currentFilter.AllItemsText, 
                null, 
                isAllItem: true
            ));

            // Get data from DataTable or Func
            DataTable? dataTable = null;
            if (_currentFilter.DataSource is DataTable dt)
            {
                dataTable = dt;
            }
            else if (_currentFilter.DataSource is Func<DataTable> func)
            {
                try
                {
                    dataTable = func();
                }
                catch
                {
                    // Silent fail - just show "All" option
                }
            }

            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var value = row[_currentFilter.ValueColumn];
                    var display = row[_currentFilter.DisplayColumn]?.ToString() ?? "";
                    _filterCombo.Items.Add(new FilterComboItem(display, value, isAllItem: false));
                }
            }

            _filterCombo.SelectedIndex = 0;
            _selectedFilterValue = null;
        }

        private void PerformSearch()
        {
            if (_txtSearch != null)
            {
                _searchText = _txtSearch.Text.Trim();
                _gridView.ResetToFirstPage();
                SearchRequested?.Invoke(this, new SearchEventArgs(_searchText, _searchField));
            }
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

            // Opening event - check if rows are selected (only if context menu is enabled)
            _contextMenu.Opening += (s, e) =>
            {
                if (!ShowContextMenu || _contextMenu.Items.Count == 0)
                {
                    e.Cancel = true;
                    return;
                }
                
                var checkedRows = _gridView.GetCheckedRows();
                if (checkedRows.Count == 0)
                {
                    e.Cancel = true;
                }
            };

            _gridView.DataGridView.ContextMenuStrip = _contextMenu;
        }

        /// <summary>
        /// Clears all context menu items to add custom ones.
        /// </summary>
        public void ClearContextMenu()
        {
            _contextMenu.Items.Clear();
        }

        /// <summary>
        /// Adds a custom context menu item.
        /// </summary>
        /// <param name="text">Menu item text</param>
        /// <param name="onClick">Click event handler</param>
        /// <param name="isDelete">If true, shows in red color for delete actions</param>
        public void AddContextMenuItem(string text, EventHandler onClick, bool isDelete = false)
        {
            var item = new ToolStripMenuItem(text, null, onClick);
            if (isDelete)
                item.ForeColor = Color.FromArgb(239, 68, 68);
            _contextMenu.Items.Add(item);
        }

        /// <summary>
        /// Adds a separator to the context menu.
        /// </summary>
        public void AddContextMenuSeparator()
        {
            _contextMenu.Items.Add(new ToolStripSeparator());
        }

        /// <summary>
        /// Adds standard status menu items (Change Status submenu with Activate/Deactivate, and Delete).
        /// </summary>
        /// <param name="hasActivate">Include Activate option</param>
        /// <param name="hasDeactivate">Include Deactivate option</param>
        /// <param name="hasDelete">Include Delete option (shown in red)</param>
        /// <param name="hasExport">Include Export to Excel option</param>
        public void AddStandardStatusMenuItems(bool hasActivate = true, bool hasDeactivate = true, bool hasDelete = true, bool hasExport = false)
        {
            if (hasActivate || hasDeactivate)
            {
                var changeStatusItem = new ToolStripMenuItem("Change Status");
                
                if (hasActivate)
                {
                    var activateItem = new ToolStripMenuItem("Activate", null, (s, e) => ActivateSelected?.Invoke(this, EventArgs.Empty));
                    changeStatusItem.DropDownItems.Add(activateItem);
                }
                
                if (hasDeactivate)
                {
                    var deactivateItem = new ToolStripMenuItem("Deactivate", null, (s, e) => DeactivateSelected?.Invoke(this, EventArgs.Empty));
                    changeStatusItem.DropDownItems.Add(deactivateItem);
                }
                
                _contextMenu.Items.Add(changeStatusItem);
            }

            if (hasExport)
            {
                if (_contextMenu.Items.Count > 0)
                    _contextMenu.Items.Add(new ToolStripSeparator());
                    
                var exportItem = new ToolStripMenuItem("Export to Excel", null, (s, e) => ExportToExcelSelected?.Invoke(this, EventArgs.Empty));
                _contextMenu.Items.Add(exportItem);
            }

            if (hasDelete)
            {
                if (_contextMenu.Items.Count > 0)
                    _contextMenu.Items.Add(new ToolStripSeparator());
                    
                var deleteItem = new ToolStripMenuItem("Delete Selected", null, (s, e) => DeleteSelected?.Invoke(this, EventArgs.Empty));
                deleteItem.ForeColor = Color.FromArgb(239, 68, 68);
                _contextMenu.Items.Add(deleteItem);
            }
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
        /// Makes the last visible column fill the remaining space
        /// </summary>
        public void FillLastColumn()
        {
            _gridView.FillLastColumn();
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
        /// Configures the tabs for filtering data. Call this method before loading data.
        /// </summary>
        /// <example>
        /// _dataGrid.SetTabs(
        ///     new TabDefinition("All", "All"),
        ///     new TabDefinition("Active", "Active"),
        ///     new TabDefinition("Inactive", "Inactive")
        /// );
        /// </example>
        public void SetTabs(params TabDefinition[] tabs)
        {
            _tabDefinitions.Clear();
            _tabDefinitions.AddRange(tabs);
            RecreateTabButtons();
        }

        /// <summary>
        /// Configures the search fields for the search combo box. Call this method before loading data.
        /// </summary>
        /// <example>
        /// _dataGrid.SetSearchFields(
        ///     new SearchFieldDefinition("Product Name", "ProductName"),
        ///     new SearchFieldDefinition("Product ID", "ProductID")
        /// );
        /// </example>
        public void SetSearchFields(params SearchFieldDefinition[] fields)
        {
            _searchFieldDefinitions.Clear();
            _searchFieldDefinitions.AddRange(fields);
            RecreateSearchControls();
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

        /// <summary>
        /// Clears the search text box and resets search state.
        /// Call this from your ClearSearchClicked event handler.
        /// </summary>
        public void ClearSearch()
        {
            if (_txtSearch != null)
                _txtSearch.Text = "";
            _searchText = "";
            _gridView.ResetToFirstPage();
            ResetTabToFirst();
        }

        /// <summary>
        /// Resets tab selection to first tab.
        /// </summary>
        public void ResetTabToFirst()
        {
            _currentTab = _tabDefinitions.Count > 0 ? _tabDefinitions[0].Tag : "All";
            UpdateTabSelection(_currentTab);
        }

        /// <summary>
        /// Sets the filter definition. Call this before SetSearchFields or after.
        /// </summary>
        /// <param name="filter">The filter definition, or null to remove filter</param>
        /// <example>
        /// _dataGrid.SetFilter(new FilterDefinition(
        ///     displayName: "Category",
        ///     filterKey: "CategoryID",
        ///     dataSource: () => clsCategory.GetAllCategory(),
        ///     valueColumn: "CategoryID",
        ///     displayColumn: "CategoryName",
        ///     allItemsText: "All Categories"
        /// ));
        /// </example>
        public void SetFilter(FilterDefinition? filter)
        {
            _currentFilter = filter;
            _selectedFilterValue = null;
            
            // Recreate search controls to include/exclude filter
            if (_searchFieldDefinitions.Count > 0)
            {
                RecreateSearchControls();
            }
        }

        /// <summary>
        /// Refreshes the filter data from the data source.
        /// Call this after adding/editing/deleting items that affect the filter.
        /// </summary>
        public void RefreshFilter()
        {
            LoadFilterData();
        }

        /// <summary>
        /// Resets the filter selection to "All".
        /// </summary>
        public void ResetFilter()
        {
            if (_filterCombo != null && _filterCombo.Items.Count > 0)
            {
                // Temporarily remove event handler to prevent double-firing
                // Force reset by setting to -1 first, then to 0
                _filterCombo.SelectedIndex = -1;
                _filterCombo.SelectedIndex = 0;
                _selectedFilterValue = null;
            }
        }

        /// <summary>
        /// Clears both search and filter, resetting to initial state.
        /// </summary>
        public void ClearAll()
        {
            ClearSearch();
            ResetFilter();
        }
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

        private class FilterComboItem
        {
            public string Text { get; }
            public object? Value { get; }
            public bool IsAllItem { get; }

            public FilterComboItem(string text, object? value, bool isAllItem)
            {
                Text = text;
                Value = value;
                IsAllItem = isAllItem;
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

    #region Configuration Classes
    /// <summary>
    /// Defines a tab for filtering data in ReusableDataGrid.
    /// </summary>
    public class TabDefinition
    {
        /// <summary>
        /// The display text shown on the tab button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The tag value used to identify the tab (returned in CurrentTab property and TabChanged event).
        /// </summary>
        public string Tag { get; set; }

        public TabDefinition(string text, string tag)
        {
            Text = text;
            Tag = tag;
        }
    }

    /// <summary>
    /// Defines a search field option for ReusableDataGrid search combo box.
    /// </summary>
    public class SearchFieldDefinition
    {
        /// <summary>
        /// The display name shown in the combo box.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The field name used for filtering (returned in SearchField property and SearchRequested event).
        /// </summary>
        public string FieldName { get; set; }

        public SearchFieldDefinition(string displayName, string fieldName)
        {
            DisplayName = displayName;
            FieldName = fieldName;
        }
    }

    /// <summary>
    /// Defines a filter for ReusableDataGrid with data source binding.
    /// </summary>
    public class FilterDefinition
    {
        /// <summary>
        /// The display name shown as label (e.g., "Category").
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The key used to identify the filter (e.g., "CategoryID").
        /// </summary>
        public string FilterKey { get; set; }

        /// <summary>
        /// The data source - can be DataTable or Func&lt;DataTable&gt; for lazy loading.
        /// </summary>
        public object DataSource { get; set; }

        /// <summary>
        /// The column name for the value (e.g., "CategoryID").
        /// </summary>
        public string ValueColumn { get; set; }

        /// <summary>
        /// The column name for display text (e.g., "CategoryName").
        /// </summary>
        public string DisplayColumn { get; set; }

        /// <summary>
        /// Text for "All" option (e.g., "All Categories").
        /// </summary>
        public string AllItemsText { get; set; }

        public FilterDefinition(string displayName, string filterKey, 
                               object dataSource, string valueColumn, 
                               string displayColumn, string allItemsText = "All")
        {
            DisplayName = displayName;
            FilterKey = filterKey;
            DataSource = dataSource;
            ValueColumn = valueColumn;
            DisplayColumn = displayColumn;
            AllItemsText = allItemsText;
        }
    }

    /// <summary>
    /// Event args for filter changed event.
    /// </summary>
    public class FilterChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The filter key (e.g., "CategoryID").
        /// </summary>
        public string FilterKey { get; }

        /// <summary>
        /// The selected value. Null means "All" is selected.
        /// </summary>
        public object? SelectedValue { get; }

        public FilterChangedEventArgs(string filterKey, object? selectedValue)
        {
            FilterKey = filterKey;
            SelectedValue = selectedValue;
        }
    }
    #endregion
}
