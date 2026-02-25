using RMS_UI.Companies;
using RMS_UI.Peoples;

namespace RMS_UI.Suppliers_Purchase
{
    partial class ctrlBusinessPartners
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
            tabPerson = new TabPage();
            ctrlPersonCardWithConfig1 = new ctrlPersonCardWithConfig();
            tabCompany = new TabPage();
            ctrlCompanyCardWithConfig1 = new ctrlCompanyCardWithConfig();
            tabControl1.SuspendLayout();
            tabPerson.SuspendLayout();
            tabCompany.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPerson);
            tabControl1.Controls.Add(tabCompany);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(447, 563);
            tabControl1.TabIndex = 0;
            tabControl1.Selecting += tabControl1_Selecting;
            // 
            // tabPerson
            // 
            tabPerson.Controls.Add(ctrlPersonCardWithConfig1);
            tabPerson.Location = new Point(4, 30);
            tabPerson.Name = "tabPerson";
            tabPerson.Padding = new Padding(3);
            tabPerson.Size = new Size(439, 529);
            tabPerson.TabIndex = 0;
            tabPerson.Text = "Person Info";
            tabPerson.UseVisualStyleBackColor = true;
            // 
            // ctrlPersonCardWithConfig1
            // 
            ctrlPersonCardWithConfig1.BackColor = Color.Transparent;
            ctrlPersonCardWithConfig1.Dock = DockStyle.Fill;
            ctrlPersonCardWithConfig1.Location = new Point(3, 3);
            ctrlPersonCardWithConfig1.Name = "ctrlPersonCardWithConfig1";
            ctrlPersonCardWithConfig1.Size = new Size(433, 523);
            ctrlPersonCardWithConfig1.TabIndex = 1;
            // 
            // tabCompany
            // 
            tabCompany.Controls.Add(ctrlCompanyCardWithConfig1);
            tabCompany.Location = new Point(4, 30);
            tabCompany.Name = "tabCompany";
            tabCompany.Padding = new Padding(3);
            tabCompany.Size = new Size(439, 529);
            tabCompany.TabIndex = 1;
            tabCompany.Text = "Company Info";
            tabCompany.UseVisualStyleBackColor = true;
            // 
            // ctrlCompanyCardWithConfig1
            // 
            ctrlCompanyCardWithConfig1.BackColor = Color.White;
            ctrlCompanyCardWithConfig1.Dock = DockStyle.Fill;
            ctrlCompanyCardWithConfig1.Location = new Point(3, 3);
            ctrlCompanyCardWithConfig1.Name = "ctrlCompanyCardWithConfig1";
            ctrlCompanyCardWithConfig1.Size = new Size(433, 523);
            ctrlCompanyCardWithConfig1.TabIndex = 0;
            // 
            // ctrlBusinessPartners
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Name = "ctrlBusinessPartners";
            Size = new Size(447, 563);
            tabControl1.ResumeLayout(false);
            tabPerson.ResumeLayout(false);
            tabCompany.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPerson;
        private TabPage tabCompany;
        private Peoples.ctrlPersonCardWithConfig ctrlPersonCardWithConfig1;
        private Companies.ctrlCompanyCardWithConfig ctrlCompanyCardWithConfig1;
    }
}
