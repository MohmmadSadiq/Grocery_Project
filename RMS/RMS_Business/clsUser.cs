using System;
using RMS_DataAccess;

namespace RMS_Business
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }

        // Composition: Person Info (Lazy Loading)
        private int _personID;
        public int PersonID { 
            
            get => _personID;
            set
                {
                    if (_personID == value)
                        return;
                    
                    _personID = value;
                    _personInfo = null; // Invalidate cached person info
                }
        }
        private clsPerson? _personInfo;
        public clsPerson? PersonInfo
        {
            get
            {
                if (_personInfo == null && PersonID > 0)
                    _personInfo = clsPerson.Find(PersonID);
                return _personInfo;
            }
        }

        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = string.Empty;
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
            IsActive = true;
            CreatedDate = null;
            CreatedByUserID = null;
            UpdatedDate = null;
            UpdatedByUserID = null;
            _personInfo = null;
            Mode = enMode.AddNew;
        }

        private clsUser(int userID, int personID, string userName, string passwordHash, string passwordSalt, bool isActive, DateTime? createdDate, int? createdByUserID, DateTime? updatedDate, int? updatedByUserID)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            IsActive = isActive;
            CreatedDate = createdDate;
            CreatedByUserID = createdByUserID;
            UpdatedDate = updatedDate;
            UpdatedByUserID = updatedByUserID;
            _personInfo = null; // Lazy loading: will be loaded when accessed
            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            UserID = clsUserData.AddNewUser(PersonID, UserName, PasswordHash, PasswordSalt, IsActive, CreatedByUserID);
            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(UserID, PersonID, UserName, PasswordHash, PasswordSalt, IsActive, UpdatedByUserID);
        }

        public static clsUser? Find(int userID)
        {
            int personID = 0;
            string userName = string.Empty;
            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;
            bool isActive = true;
            DateTime? createdDate = null;
            int? createdByUserID = null;
            DateTime? updatedDate = null;
            int? updatedByUserID = null;

            bool found = clsUserData.GetUserInfoByID(userID, ref personID, ref userName, ref passwordHash, ref passwordSalt, ref isActive, ref createdDate, ref createdByUserID, ref updatedDate, ref updatedByUserID);
            if (found)
                return new clsUser(userID, personID, userName, passwordHash, passwordSalt, isActive, createdDate, createdByUserID, updatedDate, updatedByUserID);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static bool DeleteUser(int userID, int? updatedByUserID = null)
        {
            return clsUserData.DeleteUser(userID, updatedByUserID);
        }
    }
}
