using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsEmployeeData
    {
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
    }
}
