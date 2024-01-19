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
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    public partial class CardLimitsISO : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCL"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        //For user those having Save right
                        if (accessPrefix.Contains("S"))
                        {
                            hdnAccessCaption.Value = "S";
                            userBtns.AccessButtons = StrAccessCaption;
                            userBtns.SaveClick += new EventHandler(btnSave_Click);
                            userBtns.CancelClick += new EventHandler(Page_Load);

                        }
                        else
                        {
                            hdnAccessCaption.Value = "";
                        }

                        if (!IsPostBack)
                        {
                            FillApplicationNoDropDown();
                        }
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
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardLimits, Page_Load()", Ex.Message, "");
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();

                if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                {
                    objSearch.BankCustID = txtSearchCustomerID.Value;
                }
                objSearch.CardNo = txtSearchCardNo.Value;

                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();

                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCustCardLimit(objSearch);

                //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Set Card Limit", "Searching for Card", objSearch.CardNo, objSearch.BankCustID, objSearch.CustomerName, "", "", "", "Searching", "1");

                if (ObjDTOutPut.Rows.Count > 0 && ObjDTOutPut.Rows[0]["Code"].ToString() == "0")
                {

                    GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();

                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    ConfigDataObject ObjData = new ConfigDataObject();

                    DataTable _dtCardAPISourceIdDetails = new DataTable();
                    objSearch.APIRequest = "CardDetails";
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();
                    _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);

                    ObjData.IssuerNo = Session["IssuerNo"].ToString();
                    ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                    ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                    ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);

                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { objSearch.CardNo });
                    ObjData.IsCardDetailsSearch = true;
                    _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);
                    if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        txtCustomerID.Value = ObjAPIResponse.CifId;
                        txtCustomerName.Value = ObjAPIResponse.CustomerName;
                        txtMobile.Value = ObjAPIResponse.MobileNo;
                        txtDOB.Value = ObjAPIResponse.DateOfBirth;
                        txtAddress.Text = ObjAPIResponse.Address;
                        txtEmail.Value = ObjAPIResponse.EmailID;

                        txtPurchaseNo.Value = Convert.ToString(ObjAPIResponse.POSLimitCount);
                        txtDailyPurLmt.Value = Convert.ToString(ObjAPIResponse.POSLimit);
                        txtPtPurLmt.Value = Convert.ToString(ObjAPIResponse.PTPOSLimit);

                        txtWithdrawNo.Value = Convert.ToString(ObjAPIResponse.ATMLimitCount);
                        txtDailyWithdrawLmt.Value = Convert.ToString(ObjAPIResponse.ATMLimit);
                        txtPtWithdrawLmt.Value = Convert.ToString(ObjAPIResponse.PTATMLimit);

                        txtPaymentNo.Value = Convert.ToString(ObjAPIResponse.PaymentsCount);
                        txtDailyPayLmt.Value = Convert.ToString(ObjAPIResponse.PaymentsLimit);
                        txtPtPayLmt.Value = Convert.ToString(ObjAPIResponse.PTPaymentsLimit);

                        txtDailyCNPLmt.Value = Convert.ToString(ObjAPIResponse.EComLimit);
                        txtPtCNPLmt.Value = Convert.ToString(ObjAPIResponse.PTEComLimit);

                        txtCardNo.Value = ObjAPIResponse.CardNo;
                        txtExpiryDate.Value = ObjAPIResponse.ExpiryDate;
                        txtCardStatus.Value = ObjAPIResponse.CardStatus;
                        txtCardIssued.Value = ObjAPIResponse.Date_issued;
                        txtCardStatusID.Value = "1";
                        hdnRPANID.Value = "";
                        txtAccountNo.Value = ObjAPIResponse.AccountNo;

                        hdnCustomerID.Value = ObjAPIResponse.CifId;

                        LblResult.InnerHtml = "";
                        hdnFlag.Value = "2";
                        //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Set Card Limit", "Search result", txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Searched", "1");
                    }
                    else
                    {
                        hdnFlag.Value = "";
                        LblResult.InnerHtml = "No record found";
                        //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Set Card Limit", "No search result", txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Searched", "0");
                    }
                }
                else
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Set Card Limit", "No search result", txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Searched", "0");
                    hdnFlag.Value = "";
                    LblResult.InnerHtml = "No record found";
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardLimits, btnSearch_Click()", Ex.Message, "");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ObjReturnStatus.Description = "Card limit is not saved";
                CustSearchFilter ObjFilter = new CustSearchFilter();
                ObjFilter.IntPara = 1;
                ObjFilter.CustomerID = hdnCustomerID.Value;

                ObjFilter.BankCustID = hdnCustomerID.Value;

                ObjFilter.MakerID = Convert.ToInt64(Session["LoginID"]);

                if ((!string.IsNullOrEmpty(txtPurchaseNo.Value)) && (!string.IsNullOrEmpty(txtDailyPurLmt.Value)) && (!string.IsNullOrEmpty(txtPtPurLmt.Value)) &&
                    (!string.IsNullOrEmpty(txtWithdrawNo.Value)) && (!string.IsNullOrEmpty(txtDailyWithdrawLmt.Value)) && (!string.IsNullOrEmpty(txtPtWithdrawLmt.Value))
                   && (!string.IsNullOrEmpty(txtPaymentNo.Value)) && (!string.IsNullOrEmpty(txtDailyPayLmt.Value)) && (!string.IsNullOrEmpty(txtPtPayLmt.Value))
                   && (!string.IsNullOrEmpty(txtDailyCNPLmt.Value)) && (!string.IsNullOrEmpty(txtPtCNPLmt.Value))
                   && (!string.IsNullOrEmpty(txtCardNo.Value))
                   && (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    )
                {
                    ObjFilter.PurNOTran = Convert.ToInt32(txtPurchaseNo.Value);
                    ObjFilter.PurDailyLimit = Convert.ToDecimal(txtDailyPurLmt.Value);
                    ObjFilter.PurPTLimit = Convert.ToDecimal(txtPtPurLmt.Value);
                    ObjFilter.WithDrawNOTran = Convert.ToInt32(txtWithdrawNo.Value);
                    ObjFilter.WithDrawDailyLimit = Convert.ToDecimal(txtDailyWithdrawLmt.Value);
                    ObjFilter.WithDrawPTLimit = Convert.ToDecimal(txtPtWithdrawLmt.Value);
                    ObjFilter.PaymentNOTran = Convert.ToInt32(txtPaymentNo.Value);
                    ObjFilter.PaymentDailyLimit = Convert.ToDecimal(txtDailyPayLmt.Value);
                    ObjFilter.PaymentPTLimit = Convert.ToDecimal(txtPtPayLmt.Value);
                    ObjFilter.CNPDailyLimit = Convert.ToDecimal(txtDailyCNPLmt.Value);
                    ObjFilter.CNPPTLimit = Convert.ToDecimal(txtPtCNPLmt.Value);
                    ObjFilter.CardNo = txtCardNo.Value;
                    ObjFilter.Remark = txtUpdateRemark.Text;
                    ObjFilter.SystemID = Session["SystemID"].ToString();
                    ObjFilter.BankID = Session["BankID"].ToString();
                    ObjReturnStatus = new ClsCardMasterBAL().FunSaveCardLimit(ObjFilter);

                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Set Card Limit", "Card limit set | Remark: "+ txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Searched", "1");
                }
                else
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Set Card Limit", "Card data is not proper | Remark: " + txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Searched", "0");
                    ObjReturnStatus.Description = "Data is not proper";
                }


                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardLimits, btnSave_Click()", ObjEx.Message, "");
            }
        }

        protected void FillApplicationNoDropDown()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(8, "1");
                ddlApplicationNo.DataSource = ObjDTOutPut;
                ddlApplicationNo.DataTextField = "ApplicationFormNo";
                ddlApplicationNo.DataValueField = "CustomerID";
                ddlApplicationNo.DataBind();
                ddlApplicationNo.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardLimits, FillApplicationNoDropDown()", Ex.Message, "");
            }
        }
    }
}