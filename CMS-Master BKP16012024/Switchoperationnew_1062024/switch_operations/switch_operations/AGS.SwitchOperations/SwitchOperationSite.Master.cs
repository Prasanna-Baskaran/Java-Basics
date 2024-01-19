using AGS.SwitchOperations.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace AGS.SwitchOperations
{
    public partial class SwitchOperationSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strAuthKey = string.Empty;
              //  new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", "Sessionkey and AuthKey", "LoginSessionKey" + Convert.ToString(Session["AuthKey"]) + "LoginID:" + Convert.ToString(Session["LoginID"])+ "SystemID:"+ Convert.ToString(Session["SystemID"]));
                // here session going to be check 
                if ((Session["LoginID"] == null) || (Session["SystemID"] == null) || (Session["BankID"] == null))
                {
                    //new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", "Sessionkey and AuthKeyLoginSessioncheck ", "LoginSessionKey" + Session["AuthKey"].ToString() );
                    goto lblResult;
                }
                else
                {
                    //Check Login auth key and latest sessionkey
                    using (DataTable dtSessionKey = (new ClsCommonBAL()).FunGetCommonDataTable(26, Session["SystemID"].ToString() + "," + Session["BankID"].ToString() + "," + Session["LoginID"]))
                    {

                        if (dtSessionKey != null && dtSessionKey.Rows.Count == 1 && (dtSessionKey.Columns.Contains("AuthSessionKey")))
                        {
                            if (Session["AuthKey"].ToString() == dtSessionKey.Rows[0]["AuthSessionKey"].ToString())
                            {
                                strAuthKey = dtSessionKey.Rows[0]["AuthSessionKey"].ToString();
                            }
                          //  new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", "Sessionkey and AuthKey", "LoginSessionKey" + Session["AuthKey"].ToString() + "SessionKey" + dtSessionKey.Rows[0]["AuthSessionKey"].ToString());
                        }
                    }

                    if (!IsPostBack)
                    {
                        try
                        {
                            lblLoginUser.Text = Session["FirstName"] + " " + Session["LastName"];
                            string LoginID = Session["LoginID"].ToString();
                            string SystemID = Session["SystemID"].ToString();
                            //Sheetal
                            //set span bankname on master
                            Bankname.InnerText = Session["BankName"].ToString();
                            //Get Access menu List


                            imgBankLogo.ImageUrl = Session["BankLogoPath"].ToString();

                            HttpCookie cookie_LoginPagePath = new HttpCookie("LoginPagePath");
                            cookie_LoginPagePath.Value = Session["LoginPagePath"].ToString();
                            cookie_LoginPagePath.Expires = DateTime.Now.AddDays(1);
                            HttpContext.Current.Response.Cookies.Add(cookie_LoginPagePath);


                            //using (DataTable dtOptions = (new ClsCommonBAL()).FunGetCommonDataTable(10, LoginID + "," + SystemID))
                            using (DataTable dtOptions = (DataTable)Session["MenuList"])
                            {
                                if (dtOptions != null && dtOptions.Rows.Count > 0)
                                {
                                    demoList.InnerHtml = getInnerMenus(dtOptions, "", true);
                                }
                            }
                            if (Session["UserRights"] == null)
                            {
                                //Get page wise rights
                                DataTable dtRights = (new ClsCommonBAL()).FunGetCommonDataTable(10, LoginID + "," + SystemID + "," + 1);
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
                            }
                        }
                        catch (Exception ex)
                        {
                            strAuthKey = string.Empty;
                            new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()BindMenu", ex.Message, "");
                        }
                    }
                }

                lblResult:
                if (string.IsNullOrEmpty(strAuthKey))
                {
                    //Session.Clear();
                    //Session.Abandon();
                    //Session.RemoveAll();
                    //Session["SystemID"] = null;
                    //Session["BankID"] = null;
                    //Session["LoginID"] = null;
                    //if (Request.Cookies["ASP.NET_SessionId"] != null)
                    //{
                    //    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    //    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                    //}

                    //Diksha security  changes
                    // Response.Redirect("Login.aspx", true);

                    // Response.Redirect("Login.aspx",false);
                    Logout_Redirection();
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", ex.Message, "");
            }
        }
        private string getInnerMenus(DataTable dtOptions, string strParentNeumonic, bool isFirstUl)
        {
            try
            {
                string strListItems = string.Empty, strSubMenuStart = string.Empty, strSubMenuEnd = string.Empty;
                DataRow[] drParents = dtOptions.Select("OptionParentNeumonic='" + strParentNeumonic + "'");
                if (drParents.Length > 0)
                {
                    if (!isFirstUl)
                    {
                        strSubMenuStart = "<ul class=\"submenu\">";
                        strSubMenuEnd = "</ul>";
                    }
                    strListItems += strSubMenuStart;
                    foreach (DataRow drOptions in drParents)
                    {

                        strListItems += "<li class=\"menu\"> <a  href=\"" + (Convert.ToString(drOptions["URL"]).Trim() == "" ? "#" : Convert.ToString(drOptions["URL"])) + "\"><i class=\"" + Convert.ToString(drOptions["ClassName"]) + "\"></i>" + Convert.ToString(drOptions["OptionName"]) + "<span class=\"fa fa-chevron-down\"></a>";
                        strListItems += getInnerMenus(dtOptions, Convert.ToString(drOptions["OptionNeumonic"]), false);
                        //strListItems += (!string.IsNullOrEmpty(strInnerMenus) ? strSubMenuStart + strInnerMenus + strSubMenuEnd : string.Empty);
                        strListItems += "</li>";
                    }
                    strListItems += strSubMenuEnd;
                }
                return strListItems;
            }
            catch
            {
                return string.Empty;
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Logout_Redirection();
        }

        protected void btnChangeSystem_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SystemID"] = null;
                Session["UserRights"] = null;
                Response.Redirect("SystemChange.aspx", false);
            }
            catch (Exception)
            { }

        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ChangePassword.aspx", false);
            }
            catch (Exception)
            { }

        }
        

        public void Logout_Redirection()
        {
            try
            {
           
                string LoginPagePath = string.Empty;

                if (Session["LoginPagePath"] != null)
                {
                    LoginPagePath = Session["LoginPagePath"].ToString();
                }
                else
                {

                   HttpCookie cookie_LoginPagePath = HttpContext.Current.Request.Cookies["LoginPagePath"];

                    if (cookie_LoginPagePath != null)
                    {
                        LoginPagePath = cookie_LoginPagePath.Value;
                    }
                }


                Session.Clear();
                Session.Abandon();
                Session.RemoveAll(); //REQUEST / RESPONSE
                Session["SystemID"] = null;
                Session["BankID"] = null;
                Session["LoginID"] = null;

                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }



                if (!string.IsNullOrEmpty( LoginPagePath))
                {
                    Response.Redirect(LoginPagePath, false);
                }


            }
            catch (Exception)
            { }
        }
    }
}