using System;
using System.Data;
using System.Linq; // Required for FullName logic
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
        public int? CreatedByUserID { set; get; }
        public int? UpdatedByUserID { set; get; }

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
            CreatedByUserID = null;
            UpdatedByUserID = null;

            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string? NationalNo, string FirstName, string? SecondName,
            string? ThirdName, string LastName, DateTime? DateOfBirth, byte? Gender,
            string? Address, string? Phone, string? Email, int? NationalityCountryID,
            string? ImagePath, int? CreatedByUserID)
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
            this.CreatedByUserID = CreatedByUserID;

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
                CreatedByUserID);

            return PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                PersonID, NationalNo, FirstName, SecondName,
                ThirdName, LastName, DateOfBirth, Gender,
                Address, Phone, Email, NationalityCountryID,
                ImagePath, UpdatedByUserID);
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
            int? CreatedByUserID = null;

            bool IsFound = clsPersonData.GetPersonInfoByID(
                PersonID, ref NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email,
                ref NationalityCountryID, ref ImagePath, ref CreatedByUserID
            );

            if (IsFound)
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                          DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath, CreatedByUserID);
            else
                return null;
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