using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace DVLD_DataAccessLayer
{
    public static class clsUserData
    {
        public static bool GetUserByUsernameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {

            bool IsFound = false;

            string query = "SELECT UserID, PersonID, IsActive FROM Users WHERE UserName = @UserName AND Password = @Password";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            UserID = Convert.ToInt32(reader["UserID"]);
                            PersonID = Convert.ToInt32(reader["PersonID"]);
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsFound = false;
            }
            return IsFound;
        }

        public static bool GetUserByUserID(int UserID, ref string Password, ref string UserName, ref int PersonID, ref bool IsActive)
        {

            bool IsFound = false;

            string query = "SELECT UserName, PersonID, IsActive, Password FROM Users WHERE UserID = @UserID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            UserName = reader["UserName"].ToString();
                            PersonID = Convert.ToInt32(reader["PersonID"]);
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                            Password = reader["Password"].ToString();
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsFound = false;
            }
            return IsFound;
        }

        public static bool GetUserByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            string query = "SELECT UserID, UserName, Password, IsActive FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            UserID = Convert.ToInt32(reader["UserID"]);
                            UserName = reader["UserName"].ToString();
                            Password = reader["Password"].ToString();
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsFound = false;
            }
            return IsFound;
        }
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            string query = @"select
                    UserID, 
                    u.PersonID,
                    FullName = p.FirstName + ' ' + p.LastName,
                    u.UserName,
                    IsActive 
                    from Users u
                            join People p on u.PersonID = p.PersonID;";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // log the error    
            }

            return dt;
        }


        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            string query = @"INSERT INTO Users 
                     (PersonID, UserName, Password, IsActive) 
                     VALUES 
                     (@PersonID, @UserName, @Password, @IsActive); 
                     SELECT SCOPE_IDENTITY();";

            int UserID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        UserID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return UserID; // هترجع -1 لو حصلت مشكلة
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            string query = @"UPDATE Users 
                     SET PersonID = @PersonID, 
                         UserName = @UserName, 
                         Password = @Password, 
                         IsActive = @IsActive 
                     WHERE UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }

        public static bool DeleteUser(int UserID)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }

        public static bool IsUserExists(string UserName)
        {
            string query = "SELECT TOP 1 1 FROM Users WHERE UserName = @UserName";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    conn.Open();

                    object result = command.ExecuteScalar();

                    return result != null;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }

        public static bool IsUserExists(int UserID)
        {
            string query = "SELECT TOP 1 1 FROM Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    conn.Open();

                    object result = command.ExecuteScalar();

                    return result != null;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }

        public static bool IsUserExistsForPersonID(int PersonID)
        {
            string query = "SELECT TOP 1 1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    conn.Open();

                    object result = command.ExecuteScalar();

                    return result != null;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }


        public static bool ChangePassword(int UserID, string NewPassword)
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@NewPassword", NewPassword);
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }

        }
    }
}
