using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCardsFileGeneration
{
    public class ConfigDataObject
    {
        public string IssuerNo { get; set; }
        //public string RequestId { get; set; }
        public string ProcessId { get; set; }
        public string FileUploader_SP { get; set; }
        public string Bank_SFTPServer { get; set; }
        public string Bank_SFTPUser { get; set; }
        public string Bank_SFTPPassword { get; set; }
        public string Bank_SFTPPort { get; set; }
        public string Bank_Input { get; set; }
        //public string Bank_Output { get; set; }
        //public string Bank_Archive { get; set; }
        //public string Bank_Failed { get; set; }
        //public string Vendor_SFTPServer { get; set; }
        //public string Vendor_SFTPUser { get; set; }
        //public string Vendor_SFTPPassword { get; set; }
        //public string Vendor_SFTPPort { get; set; }
        //public string Vendor_Output { get; set; }
        public string Local_Input { get; set; }
        //public string Local_Output { get; set; }
        //public string Local_Archive { get; set; }
        //public string Local_Failed { get; set; }
        //public string ErrorLogPath { get; set; }
        public bool StepStatus { get; set; }
        public string FileExtension { get; set; }
        public string FileHeader { get; set; }
        public string FileID { get; set; }
        public string FileName { get; set; }
        public string Filestatus { get; set; }
        public int TotalCount { get; set; }
        public string ErrorDesc { get; set; }
        //issurer no for bank
        public string NoOfCards { get; set; }
        public string AccountType { get; set; }
        public string Cardprefix { get; set; }
        public string LastCustomerID { get; set; }
        public string LastAccountNo { get; set; }
        public string BankName { get; set; }
        public string id { get; set; }
        public string Reason { get; set; }
        


        //public string APIRequestType { get; set; }
        //public string CardAPIURL { get; set; }
        //public string SourceID { get; set; }
        //public bool IsNewCardGenerate { get; set; }
        public string SSH_Private_key_file_Path { get; set; }
        public string passphrase { get; set; }
        //public string PREPGPPublicKeyPath { get; set; }
        //public string InputFileSecretKeyPath { get; set; }
        //public Boolean IsPGP { get; set; }
        //public string InputFilePGPPassWord { get; set; }
        //public string PGP_KeyName { get; set; }
        //public string StatusFilePPGPPublicKeyPath { get; set; }
        //public Boolean isstatusFileINPGP { get; set; }
        //public Boolean isSplitBankFile { get; set; }
        //public string Insta_Reference_No { get; set; }
        


    }
    
    public class ClsGeneratePrepaidCardBO
    {
        #region variables for generateprepaidcard
        public string BankName { get; set; }
        //issurer no for bank
        public string NoOfCards { get; set; }
        public string AccountType { get; set; }
        public string CardProgram { get; set; }
        #endregion
    }

    public class SFTPDataObject
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
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
        public string Bank_Insta_Output { get; set; }
        public string Bank_Insta_Archive { get; set; }
        public string Bank_Insta_Failed { get; set; }
        public string Local_Failed { get; set; }
        public string Local_Archive { get; set; }
        public string processid { get; set; }
        public string FileHeader { get; set; }
        public Int32 MaxHeaderCount { get; set; }
        public string FileID { get; set; }
        public string FileUploader_SP { get; set; }

        public string Local_Insta_Status_file { get; set; }





    }
}
