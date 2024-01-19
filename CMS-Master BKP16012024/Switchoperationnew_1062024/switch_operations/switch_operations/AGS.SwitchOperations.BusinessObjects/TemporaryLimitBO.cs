using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class TemporaryLimitBO
    {
    }

    public class TemporaryLimitRequestMSGParam
    {
        public string CardNo { get; set; }
        public string reserved1 { get; set; }//PerTxnLimit
        public string reserved2 { get; set; }//PerTxnCount
        public string reserved3 { get; set; }//OverallLimit
        public string reserved4 { get; set; }//OverallCount
        //param for add new account
        public string reserved5 { get; set; }//ToDate
        public string reserved6 { get; set; }//FromDate
        public string reserved8 { get; set; }//FromDate
    }
}
