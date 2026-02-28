using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Products
{
    /// <summary>
    /// Sizable dialog that wraps <see cref="ProductsPage"/>.
    /// Supports ThemeManager (Light / Dark) and matches the project visual style.
    /// Callers can add custom context-menu items via <see cref="AddContextMenuItem"/>.
    /// </summary>
    public partial class frmManageProducts : Form
    {
        // ── Win32 for rounded corners (Windows 11) ─────────────────────────────
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        // ── Fields ──────────────────────────────────────────────────────────────
        private ProductsPage _productsPage = null!;

        // ── Constructor ─────────────────────────────────────────────────────────

        public frmManageProducts()
        {
            InitializeComponent();
            _BootstrapForm();
        }

        // ── Boot ────────────────────────────────────────────────────────────────

        private void _BootstrapForm()
        {
            _productsPage = new ProductsPage();
            _productsPage.Dock = DockStyle.Fill;
            Controls.Add(_productsPage);

            // Subscribe to theme changes
            ThemeManager.ThemeChanged += _OnThemeChanged;

            // Initial theme pass after handle is created
            Load += (s, e) =>
            {
                _ApplyDwmRounding();
                ApplyTheme();
            };
        }

        // ── Context Menu ────────────────────────────────────────────────────────

        /// <summary>
        /// Adds a new item to the DataGridView's context menu.
        /// Existing (default) items are preserved.
        /// </summary>
        public void AddContextMenuItem(string text, EventHandler onClick, bool isDelete = false)
        {
            _productsPage.AddContextMenuItem(text, onClick, isDelete);
        }

        /// <summary>
        /// Adds a separator to the DataGridView's context menu.
        /// </summary>
        public void AddContextMenuSeparator()
        {
            _productsPage.AddContextMenuSeparator();
        }

        /// <summary>
        /// Gets the underlying <see cref="ProductsPage"/> control.
        /// </summary>
        public ProductsPage ProductsPage => _productsPage;

        // ── Theme ───────────────────────────────────────────────────────────────

        private void _OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => _OnThemeChanged(sender, e))); return; }
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            var c = ThemeManager.Colors;
            BackColor = c.FormBackground;
            _productsPage.ApplyTheme();
        }

        // ── Win32 helpers ───────────────────────────────────────────────────────

        private void _ApplyDwmRounding()
        {
            try
            {
                int pref = DWMWCP_ROUND;
                DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE,
                    ref pref, sizeof(int));
            }
            catch { /* non-Win11 – silently skip */ }
        }

        // ── Cleanup ─────────────────────────────────────────────────────────────

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= _OnThemeChanged;
            base.OnFormClosed(e);
        }

        // ── Custom border paint ─────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var c = ThemeManager.Colors;
            using var pen = new Pen(c.BorderColor, 1.5f);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(pen, rect);
        }
    }
}
