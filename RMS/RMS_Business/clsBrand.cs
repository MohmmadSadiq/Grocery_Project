using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsBrand
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        private int? _companyID;
        public int? CompanyID
        {
            get => _companyID;
            set
            {
                if (_companyID == value)
                    return;

                _companyID = value;

                if (!_companyID.HasValue)
                {
                    _companyInfo = null;
                }
                else if (_companyInfo != null && _companyInfo.CompanyID != _companyID.Value)
                {
                    _companyInfo = null;
                }
            }
        }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }

        private clsCompany? _companyInfo;
        public clsCompany? CompanyInfo
        {
            get
            {
                // Lazy-load the company to avoid extra database calls until needed.
                if (_companyInfo == null && CompanyID.HasValue)
                {
                    _companyInfo = clsCompany.Find(CompanyID.Value);
                }
                return _companyInfo;
            }
            private set
            {
                _companyInfo = value;
                _companyID = value?.CompanyID;
            }
        }
        
        private clsBrand()
        {
            BrandID = -1;
            BrandName = string.Empty;
            _companyID = null;
            Description = null;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            Mode = enMode.AddNew;
            _companyInfo = null;
        }

        public void AssignCompany(clsCompany? company)
        {
            CompanyInfo = company;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsBrandData.AddNewBrand(BrandName, CompanyID, Description, CreatedByUserID);
                    if (newID != -1) { BrandID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsBrandData.UpdateBrand(BrandID, BrandName, CompanyID, Description);
            }
            return false;
        }
        public static clsBrand? Find(int BrandID)
        {
            string BrandName = string.Empty;
            int? CompanyID = null;
            string? Description = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            bool found = clsBrandData.GetBrandByID(BrandID, ref BrandName, ref CompanyID, ref Description, ref CreatedDate, ref CreatedByUserID);
            if (found)
                return new clsBrand() { BrandID = BrandID, BrandName = BrandName, CompanyID = CompanyID, Description = Description, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteBrand(int BrandID)
        {
            return clsBrandData.DeleteBrand(BrandID );
        }
        public static DataTable GetAllBrand()
        {
            return clsBrandData.GetAllBrand();
        }
    }
}
