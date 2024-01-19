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
    public partial class ManageCardReissue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable ObjDTOutPut = new DataTable();
                
                ObjDTOutPut = new ClsManageCardBAL().FunGetNewBINPrefix(Convert.ToString(Session["BankID"]));
                ddlNewBinPrefix.DataSource = ObjDTOutPut;
                ddlNewBinPrefix.DataTextField = "CardPrefix";
             //   ddlNewBinPrefix.DataValueField = "BankID";
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
                //if ((!string.IsNullOrEmpty(txtCustomerID.Value)) && (!string.IsNullOrEmpty(txtCardNo.Value)) && (!string.IsNullOrEmpty(txtNewBinPrefix.Value)) &&
                //        (!string.IsNullOrEmpty(txtHoldRSPCode.Value)) && (!string.IsNullOrEmpty(txtRSPCode.Value)) && (!string.IsNullOrEmpty(txtSwitchResponse.Value))
                //       && (!string.IsNullOrEmpty(txtSTAN.Value)) && (!string.IsNullOrEmpty(txtRRN.Value)) && (!string.IsNullOrEmpty(txtAuthID.Value))
                //       && (!string.IsNullOrEmpty(txtRemark.Value)) && (!string.IsNullOrEmpty(txtFormStatusID.Value))
                //       && (!string.IsNullOrEmpty(txtIsRejected.Value)) && (!string.IsNullOrEmpty(txtReject_Reason.Value)) && (!string.IsNullOrEmpty(txtMaker_ID.Value))
                //        && (!string.IsNullOrEmpty(txtChecker_ID.Value)) && (!string.IsNullOrEmpty(txtIsAuthorized.Value))
                //     && (!string.IsNullOrEmpty(txtUploadFileName.Value)) && (!string.IsNullOrEmpty(txtBankID.Value))
                //     && (!string.IsNullOrEmpty(txtSystemID.Value)) && (!string.IsNullOrEmpty(txtProcessID.Value))
                //     && (!string.IsNullOrEmpty(txtschemecode.Value)) && (!string.IsNullOrEmpty(txtAccount1.Value))
                //     && (!string.IsNullOrEmpty(txtAccount2.Value)) && (!string.IsNullOrEmpty(txtAccount3.Value))
                //     && (!string.IsNullOrEmpty(txtAccount4.Value)) && (!string.IsNullOrEmpty(txtAccount5.Value))
                //     && (!string.IsNullOrEmpty(txtBranch_Code.Value)) && (!string.IsNullOrEmpty(txtExpiryDate.Value))
                //    && (!string.IsNullOrEmpty(txtNewCard.Value)) && (!string.IsNullOrEmpty(txtCustomer_Name.Value))
                //    && (!string.IsNullOrEmpty(txtNewCardActivationDate.Value)) && (!string.IsNullOrEmpty(txtReserved1.Value))
                //    && (!string.IsNullOrEmpty(txtReserved2.Value)) && (!string.IsNullOrEmpty(txtReserved3.Value))
                //    && (!string.IsNullOrEmpty(txtReserved4.Value)) && (!string.IsNullOrEmpty(txtReserved5.Value)))

                //CardCheckerDetail ObjChecker = new CardCheckerDetail();
                ObjCardcheck.CheckerId = Convert.ToString(Session["LoginID"]);
                //2-12-17 bankid and systemid passes to sp
                ObjCardcheck.BankID = Convert.ToString(Session["BankID"]);
                ObjCardcheck.SystemID= Convert.ToString(Session["SystemID"]);
                ObjCardcheck.CustomerID = txtCustomerID.Value;
                ObjCardcheck.CardNo = txtCardNo.Value;
                ObjCardcheck.NewBinPrefix = ddlNewBinPrefix.SelectedValue;
                ObjCardcheck.HoldRSPCode = ddlHoldResponseCode.SelectedValue;

                if (ObjCardcheck.HoldRSPCode == "41" || ObjCardcheck.HoldRSPCode == "43")
                {
                    //function to validate CardRPAN is already exist or not
                    string CardNoExist = new ClsManageCardBAL().FunOldCardRPANIDExist(ObjCardcheck, "CardReissue");



                    if (CardNoExist.Split('|')[1] == "Invalid")
                    {
                        LblCardRPANMessage.Text = CardNoExist.Split('|')[0];
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }
                    else
                    {
                        bool ObjReturnStatus = new ClsManageCardBAL().FunSaveCard(ObjCardcheck, "CardReissue");
                        if (ObjReturnStatus)
                        {
                            LblCardRPANMessage.Text = "";
                            LblMessage.Text = "Success";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                        }
                        else
                        {
                            LblMessage.Text = "Failed";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        }
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
