using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsTransactionData
    {
        public static bool GetTransactionByID(int TransactionID, ref int? PaymentID, ref DateTime TransactionDate, ref byte TransactionType, ref byte TransactionStatus, ref decimal TotalAmount, ref string? Nots, ref DateTime CreatedDate, ref int? CreatedByUserID, ref DateTime? UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spTransaction_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionID", TransactionID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PaymentID = reader["PaymentID"] != DBNull.Value ? (int?)reader["PaymentID"] : null;
                                TransactionDate = (DateTime)reader["TransactionDate"];
                                TransactionType = (byte)reader["TransactionType"];
                                TransactionStatus = (byte)reader["TransactionStatus"];
                                TotalAmount = (decimal)reader["TotalAmount"];
                                Nots = reader["Nots"] != DBNull.Value ? (string?)reader["Nots"] : null;
                                CreatedDate = (DateTime)reader["CreatedDate"];
                                CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value? (int)reader["CreatedByUserID"] : null ;
                                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime?)reader["UpdatedDate"] : null;
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
        public static int AddNewTransaction(int? PaymentID, DateTime TransactionDate, byte TransactionType, byte TransactionStatus, decimal TotalAmount, string? Nots, int? CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spTransaction_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PaymentID", System.Data.SqlDbType.Int).Value = (object?)PaymentID ?? DBNull.Value;
                    command.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = (object?)TransactionDate ?? DBNull.Value;
                    command.Parameters.Add("@TransactionType", System.Data.SqlDbType.TinyInt).Value = (object?)TransactionType ?? DBNull.Value;
                    command.Parameters.Add("@TransactionStatus", System.Data.SqlDbType.TinyInt).Value = (object?)TransactionStatus ?? DBNull.Value;
                    command.Parameters.Add("@TotalAmount", System.Data.SqlDbType.VarChar).Value = (object?)TotalAmount ?? DBNull.Value;
                    command.Parameters.Add("@Nots", System.Data.SqlDbType.NVarChar).Value = (object?)Nots ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewTransactionID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateTransaction(int TransactionID, int? PaymentID, DateTime TransactionDate, byte TransactionType, byte TransactionStatus, decimal TotalAmount, string? Nots, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spTransaction_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionID", TransactionID);
                    command.Parameters.Add("@PaymentID", System.Data.SqlDbType.Int).Value = (object?)PaymentID ?? DBNull.Value;
                    command.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = (object?)TransactionDate ?? DBNull.Value;
                    command.Parameters.Add("@TransactionType", System.Data.SqlDbType.TinyInt).Value = (object?)TransactionType ?? DBNull.Value;
                    command.Parameters.Add("@TransactionStatus", System.Data.SqlDbType.TinyInt).Value = (object?)TransactionStatus ?? DBNull.Value;
                    command.Parameters.Add("@TotalAmount", System.Data.SqlDbType.VarChar).Value = (object?)TotalAmount ?? DBNull.Value;
                    command.Parameters.Add("@Nots", System.Data.SqlDbType.NVarChar).Value = (object?)Nots ?? DBNull.Value;
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
        public static bool DeleteTransaction(int TransactionID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spTransaction_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionID", TransactionID);
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
        public static DataTable GetAllTransaction()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spTransaction_GetAll", connection))
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
