using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AGS.Configuration;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SqlClient;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsUserDetailsDAL
    {

        public ClsReturnStatusBO FunInsertIntoUserUserDetails(string firstname, string lastname, string username, string mobileno, string emailid, int userstatus, int createdby, int UserRole,string SystemID,string BankID,String Userpassword, string BranchCode)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetUserDetails", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, username);
                    sspObj.AddParameterWithValue("@FirstName", SqlDbType.VarChar, 0, ParameterDirection.Input, firstname);
                    sspObj.AddParameterWithValue("@LastName", SqlDbType.VarChar, 0, ParameterDirection.Input, lastname);
                    sspObj.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, mobileno);
                    sspObj.AddParameterWithValue("@Emailid ", SqlDbType.VarChar, 0, ParameterDirection.Input, emailid);
                    sspObj.AddParameterWithValue("@UserStatus", SqlDbType.Int, 0, ParameterDirection.Input, userstatus);
                    sspObj.AddParameterWithValue("@CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, createdby);
                    sspObj.AddParameterWithValue("@UserRole", SqlDbType.Int, 0, ParameterDirection.Input, UserRole);
                    sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID);
                    sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID);
                    sspObj.AddParameterWithValue("@Userpassword", SqlDbType.VarChar, 0, ParameterDirection.Input, Userpassword);
                    sspObj.AddParameterWithValue("@BranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, BranchCode);
                    DtinsertInstallOutput = sspObj.ExecuteDataTable();
                    sspObj.Dispose();

                    if (DtinsertInstallOutput.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(DtinsertInstallOutput.Rows[0]["Code"]);
                        ObjReturnStatus.Description = DtinsertInstallOutput.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Addition failed !";
                    }
                }
            }
            catch (Exception xObj)
            {
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = xObj.Message;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjReturnStatus;
        }


        public ClsReturnStatusBO FunUpdateIntoUserUserDetails(string firstname, string lastname, string mobileno, string emailid, int userstatus, int createdby, int UserRole, string password, int userid,string SystemID, string bankid, string BranchCode)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpUpdateUserDetails", ObjConn, CommandType.StoredProcedure))
                {

                    sspObj.AddParameterWithValue("@FirstName", SqlDbType.VarChar, 0, ParameterDirection.Input, firstname);
                    sspObj.AddParameterWithValue("@LastName", SqlDbType.VarChar, 0, ParameterDirection.Input, lastname);
                    sspObj.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, mobileno);
                    sspObj.AddParameterWithValue("@Emailid ", SqlDbType.VarChar, 0, ParameterDirection.Input, emailid);
                    sspObj.AddParameterWithValue("@UserStatus", SqlDbType.Int, 0, ParameterDirection.Input, userstatus);
                    sspObj.AddParameterWithValue("@ModifiedBy", SqlDbType.Int, 0, ParameterDirection.Input, createdby);
                    sspObj.AddParameterWithValue("@UserRole", SqlDbType.Int, 0, ParameterDirection.Input, UserRole);
                    sspObj.AddParameterWithValue("@UserId", SqlDbType.Int, 0, ParameterDirection.Input, userid);
                    sspObj.AddParameterWithValue("@Password", SqlDbType.VarChar, 0, ParameterDirection.Input, password);
                    sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID);
                    sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, bankid);
                    sspObj.AddParameterWithValue("@BranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, BranchCode);
                    DtinsertInstallOutput = sspObj.ExecuteDataTable();
                    sspObj.Dispose();

                    if (DtinsertInstallOutput.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(DtinsertInstallOutput.Rows[0]["Code"]);
                        ObjReturnStatus.Description = DtinsertInstallOutput.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Addition failed !";
                    }
                }
            }
            catch (Exception xObj)
            {
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = xObj.Message;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjReturnStatus;
        }

        //Get User details
        public DataTable FunGetUserDetails(ClsUserBO ObjUser)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetUserDetails", ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(ObjUser.UserName))
                    {
                        ObjCmd.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.UserName);
                    }
                    if (!string.IsNullOrEmpty(ObjUser.MobileNo))
                    {
                        ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.MobileNo);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsUserDetailsDAL, FunGetUserDetails()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //Reset Passward
        public ClsReturnStatusBO FunChangePassward(ClsUserBO ObjUser)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_ChangeUserPassward", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    if (ObjUser.UserID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@UserId", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjUser.UserID);
                    }
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.Int, 0, ParameterDirection.Input, ObjUser.IntPara);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Passward is not changed";

                    }
                }
            }
            catch (Exception ObjExc)
            {

                ObjReturnStatus.Code = 1;

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsUserDetailsDAL, FunChangePassward()", ObjExc.Message,"IntPara = "+ObjUser.UserID+" ,UserID ="+ObjUser.UserID);
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Passward is not changed";
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;

        }

        public bool IsValidPasswordResetLink(string guid)
        {
            SqlConnection ObjConn = null;
            bool _return;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_IsPasswordResetLinkValid", ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(guid))
                    {
                        ObjCmd.AddParameterWithValue("@GUID", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, new Guid(guid));
                    }
                    _return = Convert.ToBoolean(ObjCmd.ExecuteScalar());
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsUserDetailsDAL, IsValidPasswordResetLink()", ObjExc.Message, ObjExc.StackTrace);
                _return = false;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return _return;
        }
        //Get User Forgot Password details
        public DataTable FunGetUserForgetPassDetails(ClsUserBO ObjUser, int mode)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                string sp = (mode == 1) ? "dbo.USP_RequestPasswordReset" : "dbo.USP_GetUserForgetPassDetails";

                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(sp, ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(ObjUser.UserName))
                    {
                        ObjCmd.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.UserName);
                    }
                    if (!string.IsNullOrEmpty(ObjUser.MobileNo))
                    {
                        ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.MobileNo);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsUserDetailsDAL, FunGetUserDetails()", ObjExc.Message, ObjExc.StackTrace);
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        //Reset User Password
        public bool FunResetPassword(ClsResetPassword ObjUser)
        {
            bool _return = false;
            SqlConnection ObjConn = null;
            SqlDataReader rdr;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ResetUserPassword", ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(ObjUser.UserName))
                    {
                        ObjCmd.AddParameterWithValue("@Username", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.UserName);
                    }
                    if (!string.IsNullOrEmpty(ObjUser.UniqueId))
                    {
                        ObjCmd.AddParameterWithValue("@GUID", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, new Guid(ObjUser.UniqueId));
                    }
                    if (!string.IsNullOrEmpty(ObjUser.NewPass))
                    {
                        ObjCmd.AddParameterWithValue("@Password", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjUser.NewPass);
                    }
                    rdr = ObjCmd.ExecuteReader();
                    if (rdr != null)
                    {
                        while (rdr.Read())
                        {
                            ObjUser.Result = Convert.ToInt32(rdr[0]);
                            _return = Convert.ToBoolean(rdr[0]);
                            ObjUser.Description = Convert.ToString(rdr[1]);
                        }
                    }
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsUserDetailsDAL, FunResetPassword()", ObjExc.Message, ObjExc.StackTrace);
                _return = false;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return _return;
        }
    }
}