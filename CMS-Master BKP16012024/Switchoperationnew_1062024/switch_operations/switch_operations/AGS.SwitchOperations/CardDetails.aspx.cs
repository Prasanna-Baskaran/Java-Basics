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
    public partial class CardDetails : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCD"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {



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
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardDetails, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();
                //objSearch.MobileNo = txtSearchMobile.Value;
                //accountNo

                //if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                //{
                //    objSearch.AccountNo = txtSearchCustomerID.Value.Trim();
                //}

                objSearch.AccountNo = "";
                objSearch.CardNo = txtSearchCardNo.Value;
                objSearch.CustomerName = txtSearchName.Value;
                objSearch.IntPara = 3;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                //if(!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                //objSearch.CustomerID = Convert.ToInt64(txtSearchCustomerID.Value);
                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtl(objSearch);

                if (ObjDTOutPut.Columns.Contains("CustomerId"))
                {
                    ObjDTOutPut.Columns.Remove("CustomerId");
                }
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CardDetails.aspx", "Search button clicked", "");
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, btnSearch_Click, Page_Load()", ObjEx.Message, "");
            }
        }
    }
}