using DVLD_DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Policy;

namespace DVLD_BuisinessLayer
{
    public class clsApplicationType
    {
        enum enMode { AddNew, Update };
        enMode _Mode;

        public int ID;
        public string Title;
        public float Fees; 


        // Add
        public clsApplicationType() {
            _Mode = enMode.AddNew; 
            ID = -1;
            Title = "";
            Fees = 0; 
        }

        // Update 
        private clsApplicationType(int ID, string Title, float Fees)
        {
            _Mode = enMode.Update; 
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees; 
        }


        static public DataTable GetAllAplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes(); 
        }

        private bool _AddApplicationType()
        {
            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);
            return this.ID != -1; 
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(ID, Title, Fees); 
        }

        static public clsApplicationType FindApplicationTypeByID(int ApplicationTypeID)
        {
            string Title = "";
            float Fees = 0;
            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref Title, ref Fees))
            {
                return new clsApplicationType(ApplicationTypeID, Title, Fees);
            }
            return null;
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddApplicationType(); 
                case enMode.Update:
                    return _UpdateApplicationType(); 
                default:
                    return false; 
            }
        }
    }
}
