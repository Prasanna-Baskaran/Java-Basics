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
    public partial class ViewCustomerDetails : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                string OptionNeumonic = "VCD"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (!ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }

            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ViewCustomerDetails, Page_Load()", Ex.Message, "");
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.MobileNo = txtSearchMobile.Value;

                objSearch.CustomerName = txtSearchName.Value;

                if (!string.IsNullOrEmpty(txtSearchCardNo.Value))
                {
                    objSearch.CardNo = txtSearchCardNo.Value;
                }
                if (!string.IsNullOrEmpty(txtAccountNoView.Value))
                {
                    objSearch.AccountNo = txtAccountNoView.Value;
                }


                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();

                ObjDTOutPut = new ClsCustomerMasterBAL().FunViewCardDetails(objSearch);


                if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //For user those having accept right
                    if (accessPrefix.Contains("C"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[2];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustID" } } };
                        objAdded[1] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustID" }, new AGS.Utilities.Attribute() { AttributeName = "BankCustId", BindTableColumnValueWithAttribute = "CustomerID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustID,FormStatusID", objAdded);
                        hdnShowAuthButton.Value = "1";
                    }
                    //for Other User
                    else
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "BankCustId", BindTableColumnValueWithAttribute = "Customer ID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustID,FormStatusID", objAdded);
                        hdnShowAuthButton.Value = "";
                    }
                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ViewCustomerDetails, btnSearch_Click()", Ex.Message, "");
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDtOutPut = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.ID = Convert.ToInt64(hdnTblAuthId.Value);
                objSearch.BankCustID = hdnBankCustId.Value;
                objSearch.IntPara = 1;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                
                ObjDtOutPut = new ClsCustomerMasterBAL().FunViewCardDetails(objSearch);
                txtCifId.Value = ObjDtOutPut.Rows[0][1].ToString();
                txtCustomerName.Value = ObjDtOutPut.Rows[0][2].ToString();
                txtNameOnCard.Value = ObjDtOutPut.Rows[0][3].ToString();
                txtBinPrefix.Value = ObjDtOutPut.Rows[0][4].ToString();
                txtAccountNo.Value = ObjDtOutPut.Rows[0][5].ToString();
                txtCIFCreationDate.Value = ObjDtOutPut.Rows[0][7].ToString();
                txtAddress1.Value = ObjDtOutPut.Rows[0][8].ToString();
                txtAddress2.Value = ObjDtOutPut.Rows[0][9].ToString();
                txtAddress3.Value = ObjDtOutPut.Rows[0][10].ToString();
                txtCity.Value = ObjDtOutPut.Rows[0][11].ToString();
                txtState.Value = ObjDtOutPut.Rows[0][12].ToString();
                txtPincode.Value = ObjDtOutPut.Rows[0][13].ToString();
                txtCountry.Value = ObjDtOutPut.Rows[0][14].ToString();
                txtMotherName.Value = ObjDtOutPut.Rows[0][15].ToString();
                DOB.Value = ObjDtOutPut.Rows[0][16].ToString();
                txtCountryCode.Value = ObjDtOutPut.Rows[0][17].ToString();
                txtStdCode.Value = ObjDtOutPut.Rows[0][18].ToString();
                txtMobileNo.Value = ObjDtOutPut.Rows[0][19].ToString();
                txtEmailId.Value = ObjDtOutPut.Rows[0][20].ToString();
                tXtAccountType.Value = ObjDtOutPut.Rows[0][21].ToString();
                txtBranchCode.Value = ObjDtOutPut.Rows[0][22].ToString();
                txtPanNumber.Value = ObjDtOutPut.Rows[0][23].ToString();
                txtForthLineEmbossing.Value = ObjDtOutPut.Rows[0][25].ToString();
                TxtAadharNo.Value = ObjDtOutPut.Rows[0][26].ToString();
                hdnFlagId.Value = "1"; //for view customer form and hide  serch  div

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunOperation()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewCustomerDetails, btnView_Click()", Ex.Message, "");
            }
        }

    }
}