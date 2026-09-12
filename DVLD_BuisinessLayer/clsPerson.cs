using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsPerson
    {

        enum enMode { AddNew, Update};
        enMode _Mode = enMode.AddNew; 
        public enum enGender { Male = 0, Female = 1};

        // to prevent changing the PersonID from Presentation Layer
        public int PersonID { get; private set; }
        public string NationalNO { get; set; }
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
        // Composition Relationship with Country Class
        public clsCountry CountryInfo;

        private string _ImagePath; 
        public string ImagePath 
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }


        public clsPerson()
        {
            _Mode = enMode.AddNew; 

            PersonID = -1;
            NationalNO = ""; 
            FirstName = "";
            LastName = "";
            ThirdName = ""; 
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            CountryID = -1;
            ImagePath = "";
        }
        
        private clsPerson(int PersonID, string NationalNO, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
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
            this.CountryInfo = clsCountry.Find(CountryID); 
            this.ImagePath = ImagePath;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(NationalNO, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, CountryID, ImagePath);

            return this.PersonID != -1; 
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, NationalNO, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, CountryID, ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string FirstName = "", LastName = "", SecondName = "", ThirdName = "", NationalNO = "";
            DateTime DateOfBirth = DateTime.MinValue;
            byte Gendor = 0;
            int CountryID = -1;
            string Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPersonData.GetPersonByID(PersonID, ref NationalNO, ref FirstName, ref LastName, ref SecondName, ref ThirdName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNO, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, CountryID, ImagePath);
            }

            return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            int PersonID = -1, CountryID = -1;
            string FirstName = "", LastName = "", SecondName = "", ThirdName = "";
            DateTime DateOfBirth = DateTime.MinValue;
            byte Gendor = 0;
            string Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPersonData.GetPersonByNationalNo(NationalNo, ref PersonID, ref FirstName, ref LastName, ref SecondName, ref ThirdName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, CountryID, ImagePath);
            }

            return null;
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
                    return _UpdatePerson();

                default:
                    return false; 
            }
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool Delete(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool IsPersonExists(int PersonID)
        {
            return clsPersonData.IsPersonExists(PersonID); 
        }

        public static bool IsPersonExists(string NationalNo)
        {
            return clsPersonData.IsPersonExists(NationalNo);
        }
    }
}
