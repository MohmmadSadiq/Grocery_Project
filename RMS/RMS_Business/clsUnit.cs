using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsUnit
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        private clsUnit()
        {
            UnitID = -1;
            UnitName = string.Empty;
            Description = null;
            IsActive = false;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            Mode = enMode.AddNew;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsUnitData.AddNewUnit(UnitName, Description, IsActive, CreatedByUserID);
                    if (newID != -1) { UnitID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsUnitData.UpdateUnit(UnitID, UnitName, Description, IsActive);
            }
            return false;
        }
        public static clsUnit? Find(int UnitID)
        {
            string UnitName = string.Empty;
            string? Description = null;
            bool IsActive = false;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            bool found = clsUnitData.GetUnitByID(UnitID, ref UnitName, ref Description, ref IsActive, ref CreatedDate, ref CreatedByUserID);
            if (found)
                return new clsUnit() { UnitID = UnitID, UnitName = UnitName, Description = Description, IsActive = IsActive, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteUnit(int UnitID)
        {
            return clsUnitData.DeleteUnit(UnitID );
        }
        public static DataTable GetAllUnit()
        {
            return clsUnitData.GetAllUnit();
        }
    }
}
