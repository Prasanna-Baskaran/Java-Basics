using AGS.SqlClient;
using ReflectionIT.Common.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CardFileSplit
{
    class ModuleDAL
    {
     
     
        internal DataTable getBankConfig(int IssuerNo)
        {
            FunInsertIntoErrorLog("getBankConfig", "", "", "", "RETRIVING THE PROCESS CONFIGURED FOR THE BANK", IssuerNo.ToString(), 1);
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetProcessConfiguration", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("getBankConfig", "","",ex.ToString(), "Error", IssuerNo.ToString(),0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog("getBankConfig", "", "", "", "PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }
        internal DataSet InsertRecord(ConfigDataObject objData,DataTable DtBulkData)
        {

            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog("InsertRecord", objData.FileID, objData.ProcessId, "", "*******************DB Connection STARED To Insert The Record INTO DB*****************", objData.IssuerNo.ToString(), 1);
                
                ObjConn = new SqlConnection( ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(objData.FileUploader_SP, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input,objData.FileName);
                    ObjCmd.AddParameterWithValue("@FileID", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt32(objData.FileID));
                    ObjCmd.AddParameterWithValue("@CardBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                FunInsertIntoErrorLog("InsertRecord", objData.FileID, objData.ProcessId, "", "*******************DB Connection ENDED*****************", objData.IssuerNo.ToString(), 1);
                objData.Filestatus = "Bulk upload and Sub files generated";
                objData.StepStatus = false;

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("InsertRecord", objData.FileID, objData.ProcessId.ToString(), ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.StepStatus = true;
                objData.Filestatus = "ERROR WHILE DATA INSERT INTO DB ";
                objData.ErrorDesc ="ERROR WHILE DATA INSERT INTO DB |"+ex.ToString();
                return ObjDsOutPut;
            }
            return ObjDsOutPut;
        }
        public int GETMaxFileHeader(ConfigDataObject ObjData)
        {
            DataTable dtReturn = null;
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_GETMaxFileHeader", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(ObjData.ProcessId));
                    sspObj.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(ObjData.IssuerNo));
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                    return Convert.ToInt32(dtReturn.Rows[0]["HeaderCount"]);

                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return 31;
            }
        }
        internal void InsertINTOFileMaster(ConfigDataObject objData,String operation)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog("InsertINTOFileMaster", "", "", "", "Executing SP usp_GetSetCardOriginalFileStatus.operation : " + operation, objData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.[usp_GetSetCardOriginalFileStatus]", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, operation);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input,objData.FileName);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt32(objData.ProcessId));
                    int FileID;
                    if (int.TryParse(objData.FileID, out FileID))
                    {
                        ObjCmd.AddParameterWithValue("@FileID", SqlDbType.Int, 0, ParameterDirection.Input, FileID);
                    }
                    ObjCmd.AddParameterWithValue("@TotalCount", SqlDbType.Int, 0, ParameterDirection.Input, objData.TotalCount);
                    ObjCmd.AddParameterWithValue("@FileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Filestatus);

                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    objData.FileID = Convert.ToString(ObjDsOutPut.Rows[0]["Status"]);
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    objData.StepStatus = false;
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("InsertINTOFileMaster", objData.FileID, objData.ProcessId.ToString(), ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.StepStatus = true;    
            }
            
        }

        internal void InsertINTOSubFileStatus(ConfigDataObject objData, String operation, String subFileName)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog("InsertINTOFileMaster", "", "", "", "Executing SP usp_GetSetCardOriginalFileStatus.operation : " + operation, objData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.[usp_GetSetSubCardFileStatus]", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, operation);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);                   
                    ObjCmd.AddParameterWithValue("@subFileName", SqlDbType.VarChar, 0, ParameterDirection.Input, subFileName);
                    ObjCmd.AddParameterWithValue("@Local_Input", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Local_Input);
                    ObjCmd.AddParameterWithValue("@TotalRecord", SqlDbType.Int, 0, ParameterDirection.Input, objData.TotalCount);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt32(objData.ProcessId));
                    int FileID;
                    if (int.TryParse(objData.FileID, out FileID))
                    {
                        ObjCmd.AddParameterWithValue("@FileID", SqlDbType.Int, 0, ParameterDirection.Input, FileID);
                    }
                    ObjCmd.AddParameterWithValue("@FileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Filestatus);

                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    //objData.FileID = Convert.ToString(ObjDsOutPut.Rows[0]["Status"]);
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    objData.StepStatus = false;
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("InsertINTOFileMaster", objData.FileID, objData.ProcessId.ToString(), ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.StepStatus = true;
            }

        }



        public void FunInsertIntoErrorLog(string procedureName, string FileID, string ProcessID, string errorDesc, string Message, string IssuerNo, Int32 LogType)
        {
            Log log = new Log(IssuerNo);
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_INSERTCardSplitLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@FileID", SqlDbType.VarChar, 200, ParameterDirection.Input, FileID);
                    sspObj.AddParameterWithValue("@processID", SqlDbType.VarChar, 200, ParameterDirection.Input, ProcessID);
                    sspObj.AddParameterWithValue("@LogType", SqlDbType.Int, 200, ParameterDirection.Input, LogType);
                    sspObj.AddParameterWithValue("@Message", SqlDbType.VarChar, 0, ParameterDirection.Input, Message);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    sspObj.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
                log.WriteLog(Message +" :: " + errorDesc);
            }
            catch (Exception ex)
            {
                log.WriteLog(Message + " :: " + ex.ToString());
            }
        }


     

   
    }
}
