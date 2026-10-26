using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsInternationalLicenseData
    {
        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();
            string query = "select 'IntLicenseID' = InternationalLicenseID, ApplicationID, DriverID, 'LocalLicenseID' = IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive from InternationalLicenses; ";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
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
                }
            }
            catch (Exception ex)
            {
                // Log the error
            }

            return dt;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE InternationalLicenses 
                     SET ApplicationID = @ApplicationID, 
                         DriverID = @DriverID, 
                         IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID, 
                         IssueDate = @IssueDate, 
                         ExpirationDate = @ExpirationDate, 
                         IsActive = @IsActive, 
                         CreatedByUserID = @CreatedByUserID 
                     WHERE InternationalLicenseID = @InternationalLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return false;
            }

            return (rowsAffected > 0);
        }

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;
            string query = @"
                     update InternationalLicenses
                     set IsActive = 0
                     where DriverID = @DriverID;


                     INSERT INTO InternationalLicenses 
                     (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID) 
                     VALUES 
                     (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        connection.Open();
                        object insertedID = command.ExecuteScalar();

                        if (insertedID != null && int.TryParse(insertedID.ToString(), out int insertedId))
                        {
                            InternationalLicenseID = insertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                InternationalLicenseID = -1;
            }

            return InternationalLicenseID;
        }

        public static bool DeactivateLicense(int InternationalLicenseID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE InternationalLicenses 
                     SET IsActive = 0 
                     WHERE InternationalLicenseID = @InternationalLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return false;
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT InternationalLicenseID as 'Int License ID',
                     ApplicationID, IssuedUsingLocalLicenseID as 'L License ID',
                     IssueDate, ExpirationDate, IsActive 
                     FROM InternationalLicenses 
                     WHERE DriverID = @DriverID 
                     ORDER BY IssueDate DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
            }

            return dt;
        }

        public static bool GetLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isFound = false;
            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                IsActive = (bool)reader["IsActive"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                            else
                            {
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                isFound = false;
            }

            return isFound;
        }


        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            string query = @"select top 1 InternationalLicenseID from InternationalLicenses
                            where DriverID = @DriverID
                            and GetDate() between IssueDate and ExpirationDate -- Not Expired
                            and IsActive = 1; ";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    conn.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int foundID))
                    {
                        InternationalLicenseID = foundID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log Error
                InternationalLicenseID = -1;
            }

            return InternationalLicenseID;
        }
        public static bool IsInternationalLicenseExistsByLocalLicense(int LocalLicenseID)
        {
            bool isFound = false;
            string query = @"select found = 1 where exists (
                                select * from InternationalLicenses where IssuedUsingLocalLicenseID = @LocalLicenseID and IsActive = 1)";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);
                    conn.Open();

                    object result = command.ExecuteScalar();
                    isFound = (result != null);
                }
            }
            catch (Exception ex)
            {
                // log the error    
                isFound = false;
            }
            return isFound;
        }
    }
}
