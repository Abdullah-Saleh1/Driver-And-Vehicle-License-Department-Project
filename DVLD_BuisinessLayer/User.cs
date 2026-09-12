using System;
using System.Configuration;
using System.Data;
using DVLD_DataAccessLayer; 

namespace DVLD_BuisinessLayer
{
    public class clsUser
    {
        enum enMode { AddNew, Update };
        enMode _Mode = enMode.AddNew;
        public int UserID { get; private set; }

        private int _PersonID;

        public int PersonID 
        { 
            get { return _PersonID; }
            set { _PersonID = value;  }
        }

        private clsPerson _PersonInfo; 
        public clsPerson PersonInfo
        {
            get {
                if (_PersonInfo == null) _PersonInfo = clsPerson.Find(_PersonID); 
                return _PersonInfo;
            }
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }



        public clsUser()
        {
            _Mode = enMode.AddNew; 
            
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = ""; 
            this.IsActive = true;
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            _Mode = enMode.Update; 

            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            //this.PersonInfo = clsPerson.Find(PersonID);  
        }


        static public clsUser FindByUsernameAndPassword(string UserName, string Password) 
        {
            int UserID = -1, PersonID = -1;
            bool IsActive = false;

            if (clsUserData.GetUserByUsernameAndPassword(UserName, Password, ref UserID, ref PersonID, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null; 
        }

        static public clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string Password = ""; 
            string UserName = ""; 
            bool IsActive = false;

            if (clsUserData.GetUserByUserID(UserID, ref Password, ref UserName, ref PersonID, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;
        }

        static public clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string Password = "";
            string UserName = "";
            bool IsActive = false;

            if (clsUserData.GetUserByPersonID(PersonID, ref UserID, ref Password, ref UserName, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;
        }


        static public DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers(); 
        }

        static public bool IsUserExists(string UserName)
        {
            return clsUserData.IsUserExists(UserName);
        }
        static public bool IsUserExists(int UserID)
        {
            return clsUserData.IsUserExists(UserID);
        }
        static public bool IsUserExistsForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistsForPersonID(PersonID);
        }

        public bool AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive);

            return this.UserID != -1; 
        }

        public bool UpdateUser()
        {
            return clsUserData.UpdateUser(UserID, PersonID, UserName, Password, IsActive);
        }

        static public bool Delete(int UserID) 
        {

            return clsUserData.DeleteUser(UserID);
        }

        static public bool ChangePassword(int UserID, string NewPassword)
        {
            return clsUserData.ChangePassword(UserID, NewPassword); 
        }

        
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    _Mode = enMode.Update;
                    return AddNewUser(); ;
                case enMode.Update:
                    return UpdateUser();

                default:
                    return false; 
            }
        }

    }
}
