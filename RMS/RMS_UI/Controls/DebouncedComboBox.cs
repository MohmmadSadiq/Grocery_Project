using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RMS_UI.Controls
{
    /// <summary>
    /// A Custom Control that extends ComboBox with debounce functionality.
    /// Uses an internal TextBox overlay for typing — the ComboBox dropdown is only for display and selection.
    /// The TextBox text only changes on explicit user input, Enter, or mouse click selection.
    /// Arrow-key navigation in the dropdown does NOT alter the typed text.
    /// </summary>
    public class DebouncedComboBox : ComboBox
    {
        #region P/Invoke

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [DllImport("user32.dll")]
        private static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO pcbi);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_HIDE = 0;

        #endregion

        #region Private Fields

        private TextBox _inputTextBox;
        private System.Windows.Forms.Timer _debounceTimer;
        private int _debounceInterval = 500; // milliseconds
        private bool _textBoxMode = false;
        private bool _suppressTextBoxEvents = false;

        #endregion

        #region Events

        /// <summary>
        /// Fires after the user stops typing for the duration of DebounceInterval.
        /// The EventArgs contain the current search text.
        /// </summary>
        [Category("Behavior")]
        [Description("Occurs after the user stops typing for the debounce interval duration.")]
        public event EventHandler<SearchReadyEventArgs>? SearchReady;

        /// <summary>
        /// Fires when the user presses Enter while the control has focus.
        /// </summary>
        [Category("Behavior")]
        [Description("Occurs when the user presses Enter.")]
        public event EventHandler? EnterPressed;

        #endregion

        #region Constructor

        public DebouncedComboBox()
        {
            _debounceTimer = new System.Windows.Forms.Timer();
            _debounceTimer.Interval = _debounceInterval;
            _debounceTimer.Tick += DebounceTimer_Tick;

            // Allow the user to type freely
            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.AutoCompleteMode = AutoCompleteMode.None;
            this.AutoCompleteSource = AutoCompleteSource.None;

            // Create the overlay TextBox that sits on top of the ComboBox edit area
            _inputTextBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = this.Font,
                BackColor = this.BackColor,
                ForeColor = this.ForeColor,
                TabStop = false,
            };

            _inputTextBox.TextChanged += InputTextBox_TextChanged;
            _inputTextBox.KeyDown += InputTextBox_KeyDown;
            _inputTextBox.KeyPress += InputTextBox_KeyPress;
            _inputTextBox.KeyUp += InputTextBox_KeyUp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the debounce delay in milliseconds. Default is 500ms.
        /// </summary>
        [Category("Behavior")]
        [Description("The debounce delay in milliseconds before SearchReady fires after the last keystroke.")]
        [DefaultValue(500)]
        public int DebounceInterval
        {
            get => _debounceInterval;
            set
            {
                _debounceInterval = Math.Max(100, value); // minimum 100ms
                _debounceTimer.Interval = _debounceInterval;
            }
        }

        /// <summary>
        /// Gets the current search text from the overlay TextBox.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchText => _inputTextBox.Text;

        /// <summary>
        /// Gets or sets whether the control behaves as a TextBox only (no dropdown, no events).
        /// When true, the dropdown is disabled and SearchReady event will not fire.
        /// </summary>
        [Category("Behavior")]
        [Description("When true, the control acts as a TextBox only - no dropdown and no SearchReady event.")]
        [DefaultValue(false)]
        public bool TextBoxMode
        {
            get => _textBoxMode;
            set
            {
                _textBoxMode = value;
                if (_textBoxMode)
                {
                    _debounceTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Shadows ComboBox.Text — reads/writes the overlay TextBox instead.
        /// The base ComboBox text may change freely (e.g. during dropdown navigation)
        /// but remains invisible behind the TextBox overlay.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => _inputTextBox.Text;
            set
            {
                _suppressTextBoxEvents = true;
                _inputTextBox.Text = value ?? string.Empty;
                _inputTextBox.SelectionStart = _inputTextBox.Text.Length;
                _inputTextBox.SelectionLength = 0;
                _suppressTextBoxEvents = false;
            }
        }

        /// <summary>
        /// Shadows ComboBox.SelectionStart — delegates to the overlay TextBox.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SelectionStart
        {
            get => _inputTextBox.SelectionStart;
            set => _inputTextBox.SelectionStart = value;
        }

        /// <summary>
        /// Shadows ComboBox.SelectionLength — delegates to the overlay TextBox.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SelectionLength
        {
            get => _inputTextBox.SelectionLength;
            set => _inputTextBox.SelectionLength = value;
        }

        #endregion

        #region Overridden Methods

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            PositionTextBox();
            if (!this.Controls.Contains(_inputTextBox))
                this.Controls.Add(_inputTextBox);
            _inputTextBox.BringToFront();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (IsHandleCreated)
                PositionTextBox();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _inputTextBox.Font = this.Font;
            if (IsHandleCreated)
                PositionTextBox();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _inputTextBox.BackColor = this.BackColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _inputTextBox.ForeColor = this.ForeColor;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _inputTextBox.Focus();
        }

        /// <summary>
        /// Base ComboBox text changes are irrelevant — debounce is driven by the overlay TextBox.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (_textBoxMode)
                return;

            base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// Fires when the user explicitly selects an item (mouse click on dropdown).
        /// Commits the selected item text to the overlay TextBox.
        /// </summary>
        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            base.OnSelectionChangeCommitted(e);

            if (base.SelectedIndex >= 0 && base.SelectedItem != null)
            {
                _suppressTextBoxEvents = true;
                _inputTextBox.Text = base.SelectedItem.ToString() ?? "";
                _inputTextBox.SelectionStart = _inputTextBox.Text.Length;
                _inputTextBox.SelectionLength = 0;
                _suppressTextBoxEvents = false;
            }
        }

        protected override void OnDropDown(EventArgs e)
        {
            if (_textBoxMode)
                return;

            base.OnDropDown(e);

            // Restore mouse cursor that gets hidden when DroppedDown is set programmatically
            Cursor.Current = Cursors.Default;

            this.BeginInvoke(new Action(() =>
            {
                base.SelectedIndex = -1;
            }));
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            _inputTextBox.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _debounceTimer?.Stop();
                _debounceTimer?.Dispose();
                _inputTextBox?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Positions the overlay TextBox to cover the ComboBox's edit area
        /// (excluding the dropdown arrow button) using GetComboBoxInfo.
        /// </summary>
        private void PositionTextBox()
        {
            var info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO));
            if (GetComboBoxInfo(this.Handle, ref info))
            {
                _inputTextBox.SetBounds(
                    info.rcItem.Left,
                    info.rcItem.Top,
                    info.rcItem.Right - info.rcItem.Left,
                    info.rcItem.Bottom - info.rcItem.Top);

                // Hide the native EDIT control so it never paints over our TextBox
                if (info.hwndItem != IntPtr.Zero)
                    ShowWindow(info.hwndItem, SW_HIDE);
            }
        }

        #endregion

        #region TextBox Event Handlers

        private void InputTextBox_TextChanged(object? sender, EventArgs e)
        {
            if (_suppressTextBoxEvents || _textBoxMode)
                return;

            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void InputTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            // Ctrl+A: select all text
            if (e.Control && e.KeyCode == Keys.A)
            {
                _inputTextBox.SelectAll();
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Enter && !_textBoxMode)
            {
                e.SuppressKeyPress = true;
                _debounceTimer.Stop();
                EnterPressed?.Invoke(this, EventArgs.Empty);
                return;
            }

            // Forward arrow keys to ComboBox dropdown navigation
            if (this.DroppedDown)
            {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    if (base.SelectedIndex < this.Items.Count - 1)
                        base.SelectedIndex++;
                    return;
                }
                if (e.KeyCode == Keys.Up)
                {
                    e.Handled = true;
                    if (base.SelectedIndex > 0)
                        base.SelectedIndex--;
                    return;
                }
            }

            if (e.KeyCode == Keys.Escape && this.DroppedDown)
            {
                e.Handled = true;
                this.DroppedDown = false;
                return;
            }
        }

        private void InputTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Forward to DebouncedComboBox's KeyPress event for consumer subscribers
            OnKeyPress(e);
        }

        private void InputTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        private void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();
            OnSearchReady(new SearchReadyEventArgs(_inputTextBox.Text));
        }

        #endregion

        #region Protected Methods

        protected virtual void OnSearchReady(SearchReadyEventArgs e)
        {
            SearchReady?.Invoke(this, e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears the text and stops any pending debounce.
        /// </summary>
        public void Clear()
        {
            _debounceTimer.Stop();
            this.Text = string.Empty;
        }

        /// <summary>
        /// Stops any pending debounce timer without firing the event.
        /// </summary>
        public void CancelPendingSearch()
        {
            _debounceTimer.Stop();
        }

        /// <summary>
        /// Forces the SearchReady event to fire immediately with the current text.
        /// Does nothing if TextBoxMode is true.
        /// </summary>
        public void ForceSearch()
        {
            _debounceTimer.Stop();
            if (!_textBoxMode)
                OnSearchReady(new SearchReadyEventArgs(_inputTextBox.Text));
        }

        #endregion
    }

    #region EventArgs

    /// <summary>
    /// Event arguments for the SearchReady event.
    /// </summary>
    public class SearchReadyEventArgs : EventArgs
    {
        /// <summary>
        /// The text the user typed in the ComboBox.
        /// </summary>
        public string SearchText { get; }

        public SearchReadyEventArgs(string searchText)
        {
            SearchText = searchText;
        }
    }

    #endregion
}
