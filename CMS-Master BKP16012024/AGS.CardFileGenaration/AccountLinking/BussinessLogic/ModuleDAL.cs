using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLinking
{
    class ModuleDAL
    {
        internal DataTable GetRecordFORISOProcessing()
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "DB Connection Started to Get the Sucessfull Record For ISO", 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Usp_GETRecordForAccountLinking", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.VarChar, 0, ParameterDirection.Input,"SELECT");
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input,0);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input,"");
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "No Of Record Found For ISO :" + ObjDsOutPut.Rows.Count.ToString(), 1);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "***********DB Connection ENDED************", 1);
                                

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
                return ObjDsOutPut;
            }
            
            return ObjDsOutPut;
        }
        internal void UpdateRecordISOStatus(APIResponseObject ObjAPIRsp,string code)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "","***********DB Connection STARTED************", 1);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "","DB Connection Stared to Update The ISO RSP", 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Usp_GETRecordForAccountLinking", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.VarChar, 0, ParameterDirection.Input, "UPDATE");
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.Status);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "***********DB Connection ENDED************", 1);
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
            }

        }
        public void FunInsertIntoErrorLog(string procedureName,string errorDesc, string Message,Int32 LogType)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_INSERTAccountLinkingLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@LogType", SqlDbType.Int, 200, ParameterDirection.Input, LogType);
                    sspObj.AddParameterWithValue("@Message", SqlDbType.VarChar, 0, ParameterDirection.Input, Message);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                                       
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public DataTable GetCardAPIRequest(String tranType)
        {
            DataTable dtReturn = null;
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Usp_getCardAPIRequest", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@transtype", SqlDbType.VarChar, 0, ParameterDirection.Input, tranType);
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
                return dtReturn;
            }
            return dtReturn;
        }
        public DataTable GETAPIURLANDSOurecID()
        {
            DataTable dtReturn = null;
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_GetCardAPIURLANDSourceID", oConn, CommandType.StoredProcedure))
                {
                    
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
                return dtReturn;
            }
            return dtReturn;
        }
      
    }
}
