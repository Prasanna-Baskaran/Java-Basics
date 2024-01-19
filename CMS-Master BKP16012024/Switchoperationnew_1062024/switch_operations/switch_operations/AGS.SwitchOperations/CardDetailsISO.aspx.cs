using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class CardDetailsISO : System.Web.UI.Page
    {
        ClsCommonDAL commonDal = new ClsCommonDAL();
        public string CustomerName { get; set; }
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCD"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];
                pnltxtAccount.Visible = !(pnlnameoncard.Visible = Convert.ToString(Session["IssuerNo"]) == "3");
                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {

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
            catch (Exception ObjEx)
            {
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardDetails, Page_Load()", ObjEx.Message, "");
                //Response.Redirect("ErrorPage.aspx", false);
            }

        }

        public bool CheckSingleVal()
        {
            int i = 0;

            if (txtSearchCardNo.Value.Trim().Length > 0)
                i++;

            /* if (txtSearchName.Value.Trim().Length > 0)
                 i++;*/

            if (txtNICNo.Value.Trim().Length > 0)
                i++;

            if (txtSearchAccountNo.Value.Trim().Length > 0)
                i++;

            if (txtSearchNameOnCard.Value.Trim().Length > 0)
                i++;

            if (txtCIF.Value.Trim().Length > 0)
                i++;

            if (i > 1)
                return false;
            else
                return true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                string msg = string.Empty;
                List<ValidatorAttr> ListValid = new List<ValidatorAttr>();

                if ((txtSearchCardNo.Value == null || txtSearchCardNo.Value == "") &&
                    //(txtSearchName.Value == null || txtSearchName.Value == "") &&
                    (txtNICNo.Value == null || txtNICNo.Value == "") &&
                    (txtSearchAccountNo.Value == null || txtSearchAccountNo.Value == "") &&
                    (txtSearchNameOnCard.Value == null || txtSearchNameOnCard.Value == "") &&
                    (txtCIF.Value == null || txtCIF.Value == ""))
                {
                    msg = "All fields cannot be empty!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else if (!CheckSingleVal())
                {
                    msg = "Single field value can seach at one time!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);

                }
                else
                {
                    new ValidatorAttr { Name = "Card No", Value = txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric = true };
                    //new ValidatorAttr { Name = "Customer Name", Value = txtSearchName.Value, MinLength = 3, MaxLength = 50 };
                    new ValidatorAttr { Name = "NIC No", Value = txtNICNo.Value, MinLength = 3, MaxLength = 20 };

                    if (pnltxtAccount.Visible)
                        ListValid.Add(new ValidatorAttr { Name = "Account No", Value = txtSearchAccountNo.Value, MinLength = 5, MaxLength = 20 });

                    if (pnlnameoncard.Visible)
                        ListValid.Add(new ValidatorAttr { Name = "Nane On Card", Value = txtSearchNameOnCard.Value, MinLength = 3, MaxLength = 20, Regex = "^[a-zA-Z0-9 ]*$" });


                    if (!ListValid.Validate(out msg))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                    }
                    else
                    {
                        DataTable ObjCardStatus = new DataTable();
                        DataTable ObjCardDetails = new DataTable();

                        int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                        CustSearchFilter objSearch = new CustSearchFilter();
                        //objSearch.MobileNo = txtSearchMobile.Value;
                        //accountNo
                        if (!string.IsNullOrEmpty(txtSearchAccountNo.Value))
                        {
                            objSearch.AccountNo = txtSearchAccountNo.Value.Trim();
                        }
                        objSearch.CardNo = txtSearchCardNo.Value;
                        objSearch.CustomerName = null;// txtSearchName.Value;
                        if (!string.IsNullOrEmpty(txtNICNo.Value))
                        {
                            objSearch.NIC = txtNICNo.Value.Trim();
                        }
                        if (!string.IsNullOrEmpty(txtSearchNameOnCard.Value))
                        {
                            objSearch.NameOnCard = txtSearchNameOnCard.Value.Trim();
                        }
                        if (!string.IsNullOrEmpty(txtCIF.Value))
                        {
                            objSearch.CIF = txtCIF.Value.Trim();
                        }



                        objSearch.IntPara = 4;
                        objSearch.SystemID = Session["SystemID"].ToString();
                        objSearch.BankID = Session["BankID"].ToString();
                        objSearch.IssuerNo = Session["IssuerNo"].ToString();
                        //objSearch.IssuerNo = "7";
                        //if(!string.IsNullOrEmpty(txtSearchAccountNo.Value))
                        //objSearch.CustomerID = Convert.ToInt64(txtSearchAccountNo.Value);

                        //commonDal.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Card Details View", "Searching card details",txtSearchCardNo.Value, "", txtSearchName.Value, "", txtSearchAccountNo.Value, txtNICNo.Value, "Searching", "1");

                        ObjCardStatus = new ClsCardMasterBAL().FunSearchCardDtlISO(objSearch);

                        DataTable _dtViewCardDetails = new DataTable();

                        if (ObjCardStatus.Rows.Count > 0)
                        {
                            GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();

                            DataTable _dtCardAPISourceIdDetails = new DataTable();
                            objSearch.APIRequest = "CardDetails";
                            _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);

                            APIResponseObject ObjAPIResponse = new APIResponseObject();
                            ConfigDataObject ObjData = new ConfigDataObject();
                            //ObjData.IssuerNo = "7";
                            ObjData.IssuerNo = Session["IssuerNo"].ToString();//$@hin 121222
                            ObjData.IsCardDetailsSearch = true;

                            ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                            ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                            ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);

                            for (int i = 0; i < ObjCardStatus.Rows.Count; i++)
                            {
                                DataTable _dtRequest = new DataTable();
                                _dtRequest.Columns.Add("CardNo", typeof(string));
                                _dtRequest.Rows.Add(new Object[] { Convert.ToString(ObjCardStatus.Rows[i][1]) });
                                _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);
                                //------------------  Response start --------------
                                //ObjAPIResponse.AccountNo = "5500100119004753";
                                //ObjAPIResponse.CifId = "RAJESH0000249555";
                                //ObjAPIResponse.CustomerName = "Nitin jadhav";
                                //ObjAPIResponse.MobileNo = "7303850000";
                                //ObjAPIResponse.DateOfBirth = "20101995";
                                //ObjAPIResponse.ExpiryDate = "12122";
                                //ObjAPIResponse.CardNo = "4575510000001144";
                                //ObjAPIResponse.Date_issued = "12122018";
                                //ObjAPIResponse.Status = "000";
                                //ObjAPIResponse.CardStatus = "1";
                                //------------------  Response end --------------

                                if (ObjAPIResponse.DateOfBirth.Trim().Length == 8)
                                {
                                    ObjAPIResponse.DateOfBirth = ObjAPIResponse.DateOfBirth.Substring(6, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(4, 2) + "/" + ObjAPIResponse.DateOfBirth.Substring(0, 4);
                                }
                                else
                                {
                                    ObjAPIResponse.DateOfBirth = ObjAPIResponse.DateOfBirth;
                                }

                                if (ObjAPIResponse.ExpiryDate.Trim().Length == 4)
                                {
                                    ObjAPIResponse.ExpiryDate = ObjAPIResponse.ExpiryDate.Substring(2, 2) + "/" + ObjAPIResponse.ExpiryDate.Substring(0, 2);
                                }
                                else
                                {
                                    ObjAPIResponse.ExpiryDate = ObjAPIResponse.ExpiryDate;
                                }


                                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                                {
                                    _dtViewCardDetails = DatatableToView(_dtViewCardDetails, ObjAPIResponse);
                                }
                            }

                            if (_dtViewCardDetails.Rows.Count > 0)
                            {
                                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnView_Click()", "_dtViewCardDetails.Rows[0][0]=" + _dtViewCardDetails.Rows.Count.ToString(), "");

                                hdnTransactionDetails.Value = _dtViewCardDetails.ToHtmlTableString("");
                                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperationsISO, btnView_Click()", "ToHtmlTableString: " + _dtViewCardDetails.ToHtmlTableString(""), "");


                                LblResult.InnerHtml = "";
                            }
                            else
                            {
                                LblResult.InnerHtml = "No Record Found";
                                //commonDal.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Card Details View", "No card details found", txtSearchCardNo.Value, "", txtSearchName.Value, "", txtSearchAccountNo.Value, txtNICNo.Value, "Searched", "0");
                            }
                        }
                        else
                        {
                            LblResult.InnerHtml = "No Record Found";
                        }
                        //commonDal.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), objSearch.IssuerNo, "Card Details View", "Search button clicked", objSearch.CardNo, objSearch.CustomerID.ToString(), objSearch.CustomerName, objSearch.MobileNo, objSearch.AccountNo, objSearch.NIC, "Searched", "1");
                        AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CardDetails.aspx", "Search button clicked", "");
                    }
                }
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, btnSearch_Click, Page_Load()", ObjEx.Message, "");
                LblResult.InnerHtml = "Error Occur while getting data";
                commonDal.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Card Details View", "Exception>> " + ObjEx.ToString(), "", "", "", "", "", "", "Search", "0");
            }
        }

        protected DataTable DatatableToView(DataTable _DT, APIResponseObject ObjAPIResponse)
        {
            try
            {

                if (_DT.Columns.Count == 0)
                {
                    _DT.Columns.Add("CardNo", typeof(string));
                    _DT.Columns.Add("CardStatus", typeof(string));
                    _DT.Columns.Add("CifId", typeof(string));
                    _DT.Columns.Add("CustomerName", typeof(string));
                    _DT.Columns.Add("NameOnCard", typeof(string));
                    _DT.Columns.Add("Address", typeof(string));
                    _DT.Columns.Add("Date_activated", typeof(string));
                    _DT.Columns.Add("Date_issued", typeof(string));
                    _DT.Columns.Add("DateOfBirth", typeof(string));
                    _DT.Columns.Add("EmailID", typeof(string));
                    _DT.Columns.Add("ExpiryDate", typeof(string));
                    _DT.Columns.Add("HoldRspCode", typeof(string));
                    _DT.Columns.Add("MobileNo", typeof(string));
                    //malar
                    _DT.Columns.Add("Account 1", typeof(string));
                    _DT.Columns.Add("Account 2", typeof(string));
                    _DT.Columns.Add("Account 3", typeof(string));
                    _DT.Columns.Add("Account 4", typeof(string));

                    if (Convert.ToInt32(Session["IsEPS"]) == 0)
                    {
                        _DT.Columns.Add("CNP_daily_limit", typeof(string));
                        _DT.Columns.Add("CNP_per_txn_limit", typeof(string));
                        _DT.Columns.Add("nr_of_payment", typeof(string));
                        _DT.Columns.Add("nr_of_purchase", typeof(string));
                        _DT.Columns.Add("nr_of_withdrawal", typeof(string));

                        _DT.Columns.Add("payment_per_txn_limit", typeof(string));
                        _DT.Columns.Add("purchase_daily_limit", typeof(string));
                        _DT.Columns.Add("purchase_per_txn_limit", typeof(string));
                        _DT.Columns.Add("withdrwal_daily_limit", typeof(string));
                        _DT.Columns.Add("withdrwal_per_txn_limit", typeof(string));
                        _DT.Columns.Add("KIT_Number", typeof(string));
                    }
                }

                if (Convert.ToInt32(Session["IsEPS"]) == 0)
                {
                    _DT.Rows.Add(new Object[] {ObjAPIResponse.CardNo,ObjAPIResponse.CardStatus,ObjAPIResponse.CifId,ObjAPIResponse.CustomerName,ObjAPIResponse.NameOnCard,(Convert.ToString(ObjAPIResponse.Address) +" "+Convert.ToString(ObjAPIResponse.Address1)+" "+Convert.ToString(ObjAPIResponse.Address2)).TrimStart(' ').TrimEnd(' '),
                     ObjAPIResponse.Date_activated,ObjAPIResponse.Date_issued,ObjAPIResponse.DateOfBirth ,ObjAPIResponse.EmailID,ObjAPIResponse.ExpiryDate,
                     ObjAPIResponse.HoldRspCode,ObjAPIResponse.MobileNo,
                     ObjAPIResponse.AccountNo,ObjAPIResponse.AC1,ObjAPIResponse.AC2,ObjAPIResponse.AC3,
                     ObjAPIResponse.EComLimit,ObjAPIResponse.PTEComLimit,ObjAPIResponse.PaymentsCount,
                     ObjAPIResponse.POSLimitCount,ObjAPIResponse.ATMLimitCount,
                     ObjAPIResponse.PTPaymentsLimit,ObjAPIResponse.POSLimit,
                     ObjAPIResponse.PTPOSLimit,
                     ObjAPIResponse.ATMLimit,ObjAPIResponse.PTATMLimit,ObjAPIResponse.PGKNo
                    });
                }
                else
                {
                    _DT.Rows.Add(new Object[] {ObjAPIResponse.CardNo,ObjAPIResponse.CardStatus,ObjAPIResponse.CifId,ObjAPIResponse.CustomerName,ObjAPIResponse.NameOnCard,ObjAPIResponse.Address,
                        ObjAPIResponse.Date_activated,ObjAPIResponse.Date_issued,ObjAPIResponse.DateOfBirth ,ObjAPIResponse.EmailID,ObjAPIResponse.ExpiryDate,
                        ObjAPIResponse.HoldRspCode,ObjAPIResponse.MobileNo
                        });
                }


                return _DT;

            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, btnSearch_Click, DatatableToView()", ObjEx.Message, "");
                return _DT;
            }

        }
    }
}