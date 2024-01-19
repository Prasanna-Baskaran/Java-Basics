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

namespace AGS.SwitchOperations
{
    public partial class CardLimits : System.Web.UI.Page
    {
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

                //hdnLoginRoleID.Value = Session["UserRoleID"].ToString();
            }
            catch (Exception )
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
                //objSearch.MobileNo = txtSearchMobile.Value;
                //if (ddlApplicationNo.SelectedValue != "0")
                //{
                //    objSearch.CustomerID = Convert.ToInt16(ddlApplicationNo.SelectedValue);
                //}
                if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                {
                    objSearch.BankCustID = txtSearchCustomerID.Value;
                }
                objSearch.CardNo = txtSearchCardNo.Value;

                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCustCardLimit(objSearch);

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    txtCustomerID.Value = ObjDTOutPut.Rows[0][0].ToString();
                    txtCustomerName.Value = ObjDTOutPut.Rows[0][1].ToString();
                    txtMobile.Value = ObjDTOutPut.Rows[0][2].ToString();
                    txtDOB.Value = ObjDTOutPut.Rows[0][3].ToString();
                    txtAddress.Text = ObjDTOutPut.Rows[0][4].ToString();
                    txtEmail.Value = ObjDTOutPut.Rows[0][5].ToString();
                    txtPurchaseNo.Value = ObjDTOutPut.Rows[0][6].ToString();
                    txtDailyPurLmt.Value = ObjDTOutPut.Rows[0][7].ToString();
                    txtPtPurLmt.Value = ObjDTOutPut.Rows[0][8].ToString();
                    txtWithdrawNo.Value = ObjDTOutPut.Rows[0][9].ToString();
                    txtDailyWithdrawLmt.Value = ObjDTOutPut.Rows[0][10].ToString();
                    txtPtWithdrawLmt.Value = ObjDTOutPut.Rows[0][11].ToString();
                    txtPaymentNo.Value = ObjDTOutPut.Rows[0][12].ToString();
                    txtDailyPayLmt.Value = ObjDTOutPut.Rows[0][13].ToString();
                    txtPtPayLmt.Value = ObjDTOutPut.Rows[0][14].ToString();
                    txtDailyCNPLmt.Value = ObjDTOutPut.Rows[0][15].ToString();
                    txtPtCNPLmt.Value = ObjDTOutPut.Rows[0][16].ToString();
                    txtCardNo.Value = ObjDTOutPut.Rows[0][17].ToString();
                    txtExpiryDate.Value = ObjDTOutPut.Rows[0][18].ToString();
                    txtCardStatus.Value = ObjDTOutPut.Rows[0][19].ToString();
                    txtCardIssued.Value = ObjDTOutPut.Rows[0][20].ToString();
                    //Start Diksha
                    txtCardStatusID.Value = ObjDTOutPut.Rows[0][22].ToString();
                    //start 27/07 for customer and account logic change
                    hdnRPANID.Value= ObjDTOutPut.Rows[0][23].ToString();
                    txtAccountNo.Value= ObjDTOutPut.Rows[0][24].ToString();
                   
                    hdnCustomerID.Value= ObjDTOutPut.Rows[0][25].ToString();

                    //hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
                    LblResult.InnerHtml = "";
                    hdnFlag.Value = "2";
                }
                else
                {
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
                //start 27/07
                ObjFilter.CustomerID = hdnCustomerID.Value;
              //  ObjFilter.CustomerID = Convert.ToInt64(txtCustomerID.Value);

                ObjFilter.MakerID = Convert.ToInt64(Session["LoginID"]);
                //if (!string.IsNullOrEmpty(txtLimit.Text))
                //{
                //    ObjFilter.Limit = Convert.ToDecimal(txtLimit.Text);
                //}
                if ((!string.IsNullOrEmpty(txtPurchaseNo.Value)) && (!string.IsNullOrEmpty(txtDailyPurLmt.Value)) && (!string.IsNullOrEmpty(txtPtPurLmt.Value)) &&
                    (!string.IsNullOrEmpty(txtWithdrawNo.Value)) && (!string.IsNullOrEmpty(txtDailyWithdrawLmt.Value)) && (!string.IsNullOrEmpty(txtPtWithdrawLmt.Value))
                   && (!string.IsNullOrEmpty(txtPaymentNo.Value)) && (!string.IsNullOrEmpty(txtDailyPayLmt.Value)) && (!string.IsNullOrEmpty(txtPtPayLmt.Value))
                   && (!string.IsNullOrEmpty(txtDailyCNPLmt.Value)) && (!string.IsNullOrEmpty(txtPtCNPLmt.Value))
                   && (!string.IsNullOrEmpty(txtCardNo.Value))
                   && (!string.IsNullOrEmpty(ObjFilter.CustomerID))
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

                }
                else
                {
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
        //protected void btnAccept_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
        //        CustSearchFilter ObjCustomer = new CustSearchFilter();
        //        ObjCustomer.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
        //        ObjCustomer.CheckerId = Convert.ToInt64(Session["LoginID"]);
        //        ObjCustomer.FormStatusID = 1;
        //        ObjReturnStatus = ClsCardMasterDAL.FunAccept_RejectCardLimit(ObjCustomer);
        //        LblMsg.InnerText = ObjReturnStatus.Description.ToString();

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ClsCommonDAL.FunInsertIntoErrorLog("CS, CardLimits, btnAccept_Click()", Ex.Message, "");
        //    }

        //}

        //protected void btnReject_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
        //        CustSearchFilter ObjCustomer = new CustSearchFilter();
        //        ObjCustomer.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
        //        ObjCustomer.CheckerId = Convert.ToInt64(Session["LoginID"]);
        //        ObjCustomer.FormStatusID = 2;
        //        ObjCustomer.Remark = txtRejectReson.Text;
        //        ObjReturnStatus = ClsCardMasterDAL.FunAccept_RejectCardLimit(ObjCustomer);
        //        LblMsg.InnerText = ObjReturnStatus.Description.ToString();

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
        //    }
        //    catch (Exception Ex)
        //    {
        //        ClsCommonDAL.FunInsertIntoErrorLog("CS, CardLimits, btnReject_Click()", Ex.Message, "");
        //    }
        //}
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

                //Modal Application no dropDown fill
                //ddlAppNoModal.DataSource = ObjDTOutPut;
                //ddlAppNoModal.DataTextField = "ApplicationFormNo";
                //ddlAppNoModal.DataValueField = "CustomerID";
                //ddlAppNoModal.DataBind();
                //ddlAppNoModal.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardLimits, FillApplicationNoDropDown()", Ex.Message, "");
            }
        }
    }
}