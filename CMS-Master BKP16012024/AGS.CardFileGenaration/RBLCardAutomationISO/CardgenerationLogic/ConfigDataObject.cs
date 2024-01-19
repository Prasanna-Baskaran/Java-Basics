using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.RBLCardAutomationISO
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
        public string SSH_Private_key_file_Path { get; set; }
        public string passphrase { get; set; }
        public string PREPGPPublicKeyPath { get; set; }
        public string InputFileSecretKeyPath { get; set; }
        public Boolean IsPGP { get; set; }
        public string InputFilePGPPassWord { get; set; }
        public string PGP_KeyName { get; set; }
        public string StatusFilePPGPPublicKeyPath { get; set; }
        public Boolean isstatusFileINPGP { get; set; }

        public Boolean isStandardFourRun { get; set; }  ///*Added for international usage RBL-ATPCM-862* START/

        public Int32 ISOCallCounter { get; set; }///*RBL autojob three times ISO call for 108 switch response ATPBF-1034 */
        public Int32 SwitchRespID { get; set; } ///*RBL autojob three times ISO call for 108 switch responseATPBF-1034 */

        public bool Trace { get; set; }
        public int RetryCnt { get; set; }
        public string ISODataTemp { get; set; }
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

    //RBL CARD GENERATION OBJECT
    public class APIResponseObject
    {
        public string CardNo { get; set; }
        public string AccountNo { get; set; }
        public string EncAccountNo { get; set; }
        public string EncCardNo { get; set; }
        public string Status { get; set; }
        public string NewRspCode { get; set; }
        public string StatusDescription { get; set; }
        public string ResponseMessage { get; set; }
    }
}
