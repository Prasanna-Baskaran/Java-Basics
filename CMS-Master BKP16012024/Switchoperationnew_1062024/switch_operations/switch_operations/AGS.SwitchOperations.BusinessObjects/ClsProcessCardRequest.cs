using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsProcessCardRequest
    {
        public int ActionType { get; set; }
        public string BankId { get; set; }
        public int RequestType { get; set; }
        public string Code { get; set; }
        public string ISORSPCode { get; set; }
        public string ISORSPDesc { get; set; }

        public int IssuerNo { get; set; }
        public int LoginId { get; set; }
        public bool ErrorFlag { get; set; }
        public string ErrorDesc { get; set; }
        public string iInstaEdit { get; set; }

        public string UserBranchCode { get; set; }
        public Boolean IsAdmin { get; set; }
        public string UserID { get; set; }
        
    }
}
