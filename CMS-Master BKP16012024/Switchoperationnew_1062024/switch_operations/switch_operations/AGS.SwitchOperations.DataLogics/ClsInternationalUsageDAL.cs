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
using AGS.Configuration;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsInternationalUsageDAL
    {
        //Search Customer  for InsternationalUsage
        public DataTable FunSearchInsternationalUsageData(ClsIntNaCutDetails  ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetSetInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "Select");

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {

                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);

                    }
                    //if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    //{
                    //    ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    //}
                    if (!string.IsNullOrEmpty(ObjFilter.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@issuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(ObjFilter.IssuerNo));
                    }

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUR, ClsInternationalUsageDAL, FunSearchInsternationalUsageData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //save InsternationalUsage

        public DataTable FunInsertCustCardIntNaUsageData(ClsIntNaCutDetails ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetSetInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
            
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "INSERT");

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {

                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);

                    }
                    if (!string.IsNullOrEmpty(ObjFilter.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@issuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32 (ObjFilter.IssuerNo));
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.IntNaUsage))
                    {
                        ObjCmd.AddParameterWithValue("@Type", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IntNaUsage);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.Remark);
                    }
                    ObjCmd.AddParameterWithValue("@uploadBy", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserID);
                    ObjCmd.AddParameterWithValue("@modeofoperation", SqlDbType.VarChar, 0, ParameterDirection.Input, "SWITCH_PORTAL");

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUR, ClsInternationalUsageDAL, FunInsertCustCardIntNaUsageData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }


        //checker code 
        public DataTable FunGetIntNaUsageRequestData(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetSetInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "SELECT_REQUEST");
                    if (!string.IsNullOrEmpty(objSearch.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@issuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objSearch.IssuerNo));
                    }
                    ObjCmd.AddParameterWithValue("@modeofoperation", SqlDbType.VarChar, 0, ParameterDirection.Input, "SWITCH_PORTAL");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUA, ClsInternationalUsageDAL, FunGetIntNaUsageRequestData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunUpdateIntNaUsageResponse(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetSetInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "UPDATE_RESPONSE");

                    if (!string.IsNullOrEmpty(objSearch.ISORSPCode))
                    {
                        ObjCmd.AddParameterWithValue("@SwitchRsp", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.ISORSPCode);
                    }
                    ObjCmd.AddParameterWithValue("@SwitchDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.ISORSPDesc);
                    ObjCmd.AddParameterWithValue("@id", SqlDbType.BigInt, 0, ParameterDirection.Input, objSearch.id);
                    ObjCmd.AddParameterWithValue("@authBy", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.UserID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUA, ClsInternationalUsageDAL, FunUpdateIntNaUsageResponse()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunRejectIntNaUsageResponse(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_RejectInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "REJECT");
                    ObjCmd.AddParameterWithValue("@DtBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, objSearch.DtBulkData);
                    ObjCmd.AddParameterWithValue("@RejectedBy", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.UserID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUA, ClsInternationalUsageDAL, FunRejectIntNaUsageResponse()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunGetAllReports(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 3);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetSetInsternationalUsage", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "REPORT");
                    ObjCmd.AddParameterWithValue("@modeofoperation", SqlDbType.VarChar, 0, ParameterDirection.Input, "SWITCH_PORTAL");
                    if (objSearch.FromDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, objSearch.FromDate);
                    }
                    if (objSearch.ToDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, objSearch.ToDate);
                    }

                    ObjCmd.AddParameterWithValue("@issuerno", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objSearch.IssuerNo));
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("IUREPO, ClsInternationalUsageDAL, FunGetAllReports()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        public DataTable FunGetCardOpsReports(ClsIntNaCutDetails objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            //SqlConnection ObjConn = new SqlConnection("Data Source=10.10.0.41; Initial Catalog=SwitchOperations; user id=uagsrep; password=ags@1234");
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetCardOpsReport", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@operation", SqlDbType.VarChar, 0, ParameterDirection.Input, "REPORT");
                    if (objSearch.CardNo != null)
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.CardNo);
                    }
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objSearch.BankID));
                    ObjCmd.AddParameterWithValue("@issuerno", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(objSearch.IssuerNo));
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("COPR, ClsInternationalUsageDAL, FunGetCardOpsReports()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        


    }
}
