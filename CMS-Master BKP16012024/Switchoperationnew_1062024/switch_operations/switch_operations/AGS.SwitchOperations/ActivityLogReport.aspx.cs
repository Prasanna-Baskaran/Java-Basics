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
using System.Globalization;
using System.Configuration;

namespace AGS.SwitchOperations
{
    public partial class ActivityLogReport : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HdnDateRange.Value = Convert.ToString(ConfigurationManager.AppSettings["HdnDateRange"]);
                if (string.IsNullOrEmpty(HdnDateRange.Value)) HdnDateRange.Value = "1";
                string OptionNeumonic = "ALR"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];
                ISEPS.Value = Convert.ToString(Session["IsEPS"]); //added by uddesh ATPCM-656
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
            catch (Exception)
            {
                // new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();
                
                CultureInfo provider = CultureInfo.InvariantCulture;

                if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                { objSearch.FromDate = DateTime.ParseExact(txtSearchFromDate.Value, "dd/MM/yyyy", provider); }

                // It throws Argument null exception  
                if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                    objSearch.ToDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", provider);

                new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, btnSearch_Click() DT" + objSearch.FromDate.ToString() + " ," + objSearch.ToDate.ToString(), "", "");

                objSearch.IntPara = 2;
                objSearch.SystemID = Convert.ToString(Session["SystemID"]);
                objSearch.BankID = Convert.ToString(Session["BankID"]);
                objSearch.IsEPS = Convert.ToInt32(Session["IsEPS"]);
                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                objSearch.BankCustID = ddlUserID.SelectedValue;
                objSearch.ProductType = ddlReportType.SelectedValue;

                ObjDTOutPut = new ClsCardMasterBAL().FunGetAllReports(objSearch);

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "TransactionReport.aspx", "Search button clicked", "");
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, btnSearch_Click()" + txtSearchFromDate.Value + " ," + txtSearchToDate.Value, ObjEx.Message, "");
            }
        }

        public void FillDropDown()
        {
            string BankID = Convert.ToString(Session["BankID"]);
            ddlUserID.DataSource= new ClsCardMasterBAL().FunGetUserIdActivityReports(BankID);
            ddlUserID.DataTextField = "Text";
            ddlUserID.DataValueField = "Value";
            ddlUserID.DataBind();
        }
    }
}