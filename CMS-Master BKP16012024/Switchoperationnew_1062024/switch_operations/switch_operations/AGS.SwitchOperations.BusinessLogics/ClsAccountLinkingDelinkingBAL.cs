using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.DataLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsAccountLinkingDelinkingBAL
    {
        /// <summary>
        /// Check cardexist or not for bank
        /// </summary>
        /// <param name="ObjAcclink"></param>
        /// <returns></returns>


        public string FunCheckCardExist(ClsAccountLinkingBO ObjAcclink)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsAccountLinkingDelinkingDAL().FunCheckCardExist(ObjAcclink);
            return ObjReturnStatus;
        }
        /// <summary>
        /// get account and card datails of bank for account linking delinking of customer
        /// </summary>
        /// <param name="ObjAcclink"></param>
        /// <returns></returns>
        public DataTable FunSearchCardDetails(ClsAccountLinkingBO ObjAcclink)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsAccountLinkingDelinkingDAL().FunSearchCardDetails(ObjAcclink);
            return ObjDTOutPut;
        }
        /// <summary>
        /// GetSession for bank
        /// </summary>
        /// <param name="ObjAcclink"></param>
        /// <returns></returns>
        /// 
        //public DataTable FunGetSessionForBank(ClsAccountLinkingBO ObjAcclink, ClsAPIRequestBO APIrequestparam)
        public DataTable FunGetSessionForBank(string Sourceid)
        {
            DataTable ObjDTOutPut = new DataTable();
            //ObjDTOutPut = new ClsAccountLinkingDelinkingDAL().FunGetSessionForBank(ObjAcclink, APIrequestparam);
            ObjDTOutPut = new ClsAccountLinkingDelinkingDAL().FunGetSessionForBank(Sourceid);
            return ObjDTOutPut;
        }

        public string FunLogAccountLinkelinkRequest(string CardNo, string AccountNo, string AccountType, string AccountQualifier, string LinkingFlag, string Response, string ResponseCode, string AccountLogId)
        {
            string ReturnLogId;
            //ObjDTOutPut = new ClsAccountLinkingDelinkingDAL().FunGetSessionForBank(ObjAcclink, APIrequestparam);
            ReturnLogId = new ClsAccountLinkingDelinkingDAL().FunLogAccountLinkelinkRequest(CardNo, AccountNo, AccountType, AccountQualifier, LinkingFlag, Response, ResponseCode, AccountLogId);
            return ReturnLogId;
        }

        public DataTable FunGetSourceIdForChannel(ClsAccountLinkingBO ObjAcclink)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsAccountLinkingDelinkingDAL().FunGetSourceIdForChannel(ObjAcclink);
            return ObjDTOutPut;
        }

        public static void FunSyncAccountEnc(string BankId, string AccEnc, string AccountNo, string AccountType, string CurrencyCode)
        {
            ClsAccountLinkingDelinkingDAL.FunSyncAccountEnc(BankId, AccEnc, AccountNo, AccountType, CurrencyCode);

        }

        public static void FunSyncCardAccountLinkingDetails(string CardNo, string IssuerNo, DataTable DT)
        {
            ClsAccountLinkingDelinkingDAL.FunSyncCardAccountLinkingDetails(CardNo,IssuerNo,DT);

        }
        public static DataTable FunSyncCardDetailsForAccountLinking(string CardNo, string CIF, string NIC, string Name, string IssuerNo)
        {
            return ClsAccountLinkingDelinkingDAL.FunSyncCardDetailsForAccountLinking(CardNo, CIF, NIC, Name, IssuerNo);

        }

    }
}
