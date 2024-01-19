using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.CardAutomationISO
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
                String[] _files = Directory.GetFiles(objData.Local_Input, "*" + objData.FileExtension, SearchOption.TopDirectoryOnly);
                if (_files != null)
                {
                    if (_files.Length > 0)
                    {
                        foreach (var files in _files)
                        {
                            /*Check Prevoius File PRE CREATED OR NOT*/
                            if (!ModuleDAL.CheckPreviousfilestatus(objData))
                            {
                                return true;
                            }
                            string StrErrorMessages = "";
                            objData.FileName = Path.GetFileName(files);

                                                       
                            ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", "", objData.ProcessId.ToString(), "", "File Procssing started for:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                            FileInfo fi = new FileInfo(files);
                            if (!IsLocked(fi))
                            {
                                /*INSERT INTO FILEMASTER*/
                                ModuleDAL.InsertINTOFileMaster(objData);
                                if (!Convert.ToString(objData.FileID).Equals("DUPLICATE FILE", StringComparison.OrdinalIgnoreCase))
                                {
                                    //added by uddesh for card split start ATPCM-845
                                    //if (objData.isSplitBankFile)
                                    //{

                                    //    DataTable dtSpilt = ModuleDAL.FunGetSetSubFilesProcessStatus("GET", objData.FileName, Convert.ToInt32(objData.IssuerNo), Convert.ToInt32(objData.ProcessId), Convert.ToInt32(objData.FileID));
                                    //    if (dtSpilt == null) { return false; }
                                    //    if (dtSpilt.Rows[0]["Result"].ToString() == "0")
                                    //    {
                                    //        ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", "", objData.ProcessId.ToString(), "", dtSpilt.Rows[0]["STATUS"].ToString(), objData.IssuerNo.ToString(), 0);
                                    //        return false;
                                    //    }
                                    //    else
                                    //    {
                                    //        objData.OriginalFileName = dtSpilt.Rows[0]["OriginalFileName"].ToString();
                                    //        objData.OriginalfileId = Convert.ToInt32(dtSpilt.Rows[0]["OriginalfileId"]);
                                    //        objData.SubfileId = Convert.ToInt32(dtSpilt.Rows[0]["SubfileId"]);
                                    //    }
                                    //}
                                    //added by uddesh for card split enD
                                    /*STEP 5*/
                                    /*CONVERT TXT FILE TO DATATABLE*/


                                    DataTable DtRecord = ReadFile(files, objData.FileHeader, ref StrErrorMessages, objData);
                                    if (objData.StepStatus) { return false; }
                                    ModuleDAL.UpdateFileStatus(objData, 5);
                                    File.Move(files, objData.Local_Archive + Path.GetFileNameWithoutExtension(objData.FileName) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    /*STEP 6*/
                                    /*INSERT THE DATA RECORD IN DB AND VALIDATE THE RECORD*/
                                    if (DtRecord.Rows.Count > 0)
                                    {
                                        ModuleDAL.InsertRecord(objData, DtRecord);
                                        if (objData.StepStatus) { return false; }
                                        ModuleDAL.UpdateFileStatus(objData, 6);
                                    }

                                    /*STEP 7*/
                                    /*REOCRD FOR ISO  AND CALL CARD API*/
                                    DataTable DtISORecord = ModuleDAL.GetRecordFORISOProcessing(objData);
                                    
                                    if (objData.StepStatus) { return false; }
                                    if (DtISORecord.Rows.Count > 0)
                                    {
                                        Parallel.ForEach(DtISORecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                        drow =>
                                        {
                                        new GenerateCardAPIRequest().CallCardAPIService(drow, DtISORecord, objData);
                                        });
                                        
                                        /*UPDATE THE SWITCH RSP*/
                                        /*START*/
                                        ModuleDAL.UpdateSwitchRspStatus(objData);
                                        
                                        /*END*/
                                        /*CHECK ANY RECORD FAILED WITH RESP CODE 42 FOR LINKING*/
                                        DataTable dtAccountLinikingRecord = ModuleDAL.GETAccountlinkingRecords(objData);
                                        if (dtAccountLinikingRecord.Rows.Count > 0)
                                        {

                                            objData.APIRequestType = "AccountLinkingDelinking";
                                            objData.IsNewCardGenerate = false;
                                            Parallel.ForEach(dtAccountLinikingRecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                            drow =>
                                            {
                                                new GenerateCardAPIRequest().CallCardAPIService(drow, dtAccountLinikingRecord, objData);
                                            });
                                        }
                                        
                                        /*UPDATE THE SWITCH RSP*/
                                        /*START*/
                                        ModuleDAL.UpdateSwitchRspStatus(objData);
                                        /*END*/
                                        if (objData.StepStatus) { return false; }
                                        ModuleDAL.UpdateFileStatus(objData, 7);
                                        if (objData.StepStatus) { return false; }
                                        
                                        /*STEP 8*/
                                        /*BIN-PREFIX FOR  STD 4 AND CALL STD 4*/
                                        ////change by uddesh for Customer data update to skip stanndard four run Start ATPCM-845
                                        if (objData.isStandardFourRun)
                                        {
                                            ModuleDAL.RunSTD4(objData);
                                            ModuleDAL.UpdateFileStatus(objData, 8);
                                            if (objData.StepStatus) { return false; }
                                        }
                                        ////change by uddesh for Customer data update to skip stanndard four run end
                                        
                                       
                                    }
                                    /*PRE START*/
                                    /*RUN PRE  DLL*/
                                    new CardAutomation().FunInsertTextLog("PRE Processing Started FOR !" + objData.FileID, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                                    ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "PRE PROCESSING STARTED FOR:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                    PREFileGeneration.PREFile ObjPRE = new PREFileGeneration.PREFile();
                                    ObjPRE.Process(Convert.ToInt32(objData.IssuerNo), objData.FileID, objData.FileName, objData.ProcessId);
                                    //ObjPRE.Process(Convert.ToInt32(objData.IssuerNo), objData.FileID, objData.FileName, objData.ProcessId, objData.isSplitBankFile, objData.OriginalFileName, objData.OriginalfileId, objData.SubfileId, objData.isSplitPREfile); //change by uddesh ATPCM-845

                                    ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "PRE PROCESSING ENDED FOR:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                    /*PRE END*/


                                }

                                else
                                {
                                    /*MARK FILE AS DUPLICATE*/
                                    if (File.Exists(Path.Combine(objData.Local_Input, files)))
                                    {
                                        File.Move(files, objData.Local_Failed + Path.GetFileNameWithoutExtension(objData.FileName) + "_DuplicateFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                                    }
                                }
                                new CardAutomation().FunInsertTextLog("File PROCESS ENDED FOR !" + objData.FileName, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                                ModuleDAL.FunInsertIntoErrorLog("FileProcessor.process", objData.FileID, objData.ProcessId.ToString(), "", "File Procssing ENDED for:" + objData.FileName, objData.IssuerNo.ToString(), 1);

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.Move(objData.Local_Input + objData.FileName, objData.Local_Failed + Path.GetFileNameWithoutExtension(objData.FileName) + "_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                new ModuleDAL().FunInsertIntoErrorLog("FileProcessor.process", objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                DataTable dtSpilt = ModuleDAL.FunGetSetSubFilesProcessStatus("FAIL", objData.FileName, Convert.ToInt32(objData.IssuerNo), Convert.ToInt32(objData.ProcessId), Convert.ToInt32(objData.FileID));
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
               new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", ex.ToString(), "","", 0);
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
        public DataTable ReadFile(string file, string FileHeader, ref string StrErrorMessages,ConfigDataObject ObjData)
        {
            ModuleDAL ModuleDAL = new ModuleDAL();
            new CardAutomation().FunInsertTextLog("FILE READER STARTED  FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo),ObjData.ErrorLogPath);
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader Started for:" + ObjData.FileName, ObjData.IssuerNo.ToString(), 1);
            
            
            int ErrorRecordcount = 0;
            string [] ErrorRecords = new string [File.ReadLines(file).Count()];
            var _dataTable = new DataTable();
            var headers = FileHeader.Split('|');
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
                        if (record.Split('|').Length != _dataTable.Columns.Count)
                        {


                            /*IF RECORD ARE LESS THEN HEADER*/
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

                int   count = _dataTable.Columns.Count;
                int filedCount = ModuleDAL.GETMaxFileHeader(ObjData);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "MAX header COUNT:|" + filedCount.ToString()+"Column Count :|"+count.ToString(), ObjData.IssuerNo.ToString(), 1);
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
                    UploadRejectedRecord(ObjData,ErrorRecords.Where(x => !string.IsNullOrEmpty(x)).ToArray());
                }
                
                
            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.ErrorDesc = "FILE READ FAILED WITH REASON | " + ex.ToString();
                ObjData.StepStatus = true;
            }
            ObjData.Filestatus = "FILE READ COMPELETED";
            new CardAutomation().FunInsertTextLog("FILE READER  ENDED FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo),ObjData.ErrorLogPath);
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader ENDED for:" + ObjData.FileName, ObjData.IssuerNo.ToString(), 1);
            
            return _dataTable;
        }
        public void UploadRejectedRecord(ConfigDataObject ObjData,string [] ErrorData)
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
               objSFTP.destinationPath = ObjData.Local_Input;
               objSFTP.FileExtension = ObjData.FileExtension;
               objSFTP.SourcePath = ObjData.Local_Failed;
               objSFTP.destinationPath = ObjData.Bank_Failed;
               objSFTP.Deletebasefilepath = ObjData.Bank_Input;
               objSFTP.FileName = ErrorRecordFileName + ".txt";
               
               
               File.WriteAllLines(ObjData.Local_Failed +objSFTP.FileName, ErrorData);
               if (ObjData.IsPGP)
               {
                   
                   if (!new CardAutomation().PGP_Encrypt(ObjData.PGP_KeyName, ObjData.Local_Failed + objSFTP.FileName, ObjData.Local_Failed +ErrorRecordFileName + ".pgp", ObjData.FailedInputFilePGPPublicKeyPath, ObjData.IssuerNo))
                   {
                       throw new Exception("Error while PGP ENCRYPTION FILE");
                   }
                   objSFTP.FileName = ErrorRecordFileName + ".pgp";
               }
               
               if (!SearchFile.UploadFile(objSFTP, Convert.ToInt32(ObjData.IssuerNo),ObjData.ProcessId,ObjData.FileID))
               {
                   throw new Exception("Error while Uploading the SFTP FILE");
               }

           }
            catch(Exception ex)
           {
            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);  
           }
           
       }
        private string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter, ConfigDataObject objData)
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
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
                sbOutput = new StringBuilder();
            }

            return sbOutput.ToString();
        }
    }
}
