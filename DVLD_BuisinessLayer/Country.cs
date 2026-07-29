using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisinessLayer
{
    public class clsCountry
    {

        int CountryID { get; }
        string CountryName { get; }

        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName; 
        }


        static public DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries(); 
        }
    }
}
