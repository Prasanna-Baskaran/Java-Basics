using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Renci.SshNet;
using AGS.SwitchOperations.Common;
using System.IO;
using System.Data;


namespace AGS.SwitchOperations
{
    public class ClsCommonMethods
    {
        public static bool UploadFile(SFTPDetails ObjData)
        {
            //AysncLog _AysncLog = new AysncLog();
            try
            {
                //_AysncLog.ProcessWrite("SFTP CONNECTION STARTED TO UPLOAD FILES TO SFTP!");
                var ServerIP = ObjData.Bank_SFTPServer;

                //Below code is for SSH encryption start
                var methods = new List<AuthenticationMethod>();
                methods.Add(new PasswordAuthenticationMethod(ObjData.Bank_SFTPUser, ObjData.Bank_SFTPPassword));

                if (!string.IsNullOrEmpty(ObjData.DestServerSSH_Private_key_file_Path))
                {
                    var keyFile = new PrivateKeyFile(ObjData.DestServerSSH_Private_key_file_Path, ObjData.DestServerpassphrase);
                    var keyFiles = new[] { keyFile };
                    methods.Add(new PrivateKeyAuthenticationMethod(ObjData.Bank_SFTPUser, keyFiles));
                }
                //_AysncLog.ProcessWrite("SFTP Details to Upload FILES. ServerIP:" + ServerIP + " ,SourceServerPort:" + Convert.ToString(ObjData.DestServerPort) + " ,SourceUsername:" + ObjData.DestServerUsername + " ,SourceUsernamePw:" + ObjData.DestServerPassword);
                var con = new ConnectionInfo(ServerIP, Convert.ToInt32(ObjData.Bank_SFTPPort), ObjData.Bank_SFTPUser, methods.ToArray());
                using (var sftpClient = new SftpClient(con))
                {
                    sftpClient.Connect();
                    if (sftpClient.IsConnected)
                    {
                        //_AysncLog.ProcessWrite("SFTP Server Connected TO UPLOAD FILES ON SFTP!");
                        string WorkingDirectory = sftpClient.WorkingDirectory;
                        //_AysncLog.ProcessWrite("Checking files on local server path:" + ObjData.LocalInputPath + " TO UPLOAD FILES ON SFTP!");
                        var files = Directory.GetFiles(ObjData.SwitchPortal_Local_Input);
                        foreach (var file in files)
                        {
                            ObjData.FileName = Path.GetFileName(file);
                            //_AysncLog.ProcessWrite("File Found with Name:" + ObjData.FileName + " TO UPLOAD FILES ON SFTP!");

                            /*To Upload the file for the SFTP if the file is downloaded*/
                            if (File.Exists(ObjData.SwitchPortal_Local_Input + ObjData.FileName))
                            {
                                var file1 = new FileStream(ObjData.SwitchPortal_Local_Input + ObjData.FileName, FileMode.Open);
                                {
                                    sftpClient.UploadFile(file1, ObjData.Bank_Input + ObjData.FileName, null);
                                    //_AysncLog.ProcessWrite("File Uploaded on SFTP server Path:" + ObjData.DestinationPath + ObjData.FileName);

                                    file1.Flush();
                                    file1.Close();
                                    file1.Dispose();
                                    File.Move(ObjData.SwitchPortal_Local_Input + ObjData.FileName, ObjData.SwitchPortal_Local_Archive + ObjData.FileName);
                                    //_AysncLog.ProcessWrite("File Moved from Local server Path:" + ObjData.LocalInputPath + ObjData.FileName + " to " + ObjData.LocalArchivePath + ObjData.FileName);
                                }
                            }
                        }
                        sftpClient.Dispose();
                        //_AysncLog.ProcessWrite("SFTP server connection Closed for Upload FILES");
                    }
                }
            }
            catch (Exception Ex)
            {
                //_AysncLog.ProcessWrite("Upload File Error:" + Ex.ToString());
                //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, fileid, processid, Ex.ToString(), "", IssuerNo.ToString(), 0);
                return false;
            }
            return true;
        }

        public static T BindDatatableToClass<T>(DataTable dtTable)
        {
            var ob = Activator.CreateInstance<T>();
            try
            {
                DataRow dr = dtTable.Rows[0];
                // Get all columns' name
                List<string> columns = new List<string>();
                foreach (DataColumn dc in dtTable.Columns)
                {
                    columns.Add(dc.ColumnName);
                }
                
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
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        try
                        {
                            object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                            propertyInfo.SetValue(ob, safeValue, null);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return ob;
        }
    }
}