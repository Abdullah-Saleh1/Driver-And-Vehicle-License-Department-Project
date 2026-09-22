using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationData
    {
        public static DataTable GetAllApplications()
        {
            throw new NotImplementedException();
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


        public static bool GetApplicationInfoByID(int iD, ref int personID, ref DateTime date, ref int typeID, ref byte status, ref DateTime lastStatusDate, ref float paidFees, ref int createdByUserID)
        {
            throw new NotImplementedException();
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

        public static bool UpdateApplication(int iD, int personID, DateTime date, int typeID, byte status, DateTime lastStatusDate, float paidFees, int createdByUserID)
        {
            throw new NotImplementedException();
        }
    }
}
