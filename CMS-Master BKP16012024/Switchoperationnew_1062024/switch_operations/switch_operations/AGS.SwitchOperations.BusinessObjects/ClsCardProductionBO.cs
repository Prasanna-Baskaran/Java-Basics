using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCardProductionBO
    {
        public string CardNo { get; set; }
        public string BatchNo { get; set; }
        public bool ReDownload { get; set; }
        public string UserId { get; set; }
        public DateTime ProcessDate { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }

    }
} 
