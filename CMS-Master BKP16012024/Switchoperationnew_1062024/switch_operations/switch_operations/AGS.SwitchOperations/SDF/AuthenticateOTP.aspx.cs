using System;
using System.Configuration;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.SDF
{
    public partial class AuthenticateOTP : System.Web.UI.Page
    {
        ClsOtpvalidBO ClsOtpvalidBO = new ClsOtpvalidBO();
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        int otpTimelimit = Convert.ToInt32(ConfigurationManager.AppSettings["Otptimelimit"].ToString());
        int otplimit = Convert.ToInt32(ConfigurationManager.AppSettings["Otplimit"].ToString());
        string pass = ConfigurationManager.AppSettings["smspass"].ToString();
        ClsSmsReqBO ClsSmsReqBO1 = new ClsSmsReqBO();
        Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {


                //ClsOtpvalidBO.Username = Session["UserName"].ToString();
                //ClsOtpvalidBO.Limits = 4;
                //ClsOtpvalidBO.request = "Y";
                //if (!IsPostBack)
                //{
                //    ClsOtpvalidBO.Otp = random.Next(100000, 999999);
                //    new ClsCommonBAL().FunOtpValidation(ClsOtpvalidBO);
                //}

            }
            catch (Exception ex)
            {
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            {


                string pass = Asplbl.Text.Trim();
                if (!string.IsNullOrEmpty(pass))
                {
                    string si = "";
                    loginErrorMsg.InnerText = "Otp invalid";

                    if (!String.IsNullOrEmpty(loginErrorMsg.InnerText))
                    {
                        ClsOtpvalidBO.Otp = Convert.ToInt32(Asplbl.Text);
                        ClsOtpvalidBO.Username = Session["UserName"].ToString();
                        ClsOtpvalidBO.request = "Y";
                        ClsOtpvalidBO.Limits = otplimit;
                        ClsOtpvalidBO.Validtill = ClsOtpvalidBO.Createdon.AddMilliseconds(otpTimelimit);
                        si = new ClsCommonBAL().FunOtpValidation(ClsOtpvalidBO);

                    }
                    if (si == "IVD")
                    {
                        loginErrorMsg.InnerText = "Otp is invalid ";
                        Response.Redirect("/SDF/AuthenticateOTP.aspx", false);
                        Asplbl.Text = "";

                    }

                    else if (si == "VLD")
                    {
                        loginErrorMsg.InnerText = "Otp is verified";
                        Response.Redirect("~/Home.aspx", false);
                    }
                    else if (si == "EPR")
                    {
                        loginErrorMsg.InnerText = "Otp is Expired ";
                        resenddiv.Disabled = true;
                        Response.Redirect("/SDF/Login.aspx", false);
                    }
                    else if (si == "TMT")
                    {

                        loginErrorMsg.InnerText = "Time exceeds";
                        resenddiv.Disabled = true;
                        Response.Redirect("/SDF/Login.aspx", false);

                    }
                    else
                    {
                        loginErrorMsg.InnerText = "Otp is invalid";

                        Response.Redirect("/SDF/AuthenticateOTP.aspx", false);

                    }

                }
                else
                {
                    loginErrorMsg.InnerText = "Otp is invalid";

                    Response.Redirect("/SDF/AuthenticateOTP.aspx", false);

                }

            }
            catch (Exception ex) { }


            //Response.Redirect("~/Home.aspx", false);
        }

        protected void Resend_otp(object sender, EventArgs e)
        {
            loginErrorMsg.InnerText = "Resent OTP";
            ClsOtpvalidBO.Validtill = ClsOtpvalidBO.Createdon.AddMilliseconds(otpTimelimit);
            ClsOtpvalidBO.Username = Session["UserName"].ToString();
            ClsOtpvalidBO.Limits = 0;
            ClsOtpvalidBO.Createdon = DateTime.Now;
            ClsOtpvalidBO.request = "N";
            ClsOtpvalidBO.Otp = random.Next(100000, 999999);
            try
            {
                new ClsCommonBAL().FunOtpValidation(ClsOtpvalidBO);
            }
            catch (Exception ex) { }
            if (!string.IsNullOrEmpty(Session["MobileNo"].ToString()))
            {
                ClsSmsReqBO1.destination = Session["MobileNo"].ToString();
            }
            ClsSmsReqBO1.q = pass;
            ClsSmsReqBO1.message = "Your OTP to login CMS portal:  " + ClsOtpvalidBO.Otp.ToString();
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            SmsService smsService = new SmsService();
            ClsSmsResBO clsSmsResBO = new ClsSmsResBO();
            var sm = SmsService.sendsms(ClsSmsReqBO1);
            string msg = $"{sm}";
            clsSmsResBO.Response = sm.Result.Response.ToString();
            if (clsSmsResBO.Response == "0")
                Response.Redirect("/SDF/AuthenticateOTP.aspx", false);

        }
    }
}