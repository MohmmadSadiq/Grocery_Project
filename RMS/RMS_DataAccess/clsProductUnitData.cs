using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsProductUnitData
    {
        public static bool GetProductUnitByID(int ProductUnitID, ref int ProductID, ref int UnitID, ref string? Description, ref decimal ConversionFactor, ref decimal? SalePrice, ref string? Barcode, ref bool IsActive, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductUnitID", ProductUnitID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                ProductID = (int)reader["ProductID"];
                                UnitID = (int)reader["UnitID"];
                                Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null;
                                ConversionFactor = (decimal)reader["ConversionFactor"];
                                SalePrice = reader["SalePrice"] != DBNull.Value ? (decimal?)reader["SalePrice"] : null;
                                Barcode = reader["Barcode"] != DBNull.Value ? (string?)reader["Barcode"] : null;
                                IsActive = (bool)reader["IsActive"];
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
        public static int AddNewProductUnit(int ProductID, int UnitID, string? Description, decimal ConversionFactor, decimal? SalePrice, string? Barcode, bool IsActive, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int).Value = (object?)ProductID ?? DBNull.Value;
                    command.Parameters.Add("@UnitID", System.Data.SqlDbType.Int).Value = (object?)UnitID ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@ConversionFactor", System.Data.SqlDbType.VarChar).Value = (object?)ConversionFactor ?? DBNull.Value;
                    command.Parameters.Add("@SalePrice", System.Data.SqlDbType.VarChar).Value = (object?)SalePrice ?? DBNull.Value;
                    command.Parameters.Add("@Barcode", System.Data.SqlDbType.VarChar).Value = (object?)Barcode ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = (object?)IsActive ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewProductUnitID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateProductUnit(int ProductUnitID, int ProductID, int UnitID, string? Description, decimal ConversionFactor, decimal? SalePrice, string? Barcode, bool IsActive, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductUnitID", ProductUnitID);
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int).Value = (object?)ProductID ?? DBNull.Value;
                    command.Parameters.Add("@UnitID", System.Data.SqlDbType.Int).Value = (object?)UnitID ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@ConversionFactor", System.Data.SqlDbType.VarChar).Value = (object?)ConversionFactor ?? DBNull.Value;
                    command.Parameters.Add("@SalePrice", System.Data.SqlDbType.VarChar).Value = (object?)SalePrice ?? DBNull.Value;
                    command.Parameters.Add("@Barcode", System.Data.SqlDbType.VarChar).Value = (object?)Barcode ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = (object?)IsActive ?? DBNull.Value;
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
        public static bool DeleteProductUnit(int ProductUnitID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductUnitID", ProductUnitID);
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
        public static DataTable GetAllProductUnit()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_GetAll", connection))
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

        public static DataTable GetProductUnitsByProductID(int ProductID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_GetByProductID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
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
                        // Log error - If stored procedure doesn't exist, use GetAllProductUnit and filter
                        dt = GetAllProductUnit();
                        // Filter by ProductID
                        var filteredRows = dt.Select($"ProductID = {ProductID}");
                        if (filteredRows.Length > 0)
                        {
                            dt = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            dt.Clear();
                        }
                    }
                }
            }
            return dt;
        }
    

        public static bool GetProductUnitByBarcode(string Barcode, ref int ProductUnitID, ref int ProductID, ref int UnitID, ref string? Description, ref decimal ConversionFactor, ref decimal? SalePrice, ref bool IsActive, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_GetByBarcode", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Barcode", Barcode);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                ProductUnitID = (int)reader["ProductUnitID"];
                                ProductID = (int)reader["ProductID"];
                                UnitID = (int)reader["UnitID"];
                                Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null;
                                ConversionFactor = (decimal)reader["ConversionFactor"];
                                SalePrice = reader["SalePrice"] != DBNull.Value ? (decimal?)reader["SalePrice"] : null;
                                IsActive = (bool)reader["IsActive"];
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

        public static DataTable SearchProductUnitsByBarcode(string Barcode, int? PageNumber = null, int? PageSize = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spProductUnit_SearchByBarcode", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Barcode", Barcode);
                    command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = (object?)PageNumber ?? DBNull.Value;
                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value = (object?)PageSize ?? DBNull.Value;
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
