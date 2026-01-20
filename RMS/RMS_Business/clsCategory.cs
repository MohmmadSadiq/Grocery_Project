using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsCategory
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        private clsCategory()
        {
            CategoryID = -1;
            CategoryName = string.Empty;
            Description = null;
            CreatedDate = DateTime.MinValue;
            CreatedByUserID = null;
            Mode = enMode.AddNew;
        }
        public static clsCategory CreateNew()
        {
            return new clsCategory();
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    var newID = clsCategoryData.AddNewCategory(CategoryName, Description, CreatedByUserID);
                    if (newID != -1) { CategoryID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsCategoryData.UpdateCategory(CategoryID, CategoryName, Description);
            }
            return false;
        }
        public static clsCategory? Find(int CategoryID)
        {
            string CategoryName = string.Empty;
            string? Description = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            bool found = clsCategoryData.GetCategoryByID(CategoryID, ref CategoryName, ref Description, ref CreatedDate, ref CreatedByUserID);
            if (found)
                return new clsCategory() { CategoryID = CategoryID, CategoryName = CategoryName, Description = Description, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteCategory(int CategoryID)
        {
            return clsCategoryData.DeleteCategory(CategoryID );
        }
        public static DataTable GetAllCategory()
        {
            return clsCategoryData.GetAllCategory();
        }
    }
}
