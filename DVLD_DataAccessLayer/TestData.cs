using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsTestData
    {

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Tests ORDER BY TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return dt;
        }

        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                            TestResult = Convert.ToBoolean(reader["TestResult"]);
                            Notes = reader["Notes"] == DBNull.Value ? "" : reader["Notes"].ToString();
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
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
                // log the error    
                IsFound = false;
            }

            return IsFound;
        }
        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            string query = @"INSERT INTO Tests 
                     (TestAppointmentID, TestResult, Notes, CreatedByUserID) 
                     VALUES 
                     (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID); 

                     update TestAppointments 
                     set IsLocked = 1 where TestAppointmentID = @TestAppointmentID; 

                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);

                    // معالجة الـ Notes لو فاضية عشان متبعتش كـ null يضرب داتابيز
                    if (string.IsNullOrEmpty(Notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", Notes);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        TestID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                TestID = -1;
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Tests 
                     SET TestAppointmentID = @TestAppointmentID, 
                         TestResult = @TestResult, 
                         Notes = @Notes, 
                         CreatedByUserID = @CreatedByUserID 
                     WHERE TestID = @TestID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);

                    if (string.IsNullOrEmpty(Notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", Notes);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    conn.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                return false;
            }

            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalAppID)
        {
            byte PassedTestCount = 0;

            string query = @"SELECT COUNT(ta.TestTypeID) 
                     FROM TestAppointments ta         
                     JOIN Tests t ON ta.TestAppointmentID = t.TestAppointmentID        
                     WHERE ta.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                       AND t.TestResult = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalAppID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && byte.TryParse(result.ToString(), out byte count))
                        {
                            PassedTestCount = count;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine("Error: " + ex.Message);
                        PassedTestCount = 0;
                    }
                }
            }

            return PassedTestCount;
        }

        
    }
}
