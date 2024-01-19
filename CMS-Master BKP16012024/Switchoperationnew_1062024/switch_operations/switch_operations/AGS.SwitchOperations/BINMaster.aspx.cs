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

namespace AGS.SwitchOperations
{
    public partial class BINMaster : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "MBIN"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), 
                    Convert.ToString(Session["UserName"]), "BinMaster.aspx", "Page_Load", "");


                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack)
                        {
                            FunDropDownFill();
                        }

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
            catch (Exception ObjEx)
            {
              //  new ClsCommonBAL().FunInsertIntoErrorLog("CS, BINMaster, Page_Load()", ObjEx.Message, "");
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsBINDtl ObjFilter = new ClsBINDtl();

                if (!string.IsNullOrEmpty(hdnID.Value))
                {
                    ObjFilter.ID = Convert.ToInt16(hdnID.Value);
                }
                ObjFilter.BIN = txtBin.Value;
                ObjFilter.BinDesc = txtBinDesc.Value;
                ObjFilter.INSTID = ddlInstitutionID.SelectedValue;

                ObjReturnStatus = new ClsMasterBAL().FunSaveBinDetails(ObjFilter);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";
                    
                }
                //Get Result
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
               new ClsCommonBAL().FunInsertIntoErrorLog("CS, BINMaster, btnSave_Click()", ObjEx.Message, "");
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsBINDtl ObjBin = new ClsBINDtl();
                ObjBin.ID = Convert.ToInt32(hdnID.Value);
                ObjBin.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjBin.FormStatusID = 1;
                ObjReturnStatus =new ClsMasterBAL().FunAccept_RejectBin(ObjBin);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BINMaster, btnAccept_Click()", Ex.Message, "");
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ClsBINDtl ObjBin = new ClsBINDtl();
                ObjBin.ID = Convert.ToInt32(hdnID.Value);
                ObjBin.CheckerID = Convert.ToInt64(Session["LoginID"]);
                ObjBin.FormStatusID = 2;
                ObjBin.Remark = txtRejectReson.Text;
                ObjReturnStatus = new ClsMasterBAL().FunAccept_RejectBin(ObjBin);
                LblMsg.InnerText = ObjReturnStatus.Description.ToString();
                //Get Result
                FunGetResult();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BINMaster, btnReject_Click()", Ex.Message, "");
            }
        }

        protected void FunGetResult()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(15, "0,"+Session["BankID"]+","+Session["SystemID"]);
            int RoleID = Convert.ToInt16(Session["UserRoleID"]);
            //hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("ID");
        }

        protected void FunDropDownFill()
        {
            try
            {
                DataTable ObjINSTDTOutPut = new DataTable();
                ObjINSTDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(12, "");
                ddlInstitutionID.DataSource = ObjINSTDTOutPut;
                ddlInstitutionID.DataTextField = "InstitutionID";
                ddlInstitutionID.DataValueField = "ID";
                ddlInstitutionID.DataBind();
                ddlInstitutionID.Items.Insert(0, new ListItem("--Select--", "0"));


                //DataTable ObjOutPut = new DataTable();
                //ObjINSTDTOutPut = ClsCommonDAL.FunGetCommonDataTable(8, "");
                //ddlInstitutionID.DataSource = ObjINSTDTOutPut;
                //ddlInstitutionID.DataTextField = "InstitutionID";
                //ddlInstitutionID.DataValueField = "ID";
                //ddlInstitutionID.DataBind();
                //ddlInstitutionID.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, BINMaster, FunDropDownFill()", Ex.Message, "");
            }

        }



    }
}