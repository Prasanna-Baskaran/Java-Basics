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
    public class ClsCardMasterBAL : IDisposable
    {

        //Search Customer  for card limit
        public DataTable FunSearchCustCardLimit(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunSearchCustCardLimit(ObjFilter);
            return ObjDTOutPut;
        }

        //save Card Limit

        public ClsReturnStatusBO FunSaveCardLimit(CustSearchFilter ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = new ClsCardMasterDAL().FunSaveCardLimit(ObjFilter);

            return ObjReturnStatus;
        }
        //Search Customer for Card Ops
        public DataTable FunSearchCardDtl(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            //ObjDTOutPut = new ClsCardMasterDAL().FunSearchCardDtl(ObjFilter);
            ObjDTOutPut = new ClsCardMasterDAL().FunSearchCardDtlISO(ObjFilter);
            return ObjDTOutPut;
        }



        //save Card Operation request
        public ClsReturnStatusBO FunSaveCardOpsReq(CustSearchFilter ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            ObjReturnStatus = new ClsCardMasterDAL().FunSaveCardOpsReqISO(ObjFilter);

            return ObjReturnStatus;
        }

        //Search All Card Operations request
        public DataTable FunSearchCardRequests(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunSearchCardRequests(ObjFilter);
            return ObjDTOutPut;
        }


        public DataTable FunAccept_RejectCardReq(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunAccept_RejectCardReq(ObjCustomer);
            return ObjDTOutPut;
        }

        public DataTable FunGetSetCardDetailsForPinRepin(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetSetCardDetailsForPinRepin(ObjCustomer);
            return ObjDTOutPut;
        }

        //Search All Card Operations request
        public DataTable FunGetCardRequestByID(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetCardRequestByIDISO(ObjFilter);
            return ObjDTOutPut;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //Get reports
        public DataTable FunGetAllReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetAllReports(ObjFilter);
            return ObjDTOutPut;
        }

        //01-3-18
        //start sheetal
        public DataTable FunGetCustCardDetailsFromSwitch(CustSearchFilter objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetCustCardDetailsFromSwitch(objSearch);
            return ObjDTOutPut;
        }


        public DataTable FunGetOpsDataForISO(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetOpsDataForISO(ObjCustomer);
            return ObjDTOutPut;
        }

        public DataTable GetCardAPISourceIdDetails(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().GetCardAPISourceIdDetails(ObjCustomer);
            return ObjDTOutPut;
        }
        public DataTable FunSearchCardDtlISO(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            //ObjDTOutPut = new ClsCardMasterDAL().FunSearchCardDtl(ObjFilter);
            ObjDTOutPut = new ClsCardMasterDAL().FunSearchCardDtlISO(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable FunGetAllReportsEPS(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetAllReportsEPS(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable FunGetUserIdActivityReports(string BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetUserIdActivityReports(BankID);
            return ObjDTOutPut;
        }

        public DataTable FunGetAuditReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetAuditReports(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable FunGetActivityReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().FunGetActivityReports(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable GetCardByCIFID(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().GetCardByCIFID(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable GetPinOpertionsReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardMasterDAL().GetPinOpertionsReports(ObjFilter);
            return ObjDTOutPut;
        }


    }
}