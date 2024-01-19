using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    public partial class Registration : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDown();
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO _ClsReturnStatusBO = new ClsReturnStatusBO();
                CustomerRegistrationDetails _custRegDet = new CustomerRegistrationDetails();
                _custRegDet.CIFID = txtCifId.Value;
                _custRegDet.CustomerName = txtCustomerName.Value;
                _custRegDet.CustomerPreferredName = txtNameOnCard.Value;
                _custRegDet.CardPrefix = DdlBinPrefix.SelectedItem.Text;
                _custRegDet.ACID = txtAccountNo.Value;
                _custRegDet.CIFCreationDate = txtCIFCreationDate.Value.Replace("/", "");
                _custRegDet.Address1 = txtAddress1.Value;
                _custRegDet.Address2 = txtAddress2.Value;
                _custRegDet.Address3 = txtAddress3.Value;
                _custRegDet.City = txtCity.Value;
                _custRegDet.State = txtState.Value;
                _custRegDet.PinCode = txtPincode.Value;
                _custRegDet.Country = txtCountry.Value;
                _custRegDet.MothersMaidenName = txtMotherName.Value;
                _custRegDet.DOB = DOB.Value.Replace("/", "");
                _custRegDet.CountryDialcode = txtCountryCode.Value;
                _custRegDet.STDCode = txtStdCode.Value;
                _custRegDet.MobileNo = txtMobileNo.Value;
                _custRegDet.EmailId = txtEmailId.Value;
                _custRegDet.AccountType = DdlAccountType.SelectedValue;
                _custRegDet.BranchCode = txtBranchCode.Value;
                _custRegDet.PANNumber = txtPanNumber.Value;
                _custRegDet.ModeOfOperation = "01";
                _custRegDet.FourthLineEmbossing = txtForthLineEmbossing.Value;
                _custRegDet.Aadhaar = TxtAadharNo.Value;
                _custRegDet.Pin_Mailer = "01";
                _custRegDet.SystemID = Convert.ToInt32(Session["SystemID"].ToString());
                _custRegDet.BankID = Session["BankID"].ToString();
                _custRegDet.RequestedBy = Convert.ToString(Session["UserName"]);
                _custRegDet.RequestType = "1";

                _custRegDet.IssuerNo = Convert.ToInt32(Session["IssuerNo"].ToString());
                DataTable _dtDetails = new DataTable();
                _dtDetails = ObjectIntoDataTable(_custRegDet);

                _ClsReturnStatusBO = new ClsCustomerMasterBAL().FunSaveCardRequest(_dtDetails, _custRegDet);

                LblMsg.InnerText = _ClsReturnStatusBO.Description.ToString();

                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Registration", "New registration", _custRegDet.CardNo, _custRegDet.CIFID, _custRegDet.CustomerName, _custRegDet.MobileNo, _custRegDet.ACID, _custRegDet.OldNICNo, "Submit", "0");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                LblMsg.InnerText = "Unexpected error occured.";
                (new ClsCommonBAL()).FunInsertIntoErrorLog("Registration, btnSubmit_Click()", Ex.Message, "");
            }
        }

        protected void FillDropDown()
        {
            try
            {
                //CardType
                DataTable ObjCardTypeDTOutPut = new DataTable();
                ObjCardTypeDTOutPut = new ClsCommonBAL().FunGetCommonCardDetails(1, Session["BankID"].ToString());
                DdlBinPrefix.DataSource = ObjCardTypeDTOutPut;
                DdlBinPrefix.DataTextField = "CardPrefix";
                DdlBinPrefix.DataValueField = "ID";
                DdlBinPrefix.DataBind();
                DdlBinPrefix.Items.Insert(0, new ListItem("--Select--", "0"));

                //Account Type 
                DataTable ObjAccTypeDTOutput = new DataTable();
                ObjAccTypeDTOutput = new ClsCommonBAL().FunGetCommonCardDetails(2, Session["BankID"].ToString());
                DdlAccountType.DataSource = ObjAccTypeDTOutput;
                DdlAccountType.DataTextField = "AccountTypeName";
                DdlAccountType.DataValueField = "AccountTypeCode";
                DdlAccountType.DataBind();
                DdlAccountType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, Registration, FillDropDown()", Ex.Message, "");
            }
        }

        public DataTable ObjectIntoDataTable(CustomerRegistrationDetails _custRegDet)
        {
            //The DataTable to Return
            DataTable result = new DataTable();
            try
            {
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

                result.Rows.Add(_custRegDet.CIFID, _custRegDet.CustomerName, _custRegDet.CustomerPreferredName, _custRegDet.CardPrefix, _custRegDet.ACID, _custRegDet.ACOpenDate, _custRegDet.CIFCreationDate, _custRegDet.Address1, _custRegDet.Address2, _custRegDet.Address3,
                    _custRegDet.City, _custRegDet.State, _custRegDet.PinCode, _custRegDet.Country, _custRegDet.MothersMaidenName, _custRegDet.DOB, _custRegDet.CountryDialcode, _custRegDet.STDCode, _custRegDet.MobileNo, _custRegDet.EmailId, _custRegDet.AccountType, _custRegDet.BranchCode,
                    _custRegDet.EnteredDate, _custRegDet.VerifiedDate, _custRegDet.PANNumber, _custRegDet.ModeOfOperation, _custRegDet.FourthLineEmbossing, _custRegDet.Aadhaar, "Y", _custRegDet.Pin_Mailer, "");
            }
            catch (Exception Ex)
            {
                result = new DataTable();
                (new ClsCommonBAL()).FunInsertIntoErrorLog("Registration, ObjectIntoDataTable()", Ex.Message, "");

            }
            //Return the imported data.        
            return result;
        }

    }
}