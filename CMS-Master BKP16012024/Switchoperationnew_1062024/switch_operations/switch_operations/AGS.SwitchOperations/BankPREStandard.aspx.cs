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
    public partial class BankPREStandard : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsBankPREStandardBO PREStand = new ClsBankPREStandardBO();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if(!Page.IsPostBack)
                { 
                string OptionNeumonic = "BPS"; //unique optionneumonic from database
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

            catch(Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, Page_Load()", ex.Message, "");
            }

            
        }

        public void FunGetdropdown()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetPREPadding();
            ddlPadding.DataSource = ObjDTOutPut;
            ddlPadding.DataTextField = "Padding";
            //ddlAccountType.DataValueField = "Id";
            ddlPadding.DataBind();
            ddlPadding.Items.Insert(0, new ListItem("--Select--", "0"));

            ObjDTOutPut = new ClsBankConfigurationBAL().FunGetPREDirection();
            ddlDirection.DataSource = ObjDTOutPut;
            ddlDirection.DataTextField = "Direction";
            //ddlAccountType.DataValueField = "Id";
            ddlDirection.DataBind();
            ddlDirection.Items.Insert(0, new ListItem("--Select--", "0"));

            
            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                ClsBankPREStandardBO PREStand = new ClsBankPREStandardBO();
                PREStand.IssuerNo = txtSearchIssuerNo.Value;
                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Searching for bank PRE Details", txtSearchIssuerNo.Value, "", "", "", "", "", "Searching", "1");

                //18-12-17
                string ObjStatusIsExist = new ClsBankConfigurationBAL().FunIsBank_Cardprogram_TokenExist(PREStand);
                if (ObjStatusIsExist.Split('|')[1] == "Exist")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Result found for Bank PRE Details", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "1");
                    //hdnFlag.Value = "0";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    FunGetResult(PREStand);
                }

                if (ObjStatusIsExist.Split('|')[0] == "Bank is not configured")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Bank not configured", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "0");
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "3";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "Bank Is Not Configured";
                    //BtnAddNew.Style.Add("visibility", "hidden");
                    //Button mybut1 = (Button)FindControl("BtnAddNew");
                    //mybut1.Visible = false;


                }
                if (ObjStatusIsExist.Split('|')[0] == "Bank is not exist") 
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Bank does not exist", txtSearchIssuerNo.Value, "", "", "", "", "", "Searched", "0");
                    LblMessage.Text = ObjStatusIsExist.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //hdnFlag.Value = "1";
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";

                }
            }
            catch(Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, btnSearch_Click()", ex.Message, "");
            }
        }

        public void FunGetResult(ClsBankPREStandardBO PREStand)
        {
            try
            {
                string OptionNeumonic = "BPS"; //unique optionneumonic from database
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
                ObjDTOutPut = new ClsBankConfigurationBAL().FunGetPREStandardData(PREStand);
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    
                    //For user those having Edit right
                    if (accessPrefix.Contains("E") ) 
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData()
                        {
                            control = AGS.Utilities.Controls.Button,
                            buttonName = "Edit",
                            cssClass = "btn btn-primary",
                            

                            hideColumnName = true,
                            events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                              ,
                            attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "IssuerNo", BindTableColumnValueWithAttribute = "IssuerNo" }

                        , new AGS.Utilities.Attribute() { AttributeName = "CardProgram", BindTableColumnValueWithAttribute = "CardProgram" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Token", BindTableColumnValueWithAttribute = "Token" }
                        , new AGS.Utilities.Attribute() { AttributeName = "OutputPosition", BindTableColumnValueWithAttribute = "OutputPosition" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Padding", BindTableColumnValueWithAttribute = "Padding" }
                        , new AGS.Utilities.Attribute() { AttributeName = "FixLength", BindTableColumnValueWithAttribute = "FixLength" }
                        , new AGS.Utilities.Attribute() { AttributeName = "StartPos", BindTableColumnValueWithAttribute = "StartPos" }
                        , new AGS.Utilities.Attribute() { AttributeName = "EndPos", BindTableColumnValueWithAttribute = "EndPos" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Direction", BindTableColumnValueWithAttribute = "Direction" }

                                     }
                        };
                       
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("IssuerNo", objAdded);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, FunGetResult()", ex.Message, "");
            }
            }
        //for saving data
        protected void add_Click(object sender, EventArgs e)
        {


            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";

                PREStand.IssuerNo = txtIssuerNo.Value;
                PREStand.CardProgram = txtCardProgram.Value;
                PREStand.Token = txtToken.Value;
                PREStand.OutputPosition = txtOutputPosition.Value;
                PREStand.Padding = ddlPadding.SelectedValue;
                PREStand.FixLength = txtFixLength.Value;
                PREStand.StartPos = txtStartPos.Value;
                PREStand.EndPos = txtEndPos.Value;
                PREStand.Direction = ddlDirection.SelectedValue;

                //if (txtIssuerNo.Value == txtSearchIssuerNo.Value)
                //{

                    //string PREData = new ClsBankConfigurationBAL().FunPutPREStandardDataForBank(PREStand, "Edit");
                    string PREData = new ClsBankConfigurationBAL().FunPutPREStandardDataForBank(PREStand);
                    
                    if (PREData.Split('|')[1] == "Insert Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Bank PREStanderd details inserted", txtIssuerNo.Value, "", "", "", "", "", "Add", "1");
                    LblMessage.Text = PREData.Split('|')[0];
                    if (txtSearchIssuerNo.Value != txtIssuerNo.Value)
                    {
                        txtSearchIssuerNo.Value = "";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        
                    }

                    if (PREData.Split('|')[1] == "Update Success")
                    
                    {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Bank PREStanderd details updated", txtIssuerNo.Value, "", "", "", "", "", "Update", "1");
                    LblMessage.Text = PREData.Split('|')[0];
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    }

                    //if (PREData.Split('|')[1] == "Failed")
                    //{
                    //    LblMessage.Text = PREData.Split('|')[0];
                    //    txtSearchIssuerNo.Value = "";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    //}




                }
                //else
                //{
                //    LblMessage.Text = "Issuerno doesn't matches with search";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                //}
            
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, add_Click()", Ex.Message, "");

            }
            finally
            {
                FunGetResult(PREStand);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {


            try
            {
                LblMessage.Text = "";
                LblResult.InnerHtml = "";

                PREStand.IssuerNo = txtIssuerNo.Value;
                PREStand.CardProgram = txtCardProgram.Value;
                PREStand.Token = txtToken.Value;
                
                
                string PREData = new ClsBankConfigurationBAL().FunDeletePRERecordForBank(PREStand);
                //hdnFlag.Value = "2";
                if (PREData.Split('|')[1] == "Delete Success")
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Bank PREStandard is deleted", txtIssuerNo.Value, "", "", "", "", "", "Delete", "1");
                    LblMessage.Text = PREData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Reload()", true);
                }

                
               if (PREData.Split('|')[1] == "Failed")
               {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Bank PREStandard Details", "Faild to delete bank PREStandard", txtIssuerNo.Value, "", "", "", "", "", "Delete", "0");
                    LblMessage.Text = PREData.Split('|')[0];
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
               }

                
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankPREStandard, add_Click()", Ex.Message, "");

            }
            finally
            {
                FunGetResult(PREStand);
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery, true);
        }
        
    }
    }


            

        
    
