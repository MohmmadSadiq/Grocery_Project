namespace RMS_UI.PaymentMethods
{
    partial class ctrlAddEditPaymentMethod
    {
        private System.ComponentModel.IContainer components = null;

        #region Designer Fields
        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblMode;

        private RMS_UI.Controls.NotificationControl _notification;

        private System.Windows.Forms.Panel _pnlButtons;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;

        private System.Windows.Forms.Panel _pnlContent;
        private System.Windows.Forms.Panel _pnlCard;

        private System.Windows.Forms.Label _lblMethodName;
        private System.Windows.Forms.TextBox _txtMethodName;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.TextBox _txtDescription;
        private System.Windows.Forms.CheckBox _chkIsActiveForSales;
        private System.Windows.Forms.CheckBox _chkIsActiveForPurchases;

        private System.Windows.Forms.Panel _pnlSep1;

        private System.Windows.Forms.ErrorProvider _errorProvider;
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _pnlHeader = new System.Windows.Forms.Panel();
            _lblTitle = new System.Windows.Forms.Label();
            _lblMode = new System.Windows.Forms.Label();
            _notification = new Controls.NotificationControl();
            _pnlButtons = new System.Windows.Forms.Panel();
            _btnSave = new System.Windows.Forms.Button();
            _btnCancel = new System.Windows.Forms.Button();
            _pnlContent = new System.Windows.Forms.Panel();
            _pnlCard = new System.Windows.Forms.Panel();
            _lblMethodName = new System.Windows.Forms.Label();
            _txtMethodName = new System.Windows.Forms.TextBox();
            _pnlSep1 = new System.Windows.Forms.Panel();
            _lblDescription = new System.Windows.Forms.Label();
            _txtDescription = new System.Windows.Forms.TextBox();
            _chkIsActiveForSales = new System.Windows.Forms.CheckBox();
            _chkIsActiveForPurchases = new System.Windows.Forms.CheckBox();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);

            _pnlHeader.SuspendLayout();
            _pnlButtons.SuspendLayout();
            _pnlContent.SuspendLayout();
            _pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();

            // ─── _pnlHeader ────────────────────────────────────────────────
            _pnlHeader.BackColor = System.Drawing.Color.White;
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlHeader.Location = new System.Drawing.Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            _pnlHeader.Size = new System.Drawing.Size(480, 72);
            _pnlHeader.TabIndex = 0;

            // ─── _lblTitle ─────────────────────────────────────────────────
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            _lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _lblTitle.Location = new System.Drawing.Point(24, 12);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(280, 30);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "💳  Add Payment Method";

            // ─── _lblMode ──────────────────────────────────────────────────
            _lblMode.AutoSize = true;
            _lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblMode.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblMode.Location = new System.Drawing.Point(26, 44);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new System.Drawing.Size(300, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Fill in the details below to add a new payment method.";

            // ─── _notification ─────────────────────────────────────────────
            _notification.AutoHideDuration = 4000;
            _notification.Dock = System.Windows.Forms.DockStyle.Top;
            _notification.Location = new System.Drawing.Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new System.Drawing.Size(480, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;

            // ─── _pnlButtons ──────────────────────────────────────────────
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            _pnlButtons.Location = new System.Drawing.Point(0, 415);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new System.Windows.Forms.Padding(0, 14, 24, 14);
            _pnlButtons.Size = new System.Drawing.Size(480, 65);
            _pnlButtons.TabIndex = 3;

            // ─── _btnSave ─────────────────────────────────────────────────
            _btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnSave.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            _btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            _btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnSave.ForeColor = System.Drawing.Color.White;
            _btnSave.Location = new System.Drawing.Point(334, 14);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new System.Drawing.Size(122, 38);
            _btnSave.TabIndex = 1;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = false;
            _btnSave.Click += _btnSave_Click;

            // ─── _btnCancel ───────────────────────────────────────────────
            _btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            _btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            _btnCancel.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _btnCancel.Location = new System.Drawing.Point(206, 14);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(122, 38);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            _btnCancel.Click += _btnCancel_Click;

            // ─── _pnlContent ─────────────────────────────────────────────
            _pnlContent.AutoScroll = true;
            _pnlContent.Controls.Add(_pnlCard);
            _pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlContent.Location = new System.Drawing.Point(0, 72);
            _pnlContent.Name = "_pnlContent";
            _pnlContent.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            _pnlContent.Size = new System.Drawing.Size(480, 343);
            _pnlContent.TabIndex = 2;

            // ─── _pnlCard ────────────────────────────────────────────────
            _pnlCard.Controls.Add(_chkIsActiveForPurchases);
            _pnlCard.Controls.Add(_chkIsActiveForSales);
            _pnlCard.Controls.Add(_txtDescription);
            _pnlCard.Controls.Add(_lblDescription);
            _pnlCard.Controls.Add(_pnlSep1);
            _pnlCard.Controls.Add(_txtMethodName);
            _pnlCard.Controls.Add(_lblMethodName);
            _pnlCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlCard.Location = new System.Drawing.Point(24, 16);
            _pnlCard.Name = "_pnlCard";
            _pnlCard.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            _pnlCard.Size = new System.Drawing.Size(432, 311);
            _pnlCard.TabIndex = 0;

            // ─── _lblMethodName ───────────────────────────────────────────
            _lblMethodName.AutoSize = true;
            _lblMethodName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _lblMethodName.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblMethodName.Location = new System.Drawing.Point(24, 20);
            _lblMethodName.Name = "_lblMethodName";
            _lblMethodName.Size = new System.Drawing.Size(105, 17);
            _lblMethodName.TabIndex = 0;
            _lblMethodName.Text = "Method Name *";

            // ─── _txtMethodName ───────────────────────────────────────────
            _txtMethodName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _txtMethodName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _txtMethodName.Font = new System.Drawing.Font("Segoe UI", 11F);
            _txtMethodName.Location = new System.Drawing.Point(24, 40);
            _txtMethodName.Name = "_txtMethodName";
            _txtMethodName.Size = new System.Drawing.Size(384, 27);
            _txtMethodName.TabIndex = 1;

            // ─── _pnlSep1 ────────────────────────────────────────────────
            _pnlSep1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _pnlSep1.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            _pnlSep1.Location = new System.Drawing.Point(24, 78);
            _pnlSep1.Name = "_pnlSep1";
            _pnlSep1.Size = new System.Drawing.Size(384, 1);
            _pnlSep1.TabIndex = 2;

            // ─── _lblDescription ──────────────────────────────────────────
            _lblDescription.AutoSize = true;
            _lblDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _lblDescription.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _lblDescription.Location = new System.Drawing.Point(24, 92);
            _lblDescription.Name = "_lblDescription";
            _lblDescription.Size = new System.Drawing.Size(76, 17);
            _lblDescription.TabIndex = 3;
            _lblDescription.Text = "Description";

            // ─── _txtDescription ──────────────────────────────────────────
            _txtDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _txtDescription.Font = new System.Drawing.Font("Segoe UI", 11F);
            _txtDescription.Location = new System.Drawing.Point(24, 112);
            _txtDescription.Multiline = true;
            _txtDescription.Name = "_txtDescription";
            _txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _txtDescription.Size = new System.Drawing.Size(384, 70);
            _txtDescription.TabIndex = 4;

            // ─── _chkIsActiveForSales ─────────────────────────────────────
            _chkIsActiveForSales.AutoSize = true;
            _chkIsActiveForSales.Checked = true;
            _chkIsActiveForSales.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkIsActiveForSales.Font = new System.Drawing.Font("Segoe UI", 10F);
            _chkIsActiveForSales.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            _chkIsActiveForSales.Location = new System.Drawing.Point(24, 200);
            _chkIsActiveForSales.Name = "_chkIsActiveForSales";
            _chkIsActiveForSales.Size = new System.Drawing.Size(138, 23);
            _chkIsActiveForSales.TabIndex = 5;
            _chkIsActiveForSales.Text = "Active for Sales";

            // ─── _chkIsActiveForPurchases ─────────────────────────────────
            _chkIsActiveForPurchases.AutoSize = true;
            _chkIsActiveForPurchases.Checked = true;
            _chkIsActiveForPurchases.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkIsActiveForPurchases.Font = new System.Drawing.Font("Segoe UI", 10F);
            _chkIsActiveForPurchases.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            _chkIsActiveForPurchases.Location = new System.Drawing.Point(24, 235);
            _chkIsActiveForPurchases.Name = "_chkIsActiveForPurchases";
            _chkIsActiveForPurchases.Size = new System.Drawing.Size(164, 23);
            _chkIsActiveForPurchases.TabIndex = 6;
            _chkIsActiveForPurchases.Text = "Active for Purchases";

            // ─── _errorProvider ───────────────────────────────────────────
            _errorProvider.ContainerControl = this;

            // ─── ctrlAddEditPaymentMethod ─────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_pnlContent);
            Controls.Add(_pnlButtons);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Name = "ctrlAddEditPaymentMethod";
            Size = new System.Drawing.Size(480, 480);

            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlButtons.ResumeLayout(false);
            _pnlContent.ResumeLayout(false);
            _pnlCard.ResumeLayout(false);
            _pnlCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
