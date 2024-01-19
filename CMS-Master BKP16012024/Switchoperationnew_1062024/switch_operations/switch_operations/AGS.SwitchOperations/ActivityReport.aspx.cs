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
    public partial class ActivityReport : System.Web.UI.Page
    {

        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HdnDateRange.Value = Convert.ToString(ConfigurationManager.AppSettings["HdnDateRange"]);
                if (string.IsNullOrEmpty(HdnDateRange.Value)) HdnDateRange.Value = "1";
                string OptionNeumonic = "AUDR"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];
                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
                            FunBindDDL();
                            // FillDropDown();
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


                //if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                //    objSearch.FromDate = DateTime.ParseExact(txtSearchFromDate.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                ////DateTime date = DateTime.MinValue;
                ////FromDT = DateTime.ParseExact(txtSearchFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                //    objSearch.ToDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //CultureInfo provider = CultureInfo.InvariantCulture;

                //if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                //{ objSearch.FromDate = DateTime.ParseExact(txtSearchFromDate.Value, "dd/MM/yyyy", provider); }

                //// It throws Argument null exception  
                //if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                //    objSearch.ToDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", provider);

                //if (objSearch.FromDate > objSearch.ToDate)
                //{
                //    LblResult.InnerHtml = "Invalid From and To date";
                //    return;
                //}
                //objSearch.FromDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                objSearch.UserBranchCode = DDLBranch.SelectedItem.Text;
                objSearch.IntPara = 1;
                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                ObjDTOutPut = new ClsCardMasterBAL().FunGetActivityReports(objSearch);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, ActivityReport, btnSearch_Click()", ObjEx.ToString(), "");
            }
        }



        protected void FunBindDDL()
        {
            try
            {

                DataTable ObjDTBranchOutPut = new DataTable();
                ObjDTBranchOutPut = (new ClsCommonBAL()).FunGetCommonDataTable(33, Session["BankID"].ToString());
                DDLBranch.DataSource = ObjDTBranchOutPut;
                DDLBranch.DataTextField = "BranchCode";
                DDLBranch.DataValueField = "BranchID";
                DDLBranch.DataBind();
                //DDLBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                DDLBranch.SelectedItem.Text = Session["BranchCode"].ToString();
                if (Convert.ToBoolean(Session["IsAdmin"]))
                {
                    DDLBranch.Enabled = true;
                }
                else
                {
                    DDLBranch.Enabled = false;

                }

            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, FunBindDDL()", ex.Message, "");
            }
        }
    }
}