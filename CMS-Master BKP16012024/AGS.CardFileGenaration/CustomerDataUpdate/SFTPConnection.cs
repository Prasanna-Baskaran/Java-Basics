using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomerDataUpdate
{
    internal class SFTPConnection
    {
        public static bool __SFTPConncetionwithKey(string SourcePath, string UserName, string PassWord, int Port, string SFTPDirectoryMove, string SFTPDirectory, string DownloadPath, bool isdownload, string FileName, string key, string passphrase, EmailDataObject Eobj, string FileExtension)
        {
            string text = FileName;
            bool result = true;
            try
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP ConnectionStarted", System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModuleDAL.InsertLog(string.Concat(new string[]
                {
                    System.DateTime.Now.ToString(),
                    ">> Message # :  SFTP key:",
                    key,
                    "|",
                    passphrase
                }), System.Reflection.MethodBase.GetCurrentMethod().Name);
                PrivateKeyFile privateKeyFile = new PrivateKeyFile(key, passphrase);
                PrivateKeyFile[] array = new PrivateKeyFile[]
                {
                    privateKeyFile
                };
                ConnectionInfo connectionInfo = new ConnectionInfo(SourcePath, Port, UserName, new System.Collections.Generic.List<AuthenticationMethod>
                {
                    new PasswordAuthenticationMethod(UserName, PassWord),
                    new PrivateKeyAuthenticationMethod(UserName, array)
                }.ToArray());
                using (SftpClient sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP Connectced", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        string workingDirectory = sftpClient.WorkingDirectory;
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP INPUT FIle Path|" + SFTPDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        System.Collections.Generic.IEnumerable<SftpFile> enumerable = sftpClient.ListDirectory(workingDirectory + SFTPDirectory, null);
                        foreach (SftpFile current in enumerable)
                        {
                            ModuleDAL.InsertLog(string.Concat(new string[]
                            {
                                System.DateTime.Now.ToString(),
                                ">> Message # : Some file found|",
                                SFTPDirectory,
                                " |isdownload:",
                                isdownload.ToString()
                            }), System.Reflection.MethodBase.GetCurrentMethod().Name);
                            if (isdownload)
                            {
                                if (current.Name.EndsWith(FileExtension, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    text = current.Name;
                                    ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Local Server  Started|" + DownloadPath + FileName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                    using (System.IO.Stream stream = System.IO.File.OpenWrite(DownloadPath + text))
                                    {
                                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Download Started|" + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        sftpClient.DownloadFile(sftpClient.WorkingDirectory + SFTPDirectory + text, stream, null);
                                        stream.Flush();
                                        stream.Close();
                                        stream.Dispose();
                                    }
                                }
                            }
                            if (System.IO.File.Exists(DownloadPath + text))
                            {
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Exists|" + DownloadPath + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                System.IO.FileStream fileStream = new System.IO.FileStream(DownloadPath + text, System.IO.FileMode.Open);
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Upload Path|" + SFTPDirectoryMove, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                sftpClient.UploadFile(fileStream, sftpClient.WorkingDirectory + SFTPDirectoryMove + text, null);
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Uploaded.|" + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                if (isdownload)
                                {
                                    sftpClient.DeleteFile(sftpClient.WorkingDirectory + SFTPDirectory + text);
                                }
                                text = "";
                                fileStream.Flush();
                                fileStream.Close();
                                fileStream.Dispose();
                            }
                        }
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : SFTP Connection Closed..", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        sftpClient.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                result = false;
            }
            return result;
        }

        public static bool UploadErrorFile(string SourcePath, string UserName, string PassWord, int Port, string ReadFilePath, string UploadPath, string CurrentFileName, string SHH_KEY_Path, string passpharse)
        {
            bool result;
            try
            {
                System.Collections.Generic.List<AuthenticationMethod> list = new System.Collections.Generic.List<AuthenticationMethod>();
                list.Add(new PasswordAuthenticationMethod(UserName, PassWord));
                if (!string.IsNullOrEmpty(SHH_KEY_Path))
                {
                    PrivateKeyFile privateKeyFile = new PrivateKeyFile(SHH_KEY_Path, passpharse);
                    PrivateKeyFile[] array = new PrivateKeyFile[]
                    {
                        privateKeyFile
                    };
                    list.Add(new PrivateKeyAuthenticationMethod(UserName, array));
                }
                ConnectionInfo connectionInfo = new ConnectionInfo(SourcePath, System.Convert.ToInt32(Port), UserName, list.ToArray());
                using (SftpClient sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(ReadFilePath, System.IO.FileMode.Open))
                        {
                            //sftpClient.BufferSize = 1024;
                            sftpClient.UploadFile(fileStream, UploadPath + CurrentFileName + "_Error.txt", null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # UploadErrorFile:  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        public static bool __SFTPConncetionwithoutKey(string SourcePath, string UserName, string PassWord, int Port, string SFTPDirectoryMove, string SFTPDirectory, string DownloadPath, bool isdownload, string FileName, EmailDataObject Eobj, string FileExtension)
        {
            string text = FileName;
            bool result = true;
            try
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP ConnectionStarted", System.Reflection.MethodBase.GetCurrentMethod().Name);
                using (SftpClient sftpClient = new SftpClient(SourcePath, Port, UserName, PassWord))
                {
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP Connectced", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        string workingDirectory = sftpClient.WorkingDirectory;
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  SFTP INPUT FIle Path|" + SFTPDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        System.Collections.Generic.IEnumerable<SftpFile> enumerable = sftpClient.ListDirectory(workingDirectory + SFTPDirectory, null);
                        foreach (SftpFile current in enumerable)
                        {
                            if (isdownload)
                            {
                                if (current.Name.EndsWith(FileExtension, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    text = current.Name;
                                    ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Found With Name|" + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                    using (System.IO.Stream stream = System.IO.File.OpenWrite(DownloadPath + text))
                                    {
                                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Download Started|" + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        sftpClient.DownloadFile(sftpClient.WorkingDirectory + SFTPDirectory + text, stream, null);
                                        stream.Flush();
                                        stream.Close();
                                        stream.Dispose();
                                    }
                                }
                            }
                            if (System.IO.File.Exists(DownloadPath + text))
                            {
                                System.IO.FileStream fileStream = new System.IO.FileStream(DownloadPath + text, System.IO.FileMode.Open);
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Upload Started|" + text, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                sftpClient.UploadFile(fileStream, sftpClient.WorkingDirectory + SFTPDirectoryMove + text, null);
                                if (isdownload)
                                {
                                    sftpClient.DeleteFile(sftpClient.WorkingDirectory + SFTPDirectory + text);
                                }
                                text = "";
                                fileStream.Flush();
                                fileStream.Close();
                                fileStream.Dispose();
                            }
                        }
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : SFTP END", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        sftpClient.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                result = false;
            }
            return result;
        }

        public static bool UploadFile(string SourcePath, string UserName, string PassWord, int Port, string ReadFilePath, string UploadPath, string CurrentFileName, string SHH_KEY_Path, string passpharse, string extension)
        {
            bool result;
            try
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : SFTP CONNECTION STARTED TO UPLOAD FILES TO SFTP", System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModuleDAL.InsertLog(string.Concat(new string[]
                {
                    System.DateTime.Now.ToString(),
                    ">> Message #",
                    SourcePath,
                    "|",
                    UserName,
                    "|",
                    PassWord,
                    "|",
                    ReadFilePath,
                    "|",
                    UploadPath,
                    "|",
                    CurrentFileName,
                    "|",
                    SHH_KEY_Path,
                    "+",
                    passpharse,
                    "|",
                    extension
                }), System.Reflection.MethodBase.GetCurrentMethod().Name);
                System.Collections.Generic.List<AuthenticationMethod> list = new System.Collections.Generic.List<AuthenticationMethod>();
                list.Add(new PasswordAuthenticationMethod(UserName, PassWord));
                if (!string.IsNullOrEmpty(SHH_KEY_Path))
                {
                    PrivateKeyFile privateKeyFile = new PrivateKeyFile(SHH_KEY_Path, passpharse);
                    PrivateKeyFile[] array = new PrivateKeyFile[]
                    {
                        privateKeyFile
                    };
                    list.Add(new PrivateKeyAuthenticationMethod(UserName, array));
                }
                ConnectionInfo connectionInfo = new ConnectionInfo(SourcePath, System.Convert.ToInt32(Port), UserName, list.ToArray());
                using (SftpClient sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 2, 0);
                    sftpClient.KeepAliveInterval = new TimeSpan(0, 2, 0);
                    sftpClient.OperationTimeout = new TimeSpan(0, 4, 0);
                    sftpClient.BufferSize = 5000000;
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : SFTP CONNECTED", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        string workingDirectory = sftpClient.WorkingDirectory;
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : SFTP Connected!" + workingDirectory, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        string[] files = System.IO.Directory.GetFiles(ReadFilePath, "*" + extension, System.IO.SearchOption.TopDirectoryOnly);
                        string[] array2 = files;
                        for (int i = 0; i < array2.Length; i++)
                        {
                            string text = array2[i];
                            ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # File Found with Name!" + CurrentFileName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                            if (System.IO.File.Exists(ReadFilePath + CurrentFileName))
                            {
                                System.IO.FileStream fileStream = new System.IO.FileStream(ReadFilePath + CurrentFileName, System.IO.FileMode.Open);
                                sftpClient.UploadFile(fileStream, UploadPath + CurrentFileName, null);
                                fileStream.Flush();
                                fileStream.Close();
                                fileStream.Dispose();
                                ModuleDAL.InsertLog(string.Concat(new string[]
                                {
                                    System.DateTime.Now.ToString(),
                                    "File  with Name:",
                                    CurrentFileName,
                                    " Uploaded To Location ",
                                    UploadPath,
                                    CurrentFileName
                                }), System.Reflection.MethodBase.GetCurrentMethod().Name);
                            }
                        }
                        sftpClient.Dispose();
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + "SFTP CONNECTION COMPLETED:", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                result = false;
                return result;
            }
            result = true;
            return result;
        }
    }
}
