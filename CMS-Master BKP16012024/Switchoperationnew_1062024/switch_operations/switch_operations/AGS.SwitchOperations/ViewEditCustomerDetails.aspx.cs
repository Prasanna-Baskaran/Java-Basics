using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class ViewEditCustomerDetails : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hdnInstaEdit.Value = Session["iInstaEdit"].ToString(); //added for ATPCM-759
            if(ConfigurationManager.AppSettings["OldCardDataIssuerNos"].ToString().Split(',').Contains(Convert.ToString(Session["IssuerNo"])))
            {
                DivOldCardNo.Visible = true;
            }
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ViewEditCustomerDetails, Page_Load()", Ex.ToString(), "");
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
                //if (!string.IsNullOrEmpty(TxtOldCardNo.Value))
                //{
                //    objSearch.OldCardNo = TxtOldCardNo.Value;
                //}


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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, ViewEditCustomerDetails, btnSearch_Click()", Ex.Message, "");
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDtOutPut = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.ID = Convert.ToInt64(hdnTblAuthId.Value);
                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                objSearch.BankCustID = hdnBankCustId.Value;
                objSearch.IntPara = 1;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.OldCardNo = TxtOldCardNo.Value;
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, btnView_Click()", Ex.Message, "");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO _ClsReturnStatusBO = new ClsReturnStatusBO();
                CustomerRegistrationDetails _RegistrationDet = new CustomerRegistrationDetails();
                _RegistrationDet.CardNo = txtSearchCardNo.Value;////added for ATPCM-759
                _RegistrationDet.CIFID = txtCifId.Value;
                _RegistrationDet.CustomerName = txtCustomerName.Value;
                _RegistrationDet.CustomerPreferredName = txtNameOnCard.Value;
                _RegistrationDet.CardPrefix = txtBinPrefix.Value;
                _RegistrationDet.ACID = txtAccountNo.Value;
                _RegistrationDet.CIFCreationDate = txtCIFCreationDate.Value.Replace("/", "");
                _RegistrationDet.Address1 = txtAddress1.Value;
                _RegistrationDet.Address2 = txtAddress2.Value;
                _RegistrationDet.Address3 = txtAddress3.Value;
                _RegistrationDet.City = txtCity.Value;
                _RegistrationDet.State = txtState.Value;
                _RegistrationDet.PinCode = txtPincode.Value;
                _RegistrationDet.Country = txtCountry.Value;
                _RegistrationDet.MothersMaidenName = txtMotherName.Value;
                _RegistrationDet.DOB = DOB.Value.Replace("/", "");
                _RegistrationDet.CountryDialcode = txtCountryCode.Value;
                _RegistrationDet.STDCode = txtStdCode.Value;
                _RegistrationDet.MobileNo = txtMobileNo.Value;
                _RegistrationDet.EmailId = txtEmailId.Value;
                _RegistrationDet.AccountType = tXtAccountType.Value;
                _RegistrationDet.BranchCode = txtBranchCode.Value;
                _RegistrationDet.PANNumber = txtPanNumber.Value;
                _RegistrationDet.ModeOfOperation = "01";
                _RegistrationDet.FourthLineEmbossing = txtForthLineEmbossing.Value;
                _RegistrationDet.Aadhaar = TxtAadharNo.Value;
                _RegistrationDet.Pin_Mailer = "01";
                _RegistrationDet.SystemID = Convert.ToInt32(Session["SystemID"].ToString());
                _RegistrationDet.BankID = Session["BankID"].ToString();
                _RegistrationDet.RequestedBy = Convert.ToString(Session["UserName"]);
                _RegistrationDet.RequestType = "2";
                _RegistrationDet.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                _RegistrationDet.iInstaEdit= Session["iInstaEdit"].ToString();
                _RegistrationDet.AuthId = Convert.ToInt64(hdnTblAuthId.Value);
                _RegistrationDet.OldCardNo = TxtOldCardNo.Value; //added for ATPBF-1355
                DataTable _DtRegDet = new DataTable();
                _DtRegDet = ObjectIntoDataTable(_RegistrationDet);

                _ClsReturnStatusBO = new ClsCustomerMasterBAL().FunSaveCardRequest(_DtRegDet, _RegistrationDet);

                LblMsg.InnerHtml = _ClsReturnStatusBO.Description.ToString();

                if (_ClsReturnStatusBO.Description == "Customer details saved successfully.")
                {
                    hdnResultStatus.Value = "1";
                }
                else 
                {
                    hdnResultStatus.Value = "2";

                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, btnUpdate_Click()", Ex.Message, "");
                hdnResultStatus.Value = "2";
                LblMsg.InnerHtml = "Unexpected error occured.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClsReturnStatusBO _ClsReturnStatusBO = new ClsReturnStatusBO();
        //        CustomerRegistrationDetails _CustomerRegistrationDetails = new CustomerRegistrationDetails();
        //        _CustomerRegistrationDetails.CIFID = txtCifId.Value;
        //        _CustomerRegistrationDetails.CustomerName = txtCustomerName.Value;
        //        _CustomerRegistrationDetails.CustomerPreferredName = txtNameOnCard.Value;
        //        _CustomerRegistrationDetails.CardPrefix = txtBinPrefix.Value;
        //        _CustomerRegistrationDetails.ACID = txtAccountNo.Value;
        //        _CustomerRegistrationDetails.CIFCreationDate = txtCIFCreationDate.Value.Replace("/", "");
        //        _CustomerRegistrationDetails.Address1 = txtAddress1.Value;
        //        _CustomerRegistrationDetails.Address2 = txtAddress2.Value;
        //        _CustomerRegistrationDetails.Address3 = txtAddress3.Value;
        //        _CustomerRegistrationDetails.City = txtCity.Value;
        //        _CustomerRegistrationDetails.State = txtState.Value;
        //        _CustomerRegistrationDetails.PinCode = txtPincode.Value;
        //        _CustomerRegistrationDetails.Country = txtCountry.Value;
        //        _CustomerRegistrationDetails.MothersMaidenName = txtMotherName.Value;
        //        _CustomerRegistrationDetails.DOB = DOB.Value.Replace("/", "");
        //        _CustomerRegistrationDetails.CountryDialcode = txtCountryCode.Value;
        //        _CustomerRegistrationDetails.STDCode = txtStdCode.Value;
        //        _CustomerRegistrationDetails.MobileNo = txtMobileNo.Value;
        //        _CustomerRegistrationDetails.EmailId = txtEmailId.Value;
        //        _CustomerRegistrationDetails.AccountType = tXtAccountType.Value;
        //        _CustomerRegistrationDetails.BranchCode = txtBranchCode.Value;
        //        _CustomerRegistrationDetails.PANNumber = txtPanNumber.Value;
        //        _CustomerRegistrationDetails.ModeOfOperation = "01";
        //        _CustomerRegistrationDetails.FourthLineEmbossing = txtForthLineEmbossing.Value;
        //        _CustomerRegistrationDetails.Aadhaar = TxtAadharNo.Value;
        //        _CustomerRegistrationDetails.Pin_Mailer = "01";
        //        _CustomerRegistrationDetails.SystemID = Convert.ToInt32(Session["SystemID"].ToString());
        //        _CustomerRegistrationDetails.BankID = Session["BankID"].ToString();
        //        _CustomerRegistrationDetails.RequestedBy = Convert.ToInt32(Session["LoginID"]);
        //        _CustomerRegistrationDetails.RequestType = "3";
        //        DataTable _Dt = new DataTable();
        //        _Dt = ObjectIntoDataTable(_CustomerRegistrationDetails);

        //        _ClsReturnStatusBO = new ClsCustomerMasterBAL().FunSaveCardRequest(_Dt, _CustomerRegistrationDetails);

        //        LblMsg.InnerText = _ClsReturnStatusBO.Description.ToString();

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw;
        //    }
        //}

        public DataTable ObjectIntoDataTable(CustomerRegistrationDetails _a)
        {
            //The DataTable to Return
            DataTable result = new DataTable();
            try
            {
                //added for ATPCM-759  start
                if (_a.iInstaEdit == "3")
                {
                    
                    result.Columns.Add("CardNo", typeof(String));
                    result.Columns.Add("AuthId", typeof(Int64));
                }
                //added for ATPCM-759 end

                //Add specific column to datatable
                DataColumn workCol = result.Columns.Add("CIF_ID", typeof(String));
                workCol.AllowDBNull = false;
                workCol.Unique = false;

                result.Columns.Add("CustomerName", typeof(String));
                result.Columns.Add("NameOnCard", typeof(String));
                result.Columns.Add("Bin_Prefix", typeof(String));
                result.Columns.Add("AccountNo", typeof(String));
                result.Columns.Add("AccountOpeningDate", typeof(String));
                result.Columns.Add("CIF_Creation_Date", typeof(String));
                result.Columns.Add("Address1", typeof(String));
                result.Columns.Add("Address2", typeof(String));
                result.Columns.Add("Address3", typeof(String));
                result.Columns.Add("City", typeof(String));
                result.Columns.Add("State", typeof(String));
                result.Columns.Add("PinCode", typeof(String));
                result.Columns.Add("Country", typeof(String));
                result.Columns.Add("Mothers_Name", typeof(String));
                result.Columns.Add("DOB", typeof(String));
                result.Columns.Add("CountryCode", typeof(String));
                result.Columns.Add("STDCode", typeof(String));
                result.Columns.Add("MobileNo", typeof(String));
                result.Columns.Add("EmailID", typeof(String));
                result.Columns.Add("SCHEME_Code", typeof(String));
                result.Columns.Add("BRANCH_Code", typeof(String));
                result.Columns.Add("Entered_Date", typeof(String));
                result.Columns.Add("Verified_Date", typeof(String));
                result.Columns.Add("PAN_No", typeof(String));
                result.Columns.Add("Mode_Of_Operation", typeof(String));
                result.Columns.Add("Fourth_Line_Embossing", typeof(String));
                result.Columns.Add("Aadhar_No", typeof(String));
                result.Columns.Add("Issue_DebitCard", typeof(String));
                result.Columns.Add("Pin_Mailer", typeof(String));
                result.Columns.Add("PGKValue", typeof(String));
                //added for ATPCM-759  start
                if (_a.iInstaEdit == "3")
                {
                    result.Rows.Add(_a.CardNo,_a.AuthId, _a.CIFID, _a.CustomerName, _a.CustomerPreferredName, _a.CardPrefix, _a.ACID, _a.ACOpenDate, _a.CIFCreationDate, _a.Address1, _a.Address2, _a.Address3,
                    _a.City, _a.State, _a.PinCode, _a.Country, _a.MothersMaidenName, _a.DOB, _a.CountryDialcode, _a.STDCode, _a.MobileNo, _a.EmailId, _a.AccountType, _a.BranchCode,
                    _a.EnteredDate, _a.VerifiedDate, _a.PANNumber, _a.ModeOfOperation, _a.FourthLineEmbossing, _a.Aadhaar, "Y", _a.Pin_Mailer, "");
                }
                else {

                    result.Rows.Add(_a.CIFID, _a.CustomerName, _a.CustomerPreferredName, _a.CardPrefix, _a.ACID, _a.ACOpenDate, _a.CIFCreationDate, _a.Address1, _a.Address2, _a.Address3,
                    _a.City, _a.State, _a.PinCode, _a.Country, _a.MothersMaidenName, _a.DOB, _a.CountryDialcode, _a.STDCode, _a.MobileNo, _a.EmailId, _a.AccountType, _a.BranchCode,
                    _a.EnteredDate, _a.VerifiedDate, _a.PANNumber, _a.ModeOfOperation, _a.FourthLineEmbossing, _a.Aadhaar, "Y", _a.Pin_Mailer, "");
                }
                //added for ATPCM-759  end

            }
            catch (Exception ex)
            {
                result = new DataTable();
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, ObjectIntoDataTable()", ex.Message, "");

            }
            //Return the imported data.        
            return result;
        }
    }
}