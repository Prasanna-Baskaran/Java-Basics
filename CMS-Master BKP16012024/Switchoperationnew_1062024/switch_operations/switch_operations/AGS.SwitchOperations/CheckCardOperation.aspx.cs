using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;

namespace AGS.SwitchOperations
{
    public partial class CheckCardOperation : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        string JsonData;
        string Status = string.Empty;
        string Msg = string.Empty;
        string Description = string.Empty;
        string SessionId = string.Empty;
        string BankId = string.Empty;
        CustSearchFilter objSearch = new CustSearchFilter();
        ClsPINResetMsgParam ObjReqmsg = new ClsPINResetMsgParam();
        string FinalStat = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMACO"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        userBtns.AccessButtons = StrAccessCaption;
                        hdnAccessCaption.Value = StrAccessCaption;
                        //For user those having Accept right
                        if (accessPrefix.Contains("C"))
                        {


                            //userBtns.AddClick += new EventHandler(BtnAdd_click);
                            //userBtns.PreviousClick += new EventHandler(BtnPrevious_click);


                            userBtns.AcceptClick += new EventHandler(AcceptRejectCardOpsRequests);
                            userBtns.RejectClick += new EventHandler(AcceptRejectCardOpsRequests);
                            userBtns.CancelClick += new EventHandler(Page_Load);

                        }
                        if (!IsPostBack)
                        {
                            FillDropDown();
                        }
                    }
                }
                // Redirect to access denied page
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }

                hdnResultStatus.Value = "";


            }
            catch (Exception)
            {
                // new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, Page_Load()", Ex.Message, "");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FunSearchDetails();
        }

        public void FunSearchDetails()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                CustSearchFilter ObjSearch = new CustSearchFilter();
                ObjSearch.CardNo = txtSearchCardNo.Value;
                if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                {
                    ObjSearch.BankCustID = txtSearchCustomerID.Value;
                }
                ObjSearch.RequestTypeID = Convert.ToInt16(ddlRequestType.SelectedValue);
                ObjSearch.SystemID = Session["SystemID"].ToString();
                ObjSearch.BankID = Session["BankID"].ToString();

                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardRequests(ObjSearch);

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    hdnResultStatus.Value = "1";
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //For user those having accept right
                    if (accessPrefix.Contains("C"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[2];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        objAdded[1] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "FunShowDetails($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "reqid", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("ID,FormStatusID,Response,RequestTypeID", objAdded);
                        LblResult.InnerHtml = "";
                        hdnReqType.Value = ddlRequestType.SelectedValue;
                    }
                    else
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "FunShowDetails($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "reqid", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("ID,FormStatusID,Response,RequestTypeID", objAdded);
                        LblResult.InnerHtml = "";
                        hdnReqType.Value = ddlRequestType.SelectedValue;
                    }
                }
                else
                {
                    hdnResultStatus.Value = "";
                    hdnReqType.Value = "";
                    LblResult.InnerHtml = "No Record Found";
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, btnSearch_Click()", Ex.Message, "");
            }
        }
        //Acceept/Reject  Requests
        protected void AcceptRejectCardOpsRequests(object sender, EventArgs e)
        {
            try
            {
                objSearch.CheckerId = Convert.ToInt64(Session["LoginID"]);
                objSearch.FormStatusID = Convert.ToInt32(hdnFormStatusID.Value);
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.RequestIDs = hdnRequestIDs.Value;
                objSearch.RequestTypeID = Convert.ToInt32(hdnReqType.Value);
                objSearch.PINResetFlag = 1;
                if (!string.IsNullOrEmpty(txtRejectReson.Text))
                {
                    objSearch.Remark = txtRejectReson.Text;
                }
                Button clickedButton = sender as Button;

                DataTable ObjDTCarddetailsTOResetPin = new DataTable();
                if (Convert.ToInt32(hdnReqType.Value) == 7 && clickedButton.ID != "ModalReject" && clickedButton.ID != "ModalReject")
                {
                    ObjDTCarddetailsTOResetPin = new ClsCardMasterBAL().FunGetSetCardDetailsForPinRepin(objSearch);
                    if (ObjDTCarddetailsTOResetPin.Rows.Count > 0)
                    {
                        string cardNumber;
                        string IdOfCardNo;
                        for (int i = 0; i < ObjDTCarddetailsTOResetPin.Rows.Count; i++)
                        {
                            FinalStat = "";
                            cardNumber = ObjDTCarddetailsTOResetPin.Rows[i]["CardNo"].ToString();
                            IdOfCardNo = ObjDTCarddetailsTOResetPin.Rows[i]["ID"].ToString();
                            ISOToResetPIN(cardNumber);
                            if (FinalStat == "000") //FinalStat is 000 mean PIN reset processed successfully. If true then update the same into DB
                            {
                                LblResult.InnerHtml = "Pin Reset has been done successfully.";
                                objSearch.PINResetFlag = 2;
                                objSearch.RequestTypeID = Convert.ToInt32(IdOfCardNo);
                                ObjDTCarddetailsTOResetPin = new ClsCardMasterBAL().FunGetSetCardDetailsForPinRepin(objSearch);

                                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CheckCardOperation.aspx", "Pin Reset has been done successfully.", "");

                            }
                            else
                            {
                                LblResult.InnerHtml = "System Error";
                                objSearch.PINResetFlag = 3;
                                objSearch.RequestTypeID = Convert.ToInt32(IdOfCardNo);
                                ObjDTCarddetailsTOResetPin = new ClsCardMasterBAL().FunGetSetCardDetailsForPinRepin(objSearch);
                                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CheckCardOperation.aspx", "System Error", "");

                            }
                        }
                    }
                }
                else
                {
                    FunAcceptRejectRequest();
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, AcceptRejectCardOpsRequests()", Ex.Message, "RequestIDs = " + hdnReqType.Value + " , RequestTypeID = " + hdnRequestIDs.Value);
            }
        }

        public void FunAcceptRejectRequest()
        {
            try
            {
                DataTable ObjDTTable = new DataTable();

                if ((!string.IsNullOrEmpty(hdnRequestIDs.Value)) && (!string.IsNullOrEmpty(hdnReqType.Value)))
                {

                    ObjDTTable = new ClsCardMasterBAL().FunAccept_RejectCardReq(objSearch);
                    if (ObjDTTable.Rows.Count > 0)
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ID", BindTableColumnValueWithAttribute = "ID" } } };
                        hdnTransactionDetails.Value = ObjDTTable.ToHtmlTableString("ID,FormStatusID,RequestTypeID", objAdded);
                        LblResult.InnerHtml = "";
                        hdnReqType.Value = ddlRequestType.SelectedValue;
                        hdnResultStatus.Value = "1";
                    }
                    else
                    {
                        LblResult.InnerHtml = "System Error";
                    }
                }
                //From Modal individual req
                if ((!string.IsNullOrEmpty(hdnModalRqType.Value)) && (!string.IsNullOrEmpty(hdnRqID.Value)))
                {

                    objSearch.RequestIDs = hdnRqID.Value;
                    objSearch.RequestTypeID = Convert.ToInt32(hdnModalRqType.Value);
                    if (!string.IsNullOrEmpty(txtReason.Text))
                    {
                        objSearch.Remark = txtReason.Text;
                    }
                    ObjDTTable = new ClsCardMasterBAL().FunAccept_RejectCardReq(objSearch);
                    if (ObjDTTable.Rows.Count > 0)
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "reqtypeid", BindTableColumnValueWithAttribute = "RequestTypeID" } } };
                        //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ID", BindTableColumnValueWithAttribute = "ID" } } };
                        hdnTransactionDetails.Value = ObjDTTable.ToHtmlTableString("ID,FormStatusID,RequestTypeID", objAdded);
                        LblResult.InnerHtml = "";
                        // hdnReqType.Value = ddlRequestType.SelectedValue;
                        hdnResultStatus.Value = "1";
                    }
                    else
                    {
                        LblResult.InnerHtml = "System Error";
                    }
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, AcceptRejectCardOpsRequests()", Ex.Message, "RequestIDs = " + hdnReqType.Value + " , RequestTypeID = " + hdnRequestIDs.Value);
            }
        }

        public void ISOToResetPIN(string CardNO)
        {
            ClsAPIRequestBO APIRequestParam = new ClsAPIRequestBO();
            DataTable ObjDTOutPut = new DataTable();
            Status = string.Empty;
            Msg = string.Empty;
            Description = string.Empty;
            SessionId = string.Empty;
            try
            {
                //ObjAcclink.BankID = Session["BankID"].ToString();

                string Sourceid = Session["SourceId"].ToString();

                /*Getting Session Id and session expiry date From DB*/
                ObjDTOutPut = new ClsAccountLinkingDelinkingBAL().FunGetSessionForBank(Sourceid);
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(ObjDTOutPut.Rows[0]["SessionID"].ToString()) || Convert.ToDateTime(ObjDTOutPut.Rows[0]["SessionExpiryDateTime"]) <= DateTime.Now)
                    {
                        APIRequestParam.TranType = "GetSession";
                        Random rnext = new Random();
                        var request = rnext.Next();
                        APIRequestParam.RequestId = request.ToString(); /*Is must be an random No.*/
                        APIRequestParam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        APIRequestParam.SourceId = Sourceid;

                        string JsonData = JsonConvert.SerializeObject(APIRequestParam);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN REPIN " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId, BankId);

                        CallAPI(JsonData, out Status, out Msg);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN REPIN " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Response From CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId + " ,Status:" + Status + " ,Msg:" + Msg, BankId);

                        if (Status == "000")
                        {
                            string key = APIRequestParam.SourceId.Substring(0, 12) + APIRequestParam.RequestId.Substring(0, 4);
                            string DecrSessionId = (AESCrypt.Decrypt(Msg, key));
                            FunGetDeserializeObject(DecrSessionId, Description, out SessionId);
                            FunCallAPIWithTranType(SessionId, ObjDTOutPut, APIRequestParam, CardNO);
                        }
                        //else
                        //{
                        //    //LblMessage.Text = "Failed";
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        //}
                    }
                    else
                    {
                        FunCallAPIWithTranType("", ObjDTOutPut, APIRequestParam, CardNO);
                    }

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN RESET, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                //LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        public void FunCallAPIWithTranType(string Sessionid, DataTable ObjDTOutPut, ClsAPIRequestBO APIrequestparam, string CardNo)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                APIrequestparam.TranType = "CardPINReset";
                Random r = new Random();
                var request = r.Next();
                APIrequestparam.RequestId = request.ToString();
                APIrequestparam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                APIrequestparam.SourceId = Session["SourceId"].ToString();
                APIrequestparam.SessionId = SessionId == "" ? ObjDTOutPut.Rows[0]["SessionID"].ToString() : SessionId;

                //string m = hdnRequiredData.Value;
                //string s = hdnCardNo.Value;
                //APIrequestparam.Msg = "irCZIvRJltvHPZaMQe2Q0i7+aha6YSQZFXRs46bTXBfugOO1W0FMXFr35IN542mzRqCMAxsnQuGmspOpPLt6/vFuVRaN1w1X6sxU+VsQa0pEZTtt/snQIRRFuKLDbM31+qYYmcwxs8CzuQ+d+aSu8UU7jKv2g/5mLtEBBxDIN+8=";
                // APIrequestparam.SessionId = new ClsAccountLinkingDelinkingBAL().FunGetSourceIdForBank(ObjAcclink);
                //string ReqEncmsg = AESCrypt.Encrypt((JsonConvert.SerializeObject(hdnRequiredData.Value)), APIrequestparam.SessionId);
                string Message = GetRequestMessage(ObjReqmsg, APIrequestparam, CardNo);
                string ReqEncmsg = AESCrypt.Encrypt(Message, APIrequestparam.SessionId);
                APIrequestparam.Msg = ReqEncmsg;
                string JsonData = JsonConvert.SerializeObject(APIrequestparam);

                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN REPIN " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Sending Request to CardAPI RequestType:" + APIrequestparam.TranType + " RequestId:" + APIrequestparam.RequestId + " RequestMsg:" + ReqEncmsg + " ,TxnDateTime:" + APIrequestparam.TxnDateTime + " , SourceId:" + APIrequestparam.SourceId, BankId);
                CallAPI(JsonData, out Status, out Msg);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN REPIN " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Response From CardAPI RequestType:" + APIrequestparam.TranType + "  RequestId:" + APIrequestparam.RequestId + " ,TxnDateTime:" + APIrequestparam.TxnDateTime + " , SourceId:" + APIrequestparam.SourceId + " ,Status:" + Status + " ,Msg:" + Msg, BankId);
                FinalStat = Status;
                //if (Status == "000")
                //{
                //    string FinalStat = Status;
                //    //SkipDialogBox = false;
                //    //FunSearchDetails();
                //    //if (ObjReqmsg.LinkingFlag == "01")
                //    //{
                //    //    LblMessage.Text = "Account Linked Successfully.";
                //    //}
                //    //else
                //    //{
                //    //    LblMessage.Text = "Account De-Linked Successfully.";
                //    //}

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //}
                //else
                //{
                //    //LblMessage.Text = "Failed";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //}
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN REPIN , " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                //LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        public string GetRequestMessage(ClsPINResetMsgParam ObjReqmsg, ClsAPIRequestBO APIRequestParam, string CardNoToPINReset)
        {
            //string ObjReturnStatus = string.Empty;

            ObjReqmsg.CardNo = CardNoToPINReset;

            string ObjReturnStatus = JsonConvert.SerializeObject(ObjReqmsg);

            return ObjReturnStatus;
        }
        public void FunGetDeserializeObject(string DecrSessionId, string Description, out string SessionId)
        {
            Description = string.Empty;
            SessionId = string.Empty;
            ClsGetsessionBO objResponse = JsonConvert.DeserializeObject<ClsGetsessionBO>(DecrSessionId);

            {
                Description = objResponse.Description;
            }
            ClsGetSession GetSession = JsonConvert.DeserializeObject<ClsGetSession>(Description);
            {
                SessionId = GetSession.SessionId;

            }
        }

        public void CallAPI(string JsonData, out string Status, out string Msg)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                InitiateSSLTrust();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AccountLinkDelinkPOSTURL"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // RespCode = string.Empty;
                    //ResponsMsg = "Unable to set FX rate";
                    var result = streamReader.ReadToEnd();
                    //string APIResponse = result.ReadToEnd();
                    ClsAPIResponseBO objResponse = JsonConvert.DeserializeObject<ClsAPIResponseBO>(result);
                    Status = objResponse.Status;
                    Msg = objResponse.Msg;
                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN RESET ,  " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                //LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        //Get Details
        [System.Web.Services.WebMethod]
        public static string FunViewDetails(string ID, string ReqTypeID, string SystemID)
        {
            string StrResult = string.Empty;
            CustSearchFilter Obj = new CustSearchFilter();
            DataTable ObjDTOutPut = new DataTable();
            Obj.ID = Convert.ToInt64(ID);
            Obj.RequestTypeID = Convert.ToInt32(ReqTypeID);
            Obj.SystemID = SystemID;
            ObjDTOutPut = new ClsCardMasterBAL().FunGetCardRequestByID(Obj);
            if (ObjDTOutPut.Rows.Count > 0)
            {
                StrResult = JsonConvert.SerializeObject(ObjDTOutPut);
                StrResult = ObjDTOutPut.ToJSON();
            }
            return StrResult;

        }

        public void FillDropDown()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(19, "");
                ddlRequestType.DataSource = ObjDTOutPut;
                ddlRequestType.DataTextField = "RequestType";
                ddlRequestType.DataValueField = "ID";
                ddlRequestType.DataBind();
                ddlRequestType.Items.Insert(0, new ListItem("--Select--", "0"));
                ddlOpsReqType.DataSource = ObjDTOutPut;
                ddlOpsReqType.DataTextField = "RequestType";
                ddlOpsReqType.DataValueField = "ID";
                ddlOpsReqType.DataBind();
                ddlOpsReqType.Items.Insert(0, new ListItem("--Select--", "0"));



            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, FillDropDown()", Ex.Message, "");
            }

        }
        public static void InitiateSSLTrust()
        {
            try
            {
                //Change SSL checks so that all checks pass

                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }

                    );

            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, InitiateSSLTrust()", Ex.Message, "");
                //ActivityLog.InsertSyncActivity(ex);

            }
        }
    }
}