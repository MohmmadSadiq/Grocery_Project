using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsPaymentData
    {
        public static bool GetPaymentByID(int PaymentID, ref DateTime PaymentDate, ref int PaymentMethodID, ref decimal PaymentAmount, ref string? Notes, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime UpdatedDate, ref int? UpdatedByUserID, ref DataTable? allocations)
        {
            bool isFound = false;
            allocations = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPayment_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", PaymentID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Read Payment Header (First ResultSet)
                            if (reader.Read())
                            {
                                isFound = true;
                                PaymentDate = (DateTime)reader["PaymentDate"];
                                PaymentMethodID = (int)reader["PaymentMethodID"];
                                PaymentAmount = (decimal)reader["PaymentAmount"];
                                Notes = reader["Notes"] != DBNull.Value ? (string?)reader["Notes"] : null;
                                CreatedDate = (DateTime)reader["CreatedDate"];
                                CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                                UpdatedDate = (DateTime)reader["UpdatedDate"];
                                UpdatedByUserID = reader["UpdatedByUserID"] != DBNull.Value ? (int?)reader["UpdatedByUserID"] : null;
                            }

                            // Read Payment Allocations (Second ResultSet)
                            if (reader.NextResult())
                            {
                                allocations.Load(reader);
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
        public static int AddNewPayment(DateTime PaymentDate, int PaymentMethodID, decimal PaymentAmount, string? Notes, int? CreatedByUserID, DataTable? allocations = null)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPayment_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime).Value = (object?)PaymentDate ?? DBNull.Value;
                    command.Parameters.Add("@PaymentMethodID", System.Data.SqlDbType.Int).Value = (object?)PaymentMethodID ?? DBNull.Value;
                    command.Parameters.Add("@PaymentAmount", System.Data.SqlDbType.Decimal).Value = (object?)PaymentAmount ?? DBNull.Value;
                    command.Parameters.Add("@Notes", System.Data.SqlDbType.NVarChar).Value = (object?)Notes ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    
                    // Add Table-Valued Parameter for allocations
                    SqlParameter allocationsParam = command.Parameters.AddWithValue("@NewAllocations", allocations ?? new DataTable());
                    allocationsParam.SqlDbType = SqlDbType.Structured;
                    allocationsParam.TypeName = "PaymentAllocationsType";
                    
                    SqlParameter outputIdParam = new SqlParameter("@NewPaymentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdatePayment(int PaymentID, DateTime PaymentDate, int PaymentMethodID, decimal PaymentAmount, string? Notes, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPayment_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", PaymentID);
                    command.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime).Value = (object?)PaymentDate ?? DBNull.Value;
                    command.Parameters.Add("@PaymentMethodID", System.Data.SqlDbType.Int).Value = (object?)PaymentMethodID ?? DBNull.Value;
                    command.Parameters.Add("@PaymentAmount", System.Data.SqlDbType.Decimal).Value = (object?)PaymentAmount ?? DBNull.Value;
                    command.Parameters.Add("@Notes", System.Data.SqlDbType.NVarChar).Value = (object?)Notes ?? DBNull.Value;
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
        public static bool DeletePayment(int PaymentID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPayment_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", PaymentID);
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
        public static DataTable GetAllPayment()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPayment_GetAll", connection))
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
