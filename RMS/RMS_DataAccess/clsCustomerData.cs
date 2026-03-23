using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsCustomerData
    {
        public static bool GetCustomerByID(int CustomerID, ref int? PersonID, ref int? CompanyID, ref int? AccountID, ref bool IsActive, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCustomer_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
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
                                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime)reader["UpdatedDate"] : DateTime.MinValue;
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

        public static int AddNewCustomer(int? PersonID, int? CompanyID, int? AccountID, bool IsActive, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCustomer_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = (object?)CompanyID ?? DBNull.Value;
                    command.Parameters.Add("@AccountID", SqlDbType.Int).Value = (object?)AccountID ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewCustomerID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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

        public static bool UpdateCustomer(int CustomerID, int? PersonID, int? CompanyID, int? AccountID, bool IsActive, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCustomer_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = (object?)CompanyID ?? DBNull.Value;
                    command.Parameters.Add("@AccountID", SqlDbType.Int).Value = (object?)AccountID ?? DBNull.Value;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@UpdatedByUserID", SqlDbType.Int).Value = (object?)UpdatedByUserID ?? DBNull.Value;
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

        public static bool DeleteCustomer(int CustomerID, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCustomer_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
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

        public static DataTable GetAllCustomer()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCustomer_GetAll", connection))
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

        public class CustomerSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "CustomerName";
            public string? CustomerType { get; set; }
            public bool? IsActive { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "CustomerName";
        }

        public static DataTable SearchCustomerPages(CustomerSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchCustomerPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
                    command.Parameters.Add("@CustomerType", SqlDbType.NVarChar, 20).Value =
                        (object?)criteria.CustomerType ?? DBNull.Value;
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
