using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
namespace AGS.SwitchOperations
{
    public partial class SearchCustomers : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "ESC"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {

                        userBtns.AccessButtons = StrAccessCaption;
                        //userBtns.AddClick += new EventHandler(BtnAdd_click);
                        //userBtns.PreviousClick += new EventHandler(BtnPrevious_click);
                        userBtns.EditClick += new EventHandler(btnEdit_Click);
                        userBtns.UpdateClick += new EventHandler(btnUpdate_Click);
                        userBtns.AcceptClick += new EventHandler(btnAccept_Click);
                        userBtns.RejectClick += new EventHandler(btnReject_Click);
                        userBtns.CancelClick += new EventHandler(Page_Load);

                        if (!IsPostBack)
                        {
                            //Generix.fillDropDown(ref litCardType, Generix.getData("dbo.TblCardType", "CardTypeName,CardTypeID", "", "", "CardTypeName", 3));
                            //FillApplicationNoDropDown();
                            FillDropDown();

                        }

                        LblResult.InnerHtml = "";
                    }
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
              //  (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, Page_Load()", Ex.Message, "");
            }
        }




        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.MobileNo = txtSearchMobile.Value;
                //if (ddlApplicationNo.SelectedValue.ToString() != "0")
                //{
                //    objSearch.CustomerID = Convert.ToInt64(ddlApplicationNo.SelectedValue);
                //}
                objSearch.ApplicationNo = txtSearchApplNo.Value.Trim();

                objSearch.CustomerName = txtSearchName.Value;
                if (!string.IsNullOrEmpty(txtSearchDOB_AD.Value))
                {
                    objSearch.DOB_AD = Convert.ToDateTime(txtSearchDOB_AD.Value);
                }
                if(!string.IsNullOrEmpty(txtSearchCreatedDate.Value))
                {
                    objSearch.CreatedDate = Convert.ToDateTime(txtSearchCreatedDate.Value);
                }
                if(!string.IsNullOrEmpty(txtSearchCardNo.Value))
                {
                    objSearch.CardNo = txtSearchCardNo.Value;
                }
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();

                ObjDTOutPut = new ClsCustomerMasterBAL().FunSearchCustomer(objSearch);
              

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
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustID" }, new AGS.Utilities.Attribute() { AttributeName = "BankCustId", BindTableColumnValueWithAttribute = "Customer ID" },new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" } } };
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustID,FormStatusID", objAdded);
                        hdnShowAuthButton.Value = "";
                    }

                    LblResult.InnerHtml = "";
                }
                else
                {
                    LblResult.InnerHtml = "No Record Found";
                }
                //End 21/02/17
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnSearch_Click()", Ex.Message, "");
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ObjDsOutPut = new DataSet();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.CustomerID = hdnCustomerID.Value;
                objSearch.BankCustID = hdnBankCustId.Value;
                objSearch.IntPara = 1;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                ObjDsOutPut = new ClsCustomerMasterBAL().FunGetCustomerDetails(objSearch);
                DataTable DtCustomer = ObjDsOutPut.Tables[0];
                DataTable DtOtherCard = new DataTable();
                if (ObjDsOutPut.Tables.Count > 1)
                {
                    DtOtherCard = ObjDsOutPut.Tables[1];
                }


                txtFirstName.Value = DtCustomer.Rows[0][5].ToString();
                txtMiddleName.Value = DtCustomer.Rows[0][6].ToString();
                txtLastName.Value = DtCustomer.Rows[0][7].ToString();
                //if (DtCustomer.Rows[0][9].ToString() != "1900-01-01")
                //{
                //    DOB_BS.Value = DtCustomer.Rows[0][9].ToString();
                //}
                if (DtCustomer.Rows[0][10].ToString() != "1900-01-01")
                {
                    DOB_AD.Value = DtCustomer.Rows[0][10].ToString();
                }
                txtNationality.Value = DtCustomer.Rows[0][11].ToString();
                Gender.SelectedValue = DtCustomer.Rows[0][12].ToString();
                MaritalStatus.SelectedValue = DtCustomer.Rows[0][13].ToString();
                txtPassportNo.Value = DtCustomer.Rows[0][2].ToString();
                txtIssuedate.Value = DtCustomer.Rows[0][14].ToString();
                //ResidenceType.SelectedValue = DtCustomer.Rows[0][15].ToString();
                //txtResidenceDesc.Value = DtCustomer.Rows[0][5].ToString();
                //VehicleType.SelectedValue = DtCustomer.Rows[0][16].ToString();
                //txtVehicleTypeDesc.Value = DtCustomer.Rows[0][17].ToString();
                //txtVehicleNo.Value = DtCustomer.Rows[0][18].ToString();
                txtMobileNo.Value = DtCustomer.Rows[0][8].ToString();
                DdlCardType.SelectedValue = DtCustomer.Rows[0][19].ToString();
                txtSpouseName.Value = DtCustomer.Rows[0][20].ToString();
                txtMotherName.Value = DtCustomer.Rows[0][21].ToString();
                txtFatherName.Value = DtCustomer.Rows[0][22].ToString();
                txtGrandFatherName.Value = DtCustomer.Rows[0][23].ToString();

                hdnFormStatusID.Value = DtCustomer.Rows[0][24].ToString();
                txtPO_Box_P.Value = DtCustomer.Rows[0][28].ToString();
                txtHouseNo_P.Value = DtCustomer.Rows[0][29].ToString();
                txtStreetName_P.Value = DtCustomer.Rows[0][30].ToString();
                txtTole_P.Value = DtCustomer.Rows[0][31].ToString();
                txtWardNo_P.Value = DtCustomer.Rows[0][32].ToString();
                txtCity_P.Value = DtCustomer.Rows[0][33].ToString();
                txtDistrict_P.Value = DtCustomer.Rows[0][34].ToString();
                txtPhone1_P.Value = DtCustomer.Rows[0][35].ToString();
                txtPhone2_P.Value = DtCustomer.Rows[0][36].ToString();
                txtFAX_P.Value = DtCustomer.Rows[0][37].ToString();
                txtMobile_P.Value = DtCustomer.Rows[0][38].ToString();
                txtEmail_P.Value = DtCustomer.Rows[0][39].ToString();
                IsSameAsPermAddrs.SelectedValue = DtCustomer.Rows[0][40].ToString();


                txtPO_Box_C.Value = DtCustomer.Rows[0][41].ToString();
                txtHouseNo_C.Value = DtCustomer.Rows[0][42].ToString();
                txtStreetName_C.Value = DtCustomer.Rows[0][43].ToString();
                txtTole_C.Value = DtCustomer.Rows[0][44].ToString();
                txtWardNo_C.Value = DtCustomer.Rows[0][45].ToString();
                txtCity_C.Value = DtCustomer.Rows[0][46].ToString();
                txtDistrict_C.Value = DtCustomer.Rows[0][47].ToString();
                txtPhone1_C.Value = DtCustomer.Rows[0][48].ToString();
                txtPhone2_C.Value = DtCustomer.Rows[0][49].ToString();
                txtFAX_C.Value = DtCustomer.Rows[0][50].ToString();
                txtMobile_C.Value = DtCustomer.Rows[0][51].ToString();
                txtEmail_C.Value = DtCustomer.Rows[0][52].ToString();


                txtPO_Box_O.Value = DtCustomer.Rows[0][53].ToString();
                txtStreetName_O.Value = DtCustomer.Rows[0][54].ToString();
                txtCity_O.Value = DtCustomer.Rows[0][55].ToString();
                txtDistrict_O.Value = DtCustomer.Rows[0][61].ToString();
                txtPhone1_O.Value = DtCustomer.Rows[0][56].ToString();
                txtPhone2_O.Value = DtCustomer.Rows[0][57].ToString();
                txtFAX_O.Value = DtCustomer.Rows[0][58].ToString();
                txtMobile_O.Value = DtCustomer.Rows[0][59].ToString();
                txtEmail_O.Value = DtCustomer.Rows[0][60].ToString();

                DdlProffessionType.SelectedValue = DtCustomer.Rows[0][62].ToString();
                ddlOrgzTypeID.SelectedValue = DtCustomer.Rows[0][63].ToString();

                //txtOrgzTypeDesc.Value = DtCustomer.Rows[0][5].ToString();
                txtProffesion.Value = DtCustomer.Rows[0][93].ToString();
                //txtPreviousEmployment.Value = DtCustomer.Rows[0][5].ToString();
                txtDesignation.Value = DtCustomer.Rows[0][66].ToString();
                txtBusinessType.Value = DtCustomer.Rows[0][68].ToString();
                txtWorkSince.Value = DtCustomer.Rows[0][69].ToString();


                txtAnnualSal.Value = DtCustomer.Rows[0][70].ToString();
                txtAnnualIncentive.Value = DtCustomer.Rows[0][71].ToString();
                txtAnnualIncome.Value = DtCustomer.Rows[0][72].ToString();
                txtRentalIncome.Value = DtCustomer.Rows[0][73].ToString();
                txtAgriculture.Value = DtCustomer.Rows[0][74].ToString();
                txtIncome.Value = DtCustomer.Rows[0][75].ToString();
                txtTotalIncome.Value = DtCustomer.Rows[0][76].ToString();
                //IsOtherCreditCard.SelectedValue = DtCustomer.Rows[0][77].ToString();
                txtPrincipleBankName.Value = DtCustomer.Rows[0][78].ToString();
                DdlAccountType.SelectedValue = DtCustomer.Rows[0][79].ToString();
                IsPrabhuAccount.SelectedValue = DtCustomer.Rows[0][81].ToString();

                txtAccountNo.Value = DtCustomer.Rows[0][82].ToString();
                if (DtCustomer.Rows[0][83].ToString() == "1")
                {
                    StatementBy.SelectedValue = "1";
                }
                else if (DtCustomer.Rows[0][84].ToString() == "1")
                {
                    StatementBy.SelectedValue = "2";
                }
                txtEmailForStatement.Value = DtCustomer.Rows[0][85].ToString();
                txtRefName1.Value = DtCustomer.Rows[0][86].ToString();
                txtDesignation1_R.Value = DtCustomer.Rows[0][87].ToString();
                txtPhone1_R.Value = DtCustomer.Rows[0][88].ToString();
                txtRefName2.Value = DtCustomer.Rows[0][89].ToString();
                txtDesignation2_R.Value = DtCustomer.Rows[0][90].ToString();
                txtPhone2_R.Value = DtCustomer.Rows[0][91].ToString();
                DdlProffessionType.SelectedValue = DtCustomer.Rows[0][93].ToString();
                DdlProduct.SelectedValue = DtCustomer.Rows[0][94].ToString();
                ddlINSTID.SelectedValue = DtCustomer.Rows[0][95].ToString();
                txtNameOnCard.Value = DtCustomer.Rows[0][99].ToString();

                

                //KYC Documents
                string SignatureFileName = DtCustomer.Rows[0][96].ToString();
                string PhotoFileName = DtCustomer.Rows[0][97].ToString();
                string IDProofFileName = DtCustomer.Rows[0][98].ToString();


                if (!string.IsNullOrEmpty(SignatureFileName) && !string.IsNullOrEmpty(PhotoFileName) && !string.IsNullOrEmpty(IDProofFileName))
                {
                    string KYCPath = ConfigurationManager.AppSettings["KYCFilePath"].ToString();
                    string KYCDocPath = KYCPath + "/" + objSearch.CustomerID + "/" + SignatureFileName;


                    byte[] byteArray = null;
                    //Signature
                    if (File.Exists(KYCDocPath))
                    {
                        using (FileStream fs = new FileStream(KYCDocPath, FileMode.Open, FileAccess.Read))
                        {
                            byteArray = new byte[fs.Length];
                            int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
                            if (byteArray != null && byteArray.Length > 0)
                            {
                                // Convert the byte into image
                                string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                                SignatureImg.Src = "data:image/png;base64," + base64String;
                            }
                        }
                    }
                    //Photo
                    byteArray = null;
                    KYCDocPath = KYCPath + "/" + objSearch.CustomerID + "/" + PhotoFileName;
                    if (File.Exists(KYCDocPath))
                    {
                        using (FileStream fs = new FileStream(KYCDocPath, FileMode.Open, FileAccess.Read))
                        {
                            byteArray = new byte[fs.Length];
                            int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
                            if (byteArray != null && byteArray.Length > 0)
                            {
                                // Convert the byte into image
                                string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                                PhotoImg.Src = "data:image/png;base64," + base64String;
                            }
                        }
                    }
                    //ID proof
                    byteArray = null;
                    KYCDocPath = KYCPath + "/" + objSearch.CustomerID + "/" + IDProofFileName;
                    if (File.Exists(KYCDocPath))
                    {
                        using (FileStream fs = new FileStream(KYCDocPath, FileMode.Open, FileAccess.Read))
                        {
                            byteArray = new byte[fs.Length];
                            int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
                            if (byteArray != null && byteArray.Length > 0)
                            {
                                // Convert the byte into image
                                string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                                IDProofImg.Src = "data:image/png;base64," + base64String;
                            }
                        }
                    }

                }
                else
                {
                    IDProofImg.Src = "";
                    PhotoImg.Src = "";
                    SignatureImg.Src = "";
                }

                DataTable ObjDTOutPut = new DataTable();

                //string Bankcustid = hdnBankCustId.Value;
                ObjDTOutPut = new ClsCardMasterBAL().FunGetCustCardDetailsFromSwitch(objSearch);
                hdnCardDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                if (ObjDTOutPut.Rows.Count > 0)
                {
                    
                    LblResult.InnerHtml = "";
                }
                else
                {
                    hdnCardDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found";
                }
                //other credit card details
                //if (DtOtherCard.Rows.Count > 0)
                //{
                //    GrdOtherCard.DataSource = DtOtherCard;
                //    GrdOtherCard.DataBind();

                //}

                hdnFlagId.Value = "1"; //for view customer form and hide  serch  div

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunOperation()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnView_Click()", Ex.Message, "");
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {

                ClsCustomer ObjCustomer = new ClsCustomer();
                //ObjCustomer.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                ObjCustomer.ReqCustomerId = hdnCustomerID.Value;

                ObjCustomer.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjCustomer.FormStatusID = 1;
                //Start 21/02/2017
                //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                //ObjReturnStatus = new ClsCustomerMasterBAL().FunAccept_RejectCustomer(ObjCustomer);

                //LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                using (DataTable ObjDtOutput = new ClsCustomerMasterBAL().FunAccept_RejectCustomerForm(ObjCustomer))
                {
                    //For Checker
                    AddedTableData[] objAdded = new AddedTableData[2];
                    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" } } };
                    objAdded[1] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" } } };
                    hdnTransactionDetails.Value = ObjDtOutput.ToHtmlTableString("FormStatusID", objAdded);
                    hdnShowAuthButton.Value = "1";
                }
                //End 21/02/2017

            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnAccept_Click()", Ex.Message, "");
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {

            try
            {
                //Start 21/02/2017
                //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsCustomer ObjCustomer = new ClsCustomer();
                ObjCustomer.ReqCustomerId = hdnCustomerID.Value;
                ObjCustomer.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjCustomer.FormStatusID = 2;
                ObjCustomer.Remark = txtRejectReson.Text;
                //ObjReturnStatus = new ClsCustomerMasterBAL().FunAccept_RejectCustomer(ObjCustomer);
                //LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                using (DataTable ObjDtOutput = new ClsCustomerMasterBAL().FunAccept_RejectCustomerForm(ObjCustomer))
                {
                    //For Checker
                    AddedTableData[] objAdded = new AddedTableData[2];
                    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" } } };
                    objAdded[1] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "VIEW", cssClass = "btn btn-primary", hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" }, new AGS.Utilities.Attribute() { AttributeName = "formstatusid", BindTableColumnValueWithAttribute = "FormStatusID" } } };
                    hdnTransactionDetails.Value = ObjDtOutput.ToHtmlTableString("FormStatusID", objAdded);
                    hdnShowAuthButton.Value = "1";
                }

                //End 21/02/2017
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnReject_Click()", Ex.Message, "");
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //start sheetal to hide lables photo,signature,idproof when click on edit button
                //lblPhoto.Visible = false;
                //lblSignature.Visible = false;
                //lblIdproof.Visible = false;
                //end
                DataSet ObjDsOutPut = new DataSet();
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.CustomerID = hdnCustomerID.Value;
                objSearch.IntPara = 1;
                objSearch.SystemID = Session["SystemID"].ToString();
                //Start Diksha 29/06
                objSearch.BankID = Session["BankID"].ToString();
                ObjDsOutPut = new ClsCustomerMasterBAL().FunGetCustomerDetails(objSearch);

                DataTable DtCustomer = ObjDsOutPut.Tables[0];
                DataTable DtOtherCard = new DataTable();
                if (ObjDsOutPut.Tables.Count > 1)
                {
                    DtOtherCard = ObjDsOutPut.Tables[1];
                }

                Gender.SelectedValue = DtCustomer.Rows[0][12].ToString();
                MaritalStatus.SelectedValue = DtCustomer.Rows[0][13].ToString();
                //ResidenceType.SelectedValue = DtCustomer.Rows[0][15].ToString();
                //VehicleType.SelectedValue = DtCustomer.Rows[0][16].ToString();
                DdlCardType.SelectedValue = DtCustomer.Rows[0][19].ToString();
                IsSameAsPermAddrs.SelectedValue = DtCustomer.Rows[0][40].ToString();
                DdlProffessionType.SelectedValue = DtCustomer.Rows[0][62].ToString();
                ddlOrgzTypeID.SelectedValue = DtCustomer.Rows[0][63].ToString();
                //IsOtherCreditCard.SelectedValue = DtCustomer.Rows[0][77].ToString();
                DdlAccountType.SelectedValue = DtCustomer.Rows[0][79].ToString();
                IsPrabhuAccount.SelectedValue = DtCustomer.Rows[0][81].ToString();
                if (DtCustomer.Rows[0][83].ToString() == "1")
                {
                    StatementBy.SelectedValue = "1";
                }
                else if (DtCustomer.Rows[0][84].ToString() == "1")
                {
                    StatementBy.SelectedValue = "2";
                }
                DdlProduct.SelectedValue = DtCustomer.Rows[0][94].ToString();
                ddlINSTID.SelectedValue = DtCustomer.Rows[0][95].ToString();

                hdnFlagId.Value = "2";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunOperation()", true);

            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnEdit_Click()", Ex.Message, "");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsCustomer ObjCustomer = new ClsCustomer();
                ClsCustomerAddress ObjCustAddrs = new ClsCustomerAddress();
                ClsCustOccupationDtl ObjCustOccupation = new ClsCustOccupationDtl();
                ClsOtherCreditCardDtl ObjOtherCreditCard = new ClsOtherCreditCardDtl();
                LblMsg.InnerText = "";
                //personal info
                ObjCustomer.FirstName = txtFirstName.Value.ToString().Trim();
                ObjCustomer.MiddleName = txtMiddleName.Value.ToString().Trim();
                ObjCustomer.LastName = txtLastName.Value.ToString().Trim();
                //if (!string.IsNullOrEmpty(DOB_BS.Value))
                //{
                //    ObjCustomer.DOB_BS = Convert.ToDateTime(DOB_BS.Value);
                //}
                if (!string.IsNullOrEmpty(DOB_AD.Value))
                {
                    ObjCustomer.DOB_AD = Convert.ToDateTime(DOB_AD.Value);
                }
                ObjCustomer.Nationality = txtNationality.Value.ToString().Trim();
                //ObjCustomer.GenderID = Request.Form["Gender"];
                ObjCustomer.GenderID = Gender.SelectedValue;
                //ObjCustomer.MaritalStatusID = Request.Form["MaritalStatus"];
                ObjCustomer.MaritalStatusID = MaritalStatus.SelectedValue;
                ObjCustomer.PassportNo_CitizenShipNo = txtPassportNo.Value.ToString().Trim();
                ObjCustomer.IssueDate_District = txtIssuedate.Value.ToString().Trim();
                //ObjCustomer.ResidenceTypeID = Request.Form["ResidenceType"];
                //ObjCustomer.ResidenceTypeID = ResidenceType.SelectedValue;
                //ObjCustomer.ResidenceDesc = txtResidenceDesc.Value.ToString().Trim();
                //ObjCustomer.VehicleTypeID = Request.Form["VehicleType"];
                //ObjCustomer.VehicleTypeID = VehicleType.SelectedValue;
                //ObjCustomer.VehicleType = txtVehicleTypeDesc.Value.ToString().Trim();
                //ObjCustomer.VehicleNo = txtVehicleNo.Value.ToString().Trim();
                ObjCustomer.MobileNo = txtMobileNo.Value.ToString().Trim();
                ObjCustomer.MakerID = Convert.ToInt64(Session["LoginID"]);
                //ObjCustomer.CardTypeID = Request.Form["CardType"];
                ObjCustomer.CardTypeID = DdlCardType.SelectedValue;
                ObjCustomer.SpouseName = txtSpouseName.Value.ToString().Trim();
                ObjCustomer.MotherName = txtMotherName.Value.ToString().Trim();
                ObjCustomer.FatherName = txtFatherName.Value.ToString().Trim();
                ObjCustomer.GrandFatherName = txtGrandFatherName.Value.ToString().Trim();
                ObjCustomer.NameOnCard = txtNameOnCard.Value.ToString().Trim();


                //permenant address
                ObjCustAddrs.PO_Box_P = txtPO_Box_P.Value.ToString().Trim();
                ObjCustAddrs.HouseNo_P = txtHouseNo_P.Value.ToString().Trim();
                ObjCustAddrs.StreetName_P = txtStreetName_P.Value.ToString().Trim();
                ObjCustAddrs.Tole_P = txtTole_P.Value.ToString().Trim();
                ObjCustAddrs.WardNo_P = txtWardNo_P.Value.ToString().Trim();
                ObjCustAddrs.City_P = txtCity_P.Value.ToString().Trim();
                ObjCustAddrs.District_P = txtDistrict_P.Value.ToString().Trim();
                ObjCustAddrs.Phone1_P = txtPhone1_P.Value.ToString().Trim();
                ObjCustAddrs.Phone2_P = txtPhone2_P.Value.ToString().Trim();
                ObjCustAddrs.FAX_P = txtFAX_P.Value.ToString().Trim();
                ObjCustAddrs.Mobile_P = txtMobile_P.Value.ToString().Trim();
                ObjCustAddrs.Email_P = txtEmail_P.Value.ToString().Trim();
                if (!string.IsNullOrEmpty(IsSameAsPermAddrs.SelectedValue))
                {
                    ObjCustAddrs.IsSameAsPermAddr = Convert.ToByte(IsSameAsPermAddrs.SelectedValue);
                }

                //coresspondance addrs
                ObjCustAddrs.PO_Box_C = txtPO_Box_C.Value.ToString().Trim();
                ObjCustAddrs.HouseNo_C = txtHouseNo_C.Value.ToString().Trim();
                ObjCustAddrs.StreetName_C = txtStreetName_C.Value.ToString().Trim();
                ObjCustAddrs.Tole_C = txtTole_C.Value.ToString().Trim();
                ObjCustAddrs.WardNo_C = txtWardNo_C.Value.ToString().Trim();
                ObjCustAddrs.City_C = txtCity_C.Value.ToString().Trim();
                ObjCustAddrs.District_C = txtDistrict_C.Value.ToString().Trim();
                ObjCustAddrs.Phone1_C = txtPhone1_C.Value.ToString().Trim();
                ObjCustAddrs.Phone2_C = txtPhone2_C.Value.ToString().Trim();
                ObjCustAddrs.FAX_C = txtFAX_C.Value.ToString().Trim();
                ObjCustAddrs.Mobile_C = txtMobile_C.Value.ToString().Trim();
                ObjCustAddrs.Email_C = txtEmail_C.Value.ToString().Trim();

                //Office address
                ObjCustAddrs.PO_Box_O = txtPO_Box_O.Value.ToString().Trim();
                ObjCustAddrs.StreetName_O = txtStreetName_O.Value.ToString().Trim();
                ObjCustAddrs.City_O = txtCity_O.Value.ToString().Trim();
                ObjCustAddrs.District_O = txtDistrict_O.Value.ToString().Trim();
                ObjCustAddrs.Phone1_O = txtPhone1_O.Value.ToString().Trim();
                ObjCustAddrs.Phone2_O = txtPhone2_O.Value.ToString().Trim();
                ObjCustAddrs.FAX_O = txtFAX_O.Value.ToString().Trim();
                ObjCustAddrs.Mobile_O = txtMobile_O.Value.ToString().Trim();
                ObjCustAddrs.Email_O = txtEmail_O.Value.ToString().Trim();

                //occupation                 
                ObjCustOccupation.ProfessionTypeID = DdlProffessionType.SelectedValue;
                ObjCustOccupation.OrganizationTypeID = ddlOrgzTypeID.SelectedValue;
                //ObjCustOccupation.OrganizationTypeDesc = txtOrgzTypeDesc.Value.ToString().Trim();
                ObjCustOccupation.ProfessionType = txtProffesion.Value.ToString().Trim();
                ObjCustOccupation.PreviousEmployment = txtPreviousEmployment.Value.ToString().Trim();
                ObjCustOccupation.Designation = txtDesignation.Value.ToString().Trim();
                ObjCustOccupation.BusinessType = txtBusinessType.Value.ToString().Trim();
                ObjCustOccupation.WorkSince = txtWorkSince.Value.ToString().Trim();

                //Salary
                ObjCustOccupation.AnnualSalary = txtAnnualSal.Value.ToString().Trim();
                ObjCustOccupation.AnnualIncentive = txtAnnualIncentive.Value.ToString().Trim();
                ObjCustOccupation.AnnualBuisnessIncome = txtAnnualIncome.Value.ToString().Trim();
                ObjCustOccupation.RentalIncome = txtRentalIncome.Value.ToString().Trim();
                ObjCustOccupation.Agriculture = txtAgriculture.Value.ToString().Trim();
                ObjCustOccupation.Income = txtIncome.Value.ToString().Trim();
                ObjCustOccupation.TotalAnnualIncome = txtTotalIncome.Value.ToString().Trim();



                ObjCustOccupation.PrincipalBankName = txtPrincipleBankName.Value.ToString().Trim();
                ObjCustOccupation.AccountTypeID = DdlAccountType.SelectedValue;
                //ObjCustOccupation.AccountTypeDesc = txtAccountTypeDesc.Value.ToString().Trim();
                if (!string.IsNullOrEmpty(IsPrabhuAccount.SelectedValue))
                {
                    ObjCustOccupation.IsPrabhuBankAcnt = Convert.ToByte(IsPrabhuAccount.SelectedValue);
                }
                ObjCustOccupation.PrabhuBankAccountNo = txtAccountNo.Value.ToString().Trim();
                ObjCustOccupation.PrabhuBankBranch = txtPrabhuBranch.Value.ToString().Trim();
                //ObjCustOccupation.IsCollectStatement = Convert.ToByte(Request.Form["CollectStatement"]);
                //ObjCustOccupation.IsEmailStatemnt = Convert.ToByte(Request.Form["EmailStatement"]);

                if (!string.IsNullOrEmpty(StatementBy.SelectedValue))
                {
                    if ((StatementBy.SelectedValue) == "1")
                    {
                        ObjCustOccupation.IsCollectStatement = 1;
                        ObjCustOccupation.IsEmailStatemnt = 0;
                    }
                    else if ((StatementBy.SelectedValue) == "2")
                    {
                        ObjCustOccupation.IsEmailStatemnt = 1;
                        ObjCustOccupation.IsCollectStatement = 0;
                    }
                }
                else
                {
                    ObjCustOccupation.IsCollectStatement = 0;
                    ObjCustOccupation.IsEmailStatemnt = 0;
                }

                ObjCustOccupation.EmailForStatement = txtEmailForStatement.Value.ToString().Trim();

                ObjCustOccupation.ReffName1 = txtRefName1.Value.ToString().Trim();
                ObjCustOccupation.ReffDesignation1 = txtDesignation1_R.Value.ToString().Trim();
                ObjCustOccupation.ReffPhoneNo1 = txtPhone1_R.Value.ToString().Trim();
                ObjCustOccupation.ReffName2 = txtRefName2.Value.ToString().Trim();
                ObjCustOccupation.ReffDesignation2 = txtDesignation2_R.Value.ToString().Trim();
                ObjCustOccupation.ReffPhoneNo2 = txtPhone2_R.Value.ToString().Trim();

                //Other CreditCard details pending
                //ObjCustOccupation.IsOtherCreditCard = Convert.ToByte(Request.Form["IsOtherCreditCard"]);
                //if (!string.IsNullOrEmpty(IsOtherCreditCard.SelectedValue))
                //{
                //    ObjCustOccupation.IsOtherCreditCard = Convert.ToByte(IsOtherCreditCard.SelectedValue);
                //}

                //DataTable tbl = GrdOtherCard.DataSource as DataTable;
                DataTable dtOtherCreditCard = new DataTable();
                // check view state is not null  
                //if (ViewState["TempOtherCard"] != null)
                //{
                //    dtOtherCreditCard = (DataTable)ViewState["TempOtherCard"];
                //    if (dtOtherCreditCard.Rows[0][0].ToString() == "")
                //    {
                //        dtOtherCreditCard.Rows[0].Delete();
                //        dtOtherCreditCard.AcceptChanges();
                //    }

                //}

                ObjCustomer.IntPara = 1;  //for update
                ObjCustomer.CustomerID = Convert.ToInt32(hdnCustomerID.Value);
                ObjCustomer.ProductTypeID = Convert.ToInt32(DdlProduct.SelectedValue);
                ObjCustomer.INSTID = Convert.ToInt32(ddlINSTID.SelectedValue);

                //KYC documents
                if (SignatureUpload.PostedFile != null && SignatureUpload.PostedFile.FileName != "")
                {
                    ObjCustomer.Signature = SignatureUpload.PostedFile;
                }
                if (PhotoUpload.PostedFile != null && PhotoUpload.PostedFile.FileName != "")
                {
                    ObjCustomer.Photo = PhotoUpload.PostedFile;
                }
                if (IDProofUpload.PostedFile != null && IDProofUpload.PostedFile.FileName != "")
                {
                    ObjCustomer.IDProof = IDProofUpload.PostedFile;
                }


                ObjReturnStatus = new ClsCustomerMasterBAL().FunSaveCustomer(ObjCustomer, ObjCustOccupation, ObjCustAddrs, dtOtherCreditCard);



                if (ObjReturnStatus.Code == 0)
                {
                    //clear input controls
                    hdnResultStatus.Value = "1";
                    Gender.SelectedIndex = -1;
                    MaritalStatus.SelectedIndex = -1;
                    //ResidenceType.SelectedIndex = -1;
                    //IsOtherCreditCard.SelectedIndex = -1;
                    DdlAccountType.SelectedIndex = -1;
                    IsPrabhuAccount.SelectedIndex = -1;
                    StatementBy.SelectedIndex = -1;
                    IsSameAsPermAddrs.SelectedIndex = -1;
                    //VehicleType.SelectedIndex = 0;
                    DdlProffessionType.SelectedIndex = 0;
                    DdlCardType.SelectedIndex = 0;
                    //GrdOtherCard.DataSource = new DataTable();
                    //GrdOtherCard.DataBind();
                }
                else
                {
                    hdnResultStatus.Value = "2";
                }

                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, SearchCustomer, btnUpdate_Click()", Ex.Message, "");
            }

        }

        //protected void FillApplicationNoDropDown()
        //{

        //    try
        //    {

        //        DataTable ObjDTOutPut = new DataTable();
        //        ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(8, "");
        //        ddlApplicationNo.DataSource = ObjDTOutPut;
        //        ddlApplicationNo.DataTextField = "ApplicationFormNo";
        //        ddlApplicationNo.DataValueField = "CustomerID";
        //        ddlApplicationNo.DataBind();
        //        ddlApplicationNo.Items.Insert(0, new ListItem("--Select--", "0"));
        //    }
        //    catch (Exception Ex)
        //    {
        //        new ClsCommonBAL().FunInsertIntoErrorLog("CS, SearchCustomer, FillApplicationNoDropDown()", Ex.Message, "");
        //    }

        //}

        protected void FillDropDown()
        {

            try
            {
                //CardType
                DataTable ObjCardTypeDTOutPut = new DataTable();
                ObjCardTypeDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(4, "");
                DdlCardType.DataSource = ObjCardTypeDTOutPut;
                DdlCardType.DataTextField = "CardTypeName";
                DdlCardType.DataValueField = "CardTypeID";
                DdlCardType.DataBind();
                DdlCardType.Items.Insert(0, new ListItem("--Select--", "0"));

                //ProductType
                DataTable ObjProdTypeDTOutPut = new DataTable();
                ObjProdTypeDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(17,"1," + Session["BankID"] + "," + Session["SystemID"]);
                DdlProduct.DataSource = ObjProdTypeDTOutPut;
                DdlProduct.DataTextField = "ProductType";
                DdlProduct.DataValueField = "ID";
                DdlProduct.DataBind();
                DdlProduct.Items.Insert(0, new ListItem("--Select--", "0"));

                //InstitutionID
                DataTable ObjInstDTOutput = new DataTable();
                ObjInstDTOutput = new ClsCommonBAL().FunGetCommonDataTable(12,Session["BankID"]+","+Session["SystemID"]);
                ddlINSTID.DataSource = ObjInstDTOutput;
                ddlINSTID.DataTextField = "InstitutionID";
                ddlINSTID.DataValueField = "ID";
                ddlINSTID.DataBind();
                ddlINSTID.Items.Insert(0, new ListItem("--Select--", "0"));

                //Account Type 
                DataTable ObjAccTypeDTOutput = new DataTable();
                ObjAccTypeDTOutput = new ClsCommonBAL().FunGetCommonDataTable(27, Session["SystemID"] + "," + Session["BankID"]);
                DdlAccountType.DataSource = ObjAccTypeDTOutput;
                DdlAccountType.DataTextField = "AccountTypeName";
                DdlAccountType.DataValueField = "AccountTypeID";
                DdlAccountType.DataBind();
                DdlAccountType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, SearchCustomer, FillDropDown()", Ex.Message, "");
            }
        }
    }



}



