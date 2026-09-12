using ContactsDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsApplicationTypeData
    {

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            string query = @"select * from ApplicationTypes";
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

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string Title, ref float Fees)
        {

            bool IsFound = false;

            string query = "SELECT ApplicationTypeTitle, ApplicationFees FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            Title = reader["ApplicationTypeTitle"].ToString();
                            Fees = Convert.ToSingle(reader["ApplicationFees"]);
                            //Fees = float.Parse(reader["ApplicationFees"]);
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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            string query = @"INSERT INTO ApplicationTypes 
                     (ApplicationTypeTitle, ApplicationFees) 
                     VALUES 
                     (@Title, @Fees); 
                     SELECT SCOPE_IDENTITY();";

            int ApplicationTypeID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    conn.Open();
                    object InsertedID = command.ExecuteScalar();

                    if (InsertedID != null && int.TryParse(InsertedID.ToString(), out int insertedID))
                    {
                        ApplicationTypeID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Logging error later
                return ApplicationTypeID; // هترجع -1 لو حصلت مشكلة
            }

            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            string query = @"UPDATE ApplicationTypes 
                     SET ApplicationTypeTitle = @Title, 
                         ApplicationFees = @Fees 
                     WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@Title", Title);
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