using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Views
{
    public partial class MainPage : UserControl
    {
        private bool _isSidebarExpanded = true;
        private const int ExpandedWidth = 260;
        private const int CollapsedWidth = 70;
        private System.Windows.Forms.Timer _animationTimer = null!;
        private int _targetWidth;
        private Dictionary<Button, string> _buttonFullTexts = new Dictionary<Button, string>();

        public MainPage()
        {
            InitializeComponent();
            InitializeNavigation();
            InitializeSidebarAnimation();
            SubscribeToTheme();
            ApplyTheme();
        }

        private void InitializeNavigation()
        {
            // Store original full texts
            _buttonFullTexts[btnDashboard] = "🏠  Dashboard";
            _buttonFullTexts[btnPOS] = "💰  Point of Sale";
            _buttonFullTexts[btnProducts] = "📦  Products";
            _buttonFullTexts[btnSuppliers] = "🚚  Suppliers";
            _buttonFullTexts[btnReports] = "📊  Reports";
            _buttonFullTexts[btnSettings] = "⚙️  Settings";

            // Wire up button click events
            btnDashboard.Click += (s, e) => OnMenuItemClicked("Dashboard");
            btnPOS.Click += (s, e) => OnMenuItemClicked("POS");
            btnProducts.Click += (s, e) => OnMenuItemClicked("Products");
            btnSuppliers.Click += (s, e) => OnMenuItemClicked("Suppliers");
            btnReports.Click += (s, e) => OnMenuItemClicked("Reports");
            btnSettings.Click += (s, e) => OnMenuItemClicked("Settings");
            btnToggleSidebar.Click += (s, e) => ToggleSidebar();
        }

        private void InitializeSidebarAnimation()
        {
            _animationTimer = new System.Windows.Forms.Timer();
            _animationTimer.Interval = 10;
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        private void ToggleSidebar()
        {
            _isSidebarExpanded = !_isSidebarExpanded;
            _targetWidth = _isSidebarExpanded ? ExpandedWidth : CollapsedWidth;
            UpdateButtonsAlignment();
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            int step = 20;
            
            if (sidebarPanel.Width < _targetWidth)
            {
                sidebarPanel.Width = Math.Min(sidebarPanel.Width + step, _targetWidth);
            }
            else if (sidebarPanel.Width > _targetWidth)
            {
                sidebarPanel.Width = Math.Max(sidebarPanel.Width - step, _targetWidth);
            }
            else
            {
                _animationTimer.Stop();
            }
        }

        private void UpdateButtonsAlignment()
        {
            var buttons = new[] { btnDashboard, btnPOS, btnProducts, btnSuppliers, btnReports, btnSettings };
            
            foreach (var btn in buttons)
            {
                if (_isSidebarExpanded)
                {
                    // Show full text
                    if (_buttonFullTexts.ContainsKey(btn))
                    {
                        btn.Text = _buttonFullTexts[btn];
                    }
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(20, 0, 0, 0);
                    btn.Font = new Font("Segoe UI", 16F);
                }
                else
                {
                    // Show only icon (centered)
                    btn.Text = btn.Tag?.ToString() ?? "";
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = new Padding(0);
                    btn.Font = new Font("Segoe UI", 16F);
                }
            }

            // Update header visibility
            lblSidebarTitle.Visible = _isSidebarExpanded;
            
            // Update toggle button position - IMPORTANT: remove anchor first
            btnToggleSidebar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            
            if (_isSidebarExpanded)
            {
                btnToggleSidebar.Location = new Point(207, 28);
                btnToggleSidebar.Text = "☰";
            }
            else
            {
                // Center the button in collapsed sidebar
                btnToggleSidebar.Location = new Point(20, 28);
                btnToggleSidebar.Text = "☰";
            }
        }

        private void SubscribeToTheme()
        {
            // Subscribe to theme changes from MainForm
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void OnMenuItemClicked(string menuItem)
        {
            switch (menuItem)
            {
                case "POS":
                    LoadContent(new POSControl());
                    break;
                case "Products":
                    LoadContent(new ProductsPage());
                    break;
                case "Suppliers":
                    LoadContent(new Suppliers_PurchasePage());
                    break;
                case "Dashboard":
                    ShowWelcome();
                    break;
                default:
                    MessageBox.Show(
                        $"{menuItem} - Coming Soon",
                        "Navigation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    break;
            }
        }

        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            // Main backgrounds
            this.BackColor = colors.ContentBackground;
            contentPanel.BackColor = colors.ContentBackground;
            sidebarPanel.BackColor = colors.FormBackground;

            // Header - keep accent color
            sidebarHeader.BackColor = colors.Primary;
            lblSidebarTitle.ForeColor = Color.White;
            btnToggleSidebar.ForeColor = Color.White;
            btnToggleSidebar.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;

            // Content labels
            lblWelcome.ForeColor = colors.TitleText;
            lblSubtitle.ForeColor = colors.SecondaryText;

            // Style all menu buttons
            foreach (Control ctrl in sidebarPanel.Controls)
            {
                if (ctrl is Button btn && btn != btnToggleSidebar)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = colors.PrimaryText;
                    btn.FlatAppearance.MouseOverBackColor = colors.PrimaryLight;
                    btn.FlatAppearance.BorderSize = 0;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Load a UserControl into the content area (for future use)
        /// </summary>
        public void LoadContent(UserControl content)
        {
            contentPanel.Controls.Clear();
            content.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(content);
        }

        /// <summary>
        /// Clear content and show welcome message
        /// </summary>
        public void ShowWelcome()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(lblSubtitle);
            contentPanel.Controls.Add(lblWelcome);
        }
    }
}
