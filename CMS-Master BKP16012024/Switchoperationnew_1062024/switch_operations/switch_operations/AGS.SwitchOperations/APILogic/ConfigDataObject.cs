using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations
{
    public class ConfigDataObject
    {
        public string IssuerNo { get; set; }
        public string RequestId { get; set; }
        public string ErrorDesc { get; set; }
        public string APIRequestType { get; set; }
        public string CardAPIURL { get; set; }
        public string SourceID { get; set; }
        public bool IsCardDetailsSearch { get; set; }
        public bool IsAccountDetailsSearch { get; set; }
        public bool IsCustomerDetailsSearch { get; set; }

        public int LoginId { get; set; }

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
    }
}
