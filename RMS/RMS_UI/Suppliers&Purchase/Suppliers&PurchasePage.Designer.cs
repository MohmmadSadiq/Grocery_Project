namespace RMS_UI.Controls
{
    partial class Suppliers_PurchasePage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            SuppliersTab = new TabPage();
            tabPage2 = new TabPage();
            _tabNewPurchase = new TabPage();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(SuppliersTab);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(_tabNewPurchase);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1242, 683);
            tabControl1.TabIndex = 0;
            // 
            // SuppliersTab
            // 
            SuppliersTab.Location = new Point(4, 41);
            SuppliersTab.Name = "SuppliersTab";
            SuppliersTab.Padding = new Padding(3);
            SuppliersTab.Size = new Size(1234, 638);
            SuppliersTab.TabIndex = 0;
            SuppliersTab.Text = "👥📦 Suppliers";
            SuppliersTab.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(192, 72);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "📋📦Purchases";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // _tabNewPurchase
            // 
            _tabNewPurchase.Location = new Point(4, 24);
            _tabNewPurchase.Name = "_tabNewPurchase";
            _tabNewPurchase.Padding = new Padding(3);
            _tabNewPurchase.Size = new Size(192, 72);
            _tabNewPurchase.TabIndex = 2;
            _tabNewPurchase.Text = "➕ New Purchase";
            _tabNewPurchase.UseVisualStyleBackColor = true;
            // 
            // Suppliers_PurchasePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Name = "Suppliers_PurchasePage";
            Size = new Size(1242, 683);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage SuppliersTab;
        private TabPage tabPage2;
        private TabPage _tabNewPurchase;
    }
}
