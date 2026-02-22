using System;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Peoples
{
    public partial class frmFindPerson : Form
    {
        public event EventHandler<clsPerson>? PersonFound;

        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void frmFindPerson_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // Subscribe to PersonFound event
            ctrlFindPerson1.PersonFound += CtrlFindPerson_PersonFound;

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void CtrlFindPerson_PersonFound(object? sender, clsPerson person)
        {
            // When a person is found, update the form title
            this.Text = $"Find Person - Found: {person.FullName} (ID: {person.PersonID})";

            // Optional: Log the event or perform other operations
            // For example, you could enable additional buttons or functionality
            PersonFound?.Invoke(this, person);


        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;
            BackColor = colors.FormBackground;
        }

        private void _btnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _GetSavedPerson(object? sender, clsPerson e)
        {
            ctrlFindPerson1.LoadPerson(e);
            PersonFound?.Invoke(this, e);

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmPersonDialog frmPersonDialog1 = new frmPersonDialog();
            frmPersonDialog1.PersonSaved += _GetSavedPerson;
            frmPersonDialog1.ShowDialog();

        }
    }
}
