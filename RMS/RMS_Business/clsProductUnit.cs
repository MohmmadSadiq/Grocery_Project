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

        public void SetProductInfo(clsProduct? product)
        {
            _productInfo = product;

            if (product != null)
                _productID = product.ProductID;
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

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName);
        }

        private static clsProduct? ProductFromJoinedDataRow(DataRow row)
        {
            if (!HasColumn(row, "Product_ProductID") || row["Product_ProductID"] == DBNull.Value)
                return null;

            int productID = (int)row["Product_ProductID"];
            string productName = row["Product_ProductName"] != DBNull.Value ? (string)row["Product_ProductName"] : string.Empty;
            int? categoryID = row["Product_CategoryID"] != DBNull.Value ? (int?)row["Product_CategoryID"] : null;
            int? brandID = row["Product_BrandID"] != DBNull.Value ? (int?)row["Product_BrandID"] : null;
            string? description = row["Product_Description"] != DBNull.Value ? (string?)row["Product_Description"] : null;
            bool isActive = row["Product_IsActive"] != DBNull.Value && (bool)row["Product_IsActive"];
            int reorderLevel = row["Product_ReorderLevel"] != DBNull.Value ? (int)row["Product_ReorderLevel"] : 0;
            string? imagePath = row["Product_ImagePath"] != DBNull.Value ? (string?)row["Product_ImagePath"] : null;
            DateTime createdDate = row["Product_CreatedDate"] != DBNull.Value ? (DateTime)row["Product_CreatedDate"] : DateTime.MinValue;
            int? createdByUserID = row["Product_CreatedByUserID"] != DBNull.Value ? (int?)row["Product_CreatedByUserID"] : null;
            DateTime updatedDate = row["Product_UpdatedDate"] != DBNull.Value ? (DateTime)row["Product_UpdatedDate"] : DateTime.MinValue;
            int? updatedByUserID = row["Product_UpdatedByUserID"] != DBNull.Value ? (int?)row["Product_UpdatedByUserID"] : null;

            return clsProduct.CreateHydrated(
                productID,
                productName,
                categoryID,
                brandID,
                description,
                isActive,
                reorderLevel,
                imagePath,
                createdDate,
                createdByUserID,
                updatedDate,
                updatedByUserID);
        }

        private static clsProductUnit FromJoinedDataRow(DataRow row)
        {
            clsProductUnit productUnit = FromDataRow(row);
            clsProduct? product = ProductFromJoinedDataRow(row);
            productUnit.SetProductInfo(product);
            return productUnit;
        }

        /// <summary>
        /// Gets all active ProductUnits that have a SalePrice set, as a typed list.
        /// Useful for POS grids where only sellable units should appear.
        /// </summary>
        public static List<clsProductUnit> GetAllActiveProductUnitList()
        {
            List<clsProductUnit> list = new List<clsProductUnit>();
            DataTable dt = GetAllProductUnit();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var pu = FromDataRow(row);
                    if (pu.IsActive && pu.SalePrice.HasValue)
                        list.Add(pu);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets all ProductUnits for a given ProductID as a List of clsProductUnit objects.
        /// Uses lazy loading for ProductInfo and UnitInfo compositions.
        /// </summary>
        public static List<clsProductUnit> GetProductUnitListByProductID(int productId)
        {
            List<clsProductUnit> list = new List<clsProductUnit>();
            DataTable dt = GetProductUnitsByProductID(productId);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(FromDataRow(row));
                }
            }

            return list;
        }

        public static DataTable SearchActiveWithProductPagedAsTable(string? searchText, int pageNumber, int pageSize)
        {
            return clsProductUnitData.SearchActiveProductUnitsWithProductPaged(searchText, pageNumber, pageSize);
        }

        public static List<clsProductUnit> SearchActiveWithProductPaged(string? searchText, int pageNumber, int pageSize, out int totalCount)
        {
            List<clsProductUnit> list = new List<clsProductUnit>();
            totalCount = 0;

            DataTable dt = SearchActiveWithProductPagedAsTable(searchText, pageNumber, pageSize);

            if (dt != null)
            {
                if (dt.Rows.Count > 0 && HasColumn(dt.Rows[0], "TotalCount") && dt.Rows[0]["TotalCount"] != DBNull.Value)
                    totalCount = (int)dt.Rows[0]["TotalCount"];

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(FromJoinedDataRow(row));
                }
            }

            return list;
        }
    }
}
