using RMS_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMS_UI.Controls
{
    public partial class ctrlCompanyCard : UserControl
    {
        private RMS_Business.clsCompany? _company = null;
        private const int BorderRadius = 12;
        private const int ShadowSize = 8;

        public ctrlCompanyCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            RMS_UI.Utilities.ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        public void LoadCompany(RMS_Business.clsCompany? company)
        {
            _company = company;
            if (_company != null)
            {
                LoadCompanyInfo();
            }
        }
        public void LoadCompany(int companyID)
        {
            _company = clsCompany.Find(companyID);
            if (_company != null)
            {
                LoadCompanyInfo();
            }
        }

        private void LoadCompanyInfo()
        {
            if (_company == null) return;
            _lblCompanyName.Text = _company.CompanyName;
            _lblCompanyID.Text = $"Company ID: {_company.CompanyID}";
            _lblCommercialNumber.Text = $"Commercial No: {(_company.CommercialNumber ?? "N/A")}";
            _lblPhone.Text = _company.Phone ?? "No phone provided";
            _lblEmail.Text = _company.Email ?? "No email provided";
            _lblAddress.Text = _company.Address ?? "No address provided";
            _lblCountryID.Text = $"Country: {(_company.Country?.CountryName ?? "N/A")}";
            
            // Load Contact Person
            if (_company.ContactPerson != null)
            {
                _lblContactPersonID.Text = "Contact Person:";
                _btnViewContactPerson.Text = _company.ContactPerson.FullName;
                _btnViewContactPerson.Enabled = true;
            }
            else
            {
                _lblContactPersonID.Text = "Contact Person:";
                _btnViewContactPerson.Text = "Not assigned";
                _btnViewContactPerson.Enabled = false;
            }
            
            // Load Audit Information
            LoadAuditInfo();
        }

        private void LoadAuditInfo()
        {
            if (_company == null) return;
            
            // Created Date
            if (_company.CreatedDate != DateTime.MinValue)
            {
                _lblCreatedDate.Text = _company.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                _lblCreatedDate.Text = "N/A";
            }
            
            // Created By User
            if (_company.CreatedByUser != null)
            {
                _lblCreatedBy.Text = _company.CreatedByUser.UserName ?? "N/A";
            }
            else
            {
                _lblCreatedBy.Text = "N/A";
            }
            
            // Updated Date
            if (_company.UpdatedDate != DateTime.MinValue)
            {
                _lblUpdatedDate.Text = _company.UpdatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                _lblUpdatedDate.Text = "N/A";
            }
            
            // Updated By User
            if (_company.UpdatedByUser != null)
            {
                _lblUpdatedBy.Text = _company.UpdatedByUser.UserName ?? "N/A";
            }
            else
            {
                _lblUpdatedBy.Text = "N/A";
            }
        }

        private void ApplyTheme()
        {
            var colors = RMS_UI.Utilities.ThemeManager.Colors;
            BackColor = colors.ContentBackground;
            
            // Main content labels
            _lblCompanyName.ForeColor = colors.TitleText;
            _lblCompanyID.ForeColor = colors.SecondaryText;
            _lblCommercialNumber.ForeColor = colors.SecondaryText;
            _lblPhone.ForeColor = colors.PrimaryText;
            _lblEmail.ForeColor = colors.PrimaryText;
            _lblAddress.ForeColor = colors.PrimaryText;
            _lblCountryID.ForeColor = colors.SecondaryText;
            
            // Contact Person
            _lblContactPersonID.ForeColor = colors.SecondaryText;
            _btnViewContactPerson.ForeColor = colors.PrimaryText;
            _btnViewContactPerson.BackColor = colors.PrimaryLight;
            _btnViewContactPerson.FlatStyle = FlatStyle.Flat;
            _btnViewContactPerson.FlatAppearance.BorderColor = colors.BorderAccent;
            
            // Audit Info Labels
            _lblCreatedDateLabel.ForeColor = colors.SecondaryText;
            _lblCreatedDate.ForeColor = colors.PrimaryText;
            _lblCreatedByLabel.ForeColor = colors.SecondaryText;
            _lblCreatedBy.ForeColor = colors.PrimaryText;
            _lblUpdatedDateLabel.ForeColor = colors.SecondaryText;
            _lblUpdatedDate.ForeColor = colors.PrimaryText;
            _lblUpdatedByLabel.ForeColor = colors.SecondaryText;
            _lblUpdatedBy.ForeColor = colors.PrimaryText;
            
            _pnlAuditInfo.BackColor = colors.ContentBackground;
            
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var colors = RMS_UI.Utilities.ThemeManager.Colors;
            
            // Draw shadow
            using (var shadowBrush = new SolidBrush(colors.ShadowColor))
            {
                var shadowRect = new Rectangle(ShadowSize, ShadowSize, Width - ShadowSize * 2, Height - ShadowSize * 2);
                var path = RoundedRectangle(shadowRect, BorderRadius);
                e.Graphics.FillPath(shadowBrush, path);
            }
            
            // Draw background
            using (var backgroundBrush = new SolidBrush(colors.ContentBackground))
            {
                var bgRect = new Rectangle(0, 0, Width - ShadowSize, Height - ShadowSize);
                var path = RoundedRectangle(bgRect, BorderRadius);
                e.Graphics.FillPath(backgroundBrush, path);
            }
            
            // Draw border
            using (var borderPen = new Pen(colors.BorderColor, 1))
            {
                var borderRect = new Rectangle(0, 0, Width - ShadowSize - 1, Height - ShadowSize - 1);
                var path = RoundedRectangle(borderRect, BorderRadius);
                e.Graphics.DrawPath(borderPen, path);
            }
            
            base.OnPaint(e);
        }

        private System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y + bounds.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void BtnViewContactPerson_Click(object sender, EventArgs e)
        {
            if (_company?.ContactPerson != null)
            {
                var frmPersonCard = new RMS_UI.Forms.frmPersonCard(_company.ContactPerson.PersonID);
                frmPersonCard.ShowDialog();
            }
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }
    }
}
