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
using System;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsBranchDetailsDAL
    {
        public ClsReturnStatusBO FunInsertIntoBranchMaster(string BranchName, string BranchCode, string createdby, int BankID)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetBranchDetails", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@BranchName", SqlDbType.VarChar, 0, ParameterDirection.Input, BranchName);
                    sspObj.AddParameterWithValue("@BranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, BranchCode);
                    sspObj.AddParameterWithValue("@CreatedBy", SqlDbType.VarChar, 0, ParameterDirection.Input, createdby);
   
                    sspObj.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, BankID);
     

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
       
        public ClsReturnStatusBO FunUpdateIntoBranchMaster(string BranchName, string createdby, int BankID,  int Branchid, Boolean isActive)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpUpdateBranchDetails", ObjConn, CommandType.StoredProcedure))
                {

                    sspObj.AddParameterWithValue("@BranchName", SqlDbType.VarChar, 0, ParameterDirection.Input, BranchName);
                    sspObj.AddParameterWithValue("@ModifiedBy", SqlDbType.VarChar, 0, ParameterDirection.Input, createdby);

                    sspObj.AddParameterWithValue("@Branchid", SqlDbType.Int, 0, ParameterDirection.Input, Branchid);
                    sspObj.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, BankID);
                    sspObj.AddParameterWithValue("@isActive", SqlDbType.Bit, 0, ParameterDirection.Input, isActive);
                    
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
        public DataTable FunGetBranchDetails(int BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetBranchDetails", ObjConn, CommandType.StoredProcedure))
                {
               
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, BankID);
                 
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBranchDetailsDAL, FunGetBranchDetails()", ObjExc.Message, "");
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
    }
}
