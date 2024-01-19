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
    public class ClsRightsBAL:IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public DataSet GetOptionRights(string role,string SystemID)
        {
            DataSet ObjDsOutPut=new DataSet();
            ObjDsOutPut = (new ClsRightsDAL()).GetOptionRights(role, SystemID);
            return ObjDsOutPut;
            
        }
        public  ClsReturnStatusBO updateUserRights(DataTable dtUserRights, string userid,string SystemID)
        {
            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "User Rights is failed !";

            ObjReturnStatus = (new ClsRightsDAL()).updateUserRights(dtUserRights, userid,SystemID);
            return ObjReturnStatus;
        }
    }
}