using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    /// <summary>
    /// Toast notification control with auto-hide animation.
    /// </summary>
    [DesignerCategory("UserControl")]
    public partial class NotificationControl : UserControl
    {
        #region Private Fields
        private int _targetHeight = 50;
        private int _animationStep = 8;
        private NotificationType _currentType = NotificationType.Info;
        #endregion

        #region Enums
        public enum NotificationType
        {
            Success,
            Error,
            Warning,
            Info
        }
        #endregion

        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string Message
        {
            get => _messageLabel?.Text ?? "";
            set { if (_messageLabel != null) _messageLabel.Text = value; }
        }

        [Category("Behavior")]
        [DefaultValue(3000)]
        public int AutoHideDuration { get; set; } = 3000;

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AutoHide { get; set; } = true;
        #endregion

        #region Events
        public event EventHandler? NotificationClosed;
        #endregion

        public NotificationControl()
        {
            InitializeComponent();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        #region Show/Hide Methods
        /// <summary>
        /// Shows the notification with the specified message and type.
        /// </summary>
        public void Show(string message, NotificationType type, int? duration = null)
        {
            _currentType = type;
            _messageLabel.Text = message;

            ApplyTypeStyle(type);

            // Stop any running timers
            _hideTimer.Stop();
            _autoHideTimer.Stop();

            // Start show animation
            this.Visible = true;
            this.BringToFront();
            _showTimer.Start();

            // Setup auto-hide
            if (AutoHide)
            {
                _autoHideTimer.Interval = duration ?? AutoHideDuration;
                _autoHideTimer.Start();
            }
        }

        /// <summary>
        /// Shows a success notification.
        /// </summary>
        public void ShowSuccess(string message, int? duration = null)
        {
            Show(message, NotificationType.Success, duration);
        }

        /// <summary>
        /// Shows an error notification.
        /// </summary>
        public void ShowError(string message, int? duration = null)
        {
            Show(message, NotificationType.Error, duration);
        }

        /// <summary>
        /// Shows a warning notification.
        /// </summary>
        public void ShowWarning(string message, int? duration = null)
        {
            Show(message, NotificationType.Warning, duration);
        }

        /// <summary>
        /// Shows an info notification.
        /// </summary>
        public void ShowInfo(string message, int? duration = null)
        {
            Show(message, NotificationType.Info, duration);
        }

        /// <summary>
        /// Hides the notification with animation.
        /// </summary>
        public new void Hide()
        {
            _showTimer.Stop();
            _autoHideTimer.Stop();
            _hideTimer.Start();
        }

        /// <summary>
        /// Immediately hides the notification without animation.
        /// </summary>
        public void HideImmediately()
        {
            _showTimer.Stop();
            _hideTimer.Stop();
            _autoHideTimer.Stop();
            this.Height = 0;
            this.Visible = false;
            NotificationClosed?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Animation
        private void ShowTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height < _targetHeight)
            {
                this.Height = Math.Min(this.Height + _animationStep, _targetHeight);
            }
            else
            {
                _showTimer.Stop();
            }
        }

        private void HideTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height > 0)
            {
                this.Height = Math.Max(this.Height - _animationStep, 0);
            }
            else
            {
                _hideTimer.Stop();
                this.Visible = false;
                NotificationClosed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void AutoHideTimer_Tick(object sender, EventArgs e)
        {
            _autoHideTimer.Stop();
            Hide();
        }
        #endregion

        #region Styling
        private void ApplyTypeStyle(NotificationType type)
        {
            Color bgColor;
            string icon;

            switch (type)
            {
                case NotificationType.Success:
                    bgColor = Color.FromArgb(34, 197, 94);   // Green
                    icon = "✓";
                    break;
                case NotificationType.Error:
                    bgColor = Color.FromArgb(239, 68, 68);   // Red
                    icon = "✗";
                    break;
                case NotificationType.Warning:
                    bgColor = Color.FromArgb(245, 158, 11);  // Orange
                    icon = "⚠";
                    break;
                case NotificationType.Info:
                default:
                    bgColor = Color.FromArgb(59, 130, 246);  // Blue
                    icon = "ℹ";
                    break;
            }

            _contentPanel.BackColor = bgColor;
            _iconLabel.Text = icon;
        }

        public void ApplyTheme()
        {
            // Notification colors are based on type, not theme
            // But we ensure text is always white for contrast
            _messageLabel.ForeColor = Color.White;
            _iconLabel.ForeColor = Color.White;
            _closeButton.ForeColor = Color.White;
        }
        #endregion

        #region Rounded Corners (Optional)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Optional: Add subtle shadow or border
            if (this.Height > 0)
            {
                using (var pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
                }
            }
        }
        #endregion
    }
}
