using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.DataLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class CardTemporaryLimitBAL
    {
        public string FunLogTemporarylimitRequest(TemporaryLimitRequestMSGParam ObjReqmsg, ClsAPIRequestBO APIRequestParam, string Response, string ResponseCode, string AccountLogId)
        {
            string ReturnLogId;
            
            ReturnLogId = new CardTemporaryLimitDAL().FunLogTemporarylimitRequest(ObjReqmsg,APIRequestParam, Response,ResponseCode,AccountLogId);
            return ReturnLogId;
        }
    }
}
