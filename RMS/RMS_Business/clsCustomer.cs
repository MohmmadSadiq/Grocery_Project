using System;
using System.Data;
using RMS_DataAccess;

namespace RMS_Business
{
    public class clsCustomer
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enum enCustomer { Unknown = 0, Person = 1, Company = 2 }

        public enMode Mode = enMode.AddNew;

        private int? _personID;
        private int? _companyID;

        public int CustomerID { get; set; }
        public int? PersonID
        {
            get => _personID;
            set
            {
                if (_companyID.HasValue && value.HasValue)
                {
                    CompanyID = null;
                }

                _personID = value;
            }
        }

        public int? CompanyID
        {
            get => _companyID;
            set
            {
                if (_personID.HasValue && value.HasValue)
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
                else if (_createdByUser == null && _createdByUserID > 0)
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
                else if (_updatedByUser == null && _updatedByUserID > 0)
                    _updatedByUser = clsUser.Find(_updatedByUserID.Value);

                return _updatedByUser;
            }
        }

        public enCustomer CustomerType => GetCustomerType();

        public enCustomer GetCustomerType()
        {
            if (PersonID.HasValue) return enCustomer.Person;
            if (CompanyID.HasValue) return enCustomer.Company;
            return enCustomer.Unknown;
        }

        public clsCustomer()
        {
            CustomerID = -1;
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
                    {
                        int newID = clsCustomerData.AddNewCustomer(PersonID, CompanyID, AccountID, IsActive, CreatedByUserID);
                        if (newID != -1)
                        {
                            CustomerID = newID;
                            Mode = enMode.Update;
                            return true;
                        }
                        return false;
                    }
                case enMode.Update:
                    return clsCustomerData.UpdateCustomer(CustomerID, PersonID, CompanyID, AccountID, IsActive, UpdatedByUserID);
            }

            return false;
        }

        public static clsCustomer? Find(int CustomerID)
        {
            int? personID = null;
            int? companyID = null;
            int? accountID = null;
            bool isActive = false;
            DateTime createdDate = DateTime.MinValue;
            int? createdByUserID = null;
            DateTime updatedDate = DateTime.MinValue;
            int? updatedByUserID = null;

            bool found = clsCustomerData.GetCustomerByID(CustomerID, ref personID, ref companyID, ref accountID, ref isActive, ref createdDate, ref createdByUserID, ref updatedDate, ref updatedByUserID);

            if (found)
            {
                return new clsCustomer
                {
                    CustomerID = CustomerID,
                    PersonID = personID,
                    CompanyID = companyID,
                    AccountID = accountID,
                    IsActive = isActive,
                    CreatedDate = createdDate,
                    CreatedByUserID = createdByUserID,
                    UpdatedDate = updatedDate,
                    UpdatedByUserID = updatedByUserID,
                    Mode = enMode.Update
                };
            }

            return null;
        }

        public static bool DeleteCustomer(int CustomerID, int? UpdatedByUserID = null)
        {
            return clsCustomerData.DeleteCustomer(CustomerID, UpdatedByUserID);
        }

        public static DataTable GetAllCustomer()
        {
            return clsCustomerData.GetAllCustomer();
        }

        public static DataTable SearchCustomerPages(CustomerSearchCriteria criteria)
        {
            return clsCustomerData.SearchCustomerPages(criteria.ToDataAccessCriteria());
        }

        public class CustomerSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "CustomerName";
            public string? CustomerType { get; set; }
            public bool? IsActive { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "CustomerName";

            public clsCustomerData.CustomerSearchCriteria ToDataAccessCriteria()
            {
                return new clsCustomerData.CustomerSearchCriteria
                {
                    SearchText = this.SearchText,
                    SearchBy = this.SearchBy,
                    CustomerType = this.CustomerType,
                    IsActive = this.IsActive,
                    PageNumber = this.PageNumber,
                    PageSize = this.PageSize,
                    SortBy = this.SortBy
                };
            }

            public bool IsDefault()
            {
                return string.IsNullOrEmpty(SearchText)
                    && string.IsNullOrEmpty(CustomerType)
                    && !IsActive.HasValue
                    && PageNumber == 1
                    && PageSize == 20;
            }
        }
    }
}
