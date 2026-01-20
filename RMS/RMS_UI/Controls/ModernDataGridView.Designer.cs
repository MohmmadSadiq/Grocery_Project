namespace RMS_UI.Controls
{
    partial class ModernDataGridView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.DataGridView _dataGridView;
        private System.Windows.Forms.Panel _paginationPanel;
        private System.Windows.Forms.Button _btnPrevious;
        private System.Windows.Forms.Button _btnNext;
        private System.Windows.Forms.Label _lblPageInfo;
        private System.Windows.Forms.ComboBox _cmbPageSize;
        private System.Windows.Forms.Label _lblPageSizeLabel;
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
            _dataGridView = new DataGridView();
            _paginationPanel = new Panel();
            _btnNext = new Button();
            _lblPageInfo = new Label();
            _btnPrevious = new Button();
            _cmbPageSize = new ComboBox();
            _lblPageSizeLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)_dataGridView).BeginInit();
            _paginationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _dataGridView
            // 
            _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = false;
            _dataGridView.AllowUserToResizeRows = false;
            _dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dataGridView.BackgroundColor = Color.White;
            _dataGridView.BorderStyle = BorderStyle.None;
            _dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            _dataGridView.ColumnHeadersHeight = 45;
            _dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dataGridView.Dock = DockStyle.Fill;
            _dataGridView.EnableHeadersVisualStyles = false;
            _dataGridView.Font = new Font("Segoe UI", 9.5F);
            _dataGridView.GridColor = Color.FromArgb(240, 240, 240);
            _dataGridView.Location = new Point(0, 0);
            _dataGridView.Name = "_dataGridView";
            _dataGridView.ReadOnly = true;
            _dataGridView.RowHeadersVisible = false;
            _dataGridView.RowTemplate.Height = 40;
            _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dataGridView.Size = new Size(800, 450);
            _dataGridView.TabIndex = 0;
            _dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;
            _dataGridView.CellMouseEnter += DataGridView_CellMouseEnter;
            _dataGridView.CellMouseLeave += DataGridView_CellMouseLeave;
            _dataGridView.CellPainting += DataGridView_CellPainting;
            _dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            // 
            // _paginationPanel
            // 
            _paginationPanel.BackColor = Color.FromArgb(248, 250, 252);
            _paginationPanel.Controls.Add(_btnNext);
            _paginationPanel.Controls.Add(_lblPageInfo);
            _paginationPanel.Controls.Add(_btnPrevious);
            _paginationPanel.Controls.Add(_cmbPageSize);
            _paginationPanel.Controls.Add(_lblPageSizeLabel);
            _paginationPanel.Dock = DockStyle.Bottom;
            _paginationPanel.Location = new Point(0, 450);
            _paginationPanel.Name = "_paginationPanel";
            _paginationPanel.Padding = new Padding(15, 0, 15, 0);
            _paginationPanel.Size = new Size(800, 50);
            _paginationPanel.TabIndex = 1;
            _paginationPanel.Resize += PaginationPanel_Resize;
            // 
            // _btnNext
            // 
            _btnNext.Anchor = AnchorStyles.Right;
            _btnNext.BackColor = Color.White;
            _btnNext.Cursor = Cursors.Hand;
            _btnNext.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            _btnNext.FlatStyle = FlatStyle.Flat;
            _btnNext.Font = new Font("Segoe UI", 9F);
            _btnNext.ForeColor = Color.FromArgb(100, 116, 139);
            _btnNext.Location = new Point(705, 9);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new Size(80, 32);
            _btnNext.TabIndex = 4;
            _btnNext.Text = "Next ►";
            _btnNext.UseVisualStyleBackColor = false;
            _btnNext.Click += BtnNext_Click;
            // 
            // _lblPageInfo
            // 
            _lblPageInfo.Anchor = AnchorStyles.None;
            _lblPageInfo.AutoSize = true;
            _lblPageInfo.Font = new Font("Segoe UI", 9F);
            _lblPageInfo.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPageInfo.Location = new Point(340, 17);
            _lblPageInfo.Name = "_lblPageInfo";
            _lblPageInfo.Size = new Size(96, 15);
            _lblPageInfo.TabIndex = 3;
            _lblPageInfo.Text = "Showing 0-0 of 0";
            _lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _btnPrevious
            // 
            _btnPrevious.Anchor = AnchorStyles.Right;
            _btnPrevious.BackColor = Color.White;
            _btnPrevious.Cursor = Cursors.Hand;
            _btnPrevious.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            _btnPrevious.FlatStyle = FlatStyle.Flat;
            _btnPrevious.Font = new Font("Segoe UI", 9F);
            _btnPrevious.ForeColor = Color.FromArgb(100, 116, 139);
            _btnPrevious.Location = new Point(530, 9);
            _btnPrevious.Name = "_btnPrevious";
            _btnPrevious.Size = new Size(80, 32);
            _btnPrevious.TabIndex = 2;
            _btnPrevious.Text = "◄ Previous";
            _btnPrevious.UseVisualStyleBackColor = false;
            _btnPrevious.Click += BtnPrevious_Click;
            // 
            // _cmbPageSize
            // 
            _cmbPageSize.Anchor = AnchorStyles.Left;
            _cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbPageSize.FlatStyle = FlatStyle.Flat;
            _cmbPageSize.Font = new Font("Segoe UI", 9F);
            _cmbPageSize.FormattingEnabled = true;
            _cmbPageSize.Items.AddRange(new object[] { 25, 50, 100 });
            _cmbPageSize.Location = new Point(60, 13);
            _cmbPageSize.Name = "_cmbPageSize";
            _cmbPageSize.Size = new Size(60, 23);
            _cmbPageSize.TabIndex = 1;
            _cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;
            // 
            // _lblPageSizeLabel
            // 
            _lblPageSizeLabel.Anchor = AnchorStyles.Left;
            _lblPageSizeLabel.AutoSize = true;
            _lblPageSizeLabel.Font = new Font("Segoe UI", 9F);
            _lblPageSizeLabel.ForeColor = Color.FromArgb(100, 116, 139);
            _lblPageSizeLabel.Location = new Point(15, 17);
            _lblPageSizeLabel.Name = "_lblPageSizeLabel";
            _lblPageSizeLabel.Size = new Size(39, 15);
            _lblPageSizeLabel.TabIndex = 0;
            _lblPageSizeLabel.Text = "Show:";
            // 
            // ModernDataGridView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(_dataGridView);
            Controls.Add(_paginationPanel);
            Name = "ModernDataGridView";
            Size = new Size(800, 500);
            Load += ModernDataGridView_Load;
            ((System.ComponentModel.ISupportInitialize)_dataGridView).EndInit();
            _paginationPanel.ResumeLayout(false);
            _paginationPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
