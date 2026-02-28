using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsSalesData
    {
        public static bool GetSaleByID(int SaleID, ref int TransactionID, ref int? CustomerID, ref DataTable detailsTable)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSales_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                TransactionID = (int)reader["TransactionID"];
                                CustomerID = reader["CustomerID"] != DBNull.Value ? (int?)reader["CustomerID"] : null;
                            }
                            reader.NextResult();

                            // Load the second result set (ProductSales details)
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

        public static int AddNewSale(int TransactionID, int? CustomerID, DataTable? detailsTable)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSales_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionID", SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = (object?)CustomerID ?? DBNull.Value;

                    SqlParameter tvpParam = command.Parameters.Add("@SaleItems", SqlDbType.Structured);
                    tvpParam.TypeName = "dbo.ProductSalesType";
                    tvpParam.Value = detailsTable ?? new DataTable();

                    SqlParameter outputIdParam = new SqlParameter("@NewSaleID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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

        public static bool UpdateSale(int SaleID, int TransactionID, int? CustomerID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSales_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
                    command.Parameters.Add("@TransactionID", SqlDbType.Int).Value = (object?)TransactionID ?? DBNull.Value;
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = (object?)CustomerID ?? DBNull.Value;

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

        public static bool DeleteSale(int SaleID, int? UpdatedByUserID = null)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSales_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SaleID", SaleID);
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

        public static DataTable GetAllSales()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spSales_GetAll", connection))
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

        public class SalesSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "SaleID";        // SaleID, CustomerName
            public byte? TransactionStatus { get; set; }            // 1=InProgress, 2=Cancelled, 3=Completed, null=All
            public string? CustomerType { get; set; }               // Person, Company, null for all
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "TransactionDate";
        }

        public static DataTable SearchSalesPages(SalesSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchSalesPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
                    command.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value =
                        criteria.TransactionStatus.HasValue ? criteria.TransactionStatus.Value : DBNull.Value;
                    command.Parameters.Add("@CustomerType", SqlDbType.NVarChar, 20).Value =
                        (object?)criteria.CustomerType ?? DBNull.Value;
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
