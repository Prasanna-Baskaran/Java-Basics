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
using AGS.SwitchOperations.Common;
using System.Web.Services;
using System.Web.Script.Serialization;
using AGS.SwitchOperations.Validator;
namespace AGS.SwitchOperations
{
    public partial class ViewEditCustDetailAll : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnInstaEdit.Value = Session["iInstaEdit"] != null ? Session["iInstaEdit"].ToString() : ""; //added for ATPCM-759
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="First Name", Value= txtSearchName.Value, MinLength = 5, MaxLength = 100 },
                    new ValidatorAttr { Name="Mobile No", Value= txtSearchMobile.Value, MinLength = 10, MaxLength = 10, Numeric=true },
                    new ValidatorAttr { Name="Account No", Value= txtAccountNoView.Value, MinLength = 3, MaxLength = 100, Numeric=true },
                    new ValidatorAttr { Name="Card No", Value= txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric=true},
                    new ValidatorAttr { Name="NIC No", Value= txtNIC.Value, MinLength = 3,MaxLength = 10, AlphaNumeric=true },
                    new ValidatorAttr { Name="Name On Card", Value= txtsearchNameOnCard.Value, MinLength = 3,MaxLength = 10, Regex="^[a-zA-Z0-9 ]*$" },
                };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnValidMsg','ValidateMsgDiv','" + msg + "')", true);
                }
                else
                {
                    DataTable ObjDTBlockDet = new DataTable();
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
                    if (!string.IsNullOrEmpty(txtsearchNameOnCard.Value))
                    {
                        objSearch.NameOnCard = txtsearchNameOnCard.Value;
                    }
                    if (!string.IsNullOrEmpty(txtCIF.Value))
                    {
                        objSearch.BankCustID = txtCIF.Value;
                    }
                    if (!string.IsNullOrEmpty(txtNIC.Value))
                    {
                        objSearch.NIC = txtNIC.Value;
                    }


                    objSearch.SystemID = Session["SystemID"].ToString();
                    objSearch.BankID = Session["BankID"].ToString();
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();

                    //ObjDTBlockDet = new ClsCustomerMasterBAL().FunCheckBlockCards(objSearch);
                    //if (ObjDTBlockDet.Rows.Count > 0)
                    //{
                    //    if (ObjDTBlockDet.Rows[0]["Code"].ToString() == "1")
                    //    {
                    //        LblResult.InnerHtml = ObjDTBlockDet.Rows[0]["OutputDescription"].ToString();
                    //        return;
                    //    }
                    //}

                    ObjDTOutPut = new ClsCustomerMasterBAL().FunViewCardDetails(objSearch);
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        //For user those having accept right
                        if (accessPrefix.Contains("C"))
                        {
                            AddedTableData[] objAdded = new AddedTableData[2];
                            objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustID" } } };
                            objAdded[1] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustID" }, new AGS.Utilities.Attribute() { AttributeName = "BankCustId", BindTableColumnValueWithAttribute = "CustomerID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "CardNo", BindTableColumnValueWithAttribute = "CardNo" } } };
                            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustID,FormStatusID", objAdded);
                            hdnShowAuthButton.Value = "1";
                        }
                        //for Other User
                        else
                        {
                            AddedTableData[] objAdded = new AddedTableData[1];
                            objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "ID", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "BankCustId", BindTableColumnValueWithAttribute = "Customer ID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "CardNo", BindTableColumnValueWithAttribute = "CardNo" } } };
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
                FunBindDDL();
                DataTable ObjDtOutPut = new DataTable();
                DataTable ObjdtAPIdata = new DataTable();
                DataTable ObjDtDB = new DataTable();

                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.ID = Convert.ToInt64(hdnTblAuthId.Value);
                objSearch.BankCustID = hdnBankCustId.Value;
                txtCardNo.Value = objSearch.CardNo = hdnCardNo.Value;
                objSearch.IntPara = 1;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();
                if (!string.IsNullOrEmpty(txtCIF.Value))
                {
                    objSearch.BankCustID = txtCIF.Value;
                }
                if (!string.IsNullOrEmpty(txtNIC.Value))
                {
                    objSearch.NIC = txtNIC.Value;
                }
                ObjDtOutPut = new ClsCustomerMasterBAL().FunViewCardDetails(objSearch);
                //if (!string.IsNullOrEmpty(txtCIF.Value) && string.IsNullOrEmpty(txtNIC.Value))
                //{
                //    objSearch.tranType = "SDBCUSTDETAILS";
                //}
                //else if (string.IsNullOrEmpty(txtCIF.Value) && !string.IsNullOrEmpty(txtNIC.Value))
                //{
                //    objSearch.tranType = "SDBNICDETAILS";
                //}
                //if (!string.IsNullOrEmpty(objSearch.tranType))
                //{
                //    objSearch.LoginId = Convert.ToInt32(Session["LoginID"]);
                //    ObjDtOutPut = new ClsCustomerMasterBAL().FunGetCardAPIdata(objSearch);
                //    objSearch.SDBAPIURL = ObjDtOutPut.Rows[0]["URL"].ToString();
                //    objSearch.PARA = ObjDtOutPut.Rows[0]["PARA"].ToString();
                //    objSearch.USER = ObjDtOutPut.Rows[0]["USER"].ToString();
                //    objSearch.TOKEN = ObjDtOutPut.Rows[0]["TOKEN"].ToString();
                //}

                if (ObjDtOutPut.Rows.Count > 0)
                {

                    txtCifId.Value = ObjDtOutPut.Rows[0][1].ToString();
                    txtCustomerName.Value = ObjDtOutPut.Rows[0][2].ToString();
                    txtNameOnCard.Value = ObjDtOutPut.Rows[0][3].ToString();
                    //txtBinPrefix.Value = ObjDtDB.Rows[0][4].ToString();
                    //txtAccountNo.Value = ObjDtDB.Rows[0][5].ToString();

                    //if (ObjdtAPIdata.Rows[0][4].ToString().Trim().Length == 8)
                    //{
                    //    txtDOB.Value = ObjdtAPIdata.Rows[0][4].ToString().Substring(0, 2) + "/" + ObjdtAPIdata.Rows[0][4].ToString().Substring(2, 2) + "/" + ObjdtAPIdata.Rows[0][4].ToString().Substring(4, 4);
                    //    //DOB.Value = ObjDtOutPut.Rows[0][16].ToString().Substring(6, 2) + "/" + ObjDtOutPut.Rows[0][16].ToString().Substring(4, 2) + "/" + ObjDtOutPut.Rows[0][16].ToString().Substring(0, 4);
                    //}
                    //else
                    //{
                    //    txtCIFCreationDate.Value = ObjdtAPIdata.Rows[0][4].ToString();
                    //}

                    //txtBinPrefix.Value = ObjDtOutPut.Rows[0][4].ToString();
                    //txtAccountNo.Value = ObjDtOutPut.Rows[0][5].ToString();
                    //txtCIFCreationDate.Value = ObjDtOutPut.Rows[0][7].ToString();
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
                    //tXtAccountType.Value = ObjDtOutPut.Rows[0][21].ToString();
                    txtBranchCode.Value = ObjDtOutPut.Rows[0][22].ToString();
                    //txtPanNumber.Value = ObjDtOutPut.Rows[0][23].ToString();
                    //txtForthLineEmbossing.Value = ObjDtOutPut.Rows[0][25].ToString();
                    TxtAadharNo.Value = ObjDtOutPut.Rows[0][26].ToString();
                    dropGender.SelectedIndex = 0;

                    hdnFlagId.Value = "1";

                    DataTable ObjDTOutPut = new DataTable();
                    DataTable DtAccDet = new DataTable();

                    DtAccDet.Columns.Add(new DataColumn("ACCOUNTNO"));
                    DataRow dr = DtAccDet.NewRow();
                    //dr["ACCOUNTNO"] = ;
                    DtAccDet.Rows.Add(dr);

                    DtAccDet.Columns.Add(new DataColumn("AccQualifier"));
                    DtAccDet.AcceptChanges();

                    foreach (DataRow row in DtAccDet.Rows)
                    {
                        row["ACCOUNTNO"] = "<input type='text' class='form-control' maxlength='20' runat='server' name='Ac No' id='txtAcNo' disabled='disabled' value='" + ObjDtOutPut.Rows[0][5].ToString() + "' />";
                        row["AccQualifier"] = "<select class='form-control' disabled='disabled'><option value='1'>Primary</option><option value='2'>Secondary</option><option value='3'>Tertiary</option><option value='4'>Quaternary</option></select>";

                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "Search result", txtSearchCardNo.Value, txtCIF.Value, "", "", ObjDtOutPut.Rows[0][5].ToString(), txtNIC.Value, "Searched", "1");
                    }
                    DataTable DtAccDetFinal = DtAccDet.DefaultView.ToTable(false, "ACCOUNTNO", "AccQualifier");
                    AddedTableData[] objAdded = new AddedTableData[1];
                    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, disabled = true, attributes = new AGS.Utilities.Attribute[] { new Utilities.Attribute() { AttributeName="checked" } } };
                    //hdnAccDetails.Value = DtAccDet.ToHtmlTableString("[ACCOUNTNO],[CFPRNM],[MEMOBAL],[DMDOPN],[CURBAL],[DMBRCH],[DMTYPE],[AccQualifier]", objAdded);
                    hdnAccDetails.Value = DtAccDetFinal.ToHtmlTableString("[ACCOUNTNO],[AccQualifier]", objAdded);

                    // objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ACCOUNTNO" }, new AGS.Utilities.Attribute() { AttributeName = "ACCOUNTNO", BindTableColumnValueWithAttribute = "ACCOUNTNO" } } };
                }

                else
                {
                    hdnResultStatus.Value = "2";
                    LblMsg.InnerHtml = "No Details found for given CIF.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg();", true);
                    return;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunOperation()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, btnView_Click()", Ex.Message, "");
            }
        }

        public static String ConvertFromJulian(int m_JulianDate)
        {
            try
            {
                int jDate = m_JulianDate;
                int day = jDate % 1000; int year = (jDate - day) / 1000;
                var date1 = new DateTime(year, 1, 1);
                var result = date1.AddDays(day - 1);
                return result.ToString("dd_MM_yyyy").Replace("_", "/");
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, btnView_Click() Error DOB in julian date to normal", ex.ToString(), "");
                return null;
            }
        }

        protected void FunBindDDL()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCustomerMasterBAL().FunGetLanguage(Convert.ToInt32(Session["BankID"]));
                DDLLanguage.DataSource = ObjDTOutPut;
                DDLLanguage.DataTextField = "custLanguage";
                DDLLanguage.DataValueField = "id";
                DDLLanguage.DataBind();

            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, FunBindDDL()", ex.Message, "");
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
                //_RegistrationDet.CardPrefix = txtBinPrefix.Value;
                //_RegistrationDet.ACID = txtAccountNo.Value;
                //_RegistrationDet.CIFCreationDate = txtCIFCreationDate.Value.Replace("/", "");
                _RegistrationDet.Address1 = txtAddress1.Value;
                _RegistrationDet.Address2 = txtAddress2.Value;
                _RegistrationDet.Address3 = txtAddress3.Value;
                _RegistrationDet.City = txtCity.Value;
                _RegistrationDet.State = txtState.Value;
                _RegistrationDet.PinCode = txtPincode.Value;
                _RegistrationDet.Country = txtCountry.Value;
                //_RegistrationDet.MothersMaidenName = txtMotherName.Value;
                _RegistrationDet.DOB = DOB.Value.Replace("/", "");
                _RegistrationDet.CountryDialcode = txtCountryCode.Value;
                _RegistrationDet.STDCode = txtStdCode.Value;
                _RegistrationDet.MobileNo = txtMobileNo.Value;
                _RegistrationDet.EmailId = txtEmailId.Value;
                //_RegistrationDet.AccountType = tXtAccountType.Value;
                _RegistrationDet.BranchCode = txtBranchCode.Value;
                //_RegistrationDet.PANNumber = txtPanNumber.Value;
                _RegistrationDet.ModeOfOperation = "01";
                //_RegistrationDet.FourthLineEmbossing = txtForthLineEmbossing.Value;
                _RegistrationDet.Aadhaar = TxtAadharNo.Value;
                _RegistrationDet.Pin_Mailer = "01";
                _RegistrationDet.SystemID = Convert.ToInt32(Session["SystemID"].ToString());
                _RegistrationDet.BankID = Session["BankID"].ToString();
                _RegistrationDet.RequestedBy = Session["UserName"].ToString();
                _RegistrationDet.RequestType = "2";
                _RegistrationDet.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                _RegistrationDet.iInstaEdit = Session["iInstaEdit"].ToString();
                _RegistrationDet.AuthId = Convert.ToInt64(hdnTblAuthId.Value);

                //Aded by nitin on08/11/2019 start
                _RegistrationDet.Gender = dropGender.Value.Substring(0, 1);
                _RegistrationDet.CustLanguage = DDLLanguage.SelectedItem.Text;

                //_RegistrationDet.MarketSeqment = txtMarketSeqment.Value;
                //_RegistrationDet.OldNICNo = txtOldNICNo.Value;
                //_RegistrationDet.DrivingLicNo = txtDrivingLicNo.Value;
                //_RegistrationDet.DrivingLicExpDT = txtDrivingLicExpDT.Value;
                //_RegistrationDet.PassportNo = txtPassportNo.Value;
                //_RegistrationDet.PassportExpDt = txtDrivingLicExpDT.Value;
                //_RegistrationDet.CustomerClassification = txtCustomerClassificatn.Value;
                //_RegistrationDet.CustomerType = txtCustomerType.Value;
                //_RegistrationDet.BusinessRegNo = txtBusinessRegNo.Value;
                //_RegistrationDet.Age = txtAge.Value;

                //Aded by nitin on08/11/2019 end
                _RegistrationDet.UserBranchCode = Session["BranchCode"].ToString();

                DataTable _DtRegDet = new DataTable();
                _DtRegDet = ObjectIntoDataTable(_RegistrationDet);

                _ClsReturnStatusBO = new ClsCustomerMasterBAL().FunSaveCardRequest(_DtRegDet, _RegistrationDet);

                LblMsg.InnerHtml = _ClsReturnStatusBO.Description.ToString();



                if (_ClsReturnStatusBO.Description == "Customer details saved successfully." && FunSetResult("Insert", "1", _ClsReturnStatusBO.CardREqID, _RegistrationDet))
                {
                    hdnResultStatus.Value = "1";
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "View Modify Customer Details", "Customer detail updated", _RegistrationDet.CardNo, _RegistrationDet.CIFID, _RegistrationDet.CustomerName, _RegistrationDet.MobileNo, _RegistrationDet.ACID, _RegistrationDet.OldNICNo, "Update", "1");
                }
                else
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "View Modify Customer Details", "Failed to update customer detail", _RegistrationDet.CardNo, _RegistrationDet.CIFID, _RegistrationDet.CustomerName, _RegistrationDet.MobileNo, _RegistrationDet.ACID, _RegistrationDet.OldNICNo, "Update", "0");
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
        public static string ValidateCustID(CustomerRegistrationDetails _RegistrationDet)
        {
            String Response = "";
            try
            {

                CustSearchFilter objSearch = new CustSearchFilter();

                GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                APIResponseObject ObjAPIResponse = new APIResponseObject();
                ConfigDataObject ObjData = new ConfigDataObject();

                DataTable _dtCardAPISourceIdDetails = new DataTable();
                DataTable dtcard = new DataTable();
                objSearch.APIRequest = "CardDetails";
                objSearch.CardNo = _RegistrationDet.CardNo;
                objSearch.CIF = _RegistrationDet.CIFID;
                objSearch.IssuerNo = Convert.ToString(HttpContext.Current.Session["IssuerNo"]);
                objSearch.RequestTypeID = 0;


                dtcard = new ClsCardMasterBAL().GetCardByCIFID(objSearch);

                if (dtcard.Rows.Count > 0)
                {
                    objSearch.CardNo = Convert.ToString(dtcard.Rows[0][0]);

                    _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);
                    ObjData.IssuerNo = Convert.ToString(HttpContext.Current.Session["IssuerNo"]);
                    ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                    ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                    ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);
                    ObjData.IsCardDetailsSearch = true;

                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { objSearch.CardNo });

                    _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);
                    
                    //new ClsCommonBAL().FunInsertIntoErrorLog("CS, == Switch response "+ ObjAPIResponse.Status,"" , "");

                    if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        //new ClsCommonBAL().FunInsertIntoErrorLog("CS, == Switch response CIF " + ObjAPIResponse.CifId, "", "");

                        if (_RegistrationDet.CIFID == ObjAPIResponse.CifId)
                        {
                            if (string.IsNullOrWhiteSpace(ObjAPIResponse.HoldRspCode) || ObjAPIResponse.HoldRspCode.ToString()=="05") // changed by nitin jadhav on 24/11/2021 added 05 card status check
                            {
                                Response = "Cust id " + _RegistrationDet.CIFID + " is already mapped with card no " + objSearch.CardNo;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, ValidateCustID()", Ex.ToString(), "");

            }
            return Response;
        }



        [WebMethod]
        public static string btnUpdate(CustomerRegistrationDetails _RegistrationDet)
        {
            String Result;


            try
            {
                Result=ValidateCustID(_RegistrationDet);
                if (String.IsNullOrEmpty(Result))
                {



                ClsReturnStatusBO _ClsReturnStatusBO = new ClsReturnStatusBO();

                _RegistrationDet.ModeOfOperation = "01";


                _RegistrationDet.Pin_Mailer = "01";
                _RegistrationDet.SystemID = Convert.ToInt32(HttpContext.Current.Session["SystemID"]);
                _RegistrationDet.BankID = HttpContext.Current.Session["BankID"].ToString();
                _RegistrationDet.RequestedBy = HttpContext.Current.Session["UserName"].ToString();
                _RegistrationDet.RequestType = "2";
                _RegistrationDet.IssuerNo = Convert.ToInt32(HttpContext.Current.Session["IssuerNo"]);
                _RegistrationDet.iInstaEdit = HttpContext.Current.Session["iInstaEdit"].ToString();
                _RegistrationDet.DOB = _RegistrationDet.DOB.Replace("/", "");

                _RegistrationDet.ACID = _RegistrationDet.AllSelectedValues.Split('|')[0];

                _RegistrationDet.UserBranchCode = HttpContext.Current.Session["BranchCode"].ToString();

                DataTable _DtRegDet = new DataTable();
                _DtRegDet = ObjectIntoDataTable(_RegistrationDet);

                _ClsReturnStatusBO = new ClsCustomerMasterBAL().FunSaveCardRequest(_DtRegDet, _RegistrationDet);




                //LblMsg.InnerHtml = _ClsReturnStatusBO.Description.ToString();


                // Insert in Account linking table after successfull saved customer updation request changed by Nitin Jadhav on 17/11/2021
                if (_ClsReturnStatusBO.Description == "Customer details saved successfully."
                    // && FunSetResult("Insert", "1", _ClsReturnStatusBO.CardREqID, _RegistrationDet)
                    )
                {
                        Result = "1|" + _ClsReturnStatusBO.Description.ToString();// _ClsReturnStatusBO.Description.ToString();
                    //hdnResultStatus.Value = "1";
                    new ClsCommonDAL().FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "View Modify Customer Details", "Customer detail updated", _RegistrationDet.CardNo, _RegistrationDet.CIFID, _RegistrationDet.CustomerName, _RegistrationDet.MobileNo, _RegistrationDet.ACID, _RegistrationDet.OldNICNo, "Update", "1");
                }
                else
                {
                    new ClsCommonDAL().FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "View Modify Customer Details", "Failed to update customer detail", _RegistrationDet.CardNo, _RegistrationDet.CIFID, _RegistrationDet.CustomerName, _RegistrationDet.MobileNo, _RegistrationDet.ACID, _RegistrationDet.OldNICNo, "Update", "0");

                    Result = "2|" + _ClsReturnStatusBO.Description.ToString();
                    //hdnResultStatus.Value = "2";

                }


                }
                else
                {
                    new ClsCommonDAL().FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "View Modify Customer Details", "Failed to update customer detail", _RegistrationDet.CardNo, _RegistrationDet.CIFID, _RegistrationDet.CustomerName, _RegistrationDet.MobileNo, _RegistrationDet.ACID, _RegistrationDet.OldNICNo, "Update", "0");

                    Result = "2|" + Result;
                    //hdnResultStatus.Value = "2";

                }


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ViewEditCustomerDetails, btnUpdate_Click()", Ex.ToString(), "");
                Result = "2";
                //hdnResultStatus.Value = "2";
                //LblMsg.InnerHtml = "Unexpected error occured.";
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }


            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize((Object)Result);



            //return Result;
        }
        public static Boolean FunSetResult(String mode, string Linkingflag, long CardREqID, CustomerRegistrationDetails _RegistrationDet)
        {
            try
            {
                ClsGeneratePrepaidCardBO ObjPrepaid;

                ObjPrepaid = new ClsGeneratePrepaidCardBO();
                ObjPrepaid.RequestIDs = _RegistrationDet.AllSelectedValues.Split(',');
                ObjPrepaid.UserID = Convert.ToString(HttpContext.Current.Session["UserName"]);
                ObjPrepaid.IssuerNo = Convert.ToString(HttpContext.Current.Session["IssuerNo"]);
                ObjPrepaid.Mode = mode;
                ObjPrepaid.CardREqID = CardREqID;
                ObjPrepaid.UserBranchCode = HttpContext.Current.Session["BranchCode"].ToString();

                DataTable _dtRequest = new DataTable();
                _dtRequest.Columns.Add("AccountNo", typeof(string));
                _dtRequest.Columns.Add("CustomerId", typeof(string));
                _dtRequest.Columns.Add("CardNo", typeof(string));
                _dtRequest.Columns.Add("NICNo", typeof(string));
                _dtRequest.Columns.Add("Linkingflag", typeof(string));
                _dtRequest.Columns.Add("AccountType", typeof(string));
                _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                foreach (string sr in ObjPrepaid.RequestIDs)
                {
                    _dtRequest.Rows.Add(new Object[] { sr.Split('|')[0], _RegistrationDet.CIFID, _RegistrationDet.CardNo, _RegistrationDet.Aadhaar, Linkingflag, "10", sr.Split('|')[1] });
                }

                //var duplicates = _dtRequest.AsEnumerable().GroupBy(r => r["AccountQualifier"]).Where(gr => gr.Count() > 1).ToList().Any();

                ObjPrepaid.DtBulkData = _dtRequest;
                DataSet ObjDSOutPut = new DataSet();
                ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GetSetAccountLinkrequest(ObjPrepaid);

                if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                    {

                        //LblMsg.InnerHtml = "<span style='color:green'>Account " + ((Linkingflag == "1") ? "linking" : "delinking") + " request send successfully!</span>";
                        return true;
                    }
                    else
                    {

                        //hdnResultStatus.Value = "2";
                        //LblMsg.InnerHtml = "<span style='color:red'>" + Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][1]) + "</span>";
                        return false;
                    }
                }
                else
                {
                    // hdnResultStatus.Value = "2";
                    //  LblMsg.InnerHtml = "<span style='color:red'>Error Occured</span>";
                    return false;
                }

            }
            catch (Exception ex)
            {
                //hdnResultStatus.Value = "2";
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingRequest,FunSetResult()", ex.Message, Convert.ToString(HttpContext.Current.Session["IssuerNo"]));
                /// LblMsg.InnerHtml = "Error Occured";
                return false;
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);

        }



        public static DataTable ObjectIntoDataTable(CustomerRegistrationDetails _a)
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

                //Added by nitin on 008/11/2019
                result.Columns.Add("MarketSeqment", typeof(String));
                result.Columns.Add("OldNICNo", typeof(String));
                result.Columns.Add("DrivingLicNo", typeof(String));
                result.Columns.Add("DrivingLicExpDT", typeof(String));
                result.Columns.Add("PassportNo", typeof(String));
                result.Columns.Add("PassportExpDt", typeof(String));
                result.Columns.Add("Gender", typeof(String));
                result.Columns.Add("CustomerClassification", typeof(String));
                result.Columns.Add("CustomerType", typeof(String));
                result.Columns.Add("BusinessRegNo", typeof(String));
                result.Columns.Add("Age", typeof(String));
                result.Columns.Add("CustLanguage", typeof(String));
                result.Columns.Add("AccCategory", typeof(String));


                // end by nitin

                //added for ATPCM-759  start
                if (_a.iInstaEdit == "3")
                {
                    result.Rows.Add(_a.CardNo, _a.AuthId, _a.CIFID, _a.CustomerName, _a.CustomerPreferredName, _a.CardPrefix, _a.ACID, _a.ACOpenDate, _a.CIFCreationDate, _a.Address1, _a.Address2, _a.Address3,
                    _a.City, _a.State, _a.PinCode, _a.Country, _a.MothersMaidenName, _a.DOB, _a.CountryDialcode, _a.STDCode, _a.MobileNo, _a.EmailId, _a.AccountType, _a.BranchCode,
                    _a.EnteredDate, _a.VerifiedDate, _a.PANNumber, _a.ModeOfOperation, _a.FourthLineEmbossing, _a.Aadhaar, "Y", _a.Pin_Mailer, "", _a.MarketSeqment, _a.OldNICNo, _a.DrivingLicNo,
                    _a.DrivingLicExpDT, _a.PassportNo, _a.PassportExpDt, _a.Gender, _a.CustomerClassification, _a.CustomerType, _a.BusinessRegNo, _a.Age, _a.CustLanguage,_a.AccCategory);
                }
                else
                {

                    result.Rows.Add(_a.CIFID, _a.CustomerName, _a.CustomerPreferredName, _a.CardPrefix, _a.ACID, _a.ACOpenDate, _a.CIFCreationDate, _a.Address1, _a.Address2, _a.Address3,
                    _a.City, _a.State, _a.PinCode, _a.Country, _a.MothersMaidenName, _a.DOB, _a.CountryDialcode, _a.STDCode, _a.MobileNo, _a.EmailId, _a.AccountType, _a.BranchCode,
                    _a.EnteredDate, _a.VerifiedDate, _a.PANNumber, _a.ModeOfOperation, _a.FourthLineEmbossing, _a.Aadhaar, "Y", _a.Pin_Mailer, ""
                    //Added by nitin on 008/11/2019
                    , _a.MarketSeqment, _a.OldNICNo, _a.DrivingLicNo,
                    _a.DrivingLicExpDT, _a.PassportNo, _a.PassportExpDt, _a.Gender, _a.CustomerClassification, _a.CustomerType, _a.BusinessRegNo, _a.Age, _a.CustLanguage, _a.AccCategory);
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