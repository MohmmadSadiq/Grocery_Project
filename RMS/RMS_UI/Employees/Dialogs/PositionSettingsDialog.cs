using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    public class EmployeeSettingsDialog : Form
    {
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private TabControl _tabControl = null!;
        private TabPage _tabPositions = null!;
        private Panel _toolbarPanel = null!;
        private ModernDataGridView _positionsGrid = null!;
        private Button _btnAddPosition = null!;
        private Button _btnEditPosition = null!;
        private Button _btnDeletePosition = null!;
        private Button _btnClose = null!;
        private NotificationControl _notification = null!;

        public EmployeeSettingsDialog()
        {
            InitializeUi();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadPositions();
        }

        private void InitializeUi()
        {
            SuspendLayout();

            Text = "Employee Settings";
            Size = new Size(860, 620);
            MinimumSize = new Size(700, 460);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 10F);

            _notification = new NotificationControl
            {
                Dock = DockStyle.Top
            };

            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(59, 130, 246),
                Padding = new Padding(20, 0, 20, 0)
            };

            _titleLabel = new Label
            {
                Text = "⚙ Employee Settings",
                Font = new Font("Segoe UI Semibold", 16F),
                ForeColor = Color.White,
                AutoSize = true
            };
            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Resize += (s, e) =>
            {
                _titleLabel.Location = new Point(20, (_headerPanel.Height - _titleLabel.Height) / 2);
            };

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                Padding = new Point(15, 8)
            };

            _tabPositions = new TabPage
            {
                Text = "🧑‍💼 Positions",
                Padding = new Padding(15)
            };

            CreatePositionsTab();
            _tabControl.TabPages.Add(_tabPositions);

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(_tabControl);

            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                Padding = new Padding(15)
            };

            _btnClose = new Button
            {
                Text = "Close",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(100, 36),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Right
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.Click += (s, e) => Close();

            bottomPanel.Controls.Add(_btnClose);
            bottomPanel.Resize += (s, e) =>
            {
                _btnClose.Location = new Point(bottomPanel.Width - _btnClose.Width - 15, (bottomPanel.Height - _btnClose.Height) / 2);
            };

            Controls.Add(contentPanel);
            Controls.Add(bottomPanel);
            Controls.Add(_headerPanel);
            Controls.Add(_notification);

            ResumeLayout(false);
        }

        private void CreatePositionsTab()
        {
            _toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                Padding = new Padding(15, 8, 15, 8)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnAddPosition = CreateActionButton("➕ Add Position", Color.FromArgb(34, 197, 94));
            _btnAddPosition.Click += BtnAddPosition_Click;

            _btnEditPosition = CreateActionButton("✏️ Edit", Color.FromArgb(59, 130, 246));
            _btnEditPosition.Click += BtnEditPosition_Click;

            _btnDeletePosition = CreateActionButton("🗑️ Delete", Color.FromArgb(239, 68, 68));
            _btnDeletePosition.Click += BtnDeletePosition_Click;

            flowPanel.Controls.AddRange(new Control[] { _btnAddPosition, _btnEditPosition, _btnDeletePosition });
            _toolbarPanel.Controls.Add(flowPanel);

            _positionsGrid = new ModernDataGridView
            {
                Dock = DockStyle.Fill,
                ShowPagination = false,
                ShowCheckboxColumn = false,
                ShowContextMenu = false
            };
            _positionsGrid.CellDoubleClicked += PositionsGrid_CellDoubleClicked;

            _tabPositions.Controls.Add(_positionsGrid);
            _tabPositions.Controls.Add(_toolbarPanel);
        }

        private static Button CreateActionButton(string text, Color bgColor)
        {
            var button = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(130, 34),
                Cursor = Cursors.Hand,
                BackColor = bgColor,
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 10, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadPositions()
        {
            try
            {
                DataTable positions = clsPosition.GetAllPosition();
                _positionsGrid.SetDataSource(positions);
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error loading positions: {ex.Message}");
            }
        }

        private void ConfigureColumns()
        {
            _positionsGrid.ConfigureColumn("PositionID", "Position ID", 100,
                true, DataGridViewContentAlignment.MiddleCenter, "Consolas", 9.5f);
            _positionsGrid.ConfigureColumn("PositionName", "Position Name", 220);
            _positionsGrid.ConfigureColumn("Description", "Description", 320);
            _positionsGrid.ConfigureColumn("CreatedDate", "Created Date", 140,
                true, DataGridViewContentAlignment.MiddleCenter);
            _positionsGrid.ConfigureColumn("CreatedByUserID", string.Empty, 0, false);
            _positionsGrid.ConfigureColumn("UpdatedDate", string.Empty, 0, false);
            _positionsGrid.ConfigureColumn("UpdatedByUserID", string.Empty, 0, false);

            _positionsGrid.FillLastColumn();
        }

        private void BtnAddPosition_Click(object? sender, EventArgs e)
        {
            using var dialog = new PositionEditorDialog();
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            var position = new clsPosition
            {
                PositionName = dialog.PositionName,
                Description = string.IsNullOrWhiteSpace(dialog.Description) ? null : dialog.Description,
                CreatedByUserID = clsGlobalUser.CurrentUser?.UserID
            };

            if (position.Save())
            {
                LoadPositions();
                _notification.ShowSuccess("Position added successfully.");
            }
            else
            {
                _notification.ShowError("Failed to add position.");
            }
        }

        private void BtnEditPosition_Click(object? sender, EventArgs e)
        {
            EditSelectedPosition();
        }

        private void PositionsGrid_CellDoubleClicked(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedPosition();
        }

        private void EditSelectedPosition()
        {
            if (_positionsGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a position to edit.");
                return;
            }

            DataGridViewRow row = _positionsGrid.DataGridView.SelectedRows[0];
            if (row.Cells["PositionID"]?.Value == null || row.Cells["PositionID"].Value == DBNull.Value)
            {
                _notification.ShowWarning("Invalid position selection.");
                return;
            }

            int positionId = Convert.ToInt32(row.Cells["PositionID"].Value);
            clsPosition? position = clsPosition.Find(positionId);
            if (position == null)
            {
                _notification.ShowError("Position not found.");
                return;
            }

            using var dialog = new PositionEditorDialog(position.PositionName, position.Description);
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            position.PositionName = dialog.PositionName;
            position.Description = string.IsNullOrWhiteSpace(dialog.Description) ? null : dialog.Description;
            position.UpdatedByUserID = clsGlobalUser.CurrentUser?.UserID;

            if (position.Save())
            {
                LoadPositions();
                _notification.ShowSuccess("Position updated successfully.");
            }
            else
            {
                _notification.ShowError("Failed to update position.");
            }
        }

        private void BtnDeletePosition_Click(object? sender, EventArgs e)
        {
            if (_positionsGrid.DataGridView.SelectedRows.Count == 0)
            {
                _notification.ShowWarning("Please select a position to delete.");
                return;
            }

            DataGridViewRow row = _positionsGrid.DataGridView.SelectedRows[0];
            if (row.Cells["PositionID"]?.Value == null || row.Cells["PositionID"].Value == DBNull.Value)
            {
                _notification.ShowWarning("Invalid position selection.");
                return;
            }

            int positionId = Convert.ToInt32(row.Cells["PositionID"].Value);
            string positionName = row.Cells["PositionName"]?.Value?.ToString() ?? "this position";

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete {positionName}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                return;

            if (clsPosition.DeletePosition(positionId, clsGlobalUser.CurrentUser?.UserID))
            {
                LoadPositions();
                _notification.ShowSuccess("Position deleted successfully.");
            }
            else
            {
                _notification.ShowError("Failed to delete position. It may be linked to employees.");
            }
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;
            _toolbarPanel.BackColor = colors.ContentBackground;
            _tabControl.BackColor = colors.ContentBackground;

            foreach (TabPage tab in _tabControl.TabPages)
            {
                tab.BackColor = colors.ContentBackground;
            }

            _positionsGrid.ApplyTheme();

            Invalidate(true);
        }

        private sealed class PositionEditorDialog : Form
        {
            private readonly TextBox _txtPositionName;
            private readonly TextBox _txtDescription;

            public string PositionName => _txtPositionName.Text.Trim();
            public string? Description => _txtDescription.Text.Trim();

            public PositionEditorDialog(string? positionName = null, string? description = null)
            {
                Text = string.IsNullOrWhiteSpace(positionName) ? "Add Position" : "Edit Position";
                Size = new Size(520, 290);
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;

                var lblName = new Label
                {
                    Text = "Position Name",
                    Location = new Point(24, 22),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(51, 65, 85)
                };

                _txtPositionName = new TextBox
                {
                    Location = new Point(24, 44),
                    Width = 452,
                    Font = new Font("Segoe UI", 10F),
                    Text = positionName ?? string.Empty
                };

                var lblDescription = new Label
                {
                    Text = "Description",
                    Location = new Point(24, 84),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(51, 65, 85)
                };

                _txtDescription = new TextBox
                {
                    Location = new Point(24, 106),
                    Width = 452,
                    Height = 84,
                    Multiline = true,
                    Font = new Font("Segoe UI", 9.5F),
                    Text = description ?? string.Empty
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(292, 206),
                    Size = new Size(88, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(51, 65, 85)
                };

                var btnSave = new Button
                {
                    Text = "Save",
                    Location = new Point(388, 206),
                    Size = new Size(88, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(59, 130, 246),
                    ForeColor = Color.White
                };
                btnSave.FlatAppearance.BorderSize = 0;
                btnSave.Click += (_, _) =>
                {
                    if (string.IsNullOrWhiteSpace(PositionName))
                    {
                        MessageBox.Show("Position name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _txtPositionName.Focus();
                        return;
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                };

                Controls.Add(lblName);
                Controls.Add(_txtPositionName);
                Controls.Add(lblDescription);
                Controls.Add(_txtDescription);
                Controls.Add(btnCancel);
                Controls.Add(btnSave);

                AcceptButton = btnSave;
                CancelButton = btnCancel;
            }
        }
    }
}
