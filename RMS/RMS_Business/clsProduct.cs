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
        public string? ImagePath { get; set; }
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
            ImagePath = null;
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
                    var newID = clsProductData.AddNewProduct(ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, ImagePath, CreatedByUserID);
                    if (newID != -1) { ProductID = newID; Mode = enMode.Update; return true; }
                    else return false;
                case enMode.Update:
                    return clsProductData.UpdateProduct(ProductID, ProductName, CategoryID, BrandID, Description, IsActive, ReorderLevel, ImagePath, UpdatedByUserID);
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
            string? ImagePath = null;
            DateTime CreatedDate = DateTime.MinValue;
            int? CreatedByUserID = null;
            DateTime UpdatedDate = DateTime.MinValue;
            int? UpdatedByUserID = null;
            bool found = clsProductData.GetProductByID(ProductID, ref ProductName, ref CategoryID, ref BrandID, ref Description, ref IsActive, ref ReorderLevel, ref ImagePath, ref CreatedDate, ref CreatedByUserID, ref UpdatedDate, ref UpdatedByUserID);
            if (found)
                return new clsProduct() { ProductID = ProductID, ProductName = ProductName, CategoryID = CategoryID, BrandID = BrandID, Description = Description, IsActive = IsActive, ReorderLevel = ReorderLevel, ImagePath = ImagePath, CreatedDate = CreatedDate, CreatedByUserID = CreatedByUserID, UpdatedDate = UpdatedDate, UpdatedByUserID = UpdatedByUserID, Mode = enMode.Update };
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

        /// <summary>
        /// Gets products with pagination from ProductView.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="rowsPerPage">Number of rows per page</param>
        /// <returns>DataTable with ProductID, ProductName, Description, IsActive, ReorderLevel, ImagePath, CategoryName, BrandName, CompanyName</returns>
        public static DataTable GetProductsPaged(int pageNumber, int rowsPerPage)
        {
            return clsProductData.GetProductsPaged(pageNumber, rowsPerPage);
        }

        public static DataTable SearchProductsPages(ProductSearchCriteria searchCriteria)
        {
            return clsProductData.SearchProductsPages(searchCriteria.ToDataAccessCriteria());
        }


        /// <summary>
        /// Creates a new Product instance for adding a new product.
        /// </summary>
        public static clsProduct CreateNew()
        {
            return new clsProduct();
        }

        #region Search Criteria Class

        /// <summary>
        ///    Defines the criteria for searching and Filtering products.
        /// </summary>
        public class ProductSearchCriteria
        {
            public string SearchText { get; set; } = "";
            public string SearchBy { get; set; } = "Name"; // Name, ID, Category, Brand
            public int? CategoryId { get; set; }
            public bool? IsActive { get; set; } // Nullable for "All" tab
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "Name";

            public clsProductData.ProductSearchCriteria ToDataAccessCriteria()
            {
                return new clsProductData.ProductSearchCriteria
                {
                    SearchText = this.SearchText,
                    SearchBy = this.SearchBy,
                    CategoryId = this.CategoryId,
                    IsActive = this.IsActive,
                    PageNumber = this.PageNumber,
                    PageSize = this.PageSize,
                    SortBy = this.SortBy
                };
            }
            /// <summary>
            /// Checks if the search criteria is in its default state.
            /// </summary>
            public bool IsDefault()
            {
                return string.IsNullOrEmpty(SearchText) && !CategoryId.HasValue && !IsActive.HasValue && PageNumber == 1 && PageSize == 20 && string.IsNullOrEmpty(SortBy);
            }
        }
        #endregion    
        
    }
}
