using AGS.Utilities;
using System.Data.SqlClient;


namespace AGS.Configuration
{
    /// <summary>
    /// Summary description for ConfigManager.
    /// </summary>
    public static class ConfigManager
    {

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
            //get { return new SqlConnection("Data Source=13.71.120.14; Initial Catalog=SwitchOperations; user id=uagsrep; password=ags@1234"); }
        }

        public static SqlConnection GetRBSQLDBHSMConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBHSM"].ConnectionString.DoubleDecodeFromBase64()); }
        }

        public static SqlConnection GetRBSQLDBAPIConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBAPI"].ConnectionString.DoubleDecodeFromBase64()); }
        }

        ///*Added for international usage RBL-ATPCM-862* START
        public static SqlConnection GetRBSQLDBTEMPPOConnection
        {
            get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RBDBOLAPRBL"].ConnectionString.DoubleDecodeFromBase64()); }
        }

        ///*Added for international usage RBL-ATPCM-862* END
    }
}
