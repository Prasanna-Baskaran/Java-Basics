/************************************************************************
Object Name: RBL CardAutomation
Purpose:Automate Card Generation for RBL
Change History
-------------------------------------------------------------------------
Date            Changed By          Reason
-------------------------------------------------------------------------
16-Sept-2017    Diksha Walunj       Newly Developed
01-Oct-2017     Prerna              ATPCM-123: Fees modification (for Reffernce)
07-08-2018      Pratik M            ATPCM-576: RBL Card Automation with PGP functionality - PHASE I
*************************************************************************/




using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SqlClient;
using System.Configuration;
using Renci.SshNet;
using System.IO.Compression;
using AGSPGP;

namespace AGS.RBLCardAutomation
{
    public class RBLCardAuto : IDisposable
    {
        public string ErrorLogFilePath { get; set; }
        public RBLCardAuto()
        {
            ErrorLogFilePath = string.Empty;
        }
        public void Process(int IssurNo)
        {
            try
            {
                int iResult = 0;
                ClsPathsBO objPath = new ClsPathsBO();

                objPath = FunGetPaths(IssurNo);
                if (!string.IsNullOrEmpty(objPath.ID))
                {
                    //ATPCM-576 Start
                    if (!string.IsNullOrEmpty(objPath.SecKey_Path))
                    {
                        objPath.FileExtention = ".pgp";
                    }
                    else
                    {
                        objPath.FileExtention = ".txt";
                    }
                    //ATPCM-576 End

                    ErrorLogFilePath = objPath.ErrorLogPath;
                    FunInsertIntoErrorLog("RBLCardAuto|Process", "Process Started", "", IssurNo.ToString(), "");
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "Process Started");

                    DataTable ObjDtProcess = new DataTable();
                    FunInsertIntoErrorLog("RBLCardAuto.cs|FunGetAllCardGenProcess", "Fetching Process ID ", "", IssurNo.ToString(), "");
                    ObjDtProcess = FunGetAllCardGenProcess(IssurNo);
                    FunInsertIntoErrorLog("RBLCardAuto.cs|FunGetAllCardGenProcess", "Process ID Count Found:" + ObjDtProcess.Rows.Count, "", IssurNo.ToString(), "");
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunGetAllCardGenProcess Called");

                    if (ObjDtProcess != null && ObjDtProcess.Rows.Count > 0)
                    {
                        objPath.ProcessID = 0;
                        if (ObjDtProcess.Columns.Contains("ProcessID") && ObjDtProcess.Columns.Contains("FileName"))
                        {
                            var ProcessIDlst = ((from r in ObjDtProcess.AsEnumerable()
                                                 select r["ProcessID"].ToString()).Distinct().ToList());

                            foreach (string ProcessID in ProcessIDlst)
                            {

                                objPath.ProcessID = Convert.ToInt16(ProcessID);

                                if (objPath.ProcessID != 0)
                                {
                                    List<string> FileNamelst = ObjDtProcess.AsEnumerable()
                                        .Where(x => x.Field<string>("ProcessID") == ProcessID.ToString())
                                        .Select(x => x.Field<string>("FileName")).ToList();
                                    objPath.FileName = FileNamelst[0];

                                    FunInsertIntoErrorLog("RBLCardAuto.cs|FunInitiateCardProcess", "Executing Process ID :" + ProcessID, "", IssurNo.ToString(), "");
                                    FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunInitiateCardProcess Called");
                                    iResult = FunInitiateCardProcess(objPath);
                                    //if any process fail
                                    if (iResult != 1)
                                    {
                                        FunInsertIntoLogFile(ErrorLogFilePath, null, "Process|ProcessID:" + ProcessID);
                                        FunInsertIntoErrorLog("Process", "Process Failed", "ProcessID:" + ProcessID, IssurNo.ToString(), "");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (iResult != 1)
                {
                    FunInsertIntoErrorLog("RBLCardAuto|Process", "Process Failed", "", IssurNo.ToString(), "");
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "Process Failed");
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process");
                FunInsertIntoErrorLog("Process", ex.Message, "", IssurNo.ToString(), "");
            }
        }


        //*********** Classes ************
        public class ClsReturnStatusBO
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public string OutPutCode { get; set; }
            public int OutPutID { get; set; }
        }

        public class ClsPathsBO
        {
            public string ID { get; set; }
            public string BankName { get; set; }
            public string AGS_SFTPServer { get; set; }
            public string AGS_SFTPPath { get; set; }
            public string AGS_SFTP_User { get; set; }
            public string AGS_SFTP_Pwd { get; set; }
            public string AGS_SFTP_Port { get; set; }
            public string SFTP_CIF_Source_Path { get; set; }
            public string CardCIF_Input_Path { get; set; }
            public string CardCIF_Backup { get; set; }
            public string ZipCardFilesPath { get; set; }
            public string CardAutoSourcePath { get; set; }
            public string CardAutoOutputPath_SFTP { get; set; }
            public string PRE_InputPath { get; set; }
            public string PRE_ProcessPath { get; set; }
            public string PRE_OutputPath { get; set; }
            public string PRE_BackUp_Path { get; set; }
            public string B_SFTPServer { get; set; }
            public string B_SFTPPath { get; set; }
            public string B_SFTP_User { get; set; }
            public string B_SFTP_Pwd { get; set; }
            public string B_SFTP_Port { get; set; }
            public string B_PRE_DestinationPath_SFTP { get; set; }
            public string Zip_Exe_Path { get; set; }
            public string SFTP_CIF_BackUp_Path { get; set; }
            public string CardAutoBackUpPath { get; set; }
            public string CardAutoFailedPath { get; set; }
            public string IsSaveError { get; set; }
            public string IssuerNo { get; set; }
            public string ErrorLogPath { get; set; }
            public string FailedCIFPath { get; set; }
            public string B_SFTP_FailedCIFPath { get; set; }
            public int ProcessID { get; set; }
            public string FileName { get; set; }
            public string BAT_SourceFilePath { get; set; }
            public string BAT_SourceFilePath_BK { get; set; }
            public string SFTP_BAT_SourceFilePath { get; set; }
            public string SFTP_BAT_SourceFilePath_BK { get; set; }
            public string SFTP_OutputFile_BK_Path { get; set; }

            //ATPCM-576 Start
            public string FileExtention { get; set; }
            public string SecKey_Path { get; set; }
            public string PGP_KeyName { get; set; }
            public string PGP_PWD { get; set; }
            public string PubKey_Path { get; set; }

            public string SFTPKeyPath { get; set; }
            public string SFTPKeypassphrase { get; set; }
            //ATPCM-576 End

        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Get all configuration paths for file processing
        /// </summary>
        /// <param name="IssuerNo">Bank IssuerNo</param>
        /// <returns>ClsPathsBO</returns>
        public ClsPathsBO FunGetPaths(int IssuerNo)
        {
            ClsPathsBO objPath = new ClsPathsBO();

            DataTable ObjDTOutPut = new DataTable();

            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_CA_GetPaths_For_CardGen", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {

                        List<ClsPathsBO> objList = new List<ClsPathsBO>();
                        // objList = ConvertToList<ClsPathsBO>(ObjDTOutPut);

                        objPath = BindDatatableToClass<ClsPathsBO>(ObjDTOutPut);
                    }

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                objPath.ID = string.Empty;
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetPaths|Para : IssuerNo = " + IssuerNo);
                FunInsertIntoErrorLog("FunGetPaths", ex.Message, "IssuerNo=" + IssuerNo, IssuerNo.ToString(), string.Empty);


            }
            return objPath;
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Get All card gen process .failed first then seq wise
        /// <param name="IssuerNo"> Bank IssuerNo</param>
        /// <returns>DataTable</returns>
        private DataTable FunGetAllCardGenProcess(int IssuerNo)
        {
            DataTable ObjDtOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_CAGetAllCardGenProcess", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDtOutPut = ObjCmd.ExecuteDataTable();

                    ObjCmd.Dispose();
                    ObjConn.Close();

                }
            }
            catch (Exception ex)
            {
                ObjDtOutPut = new DataTable();
                ObjConn.Close();
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetAllCardGenProcess");
                FunInsertIntoErrorLog("RBLCardAuto.cs|FunGetAllCardGenProcess", ex.Message, "", IssuerNo.ToString(), "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDtOutPut;


        }
        //--------------------------------------------------------------
        /// <summary>
        /// Initiate Card processes, Generate Cards /Accounts Files
        /// </summary>
        /// <returns></returns>
        private int FunInitiateCardProcess(ClsPathsBO ObjPath)
        {
            int iresult = 0;
            try
            {
                //CIF Processing
                if (ObjPath.ProcessID == 1)
                {
                    FunInsertIntoErrorLog("RBLCardAuto.cs|FunCIFProcess", "FunCIFProcess is Initiated..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "FunCIFProcess is Initiated..Process ID :" + ObjPath.ProcessID);
                    iresult = FunCIFProcess(ObjPath);
                }
                //for Card Upgrade & Renew process 
                else if (ObjPath.ProcessID == 5 || ObjPath.ProcessID == 4)
                {
                    //Fetch Zip files from SFTP and Process
                    iresult = FunUpgradeRenewCardProcess(ObjPath);

                }
                else
                {
                    // Check Files not exists on  CardAuto i/p path 
                    if ((Directory.GetFiles(ObjPath.BAT_SourceFilePath, "*.txt").Length == 0) && (Directory.GetFiles(ObjPath.CardAutoSourcePath, "*.txt").Length == 0))
                    {
                        // Download card accounts Files from DB
                        //for Reissue Process
                        if (ObjPath.ProcessID == 6)
                        {
                            //Generate CardReissue File
                            DownloadReissueFile(Convert.ToInt16(ObjPath.IssuerNo), ObjPath.ZipCardFilesPath, ObjPath.BAT_SourceFilePath, ObjPath.CardAutoFailedPath, ObjPath.IsSaveError);

                        }
                        else
                        {
                            FunInsertIntoErrorLog("RBLCardAuto.cs|DownloadFiles", "DownloadFiles Process is Initiated..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "DownloadFiles Process is Initiated..Process ID :" + ObjPath.ProcessID);
                            DownloadFiles(Convert.ToInt16(ObjPath.IssuerNo), ObjPath.ZipCardFilesPath, ObjPath.BAT_SourceFilePath, ObjPath.CardAutoFailedPath, ObjPath.IsSaveError);
                        }

                    }
                    //Fetch by name from source path and move to cardAuto i/p and then SFTP folder
                    //FunInsertIntoErrorLog("RBLCardAuto.cs|FunFileUploadSFTP", "FunFileUploadSFTP is Initiated To upload file on the basis of process ID..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                    FunFileUploadSFTP(ObjPath);

                    //final Card genration 

                    if (new DirectoryInfo(ObjPath.CardAutoSourcePath).GetFiles("*.txt").Where(file => file.Name.EndsWith(".txt") && file.Name.Contains(ObjPath.FileName)).Select(file => file.Name).ToList().Count() > 0)
                    {
                        ClsReturnStatusBO ObjCardGen = new ClsReturnStatusBO();
                        FunInsertIntoErrorLog("RBLCardAuto.cs|FunCardGeneration", "FunCardGeneration Process is Initiated..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                        FunInsertIntoLogFile(ErrorLogFilePath, null, "FunCardGeneration Process is Initiated..Process ID :" + ObjPath.ProcessID);
                        ObjCardGen = FunCardGeneration(ObjPath);

                        //on Success
                        if (ObjCardGen.OutPutCode == "00")
                        {
                            iresult = 1;
                        }
                    }
                    else
                    {
                        iresult = 1;
                    }
                }
            }
            catch (Exception Ex)
            {
                iresult = 0;
                FunInsertIntoErrorLog("FunInitiateCardProcess", Ex.Message, "", ObjPath.IssuerNo, "");
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "FunInitiateCardProcess");
            }
            return iresult;
        }

        //---------------------------------------------------------------
        /// <summary>
        /// CIF File Processing and do validation
        /// </summary>
        /// <returns></returns>
        private int FunCIFProcess(ClsPathsBO ObjPath)
        {
            int iresult = 0;
            try
            {
                ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
                //get File From SFTP

                FunInsertIntoErrorLog("RBLCardAuto.cs|FunGetSFTP_CIF_Files", "FunGetSFTP_CIF_File Process Initiated..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunGetSFTP_CIF_Files|FunGetSFTP_CIF_File Process Initiated..Process ID :" + ObjPath.ProcessID);
                FunGetSFTP_CIF_Files(ObjPath);
                //Process CIF Files
                if (Directory.GetFiles(ObjPath.CardCIF_Input_Path, "*.txt").Count() > 0)
                {

                    foreach (string filePath in Directory.GetFiles(ObjPath.CardCIF_Input_Path, "*.txt"))
                    {
                        iresult = 0;
                        FunInsertIntoErrorLog("RBLCardAuto.cs|ExtractFrmCIF_File", "CIF File Received and Process Initiated..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                        FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|ExtractFrmCIF_File|CIF File Received and Process Initiated..Process ID :" + ObjPath.ProcessID);
                        ObjResult = ExtractFrmCIF_File(filePath, Convert.ToInt16(ObjPath.IssuerNo), ObjPath.FailedCIFPath);
                        if (ObjResult.Code == 0)
                        {
                            try
                            {
                                //Failed CIF with invalid Validation upload to SFTP in Failed CIF Folder
                                if (Directory.GetFiles(ObjPath.FailedCIFPath, "*.txt").Count() > 0)
                                {
                                    foreach (string filename in Directory.GetFiles(ObjPath.FailedCIFPath, "*.txt"))
                                    {
                                        FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|SFTP_FailedCIFUpload|Moving Failuer File On SFTP Path: " + ObjPath);
                                        SFTP_FailedCIFUpload(ObjPath);
                                    }
                                }
                                //move cif file to backup folder after saving in database
                                File.Move(filePath, ObjPath.CardCIF_Backup + FunGetNewFileName(filePath.Split('\\')[filePath.Split('\\').Length - 1]));
                                iresult = 1;
                            }
                            catch (Exception ex)
                            {

                                FunInsertIntoErrorLog("FunCIFProcess|Move CIF file to BackUp", ex.Message, "filePath=" + filePath + "BackUp Path=" + ObjPath.CardCIF_Backup + filePath.Split('\\')[filePath.Split('\\').Length - 1], ObjPath.IssuerNo, string.Empty);
                                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunCIFProcess|Move CIF file to BackUp");

                            }
                        }
                        else
                        {
                            //move cif file to failed folder as file can not be process
                            //File.Move(filePath, ObjPath.FailedCIFPath +"\\"+ FunGetNewFileName(filePath.Split('\\')[filePath.Split('\\').Length - 1]));
                            File.Move(filePath, Path.Combine(ObjPath.FailedCIFPath, FunGetNewFileName(filePath.Split('\\')[filePath.Split('\\').Length - 1])));

                            FunInsertIntoErrorLog("FunCIFProcess", "CIF file data not save", "filePath=" + filePath, ObjPath.IssuerNo, string.Empty);
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "FunCIFProcess|CIF file data not save|Para :filePath=" + filePath);
                        }
                    }
                }
                else
                {
                    iresult = 1;
                }
            }
            catch (Exception Ex)
            {
                FunInsertIntoErrorLog("FunCIFProcess", Ex.Message, "CIF_Input:" + ObjPath.CardCIF_Input_Path + ",CIF_BK:" + ObjPath.CardCIF_Backup + ",SFTP_CIF_Input:" + ObjPath.SFTP_CIF_Source_Path + ",SFTP_CIF_BK:" + ObjPath.SFTP_CIF_BackUp_Path, ObjPath.IssuerNo, "");
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "FunCIFProcess");
            }

            //Failed CIF which unable to process  upload to SFTP in Failed CIF Folder
            if (Directory.GetFiles(ObjPath.FailedCIFPath, "*.txt").Count() > 0)
            {
                foreach (string filename in Directory.GetFiles(ObjPath.FailedCIFPath, "*.txt"))
                {
                    SFTP_FailedCIFUpload(ObjPath);
                    iresult = 1;
                }
            }

            return iresult;
        }
        //---------------------------------------------------------------
        /// <summary>
        /// Card Upgrade& Card Renew Process .Get file from SFTP
        /// </summary>
        /// <param name="objPath"></param>
        /// <returns></returns>
        private int FunUpgradeRenewCardProcess(ClsPathsBO objPath)
        {
            int iResult = 0;
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();

            try
            {
                //Get upgrade File From SFTP       
                using (var sftp = new SftpClient(objPath.AGS_SFTPServer, Convert.ToInt32(objPath.AGS_SFTP_Port), objPath.AGS_SFTP_User, objPath.AGS_SFTP_Pwd))   //".\\dipak.gole","Di$@12345"
                {
                    sftp.Connect();
                    string remoteDirectory = objPath.SFTP_BAT_SourceFilePath;
                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains(objPath.FileName)) select i)
                    {

                        using (Stream fileStream = File.OpenWrite(Path.Combine(objPath.BAT_SourceFilePath, d.Name)))
                        {
                            sftp.DownloadFile(d.FullName, fileStream);
                            //check  folder exists
                            if (!sftp.Exists(objPath.SFTP_BAT_SourceFilePath_BK))
                            {
                                sftp.CreateDirectory(objPath.SFTP_BAT_SourceFilePath_BK);
                            }

                            //move to  archive folder 
                            d.MoveTo(objPath.SFTP_BAT_SourceFilePath_BK + d.Name);
                            //iResult = 1;


                            fileStream.Flush();
                            fileStream.Close();
                            fileStream.Dispose();

                        }
                    }
                    sftp.Disconnect();
                }
                // if Previous failed files presents then process it first
                if (new DirectoryInfo(objPath.CardAutoSourcePath).GetFiles("*.txt").Where(file => file.Name.EndsWith(".txt") && file.Name.Contains(objPath.FileName)).Select(file => file.Name).ToList().Count() > 0)
                {
                    ClsReturnStatusBO ObjCardGen = new ClsReturnStatusBO();
                    ObjCardGen = FunCardGeneration(objPath);

                    //on Success
                    if (ObjCardGen.OutPutCode == "00")
                    {
                        iResult = 1;
                    }
                    else
                    {
                        iResult = 0;
                        return iResult;
                    }
                }

                // Process Upgrade/Renew files
                if (new DirectoryInfo(objPath.BAT_SourceFilePath).GetFiles("*.zip").Where(file => file.Name.EndsWith(".zip") && file.Name.Contains(objPath.FileName)).Select(file => file.Name).ToList().Count() > 0)
                {

                    List<string> lstZipFiles = new DirectoryInfo(objPath.BAT_SourceFilePath).GetFiles("*.zip").Where(file => file.Name.EndsWith(".zip") && file.Name.Contains(objPath.FileName)).Select(file => file.FullName).ToList();
                    foreach (string FileGroup in lstZipFiles)
                    {
                        iResult = 0;
                        //unZip each fileGroup

                        string zipPath = FileGroup;
                        string extractPath = objPath.BAT_SourceFilePath;

                        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                                {
                                    entry.ExtractToFile(Path.Combine(extractPath, entry.Name));
                                }
                            }
                        }
                        //move Zip file to back up
                        File.Move(FileGroup, objPath.BAT_SourceFilePath_BK + Path.GetFileName(FileGroup));

                        //Fetch by name from source path and move to cardAuto i/p and then SFTP folder
                        FunFileUploadSFTP(objPath);

                        //final Card genration 
                        if (new DirectoryInfo(objPath.CardAutoSourcePath).GetFiles("*.txt").Where(file => file.Name.EndsWith(".txt") && file.Name.Contains(objPath.FileName)).Select(file => file.Name).ToList().Count() > 0)
                        {
                            ClsReturnStatusBO ObjCardGen = new ClsReturnStatusBO();
                            ObjCardGen = FunCardGeneration(objPath);

                            //on Success
                            if (ObjCardGen.OutPutCode == "00")
                            {
                                iResult = 1;
                            }
                            else
                            {
                                iResult = 0;
                                break;
                            }
                        }
                        else
                        {
                            iResult = 1;
                        }
                    }
                }
                else
                {
                    //FunInsertIntoErrorLog("FunUpgradeRenewCardProcess", "No File Found", "SFTP_SourcePath=" + objPath.SFTP_BAT_SourceFilePath + " ,SFTP_SourceBK =" + objPath.SFTP_BAT_SourceFilePath_BK + "BAT_Source=" + objPath.BAT_SourceFilePath + "BAT_SourceBK=" + objPath.BAT_SourceFilePath_BK, objPath.IssuerNo, string.Empty);
                    //FunInsertIntoLogFile(ErrorLogFilePath, null, "FunUpgradeRenewCardProcess|Para: SFTP_SourcePath =" + objPath.SFTP_BAT_SourceFilePath + " ,SFTP_SourceBK =" + objPath.SFTP_BAT_SourceFilePath_BK + "BAT_Source=" + objPath.BAT_SourceFilePath + "BAT_SourceBK=" + objPath.BAT_SourceFilePath_BK);
                    iResult = 1;
                }

            }
            catch (Exception ex)
            {

                FunInsertIntoErrorLog("FunUpgradeRenewCardProcess", ex.Message, "SFTP_SourcePath=" + objPath.SFTP_BAT_SourceFilePath + " ,SFTP_SourceBK =" + objPath.SFTP_BAT_SourceFilePath_BK + "BAT_Source=" + objPath.BAT_SourceFilePath + "BAT_SourceBK=" + objPath.BAT_SourceFilePath_BK, objPath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunUpgradeRenewCardProcess|Para: SFTP_SourcePath =" + objPath.SFTP_BAT_SourceFilePath + " ,SFTP_SourceBK =" + objPath.SFTP_BAT_SourceFilePath_BK + "BAT_Source=" + objPath.BAT_SourceFilePath + "BAT_SourceBK=" + objPath.BAT_SourceFilePath_BK);
            }
            //ObjResult = FunCardGeneration(objPath);
            return iResult;
        }
        //-----------------------------------------------------
        /// <summary>
        ///Fetch by name from source path and move to cardAuto i/p and then SFTP folder 
        /// </summary>
        /// <param name="ObjPath"></param>
        private void FunFileUploadSFTP(ClsPathsBO ObjPath)
        {
            try
            {
                FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunFileUploadSFTP|Uploading on SFTP FunFileUploadSFTP Initiated..");
                if (!string.IsNullOrEmpty(ObjPath.BAT_SourceFilePath))
                {
                    if (Directory.Exists(ObjPath.BAT_SourceFilePath))
                    {
                        DirectoryInfo di = new DirectoryInfo(ObjPath.BAT_SourceFilePath);
                        string[] sCompressedFile = null;
                        // Get only the txt files the ones having 
                        // "FileName" appended to them.
                        List<string> lstCardAccountsFiles = di.GetFiles("*.txt")
                                                  .Where(file => file.Name.EndsWith(".txt") &&
                                                                 file.Name.Contains(ObjPath.FileName))
                                                  .Select(file => file.FullName).ToList();
                        if (lstCardAccountsFiles.Count() > 0)
                        {
                            sCompressedFile = new string[lstCardAccountsFiles.Count()];
                            int i = 0;
                            foreach (string filePath in lstCardAccountsFiles)
                            {
                                string FullFileName = (filePath.Split('\\')[filePath.Split('\\').Length - 1]);
                                string strFileName = (FullFileName).Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                // move to cardauto i/p with name as required for card processing
                                File.Copy(filePath, ObjPath.CardAutoSourcePath + FullFileName);
                                //then move to BackUp
                                File.Move(filePath, ObjPath.BAT_SourceFilePath_BK + FullFileName);
                                ////add to  array for upload later to SFTP
                                sCompressedFile[i] = ObjPath.CardAutoSourcePath + FullFileName;
                                i++;
                            }
                        }
                        //upload card accounts files on SFTP  when newly generated     
                        if (sCompressedFile != null && sCompressedFile.Count() > 0)
                        {
                            FunInsertIntoErrorLog("RBLCardAuto.cs|SFTPFileUpload", "File Name found with Name :" + ObjPath.FileName + " Uploading on SFTP SFTPFileUpload Initiated..", "", ObjPath.IssuerNo, "");
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|SFTPFileUpload|File Name found with Name :" + ObjPath.FileName + " Uploading on SFTP SFTPFileUpload Initiated..");

                            SFTPFileUpload(ObjPath);
                        }
                        else
                        {
                            FunInsertIntoErrorLog("RBLCardAuto.cs|SFTPFileUpload", "File Name not found with Name :" + ObjPath.FileName, "", ObjPath.IssuerNo, "");
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|SFTPFileUpload|File Name not found with Name :" + ObjPath.FileName);
                        }
                    }
                }
                else
                {
                    FunInsertIntoErrorLog("FunFileUploadSFTP", "", "CardFilesPath=" + ObjPath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + ObjPath.CardAutoOutputPath_SFTP, ObjPath.IssuerNo, string.Empty);
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "FunFileUploadSFTP|Cardfiles Upload|Para:CardFilesPath=" + ObjPath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + ObjPath.CardAutoOutputPath_SFTP);
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("FunFileUploadSFTP", ex.Message, "CardFilesPath=" + ObjPath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + ObjPath.CardAutoOutputPath_SFTP, ObjPath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunFileUploadSFTP|Cardfiles Upload|Para:CardFilesPath=" + ObjPath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + ObjPath.CardAutoOutputPath_SFTP);
            }
        }
        //---------------------------------------------------------
        /// <summary>
        /// final card gen process call through SP
        /// </summary>
        /// <param name="ObjPath"></param>
        /// <returns></returns>
        private ClsReturnStatusBO FunCardGeneration(ClsPathsBO ObjPath)
        {
            ClsReturnStatusBO ObjOutResult = new ClsReturnStatusBO();
            ObjOutResult.OutPutCode = string.Empty;
            try
            {
                ClsReturnStatusBO ObjCheckFiles = new ClsReturnStatusBO();
                //check  files exists on SFTP and pass cardProgram
                FunInsertIntoErrorLog("RBLCardAuto.cs|FunCardGeneration", "CheckCardFilesOnSftp Process is Initiated To fetch Card Program..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|CheckCardFilesOnSftp Process is Initiated..Process ID :" + ObjPath.ProcessID);
                ObjCheckFiles = CheckCardFilesOnSftp(ObjPath);
                FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|CheckCardFilesOnSftp Process End..Process ID :" + ObjPath.ProcessID + " CardProgram:" + ObjCheckFiles.OutPutCode + " & Code:" + ObjCheckFiles.Code);

                if (ObjCheckFiles.Code == 1 && !string.IsNullOrEmpty(ObjCheckFiles.OutPutCode))
                {
                    // final  CardGen Step
                    FunInsertIntoErrorLog("RBLCardAuto.cs|CardFilesProcess", "CardFilesProcess Process is Initiated(Calling Std2/Std4)..Process ID :" + ObjPath.ProcessID, "", ObjPath.IssuerNo, "");
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|CardFilesProcess Process is Initiated(Calling Std2/Std4)..Process ID :" + ObjPath.ProcessID);
                    ObjOutResult = CardFilesProcess(Convert.ToInt16(ObjPath.IssuerNo), ObjPath.ProcessID.ToString(), ObjCheckFiles.OutPutCode);
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|CardFilesProcess Process is End(Calling Std2/Std4), Response Code:" + ObjOutResult.OutPutCode + " & Description:" + ObjOutResult.Description);

                    //On success
                    if (ObjOutResult.OutPutCode == "00")
                    {
                        //Check files exists at source path
                        if (Directory.GetFiles(ObjPath.CardAutoSourcePath, "*.txt").Length > 0)
                        {
                            if (!(System.IO.Directory.Exists(ObjPath.CardAutoBackUpPath)))
                            {
                                try
                                {
                                    Directory.CreateDirectory(ObjPath.CardAutoBackUpPath);
                                }
                                catch (Exception ex)
                                {
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunCardGeneration");
                                }
                            }

                            //************** Delete input files from sftp  ******************
                            
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|DeleteFilefrmSFTP Process is Initiated..Process ID :" + ObjPath.ProcessID);
                            DeleteFilefrmSFTP(ObjPath.CardAutoOutputPath_SFTP, ObjPath.AGS_SFTPServer, ObjPath.AGS_SFTP_Port, ObjPath.AGS_SFTP_User, ObjPath.AGS_SFTP_Pwd, ObjPath.IsSaveError, ObjPath.IssuerNo, ObjPath.SFTP_OutputFile_BK_Path);

                            //move source card files to success folder
                            foreach (string filePath in Directory.GetFiles(ObjPath.CardAutoSourcePath, "*.txt"))
                            {
                                try
                                {
                                    File.Move(filePath, ObjPath.CardAutoBackUpPath + filePath.Split('\\')[filePath.Split('\\').Length - 1]);
                                }
                                catch (Exception ex)
                                {

                                    FunInsertIntoErrorLog("FunCardGeneration|move cardfiles to success folder", ex.Message, "", ObjPath.ToString(), string.Empty);

                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunCardGeneration|move cardfiles to success folder");
                                }
                            }
                            FunInsertIntoErrorLog("FunCardGeneration", "Process Success", "ProcessID=" + ObjPath.ProcessID, ObjPath.IssuerNo, "");
                            FunInsertIntoLogFile(ErrorLogFilePath, null, "FunCardGeneration|Process Success|ProcessID = " + ObjPath.ProcessID);
                        }
                    }
                    //on Fail
                    else
                    {
                        FunInsertIntoErrorLog("FunCardGeneration", "Process Fail", "ProcessID=" + ObjPath.ProcessID, ObjPath.IssuerNo, "");
                        FunInsertIntoLogFile(ErrorLogFilePath, null, "FunCardGeneration|Process Fail|ProcessID = " + ObjPath.ProcessID);
                    }
                }
            }
            catch (Exception Ex)
            {
                FunInsertIntoErrorLog("FunCardGeneration", Ex.Message, "", ObjPath.IssuerNo, "");
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "FunCardGeneration|Process Success");
            }
            return ObjOutResult;
        }
        //----------------------------------------------------------
        /// <summary>
        /// Check Card file on sftp get cardprogram and pass for final Card Gen
        /// </summary>
        /// <param name="ObjPath"></param>
        /// <returns>ClsReturnStatusBO Class</returns>
        public ClsReturnStatusBO CheckCardFilesOnSftp(ClsPathsBO ObjPath)
        {
            ClsReturnStatusBO objStatus = new ClsReturnStatusBO();
            int iResult = 0;
            string strCardPrograms = string.Empty;
            try
            {
                using (var sftp = new SftpClient(ObjPath.AGS_SFTPServer, Convert.ToInt32(ObjPath.AGS_SFTP_Port), ObjPath.AGS_SFTP_User, ObjPath.AGS_SFTP_Pwd))
                {
                    sftp.Connect();
                    string remoteDirectory = ObjPath.CardAutoOutputPath_SFTP;
                    var files = sftp.ListDirectory(remoteDirectory);

                    int count = (from c in files where c.Length > 0 && c.Name.Contains(".txt") select c).ToList().Count;

                    //Check accounts cards file present on sftp / in case of upgrade check  card file  present on SFTP
                    //if (((count >= 4) && (ObjPath.ProcessID != 5))||((count >= 3) && (ObjPath.ProcessID != 4)) || ((ObjPath.ProcessID == 5) && (count == 1)))
                    //{
                    //for getting Card programs
                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains("Cards.txt")) select i)
                    {
                        using (Stream fileStream = File.OpenWrite(Path.Combine(ObjPath.ZipCardFilesPath, d.Name)))
                        {
                            sftp.DownloadFile(d.FullName, fileStream);
                            sftp.Disconnect();
                            fileStream.Close();
                            fileStream.Dispose();
                            //read cards file for getting cardprogram
                            var fileLines = File.ReadAllLines(ObjPath.ZipCardFilesPath + "Cards.txt");
                            var cardprogramArr = from csvline in fileLines
                                                 let data = csvline.Split(',')
                                                 select csvline.Split(',')[2];
                            strCardPrograms = string.Join(",", cardprogramArr.Where(c => c != null).ToList().Distinct());
                            File.Delete(ObjPath.ZipCardFilesPath + "Cards.txt");
                        }

                    }
                    if (!string.IsNullOrEmpty(strCardPrograms))
                    {
                        iResult = 1;
                    }
                    //}
                }

            }
            catch (Exception Ex)
            {
                FunInsertIntoErrorLog("CheckCardFilesOnSftp", Ex.Message, "SFTP_FilePath=" + ObjPath.CardAutoOutputPath_SFTP + " , Temp file path=" + ObjPath.ZipCardFilesPath, ObjPath.IssuerNo, string.Empty);

                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "CheckCardFilesOnSftp|Para:SFTP_FilePath = " + ObjPath.CardAutoOutputPath_SFTP);
            }
            objStatus.Code = iResult;
            objStatus.OutPutCode = strCardPrograms;
            return objStatus;
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Final CardAutomation SP call
        /// </summary>
        /// <param name="IssurNo"> Bank IssuerNo</param>
        /// <param name="ProcessID">Card Gen Process ID from CardGenProcess master</param>
        /// <param name="CardPrograms"></param>
        /// <returns>ClsReturnStatusBO Class</returns>
        public ClsReturnStatusBO CardFilesProcess(int IssurNo, string ProcessID, string CardPrograms)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.OutPutCode = string.Empty;
            ObjResult.Description = string.Empty;
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();

                FunInsertIntoErrorLog("RBLCardAuto.cs|CardFilesProcess", "DownloadFiles Process is Initiated..Process ID ", "", Convert.ToString(IssurNo), "");
                //FunInsertIntoLogFile(ErrorLogFilePath, null, "DownloadFiles Process is Initiated..Process ID :" + ObjPath.ProcessID);

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_BankCardAutomation", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntIssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssurNo);
                    ObjCmd.AddParameterWithValue("@StrCardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, CardPrograms);
                    ObjCmd.AddParameterWithValue("@StrProcessID", SqlDbType.VarChar, 0, ParameterDirection.Input, ProcessID);
                    ObjCmd.AddParameter("@StrStatusCode", SqlDbType.VarChar, 500, ParameterDirection.Output);
                    ObjCmd.AddParameter("@StrStatusDesc", SqlDbType.VarChar, 800, ParameterDirection.Output);
                    ObjCmd.ExecuteNonQuery();
                    ObjResult.Description = ObjCmd.Parameters["@StrStatusDesc"].Value.ToString();
                    ObjResult.OutPutCode = (ObjCmd.Parameters["@StrStatusCode"].Value.ToString());
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjResult.OutPutCode = "1";
                ObjResult.Description = ex.Message;
                FunInsertIntoErrorLog("CardFilesProcess", ex.Message, "CardPrograms=" + CardPrograms, IssurNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "CardFilesProcess|Para: IssuerNo :" + IssurNo + " ,CardPrograms: " + CardPrograms);
            }
            return ObjResult;
        }
        //------------------------------------------------------------------------------
        /// <summary>
        /// Delete files on SFTP and move app server files to BackUp 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="SFTP_Server"></param>
        /// <param name="SFTP_Port"></param>
        /// <param name="SFTP_User"></param>
        /// <param name="SFTP_PWD"></param>
        /// <param name="IsSaveError"></param>
        /// <param name="IssuerNo"></param>
        public void DeleteFilefrmSFTP(string FilePath, string SFTP_Server, string SFTP_Port, string SFTP_User, string SFTP_PWD, string IsSaveError, string IssuerNo, string AccountLinkageFilePath = "")
        {
            using (var sftp = new SftpClient(SFTP_Server, Convert.ToInt32(SFTP_Port), SFTP_User, SFTP_PWD))
            {
                try
                {
                    sftp.Connect();
                    string InputFilePath = FilePath;

                    var FilesArr = sftp.ListDirectory(InputFilePath);



                    foreach (var d in from i in FilesArr.AsEnumerable() where (i.Length > 0 && i.Name != "." && i.Name.Contains(".txt")) select i)
                    {
                        try
                        {
                            //if AccountLinkage File  Present Then Move it to output folder 
                            if (d.Name.Contains("AccountLinkage.txt"))
                            {
                                string NewFileName = d.Name.Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0] + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";
                                d.MoveTo(AccountLinkageFilePath + NewFileName);
                            }
                            else
                            {

                                sftp.DeleteFile(d.FullName);
                            }
                        }
                        catch (Exception Ex)
                        {
                            if (IsSaveError == "1")
                            {
                                FunInsertIntoErrorLog("DeleteFilefrmSFTP|Delete SFTP file", Ex.Message, "SFTP_File=" + d.FullName, IssuerNo, string.Empty);
                            }
                            FunInsertIntoLogFile(ErrorLogFilePath, Ex, "DeleteFilefrmSFTP|Delete SFTP file");
                        }
                    }


                }
                catch (Exception Ex)
                {

                    FunInsertIntoErrorLog("DeleteFilefrmSFTP", Ex.Message, "SFTP_FilePath=" + FilePath, IssuerNo, string.Empty);
                    FunInsertIntoLogFile(ErrorLogFilePath, Ex, "DeleteFilefrmSFTP|Para:SFTP_FilePath = " + FilePath);
                }
            }
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// cardAccounts files Upload on SFTP
        /// </summary>
        /// <param name="objpath"></param>
        public void SFTPFileUpload(ClsPathsBO objpath)
        {
            try
            {
                var sourcePath = objpath.AGS_SFTPServer;

                using (var sftp = new SftpClient(sourcePath, Convert.ToInt32(objpath.AGS_SFTP_Port), objpath.AGS_SFTP_User, objpath.AGS_SFTP_Pwd))   //".\\dipak.gole","Di$@12345"
                {
                    try
                    {
                        string InputFilePath = objpath.CardAutoSourcePath;

                        string[] FilesArr = Directory.GetFiles(InputFilePath, "*.txt");

                        if (FilesArr.Count() > 0)
                        {
                            sftp.Connect();
                            foreach (string file in FilesArr)
                            {
                                string[] path = file.Split('\\');
                                try
                                {
                                    using (var fileStream = new FileStream(file, FileMode.Open))
                                    {
                                        sftp.UploadFile(fileStream, objpath.CardAutoOutputPath_SFTP + (path[path.Length - 1].Contains('_') ? ((path[path.Length - 1].Split('_'))[0] + ".txt") : (path[path.Length - 1])), true);

                                        fileStream.Flush();
                                        fileStream.Close();
                                        fileStream.Dispose();
                                    }
                                }
                                catch (Exception ex)
                                {

                                    FunInsertIntoErrorLog("SFTPFileUpload|Cardfiles Upload", ex.Message, "CardFilesPath=" + file + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Cardfiles Upload|Para:CardFilesPath=" + file + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
                                }

                            }
                            sftp.Disconnect();
                        }

                    }
                    catch (Exception ex)
                    {
                        FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Cardfiles Upload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
                    }
                }

            }
            catch (Exception ex)
            {

                FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
            }
        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// Get CIF file from SFTP
        /// </summary>
        /// <param name="objPath"></param>
        public void FunGetSFTP_CIF_Files(ClsPathsBO objPath)
        {
            //fetch files from Sftp
            var SFTP_CIF_FileSourcePath = objPath.SFTP_CIF_Source_Path;
            var CIF_FileInputPath = objPath.CardCIF_Input_Path;
            var CIF_FileBackUp = objPath.CardCIF_Backup;
            var SFTP_CIFBackUp = objPath.SFTP_CIF_BackUp_Path;
            string rarFilePath = objPath.ZipCardFilesPath, BankName = objPath.BankName;
            string _fileExtension = string.Empty;

            try
            {
                //Get CIF files from SFTP Path

                var sourcePath = objPath.B_SFTPServer;

                //Below code is for SSH encryption start
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(objPath.B_SFTP_User, objPath.B_SFTP_Pwd));

                if (!string.IsNullOrEmpty(objPath.SFTPKeyPath))
                {
                    var keyFile = new PrivateKeyFile(objPath.SFTPKeyPath, objPath.SFTPKeypassphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(objPath.B_SFTP_User, keyFiles));
                }

                var con = new ConnectionInfo(sourcePath, Convert.ToInt32(objPath.B_SFTP_Port), objPath.B_SFTP_User, methods.ToArray());
                //SSH ENcryption End

                //using (var sftp = new SftpClient(sourcePath, Convert.ToInt32(objPath.B_SFTP_Port), objPath.B_SFTP_User, objPath.B_SFTP_Pwd)) 
                using (var sftp = new SftpClient(con))
                {
                    sftp.Connect();
                    FunInsertIntoErrorLog("RBLCardAuto.cs|FunGetSFTP_CIF_Files", "SFTP Server Connected..Getting file from SFTP server..Process ID :" + objPath.ProcessID, "SFTP File Path: " + objPath.SFTP_CIF_Source_Path, objPath.IssuerNo, "");


                    string remoteDirectory = objPath.SFTP_CIF_Source_Path;
                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains(objPath.FileExtention)) select i)
                    {

                        using (Stream fileStream = File.OpenWrite(Path.Combine(CIF_FileInputPath, d.Name)))
                        {
                            sftp.DownloadFile(d.FullName, fileStream);
                            //check  folder exists
                            if (!sftp.Exists(SFTP_CIFBackUp))
                            {
                                sftp.CreateDirectory(SFTP_CIFBackUp);
                            }
                            try
                            {
                                //move to  archive folder 
                                d.MoveTo(SFTP_CIFBackUp + d.Name);
                            }
                            catch (Exception ex)
                            {
                                FunInsertIntoErrorLog("FunGetSFTP_CIF_Files|move to SFTP BackUp", ex.Message, "SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp, objPath.IssuerNo, string.Empty);
                                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetSFTP_CIF_Files|move to CIF SFTP BackUp|Para: SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp);
                            }

                            fileStream.Flush();
                            fileStream.Close();
                            fileStream.Dispose();
                        }

                        //If File is PGP File then convert it into txt
                        if (objPath.FileExtention == ".pgp")
                        {
                            bool bResult = PGP_Decrypt(objPath.PGP_KeyName, Path.Combine(CIF_FileInputPath, d.Name), Path.Combine(CIF_FileInputPath, d.Name).Split('.')[0] + ".txt", objPath.SecKey_Path, Convert.ToString(objPath.IssuerNo), objPath.PGP_PWD);
                            if (bResult == true)
                            {
                                if (File.Exists(Path.Combine(CIF_FileInputPath, d.Name)))
                                {
                                    File.Delete(Path.Combine(CIF_FileInputPath, d.Name));
                                }
                            }
                        }

                    }
                    sftp.Disconnect();
                }

            }
            catch (Exception ex)
            {

                FunInsertIntoErrorLog("FunGetSFTP_CIF_Files", ex.Message, "SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp, objPath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetSFTP_CIF_Files|Para: SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp);
            }
        }
        //----------------------------------------------------------------------------
        /// <summary>
        /// CIF with Failed Validation upload on SFTP in FailedCIF folder
        /// </summary>
        /// <param name="objpath"></param>
        public void SFTP_FailedCIFUpload(ClsPathsBO objpath)
        {
            try
            {
                //Below code is for SSH encryption start
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(objpath.B_SFTP_User, objpath.B_SFTP_Pwd));

                if (!string.IsNullOrEmpty(objpath.SFTPKeyPath))
                {
                    var keyFile = new PrivateKeyFile(objpath.SFTPKeyPath, objpath.SFTPKeypassphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(objpath.B_SFTP_User, keyFiles));
                }

                var con = new ConnectionInfo(objpath.B_SFTPServer, Convert.ToInt32(objpath.B_SFTP_Port), objpath.B_SFTP_User, methods.ToArray());
                //SSH ENcryption End


                //using (var sftp = new SftpClient(objpath.B_SFTPServer, Convert.ToInt32(objpath.B_SFTP_Port), objpath.B_SFTP_User, objpath.B_SFTP_Pwd))
                using (var sftp = new SftpClient(con))
                {
                    try
                    {
                        string InputFilePath = objpath.FailedCIFPath;
                        string[] FilesArr = Directory.GetFiles(InputFilePath, "*.txt");

                        if (FilesArr.Count() > 0)
                        {
                            sftp.Connect();
                            foreach (string file in FilesArr)
                            {
                                //FunInsertTextLog("FunCreatePRE PGP_Encrypt Start", Convert.ToInt32(objPath.IssuerNo));
                                string FailedFileName = file;
                                bool bResult;

                                if (objpath.FileExtention == ".pgp")
                                {
                                    //Convert Failed CIF into pgp to share on SFTP
                                    bResult = PGP_Encrypt(objpath.PGP_KeyName, FailedFileName, FailedFileName.Split('.')[0] + ".pgp", objpath.PubKey_Path, objpath.IssuerNo);

                                    //If bResult is true means file created successfully on path, then delete existing txt file and if it is false it means there is an error in ecnryption process
                                    if (bResult)
                                    {
                                        File.Delete(FailedFileName);
                                        FailedFileName = FailedFileName.Split('.')[0] + ".pgp";
                                    }
                                    else
                                    {
                                        //break foreach                                        
                                        FunInsertIntoLogFile(ErrorLogFilePath, null, "SFTP_FailedCIFUpload|PGP_Encrypt|PGP Encryption Failed PGP_KeyName:" + objpath.PGP_KeyName + " FailedFileName:" + FailedFileName + ", PubKey_Path:" + objpath.PubKey_Path + ", IssuerNo:" + objpath.IssuerNo);
                                        FunInsertIntoErrorLog("SFTP_FailedCIFUpload", "SFTP_FailedCIFUpload|PGP_Encrypt|PGP Encryption Failed PGP_KeyName:" + objpath.PGP_KeyName + " FailedFileName:" + FailedFileName + ", PubKey_Path:" + objpath.PubKey_Path + ", IssuerNo:" + objpath.IssuerNo, "Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath, objpath.IssuerNo, string.Empty);
                                        continue;
                                    }
                                }

                                //FunInsertTextLog("FunCreatePRE PGP_Encrypt End", Convert.ToInt32(objPath.IssuerNo));

                                string[] path = FailedFileName.Split('\\');
                                try
                                {
                                    using (var fileStream = new FileStream(FailedFileName, FileMode.Open))
                                    {
                                        FunInsertIntoLogFile(ErrorLogFilePath, null, "SFTP_FailedCIFUpload|PGP_Encrypt|Uploading failed file on SFTP Path:" + objpath.B_SFTP_FailedCIFPath + path[path.Length - 1]);
                                        FunInsertIntoErrorLog("SFTP_FailedCIFUpload", "SFTP_FailedCIFUpload|PGP_Encrypt|Uploading failed file on SFTP Path:" + objpath.B_SFTP_FailedCIFPath + path[path.Length - 1], "Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath, objpath.IssuerNo, string.Empty);

                                        sftp.UploadFile(fileStream, objpath.B_SFTP_FailedCIFPath + path[path.Length - 1], true);
                                        fileStream.Flush();
                                        fileStream.Close();
                                        fileStream.Dispose();
                                        File.Delete(FailedFileName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    FunInsertIntoErrorLog("SFTP_FailedCIFUpload", ex.Message, "Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath, objpath.IssuerNo, string.Empty);
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTP_FailedCIFUpload|Para:Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath);
                                }

                            }
                            sftp.Disconnect();
                        }

                    }
                    catch (Exception ex)
                    {
                        if (objpath.IsSaveError == "1")
                            FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);

                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Cardfiles Upload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
                    }
                }

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
            }
        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// Read CIF File Data and save in DB
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="issuerNo"></param>
        /// <param name="FailedCIFPath"></param>
        /// <returns></returns>
        public ClsReturnStatusBO ExtractFrmCIF_File(string FilePath, int issuerNo, string FailedCIFPath)
        {
            ClsReturnStatusBO ObjStatus = new ClsReturnStatusBO();
            ObjStatus.Code = 1;
            try
            {
                DataTable DtFileData = new DataTable();
                DtFileData = GetTextFileIntoDataTable(FilePath, string.Empty, "|", issuerNo.ToString());

                //Save for card production
                string fileName = FilePath.Split('\\')[FilePath.Split('\\').Length - 1];

                FunInsertIntoErrorLog("RBLCardAuto.cs|FunProcessCIFFileData", "FunProcessCIFFileData Process Initiated..", Convert.ToString(FilePath), issuerNo.ToString(), "");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunProcessCIFFileData|Process CIF File Data To DataBase Process Initiated..");
                ObjStatus = FunProcessCIFFileData(issuerNo, DtFileData, fileName, FailedCIFPath);

            }
            catch (Exception Ex)
            {
                FunInsertIntoErrorLog("ExtractFrmCIF_File", Ex.Message, "FilePath= " + FilePath + ",issuerNo= " + issuerNo, issuerNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "ExtractFrmCIF_File|Para : FilePath = " + FilePath);
            }
            return ObjStatus;

        }
        //----------------------------------------------------------------
        /// <summary>
        /// Save CIF File Data in DB
        /// </summary>
        /// <param name="IssurNo"></param>
        /// <param name="DtBulkData"></param>
        /// <param name="CIF_fileName"></param>
        /// <param name="FailedCIFPath"></param>
        /// <returns></returns>
        public ClsReturnStatusBO FunProcessCIFFileData(int IssurNo, DataTable DtBulkData, string CIF_fileName, string FailedCIFPath)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = string.Empty;
            DataSet ObjDTOutPut = new DataSet();
            SqlConnection ObjConn = null;

            try
            {

                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BATCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CASaveCardProdData", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssurNo);
                    ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, CIF_fileName);
                    ObjCmd.AddParameterWithValue("@CustBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);

                    ObjDTOutPut = ObjCmd.ExecuteDataSet();
                    if (ObjDTOutPut.Tables[0].Rows.Count > 0)
                    {
                        ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Tables[0].Rows[0]["Code"]);
                        ObjResult.Description = ObjDTOutPut.Tables[0].Rows[0]["OutputDescription"].ToString();
                        ObjResult.OutPutCode = ObjDTOutPut.Tables[0].Rows[0]["OutPutCode"].ToString();
                        //on successful cif process Failed customer saving
                        FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunProcessCIFFileData|SP_CASaveCardProdData Returns ObjResult.Code: " + ObjResult.Code + " ,ObjResult.Description:" + ObjResult.Description + " , ObjResult.OutPutCode:" + ObjResult.OutPutCode + " & Failure DataTable Count: " + ObjDTOutPut.Tables[1].Rows.Count);
                        if (ObjResult.Code == 0)
                        {
                            if (ObjDTOutPut.Tables[1].Rows.Count > 0)
                            {

                                try
                                {
                                    string FileCreationPath = getCurrentDirectory(FailedCIFPath);
                                    string strLocFile = CreateDownloadCSVFile(ObjDTOutPut.Tables[1].ToSelectedTableColumns("Result"), FileCreationPath, false, "", "", "");
                                    System.IO.File.Move(FileCreationPath + "\\" + strLocFile, FileCreationPath + "\\" + FunGetNewFileName(CIF_fileName));
                                    FunInsertIntoLogFile(ErrorLogFilePath, null, "RBLCardAuto.cs|FunProcessCIFFileData|SP_CASaveCardProdData store procedure returns failuer Data Path: " + FileCreationPath + "\\" + FunGetNewFileName(CIF_fileName));
                                }
                                catch (Exception ex)
                                {
                                    FunInsertIntoErrorLog("FunProcessCIFFileData|Create failed CIF file", ex.Message, "CIF_fileName=" + CIF_fileName, IssurNo.ToString(), string.Empty);
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunProcessCIFFileData|Create failed CIF file|Para:CIF_fileName = " + CIF_fileName);
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

                FunInsertIntoErrorLog("FunProcessCIFFileData", ex.Message, "CIF_File" + CIF_fileName, IssurNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunProcessCIFFileData|Para : CIF_File = " + CIF_fileName);
            }
            return ObjResult;

        }
        //-------------------------------------------------------------------
        /// <summary>
        /// Download Card Accounts Files 
        /// </summary>
        /// <param name="IssuerNo"></param>
        /// <param name="CardFilesInputPath"></param>
        /// <param name="CardFilesSourcePath"></param>
        /// <param name="CardFileFailedPath"></param>
        /// <param name="IsSaveError"></param>
        /// <returns></returns>
        internal string[] DownloadFiles(int IssuerNo, string CardFilesInputPath, string CardFilesSourcePath, string CardFileFailedPath, string IsSaveError)
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                ObjConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BATCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CAGetCardProdAccountsDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Redownload", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, string.Empty);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    if (IsSaveError == "1")
                    {
                        FunInsertIntoErrorLog("DownloadFiles", null, "countCardFilesSourcePath=" + CardFilesSourcePath + ",CardFileFailedPath=" + ",filename=" + ObjDsOutPut.Tables[0].Rows[0]["FileNames"] + ",Table count=" + ObjDsOutPut.Tables.Count, IssuerNo.ToString(), string.Empty);
                    }
                    //var BankList = ((from r in ObjDsOutPut.Tables[1].AsEnumerable()
                    //                 select r["Bank"].ToString()).Distinct().ToList());

                    string[] filenames = Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["FileNames"]).Split(',');
                    string[] sCompressedFile = new string[filenames.Count()];
                    if (filenames.Length + 1 == ObjDsOutPut.Tables.Count)
                    {

                        List<string> physicalfiles = new List<string>();
                        string filename;
                        string BankName = string.Empty;
                        ObjDsOutPut.Tables.RemoveAt(0);
                        String sCreationSuffix = DateTime.Now.ToString("ddMMyyhhmmss").Replace(" ", "_") + "_" + rnd.Next();
                        int i = 0;

                        foreach (DataTable dtOutput in ObjDsOutPut.Tables)
                        {
                            if (dtOutput.Rows.Count > 0)
                            {
                                filename = filenames[i].Trim();
                                if (!string.IsNullOrEmpty(filename))
                                {
                                    if (dtOutput.Rows.Count > 0)
                                    {

                                        try
                                        {
                                            string strLocFile = CreateDownloadCSVFile(dtOutput.ToSelectedTableColumns("Result"), getCurrentDirectory(CardFilesInputPath), false, "", "", "");
                                            System.IO.File.Move(getCurrentDirectory(CardFilesInputPath) + "\\" + strLocFile, CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt");
                                            physicalfiles.Add(CardFilesSourcePath + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                            //blnfileexists = true;
                                            sCompressedFile[i] = CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt";
                                            i++;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (IsSaveError == "1")
                                            {
                                                FunInsertIntoErrorLog("DownloadFiles|Create CardFiles", ex.Message, "CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                                            }
                                            FunInsertIntoLogFile(ErrorLogFilePath, ex, "DownloadFiles|Create CardFiles|Para:CardFilesSourcePath = " + CardFilesSourcePath);
                                        }
                                    }
                                }

                            }

                        }
                        return sCompressedFile.Where(c => c != null).ToArray();

                    }
                    else
                    {
                        return sCompressedFile;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ObjConn.Close();
                if (IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("DownloadFiles", ex.Message, "IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                }
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "DownloadFiles|Para:IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath);
                return null;
            }
        }

        /// <summary>
        /// Download CardReissue File
        /// </summary>
        /// <param name="IssuerNo"></param>
        /// <param name="CardFilesInputPath"></param>
        /// <param name="CardFilesSourcePath"></param>
        /// <param name="CardFileFailedPath"></param>
        /// <param name="IsSaveError"></param>
        /// <returns></returns>
        internal string[] DownloadReissueFile(int IssuerNo, string CardFilesInputPath, string CardFilesSourcePath, string CardFileFailedPath, string IsSaveError)
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                ObjConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BATCONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CACardReissueProcess", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Redownload", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, string.Empty);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    //if (IsSaveError == "1")
                    //{
                    //    FunInsertIntoErrorLog("DownloadFiles", null, "countCardFilesSourcePath=" + CardFilesSourcePath + ",CardFileFailedPath=" + ",filename=" + ObjDsOutPut.Tables[0].Rows[0]["FileNames"] + ",Table count=" + ObjDsOutPut.Tables.Count, IssuerNo.ToString(), string.Empty);
                    //}
                    //var BankList = ((from r in ObjDsOutPut.Tables[1].AsEnumerable()
                    //                 select r["Bank"].ToString()).Distinct().ToList());

                    string[] filenames = Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["FileNames"]).Split(',');
                    string[] sCompressedFile = new string[filenames.Count()];
                    if (filenames.Length + 1 == ObjDsOutPut.Tables.Count)
                    {

                        List<string> physicalfiles = new List<string>();
                        string filename;
                        string BankName = string.Empty;
                        ObjDsOutPut.Tables.RemoveAt(0);
                        String sCreationSuffix = DateTime.Now.ToString("ddMMyyhhmmss").Replace(" ", "_") + "_" + rnd.Next();
                        int i = 0;

                        foreach (DataTable dtOutput in ObjDsOutPut.Tables)
                        {
                            if (dtOutput.Rows.Count > 0)
                            {
                                filename = filenames[i].Trim();
                                if (!string.IsNullOrEmpty(filename))
                                {
                                    if (dtOutput.Rows.Count > 0)
                                    {

                                        try
                                        {
                                            string strLocFile = CreateDownloadCSVFile(dtOutput.ToSelectedTableColumns("Result"), getCurrentDirectory(CardFilesInputPath), false, "", "", "");
                                            System.IO.File.Move(getCurrentDirectory(CardFilesInputPath) + "\\" + strLocFile, CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt");
                                            physicalfiles.Add(CardFilesSourcePath + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                            //blnfileexists = true;
                                            sCompressedFile[i] = CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt";
                                            i++;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (IsSaveError == "1")
                                            {
                                                FunInsertIntoErrorLog("DownloadReissueFile|Create CardFiles", ex.Message, "CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                                            }
                                            FunInsertIntoLogFile(ErrorLogFilePath, ex, "DownloadReissueFile|Create CardFiles|Para:CardFilesSourcePath = " + CardFilesSourcePath);
                                        }
                                    }
                                }

                            }

                        }
                        return sCompressedFile.Where(c => c != null).ToArray();

                    }
                    else
                    {
                        return sCompressedFile;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ObjConn.Close();
                FunInsertIntoErrorLog("DownloadReissueFile", ex.Message, "IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "DownloadReissueFile|Para:IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath);
                return null;
            }
        }


        //-----------------------------------------------------------
        private string getCurrentDirectory(string ZipFilePath)
        {
            string Path = ZipFilePath;

            if (!(System.IO.Directory.Exists(Path)))
            {
                try
                {
                    Directory.CreateDirectory(Path);
                }
                catch (Exception ex)
                {
                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "getCurrentDirectory|Para:ZipFilePath =" + ZipFilePath);
                }
            }


            return Path;
        }


        //****************** Datatable functions ********************
        public DataTable GetTextFileIntoDataTable(string FilePath, string TableName, string delimiter, string IssuerNo)
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
                    DataColumn workCol = result.Columns.Add("CIF_ID", typeof(String));
                    workCol.AllowDBNull = false;
                    workCol.Unique = false;

                    result.Columns.Add("CustomerName", typeof(String));
                    result.Columns.Add("NameOnCard", typeof(String));
                    result.Columns.Add("Bin_Prefix", typeof(String));
                    result.Columns.Add("AccountNo", typeof(String));
                    result.Columns.Add("AccountOpeningDate", typeof(String));
                    result.Columns.Add("CIF_Creation_Date", typeof(String));
                    result.Columns.Add("Address1", typeof(String));
                    result.Columns.Add("Address2", typeof(String));
                    result.Columns.Add("Address3", typeof(String));
                    result.Columns.Add("City", typeof(String));
                    result.Columns.Add("State", typeof(String));
                    result.Columns.Add("PinCode", typeof(String));
                    result.Columns.Add("Country", typeof(String));
                    result.Columns.Add("Mothers_Name", typeof(String));
                    result.Columns.Add("DOB", typeof(String));
                    result.Columns.Add("CountryCode", typeof(String));
                    result.Columns.Add("STDCode", typeof(String));
                    result.Columns.Add("MobileNo", typeof(String));
                    result.Columns.Add("EmailID", typeof(String));
                    result.Columns.Add("SCHEME_Code", typeof(String));
                    result.Columns.Add("BRANCH_Code", typeof(String));
                    result.Columns.Add("Entered_Date", typeof(String));
                    result.Columns.Add("Verified_Date", typeof(String));
                    result.Columns.Add("PAN_No", typeof(String));
                    result.Columns.Add("Mode_Of_Operation", typeof(String));
                    result.Columns.Add("Fourth_Line_Embossing", typeof(String));
                    result.Columns.Add("Aadhar_No", typeof(String));
                    result.Columns.Add("Issue_DebitCard", typeof(String));
                    result.Columns.Add("Pin_Mailer", typeof(String));
                    result.Columns.Add("Reason", typeof(String));
                    result.Columns.Add("Extra", typeof(String));
                    //result.Columns.Add("Gender", typeof(String));
                    //result.Columns.Add("Nationality", typeof(String));
                    //result.Columns.Add("StatementDelivery", typeof(String));
                    //result.Columns.Add("Email_For_Statement", typeof(String));
                    //result.Columns.Add("ProductType", typeof(String));

                    //Read the rest of the data in the file.        
                    AllData = s.ReadToEnd();
                    s.Close();
                    s.Dispose();

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
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "GetTextFileIntoDataTable|Para : CIF_File = " + FilePath);

                FunInsertIntoErrorLog("GetTextFileIntoDataTable", ex.Message, "CIF_File=" + FilePath, IssuerNo, string.Empty);

                result = new DataTable();
            }
            //Return the imported data.        
            return result;
        }


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

        //**************** Save Error Logs ************
        public void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList, string IssuerNo, string BatchNo)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Sp_CAInsertErrorLog", ObjConn, CommandType.StoredProcedure))
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
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunInsertIntoErrorLog");
                //FunInsertIntoErrorLog("FunInsertIntoErrorLog", ex.Message, "", IssuerNo.ToString(), string.Empty);
            }
        }

        public void FunInsertIntoLogFile(string LogPath, Exception ex, string functionName)
        {
            try
            {
                //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
                string filename = "Log_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
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
                FunInsertIntoErrorLog("FunInsertIntoLogFile", Ex.Message, "LogPath=" + LogPath, "", string.Empty);
            }
        }


        public string FunGetNewFileName(string Input)
        {
            string strNewName = string.Empty;
            strNewName = Input.Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0] + "_D_" + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";
            return strNewName;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool PGP_Decrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo, string PGPPassword)
        {
            FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP CIF file decryption started");
            PGPModel objPGP = new PGPModel();
            objPGP.DecInputFilePath = fileFrom;
            objPGP.DecOutputFilePath = fileTo;
            objPGP.PrivateKeyFilePath = PgpKeyPath;
            objPGP.Password = PGPPassword;


            bool processExited = false;
            /// File info
            FileInfo fi = new FileInfo(fileFrom);
            if (!fi.Exists)
            {
                //FunInsertTextLog("File is not exist to decrypt", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "File is not exist to decrypt");
                FunInsertIntoErrorLog("PGP_Decrypt", "Cannot find the file to decrypt", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;
            }

            if (!File.Exists(PgpKeyPath))
            {
                //FunInsertTextLog("Cannot find PGP Key", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "Cannot find PGP Key");
                FunInsertIntoErrorLog("PGP_Decrypt", "Cannot find PGP Key", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;
            }

            // Cannot encrypt a file if it already exists
            if (File.Exists(fileTo))
            {

                //FunInsertTextLog("Cannot decrypt file.File already exists", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "Cannot decrypt file.File already exists");
                FunInsertIntoErrorLog("PGP_Decrypt", "Cannot decrypt file.  File already exists", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;

            }
            //encrypt through AGSPGP dll
            try
            {
                new PGP().Decrypt(objPGP);
                //check encrypted file created 
                if (File.Exists(fileTo))
                {
                    processExited = true;
                    //FunInsertTextLog("PGP_Decryption end", Convert.ToInt32(IssuerNo));
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Decryption end");
                    FunInsertIntoErrorLog("PGP_Decrypt", "File decrypted successfully. ", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "CIFDecrypt");
                }
            }
            catch (Exception ex)
            {
                processExited = false;
                //FunInsertTextLog("PGP_Decrypt Error", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Decrypt Error");
                FunInsertIntoErrorLog("PGP_Decrypt|PGP().Decrypt(objPGP)", ex.Message, "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
            }
            return processExited;
        }

        public bool PGP_Encrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo)
        {
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
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Missing file.  Cannot find the file to encrypt");
                FunInsertIntoErrorLog("PGP_Encrypt", "Cannot find the file to encrypt", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;
            }

            if (!File.Exists(PgpKeyPath))
            {
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Cannot find PGP Key");
                FunInsertIntoErrorLog("PGP_Encrypt", "Cannot find PGP Key", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;
            }

            /// Cannot encrypt a file if it already exists
            if (File.Exists(fileTo))
            {
                //throw new Exception("Cannot encrypt file.  File already exists");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Cannot encrypt file.File already exists");
                FunInsertIntoErrorLog("PGP_Encrypt", "Cannot encrypt file.  File already exists", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
                return processExited;

            }
            //encrypt through AGSPGP dll
            try
            {
                new PGP().Encrypt(objPGP);
            }
            catch (Exception Ex)
            {
                //FunInsertTextLog("PGP_Encrypt Error", Convert.ToInt32(IssuerNo));
                FunInsertIntoErrorLog("PGP_Encrypt|PGP().Encrypt()", Ex.Message, "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), "");
            }

            //check encrypted file created 
            if (File.Exists(fileTo))
            {
                processExited = true;
            }
            return processExited;
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
