using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class TermDenominationCountMaker : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "TDCUM"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {


                        string[] _aQueryString;
                        string _tid = string.Empty;
                        string _returnOutput = string.Empty;

                        if (Request.QueryString["Fn"] != null)
                        {
                            _aQueryString = Convert.ToString(Request.QueryString["Fn"]).Split(new char[] { '|' });
                            switch (Convert.ToString(_aQueryString[0]))
                            {
                                case "TDCU":
                                    if (Request.Form["tid"] != "" && Request.Form["tid"] != null)
                                    {
                                        _tid = Convert.ToString(Request.Form["tid"]);

                                        DataTable dtOutput = new ClsCommonBAL().FunGetTerminalDetails(Session["ParticipantId"].ToString(), _tid, "maker");

                                        if (dtOutput != null)
                                        {
                                            if (dtOutput.Rows.Count > 0)
                                            {
                                                _returnOutput = Convert.ToString(dtOutput.Rows[0]["id"]) + "~" + Convert.ToString(dtOutput.Rows[0]["Current Status"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_1"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_1_count"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_2"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_2_count"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_3"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_3_count"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_4"]) + "~" + Convert.ToString(dtOutput.Rows[0]["cassette_4_count"]) + "~" + Convert.ToString(dtOutput.Rows[0]["term_type"]) + "~" + Convert.ToString(dtOutput.Rows[0]["card_acceptor"]) + "~" + Convert.ToString(dtOutput.Rows[0]["currency_code"] + "|");
                                            }
                                        }
                                    }
                                    break;
                                case "TDCUSubmit":
                                    if (Request.Form["tid"] != "" && Request.Form["tid"] != null)
                                    {
                                        _tid = Convert.ToString(Request.Form["tid"]);
                                        string[] _switchExtraFields = Convert.ToString(Request.Form["SwitchExtraFields"]).Split('|');

                                        if (new ClsCommonBAL().FunProcessTermDenominationDetails("1", Convert.ToString(Session["ParticipantId"]), _tid, Convert.ToString(Session["LoginID"]), Convert.ToString(Request.Form["Cassete1Deno"]), Convert.ToString(Request.Form["Cassete1Count"]), Convert.ToString(Request.Form["Cassete2Deno"]), Convert.ToString(Request.Form["Cassete2Count"]), Convert.ToString(Request.Form["Cassete3Deno"]), Convert.ToString(Request.Form["Cassete3Count"]), Convert.ToString(Request.Form["Cassete4Deno"]), Convert.ToString(Request.Form["Cassete4Count"]), "", 0, _switchExtraFields[0], _switchExtraFields[1], _switchExtraFields[2]))
                                        {
                                            _returnOutput = "Request submited successfully!|";
                                        }
                                        else
                                        {
                                            _returnOutput = "Request Failed!|";
                                        }
                                    }
                                    break;
                            }
                            //sOutput = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(sOutput));
                            Response.Write(_returnOutput);
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("TermDenominationCountMaker.Page_Load()", ex.Message, ex.StackTrace);
            }
        }

        public void FunGetResult()
        {
            DataTable _dtOutPut = new DataTable();
            try
            {
                _dtOutPut = new ClsCommonBAL().FunGetTerminalDetails(Session["ParticipantId"].ToString(), "", "maker");
                if (_dtOutPut.Rows.Count > 0)
                {
                    string[] _arrColumns = { "Terminal ID", "Location", "Card Acceptor", "Current Status" };
                    DataView _dvConvert = new DataView(_dtOutPut);
                    _dtOutPut = _dvConvert.ToTable(true, _arrColumns);

                    AddedTableData[] objAdded = new AddedTableData[1];
                    objAdded[0] = new AddedTableData()
                    {
                        control = AGS.Utilities.Controls.Button,
                        buttonName = "View Details",
                        cssClass = "btn btn-primary",
                        hideColumnName = true,
                        events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                          ,
                        attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "TerminalID", BindTableColumnValueWithAttribute = "Terminal ID" }
                                              , new AGS.Utilities.Attribute() { AttributeName = "CurrentStatus", BindTableColumnValueWithAttribute = "CurrentStatus" }
                        , new AGS.Utilities.Attribute() { AttributeName = "Location", BindTableColumnValueWithAttribute = "Location" }
                        
                                     }
                    };

                    hdnTransactionDetails.Value = _dtOutPut.ToHtmlTableString("", objAdded);

                }
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("TermDenominationCountMaker.FunGetResult()", ex.Message, ex.StackTrace);
            }
        }


    }
}