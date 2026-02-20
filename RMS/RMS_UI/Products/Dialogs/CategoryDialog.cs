using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    /// <summary>
    /// Dialog for adding or editing a Category.
    /// </summary>
    public partial class CategoryDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private TableLayoutPanel _formLayout = null!;
        private Label _lblCategoryName = null!;
        private TextBox _txtCategoryName = null!;
        private Label _lblDescription = null!;
        private TextBox _txtDescription = null!;
        private Panel _buttonPanel = null!;
        private Button _btnOK = null!;
        private Button _btnCancel = null!;
        // ErrorProvider is defined in the Designer file

        private int _categoryId = -1;
        private clsCategory? _category;
        private bool _isEditMode = false;
        #endregion

        #region Constructors
        public CategoryDialog() : this(-1) { }

        public CategoryDialog(int categoryId)
        {
            _categoryId = categoryId;
            _isEditMode = categoryId > 0;

            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void CategoryDialog_Load(object sender, EventArgs e)
        {
            if (_isEditMode)
                LoadCategory();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = _isEditMode ? "Edit Category" : "Add New Category";
            this.Size = new Size(450, 280);
            this.MinimumSize = new Size(400, 260);
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
                BackColor = Color.FromArgb(34, 197, 94),
                Padding = new Padding(15, 0, 15, 0)
            };

            _titleLabel = new Label
            {
                Text = _isEditMode ? "✏️ Edit Category" : "➕ Add New Category",
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
                RowCount = 3,
                Padding = new Padding(20)
            };
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Category Name
            _lblCategoryName = new Label
            {
                Text = "Category Name: *",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _txtCategoryName = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Width = 250,
                MaxLength = 100
            };
            _txtCategoryName.Validating += TxtCategoryName_Validating;

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblCategoryName, 0, 0);
            _formLayout.Controls.Add(_txtCategoryName, 1, 0);

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
                MaxLength = 500
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblDescription, 0, 1);
            _formLayout.Controls.Add(_txtDescription, 1, 1);

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
        private void LoadCategory()
        {
            _category = clsCategory.Find(_categoryId);
            if (_category != null)
            {
                _txtCategoryName.Text = _category.CategoryName;
                _txtDescription.Text = _category.Description ?? "";
            }
            else
            {
                MessageBox.Show("Category not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (_isEditMode && _category != null)
                {
                    _category.CategoryName = _txtCategoryName.Text.Trim();
                    _category.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                }
                else
                {
                    _category = clsCategory.CreateNew();
                    _category.CategoryName = _txtCategoryName.Text.Trim();
                    _category.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                }

                if (_category.Save())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Validation
        private void TxtCategoryName_Validating(object? sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtCategoryName.Text))
            {
                _errorProvider.SetError(_txtCategoryName, "Category name is required");
            }
            else if (_txtCategoryName.Text.Length > 100)
            {
                _errorProvider.SetError(_txtCategoryName, "Category name must not exceed 100 characters");
            }
            else
            {
                _errorProvider.SetError(_txtCategoryName, "");
            }
        }

        private bool ValidateForm()
        {
            TxtCategoryName_Validating(null, new CancelEventArgs());

            return string.IsNullOrEmpty(_errorProvider.GetError(_txtCategoryName));
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;

            if (_headerPanel != null)
                _headerPanel.BackColor = Color.FromArgb(34, 197, 94); // Keep green for categories

            if (_formLayout != null)
                _formLayout.BackColor = colors.ContentBackground;

            if (_lblCategoryName != null)
                _lblCategoryName.ForeColor = colors.PrimaryText;

            if (_lblDescription != null)
                _lblDescription.ForeColor = colors.PrimaryText;

            if (_txtCategoryName != null)
            {
                _txtCategoryName.BackColor = colors.ContentBackground;
                _txtCategoryName.ForeColor = colors.PrimaryText;
            }

            if (_txtDescription != null)
            {
                _txtDescription.BackColor = colors.ContentBackground;
                _txtDescription.ForeColor = colors.PrimaryText;
            }

            if (_buttonPanel != null)
                _buttonPanel.BackColor = colors.FormBackground;

            Invalidate(true);
        }
        #endregion
    }
}
