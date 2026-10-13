using ContactsDataAccessLayer;
using System;
using System.Data.SqlClient;


namespace DVLD_DataAccessLayer
{
    public class clsLicenseData
    {

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
    }
}
