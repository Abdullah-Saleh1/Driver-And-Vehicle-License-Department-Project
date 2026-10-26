using ContactsDataAccessLayer;
using System;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsDashboardData
    {
        public static bool GetDashboardStats(ref int TotalDrivers, ref int ActiveLicenses, ref int PendingApps, ref int DetainedLicenses)
        {
            bool isFound = false;
            string query = @"SELECT 
                                (SELECT COUNT(*) FROM Drivers) AS TotalDrivers,
                                (SELECT COUNT(*) FROM Licenses WHERE IsActive = 1) AS ActiveLicenses,
                                (SELECT COUNT(*) FROM Applications WHERE ApplicationStatus = 1) AS PendingApps,
                                (SELECT COUNT(*) FROM DetainedLicenses WHERE IsReleased = 0) AS DetainedLicenses;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                TotalDrivers = Convert.ToInt32(reader["TotalDrivers"]);
                                ActiveLicenses = Convert.ToInt32(reader["ActiveLicenses"]);
                                PendingApps = Convert.ToInt32(reader["PendingApps"]);
                                DetainedLicenses = Convert.ToInt32(reader["DetainedLicenses"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                isFound = false;
            }

            return isFound;
        }
    }
}
