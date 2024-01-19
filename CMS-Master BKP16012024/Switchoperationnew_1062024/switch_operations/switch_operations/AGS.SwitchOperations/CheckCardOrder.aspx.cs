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

namespace AGS.SwitchOperations
{
    public partial class CheckCardOrder : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCCO"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    //StrAccessCaption = "C";

                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        userBtns.AccessButtons = StrAccessCaption;
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        //if user has accept right
                        if (accessPrefix.Contains("C"))
                        {                           
                            userBtns.AcceptClick += new EventHandler(btnAccept_Click);
                        }
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
               // (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, Page_Load()", ex.Message, "");
            }
        }
        protected void FunGetResult()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(23, Session["SystemID"].ToString() + "," + Session["BankID"].ToString());

                if (ObjDTOutPut.Rows.Count > 0)
                {

                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //if user has accept right
                    if (accessPrefix.Contains("C"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "Code" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("Code", objAdded);
                        
                    }
                    else
                    {
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("Code");
                    }

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, FunGetResult()", ex.Message, "");
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                FunGetResult();
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, btnSearch_Click()", ex.Message, "");
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                hdnTransactionDetails.Value = (new ClsCardProductionBAL()).AuthoriseeCardDetails(new ClsCardProductionBO() { UserId = Session["LoginID"].ToString(), CardNo = hdnSelectedCustomers.Value, SystemID = Session["SystemID"].ToString(), BankID = Session["BankID"].ToString() });
                hdnFlag.Value = "1";
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOrderRequest, btnProcess_Click()", ex.Message, "");
            }
        }

    }
}