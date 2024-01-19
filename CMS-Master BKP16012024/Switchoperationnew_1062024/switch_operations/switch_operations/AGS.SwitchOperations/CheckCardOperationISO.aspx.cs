using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;
using AGS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace AGS.SwitchOperations
{
    public partial class CheckCardOperationISO : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
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
                string msg = string.Empty;

                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="Request Type", Value= ddlRequestType.SelectedValue, MinLength = 1, MaxLength = 1, Numeric = true, Isrequired=true, Regex="^[1-9]*$" },
                    new ValidatorAttr { Name="CustomerID", Value= txtSearchCustomerID.Value, MinLength = 3, MaxLength = 20 },
                    new ValidatorAttr { Name="Card No", Value= txtSearchCardNo.Value, MinLength = 16, MaxLength = 16 , Numeric=true},
                    new ValidatorAttr { Name="Name On Card", Value= txtSearchNameOnCard.Value, MinLength = 3, MaxLength = 20 , Regex="^[a-zA-Z0-9 ]*$"}
                };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {
                    DataTable ObjDTOutPut = new DataTable();
                    CustSearchFilter ObjSearch = new CustSearchFilter();
                    ObjSearch.CardNo = txtSearchCardNo.Value;
                    if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                    {
                        ObjSearch.BankCustID = txtSearchCustomerID.Value;
                    }
                    if (!string.IsNullOrEmpty(txtSearchNameOnCard.Value))
                    {
                        ObjSearch.NameOnCard = txtSearchNameOnCard.Value;
                    }
                    ObjSearch.RequestTypeID = Convert.ToInt16(ddlRequestType.SelectedValue);
                    ObjSearch.SystemID = Session["SystemID"].ToString();
                    ObjSearch.BankID = Session["BankID"].ToString();
                    ObjSearch.CheckerId = Convert.ToInt64(Session["LoginID"]); ;
                    ObjSearch.IssuerNo = Session["IssuerNo"].ToString();
                    ObjSearch.UserBranchCode = Session["BranchCode"].ToString();
                    ObjSearch.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                    ObjSearch.UserID = Convert.ToString(Session["UserName"]);

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
                objSearch.UserBranchCode = Session["BranchCode"].ToString();
                objSearch.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                objSearch.UserID = Convert.ToString(Session["UserName"]);

                if ((!string.IsNullOrEmpty(hdnRequestIDs.Value)) && (!string.IsNullOrEmpty(hdnReqType.Value)))
                {
                    if (!string.IsNullOrEmpty(txtRejectReson.Text))
                    {
                        objSearch.Remark = txtRejectReson.Text;
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
                }

                Button clickedButton = sender as Button;

                DataTable _DTCardOpsDataISO = new DataTable();
                DataTable _DTUpdateCardOpsRSP = new DataTable();

                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);

                if (string.IsNullOrEmpty(objSearch.Remark)) //If remark is null or empy means this is success req
                {
                    switch (objSearch.RequestTypeID)
                    {
                        case 1://1 for SetCardLimit
                            objSearch.ActionType = 4;
                            break;
                        case 7://7 for reset pin
                            objSearch.ActionType = 7;
                            break;
                        default:
                            objSearch.ActionType = 1;
                            break;
                    }

                    _DTCardOpsDataISO = new ClsCardMasterBAL().FunGetOpsDataForISO(objSearch);

                    string SuccessCardNo = string.Empty;
                    string FailuerCardNo = string.Empty;
                    if (_DTCardOpsDataISO.Rows.Count > 0)
                    {
                        GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                        APIResponseObject ObjAPIResponse = new APIResponseObject();
                        ConfigDataObject ObjData = new ConfigDataObject();
                        ObjData.IssuerNo = Session["IssuerNo"].ToString();
                        ObjData.APIRequestType = Convert.ToString(_DTCardOpsDataISO.Rows[0][1]);
                        ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                        ObjData.SourceID = Convert.ToString(_DTCardOpsDataISO.Rows[0][2]);

                        //ObjData.IsCardDetailsSearch = true;                    

                        for (int i = 0; i < _DTCardOpsDataISO.Rows.Count; i++)
                        {
                            objSearch.RequestIDs = _DTCardOpsDataISO.Rows[i]["id"].ToString(); ///added by uddesh ATPBF-1157
                            DataTable _dtRequest = new DataTable();
                            if (objSearch.RequestTypeID == 1) //1 for SetCardLimit
                            {
                                _dtRequest.Columns.Add("CardNo", typeof(string));
                                _dtRequest.Columns.Add("POSLimitCount", typeof(int));
                                _dtRequest.Columns.Add("POSLimit", typeof(int));
                                _dtRequest.Columns.Add("PTPOSLimit", typeof(string));
                                _dtRequest.Columns.Add("ATMLimitCount", typeof(int));
                                _dtRequest.Columns.Add("ATMLimit", typeof(int));
                                _dtRequest.Columns.Add("PTATMLimit", typeof(string));
                                _dtRequest.Columns.Add("PaymentsCount", typeof(string));
                                _dtRequest.Columns.Add("PaymentsLimit", typeof(string));
                                _dtRequest.Columns.Add("PTPaymentsLimit", typeof(string));
                                _dtRequest.Columns.Add("EComLimit", typeof(string));
                                _dtRequest.Columns.Add("PTEComLimit", typeof(string));

                                _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0], _DTCardOpsDataISO.Rows[i][3], _DTCardOpsDataISO.Rows[i][4], 
                                _DTCardOpsDataISO.Rows[i][5], _DTCardOpsDataISO.Rows[i][6], _DTCardOpsDataISO.Rows[i][7], _DTCardOpsDataISO.Rows[i][8], 
                                _DTCardOpsDataISO.Rows[i][9], _DTCardOpsDataISO.Rows[i][10], _DTCardOpsDataISO.Rows[i][11], _DTCardOpsDataISO.Rows[i][12],
                                _DTCardOpsDataISO.Rows[i][13]});


                            }
                            else if (objSearch.RequestTypeID == 2 || objSearch.RequestTypeID == 3 || objSearch.RequestTypeID == 4 || objSearch.RequestTypeID == 5)
                            {
                                _dtRequest.Columns.Add("CardNo", typeof(string));
                                _dtRequest.Columns.Add("BlockType", typeof(string));
                                _dtRequest.Columns.Add("InternationalCard", typeof(string));
                                _dtRequest.Columns.Add("Status", typeof(string));

                                string BlockType = string.Empty;
                                string CardStatus = string.Empty;

                                if (objSearch.RequestTypeID == 2)//HSC 06
                                {
                                    BlockType = "1";
                                    CardStatus = "1";
                                }
                                else if (objSearch.RequestTypeID == 3)//HSC 41
                                {
                                    BlockType = "0";
                                    CardStatus = "1";
                                }
                                else if (objSearch.RequestTypeID == 4)//43
                                {
                                    BlockType = "3";
                                    CardStatus = "1";
                                }
                                else if (objSearch.RequestTypeID == 5)//null
                                {
                                    BlockType = "1";
                                    CardStatus = "0";
                                }
                                _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0], BlockType, "0", CardStatus });
                            }
                            else if (objSearch.RequestTypeID == 7)
                            {
                                _dtRequest.Columns.Add("CardNo", typeof(string));
                                _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0] });
                            }

                            _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

                            if (objSearch.RequestTypeID == 1) //1 for SetCardLimit
                            {
                                objSearch.ActionType = 5; //TO update Status SetCardLimit
                            }
                            else
                            {
                                objSearch.ActionType = 2; //To update Status of cardopsrequests
                            }

                            objSearch.ISORSPCode = ObjAPIResponse.Status;
                            objSearch.ISORSPDesc = ObjAPIResponse.StatusDesc;
                            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                            {
                                SuccessCardNo = SuccessCardNo + _DTCardOpsDataISO.Rows[i][0] + ",";
                            }
                            else
                            {
                                FailuerCardNo = FailuerCardNo + _DTCardOpsDataISO.Rows[i][0] + ",";
                            }
                            _DTUpdateCardOpsRSP = new ClsCardMasterBAL().FunGetOpsDataForISO(objSearch);
                        }

                        if (!string.IsNullOrEmpty(SuccessCardNo))
                        {
                            LblResult.InnerHtml = "Successfully processed requests : " + removeLastChar(SuccessCardNo);
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request sucessfully accepted", LblResult.InnerHtml.Replace("Success Cards : ", ""), "", "", "", "", "", "Accept", "1");
                        }
                        if (!string.IsNullOrEmpty(FailuerCardNo))
                        {
                            LblResult.InnerHtml = LblResult.InnerHtml + "<br/>Failed to process requests : " + removeLastChar(FailuerCardNo);
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request failed to accept", LblResult.InnerHtml.Replace("<br/>Failuer Cards : ", ""), "", "", "", "", "", "Accept", "0");
                        }
                    }
                    else
                    {
                        LblResult.InnerHtml = "System Error";
                    }
                }
                else
                {
                    switch (objSearch.RequestTypeID)
                    {
                        case 1://1 for SetCardLimit
                            objSearch.ActionType = 6;
                            break;
                        default:
                            objSearch.ActionType = 3;
                            break;
                    }
                    _DTCardOpsDataISO = new DataTable();
                    _DTCardOpsDataISO = new ClsCardMasterBAL().FunGetOpsDataForISO(objSearch);
                    
                    if (_DTCardOpsDataISO.Rows.Count > 0 && Convert.ToString(_DTCardOpsDataISO.Rows[0][0]) == "1")
                    {
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request sucessfully rejected", LblResult.InnerHtml.Replace("Success Cards : ",""), "", "", "", "", "", "Reject", "1");
                        LblResult.InnerHtml = "Selected records successfully rejected.";
                    }
                    else
                    {
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request failed to rejected", LblResult.InnerHtml.Replace("<br/>Failuer Cards : ", ""), "", "", "", "","", "Reject", "0");
                        LblResult.InnerHtml = "Failuer in rejection request.";
                    }
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
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request to reset PIN", CardNO, "", "", "", "", "", "Submit", "1");
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
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Request to reset PIN", CardNO, "", "", "", "", "", "Submit", "1");
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
        public static string FunViewDetails(string ID, string ReqTypeID, string SystemID, string IssuerNo)
        {
            string StrResult = string.Empty;
            CustSearchFilter Obj = new CustSearchFilter();
            DataTable ObjDTOutPut = new DataTable();
            Obj.ID = Convert.ToInt64(ID);
            Obj.RequestTypeID = Convert.ToInt32(ReqTypeID);
            Obj.SystemID = SystemID;
            Obj.IssuerNo = IssuerNo;


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
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(32, "");
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
            }
        }

        private static String removeLastChar(String str)
        {
            if (str.LastIndexOfAny(new char[] { ',' }) == str.Length - 1)
                str = str.Substring(0, str.Length - 1);
            return str;
        }
    }
}