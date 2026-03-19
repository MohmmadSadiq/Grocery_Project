using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.POS.Cart
{
    /// <summary>
    /// Reusable sale-row control representing one product line in the cart.
    /// </summary>
    public partial class ctrlProductSaleRow : UserControl
    {
        private const int BorderRadius = 10;

        private decimal _quantity = 1m;
        private decimal _unitPrice;
        private bool _suppressQuantityChanged;
        private bool _isHovered;
        private clsProductSale? _saleItem;

        public ctrlProductSaleRow()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            _pnlRoot.MouseEnter += HandleMouseEnter;
            _pnlRoot.MouseLeave += HandleMouseLeave;
            _lblProductName.MouseEnter += HandleMouseEnter;
            _lblProductName.MouseLeave += HandleMouseLeave;
            _lblUnitPriceLabel.MouseEnter += HandleMouseEnter;
            _lblUnitPriceLabel.MouseLeave += HandleMouseLeave;
            _lblUnitPriceValue.MouseEnter += HandleMouseEnter;
            _lblUnitPriceValue.MouseLeave += HandleMouseLeave;
            _lblSubtotalLabel.MouseEnter += HandleMouseEnter;
            _lblSubtotalLabel.MouseLeave += HandleMouseLeave;
            _lblSubtotalValue.MouseEnter += HandleMouseEnter;
            _lblSubtotalValue.MouseLeave += HandleMouseLeave;

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();
            SyncUiFromState();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public clsProductSale? SaleItem
        {
            get => _saleItem;
            set
            {
                _saleItem = value;
                if (_saleItem == null)
                    return;

                Quantity = _saleItem.Quantity;
                UnitPrice = _saleItem.UnitPrice;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string ProductName
        {
            get => _lblProductName.Text;
            set => _lblProductName.Text = value ?? string.Empty;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                decimal normalized = NormalizeQuantity(value);
                if (_quantity == normalized)
                    return;

                _quantity = normalized;
                SyncUiFromState();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice == value)
                    return;

                _unitPrice = value;
                SyncUiFromState();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Subtotal => Quantity * UnitPrice;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowRemoveButton
        {
            get => _btnRemove.Visible;
            set => _btnRemove.Visible = value;
        }

        public event EventHandler<ProductSaleRowQuantityChangedEventArgs>? QuantityChanged;
        public event EventHandler<ProductSaleRowRemoveRequestedEventArgs>? RemoveRequested;
        public event EventHandler? SubtotalChanged;

        private void _nudQuantity_ValueChanged(object? sender, EventArgs e)
        {
            if (_suppressQuantityChanged)
                return;

            decimal oldQuantity = _quantity;
            decimal newQuantity = NormalizeQuantity(_nudQuantity.Value);
            if (oldQuantity == newQuantity)
                return;

            _quantity = newQuantity;
            if (_saleItem != null)
                _saleItem.Quantity = newQuantity;

            if (newQuantity == 0m)
            {
                RemoveRequested?.Invoke(this, new ProductSaleRowRemoveRequestedEventArgs(_saleItem));
                return;
            }

            UpdateSubtotalText();
            QuantityChanged?.Invoke(this,
                new ProductSaleRowQuantityChangedEventArgs(_saleItem, oldQuantity, newQuantity, Subtotal));
            SubtotalChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _btnRemove_Click(object? sender, EventArgs e)
        {
            RemoveRequested?.Invoke(this, new ProductSaleRowRemoveRequestedEventArgs(_saleItem));
        }

        private void SyncUiFromState()
        {
            _suppressQuantityChanged = true;
            try
            {
                decimal minimum = _nudQuantity.Minimum;
                decimal maximum = _nudQuantity.Maximum;
                decimal safeQuantity = Math.Max(minimum, Math.Min(maximum, _quantity));
                _nudQuantity.Value = safeQuantity;
                _quantity = safeQuantity;
            }
            finally
            {
                _suppressQuantityChanged = false;
            }

            _lblUnitPriceValue.Text = UnitPrice.ToString("N2");
            UpdateSubtotalText();
        }

        private void UpdateSubtotalText()
        {
            _lblSubtotalValue.Text = Subtotal.ToString("N2");
        }

        private decimal NormalizeQuantity(decimal value)
        {
            if (value < _nudQuantity.Minimum)
                return _nudQuantity.Minimum;

            if (value > _nudQuantity.Maximum)
                return _nudQuantity.Maximum;

            return value;
        }

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            _pnlRoot.BackColor = _isHovered ? c.ButtonHover : c.ContentBackground;
            _lblProductName.ForeColor = c.TitleText;

            _lblUnitPriceLabel.ForeColor = c.SecondaryText;
            _lblUnitPriceValue.ForeColor = c.PrimaryText;
            _lblSubtotalLabel.ForeColor = c.SecondaryText;
            _lblSubtotalValue.ForeColor = c.Primary;

            _nudQuantity.BackColor = c.ContentBackground;
            _nudQuantity.ForeColor = c.PrimaryText;

            _btnRemove.BackColor = Color.FromArgb(254, 226, 226);
            _btnRemove.ForeColor = Color.FromArgb(220, 38, 38);
            _btnRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);

            Invalidate();
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ApplyTheme));
                return;
            }

            ApplyTheme();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var c = ThemeManager.Colors;

            using var borderPen = new Pen(_isHovered ? c.BorderAccent : c.BorderColor, _isHovered ? 1.5f : 1f);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using var path = RoundedRectangle(rect, BorderRadius);
            e.Graphics.DrawPath(borderPen, path);

            base.OnPaint(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnHandleDestroyed(e);
        }

        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void HandleMouseEnter(object? sender, EventArgs e)
        {
            if (_isHovered)
                return;

            _isHovered = true;
            ApplyTheme();
        }

        private void HandleMouseLeave(object? sender, EventArgs e)
        {
            Point pt = PointToClient(Cursor.Position);
            if (ClientRectangle.Contains(pt))
                return;

            _isHovered = false;
            ApplyTheme();
        }
    }

    public sealed class ProductSaleRowQuantityChangedEventArgs : EventArgs
    {
        public clsProductSale? SaleItem { get; }
        public decimal OldQuantity { get; }
        public decimal NewQuantity { get; }
        public decimal Subtotal { get; }

        public ProductSaleRowQuantityChangedEventArgs(clsProductSale? saleItem, decimal oldQuantity, decimal newQuantity, decimal subtotal)
        {
            SaleItem = saleItem;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            Subtotal = subtotal;
        }
    }

    public sealed class ProductSaleRowRemoveRequestedEventArgs : EventArgs
    {
        public clsProductSale? SaleItem { get; }

        public ProductSaleRowRemoveRequestedEventArgs(clsProductSale? saleItem)
        {
            SaleItem = saleItem;
        }
    }
}
