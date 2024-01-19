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
    public class ClsManageCardDAL
    {
        public DataTable FunGetNewBINPrefix(string BankID)
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetddlBinPrefix", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@bankid", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetNewBINPrefix()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        public string FunOldCardRPANIDExist(ClsCardCheckerDetailsBO ObjChecker)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ValidateCustId_OldCardRPANID", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@NewBinPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.NewBinPrefix);
                    ObjCmd.AddParameterWithValue("@HoldRSPCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.HoldRSPCode);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CardNo);
                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "OldCardRPANID Exist");
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CustomerID);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CheckerId);
                    //new params added
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, ObjChecker.BankID);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, ObjChecker.SystemID);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
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
            if (ObjDTOutPut.Rows.Count > 0)
            {
                _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return _dtReturn;


        }


        public bool FunSaveCard(ClsCardCheckerDetailsBO ObjCardcheck)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ValidateCustId_OldCardRPANID", ObjConn, CommandType.StoredProcedure))
                {


                    ObjCmd.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "insert");
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCardcheck.CustomerID);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCardcheck.CardNo);
                    ObjCmd.AddParameterWithValue("@NewBinPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCardcheck.NewBinPrefix);
                    ObjCmd.AddParameterWithValue("@HoldRSPCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCardcheck.HoldRSPCode);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCardcheck.CheckerId);

                    //2-12-17 add two prams in sp
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCardcheck.BankID);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCardcheck.SystemID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSaveCard()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                string Str = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
                if (Str == "successfully inserted")
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


        public string FunUpdateCarddetails(ClsAccountLinkingBO ObjAcclink)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdateCarddata", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@cardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.CardNo);
                    ObjCmd.AddParameterWithValue("@bankid", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.BankID);
                    ObjCmd.AddParameterWithValue("@Systemid ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.SystemID);
                    ObjCmd.AddParameterWithValue("@Formstatusid ", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.FormStatusId);
                    ObjCmd.AddParameterWithValue("@accountno ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.AccountNo);
                    //ObjCmd.AddParameterWithValue("@Mode ", SqlDbType.VarChar, 0, ParameterDirection.Input, Strtype);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunPutBinDataForBank()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return _dtReturn;
        }

        public string FunValidateReissueCardRequest(ClsCardCheckerDetailsBO ObjChecker)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ValidateReissueRequest", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CustomerID);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CardNo);
                    ObjCmd.AddParameterWithValue("@NewBinPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.NewBinPrefix);
                    ObjCmd.AddParameterWithValue("@HoldRSPCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.HoldRSPCode);
                    ObjCmd.AddParameterWithValue("@LoginID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjChecker.CheckerId);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjChecker.IssuerNo);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, ObjChecker.BankID);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, ObjChecker.SystemID);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
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
            if (ObjDTOutPut.Rows.Count > 0)
            {
                _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return _dtReturn;


        }

    }

}

