namespace RMS_UI.Forms
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelShell = new TableLayoutPanel();
            panelBranding = new Panel();
            lblCapabilityBody = new Label();
            lblCapabilityTitle = new Label();
            lblBrandTagline = new Label();
            lblBrandName = new Label();
            panelCardHost = new Panel();
            panelCardBorder = new Panel();
            panelCard = new Panel();
            lblError = new Label();
            btnCancel = new Button();
            btnLogin = new Button();
            chkShowPassword = new CheckBox();
            chkRememberMe = new CheckBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUserName = new TextBox();
            lblUserName = new Label();
            lblSubTitle = new Label();
            lblWelcome = new Label();
            panelShell.SuspendLayout();
            panelBranding.SuspendLayout();
            panelCardHost.SuspendLayout();
            panelCardBorder.SuspendLayout();
            panelCard.SuspendLayout();
            SuspendLayout();
            // 
            // panelShell
            // 
            panelShell.ColumnCount = 2;
            panelShell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            panelShell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            panelShell.Controls.Add(panelBranding, 0, 0);
            panelShell.Controls.Add(panelCardHost, 1, 0);
            panelShell.Dock = DockStyle.Fill;
            panelShell.Location = new Point(0, 0);
            panelShell.Name = "panelShell";
            panelShell.RowCount = 1;
            panelShell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelShell.Size = new Size(1000, 620);
            panelShell.TabIndex = 0;
            // 
            // panelBranding
            // 
            panelBranding.Controls.Add(lblCapabilityBody);
            panelBranding.Controls.Add(lblCapabilityTitle);
            panelBranding.Controls.Add(lblBrandTagline);
            panelBranding.Controls.Add(lblBrandName);
            panelBranding.Dock = DockStyle.Fill;
            panelBranding.Location = new Point(0, 0);
            panelBranding.Margin = new Padding(0);
            panelBranding.Name = "panelBranding";
            panelBranding.Padding = new Padding(48, 54, 48, 54);
            panelBranding.Size = new Size(520, 620);
            panelBranding.TabIndex = 0;
            panelBranding.Paint += panelBranding_Paint;
            // 
            // lblCapabilityBody
            // 
            lblCapabilityBody.AutoSize = true;
            lblCapabilityBody.Font = new Font("Segoe UI", 10F);
            lblCapabilityBody.Location = new Point(53, 330);
            lblCapabilityBody.MaximumSize = new Size(380, 0);
            lblCapabilityBody.Name = "lblCapabilityBody";
            lblCapabilityBody.Size = new Size(362, 38);
            lblCapabilityBody.TabIndex = 5;
            lblCapabilityBody.Text = "Inventory precision, purchasing control, and fast POS operations in one reliable system for daily retail execution.";
            // 
            // lblCapabilityTitle
            // 
            lblCapabilityTitle.AutoSize = true;
            lblCapabilityTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblCapabilityTitle.Location = new Point(53, 292);
            lblCapabilityTitle.Name = "lblCapabilityTitle";
            lblCapabilityTitle.Size = new Size(181, 21);
            lblCapabilityTitle.TabIndex = 4;
            lblCapabilityTitle.Text = "Business Power at Scale";
            // 
            // lblBrandTagline
            // 
            lblBrandTagline.AutoSize = true;
            lblBrandTagline.Font = new Font("Segoe UI", 12F);
            lblBrandTagline.Location = new Point(53, 134);
            lblBrandTagline.MaximumSize = new Size(390, 0);
            lblBrandTagline.Name = "lblBrandTagline";
            lblBrandTagline.Size = new Size(390, 42);
            lblBrandTagline.TabIndex = 1;
            lblBrandTagline.Text = "Unified retail management for confident decisions and high-performance daily operations.";
            // 
            // lblBrandName
            // 
            lblBrandName.AutoSize = true;
            lblBrandName.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblBrandName.Location = new Point(46, 72);
            lblBrandName.Name = "lblBrandName";
            lblBrandName.Size = new Size(97, 47);
            lblBrandName.TabIndex = 0;
            lblBrandName.Text = "RMS";
            // 
            // panelCardHost
            // 
            panelCardHost.Controls.Add(panelCardBorder);
            panelCardHost.Dock = DockStyle.Fill;
            panelCardHost.Location = new Point(520, 0);
            panelCardHost.Margin = new Padding(0);
            panelCardHost.Name = "panelCardHost";
            panelCardHost.Padding = new Padding(44);
            panelCardHost.Size = new Size(480, 620);
            panelCardHost.TabIndex = 1;
            // 
            // panelCardBorder
            // 
            panelCardBorder.Controls.Add(panelCard);
            panelCardBorder.Dock = DockStyle.Fill;
            panelCardBorder.Location = new Point(44, 44);
            panelCardBorder.Name = "panelCardBorder";
            panelCardBorder.Padding = new Padding(1);
            panelCardBorder.Size = new Size(392, 532);
            panelCardBorder.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.Controls.Add(lblError);
            panelCard.Controls.Add(btnCancel);
            panelCard.Controls.Add(btnLogin);
            panelCard.Controls.Add(chkShowPassword);
            panelCard.Controls.Add(chkRememberMe);
            panelCard.Controls.Add(txtPassword);
            panelCard.Controls.Add(lblPassword);
            panelCard.Controls.Add(txtUserName);
            panelCard.Controls.Add(lblUserName);
            panelCard.Controls.Add(lblSubTitle);
            panelCard.Controls.Add(lblWelcome);
            panelCard.Dock = DockStyle.Fill;
            panelCard.Location = new Point(1, 1);
            panelCard.Name = "panelCard";
            panelCard.Padding = new Padding(30, 34, 30, 30);
            panelCard.Size = new Size(390, 530);
            panelCard.TabIndex = 0;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblError.Location = new Point(34, 348);
            lblError.MaximumSize = new Size(320, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 17);
            lblError.TabIndex = 10;
            lblError.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCancel.Location = new Point(34, 418);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(320, 42);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Exit";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnLogin
            // 
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLogin.Location = new Point(34, 370);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(320, 42);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Sign In";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.Location = new Point(190, 312);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(108, 19);
            chkShowPassword.TabIndex = 7;
            chkShowPassword.Text = "Show password";
            chkShowPassword.UseVisualStyleBackColor = true;
            // 
            // chkRememberMe
            // 
            chkRememberMe.AutoSize = true;
            chkRememberMe.Font = new Font("Segoe UI", 9.2F);
            chkRememberMe.Location = new Point(34, 312);
            chkRememberMe.Name = "chkRememberMe";
            chkRememberMe.Size = new Size(113, 21);
            chkRememberMe.TabIndex = 6;
            chkRememberMe.Text = "Remember me";
            chkRememberMe.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(34, 271);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(320, 25);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblPassword.Location = new Point(34, 247);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(68, 19);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password";
            // 
            // txtUserName
            // 
            txtUserName.BorderStyle = BorderStyle.FixedSingle;
            txtUserName.Font = new Font("Segoe UI", 10F);
            txtUserName.Location = new Point(34, 199);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(320, 25);
            txtUserName.TabIndex = 3;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblUserName.Location = new Point(34, 175);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(71, 19);
            lblUserName.TabIndex = 2;
            lblUserName.Text = "Username";
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 10F);
            lblSubTitle.Location = new Point(34, 113);
            lblSubTitle.MaximumSize = new Size(320, 0);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(296, 38);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Sign in to continue managing retail operations with secure, accountable access.";
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblWelcome.Location = new Point(31, 58);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(192, 36);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome Back";
            // 
            // frmLogin
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1000, 620);
            Controls.Add(panelShell);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(1016, 659);
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RMS Login";
            panelShell.ResumeLayout(false);
            panelBranding.ResumeLayout(false);
            panelBranding.PerformLayout();
            panelCardHost.ResumeLayout(false);
            panelCardBorder.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelShell;
        private System.Windows.Forms.Panel panelBranding;
        private System.Windows.Forms.Label lblCapabilityBody;
        private System.Windows.Forms.Label lblCapabilityTitle;
        private System.Windows.Forms.Label lblBrandTagline;
        private System.Windows.Forms.Label lblBrandName;
        private System.Windows.Forms.Panel panelCardHost;
        private System.Windows.Forms.Panel panelCardBorder;
        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblError;
    }
}
