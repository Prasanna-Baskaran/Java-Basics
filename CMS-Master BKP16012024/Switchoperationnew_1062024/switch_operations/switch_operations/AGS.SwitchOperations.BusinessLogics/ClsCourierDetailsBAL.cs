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
    public class ClsCourierDetailsBAL:IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public  ClsReturnStatusBO FunInsertIntoCourierDetails(string couriername, string officename,  string mobileno, int status, int createdby,string SystemID,string BankID)
        {
            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            ObjReturnStatus =( new ClsCourierDetailsDAL()).FunInsertIntoCourierDetails(couriername, officename, mobileno, status, createdby,SystemID,BankID);
            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunUpdateIntoCourierDetails(string couriername, string officename, string mobileno, int status, int modifiedby,int courierid,string SystemID,string BankID)
        {            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsCourierDetailsDAL()).FunUpdateIntoCourierDetails(couriername, officename, mobileno, status, modifiedby, courierid,SystemID,BankID);         
            return ObjReturnStatus;
        }
    
    }
}