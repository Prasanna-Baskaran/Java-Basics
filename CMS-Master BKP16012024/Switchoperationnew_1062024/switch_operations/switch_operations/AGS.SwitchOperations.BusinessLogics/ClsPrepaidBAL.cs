using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.DataLogics;
using AGS.SwitchOperations.BusinessObjects;


namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsPrepaidBAL:IDisposable
    {
        public ClsPrepaidBAL()
        {
        }

        public int FunSaveRequestLog(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {
            int code = 0;
            code= new ClsPrepaidDAL().FunSaveRequestLog(StrFunctionName, StrRequest, StrMessageOutput);
            return code;
        }

        public ClsReturnStatusBO FunAuthenticateCustomer(ClsLogin ObjLogin)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            ObjResult = new ClsPrepaidDAL().FunAuthenticateCustomer(ObjLogin);
            return ObjResult;
        }


        public DataTable CustomerRegisteration(string USERTYPE, string CARDNO, string USERNAME, string FIRSTNAME, string LASTNAME, string EMAILADDRS, string Q1, string Q2, string CVV, Int64 LogUser)
        {

            DataTable dtReturn = null;
            dtReturn = new ClsPrepaidDAL().CustomerRegisteration(USERTYPE, CARDNO, USERNAME, FIRSTNAME, LASTNAME, EMAILADDRS, Q1, Q2, CVV, LogUser);
            return dtReturn;
        }


        public bool PrepaidModuleLog(string StrPriTransactionType, string StrPriRequestData, string StrPriOutPutData)
        {
            Boolean bReturn = true;
            bReturn = new ClsPrepaidDAL().PrepaidModuleLog(StrPriTransactionType, StrPriRequestData, StrPriOutPutData);
            return bReturn;
        }

        public int FunTranTemp(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {

            int Code = 0;
            Code = new ClsPrepaidDAL().FunTranTemp(StrFunctionName, StrRequest, StrMessageOutput);
            return Code;
        }

        public int FunTranDetails(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {

            int Code = 0;
            Code = new ClsPrepaidDAL().FunTranDetails(StrFunctionName, StrRequest, StrMessageOutput);

            return Code;
        }


        public string changePasswordApp(string UserName, string OldPassword, string NewPassword, string CONFPASSWD)
        {
            string Status = "Suc";
            Status = new ClsPrepaidDAL().changePasswordApp(UserName, OldPassword, NewPassword, CONFPASSWD);
            return Status;
        }

        //insert prepaid txn in pre txn
        public ClsReturnStatusBO Fun_InsertPreTxnDtl(ClsTransaction ObjTxn)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            ObjResult.OutPutCode = "0";
            ObjResult = new ClsPrepaidDAL().Fun_InsertPreTxnDtl(ObjTxn);
            return ObjResult;


        }

        //insert prepaid txn in post txn
        public ClsReturnStatusBO Fun_InsertPostTxnDtl(ClsTransaction ObjTxn)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            ObjResult.OutPutCode = "0";

            ObjResult = new ClsPrepaidDAL().Fun_InsertPostTxnDtl(ObjTxn);
            return ObjResult;


        }

        //Fun Reversal 
        public DataTable Fun_Reversal(ClsReverseTxnDtl ObjRev)
        {
            DataTable dtReturn = new DataTable();
            dtReturn = new ClsPrepaidDAL().Fun_Reversal(ObjRev);

            return dtReturn;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}