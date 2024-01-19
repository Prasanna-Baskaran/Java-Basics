using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class AccountLinkingAuth : System.Web.UI.Page
    {
        ClsCommonDAL commonDal = new ClsCommonDAL();
        string StrAccessCaption = string.Empty;
        string Status = string.Empty;
        string Msg = string.Empty;
        string Description = string.Empty;
        string SessionId = string.Empty;
        string BankId = string.Empty;
        Boolean SkipDialogBox;
        bool LinkDelinkFlag;
        ClsGeneratePrepaidCardBO ObjPrepaid;

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //BankId = Session["BankID"].ToString();
            try
            {
                string OptionNeumonic = "ALSA"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {

                        if (!IsPostBack)
                        {
                            LblResult.InnerHtml = "";
                            FunGetResult("GETPENDING");
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
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("ALSA, AccountLinkingAuth, Page_Load()", ObjEx.Message, "");
                Response.Redirect("ErrorPage.aspx", false);
            }

        }

        protected void FunGetResult(String Mode)
        {
            try
            {
                ObjPrepaid = new ClsGeneratePrepaidCardBO();
                ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                ObjPrepaid.Mode = Mode;
                ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                DataSet ObjDSOutPut = new DataSet();
                ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GetSetAccountLinkrequestOperations(ObjPrepaid);
                if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                {

                    BindTable(ObjDSOutPut.Tables[0]);

                    //AddedTableData[] objAdded = new AddedTableData[1];
                    //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
                    //hdnAccLinkDetails.Value = ObjDSOutPut.Tables[0].ToHtmlTableString("[ID],[AccountNo],[CustomerId],[CardNo],[Linkingflag],[AccountType],[AccountQualifier],[issuerNo],[uploadBy]", objAdded);
                    Reject_Btn.Visible = true;
                    BtnSave.Visible = true;
                    select_all.Visible = true;
                    LBLselect_all.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunHideinfo()", true);
                }
                else
                {
                    if (Mode == "GETPENDING")
                    {
                        LblResult.InnerHtml = "<span style='color:red'>No request for Account linking</span>";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo();", true);
                        Table_Request.InnerHtml = "";
                    }
                    Reject_Btn.Visible = false;
                    BtnSave.Visible = false;
                    select_all.Visible = false;
                    LBLselect_all.Visible = false;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunHideinfo()", true);
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingAuth,FunGetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblMessage.Text = "Error while getting data";
                BtnSave.Visible = false;
                Reject_Btn.Visible = false;
                select_all.Visible = false;
                LBLselect_all.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            }

        }

        protected void Reject_Btn_Click(object sender, EventArgs e)
        {
            //SetResult DataTable start
            try
            {
                var records = hdnISOData.Value.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (records.Length > 0)
                {
                    LblMessage.Text = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    //ObjPrepaid.RequestIDs = records[i].Split('|');//(hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);


                    DataTable _dtResult = new DataTable();
                    _dtResult.Columns.Add("id", typeof(Int32));
                    _dtResult.Columns.Add("Reason", typeof(string));
                    ObjPrepaid.Mode = "RejetedByAuth";

                    foreach (string id in hdnISOData.Value.Split(','))
                    {
                        _dtResult.Rows.Add(new Object[] { id.Split('|')[0], txtRejectReson.Text });
                    }

                    ObjPrepaid.DtBulkData = _dtResult;
                    //SetResult DataTable end

                    // Set Reject Reason
                    FunSetResult(ObjPrepaid);
                    FunGetResult("GETPENDING");
                    //LblResult.InnerHtml = "<span style='color:green'>Rejected successfully!</span>";
                    LblMessage.Text = "Rejected successfully!";
                }
                // Get Pending Requests
                txtRejectReson.Text = "";
                select_all.Checked = false;
                //FunGetResult("GETPENDING");
            }
            catch (Exception exe)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingAuth,Reject_Btn_Click()", exe.Message, Convert.ToString(Session["IssuerNo"]));
                LblMessage.Text = LblResult.InnerHtml = "Error Occured";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "javascript", "$(function(){$('#memberModal').modal('show');});", true);
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
                            }
                            if (currentRow[columnNames[0]].ToString().ToUpper() == "DELINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Delink'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs ' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='1' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' CifId='" + currentRow[5].ToString() + "' > Link </ asp:Button >" + strTDEnd);
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinkingAuth " + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
                return "";
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtCardAPISourceIdDetails = new DataTable();
                CustSearchFilter objSearch = new CustSearchFilter();

                //objSearch.APIRequest = "Acclinking";  /*$@chin 14/02/2023*/
                objSearch.APIRequest = "CustomerDataUpdateInsta";
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();
                _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);


                ConfigDataObject ObjData = new ConfigDataObject();
                ObjData.IssuerNo = Session["IssuerNo"].ToString();
                ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);

                DataTable _dtRequest = new DataTable();
                _dtRequest.Columns.Add("CardNo", typeof(string));
                _dtRequest.Columns.Add("AccountNo", typeof(string));
                _dtRequest.Columns.Add("AccountType", typeof(string));
                _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                _dtRequest.Columns.Add("LinkingFlag", typeof(string));
                _dtRequest.Columns.Add("CifId", typeof(string));
                _dtRequest.Columns.Add("CustomerId", typeof(string));

                _dtRequest.Columns.Add("AccountData", typeof(string));
                _dtRequest.Columns.Add("CIFDATA", typeof(string));

                _dtRequest.Columns.Add("CustomerName", typeof(string));
                _dtRequest.Columns.Add("LastName", typeof(string));
                _dtRequest.Columns.Add("FullName", typeof(string));
                _dtRequest.Columns.Add("EmailId", typeof(string));
                _dtRequest.Columns.Add("MobileNo", typeof(string));
                _dtRequest.Columns.Add("NICNumber", typeof(string));
                _dtRequest.Columns.Add("MotherName", typeof(string));
                _dtRequest.Columns.Add("Address1", typeof(string));
                _dtRequest.Columns.Add("Address2", typeof(string));
                //_dtRequest.Columns.Add("Address3", typeof(string));
                //_dtRequest.Columns.Add("Address4", typeof(string));
                //_dtRequest.Columns.Add("Address5", typeof(string));
                _dtRequest.Columns.Add("DateOfBirth", typeof(string));



                var records = hdnISOData.Value.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (records.Length > 0)
                {
                    foreach (string record in records)
                    {
                        if (record.Split('|')[5] == "2")
                        {
                            _dtRequest.Rows.Add(new Object[] { record.Split('|')[1], record.Split('|')[2], record.Split('|')[3], record.Split('|')[4], record.Split('|')[5],record.Split('|')[6],record.Split('|')[6],
                            "5992|"+record.Split('|')[2]+ "|" +record.Split('|')[3]+ "|" +record.Split('|')[4]+"|"+record.Split('|')[6],
                            record.Split('|')[7], record.Split('|')[8], record.Split('|')[9], record.Split('|')[10], record.Split('|')[11], record.Split('|')[12], record.Split('|')[13], record.Split('|')[14], record.Split('|')[15], record.Split('|')[16]});

                        }
                        else
                        {
                            //_dtRequest.Rows.Add(new Object[] { record.Split('|')[1], record.Split('|')[2], record.Split('|')[3], record.Split('|')[4], record.Split('|')[5], record.Split('|')[6] });
                            //_dtRequest.Rows.Add(new Object[] { record.Split('|')[1], record.Split('|')[2], record.Split('|')[3], record.Split('|')[4], record.Split('|')[5], record.Split('|')[6], record.Split('|')[7], record.Split('|')[8], record.Split('|')[9], record.Split('|')[10], record.Split('|')[11], record.Split('|')[12], record.Split('|')[13], record.Split('|')[14], record.Split('|')[15], record.Split('|')[16], record.Split('|')[17] });

                            /* Edited by Mangesh 
                            _dtRequest.Rows.Add(new Object[] { record.Split('|')[1], record.Split('|')[2], record.Split('|')[3], record.Split('|')[4], record.Split('|')[5],record.Split('|')[6],record.Split('|')[6],
                            record.Split('|')[2]+ "|" +record.Split('|')[3]+ "|" +record.Split('|')[4],
                            record.Split('|')[6]+"," +record.Split('|')[11]+ ",,"+record.Split('|')[7]+",,"+record.Split('|')[16]+"," +  record.Split('|')[7]+ ",," + record.Split('|')[12]+",,,,,,,,,," + record.Split('|')[10]+ ",," + record.Split('|')[9]+ "," + record.Split('|')[13]+ "," + record.Split('|')[14]+ ",,,,,,,,,,," + DateTime.ParseExact(record.Split('|')[15],"dd-mm-yyyy",CultureInfo.InvariantCulture).ToString("yyyymmdd",CultureInfo.InvariantCulture)+ ",,,0,,||",
                            record.Split('|')[7], record.Split('|')[8], record.Split('|')[9], record.Split('|')[10], record.Split('|')[11], record.Split('|')[12], record.Split('|')[13], record.Split('|')[14], record.Split('|')[15], record.Split('|')[16] });
                        */
                            _dtRequest.Rows.Add(new Object[] { record.Split('|')[1], record.Split('|')[2], record.Split('|')[3], record.Split('|')[4], record.Split('|')[5],record.Split('|')[6],record.Split('|')[6],
                            record.Split('|')[2]+ "|" +record.Split('|')[3]+ "|" +record.Split('|')[4],
                            record.Split('|')[6]+"," +record.Split('|')[12]+ ",,"+record.Split('|')[7]+",,"+record.Split('|')[8]+"," +  record.Split('|')[7]+ ",," + record.Split('|')[13]+",,,,,,,,,," + record.Split('|')[11]+ ",," + record.Split('|')[10]+ "," + record.Split('|')[14]+ "," + record.Split('|')[15]+ ",,,,,,,,,,," + DateTime.ParseExact(record.Split('|')[16],"dd-mm-yyyy",CultureInfo.InvariantCulture).ToString("yyyymmdd",CultureInfo.InvariantCulture)+ ",,,0,,||",
                            record.Split('|')[7], record.Split('|')[9], record.Split('|')[10], record.Split('|')[11], record.Split('|')[12], record.Split('|')[13], record.Split('|')[14], record.Split('|')[15], record.Split('|')[16], record.Split('|')[8] });
                        }
                    }

                    //SetResult DataTable start

                    LblMessage.Text = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    //ObjPrepaid.RequestIDs = records[i].Split('|');//(hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);

                    ObjPrepaid.Mode = "Authorised";

                    DataTable _dtResult = new DataTable();
                    _dtResult.Columns.Add("id", typeof(Int32));
                    _dtResult.Columns.Add("Reason", typeof(string));

                    //SetResult DataTable end

                    int index = 0;
                    string Linked = "", DeLinked = "", Failed = "";

                    foreach (DataRow row in _dtRequest.Rows)
                    {
                        APIResponseObject ObjAPIResponse = new APIResponseObject();
                        GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();

                        // Call API for each request
                        _GenerateCardAPIRequest.CallCardAPIService(row, _dtRequest, ObjData, ObjAPIResponse);

                        // Add response in Result Datatable
                        if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            if (row["LinkingFlag"].ToString() == "1")
                            {
                                _dtResult.Rows.Add(new Object[] { records[index].Split('|')[0], "Account Linked Successfully" });
                                Linked = Linked + "," + row["AccountNo"].ToString() + "   Account Linked Successfully";
                            }
                            else
                            {
                                _dtResult.Rows.Add(new Object[] { records[index].Split('|')[0], "Account De-Linked Successfully" });
                                DeLinked = DeLinked + "," + row["AccountNo"].ToString() + "   Account De-Linked Successfully";
                            }


                            ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3", row["CardNo"].ToString(), row["CustomerId"].ToString());
                            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber1))
                                {

                                    DataTable _dtAccountLlinkingDetails = new DataTable();
                                    string qulifer = string.Empty;
                                    string cif = string.Empty;
                                    foreach (string record in records)
                                    {
                                        qulifer = record.Split('|')[5];
                                        cif = record.Split('|')[6];
                                    }

                                    string result = cif + "@" + ObjAPIResponse.AccountNumber1 + "@" + qulifer;
                                    _dtAccountLlinkingDetails = CardAccountDataTable(result);
                                    //_dtAccountLlinkingDetails = CardAccountDataTable(ObjAPIResponse.AccountNumber1);
                                    if (_dtAccountLlinkingDetails.Rows.Count > 0)
                                    {
                                        ClsAccountLinkingDelinkingBAL.FunSyncCardAccountLinkingDetails(row["CardNo"].ToString(), Session["IssuerNo"].ToString(), _dtAccountLlinkingDetails);
                                    }

                                    //DataTable _dtAccountDetails = new DataTable();
                                    //_dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, hdnCardNo.Value);
                                    //if (_dtAccountDetails.Rows.Count > 0)
                                    //{
                                    //    hdnTransactionDetails.Value = createTable(_dtAccountDetails);
                                    //}
                                    //else
                                    //{
                                    //    hdnTransactionDetails.Value = "";
                                    //    LblResult.InnerHtml = "No Record Found";
                                    //}
                                }
                            }
                            else
                            {
                                LblMessage.Text = "Account De-Linked Successfully but failed to sync details. Please try again.";

                            }


                        }
                        else
                        {
                            _dtResult.Rows.Add(new Object[] { records[index].Split('|')[0], "Failed to " + ((row["LinkingFlag"].ToString() == "1") ? "Link" : "De-Link") + "Account" });
                            Failed = Failed + "," + row["AccountNo"].ToString();
                        }
                        index++;
                    }

                    // Result Datatable set to object 
                    ObjPrepaid.DtBulkData = _dtResult;
                    //LblResult.InnerText = "Linked : " + ((Linked.Length>0)?Linked.Remove(0, 1):"") + "<br />Delinked : " + ((DeLinked.Length > 0) ? DeLinked.Remove(0, 1):"") + "<br />Failed : " + ((Failed.Length > 0) ? Failed.Remove(0, 1):"");
                    // Update Response to DB
                    FunSetResult(ObjPrepaid);
                    FunGetResult("GETPENDING");
                    LblMessage.Text = "Linked : " + ((Linked.Length > 0) ? Linked.Remove(0, 1) : "NA") + "<br />Delinked : " + ((DeLinked.Length > 0) ? DeLinked.Remove(0, 1) : "NA") + "<br />Failed : " + ((Failed.Length > 0) ? Failed.Remove(0, 1) : "NA");
                    // Get Pending Requests
                    //FunGetResult("GETPENDING");
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception exe)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingAuth,BtnSave_Click()", exe.Message, Convert.ToString(Session["IssuerNo"]));
                LblMessage.Text = LblResult.InnerHtml = "Error Occured";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "javascript", "$(function(){$('#memberModal').modal('show');});", true);
        }
        public APIResponseObject CallISOForAccountOperation(string APIRequest, string RequestType, string CardNo, string Cifid)
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
                _dtRequest.Columns.Add("CardNo", typeof(string));
                _dtRequest.Columns.Add("Cifid", typeof(string));
                //_dtRequest.Rows.Add(new Object[] { CardNo });                
                _dtRequest.Rows.Add(new Object[] { CardNo, Cifid });

                ObjData.IsAccountDetailsSearch = true;
                _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

            }
            catch (Exception Ex)
            {
                ObjAPIResponse.Status = "140";
                ObjAPIResponse.StatusDesc = "Unexpected error";
                //(new ClsCommonBAL()).SaveErrorLogDetails("AcManagement.aspx.cs, createTable()", Ex.Message, Ex.StackTrace);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinking>>CallISOForAccountOperation" + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
            }
            return ObjAPIResponse;
        }

        public void FunSetResult(ClsGeneratePrepaidCardBO objPrepaid)
        {
            ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
            ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                {

                    DataSet ObjDSOutPut = new DataSet();
                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().AcceptRejectAccountlink(objPrepaid);
                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(ObjDSOutPut.Tables[0].Rows[0][0]) > 0)
                        {
                            if (ObjDSOutPut.Tables[1].Rows.Count > 0)
                            {
                                BindTable(ObjDSOutPut.Tables[1]);
                                //AddedTableData[] objAdded = new AddedTableData[1];
                                //objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
                                //hdnAccLinkDetails.Value = ObjDSOutPut.Tables[0].ToHtmlTableString("[ID],[AccountNo],[CustomerId],[CardNo],[Linkingflag],[AccountType],[AccountQualifier],[issuerNo],[uploadBy]", objAdded);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "noData", ";$(function(){$('#DivResult').hide();});", true);
                            }
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
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingAuth,FunSetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblResult.InnerHtml = "Error Occured";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "$(function(){$('#memberModal').modal('show');});", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

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
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingAuth CS, DataTableToView()", ex.Message, BankId);
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

                _dtAccountDetails.Rows.Add(new Object[] { Accountdetails[0], Accountdetails[1], Accountdetails[2] });

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
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingAuth CS, CardAccountDataTable()", ex.Message, BankId);
            }
            return _dtAccountDetails;
        }

        protected DataTable DatatableToView(DataTable _DT, DataTable SourceDT, string CardNo, string NICNo)
        {
            try
            {

                if (_DT.Columns.Count == 0)
                {
                    _DT = SourceDT.Clone();
                }

                foreach (DataRow row in SourceDT.Rows)
                {
                    if (row["CardNo"].ToString().Equals(CardNo == null ? "" : CardNo.ToString()) && row["NICNo"].ToString().Equals(NICNo == null ? "" : NICNo.ToString()))
                    {
                        //_DT.Rows.Add(new Object[] {row["CardNo"],row["NICNo"]
                        //    });
                        _DT.Rows.Add(row.ItemArray);
                    }
                }



                return _DT;

            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, btnSearch_Click, DatatableToView()", ObjEx.Message, "");
                return _DT;
            }

        }

        public void BindTable(DataTable dataTable)
        {
            //HtmlGenericControl Table_Request = FindControl("Table_Request") as HtmlGenericControl;
            Table_Request.InnerHtml = "";
            var DistinctCards = (from DataRow dRow in dataTable.Rows
                                 select new
                                 {
                                     col1 = dRow["CardNo"]
                                 }).Distinct();
            int inx = 0;
            foreach (var rowCard in DistinctCards)
            {
                Table_Request.InnerHtml = Table_Request.InnerHtml + "<div id='acco_" + inx + "' class='accordian'> <b>Card No : </b>" + rowCard.col1.ToString() + "<span class='glyphicon glyphicon-plus-sign'></span><span class='glyphicon glyphicon-minus-sign' style='display:none'></span></div><div class='acco_" + inx + " panel'>";

                AddedTableData[] objAdded = new AddedTableData[1];
                objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
                Table_Request.InnerHtml = Table_Request.InnerHtml + dataTable.AsEnumerable()
                    .Where(row => row.Field<String>("CardNo") == rowCard.col1.ToString())
                    //.OrderByDescending(row => row.Field<String>("CardNo"))
                    .CopyToDataTable()
                    .ToHtmlTableString("", objAdded);

                Table_Request.InnerHtml = Table_Request.InnerHtml + "</div>";
                inx++;
            }

        }
    }
}