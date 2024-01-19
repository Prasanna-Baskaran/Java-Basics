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
    public partial class CardTypeMaster : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string OptionNeumonic = "MCT"; //unique optionneumonic from database
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
            catch (Exception )
            {
               // new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardTypeMaster, Page_Load()", ObjEx.Message, "");
            }
        }


        public void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(14, Session["BankID"]+","+Session["SystemID"]);
            //            int RoleID = Convert.ToInt16(Session["UserRoleID"]);
            //            hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
            AddedTableData[] objAdded = new AddedTableData[1];
            objAdded[0] = new AddedTableData()
            {
                control = AGS.Utilities.Controls.Button,
                buttonName = "Edit",
                cssClass = "btn btn-primary"
                                  ,
                hideColumnName = true,
                events = new Event[] { new Event() { EventName = "onclick", EventValue = "funEditClick($(this));" } }
                                  ,
                attributes = new AGS.Utilities.Attribute[]
                                  { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "ID" }

                        , new AGS.Utilities.Attribute() { AttributeName = "cardtype", BindTableColumnValueWithAttribute = "CardType" }

                        , new AGS.Utilities.Attribute() { AttributeName = "cardtypedesc", BindTableColumnValueWithAttribute = "Description" } }
            };

            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("ID", objAdded);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsMasterBO ObjFilter = new ClsMasterBO();

                if (!string.IsNullOrEmpty(hdnID.Value))
                {
                    ObjFilter.ID = Convert.ToInt16(hdnID.Value);
                }
                ObjFilter.InstitutionID = txtINSTID.Value;
                ObjFilter.INSTDesc = txtINSTDesc.Value;

                ObjFilter.IntPara = 1;  //for Customer type master
                ObjFilter.SystemID = Session["SystemID"].ToString();
                ObjFilter.BankID = Session["BankID"].ToString();
                ObjReturnStatus = new ClsMasterBAL().FunSaveMasterDetails(ObjFilter);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";
                    //Get Result
                    //DataTable ObjDTOutPut = new DataTable();
                    //ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(14, "");
                    //int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                    //hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
                    FunGetResult();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardTypeMaster, btnSave_Click()", ObjEx.Message, "");
            }
        }
    }
}