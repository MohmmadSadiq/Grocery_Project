using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Products
{
    /// <summary>
    /// A compact product-unit card for POS / catalog grids.
    /// Displays: product image, name, brand•category, unit, sale price, barcode.
    /// Fires <see cref="ProductUnitClicked"/> when clicked so the POS page can add to cart.
    /// </summary>
    public partial class ctrlProductCard : UserControl
    {
        #region Constants
        private const int BorderRadius = 12;
        private const int ShadowSize = 8;
        #endregion

        #region Fields
        private clsProductUnit? _productUnit;
        private bool _isHovered;
        #endregion

        #region Events
        /// <summary>
        /// Raised when the user clicks anywhere on the card.
        /// The argument carries the <see cref="clsProductUnit"/> displayed by this card.
        /// </summary>
        public event EventHandler<clsProductUnit>? ProductUnitClicked;
        #endregion

        #region Constructor
        public ctrlProductCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            // Subscribe to theme changes
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }
        #endregion

        #region Public API

        /// <summary>Returns the currently loaded product unit, or null.</summary>
        public clsProductUnit? ProductUnit => _productUnit;

        /// <summary>
        /// Populates the card with data from a <see cref="clsProductUnit"/> instance.
        /// Lazy-loaded properties (<c>ProductInfo</c>, <c>UnitInfo</c>) are accessed
        /// to resolve product name, image, brand, category and unit name.
        /// </summary>
        public void LoadProductUnit(clsProductUnit? productUnit)
        {
            _productUnit = productUnit;

            if (_productUnit != null)
                FillCardInfo();
            else
                Clear();
        }

        /// <summary>Resets all labels and the image to default/empty state.</summary>
        public void Clear()
        {
            _lblProductName.Text  = "Product Name";
            _lblBrandCategory.Text = "";
            _lblUnitName.Text     = "";
            _lblSalePrice.Text    = "0.00";
            _lblBarcode.Text      = "";
            _picProductImage.Image?.Dispose();
            _picProductImage.Image = null;
        }
        #endregion

        #region Private — Data Binding
        private void FillCardInfo()
        {
            if (_productUnit == null) return;

            // ── Product info (lazy-loaded) ────────────────────────────────
            var product = _productUnit.ProductInfo;

            _lblProductName.Text = product?.ProductName ?? "Unknown Product";

            // Brand • Category line
            string brandName    = GetBrandName(product);
            string categoryName = GetCategoryName(product);
            _lblBrandCategory.Text = BuildSubtitle(brandName, categoryName);

            // ── Unit info (lazy-loaded) ───────────────────────────────────
            var unit = _productUnit.UnitInfo;
            _lblUnitName.Text = unit?.UnitName ?? "";

            // ── Price ─────────────────────────────────────────────────────
            _lblSalePrice.Text = _productUnit.SalePrice.HasValue
                ? _productUnit.SalePrice.Value.ToString("N2")
                : "—";

            // ── Barcode ───────────────────────────────────────────────────
            _lblBarcode.Text = _productUnit.Barcode ?? "";

            // ── Image ─────────────────────────────────────────────────────
            LoadProductImage(product?.ImagePath);
        }

        private static string GetBrandName(clsProduct? product)
        {
            if (product?.BrandID == null) return "";
            var brand = clsBrand.Find(product.BrandID.Value);
            return brand?.BrandName ?? "";
        }

        private static string GetCategoryName(clsProduct? product)
        {
            if (product?.CategoryID == null) return "";
            var category = clsCategory.Find(product.CategoryID.Value);
            return category?.CategoryName ?? "";
        }

        /// <summary>Builds "Brand • Category", "Brand", "Category", or "" depending on available data.</summary>
        private static string BuildSubtitle(string brand, string category)
        {
            bool hasBrand = !string.IsNullOrWhiteSpace(brand);
            bool hasCat   = !string.IsNullOrWhiteSpace(category);
            if (hasBrand && hasCat) return $"{brand} • {category}";
            if (hasBrand) return brand;
            if (hasCat) return category;
            return "";
        }

        private void LoadProductImage(string? imagePath)
        {
            try
            {
                _picProductImage.Image?.Dispose();

                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    var img = ImageManager.LoadPreview(imagePath);
                    _picProductImage.Image = img;
                }
                else
                {
                    _picProductImage.Image = ImageManager.GetPlaceholderImage(100);
                }
            }
            catch
            {
                _picProductImage.Image = ImageManager.GetPlaceholderImage(100);
            }
        }
        #endregion

        #region Theme
        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            _pnlContainer.BackColor    = colors.ContentBackground;
            _pnlImageSection.BackColor = colors.ContentBackground;
            _pnlInfoSection.BackColor  = colors.ContentBackground;

            _lblProductName.ForeColor  = colors.TitleText;
            _lblBrandCategory.ForeColor = colors.SecondaryText;
            _lblUnitName.ForeColor     = colors.SecondaryText;
            _lblSalePrice.ForeColor    = colors.Primary;
            _lblBarcode.ForeColor      = colors.SecondaryText;

            Invalidate();
        }
        #endregion

        #region Hover Effects
        private void OnCardMouseEnter(object? sender, EventArgs e)
        {
            if (_isHovered) return;
            _isHovered = true;

            var colors = ThemeManager.Colors;
            _pnlContainer.BackColor    = colors.ButtonHover;
            _pnlImageSection.BackColor = colors.ButtonHover;
            _pnlInfoSection.BackColor  = colors.ButtonHover;
            Invalidate();
        }

        private void OnCardMouseLeave(object? sender, EventArgs e)
        {
            // Only un-hover when the cursor truly leaves the entire control
            Point pt = PointToClient(Cursor.Position);
            if (ClientRectangle.Contains(pt)) return;

            _isHovered = false;

            var colors = ThemeManager.Colors;
            _pnlContainer.BackColor    = colors.ContentBackground;
            _pnlImageSection.BackColor = colors.ContentBackground;
            _pnlInfoSection.BackColor  = colors.ContentBackground;
            Invalidate();
        }
        #endregion

        #region Click Handling
        private void OnCardClicked(object? sender, EventArgs e)
        {
            if (_productUnit != null)
                ProductUnitClicked?.Invoke(this, _productUnit);
        }
        #endregion

        #region Custom Paint — Shadow / Background / Border
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var colors = ThemeManager.Colors;

            // 1. Shadow
            using (var shadowBrush = new SolidBrush(colors.ShadowColor))
            {
                var shadowRect = new Rectangle(ShadowSize, ShadowSize,
                                               Width - ShadowSize * 2,
                                               Height - ShadowSize * 2);
                using var path = RoundedRectangle(shadowRect, BorderRadius);
                e.Graphics.FillPath(shadowBrush, path);
            }

            // 2. Background
            using (var bgBrush = new SolidBrush(_isHovered ? colors.ButtonHover : colors.ContentBackground))
            {
                var bgRect = new Rectangle(0, 0, Width - ShadowSize, Height - ShadowSize);
                using var path = RoundedRectangle(bgRect, BorderRadius);
                e.Graphics.FillPath(bgBrush, path);
            }

            // 3. Border
            var borderColor = _isHovered ? colors.BorderAccent : colors.BorderColor;
            float borderWidth = _isHovered ? 1.5f : 1f;
            using (var borderPen = new Pen(borderColor, borderWidth))
            {
                var borderRect = new Rectangle(0, 0,
                                               Width - ShadowSize - 1,
                                               Height - ShadowSize - 1);
                using var path = RoundedRectangle(borderRect, BorderRadius);
                e.Graphics.DrawPath(borderPen, path);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Creates a <see cref="GraphicsPath"/> for a rounded rectangle.
        /// Same helper used across all card controls in the project.
        /// </summary>
        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y + bounds.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
        #endregion

        #region Lifecycle
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }
        #endregion
    }
}
