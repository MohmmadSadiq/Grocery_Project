using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class EmployeePage : UserControl
    {
        private bool _isLoadingFilters;

        private static readonly Dictionary<string, string> _searchFieldMap = new()
        {
            ["FullName"] = "FullName",
            ["EmployeeID"] = "EmployeeID"
        };

        public EmployeePage()
        {
            InitializeComponent();
            PositionHeaderButtons();
            _headerPanel.Resize += (s, e) => PositionHeaderButtons();
            ConfigureDataGrid();
            ConfigureFilters();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        private void PositionHeaderButtons()
        {
            _btnSettings.Location = new Point(_headerPanel.Width - _btnSettings.Width - 30, 26);
            _btnNewEmployee.Location = new Point(_btnSettings.Left - _btnNewEmployee.Width - 10, 26);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshEmployees();
        }

        private void ConfigureDataGrid()
        {
            _dataGrid.SetTabs(new TabDefinition("All", "All"));

            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("Employee Name", "FullName"),
                new SearchFieldDefinition("Employee ID", "EmployeeID")
            );

            _dataGrid.AddSelectAllMenuItem();
            _dataGrid.AddContextMenuItem("Export to Excel", (s, e) => _notification.ShowInfo("Export to Excel coming soon."));

            _dataGrid.SearchRequested += (s, e) => RefreshEmployees();
            _dataGrid.PageChanged += (s, e) => RefreshEmployees();
            _dataGrid.CellDoubleClicked += DataGrid_CellDoubleClicked;
            _dataGrid.ClearSearchClicked += (s, e) =>
            {
                _dataGrid.ClearSearch();
                RefreshEmployees();
            };
            _dataGrid.ClearFiltersClicked += (s, e) =>
            {
                _dataGrid.ClearAll();
                ResetFilterCombos();
                RefreshEmployees();
            };

            _btnNewEmployee.Click += BtnNewEmployee_Click;
            _btnSettings.Click += BtnSettings_Click;

            _dataGrid.FinalizeSetup();
        }

        private void ConfigureFilters()
        {
            _isLoadingFilters = true;
            try
            {
                PopulatePositionFilter();
                PopulateCountryFilter();
                ResetFilterCombos();
            }
            finally
            {
                _isLoadingFilters = false;
            }

            _cmbPositionFilter.SelectedIndexChanged += Filters_SelectedIndexChanged;
            _cmbCountryFilter.SelectedIndexChanged += Filters_SelectedIndexChanged;
        }

        private void PopulatePositionFilter()
        {
            _cmbPositionFilter.Items.Clear();
            _cmbPositionFilter.Items.Add(new ComboBoxItem("-- All Positions --", -1));

            DataTable positions = clsPosition.GetAllPosition();
            foreach (DataRow row in positions.Rows)
            {
                int id = Convert.ToInt32(row["PositionID"]);
                string name = row["PositionName"]?.ToString() ?? $"Position #{id}";
                _cmbPositionFilter.Items.Add(new ComboBoxItem(name, id));
            }

            _cmbPositionFilter.DisplayMember = nameof(ComboBoxItem.Text);
        }

        private void PopulateCountryFilter()
        {
            _cmbCountryFilter.Items.Clear();
            _cmbCountryFilter.Items.Add(new ComboBoxItem("-- All Countries --", -1));

            DataTable countries = clsCountry.GetAllCountries();
            foreach (DataRow row in countries.Rows)
            {
                int id = row.Table.Columns.Contains("CountryID")
                    ? Convert.ToInt32(row["CountryID"])
                    : Convert.ToInt32(row["ID"]);

                string name = row["CountryName"]?.ToString() ?? $"Country #{id}";
                _cmbCountryFilter.Items.Add(new ComboBoxItem(name, id));
            }

            _cmbCountryFilter.DisplayMember = nameof(ComboBoxItem.Text);
        }

        private void ResetFilterCombos()
        {
            if (_cmbPositionFilter.Items.Count > 0)
                _cmbPositionFilter.SelectedIndex = 0;

            if (_cmbCountryFilter.Items.Count > 0)
                _cmbCountryFilter.SelectedIndex = 0;
        }

        private void Filters_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isLoadingFilters)
                return;

            _dataGrid.ResetToFirstPage();
            RefreshEmployees();
        }

        private void RefreshEmployees()
        {
            try
            {
                clsEmployee.EmployeeSearchCriteria criteria = BuildSearchCriteria();
                int totalCount = 0;
                DataTable employees = clsEmployee.SearchEmployeesPages(criteria, ref totalCount);

                _dataGrid.SetDataSource(employees, totalCount);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to load employees: {ex.Message}");
            }
        }

        private clsEmployee.EmployeeSearchCriteria BuildSearchCriteria()
        {
            string selectedSearchField = _dataGrid.SearchField;
            string searchBy = _searchFieldMap.TryGetValue(selectedSearchField, out string? mapped)
                ? mapped
                : "FullName";

            return new clsEmployee.EmployeeSearchCriteria
            {
                SearchText = _dataGrid.SearchText,
                SearchBy = searchBy,
                PositionID = GetSelectedFilterId(_cmbPositionFilter),
                CountryID = GetSelectedFilterId(_cmbCountryFilter),
                PageNumber = _dataGrid.CurrentPage,
                PageSize = _dataGrid.PageSize
            };
        }

        private static int? GetSelectedFilterId(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is not ComboBoxItem item)
                return null;

            return item.Value > 0 ? item.Value : null;
        }

        private void ConfigureGridColumns()
        {
            _dataGrid.ConfigureColumn("EmployeeID", "ID", 70, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("FullName", "Employee Name", 220);
            _dataGrid.ConfigureColumn("PositionName", "Position", 160);
            _dataGrid.ConfigureColumn("CountryName", "Country", 140);
            _dataGrid.ConfigureColumn("HireDate", "Hire Date", 120,
                true, DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("FireDate", "Fire Date", 120,
                true, DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("Gender", "Gender", 90,
                true, DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("Phone", "Phone", 130);
            _dataGrid.ConfigureColumn("Email", "Email", 220);

            _dataGrid.ConfigureColumn("ImagePath", string.Empty, 0, false);
            _dataGrid.FillLastColumn();

            foreach (DataGridViewColumn col in _dataGrid.DataGridView.Columns)
            {
                if (col.Name != "SelectCheckbox")
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private void BtnNewEmployee_Click(object? sender, EventArgs e)
        {
            OpenEmployeeDialog(-1);
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var dialog = new EmployeeSettingsDialog())
                {
                    dialog.ShowDialog(this);
                }

                RefreshPositionFilterPreserveSelection();
                RefreshEmployees();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error opening settings: {ex.Message}");
            }
        }

        private void RefreshPositionFilterPreserveSelection()
        {
            int? selectedPositionId = GetSelectedFilterId(_cmbPositionFilter);

            _isLoadingFilters = true;
            try
            {
                PopulatePositionFilter();
                SelectPositionFilterValue(selectedPositionId);
            }
            finally
            {
                _isLoadingFilters = false;
            }
        }

        private void SelectPositionFilterValue(int? positionId)
        {
            if (positionId == null)
            {
                if (_cmbPositionFilter.Items.Count > 0)
                    _cmbPositionFilter.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < _cmbPositionFilter.Items.Count; i++)
            {
                if (_cmbPositionFilter.Items[i] is ComboBoxItem item && item.Value == positionId.Value)
                {
                    _cmbPositionFilter.SelectedIndex = i;
                    return;
                }
            }

            if (_cmbPositionFilter.Items.Count > 0)
                _cmbPositionFilter.SelectedIndex = 0;
        }

        private void DataGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex >= 0)
            {
                string columnName = _dataGrid.DataGridView.Columns[e.ColumnIndex].Name;
                if (columnName == "SelectCheckbox")
                    return;
            }

            object? value = _dataGrid.DataGridView.Rows[e.RowIndex].Cells["EmployeeID"]?.Value;
            if (value == null || value == DBNull.Value)
                return;

            if (!int.TryParse(value.ToString(), out int employeeId))
                return;

            OpenEmployeeDialog(employeeId);
        }

        private void OpenEmployeeDialog(int employeeId)
        {
            try
            {
                using var dialog = employeeId > 0
                    ? new frmAddEditEmployee(employeeId)
                    : new frmAddEditEmployee();

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                _notification.ShowSuccess(employeeId > 0
                    ? "Employee updated successfully."
                    : "Employee added successfully.");
                RefreshEmployees();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to open employee form: {ex.Message}");
            }
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;

            _headerPanel.BackColor = colors.ContentBackground;
            _lblTitle.ForeColor = colors.TitleText;
            _lblSubtitle.ForeColor = colors.SecondaryText;
            _lblPositionFilter.ForeColor = colors.SecondaryText;
            _lblCountryFilter.ForeColor = colors.SecondaryText;

            _btnNewEmployee.BackColor = colors.Primary;
            _btnNewEmployee.ForeColor = Color.White;
            _btnNewEmployee.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;
            _btnNewEmployee.FlatAppearance.MouseDownBackColor = colors.PrimaryHover;

            _btnSettings.BackColor = Color.FromArgb(107, 114, 128);
            _btnSettings.ForeColor = Color.White;

            _cmbPositionFilter.BackColor = colors.ContentBackground;
            _cmbPositionFilter.ForeColor = colors.PrimaryText;
            _cmbCountryFilter.BackColor = colors.ContentBackground;
            _cmbCountryFilter.ForeColor = colors.PrimaryText;

            _dataGrid.ApplyTheme();
        }

        private sealed class ComboBoxItem
        {
            public string Text { get; }
            public int Value { get; }

            public ComboBoxItem(string text, int value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
