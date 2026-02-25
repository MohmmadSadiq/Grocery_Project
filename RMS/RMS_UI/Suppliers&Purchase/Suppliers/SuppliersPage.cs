using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Suppliers_Purchase;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class SuppliersPage : UserControl
    {
        // ── Constructor ───────────────────────────────────────────────────────────
        public SuppliersPage()
        {
            InitializeComponent();
            _ConfigureDataGrid();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _LoadSuppliers();
        }

        // ── Grid Setup ────────────────────────────────────────────────────────────

        private void _ConfigureDataGrid()
        {
            // Tabs
            _dataGrid.SetTabs(
                new TabDefinition("All",      "All"),
                new TabDefinition("Person",   "Person"),
                new TabDefinition("Company",  "Company"),
                new TabDefinition("Active",   "Active"),
                new TabDefinition("Inactive", "Inactive")
            );

            // Search fields
            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("Supplier Name", "SupplierName"),
                new SearchFieldDefinition("Phone",         "Phone"),
                new SearchFieldDefinition("ID / Code",     "Code")
            );

            // Context menu
            _dataGrid.AddSelectAllMenuItem();
            _dataGrid.AddStandardStatusMenuItems(
                hasActivate:   true,
                hasDeactivate: true,
                hasDelete:     true,
                hasExport:     true);

            // Events
            _dataGrid.TabChanged          += (s, e) => _LoadSuppliers();
            _dataGrid.SearchRequested     += (s, e) => _LoadSuppliers();
            _dataGrid.PageChanged         += (s, e) => _LoadSuppliers();
            _dataGrid.CellDoubleClicked   += _DataGrid_CellDoubleClicked;
            _dataGrid.ClearSearchClicked  += (s, e) => { _dataGrid.ClearSearch(); _LoadSuppliers(); };
            _dataGrid.ClearFiltersClicked += (s, e) => { _dataGrid.ClearAll();    _LoadSuppliers(); };
            _dataGrid.ActivateSelected    += (s, e) => _BulkUpdateStatus(true);
            _dataGrid.DeactivateSelected  += (s, e) => _BulkUpdateStatus(false);
            _dataGrid.DeleteSelected      += (s, e) => _BulkDelete();
            _dataGrid.ExportToExcelSelected += (s, e) => _notification.ShowInfo("Export to Excel coming soon.");

            // New Supplier button
            _btnNewSupplier.Click += (s, e) => _OpenSupplierDialog(-1);

            _dataGrid.FinalizeSetup();
        }

        // ── Data Loading ──────────────────────────────────────────────────────────

        private void _LoadSuppliers()
        {
            var criteria = new clsSupplier.SupplierSearchCriteria
            {
                SearchText   = _dataGrid.SearchText,
                SearchBy     = _dataGrid.SearchField ?? "SupplierName",
                SupplierType = _dataGrid.CurrentTab switch
                {
                    "Person"  => "Person",
                    "Company" => "Company",
                    _         => null
                },
                IsActive = _dataGrid.CurrentTab switch
                {
                    "Active"   => true,
                    "Inactive" => false,
                    _          => null
                },
                PageNumber = _dataGrid.CurrentPage,
                PageSize   = _dataGrid.PageSize,
                SortBy     = "SupplierName"
            };

            try
            {
                DataTable dt = clsSupplier.SearchSupplierPages(criteria);

                int totalCount = (dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount"))
                    ? Convert.ToInt32(dt.Rows[0]["TotalCount"])
                    : 0;

                _dataGrid.SetDataSource(dt, totalCount);
                _ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to load suppliers: {ex.Message}");
            }
        }

        private void _ConfigureGridColumns()
        {
            _dataGrid.ConfigureColumn("SupplierID",   "ID",            60,  true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("SupplierType", "Type",          90,  true,
                DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("SupplierName", "Supplier Name", 200, true);
            _dataGrid.ConfigureColumn("Name",         "Full Name",     160, true);
            _dataGrid.ConfigureColumn("Phone",        "Phone",         130, true);
            _dataGrid.ConfigureColumn("Email",        "Email",         180, true);
            _dataGrid.ConfigureColumn("Country",      "Country",       110, true);
            _dataGrid.ConfigureColumn("IsActive",     "Status",         80, true,
                DataGridViewContentAlignment.MiddleCenter);

            // Hidden columns
            _dataGrid.ConfigureColumn("Address",     "",  0, false);
            _dataGrid.ConfigureColumn("CreatedDate", "",  0, false);
            _dataGrid.ConfigureColumn("UpdatedDate", "",  0, false);
            _dataGrid.ConfigureColumn("TotalCount",  "",  0, false);

            _dataGrid.FillLastColumn();
        }

        // ── Grid Events ───────────────────────────────────────────────────────────

        private void _DataGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var idVal = _dataGrid.DataGridView.Rows[e.RowIndex].Cells["SupplierID"]?.Value;
            if (idVal != null && idVal != DBNull.Value)
                _OpenSupplierDialog(Convert.ToInt32(idVal));
        }

        // ── Dialog Operations ─────────────────────────────────────────────────────

        private void _OpenSupplierDialog(int supplierID)
        {
            try
            {
                using var frm = supplierID == -1
                    ? new frmAddEditSupplier()
                    : new frmAddEditSupplier(supplierID);

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    _LoadSuppliers();
                    _notification.ShowSuccess(supplierID == -1
                        ? "Supplier added successfully."
                        : "Supplier updated successfully.");
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error opening supplier form: {ex.Message}");
            }
        }

        // ── Bulk Operations ───────────────────────────────────────────────────────

        private void _BulkUpdateStatus(bool isActive)
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one supplier.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to {(isActive ? "activate" : "deactivate")} {rows.Count} supplier(s)?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                int ok = 0;
                foreach (var row in rows)
                {
                    if (row.Cells["SupplierID"]?.Value is int id)
                    {
                        var supplier = clsSupplier.Find(id);
                        if (supplier != null)
                        {
                            supplier.IsActive = isActive;
                            if (supplier.Save()) ok++;
                        }
                    }
                }
                _notification.ShowSuccess($"{ok} supplier(s) {(isActive ? "activated" : "deactivated")}.");
                _LoadSuppliers();
            }
        }

        private void _BulkDelete()
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one supplier.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete {rows.Count} supplier(s)?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm == DialogResult.Yes)
            {
                int ok = 0;
                foreach (var row in rows)
                {
                    if (row.Cells["SupplierID"]?.Value is int id)
                        if (clsSupplier.DeleteSupplier(id, clsGlobalUser.CurrentUser?.UserID))
                            ok++;
                }
                _notification.ShowSuccess($"{ok} supplier(s) deleted.");
                _LoadSuppliers();
            }
        }

        // ── Theme ─────────────────────────────────────────────────────────────────

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;

            _headerPanel.BackColor = c.ContentBackground;
            _lblTitle.ForeColor    = c.TitleText;
            _lblSubtitle.ForeColor = c.SecondaryText;

            _btnNewSupplier.BackColor = c.Primary;
            _btnNewSupplier.ForeColor = Color.White;
            _btnNewSupplier.FlatAppearance.MouseOverBackColor = c.PrimaryHover;
            _btnNewSupplier.FlatAppearance.MouseDownBackColor = c.PrimaryHover;

            _dataGrid.ApplyTheme();
        }
    }
}

