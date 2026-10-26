using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsLicenseClass
    {

        enum enMode { Add, Update}
        enMode _Mode; 

        public int LicenseClassID;
        public string ClassName;
        public string ClassDescription;
        public byte MinimumAllowedAge;
        public byte DefaultValidityLength;
        public float ClassFees;

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.ClassFees = 0;

            _Mode = enMode.Add; 
        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;

            _Mode = enMode.Update; 
        }

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


        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = "", ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }

            return null;
        }

        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1; 
            string ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }

            return null;
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:

                    if (_AddNewLicenseClass())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;


                case enMode.Update:
                    return _UpdateLicenseClass();

                default:
                    return false;
            }
        }

        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees); 
        }

        private bool _AddNewLicenseClass()
        {
            LicenseClassID = clsLicenseClassData.AddNewLicenseClass(ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);

            return LicenseClassID != -1; 
        }
    }
}
