using AGS.SwitchOperations.BusinessLogics;
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
    public partial class TermDenominationCountChecker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string StrAccessCaption = string.Empty;
                string[] _aQueryString;
                string _tid = string.Empty;
                string _resultOutput = string.Empty;

                string OptionNeumonic = "TDCUC"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {


                        if (Request.QueryString["Fn"] != null)
                        {
                            _aQueryString = Convert.ToString(Request.QueryString["Fn"]).Split(new char[] { '|' });
                            switch (Convert.ToString(_aQueryString[0]))
                            {
                                case "TDCUSubmit":
                                    if (Request.Form["tid"] != "" && Request.Form["tid"] != null)
                                    {
                                        _tid = Convert.ToString(Request.Form["tid"]);
                                        if (new ClsCommonBAL().FunProcessTermDenominationDetails("2", Session["ParticipantId"].ToString(), _tid, Convert.ToString(Session["LoginID"]), "", "", "", "", "", "", "", "", Request.Form["RequestType"], Convert.ToInt32(Request.Form["TermDenominationId"]), "", "", ""))
                                        {
                                            if (Request.Form["RequestType"] == "Approve")
                                            {
                                                _resultOutput = "Request approved successfully!|";
                                            }
                                            else if (Request.Form["RequestType"] == "Reject")
                                            {
                                                _resultOutput = "Request rejected successfully!|";
                                            }
                                        }
                                        else
                                        {
                                            _resultOutput = "Request Failed!|";
                                        }
                                    }
                                    break;
                            }
                            //sOutput = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(sOutput));
                            Response.Write(_resultOutput);
                        }
                        else
                        {
                            FunGetResult();
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
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("TermDenominationCountChecker.Page_Load()", ex.Message, ex.StackTrace);
            }
        }
        public void FunGetResult()
        {
            DataTable _dtOutPut = new DataTable();
            try
            {
                _dtOutPut = new ClsCommonBAL().FunGetTerminalDetails(Session["ParticipantId"].ToString(), "", "checker");
                //int RoleID = Convert.ToInt16(Session["UserRoleID"]);                            
                if (_dtOutPut.Rows.Count > 0)
                {
                    AddedTableData[] objAdded = new AddedTableData[1];
                    objAdded[0] = new AddedTableData()
                    {
                        control = AGS.Utilities.Controls.Button,
                        buttonName = "View Request",
                        cssClass = "btn btn-primary"
                                          ,
                        hideColumnName = true,
                        events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                          ,
                        attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "TerminalID", BindTableColumnValueWithAttribute = "TerminalID" }
                                                  , new AGS.Utilities.Attribute() { AttributeName = "Status", BindTableColumnValueWithAttribute = "Status" }
                                                  , new AGS.Utilities.Attribute() { AttributeName = "Id", BindTableColumnValueWithAttribute = "Id" }
                                              , new AGS.Utilities.Attribute() { AttributeName = "Cassete1Count", BindTableColumnValueWithAttribute = "Cassete1Count" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete2Count", BindTableColumnValueWithAttribute = "Cassete2Count" } 
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete3Count", BindTableColumnValueWithAttribute = "Cassete3Count" } 
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete4Count", BindTableColumnValueWithAttribute = "Cassete4Count" } 

                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete1Deno", BindTableColumnValueWithAttribute = "Cassete1Deno" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete2Deno", BindTableColumnValueWithAttribute = "Cassete2Deno" } 
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete3Deno", BindTableColumnValueWithAttribute = "Cassete3Deno" } 
                        , new AGS.Utilities.Attribute() { AttributeName = "Cassete4Deno", BindTableColumnValueWithAttribute = "Cassete4Deno" } 
                        
                                     }
                    };

                    hdnTransactionDetails.Value = _dtOutPut.ToHtmlTableString("Id,Cassete1Deno,Cassete2Deno,Cassete3Deno,Cassete4Deno", objAdded);

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("TermDenominationCountChecker.FunGetResult()", ex.Message, ex.StackTrace);
            }
        }
    }
}