using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS_Business;
using RMS_UI.Controls;
using RMS_UI.Utilities;

namespace RMS_UI.Suppliers_Purchase
{
    public partial class ctrlBusinessPartners : UserControl
    {
        

        public enum enBusinessPartners
        {
            Person = 1,
            Company = 2
        }
        private enBusinessPartners? _partner = null;
        private bool _togglingWithPartnerChangin = false;

        /// <summary>
        /// Gets or sets the business partner type (Person or Company).
        /// </summary>
        /// <remarks>
        /// When set, this property enables/disables the corresponding tab based on the partner type:
        /// <list type="bullet">
        /// <item><description>If null: Disables all tabs</description></item>
        /// <item><description>If Person: Enables Person tab, disables Company tab, and sets focus to Person tab</description></item>
        /// <item><description>If Company: Enables Company tab, disables Person tab, and sets focus to Company tab</description></item>
        /// </list>
        /// </remarks>
        /// <value>
        /// An <see cref="enBusinessPartners"/> enum value indicating the selected partner type, or null if not set.
        /// </value>


        

        [Browsable(true)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public enBusinessPartners? Partner
        {
            get
            {
                return _partner;
            }
            set
            {
                _partner = value;
                if (!_partner.HasValue)
                    tabControl1.Enabled = false;
                else if (_partner == enBusinessPartners.Person)
                {
                    tabControl1.Enabled = true;
                    tabPerson.Enabled = true;
                    tabPerson.Focus();
                    tabCompany.Enabled = false;
                    tabControl1.SelectedTab = tabPerson;
                }
                else if (_partner == enBusinessPartners.Company)
                {
                    tabControl1.Enabled = true;
                    tabCompany.Enabled = true;
                    tabCompany.Focus();
                    tabPerson.Enabled = false;
                    tabControl1.SelectedTab = tabCompany;

                }
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool TogglingWithPartnerChanging
        {
            get => _togglingWithPartnerChangin;

            set
            {
                AllowTabToggling = value;
                _togglingWithPartnerChangin = value;
            }

        }

        public clsPerson? Person
        {
            get
            {
                if (Partner == enBusinessPartners.Person)
                    return ctrlPersonCardWithConfig1.Person;
                else
                    return null;
            }
        }
        public clsCompany? Company 
        {
            get
            {
                if (Partner == enBusinessPartners.Company)
                    return ctrlCompanyCardWithConfig1.Company;
                else
                    return null;
            }
        }

        public void LoadPerson(int? personID)
        {
            Partner = enBusinessPartners.Person;

            if (!personID.HasValue || personID.Value <= 0)
            {
                ctrlPersonCardWithConfig1.LoadPerson(null);
                return;
            }

            ctrlPersonCardWithConfig1.LoadPerson(clsPerson.Find(personID.Value));
        }

        public void LoadCompany(int? companyID)
        {
            Partner = enBusinessPartners.Company;

            if (!companyID.HasValue || companyID.Value <= 0)
            {
                ctrlCompanyCardWithConfig1.LoadCompany(-1);
                return;
            }

            ctrlCompanyCardWithConfig1.LoadCompany(companyID.Value);
        }


        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool AllowTabToggling { get; set; } = true;

        public ctrlBusinessPartners()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += (s, e) => ApplyTheme();
            ApplyTheme();
        }

        // ── Theme ─────────────────────────────────────────────────────────────────

        public void ApplyTheme()
        {
            if (InvokeRequired) { Invoke(new Action(ApplyTheme)); return; }

            var c = ThemeManager.Colors;

            // Control background
            BackColor = c.FormBackground;

            // TabControl
            tabControl1.BackColor = c.ContentBackground;

            // Tab pages
            tabPerson.BackColor = c.ContentBackground;
            tabPerson.ForeColor = c.PrimaryText;
            tabCompany.BackColor = c.ContentBackground;
            tabCompany.ForeColor = c.PrimaryText;

            // Nested card controls
            ctrlPersonCardWithConfig1.BackColor = c.ContentBackground;
            ctrlCompanyCardWithConfig1.BackColor = c.ContentBackground;

            Invalidate();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // 1) منع التبديل عند عدم السماح
            if (!AllowTabToggling)
            {
                if (Partner == enBusinessPartners.Company && e.TabPage != tabCompany)
                {
                    e.Cancel = true;
                    return;
                }

                if (Partner == enBusinessPartners.Person && e.TabPage != tabPerson)
                {
                    e.Cancel = true;
                    return;
                }

                if (Partner == null)
                {
                    e.Cancel = true;
                    tabControl1.Enabled = false;
                    return;
                }
            }

            // 2) مزامنة Partner مع التبويب الجديد فقط إذا التبديل مسموح
            if (TogglingWithPartnerChanging)
            {
                if (e.TabPage == tabCompany)
                    Partner = enBusinessPartners.Company;
                else if (e.TabPage == tabPerson)
                    Partner = enBusinessPartners.Person;
                else
                    Partner = null;
            }
        }
    }
}
