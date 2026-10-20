using System;
using DVLD_DataAccessLayer;
using System.Data;
using System.Diagnostics;

namespace DVLD_BuisinessLayer
{
    public class clsTestAppointment
    {

        enum enMode { Add, Update};
        private enMode _Mode = enMode.Add; 

        public int AppointmentID;
        public clsTestType.enTestType TestTypeID;
        public int LocalAppID;
        public DateTime Date;
        public float PaidFees;
        public int CreatedByUserID;
        public bool IsLocked;
        public int? RetakeTestAppID;

        private clsApplication _RetakeTestAppInfo; 
        
        public clsApplication RetakeTestAppInfo
        {
            get
            {
                if (RetakeTestAppID.HasValue && _RetakeTestAppInfo == null)
                {
                    _RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestAppID.Value); 
                }

                return _RetakeTestAppInfo; 
            }
        } 


        public int TestID {
            get
            {
                return clsTestAppointmentData.GetTestID(AppointmentID); 
            }
        }


        public clsTestAppointment()
        {
            this.AppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalAppID = -1;
            this.Date = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestAppID = null;

            _Mode = enMode.Add;
        }

        private clsTestAppointment(int appointmentID, clsTestType.enTestType testTypeID, int localAppID, DateTime date, float paidFees, int createdByUserID, bool isLocked, int? retakeTestAppID)
        {
            this.AppointmentID = appointmentID;
            this.TestTypeID = testTypeID;
            this.LocalAppID = localAppID;
            this.Date = date;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.IsLocked = isLocked;
            this.RetakeTestAppID = retakeTestAppID;

            

            _Mode = enMode.Update;
        }

        private bool _AddTestAppointment()
        {
            AppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)TestTypeID, LocalAppID, Date, PaidFees, CreatedByUserID, RetakeTestAppID);
            return AppointmentID != -1; 
        }
        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(AppointmentID, (int)TestTypeID, LocalAppID, Date, PaidFees, CreatedByUserID, IsLocked, RetakeTestAppID); 
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddTestAppointment())
                    {
                        _Mode = enMode.Update; 
                        return true; 
                    }
                    return false; 
                case enMode.Update:
                    return _UpdateTestAppointment(); 
                default:
                    return false; 
            }
        }

        public static clsTestAppointment Find(int AppointmentID)
        {
            int TestTypeID = -1;
            int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int? RetakeTestApplicationID = null;

            bool IsFound = clsTestAppointmentData.GetTestAppointmentInfoByID(
                AppointmentID,
                ref TestTypeID,
                ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate,
                ref PaidFees,
                ref CreatedByUserID,
                ref IsLocked,
                ref RetakeTestApplicationID
            );

            if (IsFound)
            {
                return new clsTestAppointment(
                    AppointmentID,
                    (clsTestType.enTestType)TestTypeID,
                    LocalDrivingLicenseApplicationID,
                    AppointmentDate,
                    PaidFees,
                    CreatedByUserID,
                    IsLocked,
                    RetakeTestApplicationID
                );
            }
            else
                return null; 
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments(); 
        }
        public static DataTable GetLocalApplicationAppointmentsPerTestType(int LocalAppID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalAppID, (int)TestTypeID); 
        }

        public DataTable GetLocalApplicationAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalAppID, (int)TestTypeID);
        }
    }
}
