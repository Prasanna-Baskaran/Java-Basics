using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsPrepaidBO
    {
    }
    public class ClsLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public static class ClsMessage
    {
        public static string LoginFailed = "Invalid username password";
        public static string MinistatementFailed = "Statement is not generated";
    }

    public class ClsTransaction
    {
        public decimal TxnAmount { get; set; }
        public Int64? TxnTypeID { get; set; }
        public Int64? TxnTempID { get; set; }
        public string TerminalID { get; set; }
        public string INSTID { get; set; }
        public string FromCardNo { get; set; }
        public string ToCardNo { get; set; }
        public string ToAccountNumber { get; set; }
        public string InputStream { get; set; }
        public string SwitchRRN { get; set; }
        public string SwitchRSPCode { get; set; }
        public string SwitchProcsCode { get; set; }
        public string SwitchAmount { get; set; }
        public string SwitchStan { get; set; }
        public string SwitchTime { get; set; }
        public string SwitchDate { get; set; }
        public string SwitchTeminalID { get; set; }
        public string SwitchTransmissionDate { get; set; }
        public Int64 PostTxnID { get; set; }
    }

    public class ClsReverseTxnDtl
    {
        public string ISOString { get; set; }
        public string SwitchStan { get; set; }
        public string TerminalID { get; set; }
        public string SwitchTime { get; set; }
        public string SwitchDate { get; set; }
        public string SwitchDateTime { get; set; }
        public string SwitchReversalRspString { get; set; }
        public string SwitchRRN { get; set; }
        public string SwitchReverseRSPCode { get; set; }
        public string SwitchReverseRRN { get; set; }
        public string SwitchReverseAmount { get; set; }
        public Int64 Post_TxnID { get; set; }
        public string ReversalRemark { get; set; }


    }
}