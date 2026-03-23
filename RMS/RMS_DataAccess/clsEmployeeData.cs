using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsEmployeeData
    {
        public class EmployeeSearchCriteria
        {
            public string SearchText { get; set; } = "";
            public string SearchBy { get; set; } = "FullName";
            public int? PositionID { get; set; }
            public int? CountryID { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
        }

        public static bool GetEmployeeByID(int EmployeeID, ref int PersonID, ref int PositionID, ref DateTime HireDate, ref DateTime? FireDate, ref int CreatedByUserID, ref DateTime CreatedDate, ref int UpdatedByUserID, ref DateTime UpdatedDate)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spEmployee_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PersonID = (int)reader["PersonID"];
                                PositionID = (int)reader["PositionID"];
                                HireDate = (DateTime)reader["HireDate"];
                                FireDate = reader["FireDate"] != DBNull.Value ? (DateTime?)reader["FireDate"] : null;
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
                                UpdatedByUserID = (int)reader["UpdatedByUserID"];
                                UpdatedDate = (DateTime)reader["UpdatedDate"];
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
        public static int AddNewEmployee(int PersonID, int PositionID, DateTime HireDate, DateTime? FireDate, int CreatedByUserID)
        {
            int newID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spEmployee_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@PositionID", System.Data.SqlDbType.Int).Value = (object?)PositionID ?? DBNull.Value;
                    command.Parameters.Add("@HireDate", System.Data.SqlDbType.Date).Value = (object?)HireDate ?? DBNull.Value;
                    command.Parameters.Add("@FireDate", System.Data.SqlDbType.Date).Value = (object?)FireDate ?? DBNull.Value;
                    command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object?)CreatedByUserID ?? DBNull.Value;
                    SqlParameter outputIdParam = new SqlParameter("@NewEmployeeID", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        public static bool UpdateEmployee(int EmployeeID, int PersonID, int PositionID, DateTime HireDate, DateTime? FireDate, int UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spEmployee_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = (object?)PersonID ?? DBNull.Value;
                    command.Parameters.Add("@PositionID", System.Data.SqlDbType.Int).Value = (object?)PositionID ?? DBNull.Value;
                    command.Parameters.Add("@HireDate", System.Data.SqlDbType.Date).Value = (object?)HireDate ?? DBNull.Value;
                    command.Parameters.Add("@FireDate", System.Data.SqlDbType.Date).Value = (object?)FireDate ?? DBNull.Value;
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
        public static bool DeleteEmployee(int EmployeeID ,int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spEmployee_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
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
        public static DataTable GetAllEmployee()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spEmployee_GetAll", connection))
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

        public static DataTable SearchEmployeesPages(EmployeeSearchCriteria criteria, ref int totalCount)
        {
            totalCount = 0;
            DataTable dt = new DataTable();

            // Ensure search always has valid defaults even when caller passes null.
            criteria ??= new EmployeeSearchCriteria();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchEmployeesPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText.Trim();

                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchBy) ? "FullName" : criteria.SearchBy;

                    command.Parameters.Add("@PositionID", SqlDbType.Int).Value =
                        (criteria.PositionID.HasValue && criteria.PositionID.Value > 0) ? criteria.PositionID.Value : DBNull.Value;

                    command.Parameters.Add("@CountryID", SqlDbType.Int).Value =
                        (criteria.CountryID.HasValue && criteria.CountryID.Value > 0) ? criteria.CountryID.Value : DBNull.Value;

                    command.Parameters.Add("@PageNumber", SqlDbType.Int).Value =
                        criteria.PageNumber > 0 ? criteria.PageNumber : 1;

                    command.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                        criteria.PageSize > 0 ? criteria.PageSize : 20;

                    SqlParameter totalCountOutput = new SqlParameter("@TotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalCountOutput);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }

                        if (totalCountOutput.Value != DBNull.Value)
                            totalCount = (int)totalCountOutput.Value;
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
