using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class CardOperationsISO : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCO"; //unique optionneumonic from database
                pnlnameoncard.Visible = Convert.ToString(Session["IssuerNo"]) == "3";
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        //For user those having Save right
                        if (accessPrefix.Contains("S"))
                        {
                            hdnAccessCaption.Value = "S";
                            userBtns.AccessButtons = StrAccessCaption;
                            userBtns.SaveClick += new EventHandler(btnSave_Click);
                            userBtns.CancelClick += new EventHandler(btnCancel_Click);
                        }
                        else
                        {
                            hdnAccessCaption.Value = "";
                        }

                        if (!IsPostBack)
                        {
                            FillDropDown();
                        }
                    }
                }
                // Redirect to access denied page
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }

            }
            catch (Exception)
            {
                // (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOperations, Page_Load()", Ex.Message, "");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                string msg = string.Empty;
                bool flg = true;
                List<ValidatorAttr> ListValid = new List<ValidatorAttr>();

                if (pnlnameoncard.Visible)
                {
                    if ((txtSearchCardNo.Value == null || txtSearchCardNo.Value == "") &&
                    (txtSearchNIC.Value == null || txtSearchNIC.Value == "") &&
                    (txtSearchCIF.Value == null || txtSearchCIF.Value == "") &&
                    (txtSearchAccno.Value == null || txtSearchAccno.Value == "") &&
                    (txtSearchNameOnCard.Value == null || txtSearchNameOnCard.Value == ""))
                    {
                        flg = false;
                    }
                }
                else
                {
                    if ((txtSearchCardNo.Value == null || txtSearchCardNo.Value == "") &&
                    (txtSearchNIC.Value == null || txtSearchNIC.Value == "") &&
                    (txtSearchAccno.Value == null || txtSearchAccno.Value == "") &&
                    (txtSearchCIF.Value == null || txtSearchCIF.Value == ""))
                    {
                        flg = false;
                    }
                }


                if (!flg)
                {
                    msg = "All fields cannot be empty!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {

                    new ValidatorAttr { Name = "Card No", Value = txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric = true };
                    new ValidatorAttr { Name = "NIC No", Value = txtSearchNIC.Value, MinLength = 0, MaxLength = 20 };
                    new ValidatorAttr { Name = "CIF ID", Value = txtSearchCIF.Value, MinLength = 0, MaxLength = 20 };
                    new ValidatorAttr { Name = "Account ID", Value = txtSearchAccno.Value, MinLength = 0, MaxLength = 20 };

                    if (pnlnameoncard.Visible)
                        ListValid.Add(new ValidatorAttr { Name = "Name On Card", Value = txtSearchNameOnCard.Value, MinLength = 0, MaxLength = 30 });

                    if (!ListValid.Validate(out msg))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                    }
                    else
                    {

                        DataTable ObjDTOutPut = new DataTable();
                        int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                        CustSearchFilter objSearch = new CustSearchFilter();
                        objSearch.CardNo = txtSearchCardNo.Value;
                        if (!string.IsNullOrEmpty(txtSearchNIC.Value))
                        {
                            objSearch.NIC = txtSearchNIC.Value;
                        }
                        if (!string.IsNullOrEmpty(txtSearchCIF.Value))
                        {
                            objSearch.BankCustID = txtSearchCIF.Value;
                        }
                        if (!string.IsNullOrEmpty(txtSearchNameOnCard.Value))
                        {
                            objSearch.NameOnCard = txtSearchNameOnCard.Value;
                        }
                        if (!string.IsNullOrEmpty(txtSearchAccno.Value))
                        {
                            objSearch.AccountNo = txtSearchAccno.Value;
                        }
                        objSearch.IntPara = 0;
                        objSearch.SystemID = Session["SystemID"].ToString();
                        objSearch.BankID = Session["BankID"].ToString();
                        objSearch.IssuerNo = Session["IssuerNo"].ToString();

                        ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtlISO(objSearch);
                        //ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtl(objSearch);

                        new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnSearch_Click()", "ObjDTOutPut.rows.count=" + ObjDTOutPut.Rows.Count.ToString(), "");
                        if (ObjDTOutPut.Rows.Count > 0 && ObjDTOutPut.Rows[0]["Code"].ToString() == "0")
                        {

                            Session["objSearch"] = objSearch;
                            AddedTableData[] objAdded = new AddedTableData[1];
                            objAdded[0] = new AddedTableData()
                            {
                                control = AGS.Utilities.Controls.Button,
                                buttonName = "View",
                                columnName = "Select",
                                cssClass = "btn btn-primary",
                                index = 6,
                                hideColumnName = true,
                                events = new Event[]
                                {
                                    new Event()
                                    {
                                        EventName = "onclick",
                                        EventValue = "funViewClick($(this));"
                                    }
                                },
                                attributes = new AGS.Utilities.Attribute[]
                                {
                                    new AGS.Utilities.Attribute()
                                    {
                                        AttributeName = "CardNo", BindTableColumnValueWithAttribute = "CardNo"
                                    }
                                    //,
                                    //new AGS.Utilities.Attribute()
                                    //{
                                    //    AttributeName = "CardStatus", BindTableColumnValueWithAttribute = "BlockStatus"
                                    //}
                                }
                            };
                            //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Button, buttonName = "View", columnName = "Select", cssClass = "btn btn-primary", index = 9, hideColumnName = true, events = new Event[] { new Event() { EventName = "onclick", EventValue = "funViewClick($(this));" } }, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "CardNo", BindTableColumnValueWithAttribute = "CardNo" }} };

                            hdnCardDetails.Value = ObjDTOutPut.ToHtmlTableString("Code,OutputDescription", objAdded);

                        }
                        else
                        {
                            hdnFlag.Value = "";
                            LblResult.InnerHtml = "No record found";

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnSearch_Click()", Ex.Message, "");
                hdnFlag.Value = "";
                LblResult.InnerHtml = "No record found";
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hdnCardNo.Value))
                {
                    CustSearchFilter objSearch = (CustSearchFilter)Session["objSearch"];

                    GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    ConfigDataObject ObjData = new ConfigDataObject();

                    DataTable _dtCardAPISourceIdDetails = new DataTable();
                    objSearch.APIRequest = "CardDetails";
                    objSearch.CardNo = hdnCardNo.Value;
                    _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);
                    ObjData.IssuerNo = Session["IssuerNo"].ToString();//$@hin 121222
                    //ObjData.IssuerNo = "7";
                    ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                    ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                    ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);
                    ObjData.IsCardDetailsSearch = true;

                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { objSearch.CardNo });

                    _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

                    //------------------  Response start --------------
                    //ObjAPIResponse.AccountNo = "5500100119004753";
                    //ObjAPIResponse.CifId = "RAJESH0000249555";
                    //ObjAPIResponse.CustomerName = "Nitin jadhav";
                    //ObjAPIResponse.MobileNo = "7303850867";
                    //ObjAPIResponse.DateOfBirth = "20101995";
                    //ObjAPIResponse.ExpiryDate = "12122";
                    //ObjAPIResponse.CardNo = "4575510000001144";
                    //ObjAPIResponse.Date_issued = "12122018";
                    //ObjAPIResponse.Status = "000";
                    //ObjAPIResponse.CardStatus = "1";
                    //ObjAPIResponse.HoldRspCode = "43";
                    //------------------  Response end --------------

                    if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        string OptionNeumonic = "CMCO"; //unique optionneumonic from database
                        Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                        ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                        if (ObjDictRights.ContainsKey(OptionNeumonic))
                        {
                            userBtns.AccessButtons = ObjDictRights[OptionNeumonic];
                        }
                        userBtns.LoadDefault();


                        txtCustomerID.Value = ObjAPIResponse.CifId;
                        txtCustomerName.Value = ObjAPIResponse.CustomerName;
                        txtMobile.Value = ObjAPIResponse.MobileNo;
                        if (string.IsNullOrWhiteSpace(ObjAPIResponse.DateOfBirth))
                        {
                            ObjAPIResponse.DateOfBirth = "";
                        }
                        if (ObjAPIResponse.DateOfBirth.Trim().Length == 8)
                        {
                            //txtDOB.Value = ObjAPIResponse.DateOfBirth.Substring(0, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(2, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(4, 4);
                            txtDOB.Value = ObjAPIResponse.DateOfBirth.Substring(6, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(4, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(0, 4);
                        }
                        else
                        {
                            txtDOB.Value = ObjAPIResponse.DateOfBirth;
                        }

                        if (ObjAPIResponse.ExpiryDate.Trim().Length == 4)
                        {
                            ObjAPIResponse.ExpiryDate = ObjAPIResponse.ExpiryDate.Substring(2, 2) + "/" + ObjAPIResponse.ExpiryDate.Substring(0, 2);
                        }
                        else
                        {
                            ObjAPIResponse.ExpiryDate = ObjAPIResponse.ExpiryDate;
                        }

                        txtAddress.Text = ObjAPIResponse.Address1 + " " + ObjAPIResponse.Address2;
                        txtEmail.Value = ObjAPIResponse.EmailID;
                        txtCardNo.Value = ObjAPIResponse.CardNo;
                        txtExpiryDate.Value = ObjAPIResponse.ExpiryDate;
                        txtCardStatus.Value = ObjAPIResponse.CardStatus;

                        if (string.IsNullOrWhiteSpace(ObjAPIResponse.HoldRspCode) && ObjAPIResponse.CardStatus == "ACTIVE")
                        {
                            txtCardStatus.Value = "Active Card";
                        }
                        else if (ObjAPIResponse.HoldRspCode.ToString().Trim() == "41" || ObjAPIResponse.HoldRspCode.ToString().Trim() == "43")
                        {
                            txtCardStatus.Value = "Lost/Stolen Card";

                            userBtns.ShowButton('T');
                            userBtns.CancelClick += new EventHandler(btnCancel_Click);
                        }
                        else if (ObjAPIResponse.HoldRspCode.ToString().Trim() == "06" || ObjAPIResponse.HoldRspCode.ToString().Trim() == "05")
                        {
                            txtCardStatus.Value = "Temporary Block";
                        }

                        txtCardIssued.Value = ObjAPIResponse.Date_issued;
                        txtCardStatusID.Value = ObjAPIResponse.CardStatus;

                        LblResult.InnerHtml = "";
                        hdnFlag.Value = "2";
                        hdnRPANID.Value = "";
                        txtAccountNo.Value = ObjAPIResponse.AccountNo;
                        hdnPinResetCount.Value = ObjAPIResponse.PinTryCount;
                        hdnCustomerID.Value = ObjAPIResponse.CifId;
                        txtNameOnCard.Text = ObjAPIResponse.NameOnCard;
                        txtPinTryCount.Text = ObjAPIResponse.PinTryCount;
                        txtMotherMaidenName.Text = ObjAPIResponse.MotherMaidenName;
                        txtNicNr.Text = ObjAPIResponse.NicNr;
                        txtDateactivated.Value = ObjAPIResponse.Date_activated;
                    }
                    else
                    {
                        hdnFlag.Value = "";
                        LblResult.InnerHtml = "No record found";
                    }
                }
                else
                {
                    hdnFlag.Value = "";
                    LblResult.InnerHtml = "No record found";
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnView_Click()", Ex.Message, "");
                hdnFlag.Value = "";
                LblResult.InnerHtml = "No record found";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.Visible = false;
                LblResult.InnerHtml = "";
                hdnFlag.Value = LblResult.InnerText = txtUpdateRemark.Text = LblResult.InnerText = string.Empty;

                btnSearch_Click(this, EventArgs.Empty);
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnCancel_Click()", Ex.Message, "");
                hdnFlag.Value = "";
                LblResult.InnerHtml = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;

                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="Request Type", Value= ddlRequestType.SelectedValue, MinLength = 1, MaxLength = 1, Numeric = true, Regex="^[1-9]*$" },
                    new ValidatorAttr { Name="Remark", Value= txtUpdateRemark.Text, MinLength = 3, MaxLength = 80, Isrequired = true }
                };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {
                    ListValid = new List<ValidatorAttr>()
                    {
                    new ValidatorAttr { Name="Request Type", Value= ddlRequestType.SelectedItem.Text.Trim(), CompareName="", CompareValue=txtCardStatus.Value.Trim() , Isrequired = true }
                    };
                    if (ListValid.Validate(out msg))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','Card status is already changed to " + txtCardStatus.Value.Trim() + ", Please select different request type.')", true);
                    }
                    else
                    {
                        if (ddlRequestType.SelectedValue == "5")
                        {
                            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                            ObjReturnStatus.Description = "Card request is not saved";
                            CustSearchFilter ObjFilter = new CustSearchFilter();
                            ObjFilter.IntPara = 1;
                            ObjFilter.BankCustID = hdnCustomerID.Value;

                            ObjFilter.MakerID = Convert.ToInt64(Session["LoginID"]);

                            if (!string.IsNullOrEmpty(txtCardNo.Value))
                            {
                                CustSearchFilter objSearch = new CustSearchFilter();
                                objSearch.SystemID = Session["SystemID"].ToString();
                                objSearch.BankID = Session["BankID"].ToString();
                                objSearch.PINResetFlag = 4; // FLag 4 to find out PIN Try count
                                objSearch.CardNo = txtCardNo.Value;
                                ObjFilter.NameOnCard = objSearch.NameOnCard = txtNameOnCard.Text.Trim();
                                DataTable ObjDTCarddetailsTOResetPin = new DataTable();

                                ObjFilter.CardNo = txtCardNo.Value;
                                ObjFilter.Remark = txtUpdateRemark.Text;
                                ObjFilter.RequestTypeID = Convert.ToInt32(ddlRequestType.SelectedValue);
                                ObjFilter.SystemID = Session["SystemID"].ToString();
                                ObjFilter.BankID = Session["BankID"].ToString();
                                ObjFilter.UserBranchCode = Session["BranchCode"].ToString();
                                ObjFilter.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                                ObjFilter.UserID = Convert.ToString(Session["UserName"]);

                                if (Convert.ToInt32(ddlRequestType.SelectedValue) == 7)
                                {
                                    if (Convert.ToInt64(hdnPinResetCount.Value) < 3)
                                    {
                                        ObjReturnStatus.Description = "Card pin try count is not exceeded.";
                                    }
                                    else
                                    {
                                        ObjReturnStatus = new ClsCardMasterBAL().FunSaveCardOpsReq(ObjFilter);
                                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Card Request send to Checker for | RequestType: " + ddlRequestType.SelectedItem.Text + " | Remark: " + txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Request", "1");
                                    }
                                }
                                else
                                {
                                    ObjReturnStatus = new ClsCardMasterBAL().FunSaveCardOpsReq(ObjFilter);
                                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Card Request send to Checker for | RequestType: " + ddlRequestType.SelectedItem.Text + " | Remark: " + txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Request", "1");
                                }
                            }
                            else
                            {
                                ObjReturnStatus.Description = "Data is not proper";
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Card Status Changed | RequestType: " + ddlRequestType.SelectedItem.Text + " | Remark: " + txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Submit", "1");
                            }


                            LblResult.InnerHtml = LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                            if (ObjReturnStatus.Code == 0)
                            {
                                hdnResultStatus.Value = "1";
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                        }
                        else
                        {
                            AcceptRejectCardOpsRequests(sender, e);
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Card Operation", "Card Request send to Checker for | RequestType: " + ddlRequestType.SelectedItem.Text + " | Remark: " + txtUpdateRemark.Text, txtCardNo.Value, txtCustomerID.Value, txtCustomerName.Value, txtMobile.Value, txtAccountNo.Value, "", "Add", "1");
                        }
                    }
                }
                LblResult.Visible = true;
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnSave_Click()", ObjEx.ToString(), "");
            }
        }

        public void FillDropDown()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(19, "1");
                ddlRequestType.DataSource = ObjDTOutPut;
                ddlRequestType.DataTextField = "RequestType";
                ddlRequestType.DataValueField = "ID";
                ddlRequestType.DataBind();
                ddlRequestType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, FillDropDown()", Ex.Message, "");
            }

        }

        protected void AcceptRejectCardOpsRequests(object sender, EventArgs e)
        {
            try
            {
                CustSearchFilter objSearch = new CustSearchFilter();
                objSearch.MakerID = Convert.ToInt64(Session["LoginID"]);
                //objSearch.FormStatusID = Convert.ToInt32(hdnFormStatusID.Value);
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.RequestIDs = ddlRequestType.SelectedValue;
                objSearch.RequestTypeID = Convert.ToInt32(ddlRequestType.SelectedValue);
                objSearch.CustomerID = txtCustomerID.Value;
                objSearch.CardNo = txtCardNo.Value;
                objSearch.Remark = txtUpdateRemark.Text;
                objSearch.PINResetFlag = 1;
                objSearch.UserBranchCode = Session["BranchCode"].ToString();
                objSearch.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                objSearch.UserID = Convert.ToString(Session["UserName"]);


                Button clickedButton = sender as Button;

                DataTable _DTCardOpsDataISO = new DataTable();
                DataTable _DTUpdateCardOpsRSP = new DataTable();

                objSearch.IssuerNo = Convert.ToString(Session["IssuerNo"]); //$@hin
                //objSearch.IssuerNo = "7";

                objSearch.ActionType = 8;
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, AcceptRejectCardOpsRequests()", "objSearch.IssuerNo "+objSearch.IssuerNo, "RequestTypeID = " + ddlRequestType.SelectedValue);

                _DTCardOpsDataISO = new ClsCardMasterBAL().FunGetOpsDataForISO(objSearch);
                string SuccessCardNo = string.Empty;
                string FailuerCardNo = string.Empty;
                if (_DTCardOpsDataISO.Rows.Count > 0)
                {
                    //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, AcceptRejectCardOpsRequests()", "data found:"+_DTCardOpsDataISO.Rows.Count.ToString(), "data found");
                    GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    ConfigDataObject ObjData = new ConfigDataObject();
                    //ObjData.IssuerNo = Session["IssuerNo"].ToString();
                    ObjData.IssuerNo = "7";
                    ObjData.APIRequestType = Convert.ToString(_DTCardOpsDataISO.Rows[0][1]);
                    ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                    ObjData.SourceID = Convert.ToString(_DTCardOpsDataISO.Rows[0][2]);

                    _DTCardOpsDataISO.Rows[0][0] = objSearch.CardNo;
                    //ObjData.IsCardDetailsSearch = true;                    

                    for (int i = 0; i < _DTCardOpsDataISO.Rows.Count; i++)
                    {
                        //objSearch.RequestIDs = _DTCardOpsDataISO.Rows[i]["id"].ToString(); ///added by uddesh ATPBF-1157
                        DataTable _dtRequest = new DataTable();
                        if (objSearch.RequestTypeID == 1) //1 for SetCardLimit
                        {
                            _dtRequest.Columns.Add("CardNo", typeof(string));
                            _dtRequest.Columns.Add("POSLimitCount", typeof(int));
                            _dtRequest.Columns.Add("POSLimit", typeof(int));
                            _dtRequest.Columns.Add("PTPOSLimit", typeof(string));
                            _dtRequest.Columns.Add("ATMLimitCount", typeof(int));
                            _dtRequest.Columns.Add("ATMLimit", typeof(int));
                            _dtRequest.Columns.Add("PTATMLimit", typeof(string));
                            _dtRequest.Columns.Add("PaymentsCount", typeof(string));
                            _dtRequest.Columns.Add("PaymentsLimit", typeof(string));
                            _dtRequest.Columns.Add("PTPaymentsLimit", typeof(string));
                            _dtRequest.Columns.Add("EComLimit", typeof(string));
                            _dtRequest.Columns.Add("PTEComLimit", typeof(string));

                            _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0], _DTCardOpsDataISO.Rows[i][3], _DTCardOpsDataISO.Rows[i][4],
                                _DTCardOpsDataISO.Rows[i][5], _DTCardOpsDataISO.Rows[i][6], _DTCardOpsDataISO.Rows[i][7], _DTCardOpsDataISO.Rows[i][8],
                                _DTCardOpsDataISO.Rows[i][9], _DTCardOpsDataISO.Rows[i][10], _DTCardOpsDataISO.Rows[i][11], _DTCardOpsDataISO.Rows[i][12],
                                _DTCardOpsDataISO.Rows[i][13]});

                        }
                        else if (objSearch.RequestTypeID == 2 || objSearch.RequestTypeID == 3 || objSearch.RequestTypeID == 4)
                        {
                            _dtRequest.Columns.Add("CardNo", typeof(string));
                            _dtRequest.Columns.Add("BlockType", typeof(string));
                            _dtRequest.Columns.Add("InternationalCard", typeof(string));
                            _dtRequest.Columns.Add("Status", typeof(string));

                            string BlockType = string.Empty;
                            string CardStatus = string.Empty;

                            if (objSearch.RequestTypeID == 2)//HSC 06
                            {
                                BlockType = "1";
                                CardStatus = "1";
                            }
                            else if (objSearch.RequestTypeID == 3)//HSC 41
                            {
                                BlockType = "0";
                                CardStatus = "1";
                            }
                            else if (objSearch.RequestTypeID == 4)//43
                            {
                                BlockType = "3";
                                CardStatus = "1";
                            }
                            _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0], BlockType, "0", CardStatus });
                        }
                        else if (objSearch.RequestTypeID == 7)
                        {
                            _dtRequest.Columns.Add("CardNo", typeof(string));
                            _dtRequest.Rows.Add(new Object[] { _DTCardOpsDataISO.Rows[i][0] });
                        }

                        _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

                        objSearch.ActionType = 9; //To update Status of cardopsrequests


                        objSearch.ISORSPCode = ObjAPIResponse.Status;
                        objSearch.ISORSPDesc = ObjAPIResponse.StatusDesc;
                        if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            SuccessCardNo = SuccessCardNo + _DTCardOpsDataISO.Rows[i][0] + ",";
                        }
                        else
                        {
                            FailuerCardNo = FailuerCardNo + _DTCardOpsDataISO.Rows[i][0] + ",";
                        }

                        _DTUpdateCardOpsRSP = new ClsCardMasterBAL().FunGetOpsDataForISO(objSearch);
                    }

                    if (!string.IsNullOrEmpty(SuccessCardNo))
                    {
                        LblResult.InnerHtml = "Card has been blocked successfully";//"Success Cards : " + removeLastChar(SuccessCardNo);
                    }
                    if (!string.IsNullOrEmpty(FailuerCardNo))
                    {
                        LblResult.InnerHtml = LblResult.InnerHtml + "Failed to block Card";//"<br/>Failuer Cards : " + removeLastChar(FailuerCardNo);
                    }
                }
                else
                {
                    LblResult.InnerHtml = "System Error";
                }

            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CheckCardOperation, AcceptRejectCardOpsRequests()", Ex.Message, "RequestTypeID = " + ddlRequestType.SelectedValue);
            }
        }

        private static String removeLastChar(String str)
        {
            if (str.LastIndexOfAny(new char[] { ',' }) == str.Length - 1)
                str = str.Substring(0, str.Length - 1);
            return str;
        }
    }
}