using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Renci.SshNet;
using System.Data.SqlClient;
using System.Data;
using ReflectionIT.Common.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;

//Added logs Sai Sherlekar
namespace AGS.CardAutomationISO
{
    public class CardAuto : IDisposable
    {
        public string ErrorLogFilePath { get; set; }
        public CardAuto()
        {
            ErrorLogFilePath = string.Empty;
        }

        public void Process(int IssurNo)
        {
            ClsPathsBO objPath = new ClsPathsBO();
            //To remove
            //int IssurNo = 13;
            objPath = FunGetPaths(IssurNo);

            try
            {

                if (!string.IsNullOrEmpty(objPath.ID))
                {
                    ErrorLogFilePath = objPath.ErrorLogPath;

                    FunInsertIntoErrorLog("Process", "Process Started", "", IssurNo.ToString(), "");
                    //FunInsertIntoLogFile(ErrorLogFilePath, null, "Process Started");

                    if (!Directory.Exists(ErrorLogFilePath))
                    {
                        try
                        {
                            Directory.CreateDirectory(ErrorLogFilePath);
                        }
                        catch (Exception ex)
                        {
                            FunInsertIntoErrorLog("Process|Create Log Directory", ex.Message, "", IssurNo.ToString(), "");
                            // FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process|Create Directory|Para:" + ErrorLogFilePath);
                        }
                    }
                    //Generate Card files from CIF file

                    //************** Get CIF files from sftp *****************
                    FunGetSFTP_CIF_Files(objPath);
                    string[] CIFfilesArr = null;
                    //************** Process CIF Files save cif file data *************
                    try
                    {
                        //Start Diksha 25/06
                        CIFfilesArr = Directory.GetFiles(objPath.CardCIF_Input_Path, "*.txt");
                    }
                    catch (Exception ex)
                    {
                        if (objPath.IsSaveError == "1")
                        {
                            FunInsertIntoErrorLog("Process|GetCIFFiles", ex.Message, "", IssurNo.ToString(), "");
                        }
                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process|GetCIFFiles");
                    }
                    //   string[] CIFfilesArr = Directory.GetFiles(objPath.CardCIF_Input_Path);
                    if (CIFfilesArr != null)
                    {
                        if (CIFfilesArr.Count() > 0)
                        {
                            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
                            foreach (string filePath in CIFfilesArr)
                            {
                                ObjResult = ExtractFrmCIF_File(filePath, IssurNo, objPath.IsSaveError, objPath.FailedCIFPath);
                                if (ObjResult.Code == 0)
                                {
                                    try
                                    {
                                        if (Directory.GetFiles(objPath.FailedCIFPath, "*.txt").Count() > 0)
                                        {
                                            foreach (string filename in Directory.GetFiles(objPath.FailedCIFPath, "*.txt"))
                                            {
                                                SFTP_FailedCIFUpload(objPath);
                                            }
                                        }
                                        //move cif file to backup folder after saving in database
                                       
                                        File.Move(filePath, objPath.CardCIF_Backup + FunGetNewFileName(filePath.Split('\\')[filePath.Split('\\').Length - 1]));
                                    }
                                    catch (Exception ex)
                                    {
                                        if (objPath.IsSaveError == "1")
                                        {
                                            FunInsertIntoErrorLog("Process|Move CIF file to BackUp", ex.Message, "filePath=" + filePath + "BackUp Path=" + objPath.CardCIF_Backup + filePath.Split('\\')[filePath.Split('\\').Length - 1], IssurNo.ToString(), string.Empty);
                                        }
                                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process|Move CIF file to BackUp");
                                    }

                                }
                                else
                                {
                                    if (objPath.IsSaveError == "1")
                                    {
                                        FunInsertIntoErrorLog("Process|ExtractFrmCIF_File result", "CIF file data not save", "filePath=" + filePath, IssurNo.ToString(), string.Empty);
                                    }
                                    FunInsertIntoLogFile(ErrorLogFilePath, null, "Process|CIF file data not save|Para :filePath=" + filePath);
                                }
                            }
                        }
                    }
                    //Get card,customer,account files and put in card Auto Source path
                    string[] strCardFileName = DownloadFiles(IssurNo, objPath.ZipCardFilesPath, objPath.CardAutoSourcePath, objPath.CardAutoFailedPath, objPath.IsSaveError);


                    //// Extract zip/Rar file
                    // ExtractRarFiles(objPath);

                    //***************** Put card,customer,account files on SFTP ***************

                    SFTPFileUpload(objPath);

                    //int result = CheckCardFilesOnSftp(objPath);
                    //if (result == 1)
                    //{

                    //check cardprograms and card accts files on sftp
                    ClsReturnStatusBO ObjCheckFiles = new ClsReturnStatusBO();
                    ObjCheckFiles = CheckCardFilesOnSftp(objPath);
                    if (ObjCheckFiles.Code == 1 && !string.IsNullOrEmpty(ObjCheckFiles.OutPutCode))
                    {
                        //************** Final generation of card  ******************
                        ClsReturnStatusBO ObjOutResult = new ClsReturnStatusBO();
                        ObjOutResult = CardFilesProcess(IssurNo, objPath.IsSaveError, ObjCheckFiles.OutPutCode);
                        //to remove
                        FunInsertIntoErrorLog("Process|CardFilesProcess Response", null, "SPBankAutomationResult :" + ObjOutResult.OutPutCode + "Card files count on server Count=" + Directory.GetFiles(objPath.CardAutoSourcePath, "*.txt").Length, IssurNo.ToString(), string.Empty);
                        //Check files exists at source path
                        if (Directory.GetFiles(objPath.CardAutoSourcePath, "*.txt").Length > 0)
                        {
                            //On success
                            if (ObjOutResult.OutPutCode == "00")
                            {
                                if (!(System.IO.Directory.Exists(objPath.CardAutoBackUpPath)))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(objPath.CardAutoBackUpPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process");
                                    }
                                }
                                //move source card files to success folder
                                foreach (string filePath in Directory.GetFiles(objPath.CardAutoSourcePath, "*.txt"))
                                {
                                    try
                                    {
                                        File.Move(filePath, objPath.CardAutoBackUpPath + filePath.Split('\\')[filePath.Split('\\').Length - 1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (objPath.IsSaveError == "1")
                                        {
                                            FunInsertIntoErrorLog("Process|move cardfiles to success folder", ex.Message, "", IssurNo.ToString(), string.Empty);
                                        }
                                        FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process|move cardfiles to success folder");
                                    }
                                }

                                //************** Delete input files from sftp  ******************
                                DeleteFilefrmSFTP(objPath.CardAutoOutputPath_SFTP, objPath.AGS_SFTPServer, objPath.AGS_SFTP_Port, objPath.AGS_SFTP_User, objPath.AGS_SFTP_Pwd, objPath.IsSaveError, IssurNo.ToString());
                                //new change 
                                FunInsertIntoErrorLog("Process", "Process Success", "", IssurNo.ToString(), "");
                                FunInsertIntoLogFile(ErrorLogFilePath, null, "Process Success");
                            }
                            // on fail
                            else
                            {
                                FunInsertIntoErrorLog("Process", "Process Failed", "", IssurNo.ToString(), "");
                                FunInsertIntoLogFile(ErrorLogFilePath, null, "Process failed");
                            }

                            //    else
                            //    {                     

                            //        if (!(System.IO.Directory.Exists(objPath.CardAutoFailedPath)))
                            //        {
                            //            try
                            //            {
                            //                Directory.CreateDirectory(objPath.CardAutoFailedPath);
                            //            }
                            //            catch (Exception ex)
                            //            { }
                            //        }
                            //        //move source card files to failed folder
                            //        foreach (string filePath in strCardFileName)
                            //        {
                            //            File.Move(filePath, objPath.CardAutoFailedPath+ filePath.Split('\\')[filePath.Split('\\').Length - 1]);
                            //        }
                            //    }
                        }
                    }
                    else
                    {
                        FunInsertIntoLogFile(ErrorLogFilePath, null, "Process failed|card account files/Card programs not found on SFTP");
                        FunInsertIntoErrorLog("Process", "Process failed|card account files/Card programs not found on SFTP", "", IssurNo.ToString(), "");
                    }

                }
                else
                {
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "Process failed|Path not found");
                    FunInsertIntoErrorLog("Process", "Process failed|Paths not found", "", IssurNo.ToString(), "");
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "Process");

                if (objPath.IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("Process", ex.Message, "", IssurNo.ToString(), "");
                }

            }

        }

        //public int CheckCardFilesOnSftp(ClsPathsBO ObjPath)
        //{
        //    int iResult = 0;
        //    try
        //    {
        //        using (var sftp = new SftpClient(ObjPath.AGS_SFTPServer, Convert.ToInt32(ObjPath.AGS_SFTP_Port), ObjPath.AGS_SFTP_User, ObjPath.AGS_SFTP_Pwd))
        //        {
        //            sftp.Connect();
        //            string remoteDirectory = ObjPath.CardAutoOutputPath_SFTP;
        //            var files = sftp.ListDirectory(remoteDirectory);

        //            int count = (from c in files where c.Length > 0 && c.Name.Contains(".txt") select c).ToList().Count;

        //            sftp.Disconnect();
        //            if (count == 4)
        //            {
        //                iResult = 1;
        //            }

        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        if (ObjPath.IsSaveError == "1")
        //        {
        //            FunInsertIntoErrorLog("CheckCardFilesOnSftp", Ex.Message, "SFTP_FilePath=" + ObjPath.CardAutoOutputPath_SFTP, ObjPath.IssuerNo, string.Empty);
        //        }
        //        FunInsertIntoLogFile(ErrorLogFilePath, Ex, "CheckCardFilesOnSftp|Para:SFTP_FilePath = " + ObjPath.CardAutoOutputPath_SFTP);
        //    }
        //    return iResult;
        //}
        public ClsReturnStatusBO CheckCardFilesOnSftp(ClsPathsBO ObjPath)
        {
            ClsReturnStatusBO objStatus = new ClsReturnStatusBO();
            int iResult = 0;
            string strCardPrograms = string.Empty;
            try
            {
                using (var sftp = new SftpClient(ObjPath.AGS_SFTPServer, Convert.ToInt32(ObjPath.AGS_SFTP_Port), ObjPath.AGS_SFTP_User, ObjPath.AGS_SFTP_Pwd))
                {
                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftp.BufferSize = 5000000;
                    sftp.Connect();
                    string remoteDirectory = ObjPath.CardAutoOutputPath_SFTP;
                    var files = sftp.ListDirectory(remoteDirectory);

                    int count = (from c in files where c.Length > 0 && c.Name.Contains(".txt") select c).ToList().Count;
                    if (count >= 4)
                    {
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
                    }
                }

            }
            catch (Exception Ex)
            {

                if (ObjPath.IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("CheckCardFilesOnSftp", Ex.Message, "SFTP_FilePath=" + ObjPath.CardAutoOutputPath_SFTP + " , Temp file path=" + ObjPath.ZipCardFilesPath, ObjPath.IssuerNo, string.Empty);
                }
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "CheckCardFilesOnSftp|Para:SFTP_FilePath = " + ObjPath.CardAutoOutputPath_SFTP);
            }
            objStatus.Code = iResult;
            objStatus.OutPutCode = strCardPrograms;
            return objStatus;
        }

        public void DeleteFilefrmSFTP(string FilePath, string SFTP_Server, string SFTP_Port, string SFTP_User, string SFTP_PWD, string IsSaveError, string IssuerNo)
        {
            using (var sftp = new SftpClient(SFTP_Server, Convert.ToInt32(SFTP_Port), SFTP_User, SFTP_PWD))
            {
                try
                {
                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftp.BufferSize = 5000000;
                    sftp.Connect();
                    string InputFilePath = FilePath;

                    var FilesArr = sftp.ListDirectory(InputFilePath);

                    foreach (var d in from i in FilesArr.AsEnumerable() where (i.Length > 0 && i.Name != "." && i.Name.Contains(".txt")) select i)
                    {
                        try
                        {
                            sftp.DeleteFile(d.FullName);
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
                    if (IsSaveError == "1")
                    {
                        FunInsertIntoErrorLog("DeleteFilefrmSFTP", Ex.Message, "SFTP_FilePath=" + FilePath, IssuerNo, string.Empty);
                    }
                    FunInsertIntoLogFile(ErrorLogFilePath, Ex, "DeleteFilefrmSFTP|Para:SFTP_FilePath = " + FilePath);
                }
            }
        }

        public void SFTPFileUpload(ClsPathsBO objpath)
        {

            try
            {
                var sourcePath = objpath.AGS_SFTPServer;
                //var BackUpfilePath = ConfigurationManager.AppSettings["CardAutoSFTPUploaded"];
                using (var sftp = new SftpClient(sourcePath, Convert.ToInt32(objpath.AGS_SFTP_Port), objpath.AGS_SFTP_User, objpath.AGS_SFTP_Pwd))   //".\\dipak.gole","Di$@12345"
                {
                    try
                    {
                        string InputFilePath = objpath.CardAutoSourcePath;

                        string[] FilesArr = Directory.GetFiles(InputFilePath, "*.txt");

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
                                        sftp.UploadFile(fileStream, objpath.CardAutoOutputPath_SFTP + (path[path.Length - 1].Contains('_') ? ((path[path.Length - 1].Split('_'))[0] + ".txt") : (path[path.Length - 1])), true);

                                        //new change
                                        fileStream.Flush();
                                        fileStream.Close();
                                        fileStream.Dispose();
                                    }

                                    //after SFTP upload move file to backUp
                                    //if (!Directory.Exists(BackUpfilePath))
                                    //    Directory.CreateDirectory(BackUpfilePath);

                                    // File.Move(file, BackUpfilePath + path[path.Length - 1]);
                                    //File.Delete(file);
                                }
                                catch (Exception ex)
                                {
                                    if (objpath.IsSaveError == "1")
                                        FunInsertIntoErrorLog("SFTPFileUpload|Cardfiles Upload", ex.Message, "CardFilesPath=" + file + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Cardfiles Upload|Para:CardFilesPath=" + file + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
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
                if (objpath.IsSaveError == "1")
                    FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
            }
        }

        public void SFTP_FailedCIFUpload(ClsPathsBO objpath)
        {
            try
            {

                //var BackUpfilePath = ConfigurationManager.AppSettings["CardAutoSFTPUploaded"];
                using (var sftp = new SftpClient(objpath.B_SFTPServer, Convert.ToInt32(objpath.B_SFTP_Port), objpath.B_SFTP_User, objpath.B_SFTP_Pwd))   //".\\dipak.gole","Di$@12345"
                {
                    try
                    {
                        string InputFilePath = objpath.FailedCIFPath;

                        //Start Diksha 25/06
                        string[] FilesArr = Directory.GetFiles(InputFilePath, "*.txt");

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
                                        sftp.UploadFile(fileStream, objpath.B_SFTP_FailedCIFPath + path[path.Length - 1], true);


                                        fileStream.Flush();
                                        fileStream.Close();
                                        fileStream.Dispose();
                                        File.Delete(file);
                                    }

                                    //after SFTP upload move file to backUp
                                    //if (!Directory.Exists(BackUpfilePath))
                                    //    Directory.CreateDirectory(BackUpfilePath);

                                    // File.Move(file, BackUpfilePath + path[path.Length - 1]);
                                    //File.Delete(file);
                                }
                                catch (Exception ex)
                                {
                                    if (objpath.IsSaveError == "1")
                                        FunInsertIntoErrorLog("SFTP_FailedCIFUpload|Failed CIF Upload", ex.Message, "Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath, objpath.IssuerNo, string.Empty);
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTP_FailedCIFUpload|Failed CIF Upload|Para:Failed CIF Path=" + file + ",SFTP_FailedCIFPath=" + objpath.B_SFTP_FailedCIFPath);
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
                if (objpath.IsSaveError == "1")
                    FunInsertIntoErrorLog("SFTPFileUpload", ex.Message, "CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP, objpath.IssuerNo, string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "SFTPFileUpload|Para:CardFilesPath=" + objpath.CardAutoSourcePath + ",SFTP_Card_FilesPath=" + objpath.CardAutoOutputPath_SFTP);
            }
        }

        public ClsReturnStatusBO CardFilesProcess(int IssurNo, string IsSaveError, string CardPrograms)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.OutPutCode = string.Empty;
            ObjResult.Description = string.Empty;
            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_BankCardAutomation", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntIssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssurNo);
                    ObjCmd.AddParameterWithValue("@StrCardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, CardPrograms);
                    // ObjCmd.AddParameterWithValue("@StrStatusCode", SqlDbType.VarChar, 0, ParameterDirection.Output, ObjResult.OutPutCode);
                    //ObjCmd.AddParameterWithValue("@StrStatusDesc", SqlDbType.VarChar, 0, ParameterDirection.Output, ObjResult.Description);
                    ObjCmd.AddParameter("@StrStatusCode", SqlDbType.VarChar, 500, ParameterDirection.Output);
                    ObjCmd.AddParameter("@StrStatusDesc", SqlDbType.VarChar, 800, ParameterDirection.Output);
                    ObjCmd.ExecuteNonQuery();
                    ObjResult.Description = ObjCmd.Parameters["@StrStatusDesc"].Value.ToString();
                    // ObjResult.Code = Convert.ToInt16(ObjCmd.Parameters["@IntpriOutput"].Value);
                    ObjResult.OutPutCode = (ObjCmd.Parameters["@StrStatusCode"].Value.ToString());


                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                ObjResult.OutPutCode = "1";
                ObjResult.Description = ex.Message;
                if (IsSaveError == "1")
                    FunInsertIntoErrorLog("CardFilesProcess", ex.Message, "CardPrograms=" + CardPrograms, IssurNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "CardFilesProcess|Para: IssuerNo :" + IssurNo + " ,CardPrograms: " + CardPrograms);
            }
            return ObjResult;
        }

        public void ExtractRarFiles(ClsPathsBO ObjPath)
        {
            try
            {
                //DirectoryInfo Dir = new DirectoryInfo(ConfigurationManager.AppSettings["CARDGENPATH"]);
                string RarSourcePath = ObjPath.ZipCardFilesPath;
                string[] filesArr = Directory.GetFiles(RarSourcePath);
                if (filesArr.Count() > 0)
                {
                    foreach (string fileName in filesArr)
                    {
                        string source = string.Empty;
                        string destinationFolder = string.Empty;
                        string[] ResultArr = fileName.Split('\\')[fileName.Split('\\').Length - 1].Split('_');


                        if (ResultArr.Contains(ObjPath.BankName))
                        {
                            source = fileName;
                            destinationFolder = ObjPath.CardAutoSourcePath;
                        }



                        if ((!string.IsNullOrEmpty(source)) && (File.Exists(source)))
                        {
                            //create passward
                            FileInfo Rarfile = new FileInfo(source);
                            string Pwd = "rbl_" + Rarfile.CreationTime.ToString("ddMMyy");

                            System.Diagnostics.Process p = new System.Diagnostics.Process();
                            //for WinRAR
                            //p.StartInfo.CreateNoWindow = true;
                            //p.StartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["rarWorkingDirectory"];
                            //p.StartInfo.UseShellExecute = false;
                            //p.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["rarWorkingDirectory"] + "WinRAR.exe";
                            //p.StartInfo.Arguments = String.Format("x -p{0} {1} {2}", Pwd, source, destinationFolder);

                            // start For 7z.exe
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.WorkingDirectory = ObjPath.Zip_Exe_Path;
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.FileName = ObjPath.Zip_Exe_Path + "7z.exe";
                            p.StartInfo.Arguments = String.Format("x -p{0} {1} -o{2}", Pwd, source, destinationFolder);

                            //   p.StartInfo.Arguments = "x -p"+Pwd+" "+ source+" -o"+destinationFolder;
                            // End For 7z.exe

                            using (System.Diagnostics.Process zipProcess = System.Diagnostics.Process.Start(p.StartInfo))
                            {
                                zipProcess.WaitForExit();
                            }
                        }

                    }

                }



            }
            catch (Exception ex)
            { }
        }


        public void FunGetSFTP_CIF_Files(ClsPathsBO objPath)
        {
            //fetch files from Sftp
            var SFTP_CIF_FileSourcePath = objPath.SFTP_CIF_Source_Path;
            var CIF_FileInputPath = objPath.CardCIF_Input_Path;
            var CIF_FileBackUp = objPath.CardCIF_Backup;
            var SFTP_CIFBackUp = objPath.SFTP_CIF_BackUp_Path;
            string rarFilePath = objPath.ZipCardFilesPath, BankName = objPath.BankName;

            try
            {

                //Get CIF files from SFTP Path

                var sourcePath = objPath.B_SFTPServer;

                using (var sftp = new SftpClient(sourcePath, Convert.ToInt32(objPath.B_SFTP_Port), objPath.B_SFTP_User, objPath.B_SFTP_Pwd))   //".\\dipak.gole","Di$@12345"
                {
                    sftp.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftp.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftp.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftp.BufferSize = 5000000;
                    sftp.Connect();
                    string remoteDirectory = objPath.SFTP_CIF_Source_Path;
                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var d in from i in files.AsEnumerable() where (i.Length > 0 && i.Name.Contains(".txt")) select i)
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
                                //sftp.DeleteFile(d.FullName);
                            }
                            catch (Exception ex)
                            {
                                if (objPath.IsSaveError == "1")
                                {
                                    FunInsertIntoErrorLog("FunGetSFTP_CIF_Files|move to SFTP BackUp", ex.Message, "SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp, objPath.IssuerNo, string.Empty);
                                }
                                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetSFTP_CIF_Files|move to CIF SFTP BackUp|Para: SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp);
                            }
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
                if (objPath.IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("FunGetSFTP_CIF_Files", ex.Message, "SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp, objPath.IssuerNo, string.Empty);
                }
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunGetSFTP_CIF_Files|Para: SFTP_CIFPath =" + SFTP_CIF_FileSourcePath + " ,SFTP_CIFBackUp =" + SFTP_CIFBackUp + "CIF_Input=" + CIF_FileInputPath + "CIF_BackUp=" + CIF_FileBackUp);
            }
        }

        public ClsReturnStatusBO ExtractFrmCIF_File(string FilePath, int issuerNo, string IsSaveError, string FailedCIFPath)
        {
            ClsReturnStatusBO ObjStatus = new ClsReturnStatusBO();
            ObjStatus.Code = 1;
            try
            {

                DataTable DtFileData = new DataTable();
                DtFileData = GetTextFileIntoDataTable(FilePath, string.Empty, "|", IsSaveError, issuerNo.ToString());

                //Save for card production
                string fileName = FilePath.Split('\\')[FilePath.Split('\\').Length - 1];
                ObjStatus = FunProcessCIFFileData(issuerNo, DtFileData, fileName, IsSaveError, FailedCIFPath);

            }
            catch (Exception Ex)
            {
                if (IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("ExtractFrmCIF_File", Ex.Message, "FilePath= " + FilePath + ",issuerNo= " + issuerNo, issuerNo.ToString(), string.Empty);
                }
                FunInsertIntoLogFile(ErrorLogFilePath, Ex, "ExtractFrmCIF_File|Para : FilePath = " + FilePath);
            }
            return ObjStatus;

        }

        //public ClsReturnStatusBO FunProcessCIFFileData(int IssurNo, DataTable DtBulkData, string CIF_fileName, string IsSaveError)
        //{
        //    ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
        //    ObjResult.Code = 1;
        //    ObjResult.Description = string.Empty;
        //    DataTable ObjDTOutPut = new DataTable();
        //    SqlConnection ObjConn = null;

        //    try
        //    {

        //        DataTable ObjDtOutPut = new DataTable();

        //        ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
        //        using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_Bulk_SaveCardProdData", ObjConn, CommandType.StoredProcedure))
        //        {
        //            ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssurNo);
        //            ObjCmd.AddParameterWithValue("@FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, CIF_fileName);
        //            ObjCmd.AddParameterWithValue("@CustBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, DtBulkData);

        //            ObjDTOutPut = ObjCmd.ExecuteDataTable();
        //            if (ObjDTOutPut.Rows.Count > 0)
        //            {
        //                ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
        //                ObjResult.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
        //                ObjResult.OutPutCode = ObjDTOutPut.Rows[0]["OutPutCode"].ToString();
        //            }

        //            ObjCmd.Dispose();
        //            ObjConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjConn.Close();
        //        ObjResult.OutPutCode = "1";
        //        ObjResult.Code = 1;
        //        ObjResult.Description = ex.Message;
        //        if (IsSaveError == "1")
        //            FunInsertIntoErrorLog("FunProcessCIFFileData", ex.Message, "CIF_File" + CIF_fileName, IssurNo.ToString(), string.Empty);
        //        FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunProcessCIFFileData|Para : CIF_File = " + CIF_fileName);
        //    }
        //    return ObjResult;

        //}


        public ClsReturnStatusBO FunProcessCIFFileData(int IssurNo, DataTable DtBulkData, string CIF_fileName, string IsSaveError, string FailedCIFPath)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = string.Empty;
            DataSet ObjDTOutPut = new DataSet();
            SqlConnection ObjConn = null;

            try
            {

                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_Bulk_SaveCardProdData", ObjConn, CommandType.StoredProcedure))
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
                        if (ObjResult.Code == 0)
                        {
                            if (ObjDTOutPut.Tables[1].Rows.Count > 0)
                            {

                                try
                                {
                                    string FileCreationPath = getCurrentDirectory(FailedCIFPath);
                                    string strLocFile = CreateDownloadCSVFile(ObjDTOutPut.Tables[1].ToSelectedTableColumns("Result"), FileCreationPath, false, "", "", "");
                                    //Start Diksha 25/06
                                    //System.IO.File.Move(FileCreationPath + "\\" + strLocFile, FileCreationPath + "\\" +CIF_fileName);
                                    System.IO.File.Move(FileCreationPath + "\\" + strLocFile, FileCreationPath + "\\" + FunGetNewFileName(CIF_fileName));
                                }
                                catch (Exception ex)
                                {
                                    if (IsSaveError == "1")
                                    {
                                        FunInsertIntoErrorLog("FunProcessCIFFileData|Create failed CIF file", ex.Message, "CIF_fileName=" + CIF_fileName, IssurNo.ToString(), string.Empty);
                                    }
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
                if (IsSaveError == "1")
                    FunInsertIntoErrorLog("FunProcessCIFFileData", ex.Message, "CIF_File" + CIF_fileName, IssurNo.ToString(), string.Empty);
                FunInsertIntoLogFile(ErrorLogFilePath, ex, "FunProcessCIFFileData|Para : CIF_File = " + CIF_fileName);
            }
            return ObjResult;

        }


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
                if (objPath.IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("FunGetPaths", ex.Message, "IssuerNo=" + IssuerNo, IssuerNo.ToString(), string.Empty);
                }

            }
            return objPath;
        }

        public DataTable GetTextFileIntoDataTable(string FilePath, string TableName, string delimiter, string IsSaveError, string IssuerNo)
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
                    workCol.Unique = true;

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
                    result.Columns.Add("AccountType", typeof(String));
                    result.Columns.Add("SystemID", typeof(String));
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
                if (IsSaveError == "1")
                {
                    FunInsertIntoErrorLog("GetTextFileIntoDataTable", ex.Message, "CIF_File=" + FilePath, IssuerNo, string.Empty);
                }
                result = new DataTable();
            }
            //Return the imported data.        
            return result;
        }

        internal string[] DownloadFiles(int IssuerNo, string CardFilesInputPath, string CardFilesSourcePath, string CardFileFailedPath, string IsSaveError)
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
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
                        // foreach (string bank in BankList.ToList())
                        //{
                        //sCompressedFile = new string[ObjDsOutPut.Count()];
                        String sCreationSuffix = DateTime.Now.ToString("ddMMyy hhmmss").Replace(" ", "_") + "_" + rnd.Next();
                        int i = 0;
                        // Check if file exists at Path
                        if (Directory.GetFiles(CardFilesSourcePath, "*.txt").Length > 0)
                        {

                            //move previous source card files to failed folder

                            foreach (string filePath in (Directory.GetFiles(CardFilesSourcePath, "*.txt")))
                            {
                                try
                                {
                                    File.Move(filePath, CardFileFailedPath + filePath.Split('\\')[filePath.Split('\\').Length - 1]);
                                }
                                catch (Exception ex)
                                {
                                    if (IsSaveError == "1")
                                    {
                                        FunInsertIntoErrorLog("DownloadFiles|Move prev CardFiles to failed Path", ex.Message, "filePath=" + filePath + ",CardFileFailedPath=", IssuerNo.ToString(), string.Empty);
                                    }
                                    FunInsertIntoLogFile(ErrorLogFilePath, ex, "DownloadFiles|Move prev CardFiles to failed Path");
                                }
                            }
                        }

                        foreach (DataTable dtOutput in ObjDsOutPut.Tables)
                        {
                            if (dtOutput.Rows.Count > 0)
                            {
                                //FunInsertIntoErrorLog("DownloadFiles|Data", null, "countCardFilesSourcePath=" + CardFilesSourcePath + ",CardFileFailedPath=" + ",filename=" + ObjDsOutPut.Tables[0].Rows[0]["FileNames"] + ",Table count=" + ObjDsOutPut.Tables.Count, IssuerNo.ToString(), string.Empty);
                                //filter data bank wise
                                // DataTable dtOutput = dtOutputAll.AsEnumerable()
                                //.Where(row => row.Field<string>("Bank") == bank).CopyToDataTable();
                                //foreach (DataTable dtOutput in dtOutputAll.Select("Bank=" + bank).CopyToDataTable().Rows)
                                //foreach (DataTable dtOutput in DtFilterd)
                                //{
                                //   BankName = dtOutput.Rows[0]["BankName"].ToString();
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

                        // }
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

        public static string Chr(Int32 CharCode)
        {
            string strChar = "";

            try
            {
                strChar = Convert.ToString((char)CharCode);
            }
            catch
            {
                strChar = "";
            }
            finally
            {

            }

            return strChar;
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

        //Start Diksha 25/06
        public string FunGetNewFileName(string Input)
        {
            string strNewName = string.Empty;
            strNewName = Input.Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0] + "_D_" + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";
            return strNewName;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
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
        }

    }
    //public static class ExtentionMethods
    //{
    //    public static DataTable ToSelectedTableColumns(this DataTable dtTable, string commaSepColumns)
    //    {
    //        String[] selectedColumns = new string[commaSepColumns.Split(',').Count()]; // = new  new[30] //{ "Column1", "Column2" };
    //        int i = 0;
    //        foreach (string columns in commaSepColumns.Split(','))
    //        {
    //            selectedColumns[i] = columns;
    //            i++;
    //        }
    //        return new DataView(dtTable).ToTable(false, selectedColumns);
    //    }
    //}


}
