using System;
using System.Data;
using DVLD_DataAccessLayer; 

namespace DVLD_BuisinessLayer
{
    public class clsUser
    {

        public enum enIsActive { Active = 1, InActive = 0 };
        public int UserID { get; private set; }

        private int _PersonID; 
        public int PersonID 
        { 
            get { return _PersonID; }
            set
                {
                    _PersonID = value;
                    PersonInfo = clsPerson.Find(_PersonID); 
                }
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsActive { get; set; }

        public clsPerson PersonInfo { get; set; }

        

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = ""; 
            this.IsActive = (int)enIsActive.Active;

            this.PersonInfo = new clsPerson(); 
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, int IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            this.PersonInfo = clsPerson.Find(PersonID);  
        }


        static public clsUser Find(string UserName, string Password) 
        {
            int UserID = -1, PersonID = -1, IsActive = -1;

            if (clsUserDataAccess.GetUser(UserName, Password, ref UserID, ref PersonID, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null; 
        }


        static public DataTable GetAllUsers()
        {
            return clsUserDataAccess.GetAllUsers(); 
        }

        static public bool Delete(int UserID) 
        {
            return clsUserDataAccess.DeleteUser(UserID);
        }


    }
}
