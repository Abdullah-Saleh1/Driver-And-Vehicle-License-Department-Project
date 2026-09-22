using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsLicenseClass
    {

        public int ID;
        public string Name;
        public string description;
        public int MinimumAllowdAge;
        public int DefaultValidityLength;
        public float Fees;


        public static bool IsAgeAllowedForClass(DateTime DateOfBirth, int licenseClassID)
        {
            int PersonAge = DateTime.Now.Year - DateOfBirth.Year;

            // Handle Edge Case 
            if (DateOfBirth.Date > DateTime.Now.AddYears(-PersonAge)) --PersonAge; 

            return PersonAge >= clsLicenseClassData.GetMinAllowedAgeByClassID(licenseClassID);
        }


        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }
    }
}
