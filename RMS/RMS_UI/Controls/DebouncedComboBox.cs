using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RMS_UI.Controls
{
    /// <summary>
    /// A Custom Control that extends ComboBox with debounce functionality.
    /// When the user types, a timer resets on each keystroke.
    /// After the debounce interval elapses with no new input, the SearchReady event fires.
    /// </summary>
    public class DebouncedComboBox : ComboBox
    {
        #region Private Fields

        private System.Windows.Forms.Timer _debounceTimer;
        private int _debounceInterval = 500; // milliseconds
        private bool _textBoxMode = false;
        private bool _ctrlHeld = false;

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
        /// Gets the current search text (alias for Text).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchText => this.Text;

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

        #endregion

        #region Overridden Methods

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // Don't start search when:
            // - TextBoxMode is on (programmatic text)
            // - Ctrl is held (e.g. Ctrl+A)
            // - Dropdown is open (arrow key navigation changes text automatically)
            if (!_textBoxMode && !_ctrlHeld && !DroppedDown)
            {
                // Reset the timer on every text change (debounce)
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Track Ctrl key state
            if (e.Control)
                _ctrlHeld = true;

            // If Enter is pressed, stop debounce and fire EnterPressed event
            if (e.KeyCode == Keys.Enter && !_textBoxMode)
            {
                e.SuppressKeyPress = true;
                _debounceTimer.Stop();
                EnterPressed?.Invoke(this, EventArgs.Empty);
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            // Reset Ctrl key state
            if (e.KeyCode == Keys.ControlKey)
                _ctrlHeld = false;

            base.OnKeyUp(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            // Suppress SelectedIndexChanged in TextBoxMode
            if (_textBoxMode)
                return;

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDropDown(EventArgs e)
        {
            // Prevent dropdown from opening if in TextBoxMode
            if (_textBoxMode)
            {
                return;
            }
            base.OnDropDown(e);

            // Fix: Restore mouse cursor that gets hidden when DroppedDown is set programmatically
            Cursor.Current = Cursors.Default;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _debounceTimer?.Stop();
                _debounceTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Private Event Handlers

        private void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            // Timer elapsed — stop it and fire the event
            _debounceTimer.Stop();
            OnSearchReady(new SearchReadyEventArgs(this.Text));
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
                OnSearchReady(new SearchReadyEventArgs(this.Text));
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
