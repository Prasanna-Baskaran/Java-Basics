using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using System.Data;

namespace AGS.SwitchOperations
{
    public partial class PersonalizedCard : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "RC"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
                            FillDropDown();
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
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, PersonalizedCard, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();
                if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                    objSearch.FromDate = Convert.ToDateTime(txtSearchFromDate.Value);
                if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                    objSearch.ToDate = Convert.ToDateTime(txtSearchToDate.Value);

                if (ddlProductType.SelectedValue != "0")
                    objSearch.ProductType = ddlProductType.SelectedValue;
                if (ddlStatus.SelectedValue != "-1")
                    objSearch.Status = ddlStatus.SelectedValue;
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
               
                ObjDTOutPut = new ClsCardMasterBAL().FunGetAllReports(objSearch);
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                if(ObjDTOutPut.Rows.Count>0)
                {
                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, PersonalizedCard, btnSearch_Click()", ObjEx.Message, "");
            }
        }

        public void FillDropDown()
        {
            try
            {
                DataTable ObjDTStatus = new DataTable();
                ObjDTStatus = new ClsCommonBAL().FunGetCommonDataTable(24, "");
                ddlStatus.DataSource = ObjDTStatus;
                ddlStatus.DataTextField = "FormStatus";
                ddlStatus.DataValueField = "FormStatusID";
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("--Select--", "-1"));

                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(25, Session["SystemID"].ToString()+","+Session["BankID"].ToString());
                ddlProductType.DataSource = ObjDTOutPut;
                ddlProductType.DataTextField = "ProductType";
                ddlProductType.DataValueField = "ID";
                ddlProductType.DataBind();
                ddlProductType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, PersonalizedCard, FillDropDown()", Ex.Message, "");
            }

        }
    }
}