using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    public class CardRePIN
    {
        public void Process()
        {
            
            EmailDataObject Eobj = LoadEmailObject();
            try
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :Card Repin dll started", System.Reflection.MethodBase.GetCurrentMethod().Name);
                DataTable _Configuration = new ModuleDAL().GetConfiguration();    
                if (_Configuration.Rows.Count > 0)
                {
                    foreach (DataRow dr in _Configuration.Rows)
                    {
                        CardRePINDataObject obj = new CardRePINDataObject();
                        
                        obj.Bankid = Convert.ToInt32(dr["bankid"]);
                        obj.issuerNr = Convert.ToInt32(dr["issuerNr"]);
                        obj.ServerIP = Convert.ToString(dr["ServerIP"]);
                        obj.port = Convert.ToInt32(dr["Serverport"]);
                        obj.UserName = Convert.ToString(dr["UserName"]);
                        obj.Passphrase = Convert.ToString(dr["Passphrase"]);
                        obj.Password = Convert.ToString(dr["Password"]);
                        obj.KeyPath = Convert.ToString(dr["keyPath"]);
                        obj.InputPath = Convert.ToString(dr["FilePathInput_RePIN"]);
                        obj.OutPutPath = Convert.ToString(dr["FilePathOutput_RePIN"]);
                        obj.ArchivePath = Convert.ToString(dr["FilePathArchive_RePIN"]);
                        obj.FilePath = Convert.ToString(dr["FilePath_RePIN"]);
                        obj.FileHeader = Convert.ToString(dr["fileHeader_RePIN"]);
                        obj.ISPGP = Convert.ToBoolean(dr["isPGP"]);
                        obj.Trace = Convert.ToBoolean(dr["Trace"]);
                        obj.DecInputFilePath = Convert.ToString(dr["InputFilePath_PGP"]);
                        obj.DecOutputFilePath = obj.FilePath;
                        obj.EncInputFilePath = obj.FilePath;
                        obj.EncOutputFilePath = Convert.ToString(dr["InputFilePath_PGP"]);
                        obj.PublicKeyFilePath = Convert.ToString(dr["PublicKeyFilePath"]);
                        obj.PrivateKeyFilePath = Convert.ToString(dr["PrivateKeyFilePath"]);
                        obj.Password_PGP = Convert.ToString(dr["Password_PGP"]);                             
                        bool isfileProcessed = false;
                        ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :SFTP Conncetion Started to retrive the File ", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        
                        if (FileMove(obj, "", true,obj.ArchivePath,Eobj))
                        {
                            
                            
                            if(obj.ISPGP)
                            {
                               
                                Decrypt(obj);
                            }
                            isfileProcessed = RequestProcesser.process(obj, Eobj);
                            
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage("", Eobj);
            }

        }
        public static bool FileMove(CardRePINDataObject obj, string fileName, bool isdownload, string outputPath,EmailDataObject Eobj)
        {
            bool isSFTPsuccess = false;
            if (string.IsNullOrEmpty(obj.KeyPath) && string.IsNullOrEmpty(obj.Passphrase))
            {
                isSFTPsuccess = SFTPConnection.__SFTPConncetionwithoutKey(obj.ServerIP, obj.UserName, obj.Password, obj.port, outputPath, obj.InputPath, (obj.ISPGP?obj.DecInputFilePath:obj.FilePath), isdownload, fileName,Eobj);
            }
            else
            {
                isSFTPsuccess = SFTPConnection.__SFTPConncetionwithKey(obj.ServerIP, obj.UserName, obj.Password, obj.port, outputPath, obj.InputPath,(obj.ISPGP ? obj.DecInputFilePath : obj.FilePath), isdownload, fileName,obj.KeyPath, obj.Passphrase,Eobj);
            }
            return isSFTPsuccess;
        }
        public static void Encrypt(CardRePINDataObject model)
        {
                
           PGPEncryptDecrypt.EncryptFile(model.EncInputFilePath, model.EncOutputFilePath, model.PublicKeyFilePath, true, true);
        }
        public static void Decrypt(CardRePINDataObject model)
        {
            String[] _files = Directory.GetFiles(model.DecInputFilePath);
            foreach (var item in _files)
            {
                PGPEncryptDecrypt.Decrypt(item.ToString(), model.PrivateKeyFilePath, model.Password_PGP, model.DecOutputFilePath+Path.GetFileName(item)+".txt");
                File.Delete(item.ToString());
            }

        }
        public static EmailDataObject LoadEmailObject()
        {
            EmailDataObject obj = new EmailDataObject();
            DataTable Email = new ModuleDAL().GetEmailConfiguration();
            obj.SMTPCLIENT = Convert.ToString(Email.Rows[0]["SMTPCLIENT"]);
            obj.EmailBCC = Convert.ToString(Email.Rows[0]["EmailBCC"]);
            obj.EmailFrom = Convert.ToString(Email.Rows[0]["EmailFrom"]);
            obj.EmailMsg = Convert.ToString(Email.Rows[0]["EmailMsg"]);
            obj.EmailPassWord = Convert.ToString(Email.Rows[0]["EmailPassWord"]);
            obj.EmailPort = Convert.ToInt32(Email.Rows[0]["EmailPort"]);
            obj.EmailTo = Convert.ToString(Email.Rows[0]["EmailTo"]);
            obj.EmailUserName = Convert.ToString(Email.Rows[0]["EmailUserName"]);
            return obj;
                      
        }

        
    }
}
