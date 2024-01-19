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
    public partial class CardFeeConfigure : System.Web.UI.Page
    {
         string StrAccessCaption = string.Empty;
        ClsCardFeeConfigurationBO ObjCrdFeeCnfg = new ClsCardFeeConfigurationBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string OptionNeumonic = "CFC"; //unique optionneumonic from database
                    Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                    ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                    if (ObjDictRights.ContainsKey(OptionNeumonic))
                    {
                        StrAccessCaption = ObjDictRights[OptionNeumonic];
                        if (!string.IsNullOrEmpty(StrAccessCaption))
                        {
                            hdnAccessCaption.Value = StrAccessCaption;

                        }
                        else
                        {
                            Response.Redirect("ErrorPage.aspx", false);
                        }

                    }
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
                    }
                    LblMessage.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    FunGetdropdown();

                    pnlCardFeeMasterSave.Visible = false;

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardFeeConfigure, Page_Load()", ex.Message, "");
            }

        }

        public void FunGetdropdown()
        {
            DataTable ObjDTOutPut = new DataTable();

            ObjDTOutPut = new ClsCardFeeConfigurationBAL().FunGetIssuerNo();
            ddlSearchIssuerNo.DataSource = ObjDTOutPut;
            ddlSearchIssuerNo.DataTextField ="BankName";
            ddlSearchIssuerNo.DataValueField ="bankcode";
            ddlSearchIssuerNo.DataBind();
            ddlSearchIssuerNo.Items.Insert(0, new ListItem("--Select--", "0"));

            //ObjDTOutPut = new ClsCardFeeConfigurationBAL().FunGetFileCategory();
            //ddlFileCatagory.DataSource = ObjDTOutPut;
            //ddlFileCatagory.DataTextField = "FileCategory";
            //ddlFileCatagory.DataValueField = "CategoryValue";
            //ddlFileCatagory.DataBind();
            //ddlFileCatagory.Items.Insert(0, new ListItem("--Select--", "0"));


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            ClearAllValues();
           if (!string.IsNullOrEmpty(ddlSearchIssuerNo.SelectedValue))
            {
                ObjCrdFeeCnfg.IssuerNo = Convert.ToInt32(ddlSearchIssuerNo.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlFileCatagory.SelectedValue))
            {
                ObjCrdFeeCnfg.FileCategory = ddlFileCatagory.SelectedValue;
            }

            string ObjStatusIsExist = new ClsCardFeeConfigurationBAL().FunIsBankExistIncardFeeMaster(ObjCrdFeeCnfg);

            try
            {
                if (ObjStatusIsExist.Split('|')[1] == "Configured")
                {
                    hdnFlag.Value = "0";
                    //LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    pnlCardFeeMasterSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    try
                    {
                        DataTable ObjDTOutPut = new DataTable();

                        ObjDTOutPut = new ClsCardFeeConfigurationBAL().FunGetIssuerDataForcardFee(ObjCrdFeeCnfg);

                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            txtIssuerNo.Value = ObjDTOutPut.Rows[0]["IssuerNo"].ToString();
                            txtSequenceNo.Value = ObjDTOutPut.Rows[0]["Sequence"].ToString();
                            ddlBankstatus.SelectedValue = ObjDTOutPut.Rows[0]["IsActive"].ToString();
                            txtSFTPIncoming.Value = ObjDTOutPut.Rows[0]["SFTPIncomingFilePath"].ToString();
                            txtSFTPOutput.Value = ObjDTOutPut.Rows[0]["SFTPOutputFilePath"].ToString();
                            txtSFTPRejected.Value = ObjDTOutPut.Rows[0]["SFTPRejectedFilePath"].ToString();
                            txtUserName.Value = ObjDTOutPut.Rows[0]["Username"].ToString();
                            txtPassword.Value = ObjDTOutPut.Rows[0]["Password"].ToString();
                            txtServerPort.Value = ObjDTOutPut.Rows[0]["ServerPort"].ToString();
                            txtServerIP.Value = ObjDTOutPut.Rows[0]["serverIP"].ToString();
                            txtDateCriteria.Value = ObjDTOutPut.Rows[0]["DateCriteria"].ToString();
                            txtFileNameFormat.Value = ObjDTOutPut.Rows[0]["FileNameFormat"].ToString();
                            txtSequence.Value = ObjDTOutPut.Rows[0]["Sequence"].ToString();
                            txtStatus.Value = ObjDTOutPut.Rows[0]["Status"].ToString();
                            //txtPassword.Visible = false;
                            txtIssuerNo.Attributes.Add("readonly", "readonly");
                            btnDelete.Visible = true;

                            //txtBank.Attributes.Add("readonly", "readonly");

                            //btnDelete.Enabled = true;
                            //txtBankName.Attributes.Add("readonly", "true");
                            //txtBankName.Disabled = true;



                        }

                    }
                    //pnlBankConfigureSave.Controls.Cl}
                    catch (Exception Ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardFeeConfigure, btnSearch_Click()", Ex.Message, "");
                    }
                }

                if (ObjStatusIsExist.Split('|')[1] == "Not Exist")
                {
                    hdnFlag.Value = "2";
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    pnlCardFeeMasterSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    try
                    {
                        DataTable ObjDTOutPut = new DataTable();

                        ObjDTOutPut = new ClsCardFeeConfigurationBAL().FunGetIssuerDataForcardFee(ObjCrdFeeCnfg);

                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            txtIssuerNo.Value = ObjDTOutPut.Rows[0]["IssuerNo"].ToString();
                            txtSequenceNo.Value = ObjDTOutPut.Rows[0]["Sequence"].ToString();
                            ddlBankstatus.SelectedValue = ObjDTOutPut.Rows[0]["IsActive"].ToString();
                            txtSFTPIncoming.Value = ObjDTOutPut.Rows[0]["SFTPIncomingFilePath"].ToString();
                            txtSFTPOutput.Value = ObjDTOutPut.Rows[0]["SFTPOutputFilePath"].ToString();
                            txtSFTPRejected.Value = ObjDTOutPut.Rows[0]["SFTPRejectedFilePath"].ToString();
                            txtUserName.Value = ObjDTOutPut.Rows[0]["Username"].ToString();
                            txtPassword.Value = ObjDTOutPut.Rows[0]["password"].ToString();
                            txtServerPort.Value = ObjDTOutPut.Rows[0]["ServerPort"].ToString();
                            txtServerIP.Value = ObjDTOutPut.Rows[0]["serverIP"].ToString();
                            txtDateCriteria.Value = "";
                            txtFileNameFormat.Value = "";
                            txtSequence.Value = "";
                            txtStatus.Value = "";
                            //txtPassword.Visible = false;
                            txtIssuerNo.Attributes.Add("readonly", "readonly");
                            btnDelete.Visible = true;

                            //txtBank.Attributes.Add("readonly", "readonly");

                            //btnDelete.Enabled = true;
                            //txtBankName.Attributes.Add("readonly", "true");
                            //txtBankName.Disabled = true;



                        }
                        else
                        {
                            ClearAllValues();
                        }

                    }
                    catch (Exception Ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardFeeConfigure, btnSearch_Click()", Ex.Message, "");
                    }
                }

                

                if (ObjStatusIsExist.Split('|')[1] == "Not Configured")

                {
                    hdnFlag.Value = "1";
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0].ToString();
                    pnlCardFeeMasterSave.Visible = true;
                    ClearAllValues();
                    txtIssuerNo.Value = ddlSearchIssuerNo.SelectedValue;
                    txtIssuerNo.Attributes.Add("readonly", "readonly");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                }


               
            }

            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardFeeConfigure, btnSearch_Click()", Ex.Message, "");
            }

        }
      public void ClearAllValues()
        {
            txtIssuerNo.Value = "";
            txtSequenceNo.Value = "";
            ddlBankstatus.SelectedIndex = ddlBankstatus.Items.IndexOf(ddlBankstatus.Items.FindByText("--Select--"));
            txtSFTPIncoming.Value = "";
            txtSFTPOutput.Value = "";
            txtSFTPRejected.Value = "";
            txtUserName.Value = "";
            txtPassword.Value = "";
            txtServerIP.Value = "";
            txtServerPort.Value = "";
            txtDateCriteria.Value = "";
            txtFileNameFormat.Value = "";
            txtSequence.Value = "";
            txtStatus.Value = "";
            txtIssuerNo.Attributes.Remove("readonly");
            btnDelete.Visible = false;

            

          }
       protected void btnSave_Click(object sender, EventArgs e)
        {
         LblMessage.Text = "";
           try
            {
                ObjCrdFeeCnfg.IssuerNo = Convert.ToInt32(txtIssuerNo.Value);
                ObjCrdFeeCnfg.BankStatus = ddlBankstatus.SelectedValue;
                ObjCrdFeeCnfg.SequenceNo = Convert.ToInt32(txtSequenceNo.Value);
                ObjCrdFeeCnfg.SFTPIncomingFilePath = txtSFTPIncoming.Value;
                ObjCrdFeeCnfg.SFTPOutputFilePath = txtSFTPOutput.Value;
                ObjCrdFeeCnfg.SFTPRejectedFilePath =txtSFTPRejected.Value;
                ObjCrdFeeCnfg.SFTPUserName = txtUserName.Value;
                ObjCrdFeeCnfg.SFTPPassword =txtPassword.Value;
                ObjCrdFeeCnfg.SFTPServerIP =txtServerIP.Value;
                ObjCrdFeeCnfg.SFTPServerPort= Convert.ToInt32(txtServerPort.Value);
                //
                ObjCrdFeeCnfg.DateCriteria =(txtDateCriteria.Value);
                ObjCrdFeeCnfg.FileNameFormat =(txtFileNameFormat.Value);
                ObjCrdFeeCnfg.SequenceForCatagory = Convert.ToInt32(txtSequence.Value);
                ObjCrdFeeCnfg.Status =(txtStatus.Value);
                ObjCrdFeeCnfg.FileCategory = ddlFileCatagory.SelectedValue;

                if (hdnFlag.Value == "1")//insert
                {

                    string ObjReturnStatus = new ClsCardFeeConfigurationBAL().FunAddEditCardFeeMasterData(ObjCrdFeeCnfg);
                    LblMessage.Text = ObjReturnStatus.ToString();
                    ClearAllValues();
                    pnlCardFeeMasterSave.Visible = false;
                    ddlSearchIssuerNo.SelectedIndex= ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    ddlFileCatagory.SelectedIndex = ddlFileCatagory.Items.IndexOf(ddlFileCatagory.Items.FindByText("--Select--"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }

                
                if (hdnFlag.Value == "0")//update
                { 
                    string ObjStatuExist = new ClsCardFeeConfigurationBAL().FunAddEditCardFeeMasterData(ObjCrdFeeCnfg);
                    LblMessage.Text = ObjStatuExist.Split('|')[0];
                    ClearAllValues();
                   // ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    pnlCardFeeMasterSave.Visible = false;
                    ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    ddlFileCatagory.SelectedIndex = ddlFileCatagory.Items.IndexOf(ddlFileCatagory.Items.FindByText("--Select--"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                if(hdnFlag.Value == "2")//partial update
                {
                    string ObjStatuExist = new ClsCardFeeConfigurationBAL().FunAddEditCardFeeMasterData(ObjCrdFeeCnfg);
                    LblMessage.Text = ObjStatuExist.Split('|')[0];
                    ClearAllValues();
                    ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    ddlFileCatagory.SelectedIndex = ddlFileCatagory.Items.IndexOf(ddlFileCatagory.Items.FindByText("--Select--"));

                    // ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    pnlCardFeeMasterSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }


            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardFeeConfigure, btnSave_Click()", ex.Message, "");

            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";

            try
            {

                ObjCrdFeeCnfg.IssuerNo = Convert.ToInt32(txtIssuerNo.Value);
                string ObjReturnStatus = new ClsCardFeeConfigurationBAL().FunDeleteBankForCardFee(ObjCrdFeeCnfg);
                LblMessage.Text = ObjReturnStatus.ToString();
                ClearAllValues();
                ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                ddlFileCatagory.SelectedIndex = ddlFileCatagory.Items.IndexOf(ddlFileCatagory.Items.FindByText("--Select--"));

                //txtIssuerNo.Value = "";
                pnlCardFeeMasterSave.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);


            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CIFUpdation, btnDelete_Click()", ex.Message, "");

            }


        }

    }
}