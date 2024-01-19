using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class LimitSetAdvisory : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string OptionNeumonic = "LSAE"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;

                        FunGetResult();
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
            catch (Exception)
            {
                // new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardTypeMaster, Page_Load()", ObjEx.Message, "");
            }
        }


        public void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(28, Session["BankID"] + "," + Session["SystemID"]);
            //            int RoleID = Convert.ToInt16(Session["UserRoleID"]);
            //            hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
            AddedTableData[] objAdded = new AddedTableData[1];
            objAdded[0] = new AddedTableData()
            {
                control = AGS.Utilities.Controls.Button,
                buttonName = "Edit",
                cssClass = "btn btn-primary"
                                  ,
                hideColumnName = true,
                events = new Event[] { new Event() { EventName = "onclick", EventValue = "funEditClick($(this));" } }
                                  ,
                attributes = new AGS.Utilities.Attribute[]
                                  { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "ID" }

                        , new AGS.Utilities.Attribute() { AttributeName = "BIN", BindTableColumnValueWithAttribute = "BIN" }

                        , new AGS.Utilities.Attribute() { AttributeName = "ThresholdLimit", BindTableColumnValueWithAttribute = "ThresholdLimit" } 
                        , new AGS.Utilities.Attribute() { AttributeName = "Mobileno", BindTableColumnValueWithAttribute = "MobileNo" }
                        , new AGS.Utilities.Attribute() { AttributeName = "EmailID", BindTableColumnValueWithAttribute = "EmailID" }
                        , new AGS.Utilities.Attribute() { AttributeName = "EmailTamplate", BindTableColumnValueWithAttribute = "EmailIDTamplate" }
                        , new AGS.Utilities.Attribute() { AttributeName = "SMSTamplate", BindTableColumnValueWithAttribute = "SMSTamplate" }
                                  }
            };

            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("ID", objAdded);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsLimitSetAdvisoryBO ObjFilter = new ClsLimitSetAdvisoryBO();

                if (!string.IsNullOrEmpty(hdnID.Value))
                {
                    ObjFilter.ID = Convert.ToInt16(hdnID.Value);
                }
                ObjFilter.BIN = Convert.ToString(txtBINID.Value);
                ObjFilter.ThresholdLimit = Convert.ToDouble(txtThresholdLIMIT.Value);
                if (ObjFilter.ThresholdLimit<=0 || ObjFilter.ThresholdLimit>=100)
                {
                    LblMsg.InnerText = "Please Enter Correct Percentage Values !";
                    ObjReturnStatus.Code = 1;
                }
                
                else
                {
                    ObjFilter.SystemID = Session["SystemID"].ToString();
                    ObjFilter.BankID = Session["BankID"].ToString();
                    ObjFilter.USERID = Session["LoginID"].ToString();
                    ObjReturnStatus = new ClsLimitSetAdvisory().FunSaveLimitSetAdvisoryDetails(ObjFilter);
                    LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                }
                

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";
                     FunGetResult();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, LimitSetAdvisory, btnSave_Click()", ObjEx.Message, "");
            }
        }
        protected void btnBankSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsLimitSetAdvisoryBO ObjFilter = new ClsLimitSetAdvisoryBO();

                if (!string.IsNullOrEmpty(hdnID.Value))
                {
                    ObjFilter.ID = Convert.ToInt16(hdnID.Value);
                }
                ObjFilter.EmailId = Convert.ToString(txtEmailID.Value);
                ObjFilter.MobileNo = Convert.ToString(TxtMobileNoID.Value);
                ObjFilter.EmailTamplate = Convert.ToString(txtEmailTamplateID.Value);
                ObjFilter.SMSTamplate = Convert.ToString(TxtSMSTamplateID.Value);
                ObjFilter.ThresholdLimit = Convert.ToDouble(txtThresholdID.Value);
                if (ObjFilter.ThresholdLimit <= 0 || ObjFilter.ThresholdLimit >= 100)
                {
                    LblMsg.InnerText = "Please Enter Correct Percentage Values !";
                    ObjReturnStatus.Code = 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    return;
                }


                string InvalidInput=string.Empty;
                if(!String.IsNullOrEmpty(ObjFilter.EmailId))
                {

                  InvalidInput = Validatore(ObjFilter.EmailId,@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)");
                  if(!string.IsNullOrEmpty(InvalidInput))
                  {
                    LblMsg.InnerText = "Invalid Email ID  !" + InvalidInput;
                    ObjReturnStatus.Code = 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    return;
                  }
                }
                if(!String.IsNullOrEmpty(ObjFilter.MobileNo))
                {
                    InvalidInput=Validatore(ObjFilter.MobileNo,"^([789])\\d{9}$");
                    if(!string.IsNullOrEmpty(InvalidInput))
                  {
                    LblMsg.InnerText = LblMsg.InnerText = "Invalid Mobile No  !" + InvalidInput; 
                    ObjReturnStatus.Code = 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    return;
                  }
                }
                
                
                    ObjFilter.BIN = "RBL";
                    ObjFilter.SystemID = Session["SystemID"].ToString();
                    ObjFilter.BankID = Session["BankID"].ToString();
                    ObjFilter.USERID = Session["LoginID"].ToString();
                    ObjReturnStatus = new ClsLimitSetAdvisory().FunSaveLimitSetAdvisoryDetails(ObjFilter);
                    LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                
                
                

                
                if (ObjReturnStatus.Code == 0)
                {
                    
                    hdnResultStatus.Value = "1";
                    FunGetResult();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, LimitSetAdvisory, btnSave_Click()", ObjEx.Message, "");
            }
        }
        protected string Validatore(string InputToValidate,string RegulerExpression)
        {
            try
            {
                
                string  [] ArrInput=InputToValidate.Split(',');
                foreach (var item in ArrInput)
	            {
                    if (item.ToString().Contains('@') && item.ToString().Split('@').Count() > 2)
                 {
                     return InputToValidate;
                 }
                 if (!Regex.IsMatch(item.ToString(), RegulerExpression,RegexOptions.IgnoreCase))
                 {
                     return item.ToString();
                 }

	            }
                
            }
            catch(Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, LimitSetAdvisory, Validate()", ex.Message, "");
            }
            return null;
        }
    }
}