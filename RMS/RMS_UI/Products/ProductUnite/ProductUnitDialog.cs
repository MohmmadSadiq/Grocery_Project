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
    /// Dialog for adding or editing a ProductUnit.
    /// </summary>
    public partial class ProductUnitDialog : Form
    {
        #region Fields
        private Panel _headerPanel = null!;
        private Label _titleLabel = null!;
        private TableLayoutPanel _formLayout = null!;
        
        private Label _lblUnit = null!;
        private ComboBox _cmbUnit = null!;
        private Label _lblConversionFactor = null!;
        private NumericUpDown _nudConversionFactor = null!;
        private Label _lblSalePrice = null!;
        private NumericUpDown _nudSalePrice = null!;
        private Label _lblBarcode = null!;
        private TextBox _txtBarcode = null!;
        private Label _lblDescription = null!;
        private TextBox _txtDescription = null!;
        private CheckBox _chkIsActive = null!;
        
        private Panel _buttonPanel = null!;
        private Button _btnOK = null!;
        private Button _btnCancel = null!;
        // ErrorProvider is defined in the Designer file

        private int _productId;
        private int _productUnitId = -1;
        private clsProductUnit? _productUnit;
        private bool _isEditMode = false;
        #endregion

        #region Constructors
        public ProductUnitDialog(int productId) : this(productId, -1) { }

        public ProductUnitDialog(int productId, int productUnitId)
        {
            _productId = productId;
            _productUnitId = productUnitId;
            _isEditMode = productUnitId > 0;

            InitializeComponent();
            CreateUI();
            ApplyTheme();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
        }

        private void ProductUnitDialog_Load(object sender, EventArgs e)
        {
            LoadUnits();
            if (_isEditMode)
                LoadProductUnit();
        }
        #endregion

        #region Create UI
        private void CreateUI()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = _isEditMode ? "Edit Product Unit" : "Add New Product Unit";
            this.Size = new Size(480, 450);
            this.MinimumSize = new Size(450, 420);
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
                BackColor = Color.FromArgb(139, 92, 246), // Purple
                Padding = new Padding(15, 0, 15, 0)
            };

            _titleLabel = new Label
            {
                Text = _isEditMode ? "✏️ Edit Product Unit" : "➕ Add New Unit",
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
                RowCount = 7,
                Padding = new Padding(20)
            };
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            _formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;

            // Unit
            _lblUnit = new Label
            {
                Text = "Unit: *",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _cmbUnit = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbUnit.Validating += CmbUnit_Validating;

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblUnit, 0, row);
            _formLayout.Controls.Add(_cmbUnit, 1, row++);

            // Conversion Factor
            _lblConversionFactor = new Label
            {
                Text = "Conversion Factor: *",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _nudConversionFactor = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10F),
                Width = 150,
                Minimum = 0.01M,
                Maximum = 999999,
                Value = 1,
                DecimalPlaces = 2
            };
            _nudConversionFactor.Validating += NudConversionFactor_Validating;

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblConversionFactor, 0, row);
            _formLayout.Controls.Add(_nudConversionFactor, 1, row++);

            // Sale Price
            _lblSalePrice = new Label
            {
                Text = "Sale Price:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _nudSalePrice = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10F),
                Width = 150,
                Minimum = 0,
                Maximum = 999999,
                Value = 0,
                DecimalPlaces = 2
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblSalePrice, 0, row);
            _formLayout.Controls.Add(_nudSalePrice, 1, row++);

            // Barcode
            _lblBarcode = new Label
            {
                Text = "Barcode:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Padding = new Padding(0, 5, 0, 0)
            };
            _txtBarcode = new TextBox
            {
                Font = new Font("Consolas", 10F),
                Width = 250,
                MaxLength = 50
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(_lblBarcode, 0, row);
            _formLayout.Controls.Add(_txtBarcode, 1, row++);

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
            _formLayout.Controls.Add(_lblDescription, 0, row);
            _formLayout.Controls.Add(_txtDescription, 1, row++);

            // Is Active
            _chkIsActive = new CheckBox
            {
                Text = "Active",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Checked = true
            };

            _formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _formLayout.Controls.Add(new Label(), 0, row);
            _formLayout.Controls.Add(_chkIsActive, 1, row++);

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
        private void LoadUnits()
        {
            try
            {
                var dt = clsUnit.GetAllUnit();
                _cmbUnit.Items.Clear();
                _cmbUnit.Items.Add(new ComboBoxItem("-- Select Unit --", null));

                foreach (DataRow row in dt.Rows)
                {
                    // Only show active units
                    if (row["IsActive"] is bool isActive && isActive)
                    {
                        _cmbUnit.Items.Add(new ComboBoxItem(
                            row["UnitName"].ToString() ?? "",
                            (int)row["UnitID"]
                        ));
                    }
                }
                _cmbUnit.SelectedIndex = 0;
                _cmbUnit.DisplayMember = "Text";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading units: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductUnit()
        {
            _productUnit = clsProductUnit.Find(_productUnitId);
            if (_productUnit != null)
            {
                SelectComboBoxItem(_cmbUnit, _productUnit.UnitID);
                _nudConversionFactor.Value = _productUnit.ConversionFactor;
                _nudSalePrice.Value = _productUnit.SalePrice ?? 0;
                _txtBarcode.Text = _productUnit.Barcode ?? "";
                _txtDescription.Text = _productUnit.Description ?? "";
                _chkIsActive.Checked = _productUnit.IsActive;
            }
            else
            {
                MessageBox.Show("Product unit not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SelectComboBoxItem(ComboBox cmb, int? value)
        {
            if (!value.HasValue)
            {
                cmb.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i] is ComboBoxItem item && item.Value == value)
                {
                    cmb.SelectedIndex = i;
                    return;
                }
            }
            cmb.SelectedIndex = 0;
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
                int? selectedUnitId = GetComboBoxValue(_cmbUnit);
                if (!selectedUnitId.HasValue)
                {
                    MessageBox.Show("Please select a unit", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_isEditMode && _productUnit != null)
                {
                    _productUnit.UnitID = selectedUnitId.Value;
                    _productUnit.ConversionFactor = _nudConversionFactor.Value;
                    _productUnit.SalePrice = _nudSalePrice.Value > 0 ? _nudSalePrice.Value : (decimal?)null;
                    _productUnit.Barcode = string.IsNullOrWhiteSpace(_txtBarcode.Text) ? null : _txtBarcode.Text.Trim();
                    _productUnit.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                    _productUnit.IsActive = _chkIsActive.Checked;
                }
                else
                {
                    _productUnit = clsProductUnit.CreateNew();
                    _productUnit.ProductID = _productId;
                    _productUnit.UnitID = selectedUnitId.Value;
                    _productUnit.ConversionFactor = _nudConversionFactor.Value;
                    _productUnit.SalePrice = _nudSalePrice.Value > 0 ? _nudSalePrice.Value : (decimal?)null;
                    _productUnit.Barcode = string.IsNullOrWhiteSpace(_txtBarcode.Text) ? null : _txtBarcode.Text.Trim();
                    _productUnit.Description = string.IsNullOrWhiteSpace(_txtDescription.Text) ? null : _txtDescription.Text.Trim();
                    _productUnit.IsActive = _chkIsActive.Checked;
                }

                if (_productUnit.Save())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save product unit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetComboBoxValue(ComboBox cmb)
        {
            if (cmb.SelectedItem is ComboBoxItem item)
                return item.Value;
            return null;
        }
        #endregion

        #region Validation
        private void CmbUnit_Validating(object? sender, CancelEventArgs e)
        {
            if (_cmbUnit.SelectedIndex <= 0)
            {
                _errorProvider.SetError(_cmbUnit, "Please select a unit");
            }
            else
            {
                _errorProvider.SetError(_cmbUnit, "");
            }
        }

        private void NudConversionFactor_Validating(object? sender, CancelEventArgs e)
        {
            if (_nudConversionFactor.Value <= 0)
            {
                _errorProvider.SetError(_nudConversionFactor, "Conversion factor must be greater than zero");
            }
            else
            {
                _errorProvider.SetError(_nudConversionFactor, "");
            }
        }

        private bool ValidateForm()
        {
            CmbUnit_Validating(null, new CancelEventArgs());
            NudConversionFactor_Validating(null, new CancelEventArgs());

            return string.IsNullOrEmpty(_errorProvider.GetError(_cmbUnit)) &&
                   string.IsNullOrEmpty(_errorProvider.GetError(_nudConversionFactor));
        }
        #endregion

        #region Theme
        public void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            this.BackColor = colors.FormBackground;

            if (_headerPanel != null)
                _headerPanel.BackColor = Color.FromArgb(139, 92, 246); // Keep purple

            if (_formLayout != null)
                _formLayout.BackColor = colors.ContentBackground;

            // Labels
            foreach (var lbl in new[] { _lblUnit, _lblConversionFactor, _lblSalePrice, _lblBarcode, _lblDescription })
            {
                if (lbl != null)
                    lbl.ForeColor = colors.PrimaryText;
            }

            // Controls
            if (_cmbUnit != null)
            {
                _cmbUnit.BackColor = colors.ContentBackground;
                _cmbUnit.ForeColor = colors.PrimaryText;
            }

            foreach (var nud in new[] { _nudConversionFactor, _nudSalePrice })
            {
                if (nud != null)
                {
                    nud.BackColor = colors.ContentBackground;
                    nud.ForeColor = colors.PrimaryText;
                }
            }

            foreach (var txt in new[] { _txtBarcode, _txtDescription })
            {
                if (txt != null)
                {
                    txt.BackColor = colors.ContentBackground;
                    txt.ForeColor = colors.PrimaryText;
                }
            }

            if (_chkIsActive != null)
                _chkIsActive.ForeColor = colors.PrimaryText;

            if (_buttonPanel != null)
                _buttonPanel.BackColor = colors.FormBackground;

            Invalidate(true);
        }
        #endregion

        #region Helper Classes
        private class ComboBoxItem
        {
            public string Text { get; }
            public int? Value { get; }

            public ComboBoxItem(string text, int? value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
        #endregion
    }
}
