using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    /// <summary>
    /// Modern reusable DataGridView with pagination, bulk selection, status tags, and theme support.
    /// </summary>
    [DesignerCategory("UserControl")]
    public partial class ModernDataGridView : UserControl
    {
        #region Private Fields
        private int _currentPage = 1;
        private int _totalRecords = 0;
        private int _pageSize = 25;
        #endregion

        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value >= 1 && value <= TotalPages)
                {
                    _currentPage = value;
                    UpdatePaginationUI();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int TotalRecords
        {
            get => _totalRecords;
            set
            {
                _totalRecords = value;
                UpdatePaginationUI();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                if (_cmbPageSize != null && _cmbPageSize.Items.Contains(value))
                    _cmbPageSize.SelectedItem = value;
                UpdatePaginationUI();
            }
        }

        [Browsable(false)]
        public int TotalPages => _totalRecords > 0 ? (int)Math.Ceiling((double)_totalRecords / _pageSize) : 1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DataGridView DataGridView => _dataGridView;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public DataGridViewSelectedRowCollection SelectedRows => _dataGridView.SelectedRows;

        /// <summary>
        /// Gets or sets whether to show checkbox column for bulk selection
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowCheckboxColumn { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show pagination panel
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowPagination
        {
            get => _paginationPanel?.Visible ?? true;
            set { if (_paginationPanel != null) _paginationPanel.Visible = value; }
        }

        /// <summary>
        /// Gets or sets whether to show context menu for bulk actions.
        /// When true, right-clicking displays the context menu if rows are checked.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Set to true to enable context menu for bulk actions.")]
        public bool ShowContextMenu { get; set; } = true;

        /// <summary>
        /// Gets the context menu for bulk actions.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ContextMenuStrip ContextMenu => _contextMenu;
        #endregion

        #region Events
        public event EventHandler<PageChangedEventArgs>? PageChanged;
        public event EventHandler<DataGridViewCellEventArgs>? CellDoubleClicked;
        public event EventHandler? SelectionChanged;
        public event EventHandler<int>? PageSizeChanged;

        // Context Menu Events
        public event EventHandler? ActivateSelected;
        public event EventHandler? DeactivateSelected;
        public event EventHandler? ExportToExcelSelected;
        public event EventHandler? DeleteSelected;
        public event EventHandler<ContextMenuItemClickedEventArgs>? ContextMenuItemClicked;
        #endregion

        public ModernDataGridView()
        {
            InitializeComponent();
            CreateContextMenu();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            
            // Enable DoubleBuffered to prevent flickering during painting
            EnableDoubleBuffering(_dataGridView);
            
            // Handle DataError to prevent error dialog for image columns
            _dataGridView.DataError += DataGridView_DataError;
            
            // Commit checkbox changes immediately when clicked
            _dataGridView.CurrentCellDirtyStateChanged += DataGridView_CurrentCellDirtyStateChanged;
        }

        /// <summary>
        /// Enables DoubleBuffered property on DataGridView to prevent flickering.
        /// </summary>
        private static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true });
        }

        private void DataGridView_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            // When checkbox is clicked, commit the change immediately
            if (_dataGridView.IsCurrentCellDirty && 
                _dataGridView.CurrentCell?.OwningColumn?.Name == "SelectCheckbox")
            {
                _dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            // Suppress errors for image columns (FormatException when cell value is not an image)
            if (e.Exception is FormatException)
            {
                e.ThrowException = false;
            }
        }

        private void LayoutPaginationControls()
        {
            if (_paginationPanel == null) return;

            int centerY = (_paginationPanel.Height - 32) / 2;
            int margin = 15;

            // Left side - Page size controls
            _lblPageSizeLabel.Location = new Point(margin, (_paginationPanel.Height - _lblPageSizeLabel.Height) / 2);
            _cmbPageSize.Location = new Point(_lblPageSizeLabel.Right + 5, centerY);

            // Right side - Navigation
            _btnNext.Location = new Point(_paginationPanel.Width - _btnNext.Width - margin, centerY);
            _btnPrevious.Location = new Point(_btnNext.Left - _btnPrevious.Width - 10, centerY);

            // Center - Page info
            _lblPageInfo.Location = new Point(
                (_paginationPanel.Width - _lblPageInfo.Width) / 2,
                (_paginationPanel.Height - _lblPageInfo.Height) / 2
            );
        }

        private void UpdatePaginationUI()
        {
            if (_lblPageInfo == null) return;

            int startRecord = _totalRecords > 0 ? ((_currentPage - 1) * _pageSize) + 1 : 0;
            int endRecord = Math.Min(_currentPage * _pageSize, _totalRecords);

            _lblPageInfo.Text = $"Showing {startRecord}-{endRecord} of {_totalRecords}";

            _btnPrevious.Enabled = _currentPage > 1;
            _btnNext.Enabled = _currentPage < TotalPages;

            _btnPrevious.ForeColor = _btnPrevious.Enabled ? ThemeManager.Colors.PrimaryText : Color.FromArgb(180, 180, 180);
            _btnNext.ForeColor = _btnNext.Enabled ? ThemeManager.Colors.PrimaryText : Color.FromArgb(180, 180, 180);

            LayoutPaginationControls();
        }

        private void OnPageChanged()
        {
            UpdatePaginationUI();
            PageChanged?.Invoke(this, new PageChangedEventArgs(_currentPage, _pageSize));
        }

        #region Cell Painting
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Paint header cells with vertical separators
            if (e.RowIndex == -1)
            {
                PaintHeaderCell(e);
                return;
            }

            // Paint status tag for "IsActive" column
            var column = _dataGridView.Columns[e.ColumnIndex];
            if (column.Name == "IsActive" || column.DataPropertyName == "IsActive")
            {
                e.Handled = true;
                PaintStatusTag(e);
            }
            else
            {
                // For all other cells, paint background without selection border
                PaintCellBackground(e);
            }
        }

        private void PaintHeaderCell(DataGridViewCellPaintingEventArgs e)
        {
            // Paint background
            using (var bgBrush = new SolidBrush(e.CellStyle!.BackColor))
            {
                e.Graphics!.FillRectangle(bgBrush, e.CellBounds);
            }

            // Paint text content
            e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);

            // Draw vertical separator on the right side of each header cell
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                e.Graphics.DrawLine(pen, 
                    e.CellBounds.Right - 1, 
                    e.CellBounds.Top + 10, 
                    e.CellBounds.Right - 1, 
                    e.CellBounds.Bottom - 10);
            }

            e.Handled = true;
        }

        private void PaintCellBackground(DataGridViewCellPaintingEventArgs e)
        {
            // Use theme colors directly to ensure consistency
            Color bgColor = e.State.HasFlag(DataGridViewElementStates.Selected)
                ? ThemeManager.Colors.PrimaryLight
                : ThemeManager.Colors.ContentBackground;

            // Expand bounds by 1 pixel to cover any grid lines
            var expandedBounds = new Rectangle(
                e.CellBounds.X - 1,
                e.CellBounds.Y - 1,
                e.CellBounds.Width + 2,
                e.CellBounds.Height + 2
            );

            // Paint solid background without any borders
            using (var bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics!.FillRectangle(bgBrush, expandedBounds);
            }

            // Paint rest of cell (content only, no focus rectangle)
            e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
            e.Handled = true;
        }

        private void PaintStatusTag(DataGridViewCellPaintingEventArgs e)
        {
            // Use theme colors directly to ensure consistency
            Color cellBgColor = e.State.HasFlag(DataGridViewElementStates.Selected)
                ? ThemeManager.Colors.PrimaryLight
                : ThemeManager.Colors.ContentBackground;

            // Expand bounds by 1 pixel to cover any grid lines
            var expandedBounds = new Rectangle(
                e.CellBounds.X - 1,
                e.CellBounds.Y - 1,
                e.CellBounds.Width + 2,
                e.CellBounds.Height + 2
            );

            // Paint background without selection border to avoid lines between rows
            using (var bgBrush = new SolidBrush(cellBgColor))
            {
                e.Graphics!.FillRectangle(bgBrush, expandedBounds);
            }

            if (e.Value == null) return;

            bool isActive = false;
            if (e.Value is bool b)
                isActive = b;
            else if (bool.TryParse(e.Value.ToString(), out bool parsed))
                isActive = parsed;

            string text = isActive ? "نشط" : "غير نشط";
            Color tagBgColor = isActive ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68);
            Color textColor = Color.White;

            using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
            {
                var textSize = TextRenderer.MeasureText(text, font);
                int tagWidth = textSize.Width + 16;
                int tagHeight = 22;

                var tagRect = new Rectangle(
                    e.CellBounds.X + (e.CellBounds.Width - tagWidth) / 2,
                    e.CellBounds.Y + (e.CellBounds.Height - tagHeight) / 2,
                    tagWidth,
                    tagHeight
                );

                using (var path = CreateRoundedRectangle(tagRect, 4))
                using (var brush = new SolidBrush(tagBgColor))
                {
                    e.Graphics!.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);

                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        font,
                        tagRect,
                        textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
        #endregion

        #region Context Menu
        private ToolStripMenuItem? _selectMenuItem;

        /// <summary>
        /// Creates and initializes the context menu for bulk actions.
        /// </summary>
        private void CreateContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Font = new Font("Segoe UI", 9.5F);

            // Opening event - check if rows are selected (only if context menu is enabled)
            _contextMenu.Opening += ContextMenu_Opening;

            _dataGridView.ContextMenuStrip = _contextMenu;
        }

        private void ContextMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel if context menu is disabled
            if (!ShowContextMenu)
            {
                e.Cancel = true;
                return;
            }

            // Cancel if no menu items
            if (_contextMenu.Items.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            // Update "Select" menu item text based on drag-selected rows
            UpdateSelectMenuItemText();

            // If checkbox column exists, check if we have checked rows OR selected rows (for Select All)
            if (_dataGridView.Columns["SelectCheckbox"] != null)
            {
                var checkedRows = GetCheckedRows();
                
                // Allow menu to show if there are checked rows OR if there are rows to select
                if (checkedRows.Count == 0 && _dataGridView.Rows.Count == 0)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Updates the "Select" menu item text based on drag-selected rows.
        /// </summary>
        private void UpdateSelectMenuItemText()
        {
            if (_selectMenuItem == null) return;

            int selectedCount = _dataGridView.SelectedRows.Count;
            int checkedCount = GetCheckedRows().Count;

            if (selectedCount > 1)
            {
                _selectMenuItem.Text = $"☑ Select {selectedCount} items";
            }
            else if (checkedCount > 0)
            {
                _selectMenuItem.Text = "☐ Deselect All";
            }
            else
            {
                _selectMenuItem.Text = "☑ Select All";
            }
        }

        /// <summary>
        /// Handles the Select All / Select Items action.
        /// If rows are drag-selected, checks those rows.
        /// Otherwise, checks all rows.
        /// </summary>
        private void SelectMenuItem_Click(object? sender, EventArgs e)
        {
            if (_dataGridView.Columns["SelectCheckbox"] == null) return;

            int selectedCount = _dataGridView.SelectedRows.Count;
            int checkedCount = GetCheckedRows().Count;

            // If there are checked rows, deselect all
            if (checkedCount > 0 && selectedCount <= 1)
            {
                SelectAllCheckboxes(false);
            }
            // If multiple rows are drag-selected, check those
            else if (selectedCount > 1)
            {
                foreach (DataGridViewRow row in _dataGridView.SelectedRows)
                {
                    if (row.Cells["SelectCheckbox"] != null)
                    {
                        row.Cells["SelectCheckbox"].Value = true;
                    }
                }
            }
            // Otherwise, select all
            else
            {
                SelectAllCheckboxes(true);
            }

            // Force UI refresh to show checkbox changes immediately
            _dataGridView.RefreshEdit();
            _dataGridView.Invalidate();
        }

        /// <summary>
        /// Adds a "Select All" menu item that selects drag-selected rows or all rows.
        /// </summary>
        public void AddSelectAllMenuItem()
        {
            _selectMenuItem = new ToolStripMenuItem("☑ Select All");
            _selectMenuItem.Name = "SelectAll";
            _selectMenuItem.Click += SelectMenuItem_Click;
            
            // Insert at the beginning
            if (_contextMenu.Items.Count > 0)
            {
                _contextMenu.Items.Insert(0, _selectMenuItem);
                _contextMenu.Items.Insert(1, new ToolStripSeparator());
            }
            else
            {
                _contextMenu.Items.Add(_selectMenuItem);
            }
        }

        /// <summary>
        /// Clears all context menu items to add custom ones.
        /// </summary>
        public void ClearContextMenu()
        {
            _contextMenu.Items.Clear();
            _selectMenuItem = null;
        }

        /// <summary>
        /// Adds a custom context menu item.
        /// </summary>
        /// <param name="name">Unique name/identifier for the menu item</param>
        /// <param name="text">Display text for the menu item</param>
        /// <param name="onClick">Optional click event handler</param>
        /// <param name="isDelete">If true, shows in red color for delete actions</param>
        public void AddContextMenuItem(string name, string text, EventHandler? onClick = null, bool isDelete = false)
        {
            var item = new ToolStripMenuItem(text);
            item.Name = name;
            
            if (isDelete)
            {
                item.ForeColor = Color.FromArgb(239, 68, 68);
                item.Tag = "delete"; // For theme detection
            }

            // Wire up click event
            item.Click += (s, e) =>
            {
                // Call the provided handler if any
                onClick?.Invoke(s, e);

                // Also raise the generic event with context info
                var args = new ContextMenuItemClickedEventArgs(name, GetCheckedRowIndices());
                ContextMenuItemClicked?.Invoke(this, args);
            };

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
        /// This is a convenience method for common bulk operations.
        /// </summary>
        /// <param name="hasActivate">Include Activate option</param>
        /// <param name="hasDeactivate">Include Deactivate option</param>
        /// <param name="hasDelete">Include Delete option (shown in red)</param>
        /// <param name="hasExport">Include Export to Excel option</param>
        public void AddStandardStatusMenuItems(bool hasActivate = true, bool hasDeactivate = true,
                                               bool hasDelete = true, bool hasExport = false)
        {
            if (hasActivate || hasDeactivate)
            {
                var changeStatusItem = new ToolStripMenuItem("Change Status");
                changeStatusItem.Name = "ChangeStatus";

                if (hasActivate)
                {
                    var activateItem = new ToolStripMenuItem("Activate", null,
                        (s, e) => ActivateSelected?.Invoke(this, EventArgs.Empty));
                    activateItem.Name = "Activate";
                    changeStatusItem.DropDownItems.Add(activateItem);
                }

                if (hasDeactivate)
                {
                    var deactivateItem = new ToolStripMenuItem("Deactivate", null,
                        (s, e) => DeactivateSelected?.Invoke(this, EventArgs.Empty));
                    deactivateItem.Name = "Deactivate";
                    changeStatusItem.DropDownItems.Add(deactivateItem);
                }

                _contextMenu.Items.Add(changeStatusItem);
            }

            if (hasExport)
            {
                if (_contextMenu.Items.Count > 0)
                    _contextMenu.Items.Add(new ToolStripSeparator());

                var exportItem = new ToolStripMenuItem("Export to Excel", null,
                    (s, e) => ExportToExcelSelected?.Invoke(this, EventArgs.Empty));
                exportItem.Name = "ExportToExcel";
                _contextMenu.Items.Add(exportItem);
            }

            if (hasDelete)
            {
                if (_contextMenu.Items.Count > 0)
                    _contextMenu.Items.Add(new ToolStripSeparator());

                var deleteItem = new ToolStripMenuItem("Delete Selected", null,
                    (s, e) => DeleteSelected?.Invoke(this, EventArgs.Empty));
                deleteItem.Name = "DeleteSelected";
                deleteItem.ForeColor = Color.FromArgb(239, 68, 68);
                deleteItem.Tag = "delete"; // For theme detection
                _contextMenu.Items.Add(deleteItem);
            }
        }

        /// <summary>
        /// Gets the indices of all checked rows.
        /// </summary>
        /// <returns>List of row indices that are checked</returns>
        public List<int> GetCheckedRowIndices()
        {
            var indices = new List<int>();
            
            if (_dataGridView.Columns["SelectCheckbox"] == null)
                return indices;

            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                if (row.Cells["SelectCheckbox"].Value is true)
                {
                    indices.Add(row.Index);
                }
            }

            return indices;
        }
        #endregion



        #region Public Methods
        /// <summary>
        /// Adds a checkbox column at the beginning for bulk selection
        /// </summary>
        public void AddCheckboxColumn()
        {
            if (_dataGridView.Columns["SelectCheckbox"] != null) return;

            var checkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "SelectCheckbox",
                HeaderText = "",
                Width = 40,
                ReadOnly = false,
                Frozen = true,
                DisplayIndex = 0,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            _dataGridView.Columns.Insert(0, checkColumn);
        }

        /// <summary>
        /// Adds an image column for displaying product thumbnails
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="headerText">Header text to display</param>
        /// <param name="width">Column width (default 60)</param>
        /// <param name="insertIndex">Index to insert at. Use -1 to add at the end.</param>
        public void AddImageColumn(string columnName = "ProductImage", string headerText = "", 
            int width = 60, int insertIndex = -1)
        {
            var column = _dataGridView.Columns[columnName];
            if (column != null) 
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                column.Width = width;
                return;
            }        

            var imageColumn = new DataGridViewImageColumn
            {
                Name = columnName,
                HeaderText = headerText,
                Width = width,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ValuesAreIcons = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(4),
                    NullValue = null // Prevent default "X" icon for null values
                }
            };

            // Add at the end if insertIndex is -1 or greater than column count
            if (insertIndex < 0 || insertIndex >= _dataGridView.Columns.Count)
            {
                _dataGridView.Columns.Add(imageColumn);
            }
            else
            {
                _dataGridView.Columns.Insert(insertIndex, imageColumn);
            }
        }

        /// <summary>
        /// Loads images for a specific column from image paths
        /// </summary>
        /// <param name="imageColumnName">Name of the image column</param>
        /// <param name="pathColumnName">Name of the column containing image paths</param>
        /// <param name="loadImageFunc">Function to load image from path (should return thumbnail)</param>
        public void LoadImagesFromPaths(string imageColumnName, string pathColumnName, 
            Func<string?, Image> loadImageFunc)
        {
            if (_dataGridView.Columns[imageColumnName] == null || 
                _dataGridView.Columns[pathColumnName] == null)
                return;

            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                var pathValue = row.Cells[pathColumnName]?.Value;
                string? imagePath = pathValue?.ToString();
                
                try
                {
                    row.Cells[imageColumnName].Value = loadImageFunc(imagePath);
                }
                catch
                {
                    row.Cells[imageColumnName].Value = loadImageFunc(null);
                }
            }
        }

        /// <summary>
        /// Gets all rows with checkbox checked
        /// </summary>
        public List<DataGridViewRow> GetCheckedRows()
        {
            var checkedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                var cell = row.Cells["SelectCheckbox"];
                var cellValue = cell?.Value;
                bool isChecked = cellValue is true || 
                                 (cellValue is int intVal && intVal == 1) ||
                                 (cellValue != null && cellValue.ToString() == "True");
                if (isChecked)
                {
                    checkedRows.Add(row);
                }
            }

            return checkedRows;
        }

        /// <summary>
        /// Selects or deselects all checkboxes
        /// </summary>
        public void SelectAllCheckboxes(bool select)
        {
            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                if (row.Cells["SelectCheckbox"] != null)
                {
                    row.Cells["SelectCheckbox"].Value = select;
                }
            }

            // Force UI refresh to show checkbox changes immediately
            _dataGridView.RefreshEdit();
            _dataGridView.Invalidate();
        }

        /// <summary>
        /// Configures column with specific settings
        /// </summary>
        public void ConfigureColumn(string columnName, string headerText, int width = -1, bool visible = true,
            DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft,
            string? fontFamily = null, float fontSize = 9.5f)
        {
            if (_dataGridView.Columns[columnName] is DataGridViewColumn column)
            {
                column.HeaderText = headerText;
                column.Visible = visible;
                column.DefaultCellStyle.Alignment = alignment;

                if (width > 0)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Width = width;
                }

                if (!string.IsNullOrEmpty(fontFamily))
                {
                    column.DefaultCellStyle.Font = new Font(fontFamily, fontSize);
                }
            }
        }

        /// <summary>
        /// Makes the last visible column fill the remaining space
        /// </summary>
        public void FillLastColumn()
        {
            DataGridViewColumn? lastVisibleColumn = null;
            
            foreach (DataGridViewColumn col in _dataGridView.Columns)
            {
                if (col.Visible && col.Name != "SelectCheckbox")
                {
                    lastVisibleColumn = col;
                }
            }

            if (lastVisibleColumn != null)
            {
                lastVisibleColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        /// <summary>
        /// Sets the data source and optionally the total records count
        /// </summary>
        public void SetDataSource(object? dataSource, int? totalRecords = null)
        {
            _dataGridView.DataSource = dataSource;

            // Make all columns ReadOnly except checkbox column
            foreach (DataGridViewColumn col in _dataGridView.Columns)
            {
                col.ReadOnly = col.Name != "SelectCheckbox";
            }

            if (totalRecords.HasValue)
            {
                TotalRecords = totalRecords.Value;
            }

            UpdatePaginationUI();
        }

        /// <summary>
        /// Refreshes the current page
        /// </summary>
        public void RefreshCurrentPage()
        {
            OnPageChanged();
        }

        /// <summary>
        /// Resets to first page
        /// </summary>
        public void ResetToFirstPage()
        {
            _currentPage = 1;
            OnPageChanged();
        }
        #endregion

        #region Designer Event Handlers
        private void ModernDataGridView_Load(object sender, EventArgs e)
        {
            // Initial setup when control loads
            UpdatePaginationUI();
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CellDoubleClicked?.Invoke(this, e);
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbPageSize.SelectedItem != null)
            {
                _pageSize = (int)_cmbPageSize.SelectedItem;
                _currentPage = 1;
                PageSizeChanged?.Invoke(this, _pageSize);
                OnPageChanged();
            }
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                OnPageChanged();
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_currentPage < TotalPages)
            {
                _currentPage++;
                OnPageChanged();
            }
        }

        private void PaginationPanel_Resize(object sender, EventArgs e)
        {
            LayoutPaginationControls();
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.ContentBackground;

            if (_dataGridView != null)
            {
                _dataGridView.BackgroundColor = colors.ContentBackground;
                _dataGridView.GridColor = colors.ContentBackground; // Same as background to hide any grid lines

                _dataGridView.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CurrentTheme == ThemeMode.Light
                    ? Color.FromArgb(248, 250, 252)
                    : Color.FromArgb(31, 41, 55);
                _dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = colors.PrimaryText;
                _dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = _dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
                _dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = colors.PrimaryText;

                _dataGridView.DefaultCellStyle.BackColor = colors.ContentBackground;
                _dataGridView.DefaultCellStyle.ForeColor = colors.PrimaryText;
                _dataGridView.DefaultCellStyle.SelectionBackColor = colors.PrimaryLight;
                _dataGridView.DefaultCellStyle.SelectionForeColor = colors.Primary;

                // Update alternating rows style
                _dataGridView.AlternatingRowsDefaultCellStyle.BackColor = colors.ContentBackground;
                _dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = colors.PrimaryText;
                _dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor = colors.PrimaryLight;
                _dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = colors.Primary;

                // Update existing rows to apply new theme immediately
                foreach (DataGridViewRow row in _dataGridView.Rows)
                {
                    row.DefaultCellStyle.BackColor = colors.ContentBackground;
                    row.DefaultCellStyle.ForeColor = colors.PrimaryText;
                    row.DefaultCellStyle.SelectionBackColor = colors.PrimaryLight;
                    row.DefaultCellStyle.SelectionForeColor = colors.Primary;
                }

                // Force refresh
                _dataGridView.Refresh();
            }

            if (_paginationPanel != null)
            {
                _paginationPanel.BackColor = ThemeManager.CurrentTheme == ThemeMode.Light
                    ? Color.FromArgb(248, 250, 252)
                    : Color.FromArgb(31, 41, 55);
            }

            if (_lblPageInfo != null)
            {
                _lblPageInfo.ForeColor = colors.SecondaryText;
            }

            if (_lblPageSizeLabel != null)
            {
                _lblPageSizeLabel.ForeColor = colors.SecondaryText;
            }

            if (_cmbPageSize != null)
            {
                _cmbPageSize.BackColor = colors.ContentBackground;
                _cmbPageSize.ForeColor = colors.PrimaryText;
            }

            foreach (var btn in new[] { _btnPrevious, _btnNext })
            {
                if (btn != null)
                {
                    btn.BackColor = colors.ContentBackground;
                    btn.ForeColor = btn.Enabled ? colors.PrimaryText : Color.FromArgb(180, 180, 180);
                    btn.FlatAppearance.BorderColor = colors.BorderColor;
                    btn.FlatAppearance.MouseOverBackColor = colors.ButtonHover;
                }
            }

            // Context Menu theming
            if (_contextMenu != null)
            {
                _contextMenu.BackColor = colors.ContentBackground;
                _contextMenu.ForeColor = colors.PrimaryText;

                ApplyThemeToMenuItems(_contextMenu.Items, colors);
            }

            Invalidate(true);
        }

        /// <summary>
        /// Recursively applies theme to menu items (including submenus).
        /// </summary>
        private void ApplyThemeToMenuItems(ToolStripItemCollection items, ColorPalette colors)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    // Use tag-based detection instead of text matching
                    if (menuItem.Tag?.ToString() == "delete")
                        menuItem.ForeColor = Color.FromArgb(239, 68, 68);
                    else
                        menuItem.ForeColor = colors.PrimaryText;

                    // Recursively apply to dropdown items
                    if (menuItem.HasDropDownItems)
                    {
                        ApplyThemeToMenuItems(menuItem.DropDownItems, colors);
                    }
                }
            }
        }
        #endregion
    }

    #region Event Args
    public class PageChangedEventArgs : EventArgs
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public PageChangedEventArgs(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// EventArgs for context menu item clicks, providing the menu item name and checked row indices.
    /// </summary>
    public class ContextMenuItemClickedEventArgs : EventArgs
    {
        /// <summary>
        /// The name/identifier of the clicked menu item.
        /// </summary>
        public string MenuItemName { get; }

        /// <summary>
        /// List of row indices that are currently checked.
        /// </summary>
        public List<int> CheckedRowIndices { get; }

        public ContextMenuItemClickedEventArgs(string menuItemName, List<int> checkedRowIndices)
        {
            MenuItemName = menuItemName;
            CheckedRowIndices = checkedRowIndices;
        }
    }
    #endregion
}
