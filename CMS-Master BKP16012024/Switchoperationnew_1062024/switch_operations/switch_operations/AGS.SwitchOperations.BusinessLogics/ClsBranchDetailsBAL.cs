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
    public class ClsBranchDetailsBAL : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ClsReturnStatusBO FunInsertIntoBranchMaster(string BranchName, string BranchCode, string createdby, int BankID)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsBranchDetailsDAL()).FunInsertIntoBranchMaster(BranchName, BranchCode,createdby, BankID);

            return ObjReturnStatus;
        }

        public ClsReturnStatusBO FunUpdateIntoBranchMaster(string BranchName, string createdby, int BankID, int Branchid, Boolean isActive)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsBranchDetailsDAL()).FunUpdateIntoBranchMaster(BranchName, createdby, BankID, Branchid, isActive);
            return ObjReturnStatus;
        }

        //Get User details
        public DataTable FunGetBranchDetails(int BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBranchDetailsDAL().FunGetBranchDetails(BankID);
            return ObjDTOutPut;
        }

    }
}
