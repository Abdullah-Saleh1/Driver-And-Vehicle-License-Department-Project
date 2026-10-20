using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsTestAppointmentData
    {

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            // Query: 
            //select TestAppointmentID, ta.LocalDrivingLicenseApplicationID, tt.TestTypeTitle, lc.ClassName, ta.AppointmentDate, ta.PaidFees,
            //FullName = p.FirstName + ' ' + p.LastName, IsLocked from TestAppointments ta
            //join LocalDrivingLicenseApplications l on ta.LocalDrivingLicenseApplicationID = l.LocalDrivingLicenseApplicationID
            //join TestTypes tt on ta.TestTypeID = tt.TestTypeID
            //join LicenseClasses lc on l.LicenseClassID = lc.LicenseClassID
            //join Applications a on l.ApplicationID = a.ApplicationID
            //join People p on a.ApplicantPersonID = p.PersonID;

            string query = @"select * from TestAppointments_View";

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


        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalAppID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            string query = @"select ta.TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments ta
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID"; 

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalAppID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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


        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, int? RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            string query = @"INSERT INTO TestAppointments 
                     (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID) 
                     VALUES 
                     (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, 0, @RetakeTestApplicationID); 
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    // RetakeTestApplicationID Check
                    if (RetakeTestApplicationID.HasValue)
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID.Value);
                    else
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        TestAppointmentID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                TestAppointmentID = -1;
            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int? RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE TestAppointments 
                     SET TestTypeID = @TestTypeID, 
                         LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID, 
                         AppointmentDate = @AppointmentDate, 
                         PaidFees = @PaidFees, 
                         CreatedByUserID = @CreatedByUserID, 
                         IsLocked = @IsLocked, 
                         RetakeTestApplicationID = @RetakeTestApplicationID 
                     WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsLocked", IsLocked);

                    // RetakeTestApplicationID Check (تتعامل مع الـ Nullable بشكل آمن)
                    if (RetakeTestApplicationID.HasValue)
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID.Value);
                    else
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

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

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int? RetakeTestApplicationID)
        {
            bool IsFound = false;
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                            TestTypeID = Convert.ToInt32(reader["TestTypeID"]);
                            LocalDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            IsLocked = Convert.ToBoolean(reader["IsLocked"]);

                            if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                RetakeTestApplicationID = null;
                            else
                                RetakeTestApplicationID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
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


        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            string query = @"select TestID from Tests where TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
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
    }
}
