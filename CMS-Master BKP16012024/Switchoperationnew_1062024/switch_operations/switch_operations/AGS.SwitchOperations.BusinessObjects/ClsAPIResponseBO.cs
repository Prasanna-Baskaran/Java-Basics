using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsAPIResponseBO
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        //public string Sessionid { get; set; }
    }
    public class ClsGetsessionBO
    {
        public string Description { get; set; }
    }

    public class ClsGetSession
    {
        public string SessionId { get; set; }
    }
}
