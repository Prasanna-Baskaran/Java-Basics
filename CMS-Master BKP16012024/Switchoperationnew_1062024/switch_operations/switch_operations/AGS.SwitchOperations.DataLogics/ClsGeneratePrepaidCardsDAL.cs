using AGS.SqlClient;
using AGS.SwitchOperations.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SwitchOperations.BusinessObjects;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsGeneratePrepaidCardsDAL
    {

        public DataSet PinReissuanceRequest(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetsetPinReissuance", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.NoOfCards);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjPrepaid.IsAdmin);

                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, PinReissuanceRequest() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }


        public DataSet GeneratePrepaidCard_Operations(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GenerateInstaCards", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.CardProgram);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@NoOfCards", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.NoOfCards);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjPrepaid.IsAdmin);

                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, GeneratePrepaidCard_Operations() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }

        public DataSet AcceptRejectInstaCards(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_AcceptRejectInstaCards", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@DtBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, ObjPrepaid.DtBulkData);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjPrepaid.IsAdmin);
                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, GeneratePrepaidCard_Operations() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }



        public DataSet AcceptRejectAccountlink(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_AcceptRejectAccountlink", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@DtBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, ObjPrepaid.DtBulkData);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjPrepaid.IsAdmin);
                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, GeneratePrepaidCard_Operations() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }


        
        public DataSet GetSetAccountLinkrequest(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSetAccountLinkrequest", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@DtBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, ObjPrepaid.DtBulkData);
                    ObjCmd.AddParameterWithValue("@CardREqID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjPrepaid.CardREqID);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, GetSetAccountLinkrequest() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }


        public DataSet GetSetAccountLinkrequestOperations(ClsGeneratePrepaidCardBO ObjPrepaid)
        {

            DataSet ObjDSOutPut = new DataSet();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAccountLinkPendingData", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.Mode);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserID);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPrepaid.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjPrepaid.IsAdmin);
                    ObjDSOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, GeneratePrepaidCard_Operations() ,Mode= " + ObjPrepaid.Mode, Ex.Message, "");
                ObjDSOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDSOutPut;
        }

        public DataTable FunGetPrepaidCIFFileDetails(int IssuerNo, string Filename, string Mode)
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Usp_PrepaidCIFFileStatus", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, Mode);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@Filename", SqlDbType.VarChar, 0, ParameterDirection.Input, Filename);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsGeneratePrepaidCardsDAL, FunGetPrepaidCIFFileDetails()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
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
