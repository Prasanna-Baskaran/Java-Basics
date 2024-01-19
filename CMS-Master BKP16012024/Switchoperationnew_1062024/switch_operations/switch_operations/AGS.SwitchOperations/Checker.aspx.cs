using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class Checker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {//
                FunGetResult("CardReissue");

                DataTable ObjDTOutPut = new DataTable();
                //ObjDTOutPut = new ClsCardMasterBAL().FunGetProcessType();
                ObjDTOutPut = new ClsCardCheckerDetailsBAL().FunGetProcessType();
                ddlProcessType.DataSource = ObjDTOutPut;
                ddlProcessType.DataTextField = "ProcessType";
                ddlProcessType.DataValueField = "ProcessID";
                ddlProcessType.DataBind();

            }
        }
        protected void FunGetResult(string StrType)
        {
            ClsCardCheckerDetailsBO ObjChecker = new ClsCardCheckerDetailsBO();
            //ObjChecker.CheckerId = Convert.ToString(Session["UserRoleID"]);
            ObjChecker.CheckerId = Convert.ToString(Session["LoginID"]);
            //new
            //DataTable ObjDTOutPut = new ClsCardMasterBAL().FunGetChecker(ObjChecker, StrType);
            DataTable ObjDTOutPut = new ClsCardCheckerDetailsBAL().FunGetChecker(ObjChecker, "CardReissue");
            //hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { index = 0, control = Utilities.Controls.Checkbox, hideColumnName = true, cssClass = "CHECK" } });
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckedIDs = hdnAllSelectedValues.Value;
            ClsCardCheckerDetailsBO ObjChecker = new ClsCardCheckerDetailsBO();
            ObjChecker.IDs = CheckedIDs;
            ObjChecker.CheckerId = Convert.ToString(Session["LoginID"]);
            // bool ss = new ClsCardMasterBAL().FunUpdateChecker(ObjChecker, "CardReissue");
            bool Value = new ClsCardCheckerDetailsBAL().FunUpdateChecker(ObjChecker, "CardReissue");
            if (Value)
            {
                FunGetResult(ddlProcessType.SelectedValue);
                LblMessage.Text = "PRE File will be generated on SFTP.";

                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(
                           Convert.ToString(HttpContext.Current.Session["LoginID"]),
                           Convert.ToString(Session["UserName"]),
                            "Checker.aspx", "PRE File will be generated on SFTP", ""
                           );

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }
            else
            {
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(
                           Convert.ToString(HttpContext.Current.Session["LoginID"]),
                           Convert.ToString(Session["UserName"]),
                            "Checker.aspx", "PRE File request failed", ""
                           );

                LblMessage.Text = "Failed";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        protected void ProcessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunGetResult(ddlProcessType.SelectedValue);
        }
    }
}