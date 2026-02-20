using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class SuppliersPage : UserControl
    {
        public SuppliersPage()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            // Apply theme colors
            var colors = ThemeManager.Colors;
            
            // Header panel
            _headerPanel.BackColor = colors.ContentBackground;
            
            // Title and subtitle
            _lblTitle.ForeColor = colors.TitleText;
            _lblSubtitle.ForeColor = colors.SecondaryText;
            
            // Button styling
            _btnNewSupplier.BackColor = colors.Primary;
            _btnNewSupplier.ForeColor = Color.White;
            _btnNewSupplier.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;
            _btnNewSupplier.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(colors.Primary, 0.2f);
            
            // Background
            this.BackColor = colors.FormBackground;
            
            // Apply theme to data grid
            _dataGrid.ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Subscribe to theme changes
            ThemeManager.ThemeChanged += (s, args) => ApplyTheme();
        }
    }
}
