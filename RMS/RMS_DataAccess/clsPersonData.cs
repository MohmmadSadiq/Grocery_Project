using System;
using System.Data;
using DVLD_DataAccess;
using Microsoft.Data.SqlClient;

namespace RMS_DataAccess
{
    public class clsPersonData
    {
        // ------------------------------------------------------------------------
        // 1. GET PERSON BY ID
        // ------------------------------------------------------------------------
        public static bool GetPersonInfoByID(int PersonID, ref string? NationalNo, ref string FirstName,
            ref string? SecondName, ref string? ThirdName, ref string LastName, ref DateTime? DateOfBirth,
            ref byte? Gender, ref string? Address, ref string? Phone, ref string? Email,
            ref int? NationalityCountryID, ref string? ImagePath, ref int? CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPeople_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                // Non-Nullable Columns (Mandatory in DB)
                                FirstName = (string)reader["FirstName"];
                                LastName = (string)reader["LastName"];

                                // Nullable Columns (Handling DBNull)
                                NationalNo = reader["NationalNo"] != DBNull.Value ? (string)reader["NationalNo"] : null;
                                SecondName = reader["SecondName"] != DBNull.Value ? (string)reader["SecondName"] : null;
                                ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : null;

                                DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null;
                                Gender = reader["Gender"] != DBNull.Value ? (byte?)reader["Gender"] : null;

                                Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : null;
                                Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : null;
                                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null;

                                NationalityCountryID = reader["NationalityCountryID"] != DBNull.Value ? (int?)reader["NationalityCountryID"] : null;
                                ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : null;
                                CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int?)reader["CreatedByUserID"] : null;
                            }
                        }
                    }
                    catch (Exception )
                    {
                        // Log Error here
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        // ------------------------------------------------------------------------
        // 2. ADD NEW PERSON
        // ------------------------------------------------------------------------
        public static int AddNewPerson(string? NationalNo, string FirstName, string? SecondName,
            string? ThirdName, string LastName, DateTime? DateOfBirth, byte? Gender,
            string? Address, string? Phone, string? Email, int? NationalityCountryID,
            string? ImagePath, int? CreatedByUserID)
        {
            int newPersonID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPeople_AddNew", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Mandatory Parameters
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);

                    // Nullable Parameters (Sending DBNull if null)
                    command.Parameters.AddWithValue("@NationalNo", (object?)NationalNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SecondName", (object?)SecondName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ThirdName", (object?)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object?)DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object?)Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID", (object?)NationalityCountryID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", (object?)ImagePath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedByUserID", (object?)CreatedByUserID ?? DBNull.Value);

                    // Output Parameter
                    SqlParameter outputIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
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
                            newPersonID = (int)outputIdParam.Value;
                        }
                    }
                    catch (Exception )
                    {
                        // Log Error
                    }
                }
            }

            return newPersonID;
        }

        // ------------------------------------------------------------------------
        // 3. UPDATE PERSON
        // ------------------------------------------------------------------------
        public static bool UpdatePerson(int PersonID, string? NationalNo, string FirstName, string? SecondName,
            string? ThirdName, string LastName, DateTime? DateOfBirth, byte? Gender,
            string? Address, string? Phone, string? Email, int? NationalityCountryID,
            string? ImagePath, int? UpdatedByUserID)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPeople_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);

                    // Nullable Parameters
                    command.Parameters.AddWithValue("@NationalNo", (object?)NationalNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SecondName", (object?)SecondName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ThirdName", (object?)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", (object?)DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object?)Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID", (object?)NationalityCountryID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", (object?)ImagePath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedByUserID", (object?)UpdatedByUserID ?? DBNull.Value);

                    // Return Value Parameter
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
                    catch (Exception )
                    {
                        return false;
                    }
                }
            }

            return result == 1;
        }

        // ------------------------------------------------------------------------
        // 4. DELETE PERSON (Soft Delete)
        // ------------------------------------------------------------------------
        public static bool DeletePerson(int PersonID, int? UpdatedByUserID)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPeople_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
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
                    catch (Exception )
                    {
                        // Log Error
                    }
                }
            }
            return result == 1;
        }

        // ------------------------------------------------------------------------
        // 5. GET ALL PEOPLE
        // ------------------------------------------------------------------------
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spPeople_GetAll", connection))
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
                    catch (Exception )
                    {
                        // Log Error
                    }
                }
            }
            return dt;
        }
    }
}