using System;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsProductUnit
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ProductUnitID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public string? Description { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Barcode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdatedByUserID { get; set; }
        private clsProductUnit()
        {
            ProductUnitID = -1;
            ProductID = -1;
            UnitID = -1;
            Description = null;
            ConversionFactor = -1;
            SalePrice = null;
            Barcode = null;
            IsActive = false;
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
                    var newID = clsProductUnitData.AddNewProductUnit(ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, CreatedByUserID);
                    if (newID != -1) { ProductUnitID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsProductUnitData.UpdateProductUnit(ProductUnitID, ProductID, UnitID, Description, ConversionFactor, SalePrice, Barcode, IsActive, UpdatedByUserID);
            }
            return false;
        }
        public static clsProductUnit? Find(int ProductUnitID)
        {
            int ProductID = -1;
            int UnitID = -1;
            string? Description = null;
            decimal ConversionFactor = -1;
            decimal? SalePrice = null;
            string? Barcode = null;
            bool IsActive = false;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsProductUnitData.GetProductUnitByID(ProductUnitID, ref ProductID, ref UnitID, ref Description, ref ConversionFactor, ref SalePrice, ref Barcode, ref IsActive, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsProductUnit() { ProductUnitID = ProductUnitID, ProductID = ProductID, UnitID = UnitID, Description = Description, ConversionFactor = ConversionFactor, SalePrice = SalePrice, Barcode = Barcode, IsActive = IsActive, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
            else return null;
        }
        public static bool DeleteProductUnit(int ProductUnitID, int? UpdatedByUserID = null)
        {
            return clsProductUnitData.DeleteProductUnit(ProductUnitID , UpdatedByUserID);
        }
        public static DataTable GetAllProductUnit()
        {
            return clsProductUnitData.GetAllProductUnit();
        }
    }
}
