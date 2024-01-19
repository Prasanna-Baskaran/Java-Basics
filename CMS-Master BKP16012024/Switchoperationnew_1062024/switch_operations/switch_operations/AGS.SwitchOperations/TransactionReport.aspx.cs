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
using AGS.SwitchOperations.Validator;
namespace AGS.SwitchOperations
{
    public partial class TransactionReport : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HdnDateRange.Value = Convert.ToString(ConfigurationManager.AppSettings["HdnDateRange"]);
                if (string.IsNullOrEmpty(HdnDateRange.Value)) HdnDateRange.Value = "1";
                string OptionNeumonic = "RT"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];
                ISEPS.Value = Convert.ToString(Session["IsEPS"]); //added by uddesh ATPCM-656
                pnlnameoncard.Visible = Convert.ToString(Session["IssuerNo"]) == "3";
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
            catch (Exception)
            {
                // new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;

                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    //new ValidatorAttr { Name="Card No", Value= txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric = true, Isrequired=!pnlnameoncard.Visible, RequiredGroup="G1"},
                    //new ValidatorAttr { Name="From Date", Value= txtSearchFromDate.Value, MinLength = 10, MaxLength = 10, Isrequired=true},
                    //new ValidatorAttr { Name="To Date", Value= txtSearchToDate.Value, MinLength = 10, MaxLength = 10, Isrequired=true}
                    new ValidatorAttr { Name="From Date", Value= txtdates.Value.Split('-')[0].Trim(), MinLength = 10, MaxLength = 10, Isrequired=true},
                    new ValidatorAttr { Name="To Date", Value= txtdates.Value.Split('-')[1].Trim(), MinLength = 10, MaxLength = 10, Isrequired=true}
                };
                if (pnlnameoncard.Visible)
                    ListValid.Add(new ValidatorAttr { Name = "Name On Card", Value = txtSearchNameOnCard.Value, MinLength = 3, MaxLength = 20, RequiredGroup = "G1" });

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {
                    DataTable ObjDTOutPut = new DataTable();
                    int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                    CustSearchFilter objSearch = new CustSearchFilter();
                    //objSearch.MobileNo = txtSearchMobile.Value;

                    if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                    {
                        objSearch.BankCustID = txtSearchCustomerID.Value.Trim();
                    }
                    //if (!string.IsNullOrEmpty(txtSearchCardNo.Value))
                    //{
                    //    objSearch.CardNo = txtSearchCardNo.Value;
                    //}
                    if (!string.IsNullOrEmpty(txtSearchNameOnCard.Value))
                    {
                        objSearch.NameOnCard = txtSearchNameOnCard.Value;
                    }

                    //added by uddesh ATPCM-656 start
                    if (!string.IsNullOrEmpty(txtRRN.Value))
                    {
                        objSearch.RRN = txtRRN.Value;
                    }
                    //added by uddesh ATPCM-656 end

                    //if (!string.IsNullOrEmpty(txtSearchFromDate.Value))
                    //    objSearch.FromDate = DateTime.ParseExact(txtSearchFromDate.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ////DateTime date = DateTime.MinValue;
                    ////FromDT = DateTime.ParseExact(txtSearchFromDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //if (!string.IsNullOrEmpty(txtSearchToDate.Value))
                    //    objSearch.ToDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    CultureInfo provider = CultureInfo.InvariantCulture;

                    if (!string.IsNullOrEmpty(txtdates.Value.Split('-')[0].Trim()))
                    { objSearch.FromDate = DateTime.ParseExact(txtdates.Value.Split('-')[0].Trim(), "dd/MM/yyyy", provider); }

                    // It throws Argument null exception  
                    if (!string.IsNullOrEmpty(txtdates.Value.Split('-')[1].Trim()))
                        objSearch.ToDate = DateTime.ParseExact(txtdates.Value.Split('-')[1].Trim(), "dd/MM/yyyy", provider);

                    new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, btnSearch_Click() DT" + objSearch.FromDate.ToString() + " ," + objSearch.ToDate.ToString(), "", "");
                    //objSearch.FromDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    objSearch.IntPara = 1;
                    objSearch.SystemID = Convert.ToString(Session["SystemID"]);
                    objSearch.BankID = Convert.ToString(Session["BankID"]);
                    objSearch.IsEPS = Convert.ToInt32(Session["IsEPS"]);
                    objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                    switch (objSearch.IsEPS)
                    {
                        case 0:
                            ObjDTOutPut = new ClsCardMasterBAL().FunGetAllReports(objSearch);
                            break;
                        case 1:
                            ObjDTOutPut = new ClsCardMasterBAL().FunGetAllReportsEPS(objSearch);
                            break;
                    }
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
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, TransactionReport, btnSearch_Click()" + txtdates.Value, ObjEx.Message, "");
            }
        }


    }
}