using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.IO;

namespace AGS.RBLCardAutomationISO
{
    public class SearchFile
    {

        public static bool DownloadFile(SFTPDataObject ObjData, int IssuerNo, string processid, string fileid)
        {
            try
            {
                new CardAutomation().FunInsertTextLog("SFTP CONNECTION STARTED TO DOWNLOAD FILES FROM SFTP!", IssuerNo, ObjData.ErrorlogPath);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connection Started To Download the file", IssuerNo.ToString(), 1);
                var sourcePath = ObjData.ServerIP;

                //Below code is for SSH encryption start
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(ObjData.Username, ObjData.Password));
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connection Started ObjData.Username:" + ObjData.Username + " & ObjData.Password:" + ObjData.Password, IssuerNo.ToString(), 1);

                if (!string.IsNullOrEmpty(ObjData.SSH_Private_key_file_Path))
                {
                    var keyFile = new PrivateKeyFile(ObjData.SSH_Private_key_file_Path, ObjData.passphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(ObjData.Username, keyFiles));
                }

                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connection Started ObjData.SSH_Private_key_file_Path:" + ObjData.SSH_Private_key_file_Path + " & ObjData.passphrase:" + ObjData.passphrase, IssuerNo.ToString(), 1);
                var con = new ConnectionInfo(sourcePath, Convert.ToInt32(ObjData.ServerPort), ObjData.Username, methods.ToArray());

                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "Connecting to SFTP server", IssuerNo.ToString(), 1);

                using (var sftpClient = new SftpClient(con))
                {
                  
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "Before sftpClient.Connect()", IssuerNo.ToString(), 1);
                    //sftpClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(100);
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", " **************************SFTP Connected **************************", IssuerNo.ToString(), 1);
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        new CardAutomation().FunInsertTextLog("SFTP Connected!" + WorkingDirectory, IssuerNo, ObjData.ErrorlogPath);
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connected!" + WorkingDirectory, IssuerNo.ToString(), 1);
                        /*NEED TO OPTIMIZED THE BELOW FUNCTION FOR THE SFTP UPLOAD*/
                        // RESULT: SFTP working directory = /customers/5/7/9/domain.com/httpd.www <- NOTE httpd.www

                        var files = sftpClient.ListDirectory(WorkingDirectory + ObjData.SourcePath);
                        foreach (var file in files)
                        {

                            if ((file.Name.EndsWith(ObjData.FileExtension, StringComparison.OrdinalIgnoreCase)))
                            {
                                ObjData.FileName = file.Name;

                                new CardAutomation().FunInsertTextLog("File Found with Name:" + ObjData.FileName, IssuerNo, ObjData.ErrorlogPath);
                                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "File Found with Name:" + ObjData.FileName, IssuerNo.ToString(), 1);
                                /*To download the file for the SFTP*/
                                using (Stream file1 = File.OpenWrite(ObjData.destinationPath + ObjData.FileName))
                                {
                                    sftpClient.DownloadFile(sftpClient.WorkingDirectory + ObjData.SourcePath + ObjData.FileName, file1);
                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    new CardAutomation().FunInsertTextLog("File  with Name:" + ObjData.FileName + " DownLoaded To Location " + ObjData.SourcePath + ObjData.FileName, IssuerNo, ObjData.ErrorlogPath);
                                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "File  with Name:" + ObjData.FileName + " DownLoaded To Location " + ObjData.SourcePath + ObjData.FileName, IssuerNo.ToString(), 1);
                                }

                            }
                        }
                    }
                    sftpClient.Dispose();

                    new CardAutomation().FunInsertTextLog("SFTP CONNECTION COMPLETED!", IssuerNo, ObjData.ErrorlogPath);
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP CONNECTION COMPLETED!", IssuerNo.ToString(), 1);

                }


            }


            catch (Exception Ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, Ex.ToString(), "", IssuerNo.ToString(), 0);
                return false;
            }
            return true;
        }
        public static bool UploadFile(SFTPDataObject ObjData, int IssuerNo, string processid, string fileid)
        {
            try
            {

                new CardAutomation().FunInsertTextLog("SFTP CONNECTION STARTED TO UPLOAD FILES TO SFTP!", IssuerNo, ObjData.ErrorlogPath);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connection Started To Upload file to SFTP", IssuerNo.ToString(), 1);
                var sourcePath = ObjData.ServerIP;

                //Below code is for SSH encryption start
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(ObjData.Username, ObjData.Password));

                if (!string.IsNullOrEmpty(ObjData.SSH_Private_key_file_Path))
                {
                    var keyFile = new PrivateKeyFile(ObjData.SSH_Private_key_file_Path, ObjData.passphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(ObjData.Username, keyFiles));
                }

                var con = new ConnectionInfo(sourcePath, Convert.ToInt32(ObjData.ServerPort), ObjData.Username, methods.ToArray());
                using (var sftpClient = new SftpClient(con))
                {
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", " **************************SFTP Connected **************************", IssuerNo.ToString(), 1);
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        new CardAutomation().FunInsertTextLog("SFTP Connected!" + WorkingDirectory, IssuerNo, ObjData.ErrorlogPath);
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP Connected!" + WorkingDirectory, IssuerNo.ToString(), 1);
                        /*NEED TO OPTIMIZED THE BELOW FUNCTION FOR THE SFTP UPLOAD*/
                        // RESULT: SFTP working directory = /customers/5/7/9/domain.com/httpd.www <- NOTE httpd.www


                        var files = Directory.GetFiles(ObjData.SourcePath, (ObjData.FileName).Trim() == "" ? "*" + ObjData.FileExtension : ObjData.FileName, SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {

                            ObjData.FileName = Path.GetFileName(file);

                            new CardAutomation().FunInsertTextLog("File Found with Name:" + ObjData.FileName, IssuerNo, ObjData.ErrorlogPath);
                            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "File Found with Name:" + ObjData.FileName, IssuerNo.ToString(), 1);
                            /*To Upload the file for the SFTP if the file is downloaded*/
                            if (File.Exists(ObjData.SourcePath + ObjData.FileName))
                            {

                                var file1 = new FileStream(ObjData.SourcePath + ObjData.FileName, FileMode.Open);
                                {

                                    sftpClient.UploadFile(file1, ObjData.destinationPath + ObjData.FileName, null);

                                    /*TO DELETE THE FILE FORM THE SFTP IF THE FILE SUCCESSFULLY UPLOAD*/
                                    if (sftpClient.Exists(ObjData.Deletebasefilepath + ObjData.FileName))
                                    {
                                        sftpClient.DeleteFile(sftpClient.WorkingDirectory + ObjData.Deletebasefilepath + ObjData.FileName);
                                    }


                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    new CardAutomation().FunInsertTextLog("File  with Name:" + ObjData.FileName + " Uploaded To Location " + ObjData.destinationPath + ObjData.FileName, IssuerNo, ObjData.ErrorlogPath);
                                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "File  with Name:" + ObjData.FileName + " Uploaded To Location " + ObjData.destinationPath + ObjData.FileName, IssuerNo.ToString(), 1);
                                }
                            }
                        }
                        sftpClient.Dispose();
                        new CardAutomation().FunInsertTextLog("SFTP CONNECTION COMPLETED!", IssuerNo, ObjData.ErrorlogPath);
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, "", "SFTP CONNECTION COMPLETED!", IssuerNo.ToString(), 1);
                    }
                }
            }
            catch (Exception Ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, Ex.ToString(), "", IssuerNo.ToString(), 0);
                return false;
            }
            return true;
        }
    }
}
