using ContactsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsPeopleDataAccess
    {
        public static DataTable GetAllPeople()  
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                    PersonID, 
                    NationalNo, 
                    FirstName, 
                    SecondName, 
                    ThirdName,
                    LastName, 
                    DateOfBirth,
                    Gender,
                    CASE 
                        WHEN Gender = 0 THEN 'Male'
                        ELSE 'Female' 
                    END AS GenderCaption,
                    Address, 
                    Phone, 
                    Email, 
                    NationalityCountryID, 
                    CountryName, 
                    ImagePath 
                 FROM People p
                 JOIN Countries c ON p.NationalityCountryID = c.CountryID
                 ORDER BY FirstName;";

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

        public static bool GetPersonByID(int PersonID, ref string NationalNO, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int CountryID, ref string ImagePath)
        {
            bool IsFound = false; 
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            NationalNO = reader["NationalNO"].ToString();
                            FirstName = reader["FirstName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            ThirdName = reader["ThirdName"].ToString();
                            LastName = reader["LastName"].ToString();
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            Gender = Convert.ToByte(reader["Gender"]);
                            Address = reader["Address"].ToString();
                            Phone = reader["Phone"].ToString();
                            Email = reader["Email"].ToString();
                            CountryID = Convert.ToInt32(reader["NationalityCountryID"]);


                            ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
                        } else
                        {
                            IsFound = false; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsFound = false; 
            }
            return IsFound;
        }

        public static bool DeletePersonByID(int PersonID)
        {
            bool IsDeleted = false;
            string query = "DELETE FROM People WHERE PersonID = @PersonID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    IsDeleted = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsDeleted = false;
            }
            return IsDeleted;
        }

        public static bool FindPersonByID(int PersonID)
        {
            bool IsFound = false;
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                IsFound = false;
            }
            return IsFound;
        }


        public static bool IsNationalNoExists(string NationalNo)
        {
            bool exists = false;
            string query = @"select found = 1 where exists (
                                select * from People Where NationalNo = @NationalNo)";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    
                    conn.Open();
                    object result = command.ExecuteScalar();
                    
                    if (result != null)
                    {
                        exists = Convert.ToInt32(result) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error    
                exists = false;
            }
            return exists;
        }


        // Add 

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, int Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {

            string query = @"INSERT INTO People 
                (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath) 
                VALUES 
                (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath); 
                SELECT SCOPE_IDENTITY();";

            int PersonID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    // Null ThirdName
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    // Null Email
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);

                    command.Parameters.AddWithValue("@NationalityCountryID", CountryID);
                    // Null Image and Convert NullDB.value to an object
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        PersonID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Logging error later
            }
            return PersonID; 
            
        }

        // Edit 

        public static bool UpdatePerson(int PersonID, string NationalNO, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            string query = @"UPDATE People 
                            SET NationalNo = @NationalNo, 
                                FirstName = @FirstName, 
                                SecondName = @SecondName, 
                                ThirdName = @ThirdName, 
                                LastName = @LastName, 
                                DateOfBirth = @DateOfBirth, 
                                Gender = @Gender,
                                Address = @Address, 
                                Phone = @Phone, 
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID, 
                                ImagePath = @ImagePath 
                            WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", NationalNO);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", CountryID);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }

        // Delete

        public static bool DeletePerson(int PersonID)
        {
            string query = "DELETE FROM People WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return false;
            }
        }


    }
}

