using DVLD_DataAccessLayer;
using System;

namespace DVLD_BuisinessLayer
{
    public static class clsDashboard
    {
        public static bool GetDashboardStats(ref int TotalDrivers, ref int ActiveLicenses, ref int PendingApps, ref int DetainedLicenses)
        {
            return clsDashboardData.GetDashboardStats(ref TotalDrivers, ref ActiveLicenses, ref PendingApps, ref DetainedLicenses); 
        }
    }
}
