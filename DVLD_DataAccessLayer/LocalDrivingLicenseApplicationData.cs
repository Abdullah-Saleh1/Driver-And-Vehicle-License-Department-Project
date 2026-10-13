using ContactsDataAccessLayer;
using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public static class clsLocalDrivingLicenseApplicationData
    {

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            string query = @"select 
                LocalDrivingLicenseApplicationID as [LDLAppID], 
                c.ClassName, 
                p.NationalNo, 
                p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName, '') + ' ' + p.LastName as FullName, 
                a.ApplicationDate, 
                v.PassedTests, 
                CASE a.ApplicationStatus
                    WHEN 1 THEN 'New'
                    WHEN 2 THEN 'Cancelled'
                    WHEN 3 THEN 'Completed'
                    ELSE 'Unknown'
                END AS Status
                from (

	                select l.LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID,
                    Count(t.TestID) AS PassedTests 
                    from LocalDrivingLicenseApplications l 
	                left join TestAppointments ta on l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
	                left join Tests t on ta.TestAppointmentID = t.TestAppointmentID and TestResult = 1
	                group by l.LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID
                ) v
                join Applications a on a.ApplicationID = v.ApplicationID
                join People p on a.ApplicantPersonID = p.PersonID
                join LicenseClasses c on v.LicenseClassID = c.LicenseClassID
                order by [LDLAppID]; ";

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
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                            LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
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
        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID, ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

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

                            LocalDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                            LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
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
        
        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        { 
            string query = @"INSERT INTO LocalDrivingLicenseApplications 
             (ApplicationID, LicenseClassID) 
             VALUES 
             (@ApplicationID, @LicenseClassID); 
             SELECT SCOPE_IDENTITY();";

            int LocalApplicationID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LocalApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log Error ex
                return -1;
            }

            return LocalApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            string query = @"UPDATE LocalDrivingLicenseApplications 
             SET ApplicationID = @ApplicationID, 
                 LicenseClassID = @LicenseClassID 
             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID) 
        {
            string query = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        // There are functions need to be implemented 
        // https://programmingadvices.com/courses/2139586/lectures/49927929
    }
}
