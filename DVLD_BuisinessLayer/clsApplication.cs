using DVLD_DataAccessLayer;
using System;
using System.Data;


namespace DVLD_BuisinessLayer
{
    public class clsApplication
    {
        public enum enStatus {New = 1, Completed = 2, Cancelled = 3}
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enMode { AddNew, Update}
        protected enMode _Mode; 
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public string ApplicantFullName
        {
            get { return clsPerson.Find(ApplicantPersonID).FullName; }
        }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }

        private clsApplicationType _ApplicationTypeInfo;

        public clsApplicationType ApplicationTypeInfo
        {
            get
            {
                if (this._ApplicationTypeInfo == null) _ApplicationTypeInfo = clsApplicationType.FindApplicationTypeByID(ApplicationTypeID);
                return _ApplicationTypeInfo;
            }
            set { _ApplicationTypeInfo = value; }
        }

        public enStatus Status { get; set; } 
        public string StatusText { 
            get
            {
                switch (Status)
                {
                    case enStatus.New:
                        return "New"; 
                    case enStatus.Completed:
                        return "Completed"; 
                    case enStatus.Cancelled:
                        return "Cancelled"; 
                    default:
                        return "Unkown"; 
                }
            }

        }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo; 


        public clsApplication()
        {
            _Mode = enMode.AddNew;

            ApplicationID = -1;
            ApplicantPersonID = -1; 
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            Status = enStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
        }

        // Update 
        // Make it protected to use it in sub-class clsLocalDrivingLicenseApplication
        protected clsApplication(int ID, int PersonID, DateTime Date, int TypeID,
                                      enStatus Status, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            _Mode = enMode.Update;

            this.ApplicationID = ID;
            this.ApplicantPersonID = PersonID;
            this.ApplicationDate = Date;
            this.ApplicationTypeID = TypeID;
            this.Status = Status;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            this.PersonInfo = clsPerson.Find(PersonID); 
            this.ApplicationTypeInfo = clsApplicationType.FindApplicationTypeByID(ApplicationTypeID);
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID); 
        }


        private bool _AddApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID, (byte)this.Status, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(
                this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                (byte)this.Status, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public static clsApplication FindBaseApplication(int ApplicaitonID)
        {
            int PersonID = -1;
            DateTime Date = DateTime.Now;
            int TypeID = -1;
            byte Status = 1;
            DateTime LastStatusDate = DateTime.Now;
            float PaidFees = 0;
            int CreatedByUserID = -1;

            if (clsApplicationData.GetApplicationInfoByID(ApplicaitonID, ref PersonID, ref Date, ref TypeID, ref Status, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplication(
                    ApplicaitonID, PersonID, Date, TypeID,
                    (enStatus)Status, LastStatusDate, PaidFees, CreatedByUserID);
            }

            return null;
        }

        public static bool CancelApplication(int ApplicationID)
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 2);
        }

        public bool Cancel()
        {
            return CancelApplication(ApplicationID);
        }

        public static bool SetApplicationComplete(int ApplicationID)
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 3);
        }

        public bool SetComplete()
        {
            return SetApplicationComplete(ApplicationID); 
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID); 
        }

        public bool DeleteApplication()
        {
            return DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExists(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID); 
        }

        public static bool DoesPersonHasActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHasActiveApplicaiton(PersonID, ApplicationTypeID); 
        }


        public bool DoesPersonHasActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHasActiveApplicaiton(this.ApplicantPersonID, ApplicationTypeID);
        }


        public static int GetActiveApplicationID(int PersonID, enApplicationType NewDrivingLicense)
        {
            return clsApplicationData.GetActiveApplication(PersonID, (int)NewDrivingLicense);
        }

        public int GetActiveApplicationID(enApplicationType NewDrivingLicense)
        {
            return clsApplicationData.GetActiveApplication(this.ApplicantPersonID, (int)NewDrivingLicense);
        }


        public static int GetActiveApplicationIDForLicenseClass(int PersonID, enApplicationType NewDrivingLicense, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)NewDrivingLicense, LicenseClassID);
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false; 
                case enMode.Update:
                    return _UpdateApplication();
                default:
                    return false;
            }
        }
    }
}
