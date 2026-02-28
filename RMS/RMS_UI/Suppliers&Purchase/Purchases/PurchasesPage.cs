using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class PurchasesPage : UserControl
    {
        // ── Constructor ───────────────────────────────────────────────────────────
        public PurchasesPage()
        {
            InitializeComponent();
            _ConfigureDataGrid();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _LoadPurchases();
        }

        // ── Grid Setup ────────────────────────────────────────────────────────────

        private void _ConfigureDataGrid()
        {
            // Tabs: All / InProgress / Completed / Cancelled
            _dataGrid.SetTabs(
                new TabDefinition("All",        "All"),
                new TabDefinition("InProgress", "In Progress"),
                new TabDefinition("Completed",  "Completed"),
                new TabDefinition("Cancelled",  "Cancelled")
            );

            // Search fields
            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("Invoice Number", "InvoiceNumber"),
                new SearchFieldDefinition("Purchase ID",    "PurchaseID"),
                new SearchFieldDefinition("Supplier Name",  "SupplierName"),
                new SearchFieldDefinition("Employee Name",  "EmployeeName")
            );

            // Context menu
            _dataGrid.AddSelectAllMenuItem();
            _dataGrid.AddContextMenuItem("🔍 View / Edit", _ContextMenu_ViewEdit);
            _dataGrid.AddContextMenuSeparator();
            _dataGrid.AddStandardStatusMenuItems(
                hasActivate:   false,
                hasDeactivate: false,
                hasDelete:     true,
                hasExport:     true);

            // Events
            _dataGrid.TabChanged          += (s, e) => _LoadPurchases();
            _dataGrid.SearchRequested     += (s, e) => _LoadPurchases();
            _dataGrid.PageChanged         += (s, e) => _LoadPurchases();
            _dataGrid.CellDoubleClicked   += _DataGrid_CellDoubleClicked;
            _dataGrid.ClearSearchClicked  += (s, e) => { _dataGrid.ClearSearch(); _LoadPurchases(); };
            _dataGrid.ClearFiltersClicked += (s, e) => { _dataGrid.ClearAll();    _LoadPurchases(); };
            _dataGrid.DeleteSelected      += (s, e) => _BulkDelete();
            _dataGrid.ExportToExcelSelected += (s, e) => _notification.ShowInfo("Export to Excel coming soon.");

            // New Purchase button → navigate to the "New Purchase" tab in parent
            _btnNewPurchase.Click += (s, e) => _NavigateToNewPurchaseTab();

            _dataGrid.FinalizeSetup();
        }

        // ── Data Loading ──────────────────────────────────────────────────────────

        private void _LoadPurchases()
        {
            var criteria = new clsPurchase.PurchaseSearchCriteria
            {
                SearchText = _dataGrid.SearchText,
                SearchBy   = _dataGrid.SearchField ?? "InvoiceNumber",
                TransactionStatus = _dataGrid.CurrentTab switch
                {
                    "InProgress" => 1,
                    "Cancelled"  => 2,
                    "Completed"  => 3,
                    _            => null
                },
                PageNumber = _dataGrid.CurrentPage,
                PageSize   = _dataGrid.PageSize,
                SortBy     = "TransactionDate"
            };

            try
            {
                DataTable dt = clsPurchase.SearchPurchasePages(criteria);

                int totalCount = (dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount"))
                    ? Convert.ToInt32(dt.Rows[0]["TotalCount"])
                    : 0;

                _dataGrid.SetDataSource(dt, totalCount);
                _ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to load purchases: {ex.Message}");
            }
        }

        private void _ConfigureGridColumns()
        {
            _dataGrid.ConfigureColumn("PurchaseID",        "ID",            70,  true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("InvoiceNumber",     "Invoice #",    140,  true);
            _dataGrid.ConfigureColumn("SupplierName",      "Supplier",     180,  true);
            _dataGrid.ConfigureColumn("SupplierType",      "Type",          80,  true,
                DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("TransactionDate",   "Date",         120,  true,
                DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("TotalAmount",       "Total",        110,  true,
                DataGridViewContentAlignment.MiddleRight, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("PaidAmount",        "Paid",         110,  true,
                DataGridViewContentAlignment.MiddleRight, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("TransactionStatus", "Status",        90,  true,
                DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("EmployeeName",      "Employee",     150,  true);
            _dataGrid.ConfigureColumn("PositionName",      "Position",     120,  true);

            // Hidden columns
            _dataGrid.ConfigureColumn("TotalCount",        "", 0, false);

            _dataGrid.FillLastColumn();
        }

        // ── Grid Events ───────────────────────────────────────────────────────────

        private void _DataGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var idVal = _dataGrid.DataGridView.Rows[e.RowIndex].Cells["PurchaseID"]?.Value;
            if (idVal != null && idVal != DBNull.Value)
                _OpenPurchaseForEdit(Convert.ToInt32(idVal));
        }

        private void _ContextMenu_ViewEdit(object? sender, EventArgs e)
        {
            if (_dataGrid.DataGridView.CurrentRow == null) return;

            var idVal = _dataGrid.DataGridView.CurrentRow.Cells["PurchaseID"]?.Value;
            if (idVal != null && idVal != DBNull.Value)
                _OpenPurchaseForEdit(Convert.ToInt32(idVal));
        }

        // ── Purchase Operations ───────────────────────────────────────────────────

        private void _OpenPurchaseForEdit(int purchaseID)
        {
            try
            {
                // Navigate to the "New Purchase" tab in parent and load the purchase
                var parentTabControl = this.Parent?.Parent as TabControl;
                if (parentTabControl != null)
                {
                    // Find the New Purchase tab (index 2)
                    var newPurchaseTab = parentTabControl.TabPages.Count > 2
                        ? parentTabControl.TabPages[2]
                        : null;

                    if (newPurchaseTab != null)
                    {
                        // Remove any existing controls and create a new edit control with the purchase ID
                        newPurchaseTab.Controls.Clear();
                        var editCtrl = new RMS_UI.Suppliers_Purchase.ctrlAddEditPurchase(purchaseID);
                        editCtrl.PurchaseSaved += (s, purchase) =>
                        {
                            editCtrl.BackToAddNewMode();
                            _LoadPurchases();
                        };
                        editCtrl.Dock = DockStyle.Fill;
                        newPurchaseTab.Controls.Add(editCtrl);
                        parentTabControl.SelectedTab = newPurchaseTab;
                        return;
                    }
                }

                _notification.ShowWarning("Could not navigate to purchase editor.");
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error opening purchase: {ex.Message}");
            }
        }

        private void _NavigateToNewPurchaseTab()
        {
            try
            {
                var parentTabControl = this.Parent?.Parent as TabControl;
                if (parentTabControl != null && parentTabControl.TabPages.Count > 2)
                {
                    // Reset the edit control to AddNew mode
                    var newPurchaseTab = parentTabControl.TabPages[2];
                    foreach (Control ctrl in newPurchaseTab.Controls)
                    {
                        if (ctrl is RMS_UI.Suppliers_Purchase.ctrlAddEditPurchase editCtrl)
                        {
                            editCtrl.BackToAddNewMode();
                            break;
                        }
                    }
                    parentTabControl.SelectedTab = newPurchaseTab;
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error: {ex.Message}");
            }
        }

        // ── Bulk Operations ───────────────────────────────────────────────────────

        private void _BulkDelete()
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one purchase.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete {rows.Count} purchase(s)?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm == DialogResult.Yes)
            {
                int ok = 0;
                foreach (var row in rows)
                {
                    if (row.Cells["PurchaseID"]?.Value is int id)
                        if (clsPurchase.DeletePurchase(id, clsGlobalUser.CurrentUser?.UserID))
                            ok++;
                }
                _notification.ShowSuccess($"{ok} purchase(s) deleted.");
                _LoadPurchases();
            }
        }

        // ── Refresh ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Public method to refresh the purchases grid from outside (e.g., after saving).
        /// </summary>
        public void RefreshData()
        {
            _LoadPurchases();
        }

        // ── Theme ─────────────────────────────────────────────────────────────────

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;

            _headerPanel.BackColor = c.ContentBackground;
            _lblTitle.ForeColor    = c.TitleText;
            _lblSubtitle.ForeColor = c.SecondaryText;

            _btnNewPurchase.BackColor = c.Primary;
            _btnNewPurchase.ForeColor = Color.White;
            _btnNewPurchase.FlatAppearance.MouseOverBackColor = c.PrimaryHover;
            _btnNewPurchase.FlatAppearance.MouseDownBackColor = c.PrimaryHover;

            _dataGrid.ApplyTheme();
        }
    }
}
