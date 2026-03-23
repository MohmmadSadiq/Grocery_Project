using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class frmAddEditCustomer : Form
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        public clsCustomer? SavedCustomer { get; private set; }

        private readonly ctrlAddEditCustomer _ctrl;

        public frmAddEditCustomer()
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditCustomer();
            Bootstrap();
        }

        public frmAddEditCustomer(int customerID)
        {
            InitializeComponent();
            _ctrl = new ctrlAddEditCustomer(customerID);
            Bootstrap();
        }

        private void Bootstrap()
        {
            _ctrl.Dock = DockStyle.Fill;
            Controls.Add(_ctrl);

            _ctrl.CustomerSaved += (_, customer) =>
            {
                SavedCustomer = customer;
                DialogResult = DialogResult.OK;
                Close();
            };

            _ctrl.CancelClicked += (_, _) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            ThemeManager.ThemeChanged += OnThemeChanged;
            Load += (_, _) =>
            {
                ApplyDwmRounding();
                ApplyTheme();
            };
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            ApplyTheme();
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Colors.FormBackground;
            _ctrl.ApplyTheme();
        }

        private void ApplyDwmRounding()
        {
            try
            {
                int pref = DWMWCP_ROUND;
                DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(int));
            }
            catch
            {
                // ignore on unsupported systems
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTCAPTION = 2;
            const int HTCLIENT = 1;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if ((int)m.Result == HTCLIENT)
                {
                    Point pt = PointToClient(new Point(m.LParam.ToInt32()));
                    if (pt.Y < 72)
                        m.Result = (IntPtr)HTCAPTION;
                }
                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var c = ThemeManager.Colors;
            using var pen = new Pen(c.BorderColor, 1.5f);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(pen, rect);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
    }
}
