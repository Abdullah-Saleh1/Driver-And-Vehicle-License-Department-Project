using DVLD_DataAccessLayer;
using System;
using System.Data;


namespace DVLD_BuisinessLayer
{
    public class clsApplication
    {
        public enum enStatus {New = 1, Completed = 2, Cancelled = 3}
        public enum enMode { AddNew, Update}
        private enMode _Mode; 
        public int ID { get; set; }
        public int PersonID { get; set; }
        public DateTime Date { get; set; }
        public int TypeID { get; set; }
        public enStatus Status { get; set; } 
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }


        public clsApplication()
        {
            _Mode = enMode.AddNew;

            ID = -1;
            PersonID = -1; 
            Date = DateTime.Now;
            TypeID = -1;
            Status = enStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
        }

        // Update 
        private clsApplication(int ID, int PersonID, DateTime Date, int TypeID,
                                      enStatus Status, DateTime LastStatusDate, float PaidFees, int CreateByUserID)
        {
            _Mode = enMode.Update;

            this.ID = ID;
            this.PersonID = PersonID;
            this.Date = Date;
            this.TypeID = TypeID;
            this.Status = Status;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreateByUserID;
        }


        private bool _AddApplication()
        {
            this.ID = clsApplicationData.AddNewApplication(this.PersonID, this.Date, this.TypeID, (byte)this.Status, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(
                this.ID, this.PersonID, this.Date, this.TypeID,
                (byte)this.Status, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public static clsApplication Find(int ID)
        {
            int PersonID = -1;
            DateTime Date = DateTime.Now;
            int TypeID = -1;
            byte Status = 1;
            DateTime LastStatusDate = DateTime.Now;
            float PaidFees = 0;
            int CreatedByUserID = -1;

            if (clsApplicationData.GetApplicationInfoByID(ID, ref PersonID, ref Date, ref TypeID, ref Status, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplication(
                    ID, PersonID, Date, TypeID,
                    (enStatus)Status, LastStatusDate, PaidFees, CreatedByUserID);
            }

            return null;
        }

        public static bool Delete(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID); 
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddApplication();
                case enMode.Update:
                    return _UpdateApplication();
                default:
                    return false;
            }
        }
    }
}
