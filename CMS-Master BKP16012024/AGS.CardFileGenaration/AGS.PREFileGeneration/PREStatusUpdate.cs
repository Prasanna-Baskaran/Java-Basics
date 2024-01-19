using ReflectionIT.Common.Data.SqlClient;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AGS.PREFileGeneration
{
   
    public class PREStatusUpdate
    {
        int GlobalIssuerNo = 0;
        public void Process()
        {

        }
        private DataSet FunGetCardAutomationStatusRecords(string IssuerNo, string CurrentFileID, String SP_CardAutomationStatus)
        {
            //change
            DataSet ObjDtOutPut = new DataSet();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SP_CardAutomationStatus, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, CurrentFileID);
                    ObjDtOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataSet();
                ObjConn.Close();
                //FunInsertTextLog("Error: FunGetPREFileStandard" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return ObjDtOutPut;
        }
        private void FunInsertTextLog(string Message, int issuerNo, bool IsError)
        {
            string LogPath = "";
            try
            {
                LogPath = System.Configuration.ConfigurationManager.AppSettings["DebugLogPath"].ToString();
                if (!string.IsNullOrEmpty(LogPath))
                {
                    string filename = issuerNo.ToString() + "PREDebug_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                    string filepath = LogPath + filename;
                    if (File.Exists(filepath))
                    {
                        using (StreamWriter writer = new StreamWriter(filepath, true))
                        {
                            writer.WriteLine(DateTime.Now + ":" + issuerNo.ToString() + ": " + Message);
                            writer.Close();
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = File.CreateText(filepath))
                        {
                            writer.WriteLine(DateTime.Now + ":" + issuerNo.ToString() + ": " + Message);
                            writer.Close();
                        }
                    }
                }
                if (IsError)
                {
                    FunDBLog("Error", Message, "", issuerNo.ToString(), "");
                }
            }
            catch (Exception Ex)
            {
                //FunDBLog("FunInsertIntoLogFile", Ex.Message, "LogPath=" + LogPath, "", string.Empty);
            }
        }
        private void FunDBLog(string procedureName, string errorDesc, string parameterList, string IssuerNo, string BatchNo)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Sp_CAInsertPREErrorLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    if (!string.IsNullOrEmpty(parameterList))
                        sspObj.AddParameterWithValue("@parameterList", SqlDbType.VarChar, 0, ParameterDirection.Input, parameterList);
                    if (!string.IsNullOrEmpty(IssuerNo))
                        sspObj.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    if (!string.IsNullOrEmpty(BatchNo))
                        sspObj.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, BatchNo);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Exception in dbo.Sp_CAInsertPREErrorLog: " + ex.ToString(), Convert.ToInt32(IssuerNo), false);
                //FunInsertIntoLogFile_1(ErrorLogFilePath, ex, "FunInsertIntoErrorLog");
                //FunInsertIntoErrorLog("FunInsertIntoErrorLog", ex.Message, "", IssuerNo.ToString(), string.Empty);
            }
        }
        public void UploadStatusFileOnSFTP(string CurrentFileID, string FileName, AGS.PREFileGeneration.PREFile.ClsPathsBO objpath, string IssuerNo, String SP_CardAutomationStatus)
        {
            GlobalIssuerNo = Convert.ToInt32(IssuerNo);
            FunInsertTextLog("DownloadFile Start", Convert.ToInt32(IssuerNo), false);
            DataSet __DataSET = FunGetCardAutomationStatusRecords(IssuerNo, CurrentFileID, SP_CardAutomationStatus);
            int count = 0;
            foreach (DataTable Data in __DataSET.Tables)
            {
                ++count;
                if (Data.Rows.Count > 0)
                {
                    string ReportType = Convert.ToString(Data.Rows[0]["ReportType"]);
                    string FileStatusName = "Status_" + Path.GetFileNameWithoutExtension(FileName.Trim()) + "_" + ReportType + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                    FunInsertTextLog("record found to download status file", Convert.ToInt32(IssuerNo), false);
                    string TemproryLocationPath = System.Configuration.ConfigurationManager.AppSettings["PREStatusFile"].ToString(); //"";//local path from web.config Appkey setting
                    string localFilePath = TemproryLocationPath + FileStatusName;
                    //string RemoteFilePath = objpath.Bank_Output + "/" + FileStatusName;
                    using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(localFilePath))
                    {
                        // Data.Columns.Remove("");
                        swObj.Write(ConvertDataTableToCSV(Data, true, ",", "", "", IssuerNo));
                        swObj.Close();
                        swObj.Dispose();
                    }
                    if (objpath.isstatusFileINPGP)
                    {

                        bool bResult = new PREFile().PGP_Encrypt(objpath.PGP_KeyName, TemproryLocationPath + FileStatusName, TemproryLocationPath + Path.GetFileNameWithoutExtension(FileStatusName) + ".pgp", objpath.StatusFilePGPPublicKeyPath, IssuerNo.ToString());
                        if (!bResult)
                        {
                            FunInsertTextLog("STATUS FILE ENCRYPTION FAILED", Convert.ToInt32(IssuerNo), false);
                            return;
                        }
                        File.Delete(TemproryLocationPath + FileStatusName);
                        FileStatusName = Path.GetFileNameWithoutExtension(FileStatusName) + ".pgp";
                        localFilePath = TemproryLocationPath + FileStatusName;
                    }
                    string RemoteFilePath = (ReportType.Equals("Status", StringComparison.OrdinalIgnoreCase) ? objpath.AGS_Output : objpath.Bank_Output) + "/" + FileStatusName;
                    var Result = SFTPFileUpload_StatusFile(objpath, localFilePath, RemoteFilePath, Convert.ToInt32(IssuerNo));
                    if (Result)
                    {
                        FunUpdateStatusFileName(IssuerNo, CurrentFileID, FileStatusName);
                        //FileStatusName 
                    }
                    FunInsertTextLog("Status file uploaded on SFTP:" + RemoteFilePath, Convert.ToInt32(IssuerNo), false);
                }
                else
                {
                    FunInsertTextLog("No record found to download status file", Convert.ToInt32(IssuerNo), false);
                }
            }

        }



        public void FunUpdateStatusFileName(string IssuerNo, string CurrentFileID, string FileStatusName)
        {
            //change
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdateStatusFileName", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, CurrentFileID);
                    ObjCmd.AddParameterWithValue("@StatusFileName", SqlDbType.VarChar, 0, ParameterDirection.Input, FileStatusName);
                    ObjDtOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataTable();
                ObjConn.Close();
                FunInsertTextLog("Error: PRE update StatusFileName SP failed" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }

        }
        private string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter, string IssuerNo)
        {
            StringBuilder sbOutput = new StringBuilder();

            try
            {
                foreach (DataColumn dcTable in dtSource.Columns)
                {
                    if (dcTable.ColumnName.IndexOf('~') > 0)
                    {
                        dtSource.Columns.Remove(dcTable);
                    }
                }

                if (WithHeader)
                {
                    var columnNames = dtSource.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                    sbOutput.AppendLine(string.Join(Delimeter, columnNames));
                }
                else
                {
                    if (!String.IsNullOrEmpty(CustomHeader)) sbOutput.AppendLine(CustomHeader);
                }

                foreach (DataRow row in dtSource.Rows)
                {
                    var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    sbOutput.AppendLine(string.Join(Delimeter, fields));
                }

                if (!String.IsNullOrEmpty(CustomFooter)) sbOutput.AppendLine(CustomFooter);
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error while converting datatable to csv in status file generation", Convert.ToInt32(IssuerNo), false);
                sbOutput = new StringBuilder();
            }

            return sbOutput.ToString();
        }

        private bool SFTPFileUpload_StatusFile(AGS.PREFileGeneration.PREFile.ClsPathsBO objpath, string FromFile, string ToFile, int issuerNo)
        {
            var status = false;
            try
            {
                //var sourcePath = objpath.AGS_SFTPServer;
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(objpath.Bank_SFTPUser, objpath.Bank_SFTPPassword));

                if (!string.IsNullOrEmpty(objpath.SSH_Private_key_file_Path))
                {
                    var keyFile = new PrivateKeyFile(objpath.SSH_Private_key_file_Path, objpath.passphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(objpath.Bank_SFTPUser, keyFiles));
                }

                var con = new ConnectionInfo(objpath.Bank_SFTPServer, Convert.ToInt32(objpath.Bank_SFTPPort), objpath.Bank_SFTPUser, methods.ToArray());
                FunInsertTextLog("Status file upload: SFTPFileUpload: upload start", issuerNo, false);
                using (var sftp = new SftpClient(con))
                {
                    try
                    {
                        sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                        sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                        sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                        sftp.BufferSize = 5000000;
                        sftp.Connect();
                        using (var fileStream = new FileStream(FromFile, FileMode.Open))
                        {
                            sftp.UploadFile(fileStream, ToFile);
                            //File.Delete(FromFile);//if required to delete from local
                            fileStream.Flush();
                            fileStream.Close();
                            fileStream.Dispose();
                        }
                        sftp.Disconnect();
                        FunInsertTextLog("Status file SFTPFileUpload: end", issuerNo, false);
                        status = true;
                        return status;
                    }
                    catch (Exception ex)
                    {
                        FunInsertTextLog("Status file : SFTPFileUpload" + ex.ToString(), issuerNo, true);
                        FunInsertTextLog("Status file SFTPFileUpload: end", issuerNo, false);
                        status = false;
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: Status file SFTPFileUpload: Error" + ex.ToString(), issuerNo, false);
            }
            return status;
        }

        public void FunPREFailedStatusRecords(string IssuerNo, string CurrentFileID)
        {
            //change
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdatePREstatusForAllFailed", ObjConn, CommandType.StoredProcedure))
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
                FunInsertTextLog("Error: PRE update status SP failed" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }

        }
        public string parse(string ConnectionString)
        {
            string sParsedConnectionString = "";
            try
            {
                sParsedConnectionString = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(ConnectionString))));
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: while decoding connection string in status file" + ex.ToString(), GlobalIssuerNo, true);
            }
            return sParsedConnectionString;
        }
    }
}
