using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsDetainedLicense
    {
        enum enMode { Add, Update }
        enMode _Mode;

        public int DetainID { set; get; }
        public int LicenseID { set; get; }

        private clsLicense _LicenseInfo;
        public clsLicense LicenseInfo
        {
            get
            {
                if (_LicenseInfo == null && LicenseID != -1)
                    _LicenseInfo = clsLicense.Find(LicenseID);
                return _LicenseInfo;
            }
        }

        public DateTime DetainDate { set; get; }
        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }

        private clsUser _CreatedByUserInfo;
        public clsUser CreatedByUserInfo
        {
            get
            {
                if (_CreatedByUserInfo == null && CreatedByUserID != -1)
                    _CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
                return _CreatedByUserInfo;
            }
        }

        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }

        private clsUser _ReleasedByUserInfo;
        public clsUser ReleasedByUserInfo
        {
            get
            {
                if (_ReleasedByUserInfo == null && ReleasedByUserID != -1)
                    _ReleasedByUserInfo = clsUser.FindByUserID(ReleasedByUserID);
                return _ReleasedByUserInfo;
            }
        }

        public int ReleaseApplicationID { set; get; }


        private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            _Mode = enMode.Update;
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
        }

        public clsDetainedLicense()
        {
            _Mode = enMode.Add;

            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
        }


        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public static clsDetainedLicense Find(int DetainID)
        {
            bool IsFound = false;

            int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;

            IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByID(
                DetainID,
                ref LicenseID,
                ref DetainDate,
                ref FineFees,
                ref CreatedByUserID,
                ref IsReleased,
                ref ReleaseDate,
                ref ReleasedByUserID,
                ref ReleaseApplicationID
            );

            if (IsFound)
            {
                return new clsDetainedLicense(
                    DetainID,
                    LicenseID,
                    DetainDate,
                    FineFees,
                    CreatedByUserID,
                    IsReleased,
                    ReleaseDate,
                    ReleasedByUserID,
                    ReleaseApplicationID
                );
            }
            else
            {
                return null;
            }
        }
        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            bool IsFound = false;

            int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;

            IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(
                LicenseID,
                ref DetainID,
                ref DetainDate,
                ref FineFees,
                ref CreatedByUserID,
                ref IsReleased,
                ref ReleaseDate,
                ref ReleasedByUserID,
                ref ReleaseApplicationID
            );

            if (IsFound)
            {
                return new clsDetainedLicense(
                    DetainID,
                    LicenseID,
                    DetainDate,
                    FineFees,
                    CreatedByUserID,
                    IsReleased,
                    ReleaseDate,
                    ReleasedByUserID,
                    ReleaseApplicationID
                );
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }


        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(
                DetainID,
                LicenseID,
                DetainDate,
                FineFees,
                CreatedByUserID
            );
        }

        private bool _AddNewDetainedLicense()
        {
            DetainID = clsDetainedLicenseData.AddNewDetainedLicense(
                LicenseID,
                DetainDate,
                FineFees,
                CreatedByUserID
            );

            return DetainID != -1;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewDetainedLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateDetainedLicense();

                default:
                    return false;
            }
        }

        public bool ReleaseLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(
                this.DetainID,
                ReleasedByUserID,
                ReleaseApplicationID
            );
        }

        public static bool ReleaseLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(
                DetainID,
                ReleasedByUserID,
                ReleaseApplicationID
            );
        }

    }
}
