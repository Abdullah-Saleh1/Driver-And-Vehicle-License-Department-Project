using DVLD_DataAccessLayer;
using System;

namespace DVLD_BuisinessLayer
{
    public class clsLicense
    {



        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID); 
        }
    }
}
