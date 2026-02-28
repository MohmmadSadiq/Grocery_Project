using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsBatchData
    {
        public static bool GetBatchByID(int BatchID, ref int PurchaseID, ref int ProductUnitID, ref decimal TotalQuantity, ref decimal UniteCostPrice, ref DateTime? ProductionDate, ref DateTime? ExpiryDate, ref string? BatchNumber)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spBatch_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BatchID", BatchID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PurchaseID = (int)reader["PurchaseID"];
                                ProductUnitID = (int)reader["ProductUnitID"];
                                TotalQuantity = (decimal)reader["TotalQuantity"];
                                UniteCostPrice = (decimal)reader["UniteCostPrice"];
                                ProductionDate = reader["ProductionDate"] != DBNull.Value ? (DateTime?)reader["ProductionDate"] : null;
                                ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? (DateTime?)reader["ExpiryDate"] : null;
                                BatchNumber = reader["BatchNumber"] != DBNull.Value ? (string?)reader["BatchNumber"] : null;
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
        public static int AddNewBatch(int PurchaseID, int ProductUnitID, decimal TotalQuantity, decimal UniteCostPrice, DateTime? ProductionDate, DateTime? ExpiryDate, string? BatchNumber)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spBatch_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PurchaseID", System.Data.SqlDbType.Int).Value = (object?)PurchaseID ?? DBNull.Value;
                    command.Parameters.Add("@ProductUnitID", System.Data.SqlDbType.Int).Value = (object?)ProductUnitID ?? DBNull.Value;
                    command.Parameters.Add("@TotalQuantity", System.Data.SqlDbType.Decimal).Value = (object?)TotalQuantity ?? DBNull.Value;
                    command.Parameters.Add("@UniteCostPrice", System.Data.SqlDbType.Decimal).Value = (object?)UniteCostPrice ?? DBNull.Value;
                    command.Parameters.Add("@ProductionDate", System.Data.SqlDbType.Date).Value = (object?)ProductionDate ?? DBNull.Value;
                    command.Parameters.Add("@ExpiryDate", System.Data.SqlDbType.Date).Value = (object?)ExpiryDate ?? DBNull.Value;
                    command.Parameters.Add("@BatchNumber", System.Data.SqlDbType.NVarChar, 50).Value = (object?)BatchNumber ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewBatchID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateBatch(int BatchID, int PurchaseID, int ProductUnitID, decimal TotalQuantity, decimal UniteCostPrice, DateTime? ProductionDate, DateTime? ExpiryDate, string? BatchNumber)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spBatch_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BatchID", BatchID);
                    command.Parameters.Add("@PurchaseID", System.Data.SqlDbType.Int).Value = (object?)PurchaseID ?? DBNull.Value;
                    command.Parameters.Add("@ProductUnitID", System.Data.SqlDbType.Int).Value = (object?)ProductUnitID ?? DBNull.Value;
                    command.Parameters.Add("@TotalQuantity", System.Data.SqlDbType.Decimal).Value = (object?)TotalQuantity ?? DBNull.Value;
                    command.Parameters.Add("@UniteCostPrice", System.Data.SqlDbType.Decimal).Value = (object?)UniteCostPrice ?? DBNull.Value;
                    command.Parameters.Add("@ProductionDate", System.Data.SqlDbType.Date).Value = (object?)ProductionDate ?? DBNull.Value;
                    command.Parameters.Add("@ExpiryDate", System.Data.SqlDbType.Date).Value = (object?)ExpiryDate ?? DBNull.Value;
                    command.Parameters.Add("@BatchNumber", System.Data.SqlDbType.NVarChar, 50).Value = (object?)BatchNumber ?? DBNull.Value;
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
        public static bool DeleteBatch(int BatchID )
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spBatch_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BatchID", BatchID);
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
        public static DataTable GetAllBatch()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spBatch_GetAll", connection))
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
