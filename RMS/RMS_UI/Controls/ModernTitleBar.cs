using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class ModernTitleBar : Panel
    {
        // Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get => _titleLabel?.Text ?? "";
            set { if (_titleLabel != null) _titleLabel.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? Logo
        {
            get => _logoPanel?.BackgroundImage;
            set { if (_logoPanel != null) _logoPanel.BackgroundImage = value; }
        }

        // Events
        public event EventHandler? CloseClicked;
        public event EventHandler? MaximizeClicked;
        public event EventHandler? MinimizeClicked;
        public event EventHandler? ThemeToggleClicked;

        // For window dragging
        private bool _isDragging = false;
        private Point _dragStartPoint;

        public ModernTitleBar()
        {
            InitializeComponent();
            EnableDragging(this);
            EnableDragging(_titleLabel);
            EnableDragging(_logoPanel);
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        #region Designer Event Handlers
        private void ModernTitleBar_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void ModernTitleBar_HandleCreated(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void ThemeToggleButton_Click(object sender, EventArgs e)
        {
            ThemeToggleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            MinimizeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            MaximizeClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private void PositionControls()
        {
            int centerY = (this.Height - _logoPanel.Height) / 2;
            _logoPanel.Location = new Point(15, centerY);
            
            int titleY = (this.Height - _titleLabel.Height) / 2;
            _titleLabel.Location = new Point(_logoPanel.Right + 12, titleY);

            // Center buttons vertically
            int buttonY = (this.Height - 35) / 2;
            foreach (Control ctrl in _buttonPanel.Controls)
            {
                if (ctrl is ModernTitleBarButton btn)
                {
                    btn.Top = buttonY;
                }
            }
        }

        private void EnableDragging(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    _isDragging = true;
                    _dragStartPoint = e.Location;
                }
            };

            control.MouseMove += (s, e) =>
            {
                if (_isDragging)
                {
                    Form? form = this.FindForm();
                    if (form != null)
                    {
                        Point screenPoint = control.PointToScreen(e.Location);
                        form.Location = new Point(
                            screenPoint.X - _dragStartPoint.X - this.Left - control.Left,
                            screenPoint.Y - _dragStartPoint.Y - this.Top - control.Top
                        );
                    }
                }
            };

            control.MouseUp += (s, e) =>
            {
                _isDragging = false;
            };

            control.DoubleClick += (s, e) =>
            {
                MaximizeClicked?.Invoke(this, EventArgs.Empty);
            };
        }

        public void UpdateMaximizeButton(bool isMaximized)
        {
            _maximizeButton.IsMaximized = isMaximized;
            _maximizeButton.Invalidate();
        }

        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;
            
            this.BackColor = colors.TitleBarBackground;
            _titleLabel.ForeColor = colors.TitleText;
            _buttonPanel.BackColor = colors.TitleBarBackground;
            _logoPanel.BackColor = colors.TitleBarBackground;

            // Update all buttons
            foreach (Control ctrl in _buttonPanel.Controls)
            {
                if (ctrl is ModernTitleBarButton btn)
                {
                    btn.ApplyTheme();
                }
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            var colors = ThemeManager.Colors;
            
            // Draw bottom border
            using (var pen = new Pen(colors.BorderColor, 1))
            {
                e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
            }
        }
    }

    public enum TitleBarButtonType
    {
        Close,
        Maximize,
        Minimize,
        ThemeToggle
    }

    public class ModernTitleBarButton : Control
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TitleBarButtonType ButtonType { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMaximized { get; set; } = false;

        private bool _isHovered = false;
        private bool _isPressed = false;

        public ModernTitleBarButton()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.OptimizedDoubleBuffer, true);
            this.Size = new Size(45, 35);
            this.Cursor = Cursors.Hand;
        }

        public void ApplyTheme()
        {
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isPressed = true;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var colors = ThemeManager.Colors;
            Color bgColor = colors.TitleBarBackground;
            Color iconColor = colors.ButtonNormal;

            if (_isHovered)
            {
                if (ButtonType == TitleBarButtonType.Close)
                {
                    bgColor = colors.CloseButtonHover;
                    iconColor = colors.CloseButtonHoverText;
                }
                else
                {
                    bgColor = colors.ButtonHover;
                    iconColor = colors.ButtonHoverText;
                }
            }

            if (_isPressed)
            {
                bgColor = ControlPaint.Dark(bgColor, 0.1f);
            }

            // Draw background with rounded corners
            using (var brush = new SolidBrush(bgColor))
            {
                int radius = 6;
                using (var path = CreateRoundedRectangle(0, 0, Width, Height, radius))
                {
                    g.FillPath(brush, path);
                }
            }

            // Draw icon
            using (var pen = new Pen(iconColor, 1.5f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                int centerX = Width / 2;
                int centerY = Height / 2;

                switch (ButtonType)
                {
                    case TitleBarButtonType.Close:
                        // X icon
                        int xSize = 5;
                        g.DrawLine(pen, centerX - xSize, centerY - xSize, centerX + xSize, centerY + xSize);
                        g.DrawLine(pen, centerX + xSize, centerY - xSize, centerX - xSize, centerY + xSize);
                        break;

                    case TitleBarButtonType.Maximize:
                        int rectSize = 5;
                        if (IsMaximized)
                        {
                            // Restore icon (two overlapping rectangles)
                            g.DrawRectangle(pen, centerX - rectSize + 2, centerY - rectSize, rectSize * 2 - 2, rectSize * 2 - 2);
                            g.DrawLine(pen, centerX - rectSize, centerY - rectSize + 2, centerX - rectSize, centerY + rectSize);
                            g.DrawLine(pen, centerX - rectSize, centerY + rectSize, centerX + rectSize - 2, centerY + rectSize);
                        }
                        else
                        {
                            // Maximize icon (single rectangle)
                            g.DrawRectangle(pen, centerX - rectSize, centerY - rectSize, rectSize * 2, rectSize * 2);
                        }
                        break;

                    case TitleBarButtonType.Minimize:
                        // Line icon
                        int lineWidth = 6;
                        g.DrawLine(pen, centerX - lineWidth, centerY, centerX + lineWidth, centerY);
                        break;

                    case TitleBarButtonType.ThemeToggle:
                        // Sun/Moon icon
                        if (ThemeManager.CurrentTheme == ThemeMode.Light)
                        {
                            // Moon icon (for switching to dark)
                            using (var path = new GraphicsPath())
                            {
                                int moonRadius = 7;
                                path.AddArc(centerX - moonRadius, centerY - moonRadius, moonRadius * 2, moonRadius * 2, -40, 220);
                                path.AddArc(centerX - moonRadius + 4, centerY - moonRadius - 2, moonRadius * 2 - 4, moonRadius * 2 - 4, 180, -220);
                                path.CloseFigure();
                                using (var moonBrush = new SolidBrush(iconColor))
                                {
                                    g.FillPath(moonBrush, path);
                                }
                            }
                        }
                        else
                        {
                            // Sun icon (for switching to light)
                            int sunRadius = 5;
                            using (var sunBrush = new SolidBrush(iconColor))
                            {
                                g.FillEllipse(sunBrush, centerX - sunRadius, centerY - sunRadius, sunRadius * 2, sunRadius * 2);
                            }
                            // Sun rays
                            int rayLength = 3;
                            int rayDistance = 8;
                            for (int i = 0; i < 8; i++)
                            {
                                double angle = i * Math.PI / 4;
                                int x1 = centerX + (int)(Math.Cos(angle) * rayDistance);
                                int y1 = centerY + (int)(Math.Sin(angle) * rayDistance);
                                int x2 = centerX + (int)(Math.Cos(angle) * (rayDistance + rayLength));
                                int y2 = centerY + (int)(Math.Sin(angle) * (rayDistance + rayLength));
                                g.DrawLine(pen, x1, y1, x2, y2);
                            }
                        }
                        break;
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(x, y, diameter, diameter, 180, 90);
            path.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
            path.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
            path.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }
    }
}
