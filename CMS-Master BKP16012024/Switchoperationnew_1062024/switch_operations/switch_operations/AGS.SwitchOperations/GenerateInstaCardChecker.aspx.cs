using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    public partial class GenerateInstaCardChecker : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        ClsGeneratePrepaidCardBO ObjPrepaid;
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "GICA"; //unique optionneumonic from database
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
                            FunGetResult("GetPending");
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
                new ClsCommonBAL().FunInsertIntoErrorLog("GICA, GenerateInstaCardChecker, Page_Load()", ObjEx.Message, "");
                Response.Redirect("ErrorPage.aspx", false);
            }
        }

        protected void FunGetResult(String Mode)
        {
            try
            {
                txtRejectReson.Text = "";
                ObjPrepaid = new ClsGeneratePrepaidCardBO();
                ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                ObjPrepaid.Mode = Mode;
                ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                DataSet ObjDSOutPut = new DataSet();
                ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GeneratePrepaidCard_Operations(ObjPrepaid);
                if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                {
                    AddedTableData[] objAdded = new AddedTableData[1];
                    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute(){ AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID"}, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
                    //hdnTransactionDetails.Value = ObjDSOutPut.Tables[0].ToHtmlTableString("ID,Card Program,No Of Cards,Reason,STATUS,Inserted Date,Authorised Date,Processed Date,Rejected Date'", objAdded);
                    hdnTransactionDetails.Value = ObjDSOutPut.Tables[0].ToHtmlTableString("ID,[Card Program],[No Of Cards],[STATUS],[Reason],[Inserted Date],[Authorised Date],[Processed Date],[Rejected Date],FormStatusID", objAdded);
                    //hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { index = 0, control = Utilities.Controls.Checkbox, hideColumnName = true, cssClass = "CHECK" } });
                    Reject_Btn.Visible = true;
                    BtnSave.Visible = true;
                    select_all.Visible = true;
                    LBLselect_all.Visible = true;
                }
                else
                {
                    if (Mode == "GetPending")
                    {
                        LblResult.InnerHtml = "<span style='color:red'>No request for Insta Cards Generation</span>";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);
                    }
                    Reject_Btn.Visible = false;
                    BtnSave.Visible = false;
                    select_all.Visible = false;
                    LBLselect_all.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunHideinfo()", true);
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.GenerateInstaCardChecker,FunGetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblMessage.InnerHtml = "<span style='color:red'>Error while getting data</span>";
                Reject_Btn.Visible = false;
                BtnSave.Visible = false;
                select_all.Visible = false;
                LBLselect_all.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            FunSetResult("Authorised");
        }

        //protected void BtnReject_Click(object sender, EventArgs e)
        //{
        //    FunSetResult("RejetedByAuth");
        //}

        protected void Reject_Btn_Click(object sender, EventArgs e)
        {
            FunSetResult("RejetedByAuth");
            txtRejectReson.Text = "";
        }


        public void FunSetResult(String mode)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                {
                    LblMessage.InnerHtml = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    ObjPrepaid.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                    ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                    ObjPrepaid.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                    ObjPrepaid.Mode = mode;

                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("id", typeof(Int32));


                    _dtRequest.Columns.Add("Reason", typeof(string));
                    foreach (string sr in ObjPrepaid.RequestIDs)
                    {
                        _dtRequest.Rows.Add(new Object[] { sr, ObjPrepaid.Mode == "Authorised" ? "Authorised Successfully" : txtRejectReson.Text });
                        
                    }

                    ObjPrepaid.DtBulkData = _dtRequest;
                    DataSet ObjDSOutPut = new DataSet();
                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().AcceptRejectInstaCards(ObjPrepaid);

                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                        {
                            if (ObjPrepaid.Mode == "Authorised")
                            {
                                LblResult.InnerHtml = "<span style='color:green'>Authorised successfully!</span>";
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Generate Insta Card Auth", "Accepted instant card generate for | RequestId : " + string.Join(",", ObjPrepaid.RequestIDs) + " | CardProgram : " + ObjPrepaid.CardProgram + " | No of Cards :" + ObjDSOutPut.Tables[0].Rows.Count, "", "", "", "", "", "", "Accept", "1");
                            }
                            else
                            {
                                LblResult.InnerHtml = "<span style='color:green'>Rejected successfully!</span>";
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Generate Insta Card Auth", "Rejected instant card generate for | RequestId : " + string.Join(",", ObjPrepaid.RequestIDs) + " | CardProgram : " + ObjPrepaid.CardProgram + " | No of Cards :" + ObjDSOutPut.Tables[0].Rows.Count, "", "", "", "", "", "", "Reject", "1");
                            }

                            if (ObjDSOutPut.Tables[1].Rows.Count > 0)
                            {

                                AddedTableData[] objAdded = new AddedTableData[1];
                                objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "FormStatusID" }, new AGS.Utilities.Attribute() { AttributeName = "ReqID", BindTableColumnValueWithAttribute = "ID" } } };
                                hdnTransactionDetails.Value = ObjDSOutPut.Tables[1].ToHtmlTableString("ID,[Card Program],[No Of Cards],[STATUS],[Reason],[Inserted Date],[Authorised Date],[Processed Date],[Rejected Date],FormStatusID", objAdded);

                                //hdnTransactionDetails.Value = ObjDSOutPut.Tables[1].ToHtmlTableString();// "", new AddedTableData[] { new AddedTableData() });
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
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.GenerateInstaCardChecker,FunSetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblResult.InnerHtml = "Error Occured";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);

        }

    }
}