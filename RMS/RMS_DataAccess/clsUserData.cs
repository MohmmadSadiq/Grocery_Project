using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsUserData
    {
        // 1. Get User By ID
        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName, ref string PasswordHash, ref string PasswordSalt, ref bool IsActive, ref DateTime? CreatedDate, ref int? CreatedByUserID, ref DateTime? UpdatedDate, ref int? UpdatedByUserID)
        {
            bool isFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spGetUserInfoByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                PersonID = (int)reader["PersonID"];
                                UserName = (string)reader["UserName"];
                                PasswordHash = (string)reader["PasswordHash"];
                                PasswordSalt = (string)reader["PasswordSalt"];
                                IsActive = (bool)reader["IsActive"];
                                CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime?)reader["CreatedDate"] : null;
                                CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
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

        public static bool GetUserInfoByUserName(string userName, ref int UserID, ref int PersonID, ref string UserName, ref string PasswordHash, ref string PasswordSalt, ref bool IsActive, ref DateTime? CreatedDate, ref int? CreatedByUserID, ref DateTime? UpdatedDate, ref int? UpdatedByUserID)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            bool isFound = false;

            using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            using SqlCommand command = new SqlCommand("spGetUserInfoByUserName", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName.Trim();

            try
            {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    PasswordHash = (string)reader["PasswordHash"];
                    PasswordSalt = (string)reader["PasswordSalt"];
                    IsActive = (bool)reader["IsActive"];
                    CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime?)reader["CreatedDate"] : null;
                    CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                    UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime?)reader["UpdatedDate"] : null;
                    UpdatedByUserID = reader["UpdatedByUserID"] != DBNull.Value ? (int?)reader["UpdatedByUserID"] : null;
                }
            }
            catch (Exception)
            {
                isFound = false;
            }

            return isFound;
        }

        // 2. Add New User
        public static int AddNewUser(int PersonID, string UserName, string PasswordHash, string PasswordSalt, bool IsActive, int? CreatedByUserID)
        {
            int newUserID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spAddNewUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", PasswordSalt);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", (object?)CreatedByUserID ?? DBNull.Value);
                    SqlParameter outputIdParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        if (outputIdParam.Value != DBNull.Value)
                        {
                            newUserID = (int)outputIdParam.Value;
                        }
                    }
                    catch (Exception)
                    {
                        // Log Error
                    }
                }
            }
            return newUserID;
        }

        // 3. Update User
        public static bool UpdateUser(int UserID, int PersonID, string UserName, string PasswordHash, string PasswordSalt, bool IsActive, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spUpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", PasswordSalt);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@UpdatedByUserID", (object?)UpdatedByUserID ?? DBNull.Value);
                    SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
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

        // 4. Delete User (Soft Delete)
        public static bool DeleteUser(int UserID, int? UpdatedByUserID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spDeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UpdatedByUserID", (object?)UpdatedByUserID ?? DBNull.Value);
                    SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        result = (int)returnParameter.Value;
                    }
                    catch (Exception)
                    {
                        // Log Error
                    }
                }
            }
            return result == 1;
        }

        // 5. Get All Users
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spGetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Log Error
                    }
                }
            }
            return dt;
        }

        // 6. Search / Pagination
        public class UserSearchCriteria
        {
            public string? SearchText { get; set; }
            public string SearchBy { get; set; } = "UserName"; // UserID, UserName, FullName, IsActive
            public bool? IsActive { get; set; }                  // null for all
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
            public string SortBy { get; set; } = "UserName";   // UserID, UserName, FullName, IsActive
        }

        public static DataTable SearchUserPages(UserSearchCriteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SearchUsersPages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(criteria.SearchText) ? DBNull.Value : criteria.SearchText;
                    command.Parameters.Add("@SearchBy", SqlDbType.NVarChar, 50).Value = criteria.SearchBy;
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
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Log Error
                    }
                }
            }
            return dt;
        }

        public static bool IsUserNameExists(string userName, int? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            const string query = @"
                SELECT TOP 1 1
                FROM dbo.Users
                WHERE UserName = @UserName
                  AND (@ExcludeUserID IS NULL OR UserID <> @ExcludeUserID)
                  AND (IsDeleted = 0 OR IsDeleted IS NULL);";

            using SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName.Trim();
            command.Parameters.Add("@ExcludeUserID", SqlDbType.Int).Value = (object?)excludeUserId ?? DBNull.Value;

            try
            {
                connection.Open();
                object? result = command.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
            catch
            {
                return false;
            }
        }
    }
}
