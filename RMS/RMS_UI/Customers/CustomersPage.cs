using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class CustomersPage : UserControl
    {
        public CustomersPage()
        {
            InitializeComponent();
            ConfigureDataGrid();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadCustomers();
        }

        private void ConfigureDataGrid()
        {
            _dataGrid.SetTabs(
                new TabDefinition("All", "All"),
                new TabDefinition("Person", "Person"),
                new TabDefinition("Company", "Company"),
                new TabDefinition("Active", "Active"),
                new TabDefinition("Inactive", "Inactive")
            );

            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("Customer Name", "CustomerName"),
                new SearchFieldDefinition("Phone", "Phone"),
                new SearchFieldDefinition("ID / Code", "Code")
            );

            _dataGrid.AddSelectAllMenuItem();
            _dataGrid.AddStandardStatusMenuItems(
                hasActivate: true,
                hasDeactivate: true,
                hasDelete: true,
                hasExport: true);

            _dataGrid.TabChanged += (s, e) => LoadCustomers();
            _dataGrid.SearchRequested += (s, e) => LoadCustomers();
            _dataGrid.PageChanged += (s, e) => LoadCustomers();
            _dataGrid.CellDoubleClicked += DataGrid_CellDoubleClicked;
            _dataGrid.ClearSearchClicked += (s, e) =>
            {
                _dataGrid.ClearSearch();
                LoadCustomers();
            };
            _dataGrid.ClearFiltersClicked += (s, e) =>
            {
                _dataGrid.ClearAll();
                LoadCustomers();
            };
            _dataGrid.ActivateSelected += (s, e) => BulkUpdateStatus(true);
            _dataGrid.DeactivateSelected += (s, e) => BulkUpdateStatus(false);
            _dataGrid.DeleteSelected += (s, e) => BulkDeleteCustomers();
            _dataGrid.ExportToExcelSelected += (s, e) => _notification.ShowInfo("Export to Excel coming soon.");

            _btnNewCustomer.Click += (s, e) => OpenCustomerDialog(-1);

            _dataGrid.FinalizeSetup();
        }

        private void LoadCustomers()
        {
            var criteria = new clsCustomer.CustomerSearchCriteria
            {
                SearchText = _dataGrid.SearchText,
                SearchBy = _dataGrid.SearchField ?? "CustomerName",
                CustomerType = _dataGrid.CurrentTab switch
                {
                    "Person" => "Person",
                    "Company" => "Company",
                    _ => null
                },
                IsActive = _dataGrid.CurrentTab switch
                {
                    "Active" => true,
                    "Inactive" => false,
                    _ => null
                },
                PageNumber = _dataGrid.CurrentPage,
                PageSize = _dataGrid.PageSize,
                SortBy = "CustomerName"
            };

            try
            {
                DataTable dt = clsCustomer.SearchCustomerPages(criteria);

                int totalCount = (dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount"))
                    ? Convert.ToInt32(dt.Rows[0]["TotalCount"])
                    : 0;

                _dataGrid.SetDataSource(dt, totalCount);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to load customers: {ex.Message}");
            }
        }

        private void ConfigureGridColumns()
        {
            _dataGrid.ConfigureColumn("CustomerID", "ID", 60, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("CustomerType", "Type", 90, true,
                DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("CustomerName", "Customer Name", 220, true);
            _dataGrid.ConfigureColumn("Phone", "Phone", 130, true);
            _dataGrid.ConfigureColumn("Email", "Email", 180, true);
            _dataGrid.ConfigureColumn("Country", "Country", 110, true);
            _dataGrid.ConfigureColumn("IsActive", "Status", 80, true,
                DataGridViewContentAlignment.MiddleCenter);

            _dataGrid.ConfigureColumn("Address", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("CompanyID", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("PersonID", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("TotalCount", string.Empty, 0, false);

            _dataGrid.FillLastColumn();
        }

        private void DataGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            object? idValue = _dataGrid.DataGridView.Rows[e.RowIndex].Cells["CustomerID"]?.Value;
            if (idValue != null && idValue != DBNull.Value)
            {
                OpenCustomerDialog(Convert.ToInt32(idValue));
            }
        }

        private void OpenCustomerDialog(int customerID)
        {
            try
            {
                using var frm = customerID <= 0
                    ? new frmAddEditCustomer()
                    : new frmAddEditCustomer(customerID);

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCustomers();
                    _notification.ShowSuccess(customerID <= 0
                        ? "Customer added successfully."
                        : "Customer updated successfully.");
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error opening customer form: {ex.Message}");
            }
        }

        private void BulkUpdateStatus(bool isActive)
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one customer.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to {(isActive ? "activate" : "deactivate")} {rows.Count} customer(s)?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int updated = 0;
            foreach (DataGridViewRow row in rows)
            {
                if (row.Cells["CustomerID"]?.Value is int id)
                {
                    clsCustomer? customer = clsCustomer.Find(id);
                    if (customer != null)
                    {
                        customer.IsActive = isActive;
                        customer.UpdatedByUserID = clsGlobalUser.CurrentUser?.UserID;
                        if (customer.Save())
                        {
                            updated++;
                        }
                    }
                }
            }

            _notification.ShowSuccess($"{updated} customer(s) {(isActive ? "activated" : "deactivated")}. ");
            LoadCustomers();
        }

        private void BulkDeleteCustomers()
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one customer.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Delete {rows.Count} customer(s)?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int deleted = 0;
            foreach (DataGridViewRow row in rows)
            {
                if (row.Cells["CustomerID"]?.Value is int id)
                {
                    if (clsCustomer.DeleteCustomer(id, clsGlobalUser.CurrentUser?.UserID))
                    {
                        deleted++;
                    }
                }
            }

            _notification.ShowSuccess($"{deleted} customer(s) deleted.");
            LoadCustomers();
        }

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;

            _headerPanel.BackColor = c.ContentBackground;
            _lblTitle.ForeColor = c.TitleText;
            _lblSubtitle.ForeColor = c.SecondaryText;

            _btnNewCustomer.BackColor = c.Primary;
            _btnNewCustomer.ForeColor = Color.White;
            _btnNewCustomer.FlatAppearance.MouseOverBackColor = c.PrimaryHover;
            _btnNewCustomer.FlatAppearance.MouseDownBackColor = c.PrimaryHover;

            _dataGrid.ApplyTheme();
        }
    }
}
