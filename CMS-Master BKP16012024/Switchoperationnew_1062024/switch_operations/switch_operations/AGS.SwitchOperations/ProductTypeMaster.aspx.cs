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
    public partial class ProductTypeMaster : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "MPT"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;
                        if (!IsPostBack)
                        {
                            FunDropDownFill();
                        }

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
              //  new ClsCommonBAL().FunInsertIntoErrorLog("CS, ProductTypeMaster, Page_Load()", ObjEx.Message, "");
            }
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
                ObjFilter.ProductType = txtProductName.Value;
                ObjFilter.ProductTypeDesc = txtProductDesc.Value;
                ObjFilter.INSTID = Convert.ToInt32(ddlInstitutionID.SelectedValue);
                ObjFilter.BinID = Convert.ToInt32(ddlBin.SelectedValue);
                ObjFilter.CardTypeID = Convert.ToInt32(ddlCardType.SelectedValue);
                ObjFilter.MakerID = Convert.ToInt64(Session["LoginID"]);
                ObjFilter.SystemID = Session["SystemID"].ToString();
                ObjFilter.BankID = Session["BankID"].ToString();
                ObjReturnStatus = new ClsMasterBAL().FunSaveProductType(ObjFilter);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";

                }
                //Get Result
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, ProductTypeMaster, btnSave_Click()", ObjEx.Message, "");
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsMasterBO ObjBin = new ClsMasterBO();
                ObjBin.ID = Convert.ToInt32(hdnID.Value);
                ObjBin.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjBin.FormStatusID = 1;
                ObjReturnStatus = new ClsMasterBAL().FunAccept_RejectProductType(ObjBin);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, ProductTypeMaster, btnAccept_Click()", Ex.Message, "");
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsMasterBO ObjBin = new ClsMasterBO();
                ObjBin.ID = Convert.ToInt32(hdnID.Value);
                ObjBin.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjBin.FormStatusID = 2;
                ObjBin.Remark = txtRejectReson.Text;
                ObjReturnStatus = new ClsMasterBAL().FunAccept_RejectProductType(ObjBin);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                //Get Result
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, ProductTypeMaster, btnReject_Click()", Ex.Message, "");
            }
        }

        protected void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(17, "0,"+Session["BankID"]+","+Session["SystemID"]);
           
          
            AddedTableData[] objAdded = new AddedTableData[1];
            objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "Edit", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funEditClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "producttype", BindTableColumnValueWithAttribute = "ProductType" }, new AGS.Utilities.Attribute() { AttributeName = "instid", BindTableColumnValueWithAttribute = "INST_ID" }, new AGS.Utilities.Attribute() { AttributeName = "binid", BindTableColumnValueWithAttribute = "BIN_ID" }, new AGS.Utilities.Attribute() { AttributeName = "cardtypeid", BindTableColumnValueWithAttribute = "CardType_ID" }, new AGS.Utilities.Attribute() { AttributeName = "productdesc", BindTableColumnValueWithAttribute = "Description" } } };

            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("StatusID,INST_ID,BIN_ID,CardType_ID,ID", objAdded);
        }

        protected void FunDropDownFill()
        {
            try
            {
                DataTable ObjINSTDTOutPut = new DataTable();
                ObjINSTDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(12, Session["BankID"]+","+Session["SystemID"]);
                ddlInstitutionID.DataSource = ObjINSTDTOutPut;
                ddlInstitutionID.DataTextField = "InstitutionID";
                ddlInstitutionID.DataValueField = "ID";
                ddlInstitutionID.DataBind();
                ddlInstitutionID.Items.Insert(0, new ListItem("--Select--", "0"));

                //BIN
                DataTable ObjBinOutPut = new DataTable();
                ObjBinOutPut = new ClsCommonBAL().FunGetCommonDataTable(15, "1,"+Session["BankID"]+","+Session["SystemID"]);
                ddlBin.DataSource = ObjBinOutPut;
                ddlBin.DataTextField = "CardPrefix";
                ddlBin.DataValueField = "ID";
                ddlBin.DataBind();
                ddlBin.Items.Insert(0, new ListItem("--Select--", "0"));

                //CardType
                DataTable ObjCardTypeOutPut = new DataTable();
                ObjCardTypeOutPut = new ClsCommonBAL().FunGetCommonDataTable(14, Session["BankID"] + "," + Session["SystemID"]);
                ddlCardType.DataSource = ObjCardTypeOutPut;
                ddlCardType.DataTextField = "CardType";
                ddlCardType.DataValueField = "ID";
                ddlCardType.DataBind();
                ddlCardType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, ProductTypeMaster, FunDropDownFill()", Ex.Message, "");
            }

        }




    }
}