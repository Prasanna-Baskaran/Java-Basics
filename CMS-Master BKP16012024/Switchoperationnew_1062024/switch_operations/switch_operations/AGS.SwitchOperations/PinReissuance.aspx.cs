using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;

namespace AGS.SwitchOperations
{
    public partial class PinReissuance : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsGeneratePrepaidCardBO ObjPrepaid;
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "PRI"; //unique optionneumonic from database
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
                if (!Page.IsPostBack)
                {

                    LblMessage.InnerHtml = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    //FunFillData();
                    DataSet ObjDSOutPut = new DataSet();
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    ObjPrepaid.Mode = "GetPending";

                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().PinReissuanceRequest(ObjPrepaid);

                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        hdnTransactionDetails.Value = ObjDSOutPut.Tables[0].ToHtmlTableString();
                    }
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations, Page_Load()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                Response.Redirect("ErrorPage.aspx", false);
            }
        }


        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {

                string msg = string.Empty;
                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="Card No", Value= txtCardNo.Value, MinLength = 16, MaxLength = 16, Numeric=true},
                };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnValidMsg','ValidateMsgDiv','" + msg + "')", true);
                }
                else
                {

                    //LblMessage.InnerHtml = "<span style='color:green'>Request sent successfully!</span>";
                    //txtCardNo.Value = "";
                    //return;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                    {


                        LblMessage.InnerHtml = "";
                        ObjPrepaid = new ClsGeneratePrepaidCardBO();
                        if (!string.IsNullOrEmpty(txtCardNo.Value))
                        {
                            ObjPrepaid.NoOfCards = (txtCardNo.Value);
                        }

                        ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                        ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                        ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                        ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                        ObjPrepaid.Mode = "INSERTREQUEST";

                        DataSet ObjDSOutPut = new DataSet();
                        ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().PinReissuanceRequest(ObjPrepaid);

                        if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                            {
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Pin reissuance Request", "Request to Pin reissuance to Card No :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "1");
                                LblMessage.InnerHtml = "<span style='color:green'>Request sent successfully!</span>";
                                if (ObjDSOutPut.Tables[1].Rows.Count > 0)
                                {
                                    hdnTransactionDetails.Value = ObjDSOutPut.Tables[1].ToHtmlTableString();// "", new AddedTableData[] { new AddedTableData() });
                                }
                            }
                            else
                            {
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Pin reissuance Request", "Failed to Request to Pin reissuance to Card No :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "0");
                                LblMessage.InnerHtml = "<span style='color:red'>Error while Pin reissuance request </br>" + ObjDSOutPut.Tables[0].Rows[0][0].ToString() + "</span>";
                            }
                        }
                        else
                        {
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Pin reissuance Request", "Failed to Request to Pin reissuance to  Card No :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "0");
                            LblMessage.InnerHtml = "<span style='color:red'>Error while Pin reissuance request</span>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations,btnGenerate_Click()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblMessage.InnerHtml = "Error Occure";
            }
        }
    }
}