namespace RMS_UI.Controls
{
    partial class ReusableDataGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.Panel _tabPanel;
        private System.Windows.Forms.Panel _searchPanel;
        private ModernDataGridView _gridView;
        private System.Windows.Forms.Panel _emptyStatePanel;
        // Dynamic controls - created by RecreateSearchControls()
        private System.Windows.Forms.ComboBox _cmbSearchField;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.Button _btnClearSearch;
        // Empty state controls
        private System.Windows.Forms.Label _emptyIcon;
        private System.Windows.Forms.Label _emptyMessage;
        private System.Windows.Forms.Button _btnClearFilters;
        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Context menu is now owned by _gridView, disposed automatically
                if (components != null) components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this._topPanel = new System.Windows.Forms.Panel();
            this._tabPanel = new System.Windows.Forms.Panel();
            this._searchPanel = new System.Windows.Forms.Panel();
            this._gridView = new RMS_UI.Controls.ModernDataGridView();
            this._emptyStatePanel = new System.Windows.Forms.Panel();
            this._emptyIcon = new System.Windows.Forms.Label();
            this._emptyMessage = new System.Windows.Forms.Label();
            this._btnClearFilters = new System.Windows.Forms.Button();
            this._topPanel.SuspendLayout();
            this._emptyStatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _topPanel
            // 
            this._topPanel.BackColor = System.Drawing.Color.White;
            this._topPanel.Controls.Add(this._searchPanel);
            this._topPanel.Controls.Add(this._tabPanel);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Height = 100;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this._topPanel.Size = new System.Drawing.Size(900, 100);
            this._topPanel.TabIndex = 0;
            // 
            // _tabPanel
            // 
            this._tabPanel.BackColor = System.Drawing.Color.Transparent;
            this._tabPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._tabPanel.Height = 40;
            this._tabPanel.Location = new System.Drawing.Point(15, 10);
            this._tabPanel.Name = "_tabPanel";
            this._tabPanel.Size = new System.Drawing.Size(870, 40);
            this._tabPanel.TabIndex = 0;
            // 
            // _searchPanel
            // 
            this._searchPanel.BackColor = System.Drawing.Color.Transparent;
            this._searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._searchPanel.Height = 45;
            this._searchPanel.Location = new System.Drawing.Point(15, 50);
            this._searchPanel.Name = "_searchPanel";
            this._searchPanel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this._searchPanel.Size = new System.Drawing.Size(870, 45);
            this._searchPanel.TabIndex = 1;
            // 
            // _gridView
            // 
            this._gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridView.Location = new System.Drawing.Point(0, 100);
            this._gridView.Name = "_gridView";
            this._gridView.ShowCheckboxColumn = true;
            this._gridView.Size = new System.Drawing.Size(900, 500);
            this._gridView.TabIndex = 1;
            this._gridView.PageChanged += new System.EventHandler<RMS_UI.Controls.PageChangedEventArgs>(this.GridView_PageChanged);
            this._gridView.CellDoubleClicked += new System.EventHandler<System.Windows.Forms.DataGridViewCellEventArgs>(this.GridView_CellDoubleClicked);
            this._gridView.SelectionChanged += new System.EventHandler(this.GridView_SelectionChanged);
            // 
            // _emptyStatePanel
            // 
            this._emptyStatePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this._emptyStatePanel.Controls.Add(this._btnClearFilters);
            this._emptyStatePanel.Controls.Add(this._emptyMessage);
            this._emptyStatePanel.Controls.Add(this._emptyIcon);
            this._emptyStatePanel.Location = new System.Drawing.Point(200, 200);
            this._emptyStatePanel.Name = "_emptyStatePanel";
            this._emptyStatePanel.Size = new System.Drawing.Size(300, 200);
            this._emptyStatePanel.TabIndex = 2;
            this._emptyStatePanel.Visible = false;
            // 
            // _emptyIcon
            // 
            this._emptyIcon.Dock = System.Windows.Forms.DockStyle.Top;
            this._emptyIcon.Font = new System.Drawing.Font("Segoe UI", 48F);
            this._emptyIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this._emptyIcon.Location = new System.Drawing.Point(0, 0);
            this._emptyIcon.Name = "_emptyIcon";
            this._emptyIcon.Size = new System.Drawing.Size(300, 100);
            this._emptyIcon.TabIndex = 0;
            this._emptyIcon.Text = "📭";
            this._emptyIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _emptyMessage
            // 
            this._emptyMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this._emptyMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._emptyMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this._emptyMessage.Location = new System.Drawing.Point(0, 100);
            this._emptyMessage.Name = "_emptyMessage";
            this._emptyMessage.Size = new System.Drawing.Size(300, 50);
            this._emptyMessage.TabIndex = 1;
            this._emptyMessage.Text = "No data found";
            this._emptyMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _btnClearFilters
            // 
            this._btnClearFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this._btnClearFilters.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClearFilters.FlatAppearance.BorderSize = 0;
            this._btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClearFilters.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnClearFilters.ForeColor = System.Drawing.Color.White;
            this._btnClearFilters.Location = new System.Drawing.Point(85, 160);
            this._btnClearFilters.Name = "_btnClearFilters";
            this._btnClearFilters.Size = new System.Drawing.Size(130, 35);
            this._btnClearFilters.TabIndex = 2;
            this._btnClearFilters.Text = "Clear Filters";
            this._btnClearFilters.UseVisualStyleBackColor = false;
            this._btnClearFilters.Click += new System.EventHandler(this.BtnClearFilters_Click);
            // 
            // ReusableDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._emptyStatePanel);
            this.Controls.Add(this._gridView);
            this.Controls.Add(this._topPanel);
            this.Name = "ReusableDataGrid";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.ReusableDataGrid_Load);
            this._topPanel.ResumeLayout(false);
            this._emptyStatePanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
