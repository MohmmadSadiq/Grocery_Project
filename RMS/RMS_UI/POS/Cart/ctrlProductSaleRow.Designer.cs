using System.Drawing;
using System.Windows.Forms;

namespace RMS_UI.POS.Cart
{
    partial class ctrlProductSaleRow
    {

        #region Component Designer generated code

        private void InitializeComponent()
        {
            _pnlRoot = new Panel();
            _tblLayout = new TableLayoutPanel();
            _lblProductName = new Label();
            _lblUnitPriceLabel = new Label();
            _lblUnitPriceValue = new Label();
            _nudQuantity = new NumericUpDown();
            _lblSubtotalLabel = new Label();
            _lblSubtotalValue = new Label();
            _btnRemove = new Button();
            _pnlRoot.SuspendLayout();
            _tblLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_nudQuantity).BeginInit();
            SuspendLayout();
            // 
            // _pnlRoot
            // 
            _pnlRoot.Controls.Add(_tblLayout);
            _pnlRoot.Dock = DockStyle.Fill;
            _pnlRoot.Location = new Point(0, 0);
            _pnlRoot.Margin = new Padding(0);
            _pnlRoot.Name = "_pnlRoot";
            _pnlRoot.Padding = new Padding(12, 8, 12, 8);
            _pnlRoot.Size = new Size(640, 56);
            _pnlRoot.TabIndex = 0;
            // 
            // _tblLayout
            // 
            _tblLayout.ColumnCount = 5;
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 68F));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74F));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74F));
            _tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66F));
            _tblLayout.Controls.Add(_lblProductName, 0, 0);
            _tblLayout.Controls.Add(_lblUnitPriceLabel, 1, 0);
            _tblLayout.Controls.Add(_lblUnitPriceValue, 1, 1);
            _tblLayout.Controls.Add(_nudQuantity, 2, 1);
            _tblLayout.Controls.Add(_lblSubtotalLabel, 3, 0);
            _tblLayout.Controls.Add(_lblSubtotalValue, 3, 1);
            _tblLayout.Controls.Add(_btnRemove, 4, 0);
            _tblLayout.Dock = DockStyle.Fill;
            _tblLayout.Location = new Point(12, 8);
            _tblLayout.Margin = new Padding(0);
            _tblLayout.Name = "_tblLayout";
            _tblLayout.RowCount = 2;
            _tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tblLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tblLayout.Size = new Size(616, 40);
            _tblLayout.TabIndex = 0;
            // 
            // _lblProductName
            // 
            _lblProductName.AutoEllipsis = true;
            _lblProductName.Dock = DockStyle.Fill;
            _lblProductName.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            _lblProductName.Location = new Point(0, 0);
            _lblProductName.Margin = new Padding(0, 0, 6, 0);
            _lblProductName.Name = "_lblProductName";
            _tblLayout.SetRowSpan(_lblProductName, 2);
            _lblProductName.Size = new Size(328, 40);
            _lblProductName.TabIndex = 0;
            _lblProductName.Text = "Product Name";
            _lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblUnitPriceLabel
            // 
            _lblUnitPriceLabel.Dock = DockStyle.Fill;
            _lblUnitPriceLabel.Font = new Font("Segoe UI", 8F);
            _lblUnitPriceLabel.Location = new Point(334, 0);
            _lblUnitPriceLabel.Margin = new Padding(0, 0, 2, 0);
            _lblUnitPriceLabel.Name = "_lblUnitPriceLabel";
            _lblUnitPriceLabel.Size = new Size(66, 20);
            _lblUnitPriceLabel.TabIndex = 1;
            _lblUnitPriceLabel.Text = "Unit Price";
            _lblUnitPriceLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // _lblUnitPriceValue
            // 
            _lblUnitPriceValue.Dock = DockStyle.Fill;
            _lblUnitPriceValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _lblUnitPriceValue.Location = new Point(334, 20);
            _lblUnitPriceValue.Margin = new Padding(0, 0, 2, 0);
            _lblUnitPriceValue.Name = "_lblUnitPriceValue";
            _lblUnitPriceValue.Size = new Size(66, 20);
            _lblUnitPriceValue.TabIndex = 2;
            _lblUnitPriceValue.Text = "0.00";
            // 
            // _nudQuantity
            // 
            _nudQuantity.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _nudQuantity.DecimalPlaces = 2;
            _nudQuantity.Font = new Font("Segoe UI", 9F);
            _nudQuantity.Location = new Point(402, 20);
            _nudQuantity.Margin = new Padding(0);
            _nudQuantity.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            _nudQuantity.Name = "_nudQuantity";
            _nudQuantity.Size = new Size(74, 23);
            _nudQuantity.TabIndex = 5;
            _nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            _nudQuantity.ValueChanged += _nudQuantity_ValueChanged;
            // 
            // _lblSubtotalLabel
            // 
            _lblSubtotalLabel.Dock = DockStyle.Fill;
            _lblSubtotalLabel.Font = new Font("Segoe UI", 8F);
            _lblSubtotalLabel.Location = new Point(476, 0);
            _lblSubtotalLabel.Margin = new Padding(0, 0, 2, 0);
            _lblSubtotalLabel.Name = "_lblSubtotalLabel";
            _lblSubtotalLabel.Size = new Size(72, 20);
            _lblSubtotalLabel.TabIndex = 3;
            _lblSubtotalLabel.Text = "Subtotal";
            _lblSubtotalLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // _lblSubtotalValue
            // 
            _lblSubtotalValue.Dock = DockStyle.Fill;
            _lblSubtotalValue.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            _lblSubtotalValue.Location = new Point(476, 20);
            _lblSubtotalValue.Margin = new Padding(0, 0, 2, 0);
            _lblSubtotalValue.Name = "_lblSubtotalValue";
            _lblSubtotalValue.Size = new Size(72, 20);
            _lblSubtotalValue.TabIndex = 4;
            _lblSubtotalValue.Text = "0.00";
            // 
            // _btnRemove
            // 
            _btnRemove.Dock = DockStyle.Fill;
            _btnRemove.FlatAppearance.BorderSize = 0;
            _btnRemove.FlatStyle = FlatStyle.Flat;
            _btnRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnRemove.Location = new Point(550, 0);
            _btnRemove.Margin = new Padding(0);
            _btnRemove.Name = "_btnRemove";
            _tblLayout.SetRowSpan(_btnRemove, 2);
            _btnRemove.Size = new Size(66, 40);
            _btnRemove.TabIndex = 6;
            _btnRemove.Text = "Remove";
            _btnRemove.UseVisualStyleBackColor = true;
            _btnRemove.Click += _btnRemove_Click;
            // 
            // ctrlProductSaleRow
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Transparent;
            Controls.Add(_pnlRoot);
            Margin = new Padding(0, 0, 0, 8);
            Name = "ctrlProductSaleRow";
            Size = new Size(640, 56);
            _pnlRoot.ResumeLayout(false);
            _tblLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_nudQuantity).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlRoot;
        private TableLayoutPanel _tblLayout;
        private Label _lblProductName;
        private Label _lblUnitPriceLabel;
        private Label _lblUnitPriceValue;
        private Label _lblSubtotalLabel;
        private Label _lblSubtotalValue;
        private NumericUpDown _nudQuantity;
        private Button _btnRemove;
    }
}
