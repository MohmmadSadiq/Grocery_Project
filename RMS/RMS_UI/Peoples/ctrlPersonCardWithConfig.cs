using RMS_Business;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RMS_UI.Peoples
{
    public partial class ctrlPersonCardWithConfig  : UserControl 
    {
        // Events
        public event EventHandler<PersonEventArgs>? PersonAdded;
        public event EventHandler<PersonEventArgs>? PersonEdited;
        public event EventHandler? PersonCleared;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public clsPerson? Person
        {
            get => ctrlPersonCard1.Person;
            set => ctrlPersonCard1.LoadPerson(value);
        }

        public ctrlPersonCardWithConfig()
        {
            InitializeComponent();
        }

        public void LoadPerson(clsPerson? person)
        {
            ctrlPersonCard1.LoadPerson(person);
        }

        private void _btnConfig_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFindPerson frmFindPerson1 = new frmFindPerson();
            frmFindPerson1.PersonFound += OnPersonFound;
            frmFindPerson1.ShowDialog();
        }

        private void OnPersonFound(object? sender, clsPerson person)
        {
            LoadPerson(person);
            PersonAdded?.Invoke(this, new PersonEventArgs(person));
            if (sender != null)
                ((Form)sender).Close();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmPersonDialog1 = new frmPersonDialog();
            frmPersonDialog1.PersonSaved += (s, person) =>
            {
                LoadPerson(person);
                PersonAdded?.Invoke(this, new PersonEventArgs(person));
            };
            frmPersonDialog1.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCard1.Person == null)
            {
                MessageBox.Show("Please select a person first.", "No Person Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var frmPersonDialog1 = new frmPersonDialog(ctrlPersonCard1.Person.PersonID);
            frmPersonDialog1.PersonSaved += (s, person) =>
            {
                LoadPerson(person);
                PersonEdited?.Invoke(this, new PersonEventArgs(person));
            };
            frmPersonDialog1.ShowDialog();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPerson(null);
            ctrlPersonCard1.Clear();
            PersonCleared?.Invoke(this, EventArgs.Empty);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }

    public class PersonEventArgs : EventArgs
    {
        public clsPerson? Person { get; set; }

        public PersonEventArgs(clsPerson? person)
        {
            Person = person;
        }
    }
}
