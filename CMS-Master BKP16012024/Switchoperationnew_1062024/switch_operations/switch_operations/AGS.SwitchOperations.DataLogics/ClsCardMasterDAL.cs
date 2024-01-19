using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public class ClsCardMasterDAL
    {

        //Search Customer  for card limit
        public DataTable FunSearchCustCardLimit(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSetCardLimit_ISO", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, 0);

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {

                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);

                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSearchCustCardLimit()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //save Card Limit
        public ClsReturnStatusBO FunSaveCardLimit(CustSearchFilter ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSetCardLimit_ISO", ObjConn, CommandType.StoredProcedure))
                {

                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);

                    //if (ObjFilter.CustomerID > 0)
                    //{
                    //    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    //}

                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }

                    //if (ObjFilter.Limit > 0)
                    //{
                    //    ObjCmd.AddParameterWithValue("@Limit", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.Limit);
                    //}
                    if (ObjFilter.MakerID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@CreatedByID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.MakerID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }

                    ObjCmd.AddParameterWithValue("@UpdateRemark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UpdateRemark);
                    ObjCmd.AddParameterWithValue("@PurchaseNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.PurNOTran);
                    ObjCmd.AddParameterWithValue("@PurchaseDailyLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.PurDailyLimit);
                    ObjCmd.AddParameterWithValue("@PurchasePTLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.PurPTLimit);
                    ObjCmd.AddParameterWithValue("@WithDrawNO", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.WithDrawNOTran);
                    ObjCmd.AddParameterWithValue("@WithDrawDailyLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.WithDrawDailyLimit);
                    ObjCmd.AddParameterWithValue("@WithDrawPTLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.WithDrawPTLimit);
                    ObjCmd.AddParameterWithValue("@PaymentNO", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.PaymentNOTran);
                    ObjCmd.AddParameterWithValue("@PaymentDailyLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.PaymentDailyLimit);
                    ObjCmd.AddParameterWithValue("@PaymentPTLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.PaymentPTLimit);
                    ObjCmd.AddParameterWithValue("@CNPPTLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.CNPPTLimit);
                    ObjCmd.AddParameterWithValue("@CNPDailyLimit", SqlDbType.Decimal, 0, ParameterDirection.Input, ObjFilter.CNPDailyLimit);
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
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
                        ObjReturnStatus.Description = "Card limit is not saved";
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSaveCardLimit()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Card limit is not saved";

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        //Search Customer for Card Ops
        public DataTable FunSearchCardDtl(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_GetSaveCardOpsRequest", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {

                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);

                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CustomerName);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.AccountNo))
                    {
                        ObjCmd.AddParameterWithValue("@AccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.AccountNo);
                    }


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

            return ObjDTOutPut;
        }

        //save Card Operation request
        public ClsReturnStatusBO FunSaveCardOpsReq(CustSearchFilter ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.sp_GetSaveCardOpsRequest", ObjConn, CommandType.StoredProcedure))
                {

                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);

                    if (string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }


                    if (ObjFilter.MakerID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@CreatedByID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.MakerID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }

                    ObjCmd.AddParameterWithValue("@ReqTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.RequestTypeID);

                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }

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
                        ObjReturnStatus.Description = "Card request is not saved";
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSaveCardOpsReq()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Card request is not saved";

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        //Search All Card Operations request
        public DataTable FunSearchCardRequests(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAllCardRequestsISO", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, 0);

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (ObjFilter.RequestTypeID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@RequestTypeID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.RequestTypeID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);

                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjFilter.IsAdmin);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserID);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSearchCardRequests()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //Accept reject Card Operations request
        public DataTable FunAccept_RejectCardReq(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;

                //Accept /Reject Set Card limit Req
                if (ObjCustomer.RequestTypeID == 1)
                {
                    SpName = "dbo.Sp_AcceptRejectCardLimit";
                }
                //Accept /Reject Card Operation Req
                else
                {
                    SpName = "dbo.Sp_AcceptRejectCardOpsReq";
                }

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {

                    //ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CustomerID);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CheckerId);
                    ObjCmd.AddParameterWithValue("@ReqID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.RequestIDs);
                    ObjCmd.AddParameterWithValue("@RequestTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.RequestTypeID);

                    if (!string.IsNullOrEmpty(ObjCustomer.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.Remark);
                    }
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.FormStatusID);
                    if (!string.IsNullOrEmpty(ObjCustomer.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.BankID);
                    }



                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunAccept_RejectCardReq()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        public DataTable FunGetSetCardDetailsForPinRepin(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;

                SpName = "dbo.USP_GetPINResetCardDetails";


                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@flag", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.PINResetFlag);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CheckerId);
                    ObjCmd.AddParameterWithValue("@ReqID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.RequestIDs);
                    ObjCmd.AddParameterWithValue("@RequestTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.RequestTypeID);

                    if (!string.IsNullOrEmpty(ObjCustomer.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@cardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.Remark);
                    }
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.FormStatusID);
                    if (!string.IsNullOrEmpty(ObjCustomer.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.BankID);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetSetCardDetailsForPinRepin()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //view Card Operations request 
        public DataTable FunGetCardRequestByID(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetCardOpsReqByID", ObjConn, CommandType.StoredProcedure))
                {
                    if (ObjFilter.IntPara > 0)
                    {
                        ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, 0);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (ObjFilter.RequestTypeID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ReqTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.RequestTypeID);
                    }
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt16(ObjFilter.SystemID));
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetCardRequestByID()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        public DataTable FunGetAuditReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, Before SqlStoredProcedure , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");

                //ClsCommonDAL.FunInsertIntoErrorLog("CS, TransactionReport, Inside FunGetAllReports Method", "", "");
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAuditReport", ObjConn, CommandType.StoredProcedure))
                {

                    if (ObjFilter.FromDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.FromDate);
                    }
                    if (ObjFilter.ToDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.ToDate);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);
                    }

                    //if (!string.IsNullOrEmpty(ObjFilter.UserBranchCode))
                    //{
                    //    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserBranchCode);
                    //}

                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }


                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, ExecuteDataTable , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetAuditReports()", Ex.ToString(), "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunGetActivityReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, Before SqlStoredProcedure , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");

                //ClsCommonDAL.FunInsertIntoErrorLog("CS, TransactionReport, Inside FunGetAllReports Method", "", "");
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetActivityReport", ObjConn, CommandType.StoredProcedure))
                {

                    //if (ObjFilter.FromDate != null)
                    //{
                    //    ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.FromDate);
                    //}
                    //if (ObjFilter.ToDate != null)
                    //{
                    //    ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.ToDate);
                    //}

                    if (!string.IsNullOrEmpty(ObjFilter.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.UserBranchCode))
                    {
                        ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserBranchCode);
                    }


                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, ExecuteDataTable , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetActivityReports()", Ex.ToString(), "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //Get Reports
        public DataTable FunGetAllReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;
                //for txn Reports
                if (ObjFilter.IntPara == 1)
                {
                    SpName = "dbo.SP_GetTxnReport";
                }
                //for personalized Report
                else
                {
                    SpName = "dbo.SP_GetAllReports";
                }
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, Before SqlStoredProcedure , Cardno:" + ObjFilter.CardNo + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");

                //ClsCommonDAL.FunInsertIntoErrorLog("CS, TransactionReport, Inside FunGetAllReports Method", "", "");
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {

                    if (ObjFilter.FromDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.FromDate);
                    }
                    if (ObjFilter.ToDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.ToDate);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt16(ObjFilter.SystemID));
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    // for txn report
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@CustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }


                    // for personalized report
                    if (ObjFilter.IntPara == 0)
                    {
                        ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                        if (!string.IsNullOrEmpty(ObjFilter.ProductType))
                        {
                            ObjCmd.AddParameterWithValue("@ProductType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ProductType);
                        }
                        if (!string.IsNullOrEmpty(ObjFilter.Status))
                        {
                            ObjCmd.AddParameterWithValue("@Status", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.Status);
                        }
                    }
                    // For Activity Log Report
                    else if (ObjFilter.IntPara == 2)
                    {
                        ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                        ObjCmd.AddParameterWithValue("@ProductType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ProductType);
                    }

                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, ExecuteDataTable , Cardno:" + ObjFilter.CardNo + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetAllReports()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //start sheetal
        //1-3-18
        public DataTable FunGetCustCardDetailsFromSwitch(CustSearchFilter objSearch)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetCustCardByCustomerId", ObjConn, CommandType.StoredProcedure))
                {



                    //ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input,objSearch.CardNo);
                    //ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.SystemID);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.BankID);
                    ObjCmd.AddParameterWithValue("@CustIdentity", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.CustomerID);
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, objSearch.BankCustID);





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

            return ObjDTOutPut;
        }

        public DataTable FunSearchCardDtlISO(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSaveCardOpsRequest_ISO", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {

                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);

                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CustomerName);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.AccountNo))
                    {
                        ObjCmd.AddParameterWithValue("@AccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.AccountNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NIC))
                    {
                        ObjCmd.AddParameterWithValue("@NICNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NIC);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CIF))
                    {
                        ObjCmd.AddParameterWithValue("@CIF", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CIF);
                    }


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

            return ObjDTOutPut;
        }

        public ClsReturnStatusBO FunSaveCardOpsReqISO(CustSearchFilter ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSaveCardOpsRequest_ISO", ObjConn, CommandType.StoredProcedure))
                {

                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }


                    if (ObjFilter.MakerID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@CreatedByID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.MakerID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }

                    ObjCmd.AddParameterWithValue("@ReqTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.RequestTypeID);

                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@UpdateRemark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.Remark);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjFilter.IsAdmin);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.UserID);

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
                        ObjReturnStatus.Description = "Card request is not saved";
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunSaveCardOpsReq()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Card request is not saved";

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        public DataTable FunGetOpsDataForISO(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;

                SpName = "dbo.USP_GetSetCardOperationDataISO";

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ActionType", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.ActionType);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Requestids", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.RequestIDs);
                    if (!string.IsNullOrEmpty(ObjCustomer.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.BankID);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@RejectReason", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.Remark);
                    }

                    if (!string.IsNullOrEmpty(ObjCustomer.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.CustomerID);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(ObjCustomer.MakerID)))
                    {
                        ObjCmd.AddParameterWithValue("@MakerID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.MakerID);
                    }

                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.CheckerId);
                    ObjCmd.AddParameterWithValue("@ISORSPCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.ISORSPCode);
                    ObjCmd.AddParameterWithValue("@ISORSPDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.ISORSPDesc);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, ObjCustomer.IsAdmin);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.UserID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetOpsDataForISO()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        public DataTable GetCardAPISourceIdDetails(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;

                SpName = "dbo.USP_GetCardAPISourceIdDetails";

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ActionType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.APIRequest);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToUInt32(ObjCustomer.IssuerNo));
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToUInt32(ObjCustomer.RequestTypeID));
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, GetCardAPISourceIdDetails()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        public DataTable FunGetCardRequestByIDISO(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCardOpsReqByID", ObjConn, CommandType.StoredProcedure))
                {
                    if (ObjFilter.IntPara > 0)
                    {
                        ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, 0);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (ObjFilter.RequestTypeID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ReqTypeID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.RequestTypeID);
                    }
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt16(ObjFilter.SystemID));
                    }
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(ObjFilter.IssuerNo));

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetCardRequestByID()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunGetAllReportsEPS(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;
                //for txn Reports
                if (ObjFilter.IntPara == 1)
                {
                    SpName = "dbo.USP_GetTxnReport_EPS";
                }
                //for personalized Report
                else
                {
                    SpName = "dbo.SP_GetAllReports";
                }

                //ClsCommonDAL.FunInsertIntoErrorLog("CS, TransactionReport, Inside FunGetAllReports Method", "", "");
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {

                    if (ObjFilter.FromDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.FromDate);
                    }
                    if (ObjFilter.ToDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.ToDate);
                    }
                    // for txn report
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@CustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }

                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt16(ObjFilter.IssuerNo));

                    //if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    //{
                    //    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    //}
                    // for personalized report
                    //added by uddesh ATPCM-656 start
                    if (ObjFilter.IntPara == 1)
                    {
                        if (!string.IsNullOrEmpty(ObjFilter.RRN))
                        {
                            ObjCmd.AddParameterWithValue("@RRN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.RRN);
                        }
                    }
                    //added by uddesh ATPCM-656 end
                    if (ObjFilter.IntPara == 0)
                    {
                        ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                        if (!string.IsNullOrEmpty(ObjFilter.ProductType))
                        {
                            ObjCmd.AddParameterWithValue("@ProductType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ProductType);
                        }
                        if (!string.IsNullOrEmpty(ObjFilter.Status))
                        {
                            ObjCmd.AddParameterWithValue("@Status", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.Status);
                        }
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetAllReportsEPS()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunGetUserIdActivityReports(string BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //ClsCommonDAL.FunInsertIntoErrorLog("CS, TransactionReport, Inside FunGetAllReports Method", "", "");
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetUserIdActivityReports", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt16(BankID));

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetAllReportsEPS()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable GetCardByCIFID(CustSearchFilter ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;

            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;

                SpName = "dbo.USP_GetCardByCIFID";

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToUInt32(ObjCustomer.IssuerNo));
                    ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.CardNo);
                    ObjCmd.AddParameterWithValue("@CIF", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.CIF);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, GetCardByCIFID()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }


        public DataTable GetPinOpertionsReports(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, Before SqlStoredProcedure , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.[USP_GetPinOpertionsReport]", ObjConn, CommandType.StoredProcedure))
                {

                    if (ObjFilter.FromDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.FromDate);
                    }
                    if (ObjFilter.ToDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.ToDate);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);
                    }


                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, ExecuteDataTable , UserBranchCode:" + ObjFilter.UserBranchCode + " ,FromDate:" + ObjFilter.FromDate + " , TODate:" + ObjFilter.ToDate, "", "");
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, GetPinOpertionsReports()", Ex.ToString(), "");
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