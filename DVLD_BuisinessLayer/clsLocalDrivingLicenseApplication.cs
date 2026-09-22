using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsLocalDrivingLicenseApplication
    {

        enum enMode { AddNew, Update}; 
        private enMode _Mode; 
        public int ID;
        public int LicenseClassID;
        public int ApplicationID;

        private clsApplication _ApplicationInfo;

        public clsApplication ApplicationInfo
        {
            get {
                    // Only Find the Application when the ApplicationInfo Object is NULL
                    if (this._ApplicationInfo == null) _ApplicationInfo = clsApplication.Find(ApplicationID);
                    return _ApplicationInfo;
                }
            set { _ApplicationInfo = value; }
        }

        public clsLocalDrivingLicenseApplication()
        {
            _Mode = enMode.AddNew;

            this.ID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;
            this.ApplicationInfo = new clsApplication(); 
        }
        private clsLocalDrivingLicenseApplication(int ID, int ApplicationID, int LicenseClassID)
        {
            _Mode = enMode.Update;

            this.ID = ID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
            // this.ApplicationInfo = clsApplication.Find(ApplicationID);
        }


        public static DataTable GetAllLocalApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications(); 
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.ID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(
                this.ApplicationID, this.LicenseClassID);


            return this.ID != -1;
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(ID, ApplicationID, LicenseClassID); 
        }

        static public int DoesHaveActiveApplication(int PersonID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesHaveActiveApplication(PersonID, LicenseClassID); 
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddNewLocalDrivingLicenseApplication();
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
                default:
                    return false; 
            }
        }
    }
}
