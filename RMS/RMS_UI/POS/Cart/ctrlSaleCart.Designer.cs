using System.Drawing;
using System.Windows.Forms;

namespace RMS_UI.POS.Cart
{
    partial class ctrlSaleCart
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            _pnlRoot = new Panel();
            _pnlHeader = new Panel();
            _lblTitle = new Label();
            _lblItemsInfo = new Label();
            _pnlRowsHost = new FlowLayoutPanel();
            _pnlFooter = new Panel();
            _lblTotalLabel = new Label();
            _lblTotalValue = new Label();
            _btnClear = new Button();

            _pnlRoot.SuspendLayout();
            _pnlHeader.SuspendLayout();
            _pnlFooter.SuspendLayout();
            SuspendLayout();

            // _pnlRoot
            _pnlRoot.Controls.Add(_pnlRowsHost);
            _pnlRoot.Controls.Add(_pnlFooter);
            _pnlRoot.Controls.Add(_pnlHeader);
            _pnlRoot.Dock = DockStyle.Fill;
            _pnlRoot.Location = new Point(0, 0);
            _pnlRoot.Name = "_pnlRoot";
            _pnlRoot.Padding = new Padding(12);
            _pnlRoot.Size = new Size(700, 420);
            _pnlRoot.TabIndex = 0;

            // _pnlHeader
            _pnlHeader.Controls.Add(_lblItemsInfo);
            _pnlHeader.Controls.Add(_lblTitle);
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Location = new Point(12, 12);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Size = new Size(676, 38);
            _pnlHeader.TabIndex = 0;

            // _lblTitle
            _lblTitle.Dock = DockStyle.Left;
            _lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblTitle.Location = new Point(0, 0);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(220, 38);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "Sale Cart";
            _lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // _lblItemsInfo
            _lblItemsInfo.Dock = DockStyle.Right;
            _lblItemsInfo.Font = new Font("Segoe UI", 9F);
            _lblItemsInfo.Location = new Point(416, 0);
            _lblItemsInfo.Name = "_lblItemsInfo";
            _lblItemsInfo.Size = new Size(260, 38);
            _lblItemsInfo.TabIndex = 1;
            _lblItemsInfo.Text = "0 lines · 0 items";
            _lblItemsInfo.TextAlign = ContentAlignment.MiddleRight;

            // _pnlRowsHost
            _pnlRowsHost.AutoScroll = true;
            _pnlRowsHost.Dock = DockStyle.Fill;
            _pnlRowsHost.FlowDirection = FlowDirection.TopDown;
            _pnlRowsHost.Location = new Point(12, 50);
            _pnlRowsHost.Margin = new Padding(0);
            _pnlRowsHost.Name = "_pnlRowsHost";
            _pnlRowsHost.Padding = new Padding(0, 6, 0, 6);
            _pnlRowsHost.Size = new Size(676, 300);
            _pnlRowsHost.TabIndex = 1;
            _pnlRowsHost.WrapContents = false;
            _pnlRowsHost.SizeChanged += _pnlRowsHost_SizeChanged;

            // _pnlFooter
            _pnlFooter.Controls.Add(_btnClear);
            _pnlFooter.Controls.Add(_lblTotalValue);
            _pnlFooter.Controls.Add(_lblTotalLabel);
            _pnlFooter.Dock = DockStyle.Bottom;
            _pnlFooter.Location = new Point(12, 350);
            _pnlFooter.Name = "_pnlFooter";
            _pnlFooter.Size = new Size(676, 58);
            _pnlFooter.TabIndex = 2;

            // _lblTotalLabel
            _lblTotalLabel.AutoSize = true;
            _lblTotalLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblTotalLabel.Location = new Point(0, 20);
            _lblTotalLabel.Name = "_lblTotalLabel";
            _lblTotalLabel.Size = new Size(81, 19);
            _lblTotalLabel.TabIndex = 0;
            _lblTotalLabel.Text = "Cart Total:";

            // _lblTotalValue
            _lblTotalValue.AutoSize = true;
            _lblTotalValue.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            _lblTotalValue.Location = new Point(87, 15);
            _lblTotalValue.Name = "_lblTotalValue";
            _lblTotalValue.Size = new Size(52, 25);
            _lblTotalValue.TabIndex = 1;
            _lblTotalValue.Text = "0.00";

            // _btnClear
            _btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnClear.FlatAppearance.BorderSize = 0;
            _btnClear.FlatStyle = FlatStyle.Flat;
            _btnClear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnClear.Location = new Point(579, 14);
            _btnClear.Name = "_btnClear";
            _btnClear.Size = new Size(97, 30);
            _btnClear.TabIndex = 2;
            _btnClear.Text = "Clear Cart";
            _btnClear.UseVisualStyleBackColor = true;
            _btnClear.Click += _btnClear_Click;

            // ctrlSaleCart
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(_pnlRoot);
            Name = "ctrlSaleCart";
            Size = new Size(700, 420);

            _pnlRoot.ResumeLayout(false);
            _pnlHeader.ResumeLayout(false);
            _pnlFooter.ResumeLayout(false);
            _pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel _pnlRoot;
        private Panel _pnlHeader;
        private Label _lblTitle;
        private Label _lblItemsInfo;
        private FlowLayoutPanel _pnlRowsHost;
        private Panel _pnlFooter;
        private Label _lblTotalLabel;
        private Label _lblTotalValue;
        private Button _btnClear;
    }
}
