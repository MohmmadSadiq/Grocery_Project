using System;
using System.Data;
using RMS_DataAccess;

namespace RMS_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }

        // Nullable Strings (Map to NVARCHAR NULL)
        public string? NationalNo { set; get; }

        // Non-Nullable Strings (Map to NVARCHAR NOT NULL)
        public string FirstName { set; get; }
        public string? SecondName { set; get; }
        public string? ThirdName { set; get; }
        public string LastName { set; get; }

        // Computed Property
        public string FullName
        {
            get
            {
                // Combines names, ignoring nulls or empty strings
                return string.Join(" ",
                    new string?[] { FirstName, SecondName, ThirdName, LastName }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
            }
        }

        // Nullable Value Types (Map to NULL columns)
        public DateTime? DateOfBirth { set; get; }
        public byte? Gender { set; get; }
        public string? Address { set; get; }
        public string? Phone { set; get; }
        public string? Email { set; get; }
        public int? NationalityCountryID { set; get; }
        public string? ImagePath { set; get; }

        // Audit Columns
        public DateTime? CreatedDate { set; get; }
        
        private int? _createdByUserID;
        private clsUser? _createdByUser;
        public int? CreatedByUserID 
        { 
            get => _createdByUserID;
            set
            {
                if(_createdByUserID != value)
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
                else if(_createdByUser == null && _createdByUserID != null && _createdByUserID > 0)
                    _createdByUser = clsUser.Find(_createdByUserID.Value);
                 
                return _createdByUser; 
            }
        }
        
        public DateTime? UpdatedDate { set; get; }

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

        private int? _updatedByUserID;
        private clsUser? _updatedByUser;
        public int? UpdatedByUserID { get => _updatedByUserID ;
        set
            {
                if(_updatedByUserID != value)
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
                else if(_updatedByUser == null && _updatedByUserID != null && _updatedByUserID > 0)
                    _updatedByUser = clsUser.Find(_updatedByUserID.Value);
                 
                return _updatedByUser; 
            }
            
        } 
        

        // Optional: Linked Country Object
        // public clsCountry? CountryInfo;

        public clsPerson()
        {
            PersonID = -1;
            NationalNo = null;
            FirstName = string.Empty; // Not Null
            SecondName = null;
            ThirdName = null;
            LastName = string.Empty;  // Not Null
            DateOfBirth = null;
            Gender = null;
            Address = null;
            Phone = null;
            Email = null;
            NationalityCountryID = null;
            ImagePath = null;
            CreatedDate = null;
            CreatedByUserID = null;
            UpdatedDate = null;
            UpdatedByUserID = null;

            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string? NationalNo, string FirstName, string? SecondName,
            string? ThirdName, string LastName, DateTime? DateOfBirth, byte? Gender,
            string? Address, string? Phone, string? Email, int? NationalityCountryID,
            string? ImagePath, DateTime? CreatedDate, int? CreatedByUserID, DateTime? UpdatedDate, int? UpdatedByUserID)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CreatedDate = CreatedDate;
            this.CreatedByUserID = CreatedByUserID;
            this.UpdatedDate = UpdatedDate;
            this.UpdatedByUserID = UpdatedByUserID;

            // if (NationalityCountryID.HasValue)
            //     this.CountryInfo = clsCountry.Find(NationalityCountryID.Value);

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {

            PersonID = clsPersonData.AddNewPerson(
                NationalNo, FirstName, SecondName, ThirdName,
                LastName, DateOfBirth, Gender, Address,
                Phone, Email, NationalityCountryID, ImagePath,
                CreatedDate, CreatedByUserID);

            return PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                PersonID, NationalNo, FirstName, SecondName,
                ThirdName, LastName, DateOfBirth, Gender,
                Address, Phone, Email, NationalityCountryID,
                ImagePath, UpdatedDate, UpdatedByUserID);
        }

        public static clsPerson? Find(int PersonID)
        {
            // Variables initialized to default (null for nullable types)
            string? NationalNo = null;
            string FirstName = ""; // Mandatory
            string? SecondName = null;
            string? ThirdName = null;
            string LastName = ""; // Mandatory
            DateTime? DateOfBirth = null;
            byte? Gender = null;
            string? Address = null;
            string? Phone = null;
            string? Email = null;
            int? NationalityCountryID = null;
            string? ImagePath = null;
            DateTime? CreatedDate = null;
            int? CreatedByUserID = null;
            DateTime? UpdatedDate = null;
            int? UpdatedByUserID = null;

            bool IsFound = clsPersonData.GetPersonInfoByID(
                PersonID, ref NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email,
                ref NationalityCountryID, ref ImagePath, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID
            );

            if (IsFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                          DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID);
            else
                return null;
        }

        public static clsPerson? FindByNationalNo(string NationalNo)
        {
            int PersonID = -1;
            string? NatNo = null, SecondName = null, ThirdName = null, Address = null,
                    Phone = null, Email = null, ImagePath = null;
            string FirstName = "", LastName = "";
            DateTime? DateOfBirth = null, CreatedDate = null, UpdatedDate = null;
            byte? Gender = null;
            int? NationalityCountryID = null, CreatedByUserID = null, UpdatedByUserID = null;

            bool IsFound = clsPersonData.GetPersonInfoByNationalNo(
                NationalNo, ref PersonID, ref NatNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address,
                ref Phone, ref Email, ref NationalityCountryID, ref ImagePath,
                ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);

            return IsFound
                ? new clsPerson(PersonID, NatNo, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID,
                    ImagePath, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID)
                : null;
        }

        public static clsPerson? FindByEmail(string Email)
        {
            int PersonID = -1;
            string? NationalNo = null, SecondName = null, ThirdName = null, Address = null,
                    Phone = null, Eml = null, ImagePath = null;
            string FirstName = "", LastName = "";
            DateTime? DateOfBirth = null, CreatedDate = null, UpdatedDate = null;
            byte? Gender = null;
            int? NationalityCountryID = null, CreatedByUserID = null, UpdatedByUserID = null;

            bool IsFound = clsPersonData.GetPersonInfoByEmail(
                Email, ref PersonID, ref NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address,
                ref Phone, ref Eml, ref NationalityCountryID, ref ImagePath,
                ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);

            return IsFound
                ? new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, Gender, Address, Phone, Eml, NationalityCountryID,
                    ImagePath, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID)
                : null;
        }

        public static clsPerson? FindByPhone(string Phone)
        {
            int PersonID = -1;
            string? NationalNo = null, SecondName = null, ThirdName = null, Address = null,
                    Phn = null, Email = null, ImagePath = null;
            string FirstName = "", LastName = "";
            DateTime? DateOfBirth = null, CreatedDate = null, UpdatedDate = null;
            byte? Gender = null;
            int? NationalityCountryID = null, CreatedByUserID = null, UpdatedByUserID = null;

            bool IsFound = clsPersonData.GetPersonInfoByPhone(
                Phone, ref PersonID, ref NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address,
                ref Phn, ref Email, ref NationalityCountryID, ref ImagePath,
                ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);

            return IsFound
                ? new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, Gender, Address, Phn, Email, NationalityCountryID,
                    ImagePath, CreatedDate, CreatedByUserID, UpdatedDate, UpdatedByUserID)
                : null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static bool DeletePerson(int ID, int? UpdatedByUserID = null)
        {
            return clsPersonData.DeletePerson(ID, UpdatedByUserID);
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
    }
}