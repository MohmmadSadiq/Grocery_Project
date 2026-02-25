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
    public partial class Suppliers_PurchasePage : UserControl
    {
        public Suppliers_PurchasePage()
        {
            InitializeComponent();
            LoadSuppliersTab();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void LoadSuppliersTab()
        {
            var suppliersPage = new SuppliersPage();
            suppliersPage.Dock = DockStyle.Fill;
            SuppliersTab.Controls.Add(suppliersPage);
        }

        private void ApplyTheme()
        {
            var c = ThemeManager.Colors;
            BackColor = c.ContentBackground;
            tabControl1.BackColor = c.ContentBackground;
        }
    }
}
