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
using System.Text;


namespace AGS.SwitchOperations.Common
{
    public class ClsCommonDAL
    {
        public static bool FunLog(string StrPriTransactionType, string StrPriRequestData, string StrPriOutPutData)
        {
            Boolean bReturn = true;
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrPriTransactionType", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriTransactionType);
                    sspObj.AddParameterWithValue("@StrPriRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriRequestData);
                    sspObj.AddParameterWithValue("@StrPriOutPutData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriOutPutData);

                    sspObj.ExecuteNonQuery();

                    sspObj.Dispose();
                    ///scsc/
                }
            }
            catch
            {
                bReturn = false;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return bReturn;
        }


        public static ClsReturnStatusBO GetBankLogo()
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();

            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);

                String StrUsrSessionKey = Create16DigitString();

                DataTable ObjdtOutPut = new DataTable();
                SqlCommand sqlComm = new SqlCommand("SpGetBankLogo", ObjConn);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ObjdtOutPut);
                ObjReturnStatus.BankLogoPath = ObjdtOutPut.Rows[0]["BankLogoPath"].ToString();
            }

            catch (Exception ObjExc)
            {
                ObjReturnStatus.Description = ObjExc.Message;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunLoginValidate()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }




        public static ClsReturnStatusBO FunLoginValidate(ClsLoginBO ObjPriLoginBO)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Login failed";

            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);

                String StrUsrSessionKey = Create16DigitString();

                DataSet ObjDSOutPut = new DataSet();
                SqlCommand sqlComm = new SqlCommand("SpValidateUserLogin", ObjConn);
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.Add("@StrUserName", SqlDbType.VarChar);
                sqlComm.Parameters["@StrUserName"].Value = ObjPriLoginBO.UserName.Trim();
                sqlComm.Parameters.Add("@StrUserPassword", SqlDbType.VarChar);
                sqlComm.Parameters["@StrUserPassword"].Value = ObjPriLoginBO.Password.Trim();
                sqlComm.Parameters.Add("@StrSessionKey", SqlDbType.VarChar);
                sqlComm.Parameters["@StrSessionKey"].Value = StrUsrSessionKey.Trim();
               
                sqlComm.Parameters.Add("@BankID", SqlDbType.VarChar);
                sqlComm.Parameters["@BankID"].Value = ObjPriLoginBO.BankID;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ObjDSOutPut);

                if (ObjDSOutPut.Tables.Count > 1)
                {
                    DataTable DtUserDtl = ObjDSOutPut.Tables[0];
                    DataTable DtUserRightsDtl = ObjDSOutPut.Tables[1];
                    if (DtUserDtl.Rows[0]["Code"].ToString() == "0")
                    {
                        ObjPriLoginBO.UserID = DtUserDtl.Rows[0]["UserID"].ToString();
                        ObjPriLoginBO.BankCode = DtUserDtl.Rows[0]["BranchCode"].ToString();
                        ObjPriLoginBO.IsAdmin = Convert.ToBoolean(DtUserDtl.Rows[0]["IsAdmin"]);
                        ObjPriLoginBO.FirstName = DtUserDtl.Rows[0]["FirstName"].ToString();
                        ObjPriLoginBO.Lastname = DtUserDtl.Rows[0]["LastName"].ToString();
                        ObjPriLoginBO.UserRoleID = DtUserDtl.Rows[0]["UserRoleID"].ToString();
                        ObjPriLoginBO.MobileNo = DtUserDtl.Rows[0]["MobileNo"].ToString();
                        ObjPriLoginBO.EmailId = DtUserDtl.Rows[0]["EmailId"].ToString();
                        ObjPriLoginBO.SystemID = DtUserDtl.Rows[0]["SystemID"].ToString();
                        ObjPriLoginBO.BankID = DtUserDtl.Rows[0]["BankID"].ToString();
                        ObjPriLoginBO.BankLogoPath = DtUserDtl.Rows[0]["BankLogoPath"].ToString();
                        ObjPriLoginBO.LoginPagePath = DtUserDtl.Rows[0]["LoginPagePath"].ToString();


                        ObjPriLoginBO.SystemName = DtUserDtl.Rows[0]["SystemName"].ToString();
                        ObjPriLoginBO.AuthKey = DtUserDtl.Rows[0]["AuthKey"].ToString();
                        ObjPriLoginBO.UserPrefix = DtUserDtl.Rows[0]["UserPrefix"].ToString();
                        //Sourceid added
                        ObjPriLoginBO.SourceId = DtUserDtl.Rows[0]["SourceId"].ToString();
                        //Sheetal change for Bankname logo
                        ObjPriLoginBO.BankName = DtUserDtl.Rows[0]["BankName"].ToString();

                        ObjPriLoginBO.IssuerNo = DtUserDtl.Rows[0]["IssuerNo"].ToString();
                        ObjPriLoginBO.IsEPS = Convert.ToInt32(DtUserDtl.Rows[0]["IsEPS"]);
                        ObjPriLoginBO.iInstaEdit = Convert.ToInt32(DtUserDtl.Rows[0]["iInstaEdit"]); //added for ATPCM-759
                        ObjReturnStatus.Code = 0;

                        ObjPriLoginBO.ParticipantId = Convert.ToInt32(DtUserDtl.Rows[0]["ParticipantId"]);
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "User Not Register !";
                    }
                }
                else
                {
                    ObjReturnStatus.Code = 1;
                    ObjReturnStatus.Description = ObjDSOutPut.Tables[0].Rows[0]["OutputDescription"].ToString();
                }
            }

            catch (Exception ObjExc)
            {

                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = ObjExc.Message;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunLoginValidate()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        public static void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_InsertErrorLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    sspObj.AddParameterWithValue("@parameterList", SqlDbType.VarChar, 0, ParameterDirection.Input, parameterList);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }

        public static void FunInsertIntoISOLog(string StrFunName, string StrPriParam, string StrISOString, string StrOutput)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SPSetISOLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrFunName", SqlDbType.VarChar, 200, ParameterDirection.Input, StrFunName);
                    sspObj.AddParameterWithValue("@StrParam", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriParam);
                    sspObj.AddParameterWithValue("@StrISO", SqlDbType.VarChar, 0, ParameterDirection.Input, StrISOString);
                    sspObj.AddParameterWithValue("@StrOutput", SqlDbType.VarChar, 0, ParameterDirection.Input, StrOutput);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }

        public static DataTable FunGetCommonDataTable(Int32 IntPriContextKey, string StrPriPara)
        {
            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                string[] StrPriArrParam = { };
                if (StrPriPara != "")
                {
                    StrPriPara = StrPriPara + ",";
                    StrPriArrParam = StrPriPara.Split(',');
                }


                FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SpCommonGetDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPriContextKey", SqlDbType.VarChar, 0, ParameterDirection.Input, IntPriContextKey);
                    if (StrPriPara != "")
                    {
                        if (StrPriArrParam.Count() > 0)
                        {
                            for (Int16 IntPriParaCnt = 0; IntPriParaCnt < StrPriArrParam.Count(); IntPriParaCnt++)
                            {
                                if ((StrPriArrParam[IntPriParaCnt] != "") && (StrPriArrParam[IntPriParaCnt] != "''"))
                                {
                                    ObjCmd.AddParameterWithValue(("@StrPriPara" + (IntPriParaCnt + 1)), SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriArrParam[IntPriParaCnt].Trim());
                                }
                            }
                        }
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                }
            }

            catch (Exception ObjExc)
            {
                // throw ObjExc;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetCommonDataTable()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        private static string Create16DigitString()
        {
            var builder = new StringBuilder();
            try
            {
                Random RNG = new Random();
                while (builder.Length < 16)
                {
                    builder.Append(RNG.Next(10).ToString());
                }
            }
            catch
            {
                builder.Append("");
            }
            return builder.ToString();
        }

        public static void FunGetConnection(ref SqlConnection Connection, int Source)
        {
            switch (Source)
            {
                case 1:
                    Connection = ConfigManager.GetRBSQLDBOLAPConnection;
                    break;
                case 2:
                    Connection = ConfigManager.GetRBSQLDBAPIConnection;
                    break;
                ///*Added for international usage RBL-ATPCM-862* START/
                case 3:
                    Connection = ConfigManager.GetRBSQLDBTEMPPOConnection;
                    break;
                ///*Added for international usage RBL-ATPCM-862* END/
            }
        }

        public static void FuncloseConnection(ref SqlConnection Connection)
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Dispose();
            }
            Connection = null;
        }

        public static string FunGetParameter()
        {
            string StrConfigDtl = string.Empty;
            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {

                FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SpGetParam", ObjConn, CommandType.StoredProcedure))
                {
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut != null)
                    {
                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            StrConfigDtl = ObjDTOutPut.Rows[0][0].ToString() + "|" + ObjDTOutPut.Rows[0][1].ToString();
                        }
                    }
                    ObjCmd.Dispose();
                }
            }

            catch (Exception ObjExc)
            {
                throw ObjExc;
            }

            return StrConfigDtl;
        }

        //Encrypt Pin
        public static string FunEncryptPin(string CardNo, string Pin)
        {
            CardNo = CardNo.Trim();
            string StrResult = string.Empty;
            string StrCardNo = string.Empty;
            string StrPinCnt = string.Empty;
            string strOutput = string.Empty;
            try
            {
                //string StrZPK = "35D24122ACEF17DE20CDCC61442BC525";
                StrCardNo = CardNo.Substring(0, CardNo.Length - 1); //remove last digit
                StrCardNo = StrCardNo.Substring(StrCardNo.Length - 12, 12);  //get last 12 digits


                string strInputC1 = (Convert.ToString(Pin.Count()).PadLeft(2, '0') + Pin).PadRight(16, 'F'); //combination pin count +pin

                string strInputC2 = StrCardNo.PadLeft(16, '0');  //Card No  12 Digits

                long strInput = Convert.ToInt64(strInputC1, 16) ^ Convert.ToInt64(strInputC2, 16);  //XOR Operation

                //strOutput = TripleDes.Encrypt(strInput.ToString(), true);    //Final o/p
            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunEncryptPin()", Ex.Message, "CardNo= " + CardNo + " ,Pin= " + Pin);
            }
            return strOutput;
        }

        #region UserActivity_DBLog  <Nagendra Tiwari>
        public static bool UserActivity_DBLog(string LoginID, string UsrNm, string Page_Name,
                                                string Event_Name = "", string Event_Desc = "", string AddInfo = "", string Remarks = "")
        {
            bool IsSuccess = true;
            SqlConnection ObjConn = null;

            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_UsrActivityLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@LoginID", SqlDbType.VarChar, 0, ParameterDirection.Input, LoginID);
                    sspObj.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, UsrNm);
                    sspObj.AddParameterWithValue("@PageName", SqlDbType.VarChar, 0, ParameterDirection.Input, Page_Name);
                    sspObj.AddParameterWithValue("@EventName", SqlDbType.VarChar, 0, ParameterDirection.Input, Event_Name);
                    sspObj.AddParameterWithValue("@EventDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, Event_Desc);
                    sspObj.AddParameterWithValue("@AddInfo", SqlDbType.VarChar, 0, ParameterDirection.Input, AddInfo);

                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                bool temp = FunLog("User Activity Log", "LoginID=" + LoginID + "|Username=" + UsrNm + "|PageName=" + Page_Name + "|EventName=" + Event_Name +
                                                        "|EventDesc=" + Event_Desc + "|AddInfo=" + AddInfo + "|Remarks=" + Remarks, "Error while loggin UserActivity=> " + ex.Message);
            }
            return IsSuccess;
        }
        #endregion

        public static DataTable FunGetCommonCardDetails(Int32 ActionType, string BankId)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCommonCardDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ActionType", SqlDbType.Int, 0, ParameterDirection.Input, ActionType);
                    ObjCmd.AddParameterWithValue("@BankId", SqlDbType.VarChar, 0, ParameterDirection.Input, BankId);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                }
            }

            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetCommonDataTable()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        public static DataTable FunAcceptRejectCardRequestDetails(ClsProcessCardRequest _ClsProcessCardRequest)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_AcceptRejectCardRequest", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ActionType", SqlDbType.Int, 0, ParameterDirection.Input, _ClsProcessCardRequest.ActionType);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.IssuerNo);
                    ObjCmd.AddParameterWithValue("@BankId", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.BankId);
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.Int, 0, ParameterDirection.Input, _ClsProcessCardRequest.RequestType);
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.Code);
                    ObjCmd.AddParameterWithValue("@ISORspCode", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.ISORSPCode);
                    ObjCmd.AddParameterWithValue("@ISORspDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.ISORSPDesc);
                    ObjCmd.AddParameterWithValue("@LoginId", SqlDbType.Int, 0, ParameterDirection.Input, _ClsProcessCardRequest.LoginId);
                    ObjCmd.AddParameterWithValue("@isInsta", SqlDbType.Bit, 0, ParameterDirection.Input, _ClsProcessCardRequest.iInstaEdit == "3" ? 1 : 0);
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.UserBranchCode);
                    ObjCmd.AddParameterWithValue("@IsAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, _ClsProcessCardRequest.IsAdmin);
                    ObjCmd.AddParameterWithValue("@UserID", SqlDbType.VarChar, 0, ParameterDirection.Input, _ClsProcessCardRequest.UserID);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    _ClsProcessCardRequest.ErrorFlag = true;
                    ObjCmd.Dispose();
                }
            }
            catch (Exception ObjExc)
            {
                _ClsProcessCardRequest.ErrorFlag = false;
                _ClsProcessCardRequest.ErrorDesc = ObjExc.Message;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetCardRequestDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public static DataTable FunGetSFTPDetailsToPlaceCIF(string IssuerNo, int ProcessId)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_SFTPDetailsToProcessPortalRecords", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.AddParameterWithValue("@ProcessId", SqlDbType.Int, 0, ParameterDirection.Input, ProcessId);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetSFTPDetailsToPlaceCIF()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public static void FunInsertPortalISOLog(string TranType, string Param1, string Param2, string Param3, string Message, string MessageData, int LoginId)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_PortalISOLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@TranType", SqlDbType.VarChar, 0, ParameterDirection.Input, TranType);
                    sspObj.AddParameterWithValue("@Param1", SqlDbType.VarChar, 0, ParameterDirection.Input, Param1);
                    sspObj.AddParameterWithValue("@Param2", SqlDbType.VarChar, 0, ParameterDirection.Input, Param2);
                    sspObj.AddParameterWithValue("@Param3", SqlDbType.VarChar, 0, ParameterDirection.Input, Param3);
                    sspObj.AddParameterWithValue("@Message", SqlDbType.VarChar, 0, ParameterDirection.Input, Message);
                    sspObj.AddParameterWithValue("@MessageData", SqlDbType.VarChar, 0, ParameterDirection.Input, MessageData);
                    sspObj.AddParameterWithValue("@LoginId", SqlDbType.Int, 0, ParameterDirection.Input, LoginId);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
        }

        public void FunSOAL(string UserId, string IssuerNo, string OptionName, string Remark, string CardNo, string CustomerId, string CustomerName, string Mobile, string AccountNo, string NICNo, string Action, string Result)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_SwitchOperationActivityLog", ObjConn, CommandType.StoredProcedure))
                {

                    if (!string.IsNullOrEmpty(UserId))
                    {
                        ObjCmd.AddParameterWithValue("@UserId", SqlDbType.VarChar, 0, ParameterDirection.Input, UserId);
                    }
                    if (!string.IsNullOrEmpty(IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    }
                    if (!string.IsNullOrEmpty(OptionName))
                    {
                        ObjCmd.AddParameterWithValue("@OptionName", SqlDbType.VarChar, 0, ParameterDirection.Input, OptionName);
                    }
                    if (!string.IsNullOrEmpty(Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, Remark);
                    }
                    if (!string.IsNullOrEmpty(Mobile))
                    {
                        ObjCmd.AddParameterWithValue("@Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile);
                    }
                    if (!string.IsNullOrEmpty(AccountNo))
                    {
                        ObjCmd.AddParameterWithValue("@AccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, AccountNo);
                    }
                    if (!string.IsNullOrEmpty(NICNo))
                    {
                        ObjCmd.AddParameterWithValue("@NICNo", SqlDbType.VarChar, 0, ParameterDirection.Input, NICNo);
                    }
                    if (!string.IsNullOrEmpty(Action))
                    {
                        ObjCmd.AddParameterWithValue("@Action", SqlDbType.VarChar, 0, ParameterDirection.Input, Action);
                    }
                    if (!string.IsNullOrEmpty(Result))
                    {
                        ObjCmd.AddParameterWithValue("@Result", SqlDbType.VarChar, 0, ParameterDirection.Input, Result);
                    }
                    if (!string.IsNullOrEmpty(CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerName);
                    }
                    if (!string.IsNullOrEmpty(CustomerId))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerId", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerId);
                    }
                    if (!string.IsNullOrEmpty(CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt16(CardNo));
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
        }

        public void FunUMAL(string UserId, string IssuerNo, string OptionName, string Remark, string ProfileId, string Action, string Result)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UserModuleActivityLog", ObjConn, CommandType.StoredProcedure))
                {

                    if (!string.IsNullOrEmpty(UserId))
                    {
                        ObjCmd.AddParameterWithValue("@UserId", SqlDbType.Int, 0, ParameterDirection.Input, UserId);
                    }
                    if (!string.IsNullOrEmpty(IssuerNo))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);
                    }
                    if (!string.IsNullOrEmpty(OptionName))
                    {
                        ObjCmd.AddParameterWithValue("@OptionName", SqlDbType.VarChar, 0, ParameterDirection.Input, OptionName);
                    }
                    if (!string.IsNullOrEmpty(Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, Remark);
                    }
                    if (!string.IsNullOrEmpty(Action))
                    {
                        ObjCmd.AddParameterWithValue("@Action", SqlDbType.VarChar, 0, ParameterDirection.Input, Action);
                    }
                    if (!string.IsNullOrEmpty(Result))
                    {
                        ObjCmd.AddParameterWithValue("@Result", SqlDbType.VarChar, 0, ParameterDirection.Input, Result);
                    }
                    if (!string.IsNullOrEmpty(ProfileId))
                    {
                        ObjCmd.AddParameterWithValue("@ProfileId", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt16(ProfileId));
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
        }

        public static void FunChangepass(ClsCahngePass objChangePass)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ChangePassword", ObjConn, CommandType.StoredProcedure))
                {

                    if (!string.IsNullOrEmpty(objChangePass.UserName))
                    {
                        ObjCmd.AddParameterWithValue("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.UserName);
                    }
                    if (!string.IsNullOrEmpty(objChangePass.BankId))
                    {
                        ObjCmd.AddParameterWithValue("@BankId", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.BankId);
                    }
                    if (!string.IsNullOrEmpty(objChangePass.LoginID))
                    {
                        ObjCmd.AddParameterWithValue("@LoginID", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.LoginID);
                    }
                    if (!string.IsNullOrEmpty(objChangePass.Role))
                    {
                        ObjCmd.AddParameterWithValue("@Role", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.Role);
                    }
                    if (!string.IsNullOrEmpty(objChangePass.CurrentPass))
                    {
                        ObjCmd.AddParameterWithValue("@CurrentPass", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.CurrentPass);
                    }
                    if (!string.IsNullOrEmpty(objChangePass.NewPass))
                    {
                        ObjCmd.AddParameterWithValue("@NewPass", SqlDbType.VarChar, 0, ParameterDirection.Input, objChangePass.NewPass);
                    }
                    if (objChangePass.ChangedBy > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ChangedBy", SqlDbType.BigInt, 0, ParameterDirection.Input, objChangePass.ChangedBy);
                    }
                    ObjCmd.AddParameterWithValue("@mode", SqlDbType.Int, 0, ParameterDirection.Input, objChangePass.mode);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    if (ObjDTOutPut.Rows[0]["Result"] != null)
                        objChangePass.Result = Convert.ToInt32(ObjDTOutPut.Rows[0]["Result"].ToString());

                    if (ObjDTOutPut.Rows[0]["OutputDescription"] != null)
                        objChangePass.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
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

        }

        public static DataTable FunGetTerminalDenominationDetails(string _participantId, string _tid, string _requestType)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetTerminalDenominationDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ParticipantId", SqlDbType.VarChar, 0, ParameterDirection.Input, _participantId);
                    ObjCmd.AddParameterWithValue("@Tid", SqlDbType.VarChar, 0, ParameterDirection.Input, _tid);
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.VarChar, 0, ParameterDirection.Input, _requestType);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetTerminalDenominationDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public static bool FunProcessTermDenominationDetails(string _type, string _participantId, string _tid, string _loginId, string _cassette1Deno, string _cassette1Count, string _cassette2Deno, string _cassette2Count, string _cassette3Deno, string _cassette3Count, string _cassette4Deno, string _cassette4Count, string _requestType, int _termDenominationId, string _term_type, string _cardAcceptor, string _currencyCode)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ProcessDenominationUpdateRequest", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Type", SqlDbType.VarChar, 0, ParameterDirection.Input, _type);
                    ObjCmd.AddParameterWithValue("@ParticipantId", SqlDbType.VarChar, 0, ParameterDirection.Input, _participantId);
                    ObjCmd.AddParameterWithValue("@TerminalId", SqlDbType.VarChar, 0, ParameterDirection.Input, _tid);
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.VarChar, 0, ParameterDirection.Input, _requestType);
                    ObjCmd.AddParameterWithValue("@TermDenominationId", SqlDbType.Int, 0, ParameterDirection.Input, _termDenominationId);
                    ObjCmd.AddParameterWithValue("@Cassete1Deno", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette1Deno);
                    ObjCmd.AddParameterWithValue("@Cassete1Count", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette1Count);
                    ObjCmd.AddParameterWithValue("@Cassete2Deno", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette2Deno);
                    ObjCmd.AddParameterWithValue("@Cassete2Count", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette2Count);
                    ObjCmd.AddParameterWithValue("@Cassete3Deno", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette3Deno);
                    ObjCmd.AddParameterWithValue("@Cassete3Count", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette3Count);
                    ObjCmd.AddParameterWithValue("@Cassete4Deno", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette4Deno);
                    ObjCmd.AddParameterWithValue("@Cassete4Count", SqlDbType.VarChar, 0, ParameterDirection.Input, _cassette4Count);
                    ObjCmd.AddParameterWithValue("@LoginId", SqlDbType.VarChar, 0, ParameterDirection.Input, _loginId);

                    ObjCmd.AddParameterWithValue("@TermType", SqlDbType.VarChar, 0, ParameterDirection.Input, _term_type);
                    ObjCmd.AddParameterWithValue("@CardAcceptor", SqlDbType.VarChar, 0, ParameterDirection.Input, _cardAcceptor);
                    ObjCmd.AddParameterWithValue("@CurrencyCode", SqlDbType.VarChar, 0, ParameterDirection.Input, _currencyCode);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    return true;
                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunGetTerminalDetails()", ObjExc.Message, "");
                return false;
            }
        }


        public static DataTable FunGetEmailConfig(string issuer, string processing_code)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetEmailConfig", ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(issuer))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, issuer);
                    }
                    if (!string.IsNullOrEmpty(processing_code))
                    {
                        ObjCmd.AddParameterWithValue("@ProcessCode", SqlDbType.VarChar, 0, ParameterDirection.Input, processing_code);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception e) { }
            return ObjDTOutPut;
        }


    }
}
