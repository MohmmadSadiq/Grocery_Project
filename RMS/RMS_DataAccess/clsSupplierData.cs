using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsSupplierData
    {
        public static bool GetSupplierByID(int SupplierID, ref int? PersonID, ref int? CompanyID, ref int? AccountID, ref bool IsActive, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSupplier_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PersonID = reader["PersonID"] != DBNull.Value ? (int?)reader["PersonID"] : null;
                                CompanyID = reader["CompanyID"] != DBNull.Value ? (int?)reader["CompanyID"] : null;
                                AccountID = reader["AccountID"] != DBNull.Value ? (int?)reader["AccountID"] : null;
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
        public static int AddNewSupplier(int? PersonID, int? CompanyID, int? AccountID, bool IsActive, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSupplier_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@CompanyID", System.Data.SqlDbType.Int).Value = (object?)CompanyID ?? DBNull.Value;
                    command.Parameters.Add("@AccountID", System.Data.SqlDbType.Int).Value = (object?)AccountID ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = (object?)IsActive ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewSupplierID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateSupplier(int SupplierID, int? PersonID, int? CompanyID, int? AccountID, bool IsActive, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSupplier_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@CompanyID", System.Data.SqlDbType.Int).Value = (object?)CompanyID ?? DBNull.Value;
                    command.Parameters.Add("@AccountID", System.Data.SqlDbType.Int).Value = (object?)AccountID ?? DBNull.Value;
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
        public static bool DeleteSupplier(int SupplierID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSupplier_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);
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
        
        public static DataTable GetAllSupplier()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Suppliers_View", connection))
                {
                    command.CommandType = CommandType.Text;
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
        public class SupplierSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "SupplierName"; // SupplierName, Phone, Code
            public string? SupplierType { get; set; }              // Person, Company, null for all
            public bool? IsActive { get; set; }                    // null for all
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "SupplierName";
        }

        public static DataTable SearchSupplierPages(SupplierSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchSupplierPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
                    command.Parameters.Add("@SupplierType", SqlDbType.NVarChar, 20).Value =
                        (object?)criteria.SupplierType ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value =
                        criteria.IsActive.HasValue ? criteria.IsActive.Value : DBNull.Value;
                    command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = criteria.PageNumber;
                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value = criteria.PageSize;
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
