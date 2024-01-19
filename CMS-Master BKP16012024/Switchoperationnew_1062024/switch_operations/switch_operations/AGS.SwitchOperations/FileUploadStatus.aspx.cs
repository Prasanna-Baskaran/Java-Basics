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
    public partial class FileUploadStatus : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "BF";
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
                        
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
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, FileUploadStatus, Page_Load()", ObjEx.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();

                if(!string.IsNullOrEmpty(txtSearchUploadDate.Value))
                { 
                objSearch.CreatedDate = Convert.ToDateTime(txtSearchUploadDate.Value);
                }
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();

                ObjDTOutPut = new ClsMasterBAL().FunGetFileUploadDetails(objSearch);
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                if (ObjDTOutPut.Rows.Count > 0)
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
    }
}