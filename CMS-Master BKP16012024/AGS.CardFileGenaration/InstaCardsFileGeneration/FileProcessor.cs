using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCardsFileGeneration
{
    class FileProcessor
    {



        #region This Method is used to check if the file is process by other function before the file reading start .Create by Gufran Khan.
        public bool process(SFTPDataObject objData,String IssuerNo)
        {
            ModuleDAL ModuleDAL = new ModuleDAL();
            try
            {

                string _RejectFileName = string.Empty;
                String[] _files = Directory.GetFiles(objData.destinationPath, "*" + objData.FileExtension, SearchOption.TopDirectoryOnly);
                if (_files != null)
                {
                    if (_files.Length > 0)
                    {
                        foreach (var files in _files)
                        {

                            string StrErrorMessages = "";
                            objData.FileName = Path.GetFileName(files);


                            new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", "", objData.processid, "", "File PROCESS STARTED FOR !" + objData.FileName, IssuerNo, 1);

                            FileInfo fi = new FileInfo(files);
                            if (!IsLocked(fi))
                            {
                                /*INSERT INTO FILEMASTER*/
                                if (!ModuleDAL.InsertINTOFileMaster(objData, IssuerNo))
                                {
                                    return false;
                                }
                                if (!Convert.ToString(objData.FileID).Equals("DUPLICATE FILE", StringComparison.OrdinalIgnoreCase))
                                {



                                    DataTable DtRecord = ReadFile(files, objData.FileHeader, ref StrErrorMessages, objData,IssuerNo);
                                   
                                    File.Move(files, objData.Local_Archive + Path.GetFileNameWithoutExtension(objData.FileName) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    /*STEP 6*/
                                    /*INSERT THE DATA RECORD IN DB AND VALIDATE THE RECORD*/
                                    if (DtRecord.Rows.Count > 0)
                                    {

                                        DataTable dtRes= ModuleDAL.InsertRecord(objData, DtRecord, IssuerNo);

                                        if (dtRes is null)

                                        {

                                            return false;

                                        }


                                        if (dtRes.Rows.Count > 0)
                                        {
                                            string ReportType = "InstaRequest";
                                            string FileStatusName = "Status_" + Path.GetFileNameWithoutExtension(objData.FileName.Trim()) + "_" + ReportType + DateTime.Now.ToString("yyyyMMdd") + ".csv";

                                            new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process",objData.FileID, objData.processid, "", "record found to download status file", IssuerNo, 1);

                                            string TemproryLocationPath = objData.Local_Insta_Status_file; //"";//local path from web.config Appkey setting
                                            string localFilePath = TemproryLocationPath + FileStatusName;
                                            //string RemoteFilePath = objpath.Bank_Output + "/" + FileStatusName;
                                            using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(localFilePath))
                                            {
                                                // Data.Columns.Remove("");
                                                swObj.Write(ConvertDataTableToCSV(dtRes, true, ",", "", "",objData,IssuerNo));
                                                swObj.Close();
                                                swObj.Dispose();
                                            }
                        
                                            string RemoteFilePath = objData.Bank_Insta_Output + "/" + FileStatusName;

                                            ConfigDataObject ObjCon = new ConfigDataObject();
                                            ObjCon.Bank_SFTPUser = objData.Username;
                                            ObjCon.Bank_SFTPPassword = objData.Password;
                                            ObjCon.Bank_SFTPPort = objData.ServerPort.ToString();
                                            ObjCon.Bank_SFTPServer = objData.ServerIP;
                                            ObjCon.FileExtension = ".csv";
                                            ObjCon.IssuerNo = IssuerNo.ToString();
                                            ObjCon.Bank_Input = objData.Bank_Insta_Output;
                                            ObjCon.SSH_Private_key_file_Path = objData.SSH_Private_key_file_Path;
                                            ObjCon.SSH_Private_key_file_Path = objData.passphrase;
                                            ObjCon.Local_Input = objData.Local_Insta_Status_file;
                                            ObjCon.id = objData.FileID;
                                            ObjCon.FileID = objData.FileID;
                                            ObjCon.FileName ="";

                                            //if (searchfile.uploadfile(objcon))
                                            //{
                                            //    new moduledal().funinsertintoerrorlog("fileprocessor.process", objdata.fileid, objdata.processid, "", "status file uploaded on sftp:" + remotefilepath, issuerno, 1);
                                            //    //filestatusname 
                                            //}


                                        }
                                        
                                        //ModuleDAL.UpdateFileStatus(objData, 6);
                                    }

     

                                }

                                else
                                {
                                    new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", "", objData.processid, "", "DuplicateFile !" + objData.FileName, IssuerNo, 1);
                                    /*MARK FILE AS DUPLICATE*/
                                    if (File.Exists(Path.Combine(objData.destinationPath, files)))
                                    {
                                        File.Move(files, objData.Local_Failed + Path.GetFileNameWithoutExtension(objData.FileName) + "_DuplicateFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    }
                                }
                                new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", "", objData.processid, "", "File PROCESS ENDED FOR !" + objData.FileName, IssuerNo, 1);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.Move(objData.destinationPath + objData.FileName, objData.Local_Failed + Path.GetFileNameWithoutExtension(objData.FileName) + "_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", "", objData.processid, ex.ToString(),"", IssuerNo, 0);

                return false;
            }
            return true;
        }
        #endregion

        public bool IsLocked(FileInfo f)
        {
            try
            {
                string fpath = f.FullName;
                FileStream fs = File.OpenWrite(fpath);
                fs.Close();
                return false;
            }

            catch (Exception ex)
            {
               new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", ex.ToString(), "","", 0);
                return true;
            }
        }


        public static IEnumerable<string> ReadAsLines(string filename)
        {

            using (var reader = new StreamReader(filename))

                while (!reader.EndOfStream)
                    yield return reader.ReadLine();

        }
      
        public DataTable ReadFile(string file, string FileHeader, ref string StrErrorMessages,SFTPDataObject ObjData, string IssuerNo)
        {
            ModuleDAL ModuleDAL = new ModuleDAL();

            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.processid.ToString(), "", "File Reader Started for:" + ObjData.FileName, IssuerNo, 1);
            
            
            int ErrorRecordcount = 0;
            string [] ErrorRecords = new string [File.ReadLines(file).Count()];
            var _dataTable = new DataTable();
            var headers = FileHeader.Split(',');
            foreach (var header in headers)
                _dataTable.Columns.Add(header);
            try
            {

                var reader = ReadAsLines(file);
                //this assume the first record is filled with the column names   
                var records = reader;

                foreach (var record in records)
                {
                    if (record != "\t" && record != "")
                        if (record.Split(',').Length != _dataTable.Columns.Count)
                        {


                            /*IF RECORD ARE LESS THEN HEADER*/
                            if ((_dataTable.Columns.Count) > (record.Split(',').Length))
                            {
                                //for (int i = 0; i < (_dataTable.Columns.Count) - (record.Split('|').Length); i++)
                                //{
                                ErrorRecords[ErrorRecordcount] = record + ",LESS FIELD(,) PRESENT IN RECORD";
                                //}
                                //_dataTable.Rows.Add(ErrorRecords.Split('|'));
                            }
                            /*IF RECORD ARE MORE THEN HEADER*/
                            else
                            {
                                //for (int i = 0; i < (record.Split('|').Length)-(_dataTable.Columns.Count); i++)
                                //{
                                ErrorRecords[ErrorRecordcount] = record + ",EXTRA FIELD(,) PRESENT IN RECORD";

                                //}

                            }

                            ErrorRecordcount++;

                        }
                        else
                        {
                            _dataTable.Rows.Add(record.Split(','));
                        }

                    
                }

                int   count = _dataTable.Columns.Count;
                int filedCount = ObjData.MaxHeaderCount;
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.processid.ToString(), "", "MAX header COUNT:|" + filedCount.ToString(), IssuerNo.ToString(), 1);
                if (count < filedCount)
                {
                    int loopcount = filedCount - count;
                    for (int i = 0; i < loopcount; i++)
                    {
                        _dataTable.Columns.Add("Default_" + i.ToString());
                    }

                }
                if (ErrorRecordcount>0)
                {

                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.processid.ToString(), "", "Invalid no of columns found in file.No Column's required are " + filedCount.ToString(), IssuerNo,0);

                    //UploadRejectedRecord(ObjData,ErrorRecords.Where(x => !string.IsNullOrEmpty(x)).ToArray());
                }


            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.processid, ex.ToString(), "", IssuerNo, 0);
                return null;
            }
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.processid.ToString(), "", "File Reader ENDED for:" + ObjData.FileName, IssuerNo, 1);
            
            return _dataTable;
        }
        private string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter, SFTPDataObject objData,String IssuerNo)
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
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID.ToString(), objData.processid, ex.ToString(), "", IssuerNo.ToString(), 0);
                sbOutput = new StringBuilder();
            }

            return sbOutput.ToString();
        }
    }
}
