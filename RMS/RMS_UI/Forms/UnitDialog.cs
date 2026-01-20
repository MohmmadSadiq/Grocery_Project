using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    /// <summary>
    /// Dialog for adding or editing a Unit.
    /// </summary>
    public partial class UnitDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private TableLayoutPanel _formLayout = null!;
        private Label _lblUnitName = null!;
        private TextBox _txtUnitName = null!;
        private Label _lblDescription = null!;
        private TextBox _txtDescription = null!;
        private CheckBox _chkIsActive = null!;
        private Panel _buttonPanel = null!;
        private Button _btnOK = null!;
        private Button _btnCancel = null!;
        // ErrorProvider is defined in the Designer file

        private int _unitId = -1;
        private clsUnit? _unit;
        private bool _isEditMode = false;
        #endregion

        #region Constructors
        public UnitDialog() : this(-1) { }

        public UnitDialog(int unitId)
        {
            _unitId = unitId;
            _isEditMode = unitId > 0;

            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void UnitDialog_Load(object sender, EventArgs e)
        {
            if (_isEditMode)
                LoadUnit();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = _isEditMode ? "Edit Unit" : "Add New Unit";
            this.Size = new Size(450, 300);
            this.MinimumSize = new Size(400, 280);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Font = new Font("Segoe UI", 10F);

            // Error Provider is created in Designer

            // Header Panel
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(59, 130, 246),
                Padding = new Padding(15, 0, 15, 0)
            };

            _titleLabel = new Label
            {
                Text = _isEditMode ? "✏️ Edit Unit" : "➕ Add New Unit",
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = Color.White,
                AutoSize = true
            };
            _headerPanel.Controls.Add(_titleLabel);
            _headerPanel.Resize += (s, e) =>
            {
                _titleLabel.Location = new Point(15, (_headerPanel.Height - _titleLabel.Height) / 2);
            };

            // Form Layout
            _formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(20)
            };
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Unit Name
            _lblUnitName = new Label
            {
                Text = "Unit Name: *",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _txtUnitName = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Width = 250,
                MaxLength = 50
            };
            _txtUnitName.Validating += TxtUnitName_Validating;

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblUnitName, 0, 0);
            _formLayout.Controls.Add(_txtUnitName, 1, 0);

            // Description
            _lblDescription = new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Width = 250,
                Height = 60,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 200
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblDescription, 0, 1);
            _formLayout.Controls.Add(_txtDescription, 1, 1);

            // Is Active
            _chkIsActive = new CheckBox
            {
                Text = "Active",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Checked = true
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(new Label(), 0, 2);
            _formLayout.Controls.Add(_chkIsActive, 1, 2);

            // Button Panel
            _buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(15)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            _btnCancel = new Button
            {
                Text = "Cancel",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(90, 35),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                DialogResult = DialogResult.Cancel,
                Margin = new Padding(0, 0, 10, 0),
                CausesValidation = false
            };
            _btnCancel.FlatAppearance.BorderSize = 0;

            _btnOK = new Button
            {
                Text = "Save",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(90, 35),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 10, 0)
            };
            _btnOK.FlatAppearance.BorderSize = 0;
            _btnOK.Click += BtnOK_Click;

            flowPanel.Controls.Add(_btnCancel);
            flowPanel.Controls.Add(_btnOK);
            _buttonPanel.Controls.Add(flowPanel);

            // Add controls
            this.Controls.Add(_formLayout);
            this.Controls.Add(_buttonPanel);
            this.Controls.Add(_headerPanel);

            this.AcceptButton = _btnOK;
            this.CancelButton = _btnCancel;

            this.ResumeLayout(false);
        }
        #endregion

        #region Data Loading
        private void LoadUnit()
        {
            _unit = clsUnit.Find(_unitId);
            if (_unit != null)
            {
                _txtUnitName.Text = _unit.UnitName;
                _txtDescription.Text = _unit.Description ?? "";
                _chkIsActive.Checked = _unit.IsActive;
            }
            else
            {
                MessageBox.Show("Unit not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        #endregion

        #region Save
        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                if (_isEditMode && _unit != null)
                {
                    _unit.UnitName = _txtUnitName.Text.Trim();
                    _unit.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                    _unit.IsActive = _chkIsActive.Checked;
                }
                else
                {
                    _unit = clsUnit.CreateNew();
                    _unit.UnitName = _txtUnitName.Text.Trim();
                    _unit.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                    _unit.IsActive = _chkIsActive.Checked;
                }

                if (_unit.Save())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save unit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Validation
        private void TxtUnitName_Validating(object? sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtUnitName.Text))
            {
                _errorProvider.SetError(_txtUnitName, "Unit name is required");
            }
            else if (_txtUnitName.Text.Length > 50)
            {
                _errorProvider.SetError(_txtUnitName, "Unit name must not exceed 50 characters");
            }
            else
            {
                _errorProvider.SetError(_txtUnitName, "");
            }
        }

        private bool ValidateForm()
        {
            TxtUnitName_Validating(null, new CancelEventArgs());

            return string.IsNullOrEmpty(_errorProvider.GetError(_txtUnitName));
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;

            if (_headerPanel != null)
                _headerPanel.BackColor = colors.Primary;

            if (_formLayout != null)
                _formLayout.BackColor = colors.ContentBackground;

            if (_lblUnitName != null)
                _lblUnitName.ForeColor = colors.PrimaryText;

            if (_lblDescription != null)
                _lblDescription.ForeColor = colors.PrimaryText;

            if (_txtUnitName != null)
            {
                _txtUnitName.BackColor = colors.ContentBackground;
                _txtUnitName.ForeColor = colors.PrimaryText;
            }

            if (_txtDescription != null)
            {
                _txtDescription.BackColor = colors.ContentBackground;
                _txtDescription.ForeColor = colors.PrimaryText;
            }

            if (_chkIsActive != null)
                _chkIsActive.ForeColor = colors.PrimaryText;

            if (_buttonPanel != null)
                _buttonPanel.BackColor = colors.FormBackground;

            Invalidate(true);
        }
        #endregion
    }
}
