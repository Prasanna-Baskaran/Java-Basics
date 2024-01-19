using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Renci.SshNet;
using System.Data.SqlClient;
using ReflectionIT.Common.Data.SqlClient;

using System.Data;
using System.Diagnostics;
using AGSPGP;
using System.Text.RegularExpressions;
using System.Reflection;

namespace AGS.PREFileGenerationRBLWithOutFileID
{





    public class PREFile
    {
        public bool IsSensitiveSave { get; set; }
        //fetch and read file from perticular locaation
        public string ErrorLogFilePath { get; set; }
        public PREFile()
        {
            ErrorLogFilePath = string.Empty;
        }
        void ChangePathForDebug(ClsPathsBO objPath)
        {
            PropertyInfo[] properties = typeof(ClsPathsBO).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(objPath) != null && property.PropertyType == typeof(string))
                    property.SetValue(objPath, property.GetValue(objPath).ToString().Replace("F:", "E:"));
            }
        }
        int GlobalIssuerNo = 0;
        public void Process(int IssurNo)
        {
            FunInsertTextLog("*********************PRE DLL Started************************", IssurNo, false);

            GlobalIssuerNo = IssurNo;
            string CurrentFileID = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            string FileName = string.Empty;
            string ProcessID = string.Empty;
            //string CurrentFileID = FunGetFileId(IssurNo);
            if (string.IsNullOrEmpty(CurrentFileID))
            {
                FunInsertTextLog("*********************NO FileID Recevied Retriving the fileId from DB************************", IssurNo, false);
                ObjDTOutPut = FunGetFileIdAndFileName(IssurNo);//change
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    CurrentFileID = ObjDTOutPut.Rows[0]["id"].ToString();
                    FileName = ObjDTOutPut.Rows[0]["FileName"].ToString();
                    ProcessID = ObjDTOutPut.Rows[0]["ProcessID"].ToString();
                    FunInsertTextLog("PRE CREATION STARTED FOR FILEID: " + CurrentFileID + " , ProcessID:" + ProcessID + " and FileName: " + FileName, IssurNo, false);
                }
            }
            Int32 CardProgramCount = 0;

            DataTable FailedPRE = new DataTable();
            FailedPRE.Columns.Add("Records", typeof(string));
            FailedPRE.Columns.Add("CIF ID", typeof(string));
            FailedPRE.Columns.Add("Reasone", typeof(string));
            FailedPRE.Columns.Add("CardNo", typeof(string));
            //Get paths for PRE gen 
            ClsPathsBO objPath = FunGetPaths(IssurNo, 1, ProcessID);
            objPath.FailedPRERecords = FailedPRE.Copy();


            IsSensitiveSave = FunGetAppSettingKeysValue(IssurNo);//if true
            //ChangePathForDebug(objPath);
            //Get output files from SFTP

            FunInsertTextLog("*********************PREProcess Started************************", IssurNo, false);
            try
            {
                if (!string.IsNullOrEmpty(objPath.ID))
                {
                    ErrorLogFilePath = objPath.ErrorLogPath;
                    FunInsertTextLog("FunGetSFTP_Output_Files_ToGeneratePRE Start", IssurNo, false);
                    FunGetSFTP_Output_Files_ToGeneratePRE(objPath, IssurNo);
                    FunInsertTextLog("FunGetSFTP_Output_Files_ToGeneratePRE End", IssurNo, false);

                    string[] OutfilesArr = null;
                    //************** Process Output Files *************
                    try
                    {
                        //objPath.Local_Output_Input = @"D:\CardAutomation\";
                        //get card output file from server
                        OutfilesArr = Directory.GetFiles(objPath.Local_Output_Input, "*.txt");
                    }
                    catch (Exception ex)
                    {
                        FunInsertTextLog("Error: Process|GetOutputFiles" + ex.ToString(), IssurNo, true);
                    }
                    if (OutfilesArr.Count() > 0)
                    {
                        foreach (string filepath in OutfilesArr)
                        {
                            //create PRE
                            if (!objPath.EncryptOutputFileToPGP)
                            {
                                //ObjDTOutPut = FunGetFileIdAndFileName(IssurNo);//change
                                //if (ObjDTOutPut.Rows.Count > 0)
                                //{
                                //    CurrentFileID = ObjDTOutPut.Rows[0]["id"].ToString();
                                //    FileName = ObjDTOutPut.Rows[0]["FileName"].ToString();
                                //}
                                FunInsertTextLog("FunCreatePRE Start", IssurNo, false);
                                FunCreatePRE(filepath, objPath, CurrentFileID);
                                FunInsertTextLog("FunCreatePRE End", IssurNo, false);
                            }
                            else
                            {
                                FunInsertTextLog("Output PGP Start", IssurNo, false);
                                FunCreateOutputPGP(filepath, objPath);
                                FunInsertTextLog("Output PGP End", IssurNo, false);
                            }

                        }
                        FunInsertTextLog("FunSFTP_UploadFailedOutFiles Start", IssurNo, false);
                        //move failed Output files from server to SFTP                  
                        FunSFTP_UploadFailedOutFiles(objPath);
                        FunInsertTextLog("FunSFTP_UploadFailedOutFiles End", IssurNo, false);

                        FunInsertTextLog("FunSFTP_UploadPRE End", IssurNo, false);
                        //move PRE files to Card vendor SFTP
                        //FunSFTP_UploadPRE(objPath);
                        FunInsertTextLog("FunSFTP_UploadFailedOutFiles End", IssurNo, false);

                        //move Output backup to Back up on sftp
                        FunSFTP_UploadOutputBackUp(objPath, IssurNo);
                    }
                    else
                    {
                        FunInsertTextLog("No output file found", IssurNo, false);
                    }

                    /*UPDATE Failed PRE Record*/
                    FunInsertTextLog("UploadStatusFileOnSFTP Method Called", IssurNo, false);
                    FunUpdateFaildPRERecords(objPath.IssuerNo, objPath.FailedPRERecords, CurrentFileID);

                    new PREStatusUpdate().UploadStatusFileOnSFTP(CurrentFileID, FileName, objPath, objPath.IssuerNo);

                    if (((ProcessID.Equals("3", StringComparison.OrdinalIgnoreCase)) || (ProcessID.Equals("8", StringComparison.OrdinalIgnoreCase))) && (IssurNo == 1))
                    {
                        FunInsertTextLog("Check Any rejected records available for Issuance file, FileId:" + CurrentFileID, IssurNo, false);
                        //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID, objData.ProcessId.ToString(), "", "GETRejectRecords to create CIF file", objData.IssuerNo.ToString(), 1);
                        DataTable dtRejectRecords = GETRejectRecords(IssurNo, CurrentFileID, ProcessID);
                        if (dtRejectRecords.Rows.Count > 0)
                        {
                            FunInsertTextLog("Rejected records found.", IssurNo, false);

                            //string _rejectfilenamewithpath = System.Configuration.ConfigurationManager.AppSettings["CIFRejectFilePath"].ToString() + FileName.Split('.')[0] + "_Reject_" + DateTime.Now.ToString("ddMMyyyyss") + ".txt";

                            string _rejectfilenamewithpath = objPath.Local_Input + "Reject\\" + FileName.Split('.')[0] + "_Reject_" + DateTime.Now.ToString("ddMMyyyyss") + ".txt";

                            FunInsertTextLog("Rejected records found CREATING TXT FILE.", IssurNo, false);
                            DATATableToTextFile(dtRejectRecords, _rejectfilenamewithpath, IssurNo);

                            if (objPath.isstatusFileINPGP)
                            {
                                FunInsertTextLog("Rejected records TXT FILE. CONVERTING INTO PGP.", IssurNo, false);

                                bool bResult = new PREFile().PGP_Encrypt(objPath.PGP_KeyName, _rejectfilenamewithpath, _rejectfilenamewithpath.Replace(".txt", ".pgp"), objPath.StatusFilePGPPublicKeyPath, IssurNo.ToString());
                                if (!bResult)
                                {
                                    FunInsertTextLog("REJECT RECORDS FILE ENCRYPTION FAILED", Convert.ToInt32(IssurNo), false);
                                    return;
                                }
                                else
                                {
                                    FunInsertTextLog("REJECT RECORDS PGP FILE CREATED AND DELETING TEXT FILE.", Convert.ToInt32(IssurNo), false);
                                    File.Delete(_rejectfilenamewithpath);
                                }
                            }
                        }
                    }

                }
                else
                {
                    FunInsertTextLog("PREProcess failed|Paths not found", IssurNo, false);
                }
            }
            catch (Exception ex)
            {
                new PREStatusUpdate().FunPREFailedStatusRecords(objPath.IssuerNo, CurrentFileID);
                FunInsertTextLog("Error: PREProcess failed" + ex.ToString(), IssurNo, true);
            }
            FunInsertTextLog("*********************PRE PROCESS END*********************", IssurNo, false);
        }

        private bool FunGetAppSettingKeysValue(int IssurNo)
        {
            string ObjReturnStatus = string.Empty;
            ClsPathsBO objPath = new ClsPathsBO();
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Usp_GetLoadAssemblyAppsettingKeys", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Key", SqlDbType.VarChar, 0, ParameterDirection.Input, "AdditionalDebug");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus = Convert.ToString(ObjDTOutPut.Rows[0]["value"]);
                    }
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                objPath.ID = string.Empty;
                FunInsertTextLog("Error: FunGetAppSettingKeysValue" + ex.ToString(), IssurNo, true);
            }

            if (ObjReturnStatus.Equals("True", StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
            //throw new NotImplementedException();
        }

        public void FunCreateOutputPGP(string OutputFile, ClsPathsBO objPath)
        {
            string FileName = (OutputFile.Split('\\')[OutputFile.Split('\\').Length - 1]);
            FunMoveEncryptFile(OutputFile, objPath.Local_PRE + FileName, objPath);
        }
        public void FunCreatePRE(string OutputFile, ClsPathsBO objPath, string CurrentFileID)
        {
            PREModel ObjPreModel = new PREModel();
            ObjPreModel.IssuerNo = Convert.ToInt32(objPath.IssuerNo);
            //get files from sftp
            ObjPreModel.FilePath = OutputFile;

            ObjPreModel.PREFilePath = objPath.Local_PRE;
            ObjPreModel.InvalidOutputPath = objPath.Local_Output_Failed;
            ObjPreModel.FileName = (ObjPreModel.FilePath.Split('\\')[ObjPreModel.FilePath.Split('\\').Length - 1]);
            ObjPreModel.AllLines = File.ReadAllLines(ObjPreModel.FilePath);
            FunInsertTextLog("FunCreatePRE fileLines Found", ObjPreModel.IssuerNo, false);
            //check file is not empty 
            if (ObjPreModel.AllLines.Count() > 0)
            {

                try
                {
                    FunInsertTextLog("FunCreatePRE fileLines.Count > 0", ObjPreModel.IssuerNo, false);
                    int InvalidLineCount = 1;

                    //check file contains header and trailer
                    var checkheadTraillst = from csvline in ObjPreModel.AllLines
                                            let data = csvline.Split(',')
                                            where data.Contains("<Header>") || data.Contains("<Trailer>")
                                            select csvline;

                    IEnumerable<string> query = null; int isMagstrip = 0;
                    //file contains one card program by checking 1 header and 1 trailer
                    if (checkheadTraillst.ToList().Count() == 2)
                    {
                        FunInsertTextLog("FunCreatePRE checkheadTraillst.ToList().Count() == 2", ObjPreModel.IssuerNo, false);
                        //check  cardprogram is  of magstrip 
                        ObjPreModel.CardProgram = checkheadTraillst.ToList()[0].Split(',')[5];
                        isMagstrip = FunCheckMagstrip(ObjPreModel.CardProgram, objPath.IssuerNo);

                        query = from csvline in ObjPreModel.AllLines.Where(x => (!x.Split(',').Contains("<Header>"))
                                && (!x.Split(',').Contains("<Trailer>")))
                                let data = csvline.Split(',')
                                where ((string.IsNullOrEmpty(data[0]))   //CardNum

                                        || (string.IsNullOrEmpty(data[4])) //Expiry
                                        || (string.IsNullOrEmpty(data[11])) //CustId
                                        || (string.IsNullOrEmpty(data[15])) //cvv1
                                        || (string.IsNullOrEmpty(data[16])) //cvv2
                                    // || (string.IsNullOrEmpty(data[19])) //Pin block
                                        || (string.IsNullOrEmpty(data[26])) //pvv/pinoffset
                                        || (string.IsNullOrEmpty(data[60]))//accnt Id
                                    //start Diksha 19/07
                                        || ((isMagstrip == 0)  //for emv cards
                                             &&
                                            ((string.IsNullOrEmpty(data[64])) //icvd/iccv

                                             || (string.IsNullOrEmpty(data[1])) //Seq no
                                            ))
                                        )
                                select csvline;
                        InvalidLineCount = query.Count();
                        objPath.OutputErrorRecords = null;
                        if (InvalidLineCount > 0)
                        {
                            FunInsertTextLog("Error found" + query.FirstOrDefault(), Convert.ToInt32(objPath.IssuerNo), false);
                            /*ATPBF-824*/
                            /*Remove The Error Records*/

                            objPath.OutputErrorRecords = new string[ObjPreModel.AllLines.Count()];
                            var validationmsg = GetInvalidOuptutFileValidationMessage(query, isMagstrip, objPath, ObjPreModel);
                            FunInsertTextLog("Error found in record: " + validationmsg, Convert.ToInt32(objPath.IssuerNo), false);
                            FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);

                        }
                    }
                    /*
                    if (InvalidLineCount == 0)
                    {*/
                    HeaderProcess(ObjPreModel, ObjPreModel.AllLines);
                    if (ObjPreModel.splitfilecolumnindex == 0)
                    {
                        IEnumerable<string> NewFileLines = from csvline in ObjPreModel.AllLines.Where(x => (!x.Split(',').Contains("<Header>"))
                                && (!x.Split(',').Contains("<Trailer>")) && x.Split(',')[ObjPreModel.splitfilecolumnindex] == "1")
                                                           select csvline;
                        FunInsertTextLog("No split file for this" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                        ObjPreModel.IsFileMove = true;
                        //HeaderProcess(ObjPreModel, ObjPreModel.AllLines);
                        ExecuteSingleFileRecord(NewFileLines, objPath, ObjPreModel, CurrentFileID, false);//cardprogram passes for use to check bin
                    }
                    else
                    {
                        FunInsertTextLog("No split file for this, split file index is " + ObjPreModel.splitfilecolumnindex.ToString() + " card program:" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                        IEnumerable<string> NewFileLines = from csvline in ObjPreModel.AllLines.Where(x => (!x.Split(',').Contains("<Header>"))
                                && (!x.Split(',').Contains("<Trailer>")) && x.Split(',')[ObjPreModel.splitfilecolumnindex] == "1")
                                                           select csvline;

                        IEnumerable<string> NewFileLinesInsta = from csvline in ObjPreModel.AllLines.Where(x => (!x.Split(',').Contains("<Header>"))
                                && (!x.Split(',').Contains("<Trailer>")) && x.Split(',')[ObjPreModel.splitfilecolumnindex] != "1")
                                                                select csvline;
                        if (NewFileLinesInsta.Count() > 0)
                        {
                            FunInsertTextLog("issuance and other records found in output file" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                            ObjPreModel.IsFileMove = false;
                            ExecuteSingleFileRecord(NewFileLines, objPath, ObjPreModel, CurrentFileID, false);
                            ExecuteSingleFileRecord(NewFileLinesInsta, objPath, ObjPreModel, CurrentFileID, true);
                            FunInsertTextLog("Save only insta output file" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                            SaveFiles(NewFileLinesInsta, objPath, ObjPreModel, true);//This Savefile is to save output file in backup folder for future use
                            if (NewFileLines.Count() > 0)
                            {
                                FunInsertTextLog("Save without insta output file" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                                SaveFiles(NewFileLines, objPath, ObjPreModel, false);//sheetal change if file contains record
                            }

                            FunMoveEncryptFile(ObjPreModel.FilePath, objPath.Local_Output_Backup + ObjPreModel.FileName, objPath);//sheetal change for getting orignal output file
                            DeleteOutputFile(ObjPreModel.FilePath, Convert.ToInt32(objPath.IssuerNo));
                        }
                        else
                        {
                            FunInsertTextLog("Only issuance records found in output file" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                            ObjPreModel.IsFileMove = true;
                            //HeaderProcess(ObjPreModel, ObjPreModel.AllLines);
                            ExecuteSingleFileRecord(ObjPreModel.AllLines, objPath, ObjPreModel, CurrentFileID, false);

                        }
                    }

                    /*}
                    else
                    {
                        if (query != null)
                        {
                            // InvalidLineCount = query.Count();
                            var validationmsg = GetInvalidOuptutFileValidationMessage(query, isMagstrip);
                            FunInsertTextLog("Error found in record: " + validationmsg, Convert.ToInt32(objPath.IssuerNo), false);
                        }
                        //move invalid output file to failed for PRE folder
                        FunInsertTextLog("FunCreatePRE checkheadTraillst.ToList().Count() == 2", Convert.ToInt32(objPath.IssuerNo), false);
                        //  File.Move(FilePath, InvalidOutputPath + FileName);
                        FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
                        //add 06-09-18
                        new PREStatusUpdate().FunPREFailedStatusRecords(objPath.IssuerNo, CurrentFileID);

                    }*/
                }
                catch (Exception ex)
                {
                    FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
                    FunInsertTextLog("Error: PRE failed, Main Catch Error " + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
                    // add 06-09-18
                    new PREStatusUpdate().FunPREFailedStatusRecords(objPath.IssuerNo, CurrentFileID);
                }
            }
            else
            {
                //File.Move(FilePath, InvalidOutputPath + FileName);
                // add 06-09-18
                new PREStatusUpdate().FunPREFailedStatusRecords(objPath.IssuerNo, CurrentFileID);
                FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
            }
        }
        private void DeleteOutputFile(string FilePath, int issuerNo)
        {
            try
            {
                File.Delete(FilePath);
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in DeleteOutputFile:" + ex.ToString(), issuerNo, true);
            }
        }
        private void SaveFiles(IEnumerable<string> NewFileLines, ClsPathsBO objPath, PREModel ObjPreModel, bool IsRepin)
        {
            System.Threading.Thread.Sleep(1000); //wait for 1 sec so that file name get change
            string filepath = string.Empty;
            if (IsRepin)
            {
                filepath = objPath.Local_Output_Backup + ObjPreModel.FileName.Substring(0, ObjPreModel.FileName.Length - 4) + "_Repin_" + DateTime.Now.ToString("ddMMyyyHHss") + ".txt";
            }
            else
            {
                filepath = objPath.Local_Output_Backup + ObjPreModel.FileName.Substring(0, ObjPreModel.FileName.Length - 4) + "_" + DateTime.Now.ToString("ddMMyyyHHss") + ".txt";
            }
            try
            {
                //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
                if (File.Exists(filepath))
                    File.Delete(filepath);
                File.AppendAllText(filepath, ObjPreModel.AllLines.ElementAt(0), Encoding.UTF8);
                File.AppendAllText(filepath, Environment.NewLine, Encoding.UTF8);
                File.AppendAllLines(filepath, NewFileLines, Encoding.UTF8);
                File.AppendAllText(filepath, ObjPreModel.AllLines.ElementAt(ObjPreModel.TotalCount - 1), Encoding.UTF8);
                FunMoveEncryptFile(filepath, filepath, objPath);
            }
            catch (Exception Ex)
            {
                FunInsertTextLog("Error in FunInsertIntoLogFile" + Ex.Message, ObjPreModel.IssuerNo, true);
            }
        }
        private void ExecuteSingleFileRecord(IEnumerable<string> fileLines, ClsPathsBO objPath, PREModel ObjPreModel, string CurrentFileID, bool IsRepin)
        {
            try
            {
                FunInsertTextLog("Execute Single FileRecord If InvalidLineCount == 0", ObjPreModel.IssuerNo, false);
                ObjPreModel.TotalCount = ObjPreModel.AllLines.Count();
                //start Diksha 19/07
                ObjPreModel.DtPREStatus = new DataTable();
                ObjPreModel.DtPREStatus.Columns.Add("CustID");
                ObjPreModel.DtPREStatus.Columns.Add("AccountNo");
                ObjPreModel.DtPREStatus.Columns.Add("MaskCardNo");
                ObjPreModel.DtPREStatus.Columns.Add("CardProgram");
                //end Diksha 19/07
                //HeaderProcess(ObjPreModel, ObjPreModel.AllLines);
                /*PRABHU BANK PRE ORDER BY BRANCH CODE*/
                /*START ATPBA-1133 */

                /*Prabhu pre repin cahnges the condition if repin flag is false then go inside if condition ATPCM-681*/
                //if (ObjPreModel.OrderByField != 0 && (ObjPreModel.IsRepin))
                //if (ObjPreModel.OrderByField != 0)
                //{
                //string header = ((string[])fileLines)[0];
                //string Trailer = ((string[])fileLines)[fileLines.Count() - 1];
                //string[] array = fileLines.Where(s => s != Convert.ToString(header) && s != Trailer).ToArray();
                //fileLines = OrderbyStringarray(array, ObjPreModel.OrderByField);

                //string header = ((string[])ObjPreModel.AllLines)[0];
                //string Trailer = ((string[])ObjPreModel.AllLines)[ObjPreModel.AllLines.Count() - 1];
                //string[] array = ObjPreModel.AllLines.Where(s => s != Convert.ToString(header) && s != Trailer).ToArray();
                //fileLines = OrderbyStringarray(array, ObjPreModel.OrderByField);

                var header = ((string[])ObjPreModel.AllLines)[0];
                var Trailer = ((string[])ObjPreModel.AllLines)[ObjPreModel.AllLines.Count() - 1];
                string[] array = fileLines.Where(s => s != Convert.ToString(header) && s != Trailer).ToArray();
                fileLines = OrderbyStringarray(array, ObjPreModel.OrderByField);
                //string[] array = fileLines.ToArray();
                //fileLines = OrderbyStringarray(array, ObjPreModel.OrderByField);
                //}
                /*END*/
                ObjPreModel.IsOnlyFileIdRecords = Convert.ToInt32(ConfigurationManager.AppSettings["IsOnlyFileIdRecords"]);
                ObjPreModel.DTFINALPRERECORDS = FunGetFINALPRERECORDS(CurrentFileID, Convert.ToString(ObjPreModel.IssuerNo));
                FunInsertTextLog("IsOnlyFileIdRecords Flag:" + Convert.ToString(ObjPreModel.IsOnlyFileIdRecords) + " & FINAL PRE RECORDS COUNT:" + ObjPreModel.DTFINALPRERECORDS.Rows.Count, ObjPreModel.IssuerNo, false);

                PRESetValuesProcess(fileLines, objPath, ObjPreModel, IsRepin);
                TrailerProcess(ObjPreModel, objPath, CurrentFileID, IsRepin);
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in ExecuteSingleFileRecord:" + ex.ToString(), ObjPreModel.IssuerNo, true);
            }

        }
        /*ATPBA-1133 start*/
        private string[] OrderbyStringarray(string[] ArrayToBeOrder, int OrderBy)
        {
            try
            {
                DataTable dtRecords = Todatatable(ArrayToBeOrder);
                dtRecords.Columns.Add("OrderBY");

                for (int i = 0; i < dtRecords.Rows.Count; i++)
                {
                    dtRecords.Rows[i]["OrderBY"] = Convert.ToString(dtRecords.Rows[i][0]).Split(',')[OrderBy];
                }



                DataRow[] dataRows = dtRecords.Select().OrderBy(u => u["OrderBY"]).ToArray();
                string[] OrderArray = new string[dataRows.Length];
                int n = 0;
                foreach (var item in dataRows)
                {
                    OrderArray[n] = item.ItemArray[0].ToString();
                    n++;
                }
                return OrderArray;
            }
            catch (Exception ex)
            {
                FunInsertTextLog("OrderbyStringarray:, Main Catch Error in function OrderbyStringarray " + ex.ToString(), 0, true);
                return null;
            }
        }
        private DataTable Todatatable(string[] input)
        {
            try
            {
                DataTable Datatable = new DataTable();
                Datatable.Columns.Add();
                Array.ForEach(input, c => Datatable.Rows.Add()[0] = c);
                return Datatable;
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Todatatable:, Main Catch Error " + ex.ToString(), 0, true);
                return new DataTable();

            }


        }
        private string GetPREName(PREModel ObjPreModel, ClsPathsBO objPath, string CurrentFileID, bool IsRepin, string FileNameFormat)
        {
            string PREFileName = "";
            if (IsRepin)
            {
                if (ObjPreModel.IsRepin)
                    PREFileName = ObjPreModel.PREFilePath + FileNameFormat + "_Repin.PRE";
                //PREFileName = ObjPreModel.PREFilePath + ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_Repin" + DateTime.Now.ToString("hhmmss") + ".PRE";
                else
                    PREFileName = "";
            }
            else
            {
                PREFileName = ObjPreModel.PREFilePath + FileNameFormat + ".PRE";
            }
            return PREFileName;
        }
        /*ATPBA-1133 end*/
        private void TrailerProcess(PREModel ObjPreModel, ClsPathsBO objPath, string CurrentFileID, bool IsRepin)
        {
            if (ObjPreModel.PREGenFlag == 1)//PREGenFlag = 1 means no error while processing preformat
            {
                string PREFileName;
                string PREFileName_DIFFERRecords = string.Empty;
                if (string.IsNullOrEmpty(ObjPreModel.FileNameFormat))
                {
                    string PREDefault = ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_" + DateTime.Now.ToString("hhmmss");
                    PREFileName = GetPREName(ObjPreModel, objPath, CurrentFileID, IsRepin, PREDefault);

                    PREFileName_DIFFERRecords = ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_" + DateTime.Now.ToString("hhmmss");

                }
                else
                {
                    string FileToPathAsPerNameFormat = GetFileNameasPerFormat(ObjPreModel.FileNameFormat, ObjPreModel.BinPrefix, ObjPreModel.IsOnlyFileIdRecords == 1 ? ObjPreModel.FinalCount : ObjPreModel.count, ObjPreModel.IssuerNo, ObjPreModel.FileName);
                    //string FileToPathAsPerNameFormat = GetFileNameasPerFormat(ObjPreModel.FileNameFormat, ObjPreModel.BinPrefix, ObjPreModel.count , ObjPreModel.IssuerNo, ObjPreModel.FileName);

                    PREFileName = GetPREName(ObjPreModel, objPath, CurrentFileID, IsRepin, FileToPathAsPerNameFormat);

                    if (ObjPreModel.DifferCount > 0)
                        PREFileName_DIFFERRecords = GetFileNameasPerFormat(ObjPreModel.FileNameFormat, ObjPreModel.BinPrefix, ObjPreModel.DifferCount, ObjPreModel.IssuerNo, ObjPreModel.FileName);
                }

                if (ObjPreModel.IsRepin && IsRepin)
                {
                    PREFileName = ObjPreModel.PREFilePath + ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_Repin" + DateTime.Now.ToString("hhmmss") + ".PRE";

                    PREFileName_DIFFERRecords = ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_Repin" + DateTime.Now.ToString("hhmmss");
                }
                /*prabhu Pre repin sheetal end name change for PRE */
                if (!string.IsNullOrEmpty(PREFileName))
                {
                    StringBuilder sbFile = new StringBuilder();
                    StringBuilder sbDifferRecFile = new StringBuilder();
                    string sData = string.Empty;

                    if (ObjPreModel.IsOnlyFileIdRecords == 1)
                    {
                        FunInsertTextLog("CREATE PRE ON FILE ID BASIS.", ObjPreModel.IssuerNo, true);
                        foreach (string Result in ObjPreModel.FINALArrRESULT.Where(x => !string.IsNullOrEmpty(x)).ToArray())
                        {
                            sbFile.AppendLine(Result);
                        }

                        //ADDED TO CREATE PRE FOR EXCLUDED RECORDS
                        if (ObjPreModel.DifferCount > 0)
                        {
                            FunInsertTextLog("Excluded record count:" + Convert.ToString(ObjPreModel.DifferCount), ObjPreModel.IssuerNo, true);

                            if (string.IsNullOrEmpty(PREFileName_DIFFERRecords))
                            {
                                PREFileName_DIFFERRecords = ObjPreModel.PREFilePath + "ExcludedRecordPRE\\" + ObjPreModel.Cardtype + "_" + ObjPreModel.FileName.Split('.')[0] + "_" + DateTime.Now.ToString("hhmmss") + "_" + Convert.ToString(ObjPreModel.DifferCount) + ".PRE";
                            }
                            else
                            {
                                PREFileName_DIFFERRecords = ObjPreModel.PREFilePath + "ExcludedRecordPRE\\" + PREFileName_DIFFERRecords + ".PRE";
                            }

                            foreach (string Result in ObjPreModel.DifferArrRESULT.Where(x => !string.IsNullOrEmpty(x)).ToArray())
                            {
                                sbDifferRecFile.AppendLine(Result);
                            }
                            FunInsertTextLog("SaveExcludedRecordPREFile called:" + Convert.ToString(ObjPreModel.DifferCount), ObjPreModel.IssuerNo, true);
                            SaveExcludedRecordPREFile(sbDifferRecFile, ObjPreModel, PREFileName_DIFFERRecords, objPath, CurrentFileID);

                            FunInsertTextLog("Uploading excluded records on SFTP", ObjPreModel.IssuerNo, true);
                            FunSFTP_UploadExcludedRecordPERFile(objPath);
                        }
                    }
                    else
                    {
                        FunInsertTextLog("CREATE PRE ON OUTPUT RECORD BASIS.", ObjPreModel.IssuerNo, true);

                        foreach (string Result in ObjPreModel.ArrResult.Where(x => !string.IsNullOrEmpty(x)).ToArray())
                        {
                            sbFile.AppendLine(Result);
                        }
                    }


                    if ((ObjPreModel.IsOnlyFileIdRecords == 1 ? ObjPreModel.FinalCount : ObjPreModel.count) > 0)
                    {
                        SavePREFile(sbFile, ObjPreModel, PREFileName, objPath, CurrentFileID);
                    }
                    else
                    {
                        FunMoveEncryptFile(objPath.Local_Output_Input + ObjPreModel.FileName, objPath.Local_Output_Backup + ObjPreModel.FileName, objPath);
                    }




                    ObjPreModel.count = 0;
                    ObjPreModel.ArrResult = new string[ObjPreModel.InputLine];

                    ObjPreModel.FinalCount = 0;
                    ObjPreModel.FINALArrRESULT = new string[0];
                }
                else
                {
                    FunInsertTextLog("Skip repin output file to process" + ObjPreModel.PREGenFlag, ObjPreModel.IssuerNo, true);
                }
            }
            else
            {
                new PREStatusUpdate().FunPREFailedStatusRecords(objPath.IssuerNo, CurrentFileID);
                FunInsertTextLog("Error in TrailerProcess:PREGenFlag" + ObjPreModel.PREGenFlag, ObjPreModel.IssuerNo, true);
            }

        }
        private void SavePREFile(StringBuilder sbFile, PREModel ObjPreModel, string PREFileName, ClsPathsBO objPath, string CurrentFileID)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(PREFileName))
                {
                    file.WriteLine(sbFile.ToString());
                    file.Close();
                    //string FileToPathAsPerNameFormat = GetFileNameasPerFormat(FileNameFormat,BinPrefix, count);
                    // PGP encryption
                    FunInsertTextLog("FunCreatePRE PGP_Encrypt Start", ObjPreModel.IssuerNo, false);
                    //bool bResult = PGP_Encrypt(objPath.PGP_KeyName, PREFileName, FileToPathAsPerNameFormat + ".pgp", objPath.PGPPublicKeyPath, IssuerNo.ToString());
                    bool bResult = PGP_Encrypt(objPath.PGP_KeyName, PREFileName, PREFileName.Split('.')[0] + ".pgp", objPath.PREPGPPublicKeyPath, ObjPreModel.IssuerNo.ToString());
                    FunInsertTextLog("FunCreatePRE PGP_Encrypt End", ObjPreModel.IssuerNo, false);
                    if (bResult)
                    {
                        File.Delete(PREFileName);
                        FunInsertTextLog("File deleted:" + PREFileName, ObjPreModel.IssuerNo, false);
                        //move success files to back up                                            
                        if (ObjPreModel.IsFileMove)
                        {
                            //FunMoveEncryptFile(ObjPreModel.OutputFile, objPath.Local_Output_Backup + ObjPreModel.FileName, objPath);
                            FunMoveEncryptFile(ObjPreModel.FilePath, objPath.Local_Output_Backup + ObjPreModel.FileName, objPath);//sheetal change for getting orignal output file
                            DeleteOutputFile(ObjPreModel.FilePath, Convert.ToInt32(objPath.IssuerNo));//delete file from input location preinput
                            //FunMoveEncryptFile(objPath.Local_Output_Input + ObjPreModel.FileName, objPath.Local_Output_Backup + ObjPreModel.FileName, objPath);//change path
                        }
                        var name = Path.GetFileName(PREFileName.Split('.')[0] + ".pgp");
                        //update PRE status in database
                        FunUpdatePREStatus(ObjPreModel.IssuerNo.ToString(), ObjPreModel.DtPREStatus, CurrentFileID, name);
                        FunInsertTextLog("PRE status updated:" + PREFileName, ObjPreModel.IssuerNo, false);
                    }
                    else
                    {
                        FunInsertTextLog("PGP encryption failed", Convert.ToInt32(objPath.IssuerNo), false);
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in Save PRE File:" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
            }

        }

        private void SaveExcludedRecordPREFile(StringBuilder sbFile, PREModel ObjPreModel, string PREFileName, ClsPathsBO objPath, string CurrentFileID)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(PREFileName))
                {
                    file.WriteLine(sbFile.ToString());
                    file.Close();
                    // PGP encryption
                    FunInsertTextLog("SaveExcludedRecordPREFile PGP_Encrypt Start", ObjPreModel.IssuerNo, false);
                    bool bResult = PGP_Encrypt(objPath.PGP_KeyName, PREFileName, PREFileName.Split('.')[0] + ".pgp", objPath.PREPGPPublicKeyPath, ObjPreModel.IssuerNo.ToString());
                    FunInsertTextLog("SaveExcludedRecordPREFile PGP_Encrypt End", ObjPreModel.IssuerNo, false);
                    if (bResult)
                    {
                        File.Delete(PREFileName);
                        FunInsertTextLog("SaveExcludedRecordPREFile File deleted:" + PREFileName, ObjPreModel.IssuerNo, false);
                    }
                    else
                    {
                        FunInsertTextLog("SaveExcludedRecordPREFile PGP encryption failed", Convert.ToInt32(objPath.IssuerNo), false);
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("SaveExcludedRecordPREFile Error in Save PRE File:" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
            }

        }
        private void PRESetValuesProcess(IEnumerable<string> fileLines, ClsPathsBO objPath, PREModel ObjPreModel, bool IsRepin)
        {
            try
            {
                ObjPreModel.count = 0;
                ObjPreModel.FinalCount = 0;
                ObjPreModel.DifferCount = 0;

                if (fileLines.Count() > 0)
                {
                    int ErrorRecordcount = 0;
                    if (objPath.OutputErrorRecords == null || objPath.OutputErrorRecords.Length == 0)
                    {
                        objPath.OutputErrorRecords = new string[fileLines.Count()];
                    }
                    else
                    {
                        ErrorRecordcount = objPath.OutputErrorRecords.Where(x => !string.IsNullOrEmpty(x)).Count();
                    }

                    ObjPreModel.ArrResult = new string[fileLines.Count()];
                    ObjPreModel.FINALArrRESULT = new string[fileLines.Count()];
                    ObjPreModel.DifferArrRESULT = new string[fileLines.Count()];


                    foreach (string singleline in fileLines)
                    {
                        DataRow toInsert = objPath.FailedPRERecords.NewRow();
                        Console.WriteLine(singleline);
                        ObjPreModel.PREGenFlag = 1;
                        // int PREGenFlag = 0;
                        //trailer
                        // iterate in lines other than header and trailer and  exclude repin lines
                        //start diksha 19/07                            
                        //if ((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ((singleline.Split(','))[25].ToString() == "1"))//changed 23 to 25 as per Rehan and Mani switch team

                        /*Prabhu pre repin sheetal start condition modified below singleline ATPCM-681*/
                        //if (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ((singleline.Split(','))[25].ToString() == "1")) || ObjPreModel.IsRepin && (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ((singleline.Split(','))[25].ToString() == "1"))) || ObjPreModel.IsRepin && (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ((singleline.Split(','))[25].ToString() == "0"))))
                        //if (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ((singleline.Split(','))[25].ToString() == "1")) || ObjPreModel.IsRepin && (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ObjPreModel.splitfilecolumnindex==1)) || ObjPreModel.IsRepin && (((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")) && ObjPreModel.splitfilecolumnindex==0)))
                        if ((singleline.Split(',').All(s => s != "<Header>")) && (singleline.Split(',').All(s => s != "<Trailer>")))//changed 23 to 25 as per Rehan and Mani switch team
                        {
                            try
                            {
                                FunInsertTextLog("iterate in lines other than header and trailer and  exclude repin lines Start", ObjPreModel.IssuerNo, false);
                                if (!string.IsNullOrEmpty(ObjPreModel.PREFormat))
                                {
                                    string CARDNO = string.Empty;

                                    StringBuilder sb = new StringBuilder();
                                    sb = sb.Append(ObjPreModel.PREFormat);

                                    string[] aData = singleline.Split(',');
                                    foreach (DataRow dr in ObjPreModel.DtPRERecord.Rows)
                                    {
                                        CARDNO = aData[0]; //GETTING CARD NUMBER FROM OUTPUT FILE.

                                        //for expiry in mm/yy format
                                        if (dr["Token"].ToString() == "{ExpiryMMYY}")
                                        {
                                            string ExpiryMMYY = aData[Convert.ToInt16(dr["OutputPosition"])].Substring(2, 2) + "/" + aData[Convert.ToInt16(dr["OutputPosition"])].Substring(0, 2);
                                            sb.Replace(dr["Token"].ToString(), ExpiryMMYY);
                                        }
                                        //for expiry MM/YYYY format
                                        if (dr["Token"].ToString() == "{ExpiryMMYYYY}")
                                        {
                                            string ExpiryMMYY = aData[Convert.ToInt16(dr["OutputPosition"])].Substring(2, 2) + "/" + ConfigurationManager.AppSettings["Maxyear"].ToString() + aData[Convert.ToInt16(dr["OutputPosition"])].Substring(0, 2);
                                            sb.Replace(dr["Token"].ToString(), ExpiryMMYY);
                                        }
                                        //for  service code
                                        else if (dr["Token"].ToString() == "{ServiceCode}")
                                        {
                                            ////Start for service code
                                            string[] SrvCodeArr = (aData[Convert.ToInt16(dr["OutputPosition"])]).Split('=');
                                            string StrSrvCode = SrvCodeArr[SrvCodeArr.Length - 1];
                                            string ServiceCode = StrSrvCode.Substring(4, 3); //skip expiry date then 3 digits                            
                                            sb.Replace(dr["Token"].ToString(), ServiceCode);
                                        }
                                        else
                                        {
                                            //start diksha 19/07
                                            //if (dr["Padding"].ToString() == "Left")
                                            //    sb.Replace(dr["Token"].ToString(), aData[Convert.ToInt16(dr["OutputPosition"])].PadLeft(Convert.ToInt16(dr["FixLength"]), Convert.ToChar(dr["PadChar"].ToString())));
                                            //else if (dr["Padding"].ToString() == "Right")
                                            //    sb.Replace(dr["Token"].ToString(), aData[Convert.ToInt16(dr["OutputPosition"])].PadRight(Convert.ToInt16(dr["FixLength"]), Convert.ToChar(dr["PadChar"].ToString())));
                                            try
                                            {
                                                if ((aData[Convert.ToInt16(dr["OutputPosition"])].ToString().Trim().Length > Convert.ToInt16(dr["FixLength"])) && (!string.IsNullOrEmpty(dr["Direction"].ToString())))
                                                {
                                                    int startPos = 0;
                                                    int endPos = 0;
                                                    if (dr["Direction"].ToString() == "Left")
                                                    {
                                                        startPos = Convert.ToInt16(dr["StartPos"]);
                                                        endPos = Convert.ToInt16(dr["EndPos"]);
                                                    }
                                                    else if (dr["Direction"].ToString() == "Right")
                                                    {
                                                        startPos = ((Convert.ToInt16(aData[Convert.ToInt16(dr["OutputPosition"])].Trim().Length)) - 1) - Convert.ToInt16(dr["FixLength"]);
                                                        endPos = aData[Convert.ToInt16(dr["OutputPosition"])].ToString().Trim().Length - 1;
                                                    }

                                                    string data = aData[Convert.ToInt16(dr["OutputPosition"])].Substring(startPos, endPos);

                                                    sb.Replace(dr["Token"].ToString(), data.PadLeft(Convert.ToInt16(dr["FixLength"]), Convert.ToChar(dr["PadChar"].ToString())));
                                                }
                                                //end Diksha
                                                if (dr["Padding"].ToString() == "Left")
                                                {

                                                    sb.Replace(dr["Token"].ToString(), aData[Convert.ToInt16(dr["OutputPosition"])].PadLeft(Convert.ToInt16(dr["FixLength"]), Convert.ToChar(dr["PadChar"].ToString())));
                                                }
                                                else if (dr["Padding"].ToString() == "Right")
                                                    sb.Replace(dr["Token"].ToString(), aData[Convert.ToInt16(dr["OutputPosition"])].PadRight(Convert.ToInt16(dr["FixLength"]), Convert.ToChar(dr["PadChar"].ToString())));
                                            }
                                            catch (Exception ex)
                                            {
                                                //ObjPreModel.PREGenFlag = 0;
                                                //FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
                                                //objPath.OutputErrorRecords[ErrorRecordcount] = singleline;
                                                //toInsert[0] = singleline;
                                                //toInsert[1] = Convert.ToString(aData[11]);
                                                toInsert[2] = "Error: Replace data, For" + dr["Token"].ToString() + "Exception:" + ex.ToString();
                                                //toInsert[3] = aData[0].ToString();
                                                //objPath.FailedPRERecords.Rows.InsertAt(toInsert, ErrorRecordcount);
                                                //ErrorRecordcount++;
                                                FunInsertTextLog("Error: Replace data, For" + dr["Token"].ToString() + "Exception:" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
                                                throw ex;
                                            }
                                        }
                                    }
                                    ObjPreModel.ArrResult[ObjPreModel.count] = sb.ToString();


                                    if (ObjPreModel.IsOnlyFileIdRecords == 1)
                                    {
                                        if (ObjPreModel.DTFINALPRERECORDS.AsEnumerable().Any(row => CARDNO == row.Field<String>("CARDNO")))
                                        {
                                            ObjPreModel.FINALArrRESULT[ObjPreModel.FinalCount] = sb.ToString();
                                            ObjPreModel.DtPREStatus.Rows.Add(singleline.Split(',')[11], singleline.Split(',')[60], Convert.ToString(singleline.Split(',')[0]), ObjPreModel.CardProgram);//change by uddesh for clear card no 6/5/19
                                            ObjPreModel.FinalCount++;
                                        }
                                        else
                                        {
                                            ObjPreModel.DifferArrRESULT[ObjPreModel.DifferCount] = sb.ToString();
                                            ObjPreModel.DifferCount++;
                                        }
                                    }
                                    else
                                    {
                                        ObjPreModel.DtPREStatus.Rows.Add(singleline.Split(',')[11], singleline.Split(',')[60], Convert.ToString(singleline.Split(',')[0]), ObjPreModel.CardProgram);//change by uddesh for clear card no 6/5/19
                                    }
                                    //start Diksha 19/07
                                    //ObjPreModel.DtPREStatus.Rows.Add(singleline.Split(',')[11], singleline.Split(',')[60], GetMaskedCardNo(Convert.ToString(singleline.Split(',')[0])), ObjPreModel.CardProgram);//column value added for cardprogram  
                                    ObjPreModel.count++;
                                }
                                else
                                {
                                    ObjPreModel.PREGenFlag = 0;
                                    FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
                                    break;
                                }

                            }
                            catch (Exception exs)
                            {
                                //ObjPreModel.PREGenFlag = 0;
                                objPath.OutputErrorRecords[ErrorRecordcount] = singleline;
                                string[] TEMP = singleline.Split(',');
                                toInsert[0] = singleline;
                                toInsert[1] = Convert.ToString(TEMP[11]);
                                //toInsert[2] = exs.ToString();
                                toInsert[3] = TEMP[0].ToString();
                                objPath.FailedPRERecords.Rows.InsertAt(toInsert, ErrorRecordcount);
                                ErrorRecordcount++;
                                FunInsertTextLogSensitive("Error: FunCreatePRE loop, record:" + singleline + " Error:" + exs.ToString(), Convert.ToInt32(objPath.IssuerNo), true, IsSensitiveSave);
                                FunInsertTextLog("Error: FunCreatePRE loop" + exs.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
                                //FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
                                break;
                            }
                        }
                    }

                    FunInsertTextLog("OUTPUT FILE ALL RECORD COUNT:" + Convert.ToString(ObjPreModel.count) + " , FILE ID SUCCESS RECORDS:" + Convert.ToString(ObjPreModel.FinalCount) + " & DIIFER COUNT:" + Convert.ToString(ObjPreModel.DifferCount), ObjPreModel.IssuerNo, false);

                    if (ErrorRecordcount > 0)
                    {

                        MoveErrorOutputRecord(objPath.OutputErrorRecords.Where(x => !string.IsNullOrEmpty(x)).ToArray(), objPath, ObjPreModel);
                    }
                }
                else
                {
                    ObjPreModel.PREGenFlag = 0;
                    FunInsertTextLog("PRE file - No records ", Convert.ToInt32(objPath.IssuerNo), false);
                }
            }
            catch (Exception ex)
            {
                ObjPreModel.PREGenFlag = 0;
                FunInsertTextLog("Error: Create PRE loop" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
                FunMoveEncryptFile(ObjPreModel.FilePath, ObjPreModel.InvalidOutputPath + ObjPreModel.FileName, objPath);
            }
        }
        public void MoveErrorOutputRecord(string[] ErrorData, ClsPathsBO objPath, PREModel ObjPreModel)
        {
            try
            {
                string FileName = Path.GetFileNameWithoutExtension(ObjPreModel.FileName) + "OutputError_" + ErrorData.Count().ToString() + Path.GetExtension(ObjPreModel.FileName);
                string filePath = Path.GetDirectoryName(ObjPreModel.FilePath) + "\\";
                File.WriteAllLines(filePath + FileName, ErrorData);
                FunMoveEncryptFile(filePath + FileName, ObjPreModel.InvalidOutputPath + FileName, objPath);
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: While Moving the Error Output records" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
            }

        }

        private void HeaderProcess(PREModel ObjPreModel, string[] FileLines)
        {
            try
            {
                string singleline = FileLines.ElementAt(0);
                if (singleline.Split(',').Any(s => s == "<Header>"))
                {
                    //if (ObjPreModel.headerflag > 0 && ObjPreModel.ArrResult.Where(c => c != null).ToArray().Count() > 0)
                    //{
                    //    ObjPreModel.PREGenFlag = 1;
                    //}
                    ObjPreModel.CardProgram = singleline.Split(',')[5];
                    ObjPreModel.IssuerNo = Convert.ToInt32(singleline.Split(',')[1]);
                    //get pre format and token
                    FunInsertTextLog("FunCreatePRE FunGetPREFileStandard Start", ObjPreModel.IssuerNo, false);
                    //ObjPreModel.CardProgram = "RBEMVPlatinum10";
                    ObjPreModel.DtPRERecord = FunGetPREFileStandard(ObjPreModel.CardProgram, ObjPreModel.IssuerNo.ToString());
                    FunInsertTextLog("FunCreatePRE FunGetPREFileStandard End", ObjPreModel.IssuerNo, false);
                    if (ObjPreModel.DtPRERecord.Rows.Count > 0)
                    {
                        ObjPreModel.PREFormat = ObjPreModel.DtPRERecord.Rows[0]["PREFormat"].ToString();
                        ObjPreModel.FileNameFormat = ObjPreModel.DtPRERecord.Rows[0]["FileNameFormat"].ToString();
                        ObjPreModel.BinPrefix = ObjPreModel.DtPRERecord.Rows[0]["CardPrefix"].ToString();
                        ObjPreModel.headerflag = 1;
                        ObjPreModel.splitfilecolumnindex = Convert.ToInt32(ObjPreModel.DtPRERecord.Rows[0]["splitfilecolumnindex"]);
                        /*ATPBA-1133 start*/
                        /*ATPBA-1133 prabhu change in dll,sp- [Sp_GetPREFields],property added in ObjPreModel */
                        FunInsertTextLog("To Check Which Order PRE Need To Be Created", ObjPreModel.IssuerNo, false);
                        FunInsertTextLog("To Check Which Order PRE Need To Be Created" + Convert.ToString(ObjPreModel.DtPRERecord.Rows[0]["OrderByField"]), ObjPreModel.IssuerNo, false);
                        ObjPreModel.OrderByField = Convert.ToInt32(ObjPreModel.DtPRERecord.Rows[0]["OrderByField"]);
                        FunInsertTextLog("To Check Which Order PRE Need To Be Created" + Convert.ToString(ObjPreModel.OrderByField), ObjPreModel.IssuerNo, false);
                        /*ATPBA-1133 end*/
                        ObjPreModel.Cardtype = ObjPreModel.DtPRERecord.Rows[0]["CardType"].ToString();
                    }
                    else
                    {
                        //FunMoveFailedFile(FilePath, InvalidOutputPath + FileName, objPath);
                        //break;
                        FunInsertTextLog("Error in HeaderProcess:No PRE Configuration Found for the cardProgram:" + ObjPreModel.CardProgram.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                        ObjPreModel.PREFormat = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in HeaderProcess:" + ex.ToString(), Convert.ToInt32(ObjPreModel.IssuerNo), true);
                throw ex;
            }
        }
        private string GetFileNameasPerFormat(string FileNameFormat, string BIN, int IntNoOfRecords, int IssuerNo, string OutPutFileName)
        {
            try
            {
                FunInsertTextLog("Creating the FileName:FileNameFormat|" + FileNameFormat + "|BIN|" + BIN + "|OutPutFileName|" + OutPutFileName, IssuerNo, true);
                System.Threading.Thread.Sleep(1000);
                string FinaleFileName = "";
                // string FileNameFormat = "Output_{MM}_{DD}_{YYYY}_{HH}_{MI}_{SS}_{Bin}_{NoOfRecords}";
                //string DD = DateTime.Now.Day.ToString();
                //string MM = DateTime.Now.Month.ToString();
                //string YYYY = DateTime.Now.Year.ToString();
                string DD = DateTime.Now.ToString("dd");
                string MM = DateTime.Now.ToString("MM");
                string YYYY = DateTime.Now.ToString("yyyy");
                string HH = DateTime.Now.ToString("HH");
                string MI = DateTime.Now.ToString("mm");
                string SS = DateTime.Now.ToString("ss");
                string FileType = string.Empty;
                //string HH = DateTime.Now.Hour.ToString("");
                //string MI = DateTime.Now.Minute.ToString("");
                //string SS = DateTime.Now.Second.ToString("");
                string COUNT = IntNoOfRecords.ToString();
                //string Bin = "";
                FinaleFileName = FileNameFormat.Replace("{DD}", DD);
                FinaleFileName = FinaleFileName.Replace("{MM}", MM);
                FinaleFileName = FinaleFileName.Replace("{YYYY}", YYYY);
                FinaleFileName = FinaleFileName.Replace("{HH}", HH);
                FinaleFileName = FinaleFileName.Replace("{MI}", MI);
                FinaleFileName = FinaleFileName.Replace("{SS}", SS);
                FinaleFileName = FinaleFileName.Replace("{BIN}", BIN);
                if (FileNameFormat.Contains("{FileType}"))
                {
                    if (OutPutFileName.ToUpper().Contains("UPGRADE"))
                    {
                        FileType = "_UPGRADE";

                    }
                    else if (OutPutFileName.ToUpper().Contains("RKIT"))
                    {
                        FileType = "_RKIT";

                    }
                    else if (OutPutFileName.ToUpper().Contains("RENEWAL"))
                    {
                        FileType = "_RENEWAL";

                    }
                    FinaleFileName = FinaleFileName.Replace("{FileType}", FileType);
                }
                if (FileNameFormat.Contains("_{COUNT}"))
                {
                    FinaleFileName = FinaleFileName.Replace("{COUNT}", COUNT);
                }
                FunInsertTextLog("PRE FILE NAME:" + FinaleFileName, IssuerNo, true);
                return FinaleFileName;
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in GetFileNameasPerFormat:" + ex.ToString(), IssuerNo, true);
                throw ex;
            }
        }

        private string GetInvalidOuptutFileValidationMessage(IEnumerable<string> strQuery, int isMagstrip, ClsPathsBO ObjData, PREModel ObjPreModel)
        {
            string msg = "";
            int ErrorRecordCount = 0;
            foreach (var item in strQuery)
            {
                DataRow toInsert = ObjData.FailedPRERecords.NewRow();
                try
                {
                    var data = item.Split(',');
                    /*ATPBF-824*/
                    /*START*/
                    ObjData.OutputErrorRecords[ErrorRecordCount] = item.ToString();

                    ObjPreModel.AllLines = ObjPreModel.AllLines.Where(w => w != ObjData.OutputErrorRecords[ErrorRecordCount]).ToArray();

                    toInsert[0] = item.ToString();




                    /*END*/
                    if (string.IsNullOrEmpty(data[0]))   //CardNum
                    {
                        msg += "Card number is blank, position:0,";
                        toInsert[2] = "Card number is blank, position:0,";
                    }
                    toInsert[3] = data[0].ToString();
                    if (string.IsNullOrEmpty(data[4])) //Expiry
                    {
                        msg += "Expiry is blank, position:4,";
                        toInsert[2] = "Expiry is blank, position:4,";
                    }
                    if (string.IsNullOrEmpty(data[11])) //CustId
                    {
                        msg += "CustId is blank, position:11,";
                        toInsert[2] = "CustId is blank, position:11,";
                    }
                    else
                    {
                        msg += "CustId: " + Convert.ToString(data[11]) + ",";
                        toInsert[1] = Convert.ToString(data[11]);
                    }
                    if (string.IsNullOrEmpty(data[15])) //cvv1
                    {
                        msg += "cvv1 is blank, position:15,";
                        toInsert[2] = "cvv1 is blank, position:15,";
                    }
                    if (string.IsNullOrEmpty(data[16])) //cvv2
                    {
                        msg += "cvv2 is blank, position:16,";
                        toInsert[2] = "cvv2 is blank, position:16,";
                    }
                    if (string.IsNullOrEmpty(data[26])) //pvv/pinoffset
                    {
                        msg += "pvv/pinoffset is blank, position:26,";
                        toInsert[2] = "pvv/pinoffset is blank, position:26,";
                    }
                    if (string.IsNullOrEmpty(data[60]))//accnt Id
                    {
                        msg += "account Id is blank, position:60,";
                        toInsert[2] = "account Id is blank, position:60,";
                    }
                    if ((isMagstrip == 0) && ((string.IsNullOrEmpty(data[64])) //icvd/iccv
                                                     || (string.IsNullOrEmpty(data[1])))) //Seq no
                    {
                        msg += "icvd/iccv is blank, position:64,1,";
                        toInsert[2] = "icvd/iccv is blank, position:64,1,";
                    }
                }
                catch (Exception ex)
                {
                    toInsert[2] = ex.ToString();
                    ErrorRecordCount++;
                    FunInsertTextLog("Error in GetInvalidOuptutFileValidationMessage:" + ex.ToString(), ObjPreModel.IssuerNo, true);
                }

                ObjData.FailedPRERecords.Rows.InsertAt(toInsert, ErrorRecordCount);

                ErrorRecordCount++;
            }
            return msg;
        }
        private string GetMaskedCardNo(string cardNumber)
        {
            // var cardNumber = "3456123434561234";

            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('X', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            //var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            return maskedString;
        }
        //Start 10/07
        private void FunMoveEncryptFile(string SourceFilePath, string DestinationFilePath, ClsPathsBO objPath)
        {
            try
            {
                FunInsertTextLog("FunMoveEncryptFile Start", Convert.ToInt32(objPath.IssuerNo), false);
                FunInsertTextLog("FunMoveEncryptFile Start" + SourceFilePath + "|" + DestinationFilePath, Convert.ToInt32(objPath.IssuerNo), false);
                File.Move(SourceFilePath, DestinationFilePath);//source dest same
                bool result = PGP_Encrypt(objPath.AGS_PGP_KeyName, DestinationFilePath, DestinationFilePath.Split('.')[0] + ".pgp", objPath.AGS_PublicKeyPath, objPath.IssuerNo);
                if (result)
                {
                    FunInsertTextLog("File to be delete" + DestinationFilePath, Convert.ToInt32(objPath.IssuerNo), false);
                    File.Delete(DestinationFilePath);//SourceFilePath and DestinationFilePath path is same //after moving file no file to delete in source but text file is there on destination//03-09-18
                    FunInsertTextLog("output source file deleted:" + DestinationFilePath, Convert.ToInt32(objPath.IssuerNo), false);
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("FunMoveEncryptFile Error" + ex.ToString(), Convert.ToInt32(objPath.IssuerNo), true);
            }
        }
        public bool PGP_Encrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo)
        {
            //PgpKeyPath = @"D:\CardAutomation\PGP\pub.asc";
            PGPModel objPGP = new PGPModel();
            objPGP.EncInputFilePath = fileFrom;
            objPGP.EncOutputFilePath = fileTo;
            objPGP.PublicKeyFilePath = PgpKeyPath;

            bool processExited = false;
            /// File info
            FileInfo fi = new FileInfo(fileFrom);
            if (!fi.Exists)
            {
                //throw new Exception("Missing file.  Cannot find the file to encrypt.");
                FunInsertTextLog("PGP_Encrypt failed|Missing file.  Cannot find the file to encrypt", Convert.ToInt32(IssuerNo), false);
                return processExited;
            }
            if (!File.Exists(PgpKeyPath))
            {
                FunInsertTextLog("PGP_Encrypt failed|Cannot find PGP Key", Convert.ToInt32(IssuerNo), false);
                return processExited;
            }
            /// Cannot encrypt a file if it already exists
            if (File.Exists(fileTo))
            {
                //throw new Exception("Cannot encrypt file.  File already exists");
                FunInsertTextLog("PGP_Encrypt : Cannot encrypt file.  File already exists PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, Convert.ToInt32(IssuerNo), false);
                return processExited;
            }
            //encrypt through AGSPGP dll
            try
            {
                new PGP().Encrypt(objPGP);
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: PGP_Encrypt Error" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            //check encrypted file created 
            if (File.Exists(fileTo))
            {
                processExited = true;
            }
            return processExited;
        }

        public void FunSFTP_UploadFailedOutFiles(ClsPathsBO objpath)
        {
            try
            {
                string UploadFailedOutputFilePath = objpath.AGS_Output + "Failed\\";
                if (Directory.GetFiles(objpath.Local_Output_Failed, "*.pgp").Count() > 0)
                {
                    foreach (string filename in Directory.GetFiles(objpath.Local_Output_Failed, "*.pgp"))
                    {

                        //var BackUpfilePath = ConfigurationManager.AppSettings["CardAutoSFTPUploaded"];
                        using (var sftp = new SftpClient(objpath.AGS_SFTPServer, Convert.ToInt32(objpath.AGS_SFTPPort), objpath.AGS_SFTPUser, objpath.AGS_SFTPPassword))   //".\\dipak.gole","Di$@12345"
                        {
                            try
                            {
                                string InputFilePath = objpath.Local_Output_Failed;

                                string[] FilesArr = Directory.GetFiles(InputFilePath, "*.pgp");

                                if (FilesArr.Count() > 0)
                                {
                                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                                    sftp.BufferSize = 5000000;
                                    sftp.Connect();
                                    foreach (string file in FilesArr)
                                    {
                                        string[] path = file.Split('\\');
                                        try
                                        {

                                            //Stream fileStream = File.OpenWrite(file);
                                            using (var fileStream = new FileStream(file, FileMode.Open))
                                            {
                                                sftp.UploadFile(fileStream, UploadFailedOutputFilePath + path[path.Length - 1], true);
                                                FunInsertTextLog("upload failed output files, path:" + UploadFailedOutputFilePath + path[path.Length - 1], Convert.ToInt32(objpath.IssuerNo), false);
                                                fileStream.Flush();
                                                fileStream.Close();
                                                fileStream.Dispose();
                                                try
                                                {
                                                    File.Delete(file);
                                                    FunInsertTextLog("Deleted failed output files", Convert.ToInt32(objpath.IssuerNo), false);
                                                }
                                                catch (Exception ex)
                                                {
                                                    FunInsertTextLog("FunSFTP_UploadFailedOutFiles|Delete failed output file|Para:Failed output path=" + file + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FunInsertTextLog("FunSFTP_UploadFailedOutFiles|Upload failed output file|Para:Failed output path=" + file + ",SFTP_FailedOutFilePath=" + objpath.AGS_Output + "Failed\\" + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                        }
                                    }
                                    sftp.Disconnect();
                                }
                            }
                            catch (Exception ex)
                            {
                                FunInsertTextLog("FunSFTP_UploadFailedOutFiles | Para:SFTP Path = " + objpath.AGS_Output + "failed\\" + ", Failed Output path = " + objpath.Local_Output_Failed + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("FunSFTP_UploadFailedOutFiles | Para:SFTP Path = " + objpath.AGS_Output + "failed\\" + ", Failed Output path = " + objpath.Local_Output_Failed + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
            }
        }

        public void FunSFTP_UploadPRE(ClsPathsBO objpath)
        {
            try
            {
                if (Directory.GetFiles(objpath.Local_PRE, "*.pgp").Count() > 0)
                {
                    using (var sftp = new SftpClient(objpath.Vendor_SFTPServer, Convert.ToInt32(objpath.Vendor_SFTPPort), objpath.Vendor_SFTPUser, objpath.Vendor_SFTPPassword))   //".\\dipak.gole","Di$@12345"
                    {
                        try
                        {
                            string InputFilePath = objpath.Local_PRE;

                            string[] FilesArr = Directory.GetFiles(InputFilePath, "*.pgp");

                            if (FilesArr.Count() > 0)
                            {
                                sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                                sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                                sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                                sftp.BufferSize = 5000000;
                                sftp.Connect();
                                foreach (string file in FilesArr)
                                {
                                    string[] path = file.Split('\\');
                                    try
                                    {
                                        using (var fileStream = new FileStream(file, FileMode.Open))
                                        {
                                            sftp.UploadFile(fileStream, objpath.Vendor_Output + path[path.Length - 1], true);
                                            fileStream.Flush();
                                            fileStream.Close();
                                            fileStream.Dispose();
                                            FunInsertTextLog("PRE created on path:" + objpath.Vendor_Output + path[path.Length - 1], Convert.ToInt32(objpath.IssuerNo), false);
                                            try
                                            {
                                                File.Delete(file);
                                                FunInsertTextLog("After upload on sftp, deleted PRE file", Convert.ToInt32(objpath.IssuerNo), false);
                                            }
                                            catch (Exception ex)
                                            {
                                                FunInsertTextLog("Error: FunSFTP_UploadPRE|Delete PRE file|Para:Failed output path=" + file + "Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FunInsertTextLog("FunSFTP_UploadPRE | Upload PRE file | Para:PRE path = " + file + ", SFTP_PREFilePath = " + objpath.Vendor_Output + "Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                    }

                                }
                                sftp.Disconnect();
                                if (Directory.GetFiles(objpath.Local_PRE, "*.pgp").Count() == 0)
                                {
                                    FunInsertTextLog("PRE file uploaded successfully", Convert.ToInt32(objpath.IssuerNo), false);
                                }
                                else
                                {
                                    FunInsertTextLog("PRE file - Failed to upload", Convert.ToInt32(objpath.IssuerNo), false);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FunInsertTextLog("FunSFTP_UploadPRE | Para:SFTP Path = " + objpath.Vendor_Output + ", PRE path = " + objpath.Local_PRE + "Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                        }
                    }
                }
                else
                {
                    FunInsertTextLog("No PRE file Generated", Convert.ToInt32(objpath.IssuerNo), false);
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: FunSFTP_UploadPRE|Para:SFTP Path=" + objpath.AGS_Output + "Failed\\" + ", Failed Output path=" + objpath.Local_Output_Failed + " Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
            }
        }

        //Start 10/07
        public void FunSFTP_UploadOutputBackUp(ClsPathsBO objpath, int issuerNo)
        {
            try
            {
                if (Directory.GetFiles(objpath.Local_Output_Backup, "*.pgp").Count() > 0)
                {
                    using (var sftp = new SftpClient(objpath.AGS_SFTPServer, Convert.ToInt32(objpath.AGS_SFTPPort), objpath.AGS_SFTPUser, objpath.AGS_SFTPPassword))
                    {
                        sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                        sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                        sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                        sftp.BufferSize = 5000000;
                        sftp.Connect();
                        foreach (string strBKFile in Directory.GetFiles(objpath.Local_Output_Backup, "*.pgp"))
                        {
                            try
                            {
                                string filename = strBKFile.Split('\\')[strBKFile.Split('\\').Length - 1];
                                using (var fileStream = new FileStream(strBKFile, FileMode.Open))
                                {
                                    //upload output file for back up
                                    sftp.UploadFile(fileStream, objpath.AGS_Output + "Backup\\" + filename, true);
                                    FunInsertTextLog("upload output file for backup", issuerNo, false);
                                    fileStream.Flush();
                                    fileStream.Close();
                                    fileStream.Dispose();
                                    try
                                    {
                                        File.Delete(strBKFile);
                                    }
                                    catch (Exception ex)
                                    {
                                        FunInsertTextLog("Error: FunSFTP_UploadOutputBackUp|Delete Output BackUp output backup path=" + strBKFile + " Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                FunInsertTextLog("Error: upload output file backup exception|backup path=" + strBKFile + " Error:" + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                            }
                        }
                        sftp.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in FunSFTP_UploadOutputBackUp:" + ex.ToString(), issuerNo, true);
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

        //Get output file from SFTP
        public void FunGetSFTP_Output_Files_ToGeneratePRE(ClsPathsBO objPath, int IssurNo)
        {
            var PREInputPath = objPath.Local_Output_Input;
            //var SFTP_outputbackup_Path = objPath.Bank_Output_BackUp;
            var SFTP_OutputFailed = objPath.AGS_Output + "Failed\\";
            // string rarFilePath = objPath.ZipCardFilesPath,
            string BankName = objPath.BankName;
            FunInsertTextLog("SFTP_FileSourcePath path:" + objPath.AGS_Output, IssurNo, false);
            FunInsertTextLog("SFTP_OutputFailed path:" + SFTP_OutputFailed, IssurNo, false);

            try
            {

                using (var sftp = new SftpClient(objPath.AGS_SFTPServer, Convert.ToInt32(objPath.AGS_SFTPPort), objPath.AGS_SFTPUser, objPath.AGS_SFTPPassword))   //".\\dipak.gole","Di$@12345"
                {
                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftp.BufferSize = 5000000;
                    sftp.Connect();
                    string remoteDirectory = objPath.AGS_Output;
                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains(".txt")) select i)
                    {

                        using (Stream fileStream = File.OpenWrite(Path.Combine(PREInputPath, d.Name)))
                        {
                            sftp.DownloadFile(d.FullName, fileStream);
                            // d.MoveTo(SFTP_outputbackup_Path + d.Name);
                            sftp.DeleteFile(d.FullName);
                            //new change
                            fileStream.Flush();
                            fileStream.Close();
                            fileStream.Dispose();
                        }
                    }
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: FunGetSFTP_Output_Files_ToGeneratePRE|Para: SFTP_OutFileSourcePath = " + objPath.AGS_Output + "PREInputPath = " + PREInputPath + "SFTP_OutputFailed = " + SFTP_OutputFailed + " Error:" + ex.ToString(), IssurNo, true);
            }
        }

        public DataTable FunGetPREFileStandard(string Cardprogram, string IssuerNo)
        {
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetPREFields", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, Cardprogram);
                    ObjDtOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataTable();
                ObjConn.Close();
                FunInsertTextLog("Error: FunGetPREFileStandard" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return ObjDtOutPut;
        }

        public DataTable FunGetFINALPRERECORDS(string FILEID, string IssuerNo)
        {
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["RBLCardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GETFINALRECORDSFORPRE", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@FILEID", SqlDbType.VarChar, 0, ParameterDirection.Input, FILEID);
                    ObjDtOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataTable();
                ObjConn.Close();
                FunInsertTextLog("Error: FunGetFINALPRERECORDS" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return ObjDtOutPut;
        }

        public class ClsPathsBO
        {
            public string ID { get; set; }//id from tblbanks(bankid)
            public string BankName { get; set; }
            public string IssuerNo { get; set; }
            public string Bank_Output { get; set; }//SFTP_OutputFile_Path
            public string AGS_SFTPServer { get; set; }//AGS_SFTPServer
            public string AGS_SFTPUser { get; set; }//AGS_SFTP_User
            public string AGS_SFTPPassword { get; set; }//AGS_SFTP_Pwd
            public string AGS_SFTPPort { get; set; }//AGS_SFTP_Port


            public string Bank_SFTPServer { get; set; }//AGS_SFTPServer
            public string Bank_SFTPUser { get; set; }//AGS_SFTP_User
            public string Bank_SFTPPassword { get; set; }//AGS_SFTP_Pwd
            public string Bank_SFTPPort { get; set; }//AGS_SFTP_Port

            public string AGS_Input { get; set; }//AGS Input
            public string AGS_Output { get; set; }//AGS output
            public string Vendor_SFTPServer { get; set; }//C_SFTPServer
            public string Vendor_SFTPUser { get; set; }//C_SFTPUser
            public string Vendor_SFTPPassword { get; set; }//C_SFTPPassword
            public string Vendor_SFTPPort { get; set; }//C_SFTPPort
            public string Vendor_Output { get; set; }//C_SFTP_PRE_Path
            public string Local_Output_Input { get; set; }//PRE_Input_Path
            public string Local_PRE { get; set; }//PRE_Output_Path
            public string Local_Output_Failed { get; set; }//Outputfile_failed_Path
            public string Local_Output_Backup { get; set; }//OutputFile_BK_Path
            public string ErrorLogPath { get; set; }
            public string PGP_KeyName { get; set; }
            //public string PGPPassword { get; set; }//PGP_PWD
            public string PREPGPPublicKeyPath { get; set; }//PubKey_Path used to Encrypt PGP File
            //public string PGPSecretKeyPath { get; set; }//SecKey_Path
            public string AGS_PGP_KeyName { get; set; }
            public string AGS_PGPPassword { get; set; }
            public string AGS_PublicKeyPath { get; set; }
            public string AGS_SecretKeyPath { get; set; }
            public Boolean isstatusFileINPGP { get; set; }
            public string StatusFilePGPPublicKeyPath { get; set; }
            public bool EncryptOutputFileToPGP { get; set; }
            public string SSH_Private_key_file_Path { get; set; }
            public string passphrase { get; set; }
            /*ATPBF-824*/
            /*START*/
            public string[] OutputErrorRecords { get; set; }
            public DataTable FailedPRERecords { get; set; }

            public string Local_Input { get; set; }

            /*END*/
        }

        public ClsPathsBO FunGetPaths(int IssuerNo, int IntPara, string ProcessId)
        {
            ClsPathsBO objPath = new ClsPathsBO();
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_CA_GetPaths_For_CardGen_ISO", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.Int, 0, ParameterDirection.Input, IntPara);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, ProcessId);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        List<ClsPathsBO> objList = new List<ClsPathsBO>();
                        objPath = BindDatatableToClass<ClsPathsBO>(ObjDTOutPut);
                    }
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                objPath.ID = string.Empty;
                FunInsertTextLog("Error: FunGetPaths" + ex.ToString(), IssuerNo, true);
            }
            return objPath;
        }
        public string FunGetFileId(int IssuerNo)
        {
            string FileID = "";
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAutomationCurrentFileID", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    //ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.Int, 0, ParameterDirection.Input, IntPara);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        FileID = ObjDTOutPut.Rows[0][0].ToString();
                    }
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error: FunGetPaths" + ex.ToString(), IssuerNo, true);
            }
            return FileID;
        }

        public DataTable FunGetFileIdAndFileName(int IssuerNo)
        {

            DataTable ObjDTOutPut = new DataTable();
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAutomationCurrentFileIDAndFileName", ObjConn, CommandType.StoredProcedure))
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
                FunInsertTextLog("Error: FunGetPaths" + ex.ToString(), IssuerNo, true);
            }
            return ObjDTOutPut;
        }
        //start Diksha 19/07
        //Check CardProgram is of magstrip 
        public int FunCheckMagstrip(string CardPogram, string IssuerNo)
        {
            int result = 0;
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                SqlConnection ObjConn = null;
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["CardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CheckMagstripCard", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, CardPogram);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        result = Convert.ToInt16(ObjDTOutPut.Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Error in FunCheckMagstrip:" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return result;
        }
        //update PRE Status
        public int FunUpdatePREStatus(string IssuerNo, DataTable ObjDT, string CurrentFileID, string name)
        {
            int result = 0;
            SqlConnection ObjConn = null;
            try
            {
                FunInsertTextLog("FunUpdatePREStatus", Convert.ToInt32(IssuerNo), false);
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["RBLCardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdatePREStatus", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@TblCardAutomationStatus", SqlDbType.Structured, 0, ParameterDirection.Input, ObjDT);//structured
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, CurrentFileID);
                    ObjCmd.AddParameterWithValue("@PREFilename", SqlDbType.VarChar, 0, ParameterDirection.Input, name);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                FunInsertTextLog("FunUpdatePREStatus Error:" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return result;
        }

        /*ATPBF-823*/
        /*START*/
        public int FunUpdateFaildPRERecords(string IssuerNo, DataTable ObjDT, string CurrentFileID)
        {
            int result = 0;
            SqlConnection ObjConn = null;
            try
            {
                FunInsertTextLog("FunUpdatePREStatus", Convert.ToInt32(IssuerNo), false);
                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["RBLCardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.FunUpdateFaildPRERecords", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@FailedPRERecord", SqlDbType.Structured, 0, ParameterDirection.Input, ObjDT);//structured
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, CurrentFileID);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjConn.Close();
                FunInsertTextLog("FunUpdatePREStatus Error:" + ex.ToString(), Convert.ToInt32(IssuerNo), true);
            }
            return result;
        }
        /*END*/
        //end Diksha 19/07

        //**************** Save Error Logs ************
        public void FunDBLog(string procedureName, string errorDesc, string parameterList, string IssuerNo, string BatchNo)
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
                FunInsertIntoLogFile_1(ErrorLogFilePath, ex, "FunInsertIntoErrorLog");
                //FunInsertIntoErrorLog("FunInsertIntoErrorLog", ex.Message, "", IssuerNo.ToString(), string.Empty);
            }
        }

        public void FunInsertIntoLogFile_1(string LogPath, Exception ex, string functionName)
        {
            try
            {
                //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
                string filename = "PRE_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
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
                //FunInsertIntoErrorLog("FunInsertIntoLogFile", Ex.Message, "LogPath=" + LogPath, "", string.Empty);
            }
        }

        //public void Test()
        // {
        //     //string[] aData = singleline.Split(',');
        //     //aData.Contains("");

        //     //StringBuilder sbFile = new StringBuilder();




        //     ////Start for service code
        //     //string[] SrvCodeArr = (aData.Where(x => x.Contains("=")).FirstOrDefault()).Split('=');
        //     //string StrSrvCode = SrvCodeArr[SrvCodeArr.Length - 1];
        //     //string ServiceCode = StrSrvCode.Substring(4, 3); //skip expiry date then 3 digits
        //     ////end


        //     //aOData[0] = "$";
        //     //aOData[1] = aData[0];
        //     //aOData[2] = "!$";
        //     //aOData[3] = aData[16]; //CVV2
        //     //aOData[4] = "!$";
        //     //aOData[5] = aData[4].PadRight(2) + "/" + aData[4].PadLeft(2);//expiry period
        //     //aOData[6] = "!$";
        //     //aOData[7] = aData[6]; //name on card
        //     ////Track 1
        //     //aOData[8] = "!+";
        //     //aOData[9] = "%";
        //     //aOData[10] = "B";
        //     //aOData[11] = aData[0]; //cardNo
        //     //aOData[12] = "^";
        //     //aOData[13] = aData[6]; //name on card
        //     //aOData[14] = "/";
        //     //aOData[15] = "^";
        //     //aOData[16] = aData[4];//Expiry date
        //     //aOData[17] = ServiceCode;
        //     //aOData[18] = aData[15]; //cvv1
        //     //aOData[19] = "A";
        //     //aOData[20] = "";//adhar/UID
        //     //aOData[21] = "00000000";
        //     //aOData[22] = "?";
        //     ////track 2
        //     //aOData[23] = ";";
        //     //aOData[24] = aData[0]; //card no
        //     //aOData[25] = "=";
        //     //aOData[26] = aData[4]; //expiry date
        //     //aOData[27] = ServiceCode;
        //     //aOData[28] = aData[15]; //cvv1
        //     //aOData[29] = "0000000";
        //     //aOData[30] = aData[1];  //seq no
        //     //aOData[31] = "?";
        //     //aOData[32] = "+1";
        //     //aOData[33] = "~";
        //     //aOData[34] = ""; //branch code
        //     //aOData[35] = "~";
        //     //aOData[36] = aData[6]; //name
        //     //aOData[37] = "~";
        //     //aOData[38] = aData[27]; //addr 1
        //     //aOData[37] = "~";
        //     //aOData[39] = aData[28]; //addr 2
        //     //aOData[40] = "~";
        //     //aOData[41] = aData[29]; //addr 3
        //     //aOData[42] = "~";
        //     //aOData[43] = aData[30]; //addr 4
        //     //aOData[44] = "~";
        //     //aOData[45] = aData[31]; //addr 5
        //     //aOData[46] = "~";
        //     //aOData[47] = aData[32]; //addr 6
        //     //aOData[48] = "~";
        //     //aOData[49] = aData[46]; //telephone
        //     //aOData[50] = "~";
        //     //aOData[51] = aData[46]; //mobile
        //     //aOData[52] = "~";
        //     //aOData[53] = "";//fax
        //     //aOData[54] = "~";
        //     //aOData[55] = "";//acct no
        //     //aOData[56] = "~";
        //     //aOData[57] = ""; //withdrawl limit
        //     //aOData[58] = "~";
        //     //aOData[59] = aData[11];//custID
        //     //aOData[60] = "~";
        //     //aOData[61] = aData[aData.Length - 1]; //iCVD
        // }

        public static T BindDatatableToClass<T>(DataTable dt)
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
                string Name = propertyInfo.Name;
                if (columns.Select(x => x.Equals(Name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                {
                    // Fill the data into the property
                    try
                    {
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);
                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                        propertyInfo.SetValue(ob, safeValue, null);
                    }
                    catch (Exception ex)
                    {

                    }


                    // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                }
            }

            return ob;
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void FunInsertTextLog(string Message, int issuerNo, bool IsError)
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

        public void FunInsertTextLogSensitive(string Message, int issuerNo, bool IsError, bool IsSensitiveSave)
        {
            if (IsSensitiveSave)
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
        }

        public bool DATATableToTextFile(DataTable submittedDataTable, string FileNameWithPath, int IssurNo)
        {
            try
            {
                FunInsertTextLog("DATATableToTextFile Creating Rejected records file ", IssurNo, false);
                int i = 0;
                StreamWriter sw = null;
                sw = new StreamWriter(FileNameWithPath, false);
                foreach (DataRow row in submittedDataTable.Rows)
                {
                    object[] array = row.ItemArray;

                    for (i = 0; i < array.Length - 1; i++)
                    {
                        sw.Write(array[i].ToString() + ";");
                    }
                    sw.Write(array[i].ToString());
                    sw.WriteLine();
                }
                sw.Close();
                return true;
            }
            catch (Exception Ex)
            {
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", objData.ProcessId.ToString(), "", " ", objData.IssuerNo, 1);
                FunInsertTextLog("DATATableToTextFile Error:" + Ex.ToString(), IssurNo, false);
                return false;
            }
        }
        internal DataTable GETRejectRecords(int IssuerNo, string FileId, string ProcessId)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                //FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "DB Connection Started to Check reject records to create reject file.", ObjData.IssuerNo.ToString(), 1);
                FunInsertTextLog("DB Connection Started to Check reject records to create reject file", IssuerNo, false);

                ObjConn = new SqlConnection(parse(System.Configuration.ConfigurationManager.ConnectionStrings["RBLCardAutomationISO"].ConnectionString));
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetRejectRecordToReProcess", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo.ToString());
                    ObjCmd.AddParameterWithValue("@FileId", SqlDbType.VarChar, 0, ParameterDirection.Input, FileId);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.VarChar, 0, ParameterDirection.Input, ProcessId);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("GETRejectRecords: Error: " + ex.ToString(), IssuerNo, false);
                return ObjDsOutPut;
            }
            return ObjDsOutPut;
        }

        public void FunSFTP_UploadExcludedRecordPERFile(ClsPathsBO objpath)
        {
            try
            {
                string UploadFailedOutputFilePath = objpath.AGS_Output + "ExcludedRecordPRE\\";

                FunInsertTextLog("upload excluded PRE record files, SFTPPath:" + UploadFailedOutputFilePath + " Local path:" + objpath.Local_PRE + "ExcludedRecordPRE\\", Convert.ToInt32(objpath.IssuerNo), false);

                if (Directory.GetFiles(objpath.Local_PRE + "ExcludedRecordPRE\\", "*.pgp").Count() > 0)
                {
                    foreach (string filename in Directory.GetFiles(objpath.Local_PRE + "ExcludedRecordPRE\\", "*.pgp"))
                    {
                        //var BackUpfilePath = ConfigurationManager.AppSettings["CardAutoSFTPUploaded"];
                        using (var sftp = new SftpClient(objpath.AGS_SFTPServer, Convert.ToInt32(objpath.AGS_SFTPPort), objpath.AGS_SFTPUser, objpath.AGS_SFTPPassword))   //".\\dipak.gole","Di$@12345"
                        {
                            try
                            {
                                string InputFilePath = objpath.Local_PRE + "ExcludedRecordPRE\\";

                                string[] FilesArr = Directory.GetFiles(InputFilePath, "*.pgp");

                                if (FilesArr.Count() > 0)
                                {
                                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                                    sftp.BufferSize = 5000000;
                                    sftp.Connect();
                                    foreach (string file in FilesArr)
                                    {
                                        string[] path = file.Split('\\');
                                        try
                                        {

                                            //Stream fileStream = File.OpenWrite(file);
                                            using (var fileStream = new FileStream(file, FileMode.Open))
                                            {
                                                sftp.UploadFile(fileStream, UploadFailedOutputFilePath + path[path.Length - 1], true);
                                                FunInsertTextLog("upload excluded PRE record files, path:" + UploadFailedOutputFilePath + path[path.Length - 1], Convert.ToInt32(objpath.IssuerNo), false);
                                                fileStream.Flush();
                                                fileStream.Close();
                                                fileStream.Dispose();
                                                try
                                                {
                                                    File.Delete(file);
                                                    FunInsertTextLog("Deleted excluded PRE record files , path:" + file, Convert.ToInt32(objpath.IssuerNo), false);
                                                }
                                                catch (Exception ex)
                                                {
                                                    FunInsertTextLog("FunSFTP_UploadExcludedRecordPERFile|Delete Excluded record PRE file|Para:path=" + file + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FunInsertTextLog("FunSFTP_UploadExcludedRecordPERFile|Excluded record PRE file|Para:Excluded record PRE file path=" + file + ",SFTP_ExcludedRecordPRE=" + objpath.AGS_Output + "ExcludedRecordPRE\\" + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                                        }
                                    }
                                    sftp.Disconnect();
                                }
                            }
                            catch (Exception ex)
                            {
                                FunInsertTextLog("FunSFTP_UploadExcludedRecordPERFile | Para:SFTP Path = " + objpath.AGS_Output + "ExcludedRecordPRE\\" + ", Failed Output path = " + objpath.Local_PRE + "ExcludedRecordPRE\\" + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("FunSFTP_UploadExcludedRecordPERFile | Para:SFTP Path = " + objpath.AGS_Output + "ExcludedRecordPRE\\" + ", Failed Output path = " + objpath.Local_PRE + "ExcludedRecordPRE\\" + "Error: " + ex.ToString(), Convert.ToInt32(objpath.IssuerNo), true);
            }
        }

    }
    public class PREModel
    {

        public int splitfilecolumnindex { get; set; }
        public string[] AllLines { get; set; }
        public string FilePath { get; set; }
        public string InvalidOutputPath { get; set; }
        public DataTable DtPRERecord { get; set; }
        public DataTable DtPREStatus { get; set; }
        public string OutputFile { get; set; }
        public string[] ArrResult { get; set; }
        public string PREFormat { get; set; }
        public string FileNameFormat { get; set; }
        public string BinPrefix { get; set; }
        public int headerflag { get; set; }
        public string Cardtype { get; set; }
        public int IssuerNo { get; set; }
        public string CardProgram { get; internal set; }
        public int PREGenFlag { get; internal set; }
        public string PREFilePath { get; internal set; }
        public string FileName { get; internal set; }
        public bool IsFileMove { get; set; }
        public int count { get; set; }
        public int InputLine { get; internal set; }
        public int TotalCount { get; internal set; }

        /*ATPBA-1133 OrderByField property added*/
        public int OrderByField { get; set; }
        /* Prabhu PRE Repin flag sheetal start ATPCM-681*/
        public bool IsRepin { get; set; }

        public DataTable DTFINALPRERECORDS { get; set; }
        public string[] FINALArrRESULT { get; set; }
        public int FinalCount { get; set; }
        public int IsOnlyFileIdRecords { get; set; }

        public string[] DifferArrRESULT { get; set; }
        public int DifferCount { get; set; }
    }
}
