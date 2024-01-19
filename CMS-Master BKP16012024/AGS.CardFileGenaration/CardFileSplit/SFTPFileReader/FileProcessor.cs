using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileSplit
{
    class FileProcessor
    {



        #region This Method is used to check if the file is process by other function before the file reading start .Create by Gufran Khan.
        public bool process(ConfigDataObject objData)
        {
            ModuleDAL ModuleDAL = new ModuleDAL();
            try
            {

                string _RejectFileName = string.Empty;
                String[] _files = Directory.GetFiles(objData.Local_Input + "\\", "*" + objData.FileExtension, SearchOption.TopDirectoryOnly);

                //String[] _files = Directory.GetFiles(objData.Local_Input + "\\" + objData.IssuerNo + "\\" + objData.ProcessId + "\\", "*" + objData.FileExtension, SearchOption.TopDirectoryOnly);
                if (_files != null)
                {
                    if (_files.Length > 0)
                    {

                        foreach (var files in _files)
                        {
                            ///*Check Prevoius File PRE CREATED OR NOT*/
                            //if (!ModuleDAL.CheckPreviousfilestatus(objData))
                            //{
                            //    return true;
                            //}
                            string StrErrorMessages = "";
                            objData.FileName = Path.GetFileName(files);

                            ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", "", objData.ProcessId.ToString(), "", "File Procssing started for:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                            FileInfo fi = new FileInfo(files);
                            if (!IsLocked(fi))
                            {
                                /*INSERT INTO FILEMASTER*/
                                ModuleDAL.InsertINTOFileMaster(objData, "INSERT");
                                if (!Convert.ToString(objData.FileID).Equals("DUPLICATE FILE", StringComparison.OrdinalIgnoreCase))
                                {
                                    /*STEP 5*/
                                    /*CONVERT TXT FILE TO DATATABLE*/


                                    DataTable DtRecord = ReadFile(files, objData.FileHeader, ref StrErrorMessages, objData);
                                    objData.TotalCount = DtRecord.Rows.Count;
                                    ModuleDAL.InsertINTOFileMaster(objData, "UPDATECOUNT");
                                    if (objData.StepStatus) { return false; }

                                    File.Move(files, SearchFile.fn_Create_Directory(objData.Local_Archive+ "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    //File.Move(files, SearchFile.fn_Create_Directory(objData.Local_Archive + "\\" + objData.IssuerNo + "\\" + objData.ProcessId + "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    /*STEP 6*/
                                    /*INSERT THE DATA RECORD IN DB AND VALIDATE THE RECORD*/
                                    if (DtRecord.Rows.Count > 0)
                                    {
                                        DataSet dtSplitData = ModuleDAL.InsertRecord(objData, DtRecord);
                                        ModuleDAL.InsertINTOFileMaster(objData, "UPDATE");

                                        if (objData.StepStatus)
                                        {

                                            return false;
                                        }
                                        if (dtSplitData != null)
                                        {
                                            if (dtSplitData.Tables.Count > 0)
                                            {
                                                foreach (DataTable dt in dtSplitData.Tables)
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        System.Threading.Thread.Sleep(1000);
                                                        DataTable subDt = dt.Copy();

                                                        subDt.Columns.Remove("FileID");
                                                        subDt.Columns.Remove("FileName");
                                                        subDt.Columns.Remove("IssuerNo");
                                                        subDt.Columns.Remove("SubFileName");
                                                        subDt.Columns.Remove("Local_input");
                                                        
                                                        subDt.AcceptChanges();

                                                        String subLocalInput =Convert.ToString(dt.Rows[0]["Local_input"]); //ModuleDAL.GetSubFileLocalPath(objData, "GET", dt.Rows[0]["ProcessingCode"].ToString(), dt.Rows[0]["SubFileName"].ToString());
                                                        if (subLocalInput == "") { return false; }
                                                        String cFileStatus = new FileSplit().CreateDownloadCSVFileWithFileName(subDt,SearchFile.fn_Create_Directory(subLocalInput)  + dt.Rows[0]["SubFileName"].ToString(), false, "|","", "");
                                                        if (cFileStatus == null) { return false; }
                                                        objData.TotalCount = dt.Rows.Count;
                                                        objData.Filestatus = "Sub File Created";
                                                        ModuleDAL.InsertINTOSubFileStatus(objData, "Insert", dt.Rows[0]["SubFileName"].ToString());
                                                        if (objData.StepStatus)
                                                        {
                                                            return false;
                                                        }
                                                        
                                                    }
                                                    else
                                                    {

                                                        ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No data to split files:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                                        return false;
                                                    }

                                                }
                                                objData.Filestatus = "All Sub File Created";
                                                ModuleDAL.InsertINTOFileMaster(objData, "UPDATE");
                                                if (objData.StepStatus)
                                                {
                                                    return false;
                                                }


                                            }
                                            else
                                            {
                                                ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No data to split files:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                                return false;
                                            }


                                        }
                                        else
                                        {

                                            ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No data to split files:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                            return false;
                                        }

                                    }
                                    else
                                    {

                                        ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No record in original file:" + objData.FileName, objData.IssuerNo.ToString(), 1);

                                        return false;
                                    }

                                    /*STEP 7*/
                                    /*REOCRD FOR ISO  AND CALL CARD API*/

                                }

                                else
                                {
                                    /*MARK FILE AS DUPLICATE*/
                                    if (File.Exists(Path.Combine(objData.Local_Input, files)))
                                    {
                                        //File.Move(files, SearchFile.fn_Create_Directory(objData.Local_Failed + "\\" + objData.IssuerNo + "\\" + objData.ProcessId + "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_DuplicateFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                        File.Move(files, SearchFile.fn_Create_Directory(objData.Local_Failed + "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_DuplicateFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);

                                    }
                                }
                                ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "File Procssing ENDED for:" + objData.FileName, objData.IssuerNo.ToString(), 1);

                            }
                            else {
                                ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "file is locaked:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                break;
                            }

                        }
                    }
                    else
                    {
                        ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No file in local input path:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                        return false;
                    }
                }
                else
                {
                    ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "No file in local input path:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                File.Move(objData.Local_Input  + "\\" + objData.FileName, SearchFile.fn_Create_Directory(objData.Local_Failed + "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                //File.Move(objData.Local_Input + "\\" + objData.IssuerNo + "\\" + objData.ProcessId + "\\" + objData.FileName, SearchFile.fn_Create_Directory(objData.Local_Failed + "\\" + objData.IssuerNo + "\\" + objData.ProcessId + "\\") + Path.GetFileNameWithoutExtension(objData.FileName) + "_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);

                new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                objData.ErrorDesc = ex.ToString();
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
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", ex.ToString(), "", "", 0);
                return true;
            }
        }

        #region This Method is used to read the file .Create by Gufran Khan.
        public static IEnumerable<string> ReadAsLines(string filename)
        {

            using (var reader = new StreamReader(filename))

                while (!reader.EndOfStream)
                    yield return reader.ReadLine();


        }
        #endregion
        public DataTable ReadFile(string file, string FileHeader, ref string StrErrorMessages, ConfigDataObject ObjData)
        {
            ModuleDAL ModuleDAL = new ModuleDAL();
            ModuleDAL.FunInsertIntoErrorLog("ReadFile", string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader Started for:" + (string.IsNullOrEmpty(ObjData.FileName) ? "" : ObjData.FileName), ObjData.IssuerNo.ToString(), 1);

            int ErrorRecordcount = 0;
            string[] ErrorRecords = new string[File.ReadLines(file).Count()];
            var _dataTable = new DataTable();
            var headers = FileHeader.Split('|');
            foreach (var header in headers)
                _dataTable.Columns.Add(header);
            try
            {

                var reader = ReadAsLines(file);
                //this assume the first record is filled with the column names   

                var records = reader;
                //if (ObjData.ProcessId.Equals("4", StringComparison.OrdinalIgnoreCase) || ObjData.ProcessId.Equals("5", StringComparison.OrdinalIgnoreCase))
                //{
                //    records = reader.Skip(1);
                //}
                foreach (var record in records)
                {

                    if (record != "\t" && record != " ")
                        if (record.Split('|').Length != _dataTable.Columns.Count)
                        {
                        
                                if ((_dataTable.Columns.Count) > (record.Split('|').Length))
                                {
                                    //for (int i = 0; i < (_dataTable.Columns.Count) - (record.Split('|').Length); i++)
                                    //{
                                    ErrorRecords[ErrorRecordcount] = record + "|LESS FIELD(|) PRESENT IN RECORD";
                                    //}
                                    //_dataTable.Rows.Add(ErrorRecords.Split('|'));
                                }
                                /*IF RECORD ARE MORE THEN HEADER*/
                                else
                                {
                                    //for (int i = 0; i < (record.Split('|').Length)-(_dataTable.Columns.Count); i++)
                                    //{
                                    ErrorRecords[ErrorRecordcount] = record + "|EXTRA FIELD(|) PRESENT IN RECORD";

                                    //}

                                }

                                ErrorRecordcount++;
                          


                        }
                        else
                        {
                            _dataTable.Rows.Add(record.Split('|'));
                        }


                }

                int count = _dataTable.Columns.Count;
                int filedCount = ModuleDAL.GETMaxFileHeader(ObjData);
                new ModuleDAL().FunInsertIntoErrorLog("ReadFile", ObjData.FileID, ObjData.ProcessId.ToString(), "", "MAX header COUNT:|" + filedCount.ToString() + "Column Count :|" + count.ToString(), ObjData.IssuerNo.ToString(), 1);
                if (count < filedCount)
                {
                    int loopcount = filedCount - count;
                    for (int i = 0; i < loopcount; i++)
                    {
                        _dataTable.Columns.Add("Default_" + i.ToString());
                    }

                }
                if (ErrorRecordcount > 0)
                {
                    UploadRejectedRecord(ObjData, ErrorRecords.Where(x => !string.IsNullOrEmpty(x)).ToArray());

                }


            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog("ReadFile", ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.ErrorDesc = "FILE READ FAILED WITH REASON | " + ex.ToString();
                ObjData.StepStatus = true;
                ObjData.Filestatus = "FILE READ Exception";
            }
            ObjData.Filestatus = "FILE READ COMPELETED";
            ModuleDAL.FunInsertIntoErrorLog("ReadFile", string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader Ended for:" + (string.IsNullOrEmpty(ObjData.FileName) ? "" : ObjData.FileName), ObjData.IssuerNo.ToString(), 1);
            ObjData.StepStatus =false;
            return _dataTable;
        }


        public void UploadRejectedRecord(ConfigDataObject ObjData, string[] ErrorData)
        {

            try
            {
                string ErrorRecordFileName = Path.GetFileNameWithoutExtension(ObjData.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + "_ErrorRecord";
                SFTPDataObject objSFTP = new SFTPDataObject();
                objSFTP.ServerIP = ObjData.Bank_SFTPServer;
                objSFTP.ServerPort = Convert.ToInt32(ObjData.Bank_SFTPPort);
                objSFTP.Username = ObjData.Bank_SFTPUser;
                objSFTP.Password = ObjData.Bank_SFTPPassword;
                objSFTP.SourcePath = ObjData.Bank_Input;
                objSFTP.destinationPath = ObjData.Local_Input + "\\" + ObjData.IssuerNo + "\\" + ObjData.ProcessId + "\\";
                objSFTP.FileExtension = ObjData.FileExtension;
                objSFTP.SourcePath = ObjData.Local_Failed;
                objSFTP.destinationPath = ObjData.Bank_Failed;
                objSFTP.Deletebasefilepath = ObjData.Bank_Input;
                objSFTP.FileName = ErrorRecordFileName + ".txt";
                objSFTP.SSH_Private_key_file_Path = ObjData.SSH_Private_key_file_Path;
                objSFTP.passphrase = ObjData.passphrase;

                File.WriteAllLines(ObjData.Local_Failed + objSFTP.FileName, ErrorData);
                if (ObjData.isstatusFileINPGP)
                {

                    if (!new CardAutomation().PGP_Encrypt(ObjData.PGP_KeyName, ObjData.Local_Failed + objSFTP.FileName, ObjData.Local_Failed + ErrorRecordFileName + ".pgp", ObjData.StatusFilePPGPPublicKeyPath, ObjData.IssuerNo))
                    {
                        throw new Exception("Error while PGP ENCRYPTION FILE");
                    }
                    objSFTP.FileName = ErrorRecordFileName + ".pgp";
                }

                if (!SearchFile.UploadFile(objSFTP, Convert.ToInt32(ObjData.IssuerNo), ObjData.ProcessId, ObjData.FileID))
                {
                    throw new Exception("Error while Uploading the SFTP FILE");
                }

            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }

        }
    }
}
