using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsProduct
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? CategoryID { get; set; }
        public int? BrandID { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        private clsProduct()
        {
            ProductID = -1;
            ProductName = string.Empty;
            CategoryID = null;
            BrandID = null;
            Description = null;
            IsActive = false;
            ReorderLevel = -1;
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
                    var newID = clsProductData.AddNewProduct(ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, CreatedByUserID);
                    if (newID != -1) { ProductID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsProductData.UpdateProduct(ProductID, ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, UpdatedByUserID);
            }
            return false;
        }
        public static clsProduct? Find(int ProductID)
        {
            string ProductName = string.Empty;
            int? CategoryID = null;
            int? BrandID = null;
            string? Description = null;
            bool IsActive = false;
            int ReorderLevel = -1;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsProductData.GetProductByID(ProductID, ref ProductName, ref CategoryID, ref BrandID, ref Description, ref IsActive, ref ReorderLevel, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsProduct() { ProductID = ProductID, ProductName = ProductName, CategoryID = CategoryID, BrandID = BrandID, Description = Description, IsActive = IsActive, ReorderLevel = ReorderLevel, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteProduct(int ProductID, int? UpdatedByUserID = null)
        {
            return clsProductData.DeleteProduct(ProductID , UpdatedByUserID);
        }
        public static DataTable GetAllProduct()
        {
            return clsProductData.GetAllProduct();
        }
    }
}
