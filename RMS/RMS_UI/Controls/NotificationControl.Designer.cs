namespace RMS_UI.Controls
{
    partial class NotificationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _contentPanel;
        private System.Windows.Forms.Label _iconLabel;
        private System.Windows.Forms.Label _messageLabel;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Timer _showTimer;
        private System.Windows.Forms.Timer _hideTimer;
        private System.Windows.Forms.Timer _autoHideTimer;
        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_showTimer != null) _showTimer.Dispose();
                if (_hideTimer != null) _hideTimer.Dispose();
                if (_autoHideTimer != null) _autoHideTimer.Dispose();
                if (components != null) components.Dispose();
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
            _contentPanel = new Panel();
            _messageLabel = new Label();
            _iconLabel = new Label();
            _closeButton = new Button();
            _showTimer = new System.Windows.Forms.Timer(components);
            _hideTimer = new System.Windows.Forms.Timer(components);
            _autoHideTimer = new System.Windows.Forms.Timer(components);
            _contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _contentPanel
            // 
            _contentPanel.BackColor = Color.FromArgb(59, 130, 246);
            _contentPanel.Controls.Add(_messageLabel);
            _contentPanel.Controls.Add(_iconLabel);
            _contentPanel.Controls.Add(_closeButton);
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.Location = new Point(0, 0);
            _contentPanel.Name = "_contentPanel";
            _contentPanel.Padding = new Padding(15, 10, 15, 10);
            _contentPanel.Size = new Size(1527, 0);
            _contentPanel.TabIndex = 0;
            // 
            // _messageLabel
            // 
            _messageLabel.Dock = DockStyle.Fill;
            _messageLabel.Font = new Font("Segoe UI", 10F);
            _messageLabel.ForeColor = Color.White;
            _messageLabel.Location = new Point(48, 10);
            _messageLabel.Name = "_messageLabel";
            _messageLabel.Padding = new Padding(10, 0, 0, 0);
            _messageLabel.Size = new Size(1434, 0);
            _messageLabel.TabIndex = 1;
            _messageLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _iconLabel
            // 
            _iconLabel.AutoSize = true;
            _iconLabel.Dock = DockStyle.Left;
            _iconLabel.Font = new Font("Segoe UI", 14F);
            _iconLabel.ForeColor = Color.White;
            _iconLabel.Location = new Point(15, 10);
            _iconLabel.Name = "_iconLabel";
            _iconLabel.Size = new Size(33, 25);
            _iconLabel.TabIndex = 0;
            _iconLabel.Text = "ℹ";
            _iconLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _closeButton
            // 
            _closeButton.BackColor = Color.Transparent;
            _closeButton.Cursor = Cursors.Hand;
            _closeButton.Dock = DockStyle.Right;
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
            _closeButton.FlatStyle = FlatStyle.Flat;
            _closeButton.Font = new Font("Segoe UI", 10F);
            _closeButton.ForeColor = Color.White;
            _closeButton.Location = new Point(1482, 10);
            _closeButton.Name = "_closeButton";
            _closeButton.Size = new Size(30, 0);
            _closeButton.TabIndex = 2;
            _closeButton.Text = "✕";
            _closeButton.UseVisualStyleBackColor = false;
            _closeButton.Click += CloseButton_Click;
            // 
            // _showTimer
            // 
            _showTimer.Interval = 10;
            _showTimer.Tick += ShowTimer_Tick;
            // 
            // _hideTimer
            // 
            _hideTimer.Interval = 10;
            _hideTimer.Tick += HideTimer_Tick;
            // 
            // _autoHideTimer
            // 
            _autoHideTimer.Tick += AutoHideTimer_Tick;
            // 
            // NotificationControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_contentPanel);
            Name = "NotificationControl";
            Size = new Size(1527, 0);
            _contentPanel.ResumeLayout(false);
            _contentPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
