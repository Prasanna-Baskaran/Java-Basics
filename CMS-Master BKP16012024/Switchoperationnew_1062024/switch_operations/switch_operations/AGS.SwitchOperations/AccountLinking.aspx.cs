using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace AGS.SwitchOperations
{
    public partial class AccountLinking : System.Web.UI.Page
    {
        ClsAccountLinkingBO ObjAcclink = new ClsAccountLinkingBO();
        ClsAccountLinkingRequestMSGParam ObjReqmsg = new ClsAccountLinkingRequestMSGParam();
        ClsAPIRequestBO APIRequestParam = new ClsAPIRequestBO();
        string StrAccessCaption = string.Empty;
        string JsonData;
        string Status = string.Empty;
        string Msg = string.Empty;
        string Description = string.Empty;
        string SessionId = string.Empty;
        string BankId = string.Empty;
        Boolean SkipDialogBox;
        bool LinkDelinkFlag;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //var DecryptRequest = AESCrypt.Decrypt("3tnmd58z6Weq2jXwBJGcbbWGtjZjwxBSUTtzfdB0qiN2OjAA09rgHu/eq3B/H7TG/U5mG6EUUrMF80wL4WejfxYozU8v+P4Vbm4U2WitlsPu+2zj7ASQ5kX2ODcQ6IfkZsdpkMA0gfjEAMSHLUOFlX119e7n5dwktlNnBYkgkrXokWTnPcLRVYdKvqOVUn12OGMhd+Xd8s0iu44nx1PFvksY1yXbeW3c6hAmZzQwCg2BDipWgwXbOYMr/6uQJuKiOZRXL2U6XK4UzFu623ZOpA==", 5051001866867839);
                //var DecryptTest = "{\"Description\": \"{\r\n  \"Description\": \"Account added for customer successfully\",\r\n  \"CustomerData\": \"[\r\n    {\r\n      \"AccountNo\": \"01H214A678CAE11E3E94347E3A270E9E54719049CF34151DF3FFD1\"\r\n    }\r\n  ]\"\r\n}\"}";

                //AGS.SwitchOperations.ClsAccountLink.getSessionRsp SessionRsp = JsonConvert.DeserializeObject<AGS.SwitchOperations.ClsAccountLink.getSessionRsp>(DecryptTest);
                //dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);
                //string type = dynamicObject.CustomerData[0].AccountNo;

                //string JsonString = "{\"Description\": \"{\r\n  \"Description\": \"Account added for customer successfully\",\r\n  \"CustomerData\": \"[\r\n    {\r\n      \"AccountNo\": \"01H214A678CAE11E3E94347E3A270E9E54719049CF34151DF3FFD1\"\r\n    }\r\n  ]\"\r\n}\"}";

                //dynamic obj = JsonConvert.DeserializeObject(JsonString);

                //var IndexOfAccountNo = JsonString.IndexOf("AccountNo");
                //if (IndexOfAccountNo > 0)
                //{
                //    var SubstringAccountNo = JsonString.Substring(IndexOfAccountNo + 13, JsonString.Length - IndexOfAccountNo - 13);
                //    JsonString = SubstringAccountNo.Substring(0, SubstringAccountNo.IndexOf("\""));
                //}
                //var a = JsonConvert.DeserializeObject(aa);
                //var b = JsonConvert.DeserializeObject(a);


                BankId = Session["BankID"].ToString();
                if (!Page.IsPostBack)
                {
                    string OptionNeumonic = "ACL"; //unique optionneumonic from database
                    Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                    ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                    if (ObjDictRights.ContainsKey(OptionNeumonic))
                    {
                        StrAccessCaption = ObjDictRights[OptionNeumonic];
                        if (!string.IsNullOrEmpty(StrAccessCaption))
                        {
                            hdnAccessCaption.Value = StrAccessCaption;

                        }
                        else
                        {
                            Response.Redirect("ErrorPage.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
                    }

                    /*Hiding memberModal DIV*/
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, Page_Load()", ex.Message, BankId);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SkipDialogBox = true;
            FunSearchDetails();
        }

        public void FunSearchDetails()
        {
            try
            {
                LblResult.InnerHtml = "";
                LblMessage.Text = "";

                ObjAcclink.CardNo = txtSearchCardNo.Value;
                //ObjAcclink.IntPara = 0;
                ObjAcclink.SystemID = Session["SystemID"].ToString();
                ObjAcclink.BankID = Session["BankID"].ToString();

                /*Checking Is Card present*/
                string ObjStatusIsExist = new ClsAccountLinkingDelinkingBAL().FunCheckCardExist(ObjAcclink);

                /*If Card is exist then second index value will be Exist*/
                if (!string.IsNullOrEmpty(ObjStatusIsExist) && ObjStatusIsExist.Split('|')[1] == "Exist")
                {
                    if (SkipDialogBox)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                    FunGetAccountLinkingDetails(ObjAcclink);
                }
                else
                {
                    LblMessage.Text = "No Record Found";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
            }
        }

        public void FunGetAccountLinkingDetails(ClsAccountLinkingBO ObjAcclink)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsAccountLinkingDelinkingBAL().FunSearchCardDetails(ObjAcclink);


                if (ObjDTOutPut.Rows.Count > 0)
                {
                    hdnTransactionDetails.Value = createTable(ObjDTOutPut);

                }
                else
                {
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);

            }
        }

        public string createTable(DataTable dtResult)
        {

            StringBuilder strHTML = new StringBuilder();
            string strTRStart = string.Empty;
            string strTREnd = "</tr>";
            string strTDStart = "<td>";
            string strTDEnd = "</td>";

            try
            {
                //CREATE TABLE HEADER FROM DATATABLE
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        //ADD TABLE HEADERS
                        string[] columnNames = dtResult.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                        strHTML.Append("<thead class='tHead_style' >" + "<tr>");
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            strHTML.Append("<th class='text_middle'>" + columnNames[i].Trim() + "</th>");
                        }


                        strHTML.Append("</tr>" + "</thead>" + "<tbody>");

                        //ADD TABLE DATA 
                        foreach (DataRow currentRow in dtResult.Rows)
                        {
                            if (dtResult.Rows.IndexOf(currentRow) % 2 == 0)
                            {
                                strTRStart = "<tr class='odd gradeX text_middle'>";
                            }
                            else
                            {
                                strTRStart = "<tr class='even gradeC text_middle'>";
                            }

                            strHTML.Append(strTRStart);
                            for (int loopVar = 0; loopVar < columnNames.Length; loopVar++)
                            {
                                strHTML.Append(strTDStart + currentRow[columnNames[loopVar]].ToString().Trim() + strTDEnd);
                            }
                            if (currentRow[columnNames[0]].ToString() == "LINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='02' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' > Delink </ asp:Button >" + strTDEnd);
                                // Convert.ToInt32(Session["Bank"])
                            }
                            if (currentRow[columnNames[0]].ToString() == "DELINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Delink'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs ' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='01' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                            }
                            else
                            {
                                strHTML.Append(strTDStart + strTDEnd);
                                //strHTML.Append(strTDStart + strTDEnd);
                            }
                            //Maker         
                            strHTML.Append(strTREnd);
                        }
                        strHTML.Append("</tbody>");
                    }
                }
                if (strHTML.ToString() == "")
                    return "No Results Found !";
                else
                    return strHTML.ToString();
            }
            catch (Exception Ex)
            {
                //(new ClsCommonBAL()).SaveErrorLogDetails("AcManagement.aspx.cs, createTable()", Ex.Message, Ex.StackTrace);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
                return "";
            }
        }

        protected void hdnLinkBtn_Click(object sender, EventArgs e)
        {
            DataTable ObjDTOutPut = new DataTable();
            Status = string.Empty;
            Msg = string.Empty;
            Description = string.Empty;
            SessionId = string.Empty;
            try
            {
                ObjAcclink.BankID = Session["BankID"].ToString();
                //ObjAcclink.SourceId = Session["SourceId"].ToString();
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
                        //APIrequestparam.TxnDateTime = CurrentDatetime.ToString("yyyyMMddHHmmssffff");
                        //APIrequestparam.SourceId = Session["SourceId"].ToString();
                        APIRequestParam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //APIrequestparam.SourceId = ObjDTOutPut.Rows[0]["SourceId"].ToString();
                        APIRequestParam.SourceId = Sourceid;

                        string JsonData = JsonConvert.SerializeObject(APIRequestParam);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                            "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId +
                            " ,TxnDateTime:" + APIRequestParam.TxnDateTime +
                            " , SourceId:" + APIRequestParam.SourceId, BankId);

                        APICall(JsonData, out Status, out Msg);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                            "***Responce From CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId +
                            " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId +
                            " ,Status:" + Status + " ,Msg:" + Msg, BankId);

                        if (Status == "000")
                        {
                            string key = APIRequestParam.SourceId.Substring(0, 12) + APIRequestParam.RequestId.Substring(0, 4);
                            string DecrSessionId = (AESCrypt.Decrypt(Msg, key));
                            //{"Description":"{\"SessionId\":\"4618333687908441\"}"}
                            FunGetDeserializeObject(DecrSessionId, Description, out SessionId);
                            //string Inputdescription = Description;
                            FunAccountLinkDelink(SessionId, ObjDTOutPut, APIRequestParam);

                        }
                        else
                        {
                            LblMessage.Text = "Failed";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        }
                    }
                    else
                    {
                        FunAccountLinkDelink("", ObjDTOutPut, APIRequestParam);
                    }

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking, FunAccountLinkDelink()", ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }


        }

        public string GetRequestMessage(ClsAccountLinkingRequestMSGParam ObjReqmsg, ClsAPIRequestBO APIRequestParam)
        {
            //string ObjReturnStatus = string.Empty;
            if (APIRequestParam.TranType == "AccountLinkingDelinking")
            {
                ObjReqmsg.CardNo = hdnCardNo.Value;//"6280143030019111";
                ObjReqmsg.AccountNo = hdnAccountNo.Value; //"102901000741298000111";
                ObjReqmsg.AccountType = hdnAccountType.Value;//"10";
                ObjReqmsg.AccountQualifier = hdnAccountQuilifier.Value.PadLeft(2, '0'); //"01";
                ObjReqmsg.LinkingFlag = hdnLinkingflag.Value;//"01";
            }
            if (APIRequestParam.TranType == "AddNewAccountForCustomer")
            {
                ObjReqmsg.CardNo = txtSearchCardNo.Value;//"6280143030019111";
                ObjReqmsg.AccountNo = txtAccountNo.Value; //"102901000741298000111";
                ObjReqmsg.AccountType = ddlAccountType.SelectedValue;//"10";
                ObjReqmsg.AccountQualifier = hdnCheckAccQuntifr.Value; //"01";
                ObjReqmsg.Currency = ddlCurrencyCode.SelectedValue;//"524";
            }
            string ObjReturnStatus = JsonConvert.SerializeObject(ObjReqmsg);

            return ObjReturnStatus;


        }


        public void FunAccountLinkDelink(string Sessionid, DataTable ObjDTOutPut, ClsAPIRequestBO APIrequestparam)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                APIrequestparam.TranType = "AccountLinkingDelinking";
                Random r = new Random();
                var request = r.Next();
                APIrequestparam.RequestId = request.ToString();
                APIrequestparam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                APIrequestparam.SourceId = Session["SourceId"].ToString();
                APIrequestparam.SessionId = SessionId == "" ? ObjDTOutPut.Rows[0]["SessionID"].ToString() : SessionId;


                //string m = hdnRequiredData.Value;
                string s = hdnCardNo.Value;
                //APIrequestparam.Msg = "irCZIvRJltvHPZaMQe2Q0i7+aha6YSQZFXRs46bTXBfugOO1W0FMXFr35IN542mzRqCMAxsnQuGmspOpPLt6/vFuVRaN1w1X6sxU+VsQa0pEZTtt/snQIRRFuKLDbM31+qYYmcwxs8CzuQ+d+aSu8UU7jKv2g/5mLtEBBxDIN+8=";
                // APIrequestparam.SessionId = new ClsAccountLinkingDelinkingBAL().FunGetSourceIdForBank(ObjAcclink);
                //string ReqEncmsg = AESCrypt.Encrypt((JsonConvert.SerializeObject(hdnRequiredData.Value)), APIrequestparam.SessionId);
                string Message = GetRequestMessage(ObjReqmsg, APIrequestparam);
                string ReqEncmsg = AESCrypt.Encrypt(Message, APIrequestparam.SessionId);
                APIrequestparam.Msg = ReqEncmsg;
                string JsonData = JsonConvert.SerializeObject(APIrequestparam);


                string AccountLogId = "";
                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, "", "", "");
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " RequestId:" + APIRequestParam.RequestId + " RequestMsg:" + ReqEncmsg + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId, BankId);
                APICall(JsonData, out Status, out Msg);
                string DecryptRequest = "";

                DecryptRequest = AESCrypt.Decrypt(Msg, APIrequestparam.SessionId);

                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Response From CardAPI RequestType:" + APIRequestParam.TranType + "  RequestId:" + APIRequestParam.RequestId + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId + " ,Status:" + Status + " ,Msg:" + Msg, BankId);

                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, Status, Msg, AccountLogId);

                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, Status, DecryptRequest, @AccountLogId);

                if (Status == "000")
                {
                    SkipDialogBox = false;
                    FunSearchDetails();

                    if (ObjReqmsg.LinkingFlag == "01")
                    {
                        LblMessage.Text = "Account Linked Successfully.";
                    }
                    else
                    {
                        LblMessage.Text = "Account De-Linked Successfully.";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                else
                {
                    LblMessage.Text = "Failed";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
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

        public void APICall(string JsonData, out string Status, out string Msg)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                InitiateSSLTrust();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AccountLinkDelinkPOSTURL"]);
                //httpWebRequest.ContentType = "text/plain";
                httpWebRequest.ContentType = "application/json";
                // byte[] bytes = encoding.GetBytes(JsonData);
                httpWebRequest.Method = "POST";
                //httpWebRequest.ContentLength = bytes.Length;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "{\"TranType\":\"AccountLinkingDelinking\"," +
                    //              "\"RequestId\":\"2666359\"," + "\"TxnDateTime\":\"20180202163256703\"," +
                    //              "\"SourceId\":\"b745ef4e-5178-423f-896d-9b9fefeac5ad\"," +
                    //              "\"Msg\":\"irCZIvRJltvHPZaMQe2Q0i7+aha6YSQZFXRs46bTXBfugOO1W0FMXFr35IN542mzRqCMAxsnQuGmspOpPLt6/vFuVRaN1w1X6sxU+VsQa0pEZTtt/snQIRRFuKLDbM31+qYYmcwxs8CzuQ+d+aSu8UU7jKv2g/5mLtEBBxDIN+8=\"," +
                    //              "\"SessionId\":\"5432992580102343\"}";

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

                    //if (objResponse.Status == "000")
                    //{
                    //    Status = objResponse.Status;
                    //    Msg = objResponse.Msg;
                    //}
                    //else
                    //{
                    //    LblMessage.Text = "Failed";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //}
                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking,  " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        protected void AddAccount_Click(object sender, EventArgs e)
        {
            //
            if (CheckAccountQuantifier.Checked)
            {
                hdnCheckAccQuntifr.Value = "1";
            }
            else
            {
                hdnCheckAccQuntifr.Value = "2";
            }
            DataTable ObjDTOutPut = new DataTable();
            Status = string.Empty;
            Msg = string.Empty;
            Description = string.Empty;
            SessionId = string.Empty;
            try
            {
                ObjAcclink.BankID = Session["BankID"].ToString();
                //ObjAcclink.SourceId = Session["SourceId"].ToString();
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
                        //APIrequestparam.TxnDateTime = CurrentDatetime.ToString("yyyyMMddHHmmssffff");
                        //APIrequestparam.SourceId = Session["SourceId"].ToString();
                        APIRequestParam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //APIrequestparam.SourceId = ObjDTOutPut.Rows[0]["SourceId"].ToString();
                        APIRequestParam.SourceId = Sourceid;

                        string JsonData = JsonConvert.SerializeObject(APIRequestParam);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                            "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId +
                            " ,TxnDateTime:" + APIRequestParam.TxnDateTime +
                            " , SourceId:" + APIRequestParam.SourceId, BankId);

                        APICall(JsonData, out Status, out Msg);

                        (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                            "***Responce From CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId +
                            " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId +
                            " ,Status:" + Status + " ,Msg:" + Msg, BankId);

                        if (Status == "000")
                        {
                            string key = APIRequestParam.SourceId.Substring(0, 12) + APIRequestParam.RequestId.Substring(0, 4);
                            string DecrSessionId = (AESCrypt.Decrypt(Msg, key));
                            //{"Description":"{\"SessionId\":\"4618333687908441\"}"}
                            FunGetDeserializeObject(DecrSessionId, Description, out SessionId);
                            //string Inputdescription = Description;
                            FunAddNewAccountForCustomer(SessionId, ObjDTOutPut, APIRequestParam);

                        }
                        else
                        {
                            LblMessage.Text = "Failed";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        }
                    }
                    else
                    {
                        FunAddNewAccountForCustomer("", ObjDTOutPut, APIRequestParam);
                    }

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking, AddAccount_Click()", ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }

        }

        public void FunAddNewAccountForCustomer(string Sessionid, DataTable ObjDTOutPut, ClsAPIRequestBO APIrequestparam)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                APIrequestparam.TranType = "AddNewAccountForCustomer";
                Random r = new Random();
                var request = r.Next();
                APIrequestparam.RequestId = request.ToString();
                APIrequestparam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                APIrequestparam.SourceId = Session["SourceId"].ToString();
                APIrequestparam.SessionId = SessionId == "" ? ObjDTOutPut.Rows[0]["SessionID"].ToString() : SessionId;


                //string m = hdnRequiredData.Value;

                //APIrequestparam.Msg = "irCZIvRJltvHPZaMQe2Q0i7+aha6YSQZFXRs46bTXBfugOO1W0FMXFr35IN542mzRqCMAxsnQuGmspOpPLt6/vFuVRaN1w1X6sxU+VsQa0pEZTtt/snQIRRFuKLDbM31+qYYmcwxs8CzuQ+d+aSu8UU7jKv2g/5mLtEBBxDIN+8=";
                // APIrequestparam.SessionId = new ClsAccountLinkingDelinkingBAL().FunGetSourceIdForBank(ObjAcclink);
                //string ReqEncmsg = AESCrypt.Encrypt((JsonConvert.SerializeObject(hdnRequiredData.Value)), APIrequestparam.SessionId);
                /*APIRequestParam passes to GetRequestMessage method */
                string Message = GetRequestMessage(ObjReqmsg, APIRequestParam);
                string ReqEncmsg = AESCrypt.Encrypt(Message, APIrequestparam.SessionId);
                APIrequestparam.Msg = ReqEncmsg;
                string JsonData = JsonConvert.SerializeObject(APIrequestparam);

                string AccountLogId = "";
                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, "", "", "");

                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " RequestId:" + APIRequestParam.RequestId + " RequestMsg:" + ReqEncmsg + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId, BankId);
                APICall(JsonData, out Status, out Msg);
                string DecryptRequest = "";

                DecryptRequest = AESCrypt.Decrypt(Msg, APIrequestparam.SessionId);
                dynamic obj = JsonConvert.DeserializeObject(DecryptRequest);


                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Response From CardAPI RequestType:" + APIRequestParam.TranType + "  RequestId:" + APIRequestParam.RequestId + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId + " ,Status:" + Status + " ,Msg:" + Msg, BankId);


                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, Status, Msg, AccountLogId);
 
                AccountLogId = new ClsAccountLinkingDelinkingBAL().FunLogAccountLinkelinkRequest(ObjReqmsg.CardNo, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ObjReqmsg.AccountQualifier, ObjReqmsg.LinkingFlag, Status, DecryptRequest, AccountLogId);


 

                if (Status == "000")
                {
                    AGS.SwitchOperations.ClsAccountLink.getSessionRsp SessionRsp = JsonConvert.DeserializeObject<AGS.SwitchOperations.ClsAccountLink.getSessionRsp>(DecryptRequest);
                    dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);
                    string AccountNumber = dynamicObject.CustomerData[0].AccountNo;
                    ClsAccountLinkingDelinkingBAL.FunSyncAccountEnc(BankId, AccountNumber, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ddlCurrencyCode.SelectedValue);
                    //string AccEnc = "";
                    //var IndexOfAccountNo = DecryptRequest.IndexOf("AccountNo");
                    //if (IndexOfAccountNo > 0)
                    //{
                    //    var SubstringAccountNo = DecryptRequest.Substring(IndexOfAccountNo + 13, DecryptRequest.Length - IndexOfAccountNo - 13);
                    //    AccEnc = SubstringAccountNo.Substring(0, SubstringAccountNo.IndexOf("\""));
                    //    ClsAccountLinkingDelinkingBAL.FunSyncAccountDetails(BankId, AccountNumber, ObjReqmsg.AccountNo, ObjReqmsg.AccountType, ddlCurrencyCode.SelectedValue);
                    //}

                    //dynamic data = JObject.Parse(source);
                    SkipDialogBox = false;

                    FunSearchDetails();
                    //if (ObjReqmsg.LinkingFlag == "01")
                    //{
                    LblMessage.Text = "Account Added Successfully.";
                    //}
                    //else
                    //{
                    //    LblMessage.Text = "Failed To Link";
                    //}

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                else
                {
                    LblMessage.Text = "Failed";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
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