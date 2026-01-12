using System;
using System.Data;
using DVLD_DataAccess;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsCompanyData
    {
        public static bool GetCompanyByID(int CompanyID, ref string CompanyName, ref int? ContactPersonID, ref string? Phone, ref string? Email, ref string? Address, ref int? CountryID, ref string? CommercialNumber, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID)
        {
            
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCompany_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyID", CompanyID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                CompanyName = (string)reader["CompanyName"];
                                ContactPersonID = reader["ContactPersonID"] != DBNull.Value ? (int?)reader["ContactPersonID"] : null;
                                Phone = reader["Phone"] != DBNull.Value ? (string?)reader["Phone"] : null;
                                Email = reader["Email"] != DBNull.Value ? (string?)reader["Email"] : null;
                                Address = reader["Address"] != DBNull.Value ? (string?)reader["Address"] : null;
                                CountryID = reader["CountryID"] != DBNull.Value ? (int?)reader["CountryID"] : null;
                                CommercialNumber = reader["CommercialNumber"] != DBNull.Value ? (string?)reader["CommercialNumber"] : null;
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
        public static int AddNewCompany(string CompanyName, int? ContactPersonID, string? Phone, string? Email, string? Address, int? CountryID, string? CommercialNumber, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCompany_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CompanyName", System.Data.SqlDbType.VarChar).Value = (object?)CompanyName ?? DBNull.Value;
                    command.Parameters.Add("@ContactPersonID", System.Data.SqlDbType.Int).Value = (object?)ContactPersonID ?? DBNull.Value;
                    command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar).Value = (object?)Phone ?? DBNull.Value;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = (object?)Email ?? DBNull.Value;
                    command.Parameters.Add("@Address", System.Data.SqlDbType.VarChar).Value = (object?)Address ?? DBNull.Value;
                    command.Parameters.Add("@CountryID", System.Data.SqlDbType.Int).Value = (object?)CountryID ?? DBNull.Value;
                    command.Parameters.Add("@CommercialNumber", System.Data.SqlDbType.VarChar).Value = (object?)CommercialNumber ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewCompanyID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateCompany(int CompanyID, string CompanyName, int? ContactPersonID, string? Phone, string? Email, string? Address, int? CountryID, string? CommercialNumber, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCompany_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyID", CompanyID);
                    command.Parameters.Add("@CompanyName", System.Data.SqlDbType.VarChar).Value = (object?)CompanyName ?? DBNull.Value;
                    command.Parameters.Add("@ContactPersonID", System.Data.SqlDbType.Int).Value = (object?)ContactPersonID ?? DBNull.Value;
                    command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar).Value = (object?)Phone ?? DBNull.Value;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = (object?)Email ?? DBNull.Value;
                    command.Parameters.Add("@Address", System.Data.SqlDbType.VarChar).Value = (object?)Address ?? DBNull.Value;
                    command.Parameters.Add("@CountryID", System.Data.SqlDbType.Int).Value = (object?)CountryID ?? DBNull.Value;
                    command.Parameters.Add("@CommercialNumber", System.Data.SqlDbType.VarChar).Value = (object?)CommercialNumber ?? DBNull.Value;
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
        public static bool DeleteCompany(int CompanyID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCompany_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyID", CompanyID);
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
        public static DataTable GetAllCompany()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spCompany_GetAll", connection))
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
    }
}
