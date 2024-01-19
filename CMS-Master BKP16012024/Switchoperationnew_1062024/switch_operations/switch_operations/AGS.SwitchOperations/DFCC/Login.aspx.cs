using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.Common;
using System.Data;
using System.Configuration;

namespace AGS.SwitchOperations.DFCC
{
    public partial class DFCCLogin : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {

            loginErrorMsg.Attributes.Add("class", "alert alert-error hide");
            loginErrorMsg.InnerText = "";

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
               
                ClsLoginBO objLogin = new ClsLoginBO();

                objLogin.BankID = "6094";
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                if ((!string.IsNullOrEmpty(txtUsername.Value)) && (!string.IsNullOrEmpty(txtPwd.Value)))
                {
                    objLogin.UserName = txtUsername.Value.Trim();
                    objLogin.Password = txtPwd.Value.Trim();
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
    }
}