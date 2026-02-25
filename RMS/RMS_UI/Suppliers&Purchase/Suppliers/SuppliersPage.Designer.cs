namespace RMS_UI.Controls
{
    partial class SuppliersPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _headerPanel;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblSubtitle;
        private System.Windows.Forms.Button _btnNewSupplier;
        private ReusableDataGrid _dataGrid;
        private NotificationControl _notification;
        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _headerPanel = new System.Windows.Forms.Panel();
            _btnNewSupplier = new System.Windows.Forms.Button();
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
            _headerPanel.Controls.Add(_btnNewSupplier);
            _headerPanel.Controls.Add(_lblSubtitle);
            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            _headerPanel.Location = new System.Drawing.Point(0, 0);
            _headerPanel.Name = "_headerPanel";
            _headerPanel.Padding = new System.Windows.Forms.Padding(24, 20, 24, 16);
            _headerPanel.Size = new System.Drawing.Size(1600, 90);
            _headerPanel.TabIndex = 0;
            // 
            // _btnNewSupplier
            // 
            _btnNewSupplier.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnNewSupplier.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _btnNewSupplier.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnNewSupplier.FlatAppearance.BorderSize = 0;
            _btnNewSupplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            _btnNewSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            _btnNewSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnNewSupplier.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnNewSupplier.ForeColor = System.Drawing.Color.White;
            _btnNewSupplier.Location = new System.Drawing.Point(1414, 26);
            _btnNewSupplier.Name = "_btnNewSupplier";
            _btnNewSupplier.Size = new System.Drawing.Size(158, 42);
            _btnNewSupplier.TabIndex = 0;
            _btnNewSupplier.Text = "+ New Supplier";
            _btnNewSupplier.UseVisualStyleBackColor = false;
            // 
            // _lblSubtitle
            // 
            _lblSubtitle.AutoSize = true;
            _lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblSubtitle.Location = new System.Drawing.Point(24, 56);
            _lblSubtitle.Name = "_lblSubtitle";
            _lblSubtitle.Size = new System.Drawing.Size(258, 17);
            _lblSubtitle.TabIndex = 2;
            _lblSubtitle.Text = "Manage suppliers, companies and contacts";
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            _lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new System.Drawing.Point(24, 20);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(297, 37);
            _lblTitle.TabIndex = 1;
            _lblTitle.Text = "Suppliers Management";
            // 
            // _dataGrid
            // 
            _dataGrid.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            _dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            _dataGrid.Location = new System.Drawing.Point(0, 90);
            _dataGrid.Name = "_dataGrid";
            _dataGrid.ShowCheckboxColumn = true;
            _dataGrid.ShowContextMenu = true;
            _dataGrid.ShowSearch = true;
            _dataGrid.ShowTabs = true;
            _dataGrid.Size = new System.Drawing.Size(1600, 710);
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
            // SuppliersPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            Controls.Add(_dataGrid);
            Controls.Add(_headerPanel);
            Controls.Add(_notification);
            Name = "SuppliersPage";
            Size = new System.Drawing.Size(1600, 800);
            _headerPanel.ResumeLayout(false);
            _headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
