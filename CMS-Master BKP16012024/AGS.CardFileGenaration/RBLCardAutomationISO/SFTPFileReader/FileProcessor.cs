using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.RBLCardAutomationISO
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

                            new CardAutomation().FunInsertTextLog("File PROCESS STARTED FOR !" + objData.FileName, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", objData.ProcessId.ToString(), "", "File Procssing started for:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                            FileInfo fi = new FileInfo(files);
                            if (!IsLocked(fi))
                            {
                                /*INSERT INTO FILEMASTER*/
                                ModuleDAL.InsertINTOFileMaster(objData);
                                if (!Convert.ToString(objData.FileID).Equals("DUPLICATE FILE", StringComparison.OrdinalIgnoreCase))
                                {
                                    /*STEP 5*/
                                    /*CONVERT TXT FILE TO DATATABLE*/

                                    DataTable DtRecord = ReadFile(files, objData.FileHeader, ref StrErrorMessages, objData);
                                    if (objData.StepStatus) { return false; }
                                    ModuleDAL.UpdateFileStatus(objData, 5);

                                    File.Move(files, objData.Local_Archive + Path.GetFileNameWithoutExtension(objData.FileName) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension); ///ATPCM-862 Added by uddesh
                                   
                                    /*STEP 6*/
                                    /*INSERT THE DATA RECORD IN DB AND VALIDATE THE RECORD*/
                                    if (DtRecord.Rows.Count > 0)
                                    {
                                       
                                        ModuleDAL.InsertRecord(objData, DtRecord);
                                        if (objData.StepStatus) { return false; }
                                        ModuleDAL.UpdateFileStatus(objData, 6);
                                    }
                                    ///ATPCM-862 Added by uddesh start 
                                    else {
                                        objData.Filestatus = "NO RECORD IN FILE/ERROR WHILE READING RECORDS";
                                        ModuleDAL.UpdateFileStatus(objData, 9);
                                        return false;
                                    } ///ATPCM-862 Added by uddesh end

                                    /*STEP 7*/
                                    /*REOCRD FOR ISO  AND CALL CARD API*/
                                    DataTable DtISORecord = ModuleDAL.GetRecordFORISOProcessing(objData);
                                   if (objData.StepStatus) { return false; }
                                    if (DtISORecord.Rows.Count > 0)
                                    {
                                        // only for cardgeneration and insta card generation
                                        if (((objData.ProcessId.Equals("3", StringComparison.OrdinalIgnoreCase)) || (objData.ProcessId.Equals("8", StringComparison.OrdinalIgnoreCase)) || (objData.ProcessId.Equals("4", StringComparison.OrdinalIgnoreCase))|| (objData.ProcessId.Equals("5", StringComparison.OrdinalIgnoreCase))) && (objData.IssuerNo.Equals("1", StringComparison.OrdinalIgnoreCase)))
                                        {
                                            DataTable CardConfig = ModuleDAL.GetCardConfig(objData);
                                            //set data from table
                                            if (CardConfig.Rows.Count > 0)
                                            {
                                                Parallel.ForEach(DtISORecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                                drow =>
                                                {
                                                    new RBLCardGenerationNewISOLogic().Process(drow, DtISORecord, objData, CardConfig);
                                                });
                                                //new RBLCardGenerationNewISOLogic().Process(DtISORecord.Rows[0], DtISORecord, objData, CardConfig);
                                            }
                                        }
                                        else
                                        {
                                            Parallel.ForEach(DtISORecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                            drow =>
                                            {
                                                new GenerateCardAPIRequest().CallCardAPIService(drow, DtISORecord, objData);
                                            });
                                        }
                                        /*UPDATE THE SWITCH RSP*/
                                        /*START*/
                                        ModuleDAL.UpdateSwitchRspStatus(objData);
                                        /*END*/
                                        /*CHECK ANY RECORD FAILED WITH RESP CODE 42 FOR LINKING*/
                                        DataTable dtAccountLinikingRecord = ModuleDAL.GETAccountlinkingRecords(objData);
                                        if (dtAccountLinikingRecord.Rows.Count > 0)
                                        {
                                            if (((objData.ProcessId.Equals("3", StringComparison.OrdinalIgnoreCase)) || (objData.ProcessId.Equals("8", StringComparison.OrdinalIgnoreCase))) && (objData.IssuerNo.Equals("1", StringComparison.OrdinalIgnoreCase)))
                                            {
                                                objData.APIRequestType = "AccountLinkingDelinking";
                                                DataTable CardConfig = ModuleDAL.GetCardConfig(objData);
                                                objData.IsNewCardGenerate = false;
                                                if (CardConfig.Rows.Count > 0)
                                                {
                                                    Parallel.ForEach(dtAccountLinikingRecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                                    drow =>
                                                    {
                                                        new RBLCardGenerationNewISOLogic().Process(drow, dtAccountLinikingRecord, objData, CardConfig);
                                                    });

                                                }
                                            }
                                            else
                                            {
                                                Parallel.ForEach(dtAccountLinikingRecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                                drow =>
                                                {
                                                    new GenerateCardAPIRequest().CallCardAPIService(drow, dtAccountLinikingRecord, objData);
                                                });

                                            }
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
                                        ///*Added for international usage RBL-ATPCM-862* START/
                                        if (objData.isStandardFourRun)
                                        {
                                            ModuleDAL.RunSTD4(objData);
                                        }
                                        ///*Added for international usage RBL-ATPCM-862* END/
                                        ModuleDAL.UpdateFileStatus(objData, 8);
                                        if (objData.StepStatus) { return false; }

                                    }

                                    /*RUN PRE  DLL*/
                                    new CardAutomation().FunInsertTextLog("PRE Processing Started FOR !" + objData.FileID, Convert.ToInt32(objData.IssuerNo), objData.ErrorLogPath);
                                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), "", "PRE PROCESSING STARTED FOR:" + objData.FileName, objData.IssuerNo.ToString(), 1);
                                    PREFileGenerationRBL.PREFile ObjPRE = new PREFileGenerationRBL.PREFile();
                                    ObjPRE.Process(Convert.ToInt32(objData.IssuerNo), objData.FileID, objData.FileName, objData.ProcessId);
                                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), "", "PRE PROCESSING ENDED FOR:" + objData.FileName, objData.IssuerNo.ToString(), 1);



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
                                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), "", "File Procssing ENDED for:" + objData.FileName, objData.IssuerNo.ToString(), 1);

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.Move(objData.Local_Input + objData.FileName, objData.Local_Failed + Path.GetFileNameWithoutExtension(objData.FileName) + "_Error_" + DateTime.Now.ToString("yyyyMMddHHmmss") + objData.FileExtension);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
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
            new CardAutomation().FunInsertTextLog("FILE READER STARTED  FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader Started for:" + (string.IsNullOrEmpty(ObjData.FileName) ? "" : ObjData.FileName), ObjData.IssuerNo.ToString(), 1);


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
                if (ObjData.ProcessId.Equals("4", StringComparison.OrdinalIgnoreCase) || ObjData.ProcessId.Equals("5", StringComparison.OrdinalIgnoreCase))
                {
                    records = reader.Skip(1);
                }
                foreach (var record in records)
                {

                    if (record != "\t" && record != " ")
                        if (record.Split('|').Length != _dataTable.Columns.Count)
                        {


                            /*IF RECORD ARE LESS THEN HEADER*/

                            if (ObjData.ProcessId == "5")
                            {
                                if ((_dataTable.Columns.Count) > (record.Split('|').Length))
                                {
                                    // added by uddesh on 05-11-2019
                                    if ((_dataTable.Columns.Count) - (record.Split('|').Length) == 1)
                                    {
                                        new CardAutomation().FunInsertTextLog("Adding magStrip card as 0 column !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                                        string ExtraRecord = record + "|";
                                        _dataTable.Rows.Add(ExtraRecord.Split('|'));
                                    }
                                    else {
                                        ErrorRecords[ErrorRecordcount] = record + "|LESS FIELD(|) PRESENT IN RECORD";
                                        ErrorRecordcount++;
                                    }
                                   
                                }
                                /*IF RECORD ARE MORE THEN HEADER*/
                                else
                                {
                                    //for (int i = 0; i < (record.Split('|').Length)-(_dataTable.Columns.Count); i++)
                                    //{
                                    ErrorRecords[ErrorRecordcount] = record + "|EXTRA FIELD(|) PRESENT IN RECORD";

                                    //}
                                    ErrorRecordcount++;
                                }

                            }
                            else
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


                        }
                        else
                        {
                            _dataTable.Rows.Add(record.Split('|'));
                        }


                }
                if (ErrorRecordcount > 0)
                {
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "LESS/EXTRA FIELD(|) PRESENT IN RECORDS,File move to reject folder Start.", ObjData.IssuerNo.ToString(), 1);
                    UploadRejectedRecord(ObjData, ErrorRecords.Where(x => !string.IsNullOrEmpty(x)).ToArray());
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "LESS/EXTRA FIELD(|) PRESENT IN RECORDS,File move to reject folder End.", ObjData.IssuerNo.ToString(), 1);
                }


            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.ErrorDesc = "FILE READ FAILED WITH REASON | " + ex.ToString();
                ObjData.StepStatus = true;
            }
            ObjData.Filestatus = "FILE READ COMPELETED";
            new CardAutomation().FunInsertTextLog("FILE READER  ENDED FOR !" + ObjData.FileName, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, string.IsNullOrEmpty(ObjData.FileID) ? "0" : ObjData.FileID.ToString(), ObjData.ProcessId.ToString(), "", "File Reader Ended for:" + (string.IsNullOrEmpty(ObjData.FileName) ? "" : ObjData.FileName), ObjData.IssuerNo.ToString(), 1);
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
                objSFTP.destinationPath = ObjData.Local_Input;
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
