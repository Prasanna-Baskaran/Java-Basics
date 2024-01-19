﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileSplit
{
    public class ReadSFTPFile
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public void Process(ConfigDataObject ObjData)
        {
            try
            {


                SFTPDataObject objSFTP = new SFTPDataObject();
                objSFTP.ServerIP = ObjData.Bank_SFTPServer;
                objSFTP.ServerPort = Convert.ToInt32(ObjData.Bank_SFTPPort);
                objSFTP.Username = ObjData.Bank_SFTPUser;
                objSFTP.Password = ObjData.Bank_SFTPPassword;
                objSFTP.SSH_Private_key_file_Path = ObjData.SSH_Private_key_file_Path;
                objSFTP.passphrase = ObjData.passphrase;
                ModuleDAL.FunInsertIntoErrorLog("ReadSFTPFile.Process", ObjData.FileID, ObjData.ProcessId.ToString(),"", "SSH KEY and Passpharse|"+objSFTP.SSH_Private_key_file_Path+"|"+objSFTP.passphrase, ObjData.IssuerNo.ToString(), 1);
                objSFTP.SourcePath = ObjData.Bank_Input;
                objSFTP.destinationPath = ObjData.Local_Input + "\\";//+"\\"+ ObjData.IssuerNo+"\\" + ObjData.ProcessId+"\\";
                objSFTP.FileExtension = ObjData.FileExtension;
                objSFTP.ErrorlogPath = ObjData.ErrorLogPath;
                /*DOWNLOAD THE FILE FROM SFTP AND PLACE IT  ON LOCAL SERVER */

                if (SearchFile.DownloadFile(objSFTP,Convert.ToInt32(ObjData.IssuerNo),ObjData.ProcessId,ObjData.FileID))
                {
                    /*Upload The File from Local server To SFTP*/
                    objSFTP.SourcePath = ObjData.Local_Input + "\\";// + "\\" + ObjData.IssuerNo + "\\" + ObjData.ProcessId + "\\";
                    objSFTP.destinationPath = ObjData.Bank_Archive;
                    objSFTP.Deletebasefilepath = ObjData.Bank_Input;
                    objSFTP.FileName ="";

                    if (!SearchFile.UploadFile(objSFTP, Convert.ToInt32(ObjData.IssuerNo),ObjData.ProcessId,ObjData.FileID))
                    {
                        ObjData.StepStatus = true;
                        return;
                    }
                }
                else
                {
                    ObjData.StepStatus = true;
                     return;
                }
                ObjData.StepStatus = false;
            }
            catch (Exception Ex)
            {
                ModuleDAL.FunInsertIntoErrorLog("ReadSFTPFile.Process", ObjData.FileID, ObjData.ProcessId.ToString(), Ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.StepStatus = true;
                ObjData.ErrorDesc = "ERROR WHILE SFTP CONNECTION|" + Ex.ToString();
            }

        }
    }
}
