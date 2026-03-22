namespace RMS_UI.Peoples
{
    partial class ctrlPersonCardWithConfig
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
            ctrlPersonCard1 = new Controls.ctrlPersonCard();
            contextMenuStrip1 = new ContextMenuStrip(components);
            findToolStripMenuItem = new ToolStripMenuItem();
            addNewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            _btnConfig = new Button();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlPersonCard1
            // 
            ctrlPersonCard1.BackColor = Color.FromArgb(255, 255, 255);
            ctrlPersonCard1.ContextMenuStrip = contextMenuStrip1;
            ctrlPersonCard1.Location = new Point(0, 0);
            ctrlPersonCard1.Margin = new Padding(10);
            ctrlPersonCard1.Name = "ctrlPersonCard1";
            ctrlPersonCard1.Size = new Size(467, 540);
            ctrlPersonCard1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { findToolStripMenuItem, addNewToolStripMenuItem, editToolStripMenuItem, clearToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(139, 92);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new Size(138, 22);
            findToolStripMenuItem.Text = "🔎 Find";
            findToolStripMenuItem.Click += findToolStripMenuItem_Click;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(138, 22);
            addNewToolStripMenuItem.Text = "➕ Add New";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(138, 22);
            editToolStripMenuItem.Text = "✍️ Edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(138, 22);
            clearToolStripMenuItem.Text = "\U0001f9f9 Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
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
            _btnConfig.Location = new Point(401, 9);
            _btnConfig.Name = "_btnConfig";
            _btnConfig.Size = new Size(47, 31);
            _btnConfig.TabIndex = 1;
            _btnConfig.Text = "⚙️";
            _btnConfig.UseVisualStyleBackColor = false;
            _btnConfig.Click += _btnConfig_Click;
            // 
            // ctrlPersonCardWithConfig
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Transparent;
            Controls.Add(_btnConfig);
            Controls.Add(ctrlPersonCard1);
            Name = "ctrlPersonCardWithConfig";
            Size = new Size(467, 541);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.ctrlPersonCard ctrlPersonCard1;
        private Button _btnConfig;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
    }
}
