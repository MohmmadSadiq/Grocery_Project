using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    public partial class frmPersonCard : Form
    {
        private int _personID = -1;
        private clsPerson? _person = null;

        // Constructor بدون معاملات - للـ Designer
        public frmPersonCard()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        // Constructor يقبل PersonID
        public frmPersonCard(int personID) : this()
        {
            if (personID > 0)
            {
                _personID = personID;
            }
        }

        private void frmPersonCard_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // تحميل بيانات الشخص إذا تم تعيين ID
            if (_personID > 0)
            {
                LoadPersonData();
            }
        }

        private void LoadPersonData()
        {
            _person = clsPerson.Find(_personID);

            if (_person != null)
            {
                ctrlPersonCard1.LoadPerson(_person);
                this.Text = $"Person Details - {_person.FullName}";
            }
            else
            {
                MessageBox.Show("Person not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;
            BackColor = colors.FormBackground;
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
