namespace RMS_UI.Controls
{
    partial class ModernTitleBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _logoPanel;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Panel _buttonPanel;
        private ModernTitleBarButton _closeButton;
        private ModernTitleBarButton _maximizeButton;
        private ModernTitleBarButton _minimizeButton;
        private ModernTitleBarButton _themeToggleButton;
        #endregion

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
            this.components = new System.ComponentModel.Container();
            this._logoPanel = new System.Windows.Forms.Panel();
            this._titleLabel = new System.Windows.Forms.Label();
            this._buttonPanel = new System.Windows.Forms.Panel();
            this._themeToggleButton = new RMS_UI.Controls.ModernTitleBarButton();
            this._minimizeButton = new RMS_UI.Controls.ModernTitleBarButton();
            this._maximizeButton = new RMS_UI.Controls.ModernTitleBarButton();
            this._closeButton = new RMS_UI.Controls.ModernTitleBarButton();
            this._buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _logoPanel
            // 
            this._logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._logoPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this._logoPanel.Location = new System.Drawing.Point(15, 12);
            this._logoPanel.Name = "_logoPanel";
            this._logoPanel.Size = new System.Drawing.Size(45, 45);
            this._logoPanel.TabIndex = 0;
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular);
            this._titleLabel.Location = new System.Drawing.Point(70, 20);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(108, 30);
            this._titleLabel.TabIndex = 1;
            this._titleLabel.Text = "Application";
            // 
            // _buttonPanel
            // 
            this._buttonPanel.Controls.Add(this._themeToggleButton);
            this._buttonPanel.Controls.Add(this._minimizeButton);
            this._buttonPanel.Controls.Add(this._maximizeButton);
            this._buttonPanel.Controls.Add(this._closeButton);
            this._buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonPanel.Location = new System.Drawing.Point(620, 0);
            this._buttonPanel.Name = "_buttonPanel";
            this._buttonPanel.Padding = new System.Windows.Forms.Padding(0);
            this._buttonPanel.Size = new System.Drawing.Size(180, 70);
            this._buttonPanel.TabIndex = 2;
            // 
            // _themeToggleButton
            // 
            this._themeToggleButton.ButtonType = RMS_UI.Controls.TitleBarButtonType.ThemeToggle;
            this._themeToggleButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._themeToggleButton.Location = new System.Drawing.Point(0, 0);
            this._themeToggleButton.Name = "_themeToggleButton";
            this._themeToggleButton.Size = new System.Drawing.Size(45, 35);
            this._themeToggleButton.TabIndex = 0;
            this._themeToggleButton.Click += new System.EventHandler(this.ThemeToggleButton_Click);
            // 
            // _minimizeButton
            // 
            this._minimizeButton.ButtonType = RMS_UI.Controls.TitleBarButtonType.Minimize;
            this._minimizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._minimizeButton.Location = new System.Drawing.Point(45, 0);
            this._minimizeButton.Name = "_minimizeButton";
            this._minimizeButton.Size = new System.Drawing.Size(45, 35);
            this._minimizeButton.TabIndex = 1;
            this._minimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // _maximizeButton
            // 
            this._maximizeButton.ButtonType = RMS_UI.Controls.TitleBarButtonType.Maximize;
            this._maximizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._maximizeButton.Location = new System.Drawing.Point(90, 0);
            this._maximizeButton.Name = "_maximizeButton";
            this._maximizeButton.Size = new System.Drawing.Size(45, 35);
            this._maximizeButton.TabIndex = 2;
            this._maximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.ButtonType = RMS_UI.Controls.TitleBarButtonType.Close;
            this._closeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._closeButton.Location = new System.Drawing.Point(135, 0);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(45, 35);
            this._closeButton.TabIndex = 3;
            this._closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ModernTitleBar
            // 
            this.Controls.Add(this._buttonPanel);
            this.Controls.Add(this._titleLabel);
            this.Controls.Add(this._logoPanel);
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.Name = "ModernTitleBar";
            this.Padding = new System.Windows.Forms.Padding(15, 0, 10, 0);
            this.Size = new System.Drawing.Size(800, 70);
            this.Resize += new System.EventHandler(this.ModernTitleBar_Resize);
            this.HandleCreated += new System.EventHandler(this.ModernTitleBar_HandleCreated);
            this._buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
