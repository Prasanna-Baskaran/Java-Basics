using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLinking
{
    public class ConfigDataObject
    {
        public string IssuerNo { get; set; }
        public string RequestId { get; set; }
        public string ProcessId { get; set; }
        public string FileUploader_SP { get; set; }
        public string FileValidator_SP { get; set; }
        public string FileProcessor_SP { get; set; }
        public string AGS_SFTPServer { get; set; }
        public string AGS_SFTPUser { get; set; }
        public string AGS_SFTPPassword { get; set; }
        public string AGS_SFTPPort { get; set; }
        public string AGS_Input { get; set; }
        public string AGS_Output { get; set; }
        public string Bank_SFTPServer { get; set; }
        public string Bank_SFTPUser { get; set; }
        public string Bank_SFTPPassword { get; set; }
        public string Bank_SFTPPort { get; set; }
        public string Bank_Input { get; set; }
        public string Bank_Output { get; set; }
        public string Bank_Archive { get; set; }
        public string Bank_Failed { get; set; }
        public string Vendor_SFTPServer { get; set; }
        public string Vendor_SFTPUser { get; set; }
        public string Vendor_SFTPPassword { get; set; }
        public string Vendor_SFTPPort { get; set; }
        public string Vendor_Output { get; set; }
        public string Local_Input { get; set; }
        public string Local_Output { get; set; }
        public string Local_Archive { get; set; }
        public string Local_Failed { get; set; }
        public string ErrorLogPath { get; set; }
        public string PGPPublicKeyPath { get; set; }
        public string PGPSecretKeyPath { get; set; }
        public string IsPGP { get; set; }
        public string PGPPassword { get; set; }
        public string PGP_KeyName { get; set; }
        public bool   StepStatus { get; set; }
        public string FileExtension { get; set; }
        public string FileHeader { get; set; }
        public string FileID { get; set; }
        public string FileName { get; set; }
        public string Filestatus { get; set; }
        public string ErrorDesc { get; set; }
        public string APIRequestType { get; set; }
        public string CardAPIURL { get; set; }
        public string SourceID { get; set; }
        public bool IsNewCardGenerate { get; set; }

        
      

    }
   public class SFTPDataObject
    {
        public string ServerIP { get; set; }
        public int    ServerPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SourcePath { get; set; }
        public string destinationPath { get; set; }
        public string SSH_Private_key_file_Path { get; set; }
        public string passphrase { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Deletebasefilepath { get; set; } //USED FOR DELETE THE FILE FROM SFTP
        public string ErrorlogPath { get; set; } //USED FOR DELETE THE FILE FROM SFTP
    }
}
