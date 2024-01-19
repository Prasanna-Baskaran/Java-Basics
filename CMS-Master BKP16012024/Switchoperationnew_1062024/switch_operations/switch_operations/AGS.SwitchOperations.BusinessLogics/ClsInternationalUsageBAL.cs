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
    public class ClsInternationalUsageBAL: IDisposable
    {
        //Search Customer  for card limit
        public DataTable FunSearchCustCardIntNaUsageData(ClsIntNaCutDetails ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunSearchInsternationalUsageData(ObjFilter);
            return ObjDTOutPut;
        }

        //save Card Limit

        public DataTable FunInsertCustCardIntNaUsageData(ClsIntNaCutDetails ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunInsertCustCardIntNaUsageData(ObjFilter);
            return ObjDTOutPut;
        }


        //Checker code
        
        public DataTable FunGetIntNaUsageRequestData(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunGetIntNaUsageRequestData(objSearch);
            return ObjDTOutPut;
        }
        public DataTable FunUpdateIntNaUsageResponse(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunUpdateIntNaUsageResponse(objSearch);
            return ObjDTOutPut;
        }

        public DataTable FunRejectIntNaUsageResponse(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunRejectIntNaUsageResponse(objSearch);
            return ObjDTOutPut;
        }

        //report
        public DataTable FunGetAllReports(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunGetAllReports(objSearch);
            return ObjDTOutPut;
        }

        public DataTable FunGetCardOpsReports(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsInternationalUsageDAL().FunGetCardOpsReports(objSearch);
            return ObjDTOutPut;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
