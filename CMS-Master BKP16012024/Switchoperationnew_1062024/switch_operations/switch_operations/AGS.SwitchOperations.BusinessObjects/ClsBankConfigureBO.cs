using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
   
    
    public class ClsBankConfigureBO
    {
        #region Class for properties required for bank and bin configuration
        //tblbank param
        public string BankName { get; set; }
        //issurer no for bank
     public string IssuerNo { get; set; }
     public string SystemID { get; set; }
     public string SourceNodes { get; set; }
     public string SinkNodes { get; set; }
     public string UserPrefix { get; set; }
     public string CustIdentity { get; set; }
     public string CustomerIDLen { get; set; }
        //tblcardautomation and tblcardautofilepath param
        public string BankFolder { get; set; }
        public string SwitchInstitutionID { get; set; }
        public string Bank { get; set; }
        public string WinSCP_User { get; set; }
        public string WinSCP_PWD { get; set; }

        //new params
        public string WinSCP_IP { get; set; }
        public string WinSCP_Port { get; set; }
        public string PGP_KeyName { get; set; }
        public string PGP_PWD { get; set; }
        public string AGS_PGP_KeyName { get; set; }
        public string AGS_PGP_PWD { get; set; }
        public string RCVREmailID { get; set; }

        //AGS SFTP params
        public string AGS_SFTPServer { get; set; }
        public string AGS_SFTP_Port{ get; set; }
        public string AGS_SFTP_Pwd{ get; set; }
        public string AGS_SFTP_User{ get; set; }
        //Bank SFTP Params
        public string B_SFTPServer { get; set; }
        public string B_SFTP_Port{ get; set; }
        public string B_SFTP_Pwd{ get; set; }
        public string B_SFTP_User{ get; set; }
        //PRE SFTP Params
        public string C_SFTPServer{ get; set; }
        public string C_SFTP_Port{ get; set; }
        public string C_SFTP_Pwd{ get; set; }
        public string C_SFTP_User{ get; set; }

        //tblbin param

        public string CardPrefix { get; set; }
        public string AccountType { get; set; }
        public string CardType { get; set; }
        public string CardProgram { get; set; }
        public string BinDesc { get; set; }
        public string PREFormat { get; set; }
        public string Currency { get; set; }
        public string SwitchCardType { get; set; }
        public string Bankid { get; set; }

        public int IsMagstrip { get; set; }
        public int ReturnStatus { get; set; }
        // tblmassmodule param
        // public string ModuleName { get; set; }
        // public int RefCardmodulecode { get; set; }
        // public int RefPREmodulecode { get; set; }
        // public int Frequency { get; set; }


#endregion

    }

    public class BankModuleDataBO
    {
        #region Class for properties required for Module  configuration for bank
        public string ModuleName { get; set; }
        public int Frequency { get; set; }
        public string FrequencyUnit { get; set; }
        public string EnableState { get; set; }
        public int Status { get; set; }
        public string DllPath { get; set; }
        public string ClassName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int IssuerNum { get; set; }

#endregion   

    }
    public class ClsBankPREStandardBO
    {
        #region Class for properties required for Pre Data configuration for bank
        public string IssuerNo { get; set; }
        public string CardProgram { get; set; }
        public string Token { get; set; }
        public string OutputPosition { get; set; }
        public string Padding { get; set; }
        public string FixLength { get; set; }
        public string StartPos { get; set; }
        public string EndPos { get; set; }
        public string Direction { get; set; }
#endregion

    }
}
