using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AGS.SwitchOperations
{
    public partial class UserRole : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "MUR"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;

                        if (!IsPostBack)
                        {
                            FunGetResult();
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
            catch (Exception )
            {
               // new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserRole, Page_Load()", ex.Message, "");
            }
        }

        protected void add_Click(object sender, EventArgs e)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            DataTable ObjDTOutPut = new DataTable();
            ClsUserRoleBO ObjPriFeeBO = new ClsUserRoleBO() { UserRole = txtUserRole.Value };
            try
            { 
                string[] RequestIDs;
                RequestIDs = 
                    (hdnAllSelectedValues.Value).Split(',');
                if (!string.IsNullOrEmpty(txt_userid.Value))
                {
                    try
                    {
                        ObjReturnStatus = (new ClsUserRoleBAL()).FunUpdateIntoUserRoleMaster(Convert.ToString(txtUserRole.Value), Convert.ToInt32(txt_userid.Value), Convert.ToInt16(Session["LoginID"]),Session["SystemID"].ToString(),Session["BankID"].ToString(), hdnAllSelectedValues.Value);
                        LblMsg.InnerText = ObjReturnStatus.Description;
                        hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                    }
                    catch (Exception ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserRole, add_Click(for update)", ex.Message, "");
                    }
                    finally
                    {
                        //       hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(13, string.Empty);
                        FunGetResult();

                        txtUserRole.Value = string.Empty;
                        txt_userid.Value = string.Empty;
                            
                    }

                }
                else
                {
                    try
                    {
                        ObjReturnStatus = (new ClsUserRoleBAL()).FunInsertIntoUserRoleMaster(ObjPriFeeBO.UserRole, Convert.ToInt16(Session["LoginID"]), Session["SystemID"].ToString(), Session["BankID"].ToString(), hdnAllSelectedValues.Value);
                        LblMsg.InnerText = ObjReturnStatus.Description;
                        hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }

                    catch (Exception ex)
                    {

                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserRole, add_Click(for insert)", ex.Message, "");

                    }

                    finally
                    {
                        //hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(13, string.Empty);
                        FunGetResult();
                    }
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "UserRole.aspx", "Add button clicked", "");
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserRole, add_click_btn()", ex.Message, "");
            }

        }

        public void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(13, Session["BankID"]+","+Session["SystemID"]);
           
          
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
                                          { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "UserId" }

                        , new AGS.Utilities.Attribute() { AttributeName = "userrole", BindTableColumnValueWithAttribute = "User Role" }
                         , new AGS.Utilities.Attribute() { AttributeName = "isdefault", BindTableColumnValueWithAttribute = "IsDefault" }

                                 }
                    };

                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("UserId,IsDefault", objAdded);
                }
                else
                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("UserId,IsDefault");
            }

            DataTable ObjDTOptions = new DataTable();
            ObjDTOptions = new ClsCommonBAL().FunGetCommonDataTable(30, "");

            AddedTableData[] objAdded2 = new AddedTableData[1];
            objAdded2[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID"} } };
            hdnMenudetails.Value = ObjDTOptions.ToHtmlTableString("[OptionName],ID", objAdded2);



        }

        [System.Web.Services.WebMethod]
        public static string FunGetUserRoleDetails(String roleid)
        {
            
            DataTable ObjDTOptions = new DataTable();
            ObjDTOptions = new ClsCommonBAL().FunGetCommonDataTable(31, roleid);

            //AddedTableData[] objAdded2 = new AddedTableData[1];
            //objAdded2[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
            //hdnMenudetails.Value = ObjDTOptions.ToHtmlTableString("[OptionName],ID", objAdded2);
            if (ObjDTOptions.Rows.Count > 0)
            {
                //var JMsg = JObject.Parse(ObjDTOptions.Rows[0][0].ToString());
                return ObjDTOptions.Rows[0][0].ToString();// JMsg;
            }
            else {

               // var JMsg = JObject.Parse("");
                return "";// JMsg;
            }
        }



        //protected void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    try
        //    { 


        //    DataTable ObjDTOptions = new DataTable();
        //    ObjDTOptions = new ClsCommonBAL().FunGetCommonDataTable(30, "");

        //    AddedTableData[] objAdded2 = new AddedTableData[1];
        //    objAdded2[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
        //    hdnMenudetails.Value = ObjDTOptions.ToHtmlTableString("[ID],[URL]", objAdded2);
        //    }
        //    catch (Exception ex)
        //    {
        //        new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserRole, btnAddNew_Click()", ex.Message, "");
        //    }
        //}


    }
}
