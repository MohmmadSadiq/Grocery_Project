using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RMS_UI.Controls;
using RMS_UI.Utilities;
using RMS_UI.Views;
using Syncfusion.WinForms.Controls;

namespace RMS_UI
{
    public partial class MainForm : SfForm
    {
        private ModernTitleBar _titleBar = null!;
        private Panel _contentPanel = null!;
        private MainPage _mainPage = null!;
        private const int BorderRadius = 15;
        private const int BorderSize = 2;
        private const int ResizeAreaSize = 10;

        // For window resizing
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 1;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        public MainForm()
        {
            InitializeComponent();
            InitializeModernForm();
        }

        private void InitializeModernForm()
        {
            // Form settings
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 500);
            this.Size = new Size(1100, 700);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // Hide Syncfusion title bar
            this.Style.TitleBar.Height = 0;

            // Create title bar
            _titleBar = new ModernTitleBar
            {
                Title = "RMS - Retail Management System",
                Dock = DockStyle.Top,
                Height = 70
            };

            // Wire up title bar events
            _titleBar.CloseClicked += (s, e) => this.Close();
            _titleBar.MaximizeClicked += (s, e) => ToggleMaximize();
            _titleBar.MinimizeClicked += (s, e) => this.WindowState = FormWindowState.Minimized;
            _titleBar.ThemeToggleClicked += (s, e) =>
            {
                ThemeManager.ToggleTheme();
                ApplyTheme();
            };

            // Create content panel
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            // Create and add MainPage
            _mainPage = new MainPage
            {
                Dock = DockStyle.Fill
            };
            _contentPanel.Controls.Add(_mainPage);

            // Add controls
            this.Controls.Add(_contentPanel);
            this.Controls.Add(_titleBar);

            // Subscribe to theme changes
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();

            // Apply initial theme
            ApplyTheme();
            
            // Apply rounded corners after form is fully loaded
            this.Load += (s, e) => ApplyRoundedCorners();
            this.Shown += (s, e) => ApplyRoundedCorners();
        }

        private void ToggleMaximize()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                ApplyRoundedCorners();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.Region = null; // Remove rounded corners when maximized
            }
            _titleBar.UpdateMaximizeButton(this.WindowState == FormWindowState.Maximized);
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;
            _contentPanel.BackColor = colors.ContentBackground;
            _titleBar.ApplyTheme();

            this.Invalidate();
        }

        private void ApplyRoundedCorners()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Region = null;
                return;
            }

            using (var path = new GraphicsPath())
            {
                int radius = BorderRadius * 2;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
                path.AddArc(0, Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState != FormWindowState.Maximized)
            {
                ApplyRoundedCorners();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var colors = ThemeManager.Colors;

            // Draw border
            using (var pen = new Pen(colors.BorderAccent, BorderSize))
            {
                using (var path = CreateRoundedRectangle(BorderSize / 2, BorderSize / 2, 
                    Width - BorderSize, Height - BorderSize, BorderRadius))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(float x, float y, float width, float height, float radius)
        {
            var path = new GraphicsPath();
            float diameter = radius * 2;

            path.AddArc(x, y, diameter, diameter, 180, 90);
            path.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
            path.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
            path.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        // Enable window resizing from edges
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST && this.WindowState != FormWindowState.Maximized)
            {
                base.WndProc(ref m);

                Point cursor = this.PointToClient(Cursor.Position);
                
                if (cursor.Y <= ResizeAreaSize)
                {
                    if (cursor.X <= ResizeAreaSize)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (cursor.X >= Width - ResizeAreaSize)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else
                        m.Result = (IntPtr)HTTOP;
                }
                else if (cursor.Y >= Height - ResizeAreaSize)
                {
                    if (cursor.X <= ResizeAreaSize)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (cursor.X >= Width - ResizeAreaSize)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else
                        m.Result = (IntPtr)HTBOTTOM;
                }
                else if (cursor.X <= ResizeAreaSize)
                {
                    m.Result = (IntPtr)HTLEFT;
                }
                else if (cursor.X >= Width - ResizeAreaSize)
                {
                    m.Result = (IntPtr)HTRIGHT;
                }

                return;
            }

            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // WS_MINIMIZEBOX - Enable minimize animation
                return cp;
            }
        }
    }
}