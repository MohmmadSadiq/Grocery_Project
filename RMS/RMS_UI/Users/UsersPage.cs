using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class UsersPage : UserControl
    {
        private const int UserImageSize = 50;

        private static readonly Dictionary<string, bool?> _tabStatusMap = new()
        {
            ["All"] = null,
            ["Active"] = true,
            ["Inactive"] = false
        };

        private static readonly Dictionary<string, string> _searchFieldMap = new()
        {
            ["UserName"] = "UserName",
            ["UserID"] = "UserID",
            ["FullName"] = "FullName"
        };

        public UsersPage()
        {
            InitializeComponent();
            ConfigureDataGrid();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshUsers();
        }

        private void ConfigureDataGrid()
        {
            _dataGrid.SetTabs(
                new TabDefinition("All", "All"),
                new TabDefinition("Active", "Active"),
                new TabDefinition("Inactive", "Inactive")
            );

            _dataGrid.SetSearchFields(
                new SearchFieldDefinition("User Name", "UserName"),
                new SearchFieldDefinition("User ID", "UserID"),
                new SearchFieldDefinition("Full Name", "FullName")
            );

            _dataGrid.AddSelectAllMenuItem();
            _dataGrid.AddStandardStatusMenuItems(
                hasActivate: true,
                hasDeactivate: true,
                hasDelete: true,
                hasExport: true);

            _dataGrid.TabChanged += (s, e) => RefreshUsers();
            _dataGrid.SearchRequested += (s, e) => RefreshUsers();
            _dataGrid.PageChanged += (s, e) => RefreshUsers();
            _dataGrid.CellDoubleClicked += DataGrid_CellDoubleClicked;
            _dataGrid.ClearSearchClicked += (s, e) =>
            {
                _dataGrid.ClearSearch();
                RefreshUsers();
            };
            _dataGrid.ClearFiltersClicked += (s, e) =>
            {
                _dataGrid.ClearAll();
                RefreshUsers();
            };

            _dataGrid.ActivateSelected += (s, e) => BulkUpdateStatus(true);
            _dataGrid.DeactivateSelected += (s, e) => BulkUpdateStatus(false);
            _dataGrid.DeleteSelected += (s, e) => BulkDeleteUsers();
            _dataGrid.ExportToExcelSelected += (s, e) => _notification.ShowInfo("Export to Excel coming soon.");

            _btnNewUser.Click += BtnNewUser_Click;

            _dataGrid.FinalizeSetup();
        }

        private void RefreshUsers()
        {
            try
            {
                clsUser.UserSearchCriteria criteria = BuildSearchCriteria();
                DataTable users = clsUser.SearchUserPages(criteria);

                int totalCount = ExtractTotalCount(users);
                _dataGrid.SetDataSource(users, totalCount);
                ConfigureGridColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to load users: {ex.Message}");
            }
        }

        private clsUser.UserSearchCriteria BuildSearchCriteria()
        {
            string selectedSearchField = _dataGrid.SearchField;
            string searchBy = _searchFieldMap.TryGetValue(selectedSearchField, out string? mapped)
                ? mapped
                : "UserName";

            bool? statusFilter = _tabStatusMap.TryGetValue(_dataGrid.CurrentTab, out bool? isActive)
                ? isActive
                : null;

            return new clsUser.UserSearchCriteria
            {
                SearchText = _dataGrid.SearchText,
                SearchBy = searchBy,
                IsActive = statusFilter,
                PageNumber = _dataGrid.CurrentPage,
                PageSize = _dataGrid.PageSize,
                SortBy = "UserName"
            };
        }

        private static int ExtractTotalCount(DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                return 0;
            }

            if (table.Columns.Contains("TotalRows"))
            {
                return Convert.ToInt32(table.Rows[0]["TotalRows"]);
            }

            if (table.Columns.Contains("TotalCount"))
            {
                return Convert.ToInt32(table.Rows[0]["TotalCount"]);
            }

            return table.Rows.Count;
        }

        private void ConfigureGridColumns()
        {
            _dataGrid.AddImageColumn("UserImage", "", 55, 1);

            _dataGrid.ConfigureColumn("UserID", "ID", 70, true,
                DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _dataGrid.ConfigureColumn("UserName", "Username", 180);
            _dataGrid.ConfigureColumn("FullName", "Full Name", 220);
            _dataGrid.ConfigureColumn("IsActive", "Status", 80,
                true, DataGridViewContentAlignment.MiddleCenter);
            _dataGrid.ConfigureColumn("CreatedDate", "Created", 130,
                true, DataGridViewContentAlignment.MiddleCenter);

            _dataGrid.ConfigureColumn("PersonID", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("PasswordHash", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("PasswordSalt", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("CreatedByUserID", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("UpdatedDate", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("UpdatedByUserID", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("ImagePath", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("TotalRows", string.Empty, 0, false);
            _dataGrid.ConfigureColumn("TotalCount", string.Empty, 0, false);

            _dataGrid.FillLastColumn();

            foreach (DataGridViewColumn col in _dataGrid.DataGridView.Columns)
            {
                if (col.Name != "SelectCheckbox" && col.Name != "UserImage")
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }

            LoadUserImages();
        }

        private void LoadUserImages()
        {
            DataGridView dgv = _dataGrid.DataGridView;
            if (dgv.Columns["UserImage"] == null || dgv.Columns["ImagePath"] == null)
            {
                return;
            }

            if (dgv.IsHandleCreated)
            {
                dgv.BeginInvoke(new Action(() => SetUserImages(dgv)));
                return;
            }

            SetUserImages(dgv);
        }

        private static void SetUserImages(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                if (row.Cells["UserImage"]?.Value is Image oldImage)
                {
                    row.Cells["UserImage"].Value = null;
                    oldImage.Dispose();
                }

                try
                {
                    string? imagePath = row.Cells["ImagePath"]?.Value == DBNull.Value
                        ? null
                        : row.Cells["ImagePath"]?.Value?.ToString();

                    row.Cells["UserImage"].Value = TryCreateUserThumbnail(imagePath);
                }
                catch
                {
                    row.Cells["UserImage"].Value = null;
                }
            }

            dgv.Refresh();
        }

        private static Image? TryCreateUserThumbnail(string? imagePath)
        {
            Image? image = ImageManager.LoadImage(imagePath);
            if (image == null)
            {
                return null;
            }

            try
            {
                return new Bitmap(image, new Size(UserImageSize, UserImageSize));
            }
            finally
            {
                image.Dispose();
            }
        }

        private void BulkUpdateStatus(bool isActive)
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one user.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to {(isActive ? "activate" : "deactivate")} {rows.Count} user(s)?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int updatedCount = 0;
            foreach (DataGridViewRow row in rows)
            {
                if (!TryGetUserId(row, out int userId))
                {
                    continue;
                }

                clsUser? user = clsUser.Find(userId);
                if (user == null || user.IsActive == isActive)
                {
                    continue;
                }

                user.IsActive = isActive;
                user.UpdatedByUserID = clsGlobalUser.CurrentUser?.UserID;

                if (user.Save())
                {
                    updatedCount++;
                }
            }

            _notification.ShowSuccess($"{updatedCount} user(s) {(isActive ? "activated" : "deactivated")}.");
            RefreshUsers();
        }

        private void BulkDeleteUsers()
        {
            var rows = _dataGrid.GetCheckedRows();
            if (rows.Count == 0)
            {
                _notification.ShowWarning("Please select at least one user.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Deactivate {rows.Count} user(s)?\n\nThis action will set them as inactive.",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int deletedCount = 0;
            int? currentUserId = clsGlobalUser.CurrentUser?.UserID;

            foreach (DataGridViewRow row in rows)
            {
                if (!TryGetUserId(row, out int userId))
                {
                    continue;
                }

                if (clsUser.DeleteUser(userId, currentUserId))
                {
                    deletedCount++;
                }
            }

            _notification.ShowSuccess($"{deletedCount} user(s) deactivated.");
            RefreshUsers();
        }

        private static bool TryGetUserId(DataGridViewRow row, out int userId)
        {
            userId = 0;

            if (row.Cells["UserID"]?.Value == null || row.Cells["UserID"].Value == DBNull.Value)
            {
                return false;
            }

            string? rawValue = row.Cells["UserID"].Value?.ToString();
            return int.TryParse(rawValue, out userId);
        }

        private void BtnNewUser_Click(object? sender, EventArgs e)
        {
            OpenAddUserDialog();
        }

        private void DataGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                string columnName = _dataGrid.DataGridView.Columns[e.ColumnIndex].Name;
                if (columnName == "SelectCheckbox" || columnName == "UserImage")
                {
                    return;
                }
            }

            object? value = _dataGrid.DataGridView.Rows[e.RowIndex].Cells["UserID"]?.Value;
            if (value == null || value == DBNull.Value)
            {
                return;
            }

            if (!int.TryParse(value.ToString(), out int userId))
            {
                return;
            }

            OpenEditUserDialog(userId);
        }

        private void OpenAddUserDialog()
        {
            try
            {
                using var dialog = new frmAddEditUser();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _notification.ShowSuccess("User added successfully.");
                    RefreshUsers();
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to open add user form: {ex.Message}");
            }
        }

        private void OpenEditUserDialog(int userId)
        {
            try
            {
                using var dialog = new frmAddEditUser(userId);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _notification.ShowSuccess("User updated successfully.");
                    RefreshUsers();
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Failed to open edit user form: {ex.Message}");
            }
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;

            _headerPanel.BackColor = colors.ContentBackground;
            _lblTitle.ForeColor = colors.TitleText;
            _lblSubtitle.ForeColor = colors.SecondaryText;

            _btnNewUser.BackColor = colors.Primary;
            _btnNewUser.ForeColor = Color.White;
            _btnNewUser.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;
            _btnNewUser.FlatAppearance.MouseDownBackColor = colors.PrimaryHover;

            _dataGrid.ApplyTheme();
        }
    }
}
