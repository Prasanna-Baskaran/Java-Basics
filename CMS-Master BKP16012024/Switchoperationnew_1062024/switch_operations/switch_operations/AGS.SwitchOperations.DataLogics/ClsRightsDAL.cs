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
    public class ClsRightsDAL
    {
        public DataSet GetOptionRights(string role,string SystemID)
        {
            DataSet ObjDsOutPut;
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.spGetOptionRights", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Role", SqlDbType.VarChar, 0, ParameterDirection.Input, role);
                    if(!string.IsNullOrEmpty(SystemID))
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                }
                return ObjDsOutPut;
            }
            catch
            {
                return new DataSet();   
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }

        public  ClsReturnStatusBO updateUserRights(DataTable dtUserRights, string userid,string SystemID)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "User Rights is failed !";
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.spUpdateUserRights", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@userrights", SqlDbType.Structured, 0, ParameterDirection.Input, dtUserRights);
                    ObjCmd.AddParameterWithValue("@userid", SqlDbType.VarChar, 0, ParameterDirection.Input, userid);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID);
                    DataTable dtResult = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    if (dtResult.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(dtResult.Rows[0]["ResponseCode"]);
                        ObjReturnStatus.Description = dtResult.Rows[0]["ResponseDesc"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "User Rights is failed !";
                    }
                   
                }
               
            }
            catch(Exception ex)
            {
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = ex.Message;
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