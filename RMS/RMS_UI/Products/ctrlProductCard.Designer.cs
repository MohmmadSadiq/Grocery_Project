namespace RMS_UI.Products
{
    partial class ctrlProductCard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            _pnlContainer = new Panel();
            _pnlImageSection = new Panel();
            _picProductImage = new PictureBox();
            _pnlInfoSection = new Panel();
            _lblProductName = new Label();
            _lblBrandCategory = new Label();
            _lblUnitName = new Label();
            _lblSalePrice = new Label();
            _lblBarcode = new Label();

            _pnlContainer.SuspendLayout();
            _pnlImageSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picProductImage).BeginInit();
            _pnlInfoSection.SuspendLayout();
            SuspendLayout();

            // ─── _pnlContainer ──────────────────────────────────────────────
            _pnlContainer.Controls.Add(_pnlInfoSection);
            _pnlContainer.Controls.Add(_pnlImageSection);
            _pnlContainer.Dock = DockStyle.Fill;
            _pnlContainer.Location = new Point(0, 0);
            _pnlContainer.Name = "_pnlContainer";
            _pnlContainer.Padding = new Padding(10, 10, 10, 8);
            _pnlContainer.Size = new Size(200, 240);
            _pnlContainer.TabIndex = 0;
            _pnlContainer.Cursor = Cursors.Hand;
            _pnlContainer.Click += OnCardClicked;
            _pnlContainer.MouseEnter += OnCardMouseEnter;
            _pnlContainer.MouseLeave += OnCardMouseLeave;

            // ─── _pnlImageSection ───────────────────────────────────────────
            _pnlImageSection.Controls.Add(_picProductImage);
            _pnlImageSection.Dock = DockStyle.Top;
            _pnlImageSection.Location = new Point(10, 10);
            _pnlImageSection.Name = "_pnlImageSection";
            _pnlImageSection.Padding = new Padding(10, 5, 10, 5);
            _pnlImageSection.Size = new Size(180, 110);
            _pnlImageSection.TabIndex = 0;
            _pnlImageSection.Cursor = Cursors.Hand;
            _pnlImageSection.Click += OnCardClicked;
            _pnlImageSection.MouseEnter += OnCardMouseEnter;
            _pnlImageSection.MouseLeave += OnCardMouseLeave;

            // ─── _picProductImage ───────────────────────────────────────────
            _picProductImage.Dock = DockStyle.Fill;
            _picProductImage.Location = new Point(10, 5);
            _picProductImage.Name = "_picProductImage";
            _picProductImage.Size = new Size(160, 100);
            _picProductImage.SizeMode = PictureBoxSizeMode.Zoom;
            _picProductImage.TabIndex = 0;
            _picProductImage.TabStop = false;
            _picProductImage.Cursor = Cursors.Hand;
            _picProductImage.Click += OnCardClicked;
            _picProductImage.MouseEnter += OnCardMouseEnter;
            _picProductImage.MouseLeave += OnCardMouseLeave;

            // ─── _pnlInfoSection ────────────────────────────────────────────
            _pnlInfoSection.Controls.Add(_lblBarcode);
            _pnlInfoSection.Controls.Add(_lblSalePrice);
            _pnlInfoSection.Controls.Add(_lblUnitName);
            _pnlInfoSection.Controls.Add(_lblBrandCategory);
            _pnlInfoSection.Controls.Add(_lblProductName);
            _pnlInfoSection.Dock = DockStyle.Fill;
            _pnlInfoSection.Location = new Point(10, 120);
            _pnlInfoSection.Name = "_pnlInfoSection";
            _pnlInfoSection.Padding = new Padding(6, 4, 6, 2);
            _pnlInfoSection.Size = new Size(180, 112);
            _pnlInfoSection.TabIndex = 1;
            _pnlInfoSection.Cursor = Cursors.Hand;
            _pnlInfoSection.Click += OnCardClicked;
            _pnlInfoSection.MouseEnter += OnCardMouseEnter;
            _pnlInfoSection.MouseLeave += OnCardMouseLeave;

            // ─── _lblProductName ────────────────────────────────────────────
            _lblProductName.Dock = DockStyle.Top;
            _lblProductName.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            _lblProductName.Location = new Point(6, 4);
            _lblProductName.Name = "_lblProductName";
            _lblProductName.Size = new Size(168, 22);
            _lblProductName.TabIndex = 0;
            _lblProductName.Text = "Product Name";
            _lblProductName.AutoEllipsis = true;
            _lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            _lblProductName.Cursor = Cursors.Hand;
            _lblProductName.Click += OnCardClicked;
            _lblProductName.MouseEnter += OnCardMouseEnter;
            _lblProductName.MouseLeave += OnCardMouseLeave;

            // ─── _lblBrandCategory ──────────────────────────────────────────
            _lblBrandCategory.Dock = DockStyle.Top;
            _lblBrandCategory.Font = new Font("Segoe UI", 7.5F);
            _lblBrandCategory.Location = new Point(6, 26);
            _lblBrandCategory.Name = "_lblBrandCategory";
            _lblBrandCategory.Size = new Size(168, 16);
            _lblBrandCategory.TabIndex = 1;
            _lblBrandCategory.Text = "Brand • Category";
            _lblBrandCategory.AutoEllipsis = true;
            _lblBrandCategory.TextAlign = ContentAlignment.MiddleLeft;
            _lblBrandCategory.Cursor = Cursors.Hand;
            _lblBrandCategory.Click += OnCardClicked;
            _lblBrandCategory.MouseEnter += OnCardMouseEnter;
            _lblBrandCategory.MouseLeave += OnCardMouseLeave;

            // ─── _lblUnitName ───────────────────────────────────────────────
            _lblUnitName.Dock = DockStyle.Top;
            _lblUnitName.Font = new Font("Segoe UI", 8F);
            _lblUnitName.Location = new Point(6, 42);
            _lblUnitName.Name = "_lblUnitName";
            _lblUnitName.Size = new Size(168, 18);
            _lblUnitName.TabIndex = 2;
            _lblUnitName.Text = "Unit: Piece";
            _lblUnitName.AutoEllipsis = true;
            _lblUnitName.TextAlign = ContentAlignment.MiddleLeft;
            _lblUnitName.Cursor = Cursors.Hand;
            _lblUnitName.Click += OnCardClicked;
            _lblUnitName.MouseEnter += OnCardMouseEnter;
            _lblUnitName.MouseLeave += OnCardMouseLeave;

            // ─── _lblSalePrice ──────────────────────────────────────────────
            _lblSalePrice.Dock = DockStyle.Top;
            _lblSalePrice.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _lblSalePrice.Location = new Point(6, 60);
            _lblSalePrice.Name = "_lblSalePrice";
            _lblSalePrice.Size = new Size(168, 26);
            _lblSalePrice.TabIndex = 3;
            _lblSalePrice.Text = "0.00";
            _lblSalePrice.TextAlign = ContentAlignment.MiddleLeft;
            _lblSalePrice.Cursor = Cursors.Hand;
            _lblSalePrice.Click += OnCardClicked;
            _lblSalePrice.MouseEnter += OnCardMouseEnter;
            _lblSalePrice.MouseLeave += OnCardMouseLeave;

            // ─── _lblBarcode ────────────────────────────────────────────────
            _lblBarcode.Dock = DockStyle.Top;
            _lblBarcode.Font = new Font("Segoe UI", 7F);
            _lblBarcode.Location = new Point(6, 86);
            _lblBarcode.Name = "_lblBarcode";
            _lblBarcode.Size = new Size(168, 14);
            _lblBarcode.TabIndex = 4;
            _lblBarcode.Text = "";
            _lblBarcode.AutoEllipsis = true;
            _lblBarcode.TextAlign = ContentAlignment.MiddleLeft;
            _lblBarcode.Cursor = Cursors.Hand;
            _lblBarcode.Click += OnCardClicked;
            _lblBarcode.MouseEnter += OnCardMouseEnter;
            _lblBarcode.MouseLeave += OnCardMouseLeave;

            // ─── ctrlProductCard ────────────────────────────────────────────
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(_pnlContainer);
            Margin = new Padding(8);
            Name = "ctrlProductCard";
            Size = new Size(200, 240);
            BackColor = Color.Transparent;

            _pnlContainer.ResumeLayout(false);
            _pnlImageSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picProductImage).EndInit();
            _pnlInfoSection.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlContainer;
        private Panel _pnlImageSection;
        private PictureBox _picProductImage;
        private Panel _pnlInfoSection;
        private Label _lblProductName;
        private Label _lblBrandCategory;
        private Label _lblUnitName;
        private Label _lblSalePrice;
        private Label _lblBarcode;
    }
}
