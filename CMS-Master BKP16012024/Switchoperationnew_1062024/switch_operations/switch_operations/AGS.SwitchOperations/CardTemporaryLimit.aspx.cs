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
    public partial class CardTemporaryLimit : System.Web.UI.Page
    {
        ClsAPIRequestBO APIRequestParam = new ClsAPIRequestBO();

        TemporaryLimitBO ObjTemporaryLimitBO = new TemporaryLimitBO();
        TemporaryLimitRequestMSGParam ObjReqmsg = new TemporaryLimitRequestMSGParam();
        
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

                if (!Page.IsPostBack)
                {

                    string OptionNeumonic = "CMTCL"; //unique optionneumonic from database
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
            catch(Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardTemporaryLimit, Page_Load()", ex.Message, BankId);
            }
          }
            
        
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable ObjDTOutPut = new DataTable();

            try
            {
                string BankID = Session["BankID"].ToString();
                string Sourceid = Session["SourceId"].ToString();
                
               
                
                if (string.IsNullOrEmpty(BankID) || string.IsNullOrEmpty(Sourceid))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
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

                            (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " ,RequestId:" + APIRequestParam.RequestId +
                                " ,TxnDateTime:" + APIRequestParam.TxnDateTime +
                                " , SourceId:" + APIRequestParam.SourceId, BankId);

                            APICall(JsonData, out Status, out Msg);

                            (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit " + System.Reflection.MethodBase.GetCurrentMethod().Name,
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
                                TemporaryLimit(SessionId, ObjDTOutPut, APIRequestParam);

                            }
                            else if (Status == "108")
                            {

                                LblMessage.Text = "Please check card limit in sometime";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                            }
                            else
                            {
                                LblMessage.Text = "Failed";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                            }
                        }
                        else
                        {
                            TemporaryLimit("", ObjDTOutPut, APIRequestParam);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit, CardTemporaryLimit()", ex.ToString(), BankId);
                LblMessage.Text = ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit,  " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }


        public void FunGetDeserializeObject(string DecrSessionId, string Description, out string SessionId)
        {
            
            
                Description = string.Empty;
                SessionId = string.Empty;
            try
            {
                ClsGetsessionBO objResponse = JsonConvert.DeserializeObject<ClsGetsessionBO>(DecrSessionId);

                {
                    Description = objResponse.Description;
                }
                ClsGetSession GetSession = JsonConvert.DeserializeObject<ClsGetSession>(Description);
                {
                    SessionId = GetSession.SessionId;

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit,  " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);

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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardTemporaryLimit, InitiateSSLTrust()", Ex.Message, "");
                //ActivityLog.InsertSyncActivity(ex);

            }
        }

        public void TemporaryLimit(string Sessionid, DataTable ObjDTOutPut, ClsAPIRequestBO APIrequestparam)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                APIrequestparam.TranType = "TemporaryLimit";
                Random r = new Random();
                var request = r.Next();
                APIrequestparam.RequestId = request.ToString();
                APIrequestparam.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                APIrequestparam.SourceId = Session["SourceId"].ToString();
                APIrequestparam.SessionId = SessionId == "" ? ObjDTOutPut.Rows[0]["SessionID"].ToString() : SessionId;


                
                string Message = GetRequestMessage(ObjReqmsg, APIrequestparam);
                string ReqEncmsg = AESCrypt.Encrypt(Message, APIrequestparam.SessionId);
                APIrequestparam.Msg = ReqEncmsg;
                string JsonData = JsonConvert.SerializeObject(APIrequestparam);


                string AccountLogId = "";
                AccountLogId = new CardTemporaryLimitBAL().FunLogTemporarylimitRequest(ObjReqmsg, APIRequestParam,"","","");
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Sending Request to CardAPI RequestType:" + APIRequestParam.TranType + " RequestId:" + APIRequestParam.RequestId + " RequestMsg:" + ReqEncmsg + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId, BankId);

                APICall(JsonData, out Status, out Msg);
                string DecryptRequest = "";

                DecryptRequest = AESCrypt.Decrypt(Msg, APIrequestparam.SessionId);

                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit " + System.Reflection.MethodBase.GetCurrentMethod().Name, "***Response From CardAPI RequestType:" + APIRequestParam.TranType + "  RequestId:" + APIRequestParam.RequestId + " ,TxnDateTime:" + APIRequestParam.TxnDateTime + " , SourceId:" + APIRequestParam.SourceId + " ,Status:" + Status + " ,Msg:" + Msg, BankId);

                AccountLogId = new CardTemporaryLimitBAL().FunLogTemporarylimitRequest(ObjReqmsg, APIRequestParam, Status, Msg, AccountLogId);

                //AccountLogId = new CardTemporaryLimitBAL().FunLogTemporarylimitRequest(ObjReqmsg, APIRequestParam, Status, DecryptRequest, AccountLogId);

                if (Status == "000")
                {
                    SkipDialogBox = false;
                    //FunSearchDetails();

                    //if (ObjReqmsg.LinkingFlag == "01")
                    //{
                    //    LblMessage.Text = "Account Linked Successfully.";
                    //}
                    //else
                    //{
                    //    LblMessage.Text = "Account De-Linked Successfully.";
                    //}
                    LblMessage.Text = "Card temporary limit set successfully.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                else if (Status == "108")
                {

                    LblMessage.Text = "Please check card limit in sometime";
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                LblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        public string GetRequestMessage(TemporaryLimitRequestMSGParam ObjReqmsg, ClsAPIRequestBO APIRequestParam)
        {
            
            
                //string ObjReturnStatus = string.Empty;
          if (APIRequestParam.TranType == "TemporaryLimit")
                {
                try
                {
                    ObjReqmsg.CardNo = txtCardNo.Value;//"6280143030019111";
                    //perdaylimit reser8
                    ObjReqmsg.reserved8 = txtPerDayLimit.Value; //"10000";
                    ObjReqmsg.reserved1 = txtPerTxnLimit.Value; //"1000";
                    ObjReqmsg.reserved2 = txtPerDayCount.Value;//"10";perdaycount
                    ObjReqmsg.reserved3 = txtOverallLimit.Value; //"20000";
                    ObjReqmsg.reserved4 = txtOverallCount.Value;//"20";
                    ObjReqmsg.reserved5 = txtToDate.Value;//"01-01-19";
                    ObjReqmsg.reserved6 = txtFromDate.Value;//"02=01-19";
                }
                catch(Exception ex)
                {
                    (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardTemporaryLimit, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                    
                }
            }

                string ObjReturnStatus = JsonConvert.SerializeObject(ObjReqmsg);
                return ObjReturnStatus;
            }
            

        }
    }
