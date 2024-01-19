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
    public partial class CIFUpdation : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        ClsCIFUpdationBO ObjCIFUpdt = new ClsCIFUpdationBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string OptionNeumonic = "CIFU"; //unique optionneumonic from database
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
                    //FunGetdropdown();
                    pnlCIFUpadationSave.Visible = false;

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CIFUpdation, Page_Load()", ex.Message, "");
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            ClearAllValues();
            if (!string.IsNullOrEmpty(txtSearchIssuerNo.Value))
            {
                ObjCIFUpdt.IssuerNo = Convert.ToInt32(txtSearchIssuerNo.Value);
            }

            string ObjStatusIsExist = new ClsCIFUpdationBAL().FunIsBankExistWithCIFFormat(ObjCIFUpdt);

            try
            {
                if (ObjStatusIsExist.Split('|')[1] == "Configured")
                {
                    hdnFlag.Value = "0";
                    //LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    pnlCIFUpadationSave.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    try
                    {
                        DataTable ObjDTOutPut = new DataTable();

                        ObjDTOutPut = new ClsCIFUpdationBAL().FunGetBankCIFFormat(ObjCIFUpdt);

                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            txtIssuerNo.Value= ObjDTOutPut.Rows[0]["IssuerNr"].ToString();
                            txtSFTPServer.Value = ObjDTOutPut.Rows[0]["ServerIp"].ToString();
                            txtSFTPPort.Value = ObjDTOutPut.Rows[0]["ServerPort"].ToString();
                            txtSFTPIncoming.Value = ObjDTOutPut.Rows[0]["FilePathInput"].ToString();
                            txtSFTPOutput.Value = ObjDTOutPut.Rows[0]["FilePathOutPut"].ToString();
                            txtSFTPArchive.Value = ObjDTOutPut.Rows[0]["FilePathArchive"].ToString();
                            txtSFTPUser.Value = ObjDTOutPut.Rows[0]["Username"].ToString();
                            txtSFTPPassword.Value = ObjDTOutPut.Rows[0]["password"].ToString();
                            ddlEnable.SelectedValue = (ObjDTOutPut.Rows[0]["Enable"].ToString() == "True" ? "1" : ObjDTOutPut.Rows[0]["Enable"].ToString() == "False" ? "0" : "-1");

                            txtFilePath.Value = ObjDTOutPut.Rows[0]["filepath"].ToString();
                            txtFileHeader.Value = ObjDTOutPut.Rows[0]["FileHeader"].ToString();
                            txtSFTPRepinInput.Value = ObjDTOutPut.Rows[0]["FilePathInput_RePIN"].ToString();
                            txtSFTPRepinOutput.Value = ObjDTOutPut.Rows[0]["FilePathOutPut_RePIN"].ToString();
                            txtSFTPRepinArchive.Value = ObjDTOutPut.Rows[0]["FilePathArchive_RePIN"].ToString();
                            txtRePINFilePath.Value = ObjDTOutPut.Rows[0]["FilePath_RePIN"].ToString();
                            txtRePINFileHeader.Value = ObjDTOutPut.Rows[0]["fileHeader_RePIN"].ToString();
                            ddlPGP.SelectedValue = (ObjDTOutPut.Rows[0]["IsPGP"].ToString() == "True" ? "1" : ObjDTOutPut.Rows[0]["IsPGP"].ToString() == "False" ? "0" : "-1");
                            if(ddlPGP.SelectedValue=="1")
                            {
                                hdnPGP.Value = "1";
                            }
                            if (ddlPGP.SelectedValue == "0")
                            {
                                hdnPGP.Value = "0";
                            }
                            if (ddlPGP.SelectedValue == "-1")
                            {
                                hdnPGP.Value = "";
                            }
                            ddlTrace.SelectedValue = (ObjDTOutPut.Rows[0]["Trace"].ToString() == "True" ? "1" : ObjDTOutPut.Rows[0]["Trace"].ToString() == "False" ? "0" : "-1");
                            txtPGPPublicKeyFilePath.Value = ObjDTOutPut.Rows[0]["PublicKeyFilePath"].ToString();
                            txtPGPPrivateKeyFilePath.Value = ObjDTOutPut.Rows[0]["PrivateKeyFilePath"].ToString();
                            txtPGPPassword.Value = ObjDTOutPut.Rows[0]["Password_PGP"].ToString();
                            txtPGPInputFilePath.Value = ObjDTOutPut.Rows[0]["InputFilePath_PGP"].ToString();
                            txtIssuerNo.Attributes.Add("readonly", "readonly");
                            btnDelete.Visible = true;
                            //btnDelete.Enabled = true;

                        }
                        else
                        {
                            ClearAllValues();
                        }

                    }

                    catch (Exception Ex)
                    {
                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, CIFUpdation, btnSearch_Click()", Ex.Message, "");
                    }
                }



                else

                {
                    hdnFlag.Value = "1";
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0].ToString();
                    pnlCIFUpadationSave.Visible = true;
                    ClearAllValues();
                    //txtIssuerNo.Value = txtSearchIssuerNo.Value;
                    //txtIssuerNo.Attributes.Add("readonly", "readonly");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                }



            }

            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CIFUpdation, btnSearch_Click()", Ex.Message, "");
            }

        }
        public void ClearAllValues()
        {
            txtIssuerNo.Value = ""; 
            txtSFTPServer.Value = "";
            txtSFTPPort.Value = "";
            txtSFTPIncoming.Value = "";
            txtSFTPOutput.Value = "";
            txtSFTPArchive.Value = "";
            txtSFTPUser.Value = "";
            txtSFTPPassword.Value = "";
            ddlEnable.SelectedIndex = ddlEnable.Items.IndexOf(ddlEnable.Items.FindByText("--Select--"));
            txtFilePath.Value = "";
            txtFileHeader.Value = "";
            txtSFTPRepinInput.Value = "";
            txtSFTPRepinOutput.Value = "";
            txtSFTPRepinArchive.Value = "";
            txtRePINFilePath.Value = "";
            txtRePINFileHeader.Value = "";
            ddlPGP.SelectedIndex = ddlPGP.Items.IndexOf(ddlPGP.Items.FindByText("--Select--"));
            ddlTrace.SelectedIndex = ddlTrace.Items.IndexOf(ddlTrace.Items.FindByText("--Select--"));
            txtPGPPublicKeyFilePath.Value = "";
            txtPGPPrivateKeyFilePath.Value = "";
            txtPGPPassword.Value = "";
            txtPGPInputFilePath.Value = "";

            txtIssuerNo.Attributes.Remove("readonly");
            btnDelete.Visible = false;



        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            try
            {
                ObjCIFUpdt.IssuerNo= Convert.ToInt32(txtIssuerNo.Value);
                ObjCIFUpdt.ServerIp = (txtSFTPServer.Value);
                ObjCIFUpdt.ServerPort = Convert.ToInt32(txtSFTPPort.Value);
                ObjCIFUpdt.FilePathInput = (txtSFTPIncoming.Value);
                ObjCIFUpdt.FilePathOutPut = (txtSFTPOutput.Value);
                ObjCIFUpdt.FilePathArchive = (txtSFTPArchive.Value);
                ObjCIFUpdt.Username = (txtSFTPUser.Value);
                ObjCIFUpdt.password = (txtSFTPPassword.Value);
                ObjCIFUpdt.EnableState = Convert.ToBoolean(ddlEnable.SelectedItem.Text == "True" ? 1 : ddlEnable.SelectedItem.Text == "False" ? 0 : -1);

                ObjCIFUpdt.filepath = txtFilePath.Value;
                ObjCIFUpdt.FileHeader = (txtFileHeader.Value);
                ObjCIFUpdt.FilePathInput_RePIN = (txtSFTPRepinInput.Value);
                ObjCIFUpdt.FilePathOutPut_RePIN = (txtSFTPRepinOutput.Value);
                ObjCIFUpdt.FilePathArchive_RePIN = (txtSFTPRepinArchive.Value);
                ObjCIFUpdt.FilePath_RePIN = (txtRePINFilePath.Value);
                ObjCIFUpdt.fileHeader_RePIN = (txtRePINFileHeader.Value);
                ObjCIFUpdt.IsPGP = Convert.ToBoolean(ddlPGP.SelectedItem.Text == "True" ? 1 : ddlPGP.SelectedItem.Text == "False" ? 0 : -1);
                ObjCIFUpdt.Trace = Convert.ToBoolean(ddlTrace.SelectedItem.Text == "True" ? 1 : ddlTrace.SelectedItem.Text == "False" ? 0 : -1);
                ObjCIFUpdt.Password_PGP = (txtPGPPassword.Value);
                ObjCIFUpdt.InputFilePath_PGP = (txtPGPInputFilePath.Value);
                ObjCIFUpdt.PublicKeyFilePath = (txtPGPPublicKeyFilePath.Value);
                ObjCIFUpdt.PrivateKeyFilePath = (txtPGPPrivateKeyFilePath.Value);


                if (hdnFlag.Value == "1")//insert
                {

                    string ObjReturnStatus = new ClsCIFUpdationBAL().FunAddEditBankCIFData(ObjCIFUpdt);
                    LblMessage.Text = ObjReturnStatus.ToString();
                    ClearAllValues();
                    txtSearchIssuerNo.Value = "";
                    pnlCIFUpadationSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }


                if (hdnFlag.Value == "0")//update
                {
                    string ObjStatuExist = new ClsCIFUpdationBAL().FunAddEditBankCIFData(ObjCIFUpdt);
                    LblMessage.Text = ObjStatuExist.ToString();
                    ClearAllValues();
                    txtSearchIssuerNo.Value = "";
                    // ddlSearchIssuerNo.SelectedIndex = ddlSearchIssuerNo.Items.IndexOf(ddlSearchIssuerNo.Items.FindByText("--Select--"));
                    pnlCIFUpadationSave.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CIFUpdation.aspx", "Save button clicked", "");


            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CIFUpdation, btnSave_Click()", ex.Message, "");

            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LblMessage.Text = "";

            try
            {

                ObjCIFUpdt.IssuerNo = Convert.ToInt32(txtSearchIssuerNo.Value);
                string ObjReturnStatus = new ClsCIFUpdationBAL().FunDeleteBankForCIF(ObjCIFUpdt);
                LblMessage.Text = ObjReturnStatus.ToString();
                ClearAllValues();
                txtSearchIssuerNo.Value = "";
                pnlCIFUpadationSave.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CIFUpdation.aspx", "Delete button clicked", "");

            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CIFUpdation, btnDelete_Click()", ex.Message, "");

            }


        }

        

        protected void ddlPGP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPGP.SelectedItem.Text == "True")
            {
                hdnPGP.Value = "1";
            }
            if (ddlPGP.SelectedItem.Text == "False")
            {
                hdnPGP.Value = "0";
            }
            if (ddlPGP.SelectedItem.Text == "--Select--")
            {
                hdnPGP.Value = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
        }
    }
}
