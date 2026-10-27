using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsLicense
    {
        enum enMode { Add, Update }
        enMode _Mode;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }


        private clsDriver _DriverInfo;
        public clsDriver DriverInfo
        {
            get
            {
                if (_DriverInfo == null && DriverID != -1)
                    _DriverInfo = clsDriver.FindByDriverID(DriverID);
                return _DriverInfo;
            }
        }
        public int LicenseClassID { set; get; }

        private clsLicenseClass _LicenseClassInfo; 
        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                if (_LicenseClassInfo == null && LicenseClassID != -1)
                    _LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
                return _LicenseClassInfo; 
            }
        }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }


        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }

        public string IssueReasonText
        {
            get { return GetIssueReasonText(this.IssueReason); }
        }

        private string GetIssueReasonText(enIssueReason issueReason)
        {
            switch (issueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Damaged Replacement";
                case enIssueReason.LostReplacement:
                    return "Lost Replacement";
                default:
                    return "Uknown";
            }
        }

        public int CreatedByUserID { set; get; }


        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, 
            string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            _Mode = enMode.Update;
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID; 
        }

        public clsLicense()
        {
            _Mode = enMode.Add;

            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now; 
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.IssueDate = DateTime.Now;
            this.CreatedByUserID = -1;
        }


        public static bool IsLicenseExistsByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1 ; 
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID); 
        }


        public static clsLicense Find(int LicenseID)
        {
            bool IsFound = false;

            int ApplicationID = -1, DriverID = -1, LicenseClassID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 0;

            IsFound = clsLicenseData.GetLicenseInfoByID(
                LicenseID,
                ref ApplicationID,
                ref DriverID,
                ref LicenseClassID,
                ref IssueDate,
                ref ExpirationDate,
                ref Notes,
                ref PaidFees,
                ref IsActive,
                ref IssueReason,
                ref CreatedByUserID
            );

            if (IsFound)
            {
                return new clsLicense(
                    LicenseID,
                    ApplicationID,
                    DriverID,
                    LicenseClassID,
                    IssueDate,
                    ExpirationDate,
                    Notes,
                    PaidFees,
                    IsActive,
                    (enIssueReason)IssueReason,
                    CreatedByUserID
                );
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }


        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (byte)IssueReason, CreatedByUserID);
        }

        private bool _AddNewLicense()
        {
            LicenseID = clsLicenseData.AddNewLicense(ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (byte)IssueReason, CreatedByUserID);

            return LicenseID != -1;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:

                    if (_AddNewLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;


                case enMode.Update:
                    return _UpdateLicense();

                default:
                    return false;
            }       
        }


        public static bool DeactivateLicense(int LicenseID)
        {
            return clsLicenseData.DeactivateLicense(LicenseID); 
        }

        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(LicenseID);
        }

        public bool IsExpired()
        {
            return this.ExpirationDate < DateTime.Now; 
        }


        public int Detain(float FineFees, int CreatedByUserID)
        {

            clsDetainedLicense Detain = new clsDetainedLicense();

            Detain.LicenseID = this.LicenseID; 
            Detain.DetainDate = DateTime.Now;
            Detain.FineFees = FineFees;
            Detain.CreatedByUserID = CreatedByUserID; 


            if (Detain.Save())
            {
                return Detain.DetainID; 
            }

            return -1;
        }

        public bool ReleaseDetainedLicense(int DetainID, int CreatedByUserID, ref int AppID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.Status = clsApplication.enStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense);
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return false; 
            }

            AppID = Application.ApplicationID;

            if (clsDetainedLicense.ReleaseLicense(DetainID, CreatedByUserID, Application.ApplicationID))
            {
                return true; 
            }

            return false; 
        }


        public clsLicense Renew(string Notes, int CreatedByUserID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.Status = clsApplication.enStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.RenewDrivingLicense);
            Application.CreatedByUserID = CreatedByUserID;


            if (!Application.Save())
            {
                return null;
            }

            clsLicense License = new clsLicense();

            License.ApplicationID = Application.ApplicationID;
            License.DriverID = this.DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength); // CHECK IT
            License.Notes = Notes;
            License.PaidFees = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.NewDrivingLicense);
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.Renew;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                DeactivateLicense(); // Old License

                return License; 
            }

            return null; 
        }

        public clsLicense Replace(clsApplication.enApplicationType ApplicationType, int CreatedByUserID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)ApplicationType;
            Application.Status = clsApplication.enStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.GetApplicationFees((int)ApplicationType);
            Application.CreatedByUserID = CreatedByUserID;


            if (!Application.Save())
            {
                return null;
            }

            clsLicense License = new clsLicense();

            License.ApplicationID = Application.ApplicationID;
            License.DriverID = this.DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = this.ExpirationDate;
            License.Notes = this.Notes;
            License.PaidFees = 0; // no fees because it's replacement 
            License.IsActive = true;

            if (ApplicationType == clsApplication.enApplicationType.ReplaceDamagedDrivingLicense)
            {
                License.IssueReason = clsLicense.enIssueReason.DamagedReplacement;
            } else
            {
                License.IssueReason = clsLicense.enIssueReason.LostReplacement;
            }
            
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                DeactivateLicense(); // Old License

                return License;
            }

            return null;
        }

    }
}
