using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsPaymentMethodData
    {
        public static bool GetPaymentMethodByID(int PaymentMethodID, ref string MethodName, ref string? Description, ref bool IsActiveForSales, ref bool IsActiveForPurchases)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPaymentMethod_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                MethodName = (string)reader["MethodName"];
                                Description = reader["Description"] != DBNull.Value ? (string?)reader["Description"] : null;
                                IsActiveForSales = (bool)reader["IsActiveForSales"];
                                IsActiveForPurchases = (bool)reader["IsActiveForPurchases"];
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
        public static int AddNewPaymentMethod(string MethodName, string? Description, bool IsActiveForSales, bool IsActiveForPurchases)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPaymentMethod_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@MethodName", System.Data.SqlDbType.VarChar).Value = (object?)MethodName ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@IsActiveForSales", System.Data.SqlDbType.Bit).Value = (object?)IsActiveForSales ?? DBNull.Value;
                    command.Parameters.Add("@IsActiveForPurchases", System.Data.SqlDbType.Bit).Value = (object?)IsActiveForPurchases ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewPaymentMethodID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdatePaymentMethod(int PaymentMethodID, string MethodName, string? Description, bool IsActiveForSales, bool IsActiveForPurchases)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPaymentMethod_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
                    command.Parameters.Add("@MethodName", System.Data.SqlDbType.VarChar).Value = (object?)MethodName ?? DBNull.Value;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = (object?)Description ?? DBNull.Value;
                    command.Parameters.Add("@IsActiveForSales", System.Data.SqlDbType.Bit).Value = (object?)IsActiveForSales ?? DBNull.Value;
                    command.Parameters.Add("@IsActiveForPurchases", System.Data.SqlDbType.Bit).Value = (object?)IsActiveForPurchases ?? DBNull.Value;
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
        public static bool DeletePaymentMethod(int PaymentMethodID )
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPaymentMethod_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
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
        public static DataTable GetAllPaymentMethod()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPaymentMethod_GetAll", connection))
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
