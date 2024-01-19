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
    public partial class BankModuleData : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        BankModuleDataBO Obj = new BankModuleDataBO();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!Page.IsPostBack)
                {

                    string OptionNeumonic = "BMD"; //unique optionneumonic from database
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
                    //FunGetdropdown();


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

            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetFrequencyUnit();
            ddlFrequencyUnit.DataSource = ObjDTOutPut;
            ddlFrequencyUnit.DataTextField = "Frequency Unit";
            //ddlFrequencyUnit.DataValueField = "Id";
            ddlFrequencyUnit.DataBind();
            ddlFrequencyUnit.Items.Insert(0, new ListItem("--Select--", "0"));
            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetEnableState();
            ddlEnableState.DataSource = ObjDTOutPut;
            ddlEnableState.DataTextField = "Enable state";
            ddlEnableState.DataValueField = "Id";
            ddlEnableState.DataBind();
            ddlEnableState.Items.Insert(0, new ListItem("--Select--", "-1"));
            //13-12-17
            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetStatus();
            ddlStatus.DataSource = ObjDTOutPut;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "Id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("--Select--", "-1"));


        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtSearchModuleName.Value))
            //{
            //    Response.Write("<script>alert('please insert issueer no');</script>");
            //}
            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";
                //Obj.ModuleName = txtSearchModuleName.Value;
                Obj.IssuerNum = Convert.ToInt32(txtSearchIssuerNo.Value);
                string ObjStatusIsExist = new ClsBankConfigurationBAL().FunIsModuleAndBankExist(Obj);
                //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Searching for bank", txtSearchIssuerNo.Value, "", "", "", "", "", "Searching", "1");

                if (ObjStatusIsExist.Split('|')[1] == "Exist")
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Search result", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "1");
                    //hdnFlag.Value = "0";//to update
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    FunGetResult(Obj);
                }
                 if (ObjStatusIsExist.Split('|')[0] == "Bank is not configured")
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Searched bank is not configured", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "1");
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "3";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "Bank Is Not Configured";
                                        

                }
                 if(ObjStatusIsExist.Split('|')[0] == "Bank is not exist") 
                {
                    //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Searched bank does not exist", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "0");
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //hdnFlag.Value = "1";//to insert
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";

                }
            }
            
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankModuleData, btnSearch_Click()", Ex.Message, "");
            }
        }

        public void FunGetResult(BankModuleDataBO obj)
        {
            try
            {
                string OptionNeumonic = "BMD"; //unique optionneumonic from database
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
                ObjDTOutPut = new ClsBankConfigurationBAL().FunSearchBankModuleData(obj);
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
                                              { new AGS.Utilities.Attribute() { AttributeName = "IssuerNo", BindTableColumnValueWithAttribute = "Issuernum" }

                        , new AGS.Utilities.Attribute() { AttributeName = "ModuleName", BindTableColumnValueWithAttribute = "ModuleName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "ClassName", BindTableColumnValueWithAttribute = "ClassName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "DllPath", BindTableColumnValueWithAttribute = "DllPath" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Frequency", BindTableColumnValueWithAttribute = "Frequency" }
                        , new AGS.Utilities.Attribute() { AttributeName = "FrequencyUnit", BindTableColumnValueWithAttribute = "FrequencyUnit" }
                        , new AGS.Utilities.Attribute() { AttributeName = "EnableState", BindTableColumnValueWithAttribute = "EnableState" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Status", BindTableColumnValueWithAttribute = "Status" }
                                     }
                        };

                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("Issuernum", objAdded);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankModuleData, FunGetResult()", ex.Message, "");
            }
        }
        
        protected void add_Click(object sender, EventArgs e)
        {
            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";
                Obj.ModuleName = txtModuleName.Value;
                Obj.IssuerNum = Convert.ToInt32(txtIssuerNo.Value);
                
                Obj.Frequency = Convert.ToInt32(txtFrequency.Value);
                Obj.FrequencyUnit = ddlFrequencyUnit.SelectedValue;
                //Obj.EnableState = (txtEnableState.Value=="True" ? 1 : 0);
                Obj.EnableState = ddlEnableState.SelectedValue;
                Obj.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                
                Obj.DllPath = txtDllPath.Value;
                Obj.ClassName = txtClassName.Value;
                //if (txtIssuerNo.Value == txtSearchIssuerNo.Value)
                //{
                    string ModuleData = new ClsBankConfigurationBAL().FunPutBankModuleData(Obj);

                    if (ModuleData.Split('|')[1] == "Insert Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Bank is inserted", txtIssuerNo.Value, "", "", "", "", "", "Add", "1");
                    LblMessage.Text = ModuleData.Split('|')[0];
                    if (txtSearchIssuerNo.Value != txtIssuerNo.Value)
                    {
                        txtSearchIssuerNo.Value = "";
                    }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }

                    if (ModuleData.Split('|')[1] == "Update Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Bank is updated", txtIssuerNo.Value, "", "", "", "", "", "Update", "1");
                    LblMessage.Text = ModuleData.Split('|')[0];
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }

                    //if (ModuleData.Split('|')[1] == "Failed")
                    //{
                    //    LblMessage.Text = ModuleData.Split('|')[0];
                    //    //txtSearchIssuerNo.Value = "";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //}

                    

                }
                //else
                //{
                //    LblMessage.Text = "Modulename and issuerno doesn't matches";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //}
            
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankModuleData, add_Click()", Ex.Message, "");
            }
            finally
            {
                FunGetResult(Obj);
            }
            // Response.Redirect(Request.Url.PathAndQuery, true);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {


            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";

                Obj.ModuleName = txtModuleName.Value;
                Obj.IssuerNum = Convert.ToInt32(txtIssuerNo.Value);
                
                
                string ModuleData = new ClsBankConfigurationBAL().FunDeleteModuleRecordForBank(Obj);
                
                if (ModuleData.Split('|')[1] == "Delete Success")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Bank is deleted", txtIssuerNo.Value, "", "", "", "", "", "Delete", "1");
                    LblMessage.Text = ModuleData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    
                }


                if (ModuleData.Split('|')[1] == "Failed")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank Module Details", "Faild to delete bank", txtIssuerNo.Value, "", "", "", "", "", "Delete", "0");
                    LblMessage.Text = ModuleData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }

                
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankModuleData, Delete_Click()", Ex.Message, "");

            }
            finally
            {
                FunGetResult(Obj);
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect("BankModuleData.aspx");
            Response.Redirect(Request.Url.PathAndQuery, true);
        }
    }
}