using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.Utilities;
using System.Data;

namespace AGS.SwitchOperations
{
    public partial class InternationalUsageRequest : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           try
            {
                string OptionNeumonic = "IUR"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        if (!IsPostBack) {
                            fnvisiDisi(false);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("IUR, InternationalUsageRequest, Page_Load()", ObjEx.Message, "");
            }
        }

        protected void BtnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.InnerHtml = "";

                if (txtCardNum.Value == "")
                {
                    LblResult.InnerHtml = "<span style='color:red'> Please Enter CardNo </span>";
                    txtCardNum.Focus();

                }
                else
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();

                    objSearch.CardNo = txtCardNum.Value;
                    objSearch.Remark = txtRemark.Value;
                    if (chkIntUsage.Checked==true)
                    {
                        objSearch.IntNaUsage = "1";
                    }
                    else
                    {
                        objSearch.IntNaUsage = "0";
                    }
                    objSearch.UserID= Session["UserName"].ToString();
                    //objSearch.SystemID = Session["SystemID"].ToString();
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();

                    ObjDTOutPut = new ClsInternationalUsageBAL().FunInsertCustCardIntNaUsageData(objSearch);
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        if (Convert.ToString(ObjDTOutPut.Rows[0][0]) == "1")
                        {
                            LblResult.InnerHtml = "<span style='color:green'>Request sent successfully!</span>";
                            fnvisiDisi(false);
                        }
                        else {

                            LblResult.InnerHtml = "<span style='color:red'>"+ Convert.ToString(ObjDTOutPut.Rows[0][1]) + "</span>";

                        }
                
                    }
                    else
                    {
                        LblResult.InnerHtml = "<span style='color:red'>Error while generating request</span>";
                    }
                }
                //AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CardDetails.aspx", "Search button clicked", "");
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUR, InternationalUsageRequest, BtnRequest_Click()", ObjEx.Message, "");
            }
        }

        public void fnvisiDisi(Boolean op)
        {
            txtRemark.Visible = op;
            BtnRequest.Visible = op;
            chkIntUsage.Visible = op;
            LblIntUsage.Visible = op;
            LblRemark.Visible = op;
            txtRemark.Value = "";
            chkIntUsage.Checked = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LblResult.InnerHtml = "";

                if (txtCardNum.Value == "")
                {
                    LblResult.InnerHtml = "<span style='color:red'> Please Enter CardNo </span>";
                    txtCardNum.Focus();

                }
                else
                {
                    DataTable ObjDTOutPut = new DataTable();
                    int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                    ClsIntNaCutDetails objSearch = new ClsIntNaCutDetails();

                    objSearch.CardNo = txtCardNum.Value;
                    //objSearch.Remark = txtRemark.Value;
                    //objSearch.SystemID = Session["SystemID"].ToString();
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();

                    ObjDTOutPut = new ClsInternationalUsageBAL().FunSearchCustCardIntNaUsageData(objSearch);
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        LblResult.InnerHtml = "";

                        txtRemark.Visible = true;
                        BtnRequest.Visible = true;
                        chkIntUsage.Visible = true;
                        LblIntUsage.Visible = true;
                        LblRemark.Visible = true;
                        txtRemark.Value = "";
                        if (Convert.ToString(ObjDTOutPut.Rows[0][0]) == "1")
                        {
                            chkIntUsage.Checked = false;
                        }
                        else
                        {
                            chkIntUsage.Checked = true;
                        }
                    }
                    else
                    {
                        LblResult.InnerHtml = "<span style='color:red'>No Record Found</span>";
                    }
                }
                //AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(Session["UserName"]), "CardDetails.aspx", "Search button clicked", "");
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("IUR, InternationalUsageRequest, btnSearch_Click()", ObjEx.Message, "");
            }
        }
    }
}