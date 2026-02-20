using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsEmployee
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int EmployeeID { get; set; }
        public int PersonID {get; init;}        
        public int PositionID { get ; set; }
        public DateTime HireDate { get; set; }
        public DateTime? FireDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        private clsPerson? _person;
        public clsPerson? Person
        {
            get
            {
                if (_person == null && PersonID > 0)
                {
                    _person = clsPerson.Find(PersonID);
                }
                return _person;
            }
        }
        public clsEmployee()
        {
            EmployeeID = -1;
            PersonID = -1;
            PositionID = -1;
            HireDate = DateTime.MinValue;
            FireDate = null;
            CreatedByUserID = -1;
            CreatedDate = DateTime.MinValue;
            UpdatedByUserID = -1;
            UpdatedDate = DateTime.MinValue;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsEmployeeData.AddNewEmployee(PersonID, PositionID, HireDate, FireDate, CreatedByUserID);
                    if (newID != -1) { EmployeeID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsEmployeeData.UpdateEmployee(EmployeeID, PersonID, PositionID, HireDate, FireDate, UpdatedByUserID);
            }
            return false;
        }
        public static clsEmployee? Find(int EmployeeID)
        {
            int PersonID = -1;
            int PositionID = -1;
            DateTime HireDate = DateTime.MinValue;
            DateTime? FireDate = null;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.MinValue;
            int UpdatedByUserID = -1;
            DateTime UpdatedDate = DateTime.MinValue;
            bool found = clsEmployeeData.GetEmployeeByID(EmployeeID, ref PersonID, ref PositionID, ref HireDate, ref FireDate, ref CreatedByUserID, ref CreatedDate, ref UpdatedByUserID, ref UpdatedDate);
            if (found)
                return new clsEmployee() { EmployeeID = EmployeeID, PersonID = PersonID, PositionID = PositionID, HireDate = HireDate, FireDate = FireDate, CreatedByUserID = CreatedByUserID, CreatedDate = CreatedDate, UpdatedByUserID = UpdatedByUserID, UpdatedDate = UpdatedDate, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteEmployee(int EmployeeID, int? UpdatedByUserID = null)
        {
            return clsEmployeeData.DeleteEmployee(EmployeeID , UpdatedByUserID);
        }
        public static DataTable GetAllEmployee()
        {
            return clsEmployeeData.GetAllEmployee();
        }
    }
}
