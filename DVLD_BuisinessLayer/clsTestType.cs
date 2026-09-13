using DVLD_DataAccessLayer;
using System;
using System.Configuration;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsTestType
    {
        enum enMode { AddNew, Update };
        enMode _Mode;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, PracticalTest = 3 };

        public enTestType ID;
        public string Title;
        public string Description; 
        public float Fees;


        // Add
        public clsTestType()
        {
            _Mode = enMode.AddNew;
            ID = enTestType.VisionTest;
            Title = "";
            Description = "";
            Fees = 0;
        }

        // Update 
        private clsTestType(enTestType ID, string Title, string Description, float Fees)
        {
            _Mode = enMode.Update;
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }


        static public DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        private bool _AddTestType()
        {
            this.ID = (enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);
            return this.Title != ""; 
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)ID, Title, Description, Fees);
        }

        static public clsTestType Find(enTestType TestTypeID)
        {
            string Title = "";
            string Description = "";
            float Fees = 0;
            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref Description, ref Fees))
            {
                return new clsTestType(TestTypeID, Title, Description, Fees);
            }
            return null;
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return _AddTestType();
                case enMode.Update:
                    return _UpdateTestType();
                default:
                    return false;
            }
        }
    }
}
