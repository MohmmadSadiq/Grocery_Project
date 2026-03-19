using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.POS.Cart
{
    /// <summary>
    /// Container control that manages and renders multiple cart rows.
    /// </summary>
    public partial class ctrlSaleCart : UserControl
    {
        private readonly Dictionary<clsProductSale, ctrlProductSaleRow> _rowControls = new();
        private readonly List<clsProductSale> _sales = new();
        private readonly Dictionary<int, string> _productDisplayNameCache = new();

        private IProductSalePricingProvider _pricingProvider;
        private decimal _total;

        public ctrlSaleCart()
            : this(new DefaultProductSalePricingProvider())
        {
        }

        public ctrlSaleCart(IProductSalePricingProvider pricingProvider)
        {
            _pricingProvider = pricingProvider ?? throw new ArgumentNullException(nameof(pricingProvider));

            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();
            RefreshCartRows();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Total => _total;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IProductSalePricingProvider PricingProvider
        {
            get => _pricingProvider;
            set
            {
                _pricingProvider = value ?? throw new ArgumentNullException(nameof(value));
                RebuildRowsFromService();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<clsProductSale> Sales => _sales.AsReadOnly();

        public event EventHandler? SalesChanged;
        public event EventHandler<SaleCartTotalChangedEventArgs>? TotalChanged;
        public event EventHandler<SaleCartRowEventArgs>? SaleAdded;
        public event EventHandler<SaleCartRowEventArgs>? SaleRemoved;
        public event EventHandler<SaleCartRowQuantityChangedEventArgs>? SaleQuantityChanged;

        public void AddSale(clsProductSale sale)
        {
            if (sale == null)
                return;

            _sales.Add(sale);
            var row = CreateRow(sale);
            _rowControls[sale] = row;
            _pnlRowsHost.Controls.Add(row);
            FitRowsToContainerWidth();
            SaleAdded?.Invoke(this, new SaleCartRowEventArgs(sale));

            UpdateSummaryAndNotify();
        }

        public void ShowSales(IEnumerable<clsProductSale> sales)
        {
            ArgumentNullException.ThrowIfNull(sales);

            ClearRowsOnly();
            _sales.Clear();

            foreach (var sale in sales.Where(s => s != null))
            {
                _sales.Add(sale);
                var row = CreateRow(sale);
                _rowControls[sale] = row;
                _pnlRowsHost.Controls.Add(row);
            }

            FitRowsToContainerWidth();
            UpdateSummaryAndNotify();
            SalesChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool RemoveSale(clsProductSale sale)
        {
            if (sale == null)
                return false;

            bool removedFromList = _sales.Remove(sale);
            if (!removedFromList)
                return false;

            RemoveRowControl(sale);
            SaleRemoved?.Invoke(this, new SaleCartRowEventArgs(sale));

            UpdateSummaryAndNotify();
            return true;
        }

        public bool UpdateQuantity(clsProductSale sale, decimal quantity)
        {
            if (sale == null || !_sales.Contains(sale))
                return false;

            if (quantity <= 0)
                return RemoveSale(sale);

            sale.Quantity = quantity;

            if (_rowControls.TryGetValue(sale, out var row))
                row.Quantity = quantity;

            UpdateSummaryAndNotify();
            return true;
        }

        public void Clear()
        {
            _sales.Clear();
            ClearRowsOnly();
            UpdateSummaryAndNotify();
        }

        private void ClearRowsOnly()
        {
            foreach (var row in _rowControls.Values)
            {
                UnwireRowEvents(row);
                row.Dispose();
            }

            _rowControls.Clear();
            _pnlRowsHost.Controls.Clear();
        }

        private ctrlProductSaleRow CreateRow(clsProductSale sale)
        {
            var row = new ctrlProductSaleRow
            {
                SaleItem = sale,
                ProductName = ResolveProductDisplayName(sale.ProductUnitID),
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                ShowRemoveButton = true
            };

            row.QuantityChanged += OnRowQuantityChanged;
            row.RemoveRequested += OnRowRemoveRequested;
            return row;
        }

        private void OnRowQuantityChanged(object? sender, ProductSaleRowQuantityChangedEventArgs e)
        {
            if (e.SaleItem == null || !_sales.Contains(e.SaleItem))
                return;

            e.SaleItem.Quantity = e.NewQuantity;

            SaleQuantityChanged?.Invoke(this,
                new SaleCartRowQuantityChangedEventArgs(e.SaleItem, e.OldQuantity, e.NewQuantity));

            UpdateSummaryAndNotify();
        }

        private void OnRowRemoveRequested(object? sender, ProductSaleRowRemoveRequestedEventArgs e)
        {
            if (e.SaleItem == null)
                return;

            if (!_sales.Remove(e.SaleItem))
                return;

            RemoveRowControl(e.SaleItem);
            SaleRemoved?.Invoke(this, new SaleCartRowEventArgs(e.SaleItem));
            UpdateSummaryAndNotify();
        }

        private void RemoveRowControl(clsProductSale sale)
        {
            if (!_rowControls.TryGetValue(sale, out var row))
                return;

            UnwireRowEvents(row);
            _pnlRowsHost.Controls.Remove(row);
            _rowControls.Remove(sale);
            row.Dispose();
        }

        private void UnwireRowEvents(ctrlProductSaleRow row)
        {
            row.QuantityChanged -= OnRowQuantityChanged;
            row.RemoveRequested -= OnRowRemoveRequested;
        }

        private void RefreshCartRows()
        {
            ClearRowsOnly();

            foreach (var sale in _sales)
            {
                var row = CreateRow(sale);
                _rowControls[sale] = row;
                _pnlRowsHost.Controls.Add(row);
            }

            FitRowsToContainerWidth();
            UpdateSummaryAndNotify();
        }

        private void RebuildRowsFromService()
        {
            RefreshCartRows();
            SalesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSummaryAndNotify()
        {
            int lineCount = _sales.Count;
            decimal totalItemQuantity = _sales.Sum(i => i.Quantity);
            decimal oldTotal = _total;
            _total = _pricingProvider.CalculateTotal(_sales);

            _lblItemsInfo.Text = $"{lineCount} line{(lineCount == 1 ? string.Empty : "s")} · {totalItemQuantity:N2} item{(totalItemQuantity == 1 ? string.Empty : "s")}";
            _lblTotalValue.Text = _total.ToString("N2");

            if (oldTotal != _total)
            {
                TotalChanged?.Invoke(this, new SaleCartTotalChangedEventArgs(oldTotal, _total));
            }

            SalesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _btnClear_Click(object? sender, EventArgs e)
        {
            if (_rowControls.Count == 0)
                return;

            Clear();
        }

        private void _pnlRowsHost_SizeChanged(object? sender, EventArgs e)
        {
            FitRowsToContainerWidth();
        }

        private void FitRowsToContainerWidth()
        {
            int scrollbarWidth = _pnlRowsHost.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;
            int width = Math.Max(100, _pnlRowsHost.ClientSize.Width - scrollbarWidth - 4);
            foreach (Control row in _pnlRowsHost.Controls)
            {
                row.Width = width;
            }
        }

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            BackColor = c.FormBackground;
            _pnlRoot.BackColor = c.ContentBackground;
            _pnlHeader.BackColor = c.ContentBackground;
            _pnlRowsHost.BackColor = c.FormBackground;
            _pnlFooter.BackColor = c.ContentBackground;

            _lblTitle.ForeColor = c.TitleText;
            _lblItemsInfo.ForeColor = c.SecondaryText;
            _lblTotalLabel.ForeColor = c.TitleText;
            _lblTotalValue.ForeColor = c.Primary;

            _btnClear.BackColor = Color.FromArgb(254, 226, 226);
            _btnClear.ForeColor = Color.FromArgb(220, 38, 38);
            _btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);

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

            using var borderPen = new Pen(c.BorderColor, 1f);
            using var path = RoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), 12);
            e.Graphics.DrawPath(borderPen, path);

            base.OnPaint(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;

            foreach (var row in _rowControls.Values)
            {
                UnwireRowEvents(row);
            }

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

        private string ResolveProductDisplayName(int productUnitId)
        {
            if (_productDisplayNameCache.TryGetValue(productUnitId, out var cachedName))
                return cachedName;

            string displayName = $"Product #{productUnitId}";
            var unit = clsProductUnit.Find(productUnitId);
            if (unit != null)
            {
                string productName = unit.ProductInfo?.ProductName ?? displayName;
                string unitName = unit.UnitInfo?.UnitName ?? string.Empty;
                displayName = string.IsNullOrWhiteSpace(unitName)
                    ? productName
                    : $"{productName} ({unitName})";
            }

            _productDisplayNameCache[productUnitId] = displayName;
            return displayName;
        }
    }

    public interface IProductSalePricingProvider
    {
        decimal CalculateLineSubtotal(clsProductSale sale);
        decimal CalculateTotal(IEnumerable<clsProductSale> sales);
    }

    public sealed class DefaultProductSalePricingProvider : IProductSalePricingProvider
    {
        public decimal CalculateLineSubtotal(clsProductSale sale)
        {
            ArgumentNullException.ThrowIfNull(sale);
            return sale.Quantity * sale.UnitPrice;
        }

        public decimal CalculateTotal(IEnumerable<clsProductSale> sales)
        {
            ArgumentNullException.ThrowIfNull(sales);
            return sales.Sum(CalculateLineSubtotal);
        }
    }

    public sealed class SaleCartRowEventArgs : EventArgs
    {
        public clsProductSale SaleItem { get; }

        public SaleCartRowEventArgs(clsProductSale saleItem)
        {
            SaleItem = saleItem;
        }
    }

    public sealed class SaleCartRowQuantityChangedEventArgs : EventArgs
    {
        public clsProductSale SaleItem { get; }
        public decimal OldQuantity { get; }
        public decimal NewQuantity { get; }

        public SaleCartRowQuantityChangedEventArgs(clsProductSale saleItem, decimal oldQuantity, decimal newQuantity)
        {
            SaleItem = saleItem;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }
    }

    public sealed class SaleCartTotalChangedEventArgs : EventArgs
    {
        public decimal OldTotal { get; }
        public decimal NewTotal { get; }

        public SaleCartTotalChangedEventArgs(decimal oldTotal, decimal newTotal)
        {
            OldTotal = oldTotal;
            NewTotal = newTotal;
        }
    }
}
