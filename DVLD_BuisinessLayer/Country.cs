using DVLD_DataAccessLayer;
using System.Data;

namespace DVLD_BuisinessLayer
{
    public class clsCountry
    {

        int CountryID { get; }
        string CountryName { get; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = ""; 
        }
        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName; 
        }


        static public DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries(); 
        }

        static public clsCountry Find(int CountryID) 
        {
            string CountryName = ""; 

            if (clsCountryDataAccess.GetCountryByID(CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            
            return null;
        }

        static public clsCountry Find (string CountryName)
        {
            int CountryID = -1; 

            if (clsCountryDataAccess.GetCountryByName(CountryName, ref CountryID))
            {
                return new clsCountry(CountryID, CountryName); 
            }
            return null; 
        }
    }
}
