using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using System.Data;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class ResetPassward : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClsUserBO objuser = new ClsUserBO();
                objuser.UserName = txtSearchUserName.Value.Trim();
                DataTable ObjDtOutput = new DataTable();
                ObjDtOutput = new ClsUserDetailsBAL().FunGetUserDetails(objuser);
                if (ObjDtOutput.Rows.Count > 0)
                {
                    AddedTableData[] objAdded = new AddedTableData[1];
                    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "Reset Passward", cssClass = " btn-link", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "FunReset($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "userid", BindTableColumnValueWithAttribute = "UserID" } } };
                    hdnTransactionDetails.Value = ObjDtOutput.ToHtmlTableString("", objAdded);
                }
                else
                {
                    hdnTransactionDetails.Value = "";
                }
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ResetPassward, btnSearch_Click()", Ex.Message, "");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsUserBO ObjUser = new ClsUserBO();
                if (!string.IsNullOrEmpty(hdnUserID.Value))
                {
                    ObjUser.UserID = Convert.ToInt64(hdnUserID.Value);
                }
                ObjUser.IntPara = 0;
                ObjReturnStatus = new ClsUserDetailsBAL().FunChangePassward(ObjUser);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ResetPassward, btnReset_Click()", Ex.Message, "");
            }
        }
    }
}