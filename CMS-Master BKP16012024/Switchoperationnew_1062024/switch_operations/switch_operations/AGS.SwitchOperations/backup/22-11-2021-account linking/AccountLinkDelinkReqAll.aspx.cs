using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
namespace AGS.SwitchOperations
{
    public partial class AccountLinkDelinkReqAll : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        string Status = string.Empty;
        string Msg = string.Empty;
        string Description = string.Empty;
        string SessionId = string.Empty;
        string BankId = string.Empty;
        Boolean SkipDialogBox;
        bool LinkDelinkFlag;
        ClsGeneratePrepaidCardBO ObjPrepaid;
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //BtnSave.Visible = false;
                //BtnDelink.Visible = false;
                //select_all.Visible = false;
                //LBLselect_all.Visible = false;
                //BtnAddNew.Visible = false;
                BankId = Session["BankID"].ToString();
                if (!Page.IsPostBack)
                {
                    string OptionNeumonic = "ALDRA"; //unique optionneumonic from database
                    Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                    ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                    if (ObjDictRights.ContainsKey(OptionNeumonic))
                    {
                        StrAccessCaption = ObjDictRights[OptionNeumonic];
                        if (!string.IsNullOrEmpty(StrAccessCaption))
                        {
                            hdnAccessCaption.Value = StrAccessCaption;

                        }
                        else
                        {
                            Response.Redirect("ErrorPage.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
                    }

                    /*Hiding memberModal DIV*/
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, AccountLinkingRequest, Page_Load()", ex.Message, BankId);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SkipDialogBox = true;
            string msg = string.Empty;

            List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="Card No", Value= txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric = true, RequiredGroup="G1" },
                    new ValidatorAttr { Name="CIF No", Value= txtCIF.Value, MinLength = 3, MaxLength = 20, RequiredGroup="G1" },
                    new ValidatorAttr { Name="NIC No", Value= txtNIC.Value, MinLength = 3, MaxLength = 20, RequiredGroup="G1" },
                    new ValidatorAttr { Name="Name On Card", Value= txtSearchNameOnCard.Value, MinLength = 3, MaxLength = 20, Regex="^[a-zA-Z0-9 ]*$", RequiredGroup="G1" }
                };

            if (!ListValid.Validate(out msg))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel();validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
            }
            else
            {
                FunSearchDetails();
            }
        }

        public void FunSearchDetails()
        {
            try
            {
                LblResult.InnerHtml = "";
                LblMessage.Text = "";

                //if (!string.IsNullOrEmpty(txtCIF.Value) && !string.IsNullOrEmpty(txtNIC.Value))
                //{
                //    LblMessage.Text = "Please enter either CIF No or NIC No!";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //    hdnFlag.Value = "";
                //    hdnTransactionDetails.Value = "";
                //    LblResult.InnerHtml = "Please enter either CIF No or NIC No!";
                //    return;
                //}

                CustSearchFilter objSearch = new CustSearchFilter();
                DataTable ObjDTOutPut = new DataTable();
                DataTable DtAccDet = new DataTable();

                objSearch.CardNo = txtSearchCardNo.Value;
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();

                objSearch.BankCustID = Convert.ToString(txtCIF.Value);
                objSearch.NIC = Convert.ToString(txtNIC.Value);
                objSearch.NameOnCard = Convert.ToString(txtSearchNameOnCard.Value);
                
                hdnFlag.Value = "";
                hdnTransactionDetails.Value = "";

                //BtnSave.Visible = true;
                //BtnDelink.Visible = true;
                //select_all.Visible = true;
                //LBLselect_all.Visible = true;

                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtlISO(objSearch);

                if (ObjDTOutPut.Rows.Count > 0 && ObjDTOutPut.Rows[0]["Code"].ToString() == "0")
                {
                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    hdnCardNo.Value = objSearch.CardNo = Convert.ToString(ObjDTOutPut.Rows[0]["CardNo"]);
                    hdnCifId.Value = objSearch.BankCustID = Convert.ToString(ObjDTOutPut.Rows[0]["CIF ID"]);
                    ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3");

                    if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNo))
                        {
                            DataTable _dtAccountDetails = new DataTable();
                            _dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, objSearch.CardNo);


                            if (SkipDialogBox)
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                            if (_dtAccountDetails.Rows.Count > 0)
                            {
                                hdnAccLinkDetails.Value = createTable(_dtAccountDetails);
                                //BtnAddNew.Visible = true;
                            }
                            else
                            {
                                hdnAccLinkDetails.Value = "";
                                LblResult.InnerHtml = "No Linking Records Found";
                            }
                        }
                        else
                        {
                            LblMessage.Text = "No Linking Records Found";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                            hdnFlag.Value = "";
                            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                            hdnAccLinkDetails.Value = "";
                            LblResult.InnerHtml = "No Linking Records Found";
                        }
                    }
                    else
                    {
                        LblMessage.Text = "No Linking Records Found!";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        hdnFlag.Value = "";
                        hdnAccLinkDetails.Value = "";
                        LblResult.InnerHtml = "No Linking Records Found!";
                    }
                }
                else
                {
                    LblMessage.Text = "Invalid card number!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "";
                    hdnAccLinkDetails.Value = "";
                    LblResult.InnerHtml = "Invalid card number!";
                }

            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, AccountLinkingRequest, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            FunSetResult("Insert", "1");
            hdnAccLinkDetails.Value = "";
        }
        protected void BtnDelink_Click(object sender, EventArgs e)
        {
            FunSetResult("Insert", "2");
        }

        public void FunSetResult(String mode, string Linkingflag)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                {
                    LblMessage.Text = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    ObjPrepaid.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                    ObjPrepaid.Mode = mode;
                    ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                    ObjPrepaid.CardREqID = 0;
                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("CustomerId", typeof(string));
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("NICNo", typeof(string));
                    _dtRequest.Columns.Add("Linkingflag", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    //foreach (string sr in ObjPrepaid.RequestIDs)
                    //{
                    //    _dtRequest.Rows.Add(new Object[] { txtAccountNo.Value, txtCIF.Value, txtSearchCardNo.Value, txtNIC.Value, "1", ddlAccountType.SelectedValue, hdnCheckAccQuntifr.Value });
                    //}
                    _dtRequest.Rows.Add(new Object[] { txtAccountNo.Value, txtCIF.Value==""? hdnCifId.Value: txtCIF.Value, txtSearchCardNo.Value==""?hdnCardNo.Value:txtSearchCardNo.Value, txtNIC.Value, "1", ddlAccountType.SelectedValue, hdnCheckAccQuntifr.Value });


                    //var duplicates = _dtRequest.AsEnumerable().GroupBy(r => r["AccountQualifier"]).Where(gr => gr.Count() > 1).ToList().Any();

                    ObjPrepaid.DtBulkData = _dtRequest;
                    DataSet ObjDSOutPut = new DataSet();
                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GetSetAccountLinkrequest(ObjPrepaid);

                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                        {

                            LblResult.InnerHtml = "<span style='color:green'>Account " + ((Linkingflag == "1") ? "linking" : "delinking") + " request send successfully!</span>";

                        }
                        else
                        {
                            LblResult.InnerHtml = "<span style='color:red'>" + Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][1]) + "</span>";
                        }
                    }
                    else
                    {
                        LblResult.InnerHtml = "<span style='color:red'>Error Occured</span>";
                    }
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingRequest,FunSetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblResult.InnerHtml = "Error Occured";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);

        }


        public string createTable(DataTable dtResult)
        {

            StringBuilder strHTML = new StringBuilder();
            string strTRStart = string.Empty;
            string strTREnd = "</tr>";
            string strTDStart = "<td>";
            string strTDEnd = "</td>";

            try
            {
                //CREATE TABLE HEADER FROM DATATABLE
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        //ADD TABLE HEADERS
                        string[] columnNames = dtResult.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                        strHTML.Append("<thead class='tHead_style' >" + "<tr>");
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            strHTML.Append("<th class='text_middle'>" + columnNames[i].Trim() + "</th>");
                        }


                        strHTML.Append("</tr>" + "</thead>" + "<tbody>");

                        //Clear old Linked/Delinked account list
                        hdnLinkedAccList.Value = hdnDeLinkedAccList.Value = string.Empty;

                        //ADD TABLE DATA 
                        foreach (DataRow currentRow in dtResult.Rows)
                        {
                            if (dtResult.Rows.IndexOf(currentRow) % 2 == 0)
                            {
                                strTRStart = "<tr class='odd gradeX text_middle'>";
                            }
                            else
                            {
                                strTRStart = "<tr class='even gradeC text_middle'>";
                            }

                            strHTML.Append(strTRStart);
                            for (int loopVar = 0; loopVar < columnNames.Length; loopVar++)
                            {
                                strHTML.Append(strTDStart + currentRow[columnNames[loopVar]].ToString().Trim() + strTDEnd);
                            }
                            if (currentRow[columnNames[0]].ToString().ToUpper() == "LINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='2' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' CifId='" + currentRow[5].ToString() + "' > Delink </ asp:Button >" + strTDEnd);
                                // Convert.ToInt32(Session["Bank"])
                                hdnLinkedAccList.Value = hdnLinkedAccList.Value + "," + currentRow[2].ToString();
                            }
                            if (currentRow[columnNames[0]].ToString().ToUpper() == "DELINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Delink'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs ' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='1' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' CifId='" + currentRow[5].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                hdnDeLinkedAccList.Value = hdnDeLinkedAccList.Value + "," + currentRow[2].ToString();
                            }
                            else
                            {
                                strHTML.Append(strTDStart + strTDEnd);
                                //strHTML.Append(strTDStart + strTDEnd);
                            }
                            //Maker         
                            strHTML.Append(strTREnd);
                        }
                        strHTML.Append("</tbody>");
                    }
                }
                if (strHTML.ToString() == "")
                    return "No Results Found !";
                else
                    return strHTML.ToString();
            }
            catch (Exception Ex)
            {
                //(new ClsCommonBAL()).SaveErrorLogDetails("AcManagement.aspx.cs, createTable()", Ex.Message, Ex.StackTrace);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinkingRequest " + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
                return "";
            }
        }



        public void FunSetResultLinkDeLink(String mode, string Linkingflag)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                {
                    LblMessage.Text = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    ObjPrepaid.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                    ObjPrepaid.Mode = mode;
                    ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                    ObjPrepaid.CardREqID = 0;
                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("CustomerId", typeof(string));
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("NICNo", typeof(string));
                    _dtRequest.Columns.Add("Linkingflag", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    //foreach (string sr in ObjPrepaid.RequestIDs)
                    //{
                    //    _dtRequest.Rows.Add(new Object[] { txtAccountNo.Value, txtCIF.Value, txtSearchCardNo.Value, txtNIC.Value, "1", ddlAccountType.SelectedValue, hdnCheckAccQuntifr.Value });
                    //}
                    _dtRequest.Rows.Add(new Object[] { hdnAccountNo.Value, hdnCifId.Value, hdnCardNo.Value, txtNIC.Value, Linkingflag, hdnAccountType.Value, hdnAccountQuilifier.Value });


                    //var duplicates = _dtRequest.AsEnumerable().GroupBy(r => r["AccountQualifier"]).Where(gr => gr.Count() > 1).ToList().Any();

                    ObjPrepaid.DtBulkData = _dtRequest;
                    DataSet ObjDSOutPut = new DataSet();
                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GetSetAccountLinkrequest(ObjPrepaid);

                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                        {

                            LblResult.InnerHtml = "<span style='color:green'>Account " + ((Linkingflag == "1") ? "linking" : "delinking") + " request send successfully!</span>";

                        }
                        else
                        {
                            LblResult.InnerHtml = "<span style='color:red'>" + Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][1]) + "</span>";
                        }
                    }
                    else
                    {
                        LblResult.InnerHtml = "<span style='color:red'>Error Occured</span>";
                    }
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingRequest,FunSetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblResult.InnerHtml = "Error Occured";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);

        }

        protected void hdnLinkBtn_Click(object sender, EventArgs e)
        {


            FunSetResultLinkDeLink("Insert", hdnLinkingflag.Value);
            hdnAccLinkDetails.Value = "";
            return;

            APIResponseObject ObjAPIResponse = new APIResponseObject();
            ObjAPIResponse = CallISOForAccountOperation("Acclinking", "1");

            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
            {
                if (hdnLinkingflag.Value == "1")
                {
                    LblMessage.Text = "Account Linked Successfully.";
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully.";
                }


                ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3");
                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNo))
                    {

                        DataTable _dtAccountLlinkingDetails = new DataTable();
                        _dtAccountLlinkingDetails = CardAccountDataTable(ObjAPIResponse.AccountNo);
                        if (_dtAccountLlinkingDetails.Rows.Count > 0)
                        {
                            ClsAccountLinkingDelinkingBAL.FunSyncCardAccountLinkingDetails(hdnCardNo.Value, Session["IssuerNo"].ToString(), _dtAccountLlinkingDetails);
                        }

                        DataTable _dtAccountDetails = new DataTable();
                        _dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, hdnCardNo.Value);
                        if (_dtAccountDetails.Rows.Count > 0)
                        {
                            hdnTransactionDetails.Value = createTable(_dtAccountDetails);
                        }
                        else
                        {
                            hdnTransactionDetails.Value = "";
                            LblResult.InnerHtml = "No Record Found";
                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully but failed to sync details. Please try again.";

                }
            }
            else
            {
                LblMessage.Text = "Failed";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

        }

        protected void AddAccount_Click(object sender, EventArgs e)
        {
            //if (CheckAccountQuantifier.Checked)
            //{
            //    hdnCheckAccQuntifr.Value = "1";
            //}
            //else
            //{
            //    hdnCheckAccQuntifr.Value = "2";
            //}
            //string msg = string.Empty;

            //List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
            //    {
            //        new ValidatorAttr { Name="CIF No", Value= txtCIF.Value, MinLength = 3, MaxLength = 20, RequiredGroup="G1" },
            //    };

            //if (!ListValid.Validate(out msg))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
            //    return;
            //}
            hdnCheckAccQuntifr.Value = ddlAccQual.SelectedValue;

            FunSetResult("Insert", "1");
            return;
            //
         
            APIResponseObject ObjAPIResponse = new APIResponseObject();
            ObjAPIResponse = CallISOForAccountOperation("AddAccount", "2");

            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
            {
                ClsAccountLinkingDelinkingBAL.FunSyncAccountEnc(BankId, ObjAPIResponse.AccountNo, txtAccountNo.Value, ddlAccountType.SelectedValue, ddlCurrencyCode.SelectedValue);

                LblMessage.Text = "Account No. added successfully.";
                //FunSearchDetails();

                ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3");
                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNo))
                    {

                        DataTable _dtAccountLlinkingDetails = new DataTable();
                        _dtAccountLlinkingDetails = CardAccountDataTable(ObjAPIResponse.AccountNo);
                        if (_dtAccountLlinkingDetails.Rows.Count > 0)
                        {
                            ClsAccountLinkingDelinkingBAL.FunSyncCardAccountLinkingDetails(txtSearchCardNo.Value, Session["IssuerNo"].ToString(), _dtAccountLlinkingDetails);
                        }
                        //hdnCardNo.Value
                        DataTable _dtAccountDetails = new DataTable();
                        _dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, txtSearchCardNo.Value);
                        if (_dtAccountDetails.Rows.Count > 0)
                        {
                            hdnTransactionDetails.Value = createTable(_dtAccountDetails);
                        }
                        else
                        {
                            hdnTransactionDetails.Value = "";
                            LblResult.InnerHtml = "No Record Found";
                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully but failed to sync details. Please try again.";

                }

            }
            else
            {
                LblMessage.Text = "Failed";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
        }
        public APIResponseObject CallISOForAccountOperation(string APIRequest, string RequestType)
        {
            APIResponseObject ObjAPIResponse = new APIResponseObject();

            try
            {

                GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                ConfigDataObject ObjData = new ConfigDataObject();

                DataTable _dtCardAPISourceIdDetails = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();

                objSearch.APIRequest = APIRequest;
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();
                _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);

                ObjData.IssuerNo = Session["IssuerNo"].ToString();
                ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);

                DataTable _dtRequest = new DataTable();
                if (RequestType == "1")//AccountLinkDelink
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    _dtRequest.Columns.Add("LinkingFlag", typeof(string));
                    _dtRequest.Columns.Add("CifId", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { hdnCardNo.Value, hdnAccountNo.Value, hdnAccountType.Value, hdnAccountQuilifier.Value, hdnLinkingflag.Value, hdnCifId.Value });
                }
                else if (RequestType == "2")//Add New Account
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    _dtRequest.Columns.Add("Currency", typeof(string));
                    _dtRequest.Columns.Add("CifId", typeof(string));

                    _dtRequest.Rows.Add(new Object[] { txtSearchCardNo.Value, txtAccountNo.Value, ddlAccountType.SelectedValue, hdnCheckAccQuntifr.Value, ddlCurrencyCode.SelectedValue, hdnCifId.Value });
                    ObjData.IsAccountDetailsSearch = true;
                }
                else
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { hdnCardNo.Value });
                    ObjData.IsAccountDetailsSearch = true;
                }
                _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

            }
            catch (Exception Ex)
            {
                ObjAPIResponse.Status = "140";
                ObjAPIResponse.StatusDesc = "Unexpected error";
                //(new ClsCommonBAL()).SaveErrorLogDetails("AcManagement.aspx.cs, createTable()", Ex.Message, Ex.StackTrace);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinkingRequest>>CallISOForAccountOperation" + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
            }
            return ObjAPIResponse;
        }

        public Tuple<string, string, string> GetAccountLinkingDetails(string[] SpecificAccDet)
        {
            string Linkingflag = string.Empty;
            string AccQulifier = string.Empty;
            string AccType = string.Empty;

            switch (SpecificAccDet[1])
            {
                case "10":
                    AccType = "Saving";
                    break;
                case "20":
                    AccType = "Current";
                    break;
            }

            switch (SpecificAccDet[2])
            {
                case "1":
                    AccQulifier = "Primary";
                    break;
                case "2":
                    AccQulifier = "Secondary";
                    break;
                case "3":
                    AccQulifier = "Tertiary";
                    break;
                case "4":
                    AccQulifier = "Quaternary";
                    break;
                case "5":
                    AccQulifier = "Quinary";
                    break;
            }

            switch (SpecificAccDet[3])
            {
                case "null":
                    Linkingflag = "Link";
                    break;
                default:
                    Linkingflag = "DeLink";
                    break;
            }

            var returnTuple = new Tuple<string, string, string>(
            AccType, AccQulifier, Linkingflag);
            return returnTuple;
        }

        private DataTable DataTableToView(string ResponseString, string CardNo)
        {
            DataTable _dtAccountDetails = new DataTable();
            try
            {
                _dtAccountDetails.Columns.Add("Linkingflag", typeof(string));
                _dtAccountDetails.Columns.Add("CardNo", typeof(string));
                _dtAccountDetails.Columns.Add("AccountNo", typeof(string));
                _dtAccountDetails.Columns.Add("AccountType", typeof(string));
                _dtAccountDetails.Columns.Add("AccountQualifier", typeof(string));
                _dtAccountDetails.Columns.Add("CustomerId", typeof(string));

                string[] Accountdetails = ResponseString.Split('@');
                if (Accountdetails.Length > 0)
                {
                    for (int i = 0; i < Accountdetails.Length; i++)
                    {
                        string[] SpecificAccDet = Accountdetails[i].Split(',');
                        if (SpecificAccDet.Length == 5)
                        {
                            var tupleSpecAccount = GetAccountLinkingDetails(SpecificAccDet);
                            _dtAccountDetails.Rows.Add(new Object[] { tupleSpecAccount.Item3, CardNo, SpecificAccDet[0], tupleSpecAccount.Item1, tupleSpecAccount.Item2, SpecificAccDet[4] });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingRequest CS, DataTableToView()", ex.Message, BankId);
            }
            return _dtAccountDetails;
        }


        private DataTable CardAccountDataTable(string ResponseString)
        {
            DataTable _dtAccountDetails = new DataTable();
            try
            {
                _dtAccountDetails.Columns.Add("CustomerId", typeof(string));
                _dtAccountDetails.Columns.Add("AccountNo", typeof(string));
                _dtAccountDetails.Columns.Add("AccountQualifier", typeof(string));

                string[] Accountdetails = ResponseString.Split('@');
                if (Accountdetails.Length > 0)
                {
                    for (int i = 0; i < Accountdetails.Length; i++)
                    {
                        string[] SpecificAccDet = Accountdetails[i].Split(',');
                        if (SpecificAccDet.Length == 5)
                        {
                            _dtAccountDetails.Rows.Add(new Object[] { SpecificAccDet[4], SpecificAccDet[0], SpecificAccDet[2] });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingRequest CS, CardAccountDataTable()", ex.Message, BankId);
            }
            return _dtAccountDetails;
        }
    }
}