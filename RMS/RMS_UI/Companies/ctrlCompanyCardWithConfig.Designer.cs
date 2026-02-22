namespace RMS_UI.Companies
{
    partial class ctrlCompanyCardWithConfig
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
            components = new System.ComponentModel.Container();
            ctrlCompanyCard1 = new Controls.ctrlCompanyCard();
            contextMenuStrip1 = new ContextMenuStrip(components);
            addNewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem1 = new ToolStripMenuItem();
            addNewCompanyToolStripMenuItem = new ToolStripMenuItem();
            editCompanyToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            _btnConfig = new Button();
            findToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlCompanyCard1
            // 
            ctrlCompanyCard1.BackColor = Color.FromArgb(255, 255, 255);
            ctrlCompanyCard1.ContextMenuStrip = contextMenuStrip1;
            ctrlCompanyCard1.Location = new Point(0, 0);
            ctrlCompanyCard1.Margin = new Padding(10);
            ctrlCompanyCard1.Name = "ctrlCompanyCard1";
            ctrlCompanyCard1.Size = new Size(397, 435);
            ctrlCompanyCard1.TabIndex = 0;
            ctrlCompanyCard1.Load += ctrlCompanyCard1_Load;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { findToolStripMenuItem, addNewToolStripMenuItem, editToolStripMenuItem, clearToolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 114);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(180, 22);
            addNewToolStripMenuItem.Text = "➕ Add New";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(180, 22);
            editToolStripMenuItem.Text = "✍️ Edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem1
            // 
            clearToolStripMenuItem1.Name = "clearToolStripMenuItem1";
            clearToolStripMenuItem1.Size = new Size(180, 22);
            clearToolStripMenuItem1.Text = "\U0001f9f9 Clear";
            clearToolStripMenuItem1.Click += clearToolStripMenuItem1_Click;
            // 
            // addNewCompanyToolStripMenuItem
            // 
            addNewCompanyToolStripMenuItem.Name = "addNewCompanyToolStripMenuItem";
            addNewCompanyToolStripMenuItem.Size = new Size(193, 22);
            addNewCompanyToolStripMenuItem.Text = "➕ Add New Company";
            // 
            // editCompanyToolStripMenuItem
            // 
            editCompanyToolStripMenuItem.Name = "editCompanyToolStripMenuItem";
            editCompanyToolStripMenuItem.Size = new Size(193, 22);
            editCompanyToolStripMenuItem.Text = "✍️  Edit Company";
            editCompanyToolStripMenuItem.Click += editCompanyToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(193, 22);
            clearToolStripMenuItem.Text = "\U0001f9f9 Clear";
            // 
            // _btnConfig
            // 
            _btnConfig.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnConfig.BackColor = Color.FromArgb(219, 234, 254);
            _btnConfig.ContextMenuStrip = contextMenuStrip1;
            _btnConfig.Cursor = Cursors.Hand;
            _btnConfig.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
            _btnConfig.FlatAppearance.MouseOverBackColor = Color.FromArgb(191, 219, 254);
            _btnConfig.FlatStyle = FlatStyle.Flat;
            _btnConfig.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnConfig.ForeColor = Color.FromArgb(37, 99, 235);
            _btnConfig.Location = new Point(331, 9);
            _btnConfig.Name = "_btnConfig";
            _btnConfig.Size = new Size(47, 31);
            _btnConfig.TabIndex = 16;
            _btnConfig.Text = "⚙️";
            _btnConfig.UseVisualStyleBackColor = false;
            _btnConfig.Click += _btnFindPerson_Click;
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new Size(180, 22);
            findToolStripMenuItem.Text = "🔎 Find";
            findToolStripMenuItem.Click += findToolStripMenuItem_Click;
            // 
            // ctrlCompanyCardWithConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(_btnConfig);
            Controls.Add(ctrlCompanyCard1);
            Name = "ctrlCompanyCardWithConfig";
            Size = new Size(397, 435);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.ctrlCompanyCard ctrlCompanyCard1;
        private Button _btnConfig;
        private ToolStripMenuItem addNewCompanyToolStripMenuItem;
        private ToolStripMenuItem editCompanyToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem1;
        private ToolStripMenuItem findToolStripMenuItem;
    }
}
