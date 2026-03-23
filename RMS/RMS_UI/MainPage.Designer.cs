using System.Drawing;
using System.Windows.Forms;

namespace RMS_UI.Views
{
    partial class MainPage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sidebarPanel = new Panel();
            btnSettings = new Button();
            btnReports = new Button();
            btnSuppliers = new Button();
            btnCustomers = new Button();
            btnProducts = new Button();
            btnPOS = new Button();
            btnDashboard = new Button();
            sidebarHeader = new Panel();
            btnToggleSidebar = new Button();
            lblSidebarTitle = new Label();
            contentPanel = new Panel();
            lblSubtitle = new Label();
            lblWelcome = new Label();
            sidebarPanel.SuspendLayout();
            sidebarHeader.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(248, 250, 252);
            sidebarPanel.Controls.Add(btnSettings);
            sidebarPanel.Controls.Add(btnReports);
            sidebarPanel.Controls.Add(btnSuppliers);
            sidebarPanel.Controls.Add(btnCustomers);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnPOS);
            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(sidebarHeader);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Margin = new Padding(2);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(260, 683);
            sidebarPanel.TabIndex = 0;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.Transparent;
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.Dock = DockStyle.Top;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI", 15.75F);
            btnSettings.ForeColor = Color.FromArgb(51, 65, 85);
            btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnSettings.Location = new Point(0, 460);
            btnSettings.Margin = new Padding(2);
            btnSettings.Name = "btnSettings";
            btnSettings.Padding = new Padding(16, 0, 0, 0);
            btnSettings.Size = new Size(260, 60);
            btnSettings.TabIndex = 6;
            btnSettings.Tag = "⚙️";
            btnSettings.Text = "⚙️  Settings";
            btnSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.Transparent;
            btnReports.Cursor = Cursors.Hand;
            btnReports.Dock = DockStyle.Top;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 15.75F);
            btnReports.ForeColor = Color.FromArgb(51, 65, 85);
            btnReports.ImageAlign = ContentAlignment.MiddleLeft;
            btnReports.Location = new Point(0, 400);
            btnReports.Margin = new Padding(2);
            btnReports.Name = "btnReports";
            btnReports.Padding = new Padding(16, 0, 0, 0);
            btnReports.Size = new Size(260, 60);
            btnReports.TabIndex = 5;
            btnReports.Tag = "📊";
            btnReports.Text = "📊  Reports";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            // 
            // btnSuppliers
            // 
            btnSuppliers.BackColor = Color.Transparent;
            btnSuppliers.Cursor = Cursors.Hand;
            btnSuppliers.Dock = DockStyle.Top;
            btnSuppliers.FlatAppearance.BorderSize = 0;
            btnSuppliers.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnSuppliers.FlatStyle = FlatStyle.Flat;
            btnSuppliers.Font = new Font("Segoe UI", 15.75F);
            btnSuppliers.ForeColor = Color.FromArgb(51, 65, 85);
            btnSuppliers.ImageAlign = ContentAlignment.MiddleLeft;
            btnSuppliers.Location = new Point(0, 340);
            btnSuppliers.Margin = new Padding(2);
            btnSuppliers.Name = "btnSuppliers";
            btnSuppliers.Padding = new Padding(16, 0, 0, 0);
            btnSuppliers.Size = new Size(260, 60);
            btnSuppliers.TabIndex = 4;
            btnSuppliers.Tag = "🚚";
            btnSuppliers.Text = "🚚  Suppliers";
            btnSuppliers.TextAlign = ContentAlignment.MiddleLeft;
            btnSuppliers.UseVisualStyleBackColor = false;
            // 
            // btnCustomers
            // 
            btnCustomers.BackColor = Color.Transparent;
            btnCustomers.Cursor = Cursors.Hand;
            btnCustomers.Dock = DockStyle.Top;
            btnCustomers.FlatAppearance.BorderSize = 0;
            btnCustomers.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnCustomers.FlatStyle = FlatStyle.Flat;
            btnCustomers.Font = new Font("Segoe UI", 15.75F);
            btnCustomers.ForeColor = Color.FromArgb(51, 65, 85);
            btnCustomers.ImageAlign = ContentAlignment.MiddleLeft;
            btnCustomers.Location = new Point(0, 280);
            btnCustomers.Margin = new Padding(2);
            btnCustomers.Name = "btnCustomers";
            btnCustomers.Padding = new Padding(16, 0, 0, 0);
            btnCustomers.Size = new Size(260, 60);
            btnCustomers.TabIndex = 3;
            btnCustomers.Tag = "👥";
            btnCustomers.Text = "👥  Customers";
            btnCustomers.TextAlign = ContentAlignment.MiddleLeft;
            btnCustomers.UseVisualStyleBackColor = false;
            // 
            // btnProducts
            // 
            btnProducts.BackColor = Color.Transparent;
            btnProducts.Cursor = Cursors.Hand;
            btnProducts.Dock = DockStyle.Top;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Segoe UI", 15.75F);
            btnProducts.ForeColor = Color.FromArgb(51, 65, 85);
            btnProducts.ImageAlign = ContentAlignment.MiddleLeft;
            btnProducts.Location = new Point(0, 220);
            btnProducts.Margin = new Padding(2);
            btnProducts.Name = "btnProducts";
            btnProducts.Padding = new Padding(16, 0, 0, 0);
            btnProducts.Size = new Size(260, 60);
            btnProducts.TabIndex = 2;
            btnProducts.Tag = "📦";
            btnProducts.Text = "📦  Products";
            btnProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnProducts.UseVisualStyleBackColor = false;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.Transparent;
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 15.75F);
            btnDashboard.ForeColor = Color.FromArgb(51, 65, 85);
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(0, 100);
            btnDashboard.Margin = new Padding(2);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(16, 0, 0, 0);
            btnDashboard.Size = new Size(260, 60);
            btnDashboard.TabIndex = 1;
            btnDashboard.Tag = "🏠";
            btnDashboard.Text = "🏠  Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnPOS
            // 
            btnPOS.BackColor = Color.Transparent;
            btnPOS.Cursor = Cursors.Hand;
            btnPOS.Dock = DockStyle.Top;
            btnPOS.FlatAppearance.BorderSize = 0;
            btnPOS.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 234, 254);
            btnPOS.FlatStyle = FlatStyle.Flat;
            btnPOS.Font = new Font("Segoe UI", 15.75F);
            btnPOS.ForeColor = Color.FromArgb(51, 65, 85);
            btnPOS.ImageAlign = ContentAlignment.MiddleLeft;
            btnPOS.Location = new Point(0, 160);
            btnPOS.Margin = new Padding(2);
            btnPOS.Name = "btnPOS";
            btnPOS.Padding = new Padding(16, 0, 0, 0);
            btnPOS.Size = new Size(260, 60);
            btnPOS.TabIndex = 7;
            btnPOS.Tag = "🛒";
            btnPOS.Text = "🛒  POS";
            btnPOS.TextAlign = ContentAlignment.MiddleLeft;
            btnPOS.UseVisualStyleBackColor = false;
            // 
            // sidebarHeader
            // 
            sidebarHeader.BackColor = Color.FromArgb(59, 130, 246);
            sidebarHeader.Controls.Add(btnToggleSidebar);
            sidebarHeader.Controls.Add(lblSidebarTitle);
            sidebarHeader.Dock = DockStyle.Top;
            sidebarHeader.Font = new Font("Segoe UI", 15.75F);
            sidebarHeader.Location = new Point(0, 0);
            sidebarHeader.Margin = new Padding(2);
            sidebarHeader.Name = "sidebarHeader";
            sidebarHeader.Size = new Size(260, 100);
            sidebarHeader.TabIndex = 0;
            // 
            // btnToggleSidebar
            // 
            btnToggleSidebar.BackColor = Color.Transparent;
            btnToggleSidebar.Cursor = Cursors.Hand;
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnToggleSidebar.FlatStyle = FlatStyle.Flat;
            btnToggleSidebar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnToggleSidebar.ForeColor = Color.White;
            btnToggleSidebar.Location = new Point(207, 28);
            btnToggleSidebar.Margin = new Padding(2);
            btnToggleSidebar.Name = "btnToggleSidebar";
            btnToggleSidebar.Size = new Size(31, 30);
            btnToggleSidebar.TabIndex = 1;
            btnToggleSidebar.Text = "☰";
            btnToggleSidebar.UseVisualStyleBackColor = false;
            // 
            // lblSidebarTitle
            // 
            lblSidebarTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSidebarTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSidebarTitle.ForeColor = Color.White;
            lblSidebarTitle.Location = new Point(0, 13);
            lblSidebarTitle.Margin = new Padding(2, 0, 2, 0);
            lblSidebarTitle.Name = "lblSidebarTitle";
            lblSidebarTitle.Padding = new Padding(16, 0, 39, 0);
            lblSidebarTitle.Size = new Size(260, 60);
            lblSidebarTitle.TabIndex = 0;
            lblSidebarTitle.Text = "📦 RMS Menu";
            lblSidebarTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.White;
            contentPanel.Controls.Add(lblSubtitle);
            contentPanel.Controls.Add(lblWelcome);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(260, 0);
            contentPanel.Margin = new Padding(2);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(31, 30, 31, 30);
            contentPanel.Size = new Size(1242, 683);
            contentPanel.TabIndex = 1;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 12F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(31, 90);
            lblSubtitle.Margin = new Padding(2, 0, 2, 0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(325, 21);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Select an option from the menu to get started";
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(30, 41, 59);
            lblWelcome.Location = new Point(31, 45);
            lblWelcome.Margin = new Padding(2, 0, 2, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(320, 51);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome to RMS";
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(contentPanel);
            Controls.Add(sidebarPanel);
            Margin = new Padding(2);
            Name = "MainPage";
            Size = new Size(1502, 683);
            sidebarPanel.ResumeLayout(false);
            sidebarHeader.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel sidebarHeader;
        private System.Windows.Forms.Button btnToggleSidebar;
        private System.Windows.Forms.Label lblSidebarTitle;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnPOS;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSubtitle;
    }
}
