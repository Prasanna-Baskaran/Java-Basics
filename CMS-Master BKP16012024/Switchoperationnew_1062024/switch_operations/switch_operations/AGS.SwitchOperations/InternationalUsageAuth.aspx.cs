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
    public partial class InternationalUsageAuth : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                string OptionNeumonic = "IUA"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        
                        if (!IsPostBack)
                        {
                            LblResult.InnerHtml = "";
                            FunGetResult("LOAD");
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
                new ClsCommonBAL().FunInsertIntoErrorLog("IUA, InternationalUsageAuth, Page_Load()", ObjEx.Message, "");
                Response.Redirect("ErrorPage.aspx", false);
            }

         
        }
        protected void FunGetResult(String fname)
        {
            ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();

            objSearch.UserID = Session["UserName"].ToString();
            //objSearch.SystemID = Session["SystemID"].ToString();
            objSearch.IssuerNo = Session["IssuerNo"].ToString();
            DataTable ObjDTOutPut = new ClsInternationalUsageBAL().FunGetIntNaUsageRequestData(objSearch);
            if (ObjDTOutPut.Rows.Count > 0)
            {
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { index = 0, control = Utilities.Controls.Checkbox, hideColumnName = true, cssClass = "CHECK" } });
                BtnReject.Visible = true;
                BtnSave.Visible = true;
                select_all.Visible = true;
                LBLselect_all.Visible = true;
            }
            else
            {
                if (fname == "LOAD")
                {
                    LblResult.InnerHtml = "<span style='color:red'>No request for International usage change</span>";
               
                }
                BtnReject.Visible = false;
                BtnSave.Visible = false;
                select_all.Visible = false;
                LBLselect_all.Visible = false;
            }

        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.InnerHtml = "";
                LblSuccess.InnerHtml = "";
                LblFail.InnerHtml = "";

                ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();
                objSearch.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.UserID = Session["UserName"].ToString();
                String sSuccess = "Success Card Nos: ";
                String sFail = "Fail Card Nos: ";
                foreach (string sr in objSearch.RequestIDs)
                {

                    GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    ConfigDataObject ObjData = new ConfigDataObject();
                    ObjData.IssuerNo = Session["IssuerNo"].ToString();
                    ObjData.APIRequestType = ConfigurationManager.AppSettings["API_Request_Type"].ToString();
                    ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                    ObjData.SourceID = ConfigurationManager.AppSettings["RBLSourceId"].ToString();
                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("BlockType", typeof(string));
                    _dtRequest.Columns.Add("Status", typeof(string));
                    _dtRequest.Columns.Add("InternationalCard", typeof(string));

                    _dtRequest.Rows.Add(new Object[] { sr.Split('|')[1], sr.Split('|')[2]=="0" ?"1":"0", sr.Split('|')[2] == "0" ? "0": "1", "1" });


                    _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);
                    objSearch.id = Convert.ToInt32(sr.Split('|')[0]);
                    objSearch.ISORSPCode = ObjAPIResponse.Status;
                    objSearch.ISORSPDesc = ObjAPIResponse.StatusDesc;
                    DataTable ObjDTOutPut = new ClsInternationalUsageBAL().FunUpdateIntNaUsageResponse(objSearch);
                    if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        sSuccess = sSuccess + GetMaskedCardNo(sr.Split('|')[1]) + " ,";
                    }
                    else
                    {
                        sFail = sFail + GetMaskedCardNo(sr.Split('|')[1]) + " ,";
                    }

                }
                if (sSuccess != "Success Card Nos: ")
                { LblSuccess.InnerHtml ="<span style='color:green'>" + sSuccess.TrimEnd(',') + "</span>"; }
                if (sFail != "Fail Card Nos: ")
                { LblFail.InnerHtml = "<span style='color:red'>" + sFail.TrimEnd(',') + "</span>"; }
                FunGetResult("APPROVE");
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUA, InternationalUsageAuth, BtnSave_Click()", ex.Message, "");
                Response.Redirect("ErrorPage.aspx", false);

            }
        }
        private string GetMaskedCardNo(string cardNumber)
        {
            try
            {
                // var cardNumber = "3456123434561234";

                var firstDigits = cardNumber.Substring(0, 6);
                var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

                var requiredMask = new String('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

                var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
                //var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
                return maskedString;
            }
            catch
            {
                return cardNumber;

            }
        }
        protected void BtnReject_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.InnerHtml = "";
                LblSuccess.InnerHtml = "";
                LblFail.InnerHtml = "";
                ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();
                objSearch.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.UserID = Session["UserName"].ToString();
                DataTable _dtRequest = new DataTable();
                _dtRequest.Columns.Add("id", typeof(Int32));
                _dtRequest.Columns.Add("Reason", typeof(string));
                foreach (string sr in objSearch.RequestIDs)
                {
                    _dtRequest.Rows.Add(new Object[] { sr.Split('|')[0], "rejected by authoriser" });
                }
                objSearch.DtBulkData = _dtRequest;
                DataTable ObjDTOutPut = new ClsInternationalUsageBAL().FunRejectIntNaUsageResponse(objSearch);
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    if (Convert.ToString(ObjDTOutPut.Rows[0][0]) == "1")
                    {
                        FunGetResult("REJECT");
                        LblResult.InnerHtml = "<span style='color:green'>Rejected successfully!</span>";
                    }
                    else
                    {

                        LblResult.InnerHtml = "<span style='color:red'>" + "Error while rejecting" + "</span>";

                    }

                }
                else
                {
                    LblResult.InnerHtml = "<span style='color:red'>Error while rejecting</span>";
                }

            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUA, InternationalUsageAuth, BtnReject_Click()", ex.Message, "");
                Response.Redirect("ErrorPage.aspx", false);

            }
        }
    }
}