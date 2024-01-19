using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using AGS.SwitchOperations.Validator;
namespace AGS.SwitchOperations
{
    public partial class UserDetails : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "MUD"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;

                        FunGetResult();
                        if (!Page.IsPostBack)
                        {
                            //hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(16, string.Empty);
                            DDLUserRole.Items.Insert(0, new ListItem("Select", "0"));
                            FunBindDDL();
                        }

                    }
                    // Redirect to access denied page
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
                    }
                }
                // Redirect to access denied page
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }

            }
            catch (Exception ex )
            {
               // new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, Page_Load()", ex.Message, "");
            }



        }

        public void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(16, Session["BankID"].ToString() + "," + Session["AccessSystemLst"].ToString().Replace(",", "|") + "," + Session["LoginID"].ToString());
                //int RoleID = Convert.ToInt16(Session["UserRoleID"]);

            }
            catch (Exception ex) { }
            if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //For user those having Edit right
                    if (accessPrefix.Contains("E"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData()
                        {
                            control = AGS.Utilities.Controls.Button,
                            buttonName = "Edit",
                            cssClass = "btn btn-primary"
                                              ,
                            hideColumnName = true,
                            events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                              ,
                            attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "UserID" }
                                              , new AGS.Utilities.Attribute() { AttributeName = "BranchCode", BindTableColumnValueWithAttribute = "BranchCode" }
                        , new AGS.Utilities.Attribute() { AttributeName = "firstname", BindTableColumnValueWithAttribute = "FirstName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "lastname", BindTableColumnValueWithAttribute = "LastName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "username", BindTableColumnValueWithAttribute = "UserName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "userrole", BindTableColumnValueWithAttribute = "UserRole" }
                        , new AGS.Utilities.Attribute() { AttributeName = "mobile", BindTableColumnValueWithAttribute = "MobileNo" }
                        , new AGS.Utilities.Attribute() { AttributeName = "email", BindTableColumnValueWithAttribute = "EmailId" }
                        , new AGS.Utilities.Attribute() { AttributeName = "userstatus", BindTableColumnValueWithAttribute = "UserStatus" }
                        , new AGS.Utilities.Attribute() { AttributeName = "roleid", BindTableColumnValueWithAttribute = "RoleId" }
                        , new AGS.Utilities.Attribute() { AttributeName = "activeid", BindTableColumnValueWithAttribute = "Activeid" }
                        , new AGS.Utilities.Attribute() { AttributeName = "systemid", BindTableColumnValueWithAttribute = "SystemID" }
                        , new AGS.Utilities.Attribute() { AttributeName = "systemname", BindTableColumnValueWithAttribute = "SystemName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "isedit", BindTableColumnValueWithAttribute = "IsEdit" }
                        , new AGS.Utilities.Attribute() { AttributeName = "approveddate", BindTableColumnValueWithAttribute = "ApprovedDate" }
                        
                                     }
                        };

                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("UserID,RoleId,Activeid,SystemID,IsEdit,approveddate", objAdded);
                    }
                    else
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("UserID,RoleId,Activeid,SystemID,IsEdit,approveddate");
                }

        }

        protected void add_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
            {
                new ValidatorAttr { Name="First Name", Value= txt_firstname.Value, MinLength = 0, MaxLength = 20, Isrequired = true, Alpabetic=true },
                new ValidatorAttr { Name="Last Name", Value= txt_lastname.Value, MinLength = 0, MaxLength = 20, Isrequired = true, Alpabetic=true},
                new ValidatorAttr { Name="Username", Value= txt_username.Value, MinLength = 0, MaxLength = 15, Isrequired = true, AlphaNumeric=true },
                new ValidatorAttr { Name="Mobile No", Value= txt_mobileno.Value, MinLength = 0, MaxLength = 10, Isrequired = true, Numeric=true },
                new ValidatorAttr { Name="Email ID", Value= txt_emailid.Value, MinLength = 0, MaxLength = 50, Isrequired = true, Regex="^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$" },
                new ValidatorAttr { Name="User Status", Value= dropdown_status.SelectedValue, MinLength = 1,MaxLength = 1, Isrequired = true, Regex="^[1-9]*$" },
                new ValidatorAttr { Name="User Role", Value= DDLUserRole.SelectedValue, Isrequired = true, Regex="^[1-9]*$" },
                new ValidatorAttr { Name="Branch", Value= DDLUserRole.SelectedValue, MinLength = 1, Isrequired = true, Numeric=true, Regex="^[1-9]*$" },
                new ValidatorAttr { Name = "Password", Value = txt_password.Value, MinLength = 0, MaxLength = 30, Isrequired = !(txt_userid.Value == null || txt_userid.Value == ""), Regex = "^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$" },
                new ValidatorAttr { Name = "Confirm Password", CompareName = "Password", CompareValue = txt_password.Value, Value = txt_retrypassword.Value, MinLength = 10, Isrequired = !(txt_userid.Value == null || txt_userid.Value == ""), Regex = "^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$" }
            };
            if (!ListValid.Validate(out msg))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "$('#AddEditModal').modal('show'); validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
            }
            else
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                DataTable ObjDTOutPut = new DataTable();
                ClsUserDetailsBO ObjPriFeeBO = new ClsUserDetailsBO() { FirstName = txt_firstname.Value.Trim(), LastName = txt_lastname.Value.Trim(), UserName = txt_username.Value.Trim(), MobileNo = txt_mobileno.Value.ToString().Trim(), Emailid = txt_emailid.Value, UserRole = DDLUserRole.SelectedValue, UserStatus = dropdown_status.SelectedValue, UserPassword = txt_password.Value, Userid = txt_userid.Value, SystemID = hdnSystems.Value, BranchCode = DDLBranch.SelectedItem.Text };
                if (ObjPriFeeBO.Userid == "" || ObjPriFeeBO.Userid == null)
                {

                    try
                    {
                        ObjReturnStatus = (new ClsUserDetailsBAL()).FunInsertIntoUserUserDetails(ObjPriFeeBO.FirstName, ObjPriFeeBO.LastName, Convert.ToString(Session["UserPrefix"]) + ObjPriFeeBO.UserName, ObjPriFeeBO.MobileNo, ObjPriFeeBO.Emailid, Convert.ToInt32(ObjPriFeeBO.UserStatus), Convert.ToInt32(Session["LoginID"]), Convert.ToInt32(ObjPriFeeBO.UserRole), ObjPriFeeBO.SystemID, Convert.ToString(Session["BankID"]), txt_password.Value, ObjPriFeeBO.BranchCode);
                        LblMsg.InnerText = ObjReturnStatus.Description;
                        hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                        FunGetResult();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                    }


                    catch (Exception ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, add_click_btn()", ex.Message, "");
                    }

                    finally
                    {
                        //hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(16, string.Empty);
                        FunGetResult();

                        var flag = Convert.ToInt32(hdnResultStatus.Value);
                        if (flag == 0)
                        {
                            FunBindDDL();
                            txt_firstname.Value = string.Empty;
                            txt_lastname.Value = string.Empty;
                            txt_username.Value = string.Empty;
                            txt_mobileno.Value = string.Empty;
                            txt_emailid.Value = string.Empty;
                            txt_password.Value = string.Empty;
                            txt_userid.Value = string.Empty;
                            txt_retrypassword.Value = string.Empty;
                            dropdown_status.SelectedIndex = 0;
                        }
                        else
                        {

                        }

                    }
                }

                else
                {
                    try
                    {
                        ObjReturnStatus = (new ClsUserDetailsBAL()).FunUpdateIntoUserUserDetails(ObjPriFeeBO.FirstName, ObjPriFeeBO.LastName, ObjPriFeeBO.MobileNo, ObjPriFeeBO.Emailid, Convert.ToInt32(ObjPriFeeBO.UserStatus), Convert.ToInt16(Session["LoginID"]), Convert.ToInt32(ObjPriFeeBO.UserRole), ObjPriFeeBO.UserPassword, Convert.ToInt32(ObjPriFeeBO.Userid), hdnSystems.Value, Convert.ToString(Session["BankID"]), ObjPriFeeBO.BranchCode);
                        LblMsg.InnerText = ObjReturnStatus.Description;
                        hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }
                    catch (Exception ex)
                    {

                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, add_click_btn()", ex.Message, "");

                    }

                    finally
                    {

                        //hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(16, string.Empty);
                        FunGetResult();

                        FunBindDDL();
                        txt_firstname.Value = string.Empty;
                        txt_lastname.Value = string.Empty;
                        txt_username.Value = string.Empty;
                        txt_mobileno.Value = string.Empty;
                        txt_emailid.Value = string.Empty;
                        txt_password.Value = string.Empty;
                        txt_userid.Value = string.Empty;
                        txt_retrypassword.Value = string.Empty;
                        dropdown_status.SelectedIndex = 0;


                    }
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "UserDetails.aspx", "Add button clicked", "");
            }
        }

        protected void FunBindDDL()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = (new ClsCommonBAL()).FunGetCommonDataTable(2, Session["BankID"].ToString());
                DDLUserRole.DataSource = ObjDTOutPut;
                DDLUserRole.DataTextField = "RoleName";
                DDLUserRole.DataValueField = "UserRoleID";
                DDLUserRole.DataBind();
                DDLUserRole.Items.Insert(0, new ListItem("--Select--", "0"));



                DataTable ObjDTBranchOutPut = new DataTable();
                ObjDTBranchOutPut = (new ClsCommonBAL()).FunGetCommonDataTable(33, Session["BankID"].ToString());
                DDLBranch.DataSource = ObjDTBranchOutPut;
                DDLBranch.DataTextField = "BranchCode";
                DDLBranch.DataValueField = "BranchID";
                DDLBranch.DataBind();
                DDLBranch.Items.Insert(0, new ListItem("--Select--", "0"));


                DataTable ObjDTUserSystem = new DataTable();
                ObjDTUserSystem = (new ClsCommonBAL()).FunGetCommonDataTable(20, Session["LoginID"].ToString()+","+"1");
                //DdlSystem.DataSource = ObjDTSystem;
                //DdlSystem.DataTextField = "SystemName";
                //DdlSystem.DataValueField = "SystemID";
                //DdlSystem.DataBind();
                //DdlSystem.Items.Insert(0, new ListItem("--Select--", "0"));
                ////  ------------- User wise Systems   
                LoginUserSystems.Value = ObjDTUserSystem.Rows[0]["SystemID"].ToString();

                //Bank Wise System List
                DataTable ObjDTSystem = new DataTable();
                ObjDTSystem = (new ClsCommonBAL()).FunGetCommonDataTable(20, string.Empty+","+Session["BankID"]);
                SystemList.DataSource = ObjDTSystem;
                SystemList.DataTextField = "SystemName";
                SystemList.DataValueField = "SystemID";
                SystemList.DataBind();
                string[] arrayList= ObjDTUserSystem.Rows[0]["SystemID"].ToString().Split(',');
                foreach (ListItem li in SystemList.Items)
                {
                    //foreach (string value in arrayList)
                    //{ 
                    //    if (li.Value != value)
                    //        li.Enabled = false;
                    //}
                    if(arrayList.Contains(li.Value))
                    {
                        li.Enabled = true;
                    }
                    else
                        li.Enabled = false;
                    //li.Selected = arrayList.Contains(li.Value);
                }


            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, FunBindDDL()", ex.Message, "");
            }
        }

        public DataTable FunLoadFeetype()
        {
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ObjDTOutPut = (new ClsCommonBAL()).FunGetCommonDataTable(2, "");
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, FunLoadFeetype()", ex.Message, "");
            }
            return ObjDTOutPut;

        }

        protected void ApproveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                dropdown_status.SelectedValue = "1";
                add_Click(this, EventArgs.Empty);
            }
            catch (Exception )
            {

            }

        }
    }
}
