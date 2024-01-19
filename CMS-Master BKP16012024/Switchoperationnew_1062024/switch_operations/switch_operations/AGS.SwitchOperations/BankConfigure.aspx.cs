using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class BankConfigure : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        ClsBankConfigureBO ObjBank = new ClsBankConfigureBO();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if(!Page.IsPostBack)
                { 
                
                    string OptionNeumonic = "BCR"; //unique optionneumonic from database
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

                }

            }

            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, Page_Load()", ex.Message, "");
            }

            
        }

        public void FunGetdropdown()
        {
            DataTable ObjDTOutPut = new DataTable();

            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetSystemId();
            ddlSystemId.DataSource = ObjDTOutPut;
            ddlSystemId.DataTextField = "SystemName";
            ddlSystemId.DataValueField = "SystemID";
            ddlSystemId.DataBind();
            ddlSystemId.Items.Insert(0, new ListItem("--Select--", "0"));
            pnlBankConfigureSave.Visible = false;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
            {
            
            LblMessage.Text = "";
            ClearAllValues();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //if (string.IsNullOrEmpty(txtSearchIssuerNo.Value))
            //{
            //    Response.Write("<script>alert('please insert issuer no');</script>");
            //}

            if (!string.IsNullOrEmpty(txtSearchIssuerNo.Value))
            {
                ObjBank.IssuerNo = txtSearchIssuerNo.Value;
            }

            
            string ObjStatusIsExist = new ClsBankConfigurationBAL().FunIsBankExist(ObjBank);

            try
            {
                if (ObjStatusIsExist.Split('|')[1] == "Exist")
                {
                    hdnFlag.Value = "0";
                    pnlBankConfigureSave.Visible = true;
                    try
                    {
                        DataTable ObjDTOutPut = new DataTable();
                        ObjDTOutPut = new ClsBankConfigurationBAL().FunGetBankFolder(ObjBank);
                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            //tblbanks param
                            txtBankFolder.Value = ObjDTOutPut.Rows[0][0].ToString();
                            //11-1-18
                            if (!string.IsNullOrEmpty(txtBankFolder.Value))
                            //if (ObjDTOutPut.Rows[0][0].ToString() != "")
                            {
                                txtBankFolder.Attributes.Add("readonly", "readonly");
                            }
                        }
                        ObjDTOutPut = new ClsBankConfigurationBAL().FunGetBankData(ObjBank);

                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            //tblbanks param

                            txtBankName.Value = ObjDTOutPut.Rows[0]["BankName"].ToString();
                            txtIssuerNo.Value = ObjDTOutPut.Rows[0]["BankCode"].ToString();
                            ddlSystemId.SelectedValue = ObjDTOutPut.Rows[0]["SystemID"].ToString();
                            txtSourceNodes.Value = ObjDTOutPut.Rows[0]["SourceNodes"].ToString();
                            txtSinkNodes.Value = ObjDTOutPut.Rows[0]["SinkNodes"].ToString();

                            txtUserPrefix.Value = ObjDTOutPut.Rows[0]["UserPrefix"].ToString();
                            txtCustIdentity.Value = ObjDTOutPut.Rows[0]["CustIdentity"].ToString();
                            txtCustomerIDLen.Value = ObjDTOutPut.Rows[0]["CustomerIDLen"].ToString();

                            //tblcardautomation and tblcardautofilepath param
                            txtSwitchInstitutionID.Value = ObjDTOutPut.Rows[0]["SwitchInstitutionID"].ToString();
                            //txtBank.Value = ObjDTOutPut.Rows[0]["Bank"].ToString();

                            txtWinSCP_User.Value = ObjDTOutPut.Rows[0]["WinSCP_User"].ToString();
                            txtWinSCP_PWD.Value = ObjDTOutPut.Rows[0]["WinSCP_PWD"].ToString();
                            txtWinSCP_IP.Value = ObjDTOutPut.Rows[0]["WinSCP_IP"].ToString();
                            txtWinSCP_Port.Value = ObjDTOutPut.Rows[0]["WinSCP_Port"].ToString();
                            txtPGP_KeyName.Value = ObjDTOutPut.Rows[0]["PGP_KeyName"].ToString();

                            txtPGP_PWD.Value = ObjDTOutPut.Rows[0]["PGP_PWD"].ToString();
                            txtAGS_PGP_KeyName.Value = ObjDTOutPut.Rows[0]["AGS_PGP_KeyName"].ToString();
                            txtAGS_PGP_PWD.Value = ObjDTOutPut.Rows[0]["AGS_PGP_PWD"].ToString();
                            txtRCVREmailID.Value = ObjDTOutPut.Rows[0]["RCVREmailID"].ToString();

                            txtAGS_SFTPServer.Value = ObjDTOutPut.Rows[0]["AGS_SFTPServer"].ToString();
                            txtAGS_SFTP_Port.Value = ObjDTOutPut.Rows[0]["AGS_SFTP_Port"].ToString();
                            txtAGS_SFTP_Pwd.Value = ObjDTOutPut.Rows[0]["AGS_SFTP_Pwd"].ToString();
                            txtAGS_SFTP_User.Value = ObjDTOutPut.Rows[0]["AGS_SFTP_User"].ToString();

                            txtB_SFTPServer.Value = ObjDTOutPut.Rows[0]["B_SFTPServer"].ToString();
                            txtB_SFTP_Port.Value = ObjDTOutPut.Rows[0]["B_SFTP_Port"].ToString();
                            txtB_SFTP_Pwd.Value = ObjDTOutPut.Rows[0]["B_SFTP_Pwd"].ToString();
                            txtB_SFTP_User.Value = ObjDTOutPut.Rows[0]["B_SFTP_User"].ToString();

                            txtC_SFTPServer.Value = ObjDTOutPut.Rows[0]["C_SFTPServer"].ToString();
                            txtC_SFTP_Port.Value = ObjDTOutPut.Rows[0]["C_SFTP_Port"].ToString();
                            txtC_SFTP_Pwd.Value = ObjDTOutPut.Rows[0]["C_SFTP_Pwd"].ToString();
                            txtC_SFTP_User.Value = ObjDTOutPut.Rows[0]["C_SFTP_User"].ToString();

                            txtBankName.Attributes.Add("readonly", "readonly");
                            txtIssuerNo.Attributes.Add("readonly", "readonly");
                            //txtBank.Attributes.Add("readonly", "readonly");
                            btnDelete.Visible = true;
                            //btnDelete.Enabled = true;
                            //txtBankName.Attributes.Add("readonly", "true");
                            //txtBankName.Disabled = true;



                        }

                        else
                        {
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunClearAllTextValue()", true);
                            //pnlBankConfigureSave.Controls.Clear();
                            //ClearDropdown(this);
                            ClearAllValues();
                        }



                    }




                    catch (Exception Ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankConfigure, btnSearch_Click()", Ex.Message, "");
                    }
                    //LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                }
                if (ObjStatusIsExist.Split('|')[1] == "Not Exist")

                {
                    hdnFlag.Value = "1";
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0].ToString();
                    ClearAllValues();
                    pnlBankConfigureSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
            }
            catch(Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankConfigure, FunIsBankExist()", Ex.Message, "");
            }
            //12-12-17
            
        }
    //    public static void ScrollTo(this HtmlGenericControl control)
    //    {
    //        control.Page.RegisterClientScriptBlock("ScrollTo", string.Format(@"<script type='text/javascript'> $(document).ready(function() {{
    //            var element = document.getElementById('{0}');
    //            element.scrollIntoView();
    //            element.focus();
    //        }});

    //    </script>

    //", control.ClientID));
    //    }
        public void ClearAllValues()
        {
            txtBankName.Value = "";
            txtIssuerNo.Value = "";
            ddlSystemId.SelectedIndex = ddlSystemId.Items.IndexOf(ddlSystemId.Items.FindByText("--Select--"));
            txtSourceNodes.Value = "";
            txtSinkNodes.Value ="";

            txtUserPrefix.Value = "";
            txtCustIdentity.Value = "";
            txtCustomerIDLen.Value = "";

            //tblcardautomation and tblcardautofilepath param
            txtSwitchInstitutionID.Value = "";
            //txtBank.Value = "";

            txtWinSCP_User.Value = "";
            txtWinSCP_PWD.Value = "";
            txtWinSCP_IP.Value = "";
            txtWinSCP_Port.Value = "";
            txtPGP_KeyName.Value = "";
            txtBankFolder.Value = "";
            txtPGP_PWD.Value = "";
            txtAGS_PGP_KeyName.Value = "";
            txtAGS_PGP_PWD.Value = "";
            txtRCVREmailID.Value = "";

            txtAGS_SFTPServer.Value = "";
            txtAGS_SFTP_Port.Value = "";
            txtAGS_SFTP_Pwd.Value = "";
            txtAGS_SFTP_User.Value = "";
            txtB_SFTPServer.Value = "";
            txtB_SFTP_Port.Value = "";
            txtB_SFTP_Pwd.Value = "";
            txtB_SFTP_User.Value = "";
            txtC_SFTPServer.Value = "";
            txtC_SFTP_Port.Value = "";
            txtC_SFTP_Pwd.Value = "";
            txtC_SFTP_User.Value = "";
            txtBankName.Attributes.Remove("readonly");
            txtIssuerNo.Attributes.Remove("readonly");
            //txtBank.Attributes.Remove("readonly");
            txtBankFolder.Attributes.Remove("readonly");
            btnDelete.Visible = false;

          //ddlAccountType.SelectedIndex = ddlAccountType.Items.IndexOf(ddlAccountType.Items.FindByText("--Select--"));

        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";
           

            //ClsReturnStatusBO objReturnstatus = new ClsReturnStatusBO();
            try
            {
                ClsBankConfigureBO ObjBankConfig = new ClsBankConfigureBO();
                //added
                //tblbanks param
                ObjBankConfig.BankName = txtBankName.Value;
                ObjBankConfig.IssuerNo = txtIssuerNo.Value;
                ObjBankConfig.SystemID = ddlSystemId.SelectedValue;
                ObjBankConfig.SourceNodes = txtSourceNodes.Value;
                ObjBankConfig.SinkNodes = txtSinkNodes.Value;
                ObjBankConfig.UserPrefix = txtUserPrefix.Value;
                ObjBankConfig.CustIdentity = txtCustIdentity.Value;
                ObjBankConfig.CustomerIDLen = txtCustomerIDLen.Value;

                //tblcardautomation param
                ObjBankConfig.BankFolder = txtBankFolder.Value;
                ObjBankConfig.SwitchInstitutionID = txtSwitchInstitutionID.Value;
               // ObjBankConfig.Bank = txtBank.Value;
                ObjBankConfig.WinSCP_User = txtWinSCP_User.Value;
                ObjBankConfig.WinSCP_PWD = txtWinSCP_PWD.Value;

                //newly added tblcardautomation param and tblcardautofilepath
                ObjBankConfig.WinSCP_IP = txtWinSCP_IP.Value;
                ObjBankConfig.WinSCP_Port = txtWinSCP_Port.Value;
                ObjBankConfig.PGP_KeyName = txtPGP_KeyName.Value;
                ObjBankConfig.PGP_PWD = txtPGP_PWD.Value;
                ObjBankConfig.AGS_PGP_KeyName = txtAGS_PGP_KeyName.Value;
                ObjBankConfig.AGS_PGP_PWD = txtAGS_PGP_PWD.Value;
                ObjBankConfig.RCVREmailID = txtRCVREmailID.Value;

                ObjBankConfig.AGS_SFTPServer = txtAGS_SFTPServer.Value;
                ObjBankConfig.AGS_SFTP_Port = txtAGS_SFTP_Port.Value;
                ObjBankConfig.AGS_SFTP_Pwd = txtAGS_SFTP_Pwd.Value;
                ObjBankConfig.AGS_SFTP_User = txtAGS_SFTP_User.Value;
                ObjBankConfig.B_SFTPServer = txtB_SFTPServer.Value;
                ObjBankConfig.B_SFTP_Port = txtB_SFTP_Port.Value;
                ObjBankConfig.B_SFTP_Pwd = txtB_SFTP_Pwd.Value;
                ObjBankConfig.B_SFTP_User = txtB_SFTP_User.Value;
                ObjBankConfig.C_SFTPServer = txtC_SFTPServer.Value;
                ObjBankConfig.C_SFTP_Port = txtC_SFTP_Port.Value;
                ObjBankConfig.C_SFTP_Pwd = txtC_SFTP_Pwd.Value;
                ObjBankConfig.C_SFTP_User = txtC_SFTP_User.Value;
                               
                  if(hdnFlag.Value=="1")
                {
                    
                    string ObjReturnStatus = new ClsBankConfigurationBAL().FunConfigureBank(ObjBankConfig);
                    LblMessage.Text = ObjReturnStatus.ToString();
                    ClearAllValues();
                    txtSearchIssuerNo.Value = "";
                    pnlBankConfigureSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }

                //if (ObjStatusFolderIsExist.Split('|')[1] == "Exist")
                else
                {


                    string ObjStatuExist = new ClsBankConfigurationBAL().FunUpdateConfiguredBank(ObjBankConfig);
                    LblMessage.Text = ObjStatuExist.Split('|')[0];
                    ClearAllValues();
                    txtSearchIssuerNo.Value = "";
                    pnlBankConfigureSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                

            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, BankConfigure, btnSave_Click()", ex.Message, "");
                
            }


            
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            //ClsReturnStatusBO objReturnstatus = new ClsReturnStatusBO();
            try
            {
                ClsBankConfigureBO ObjBankConfig = new ClsBankConfigureBO();
                //added
                //tblbanks param
                ObjBankConfig.BankName = txtBankName.Value;
                ObjBankConfig.IssuerNo = txtIssuerNo.Value;
                string ObjReturnStatus = new ClsBankConfigurationBAL().FunDeleteBank(ObjBankConfig);
                LblMessage.Text = ObjReturnStatus.ToString();
                txtSearchIssuerNo.Value = "";
                ClearAllValues();
                pnlBankConfigureSave.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                

            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, BankConfigure, btnDelete_Click()", ex.Message, "");
                
            }


            }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect("BankConfigure.aspx");
            Response.Redirect(Request.Url.PathAndQuery, true);
        }
        
                
        protected void SystemId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }

}