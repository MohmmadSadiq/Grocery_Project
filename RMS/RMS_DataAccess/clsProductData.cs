using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsProductData
    {
        public static bool GetProductByID(int ProductID, ref string ProductName, ref int? CategoryID, ref int? BrandID, ref string? Description, ref bool IsActive, ref int ReorderLevel, ref string? ImagePath, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProduct_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                ProductName = (string)reader["ProductName"];
                                CategoryID = reader["CategoryID"] != DBNull.Value ? (int?)reader["CategoryID"] : null;
                                BrandID = reader["BrandID"] != DBNull.Value ? (int?)reader["BrandID"] : null;
                                Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null;
                                IsActive = (bool)reader["IsActive"];
                                ReorderLevel = (int)reader["ReorderLevel"];
                                ImagePath = reader["ImagePath"] != DBNull.Value ? (string?)reader["ImagePath"] : null;
                                CreatedDate = (DateTime)reader["CreatedDate"];
                                CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                UpdatedDate = (DateTime)reader["UpdatedDate"];
                                UpdatedByUserID = reader["UpdatedByUserID"] != DBNull.Value ? (int?)reader["UpdatedByUserID"] : null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
        public static int AddNewProduct(string ProductName, int? CategoryID, int? BrandID, string? Description, bool IsActive, int ReorderLevel, string? ImagePath, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProduct_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductName", System.Data.SqlDbType.VarChar).Value = (object?)ProductName ?? DBNull.Value;
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = (object?)CategoryID ?? DBNull.Value;
                    command.Parameters.Add("@BrandID", System.Data.SqlDbType.Int).Value = (object?)BrandID ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = (object?)IsActive ?? DBNull.Value;
                    command.Parameters.Add("@ReorderLevel", System.Data.SqlDbType.Int).Value = (object?)ReorderLevel ?? DBNull.Value;
                    command.Parameters.Add("@ImagePath", System.Data.SqlDbType.NVarChar, 500).Value = (object?)ImagePath ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewProductID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(outputIdParam);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (outputIdParam.Value != DBNull.Value)
                            newID = (int)outputIdParam.Value;
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return newID;
        }
        public static bool UpdateProduct(int ProductID, string ProductName, int? CategoryID, int? BrandID, string? Description, bool IsActive, int ReorderLevel, string? ImagePath, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProduct_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    command.Parameters.Add("@ProductName", System.Data.SqlDbType.VarChar).Value = (object?)ProductName ?? DBNull.Value;
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = (object?)CategoryID ?? DBNull.Value;
                    command.Parameters.Add("@BrandID", System.Data.SqlDbType.Int).Value = (object?)BrandID ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = (object?)IsActive ?? DBNull.Value;
                    command.Parameters.Add("@ReorderLevel", System.Data.SqlDbType.Int).Value = (object?)ReorderLevel ?? DBNull.Value;
                    command.Parameters.Add("@ImagePath", System.Data.SqlDbType.NVarChar, 500).Value = (object?)ImagePath ?? DBNull.Value;
                    command.Parameters.Add("@UpdatedByUserID", System.Data.SqlDbType.Int).Value = (object?)UpdatedByUserID ?? DBNull.Value;
                    SqlParameter returnParameter = new SqlParameter() { Direction = ParameterDirection.ReturnValue };
                    command.Parameters.Add(returnParameter);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        result = (int)returnParameter.Value;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return result == 1;
        }
        public static bool DeleteProduct(int ProductID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProduct_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    command.Parameters.AddWithValue("@UpdatedByUserID", (object?)UpdatedByUserID ?? DBNull.Value);
                    SqlParameter returnParameter = new SqlParameter() { Direction = ParameterDirection.ReturnValue };
                    command.Parameters.Add(returnParameter);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        result = (int)returnParameter.Value;
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return result == 1;
        }
        public static DataTable GetAllProduct()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProduct_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return dt;
        }

        public static DataTable GetProductsPaged(int PageNumber, int RowsPerPage)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetProductsPaged", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                    command.Parameters.Add("@RowsPerPage", SqlDbType.Int).Value = RowsPerPage;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return dt;
        }

        public class ProductSearchCriteria
        {
            public string SearchText { get; set; } = "";
            public string SearchBy { get; set; } = "Name"; // Name, ID, Category, Brand
            public int? CategoryId { get; set; }
            public bool? IsActive { get; set; } // Nullable for "All" tab
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "Name";
        }

        public static DataTable GetProductWithUnitsByBarcode(string barcode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetProductWithUnitsByBarcode", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Barcode", SqlDbType.NVarChar, 100).Value = barcode;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return dt;
        }

        public static DataTable SearchProductsPages(ProductSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchProductsPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    // SearchText - NULL if empty
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = 
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    
                    // SearchBy
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
                    
                    // CategoryId - NULL if not specified or -1
                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = 
                        (criteria.CategoryId.HasValue && criteria.CategoryId.Value > 0) ? criteria.CategoryId.Value : DBNull.Value;
                    
                    // IsActive - NULL for "All" tab
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 
                        criteria.IsActive.HasValue ? criteria.IsActive.Value : DBNull.Value;
                    
                    // Pagination
                    command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = criteria.PageNumber;
                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value = criteria.PageSize;
                    
                    // SortBy
                    command.Parameters.Add("@SortBy", SqlDbType.NVarChar, 50).Value = criteria.SortBy;
                    
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        // Log error
                    }
                }
            }
            return dt;
        }
    }
}
