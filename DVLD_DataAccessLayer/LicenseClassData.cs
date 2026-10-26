using ContactsDataAccessLayer;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{

    public static class clsLicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            string query = "SELECT LicenseClassID, ClassName FROM LicenseClasses";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Catch or log exception
                    }
                }
            }

            return dt;
        }

        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsFound = false;
            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            ClassName = reader["ClassName"].ToString();
                            ClassDescription = reader["ClassDescription"] == DBNull.Value ? "" : reader["ClassDescription"].ToString();
                            MinimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]);
                            DefaultValidityLength = Convert.ToByte(reader["DefaultValidityLength"]);
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
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
                // Log the error
                IsFound = false;
            }

            return IsFound;
        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsFound = false;
            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ClassName", ClassName);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                            ClassDescription = reader["ClassDescription"] == DBNull.Value ? "" : reader["ClassDescription"].ToString();
                            MinimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]);
                            DefaultValidityLength = Convert.ToByte(reader["DefaultValidityLength"]);
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
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
                // Log the error
                IsFound = false;
            }

            return IsFound;
        }

        public static int GetMinAllowedAgeByClassID(int LicenseClassID)
        {
            int Age = 0;

            string query = @"SELECT MinimumAllowedAge 
                         FROM LicenseClasses 
                         WHERE LicenseClassID = @LicenseClassID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int age))
                        {
                            Age = age;
                        }
                    }
                    catch (Exception ex)
                    {
                        Age = 0;
                    }
                }
            }

            return Age;
        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            string query = @"INSERT INTO LicenseClasses 
                             (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees) 
                             VALUES 
                             (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClassName", ClassName);

                        if (string.IsNullOrEmpty(ClassDescription))
                            command.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);

                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);

                        connection.Open();
                        object insertedID = command.ExecuteScalar();

                        if (insertedID != null && int.TryParse(insertedID.ToString(), out int insertedId))
                        {
                            LicenseClassID = insertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                LicenseClassID = -1;
            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {

            string query = @"UPDATE LicenseClasses 
                             SET ClassName = @ClassName, 
                                 ClassDescription = @ClassDescription, 
                                 MinimumAllowedAge = @MinimumAllowedAge, 
                                 DefaultValidityLength = @DefaultValidityLength, 
                                 ClassFees = @ClassFees 
                             WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@ClassName", ClassName);

                        if (string.IsNullOrEmpty(ClassDescription))
                            command.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);

                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        return (rowsAffected > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return false; 
            }
        }

    }
}
