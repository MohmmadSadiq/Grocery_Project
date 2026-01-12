using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsCompany
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? ContactPersonID { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? CountryID { get; set; }
        public string? CommercialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        public clsCompany()
        {
            CompanyID = -1;
            CompanyName = string.Empty;
            ContactPersonID = null;
            Phone = null;
            Email = null;
            Address = null;
            CountryID = null;
            CommercialNumber = null;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            UpdatedDate = DateTime.MinValue;
            UpdatedByUserID = null;
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
    }
}
