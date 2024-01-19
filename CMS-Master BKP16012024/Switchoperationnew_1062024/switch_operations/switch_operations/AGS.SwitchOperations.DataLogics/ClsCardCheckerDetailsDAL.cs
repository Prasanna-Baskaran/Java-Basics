using AGS.SqlClient;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsCardCheckerDetailsDAL
    {
        //START SHEETAL
        public DataTable FunGetProcessType()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetProcessType", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, funGetProcessType()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        public DataTable FunGetChecker(ClsCardCheckerDetailsBO ObjChecker)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCardGenerateRequestData", ObjConn, CommandType.StoredProcedure))
                {

                    {//new
                        ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CheckerId);
                        ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "select");
                        //ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CustomerID);
                        //ObjCmd.AddParameterWithValue("@OldCardRPANID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CardNo);
                        ObjCmd.AddParameterWithValue("@CheckedIDList", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.IDs);
                        ObjDTOutPut = ObjCmd.ExecuteDataTable();
                        ObjCmd.Dispose();
                        ObjConn.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSearchCardDtl()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public bool FunUpdateChecker(ClsCardCheckerDetailsBO ObjChecker)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCardGenerateRequestData", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@CheckedIDList", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.IDs);
                    ObjCmd.AddParameterWithValue("@CheckerId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CheckerId);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "update");
                    //ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjChecker.CustomerID );
                    //ObjCmd.AddParameterWithValue("@OldCardRPANID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CardNo);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunUpdateChecker()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                string Str = Convert.ToString(ObjDTOutPut.Rows[0]["Success"]);
                if (Convert.ToString(Str.ToLower()).Contains("success"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public DataTable ValidateReissueCardRequest(ProcessCardReissueRequest ObjChecker)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ProcessCardReissueRequest", ObjConn, CommandType.StoredProcedure))
                {
                    {
                        ObjCmd.AddParameterWithValue("@ActionType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.ActionType);
                        ObjCmd.AddParameterWithValue("@LoginID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.LoginId);
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.IssuerNo);
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.BankID);
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.SystemID);
                        ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.IDs);
                        ObjDTOutPut = ObjCmd.ExecuteDataTable();
                        ObjCmd.Dispose();
                        ObjConn.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardCheckerDetailsDAL, ValidateReissueCardRequest()", Ex.Message, "");
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
