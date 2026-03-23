namespace RMS_UI.Controls
{
    partial class EmployeePage
    {
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _headerPanel;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblSubtitle;
        private System.Windows.Forms.Label _lblPositionFilter;
        private System.Windows.Forms.ComboBox _cmbPositionFilter;
        private System.Windows.Forms.Label _lblCountryFilter;
        private System.Windows.Forms.ComboBox _cmbCountryFilter;
        private System.Windows.Forms.Button _btnSettings;
        private System.Windows.Forms.Button _btnNewEmployee;
        private ReusableDataGrid _dataGrid;
        private NotificationControl _notification;
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            _headerPanel = new System.Windows.Forms.Panel();
            _cmbCountryFilter = new System.Windows.Forms.ComboBox();
            _lblCountryFilter = new System.Windows.Forms.Label();
            _cmbPositionFilter = new System.Windows.Forms.ComboBox();
            _lblPositionFilter = new System.Windows.Forms.Label();
            _btnSettings = new System.Windows.Forms.Button();
            _btnNewEmployee = new System.Windows.Forms.Button();
            _lblSubtitle = new System.Windows.Forms.Label();
            _lblTitle = new System.Windows.Forms.Label();
            _dataGrid = new ReusableDataGrid();
            _notification = new NotificationControl();
            _headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _headerPanel
            // 
            _headerPanel.BackColor = System.Drawing.Color.White;
            _headerPanel.Controls.Add(_cmbCountryFilter);
            _headerPanel.Controls.Add(_lblCountryFilter);
            _headerPanel.Controls.Add(_cmbPositionFilter);
            _headerPanel.Controls.Add(_lblPositionFilter);
            _headerPanel.Controls.Add(_btnSettings);
            _headerPanel.Controls.Add(_btnNewEmployee);
            _headerPanel.Controls.Add(_lblSubtitle);
            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            _headerPanel.Location = new System.Drawing.Point(0, 0);
            _headerPanel.Name = "_headerPanel";
            _headerPanel.Padding = new System.Windows.Forms.Padding(24, 14, 24, 14);
            _headerPanel.Size = new System.Drawing.Size(1600, 120);
            _headerPanel.TabIndex = 0;
            // 
            // _cmbCountryFilter
            // 
            _cmbCountryFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _cmbCountryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbCountryFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _cmbCountryFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _cmbCountryFilter.FormattingEnabled = true;
            _cmbCountryFilter.Location = new System.Drawing.Point(1080, 80);
            _cmbCountryFilter.Name = "_cmbCountryFilter";
            _cmbCountryFilter.Size = new System.Drawing.Size(220, 25);
            _cmbCountryFilter.TabIndex = 4;
            // 
            // _lblCountryFilter
            // 
            _lblCountryFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _lblCountryFilter.AutoSize = true;
            _lblCountryFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblCountryFilter.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblCountryFilter.Location = new System.Drawing.Point(1016, 84);
            _lblCountryFilter.Name = "_lblCountryFilter";
            _lblCountryFilter.Size = new System.Drawing.Size(50, 15);
            _lblCountryFilter.TabIndex = 5;
            _lblCountryFilter.Text = "Country";
            // 
            // _cmbPositionFilter
            // 
            _cmbPositionFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _cmbPositionFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbPositionFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _cmbPositionFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _cmbPositionFilter.FormattingEnabled = true;
            _cmbPositionFilter.Location = new System.Drawing.Point(760, 80);
            _cmbPositionFilter.Name = "_cmbPositionFilter";
            _cmbPositionFilter.Size = new System.Drawing.Size(220, 25);
            _cmbPositionFilter.TabIndex = 2;
            // 
            // _lblPositionFilter
            // 
            _lblPositionFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _lblPositionFilter.AutoSize = true;
            _lblPositionFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblPositionFilter.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblPositionFilter.Location = new System.Drawing.Point(706, 84);
            _lblPositionFilter.Name = "_lblPositionFilter";
            _lblPositionFilter.Size = new System.Drawing.Size(48, 15);
            _lblPositionFilter.TabIndex = 3;
            _lblPositionFilter.Text = "Position";
            // 
            // _btnSettings
            // 
            _btnSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnSettings.BackColor = System.Drawing.Color.FromArgb(107, 114, 128);
            _btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnSettings.FlatAppearance.BorderSize = 0;
            _btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnSettings.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _btnSettings.ForeColor = System.Drawing.Color.White;
            _btnSettings.Location = new System.Drawing.Point(1310, 26);
            _btnSettings.Name = "_btnSettings";
            _btnSettings.Size = new System.Drawing.Size(110, 42);
            _btnSettings.TabIndex = 1;
            _btnSettings.Text = "⚙ Settings";
            _btnSettings.UseVisualStyleBackColor = false;
            // 
            // _btnNewEmployee
            // 
            _btnNewEmployee.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnNewEmployee.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _btnNewEmployee.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnNewEmployee.FlatAppearance.BorderSize = 0;
            _btnNewEmployee.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            _btnNewEmployee.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            _btnNewEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnNewEmployee.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnNewEmployee.ForeColor = System.Drawing.Color.White;
            _btnNewEmployee.Location = new System.Drawing.Point(1158, 26);
            _btnNewEmployee.Name = "_btnNewEmployee";
            _btnNewEmployee.Size = new System.Drawing.Size(142, 42);
            _btnNewEmployee.TabIndex = 2;
            _btnNewEmployee.Text = "+ New Employee";
            _btnNewEmployee.UseVisualStyleBackColor = false;
            // 
            // _lblSubtitle
            // 
            _lblSubtitle.AutoSize = true;
            _lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblSubtitle.Location = new System.Drawing.Point(24, 56);
            _lblSubtitle.Name = "_lblSubtitle";
            _lblSubtitle.Size = new System.Drawing.Size(275, 17);
            _lblSubtitle.TabIndex = 7;
            _lblSubtitle.Text = "Manage employees with role and country filters";
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            _lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new System.Drawing.Point(24, 20);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(314, 37);
            _lblTitle.TabIndex = 6;
            _lblTitle.Text = "Employees Management";
            // 
            // _dataGrid
            // 
            _dataGrid.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            _dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            _dataGrid.Location = new System.Drawing.Point(0, 120);
            _dataGrid.Name = "_dataGrid";
            _dataGrid.ShowCheckboxColumn = true;
            _dataGrid.ShowContextMenu = true;
            _dataGrid.ShowSearch = true;
            _dataGrid.ShowTabs = true;
            _dataGrid.Size = new System.Drawing.Size(1600, 680);
            _dataGrid.TabIndex = 1;
            // 
            // _notification
            // 
            _notification.Dock = System.Windows.Forms.DockStyle.Top;
            _notification.Location = new System.Drawing.Point(0, 0);
            _notification.Name = "_notification";
            _notification.Size = new System.Drawing.Size(1600, 0);
            _notification.TabIndex = 2;
            _notification.Visible = false;
            // 
            // EmployeePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            Controls.Add(_dataGrid);
            Controls.Add(_headerPanel);
            Controls.Add(_notification);
            Name = "EmployeePage";
            Size = new System.Drawing.Size(1600, 800);
            _headerPanel.ResumeLayout(false);
            _headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
