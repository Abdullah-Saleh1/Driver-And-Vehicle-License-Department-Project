using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {

        enum enMode { AddNew, Update}; 
        private enMode _Mode; 

        public int LocalDrivingLicenseApplicationID;
        public int LicenseClassID;

        private clsLicenseClass _LicenseClassInfo;
        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                if (this._LicenseClassInfo == null) _LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
                return _LicenseClassInfo;
            }
            set { _LicenseClassInfo = value; }
        }

        public string PersonFullName
        {
            get { return base.PersonInfo.FullName; }
        }

        public clsLocalDrivingLicenseApplication() : base()
        {
            _Mode = enMode.AddNew;

            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
        }
        private clsLocalDrivingLicenseApplication(
            int LocalDrivingLicenseApplicationID,
            int ApplicationID,
            int ApplicantPersonID,
            DateTime ApplicationDate,
            int ApplicationTypeID,
            enStatus ApplicationStatus,
            DateTime LastStatusDate,
            float PaidFees,
            int CreatedByUserID,
            int LicenseClassID) : base (ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                                      ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
        {
            _Mode = enMode.Update;

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.LicenseClassID = LicenseClassID; 
        }


        public static DataTable GetAllLocalApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications(); 
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(
                this.ApplicationID, this.LicenseClassID);


            return this.LocalDrivingLicenseApplicationID != -1;
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID); 
        }

        public bool Delete()
        {
            if (LocalDrivingLicenseApplicationID == -1 || ApplicationID == -1) return false; 

            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplication = false;

            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);

            if (!IsLocalDrivingApplicationDeleted) return false;

            IsBaseApplication = base.DeleteApplication();

            return IsBaseApplication; 
        }


        public new bool Save()
        {
            base._Mode = (clsApplication.enMode)_Mode;

            // Here we save the clsApplication for adding or updating
            if (!base.Save()) return false; 

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        _Mode = enMode.Update;
                        return true; 
                    }
                    clsApplication.DeleteApplication(ApplicationID); // Roll back 
                    return false; 
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
                default:
                    return false; 
            }
        }

        public static clsLocalDrivingLicenseApplication FindByLocalDrivingAppLicenseID(int LocalDrivingLicenseApplicationID)
        {

            int ApplicationID = -1, LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(
                    LocalDrivingLicenseApplicationID,
                    ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate,
                    Application.ApplicationTypeID,
                    Application.Status,
                    Application.LastStatusDate,
                    Application.PaidFees,
                    Application.CreatedByUserID,
                    LicenseClassID); 
            }
            return null;
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {

            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1; 

            bool IsFound =  clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID); 

            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(
                    LocalDrivingLicenseApplicationID,
                    ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate,
                    Application.ApplicationTypeID,
                    Application.Status,
                    Application.LastStatusDate,
                    Application.PaidFees,
                    Application.CreatedByUserID,
                    LicenseClassID);
            }
            return null;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID); 
        }

        public int GetPassedTestCount()
        {
            return -1; // Placeholder for actual implementation
        }
    }
}
