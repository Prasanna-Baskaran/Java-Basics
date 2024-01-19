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
    public partial class GenerateInstaCardMaker : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsGeneratePrepaidCardBO ObjPrepaid;
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "GICR"; //unique optionneumonic from database
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
                    FunFillData();

                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations, Page_Load()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                Response.Redirect("ErrorPage.aspx", false);
            }
        }


        public void FunFillData()
        {
            try
            {
                ObjPrepaid = new ClsGeneratePrepaidCardBO();
                ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                ObjPrepaid.Mode = "GetCardProgram";
                ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);

                DataSet ObjDSOutPut = new DataSet();
                ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GeneratePrepaidCard_Operations(ObjPrepaid);
                if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                {
                    ddlCardProgram.DataSource = ObjDSOutPut.Tables[0];
                    ddlCardProgram.DataValueField = "CardProgram";
                    ddlCardProgram.DataBind();                 
                }
                ddlCardProgram.Items.Insert(0, new ListItem("--Select--", "0"));
                if (ObjDSOutPut.Tables[1].Rows.Count > 0)
                {
                    hdnTransactionDetails.Value = ObjDSOutPut.Tables[1].ToHtmlTableString();//"", new AddedTableData[] { new AddedTableData() });
                }

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog(" AGS.SwitchOperations,FunGetdropdown()", ex.Message, Convert.ToString(Session["IssuerNo"]));
            }

        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;

                List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                {
                    new ValidatorAttr { Name="Card Program", Value= ddlCardProgram.SelectedValue, MinLength = 1, MaxLength = 8, Numeric = true, Regex="^[0-9]*$"},
                    new ValidatorAttr { Name="Number of Cards", Value= txtNoOfCards.Value, MinLength = 1, MaxLength = 4, Numeric = true },
                    new ValidatorAttr { Name="Number of Cards", Value= txtNoOfCards.Value, Regex="^(([1-4][0-9]{0,3})|([1-9][0-9]{0,2})|(5000))$" },
                };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {


                    if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                    {


                        LblMessage.InnerHtml = "";
                        ObjPrepaid = new ClsGeneratePrepaidCardBO();
                        if (!string.IsNullOrEmpty(txtNoOfCards.Value))
                        {
                            ObjPrepaid.NoOfCards = (txtNoOfCards.Value);
                        }
                        if (!string.IsNullOrEmpty(ddlCardProgram.SelectedValue))
                        {
                            ObjPrepaid.CardProgram = (ddlCardProgram.SelectedValue);
                        }
                        ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                        ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                        ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                        ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                        ObjPrepaid.Mode = "INSERTREQUEST";

                        DataSet ObjDSOutPut = new DataSet();
                        ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GeneratePrepaidCard_Operations(ObjPrepaid);

                        if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                            {
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Generate Insta Card Request", "Request to instant card generate for | CardProgram : " + ObjPrepaid.CardProgram + " | No of Cards :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "1");
                                LblMessage.InnerHtml = "<span style='color:green'>Request sent successfully!</span>";
                                if (ObjDSOutPut.Tables[1].Rows.Count > 0)
                                {
                                    hdnTransactionDetails.Value = ObjDSOutPut.Tables[1].ToHtmlTableString();// "", new AddedTableData[] { new AddedTableData() });
                                }
                            }
                            else
                            {
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Generate Insta Card Request", "Failed to request instant card generate for | CardProgram : " + ObjPrepaid.CardProgram + " | No of Cards :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "0");
                                LblMessage.InnerHtml = "<span style='color:red'>Error while generating request</span>";
                            }
                        }
                        else
                        {
                            ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Generate Insta Card Request", "Failed to request instant card generate for | CardProgram : " + ObjPrepaid.CardProgram + " | No of Cards :" + ObjPrepaid.NoOfCards, "", "", "", "", "", "", "Submit", "0");
                            LblMessage.InnerHtml = "<span style='color:red'>Error while generating request</span>";
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