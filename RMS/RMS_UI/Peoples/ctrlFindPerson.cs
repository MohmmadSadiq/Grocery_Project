using System;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Peoples
{
    public partial class ctrlFindPerson : UserControl
    {
        // -----------------------------------------------------------------------
        // Event: fired only when a person is successfully found
        // -----------------------------------------------------------------------
        public event EventHandler<clsPerson>? PersonFound;

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------
        public ctrlFindPerson()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // Populate search-type options
            _cmbSearchBy.Items.AddRange(new object[]
            {
                "By ID",
                "By Phone",
                "By Email",
                "By National No"
            });
            _cmbSearchBy.SelectedIndex = 0;

            // Keyboard shortcut: Enter triggers search
            _txtSearchValue.KeyDown += (sender, e) => _txtSearchValue_KeyDown(sender!, e);

            // Theme
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        public ctrlFindPerson(int PersonID)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // Populate search-type options
            _cmbSearchBy.Items.AddRange(new object[]
            {
                "By ID",
                "By Phone",
                "By Email",
                "By National No"
            });
            _cmbSearchBy.SelectedIndex = 0;

            // Keyboard shortcut: Enter triggers search
            _txtSearchValue.KeyDown += (sender, e) => _txtSearchValue_KeyDown(sender!, e);

            // Theme
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
            _pnlTop.Enabled = false;
        }

        // -----------------------------------------------------------------------
        // Search Logic
        // -----------------------------------------------------------------------
        private void _btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void _txtSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PerformSearch();
        }

        private void PerformSearch()
        {
            string searchValue = _txtSearchValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                _notification.ShowWarning("Please enter a search value.");
                return;
            }

            clsPerson? person = null;

            try
            {
                switch (_cmbSearchBy.SelectedIndex)
                {
                    case 0: // By ID
                        if (int.TryParse(searchValue, out int id))
                            person = clsPerson.Find(id);
                        else
                        {
                            _notification.ShowWarning("ID must be a valid number.");
                            return;
                        }
                        break;

                    case 1: // By Phone
                        person = clsPerson.FindByPhone(searchValue);
                        break;

                    case 2: // By Email
                        person = clsPerson.FindByEmail(searchValue);
                        break;

                    case 3: // By National No
                        person = clsPerson.FindByNationalNo(searchValue);
                        break;
                }
            }
            catch (Exception)
            {
                _notification.ShowError("An error occurred while searching.");
                return;
            }

            if (person != null)
            {
                _ctrlPersonCard.LoadPerson(person);
                _notification.HideImmediately();
                PersonFound?.Invoke(this, person);
            }
            else
            {
                _notification.ShowError($"No person found with the given {_cmbSearchBy.Text.Replace("By ", "")}.");
            }
        }

        public void LoadPerson (clsPerson person)
        {
            _ctrlPersonCard.LoadPerson(person);
            _pnlTop.Enabled = false;
        }

        // -----------------------------------------------------------------------
        // Theme
        // -----------------------------------------------------------------------
        private void ApplyTheme()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            var colors = ThemeManager.Colors;

            BackColor           = colors.FormBackground;
            _pnlTop.BackColor   = colors.ContentBackground;
            _lblTitle.ForeColor = colors.TitleText;

            _lblSearchBy.ForeColor    = colors.SecondaryText;
            _cmbSearchBy.BackColor    = colors.ContentBackground;
            _cmbSearchBy.ForeColor    = colors.PrimaryText;

            _lblSearchValue.ForeColor = colors.SecondaryText;
            _txtSearchValue.BackColor = colors.ContentBackground;
            _txtSearchValue.ForeColor = colors.PrimaryText;
            _txtSearchValue.BorderStyle = BorderStyle.FixedSingle;

            _btnSearch.BackColor = colors.Primary;
            _btnSearch.ForeColor = Color.White;
            _btnSearch.FlatAppearance.BorderColor      = colors.Primary;
            _btnSearch.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;

            Invalidate();
        }
    }
}
