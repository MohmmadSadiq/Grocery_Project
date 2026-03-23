namespace RMS_UI.Controls
{
    partial class ctrlAddEditEmployee
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblMode;
        private RMS_UI.Controls.NotificationControl _notification;
        private System.Windows.Forms.Panel _pnlButtons;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.TableLayoutPanel _splitLayout;
        private RMS_UI.Peoples.ctrlPersonCardWithConfig _personCard;
        private System.Windows.Forms.Panel _pnlEmployeeCard;
        private System.Windows.Forms.Label _lblEmployeeSection;
        private System.Windows.Forms.Label _lblPosition;
        private System.Windows.Forms.ComboBox _cmbPosition;
        private System.Windows.Forms.Label _lblHireDate;
        private System.Windows.Forms.DateTimePicker _dtpHireDate;
        private System.Windows.Forms.CheckBox _chkFireDateEnabled;
        private System.Windows.Forms.Label _lblFireDate;
        private System.Windows.Forms.DateTimePicker _dtpFireDate;
        private System.Windows.Forms.ErrorProvider _errorProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _pnlHeader = new System.Windows.Forms.Panel();
            _lblTitle = new System.Windows.Forms.Label();
            _lblMode = new System.Windows.Forms.Label();
            _notification = new RMS_UI.Controls.NotificationControl();
            _pnlButtons = new System.Windows.Forms.Panel();
            _btnSave = new System.Windows.Forms.Button();
            _btnCancel = new System.Windows.Forms.Button();
            _splitLayout = new System.Windows.Forms.TableLayoutPanel();
            _personCard = new RMS_UI.Peoples.ctrlPersonCardWithConfig();
            _pnlEmployeeCard = new System.Windows.Forms.Panel();
            _dtpFireDate = new System.Windows.Forms.DateTimePicker();
            _lblFireDate = new System.Windows.Forms.Label();
            _chkFireDateEnabled = new System.Windows.Forms.CheckBox();
            _dtpHireDate = new System.Windows.Forms.DateTimePicker();
            _lblHireDate = new System.Windows.Forms.Label();
            _cmbPosition = new System.Windows.Forms.ComboBox();
            _lblPosition = new System.Windows.Forms.Label();
            _lblEmployeeSection = new System.Windows.Forms.Label();
            _errorProvider = new System.Windows.Forms.ErrorProvider(components);
            _pnlHeader.SuspendLayout();
            _pnlButtons.SuspendLayout();
            _splitLayout.SuspendLayout();
            _pnlEmployeeCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Controls.Add(_lblMode);
            _pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlHeader.Location = new System.Drawing.Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            _pnlHeader.Size = new System.Drawing.Size(1120, 72);
            _pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            _lblTitle.Location = new System.Drawing.Point(24, 8);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new System.Drawing.Size(248, 28);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "🧑‍💼  Add New Employee";
            // 
            // _lblMode
            // 
            _lblMode.AutoSize = true;
            _lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblMode.Location = new System.Drawing.Point(26, 39);
            _lblMode.Name = "_lblMode";
            _lblMode.Size = new System.Drawing.Size(424, 15);
            _lblMode.TabIndex = 1;
            _lblMode.Text = "Link person information on the left and complete employee details on the right.";
            // 
            // _notification
            // 
            _notification.AutoHideDuration = 4000;
            _notification.Dock = System.Windows.Forms.DockStyle.Top;
            _notification.Location = new System.Drawing.Point(0, 72);
            _notification.Name = "_notification";
            _notification.Size = new System.Drawing.Size(1120, 0);
            _notification.TabIndex = 1;
            _notification.Visible = false;
            // 
            // _pnlButtons
            // 
            _pnlButtons.Controls.Add(_btnSave);
            _pnlButtons.Controls.Add(_btnCancel);
            _pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            _pnlButtons.Location = new System.Drawing.Point(0, 635);
            _pnlButtons.Name = "_pnlButtons";
            _pnlButtons.Padding = new System.Windows.Forms.Padding(0, 14, 24, 14);
            _pnlButtons.Size = new System.Drawing.Size(1120, 65);
            _pnlButtons.TabIndex = 3;
            // 
            // _btnSave
            // 
            _btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnSave.Location = new System.Drawing.Point(974, 14);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new System.Drawing.Size(122, 38);
            _btnSave.TabIndex = 1;
            _btnSave.Text = "💾  Save";
            _btnSave.UseVisualStyleBackColor = true;
            _btnSave.Click += _btnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            _btnCancel.Location = new System.Drawing.Point(844, 14);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(120, 38);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            _btnCancel.Click += _btnCancel_Click;
            // 
            // _splitLayout
            // 
            _splitLayout.ColumnCount = 2;
            _splitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            _splitLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            _splitLayout.Controls.Add(_personCard, 0, 0);
            _splitLayout.Controls.Add(_pnlEmployeeCard, 1, 0);
            _splitLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            _splitLayout.Location = new System.Drawing.Point(0, 72);
            _splitLayout.Name = "_splitLayout";
            _splitLayout.Padding = new System.Windows.Forms.Padding(20);
            _splitLayout.RowCount = 1;
            _splitLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _splitLayout.Size = new System.Drawing.Size(1120, 563);
            _splitLayout.TabIndex = 2;
            // 
            // _personCard
            // 
            _personCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _personCard.Location = new System.Drawing.Point(23, 23);
            _personCard.Name = "_personCard";
            _personCard.Size = new System.Drawing.Size(577, 517);
            _personCard.TabIndex = 0;
            // 
            // _pnlEmployeeCard
            // 
            _pnlEmployeeCard.Controls.Add(_dtpFireDate);
            _pnlEmployeeCard.Controls.Add(_lblFireDate);
            _pnlEmployeeCard.Controls.Add(_chkFireDateEnabled);
            _pnlEmployeeCard.Controls.Add(_dtpHireDate);
            _pnlEmployeeCard.Controls.Add(_lblHireDate);
            _pnlEmployeeCard.Controls.Add(_cmbPosition);
            _pnlEmployeeCard.Controls.Add(_lblPosition);
            _pnlEmployeeCard.Controls.Add(_lblEmployeeSection);
            _pnlEmployeeCard.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlEmployeeCard.Location = new System.Drawing.Point(606, 23);
            _pnlEmployeeCard.Name = "_pnlEmployeeCard";
            _pnlEmployeeCard.Padding = new System.Windows.Forms.Padding(20);
            _pnlEmployeeCard.Size = new System.Drawing.Size(491, 517);
            _pnlEmployeeCard.TabIndex = 1;
            _pnlEmployeeCard.Paint += _pnlEmployeeCard_Paint;
            // 
            // _dtpFireDate
            // 
            _dtpFireDate.Enabled = false;
            _dtpFireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            _dtpFireDate.Location = new System.Drawing.Point(20, 237);
            _dtpFireDate.Name = "_dtpFireDate";
            _dtpFireDate.ShowCheckBox = true;
            _dtpFireDate.Size = new System.Drawing.Size(445, 23);
            _dtpFireDate.TabIndex = 7;
            // 
            // _lblFireDate
            // 
            _lblFireDate.AutoSize = true;
            _lblFireDate.Location = new System.Drawing.Point(20, 219);
            _lblFireDate.Name = "_lblFireDate";
            _lblFireDate.Size = new System.Drawing.Size(52, 15);
            _lblFireDate.TabIndex = 6;
            _lblFireDate.Text = "Fire Date";
            // 
            // _chkFireDateEnabled
            // 
            _chkFireDateEnabled.AutoSize = true;
            _chkFireDateEnabled.Location = new System.Drawing.Point(20, 182);
            _chkFireDateEnabled.Name = "_chkFireDateEnabled";
            _chkFireDateEnabled.Size = new System.Drawing.Size(159, 19);
            _chkFireDateEnabled.TabIndex = 5;
            _chkFireDateEnabled.Text = "Employee has been fired";
            _chkFireDateEnabled.UseVisualStyleBackColor = true;
            _chkFireDateEnabled.CheckedChanged += _chkFireDateEnabled_CheckedChanged;
            // 
            // _dtpHireDate
            // 
            _dtpHireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            _dtpHireDate.Location = new System.Drawing.Point(20, 145);
            _dtpHireDate.Name = "_dtpHireDate";
            _dtpHireDate.Size = new System.Drawing.Size(445, 23);
            _dtpHireDate.TabIndex = 4;
            // 
            // _lblHireDate
            // 
            _lblHireDate.AutoSize = true;
            _lblHireDate.Location = new System.Drawing.Point(20, 127);
            _lblHireDate.Name = "_lblHireDate";
            _lblHireDate.Size = new System.Drawing.Size(55, 15);
            _lblHireDate.TabIndex = 3;
            _lblHireDate.Text = "Hire Date";
            // 
            // _cmbPosition
            // 
            _cmbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbPosition.FormattingEnabled = true;
            _cmbPosition.Location = new System.Drawing.Point(20, 84);
            _cmbPosition.Name = "_cmbPosition";
            _cmbPosition.Size = new System.Drawing.Size(445, 23);
            _cmbPosition.TabIndex = 2;
            // 
            // _lblPosition
            // 
            _lblPosition.AutoSize = true;
            _lblPosition.Location = new System.Drawing.Point(20, 66);
            _lblPosition.Name = "_lblPosition";
            _lblPosition.Size = new System.Drawing.Size(47, 15);
            _lblPosition.TabIndex = 1;
            _lblPosition.Text = "Position";
            // 
            // _lblEmployeeSection
            // 
            _lblEmployeeSection.AutoSize = true;
            _lblEmployeeSection.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            _lblEmployeeSection.Location = new System.Drawing.Point(20, 20);
            _lblEmployeeSection.Name = "_lblEmployeeSection";
            _lblEmployeeSection.Size = new System.Drawing.Size(136, 21);
            _lblEmployeeSection.TabIndex = 0;
            _lblEmployeeSection.Text = "Employee Details";
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // ctrlAddEditEmployee
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_splitLayout);
            Controls.Add(_pnlButtons);
            Controls.Add(_notification);
            Controls.Add(_pnlHeader);
            Name = "ctrlAddEditEmployee";
            Size = new System.Drawing.Size(1120, 700);
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlButtons.ResumeLayout(false);
            _splitLayout.ResumeLayout(false);
            _pnlEmployeeCard.ResumeLayout(false);
            _pnlEmployeeCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }
    }
}
