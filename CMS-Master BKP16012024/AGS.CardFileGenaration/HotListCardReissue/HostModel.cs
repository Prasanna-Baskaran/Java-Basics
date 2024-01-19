using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotListCardReissue
{
    public class HostModel
    {
        internal string err_desc { get; set; }
        internal string response_code { get; set; }
        internal string stan { get; set; }
        internal string rsp_time { get; set; }
        internal string rsp_date { get; set; }
        internal string rrn { get; set; }
        internal string auth_code { get; set; }
        internal string uidai_authentication_data { get; set; }
        internal string account_balance { get; set; }
        internal string customer_details { get; set; }
    }
    public class HostModelRequest
    {
        internal string IssuerNo { get; set; }
        internal string CardReissueId { get; set; }
        internal string cardno { get; set; }
        internal string stan { get; set; }
        internal string proessingcode { get; set; }
        internal string HoldRespCode { get; set; }
    }
}
