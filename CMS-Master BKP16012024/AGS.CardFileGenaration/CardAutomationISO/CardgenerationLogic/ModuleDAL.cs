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

namespace AGS.CardAutomationISO
{
    class ModuleDAL
    {
        internal DataTable GetRecordFORISOProcessing(ConfigDataObject ObjData)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                new CardAutomation().FunInsertTextLog("RECORD FEATCHING FOR ISO CREATION STARTED FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "","DB Connection Started to Get the Sucessfull Record For ISO", ObjData.IssuerNo.ToString(), 1);
                
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(ObjData.FileProcessor_SP, ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "No Of Record Found For ISO :" + ObjDsOutPut.Rows.Count.ToString(), ObjData.IssuerNo.ToString(), 1);
                new CardAutomation().FunInsertTextLog("RECORD FEATCHING FOR ISO CREATION ENDED FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "***********DB Connection ENDED************", ObjData.IssuerNo.ToString(), 1);
                

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(),ex.ToString(), "", ObjData.IssuerNo.ToString(),0);
                ObjData.StepStatus = true;
                return ObjDsOutPut;
            }
            ObjData.Filestatus = "Record Featch FOR ISO COMPELETED";
            return ObjDsOutPut;
        }
        internal DataTable GETAccountlinkingRecords(ConfigDataObject ObjData)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {

                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "DB Connection Started to CHECKING FOR FAILED ISO WITH 42 FOR ACCOUNT LINKING", ObjData.IssuerNo.ToString(), 1);

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetDeclineRecordForAccountLinking", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "No Of Record Found For ACCOUNT LINKING ISO :" + ObjDsOutPut.Rows.Count.ToString(), ObjData.IssuerNo.ToString(), 1);

                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "***********DB Connection ENDED************", ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.StepStatus = true;
                return ObjDsOutPut;
            }
            ObjData.Filestatus = "Record Featch FOR ISO COMPELETED";
            return ObjDsOutPut;
        }
        internal void updateAccountLinkingStatus(APIResponseObject ObjAPIRsp, ConfigDataObject ObjData, string code)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************DB Connection STRTED TO UPDATE THE RECORD**************************", ObjData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdateAccountLinkingResponseFromSwitch", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.Status);

                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "SWITCH RSP UPDATION ENDED FOR RECORD ID !| " + code, ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }

        }
        internal void UpdateRecordISOStatus(APIResponseObject ObjAPIRsp,ConfigDataObject ObjData, string code)
        {
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("SWITCH RSP UPDATION STARTED FOR RECORD ID !" + code, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "DB Connection Stared to Update The ISO RSP", ObjData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdateCardDataResponseFromSwitch", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Fileid", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.Status);
                    ObjCmd.AddParameterWithValue("@NewEncPan", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjAPIRsp.EncCardNo);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjAPIRsp.CardNo);
                    ObjCmd.AddParameterWithValue("@NewEncacc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.EncAccountNo);

                    
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                new CardAutomation().FunInsertTextLog("SWITCH RSP UPDATION ENDED FOR RECORD ID !" + code, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "***********DB Connection ENDED************" , ObjData.IssuerNo.ToString(), 1);
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }

        }
        internal void INSERTSwitchRespStatus(APIResponseObject ObjAPIRsp, ConfigDataObject ObjData, string code)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************DB Connection STRTED TO INSERT THE Switch Response**************************", ObjData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_insertIntoTBLcardAutomationSwitchResponse", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.Status);
                    ObjCmd.AddParameterWithValue("@NewEncPan", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.EncCardNo);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.CardNo);
                    ObjCmd.AddParameterWithValue("@NewEncacc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.EncAccountNo);
                    ObjCmd.AddParameterWithValue("@AccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.AccountNo); //added for ATPCM-759
                    ObjCmd.AddParameterWithValue("@SwitchRspDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.StatusDescription);
                    ObjCmd.AddParameterWithValue("@Cardprogram", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.CardProgram);
                    ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.MobileNo);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "SWITCH RSP INSERT ENDED FOR RECORD ID !| " + code, ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }

        }
        internal void UpdateSwitchRspStatus(ConfigDataObject ObjData)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************DB Connection STRTED TO UPDATE THE RECORD**************************", ObjData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_UpdateSwitchResponse", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "SWITCH RSP UPDATION ENDED FOR RECORD ID !", ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }

        }
        internal DataTable usp_getExceptionRecordForProcessing(ConfigDataObject ObjData)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                new CardAutomation().FunInsertTextLog("Executing the exception Store Procedure !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "DB Connection Started to Get the Sucessfull Record For ISO", ObjData.IssuerNo.ToString(), 1);

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_getExceptionRecordForProcessing", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "No Of Record Found For Pending ISO :" + ObjDsOutPut.Rows.Count.ToString(), ObjData.IssuerNo.ToString(), 1);
                new CardAutomation().FunInsertTextLog("Execution Ended for exception Store Procedure !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "***********DB Connection ENDED************", ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.StepStatus = true;
                return ObjDsOutPut;
            }
            ObjData.Filestatus = "Record Featch FOR ISO COMPELETED";
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
                new CardAutomation().FunInsertTextLog("STD 4 RUN FOR "+Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo),Objdata.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "", "***********STARING STD 4************", Objdata.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_InitiateStandard4JobOnFileID", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, Objdata.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, Objdata.FileID);
                    ObjDataSET = ObjCmd.ExecuteDataSet();
                    ObjDsOutPut = ObjDataSET.Tables[ObjDataSET.Tables.Count-1];
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
            new CardAutomation().FunInsertTextLog(Objdata.Filestatus+ " "+ Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo), Objdata.ErrorLogPath);
            new CardAutomation().FunInsertTextLog("STD 4 RUN ENDED " + Objdata.FileID, Convert.ToInt32(Objdata.IssuerNo), Objdata.ErrorLogPath);
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "",Objdata.Filestatus, Objdata.IssuerNo.ToString(), 1);
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, Objdata.FileID, Objdata.ProcessId.ToString(), "", "***********STD 4 ENDED************", Objdata.IssuerNo.ToString(), 1);
            
            
        }
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
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "","",ex.ToString(), "", IssuerNo.ToString(),0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }
        internal void InsertRecord(ConfigDataObject objData,DataTable DtBulkData)
        {

            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("Record DB INSERT STARTED FOR !" + objData.FileName, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId, "", "*******************DB Connection STARED To Insert The Record INTO DB*****************", objData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection( ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(objData.FileUploader_SP, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input,objData.FileName);
                    ObjCmd.AddParameterWithValue("@FileID", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@CustBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                new CardAutomation().FunInsertTextLog("Record DB INSERT ENDED FOR !" + objData.FileName, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId, "", "*******************DB Connection ENDED*****************", objData.IssuerNo.ToString(), 1);
                

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.StepStatus = true;
                objData.Filestatus = "ERROR WHILE DATA INSERT INTO DB ";
                objData.ErrorDesc ="ERROR WHILE DATA INSERT INTO DB |"+ex.ToString();
            }
            
        }
        internal void InsertINTOFileMaster(ConfigDataObject objData)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("Record Fetch: Start", Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_InsertIntoFileMaster", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.ProcessId);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    objData.FileID = Convert.ToString(ObjDsOutPut.Rows[0]["Status"]);
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
        internal void UpdateFileStatus(ConfigDataObject objData,Int32 ProcessStep)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("Record Fetch: Start", Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_UpdateFileMasterStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessStep", SqlDbType.Int, 0, ParameterDirection.Input, ProcessStep);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input,objData.ProcessId);
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
        internal void usp_MarkBankAsError(ConfigDataObject objData)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("Record Fetch: Start", Convert.ToInt32(objData.IssuerNo),objData.ErrorLogPath);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_MarkBankAsError", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objData.IssuerNo));
                    ObjCmd.AddParameterWithValue("@Status", SqlDbType.VarChar, 0, ParameterDirection.Input,40);
                    ObjCmd.AddParameterWithValue("@Error", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.ErrorDesc);
                    ObjCmd.AddParameterWithValue("@FileID", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@FileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Filestatus);
                    ObjCmd.AddParameterWithValue("@ProcessID", SqlDbType.BigInt, 0, ParameterDirection.Input, objData.ProcessId);
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
                    sspObj.AddParameterWithValue("@FileID", SqlDbType.VarChar, 200, ParameterDirection.Input,FileID);
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
                new CardAutomation().FunInsertTextLog("FunInsertIntoErrorLog !" + "usp_INSERTAPPLICATIONLog", Convert.ToInt32(IssuerNo), ex.ToString());
            }
        }
        public DataTable GetCardAPIRequest(String tranType, ConfigDataObject ObjData)
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
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return dtReturn;
            }
            return dtReturn;
        }
        public bool CheckPreviousfilestatus(ConfigDataObject ObjData)
        {
            DataTable dtReturn = null;
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Usp_checkPreviousFileStatus", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(ObjData.IssuerNo));
                    sspObj.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToString(ObjData.ProcessId));
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                    if(Convert.ToString(dtReturn.Rows[0]["FileStatusCode"]).Equals("0",StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ObjData.ProcessId.ToString(), "", "**************************Previous File PRE PENDING**************************", ObjData.IssuerNo.ToString(), 1);
                        FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ObjData.ProcessId.ToString(), "", "Previous File PRE PENDING FOR |FILEID|" + Convert.ToString(dtReturn.Rows[0]["FileID"]) + "|FileName|" + Convert.ToString(dtReturn.Rows[0]["fileName"]), ObjData.IssuerNo.ToString(), 1);
                        return false;
                    }
                    
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return false;
            }
            
        }
        public int GETMaxFileHeader (ConfigDataObject ObjData)
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
        public DataTable FunGetCardAutomationStatusRecords(string IssuerNo, string CurrentFileID)
        {
            //change
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DownloadCardAutomationStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, CurrentFileID);
                    ObjDtOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataTable();
                ObjConn.Close();
                //FunInsertTextLog("Error: FunGetPREFileStandard" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return ObjDtOutPut;
        }

        //added by uddesh for split file logic start
        public DataTable FunGetSetSubFilesProcessStatus(String operation, String fileName, int IssuerNo, int ProceessId, int fileid)
        {

            DataTable ObjDTOutPut = new DataTable();
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();
                ObjConn = new SqlConnection(ConfigManager.parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSetSubFilesProcessStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, operation);
                    ObjCmd.AddParameterWithValue("@fileName", SqlDbType.VarChar, 0, ParameterDirection.Input, fileName);
                    ObjCmd.AddParameterWithValue("@fileId", SqlDbType.BigInt, 0, ParameterDirection.Input, fileid);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("FunGetSetSubFilesProcessStatus", null, ProceessId.ToString(), ex.ToString(), "", IssuerNo.ToString(), 0);
                return null;
            }

            if (ObjDTOutPut == null)
            {
                FunInsertIntoErrorLog("FunGetSetSubFilesProcessStatus", null, ProceessId.ToString(), "No data found for subfile", "", IssuerNo.ToString(), 0);
                return null;
            }
            if (ObjDTOutPut.Rows.Count == 0)
            {
                FunInsertIntoErrorLog("FunGetSetSubFilesProcessStatus", null, ProceessId.ToString(), "No data found for subfile", "", IssuerNo.ToString(), 0);
                return null;
            }

            return ObjDTOutPut;
        }
        //added by uddesh for split file logic 
        ///* autojob three times ISO call for 108 switch response ATPBF-1039* START/
        internal DataTable ManageSwitchRespStatus(APIResponseObject ObjAPIRsp, ConfigDataObject ObjData, string code)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************DB Connection STRTED TO INSERT THE Switch Response**************************", ObjData.IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_ManageTBLcardAutomationSwitchResponseForALL", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjData.ProcessId);
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.Status);
                    ObjCmd.AddParameterWithValue("@NewEncPan", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.EncCardNo);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.CardNo);
                    ObjCmd.AddParameterWithValue("@NewEncAcc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.EncAccountNo);
                    ObjCmd.AddParameterWithValue("@ISOCallCounter", SqlDbType.Int, 0, ParameterDirection.Input, ObjData.ISOCallCounter);
                    ObjCmd.AddParameterWithValue("@SwitchRespID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjData.SwitchRespID);
                    ObjCmd.AddParameterWithValue("@SwitchRspDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.StatusDescription);
                    ObjCmd.AddParameterWithValue("@Cardprogram", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.CardProgram);
                    ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAPIRsp.MobileNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog("", ObjData.FileID, ObjData.ProcessId, "", "SWITCH RSP INSERT ENDED FOR RECORD ID !| " + code, ObjData.IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("ManageSwitchRespStatus", ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }
            return ObjDsOutPut;

        }
        ///*autojob three times ISO call for 108 switch response ATPBF-1039 * END/
    }
}
