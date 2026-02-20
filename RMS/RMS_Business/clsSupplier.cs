using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsSupplier
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enSupplier {Unknown = 0, Person = 1, Company = 2 };
        public enMode Mode = enMode.AddNew;

        private int? _personID;
        private int? _companyID;

        public int SupplierID { get; set; }
        public int? PersonID { get => _personID; 
            set
            {
                if(_companyID.HasValue && value.HasValue)
                {
                    CompanyID = null;
                }
                
                _personID = value;
            } 
        }        public int? CompanyID { get => _companyID; 
        set
            {
                if(_personID.HasValue && value.HasValue)
                {
                    PersonID = null;
                }
                
                _companyID = value;
            } 
        }
        public int? AccountID { get; set; }
        public bool IsActive { get; set; }
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
        public enSupplier SupplierType { get { return GetSupplierType(); } }    
        public enSupplier GetSupplierType()
        {
            if (PersonID.HasValue) return enSupplier.Person;
            else if (CompanyID.HasValue) return enSupplier.Company;
            else return enSupplier.Unknown;
        }

        public clsSupplier()
        {
            SupplierID = -1;
            PersonID = null;
            CompanyID = null;
            AccountID = null;
            IsActive = false;
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
                    var newID = clsSupplierData.AddNewSupplier(PersonID, CompanyID, AccountID, IsActive, CreatedByUserID);
                    if (newID != -1) { SupplierID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsSupplierData.UpdateSupplier(SupplierID, PersonID, CompanyID, AccountID, IsActive, UpdatedByUserID);
            }
            return false;
        }
        public static clsSupplier? Find(int SupplierID)
        {
            int? PersonID = null;
            int? CompanyID = null;
            int? AccountID = null;
            bool IsActive = false;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsSupplierData.GetSupplierByID(SupplierID, ref PersonID, ref CompanyID, ref AccountID, ref IsActive, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsSupplier() { SupplierID = SupplierID, PersonID = PersonID, CompanyID = CompanyID, AccountID = AccountID, IsActive = IsActive, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteSupplier(int SupplierID, int? UpdatedByUserID = null)
        {
            return clsSupplierData.DeleteSupplier(SupplierID , UpdatedByUserID);
        }
        public static DataTable GetAllSupplier()
        {
            return clsSupplierData.GetAllSupplier();
        }
        public static DataTable SearchSupplierPages(string? searchText = null, string searchBy = "SupplierName", string? supplierType = null, bool? isActive = null, int pageNumber = 1, int pageSize = 20, string sortBy = "SupplierName")
        {
            return clsSupplierData.SearchSupplierPages(searchText, searchBy, supplierType, isActive, pageNumber, pageSize, sortBy);
        }
        
        public class SupplierSearchCriteria
        {
            public string SearchText { get; set; } = "";
            public string SearchBy { get; set; } = "SupplierName"; // SupplierName, Phone, Code
            public string? SupplierType { get; set; } = null; // Person, Company, null for all
            public bool? IsActive { get; set; } // Nullable for "All" tab
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "SupplierName"; // SupplierName, Phone, Code, Country

            /// <summary>
            /// Checks if the search criteria is in its default state.
            /// </summary>
            public bool IsDefault()
            {
                return string.IsNullOrEmpty(SearchText) && string.IsNullOrEmpty(SupplierType) && !IsActive.HasValue && PageNumber == 1 && PageSize == 20;
            }
        }
        
        public class ProductSearchCriteria
        {
            public string SearchText { get; set; } = "";
            public string SearchBy { get; set; } = "Name"; // Name, ID, Category, Brand
            public int? CategoryId { get; set; }
            public bool? IsActive { get; set; } // Nullable for "All" tab
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "Name";

            public clsProductData.ProductSearchCriteria ToDataAccessCriteria()
            {
                return new clsProductData.ProductSearchCriteria
                {
                    SearchText = this.SearchText,
                    SearchBy = this.SearchBy,
                    CategoryId = this.CategoryId,
                    IsActive = this.IsActive,
                    PageNumber = this.PageNumber,
                    PageSize = this.PageSize,
                    SortBy = this.SortBy
                };
            }
            /// <summary>
            /// Checks if the search criteria is in its default state.
            /// </summary>
            public bool IsDefault()
            {
                return string.IsNullOrEmpty(SearchText) && !CategoryId.HasValue && !IsActive.HasValue && PageNumber == 1 && PageSize == 20 && string.IsNullOrEmpty(SortBy);
            }
        }
        
    }
}
