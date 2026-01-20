using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private int _hoveredRowIndex = -1;
        private Color _hoverColor;
        private Color _normalColor;
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
        #endregion

        #region Events
        public event EventHandler<PageChangedEventArgs>? PageChanged;
        public event EventHandler<DataGridViewCellEventArgs>? CellDoubleClicked;
        public event EventHandler? SelectionChanged;
        public event EventHandler<int>? PageSizeChanged;
        #endregion

        public ModernDataGridView()
        {
            InitializeComponent();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
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
            if (e.RowIndex < 0) return;

            // Paint status tag for "IsActive" column
            var column = _dataGridView.Columns[e.ColumnIndex];
            if (column.Name == "IsActive" || column.DataPropertyName == "IsActive")
            {
                e.Handled = true;
                PaintStatusTag(e);
            }
        }

        private void PaintStatusTag(DataGridViewCellPaintingEventArgs e)
        {
            e.PaintBackground(e.CellBounds, true);

            if (e.Value == null) return;

            bool isActive = false;
            if (e.Value is bool b)
                isActive = b;
            else if (bool.TryParse(e.Value.ToString(), out bool parsed))
                isActive = parsed;

            string text = isActive ? "نشط" : "غير نشط";
            Color bgColor = isActive ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68);
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
                using (var brush = new SolidBrush(bgColor))
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

        #region Hover Effect
        private void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex != _hoveredRowIndex)
            {
                // Reset previous hovered row
                if (_hoveredRowIndex >= 0 && _hoveredRowIndex < _dataGridView.Rows.Count)
                {
                    _dataGridView.Rows[_hoveredRowIndex].DefaultCellStyle.BackColor = _normalColor;
                }

                _hoveredRowIndex = e.RowIndex;
                _dataGridView.Rows[_hoveredRowIndex].DefaultCellStyle.BackColor = _hoverColor;
            }
        }

        private void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (_hoveredRowIndex >= 0 && _hoveredRowIndex < _dataGridView.Rows.Count)
            {
                _dataGridView.Rows[_hoveredRowIndex].DefaultCellStyle.BackColor = _normalColor;
            }
            _hoveredRowIndex = -1;
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
        /// Gets all rows with checkbox checked
        /// </summary>
        public List<DataGridViewRow> GetCheckedRows()
        {
            var checkedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in _dataGridView.Rows)
            {
                if (row.Cells["SelectCheckbox"]?.Value is true)
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
        /// Sets the data source and optionally the total records count
        /// </summary>
        public void SetDataSource(object? dataSource, int? totalRecords = null)
        {
            _dataGridView.DataSource = dataSource;

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
            _normalColor = colors.ContentBackground;
            _hoverColor = ThemeManager.CurrentTheme == ThemeMode.Light
                ? Color.FromArgb(248, 250, 252)
                : Color.FromArgb(45, 55, 72);

            if (_dataGridView != null)
            {
                _dataGridView.BackgroundColor = colors.ContentBackground;
                _dataGridView.GridColor = colors.BorderColor;

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

            Invalidate(true);
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
    #endregion
}
