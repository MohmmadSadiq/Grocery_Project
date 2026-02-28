using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsPurchaseData
    {
        public static bool GetPurchaseByID(int PurchaseID, ref int TransactionID, ref int? SupplierID, ref string? InvoiceNumber, ref int? PurchasedByEmployeeID, ref string? InvoiceDocumentPath, ref DataTable detailsTable)
        {
            bool isFound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPurchase_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                TransactionID = (int)reader["TransactionID"];
                                SupplierID = reader["SupplierID"] != DBNull.Value ? (int?)reader["SupplierID"] : null;
                                InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string?)reader["InvoiceNumber"] : null;
                                PurchasedByEmployeeID = reader["PurchasedByEmployeeID"] != DBNull.Value ? (int?)reader["PurchasedByEmployeeID"] : null;
                                InvoiceDocumentPath = reader["InvoiceDocumentPath"] != DBNull.Value ? (string?)reader["InvoiceDocumentPath"] : null;
                            }
                            reader.NextResult();

                            // Load the second result set (details)
                            if (reader.HasRows)
                                detailsTable.Load(reader);
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
        // public static int AddNewPurchase(int TransactionID, int? SupplierID, string? InvoiceNumber, int? PurchasedByEmployeeID)
        // {
        //     int newID = -1;
        //     using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //     {
        //         using (SqlCommand command = new SqlCommand("spPurchase_AddNew", connection))
        //         {
        //             command.CommandType = CommandType.StoredProcedure;
        //             command.Parameters.Add("@TransactionID", System.Data.SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
        //             command.Parameters.Add("@SupplierID", System.Data.SqlDbType.Int).Value = (object?)SupplierID ?? DBNull.Value;
        //             command.Parameters.Add("@InvoiceNumber", System.Data.SqlDbType.VarChar).Value = (object?)InvoiceNumber ?? DBNull.Value;
        //             command.Parameters.Add("@PurchasedByEmployeeID", System.Data.SqlDbType.Int).Value = (object?)PurchasedByEmployeeID ?? DBNull.Value;
        //             SqlParameter outputIdParam = new SqlParameter("@NewPurchaseID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //             command.Parameters.Add(outputIdParam);
        //             try
        //             {
        //                 connection.Open();
        //                 command.ExecuteNonQuery();
        //                 if (outputIdParam.Value != DBNull.Value)
        //                     newID = (int)outputIdParam.Value;
        //             }
        //             catch (Exception)
        //             {
        //                 // Log error
        //             }
        //         }
        //     }
        //     return newID;
        // }
        public static int AddNewPurchase(int TransactionID, int? SupplierID, string? InvoiceNumber, int? PurchasedByEmployeeID, string? InvoiceDocumentPath, DataTable? detailsTable)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPurchase_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionID", System.Data.SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
                    command.Parameters.Add("@SupplierID", System.Data.SqlDbType.Int).Value = (object?)SupplierID ?? DBNull.Value;
                    command.Parameters.Add("@InvoiceNumber", System.Data.SqlDbType.VarChar).Value = (object?)InvoiceNumber ?? DBNull.Value;
                    command.Parameters.Add("@PurchasedByEmployeeID", System.Data.SqlDbType.Int).Value = (object?)PurchasedByEmployeeID ?? DBNull.Value;
                    command.Parameters.Add("@InvoiceDocumentPath", System.Data.SqlDbType.NVarChar, 500).Value = (object?)InvoiceDocumentPath ?? DBNull.Value;
                    SqlParameter tvpParam =command.Parameters.Add("@NewBatches", SqlDbType.Structured);
                       tvpParam.TypeName = "dbo.PurchaseProductBatchesType";
                       tvpParam.Value = detailsTable ?? new DataTable(); // هنا يكون DataTable أو List<PurchaseBatch> محوّل إلى DataTable
                    SqlParameter outputIdParam = new SqlParameter("@NewPurchaseID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdatePurchase(int PurchaseID, int TransactionID, int? SupplierID, string? InvoiceNumber, int? PurchasedByEmployeeID, string? InvoiceDocumentPath)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPurchase_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PurchaseID", PurchaseID);
                    command.Parameters.Add("@TransactionID", System.Data.SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
                    command.Parameters.Add("@SupplierID", System.Data.SqlDbType.Int).Value = (object?)SupplierID ?? DBNull.Value;
                    command.Parameters.Add("@InvoiceNumber", System.Data.SqlDbType.VarChar).Value = (object?)InvoiceNumber ?? DBNull.Value;
                    command.Parameters.Add("@PurchasedByEmployeeID", System.Data.SqlDbType.Int).Value = (object?)PurchasedByEmployeeID ?? DBNull.Value;
                    command.Parameters.Add("@InvoiceDocumentPath", System.Data.SqlDbType.NVarChar, 500).Value = (object?)InvoiceDocumentPath ?? DBNull.Value;
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
        public static bool DeletePurchase(int PurchaseID, int? UpdatedByUserID = null)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPurchase_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PurchaseID", PurchaseID);
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
        public static DataTable GetAllPurchase()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPurchase_GetAll", connection))
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

        // ── Search / Pagination ───────────────────────────────────────────────────

        public class PurchaseSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "InvoiceNumber"; // InvoiceNumber, PurchaseID, SupplierName, EmployeeName
            public byte? TransactionStatus { get; set; }            // 1=InProgress, 2=Cancelled, 3=Completed, null=All
            public string? SupplierType { get; set; }               // Person, Company, null for all
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "TransactionDate";
        }

        public static DataTable SearchPurchasePages(PurchaseSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchPurchasePages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
                    command.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value =
                        criteria.TransactionStatus.HasValue ? criteria.TransactionStatus.Value : DBNull.Value;
                    command.Parameters.Add("@SupplierType", SqlDbType.NVarChar, 20).Value =
                        (object?)criteria.SupplierType ?? DBNull.Value;
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

        public static bool SavePurchaseWithDetails(
        int TransactionID,
        int? SupplierID,
        string? InvoiceNumber,
        int? PurchasedByEmployeeID,
        DataTable detailsTable)
    {
        bool isSuccess = false;
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Insert Purchase Header
                        int newPurchaseID = -1;
                        using (SqlCommand cmd = new SqlCommand("spPurchase_AddNew", connection, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@TransactionID", SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
                            cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = (object?)SupplierID ?? DBNull.Value;
                            cmd.Parameters.Add("@InvoiceNumber", SqlDbType.VarChar).Value = (object?)InvoiceNumber ?? DBNull.Value;
                            cmd.Parameters.Add("@PurchasedByEmployeeID", SqlDbType.Int).Value = (object?)PurchasedByEmployeeID ?? DBNull.Value;
                            SqlParameter outputIdParam = new SqlParameter("@NewPurchaseID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            cmd.Parameters.Add(outputIdParam);

                            cmd.ExecuteNonQuery();
                            if (outputIdParam.Value != DBNull.Value)
                                newPurchaseID = (int)outputIdParam.Value;
                            else
                                throw new Exception("Failed to insert purchase header.");
                        }

                        // 2. Insert Purchase Details
                        foreach (DataRow row in detailsTable.Rows)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand("spPurchaseProductBatch_AddNew", connection, transaction))
                            {
                                cmdDetail.CommandType = CommandType.StoredProcedure;
                                cmdDetail.Parameters.AddWithValue("@PurchaseID", newPurchaseID);
                                cmdDetail.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                                cmdDetail.Parameters.AddWithValue("@BatchNumber", row["BatchNumber"]);
                                cmdDetail.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                                cmdDetail.Parameters.AddWithValue("@ExpiryDate", row["ExpiryDate"]);
                                // Add other parameters as needed
                                cmdDetail.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        isSuccess = false;
                    }
                }
            }
            return isSuccess;
        }
    }
}
