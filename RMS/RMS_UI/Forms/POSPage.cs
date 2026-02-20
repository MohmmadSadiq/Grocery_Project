using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_UI.Controls;

namespace RMS_UI.Forms
{
    public partial class POSPage : Form
    {
        private POSControl posControl = null!;

        public POSPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Point of Sale - RMS";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Add POS Control
            posControl = new POSControl
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(posControl);
        }
    }
}
