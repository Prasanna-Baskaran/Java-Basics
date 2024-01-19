using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;

namespace AGS.SwitchOperations
{

    public partial class PinOperationsReports : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HdnDateRange.Value = Convert.ToString(ConfigurationManager.AppSettings["HdnDateRange"]);
                if (string.IsNullOrEmpty(HdnDateRange.Value)) HdnDateRange.Value = "1";
                string OptionNeumonic = "PRO"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];
                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
                            //FunBindDDL();
                            //FillDropDown();                            
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

                if (objSearch.FromDate > objSearch.ToDate)
                {
                    LblResult.InnerHtml = "Invalid From and To date";
                    return;
                }
                objSearch.IntPara = 1;
                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                ObjDTOutPut = new ClsCardMasterBAL().GetPinOpertionsReports(objSearch);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, PinOperationsReports, btnSearch_Click()" + txtSearchFromDate.Value + " ," + txtSearchToDate.Value, ObjEx.Message, "");
            }
        }


    }

}