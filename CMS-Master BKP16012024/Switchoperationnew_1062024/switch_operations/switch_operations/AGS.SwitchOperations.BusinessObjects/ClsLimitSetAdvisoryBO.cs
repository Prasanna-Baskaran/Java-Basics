using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
  public   class ClsLimitSetAdvisoryBO
    {
        public int ID { get; set; }
        public string BIN { get; set; }
        public Double ThresholdLimit { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
        public string USERID { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string EmailTamplate { get; set; }
        public string SMSTamplate { get; set; }
    }
}
