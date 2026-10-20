using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace DVLD_BuisinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {

        enum enMode { AddNew, Update}; 
        private enMode _Mode; 

        public int LocalAppID;
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

            this.LocalAppID = -1;
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

            this.LocalAppID = LocalDrivingLicenseApplicationID;
            this.LicenseClassID = LicenseClassID; 
        }


        public static DataTable GetAllLocalApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications(); 
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalAppID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(
                this.ApplicationID, this.LicenseClassID);


            return this.LocalAppID != -1;
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(LocalAppID, ApplicationID, LicenseClassID); 
        }

        public bool Delete()
        {
            if (LocalAppID == -1 || ApplicationID == -1) return false; 

            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplication = false;

            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(LocalAppID);

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

        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalAppID);    
        }

        public static bool DoesPassTestType(int LocalAppID, int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalAppID, TestTypeID);
        }

        public int GetActiveLicneseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(ApplicantPersonID, LicenseClassID); 
        }
        public bool IsLicenseIssued()
        {
            return GetActiveLicneseID() != -1; 
        }

        public static int GetToalTrailsPerTest(int LocalAppID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalAppID, (int)TestTypeID);
        }

        public int GetToalTrailsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalAppID, (int)TestTypeID);
        }

        public static bool DoesPassTestType(int LocalAppID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalAppID, (int)TestTypeID);
        }
        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalAppID, (int)TestTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalAppID, clsTestType.enTestType TestTypeID)
        {

            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalAppID, (int)TestTypeID);

        }

        public bool DoesAttendTestType(clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalAppID, (int)TestType); 
        }
    }
}
