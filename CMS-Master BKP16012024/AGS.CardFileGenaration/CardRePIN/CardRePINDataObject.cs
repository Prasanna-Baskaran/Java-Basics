using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    
        public class CardRePINDataObject
        {
            public int Bankid { get; set; }
            public int issuerNr { get; set; }
            public string ServerIP { get; set; }
            public int port { get; set; }
            public string InputPath { get; set; }
            public string OutPutPath { get; set; }
            public string ArchivePath { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string KeyPath { get; set; }
            public string FilePath { get; set; }
            public string Passphrase { get; set; }
            public string FileHeader { get; set; }
            public bool ISPGP { get; set; }
            public bool Trace { get; set; }
            public string FileName { get; set; }
            public string UserName_PGP { get; set; }
            public string Password_PGP { get; set; }
            public string DirectoryPath { get; set; }
            public string EncInputFilePath { get; set; }
            public string EncOutputFilePath { get; set; }
            public string DecInputFilePath { get; set; }
            public string DecOutputFilePath { get; set; }
            public string PublicKeyFilePath { get; set; }
            public string PrivateKeyFilePath { get; set; }
            public string mode { get; set; }
        }
        class RePINISOObject
        {

            internal string cardNo { get; set; }
            internal string CIFID { get; set; }
            internal string AccountNo { get; set; }
            internal string stan { get; set; }
            internal string proessingcode { get; set; }
            internal string IssuerNo { get; set; }
            internal string CODE { get; set; }
            internal bool trace { get; set; }
            internal string ServerIP { get; set; }
            internal int Port { get; set; }
            internal string FileName { get; set; }
            
        }
        public class EmailDataObject
        {
          public  string SMTPCLIENT { get; set; }
          public string EmailFrom { get; set; }
          public int EmailPort { get; set; }
          public string EmailUserName { get; set; }
          public string EmailPassWord { get; set; }
          public string EmailTo { get; set; }
          public string EmailBCC { get; set; }
          public string EmailMsg { get; set; }
          
        }
     
}
