using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class BranchMaster : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "BM"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;

                        FunGetResult();

                    }
                    // Redirect to access denied page
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
                    }
                }
                // Redirect to access denied page
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }

            }
            catch (Exception ex)
            {
                // new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, Page_Load()", ex.Message, "");
            }



        }

        public void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            try
            {               
                ObjDTOutPut = (new ClsBranchDetailsBAL()).FunGetBranchDetails(Convert.ToInt32(Session["BankID"]));
            }
            catch (Exception ex) { }
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
                                          { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "id" }

                        , new AGS.Utilities.Attribute() { AttributeName = "BranchName", BindTableColumnValueWithAttribute = "BranchName" }
                    , new AGS.Utilities.Attribute() { AttributeName = "BranchCode", BindTableColumnValueWithAttribute = "BranchCode" }
                    ,new AGS.Utilities.Attribute() { AttributeName = "ACTIVE", BindTableColumnValueWithAttribute = "isACTIVE" }
                    
                                 }
                    };

                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("id,BranchID,BrachName,isACTIVE,CreatedBy,CreatedOn,ModifiedOn,ModifiedBy", objAdded);
                }
                else
                    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("id,BranchID,BrachName,isACTIVE,CreatedBy,CreatedOn,ModifiedOn,ModifiedBy");
            }

        }

        protected void add_Click(object sender, EventArgs e)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            DataTable ObjDTOutPut = new DataTable();
            ClsBranchDetailsBO ObjPriFeeBO = new ClsBranchDetailsBO() { BranchName = txt_Branchname.Value.Trim(), BranchCode = txt_Branchcode.Value.Trim(), BranchID = txt_Branchid.Value.Trim(),UpdatedBy= Convert.ToString(Session["UserName"]) };
            if (ObjPriFeeBO.BranchID == "" || ObjPriFeeBO.BranchID == null)
            {

                try
                {
                    ObjReturnStatus = (new ClsBranchDetailsBAL()).FunInsertIntoBranchMaster(ObjPriFeeBO.BranchName,ObjPriFeeBO.BranchCode, Convert.ToString(Session["UserName"]), Convert.ToInt32(Session["BankID"]));
                    LblMsg.InnerText = ObjReturnStatus.Description;
                    hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                    FunGetResult();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                catch (Exception ex)
                {
                    new ClsCommonBAL().FunInsertIntoErrorLog("CS, BranchDetails, add_click_btn()", ex.Message, "");
                }

                finally
                {
                    FunGetResult();

                    var flag = Convert.ToInt32(hdnResultStatus.Value);
                    if (flag == 0)
                    {
                        txt_Branchname.Value = string.Empty;
                        txt_Branchcode.Value = string.Empty;
                        txt_Branchid.Value = string.Empty;
                        ACTIVE.Checked = false;
                    }
                    else
                    {

                    }

                }
            }

            else
            {
                try
                {
                    ObjReturnStatus = (new ClsBranchDetailsBAL()).FunUpdateIntoBranchMaster(ObjPriFeeBO.BranchName, Convert.ToString(Session["UserName"]), Convert.ToInt32(Session["BankID"]), Convert.ToInt32(ObjPriFeeBO.BranchID),(ACTIVE.Checked?true:false));
                    LblMsg.InnerText = ObjReturnStatus.Description;
                    hdnResultStatus.Value = Convert.ToString(ObjReturnStatus.Code);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                catch (Exception ex)
                {

                    new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, add_click_btn()", ex.Message, "");

                }

                finally
                {

                    //hdnTransactionDetails.Value = (new ClsCommonBAL()).FunGetGridData(16, string.Empty);
                    FunGetResult();
                    txt_Branchcode.Value = string.Empty;
                    txt_Branchname.Value = string.Empty;
                    txt_Branchid.Value = string.Empty;
                    ACTIVE.Checked=false;


                }
            }
            AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "BranchDetails.aspx", "Add button clicked", "");
        }





    }
}