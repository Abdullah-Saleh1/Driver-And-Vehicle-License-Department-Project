using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsDriverData
    {

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            string query = @"select d.DriverID, d.PersonID, p.NationalNo, 
                            FullName = p.FirstName + ' ' + p.LastName, Date = d.CreatedDate, 
                            ActiveLicenses = ISNULL(v.ActiveLicenses, 0)
                            from Drivers d 
                            join People p on d.PersonID = p.PersonID
                            left join (
	                            select DriverID, ActiveLicenses = count(LicenseID) from Licenses
	                            where IsActive = 1
	                            group by DriverID
                            ) v on d.DriverID = v.DriverID
                            order by FullName";

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

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            PersonID = Convert.ToInt32(reader["PersonID"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                IsFound = false;
            }

            return IsFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

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

                            DriverID = Convert.ToInt32(reader["DriverID"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                IsFound = false;
            }

            return IsFound;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate) 
                     VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    conn.Open();
                    object insertedID = command.ExecuteScalar();

                    if (insertedID != null && int.TryParse(insertedID.ToString(), out int ID))
                    {
                        DriverID = ID;
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                DriverID = -1;
            }

            return DriverID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {
            string query = @"UPDATE Drivers 
                     SET PersonID = @PersonID, 
                     CreatedByUserID = @CreatedByUserID 
                     WHERE DriverID = @DriverID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; 
                }
            }
            catch (Exception ex)
            {
                // log error
                return false; 
            }
        }
    }
}
