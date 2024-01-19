using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.SDF
{
    public partial class SDFLogin : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsOtpvalidBO ClsOtpvalidBO = new ClsOtpvalidBO();
        int otpTimelimit = Convert.ToInt32(ConfigurationManager.AppSettings["Otptimelimit"].ToString());
        int otplimit = Convert.ToInt32(ConfigurationManager.AppSettings["Otplimit"].ToString());
        string pass = ConfigurationManager.AppSettings["smspass"].ToString();
        ClsSmsReqBO ClsSmsReqBO1 = new ClsSmsReqBO();
        Random random = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {

            loginErrorMsg.Attributes.Add("class", "alert alert-error hide");
            loginErrorMsg.InnerText = "";
            if (!IsPostBack)
            {
                hfPublicKey.Value = new RSABuilder().InitiateRSA();
            }

        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                ClsLoginBO objLogin = new ClsLoginBO();

                objLogin.BankID = "6093";
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                if ((!string.IsNullOrEmpty(txtUsername.Value)) && (!string.IsNullOrEmpty(txtPwd.Value)))
                {
                    objLogin.UserName = txtUsername.Value.Trim();
                    objLogin.UserName = new RSABuilder().Decrypt(txtUsername.Value.Trim());
                    objLogin.Password = txtPwd.Value.Trim();
                    objLogin.Password = new RSABuilder().Decrypt(txtPwd.Value.Trim());
                    byte[] bytesToDecode1 = Convert.FromBase64String(objLogin.UserName);

                    string decodedString1 = System.Text.Encoding.UTF8.GetString(bytesToDecode1);
                    /**************************************************************/
                    objLogin.UserName = decodedString1;
                    byte[] bytesToDecode = Convert.FromBase64String(objLogin.Password);

                    string decodedString = System.Text.Encoding.UTF8.GetString(bytesToDecode);
                    /**************************************************************/
                    objLogin.Password = decodedString;

                    /************************************************************************Prasanna-02.08.23*/
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(objLogin.Password);

                    // Encode the bytes to Base64
                    string base64Encoded = Convert.ToBase64String(bytesToEncode);
                    /**************************************************************/
                    objLogin.Password = base64Encoded;
                    ObjReturnStatus = new ClsCommonBAL().FunLoginValidate(objLogin);
                    if (ObjReturnStatus.Code == 0)
                    {
                        //if (Request.Cookies["ASP.NET_SessionId"] != null)
                        //{
                        //    if (ConfigurationManager.AppSettings["SetCookiePath"].ToString() == "1")
                        //    {
                        //        Response.Cookies["ASP.NET_SessionId"].Path = ConfigurationManager.AppSettings["CookiePath"].ToString();
                        //    }
                        //}

                        Session["UserName"] = objLogin.UserName;
                        Session["BranchCode"] = objLogin.BankCode;
                        Session["IsAdmin"] = objLogin.IsAdmin;

                        Session["LoginID"] = objLogin.UserID;
                        Session["FirstName"] = objLogin.FirstName;
                        Session["LastName"] = objLogin.Lastname;
                        Session["MobileNo"] = objLogin.MobileNo;
                        // Session["session_key"] = parameters[10].Split('=')[1];
                        Session["EmailId"] = objLogin.EmailId;
                        Session["UserRoleID"] = objLogin.UserRoleID;

                        //Server.Transfer("Dashboard.aspx");
                        //  Session["UserRights"] = objLogin.UserRightsList;
                        Session["BankID"] = objLogin.BankID;


                        Session["LoginPagePath"] = objLogin.LoginPagePath;
                        Session["BankLogoPath"] = objLogin.BankLogoPath;



                        //if(objLogin.SystemID.Split(','))
                        Session["AccessSystemLst"] = objLogin.SystemID;
                        Session["AuthKey"] = objLogin.AuthKey;
                        Session["UserPrefix"] = objLogin.UserPrefix;


                        //sourceid for card api call account link delink
                        Session["SourceId"] = objLogin.SourceId;
                        //Start Sheetal for logo change
                        Session["BankName"] = objLogin.BankName;

                        Session["IssuerNo"] = objLogin.IssuerNo;
                        Session["IsEPS"] = objLogin.IsEPS;
                        Session["iInstaEdit"] = objLogin.iInstaEdit; //added for ATPCM-759

                        Session["ParticipantId"] = objLogin.ParticipantId;// added for NTPCM-132 denomination count update dfcc
                        Session["EnableOtp"] = objLogin.EnableOtp;


                        //if user has multiple system access
                        string[] Systemaccess = objLogin.SystemID.Split(',');
                        if (Systemaccess.Count() == 1)
                        {
                            Session["SystemID"] = objLogin.SystemID;
                            Session["SystemName"] = objLogin.SystemName;
                            //start Diksha Get Menu And Submenu  and access rights
                            DataTable dtRights = (new ClsCommonBAL()).FunGetCommonDataTable(10, Session["LoginID"].ToString() + "," + objLogin.SystemID + "," + 1);
                            Dictionary<string, string> ObjDict = new Dictionary<string, string>();

                            foreach (DataRow dr in dtRights.Rows)
                            {
                                string OptionNeumonic = dr["OptionNeumonic"].ToString();
                                string AccessCaptions = dr["AccessCaptions"].ToString();
                                if (!ObjDict.ContainsKey(OptionNeumonic))
                                {
                                    ObjDict.Add(OptionNeumonic, AccessCaptions);
                                }

                            }
                            Session["UserRights"] = ObjDict;
                            DataTable dtOptions = (new ClsCommonBAL()).FunGetCommonDataTable(10, Session["LoginID"].ToString() + "," + objLogin.SystemID);
                            Session["MenuList"] = dtOptions;
                            //   new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", "Redirecting to Home.aspx","");
                            ClsCommonDAL.FunSOAL(objLogin.UserID, objLogin.IssuerNo, "LogIn", "User Logged in sucessfully", "", "", "", "", "", "", "Login", "1");
                            if (objLogin.EnableOtp == 1 && !string.IsNullOrEmpty(objLogin.MobileNo))
                            {
                                ClsOtpvalidBO.Username = Session["UserName"].ToString();
                                ClsOtpvalidBO.request = "N";
                                ClsOtpvalidBO.Limits = otplimit;
                                ClsOtpvalidBO.Validtill = ClsOtpvalidBO.Createdon.AddMilliseconds(otpTimelimit);
                                int sc = 123456;
                                ClsOtpvalidBO.Otp = random.Next(100000, 999999);
                                ClsOtpvalidBO.Otp = (ClsOtpvalidBO.Otp ^ sc);
                                new ClsCommonBAL().FunOtpValidation(ClsOtpvalidBO);
                                if (!string.IsNullOrEmpty(objLogin.MobileNo) && objLogin.IsAdmin == true)
                                {
                                    ClsSmsReqBO1.destination = objLogin.MobileNo;
                                }
                                else
                                {
                                    loginErrorMsg.Attributes.Add("class", "alert alert-error");
                                    loginErrorMsg.InnerText = "Admin User failed to login";
                                }

                                ClsSmsReqBO1.q = pass;
                                ClsSmsReqBO1.message = "Your OTP to login CMS portal:  " + (ClsOtpvalidBO.Otp ^ sc).ToString();
                                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                                SmsService smsService = new SmsService();
                                ClsSmsResBO clsSmsResBO = new ClsSmsResBO();
                                try
                                {
                                    var sm = SmsService.sendsms(ClsSmsReqBO1);
                                    string msg = $"{await sm}";
                                    clsSmsResBO.Response = sm.Result.Response.ToString();
                                    //clsSmsResBO.Response = "0";
                                }
                                catch (Exception ex) { }

                                if (clsSmsResBO.Response == "0")
                                {
                                    Response.Redirect("/SDF/AuthenticateOTP.aspx", false);
                                }
                                else
                                {
                                    loginErrorMsg.Attributes.Add("class", "alert alert-error");
                                    loginErrorMsg.InnerText = "Admin User failed to login";
                                }

                            }


                            else
                                Response.Redirect("~/Home.aspx", false);

                            //Response.RedirectPermanent("Home.aspx", false);
                        }
                        else
                        {
                            //Session["SystemList"] = objLogin.SystemID;
                            Session["SystemID"] = null;
                            ClsCommonDAL.FunSOAL(objLogin.UserID, objLogin.IssuerNo, "LogIn", "User Log in failed", "", "", "", "", "", "", "Login", "0");
                            Response.Redirect("SystemChange.aspx", false);
                        }
                    }
                    else
                    {

                        loginErrorMsg.Attributes.Add("class", "alert alert-error");
                        loginErrorMsg.InnerText = ObjReturnStatus.Description.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, Login, btnLogin_Click()", Ex.Message, Ex.StackTrace);
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                ClsUserBO objReset = new ClsUserBO();

                if (string.IsNullOrEmpty(txtresetusername.Value))
                {
                    loginErrorMsg.Attributes.Add("class", "alert alert-error");
                    loginErrorMsg.InnerText = "Please Enter Username";
                    return;
                }
                else
                {
                    objReset.UserName = txtresetusername.Value.Trim();
                    ClsReturnStatusBO result = SendPasswordResetEmail(objReset.UserName);
                    if (result.Code > 0)
                    {
                        //ClsCommonDAL.FunSOAL("Password reset link sent Success.", "UserName:" + objReset.UserName);
                        (new ClsCommonBAL()).FunLog("CS, Login, btnResetPassword_Click()", "Password reset link sent Success. for username: " + objReset.UserName, "");
                        loginErrorMsg.Attributes.Add("class", "alert alert-success");
                        loginErrorMsg.InnerText = result.Description;
                    }
                    else
                    {
                        //BAL.DebugLog("Password reset link sent failed.", "UserName:" + objReset.UserName);
                        (new ClsCommonBAL()).FunLog("CS, Login, btnResetPassword_Click()", "Password reset link sent Success. for failed: " + objReset.UserName, "");
                        loginErrorMsg.Attributes.Add("class", "alert alert-error");
                        loginErrorMsg.InnerText = result.Description;
                    }
                }
            }
            catch (Exception Ex)
            {
                //BAL.ErrorLog(Ex, "")
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, Login, btnResetPassword_Click()", Ex.Message, Ex.StackTrace);
                loginErrorMsg.Attributes.Add("class", "alert alert-error");
                loginErrorMsg.InnerText = Ex.Message;
            }
        }

        public ClsReturnStatusBO SendPasswordResetEmail(string username)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            try
            {

                ClsUserBO objUser = new ClsUserBO();
                DataTable dt = new DataTable();
                objUser.UserName = username;
                dt = (new DataLogics.ClsUserDetailsDAL()).FunGetUserForgetPassDetails(objUser, 1);
                if (dt == null || dt.Rows.Count < 1)
                {
                    ObjReturnStatus.Code = 0;
                    ObjReturnStatus.Description = "Faild";
                    //BAL.DebugLog("No User details found", "");
                    (new ClsCommonBAL()).FunLog("CS, Login, SendPasswordResetEmail()", "No User details found", "");
                }
                else if (!Convert.ToBoolean(dt.Rows[0]["Result"]))
                {
                    ObjReturnStatus.Code = 0;
                    ObjReturnStatus.Description = Convert.ToString(dt.Rows[0]["Description"]);
                }
                else
                {
                    DataTable mailconfig = ClsCommonDAL.FunGetEmailConfig(Convert.ToString(dt.Rows[0]["Issuer"]), "ResetUserPassword");
                    if (mailconfig != null)
                    {
                        mailconfig.Columns.Add("ID");
                        mailconfig.Columns.Add("Email");
                        mailconfig.Columns.Add("FileName");
                    }
                    else
                    {
                        //BAL.DebugLog("No mail configuration details found for password reset", "");
                        (new ClsCommonBAL()).FunLog("CS, Login, SendPasswordResetEmail()", "No mail configuration details found for password reset", "");
                        ObjReturnStatus.Code = 0;
                        ObjReturnStatus.Description = "No mail configuration details found";
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToBoolean(dr["Result"]))
                        {
                            mailconfig.Rows[0]["ID"] = dr["UserId"];
                            mailconfig.Rows[0]["Email"] = dr["Email"];
                            mailconfig.Rows[0]["body"] = mailconfig.Rows[0]["body"].ToString().Replace("{{guid}}", dr["UniqueId"].ToString());
                            mailconfig.Rows[0]["body"] = mailconfig.Rows[0]["body"].ToString().Replace("{{name}}", dr["name"].ToString());
                            ObjReturnStatus.Code = 0;
                            ObjReturnStatus.Description = "Email not sent";
                            ClsEmailSMS mailsms = new ClsEmailSMS();
                            string response = mailsms.SendEmail(mailconfig.Rows[0]);
                            if ("success" == response)
                            {
                                ObjReturnStatus.Code = 1;
                                ObjReturnStatus.Description = "The password reset link is sent to the registered Email.";
                            }
                            else
                            {
                                ObjReturnStatus.Code = 0;
                                ObjReturnStatus.Description = response;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //BAL.ErrorLog(e, "");
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, Login, SendPasswordResetEmail()", e.Message, e.StackTrace);
                ObjReturnStatus.Code = 0;
                ObjReturnStatus.Description = e.Message;
            }
            return ObjReturnStatus;
        }

    }
}