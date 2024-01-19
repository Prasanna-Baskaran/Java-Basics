using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsAPIRequestBO
    {
        public string TranType { get; set; }
        public string RequestId { get; set; }
        public string TxnDateTime { get; set; }
        public string SourceId { get; set; }
        public string Msg { get; set; }
        //public string CardNo { get; set; }
        //public string AccountNo { get; set; }
        //public string AccountType { get; set; }
        //public string AccountQualifier { get; set; }
        //public string LinkingFlag { get; set; }
        public string SessionId { get; set; }

    }
    public class TemporaryLimitBORequestMSGParam
    {
        public string CardNo { get; set; }
        public string reserved1 { get; set; }//PerTxnLimit
        public string reserved2 { get; set; }//PerTxnCount
        public string reserved3 { get; set; }//OverallLimit
        public string reserved4 { get; set; }//OverallCount
        //param for add new account
        public string reserved5 { get; set; }//ToDate
        public string reserved6 { get; set; }//FromDate
    }
}
