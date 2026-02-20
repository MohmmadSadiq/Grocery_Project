using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsPosition
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        public clsPosition()
        {
            PositionID = -1;
            PositionName = string.Empty;
            Description = null;
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
                    var newID = clsPositionData.AddNewPosition(PositionName, Description, CreatedByUserID);
                    if (newID != -1) { PositionID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsPositionData.UpdatePosition(PositionID, PositionName, Description, UpdatedByUserID);
            }
            return false;
        }
        public static clsPosition? Find(int PositionID)
        {
            string PositionName = string.Empty;
            string? Description = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsPositionData.GetPositionByID(PositionID, ref PositionName, ref Description, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsPosition() { PositionID = PositionID, PositionName = PositionName, Description = Description, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeletePosition(int PositionID, int? UpdatedByUserID = null)
        {
            return clsPositionData.DeletePosition(PositionID , UpdatedByUserID);
        }
        public static DataTable GetAllPosition()
        {
            return clsPositionData.GetAllPosition();
        }
    }
}
