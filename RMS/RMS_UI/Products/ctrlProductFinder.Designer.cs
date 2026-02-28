namespace RMS_UI.Products
{
    partial class ctrlProductFinder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields

        // ─── Card container ────────────────────────────────────────────────
        private System.Windows.Forms.Panel                  _pnlCard;

        // ─── Section header ────────────────────────────────────────────────
        private System.Windows.Forms.Label                  _lblSectionTitle;
        private System.Windows.Forms.Panel                  _pnlSeparator;

        // ─── Row 1: Name + Unit ────────────────────────────────────────────
        private System.Windows.Forms.Label                  _lblSearchByName;
        private RMS_UI.Controls.DebouncedComboBox           _cmbSearchByName;
        private System.Windows.Forms.Label                  _lblUnits;
        private System.Windows.Forms.ComboBox               _cmbUnits;

        // ─── Row 2: Barcode ────────────────────────────────────────────────
        private System.Windows.Forms.Label                  _lblSearchByBarcode;
        private RMS_UI.Controls.DebouncedComboBox           _cmbSearchByBarcode;
        // ─── Browse button ─────────────────────────────────────────────
        private System.Windows.Forms.Button                 _btnBrowseProducts;
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
            _pnlCard = new Panel();
            _lblSectionTitle = new Label();
            _pnlSeparator = new Panel();
            _lblSearchByName = new Label();
            _cmbSearchByName = new Controls.DebouncedComboBox();
            _lblUnits = new Label();
            _cmbUnits = new ComboBox();
            _lblSearchByBarcode = new Label();
            _cmbSearchByBarcode = new Controls.DebouncedComboBox();
            _btnBrowseProducts = new Button();
            _pnlCard.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlCard
            // 
            _pnlCard.BackColor = Color.White;
            _pnlCard.Controls.Add(_lblSectionTitle);
            _pnlCard.Controls.Add(_pnlSeparator);
            _pnlCard.Controls.Add(_lblSearchByName);
            _pnlCard.Controls.Add(_cmbSearchByName);
            _pnlCard.Controls.Add(_lblUnits);
            _pnlCard.Controls.Add(_cmbUnits);
            _pnlCard.Controls.Add(_lblSearchByBarcode);
            _pnlCard.Controls.Add(_cmbSearchByBarcode);
            _pnlCard.Controls.Add(_btnBrowseProducts);
            _pnlCard.Dock = DockStyle.Fill;
            _pnlCard.Location = new Point(0, 0);
            _pnlCard.Name = "_pnlCard";
            _pnlCard.Padding = new Padding(20);
            _pnlCard.Size = new Size(520, 180);
            _pnlCard.TabIndex = 0;
            // 
            // _lblSectionTitle
            // 
            _lblSectionTitle.AutoSize = true;
            _lblSectionTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSectionTitle.ForeColor = Color.FromArgb(59, 130, 246);
            _lblSectionTitle.Location = new Point(20, 10);
            _lblSectionTitle.Name = "_lblSectionTitle";
            _lblSectionTitle.Size = new Size(138, 19);
            _lblSectionTitle.TabIndex = 0;
            _lblSectionTitle.Text = "🔍  Product Search";
            // 
            // _pnlSeparator
            // 
            _pnlSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlSeparator.BackColor = Color.FromArgb(226, 232, 240);
            _pnlSeparator.Location = new Point(20, 34);
            _pnlSeparator.Name = "_pnlSeparator";
            _pnlSeparator.Size = new Size(480, 1);
            _pnlSeparator.TabIndex = 1;
            // 
            // _lblSearchByName
            // 
            _lblSearchByName.AutoSize = true;
            _lblSearchByName.Font = new Font("Segoe UI", 8.5F);
            _lblSearchByName.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSearchByName.Location = new Point(20, 44);
            _lblSearchByName.Name = "_lblSearchByName";
            _lblSearchByName.Size = new Size(88, 15);
            _lblSearchByName.TabIndex = 2;
            _lblSearchByName.Text = "Product Name";
            // 
            // _cmbSearchByName
            // 
            _cmbSearchByName.DebounceInterval = 400;
            _cmbSearchByName.Font = new Font("Segoe UI", 10F);
            _cmbSearchByName.Location = new Point(20, 62);
            _cmbSearchByName.Name = "_cmbSearchByName";
            _cmbSearchByName.Size = new Size(260, 25);
            _cmbSearchByName.TabIndex = 3;
            // 
            // _lblUnits
            // 
            _lblUnits.AutoSize = true;
            _lblUnits.Font = new Font("Segoe UI", 8.5F);
            _lblUnits.ForeColor = Color.FromArgb(100, 116, 139);
            _lblUnits.Location = new Point(300, 44);
            _lblUnits.Name = "_lblUnits";
            _lblUnits.Size = new Size(29, 15);
            _lblUnits.TabIndex = 4;
            _lblUnits.Text = "Unit";
            // 
            // _cmbUnits
            // 
            _cmbUnits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _cmbUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbUnits.Font = new Font("Segoe UI", 10F);
            _cmbUnits.Location = new Point(300, 62);
            _cmbUnits.Name = "_cmbUnits";
            _cmbUnits.Size = new Size(200, 25);
            _cmbUnits.TabIndex = 5;
            // 
            // _lblSearchByBarcode
            // 
            _lblSearchByBarcode.AutoSize = true;
            _lblSearchByBarcode.Font = new Font("Segoe UI", 8.5F);
            _lblSearchByBarcode.ForeColor = Color.FromArgb(100, 116, 139);
            _lblSearchByBarcode.Location = new Point(20, 100);
            _lblSearchByBarcode.Name = "_lblSearchByBarcode";
            _lblSearchByBarcode.Size = new Size(51, 15);
            _lblSearchByBarcode.TabIndex = 6;
            _lblSearchByBarcode.Text = "Barcode";
            // 
            // _cmbSearchByBarcode
            // 
            _cmbSearchByBarcode.DebounceInterval = 400;
            _cmbSearchByBarcode.Font = new Font("Segoe UI", 10F);
            _cmbSearchByBarcode.Location = new Point(20, 118);
            _cmbSearchByBarcode.Name = "_cmbSearchByBarcode";
            _cmbSearchByBarcode.Size = new Size(260, 25);
            _cmbSearchByBarcode.TabIndex = 7;
            // 
            // _btnBrowseProducts
            // 
            _btnBrowseProducts.BackColor = Color.FromArgb(59, 130, 246);
            _btnBrowseProducts.Cursor = Cursors.Hand;
            _btnBrowseProducts.FlatAppearance.BorderSize = 0;
            _btnBrowseProducts.FlatStyle = FlatStyle.Flat;
            _btnBrowseProducts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnBrowseProducts.ForeColor = Color.White;
            _btnBrowseProducts.Location = new Point(300, 115);
            _btnBrowseProducts.Name = "_btnBrowseProducts";
            _btnBrowseProducts.Size = new Size(200, 30);
            _btnBrowseProducts.TabIndex = 8;
            _btnBrowseProducts.Text = "📋 Browse Products";
            _btnBrowseProducts.UseVisualStyleBackColor = false;
            _btnBrowseProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _btnBrowseProducts.Click += _btnBrowseProducts_Click;
            // 
            // ctrlProductFinder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(_pnlCard);
            Name = "ctrlProductFinder";
            Size = new Size(520, 180);
            _pnlCard.ResumeLayout(false);
            _pnlCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
