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
    public partial class CustomerRegistration : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "ECR"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        userBtns.AccessButtons = StrAccessCaption;
                        userBtns.SubmitClick += new EventHandler(btnSubmit_Click);

                        if (!IsPostBack)
                        {

                            FillDropDown();

                        }
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
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CustomerRegistration, Page_Load()", Ex.Message, "");
            }


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ClsCustomer ObjCustomer = new ClsCustomer();
                ClsCustomerAddress ObjCustAddrs = new ClsCustomerAddress();
                ClsCustOccupationDtl ObjCustOccupation = new ClsCustOccupationDtl();
                ClsOtherCreditCardDtl ObjOtherCreditCard = new ClsOtherCreditCardDtl();
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                LblMsg.InnerText = "";
                //personal info
                ObjCustomer.FirstName = txtFirstName.Value.ToString().Trim();
                ObjCustomer.MiddleName = txtMiddleName.Value.ToString().Trim();
                ObjCustomer.LastName = txtLastName.Value.ToString().Trim();

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
                ObjCustomer.MobileNo = txtMobileNo.Value.ToString().Trim();
                ObjCustomer.MakerID = Convert.ToInt64(Session["LoginID"]);
                //ObjCustomer.CardTypeID = Request.Form["CardType"];
                ObjCustomer.CardTypeID = DdlCardType.SelectedValue;
                ObjCustomer.SpouseName = txtSpouseName.Value.ToString().Trim();
                ObjCustomer.MotherName = txtMotherName.Value.ToString().Trim();
                ObjCustomer.FatherName = txtFatherName.Value.ToString().Trim();
                ObjCustomer.GrandFatherName = txtGrandFatherName.Value.ToString().Trim();
                ObjCustomer.ProductTypeID = Convert.ToInt32(DdlProduct.SelectedValue);
                ObjCustomer.INSTID = Convert.ToInt32(ddlINSTID.SelectedValue);
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
                //ObjCustAddrs.Email_P = txtEmail_P.Text.ToString().Trim();
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
                if (!string.IsNullOrEmpty(IsPrabhuAccount.SelectedValue))
                {
                    ObjCustOccupation.IsPrabhuBankAcnt = Convert.ToByte(IsPrabhuAccount.SelectedValue);
                }
                else
                {
                    ObjCustOccupation.IsPrabhuBankAcnt = 0;
                }


                ObjCustOccupation.PrabhuBankAccountNo = txtAccountNo.Value.ToString().Trim();
                ObjCustOccupation.PrabhuBankBranch = txtPrabhuBranch.Value.ToString().Trim();

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

                //Refference
                ObjCustOccupation.ReffName1 = txtRefName1.Value.ToString().Trim();
                ObjCustOccupation.ReffDesignation1 = txtDesignation1_R.Value.ToString().Trim();
                ObjCustOccupation.ReffPhoneNo1 = txtPhone1_R.Value.ToString().Trim();
                ObjCustOccupation.ReffName2 = txtRefName2.Value.ToString().Trim();
                ObjCustOccupation.ReffDesignation2 = txtDesignation2_R.Value.ToString().Trim();
                ObjCustOccupation.ReffPhoneNo2 = txtPhone2_R.Value.ToString().Trim();
                //Other Credit card details
                DataTable dtOtherCreditCard = new DataTable();

                //KYC documents
                if (SignatureUpload.PostedFile != null)
                {
                    ObjCustomer.Signature = SignatureUpload.PostedFile;
                }
                if (PhotoUpload.PostedFile != null)
                {
                    ObjCustomer.Photo = PhotoUpload.PostedFile;
                }
                if (IDProofUpload.PostedFile != null)
                {
                    ObjCustomer.IDProof = IDProofUpload.PostedFile;
                }
                ObjCustomer.SystemID = Session["SystemID"].ToString();
                ObjCustomer.BankID = Session["BankID"].ToString();

                ObjReturnStatus = new ClsCustomerMasterBAL().FunSaveCustomer(ObjCustomer, ObjCustOccupation, ObjCustAddrs, dtOtherCreditCard);

                if (ObjReturnStatus.Code == 0)
                {
                    //clear input controls
                    hdnResultStatus.Value = "1";
                    Gender.SelectedIndex = -1;
                    MaritalStatus.SelectedIndex = -1;
                    DdlAccountType.SelectedIndex = -1;
                    IsPrabhuAccount.SelectedIndex = -1;
                    StatementBy.SelectedIndex = -1;
                    IsSameAsPermAddrs.SelectedIndex = -1;
                    DdlProffessionType.SelectedIndex = 0;
                    DdlCardType.SelectedIndex = 0;
                }
                else
                {
                    hdnResultStatus.Value = "0";
                }

                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CustomerRegistration, btnSubmit_Click()", Ex.Message, "");
                LblMsg.InnerText = "Customer is not saved";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

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
                ObjProdTypeDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(17, "1," + Session["BankID"] + "," + Session["SystemID"]);
                DdlProduct.DataSource = ObjProdTypeDTOutPut;
                DdlProduct.DataTextField = "ProductType";
                DdlProduct.DataValueField = "ID";
                DdlProduct.DataBind();
                DdlProduct.Items.Insert(0, new ListItem("--Select--", "0"));

                //InstitutionID
                DataTable ObjInstDTOutput = new DataTable();
                ObjInstDTOutput = new ClsCommonBAL().FunGetCommonDataTable(12, Session["BankID"]+","+Session["SystemID"]);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CustomerRegistration, FillDropDown()", Ex.Message, "");
            }
        }
    }
}