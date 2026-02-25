using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.PaymentMethods;
using RMS_UI.Utilities;

namespace RMS_UI.Settings
{
    /// <summary>
    /// General Settings page loaded in MainPage's content panel.
    /// Displays setting categories as clickable cards.
    /// </summary>
    public partial class GeneralSettingsPage : UserControl
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _lblTitle = null!;
        private Label _lblSubtitle = null!;
        private NotificationControl _notification = null!;
        private FlowLayoutPanel _cardsContainer = null!;

        // Cards
        private Panel _cardPaymentMethods = null!;
        private Panel _cardProductSettings = null!;
        private Panel _cardUserSettings = null!;
        private Panel _cardCompanyInfo = null!;
        #endregion

        #region Constructor
        public GeneralSettingsPage()
        {
            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();
            this.Font = new Font("Segoe UI", 10F);

            // Notification
            _notification = new NotificationControl
            {
                Dock = DockStyle.Top
            };

            // Header Panel
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(30, 0, 30, 0)
            };

            _lblTitle = new Label
            {
                Text = "⚙️  General Settings",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 14)
            };

            _lblSubtitle = new Label
            {
                Text = "Manage system settings and configurations",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(32, 48)
            };

            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Controls.Add(_lblSubtitle);

            // Cards container
            _cardsContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(25, 20, 25, 20)
            };

            // Create cards
            _cardPaymentMethods = CreateSettingsCard(
                "💳", "Payment Methods",
                "Manage payment types used\nin sales and purchases",
                GetPaymentMethodCount,
                "methods",
                true,
                () => OpenPaymentMethodSettings());

            _cardProductSettings = CreateSettingsCard(
                "📦", "Product Settings",
                "Manage units, categories\nand product configurations",
                GetProductSettingsCount,
                "items",
                true,
                () => OpenProductSettings());

            _cardUserSettings = CreateSettingsCard(
                "👤", "User Settings",
                "Manage user accounts\nand permissions",
                null,
                "",
                false,
                null);

            _cardCompanyInfo = CreateSettingsCard(
                "🏢", "Company Info",
                "Business information\nand branding",
                null,
                "",
                false,
                null);

            _cardsContainer.Controls.Add(_cardPaymentMethods);
            _cardsContainer.Controls.Add(_cardProductSettings);
            _cardsContainer.Controls.Add(_cardUserSettings);
            _cardsContainer.Controls.Add(_cardCompanyInfo);

            // Add controls (reverse dock order)
            this.Controls.Add(_cardsContainer);
            this.Controls.Add(_headerPanel);
            this.Controls.Add(_notification);

            this.ResumeLayout(false);
        }

        /// <summary>
        /// Creates a settings card panel.
        /// </summary>
        private Panel CreateSettingsCard(
            string icon,
            string title,
            string description,
            Func<int>? countProvider,
            string countSuffix,
            bool isEnabled,
            Action? onClick)
        {
            var card = new Panel
            {
                Size = new Size(230, 170),
                Margin = new Padding(10),
                Cursor = isEnabled ? Cursors.Hand : Cursors.Default,
                Tag = isEnabled ? "enabled" : "disabled"
            };

            // Icon
            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 28F),
                AutoSize = false,
                Size = new Size(60, 50),
                Location = new Point(20, 16),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Title
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 68)
            };

            // Description
            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                Size = new Size(190, 36),
                Location = new Point(20, 92)
            };

            // Footer (count or "Coming Soon")
            var lblFooter = new Label
            {
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true,
                Location = new Point(20, 140)
            };

            if (isEnabled && countProvider != null)
            {
                try
                {
                    int count = countProvider();
                    lblFooter.Text = $"{count} {countSuffix}  →";
                }
                catch
                {
                    lblFooter.Text = $"→";
                }
            }
            else if (!isEnabled)
            {
                lblFooter.Text = "Coming Soon";
            }

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblFooter);

            // Store references for theming
            lblIcon.Tag = "icon";
            lblTitle.Tag = "title";
            lblDesc.Tag = "desc";
            lblFooter.Tag = isEnabled ? "footer" : "coming-soon";

            if (isEnabled && onClick != null)
            {
                // Wire click events to all children
                card.Click += (s, e) => onClick();
                foreach (Control ctrl in card.Controls)
                {
                    ctrl.Cursor = Cursors.Hand;
                    ctrl.Click += (s, e) => onClick();
                }

                // Hover effects
                card.MouseEnter += (s, e) => OnCardMouseEnter(card);
                card.MouseLeave += (s, e) => OnCardMouseLeave(card);
                foreach (Control ctrl in card.Controls)
                {
                    ctrl.MouseEnter += (s, e) => OnCardMouseEnter(card);
                    ctrl.MouseLeave += (s, e) => OnCardMouseLeave(card);
                }
            }

            return card;
        }
        #endregion

        #region Card Hover Effects
        private void OnCardMouseEnter(Panel card)
        {
            if (card.Tag?.ToString() != "enabled") return;
            var colors = ThemeManager.Colors;
            card.BackColor = colors.ButtonHover;
            card.Invalidate();
        }

        private void OnCardMouseLeave(Panel card)
        {
            if (card.Tag?.ToString() != "enabled") return;

            // Check if mouse is still within the card bounds
            Point pt = card.PointToClient(Cursor.Position);
            if (card.ClientRectangle.Contains(pt)) return;

            var colors = ThemeManager.Colors;
            card.BackColor = colors.ContentBackground;
            card.Invalidate();
        }
        #endregion

        #region Card Paint (rounded border)
        private void PaintCardBorder(Panel card, PaintEventArgs e)
        {
            var colors = ThemeManager.Colors;
            bool isHovered = card.ClientRectangle.Contains(card.PointToClient(Cursor.Position));
            bool isEnabled = card.Tag?.ToString() == "enabled";

            var borderColor = (isHovered && isEnabled) ? colors.BorderAccent : colors.BorderColor;
            int radius = 12;

            using var pen = new Pen(borderColor, 1.5f);
            var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);

            using var path = CreateRoundedRect(rect, radius);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion

        #region Navigation Actions
        private void OpenPaymentMethodSettings()
        {
            using (var dlg = new PaymentMethodSettingsDialog())
            {
                dlg.ShowDialog(this.FindForm());
            }
            RefreshCardCount(_cardPaymentMethods, GetPaymentMethodCount, "methods");
        }

        private void OpenProductSettings()
        {
            using (var dlg = new RMS_UI.Forms.ProductSettingsDialog())
            {
                dlg.ShowDialog(this.FindForm());
            }
            RefreshCardCount(_cardProductSettings, GetProductSettingsCount, "items");
        }

        private void RefreshCardCount(Panel card, Func<int> countProvider, string suffix)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl.Tag?.ToString() == "footer")
                {
                    try
                    {
                        int count = countProvider();
                        ctrl.Text = $"{count} {suffix}  →";
                    }
                    catch { }
                    break;
                }
            }
        }
        #endregion

        #region Count Providers
        private int GetPaymentMethodCount()
        {
            try
            {
                var dt = clsPaymentMethod.GetAllPaymentMethod();
                return dt?.Rows.Count ?? 0;
            }
            catch { return 0; }
        }

        private int GetProductSettingsCount()
        {
            try
            {
                int count = 0;
                var units = clsUnit.GetAllUnit();
                if (units != null) count += units.Rows.Count;
                var cats = clsCategory.GetAllCategory();
                if (cats != null) count += cats.Rows.Count;
                return count;
            }
            catch { return 0; }
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;

            if (_headerPanel != null)
            {
                _headerPanel.BackColor = colors.ContentBackground;
                _lblTitle.ForeColor = colors.TitleText;
                _lblSubtitle.ForeColor = colors.SecondaryText;
            }

            if (_cardsContainer != null)
            {
                _cardsContainer.BackColor = colors.FormBackground;

                foreach (Control ctrl in _cardsContainer.Controls)
                {
                    if (ctrl is Panel card)
                    {
                        ApplyCardTheme(card, colors);
                    }
                }
            }

            Invalidate(true);
        }

        private void ApplyCardTheme(Panel card, dynamic colors)
        {
            bool isEnabled = card.Tag?.ToString() == "enabled";

            card.BackColor = colors.ContentBackground;

            // Rewire paint for rounded border
            card.Paint -= Card_Paint;
            card.Paint += Card_Paint;

            if (!isEnabled)
            {
                // Dim disabled cards
            }

            foreach (Control ctrl in card.Controls)
            {
                if (ctrl.Tag?.ToString() == "icon")
                {
                    // Icon stays as-is (emoji)
                }
                else if (ctrl.Tag?.ToString() == "title")
                {
                    ctrl.ForeColor = isEnabled ? colors.TitleText : colors.SecondaryText;
                }
                else if (ctrl.Tag?.ToString() == "desc")
                {
                    ctrl.ForeColor = colors.SecondaryText;
                }
                else if (ctrl.Tag?.ToString() == "footer")
                {
                    ctrl.ForeColor = colors.Primary;
                }
                else if (ctrl.Tag?.ToString() == "coming-soon")
                {
                    ctrl.ForeColor = colors.SecondaryText;
                }
            }
        }

        private void Card_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is Panel card)
                PaintCardBorder(card, e);
        }
        #endregion
    }
}
