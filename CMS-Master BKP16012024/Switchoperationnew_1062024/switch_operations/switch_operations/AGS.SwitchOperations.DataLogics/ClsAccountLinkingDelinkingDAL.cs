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
    public class ClsAccountLinkingDelinkingDAL
    {

        //END SHEETAL
        //get card details account link delink
        public DataTable FunSearchCardDetails(ClsAccountLinkingBO ObjAcclink)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                //USP_GetCarddetails
                //Sp_GetSwitchAccountDetails
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetSwitchAccountDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CardNO", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.CardNo);
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, "");
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.SystemID);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.BankID);
                    ObjCmd.AddParameterWithValue("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, "");
                    ObjCmd.AddParameterWithValue("@AccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, "");
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.VarChar, 0, ParameterDirection.Input, "0");

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunSearchCardDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }


        //check cardexist
        public string FunCheckCardExist(ClsAccountLinkingBO ObjAcclink)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                //USP_CheckCardExist
                //USP_CheckCardExistInCardRpan
                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckCardExistInCardRpan", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@CardNo ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.CardNo);
                    ObjCmd.AddParameterWithValue("@BankId ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.BankID);
                    ObjCmd.AddParameterWithValue("@SystemId ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.SystemID);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunCheckCardExist()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                ObjReturnStatus = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return ObjReturnStatus;


        }
        //public DataTable FunGetSessionForBank(ClsAccountLinkingBO ObjAcclink, ClsAPIRequestBO APIrequestparam)
        public DataTable FunGetSessionForBank(string Sourceid)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                //ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                ClsCommonDAL.FunGetConnection(ref ObjConn, 2);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSessionIdForChannelBySource", ObjConn, CommandType.StoredProcedure))
                {

                    //ObjCmd.AddParameterWithValue("@Bankid", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.BankID);
                    ObjCmd.AddParameterWithValue("@SourceId", SqlDbType.VarChar, 0, ParameterDirection.Input, Sourceid);



                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunGetSessionForBank()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }


        public string FunLogAccountLinkelinkRequest(string CardNo, string AccountNo, string AccountType, string AccountQualifier, string LinkingFlag, string Response, string ResponseCode, string AccountLogId)
        {
            string ReturnLogId;
            SqlConnection ObjConn = null;
            try
            {
                //ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_LogAccountLinkDelinkRequest", ObjConn, CommandType.StoredProcedure))
                {

                    //ObjCmd.AddParameterWithValue("@Bankid", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.BankID);

                    //AccountLogId is blank it means this request
                    if (string.IsNullOrEmpty(AccountLogId))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, @CardNo);
                        ObjCmd.AddParameterWithValue("@AccounNo", SqlDbType.VarChar, 0, ParameterDirection.Input, @AccountNo);
                        ObjCmd.AddParameterWithValue("@AccountType", SqlDbType.VarChar, 0, ParameterDirection.Input, @AccountType);
                        ObjCmd.AddParameterWithValue("@AccountQualifier", SqlDbType.VarChar, 0, ParameterDirection.Input, @AccountQualifier);
                        ObjCmd.AddParameterWithValue("@LinkingFlag", SqlDbType.VarChar, 0, ParameterDirection.Input, @LinkingFlag);
                    }
                    else//Response
                    {
                        ObjCmd.AddParameterWithValue("@Response", SqlDbType.VarChar, 0, ParameterDirection.Input, @Response);
                        ObjCmd.AddParameterWithValue("@ResponseDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, @ResponseCode);
                        ObjCmd.AddParameterWithValue("@LogId", SqlDbType.VarChar, 0, ParameterDirection.Input, @AccountLogId);
                    }

                    ReturnLogId = Convert.ToString(ObjCmd.ExecuteScalar());
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ReturnLogId = "";
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunGetSessionForBank()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ReturnLogId;
        }


        public DataTable FunGetSourceIdForChannel(ClsAccountLinkingBO ObjAcclink)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSourceIdForChannel", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@Bankid", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.BankID);
                    //ObjCmd.AddParameterWithValue("@SourceId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAcclink.SourceId);



                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunGetSourceIdForChannel()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }


        public static void FunSyncAccountEnc(string BankId, string EncAcc, string Accno, string AccounType, string Currency)
        {
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_insertAccountEncDecFromAdditionofAcc", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@BankId", SqlDbType.VarChar, 0, ParameterDirection.Input, BankId);
                    ObjCmd.AddParameterWithValue("@EncAcc", SqlDbType.VarChar, 0, ParameterDirection.Input, EncAcc);
                    ObjCmd.AddParameterWithValue("@Accno", SqlDbType.VarChar, 0, ParameterDirection.Input, Accno);
                    ObjCmd.AddParameterWithValue("@AccounType", SqlDbType.VarChar, 0, ParameterDirection.Input, AccounType);
                    ObjCmd.AddParameterWithValue("@Currency", SqlDbType.VarChar, 0, ParameterDirection.Input, Currency);

                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunSyncAccountDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }

        public static void FunSyncCardAccountLinkingDetails(string CardNo, string IssuerNo, DataTable DT)
        {
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_SyncCardAccountLinkingDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, CardNo);
                    ObjCmd.AddParameterWithValue("@Type_CardAccountLinkingDetails", SqlDbType.Structured, 0, ParameterDirection.Input, DT);
                    
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunSyncAccountDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }
        public static DataTable FunSyncCardDetailsForAccountLinking(string CardNo, string CIF, string NIC, string Name, string IssuerNo)
        {
            SqlConnection ObjConn = null;
            DataTable dt = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_SyncCardDetailForAccLink", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, CardNo);
                    ObjCmd.AddParameterWithValue("@CIFID", SqlDbType.VarChar, 0, ParameterDirection.Input, CIF);
                    ObjCmd.AddParameterWithValue("@NIC", SqlDbType.VarChar, 0, ParameterDirection.Input, NIC);
                    ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, Name);
                    
                    dt = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunSyncAccountDetails()", ObjExc.Message, "");
                dt = null;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return dt;
        }
    }
}
