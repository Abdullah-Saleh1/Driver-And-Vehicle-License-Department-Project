using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationData
    {

        public static bool GetApplicationInfoByID(int ApplicationID, ref int PersonID, ref DateTime Date, ref int TypeID, ref byte Status, ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
        {
            bool IsFound = false;

            string query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            PersonID = Convert.ToInt32(reader["ApplicantPersonID"]);
                            Date = Convert.ToDateTime(reader["ApplicationDate"]);
                            TypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                            Status = Convert.ToByte(reader["ApplicationStatus"]);
                            LastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
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
                // log the error
                IsFound = false;
            }

            return IsFound;
        }
        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM Applications ORDER BY ApplicationDate DESC;";

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
                return null; 
            }

            return dt;
        }
        public static int AddNewApplication(int PersonID, DateTime Date, int TypeID, byte Status, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            string query = @"INSERT INTO Applications 
             (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID) 
             VALUES 
             (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID); 
             SELECT SCOPE_IDENTITY();";

            int ApplicationID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", Date);
                    command.Parameters.AddWithValue("@ApplicationTypeID", TypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", Status);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log Error ex
                return -1;
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int PersonID, DateTime Date, int TypeID, byte Status, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Applications  
                     SET ApplicantPersonID = @ApplicantPersonID,
                         ApplicationDate = @ApplicationDate,
                         ApplicationTypeID = @ApplicationTypeID,
                         ApplicationStatus = @ApplicationStatus,
                         LastStatusDate = @LastStatusDate,
                         PaidFees = @PaidFees,
                         CreatedByUserID = @CreatedByUserID
                     WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", Date);
                    command.Parameters.AddWithValue("@ApplicationTypeID", TypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", Status);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log Error ex
                return false;
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT found = 1 WHERE EXISTS (SELECT 1 FROM Applications WHERE ApplicationID = @ApplicationID)";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        
        public static bool DoesPersonHasActiveApplicaiton(int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplication(PersonID, ApplicationTypeID) != -1); 
        }
        public static int GetActiveApplication(int ApplicantPersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;
            string query = @"select top 1 ApplicationID from Applications
	        where ApplicantPersonID = @ApplicantPersonID
            and ApplicationTypeID = @ApplicationTypeID
            and ApplicationStatus = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ActiveApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                return ActiveApplicationID;
            }

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;
            string query = @"select top 1 l.ApplicationID from Applications a
	        join LocalDrivingLicenseApplications l on a.ApplicationID = l.ApplicationID
	        where ApplicantPersonID = @ApplicantPersonID
            and ApplicationTypeID = @ApplicationTypeID
            and l.LicenseClassID = @LicenseClassID 
            and ApplicationStatus = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ActiveApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                return ActiveApplicationID;
            }

            return ActiveApplicationID;
        }


        public static bool UpdateStatus(int ApplicationID, int NewStatus) 
        {
            int rowsAffected = 0;

            string query = @"UPDATE Applications  
                     SET 
                         ApplicationStatus = @ApplicationStatus,
                         LastStatusDate = @LastStatusDate
                     WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                    conn.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log Error ex
                return false;
            }

            return (rowsAffected > 0);
        }
    }
}
