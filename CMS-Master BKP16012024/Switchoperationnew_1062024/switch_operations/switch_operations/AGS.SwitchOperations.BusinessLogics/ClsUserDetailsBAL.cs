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
    public class ClsUserDetailsBAL:IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ClsReturnStatusBO FunInsertIntoUserUserDetails(string firstname, string lastname, string username, string mobileno, string emailid, int userstatus, int createdby, int UserRole,string SystemID,string BankID, string userPassword, string BranchCode)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsUserDetailsDAL()).FunInsertIntoUserUserDetails(firstname, lastname, username, mobileno, emailid, userstatus, createdby, UserRole,SystemID,BankID, userPassword,BranchCode);

            return ObjReturnStatus;
        }

        public ClsReturnStatusBO FunUpdateIntoUserUserDetails(string firstname, string lastname, string mobileno, string emailid, int userstatus, int createdby, int UserRole, string password, int userid,string SystemID, string bankid, string BranchCode)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = (new ClsUserDetailsDAL()).FunUpdateIntoUserUserDetails(firstname, lastname, mobileno, emailid, userstatus, createdby, UserRole, password, userid,SystemID,bankid, BranchCode);
            return ObjReturnStatus;
        }

        //Get User details
        public DataTable FunGetUserDetails(ClsUserBO ObjUser)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsUserDetailsDAL().FunGetUserDetails(ObjUser);
            return ObjDTOutPut;
        }

        //change Passward
        public ClsReturnStatusBO FunChangePassward(ClsUserBO ObjUser)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            ObjReturnStatus = new ClsUserDetailsDAL().FunChangePassward(ObjUser);
            return ObjReturnStatus;


        }

        }
}