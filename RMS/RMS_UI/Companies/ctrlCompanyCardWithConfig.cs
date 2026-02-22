using RMS_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMS_UI.Companies
{

    public partial class ctrlCompanyCardWithConfig : UserControl
    {
        // Events
        public event EventHandler<CompanyEventArgs>? CompanyAdded;
        public event EventHandler<CompanyEventArgs>? CompanyEdited;
        public event EventHandler? CompanyCleared;

        [System.ComponentModel.DesignerSerializationVisibility(
   System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int? CompanyID
        {
            get => ctrlCompanyCard1.Company?.CompanyID ?? null;
            set { ctrlCompanyCard1.LoadCompany(value ?? -1); }
        }

        [System.ComponentModel.DesignerSerializationVisibility(
    System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public clsCompany? Company
        {
            get
            {
                return ctrlCompanyCard1.Company;
            }
            set
            {
                ctrlCompanyCard1.LoadCompany(value?.CompanyID ?? -1);
            }
        }
        public ctrlCompanyCardWithConfig()
        {
            InitializeComponent();
        }

        private void ctrlCompanyCard1_Load(object sender, EventArgs e)
        {

        }

        public void LoadCompany(int CompanyID)
        {
            ctrlCompanyCard1.LoadCompany(CompanyID);
        }

        private void _btnFindPerson_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void editCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAddEditCompany = new frmAddEditCompany(-1);
            if (frmAddEditCompany.ShowDialog() == DialogResult.OK)
            {
                if (frmAddEditCompany.SavedCompany?.CompanyID > 0)
                {
                    ctrlCompanyCard1.LoadCompany(frmAddEditCompany.SavedCompany?.CompanyID ?? -1);
                    CompanyAdded?.Invoke(this, new CompanyEventArgs(frmAddEditCompany.SavedCompany));
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctrlCompanyCard1.Company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var frmAddEditCompany = new frmAddEditCompany(ctrlCompanyCard1.Company.CompanyID);
            if (frmAddEditCompany.ShowDialog() == DialogResult.OK)
            {
                // Reload the company to refresh the data
                ctrlCompanyCard1.LoadCompany(ctrlCompanyCard1.Company.CompanyID);
                CompanyEdited?.Invoke(this, new CompanyEventArgs(ctrlCompanyCard1.Company));
            }
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ctrlCompanyCard1.Clear();
            CompanyCleared?.Invoke(this, EventArgs.Empty);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFindCompany frmFindCompany1 = new frmFindCompany();
            frmFindCompany1.CompanyFound += OnCompanyFound
                ;
            frmFindCompany1.ShowDialog();


        }

        private void OnCompanyFound(object? sender, clsCompany e)
        {

            LoadCompany(e.CompanyID);
            MessageBox.Show("Company Add Successfully");
            if(sender != null)
            ((Form)sender).Close(); 
        }
    }
    public class CompanyEventArgs : EventArgs
    {
        public clsCompany? Company { get; set; }

        public CompanyEventArgs(clsCompany? company)
        {
            Company = company;
        }
    }

}
