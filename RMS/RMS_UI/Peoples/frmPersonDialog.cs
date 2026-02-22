using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Peoples
{
    /// <summary>
    /// Dialog for adding or editing a person.
    /// Pass personId = -1 to add a new person.
    /// </summary>
    public partial class frmPersonDialog : Form
    {
        #region Events

        public event EventHandler<clsPerson> PersonSaved;

        #endregion

        #region Fields
        private int _personId;
        private clsPerson? _person;
        private bool _isEditMode;
        private string? _selectedImagePath;
        private bool _imageRemoved;
        private int _currentUserID = clsGlobalUser.CurrentUser?.UserID ?? 1;

        // Header
        private Panel _headerPanel = null!;
        private Label _lblTitle = null!;
        private Button _btnClose = null!;

        // Content
        private Panel _contentPanel = null!;
        private Panel _scrollPanel = null!;

        // Basic Info
        private TextBox _txtFirstName = null!;
        private TextBox _txtSecondName = null!;
        private TextBox _txtThirdName = null!;
        private TextBox _txtLastName = null!;
        private TextBox _txtNationalNo = null!;
        private Label _lblFullNameValue = null!;

        // Demographics
        private DateTimePicker _dtpDateOfBirth = null!;
        private CheckBox _chkHasDOB = null!;
        private ComboBox _cmbGender = null!;
        private PictureBox _picPersonImage = null!;
        private Button _btnUploadImage = null!;
        private Button _btnRemoveImage = null!;

        // Contact
        private TextBox _txtPhone = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtAddress = null!;

        // Location
        private ComboBox _cmbCountry = null!;

        // Audit (Edit mode only)
        private Label _lblCreatedDate = null!;
        private Label _lblCreatedBy = null!;
        private Label _lblUpdatedDate = null!;
        private Label _lblUpdatedBy = null!;
        private Panel _auditPanel = null!;

        // Buttons
        private Panel _buttonsPanel = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Button _btnDelete = null!;

        // Validation
        private ErrorProvider _errorProvider = null!;

        // Notification & Person ID
        private NotificationControl _notification = null!;
        private Label _lblPersonID = null!;
        #endregion

        #region Constructor
        public frmPersonDialog(int personId = -1)
        {
            _personId = personId;
            _isEditMode = personId > 0;

            InitializeComponent();
            CreateUI();
            LoadComboBoxData();
            ApplyTheme();

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            FormClosing += PersonDialog_FormClosing;

            if (_isEditMode)
            {
                LoadPersonData();
            }
            else
            {
                _person = new clsPerson();
            }
        }

        private void PersonDialog_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_picPersonImage?.Image != null)
            {
                var img = _picPersonImage.Image;
                _picPersonImage.Image = null;
                img.Dispose();
            }
        }
        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }
        #endregion

        #region CreateUI
        private void CreateUI()
        {
            SuspendLayout();

            Text = _isEditMode ? "Edit Person" : "Add New Person";
            Size = new Size(570, 700);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            DoubleBuffered = true;

            CreateHeader();
            CreateContent();
            CreateButtonsPanel();

            // Add controls: order matters for Dock
            Controls.Add(_contentPanel);
            Controls.Add(_notification);   // sits between content and buttons
            Controls.Add(_buttonsPanel);
            Controls.Add(_headerPanel);

            ResumeLayout(false);

            // Drag support
            _headerPanel.MouseDown += Header_MouseDown;
            _lblTitle.MouseDown += Header_MouseDown;
        }
        #endregion

        #region Header
        private void CreateHeader()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
                BackColor = Color.FromArgb(59, 130, 246),
                Padding = new Padding(20, 0, 10, 0)
            };

            _lblTitle = new Label
            {
                Text = _isEditMode ? "✏️  Edit Person" : "➕  Add New Person",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 10),
                Cursor = Cursors.SizeAll
            };

            _lblPersonID = new Label
            {
                Text = _isEditMode ? $"ID: {_personId}" : "ID: ???",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(186, 212, 255),
                AutoSize = true,
                Location = new Point(22, 40),
                Cursor = Cursors.SizeAll
            };

            _btnClose = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F),
                Size = new Size(40, 36),
                Cursor = Cursors.Hand,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                CausesValidation = false
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 68, 68);
            _btnClose.Location = new Point(_headerPanel.Width - 50, 14);
            _btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _headerPanel.Resize += (s, e) =>
            {
                _btnClose.Location = new Point(_headerPanel.Width - 50, 14);
            };

            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Controls.Add(_lblPersonID);
            _headerPanel.Controls.Add(_btnClose);
        }
        #endregion

        #region Content
        private void CreateContent()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                AutoScroll = true
            };

            // Notification initialized here but added to form in CreateUI
            _notification = new NotificationControl
            {
                Dock = DockStyle.Bottom,
                Height = 0,
                Visible = false
            };

            _scrollPanel = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(0)
            };

            const int leftX = 20;
            const int colW = 242;   // each column width
            const int gap = 16;
            const int rightX = leftX + colW + gap;  // 278
            const int fullW = colW * 2 + gap;       // 500

            int y = 12;

            // ── Left column (Personal Info) ──
            var leftPanel = new Panel
            {
                Location = new Point(leftX, y),
                Width = colW + 15,
                AutoSize = true
            };

            // ── Right column (Contact Info) ──
            var rightPanel = new Panel
            {
                Location = new Point(rightX, y),
                Width = colW + 15,
                AutoSize = true
            };

            // ══ LEFT COLUMN ══
            int ly = 0;
            ly = AddColSectionHeader(leftPanel, "Personal Information", ly, colW);

            ly = AddColFormRow(leftPanel, "First Name *", out _txtFirstName, ly, colW);
            _txtFirstName.Validating += (s, e) => ValidateRequired(_txtFirstName, "First name is required");
            _txtFirstName.TextChanged += (s, e) => UpdateFullName();

            ly = AddColFormRow(leftPanel, "Second Name", out _txtSecondName, ly, colW);
            _txtSecondName.TextChanged += (s, e) => UpdateFullName();

            ly = AddColFormRow(leftPanel, "Third Name", out _txtThirdName, ly, colW);
            _txtThirdName.TextChanged += (s, e) => UpdateFullName();

            ly = AddColFormRow(leftPanel, "Last Name *", out _txtLastName, ly, colW);
            _txtLastName.Validating += (s, e) => ValidateRequired(_txtLastName, "Last name is required");
            _txtLastName.TextChanged += (s, e) => UpdateFullName();

            ly = AddColFormRow(leftPanel, "National No", out _txtNationalNo, ly, colW);

            // Full Name (read-only italic)
            _lblFullNameValue = new Label
            {
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(2, ly),
                Size = new Size(colW - 4, 18),
                Text = ""
            };
            leftPanel.Controls.Add(_lblFullNameValue);
            ly += 22;

            // Demographics section
            ly += 4;
            ly = AddColSectionHeader(leftPanel, "Demographics", ly, colW);

            // Gender
            leftPanel.Controls.Add(CreateColLabel("Gender", 0, ly));
            _cmbGender = new ComboBox
            {
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(88, ly - 2),
                Size = new Size(colW - 92, 26)
            };
            leftPanel.Controls.Add(_cmbGender);
            ly += 30;

            // Date of Birth
            leftPanel.Controls.Add(CreateColLabel("Birth Date", 0, ly));
            _chkHasDOB = new CheckBox
            {
                Location = new Point(88, ly),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                Text = "",
                Checked = false
            };
            _dtpDateOfBirth = new DateTimePicker
            {
                Font = new Font("Segoe UI", 9F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(108, ly - 2),
                Size = new Size(colW - 112, 26),
                Enabled = false,
                Value = DateTime.Now.AddYears(-18)
            };
            _chkHasDOB.CheckedChanged += (s, e) => _dtpDateOfBirth.Enabled = _chkHasDOB.Checked;
            leftPanel.Controls.Add(_chkHasDOB);
            leftPanel.Controls.Add(_dtpDateOfBirth);
            ly += 30;

            // Photo
            leftPanel.Controls.Add(CreateColLabel("Photo", 0, ly));
            _picPersonImage = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(88, ly),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            leftPanel.Controls.Add(_picPersonImage);

            _btnUploadImage = new Button
            {
                Text = "📁 Upload",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F),
                Size = new Size(colW - 172, 28),
                Location = new Point(172, ly + 6),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White
            };
            _btnUploadImage.FlatAppearance.BorderSize = 0;
            _btnUploadImage.Click += BtnUploadImage_Click;
            leftPanel.Controls.Add(_btnUploadImage);

            _btnRemoveImage = new Button
            {
                Text = "🗑️ Remove",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F),
                Size = new Size(colW - 172, 28),
                Location = new Point(172, ly + 44),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Enabled = false
            };
            _btnRemoveImage.FlatAppearance.BorderSize = 0;
            _btnRemoveImage.Click += BtnRemoveImage_Click;
            leftPanel.Controls.Add(_btnRemoveImage);
            ly += 90;

            leftPanel.Height = ly + 4;
            _scrollPanel.Controls.Add(leftPanel);

            // ══ RIGHT COLUMN ══
            int ry = 0;
            ry = AddColSectionHeader(rightPanel, "Contact Information", ry, colW);

            ry = AddColFormRow(rightPanel, "Phone", out _txtPhone, ry, colW);
            _txtPhone.Validating += (s, e) => ValidatePhoneField();

            ry = AddColFormRow(rightPanel, "Email", out _txtEmail, ry, colW);
            _txtEmail.Validating += (s, e) => ValidateEmailField();

            // Address multiline
            rightPanel.Controls.Add(CreateColLabel("Address", 0, ry));
            _txtAddress = new TextBox
            {
                Font = new Font("Segoe UI", 9F),
                Location = new Point(88, ry),
                Size = new Size(colW - 92, 54),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            rightPanel.Controls.Add(_txtAddress);
            ry += 60;

            // Location section
            ry += 4;
            ry = AddColSectionHeader(rightPanel, "Location", ry, colW);

            rightPanel.Controls.Add(CreateColLabel("Country", 0, ry));
            _cmbCountry = new ComboBox
            {
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(88, ry - 2),
                Size = new Size(colW - 92, 26)
            };
            rightPanel.Controls.Add(_cmbCountry);
            ry += 30;

            rightPanel.Height = ry + 4;
            _scrollPanel.Controls.Add(rightPanel);

            // ── Bottom: Audit section (always created; visible only in edit mode or after save) ──
            int bottomY = y + Math.Max(leftPanel.Height, rightPanel.Height) + 10;
            bottomY = AddFullWidthSectionHeader(_scrollPanel, "Audit Information", leftX, bottomY, fullW);

            _auditPanel = new Panel
            {
                Location = new Point(leftX, bottomY),
                Size = new Size(fullW, 52),
                BackColor = Color.Transparent,
                Visible = _isEditMode
            };

            _lblCreatedDate = CreateAuditLabel("Created: —", 0, 0);
            _lblCreatedBy = CreateAuditLabel("", 180, 0);
            _lblUpdatedDate = CreateAuditLabel("Updated: —", 0, 24);
            _lblUpdatedBy = CreateAuditLabel("", 180, 24);

            _auditPanel.Controls.AddRange(new Control[] { _lblCreatedDate, _lblCreatedBy, _lblUpdatedDate, _lblUpdatedBy });
            _scrollPanel.Controls.Add(_auditPanel);
            bottomY += 56;

            _scrollPanel.Height = bottomY + 10;
            _contentPanel.Controls.Add(_scrollPanel);
        }
        #endregion

        #region Buttons Panel
        private void CreateButtonsPanel()
        {
            _buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                Padding = new Padding(20, 12, 20, 12)
            };

            _btnSave = new Button
            {
                Text = "💾  Save",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(100, 38),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Location = new Point(20, 12)
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(90, 38),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                CausesValidation = false,
                Location = new Point(128, 12)
            };
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnDelete = new Button
            {
                Text = "🗑️  Delete",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(100, 38),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Visible = _isEditMode,
                Location = new Point(226, 12)
            };
            _btnDelete.FlatAppearance.BorderSize = 0;
            _btnDelete.Click += BtnDelete_Click;

            _buttonsPanel.Controls.AddRange(new Control[] { _btnSave, _btnCancel, _btnDelete });
        }
        #endregion

        #region UI Helper Methods
        /// <summary>Adds a bold section header + separator line inside a column panel. Returns new y.</summary>
        private int AddColSectionHeader(Panel panel, string text, int y, int width)
        {
            panel.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 9.5F),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(0, y),
                AutoSize = true
            });
            panel.Controls.Add(new Panel
            {
                BackColor = Color.FromArgb(226, 232, 240),
                Location = new Point(0, y + 20),
                Size = new Size(width, 1)
            });
            return y + 28;
        }

        /// <summary>Adds a full-width section header spanning both columns. Returns new y.</summary>
        private int AddFullWidthSectionHeader(Panel panel, string text, int x, int y, int width)
        {
            panel.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 9.5F),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(x, y),
                AutoSize = true
            });
            panel.Controls.Add(new Panel
            {
                BackColor = Color.FromArgb(226, 232, 240),
                Location = new Point(x, y + 20),
                Size = new Size(width, 1)
            });
            return y + 28;
        }

        /// <summary>Adds label + TextBox row inside a column panel. Returns new y.</summary>
        private int AddColFormRow(Panel panel, string labelText, out TextBox textBox, int y, int colWidth)
        {
            panel.Controls.Add(CreateColLabel(labelText, 0, y + 2));
            textBox = new TextBox
            {
                Font = new Font("Segoe UI", 9F),
                Location = new Point(88, y),
                Size = new Size(colWidth - 92, 24)
            };
            panel.Controls.Add(textBox);
            return y + 30;
        }

        private Label CreateColLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private Label CreateAuditLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(x, y),
                AutoSize = true
            };
        }
        #endregion

        #region Data Loading
        private void LoadComboBoxData()
        {
            // Gender
            _cmbGender.Items.Clear();
            _cmbGender.Items.Add(new ComboBoxItem(-1, "-- Not Specified --"));
            _cmbGender.Items.Add(new ComboBoxItem(0, "Male"));
            _cmbGender.Items.Add(new ComboBoxItem(1, "Female"));
            _cmbGender.SelectedIndex = 0;

            // Countries
            _cmbCountry.Items.Clear();
            _cmbCountry.Items.Add(new ComboBoxItem(-1, "-- Select Country --"));
            try
            {
                DataTable countries = clsCountry.GetAllCountries();
                if (countries != null)
                {
                    foreach (DataRow row in countries.Rows)
                    {
                        int id = Convert.ToInt32(row["CountryID"]);
                        string name = row["CountryName"]?.ToString() ?? "";
                        _cmbCountry.Items.Add(new ComboBoxItem(id, name));
                    }
                }
            }
            catch { }
            _cmbCountry.SelectedIndex = 0;
        }

        private void LoadPersonData()
        {
            _person = clsPerson.Find(_personId);
            if (_person == null)
            {
                _notification.ShowError("Person not found!");
                DialogResult = DialogResult.Cancel;
                return;
            }

            // Update Person ID label
            _lblPersonID.Text = $"ID: {_person.PersonID}";

            _txtFirstName.Text = _person.FirstName ?? "";
            _txtSecondName.Text = _person.SecondName ?? "";
            _txtThirdName.Text = _person.ThirdName ?? "";
            _txtLastName.Text = _person.LastName ?? "";
            _txtNationalNo.Text = _person.NationalNo ?? "";
            UpdateFullName();

            // Gender
            if (_person.Gender.HasValue)
                SelectComboBoxByValue(_cmbGender, _person.Gender.Value);

            // Date of Birth
            if (_person.DateOfBirth.HasValue)
            {
                _chkHasDOB.Checked = true;
                _dtpDateOfBirth.Value = _person.DateOfBirth.Value;
            }

            // Image
            if (!string.IsNullOrEmpty(_person.ImagePath))
            {
                try
                {
                    _picPersonImage.Image = Image.FromFile(_person.ImagePath);
                    _selectedImagePath = _person.ImagePath;
                    _btnRemoveImage.Enabled = true;
                }
                catch { }
            }

            // Contact
            _txtPhone.Text = _person.Phone ?? "";
            _txtEmail.Text = _person.Email ?? "";
            _txtAddress.Text = _person.Address ?? "";

            // Country
            if (_person.NationalityCountryID.HasValue)
                SelectComboBoxByValue(_cmbCountry, _person.NationalityCountryID.Value);

            // Audit
            if (_lblCreatedDate != null)
            {
                _lblCreatedDate.Text = $"Created: {_person.CreatedDate?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";
                _lblCreatedBy.Text = $"By: {_person.CreatedByUser?.UserName ?? "N/A"}";
                _lblUpdatedDate.Text = $"Updated: {_person.UpdatedDate?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";
                _lblUpdatedBy.Text = $"By: {_person.UpdatedByUser?.UserName ?? "N/A"}";
            }
        }

        private void SelectComboBoxByValue(ComboBox cmb, int value)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i] is ComboBoxItem item && item.Value == value)
                {
                    cmb.SelectedIndex = i;
                    return;
                }
            }
        }
        #endregion

        #region Image Handlers
        private void BtnUploadImage_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select Person Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _picPersonImage.Image?.Dispose();
                    _picPersonImage.Image = Image.FromFile(dlg.FileName);
                    _selectedImagePath = dlg.FileName;
                    _imageRemoved = false;
                    _btnRemoveImage.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemoveImage_Click(object? sender, EventArgs e)
        {
            _picPersonImage.Image?.Dispose();
            _picPersonImage.Image = null;
            _selectedImagePath = null;
            _imageRemoved = true;
            _btnRemoveImage.Enabled = false;
        }
        #endregion

        #region Validation
        private void ValidateRequired(TextBox txt, string message)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
                _errorProvider.SetError(txt, message);
            else
                _errorProvider.SetError(txt, "");
        }

        private void ValidateEmailField()
        {
            if (string.IsNullOrWhiteSpace(_txtEmail.Text))
            {
                _errorProvider.SetError(_txtEmail, "");
                return;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(_txtEmail.Text.Trim());
                _errorProvider.SetError(_txtEmail, addr.Address == _txtEmail.Text.Trim() ? "" : "Invalid email format");
            }
            catch
            {
                _errorProvider.SetError(_txtEmail, "Invalid email format");
            }
        }

        private void ValidatePhoneField()
        {
            if (string.IsNullOrWhiteSpace(_txtPhone.Text))
            {
                _errorProvider.SetError(_txtPhone, "");
                return;
            }
            bool ok = _txtPhone.Text.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ');
            _errorProvider.SetError(_txtPhone, ok ? "" : "Phone: digits, +, - only");
        }

        private bool ValidateForm()
        {
            bool valid = true;
            _errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(_txtFirstName.Text))
            {
                _errorProvider.SetError(_txtFirstName, "First name is required");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(_txtLastName.Text))
            {
                _errorProvider.SetError(_txtLastName, "Last name is required");
                valid = false;
            }

            if (!string.IsNullOrWhiteSpace(_txtEmail.Text))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(_txtEmail.Text);
                    if (addr.Address != _txtEmail.Text)
                        throw new Exception();
                }
                catch
                {
                    _errorProvider.SetError(_txtEmail, "Invalid email format");
                    valid = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(_txtPhone.Text))
            {
                if (!_txtPhone.Text.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' '))
                {
                    _errorProvider.SetError(_txtPhone, "Phone: digits, +, - only");
                    valid = false;
                }
            }

            if (_chkHasDOB.Checked && _dtpDateOfBirth.Value > DateTime.Now)
            {
                _errorProvider.SetError(_dtpDateOfBirth, "Date of birth cannot be in the future");
                valid = false;
            }

            return valid;
        }
        #endregion

        #region Save / Delete
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                _notification.ShowWarning("Please correct the highlighted errors before saving.");
                return;
            }

            try
            {
                if (_person == null)
                    _person = new clsPerson();

                _person.FirstName = _txtFirstName.Text.Trim();
                _person.SecondName = NullIfEmpty(_txtSecondName.Text);
                _person.ThirdName = NullIfEmpty(_txtThirdName.Text);
                _person.LastName = _txtLastName.Text.Trim();
                _person.NationalNo = NullIfEmpty(_txtNationalNo.Text);

                // Gender
                if (_cmbGender.SelectedItem is ComboBoxItem genderItem && genderItem.Value >= 0)
                    _person.Gender = (byte)genderItem.Value;
                else
                    _person.Gender = null;

                // Date of Birth
                _person.DateOfBirth = _chkHasDOB.Checked ? _dtpDateOfBirth.Value : null;

                // Contact
                _person.Phone = NullIfEmpty(_txtPhone.Text);
                _person.Email = NullIfEmpty(_txtEmail.Text);
                _person.Address = NullIfEmpty(_txtAddress.Text);

                // Country
                if (_cmbCountry.SelectedItem is ComboBoxItem countryItem && countryItem.Value > 0)
                    _person.NationalityCountryID = countryItem.Value;
                else
                    _person.NationalityCountryID = null;

                // Image
                if (_imageRemoved)
                    _person.ImagePath = null;
                else if (!string.IsNullOrEmpty(_selectedImagePath) && _selectedImagePath != _person.ImagePath)
                    _person.ImagePath = _selectedImagePath;

                // Audit
                bool wasAddMode = !_isEditMode;
                if (wasAddMode)
                {
                    _person.CreatedDate = DateTime.Now;
                    _person.CreatedByUserID = _currentUserID;
                }
                else
                {
                    _person.UpdatedDate = DateTime.Now;
                    _person.UpdatedByUserID = _currentUserID;
                }

                if (_person.Save())
                {
                    if (wasAddMode)
                    {
                        // Switch to Edit mode — don't close the form
                        _isEditMode = true;
                        _lblTitle.Text = "✏️  Edit Person";
                        _lblPersonID.Text = $"ID: {_person.PersonID}";
                        _btnDelete.Visible = true;

                        // Populate and show audit section
                        _lblCreatedDate.Text = $"Created: {_person.CreatedDate?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";
                        _lblCreatedBy.Text = $"By: {clsGlobalUser.CurrentUser?.UserName ?? "N/A"}";
                        _lblUpdatedDate.Text = "Updated: —";
                        _lblUpdatedBy.Text = "";
                        _auditPanel.Visible = true;

                        _notification.ShowSuccess($"Person saved successfully! ID: {_person.PersonID}");
                        DialogResult = DialogResult.None;
                    }
                    else
                    {
                        // Update audit labels then close
                        _lblUpdatedDate.Text = $"Updated: {_person.UpdatedDate?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";
                        _lblUpdatedBy.Text = $"By: {clsGlobalUser.CurrentUser?.UserName ?? "N/A"}";

                        _notification.ShowSuccess("Changes saved successfully!");
                        // Short delay so notification is visible, then close
                        var t = new System.Windows.Forms.Timer { Interval = 1200 };
                        t.Tick += (_, __) => { t.Stop(); t.Dispose(); DialogResult = DialogResult.OK; };
                        t.Start();
                    }
                    PersonSaved.Invoke(this, _person);
                }
                else
                {
                    _notification.ShowError("Failed to save person. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _notification.ShowError($"Error saving person: {ex.Message}");
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!_isEditMode || _person == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{_person.FullName}'?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (clsPerson.DeletePerson(_person.PersonID, _currentUserID))
                    {
                        ResetToAddMode();
                    }
                    else
                    {
                        _notification.ShowError("Failed to delete person. It may be referenced by other records.");
                    }
                }
                catch (Exception ex)
                {
                    _notification.ShowError($"Error deleting person: {ex.Message}");
                }
            }
        }
        #endregion

        #region Helpers
        private void UpdateFullName()
        {
            string full = string.Join(" ",
                new[] { _txtFirstName.Text, _txtSecondName.Text, _txtThirdName.Text, _txtLastName.Text }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            _lblFullNameValue.Text = string.IsNullOrWhiteSpace(full) ? "" : full;
        }

        private static string? NullIfEmpty(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        /// <summary>Resets the form to Add New Person mode after successful deletion.</summary>
        private void ResetToAddMode()
        {
            _isEditMode = false;
            _personId = -1;
            _person = new clsPerson();

            // Header
            _lblTitle.Text = "➕  Add New Person";
            _lblPersonID.Text = "ID: ???";

            // Hide delete button and audit section
            _btnDelete.Visible = false;
            _auditPanel.Visible = false;

            // Clear all text boxes
            _txtFirstName.Text = "";
            _txtSecondName.Text = "";
            _txtThirdName.Text = "";
            _txtLastName.Text = "";
            _txtNationalNo.Text = "";
            _txtPhone.Text = "";
            _txtEmail.Text = "";
            _txtAddress.Text = "";

            // Reset combo boxes
            _cmbGender.SelectedIndex = 0;
            _cmbCountry.SelectedIndex = 0;

            // Reset date of birth
            _chkHasDOB.Checked = false;
            _dtpDateOfBirth.Value = DateTime.Now.AddYears(-18);
            _dtpDateOfBirth.Enabled = false;

            // Clear image
            _picPersonImage.Image?.Dispose();
            _picPersonImage.Image = null;
            _selectedImagePath = null;
            _imageRemoved = false;
            _btnRemoveImage.Enabled = false;

            // Clear full name
            _lblFullNameValue.Text = "";

            // Clear error provider
            _errorProvider.Clear();

            // Reset audit labels
            _lblCreatedDate.Text = "Created: —";
            _lblCreatedBy.Text = "";
            _lblUpdatedDate.Text = "Updated: —";
            _lblUpdatedBy.Text = "";

            _notification.ShowSuccess("Person deleted successfully. Ready to add a new one!");
        }
        #endregion

        #region Drag Support
        private void Header_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                const int WM_NCLBUTTONDOWN = 0xA1;
                const int HT_CAPTION = 0x2;
                Capture = false;
                var msg = Message.Create(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, nint.Zero);
                WndProc(ref msg);
            }
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var c = ThemeManager.Colors;

            // Form & panels
            BackColor = c.ContentBackground;
            _contentPanel.BackColor = c.ContentBackground;
            _scrollPanel.BackColor = c.ContentBackground;
            _buttonsPanel.BackColor = c.FormBackground;

            // Header stays brand blue
            _headerPanel.BackColor = c.Primary;
            _lblPersonID.ForeColor = Color.FromArgb(186, 212, 255);

            // All TextBoxes
            foreach (var txt in new[] { _txtFirstName, _txtSecondName, _txtThirdName, _txtLastName, _txtNationalNo, _txtPhone, _txtEmail, _txtAddress })
            {
                if (txt != null)
                {
                    txt.BackColor = c.ContentBackground;
                    txt.ForeColor = c.PrimaryText;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }

            // ComboBoxes
            foreach (var cmb in new[] { _cmbGender, _cmbCountry })
            {
                if (cmb != null)
                {
                    cmb.BackColor = c.ContentBackground;
                    cmb.ForeColor = c.PrimaryText;
                }
            }

            // DateTimePicker
            if (_dtpDateOfBirth != null)
                _dtpDateOfBirth.CalendarForeColor = c.PrimaryText;

            // PictureBox
            if (_picPersonImage != null)
                _picPersonImage.BackColor = c.FormBackground;

            // Full name label
            if (_lblFullNameValue != null)
                _lblFullNameValue.ForeColor = c.SecondaryText;

            // Section headers and separator lines in scrollPanel (top-level only)
            foreach (Control ctrl in _scrollPanel.Controls)
            {
                if (ctrl is Label lbl)
                {
                    if (lbl.Font.Name.Contains("Semibold", StringComparison.OrdinalIgnoreCase))
                        lbl.ForeColor = c.Primary;
                    else
                        lbl.ForeColor = c.SecondaryText;
                }
                if (ctrl is Panel p && p.Height == 1)
                    p.BackColor = c.BorderColor;
            }

            // Column panels — recurse into them for labels and separators
            ApplyThemeToColumnPanel(_scrollPanel, c);

            // Audit labels
            if (_auditPanel != null)
            {
                foreach (Control ctrl in _auditPanel.Controls)
                {
                    if (ctrl is Label lbl)
                        lbl.ForeColor = c.SecondaryText;
                }
                _auditPanel.BackColor = Color.Transparent;
            }

            Invalidate(true);
        }

        private void ApplyThemeToColumnPanel(Panel parent, ColorPalette c)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Panel col && col != _auditPanel && col.Height != 1)
                {
                    col.BackColor = c.ContentBackground;
                    foreach (Control child in col.Controls)
                    {
                        if (child is Label lbl && lbl != _lblFullNameValue)
                        {
                            if (lbl.Font.Name.Contains("Semibold", StringComparison.OrdinalIgnoreCase))
                                lbl.ForeColor = c.Primary;
                            else
                                lbl.ForeColor = c.SecondaryText;
                        }
                        if (child is Panel sep && sep.Height == 1)
                            sep.BackColor = c.BorderColor;
                        if (child is TextBox txt)
                        {
                            txt.BackColor = c.ContentBackground;
                            txt.ForeColor = c.PrimaryText;
                        }
                        if (child is ComboBox cmb)
                        {
                            cmb.BackColor = c.ContentBackground;
                            cmb.ForeColor = c.PrimaryText;
                        }
                    }
                }
            }
        }
        #endregion

        #region ComboBoxItem
        private class ComboBoxItem
        {
            public int Value { get; }
            public string Name { get; }
            public ComboBoxItem(int value, string name) { Value = value; Name = name; }
            public override string ToString() => Name;
        }
        #endregion
    }
}
