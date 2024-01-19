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
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    public partial class CardOperationReport : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "COPR"; //unique optionneumonic from database
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
                new ClsCommonBAL().FunInsertIntoErrorLog("COPR, CardOperationReport, Page_Load()", ObjEx.Message, "");
            }

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();

                if (!string.IsNullOrEmpty(txtCardNo.Value.Replace(" ","")))
                    objSearch.CardNo = txtCardNo.Value.Replace(" ", "");

                new ClsCommonBAL().FunInsertIntoErrorLog("COPR, CardOperationReport, btnSearch_Click() >> card no :-" + GetMaskedCardNo(objSearch.CardNo), "", "");
                //objSearch.FromDate = DateTime.ParseExact(txtSearchToDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                objSearch.IssuerNo = Session["IssuerNo"].ToString(); //"69"; 
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID =Session["BankID"].ToString();// "1052"
                ObjDTOutPut = new ClsInternationalUsageBAL().FunGetCardOpsReports(objSearch);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("COPR, CardOperationReport, btnSearch_Click() >> cardNo > " + GetMaskedCardNo(txtCardNo.Value), ObjEx.Message, "");
            }
        }


        private string GetMaskedCardNo(string cardNumber)
        {
            if (cardNumber.Length != 16)
                return cardNumber;
            // var cardNumber = "3456123434561234";

            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('X', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            //var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            return maskedString;
        }


    }
}