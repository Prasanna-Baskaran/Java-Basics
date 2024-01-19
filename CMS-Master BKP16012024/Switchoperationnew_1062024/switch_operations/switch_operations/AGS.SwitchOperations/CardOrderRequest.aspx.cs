using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using System.Data;
using AGS.Utilities;
using Newtonsoft;

namespace AGS.SwitchOperations
{
    public partial class CardOrderRequest : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCR"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    //StrAccessCaption = "P";

                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {

                        userBtns.AccessButtons = StrAccessCaption;
                        hdnAccessCaption.Value = StrAccessCaption;
                        userBtns.ProcessClick += new EventHandler(btnProcess_Click);
                        FunGetResult();
                        hdnFlag.Value = "";
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
            catch (Exception )
            {
                //(new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, Page_Load()", ex.Message, "");
            }
        }
        protected void FunGetResult()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(22, Session["SystemID"].ToString() + "," + Session["BankID"].ToString());

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //if user has accept right
                    if (accessPrefix.Contains("P"))
                    {

                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustomerID", objAdded);
                    }
                    else
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustomerID");
                }
                

            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, FunGetResult()", ex.Message, "");
            }


        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                hdnTransactionDetails.Value = (new ClsCardProductionBAL()).ProcessPendingCardDetails(new ClsCardProductionBO() { UserId = Session["LoginID"].ToString(), CardNo = hdnSelectedCustomers.Value,SystemID=Session["SystemID"].ToString(),BankID=Session["BankID"].ToString() });
                hdnFlag.Value = "1";
                
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, btnProcess_Click()", ex.Message, "");
            }
        }

        protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnSet_Click(object sender, EventArgs e)
        {
            BindBatchNo();
        }
        private void BindBatchNo()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDate.Value))
                {
                    ddlBatch.DataSource = (new ClsCardProductionBAL()).GetBatchNos((new ClsCardProductionBO() { ProcessDate = DateTime.ParseExact(txtDate.Value, "dd/MM/yyyy", null) }));
                    ddlBatch.DataTextField = "BatchNo";
                    ddlBatch.DataValueField = "BatchNo";
                    ddlBatch.DataBind();
                }
                else
                {
                    foreach (ListItem li in ddlBatch.Items)
                    {
                        ddlBatch.Items.Remove(li);
                    }
                }
                ddlBatch.Items.Insert(0, new ListItem() { Selected = true, Text = "--Select--", Value = "0" });
            }
            catch
            {

            }
        }

        protected void hdnFlag_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}