using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionIT.Common.Data.SqlClient;
using System.Configuration;
using System.Data;
using Renci.SshNet;
/************************************************************************
Object Name: Bulk Upload
Purpose: process uploaded file
Change History
Date         Changed By				Reason
28/7/2017	Diksha Walunj			Newly Developed
07/10/2017  Diksha Walunj          ATPCM224: Modified for Reissue file Upload



*************************************************************************/
namespace AGS.BulkUpload
{
    public class BulkUpload : IDisposable
    {
        string ErrorLogFilePath = string.Empty;
        public void Process(int IssuerNo)
        {
            try
            {
                FunInsertIntoErrorLog("BulkUpload.cs,Process", "BulkUpload Process Start", IssuerNo.ToString());
                // Start Get Config Paths
                ClsPathsBO objPath = new ClsPathsBO();

                DataTable DtPath = new DataTable();
                DtPath = FunGetPaths(IssuerNo);

                if (DtPath != null && DtPath.Rows.Count > 0)
                {
                    foreach (DataRow objDr in DtPath.Rows)
                    {
                        DataTable DtResult = new DataTable();
                        DtResult = DtPath.Clone();
                        DtResult.Rows.Add(objDr.ItemArray);
                        objPath = BindDatatableToClass<ClsPathsBO>(DtResult);
                        FunInsertIntoErrorLog("BulkUpload.cs,Process", "BulkUpload|SFTP Path:" + objPath.SFTP_Path + ",InputPath:" + objPath.InputPath, IssuerNo.ToString());
                        //check file exists at SFTP and get for processing
                        FunGetSFTPFile(objPath);
                    }
                }
                //End

                //Get Files to process
                DataTable DtProcessFile = FunCheckFileToProcess(IssuerNo);
                if (DtProcessFile != null && DtProcessFile.Rows.Count > 0)
                {
                    foreach (DataRow objDr in DtProcessFile.Rows)
                    {
                        DataTable DtResult = new DataTable();
                        DtResult = DtProcessFile.Clone();
                        DtResult.Rows.Add(objDr.ItemArray);
                        objPath = BindDatatableToClass<ClsPathsBO>(DtResult);
                        if (!string.IsNullOrEmpty(objPath.ID))
                        {
                            //Process bulk upload file data
                            FunProcessFileData(objPath);
                            //upload failed files on sftp
                            if (Directory.GetFiles(objPath.FailedPath, "*.txt").Count() > 0)
                            {
                                FunSFTP_Upload(objPath.SFTP_Server, objPath.SFTP_Port, objPath.SFTP_User, objPath.SFTP_PWD, objPath.FailedPath, objPath.SFTP_Failed);
                            }

                            //upload output status files on sftp
                            if (Directory.GetFiles(objPath.OutputPath, "*.txt").Count() > 0)
                            {
                                FunSFTP_Upload(objPath.SFTP_Server, objPath.SFTP_Port, objPath.SFTP_User, objPath.SFTP_PWD, objPath.OutputPath, objPath.SFTP_StatusFile);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("BulkUpload.cs,Process", ex.Message, IssuerNo.ToString());
            }

        }

        private void FunGetSFTPFile(ClsPathsBO objPath)
        {
            DataTable ObjDt = new DataTable();
            ObjDt.Columns.Add("FileName");
            ObjDt.Columns.Add("FilePath");
            ObjDt.Columns.Add("FileID");
            try
            {
                //fetch files from Sftp
                using (var sftp = new SftpClient(objPath.SFTP_Server, Convert.ToInt32(objPath.SFTP_Port), objPath.SFTP_User, objPath.SFTP_PWD))
                {
                    sftp.Connect();
                    string remoteDirectory = objPath.SFTP_Path;
                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains(".txt")) select i)
                    {
                        //check  path exists on server
                        if (Directory.Exists(objPath.InputPath))
                        {
                            //Check file Not exists in Archive Folder
                            if (!sftp.Exists(objPath.SFTP_BackUp + d.Name))
                            {
                                using (Stream fileStream = File.OpenWrite(Path.Combine(objPath.InputPath, d.Name)))
                                {

                                    sftp.DownloadFile(d.FullName, fileStream);

                                    sftp.RenameFile(d.FullName, objPath.SFTP_BackUp + d.Name);
                                    fileStream.Flush();
                                    fileStream.Close();
                                    fileStream.Dispose();
                                    ObjDt.Rows.Add(d.Name, objPath.InputPath + d.Name, objPath.FileID);
                                }
                            }
                            else
                            {
                                FunInsertIntoErrorLog("BulkUpload.cs,FunGetSFTPFile", "File already exists on SFTP BackUp" , "fileName: "+d.Name+", SFTPBackUpPath: "+objPath.SFTP_BackUp ); }
                        }
                        else
                        {
                            //path  not exists to download files
                            FunInsertIntoErrorLog("BulkUpload.cs,FunGetSFTPFile", "Path not exists| InputPath: " + objPath.InputPath + ",SFTP Path:" + objPath.SFTP_Path, objPath.IssuerNo.ToString());
                        }
                    }
                    sftp.Disconnect();
                    sftp.Dispose();

                    //save file info
                    if (ObjDt != null && ObjDt.Rows.Count > 0)
                    {
                        FunSaveFileInfo(ObjDt);
                    }
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("BulkUpload.cs,FunGetSFTPFile", ex.Message, objPath.SFTP_Server);
            }


        }

        private string FunGetNewFileName(string Input)
        {
            string strNewName = string.Empty;
            strNewName = Input.Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0] + "_D_" + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";
            return strNewName;
        }

        private void FunSaveFileInfo(DataTable DtFileInfo)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_SaveFileInfo", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@FileInfo", SqlDbType.Structured, 0, ParameterDirection.Input, DtFileInfo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                //objPath.FileID = string.Empty;
                ObjDTOutPut = new DataTable();
                //    FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunSaveFileInfo|Para : IssuerNo = " + IssuerNo);
                FunInsertIntoErrorLog("BulkUpload.cs,FunSaveFileInfo", ex.Message, "");
            }
        }

        private DataTable FunCheckFileToProcess(int IssuerNo)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetFileToProcess", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                //objPath.FileID = string.Empty;
                ObjDTOutPut = new DataTable();
                //    FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunSaveFileInfo|Para : IssuerNo = " + IssuerNo);
                FunInsertIntoErrorLog("BulkUpload.cs,FunCheckFileToProcess", ex.Message, "IssuerNo=" + IssuerNo);
            }
            return ObjDTOutPut;
        }

        private DataTable FunGetPaths(int IssuerNo)
        {
            // ClsPathsBO objPath = new ClsPathsBO();

            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_GetConfigPathForFileUpload", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    //ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.Int, 0, ParameterDirection.Input, IntPara);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                //objPath.FileID = string.Empty;
                ObjDTOutPut = new DataTable();
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetPaths|Para : IssuerNo = " + IssuerNo);
                FunInsertIntoErrorLog("BulkUpload.cs,FunGetPaths", ex.Message, "IssuerNo:" + IssuerNo);
            }
            return ObjDTOutPut;
        }

        private void FunSFTP_Upload(string Server, string Port, string Sftp_User, string Sftp_Pwd, string inputPath, string OutputPath)
        {
            try
            {
                string InputFilePath = inputPath;

                string[] FilesArr = Directory.GetFiles(InputFilePath, "*.txt");

                if (FilesArr.Count() > 0)
                {
                    using (var sftp = new SftpClient(Server, Convert.ToInt32(Port), Sftp_User, Sftp_Pwd))
                    {
                        sftp.Connect();
                        foreach (string file in FilesArr)
                        {
                            string[] path = file.Split('\\');
                            try
                            {
                                //Stream fileStream = File.OpenWrite(file);
                                using (var fileStream = new FileStream(file, FileMode.Open))
                                {
                                    sftp.UploadFile(fileStream, OutputPath + path[path.Length - 1], true);
                                    fileStream.Flush();
                                    fileStream.Close();
                                    fileStream.Dispose();
                                    try
                                    {
                                        File.Delete(file);
                                    }
                                    catch (Exception ex)
                                    {
                                        FunInsertIntoErrorLog("BulkUpload.cs,FunSFTP_Upload", ex.Message, "inputPath:" + inputPath + ",outputPath:" + OutputPath);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                FunInsertIntoErrorLog("BulkUpload.cs,FunSFTP_Upload", ex.Message, "inputPath:" + inputPath + ",outputPath:" + OutputPath);
                            }
                        }
                        sftp.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("BulkUpload.cs,FunSFTP_Upload", ex.Message, "inputPath:" + inputPath + ",outputPath:" + OutputPath);
            }
        }

        private ClsReturnStatusBO FunSaveFileData(DataTable DtBulkData, string FileName, string UploadID, string OutputPath, string SPName)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = string.Empty;
            DataSet ObjDTOutPut = new DataSet();
            SqlConnection ObjConn = null;
            string strSPName = "dbo." + SPName;

            try
            {

                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                // using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_SaveBulkCustomerInfo", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(strSPName, ObjConn, CommandType.StoredProcedure))
                {
                    //  ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssurNo);
                    ObjCmd.AddParameterWithValue("@UploadID", SqlDbType.VarChar, 0, ParameterDirection.Input, UploadID);
                    ObjCmd.AddParameterWithValue("@CustInfo", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);

                    ObjDTOutPut = ObjCmd.ExecuteDataSet();
                    if (ObjDTOutPut.Tables[0].Rows.Count > 0)
                    {
                        ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Tables[0].Rows[0]["Code"]);
                        ObjResult.Description = ObjDTOutPut.Tables[0].Rows[0]["OutputDescription"].ToString();
                        ObjResult.OutPutCode = ObjDTOutPut.Tables[0].Rows[0]["OutPutCode"].ToString();
                        //on successful cif process
                        if (ObjResult.Code == 0)
                        {
                            if (ObjDTOutPut.Tables[1].Rows.Count > 0)
                            {

                                try
                                {
                                    string NewFailCIFName = FileName;
                                    string FileCreationPath = OutputPath;
                                    string strLocFile = CreateDownloadCSVFile(ObjDTOutPut.Tables[1].ToSelectedTableColumns("Result"), FileCreationPath, false, "", "", "");
                                    System.IO.File.Move(FileCreationPath + "\\" + strLocFile, FileCreationPath + "\\" + FunGetNewFileName(FileName));
                                }
                                catch (Exception ex)
                                {
                                    FunInsertIntoErrorLog("BulkUpload.cs,FunSaveFileData|Create status file", ex.Message, "FileName=" + FileName + ",UploadID=" + UploadID);

                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "BulkUpload.cs,FunSaveFileData|Create status file|Para:FileName = " + FileName + ",UploadID=" + UploadID);
                                }
                            }
                        }

                    }

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                ObjResult.OutPutCode = "1";
                ObjResult.Code = 1;
                ObjResult.Description = ex.Message;

                FunInsertIntoErrorLog("BulkUpload.cs,FunSaveFileData", ex.Message, "FileName" + FileName);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunSaveFileData|Para : FileName = " + FileName);
            }
            return ObjResult;

        }

        private ClsReturnStatusBO FunProcessFileData(ClsPathsBO obj)
        {
            ClsReturnStatusBO objStatus = new ClsReturnStatusBO();
            objStatus.Code = 1;
            bool IsInvalid = false;
            try
            {
                var fileLines = File.ReadAllLines(obj.FullFilePath);
                if ((fileLines.Count() == 0))
                {
                    IsInvalid = true;
                }
                else
                {
                    //check lines which not contains required seperator
                    var CheckLines = from csvline in fileLines
                                     let data = csvline.Split(Convert.ToChar(obj.Seperator))
                                     where (data.Length == 1) || (csvline.Contains(obj.Seperator) == false)
                                     select csvline;
                    if (CheckLines.ToList().Count() > 0)
                        IsInvalid = true;
                }

                if (IsInvalid == false)
                {
                    DataTable DtResult = GetTextFileIntoDataTable(obj.FullFilePath, string.Empty, obj.Seperator, obj.FileType);
                    if (DtResult!=null &&DtResult.Rows.Count > 0) //ATPCM224: Diksha Walunj:07/10/2017:for Reissue file upload
                    {
                        objStatus = FunSaveFileData(DtResult, obj.FileName, obj.ID, obj.OutputPath, obj.InsertValidationSP);
                    }
                }
                //on success 
                if (objStatus.Code == 0)
                {
                    File.Delete(obj.FullFilePath);
                }
                //on failed case
                else
                {
                    File.Move(obj.FullFilePath, obj.FailedPath + obj.FileName);
                    //update failed status for uploaded file
                    FunUpdateFileStatus(obj.ID);  //ATPCM224: Diksha Walunj:07/10/2017:for Reissue file upload
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("BulkUpload.cs ,FunProcessFileData", ex.Message, obj.FullFilePath);
            }
            return objStatus;
        }

        private void FunUpdateFileStatus(string  UploadID )
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Sp_UpdateFileUploadStatus", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@UploadID", SqlDbType.VarChar, 800, ParameterDirection.Input, UploadID);                  
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunUpdateFileStatus");
                FunInsertIntoErrorLog("FunUpdateFileStatus", ex.Message, string.Empty);
            }
        }

        private T BindDatatableToClass<T>(DataTable dt)
        {

            DataRow dr = dt.Rows[0];

            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object
            var ob = Activator.CreateInstance<T>();

            // Get all fields
            var fields = typeof(T).GetFields();
            foreach (var fieldInfo in fields)
            {
                if (columns.Contains(fieldInfo.Name))
                {
                    // Fill the data into the field
                    fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                }
            }

            // Get all properties
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (columns.Contains(propertyInfo.Name))
                {
                    // Fill the data into the property
                    System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                    Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                    object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                    propertyInfo.SetValue(ob, safeValue, null);

                    // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                }
            }

            return ob;
        }

        private DataTable GetTextFileIntoDataTable(string FilePath, string TableName, string delimiter, string FileType)
        {
            //The DataTable to Return
            DataTable result = new DataTable();
            try
            {

                string AllData = string.Empty;
                //Open the file in a stream reader.
                using (StreamReader s = new StreamReader(FilePath))
                {

                    //Add specific column to datatable
                    //for customer onboarding
                    if (FileType == "1")
                    {
                        result.Columns.Add("FirstName", typeof(String));

                        result.Columns.Add("LastName", typeof(String));
                        result.Columns.Add("DOB", typeof(String));
                        result.Columns.Add("MobileNo", typeof(String));
                        result.Columns.Add("Email", typeof(String));
                        result.Columns.Add("Gender", typeof(String));
                        result.Columns.Add("Nationality", typeof(String));
                        DataColumn workCol = result.Columns.Add("Passport_IdentiNo", typeof(String));
                        workCol.AllowDBNull = false;
                        workCol.Unique = false;
                        result.Columns.Add("IssueDate", typeof(String));
                        result.Columns.Add("StatementDelivery", typeof(String));
                        result.Columns.Add("HouseNo", typeof(String));
                        result.Columns.Add("StreetName", typeof(String));
                        result.Columns.Add("City", typeof(String));
                        result.Columns.Add("District", typeof(String));
                        result.Columns.Add("AccountType", typeof(String));
                        result.Columns.Add("AccountNo", typeof(String));
                        result.Columns.Add("CardPrefix", typeof(String));
                        result.Columns.Add("NameOnCard", typeof(String));
                        result.Columns.Add("Remark", typeof(String));

                        //Read the rest of the data in the file.        
                        AllData = s.ReadToEnd();
                        s.Close();
                        s.Dispose();
                    }
                    //Reissue File upload
                    else if (FileType == "2")
                    {
                        //DataColumn workCol =result.Columns.Add("ID", typeof(Int64));
                        //workCol.AutoIncrement = true;
                        //workCol.AutoIncrementSeed = 1;
                        //workCol.AutoIncrementStep = 1;
                        result.Columns.Add("OldCardNumber", typeof(String));
                        result.Columns.Add("HoldRSPCode", typeof(String));
                        result.Columns.Add("NewBINPrefix", typeof(String));
                        result.Columns.Add("Remark", typeof(String));
                        result.Columns.Add("Reason", typeof(String));
                        result.Columns.Add("Extra1", typeof(String));
                        result.Columns.Add("Extra2", typeof(String));
                        result.Columns.Add("Extra3", typeof(String));
                        //Read the rest of the data in the file.        
                        AllData = s.ReadToEnd();
                        s.Close();
                        s.Dispose();
                    }
                }
                //Split off each row at the Carriage Return/Line Feed
                //Default line ending in most windows exports.  
                //You may have to edit this to match your particular file.
                //This will work for Excel, Access, etc. default exports.
                string[] rows = AllData.Split("\r\n".ToCharArray());


                //Now add each row to the DataTable        
                foreach (string r in rows)
                {
                    if (!string.IsNullOrEmpty(r))
                    {
                        //Split the row at the delimiter.
                        string[] items = r.Split(delimiter.ToCharArray());

                        //Add the item
                        result.Rows.Add(items);
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "GetTextFileIntoDataTable|Para : BulkUploadFile = " + FilePath);

                FunInsertIntoErrorLog("BulkUpload.cs, GetTextFileIntoDataTable", ex.Message, "BulkUploadFile=" + FilePath);

                result = new DataTable();
            }
            //Return the imported data.        
            return result;
        }

        //**************** Save Error Logs ************
        private void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BULKUPLOADCONSTR"].ConnectionString);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_InsertErrorLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    if (!string.IsNullOrEmpty(parameterList))
                        sspObj.AddParameterWithValue("@parameterList", SqlDbType.VarChar, 0, ParameterDirection.Input, parameterList);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunInsertIntoErrorLog");
                //FunInsertIntoErrorLog("FunInsertIntoErrorLog", ex.Message, "", IssuerNo.ToString(), string.Empty);
            }
        }

        private void FunInsertIntoLogFile(string LogPath, Exception ex, string functionName)
        {
            try
            {
                //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
                string filename = "BulkUpload_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                string filepath = LogPath + filename;
                if (File.Exists(filepath))
                {
                    using (StreamWriter writer = new StreamWriter(filepath, true))
                    {
                        if (ex != null)
                        {
                            writer.WriteLine("——————-START————-" + DateTime.Now);
                            if (!string.IsNullOrEmpty(functionName))
                                writer.WriteLine("FunctionName: " + functionName);

                            writer.WriteLine("Error Message: " + ex.Message);
                            writer.WriteLine("Source: " + ex.Source);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            writer.WriteLine("——————-END————-" + DateTime.Now);
                        }
                        else
                        {
                            writer.WriteLine(functionName + " " + DateTime.Now);
                        }
                        writer.Close();
                    }
                }
                else
                {
                    using (StreamWriter writer = File.CreateText(filepath))
                    {
                        if (ex != null)
                        {
                            writer.WriteLine("——————-START————-" + DateTime.Now);
                            writer.WriteLine("FunctionName : " + functionName);
                            writer.WriteLine("Error Message : " + ex.Message);
                            writer.WriteLine("Source : " + ex.Source);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);
                            writer.WriteLine("——————-END————-" + DateTime.Now);
                        }
                        else
                        {
                            writer.WriteLine(functionName + " " + DateTime.Now);
                        }
                        writer.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                //  FunInsertIntoErrorLog("FunInsertIntoLogFile", Ex.Message, "LogPath=" + LogPath);
                FunInsertIntoErrorLog("BulkUpload.cs ,FunInsertIntoLogFile", Ex.Message, LogPath);

            }
        }

        //****************** Datatable functions ********************
        public string CreateDownloadCSVFile(DataTable Data, string FilePath, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            string sFileName = "";

            try
            {
                sFileName = System.IO.Path.GetRandomFileName();

                using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(FilePath + "/" + sFileName))
                {
                    // Data.Columns.Remove("");
                    swObj.Write(ConvertDataTableToCSV(Data, WithHeader, Delimeter, CustomHeader, CustomFooter));

                    swObj.Close();
                    swObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                sFileName = "";
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "CreateDownloadCSVFile|Para:FilePath =" + FilePath + "/" + sFileName);
                FunInsertIntoErrorLog("BulkUpload.cs ,CreateDownloadCSVFile", ex.Message, FilePath);
            }
            finally
            {
            }

            return sFileName;
        }

        public string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
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
                sbOutput = new StringBuilder();
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "ConvertDataTableToCSV");
            }

            return sbOutput.ToString();
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            try
            {
                var columnNames = dt.Columns.Cast<DataColumn>()
            .Select(c => c.ColumnName)
            .ToList();
                var properties = typeof(T).GetProperties();
                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();
                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name))
                        {
                            System.Reflection.PropertyInfo pI = objT.GetType().GetProperty(pro.Name);

                            Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                            object safeValue = row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], t);
                            pro.SetValue(objT, safeValue, null);
                        }
                    }
                    return objT;
                }).ToList();
            }
            catch (Exception ex)
            {
                //ErrorLogger.DBLog(ex, "DBHelper");
                throw ex;
            }
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        //*********** Classes ************
        private class ClsReturnStatusBO
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public string OutPutCode { get; set; }
            public int OutPutID { get; set; }
        }
        private class ClsPathsBO
        {
            public string ID { get; set; }
            public string BankName { get; set; }
            public string IssuerNo { get; set; }
            public string FileID { get; set; }
            public string Path { get; set; }
            public string FileName { get; set; }
            public string SFTP_Server { get; set; }
            public string SFTP_Port { get; set; }
            public string SFTP_Path { get; set; }
            public string SFTP_User { get; set; }
            public string SFTP_PWD { get; set; }
            public string FileType { get; set; }
            public string SFTP_BackUp { get; set; }
            public string InputPath { get; set; }
            public string OutputPath { get; set; }
            public string FullFilePath { get; set; }
            public string FailedPath { get; set; }
            public string SFTP_Failed { get; set; }
            public string SFTP_StatusFile { get; set; }
            public string Seperator { get; set; }
            public string InsertValidationSP { get; set; }
        }
    }
    public static class ExtentionMethods
    {
        public static DataTable ToSelectedTableColumns(this DataTable dtTable, string commaSepColumns)
        {
            String[] selectedColumns = new string[commaSepColumns.Split(',').Count()]; // = new  new[30] //{ "Column1", "Column2" };
            int i = 0;
            foreach (string columns in commaSepColumns.Split(','))
            {
                selectedColumns[i] = columns;
                i++;
            }
            return new DataView(dtTable).ToTable(false, selectedColumns);
        }
    }

}
