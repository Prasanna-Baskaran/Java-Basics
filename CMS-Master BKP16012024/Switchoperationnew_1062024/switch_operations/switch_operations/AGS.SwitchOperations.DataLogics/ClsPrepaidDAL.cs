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
    public class ClsPrepaidDAL : IDisposable
    {

        public  int FunSaveRequestLog(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {

            int Code = 0;
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_RequestResponseLog", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@StrRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrRequest);
                    ObjCmd.AddParameterWithValue("@StrFunctionName", SqlDbType.VarChar, 0, ParameterDirection.Input, StrFunctionName);

                    if (!string.IsNullOrEmpty(StrMessageOutput))
                    {
                        ObjCmd.AddParameterWithValue("@StrResponseData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrMessageOutput);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);

                    }
                    else
                    {
                        Code = 0;
                    }
                }
            }

            catch
            {
                Code = 0;
            }

            return Code;
        }

        public  ClsReturnStatusBO FunAuthenticateCustomer(ClsLogin ObjLogin)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_AuthenticateCustomer", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@StrUsername", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjLogin.Username);
                    ObjCmd.AddParameterWithValue("@StrPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjLogin.Password);


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjResult.Description = ObjDTOutPut.Rows[0]["Description"].ToString();
                    }
                    else
                    {
                        ObjResult.Code = 1;
                        ObjResult.Description = "Login failed";

                    }
                }
            }

            catch (Exception ObjExc)
            {
                ObjResult.Code = 1;
                ObjResult.Description = ObjExc.Message;
            }
            return ObjResult;
        }


        public  DataTable CustomerRegisteration(string USERTYPE, string CARDNO, string USERNAME, string FIRSTNAME, string LASTNAME, string EMAILADDRS, string Q1, string Q2, string CVV, Int64 LogUser)
        {
            SqlConnection oConn = null;
            DataTable dtReturn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref oConn, 3);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Usp_CustomerRegisteration", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@USERTYPE", SqlDbType.VarChar, 0, ParameterDirection.Input, USERTYPE);
                    sspObj.AddParameterWithValue("@CARDNO", SqlDbType.VarChar, 0, ParameterDirection.Input, CARDNO);
                    sspObj.AddParameterWithValue("@USERNAME", SqlDbType.VarChar, 0, ParameterDirection.Input, USERNAME);
                    sspObj.AddParameterWithValue("@FIRSTNAME", SqlDbType.VarChar, 0, ParameterDirection.Input, FIRSTNAME);
                    sspObj.AddParameterWithValue("@LASTNAME", SqlDbType.VarChar, 0, ParameterDirection.Input, LASTNAME);
                    sspObj.AddParameterWithValue("@EMAILADDRS", SqlDbType.VarChar, 0, ParameterDirection.Input, EMAILADDRS);
                    sspObj.AddParameterWithValue("@Q1", SqlDbType.VarChar, 0, ParameterDirection.Input, Q1);
                    sspObj.AddParameterWithValue("@Q2", SqlDbType.VarChar, 0, ParameterDirection.Input, Q2);
                    sspObj.AddParameterWithValue("@CVV", SqlDbType.VarChar, 0, ParameterDirection.Input, CVV);
                    sspObj.AddParameterWithValue("@LogUser", SqlDbType.Int, 0, ParameterDirection.Input, LogUser);
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch
            {
            }
            finally
            {
                ClsCommonDAL.FuncloseConnection(ref oConn);
            }
            return dtReturn;
        }


        public  bool PrepaidModuleLog(string StrPriTransactionType, string StrPriRequestData, string StrPriOutPutData)
        {
            Boolean bReturn = true;
            SqlConnection oConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref oConn, 3);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpMvisaLog", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrPriTransactionType", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriTransactionType);
                    sspObj.AddParameterWithValue("@StrPriRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriRequestData);
                    sspObj.AddParameterWithValue("@StrPriOutPutData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriOutPutData);

                    sspObj.ExecuteNonQuery();

                    sspObj.Dispose();

                }
            }
            catch
            {
                bReturn = false;
            }
            finally
            {
                ClsCommonDAL.FuncloseConnection(ref oConn);
            }

            return bReturn;
        }

        public  int FunTranTemp(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {

            int Code = 0;
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_TranTemp", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@StrRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrRequest);
                    ObjCmd.AddParameterWithValue("@StrFunctionName", SqlDbType.VarChar, 0, ParameterDirection.Input, StrFunctionName);

                    if (!string.IsNullOrEmpty(StrMessageOutput))
                    {
                        ObjCmd.AddParameterWithValue("@StrResponseData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrMessageOutput);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);

                    }
                    else
                    {
                        Code = 0;
                    }
                }
            }

            catch
            {
                Code = 0;
            }

            return Code;
        }

        public  int FunTranDetails(string StrFunctionName, string StrRequest, string StrMessageOutput)
        {

            int Code = 0;
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_RequestResponseLog", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@StrRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrRequest);
                    ObjCmd.AddParameterWithValue("@StrFunctionName", SqlDbType.VarChar, 0, ParameterDirection.Input, StrFunctionName);

                    if (!string.IsNullOrEmpty(StrMessageOutput))
                    {
                        ObjCmd.AddParameterWithValue("@StrResponseData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrMessageOutput);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);

                    }
                    else
                    {
                        Code = 0;
                    }
                }
            }

            catch
            {
                Code = 0;
            }

            return Code;
        }


        public  string changePasswordApp(string UserName, string OldPassword, string NewPassword, string CONFPASSWD)
        {
            string Status = "Suc";
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_changePassword", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, UserName);
                    sspObj.AddParameterWithValue("@OldUserPass", SqlDbType.VarChar, 0, ParameterDirection.Input, OldPassword);
                    sspObj.AddParameterWithValue("@NewUserPass", SqlDbType.VarChar, 0, ParameterDirection.Input, NewPassword);
                    sspObj.AddParameterWithValue("@CONFPASSWD", SqlDbType.VarChar, 0, ParameterDirection.Input, CONFPASSWD);

                    sspObj.ExecuteNonQuery();

                    sspObj.Dispose();
                }
            }
            catch (Exception xObj)
            {
                Status = Convert.ToString(xObj.Message);
            }

            return Status;
        }

        //insert prepaid txn in pre txn
        public  ClsReturnStatusBO Fun_InsertPreTxnDtl(ClsTransaction ObjTxn)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            ObjResult.OutPutCode = "0";
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_SavePreTransactionDetails", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@TerminalID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.TerminalID);
                    ObjCmd.AddParameterWithValue("@INSTID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.INSTID);
                    ObjCmd.AddParameterWithValue("@FromCardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.FromCardNo);
                    if (!string.IsNullOrEmpty(ObjTxn.ToCardNo))
                    {
                        ObjCmd.AddParameterWithValue("@ToCardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.ToCardNo);
                    }

                    if (!string.IsNullOrEmpty(ObjTxn.ToAccountNumber))
                    {
                        ObjCmd.AddParameterWithValue("@ToAccountNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.ToAccountNumber);
                    }
                    ObjCmd.AddParameterWithValue("@InputStream", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.InputStream);

                    ObjCmd.AddParameterWithValue("@TxnAmount", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjTxn.TxnAmount);
                    ObjCmd.AddParameterWithValue("@TxnTypeID",SqlDbType.Int,0,ParameterDirection.Input,ObjTxn.TxnTypeID);


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjResult.Description = ObjDTOutPut.Rows[0]["Description"].ToString();
                        ObjResult.OutPutCode= ObjDTOutPut.Rows[0]["OutPutCode"].ToString();
                    }
                    else
                    {
                        ObjResult.Code = 1;
                        ObjResult.OutPutCode = "0";
                        ObjResult.Description = "Transaction details are not saved";

                    }
                }

            }
            catch (Exception ObjExc)
            {
                ObjResult.Code = 98;
                ObjResult.Description = ObjExc.Message;
                ObjResult.OutPutCode = "0";
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsPrepaidDAL, Fun_InsertPreTxnDtl()", ObjExc.Message, "");
            }
            return ObjResult;


        }

        //insert prepaid txn in post txn
        public  ClsReturnStatusBO Fun_InsertPostTxnDtl(ClsTransaction ObjTxn)
        {
            ClsReturnStatusBO ObjResult = new ClsReturnStatusBO();
            ObjResult.Code = 1;
            ObjResult.Description = "Failed";
            ObjResult.OutPutCode = "0";
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_SavePostTransactionDetails", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@TxnTempID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjTxn.TxnTempID);
                    ObjCmd.AddParameterWithValue("@SwitchRRN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchRRN);
                    ObjCmd.AddParameterWithValue("@SwitchRSPCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchRSPCode);
                    ObjCmd.AddParameterWithValue("@SwitchProcsCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchProcsCode);
                    ObjCmd.AddParameterWithValue("@SwitchAmount", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchAmount);
                    ObjCmd.AddParameterWithValue("@SwitchStan", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchStan);
                    ObjCmd.AddParameterWithValue("@SwitchTime", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchTime);
                    ObjCmd.AddParameterWithValue("@SwitchDate", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchDate);
                    ObjCmd.AddParameterWithValue("@SwitchTeminalID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchTeminalID);
                    ObjCmd.AddParameterWithValue("@SwitchTransmissionDate", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjTxn.SwitchTransmissionDate);               
                    

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjResult.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjResult.Description = ObjDTOutPut.Rows[0]["Description"].ToString();
                        ObjResult.OutPutCode = ObjDTOutPut.Rows[0]["OutPutCode"].ToString();
                    }
                    else
                    {
                        ObjResult.Code = 1;
                        ObjResult.Description = "Transaction details are not saved";
                        ObjResult.OutPutCode = "0";
                    }
                }
            }
            catch (Exception ObjExc)
            {
                ObjResult.Code = 98;
                ObjResult.Description = ObjExc.Message;
                ObjResult.OutPutCode = "0";
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsPrepaidDAL, Fun_InsertPostTxnDtl()", ObjExc.Message, "");
            }
            return ObjResult;


        }

        //Fun Reversal 
        public  DataTable Fun_Reversal(ClsReverseTxnDtl ObjRev)
        {
            DataTable dtReturn;
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SP_InsertPrepaidRevesalTranLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrReqISOString", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.ISOString);
                    sspObj.AddParameterWithValue("@StrReqSwitchStan", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchStan);
                    sspObj.AddParameterWithValue("@StrReqTerminalID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.TerminalID);
                    sspObj.AddParameterWithValue("@StrReqTimeVal", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchTime);
                    sspObj.AddParameterWithValue("@StrReqDateVal", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchDate);
                    sspObj.AddParameterWithValue("@StrReqDateTimeVal", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchDateTime);
                    sspObj.AddParameterWithValue("@StrReversalRSPString", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchReversalRspString);
                    sspObj.AddParameterWithValue("@StrReqSwtichRRN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchRRN);
                    sspObj.AddParameterWithValue("@StrReversedResponseCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchReverseRSPCode);                    
                    sspObj.AddParameterWithValue("@ReversedAmount", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchReverseAmount);
                    sspObj.AddParameterWithValue("@ReversalRemark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.ReversalRemark);
                    sspObj.AddParameterWithValue("@Post_TxnID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjRev.Post_TxnID);
                    sspObj.AddParameterWithValue("@StrReversedRRN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjRev.SwitchReverseRRN);
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception xObj)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsPrepaidDAL, Fun_Reversal()", xObj.Message, "StrISOString=" + Convert.ToString(ObjRev.ISOString) + ",StrReversalRspString=" + Convert.ToString(ObjRev.SwitchReversalRspString) +
                     ",StrSwtichRRN=" + Convert.ToString(ObjRev.SwitchRRN) + ",StrReversedResponseCode=" + Convert.ToString(ObjRev.SwitchReverseRSPCode) +
                     ",StrReversedRRN=" + Convert.ToString(ObjRev.SwitchReverseRRN));
               
                dtReturn = new DataTable();
            }
            return dtReturn;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}