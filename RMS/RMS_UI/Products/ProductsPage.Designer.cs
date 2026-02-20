namespace RMS_UI.Controls
{
    partial class ProductsPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _headerPanel;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblSubtitle;
        private System.Windows.Forms.Button _btnNewProduct;
        private System.Windows.Forms.Button _btnSettings;
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
            _headerPanel = new Panel();
            _btnSettings = new Button();
            _btnNewProduct = new Button();
            _lblSubtitle = new Label();
            _lblTitle = new Label();
            _dataGrid = new ReusableDataGrid();
            _notification = new NotificationControl();
            _headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _headerPanel
            // 
            _headerPanel.BackColor = Color.White;
            _headerPanel.Controls.Add(_btnSettings);
            _headerPanel.Controls.Add(_btnNewProduct);
            _headerPanel.Controls.Add(_lblSubtitle);
            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Dock = DockStyle.Top;
            _headerPanel.Location = new Point(0, 0);
            _headerPanel.Name = "_headerPanel";
            _headerPanel.Padding = new Padding(20, 15, 20, 15);
            _headerPanel.Size = new Size(1527, 80);
            _headerPanel.TabIndex = 1;
            // 
            // _btnSettings
            // 
            _btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnSettings.BackColor = Color.Transparent;
            _btnSettings.Cursor = Cursors.Hand;
            _btnSettings.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnSettings.FlatStyle = FlatStyle.Flat;
            _btnSettings.Font = new Font("Segoe UI", 10F);
            _btnSettings.ForeColor = Color.FromArgb(100, 116, 139);
            _btnSettings.Location = new Point(1287, 22);
            _btnSettings.Name = "_btnSettings";
            _btnSettings.Size = new Size(50, 38);
            _btnSettings.TabIndex = 3;
            _btnSettings.Text = "⚙";
            _btnSettings.UseVisualStyleBackColor = false;
            _btnSettings.Click += BtnSettings_Click;
            // 
            // _btnNewProduct
            // 
            _btnNewProduct.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnNewProduct.BackColor = Color.FromArgb(59, 130, 246);
            _btnNewProduct.Cursor = Cursors.Hand;
            _btnNewProduct.FlatAppearance.BorderSize = 0;
            _btnNewProduct.FlatStyle = FlatStyle.Flat;
            _btnNewProduct.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnNewProduct.ForeColor = Color.White;
            _btnNewProduct.Location = new Point(1347, 22);
            _btnNewProduct.Name = "_btnNewProduct";
            _btnNewProduct.Size = new Size(130, 38);
            _btnNewProduct.TabIndex = 2;
            _btnNewProduct.Text = "+ New Product";
            _btnNewProduct.UseVisualStyleBackColor = false;
            _btnNewProduct.Click += BtnNewProduct_Click;
            // 
            // _lblSubtitle
            // 
            _lblSubtitle.AutoSize = true;
            _lblSubtitle.Font = new Font("Segoe UI", 9F);
            _lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSubtitle.Location = new Point(20, 50);
            _lblSubtitle.Name = "_lblSubtitle";
            _lblSubtitle.Size = new Size(175, 15);
            _lblSubtitle.TabIndex = 1;
            _lblSubtitle.Text = "Manage your product inventory";
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new Point(20, 15);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(116, 32);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "Products";
            // 
            // _dataGrid
            // 
            _dataGrid.BackColor = Color.White;
            _dataGrid.Dock = DockStyle.Fill;
            _dataGrid.Location = new Point(0, 80);
            _dataGrid.Name = "_dataGrid";
            _dataGrid.Size = new Size(1527, 630);
            _dataGrid.TabIndex = 2;
            _dataGrid.TabChanged += DataGrid_TabChanged;
            _dataGrid.SearchRequested += DataGrid_SearchRequested;
            _dataGrid.PageChanged += DataGrid_PageChanged;
            _dataGrid.CellDoubleClicked += DataGrid_CellDoubleClicked;
            _dataGrid.ClearFiltersClicked += DataGrid_ClearFiltersClicked;
            _dataGrid.ActivateSelected += DataGrid_ActivateSelected;
            _dataGrid.DeactivateSelected += DataGrid_DeactivateSelected;
            _dataGrid.ExportToExcelSelected += DataGrid_ExportToExcelSelected;
            _dataGrid.DeleteSelected += DataGrid_DeleteSelected;
            // 
            // _notification
            // 
            _notification.Dock = DockStyle.Top;
            _notification.Location = new Point(0, 0);
            _notification.Name = "_notification";
            _notification.Size = new Size(1527, 0);
            _notification.TabIndex = 0;
            _notification.Visible = false;
            // 
            // ProductsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(_dataGrid);
            Controls.Add(_headerPanel);
            Controls.Add(_notification);
            Name = "ProductsPage";
            Size = new Size(1527, 710);
            Load += ProductsPage_Load;
            _headerPanel.ResumeLayout(false);
            _headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
