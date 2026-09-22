using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace DVLD_DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationData
    {
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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            string query = @"select 
                LocalDrivingLicenseApplicationID as [LDLAppID], 
                c.ClassName, 
                p.NationalNo, 
                p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName as FullName, 
                a.ApplicationDate, 
                v.PassedTests, 
                CASE a.ApplicationStatus
                    WHEN 1 THEN 'New'
                    WHEN 2 THEN 'Cancelled'
                    WHEN 3 THEN 'Completed'
                    ELSE 'Unknown'
                END AS StatusText
                from (
	                select l.LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID, IsNull(SUM(CAST(t.TestResult AS INT)), 0) AS PassedTests from LocalDrivingLicenseApplications l 
	                left join TestAppointments ta on l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
	                left join Tests t on ta.TestAppointmentID = t.TestAppointmentID
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


        public static int DoesHaveActiveApplication(int ApplicantPersonID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            //string query = @"SELECT found = 1 
            //         WHERE EXISTS (
            //             SELECT 1 
            //             FROM LocalDrivingLicenseApplications l
            //             JOIN Applications a ON l.ApplicationID = a.ApplicationID
            //             WHERE a.ApplicantPersonID = @ApplicantPersonID 
            //               AND l.LicenseClassID = @LicenseClassID 
            //               AND a.ApplicationStatus = 1
            //         );";
            string query = @"select top 1 l.ApplicationID from Applications a
	        join LocalDrivingLicenseApplications l on a.ApplicationID = l.ApplicationID
	        where ApplicantPersonID = @ApplicantPersonID and l.LicenseClassID = @LicenseClassID and ApplicationStatus = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
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
    }
}
