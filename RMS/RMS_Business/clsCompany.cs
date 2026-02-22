using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsCompany
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        
        public int CompanyID { get ; set; }
        public string CompanyName { get; set; }
        
        private int? _contactPersonID;
        private clsPerson? _contactPerson;
        public int? ContactPersonID
        {
            get => _contactPersonID;
            set
            {
                if (_contactPersonID != value)
                    _contactPerson = null;
                _contactPersonID = value;
            }
        }
        public clsPerson? ContactPerson
        {
            get
            {
                if (_contactPersonID == null)
                    _contactPerson = null;
                else if (_contactPerson == null && _contactPersonID != null && _contactPersonID > 0)
                    _contactPerson = clsPerson.Find(_contactPersonID.Value);
                
                return _contactPerson;
            }
        }
        
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        
        public string? CommercialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        
        private int? _createdByUserID;
        private clsUser? _createdByUser;
        public int? CreatedByUserID
        {
            get => _createdByUserID;
            set
            {
                if (_createdByUserID != value)
                    _createdByUser = null;
                _createdByUserID = value;
            }
        }
        public clsUser? CreatedByUser
        {
            get
            {
                if (_createdByUserID == null)
                    _createdByUser = null;
                else if (_createdByUser == null && _createdByUserID != null && _createdByUserID > 0)
                    _createdByUser = clsUser.Find(_createdByUserID.Value);
                
                return _createdByUser;
            }
        }
        
        public DateTime UpdatedDate { get; set; }
        
        private int? _updatedByUserID;
        private clsUser? _updatedByUser;
        public int? UpdatedByUserID
        {
            get => _updatedByUserID;
            set
            {
                if (_updatedByUserID != value)
                    _updatedByUser = null;
                _updatedByUserID = value;
            }
        }
        public clsUser? UpdatedByUser
        {
            get
            {
                if (_updatedByUserID == null)
                    _updatedByUser = null;
                else if (_updatedByUser == null && _updatedByUserID != null && _updatedByUserID > 0)
                    _updatedByUser = clsUser.Find(_updatedByUserID.Value);
                
                return _updatedByUser;
            }
        }
        
        private int? _countryID;
        private clsCountry? _country;
        public int? CountryID { get => _countryID ;
        set
            {
                if(_countryID != value)
                    _country = null;
                _countryID = value;
            }
         }
        public clsCountry? Country
        {
            get
            {
                if (_countryID == null)
                   _country = null;
                else if(_country == null && _countryID != null && _countryID > 0)
                    _country = clsCountry.Find(_countryID.Value);
                 
                return _country; 
            }
            
        } 
        public clsCompany()
        {
            CompanyID = -1;
            CompanyName = string.Empty;
            _contactPersonID = null;
            Phone = null;
            Email = null;
            Address = null;
            CountryID = null;
            CommercialNumber = null;
            CreatedDate = DateTime.MinValue;
            _createdByUserID = null;
            UpdatedDate = DateTime.MinValue;
            _updatedByUserID = null;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsCompanyData.AddNewCompany(CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, CreatedByUserID);
                    if (newID != -1) { CompanyID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsCompanyData.UpdateCompany(CompanyID, CompanyName, ContactPersonID, Phone, Email, Address, CountryID, CommercialNumber, UpdatedByUserID);
            }
            return false;
        }
        public static clsCompany? Find(int CompanyID)
        {
            string CompanyName = string.Empty;
            int? ContactPersonID = null;
            string? Phone = null;
            string? Email = null;
            string? Address = null;
            int? CountryID = null;
            string? CommercialNumber = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsCompanyData.GetCompanyByID(CompanyID, ref CompanyName, ref ContactPersonID, ref Phone, ref Email, ref Address, ref CountryID, ref CommercialNumber, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsCompany() { CompanyID = CompanyID, CompanyName = CompanyName, ContactPersonID = ContactPersonID, Phone = Phone, Email = Email, Address = Address, CountryID = CountryID, CommercialNumber = CommercialNumber, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteCompany(int CompanyID, int? UpdatedByUserID = null)
        {
            return clsCompanyData.DeleteCompany(CompanyID , UpdatedByUserID);
        }
        public static DataTable GetAllCompany()
        {
            return clsCompanyData.GetAllCompany();
        }

        public static clsCompany? FindByCompanyName(string CompanyName)
        {
            int  CompanyID        = -1;
            int? ContactPersonID  = null;
            string? Phone         = null;
            string? Email         = null;
            string? Address       = null;
            int? CountryID        = null;
            string? CommercialNumber = null;
            DateTime CreatedDate  = DateTime.MinValue;
            int? CreatedByUserID  = null;
            DateTime UpdatedDate  = DateTime.MinValue;
            int? UpdatedByUserID  = null;
            bool found = clsCompanyData.GetCompanyByCompanyName(CompanyName, ref CompanyID, ref ContactPersonID, ref Phone, ref Email, ref Address, ref CountryID, ref CommercialNumber, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsCompany() { CompanyID = CompanyID, CompanyName = CompanyName, ContactPersonID = ContactPersonID, Phone = Phone, Email = Email, Address = Address, CountryID = CountryID, CommercialNumber = CommercialNumber, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }

        public static clsCompany? FindByCommercialNumber(string CommercialNumber)
        {
            int  CompanyID        = -1;
            string CompanyName    = string.Empty;
            int? ContactPersonID  = null;
            string? Phone         = null;
            string? Email         = null;
            string? Address       = null;
            int? CountryID        = null;
            DateTime CreatedDate  = DateTime.MinValue;
            int? CreatedByUserID  = null;
            DateTime UpdatedDate  = DateTime.MinValue;
            int? UpdatedByUserID  = null;
            bool found = clsCompanyData.GetCompanyByCommercialNumber(CommercialNumber, ref CompanyID, ref CompanyName, ref ContactPersonID, ref Phone, ref Email, ref Address, ref CountryID, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsCompany() { CompanyID = CompanyID, CompanyName = CompanyName, ContactPersonID = ContactPersonID, Phone = Phone, Email = Email, Address = Address, CountryID = CountryID, CommercialNumber = CommercialNumber, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }

        public static clsCompany? FindByPhone(string Phone)
        {
            int  CompanyID        = -1;
            string CompanyName    = string.Empty;
            int? ContactPersonID  = null;
            string? Email         = null;
            string? Address       = null;
            int? CountryID        = null;
            string? CommercialNumber = null;
            DateTime CreatedDate  = DateTime.MinValue;
            int? CreatedByUserID  = null;
            DateTime UpdatedDate  = DateTime.MinValue;
            int? UpdatedByUserID  = null;
            bool found = clsCompanyData.GetCompanyByPhone(Phone, ref CompanyID, ref CompanyName, ref ContactPersonID, ref Email, ref Address, ref CountryID, ref CommercialNumber, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsCompany() { CompanyID = CompanyID, CompanyName = CompanyName, ContactPersonID = ContactPersonID, Phone = Phone, Email = Email, Address = Address, CountryID = CountryID, CommercialNumber = CommercialNumber, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }

        public static clsCompany? FindByEmail(string Email)
        {
            int  CompanyID        = -1;
            string CompanyName    = string.Empty;
            int? ContactPersonID  = null;
            string? Phone         = null;
            string? Address       = null;
            int? CountryID        = null;
            string? CommercialNumber = null;
            DateTime CreatedDate  = DateTime.MinValue;
            int? CreatedByUserID  = null;
            DateTime UpdatedDate  = DateTime.MinValue;
            int? UpdatedByUserID  = null;
            bool found = clsCompanyData.GetCompanyByEmail(Email, ref CompanyID, ref CompanyName, ref ContactPersonID, ref Phone, ref Address, ref CountryID, ref CommercialNumber, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsCompany() { CompanyID = CompanyID, CompanyName = CompanyName, ContactPersonID = ContactPersonID, Phone = Phone, Email = Email, Address = Address, CountryID = CountryID, CommercialNumber = CommercialNumber, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
    }
}
