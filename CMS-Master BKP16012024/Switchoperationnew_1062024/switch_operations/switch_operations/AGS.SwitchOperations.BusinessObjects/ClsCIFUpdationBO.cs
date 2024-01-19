using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCIFUpdationBO
    {
        #region Class for Properties required for CIF data updation for bank
        public int IssuerNo { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string FilePathInput { get; set; }
        public string FilePathOutPut { get; set; }
        public string FilePathArchive { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
        public Boolean EnableState { get; set; }
        //public string EnableState { get; set; }
        public string filepath { get; set; }
        public string FileHeader { get; set; }
        public string FilePathInput_RePIN { get; set; }
        public string FilePathOutPut_RePIN { get; set; }
        public string FilePathArchive_RePIN { get; set; }
        public string FilePath_RePIN { get; set; }
        public string fileHeader_RePIN { get; set; }
        public Boolean IsPGP { get; set; }
        //public string IsPGP { get; set; }
        public Boolean Trace { get; set; }
        //public string Trace { get; set; }
        public string PublicKeyFilePath { get; set; }
        public string PrivateKeyFilePath { get; set; }
        public string Password_PGP { get; set; }
        public string InputFilePath_PGP { get; set; }

        //public int IssuerNosave { get; set; }
#endregion
    }
}
