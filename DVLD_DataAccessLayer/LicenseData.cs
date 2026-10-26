using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLD_DataAccessLayer
{
    public class clsLicenseData
    {

        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                            DriverID = Convert.ToInt32(reader["DriverID"]);
                            LicenseClassID = Convert.ToInt32(reader["LicenseClass"]);
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                            ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                            Notes = reader["Notes"] == DBNull.Value ? "" : reader["Notes"].ToString();
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsActive = Convert.ToBoolean(reader["IsActive"]);
                            IssueReason = Convert.ToByte(reader["IssueReason"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
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
                // Log the error
                IsFound = false;
            }

            return IsFound;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @"select l.LicenseID, ApplicationID, lc.ClassName, IssueDate, ExpirationDate, IsActive from Licenses l
                            join LicenseClasses lc on l.LicenseClass = lc.LicenseClassID
                            where DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

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
                    catch (Exception ex)
                    {
                        // Catch or log exception
                    }
                }
            }

            return dt;
        }

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            string query = @"INSERT INTO Licenses 
                             (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID) 
                             VALUES 
                             (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (string.IsNullOrEmpty(Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        connection.Open();
                        object insertedID = command.ExecuteScalar();

                        if (insertedID != null && int.TryParse(insertedID.ToString(), out int insertedId))
                        {
                            LicenseID = insertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                LicenseID = -1;
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            bool IsUpdated = false;

            string query = @"UPDATE Licenses 
                             SET ApplicationID = @ApplicationID, 
                                 DriverID = @DriverID, 
                                 LicenseClassID = @LicenseClassID, 
                                 IssueDate = @IssueDate, 
                                 ExpirationDate = ExpirationDate, 
                                 Notes = @Notes, 
                                 PaidFees = @PaidFees, 
                                 IsActive = @IsActive, 
                                 IssueReason = @IssueReason, 
                                 CreatedByUserID = @CreatedByUserID 
                             WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (string.IsNullOrEmpty(Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        IsUpdated = (rowsAffected > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                IsUpdated = false;
            }

            return IsUpdated;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            string query = @"SELECT TOP 1 l.LicenseID 
                     FROM Drivers d 
                     JOIN Licenses l ON d.DriverID = l.DriverID
                     WHERE d.PersonID = @PersonID
                       AND l.LicenseClass = @LicenseClassID
                       AND l.IsActive = 1;";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    conn.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int foundID))
                    {
                        LicenseID = foundID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log Error
                LicenseID = -1;
            }

            return LicenseID;
        }


        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE Licenses
                    SET IsActive = 0
                    WHERE LicenseID = @LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

    }
}
