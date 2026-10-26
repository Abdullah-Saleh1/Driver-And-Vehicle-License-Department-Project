using DVLD_DataAccessLayer;
using System;
using System.ComponentModel;
using System.Data;
using static DVLD_BuisinessLayer.clsApplication;
using static DVLD_BuisinessLayer.clsLicense;

namespace DVLD_BuisinessLayer
{
    public class clsInternationalLicense
    {

        enum enMode { Add, Update }
        enMode _Mode; 

        public int ID;
        public int ApplicationID;
        public int DriverID;
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
        public int IssuedUsingLocalLicenseID;
        public DateTime IssueDate;
        public DateTime ExpirationDate;
        public bool IsActive;
        public int CreatedByUserID;

        private clsInternationalLicense(int InternationalID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
    bool IsActive, int CreatedByUserID)
        {
            _Mode = enMode.Update;
            this.ID = InternationalID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID; 
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = true;
            this.CreatedByUserID = CreatedByUserID;
        }

        public clsInternationalLicense()
        {
            _Mode = enMode.Add;

            this.ID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            this.CreatedByUserID = -1;
        }

        public static bool IsInternationalLicenseExistsByLocalLicense(int LocalLicenseID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistsByLocalLicense(LocalLicenseID);
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses(); 
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            bool IsFound = false;

            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            IsFound = clsInternationalLicenseData.GetLicenseInfoByID(
                InternationalLicenseID,
                ref ApplicationID,
                ref DriverID,
                ref IssuedUsingLocalLicenseID,
                ref IssueDate,
                ref ExpirationDate,
                ref IsActive,
                ref CreatedByUserID
            );

            if (IsFound)
            {
                return new clsInternationalLicense(
                    InternationalLicenseID,
                    ApplicationID,
                    DriverID,
                    IssuedUsingLocalLicenseID,
                    IssueDate,
                    ExpirationDate,
                    IsActive,
                    CreatedByUserID
                );
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }


        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(ID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
        }

        private bool _AddNewInternationalLicense()
        {
            ID = clsInternationalLicenseData.AddNewInternationalLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);

            return ID != -1;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:

                    if (_AddNewInternationalLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;


                case enMode.Update:
                    return _UpdateInternationalLicense();

                default:
                    return false;
            }
        }


        public static bool DeactivateLicense(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.DeactivateLicense(InternationalLicenseID);
        }

        public bool DeactivateLicense()
        {
            return clsInternationalLicenseData.DeactivateLicense(ID);
        }

        public bool IsExpired()
        {
            // ExpirationDate is an old date 
            return this.ExpirationDate < DateTime.Now;
        }

    }
}
