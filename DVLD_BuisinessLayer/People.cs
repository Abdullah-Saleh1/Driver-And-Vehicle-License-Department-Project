using DVLD_DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DVLD_BuisinessLayer
{
    public class clsPeople
    {

        enum enMode { AddNew, Update};
        enMode _Mode; 
        public enum enGender { Male = 0, Female = 1};

        // to prevent changing the PersonID from Presentation Layer
        public int PersonID { get; private set; }
        public string NationalNO { get; private set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }

        public enGender GenderText
        {
            get
            {
                return Gender == 0 ? enGender.Male : enGender.Female; 
            }
        }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CountryID { get; set; }
        public string ImagePath { get; set; }


        public clsPeople()
        {
            _Mode = enMode.AddNew; 

            PersonID = -1;
            NationalNO = ""; 
            FirstName = "";
            LastName = "";
            ThirdName = ""; 
            LastName = "";
            DateOfBirth = DateTime.MinValue;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            CountryID = -1;
            ImagePath = "";
        }
        
        private clsPeople(int PersonID, string NationalNO, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            _Mode = enMode.Update; 

            this.PersonID = PersonID;
            this.NationalNO = NationalNO;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleDataAccess.GetAllPeople(); 
        }

        public static clsPeople GetPersonByID(int PersonID)
        {
            string FirstName = "", LastName = "", SecondName = "", ThirdName = "", NationalNO = "";
            DateTime DateOfBirth = DateTime.MinValue;
            byte Gendor = 0;
            int CountryID = -1; 
            string Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPeopleDataAccess.GetPersonByID(PersonID, ref NationalNO, ref FirstName, ref LastName, ref SecondName, ref ThirdName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
            {
                return new clsPeople(PersonID, NationalNO, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, CountryID, ImagePath);
            }

            return null;
        }

        public static bool Delete(int PersonID)
        {
            return clsPeopleDataAccess.DeletePersonByID(PersonID);
        }


        public static bool IsNationalNoExists(string NationalNo)
        {
            return clsPeopleDataAccess.IsNationalNoExists(NationalNo);
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPeopleDataAccess.AddNewPerson(NationalNO, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, CountryID, ImagePath);

            return this.PersonID != -1; 
        }



        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:

                    if (_AddNewPerson())
                    {
                        _Mode = enMode.Update;
                        return true; 
                    }
                    return false;   

                   
                case enMode.Update:
                    return false;

                default:
                    return false; 
            }
        }
    }
}
