using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Products
{
    [DesignerCategory("UserControl")]
    public partial class ctrlProductFinder : UserControl
    {
        #region Private Fields

        private DataTable? _nameSearchResults;
        private DataTable? _barcodeSearchResults;

        #endregion

        #region Properties

        /// <summary>
        /// List of all ProductUnits for the currently selected product.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<clsProductUnit> ProductUnits { get; private set; } = new List<clsProductUnit>();

        /// <summary>
        /// The currently selected ProductUnit.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public clsProductUnit? SelectedProductUnit { get; private set; }

        /// <summary>
        /// The currently selected Product.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public clsProduct? SelectedProduct { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Fires when a product is selected (via name search or barcode search).
        /// </summary>
        public event EventHandler? ProductSelected;

        /// <summary>
        /// Fires when a unit is selected from the units combo box or via barcode search.
        /// </summary>
        public event EventHandler? UnitSelected;

        #endregion

        #region Constructor

        public ctrlProductFinder()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // Wire events
            _cmbSearchByName.SearchReady += _cmbSearchByName_SearchReady;
            _cmbSearchByName.SelectedIndexChanged += _cmbSearchByName_SelectedIndexChanged;
            _cmbSearchByName.EnterPressed += _cmbSearchByName_EnterPressed;
            _cmbSearchByName.KeyPress += _cmbSearchByName_KeyPress;
            _cmbUnits.SelectedIndexChanged += _cmbUnits_SelectedIndexChanged;
            _cmbSearchByBarcode.SearchReady += _cmbSearchByBarcode_SearchReady;
            _cmbSearchByBarcode.SelectedIndexChanged += _cmbSearchByBarcode_SelectedIndexChanged;
            _cmbSearchByBarcode.EnterPressed += _cmbSearchByBarcode_EnterPressed;
            _cmbSearchByBarcode.KeyPress += _cmbSearchByBarcode_KeyPress;

            // Card border
            _pnlCard.Paint += _pnlCard_Paint;

            // Theme
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        #endregion

        #region Theme & Painting

        /// <summary>
        /// Applies the current theme colors to all child controls.
        /// </summary>
        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;

            // Card background
            _pnlCard.BackColor = c.ContentBackground;

            // Section header
            _lblSectionTitle.ForeColor = c.Primary;

            // Separator
            _pnlSeparator.BackColor = c.BorderColor;

            // Field labels
            foreach (var lbl in new[] { _lblSearchByName, _lblUnits, _lblSearchByBarcode })
            {
                lbl.ForeColor = c.SecondaryText;
            }

            // ComboBoxes
            _cmbUnits.BackColor = c.ContentBackground;
            _cmbUnits.ForeColor = c.PrimaryText;

            _cmbSearchByName.BackColor = c.ContentBackground;
            _cmbSearchByName.ForeColor = c.PrimaryText;

            _cmbSearchByBarcode.BackColor = c.ContentBackground;
            _cmbSearchByBarcode.ForeColor = c.PrimaryText;

            // Browse button
            _btnBrowseProducts.BackColor = c.Primary;
            _btnBrowseProducts.ForeColor = Color.White;

            _pnlCard.Invalidate();
        }

        private void _pnlCard_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is Panel pnl)
            {
                var c = ThemeManager.Colors;
                using var pen = new Pen(c.BorderColor, 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        #endregion

        #region Search by Name

        private void _cmbSearchByName_SearchReady(object? sender, SearchReadyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.SearchText))
                return;

            // Clear previous selections when starting a new search
            _ClearPreviousSelection();

            // Search products by name, 5 results max
            var criteria = new clsProduct.ProductSearchCriteria
            {
                SearchText = e.SearchText,
                SearchBy = "Name",
                PageNumber = 1,
                PageSize = 5
            };

            _nameSearchResults = clsProduct.SearchProductsPages(criteria);

            // TextBoxMode blocks all events during population
            _cmbSearchByName.TextBoxMode = true;
            _cmbSearchByName.Items.Clear();

            if (_nameSearchResults != null && _nameSearchResults.Rows.Count > 0)
            {
                foreach (DataRow row in _nameSearchResults.Rows)
                {
                    _cmbSearchByName.Items.Add(row["ProductName"].ToString() ?? "");
                }
            }

            // Restore user's typed text and cursor (no item selected)
            _cmbSearchByName.SelectedIndex = -1;
            _cmbSearchByName.Text = e.SearchText;
            _cmbSearchByName.SelectionStart = e.SearchText.Length;
            _cmbSearchByName.SelectionLength = 0;
            _cmbSearchByName.TextBoxMode = false;

            if (_nameSearchResults != null && _nameSearchResults.Rows.Count > 0)
            {
                _cmbSearchByName.DroppedDown = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void _cmbSearchByName_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Only act when dropdown is closed (mouse click selection).
            // Arrow key navigation keeps dropdown open — let EnterPressed handle that.
            if (_cmbSearchByName.DroppedDown)
                return;

            int selectedIndex = _cmbSearchByName.SelectedIndex;
            if (selectedIndex < 0 || _nameSearchResults == null || selectedIndex >= _nameSearchResults.Rows.Count)
                return;

            SelectProductFromNameSearch(selectedIndex);
        }

        /// <summary>
        /// When Enter is pressed: select first item if available, otherwise clear.
        /// </summary>
        private void _cmbSearchByName_EnterPressed(object? sender, EventArgs e)
        {
            if (_nameSearchResults != null && _nameSearchResults.Rows.Count > 0)
            {
                // Use the highlighted item if user navigated with arrow keys, otherwise first item
                int indexToSelect = _cmbSearchByName.SelectedIndex >= 0
                    ? _cmbSearchByName.SelectedIndex
                    : 0;

                _cmbSearchByName.DroppedDown = false;
                SelectProductFromNameSearch(indexToSelect);
            }
            else
            {
                // No results — clear everything
                _ClearNameSearch();
                _cmbUnits.Items.Clear();
                _cmbUnits.SelectedIndex = -1;
                SelectedProduct = null;
                SelectedProductUnit = null;
                ProductUnits = new List<clsProductUnit>();
            }
        }

        /// <summary>
        /// Selects a product from the name search results at the given index.
        /// Loads all units for the product into the units combo box.
        /// </summary>
        private void SelectProductFromNameSearch(int index)
        {
            if (_nameSearchResults == null || index < 0 || index >= _nameSearchResults.Rows.Count)
                return;

            DataRow row = _nameSearchResults.Rows[index];
            int productId = Convert.ToInt32(row["ProductID"]);
            string productName = row["ProductName"].ToString() ?? "";

            // Load the full product object
            SelectedProduct = clsProduct.Find(productId);
            if (SelectedProduct == null)
                return;

            // Load all units for this product
            ProductUnits = clsProductUnit.GetProductUnitListByProductID(productId);

            // Set the product name in the search combo box (TextBoxMode blocks all events)
            _cmbSearchByName.TextBoxMode = true;
            _cmbSearchByName.Items.Clear();
            _cmbSearchByName.Text = productName;

            // Populate units combo box (will auto-select base unit and set barcode)
            _LoadUnitsComboBox();

            // Fire event
            ProductSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Units ComboBox

        private void _LoadUnitsComboBox()
        {
            _cmbUnits.Items.Clear();
            _cmbUnits.SelectedIndex = -1;
            SelectedProductUnit = null;

            if (ProductUnits.Count == 0)
            {
                _cmbUnits.Items.Add("No Units Found");
                _cmbUnits.SelectedIndex = 0;
                _cmbUnits.Enabled = false;
                return;
            }

            _cmbUnits.Enabled = true;

            int baseUnitIndex = -1;

            for (int i = 0; i < ProductUnits.Count; i++)
            {
                var unit = ProductUnits[i];
                string unitName = unit.UnitInfo?.UnitName ?? $"Unit {unit.UnitID}";
                _cmbUnits.Items.Add(unitName);

                // Track the base unit (ConversionFactor == 1)
                if (unit.ConversionFactor == 1)
                    baseUnitIndex = i;
            }

            // Auto-select the base unit; fallback to first unit
            int selectIndex = baseUnitIndex >= 0 ? baseUnitIndex : 0;
            _cmbUnits.SelectedIndex = selectIndex;
        }

        private void _cmbUnits_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int selectedIndex = _cmbUnits.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= ProductUnits.Count)
            {
                SelectedProductUnit = null;
                return;
            }

            SelectedProductUnit = ProductUnits[selectedIndex];

            // Write the selected unit's barcode into the barcode search box (TextBoxMode blocks all events)
            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.Items.Clear();
            _cmbSearchByBarcode.Text = SelectedProductUnit.Barcode ?? "";

            // Fire event
            UnitSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Search by Barcode

        private void _cmbSearchByBarcode_SearchReady(object? sender, SearchReadyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.SearchText))
                return;

            _PerformBarcodeSearch(e.SearchText);
        }

        /// <summary>
        /// When user types a real character in the barcode box, activate it and deactivate name box.
        /// </summary>
        private void _cmbSearchByBarcode_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (_cmbSearchByBarcode.TextBoxMode)
                _cmbSearchByBarcode.TextBoxMode = false;

            if (!_cmbSearchByName.TextBoxMode)
                _cmbSearchByName.TextBoxMode = true;
        }

        /// <summary>
        /// When user types a real character in the name box, activate it and deactivate barcode box.
        /// </summary>
        private void _cmbSearchByName_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (_cmbSearchByName.TextBoxMode)
                _cmbSearchByName.TextBoxMode = false;

            if (!_cmbSearchByBarcode.TextBoxMode)
                _cmbSearchByBarcode.TextBoxMode = true;
        }

        /// <summary>
        /// Searches for barcodes matching the given text and populates the dropdown.
        /// </summary>
        private void _PerformBarcodeSearch(string searchText)
        {
            // Search product units by barcode prefix, 5 results max
            _barcodeSearchResults = clsProductUnit.SearchByBarcode(searchText, 1, 5);

            // TextBoxMode blocks all events during population
            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.Items.Clear();

            if (_barcodeSearchResults != null && _barcodeSearchResults.Rows.Count > 0)
            {
                foreach (DataRow row in _barcodeSearchResults.Rows)
                {
                    string barcode = row["Barcode"]?.ToString() ?? "";
                    _cmbSearchByBarcode.Items.Add(barcode);
                }
            }

            // Restore user's typed text and cursor (no item selected)
            _cmbSearchByBarcode.SelectedIndex = -1;
            _cmbSearchByBarcode.Text = searchText;
            _cmbSearchByBarcode.SelectionStart = searchText.Length;
            _cmbSearchByBarcode.SelectionLength = 0;
            _cmbSearchByBarcode.TextBoxMode = false;

            if (_barcodeSearchResults != null && _barcodeSearchResults.Rows.Count > 0)
            {
                _cmbSearchByBarcode.DroppedDown = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void _cmbSearchByBarcode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Only act when dropdown is closed (mouse click selection).
            if (_cmbSearchByBarcode.DroppedDown)
                return;

            int selectedIndex = _cmbSearchByBarcode.SelectedIndex;
            if (selectedIndex < 0 || _barcodeSearchResults == null || selectedIndex >= _barcodeSearchResults.Rows.Count)
                return;

            SelectProductUnitFromBarcodeSearch(selectedIndex);
        }

        /// <summary>
        /// When Enter is pressed: select first item if available, otherwise clear.
        /// </summary>
        private void _cmbSearchByBarcode_EnterPressed(object? sender, EventArgs e)
        {
            if (_barcodeSearchResults != null && _barcodeSearchResults.Rows.Count > 0)
            {
                // Use the highlighted item if user navigated with arrow keys, otherwise first item
                int indexToSelect = _cmbSearchByBarcode.SelectedIndex >= 0
                    ? _cmbSearchByBarcode.SelectedIndex
                    : 0;

                _cmbSearchByBarcode.DroppedDown = false;
                SelectProductUnitFromBarcodeSearch(indexToSelect);
            }
            else
            {
                // No results — clear everything
                _ClearBarcodeSearch();
                _cmbUnits.Items.Clear();
                _cmbUnits.SelectedIndex = -1;
                SelectedProduct = null;
                SelectedProductUnit = null;
                ProductUnits = new List<clsProductUnit>();
            }
        }

        /// <summary>
        /// Selects a product unit from the barcode search results at the given index.
        /// Loads only that single unit (per user requirement).
        /// </summary>
        private void SelectProductUnitFromBarcodeSearch(int index)
        {
            if (_barcodeSearchResults == null || index < 0 || index >= _barcodeSearchResults.Rows.Count)
                return;

            DataRow row = _barcodeSearchResults.Rows[index];
            string barcode = row["Barcode"]?.ToString() ?? "";

            // Find the full ProductUnit by exact barcode match
            clsProductUnit? productUnit = clsProductUnit.FindByBarcode(barcode);
            if (productUnit == null)
                return;

            SelectedProductUnit = productUnit;
            SelectedProduct = productUnit.ProductInfo; // Lazy loaded
            ProductUnits = new List<clsProductUnit> { productUnit };

            // Set the barcode in the barcode search combo box (TextBoxMode blocks all events)
            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.CancelPendingSearch();
            _cmbSearchByBarcode.Text = barcode;
            _cmbSearchByBarcode.SelectionStart = barcode.Length;

            // Update name search to show the product name (TextBoxMode blocks all events)
            _cmbSearchByName.TextBoxMode = true;
            _cmbSearchByName.Items.Clear();
            _cmbSearchByName.Text = SelectedProduct?.ProductName ?? "";

            // Load units combo box with single unit
            _LoadUnitsComboBox();

            // Fire events
            ProductSelected?.Invoke(this, EventArgs.Empty);
            UnitSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Browse Products

        private void _btnBrowseProducts_Click(object? sender, EventArgs e)
        {
            using var frm = new frmManageProducts();

            int selectedProductId = -1;

            // Add custom context menu item
            frm.AddContextMenuSeparator();
            frm.AddContextMenuItem("🛒 Select Product", (s, ev) =>
            {
                var dgv = frm.ProductsPage.DataGridView;
                if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                    return;

                var productIdValue = dgv.CurrentRow.Cells["ProductID"]?.Value;
                if (productIdValue == null || productIdValue == DBNull.Value)
                    return;

                selectedProductId = Convert.ToInt32(productIdValue);
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            });

            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK && selectedProductId > 0)
            {
                // Load all units for the selected product and pick the base unit
                var units = clsProductUnit.GetProductUnitListByProductID(selectedProductId);
                if (units.Count > 0)
                {
                    // Prefer the base unit (ConversionFactor == 1), fallback to first
                    var baseUnit = units.Find(u => u.ConversionFactor == 1) ?? units[0];
                    SetProductUnitByID(baseUnit.ProductUnitID);

                    // Fire events
                    ProductSelected?.Invoke(this, EventArgs.Empty);
                    UnitSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Clears previous product/unit selections and the units/barcode controls
        /// without clearing the name search text (called when a new name search starts).
        /// </summary>
        private void _ClearPreviousSelection()
        {
            SelectedProduct = null;
            SelectedProductUnit = null;
            ProductUnits = new List<clsProductUnit>();

            _cmbUnits.Items.Clear();
            _cmbUnits.SelectedIndex = -1;
            _cmbUnits.Enabled = true;

            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.SelectedIndex = -1;
            _cmbSearchByBarcode.Items.Clear();
            _cmbSearchByBarcode.Text = string.Empty;
            _cmbSearchByBarcode.CancelPendingSearch();
            _barcodeSearchResults = null;
        }

        private void _ClearBarcodeSearch()
        {
            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.CancelPendingSearch();
            _cmbSearchByBarcode.SelectedIndex = -1;
            _cmbSearchByBarcode.Items.Clear();
            _cmbSearchByBarcode.Text = string.Empty;
            _cmbSearchByBarcode.TextBoxMode = false;
            _barcodeSearchResults = null;
        }

        private void _ClearNameSearch()
        {
            _cmbSearchByName.TextBoxMode = true;
            _cmbSearchByName.CancelPendingSearch();
            _cmbSearchByName.SelectedIndex = -1;
            _cmbSearchByName.Items.Clear();
            _cmbSearchByName.Text = string.Empty;
            _cmbSearchByName.TextBoxMode = false;
            _nameSearchResults = null;
        }

        /// <summary>
        /// Programmatically sets the control to display a specific product unit (by ID).
        /// Loads the product, all its units, and selects the specified unit.
        /// </summary>
        public void SetProductUnitByID(int productUnitID)
        {
            clsProductUnit? pu = clsProductUnit.Find(productUnitID);
            if (pu == null) return;

            SelectedProduct = pu.ProductInfo;
            if (SelectedProduct == null) return;

            // Load all units for this product
            ProductUnits = clsProductUnit.GetProductUnitListByProductID(SelectedProduct.ProductID);

            // Set the product name (TextBoxMode blocks events)
            _cmbSearchByName.TextBoxMode = true;
            _cmbSearchByName.CancelPendingSearch();
            _cmbSearchByName.Items.Clear();
            _cmbSearchByName.Text = SelectedProduct.ProductName ?? "";

            // Set the barcode (TextBoxMode blocks events)
            _cmbSearchByBarcode.TextBoxMode = true;
            _cmbSearchByBarcode.CancelPendingSearch();
            _cmbSearchByBarcode.Items.Clear();
            _cmbSearchByBarcode.Text = pu.Barcode ?? "";

            // Load units combo box and select the matching unit
            _cmbUnits.Items.Clear();
            _cmbUnits.SelectedIndex = -1;
            _cmbUnits.Enabled = true;

            int selectIndex = 0;
            for (int i = 0; i < ProductUnits.Count; i++)
            {
                var unit = ProductUnits[i];
                string unitName = unit.UnitInfo?.UnitName ?? $"Unit {unit.UnitID}";
                _cmbUnits.Items.Add(unitName);

                if (unit.ProductUnitID == productUnitID)
                    selectIndex = i;
            }

            if (_cmbUnits.Items.Count > 0)
                _cmbUnits.SelectedIndex = selectIndex;
            // SelectedProductUnit will be set by _cmbUnits_SelectedIndexChanged
        }

        /// <summary>
        /// Resets the control to its initial empty state.
        /// </summary>
        public void ResetAll()
        {
            _ClearNameSearch();
            _ClearBarcodeSearch();

            _cmbUnits.Items.Clear();
            _cmbUnits.SelectedIndex = -1;

            ProductUnits = new List<clsProductUnit>();
            SelectedProductUnit = null;
            SelectedProduct = null;
        }

        #endregion
    }
}
