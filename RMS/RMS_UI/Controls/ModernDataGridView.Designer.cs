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
            this.components = new System.ComponentModel.Container();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this._paginationPanel = new System.Windows.Forms.Panel();
            this._lblPageSizeLabel = new System.Windows.Forms.Label();
            this._cmbPageSize = new System.Windows.Forms.ComboBox();
            this._btnPrevious = new System.Windows.Forms.Button();
            this._lblPageInfo = new System.Windows.Forms.Label();
            this._btnNext = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            this._paginationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.AllowUserToOrderColumns = false;
            this._dataGridView.AllowUserToResizeRows = false;
            this._dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dataGridView.BackgroundColor = System.Drawing.Color.White;
            this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this._dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this._dataGridView.ColumnHeadersHeight = 45;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGridView.EnableHeadersVisualStyles = false;
            this._dataGridView.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._dataGridView.Location = new System.Drawing.Point(0, 0);
            this._dataGridView.MultiSelect = true;
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.ReadOnly = true;
            this._dataGridView.RowHeadersVisible = false;
            this._dataGridView.RowTemplate.Height = 40;
            this._dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dataGridView.Size = new System.Drawing.Size(800, 450);
            this._dataGridView.TabIndex = 0;
            this._dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellDoubleClick);
            this._dataGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellMouseEnter);
            this._dataGridView.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellMouseLeave);
            this._dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridView_CellPainting);
            this._dataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            // 
            // _paginationPanel
            // 
            this._paginationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this._paginationPanel.Controls.Add(this._btnNext);
            this._paginationPanel.Controls.Add(this._lblPageInfo);
            this._paginationPanel.Controls.Add(this._btnPrevious);
            this._paginationPanel.Controls.Add(this._cmbPageSize);
            this._paginationPanel.Controls.Add(this._lblPageSizeLabel);
            this._paginationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._paginationPanel.Location = new System.Drawing.Point(0, 450);
            this._paginationPanel.Name = "_paginationPanel";
            this._paginationPanel.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this._paginationPanel.Size = new System.Drawing.Size(800, 50);
            this._paginationPanel.TabIndex = 1;
            this._paginationPanel.Resize += new System.EventHandler(this.PaginationPanel_Resize);
            // 
            // _lblPageSizeLabel
            // 
            this._lblPageSizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblPageSizeLabel.AutoSize = true;
            this._lblPageSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblPageSizeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._lblPageSizeLabel.Location = new System.Drawing.Point(15, 17);
            this._lblPageSizeLabel.Name = "_lblPageSizeLabel";
            this._lblPageSizeLabel.Size = new System.Drawing.Size(39, 15);
            this._lblPageSizeLabel.TabIndex = 0;
            this._lblPageSizeLabel.Text = "Show:";
            // 
            // _cmbPageSize
            // 
            this._cmbPageSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbPageSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cmbPageSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._cmbPageSize.FormattingEnabled = true;
            this._cmbPageSize.Items.AddRange(new object[] { 25, 50, 100 });
            this._cmbPageSize.Location = new System.Drawing.Point(60, 13);
            this._cmbPageSize.Name = "_cmbPageSize";
            this._cmbPageSize.Size = new System.Drawing.Size(60, 23);
            this._cmbPageSize.TabIndex = 1;
            this._cmbPageSize.SelectedIndexChanged += new System.EventHandler(this.CmbPageSize_SelectedIndexChanged);
            // 
            // _btnPrevious
            // 
            this._btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnPrevious.BackColor = System.Drawing.Color.White;
            this._btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this._btnPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this._btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnPrevious.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this._btnPrevious.Location = new System.Drawing.Point(530, 9);
            this._btnPrevious.Name = "_btnPrevious";
            this._btnPrevious.Size = new System.Drawing.Size(80, 32);
            this._btnPrevious.TabIndex = 2;
            this._btnPrevious.Text = "◄ Previous";
            this._btnPrevious.UseVisualStyleBackColor = false;
            this._btnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // _lblPageInfo
            // 
            this._lblPageInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._lblPageInfo.AutoSize = true;
            this._lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblPageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._lblPageInfo.Location = new System.Drawing.Point(340, 17);
            this._lblPageInfo.Name = "_lblPageInfo";
            this._lblPageInfo.Size = new System.Drawing.Size(100, 15);
            this._lblPageInfo.TabIndex = 3;
            this._lblPageInfo.Text = "Showing 0-0 of 0";
            this._lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _btnNext
            // 
            this._btnNext.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnNext.BackColor = System.Drawing.Color.White;
            this._btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this._btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this._btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this._btnNext.Location = new System.Drawing.Point(705, 9);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(80, 32);
            this._btnNext.TabIndex = 4;
            this._btnNext.Text = "Next ►";
            this._btnNext.UseVisualStyleBackColor = false;
            this._btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // ModernDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._dataGridView);
            this.Controls.Add(this._paginationPanel);
            this.Name = "ModernDataGridView";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.ModernDataGridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            this._paginationPanel.ResumeLayout(false);
            this._paginationPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
