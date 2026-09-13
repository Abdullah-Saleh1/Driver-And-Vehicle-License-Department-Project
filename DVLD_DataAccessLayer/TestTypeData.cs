using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsTestTypeData
    {

        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            string query = @"select * from TestTypes";
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

        public static bool GetTestTypeInfoByID(int TestTypeID, ref string Title, ref string Description, ref float Fees)
        {

            bool IsFound = false;

            string query = "SELECT TestTypeTitle, TestTypeDescription, TestTypeFees FROM TestTypes WHERE TestTypeID = @TestTypeID order by TestTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            Title = reader["TestTypeTitle"].ToString();
                            Description = reader["TestTypeDescription"].ToString();
                            Fees = Convert.ToSingle(reader["TestTypeFees"]);
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

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            string query = @"INSERT INTO TestTypes 
                     (TestTypeTitle, TestTypeDescription, TestFees) 
                     VALUES 
                     (@Title, @Description, @Fees); 
                     SELECT SCOPE_IDENTITY();";

            int TestTypeID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        TestTypeID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return TestTypeID; // هترجع -1 لو حصلت مشكلة
            }

            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            string query = @"UPDATE TestTypes 
                     SET TestTypeTitle = @Title, 
                         TestTypeDescription = @Description, 
                         TestTypeFees = @Fees 
                     WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Fees", Fees);

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