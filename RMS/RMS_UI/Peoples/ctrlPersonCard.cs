using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Utilities;

namespace RMS_UI.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson? _person = null;
        private const int BorderRadius = 12;
        private const int ShadowSize = 8;

        public ctrlPersonCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            
            // Subscribe to theme changes
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        public clsPerson? Person => _person;

        public void LoadPerson(clsPerson? person)
        {
            _person = person;
            if (_person != null)
            {
                LoadPersonInfo();
            }
            else
            {
                Clear();
            }
        }

        private void LoadPersonInfo()
        {
            if (_person == null) return;

            // Load basic info
            _lblPersonID.Text = $"ID: {_person.PersonID}";
            _lblFullName.Text = _person.FullName;
            _lblNationalNo.Text = $"National ID: {(_person.NationalNo ?? "N/A")}";

            // Load contact info
            _lblPhone.Text = _person.Phone ?? "No phone provided";
            _lblEmail.Text = _person.Email ?? "No email provided";
            _lblAddress.Text = _person.Address ?? "No address provided";

            // Load country
            _lblCountry.Text = _person.Country?.CountryName ?? "Not specified";

            // Load date of birth
            if (_person.DateOfBirth.HasValue)
            {
                _lblDateOfBirth.Text = _person.DateOfBirth.Value.ToString("dd/MM/yyyy");
                int age = DateTime.Now.Year - _person.DateOfBirth.Value.Year;
                _lblAge.Text = $"({age} years)";
            }
            else
            {
                _lblDateOfBirth.Text = "N/A";
                _lblAge.Text = "";
            }

            // Load gender
            if (_person.Gender.HasValue)
            {
                _lblGender.Text = _person.Gender.Value == 1 ? "Male" : "Female";
            }
            else
            {
                _lblGender.Text = "Not specified";
            }

            // Load audit info
            _lblCreatedDate.Text = _person.CreatedDate != null ? _person.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "N/A";
            _lblCreatedBy.Text = _person.CreatedByUser?.UserName ?? "N/A";
            _lblUpdatedDate.Text = _person.UpdatedDate != null ? _person.UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "N/A";
            _lblUpdatedBy.Text = _person.UpdatedByUser?.UserName ?? "N/A";

            // Load image
            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_person?.ImagePath) && System.IO.File.Exists(_person.ImagePath))
                {
                    _picPersonImage.BackgroundImage = Image.FromFile(_person.ImagePath);
                }
                else
                {
                    // Create a placeholder image
                    _picPersonImage.BackgroundImage = CreatePlaceholderImage();
                }
            }
            catch
            {
                _picPersonImage.BackgroundImage = CreatePlaceholderImage();
            }
        }

        private Image CreatePlaceholderImage()
        {
            var colors = ThemeManager.Colors;
            var bitmap = new Bitmap(_picPersonImage.Width > 0 ? _picPersonImage.Width : 150, 
                                    _picPersonImage.Height > 0 ? _picPersonImage.Height : 150);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(colors.Primary);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Draw a simple person icon
                using (var brush = new SolidBrush(Color.White))
                {
                    var centerX = bitmap.Width / 2;
                    var centerY = bitmap.Height / 2;
                    
                    // Draw circle for head
                    g.FillEllipse(brush, centerX - 25, centerY - 35, 50, 50);
                    
                    // Draw rectangle for body
                    g.FillRectangle(brush, centerX - 30, centerY + 15, 60, 50);
                }
            }
            return bitmap;
        }

        private void ApplyTheme()
        {
            var colors = ThemeManager.Colors;

            BackColor = colors.ContentBackground;
            _lblPersonID.ForeColor = colors.SecondaryText;
            _lblFullName.ForeColor = colors.TitleText;
            _lblNationalNo.ForeColor = colors.SecondaryText;
            _lblPhone.ForeColor = colors.PrimaryText;
            _lblEmail.ForeColor = colors.PrimaryText;
            _lblAddress.ForeColor = colors.PrimaryText;
            _lblCountry.ForeColor = colors.PrimaryText;
            _lblDateOfBirth.ForeColor = colors.PrimaryText;
            _lblAge.ForeColor = colors.SecondaryText;
            _lblGender.ForeColor = colors.PrimaryText;
            _lblPhoneLabel.ForeColor = colors.SecondaryText;
            _lblEmailLabel.ForeColor = colors.SecondaryText;
            _lblAddressLabel.ForeColor = colors.SecondaryText;
            _lblCountryLabel.ForeColor = colors.SecondaryText;
            _lblDateLabel.ForeColor = colors.SecondaryText;
            _lblGenderLabel.ForeColor = colors.SecondaryText;
            _lblCreatedDateLabel.ForeColor = colors.SecondaryText;
            _lblCreatedDate.ForeColor = colors.PrimaryText;
            _lblCreatedByLabel.ForeColor = colors.SecondaryText;
            _lblCreatedBy.ForeColor = colors.PrimaryText;
            _lblUpdatedDateLabel.ForeColor = colors.SecondaryText;
            _lblUpdatedDate.ForeColor = colors.PrimaryText;
            _lblUpdatedByLabel.ForeColor = colors.SecondaryText;
            _lblUpdatedBy.ForeColor = colors.PrimaryText;

            Invalidate();
        }
        public void Clear()
        {
            _lblPersonID.Text = "ID: N/A";
            _lblFullName.Text = "Full Name";
            _lblNationalNo.Text = "National ID: N/A";
            _lblAddressLabel.Text = "Address:";
            _lblAddress.Text = "No address provided";
            _lblCountryLabel.Text = "Country:";
            _lblCountry.Text = "Not specified";
            _lblEmailLabel.Text = "Email:";
            _lblEmail.Text = "No email provided";
            _lblPhoneLabel.Text = "Phone:";
            _lblPhone.Text = "No phone provided";
            _lblGenderLabel.Text = "Gender:";
            _lblGender.Text = "Not specified";
            _lblDateLabel.Text = "Date of Birth:";
            _lblAge.Text = "(0 years)";
            _lblDateOfBirth.Text = "N/A";
            _lblCreatedDateLabel.Text = "Created Date:";
            _lblCreatedDate.Text = "N/A";
            _lblCreatedByLabel.Text = "Created By:";
            _lblCreatedBy.Text = "N/A";
            _lblUpdatedDateLabel.Text = "Updated Date:";
            _lblUpdatedDate.Text = "N/A";
            _lblUpdatedByLabel.Text = "Updated By:";
            _lblUpdatedBy.Text = "N/A";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var colors = ThemeManager.Colors;

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

        private GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.X + bounds.Width - diameter, bounds.Y + bounds.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }
    }
}
