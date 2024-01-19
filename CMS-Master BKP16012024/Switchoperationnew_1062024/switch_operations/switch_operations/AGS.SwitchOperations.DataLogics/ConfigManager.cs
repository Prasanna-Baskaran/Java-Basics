using AGS.Utilities;
using System.Data.SqlClient;


namespace AGS.Configuration
{
	/// <summary>
	/// Summary description for ConfigManager.
	/// </summary>
	public static class ConfigManager {

		public static SqlConnection GetRBSQLDBConnection 
		{
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDB"].ConnectionString.DoubleDecodeFromBase64()); }
		}

        public static SqlConnection GetRBSQLDBOfficeConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBOffice"].ConnectionString.DoubleDecodeFromBase64()); }
        }

        public static SqlConnection GetRBSQLDBOLAPConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBOLAP"].ConnectionString.DoubleDecodeFromBase64()); }
        }

        public static SqlConnection GetRBSQLDBHSMConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBHSM"].ConnectionString.DoubleDecodeFromBase64()); }
        }

	}
}
