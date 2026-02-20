using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Forms
{
    /// <summary>
    /// Dialog for adding or editing a product.
    /// Pass productId = -1 to add a new product.
    /// </summary>
    public partial class ProductDialog : Form
    {
        #region Fields
        private readonly int _productId;
        private clsProduct? _product;
        private bool _isEditMode;
        private string? _selectedImagePath; // Path of new image selected by user
        private bool _imageRemoved; // Flag to track if user removed the image

        // Header
        private Panel _headerPanel = null!;
        private Label _lblTitle = null!;
        private Button _btnClose = null!;

        // Form Controls
        private Panel _contentPanel = null!;
        private TableLayoutPanel _formLayout = null!;

        private Label _lblProductID = null!;
        private TextBox _txtProductID = null!;

        private Label _lblProductName = null!;
        private TextBox _txtProductName = null!;

        private Label _lblCategory = null!;
        private ComboBox _cmbCategory = null!;

        private Label _lblBrand = null!;
        private ComboBox _cmbBrand = null!;

        private Label _lblReorderLevel = null!;
        private NumericUpDown _numReorderLevel = null!;

        private Label _lblDescription = null!;
        private TextBox _txtDescription = null!;

        private Label _lblIsActive = null!;
        private CheckBox _chkIsActive = null!;

        // Image Controls
        private Label _lblImage = null!;
        private Panel _imagePanel = null!;
        private PictureBox _pictureBox = null!;
        private Button _btnUploadImage = null!;
        private Button _btnRemoveImage = null!;

        // Buttons Panel
        private Panel _buttonsPanel = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Button _btnDelete = null!;
        private Button _btnManageUnits = null!;
        #endregion

        #region Constructor
        public ProductDialog(int productId = -1)
        {
            _productId = productId;
            _isEditMode = productId > 0;

            InitializeComponent();
            CreateUI();
            LoadComboBoxData();
            ApplyTheme();

            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            this.FormClosing += ProductDialog_FormClosing;

            if (_isEditMode)
            {
                LoadProductData();
            }
            else
            {
                _product = clsProduct.CreateNew();
                _txtProductID.Text = "Auto-generated";
                _chkIsActive.Checked = true;
            }
        }

        private void ProductDialog_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Dispose image to release file locks
            if (_pictureBox?.Image != null)
            {
                var img = _pictureBox.Image;
                _pictureBox.Image = null;
                img.Dispose();
            }
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = _isEditMode ? "Edit Product" : "Add New Product";
            this.Size = new Size(500, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            // Error Provider is created in Designer

            // Create sections
            CreateHeader();
            CreateContent();
            CreateButtonsPanel();

            // Add controls in order
            this.Controls.Add(_contentPanel);
            this.Controls.Add(_buttonsPanel);
            this.Controls.Add(_headerPanel);

            this.ResumeLayout(false);

            // Allow dragging from header
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
                Height = 60,
                BackColor = Color.FromArgb(59, 130, 246),
                Padding = new Padding(20, 0, 10, 0)
            };

            _lblTitle = new Label
            {
                Text = _isEditMode ? "Edit Product" : "Add New Product",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 18)
            };

            _btnClose = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Size = new Size(40, 40),
                Cursor = Cursors.Hand,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                CausesValidation = false
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 68, 68);
            _btnClose.Location = new Point(_headerPanel.Width - 50, 10);
            _btnClose.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            _headerPanel.Resize += (s, e) =>
            {
                _btnClose.Location = new Point(_headerPanel.Width - 50, 10);
            };

            _headerPanel.Controls.Add(_lblTitle);
            _headerPanel.Controls.Add(_btnClose);
        }
        #endregion

        #region Content
        private void CreateContent()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 20, 30, 20),
                BackColor = Color.White
            };

            _formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 8,
                AutoSize = false
            };
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25)); // Space for ErrorProvider

            for (int i = 0; i < 8; i++)
            {
                _formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            }

            int row = 0;

            // Product ID
            _lblProductID = CreateLabel("Product ID:");
            _txtProductID = new TextBox
            {
                Font = new Font("Consolas", 10F),
                ReadOnly = true,
                BackColor = Color.FromArgb(248, 250, 252),
                Dock = DockStyle.Fill
            };
            AddFormRow(row++, _lblProductID, _txtProductID);

            // Product Name
            _lblProductName = CreateLabel("Name: *");
            _txtProductName = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                MaxLength = 100,
                Dock = DockStyle.Fill
            };
            _txtProductName.Validating += TxtProductName_Validating;
            AddFormRow(row++, _lblProductName, _txtProductName);

            // Category
            _lblCategory = CreateLabel("Category: *");
            _cmbCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            _cmbCategory.Validating += CmbCategory_Validating;
            AddFormRow(row++, _lblCategory, _cmbCategory);

            // Brand
            _lblBrand = CreateLabel("Brand:");
            _cmbBrand = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            AddFormRow(row++, _lblBrand, _cmbBrand);

            // Reorder Level
            _lblReorderLevel = CreateLabel("Reorder Level:");
            _numReorderLevel = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100000,
                Value = 10,
                Dock = DockStyle.Fill
            };
            AddFormRow(row++, _lblReorderLevel, _numReorderLevel);

            // Description
            _lblDescription = CreateLabel("Description:");
            _txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Multiline = true,
                Height = 60,
                MaxLength = 500,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill
            };
            _formLayout.RowStyles[row] = new RowStyle(SizeType.Absolute, 80);
            AddFormRow(row++, _lblDescription, _txtDescription);

            // Image
            _lblImage = CreateLabel("Image:");
            _formLayout.RowStyles[row] = new RowStyle(SizeType.Absolute, 120);
            CreateImagePanel();
            AddFormRow(row++, _lblImage, _imagePanel);

            // Is Active
            _lblIsActive = CreateLabel("Active:");
            _chkIsActive = new CheckBox
            {
                Font = new Font("Segoe UI", 10F),
                Text = "Product is active",
                Checked = true,
                AutoSize = true
            };
            AddFormRow(row++, _lblIsActive, _chkIsActive);

            _contentPanel.Controls.Add(_formLayout);
        }

        private void CreateImagePanel()
        {
            _imagePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 110
            };

            // PictureBox for image preview
            _pictureBox = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(0, 5),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 250, 252),
                Image = ImageManager.GetPlaceholderImage()
            };

            // Upload button
            _btnUploadImage = new Button
            {
                Text = "📁 Upload",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Size = new Size(90, 32),
                Location = new Point(110, 20),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White
            };
            _btnUploadImage.FlatAppearance.BorderSize = 0;
            _btnUploadImage.Click += BtnUploadImage_Click;

            // Remove button
            _btnRemoveImage = new Button
            {
                Text = "🗑️ Remove",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Size = new Size(90, 32),
                Location = new Point(110, 58),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Enabled = false
            };
            _btnRemoveImage.FlatAppearance.BorderSize = 0;
            _btnRemoveImage.Click += BtnRemoveImage_Click;

            _imagePanel.Controls.Add(_pictureBox);
            _imagePanel.Controls.Add(_btnUploadImage);
            _imagePanel.Controls.Add(_btnRemoveImage);
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Padding = new Padding(0, 8, 0, 0)
            };
        }

        private void AddFormRow(int row, Label label, Control control)
        {
            _formLayout.Controls.Add(label, 0, row);
            _formLayout.Controls.Add(control, 1, row);
        }
        #endregion

        #region Buttons Panel
        private void CreateButtonsPanel()
        {
            _buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20, 15, 20, 15)
            };

            // Save Button
            _btnSave = new Button
            {
                Text = "Save",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(100, 40),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;

            // Cancel Button
            _btnCancel = new Button
            {
                Text = "Cancel",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(100, 40),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                CausesValidation = false
            };
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Delete Button (edit mode only)
            _btnDelete = new Button
            {
                Text = "Delete",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(100, 40),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Visible = _isEditMode
            };
            _btnDelete.FlatAppearance.BorderSize = 0;
            _btnDelete.Click += BtnDelete_Click;

            // Manage Units Button (edit mode only)
            _btnManageUnits = new Button
            {
                Text = "Manage Units",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Visible = _isEditMode
            };
            _btnManageUnits.FlatAppearance.BorderSize = 0;
            _btnManageUnits.Click += BtnManageUnits_Click;

            // Position buttons
            _btnSave.Location = new Point(20, 15);
            _btnCancel.Location = new Point(130, 15);
            _btnDelete.Location = new Point(240, 15);

            _buttonsPanel.Resize += (s, e) =>
            {
                _btnManageUnits.Location = new Point(_buttonsPanel.Width - _btnManageUnits.Width - 20, 15);
            };
            _btnManageUnits.Location = new Point(_buttonsPanel.Width - _btnManageUnits.Width - 20, 15);

            _buttonsPanel.Controls.AddRange(new Control[]
            {
                _btnSave,
                _btnCancel,
                _btnDelete,
                _btnManageUnits
            });
        }
        #endregion

        #region Data Loading
        private void LoadComboBoxData()
        {
            // Load Categories
            try
            {
                var categories = clsCategory.GetAllCategory();
                _cmbCategory.Items.Clear();
                _cmbCategory.Items.Add(new ComboBoxItem(-1, "-- Select Category --"));

                if (categories != null)
                {
                    foreach (DataRow row in categories.Rows)
                    {
                        int id = Convert.ToInt32(row["CategoryID"]);
                        string name = row["CategoryName"]?.ToString() ?? "";
                        _cmbCategory.Items.Add(new ComboBoxItem(id, name));
                    }
                }
                _cmbCategory.SelectedIndex = 0;
                _cmbCategory.DisplayMember = "Name";
            }
            catch { }

            // Load Brands
            try
            {
                var brands = clsBrand.GetAllBrand();
                _cmbBrand.Items.Clear();
                _cmbBrand.Items.Add(new ComboBoxItem(-1, "-- Select Brand (Optional) --"));

                if (brands != null)
                {
                    foreach (DataRow row in brands.Rows)
                    {
                        int id = Convert.ToInt32(row["BrandID"]);
                        string name = row["BrandName"]?.ToString() ?? "";
                        _cmbBrand.Items.Add(new ComboBoxItem(id, name));
                    }
                }
                _cmbBrand.SelectedIndex = 0;
                _cmbBrand.DisplayMember = "Name";
            }
            catch { }
        }

        private void LoadProductData()
        {
            _product = clsProduct.Find(_productId);
            if (_product == null)
            {
                MessageBox.Show("Product not found!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            _txtProductID.Text = _product.ProductID.ToString();
            _txtProductName.Text = _product.ProductName;
            _numReorderLevel.Value = _product.ReorderLevel;
            _txtDescription.Text = _product.Description ?? "";
            _chkIsActive.Checked = _product.IsActive;

            // Load product image
            if (!string.IsNullOrEmpty(_product.ImagePath))
            {
                try
                {
                    var image = ImageManager.LoadPreview(_product.ImagePath);
                    if (image != null)
                    {
                        // Dispose old image first
                        if (_pictureBox.Image != null)
                        {
                            var oldImage = _pictureBox.Image;
                            _pictureBox.Image = null;
                            oldImage.Dispose();
                        }
                        _pictureBox.Image = image;
                        _btnRemoveImage.Enabled = true;
                    }
                }
                catch
                {
                    // If image loading fails, use placeholder
                    _pictureBox.Image = ImageManager.GetPlaceholderImage();
                }
            }

            // Select Category
            for (int i = 0; i < _cmbCategory.Items.Count; i++)
            {
                if (_cmbCategory.Items[i] is ComboBoxItem item && item.Value == _product.CategoryID)
                {
                    _cmbCategory.SelectedIndex = i;
                    break;
                }
            }

            // Select Brand
            if (_product.BrandID.HasValue)
            {
                for (int i = 0; i < _cmbBrand.Items.Count; i++)
                {
                    if (_cmbBrand.Items[i] is ComboBoxItem item && item.Value == _product.BrandID.Value)
                    {
                        _cmbBrand.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Image Handlers
        private void BtnUploadImage_Click(object? sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog
            {
                Title = "Select Product Image",
                Filter = ImageManager.GetImageFileFilter(),
                FilterIndex = 1
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openDialog.FileName;

                // Validate file
                if (!ImageManager.IsValidImageFile(filePath, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Invalid Image", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Load preview into PictureBox
                    var image = ImageManager.LoadPreview(filePath);
                    if (image != null)
                    {
                        _pictureBox.Image?.Dispose();
                        _pictureBox.Image = image;
                        _selectedImagePath = filePath;
                        _imageRemoved = false;
                        _btnRemoveImage.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemoveImage_Click(object? sender, EventArgs e)
        {
            _pictureBox.Image?.Dispose();
            _pictureBox.Image = ImageManager.GetPlaceholderImage();
            _selectedImagePath = null;
            _imageRemoved = true;
            _btnRemoveImage.Enabled = false;
        }
        #endregion

        #region Validation
        private void TxtProductName_Validating(object? sender, CancelEventArgs e)
        {
            // Show error but don't block navigation (e.Cancel = false)
            if (string.IsNullOrWhiteSpace(_txtProductName.Text))
            {
                _errorProvider.SetError(_txtProductName, "Product name is required");
            }
            else
            {
                _errorProvider.SetError(_txtProductName, "");
            }
        }

        private void CmbCategory_Validating(object? sender, CancelEventArgs e)
        {
            // Show error but don't block navigation (e.Cancel = false)
            if (_cmbCategory.SelectedItem is ComboBoxItem item && item.Value == -1)
            {
                _errorProvider.SetError(_cmbCategory, "Please select a category");
            }
            else
            {
                _errorProvider.SetError(_cmbCategory, "");
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Validate Product Name
            if (string.IsNullOrWhiteSpace(_txtProductName.Text))
            {
                _errorProvider.SetError(_txtProductName, "Product name is required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(_txtProductName, "");
            }

            // Validate Category
            if (_cmbCategory.SelectedItem is not ComboBoxItem catItem || catItem.Value == -1)
            {
                _errorProvider.SetError(_cmbCategory, "Please select a category");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(_cmbCategory, "");
            }

            return isValid;
        }
        #endregion

        #region Button Handlers
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Please correct the errors before saving.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_product == null)
                    _product = clsProduct.CreateNew();

                _product.ProductName = _txtProductName.Text.Trim();
                _product.CategoryID = (_cmbCategory.SelectedItem as ComboBoxItem)?.Value ?? -1;

                var brandItem = _cmbBrand.SelectedItem as ComboBoxItem;
                _product.BrandID = (brandItem != null && brandItem.Value > 0) ? brandItem.Value : null;

                _product.ReorderLevel = (int)_numReorderLevel.Value;
                _product.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                _product.IsActive = _chkIsActive.Checked;

                // Handle image changes
                if (_imageRemoved && !string.IsNullOrEmpty(_product.ImagePath))
                {
                    // Delete old image file
                    ImageManager.DeleteImage(_product.ImagePath);
                    _product.ImagePath = null;
                }
                else if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(_product.ImagePath))
                    {
                        ImageManager.DeleteImage(_product.ImagePath);
                    }
                    
                    // Save new image
                    _product.ImagePath = ImageManager.SaveProductImage(_selectedImagePath);
                }

                if (_product.Save())
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Failed to save product. Please try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!_isEditMode || _product == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{_product.ProductName}'?\n\nThis action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (clsProduct.DeleteProduct(_product.ProductID))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product. It may be referenced by other records.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnManageUnits_Click(object? sender, EventArgs e)
        {
            if (!_isEditMode || _product == null) return;

            try
            {
                using (var dialog = new ProductUnitsDialog(_product.ProductID, _product.ProductName))
                {
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening units dialog: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Drag Support
        private void Header_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Windows API for dragging
                const int WM_NCLBUTTONDOWN = 0xA1;
                const int HT_CAPTION = 0x2;

                this.Capture = false;
                var msg = Message.Create(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.ContentBackground;
            _contentPanel.BackColor = colors.ContentBackground;

            // Labels
            var labels = new[] { _lblProductID, _lblProductName, _lblCategory, _lblBrand, _lblReorderLevel, _lblDescription, _lblIsActive };
            foreach (var lbl in labels)
            {
                if (lbl != null)
                    lbl.ForeColor = colors.SecondaryText;
            }

            // TextBoxes
            _txtProductName.BackColor = colors.ContentBackground;
            _txtProductName.ForeColor = colors.PrimaryText;
            _txtDescription.BackColor = colors.ContentBackground;
            _txtDescription.ForeColor = colors.PrimaryText;

            // ComboBoxes
            _cmbCategory.BackColor = colors.ContentBackground;
            _cmbCategory.ForeColor = colors.PrimaryText;
            _cmbBrand.BackColor = colors.ContentBackground;
            _cmbBrand.ForeColor = colors.PrimaryText;

            // NumericUpDown
            _numReorderLevel.BackColor = colors.ContentBackground;
            _numReorderLevel.ForeColor = colors.PrimaryText;

            // CheckBox
            _chkIsActive.ForeColor = colors.PrimaryText;

            // Buttons panel
            _buttonsPanel.BackColor = colors.FormBackground;

            Invalidate(true);
        }
        #endregion

        #region Helper Class
        private class ComboBoxItem
        {
            public int Value { get; }
            public string Name { get; }

            public ComboBoxItem(int value, string name)
            {
                Value = value;
                Name = name;
            }

            public override string ToString() => Name;
        }
        #endregion
    }
}
