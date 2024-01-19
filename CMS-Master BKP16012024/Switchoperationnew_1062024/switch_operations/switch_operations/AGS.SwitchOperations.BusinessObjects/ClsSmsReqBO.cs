using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsSmsReqBO
    {
        public string destination { get; set; }
        public string q { get; set; }
        public string message { get; set; }
        public string from { get; set; }
    }
}
