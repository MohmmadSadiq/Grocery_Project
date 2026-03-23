using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Peoples;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class ctrlAddEditEmployee : UserControl
    {
        public event EventHandler<clsEmployee>? EmployeeSaved;
        public event EventHandler? CancelClicked;

        private enum enMode { AddNew = 1, Edit = 2 }

        private clsEmployee _employee;
        private enMode _mode;
        private int? _selectedPersonID;

        public ctrlAddEditEmployee()
        {
            _mode = enMode.AddNew;
            _employee = new clsEmployee();
            InitializeComponent();
            InitializeControl();
        }

        public ctrlAddEditEmployee(int employeeId)
        {
            _employee = clsEmployee.Find(employeeId) ?? new clsEmployee();
            _mode = _employee.EmployeeID > 0 ? enMode.Edit : enMode.AddNew;
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            _personCard.PersonAdded += PersonCard_PersonAdded;
            _personCard.PersonEdited += PersonCard_PersonEdited;
            _personCard.PersonCleared += PersonCard_PersonCleared;

            PopulatePositions();

            if (_mode == enMode.Edit)
            {
                LoadEmployeeData();
            }
            else
            {
                _dtpHireDate.Value = DateTime.Today;
                _dtpFireDate.Checked = false;
            }

            _cmbPosition.SelectedIndexChanged += (_, _) => UpdateSaveEnabled();
            _dtpHireDate.ValueChanged += (_, _) => UpdateSaveEnabled();
            _dtpFireDate.ValueChanged += (_, _) => UpdateSaveEnabled();

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme();
            UpdateSaveEnabled();
        }

        private void LoadEmployeeData()
        {
            _lblTitle.Text = "🧑‍💼  Edit Employee";
            _lblMode.Text = "Update person details and employee information.";

            _selectedPersonID = _employee.PersonID;
            if (_employee.Person != null)
            {
                _personCard.LoadPerson(_employee.Person);
            }
            else if (_selectedPersonID.HasValue && _selectedPersonID.Value > 0)
            {
                _personCard.LoadPerson(clsPerson.Find(_selectedPersonID.Value));
            }

            SelectPositionById(_employee.PositionID);
            _dtpHireDate.Value = _employee.HireDate == DateTime.MinValue ? DateTime.Today : _employee.HireDate;

            if (_employee.FireDate.HasValue)
            {
                _dtpFireDate.Checked = true;
                _dtpFireDate.Value = _employee.FireDate.Value;
            }
            else
            {
                _dtpFireDate.Checked = false;
                _dtpFireDate.Value = DateTime.Today;
            }
        }

        private void PopulatePositions()
        {
            DataTable dt = clsPosition.GetAllPosition();

            if (!dt.Columns.Contains("PositionID") || !dt.Columns.Contains("PositionName"))
            {
                _cmbPosition.DataSource = null;
                return;
            }

            DataTable source = dt.Copy();
            DataRow defaultRow = source.NewRow();
            defaultRow["PositionID"] = -1;
            defaultRow["PositionName"] = "-- Select Position --";
            source.Rows.InsertAt(defaultRow, 0);

            _cmbPosition.DisplayMember = "PositionName";
            _cmbPosition.ValueMember = "PositionID";
            _cmbPosition.DataSource = source;
            _cmbPosition.SelectedIndex = 0;
        }

        private void SelectPositionById(int positionId)
        {
            if (_cmbPosition.DataSource == null)
                return;

            _cmbPosition.SelectedValue = positionId;
            if (_cmbPosition.SelectedIndex < 0)
            {
                _cmbPosition.SelectedIndex = 0;
            }
        }

        private int GetSelectedPositionId()
        {
            if (_cmbPosition.SelectedValue == null || _cmbPosition.SelectedValue == DBNull.Value)
                return -1;

            return int.TryParse(_cmbPosition.SelectedValue.ToString(), out int positionId)
                ? positionId
                : -1;
        }

        private void UpdateSaveEnabled()
        {
            _btnSave.Enabled = IsSaveReady();
        }

        private bool IsSaveReady()
        {
            if (!_selectedPersonID.HasValue || _selectedPersonID.Value <= 0)
                return false;

            if (GetSelectedPositionId() <= 0)
                return false;

            if (_dtpFireDate.Checked && _dtpFireDate.Value.Date < _dtpHireDate.Value.Date)
                return false;

            return true;
        }

        private void PersonCard_PersonAdded(object? sender, PersonEventArgs e)
        {
            _selectedPersonID = e.Person?.PersonID;
            UpdateSaveEnabled();
        }

        private void PersonCard_PersonEdited(object? sender, PersonEventArgs e)
        {
            _selectedPersonID = e.Person?.PersonID;
            UpdateSaveEnabled();
        }

        private void PersonCard_PersonCleared(object? sender, EventArgs e)
        {
            _selectedPersonID = null;
            UpdateSaveEnabled();
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            int? currentUserId = clsGlobalUser.CurrentUser?.UserID;
            int positionId = GetSelectedPositionId();
            DateTime hireDate = _dtpHireDate.Value.Date;
            DateTime? fireDate = _dtpFireDate.Checked ? _dtpFireDate.Value.Date : null;

            clsEmployee employeeToSave;
            if (_mode == enMode.AddNew)
            {
                employeeToSave = new clsEmployee
                {
                    PersonID = _selectedPersonID!.Value,
                    PositionID = positionId,
                    HireDate = hireDate,
                    FireDate = fireDate,
                    CreatedByUserID = currentUserId ?? -1
                };
            }
            else
            {
                employeeToSave = new clsEmployee
                {
                    EmployeeID = _employee.EmployeeID,
                    PersonID = _selectedPersonID!.Value,
                    PositionID = positionId,
                    HireDate = hireDate,
                    FireDate = fireDate,
                    CreatedByUserID = _employee.CreatedByUserID,
                    CreatedDate = _employee.CreatedDate,
                    UpdatedByUserID = currentUserId ?? -1,
                    UpdatedDate = DateTime.Now,
                    Mode = clsEmployee.enMode.Update
                };
            }

            if (!employeeToSave.Save())
            {
                _notification.ShowError("Failed to save employee. Please try again.");
                return;
            }

            if (_mode == enMode.AddNew)
            {
                _mode = enMode.Edit;
                _employee = clsEmployee.Find(employeeToSave.EmployeeID) ?? employeeToSave;
                _lblTitle.Text = "🧑‍💼  Edit Employee";
                _lblMode.Text = "Update person details and employee information.";
            }
            else
            {
                _employee = clsEmployee.Find(employeeToSave.EmployeeID) ?? employeeToSave;
            }

            _notification.ShowSuccess("Employee saved successfully.");
            EmployeeSaved?.Invoke(this, _employee);
        }

        private bool ValidateInput()
        {
            _errorProvider.Clear();
            _notification.HideImmediately();

            bool isValid = true;

            if (!_selectedPersonID.HasValue || _selectedPersonID.Value <= 0)
            {
                _notification.ShowWarning("Please select a person from the left section before saving.");
                isValid = false;
            }

            if (GetSelectedPositionId() <= 0)
            {
                _errorProvider.SetError(_cmbPosition, "Please select a position.");
                isValid = false;
            }

            if (_dtpFireDate.Checked && _dtpFireDate.Value.Date < _dtpHireDate.Value.Date)
            {
                _errorProvider.SetError(_dtpFireDate, "Fire date cannot be before hire date.");
                isValid = false;
            }

            if (!isValid && !_notification.Visible)
            {
                _notification.ShowWarning("Please fix validation errors before saving.");
            }

            return isValid;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyTheme));
                return;
            }

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.FormBackground;
            _pnlHeader.BackColor = colors.ContentBackground;
            _splitLayout.BackColor = colors.FormBackground;
            _pnlButtons.BackColor = colors.ContentBackground;
            _pnlEmployeeCard.BackColor = colors.ContentBackground;

            _lblTitle.ForeColor = colors.TitleText;
            _lblMode.ForeColor = colors.SecondaryText;
            _lblEmployeeSection.ForeColor = colors.Primary;
            _lblPosition.ForeColor = colors.SecondaryText;
            _lblHireDate.ForeColor = colors.SecondaryText;
            _lblFireDate.ForeColor = colors.SecondaryText;

            _cmbPosition.BackColor = colors.ContentBackground;
            _cmbPosition.ForeColor = colors.PrimaryText;
            _dtpHireDate.CalendarForeColor = colors.PrimaryText;
            _dtpFireDate.CalendarForeColor = colors.PrimaryText;

            _chkFireDateEnabled.ForeColor = colors.PrimaryText;

            _btnSave.BackColor = colors.Primary;
            _btnSave.ForeColor = Color.White;
            _btnSave.FlatAppearance.BorderColor = colors.Primary;
            _btnSave.FlatAppearance.MouseOverBackColor = colors.PrimaryHover;

            _btnCancel.BackColor = colors.ContentBackground;
            _btnCancel.ForeColor = colors.SecondaryText;
            _btnCancel.FlatAppearance.BorderColor = colors.BorderColor;
            _btnCancel.FlatAppearance.MouseOverBackColor = colors.FormBackground;

            _personCard.BackColor = colors.ContentBackground;
            _personCard.Invalidate();
            _pnlEmployeeCard.Invalidate();
        }

        private void _pnlEmployeeCard_Paint(object sender, PaintEventArgs e)
        {
            var colors = ThemeManager.Colors;
            using var pen = new Pen(colors.BorderColor, 1.5f);
            var rect = new Rectangle(0, 0, _pnlEmployeeCard.Width - 1, _pnlEmployeeCard.Height - 1);
            e.Graphics.DrawRectangle(pen, rect);
        }

        private void _chkFireDateEnabled_CheckedChanged(object? sender, EventArgs e)
        {
            _dtpFireDate.Enabled = _chkFireDateEnabled.Checked;
            _dtpFireDate.Checked = _chkFireDateEnabled.Checked;
            UpdateSaveEnabled();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnHandleDestroyed(e);
        }
    }
}
