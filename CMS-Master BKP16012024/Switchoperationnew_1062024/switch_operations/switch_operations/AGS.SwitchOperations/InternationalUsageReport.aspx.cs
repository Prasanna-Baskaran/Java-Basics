using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;

namespace AGS.SwitchOperations
{

    public partial class InternationalUsageReport : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;

        DateTime FromDT;
        DateTime ToDT;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "IUREPO"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
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
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUREPO, InternationalUsageReport, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();

                if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                    objSearch.FromDate = Convert.ToDateTime(txtSearchFromDate.Value);

                if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                    objSearch.ToDate = Convert.ToDateTime(txtSearchToDate.Value+" 23:59:59");

                new ClsCommonBAL().FunInsertIntoErrorLog("IUREPO, InternationalUsageReport, btnSearch_Click()" + objSearch.FromDate.ToString() + " ," + objSearch.ToDate.ToString(), "", "");
                //objSearch.FromDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                objSearch.IssuerNo = Session["IssuerNo"].ToString();
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                ObjDTOutPut = new ClsInternationalUsageBAL().FunGetAllReports(objSearch);

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
                //AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "TransactionReport.aspx", "Search button clicked", "");
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUREPO, InternationalUsageReport, btnSearch_Click()" + txtSearchFromDate.Value + " ," + txtSearchToDate.Value, ObjEx.Message, "");
            }
        }
    }
}