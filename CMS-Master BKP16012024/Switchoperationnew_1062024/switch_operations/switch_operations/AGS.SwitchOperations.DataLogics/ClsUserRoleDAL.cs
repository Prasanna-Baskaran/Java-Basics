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
    public class ClsUserRoleDAL
    {

        public  ClsReturnStatusBO FunInsertIntoUserRoleMaster(string UserRole, Int64 CreatedBy,string SystemID,string BankID, String IDs)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetUserRoleMaster", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@UserRole", SqlDbType.VarChar, 0, ParameterDirection.Input, UserRole);
                    sspObj.AddParameterWithValue("@CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, CreatedBy);
                    if (!string.IsNullOrEmpty(SystemID))
                    {
                        sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID.Trim());
                    }
                    if (!string.IsNullOrEmpty(BankID))
                    {
                        sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID.Trim());
                    }
                    sspObj.AddParameterWithValue("@IDs", SqlDbType.VarChar, 0, ParameterDirection.Input, IDs);

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

        public  ClsReturnStatusBO FunUpdateIntoUserRoleMaster(string UserRole, Int32 UserID, Int32 ModifiedBy,string SystemID,string BankID, String IDs)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SP_UpdateUserRoleMaster", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable DtInstallOutput = new DataTable();
                    sspObj.AddParameterWithValue("@UserRole", SqlDbType.VarChar, 0, ParameterDirection.Input, UserRole);
                    sspObj.AddParameterWithValue("@UserID", SqlDbType.Int, 0, ParameterDirection.Input, UserID);
                    sspObj.AddParameterWithValue("@ModifiedBy", SqlDbType.Int, 0, ParameterDirection.Input, ModifiedBy);
                    if (!string.IsNullOrEmpty(SystemID))
                    {
                        sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID.Trim());
                    }
                    if (!string.IsNullOrEmpty(BankID))
                    {
                        sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID.Trim());
                    }
    
                    sspObj.AddParameterWithValue("@IDs", SqlDbType.VarChar, 0, ParameterDirection.Input, IDs);
 



                    DtInstallOutput = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                    if (DtInstallOutput.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(DtInstallOutput.Rows[0]["Code"]);
                        ObjReturnStatus.Description = DtInstallOutput.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Updation failed !";
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
    }
}