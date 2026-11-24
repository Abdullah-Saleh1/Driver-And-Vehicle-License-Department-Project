using System.Configuration;

namespace ContactsDataAccessLayer
{
    static class clsDataAccessSettings
    {


        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DVLDConnection"]?.ConnectionString;
    }
}
