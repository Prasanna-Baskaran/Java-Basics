using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsGeneratePrepaidCardBO
    {
        #region variables for generateprepaidcard
        public string BankName { get; set; }
        //issurer no for bank
        public string NoOfCards { get; set; }
        public string AccountType { get; set; }
        public string CardProgram { get; set; }
        public string IssuerNo { get; set; }
        public string UserID { get; set; }
        public string Mode { get; set; }

        public string[] RequestIDs { get; set; }
        public DataTable DtBulkData { get; set; }
        public long CardREqID { get; set; }

        public string UserBranchCode { get; set; }
        public Boolean IsAdmin { get; set; }
        
        #endregion
    }
}
