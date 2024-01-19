using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VDCToPDCSTD4
{
    class ModuleDAL
    {
        internal DataTable getBankConfig(int IssuerNo)
        {
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "RETRIVING THE PROCESS CONFIGURED FOR THE BANK", IssuerNo.ToString(), 1);
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetProcessMaster", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", ex.ToString(), "", IssuerNo.ToString(), 0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }

        internal DataTable checkedSwitchProcessRecord(int IssuerNo,string ProcessID)
        {
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "RETRIVING THE Switch PROCESS Record VDC To PDC", IssuerNo.ToString(), 1);
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckRecVDCToPDC", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssurNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@ProcessType", SqlDbType.VarChar, 0, ParameterDirection.Input, ProcessID);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", ex.ToString(), "", IssuerNo.ToString(), 0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }

        internal void RunSTD4(ConfigDataObject Objdata)
        {
            DataTable ObjDsOutPut = new DataTable();
            DataSet ObjDataSET = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                new RunSTD4().FunInsertTextLog("STD 4 RUN FOR " + Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo), Objdata.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "", "***********STARING STD 4************", Objdata.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_InitiateStandard4JobOnFileID", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, Objdata.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, Objdata.FileID);
                    ObjDataSET = ObjCmd.ExecuteDataSet();
                    ObjDsOutPut = ObjDataSET.Tables[ObjDataSET.Tables.Count - 1];
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToString(ObjDsOutPut.Rows[0]["Standard4LastStatus"]).Trim().Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    Objdata.StepStatus = true;
                    Objdata.ErrorDesc = "ERROR IN STD 4";
                    Objdata.Filestatus = "ERROR IN STD 4";

                }
                else
                {
                    Objdata.Filestatus = "STD 4 Sucessfully";
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), ex.ToString(), "", Objdata.IssuerNo.ToString(), 0);
                Objdata.StepStatus = true;
                Objdata.ErrorDesc = "ERROR IN STD 4";
                Objdata.Filestatus = "ERROR IN STD 4";
            }
            new RunSTD4().FunInsertTextLog(Objdata.Filestatus + " " + Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo), Objdata.ErrorLogPath);
            new RunSTD4().FunInsertTextLog("STD 4 RUN ENDED " + Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo), Objdata.ErrorLogPath);
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "", Objdata.Filestatus, Objdata.IssuerNo.ToString(), 1);
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "", "***********STD 4 ENDED************", Objdata.IssuerNo.ToString(), 1);


        }

        internal void UpdateFileStatus(ConfigDataObject objData, Int32 ProcessStep)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                new RunSTD4().FunInsertTextLog("Record Fetch: Start", Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_UpdateFileMasterStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessStep", SqlDbType.Int, 0, ParameterDirection.Input, ProcessStep);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, objData.ProcessId);
                    ObjCmd.AddParameterWithValue("@fileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Filestatus);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.StepStatus = true;
            }

        }

        public void FunInsertIntoErrorLog(string procedureName, string FileID, string ProcessID, string errorDesc, string Message, string IssuerNo, Int32 LogType)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_INSERTAPPLICATIONLog", ObjConn, CommandType.StoredProcedure))
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
            }
            catch (Exception ex)
            {

            }
        }
    }
}
