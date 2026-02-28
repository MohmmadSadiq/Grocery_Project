using System;
using System.Collections.Generic;
using System.Data;
using RMS_DataAccess;
namespace RMS_Business
{
    public class clsProductUnit
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ProductUnitID { get; set; }

        // Composition: Product Info (Lazy Loading)
        private int _productID;
        public int ProductID
        {
            get => _productID;
            set
            {
                if (_productID == value)
                    return;

                _productID = value;
                _productInfo = null; // Invalidate cached product info
            }
        }
        private clsProduct? _productInfo;
        public clsProduct? ProductInfo
        {
            get
            {
                if (_productInfo == null && ProductID > 0)
                    _productInfo = clsProduct.Find(ProductID);
                return _productInfo;
            }
        }

        // Composition: Unit Info (Lazy Loading)
        private int _unitID;
        public int UnitID
        {
            get => _unitID;
            set
            {
                if (_unitID == value)
                    return;

                _unitID = value;
                _unitInfo = null; // Invalidate cached unit info
            }
        }
        private clsUnit? _unitInfo;
        public clsUnit? UnitInfo
        {
            get
            {
                if (_unitInfo == null && UnitID > 0)
                    _unitInfo = clsUnit.Find(UnitID);
                return _unitInfo;
            }
        }

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
            _productID = -1;
            _productInfo = null;
            _unitID = -1;
            _unitInfo = null;
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

        public static DataTable GetProductUnitsByProductID(int productId)
        {
            return clsProductUnitData.GetProductUnitsByProductID(productId);
        }

        /// <summary>
        /// Creates a new ProductUnit instance for adding a new product unit.
        /// </summary>
        public static clsProductUnit CreateNew()
        {
            return new clsProductUnit();
        }

        /// <summary>
        /// Finds a ProductUnit by its Barcode. Returns null if not found.
        /// </summary>
        public static clsProductUnit? FindByBarcode(string Barcode)
        {
            int ProductUnitID = -1;
            int ProductID = -1;
            int UnitID = -1;
            string? Description = null;
            decimal ConversionFactor = -1;
            decimal? SalePrice = null;
            bool IsActive = false;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;

            bool found = clsProductUnitData.GetProductUnitByBarcode(Barcode, ref ProductUnitID, ref ProductID, ref UnitID, ref Description, ref ConversionFactor, ref SalePrice, ref IsActive, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);

            if (found)
                return new clsProductUnit()
                {
                    ProductUnitID = ProductUnitID,
                    ProductID = ProductID,
                    UnitID = UnitID,
                    Description = Description,
                    ConversionFactor = ConversionFactor,
                    SalePrice = SalePrice,
                    Barcode = Barcode,
                    IsActive = IsActive,
                    CreatedDate = CreatedDate,
                    CreatedByUserID = CreatedByUserID,
                    UpdatedDate = UpdatedDate,
                    UpdatedByUserID = UpdatedByUserID,
                    Mode = enMode.Update
                };
            else return null;
        }

        /// <summary>
        /// Searches for ProductUnits whose Barcode starts with the given text.
        /// Pass null for PageNumber/PageSize to get all results without pagination.
        /// </summary>
        public static DataTable SearchByBarcode(string Barcode, int? PageNumber = null, int? PageSize = null)
        {
            return clsProductUnitData.SearchProductUnitsByBarcode(Barcode, PageNumber, PageSize);
        }

        /// <summary>
        /// Creates a clsProductUnit instance from a DataRow.
        /// Expected columns: ProductUnitID, ProductID, UnitID, Description,
        /// ConversionFactor, SalePrice, Barcode, IsActive, CreatedDate,
        /// CreatedByUserID, UpdatedDate, UpdatedByUserID.
        /// </summary>
        private static clsProductUnit FromDataRow(DataRow row)
        {
            return new clsProductUnit()
            {
                ProductUnitID = (int)row["ProductUnitID"],
                ProductID = (int)row["ProductID"],
                UnitID = (int)row["UnitID"],
                Description = row["Description"] != DBNull.Value ? (string?)row["Description"] : null,
                ConversionFactor = (decimal)row["ConversionFactor"],
                SalePrice = row["SalePrice"] != DBNull.Value ? (decimal?)row["SalePrice"] : null,
                Barcode = row["Barcode"] != DBNull.Value ? (string?)row["Barcode"] : null,
                IsActive = (bool)row["IsActive"],
                CreatedDate = (DateTime)row["CreatedDate"],
                CreatedByUserID = row["CreatedByUserID"] != DBNull.Value ? (int?)row["CreatedByUserID"] : null,
                UpdatedDate = (DateTime)row["UpdatedDate"],
                UpdatedByUserID = row["UpdatedByUserID"] != DBNull.Value ? (int?)row["UpdatedByUserID"] : null,
                Mode = enMode.Update
            };
        }

        /// <summary>
        /// Gets all ProductUnits for a given ProductID as a List of clsProductUnit objects.
        /// Uses lazy loading for ProductInfo and UnitInfo compositions.
        /// </summary>
        public static List<clsProductUnit> GetProductUnitListByProductID(int productId)
        {
            List<clsProductUnit> list = new List<clsProductUnit>();
            DataTable dt = GetProductUnitsByProductID(productId);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(FromDataRow(row));
            }

            return list;
        }
    }
}
