using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.DataLogics;
using AGS.SwitchOperations.BusinessObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsCustomerMasterBAL : IDisposable
    {
        //Search Customer
        public DataTable FunSearchCustomer(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunSearchCustomer(ObjFilter);
            return ObjDTOutPut;
        }
        public ClsReturnStatusBO FunSaveCustomer(ClsCustomer CustomerDtl, ClsCustOccupationDtl OccupationDtl, ClsCustomerAddress CustAddress, DataTable OtherCreditCardDtl)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = new ClsCustomerMasterDAL().FunSaveCustomer(CustomerDtl, OccupationDtl, CustAddress, OtherCreditCardDtl);
            return ObjReturnStatus;
        }
        //GetCustomer Details
        public DataSet FunGetCustomerDetails(CustSearchFilter ObjFilter)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunGetCustomerDetails(ObjFilter);
            return ObjDTOutPut;
        }

        public ClsReturnStatusBO FunAccept_RejectCustomer(ClsCustomer ObjCustomer)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = new ClsCustomerMasterDAL().FunAccept_RejectCustomer(ObjCustomer);
            return ObjReturnStatus;
        }

        public DataTable FunAccept_RejectCustomerForm(ClsCustomer ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            return new ClsCustomerMasterDAL().FunAccept_RejectCustomerForm(ObjCustomer);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public ClsReturnStatusBO FunSaveCardRequest(DataTable _Dt, CustomerRegistrationDetails _CustomerRegistrationDetails)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = new ClsCustomerMasterDAL().FunSaveCardRequest(_Dt,_CustomerRegistrationDetails);
            return ObjReturnStatus;
        }

        public DataTable FunViewCardDetails(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunViewCardDetails(ObjFilter);
            return ObjDTOutPut;
        }

        public DataTable FunGetCardAPIdata(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunGetCardAPIdata(ObjFilter);
            return ObjDTOutPut;
        }


        public DataTable FunGetLanguage(int BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunGetLanguage(BankID);
            return ObjDTOutPut;
        }

        public DataTable FunCheckBlockCards(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCustomerMasterDAL().FunCheckBlockCards(ObjFilter);
            return ObjDTOutPut;
        }


    }
}
