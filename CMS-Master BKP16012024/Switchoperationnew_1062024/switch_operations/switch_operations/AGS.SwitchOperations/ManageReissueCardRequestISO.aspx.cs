using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class ManageReissueCardRequestISO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable ObjDTOutPut = new DataTable();

                ObjDTOutPut = new ClsManageCardBAL().FunGetNewBINPrefix(Convert.ToString(Session["BankID"]));
                ddlNewBinPrefix.DataSource = ObjDTOutPut;
                ddlNewBinPrefix.DataTextField = "CardPrefix";
                ddlNewBinPrefix.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //start sheetal
            try
            {
                ClsCardCheckerDetailsBO ObjCardcheck = new ClsCardCheckerDetailsBO();
                LblMessage.Text = "";
                LblCardRPANMessage.Text = "";
                ObjCardcheck.CheckerId = Convert.ToString(Session["LoginID"]);
                //2-12-17 bankid and systemid passes to sp
                ObjCardcheck.BankID = Convert.ToString(Session["BankID"]);
                ObjCardcheck.SystemID = Convert.ToString(Session["SystemID"]);
                ObjCardcheck.CustomerID = txtCustomerID.Value;
                ObjCardcheck.CardNo = txtCardNo.Value;
                ObjCardcheck.NewBinPrefix = ddlNewBinPrefix.SelectedValue;
                ObjCardcheck.HoldRSPCode = ddlHoldResponseCode.SelectedValue;

                ObjCardcheck.IssuerNo = Convert.ToString(Session["IssuerNo"]);

                if (ObjCardcheck.HoldRSPCode == "41" || ObjCardcheck.HoldRSPCode == "43")
                {
                    //function to validate CardRPAN is already exist or not
                    string Status = new ClsManageCardBAL().FunValidateReissueCardRequest(ObjCardcheck);

                    if (!string.IsNullOrEmpty(Status))
                    {
                        LblCardRPANMessage.Text = Status;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }
                    else
                    {
                        LblMessage.Text = "Unexpected error occured.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }
                }
                else
                {
                    LblCardRPANMessage.Text = "Please Select  correct Hold Response Code";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "ManageCardReissue.aspx", "Save button clicked", "");
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS,ManageCardReissue , btnSave_Click()", Ex.Message, "");
            }
        }
    }
}