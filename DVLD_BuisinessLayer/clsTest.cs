using DVLD_DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DVLD_BuisinessLayer
{
    public class clsTest
    {

        enum enMode { Add, Update }
        private enMode _Mode; 

        public int ID;
        public int TestAppointmentID;

        private clsTestAppointment _TestAppointmentInfo; 

        public clsTestAppointment TestAppointment
        {

            get
            {
                if (_TestAppointmentInfo == null && TestAppointmentID != -1)
                    _TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
                return _TestAppointmentInfo; 
            }

            private set { _TestAppointmentInfo = value; }
        }

        public bool TestResult;
        public string Notes;
        public int CreatedByUserID; 


        public clsTest()
        {
            _Mode = enMode.Add; 

            ID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1; 
        }

        private clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            _Mode = enMode.Update;

            this.ID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
        }


        private bool _AddNewTest()
        {
            this.ID = clsTestData.AddNewTest(TestAppointmentID, TestResult, Notes, CreatedByUserID);
            return ID != -1; 
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(ID, TestAppointmentID, TestResult, Notes, CreatedByUserID); 
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewTest())
                    {
                        _Mode = enMode.Update;

                        return true; 
                    }
                    return false;   
                case enMode.Update:
                    return _UpdateTest(); 
                default:
                    return false;   
            }
        }


        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests(); 
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestData.GetTestInfoByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public static byte GetPassedTestCount(int LocalAppID)
        {
            return clsTestData.GetPassedTestCount(LocalAppID); 
        }

        public static bool PassedAllTests(int LocalAppID)
        {
            return GetPassedTestCount(LocalAppID) == 3; 
        }
    }
}
