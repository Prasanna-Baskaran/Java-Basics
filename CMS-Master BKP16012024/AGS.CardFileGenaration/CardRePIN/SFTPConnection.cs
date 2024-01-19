using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    class SFTPConnection
    {
        public static bool __SFTPConncetionwithKey(String SourcePath, String UserName, String PassWord, int Port, String SFTPDirectoryMove, string SFTPDirectory, string DownloadPath, bool isdownload, String FileName, string key, string passphrase,EmailDataObject Eobj)
        {
            string remoteFileName = FileName;
            bool _return = true;

            try
            {
                var keyFile = new PrivateKeyFile(key, passphrase);
                var keyFiles = new[] { keyFile };
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(UserName, PassWord));
                methods.Add(new PrivateKeyAuthenticationMethod(UserName, keyFiles));
                var con = new ConnectionInfo(SourcePath, Port, UserName, methods.ToArray());
                using (var sftpClient = new SftpClient(con))
                {
                    sftpClient.Connect();

                    if (sftpClient.IsConnected)
                    {
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        /*NEED TO OPTIMIZED THE BELOW FUNCTION FOR THE SFTP UPLOAD*/
                        // RESULT: SFTP working directory = /customers/5/7/9/domain.com/httpd.www <- NOTE httpd.www

                        var files = sftpClient.ListDirectory(WorkingDirectory + SFTPDirectory);
                        foreach (var file in files)
                        {

                            if (isdownload)
                            {
                               
                                if (!(file.Name.StartsWith(".")) && !(file.Name.StartsWith("..")))
                                { 
                                    remoteFileName = file.Name;
                                    

                                    /*To download the file for the SFTP*/
                                    using (Stream file1 = File.OpenWrite(DownloadPath + remoteFileName))
                                    {
                                        sftpClient.DownloadFile(sftpClient.WorkingDirectory + SFTPDirectory + remoteFileName, file1);
                                        file1.Flush();
                                        file1.Close();
                                        file1.Dispose();
                                        ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + remoteFileName + " File Download Suceesfully", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        
                                    }

                                }
                                
                            }
                            /*To Upload the file for the SFTP if the file is downloaded*/
                            if (File.Exists(DownloadPath + remoteFileName))
                            {
                                var file1 = new FileStream(DownloadPath + remoteFileName, FileMode.Open);
                                //using (Stream file1 = File.OpenWrite(ConfigurationManager.AppSettings["downloadFilePath"] + remoteFileName))
                                {
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "Uploading the file to sftp");
                                    sftpClient.UploadFile(file1, sftpClient.WorkingDirectory + SFTPDirectoryMove + remoteFileName, null);
                                    ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + remoteFileName + " File Uploaded Suceesfully", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                    
                                    /*To DELETE the file for the SFTP if the file successfully upload*/
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "Delete File FROM Sftp");
                                    if (isdownload)
                                    {
                                        sftpClient.DeleteFile(sftpClient.WorkingDirectory + SFTPDirectory + remoteFileName);
                                    }
                                    remoteFileName = "";
                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "File Deleted Suceesfully");
                                }
                            }
                        }


                        sftpClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage(remoteFileName, Eobj);
                _return = false;
            }
            return _return;
        }
        public static bool __SFTPConncetionwithoutKey(String SourcePath, String UserName, String PassWord, int Port, String SFTPDirectoryMove, string SFTPDirectory, string DownloadPath, bool isdownload, String FileName,EmailDataObject Eobj)
        {
            string remoteFileName = FileName;
            bool _return = true;

            try
            {

                using (var sftpClient = new SftpClient(SourcePath, Port, UserName, PassWord))
                {
                    sftpClient.Connect();

                    if (sftpClient.IsConnected)
                    {
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        /*NEED TO OPTIMIZED THE BELOW FUNCTION FOR THE SFTP UPLOAD*/
                        // RESULT: SFTP working directory = /customers/5/7/9/domain.com/httpd.www <- NOTE httpd.www

                        var files = sftpClient.ListDirectory(WorkingDirectory + SFTPDirectory);
                        foreach (var file in files)
                        {

                            if (isdownload)
                            {
                                if (!(file.Name.StartsWith(".")) && !(file.Name.StartsWith("..")))
                                { 
                                    remoteFileName = file.Name;
                                    

                                    /*To download the file for the SFTP*/
                                    using (Stream file1 = File.OpenWrite(DownloadPath + remoteFileName))
                                    {
                                        sftpClient.DownloadFile(sftpClient.WorkingDirectory + SFTPDirectory + remoteFileName, file1);
                                        file1.Flush();
                                        file1.Close();
                                        file1.Dispose();
                                        ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + remoteFileName + " File Download Suceesfully", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        
                                    }

                                }
                                
                            }
                            /*To Upload the file for the SFTP if the file is downloaded*/
                            if (File.Exists(DownloadPath + remoteFileName))
                            {
                                var file1 = new FileStream(DownloadPath + remoteFileName, FileMode.Open);
                                //using (Stream file1 = File.OpenWrite(ConfigurationManager.AppSettings["downloadFilePath"] + remoteFileName))
                                {
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "Uploading the file to sftp");
                                    sftpClient.UploadFile(file1, sftpClient.WorkingDirectory + SFTPDirectoryMove + remoteFileName, null);
                                    ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + remoteFileName + " File Uploaded Suceesfully", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                    
                                    /*To DELETE the file for the SFTP if the file successfully upload*/
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "Delete File FROM Sftp");
                                    if (isdownload)
                                    {
                                        sftpClient.DeleteFile(sftpClient.WorkingDirectory + SFTPDirectory + remoteFileName);
                                    }
                                    remoteFileName = "";
                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    //Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + "File Deleted Suceesfully");
                                }
                            }
                        }


                        sftpClient.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " +ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage(remoteFileName, Eobj);
                _return = false;
            }
            return _return;
        }
    }
}
