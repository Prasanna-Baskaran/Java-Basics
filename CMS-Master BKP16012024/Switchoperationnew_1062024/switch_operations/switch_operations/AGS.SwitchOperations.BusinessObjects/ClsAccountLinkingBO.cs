using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsAccountLinkingBO
    {
        public string CardNo { get; set; }
        public string AccountNo { get; set; }
     

        public string SystemID { get; set; }
        public string BankID { get; set; }
        public int FormStatusId { get; set; }


        public string SourceId { get; set; }
    }

    public class ClsAccountLinkingRequestMSGParam
    {
        public string CardNo { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public string AccountQualifier { get; set; }
        public string LinkingFlag { get; set; }
        //param for add new account
        public string Currency { get; set; }

    }
}
