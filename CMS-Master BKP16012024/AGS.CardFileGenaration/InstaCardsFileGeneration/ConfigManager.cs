using System.Data.SqlClient;
using System;

namespace ReflectionIT.Common.Data.Configuration
{
	/// <summary>
	/// Summary description for ConfigManager.
	/// </summary>
	class ConfigManager {

        public static SqlConnection GetRBSQLDBOLAPConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString); }
        }

        public static string parse(string ConnectionString)
        {
            string sParsedConnectionString = "";

            try
            {
                sParsedConnectionString = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(ConnectionString))));
            }
            catch
            {
                sParsedConnectionString = "";
            }

            return sParsedConnectionString;
        }
	}
}
