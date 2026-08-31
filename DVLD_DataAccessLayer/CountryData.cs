using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsCountryDataAccess
    {
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string query = @"select * from Countries order by CountryName";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error
            }
            return dt;
        }

        public static bool GetCountryByID(int CountryID, ref string CountryName)
        {
            string query = @"select * from Countries where CountryID = @CountryID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            CountryName = reader["CountryName"].ToString();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error
            }
            return false;
        }


        static public bool GetCountryByName(string CountryName, ref int CountryID)
        {
            CountryID = -1;
            string query = @"select * from Countries where CountryName = @CountryName";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read(); 
                            CountryID = Convert.ToInt32(reader["CountryID"]);
                            return true; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error
            }
            return false;
        }
    }
}

