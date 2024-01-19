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
    public class ClsUserRoleBAL:IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public  ClsReturnStatusBO FunInsertIntoUserRoleMaster(string UserRole, Int64 CreatedBy,string SystemID,string BankID, String IDs)
        {
            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            ObjReturnStatus = (new ClsUserRoleDAL()).FunInsertIntoUserRoleMaster(UserRole, CreatedBy,SystemID,BankID,IDs);
            return ObjReturnStatus;
        }

        public  ClsReturnStatusBO FunUpdateIntoUserRoleMaster(string UserRole, Int32 UserID, Int32 ModifiedBy,string SystemID,string BankID,String IDs)
        {
            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsUserRoleDAL()).FunUpdateIntoUserRoleMaster(UserRole, UserID, ModifiedBy,SystemID,BankID, IDs);
            
            return ObjReturnStatus;
        }
    }
}