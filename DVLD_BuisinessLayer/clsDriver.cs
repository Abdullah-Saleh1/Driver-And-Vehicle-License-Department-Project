using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsDriver
    {
        private enum enMode { Add, Update};
        private enMode _Mode; 

        public int ID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; } // Can be Initialized in the constructor only

        private clsPerson _PersonInfo; 

        public clsPerson PersonInfo
        {
            get
            {
                if (_PersonInfo == null && PersonID != -1)
                    _PersonInfo = clsPerson.Find(PersonID);
                return _PersonInfo; 
            }
        }



        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            ID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate; 

            _Mode = enMode.Update; 
        }

        public clsDriver()
        {
            ID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now; 
            _Mode = enMode.Add; 
        }


        private bool _AddNewDriver()
        {
            this.ID = clsDriverData.AddNewDriver(PersonID, CreatedByUserID);
            return ID != -1;
        }

        private bool _UpdateDriver()
        {
            return clsDriverData.UpdateDriver(ID, PersonID, CreatedByUserID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewDriver())
                    {
                        _Mode = enMode.Update;

                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateDriver();
                default:
                    return false;
            }
        }

        public static clsDriver FindByDriverID(int DriverID)
        {

            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now; 

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDriver FindByPersonID(int PersonID)
        {

            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now; 

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }


        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers(); 
        }

        // Hint: GetLicenses, GetInternationalLicenses.  static methods

    }
}
