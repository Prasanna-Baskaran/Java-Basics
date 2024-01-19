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
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    public partial class BinConfigure : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsBankConfigureBO ObjBank = new ClsBankConfigureBO();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if(!Page.IsPostBack)
                { 
                string OptionNeumonic = "BN"; //unique optionneumonic from database
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    FunGetdropdown();
                   
               }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, Page_Load()", ex.Message, "");
            }

            
        }

        public void FunGetdropdown()
        {
            DataTable ObjDTOutPut = new DataTable();

            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetAccountType();
            ddlAccountType.DataSource = ObjDTOutPut;
            ddlAccountType.DataTextField = "AccountTypeName";
            ddlAccountType.DataValueField = "AccountTypeCode";
            ddlAccountType.DataBind();
            ddlAccountType.Items.Insert(0, new ListItem("--Select--", "0"));

            //12-12-17
            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetCardType();
            ddlCardType.DataSource = ObjDTOutPut;
            ddlCardType.DataTextField = "CardType";
            //ddlCardType.DataValueField = "Id";
            ddlCardType.DataBind();
            ddlCardType.Items.Insert(0, new ListItem("--Select--", "0"));

            //4-1-17
            //ObjDTOutPut = new ClsBankConfigurationBAL().FunGetIsMagstrip();
            //ddlIsMagstrip.DataSource = ObjDTOutPut;
            //ddlIsMagstrip.DataTextField = "Cardtype";
            //ddlIsMagstrip.DataValueField = "IsMagstrip";
            //ddlIsMagstrip.DataBind();
            //ddlIsMagstrip.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.InnerHtml = "";
                LblMessage.Text = "";
                
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                ClsBankConfigureBO ObjBank = new ClsBankConfigureBO();
                ObjBank.IssuerNo = txtSearchIssuerNo.Value;

                //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "Searching for BIN", txtSearchIssuerNo.Value, "", "", "", "", "", "Searching", "1");

                string ObjStatusIsExist = new ClsBankConfigurationBAL().FunIsBankBinExist(ObjBank);
                if (ObjStatusIsExist.Split('|')[1] == "Exist")
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "Search result", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "1");
                    
                    //hdnFlag.Value = "0";
                    //LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    FunGetResult(ObjBank);
                }
                
                if (ObjStatusIsExist.Split('|')[0] == "Bank is not configured")
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "Searched BIN is not configured", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "1");
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "3";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "Bank Is Not Configured";
                    

                }
                 if  (ObjStatusIsExist.Split('|')[0] == "Bank is not exist")
                    {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "Searched BIN does not exist", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "0");
                    // hdnFlag.Value = "1";
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, btnSearch_Click()", ex.Message, "");
            }
        }

        public void FunGetResult(ClsBankConfigureBO ObjBank)
        {
            try
            {
                string OptionNeumonic = "BN"; //unique optionneumonic from database
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

                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsBankConfigurationBAL().FunGetBankBinData(ObjBank);
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    
                    //For user those having Edit right
                    if (accessPrefix.Contains("E"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData()
                        {
                            control = AGS.Utilities.Controls.Button,
                            buttonName = "Edit",
                            cssClass = "btn btn-primary"
                                              ,
                            hideColumnName = true,
                            events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                              ,
                            attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "IssuerNo", BindTableColumnValueWithAttribute = "BankCode" }

                         ,new AGS.Utilities.Attribute() { AttributeName = "CardProgram", BindTableColumnValueWithAttribute = "CardProgram" }
                         , new AGS.Utilities.Attribute() { AttributeName ="CardPrefix", BindTableColumnValueWithAttribute = "CardPrefix" }
                        , new AGS.Utilities.Attribute() { AttributeName = "InstitutionID", BindTableColumnValueWithAttribute = "InstitutionID" }
                        , new AGS.Utilities.Attribute() { AttributeName = "AccountType", BindTableColumnValueWithAttribute = "AccountType" }
                        , new AGS.Utilities.Attribute() { AttributeName = "CardType", BindTableColumnValueWithAttribute = "CardType" }
                        , new AGS.Utilities.Attribute() { AttributeName = "BinDesc", BindTableColumnValueWithAttribute = "BinDesc" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Currency", BindTableColumnValueWithAttribute = "Currency" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Switch_CardType", BindTableColumnValueWithAttribute = "Switch_CardType" }
                        , new AGS.Utilities.Attribute() { AttributeName = "SystemID", BindTableColumnValueWithAttribute = "SystemID" }
                        , new AGS.Utilities.Attribute() { AttributeName = "IsMagstrip", BindTableColumnValueWithAttribute = "IsMagstrip" }
                        , new AGS.Utilities.Attribute() { AttributeName = "BankID", BindTableColumnValueWithAttribute = "BankID" }
                        , new AGS.Utilities.Attribute() { AttributeName = "PREFormat", BindTableColumnValueWithAttribute = "PREFormat" }
                                     }
                        };

                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("BankID,BankCode", objAdded);
                    }
                 }
                else
                {
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    
                    hdnFlag.Value = "";
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "Bank Is Not Configured";
                    
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, FunGetResult()", ex.Message, "");
            }
        }

        protected void add_Click(object sender, EventArgs e)
        {
            //txtSystemID.Visible = false;
            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";
                ObjBank.IssuerNo = txtIssuerNo.Value;
                ObjBank.CardProgram = txtCardProgram.Value;
                ObjBank.CardPrefix = txtCardPrefix.Value;
                ObjBank.BinDesc = txtBinDesc.Value;
                ObjBank.SystemID = txtSystemID.Value;
                ObjBank.PREFormat = txtPREFormat.Value;
                ObjBank.SwitchInstitutionID = txtInstitutionID.Value;
                ObjBank.Currency = txtCurrency.Value;
                ObjBank.SwitchCardType = txtSwitch_CardType.Value;
                ObjBank.IsMagstrip = Convert.ToInt32(ddlIsMagstrip.SelectedValue);
                ObjBank.AccountType = (ddlAccountType.SelectedValue);
                ObjBank.CardType = ddlCardType.SelectedValue;
                string BinData = new ClsBankConfigurationBAL().FunPutBinDataForBank(ObjBank);
                
                //if (txtIssuerNo.Value == txtSearchIssuerNo.Value)
                //{
                    if (BinData.Split('|')[1] == "Insert Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "BIN is inserted", txtIssuerNo.Value, "", "", "", "", "", "Add", "1");
                    LblMessage.Text = BinData.Split('|')[0];
                    if (txtSearchIssuerNo.Value != txtIssuerNo.Value)
                    {
                        txtSearchIssuerNo.Value = "";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Reload()", true);
                    }
                    
                    if (BinData.Split('|')[1] == "Update Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "BIN is updated", txtIssuerNo.Value, "", "", "", "", "", "Update", "1");
                    LblMessage.Text = BinData.Split('|')[0];
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }

                    //if (BinData.Split('|')[1] == "Failed")
                    //{
                    //    LblMessage.Text = BinData.Split('|')[0];
                    //    txtSearchIssuerNo.Value = "";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //}
                }

                //else
                //{
                //    LblMessage.Text = "Issuerno doesn't matches";
                 //   ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //}
            //}
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, add_Click()", Ex.Message, "");

            }
            finally
            {
                FunGetResult(ObjBank);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {


            try
            {
                
                LblMessage.Text = "";
                LblResult.InnerHtml = "";
                ObjBank.IssuerNo = txtIssuerNo.Value;
                ObjBank.CardProgram = txtCardProgram.Value;
                ObjBank.CardPrefix = txtCardPrefix.Value;
                

                
                //string PREData = new ClsBankConfigurationBAL().FunPutPREStandardDataForBank(PREStand, "Edit");
                string BinData = new ClsBankConfigurationBAL().FunDeleteBINRecordForBank(ObjBank);
                //hdnFlag.Value = "2";
                if (BinData.Split('|')[1] == "Delete Success")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "BIN is deleted", txtIssuerNo.Value, "", "", "", "", "", "Delete", "1");
                    LblMessage.Text = BinData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Reload()", true);
                }


                if (BinData.Split('|')[1] == "Failed")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "BIN Configure", "Faild to delete BIN", txtIssuerNo.Value, "", "", "", "", "", "Delete", "0");
                    LblMessage.Text = BinData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }

               
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BinConfigure, Delete_Click()", Ex.Message, "");

            }
            finally
            {
                FunGetResult(ObjBank);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery, true);
        }

    }
}