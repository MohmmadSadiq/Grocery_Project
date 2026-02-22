using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Companies
{
    /// <summary>
    /// Modal dialog that wraps <see cref="ctrlAddEditCompany"/>.
    /// Supports ThemeManager (Light / Dark) and matches the project visual style.
    /// </summary>
    public partial class frmAddEditCompany : Form
    {
        // ── Win32 for rounded corners (Windows 11) ────────────────────────────
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        // ── Properties ───────────────────────────────────────────────────────────
        public clsCompany? SavedCompany { get; private set; }

        // ── Fields ───────────────────────────────────────────────────────────────
        private ctrlAddEditCompany _ctrl = null!;
        private const int CornerRadius = 12;

        // ── Constructors ─────────────────────────────────────────────────────────

        /// <summary>Open in Add-New mode.</summary>
        public frmAddEditCompany()
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditCompany();
            _BootstrapForm();
        }

        /// <summary>Open in Edit mode for an existing company.</summary>
        public frmAddEditCompany(int companyID)
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditCompany(companyID);
            _BootstrapForm();
        }

        // ── Boot ─────────────────────────────────────────────────────────────────

        private void _BootstrapForm()
        {
            // Wire UserControl events
            _ctrl.CompanySaved += (s, company) =>
            {
                SavedCompany = company;

                DialogResult = DialogResult.OK;
                MessageBox.Show("CompanySaved Successfully");
            };

            _ctrl.CancelClicked += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            // Size the form to fit the control exactly
            _ctrl.Dock = DockStyle.Fill;
            Controls.Add(_ctrl);

            // Subscribe to theme changes
            ThemeManager.ThemeChanged += OnThemeChanged;

            // Initial theme pass after handle is created
            Load += (s, e) =>
            {
                _ApplyDwmRounding();
                ApplyTheme();
            };
        }

        // ── Theme ────────────────────────────────────────────────────────────────

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => OnThemeChanged(sender, e))); return; }
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            var c = ThemeManager.Colors;
            BackColor = c.FormBackground;
            _ctrl.ApplyTheme();
        }

        // ── Win32 helpers ────────────────────────────────────────────────────────

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

        // ── Drag borderless form ─────────────────────────────────────────────────

        protected override void WndProc(ref Message m)
        {
            // Allow dragging by clicking anywhere on the header panel of the control
            const int WM_NCHITTEST = 0x0084;
            const int HTCAPTION    = 2;
            const int HTCLIENT     = 1;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if ((int)m.Result == HTCLIENT)
                {
                    Point pt = PointToClient(new Point(m.LParam.ToInt32()));
                    // Drag zone = top 72 px (header height of the UserControl)
                    if (pt.Y < 72)
                        m.Result = (IntPtr)HTCAPTION;
                }
                return;
            }
            base.WndProc(ref m);
        }

        // ── Cleanup ──────────────────────────────────────────────────────────────

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }

        // ── Custom border paint ──────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var c = ThemeManager.Colors;
            using var pen = new Pen(c.BorderColor, 1.5f);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Thin border around the whole form
            e.Graphics.DrawRectangle(pen, rect);
        }
    }
}
