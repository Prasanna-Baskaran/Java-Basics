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
namespace InstaCardsFileGeneration
{
    class ModuleDAL
    {
        internal void UpdateFileStatus(SFTPDataObject objData, Int32 ProcessStep,String IssuerNo)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid.ToString(), "", "Update file status", IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_UpdateFileMasterStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@fileid", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@ProcessStep", SqlDbType.Int, 0, ParameterDirection.Input, ProcessStep);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, objData.processid);
                    ObjCmd.AddParameterWithValue("@fileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, "File Upload Succsessfully");
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid.ToString(), ex.ToString(), "", IssuerNo.ToString(), 0);
            
            }

        }
        internal Boolean InsertINTOFileMaster(SFTPDataObject objData, String IssuerNo)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog("Record Fetch: Start", "", "", "", "Retriving data for Insta card generation", IssuerNo, 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_InsertIntoFileMaster", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.processid);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    objData.FileID = Convert.ToString(ObjDsOutPut.Rows[0]["Status"]);
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid.ToString(), ex.ToString(), "", IssuerNo.ToString(), 0);
               return false;
            }
            return true;

        }


        internal DataTable InsertRecord(SFTPDataObject objData, DataTable DtBulkData,String IssuerNo)
        {

            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid, "", "*******************DB Connection STARED To Insert The Record INTO DB*****************", IssuerNo.ToString(), 1);
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(objData.FileUploader_SP, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input,"UPLOAD");
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(IssuerNo));
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@FileID", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileID);
                    ObjCmd.AddParameterWithValue("@InstaRequestBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid, "", "*******************DB Connection ENDED*****************", IssuerNo.ToString(), 1);


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.processid.ToString(), ex.ToString(), "",IssuerNo.ToString(), 0);
                return null;
            }
            return ObjDsOutPut;
        }


        internal DataTable getConfiguration(int IssuerNo)
        {
            FunInsertIntoErrorLog("getInstaCardsRequestData", "", "", "", "Retriving data for Insta card generation", IssuerNo.ToString(), 1);
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSetInstaFileBaseGeneration", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "GetConfig");
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("getInstaCardsRequestData", "", "", ex.ToString(), "getInstaCardsRequestData-Error", IssuerNo.ToString(), 0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog("getInstaCardsRequestData", "", "", "", "getInstaCardsRequestData PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }

        internal DataTable getInstaCardsRequestData(int IssuerNo)
        {
            FunInsertIntoErrorLog("getInstaCardsRequestData", "", "", "", "Retriving data for Insta card generation", IssuerNo.ToString(), 1);
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GenerateInstaCards", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "GetForFileGeneration");
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("getInstaCardsRequestData", "","",ex.ToString(), "getInstaCardsRequestData-Error", IssuerNo.ToString(),0);
                return ObjDsOutPut;
            }
            FunInsertIntoErrorLog("getInstaCardsRequestData", "", "", "", "getInstaCardsRequestData PROCESS ENDED", IssuerNo.ToString(), 1);
            return ObjDsOutPut;
        }


        internal void UpdateFileUploadStatus(ConfigDataObject objData)
        {
            FunInsertIntoErrorLog("UpdateFileUploadStatus", objData.id, "", "", "Update FileUpload Status for Insta card generation", objData.IssuerNo, 1);
            SqlConnection ObjConn = null;
            try
            {

                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GenerateInstaCards", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "FileUploadStatus");
                    ObjCmd.AddParameterWithValue("@id", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.id);
                    ObjCmd.AddParameterWithValue("@InstaCifFileName", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.FileName);
                    ObjCmd.AddParameterWithValue("@InstaCifFilePath", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Bank_Input);
                    ObjCmd.AddParameterWithValue("@fileStatus", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Filestatus);
                    ObjCmd.AddParameterWithValue("@Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Reason);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@LastCustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.LastCustomerID);
                    //ObjCmd.AddParameterWithValue("@LastAccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.LastAccountNo);
                    //ObjCmd.AddParameterWithValue("@Insta_Reference_No", SqlDbType.VarChar, 0, ParameterDirection.Input, objData.Insta_Reference_No);

                    ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("UpdateFileUploadStatus", objData.id, "", ex.ToString(), "Error", objData.IssuerNo, 0);
            }
            FunInsertIntoErrorLog("UpdateFileUploadStatus", objData.id, "", "", "Update FileUpload Status for Insta card generation ENDED", objData.IssuerNo, 1);
        }


        public void FunInsertIntoErrorLog(string procedureName, string FileID, string ProcessID, string errorDesc, string Message, string IssuerNo, Int32 LogType)
        {
            Log log = new Log(IssuerNo);
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(ConfigManager.parse(ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_InsertInstaCardLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@FileID", SqlDbType.VarChar, 200, ParameterDirection.Input, FileID);
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


        public bool FunCreateUploadInstaCIF(ConfigDataObject objData, string[] ArrResult)
        {
            try
            {
                FunInsertIntoErrorLog("FunCreateUploadInstaCIF", objData.id,"", "", "FunCreateUploadInstaCIF Started",  objData.IssuerNo.ToString(),1);
                string Cardprefix = objData.Cardprefix;

                string PrepaidCIFFilePath = objData.Local_Input;
                objData.FileName= objData.BankName + "_CIF_" + Cardprefix + "_ID#" + objData.id.ToString() + "#_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".txt";
                string CIFFileName = fn_Create_Directory(PrepaidCIFFilePath) + objData.FileName;

                FunInsertIntoErrorLog("FunCreateUploadInstaCIF", objData.id, "", "", "FilePath: " + CIFFileName + " : FileName: " + objData.FileName + " : Cardprefix: " + Cardprefix, objData.IssuerNo.ToString(), 1);

                StringBuilder sbFile = new StringBuilder();
                string sData = string.Empty;


                foreach (string Result in ArrResult.Where(c => c != null).ToArray())
                {
                    sbFile.AppendLine(Result);
                }
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(CIFFileName))
                    {
                        file.WriteLine(sbFile.ToString());
                        file.Close();        
                        FunInsertIntoErrorLog("FunCreateUploadInstaCIF", objData.id, "", "", "InstaCIFFile Generated On Local Path: " + CIFFileName,  objData.IssuerNo.ToString(), 1);

                        //if (!SearchFile.UploadFile(objData))
                        //{
                        //    if (File.Exists(objData.Local_Input + objData.FileName))
                        //    {
                        //        File.Delete(objData.Local_Input + objData.FileName);
                        //        new ModuleDAL().FunInsertIntoErrorLog("UploadFile", objData.id.ToString(), "", "", "Local file Deleted file name:" + objData.Local_Input + objData.FileName, objData.IssuerNo, 1);
                        //    }
                        //    return false;
                        //}
                        
                    }


                }
                catch (Exception ex)
                {
                    FunInsertIntoErrorLog("FunCreateUploadInstaCIF", objData.id, "", ex.ToString(),"Exception 1", objData.IssuerNo, 0);
                    objData.Filestatus = "FAIL";
                    objData.Reason = "Eror while creating/Uploading Insta file";
                    return false;
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("FunCreateUploadInstaCIF", objData.id, "", ex.ToString(), "Exception 2", objData.IssuerNo, 0);
                objData.Filestatus = "FAIL";
                objData.Reason = "Eror while creating/Uploading Insta file";
                return false;
            }

            return true;
        }

        public static String fn_Create_Directory(String DirPath)
        {
            try
            {
                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);

                }
                return DirPath;
            }
            catch (Exception Ex)
            {
                return DirPath;

            }

        }


    }
}
