using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.DataLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.Utilities;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsCommonBAL : IDisposable
    {
        public bool FunLog(string StrPriTransactionType, string StrPriRequestData, string StrPriOutPutData)
        {
            Boolean bReturn = true;
            bReturn = ClsCommonDAL.FunLog(StrPriTransactionType, StrPriRequestData, StrPriOutPutData);
            return bReturn;

        }


       public ClsReturnStatusBO FunGetBankLogo()
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Get Logo Failed";
            ObjReturnStatus = ClsCommonDAL.GetBankLogo();
            return ObjReturnStatus;
        }

        public ClsReturnStatusBO FunLoginValidate(ClsLoginBO ObjPriLoginBO)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Login Failed";
            ObjReturnStatus = ClsCommonDAL.FunLoginValidate(ObjPriLoginBO);
            return ObjReturnStatus;
        }

        public string FunOtpValidation(ClsOtpvalidBO clsOtpvalid)
        {
            string s = "";
            s = ClsCommonDAL.FunOtpValidation(clsOtpvalid);
            return s;

        }
        public void FunChangepass(ClsCahngePass objChangePass)
        {
            objChangePass.Description = "Failed to change password";
            ClsCommonDAL.FunChangepass(objChangePass);
        }

        public void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList)
        {
            ClsCommonDAL.FunInsertIntoErrorLog(procedureName, errorDesc, parameterList);
        }

        public void FunInsertIntoISOLog(string StrFunName, string StrPriParam, string StrISOString, string StrOutput)
        {
            ClsCommonDAL.FunInsertIntoISOLog(StrFunName, StrPriParam, StrISOString, StrOutput);
        }

        public DataTable FunGetCommonDataTable(Int32 IntPriContextKey, string StrPriPara)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = ClsCommonDAL.FunGetCommonDataTable(IntPriContextKey, StrPriPara);
            return ObjDTOutPut;
        }

        public string FunGetParameter()
        {
            string StrConfigDtl = string.Empty;
            StrConfigDtl = ClsCommonDAL.FunGetParameter();
            return StrConfigDtl;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string FunGetGridData(int ContextKey, string Param)
        {
            try
            {
                using (DataTable ObjDTOutPut = (new ClsCommonBAL()).FunGetCommonDataTable(ContextKey, Param))
                {
                    return ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { control = Controls.Button, buttonName = "Edit", hideColumnName = true, cssClass = "btn btn-link", events = new Event[] { new Event() { EventName = "onClick", EventValue = "funedit($(this))" } } } });
                }
            }
            catch
            {
                return "";
            }
        }

        public DataTable FunGetCommonCardDetails(Int32 ActionType, string BankId)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = ClsCommonDAL.FunGetCommonCardDetails(ActionType, BankId);
            return ObjDTOutPut;
        }

        public DataTable FunAcceptRejectCardRequestDetails(ClsProcessCardRequest _ClsProcessCardRequest)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = ClsCommonDAL.FunAcceptRejectCardRequestDetails(_ClsProcessCardRequest);
            return ObjDTOutPut;
        }
        public DataTable FunGetSFTPDetailsToPlaceCIF(string IssuerNo, Int32 ProcessId)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = ClsCommonDAL.FunGetSFTPDetailsToPlaceCIF(IssuerNo, ProcessId);
            return ObjDTOutPut;
        }

        public void FunInsertPortalISOLog(string TranType, string Param1, string Param2, string Param3, string Message, string MessageData, int LoginId)
        {
            ClsCommonDAL.FunInsertPortalISOLog(TranType, Param1, Param2, Param3, Message, MessageData, LoginId);
        }

        public DataTable FunGetTerminalDetails(string ParticipantId, string _tid, string _requestType)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = ClsCommonDAL.FunGetTerminalDenominationDetails(ParticipantId, _tid, _requestType);
            return ObjDTOutPut;
        }

        public bool FunProcessTermDenominationDetails(string _type, string _participantId, string _tid, string _loginId, string _cassette1Deno, string _cassette1Count, string _cassette2Deno, string _cassette2Count, string _cassette3Deno, string _cassette3Count, string _cassette4Deno, string _cassette4Count, string _requestType, int _termDenominationId, string _term_type, string _cardAcceptor, string _currencyCode)
        {
            return ClsCommonDAL.FunProcessTermDenominationDetails(_type, _participantId, _tid, _loginId, _cassette1Deno, _cassette1Count, _cassette2Deno, _cassette2Count, _cassette3Deno, _cassette3Count, _cassette4Deno, _cassette4Count, _requestType, _termDenominationId, _term_type, _cardAcceptor, _currencyCode);
        }


    }
}
