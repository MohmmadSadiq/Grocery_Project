using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RMS_UI.Forms
{
    public partial class frmCompanyCard : Form
    {
        int number = 0;
        public frmCompanyCard(int number)
        {
            InitializeComponent();
            this.number = number;
            ctrlCompanyCard1.LoadCompany(number);
        }

        private void frmCompanyCard_Load(object sender, EventArgs e)
        {
            ctrlCompanyCard1.LoadCompany(number);

        }
    }
}
