using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.IO;

namespace InstaCardsFileGeneration
{
    public class SearchFile
    {
        public static String fn_Create_Directory(String DirPath)
        {
            try
            {
                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
              
                }
                return DirPath;
            }
            catch (Exception Ex)
            {
                return DirPath;

            }

        }


        public static bool DownloadFile(SFTPDataObject ObjData,String IssuerNo)
        {
            try

            {
                ConfigDataObject ObjConfig = new ConfigDataObject();
                new ModuleDAL().FunInsertIntoErrorLog("DownloadFile","", ObjData.processid, "", "SFTP CONNECTION STARTED TO DOWNLOAD FILES FROM SFTP!", IssuerNo, 1);
                //new CardAutomation().FunInsertTextLog("SFTP CONNECTION STARTED TO DOWNLOAD FILES FROM SFTP!", IssuerNo, ObjData.ErrorlogPath);
 
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
                        new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", " **************************SFTP Connected **************************", IssuerNo, 1);
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        new ModuleDAL().FunInsertIntoErrorLog("DownloadFile","", ObjData.processid, "", "SFTP Connected!" + WorkingDirectory + ObjData.SourcePath, IssuerNo, 1);
                        
                        /*NEED TO OPTIMIZED THE BELOW FUNCTION FOR THE SFTP UPLOAD*/
                        // RESULT: SFTP working directory = /customers/5/7/9/domain.com/httpd.www <- NOTE httpd.www

                        var files = sftpClient.ListDirectory(WorkingDirectory + ObjData.SourcePath);

                        foreach (var file in files)
                        {
                          if ((file.Name.EndsWith(ObjData.FileExtension, StringComparison.OrdinalIgnoreCase)))
                            {
                                ObjData.FileName = file.Name;
                                new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", "File Found with Name:" + ObjData.FileName, IssuerNo, 1);
                              
                                /*To download the file for the SFTP*/
                                using (Stream file1 = File.OpenWrite(ObjData.destinationPath + ObjData.FileName))
                                {
                                    sftpClient.DownloadFile(sftpClient.WorkingDirectory + ObjData.SourcePath + ObjData.FileName, file1);
                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", "File  with path and name :" + sftpClient.WorkingDirectory + ObjData.SourcePath + ObjData.FileName + " DownLoaded To Location " + ObjData.destinationPath + ObjData.FileName, IssuerNo, 1);
                                }

                                var file2 = new FileStream(ObjData.destinationPath + ObjData.FileName, FileMode.Open);
                                {
                                //    using (Stream file1 = File.OpenWrite(ObjData.destinationPath + ObjData.FileName))
                                //{
                                    sftpClient.UploadFile(file2, ObjData.Bank_Insta_Archive + ObjData.FileName,null);

                                    file2.Flush();
                                    file2.Close();
                                    file2.Dispose();
                                    new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", "File  with Name:" + ObjData.FileName + " Uploaded To Archive Location " + ObjData.Bank_Insta_Archive + ObjData.FileName,IssuerNo, 1);
                                }

                                sftpClient.DeleteFile(sftpClient.WorkingDirectory + ObjData.SourcePath + ObjData.FileName);
                                new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", "File  with Name:" + sftpClient.WorkingDirectory + ObjData.SourcePath + ObjData.FileName + " is get deleted", IssuerNo, 1);
                            }
                        }


                    }
                    sftpClient.Dispose();
                    
                    new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", "SFTP CONNECTION COMPLETED!", IssuerNo, 1);

                }


            }


            catch (Exception Ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog("DownloadFile", "", ObjData.processid, "", Ex.ToString(), IssuerNo,0);
                return false;
            }
            return true;
        }

        
    }
}
