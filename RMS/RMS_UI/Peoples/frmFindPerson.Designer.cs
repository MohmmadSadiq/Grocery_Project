namespace RMS_UI.Peoples
{
    partial class frmFindPerson
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ctrlFindPerson1 = new ctrlFindPerson();
            _btnSearch = new Button();
            btnAddPerson = new Button();
            SuspendLayout();
            // 
            // ctrlFindPerson1
            // 
            ctrlFindPerson1.BackColor = Color.FromArgb(245, 247, 250);
            ctrlFindPerson1.Dock = DockStyle.Fill;
            ctrlFindPerson1.Location = new Point(0, 0);
            ctrlFindPerson1.Name = "ctrlFindPerson1";
            ctrlFindPerson1.Size = new Size(520, 830);
            ctrlFindPerson1.TabIndex = 0;
            // 
            // _btnSearch
            // 
            _btnSearch.BackColor = Color.Red;
            _btnSearch.Cursor = Cursors.Hand;
            _btnSearch.FlatAppearance.BorderSize = 0;
            _btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            _btnSearch.FlatStyle = FlatStyle.Flat;
            _btnSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnSearch.ForeColor = Color.White;
            _btnSearch.Location = new Point(420, 790);
            _btnSearch.Name = "_btnSearch";
            _btnSearch.Size = new Size(88, 30);
            _btnSearch.TabIndex = 6;
            _btnSearch.Text = "Close";
            _btnSearch.UseVisualStyleBackColor = false;
            _btnSearch.Click += _btnSearch_Click;
            // 
            // btnAddPerson
            // 
            btnAddPerson.BackColor = Color.FromArgb(0, 192, 192);
            btnAddPerson.Cursor = Cursors.Hand;
            btnAddPerson.FlatAppearance.BorderSize = 0;
            btnAddPerson.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnAddPerson.FlatStyle = FlatStyle.Flat;
            btnAddPerson.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAddPerson.ForeColor = Color.White;
            btnAddPerson.Location = new Point(350, 12);
            btnAddPerson.Name = "btnAddPerson";
            btnAddPerson.Size = new Size(158, 30);
            btnAddPerson.TabIndex = 7;
            btnAddPerson.Text = "➕ Add New Person";
            btnAddPerson.UseVisualStyleBackColor = false;
            btnAddPerson.Click += btnAddPerson_Click;
            // 
            // frmFindPerson
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(520, 830);
            Controls.Add(btnAddPerson);
            Controls.Add(_btnSearch);
            Controls.Add(ctrlFindPerson1);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(520, 300);
            Name = "frmFindPerson";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🔍 Find Person";
            Load += frmFindPerson_Load;
            ResumeLayout(false);
        }

        #endregion

        private ctrlFindPerson ctrlFindPerson1;
        private Button _btnSearch;
        private Button btnAddPerson;
    }
}
