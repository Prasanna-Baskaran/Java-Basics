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
    public partial class CardOperations : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "CMCO"; //unique optionneumonic from database

                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];


                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        string[] accessPrefix = StrAccessCaption.Split(',');
                        //For user those having Save right
                        if (accessPrefix.Contains("S"))
                        {
                            hdnAccessCaption.Value = "S";
                            userBtns.AccessButtons = StrAccessCaption;
                            userBtns.SaveClick += new EventHandler(btnSave_Click);

                        }
                        else
                        {
                            hdnAccessCaption.Value = "";
                        }

                        if (!IsPostBack)
                        {
                            FillDropDown();
                        }
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
                // (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, CardOperations, Page_Load()", Ex.Message, "");
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {                
                DataTable ObjDTOutPut = new DataTable();
                int RoleID = Convert.ToInt16(Session["UserRoleID"]);
                CustSearchFilter objSearch = new CustSearchFilter();
                //objSearch.MobileNo = txtSearchMobile.Value;

                if (!string.IsNullOrEmpty(txtSearchCustomerID.Value))
                {
                    objSearch.BankCustID = txtSearchCustomerID.Value;
                }
                if (!string.IsNullOrEmpty(txtSearchName.Value))
                {
                    objSearch.CustomerName = txtSearchName.Value;
                }
                objSearch.CardNo = txtSearchCardNo.Value;

                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();

                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtlISO(objSearch);

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    txtCustomerID.Value = ObjDTOutPut.Rows[0][0].ToString();
                    txtCustomerName.Value = ObjDTOutPut.Rows[0][1].ToString();
                    txtMobile.Value = ObjDTOutPut.Rows[0][2].ToString();
                    txtDOB.Value = ObjDTOutPut.Rows[0][3].ToString();
                    txtAddress.Text = ObjDTOutPut.Rows[0][4].ToString();
                    txtEmail.Value = ObjDTOutPut.Rows[0][5].ToString();
                    txtCardNo.Value = ObjDTOutPut.Rows[0][17].ToString();
                    txtExpiryDate.Value = ObjDTOutPut.Rows[0][18].ToString();
                    txtCardStatus.Value = ObjDTOutPut.Rows[0][19].ToString();
                    txtCardIssued.Value = ObjDTOutPut.Rows[0][20].ToString();
                    txtCardStatusID.Value = ObjDTOutPut.Rows[0][22].ToString();
                    LblResult.InnerHtml = "";
                    hdnFlag.Value = "2";
                    //start 27/07 for customer and account logic change
                    hdnRPANID.Value = ObjDTOutPut.Rows[0][23].ToString();
                    txtAccountNo.Value = ObjDTOutPut.Rows[0][24].ToString();
                    hdnCustomerID.Value = ObjDTOutPut.Rows[0][25].ToString();
                    //start sheetal to disable respective requesttype on cardstatus
                    //15-12-17
                    foreach (ListItem item in ddlRequestType.Items)
                    {
                        if (txtCardStatus.Value == item.Text)
                        {
                            //item.Attributes.Add("disabled", "disabled");
                            item.Enabled = false;
                        }
                    }
                    //end
                }
                else
                {
                    hdnFlag.Value = "";
                    LblResult.InnerHtml = "No record found";
                }
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperations, btnSearch_Click()", Ex.Message, "");
                hdnFlag.Value = "";
                LblResult.InnerHtml = "No record found";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                ObjReturnStatus.Description = "Card request is not saved";
                CustSearchFilter ObjFilter = new CustSearchFilter();
                ObjFilter.IntPara = 1;
                //ObjFilter.CustomerID = Convert.ToInt64(txtCustomerID.Value);
                //start 27/07
                ObjFilter.CustomerID = hdnCustomerID.Value;

                ObjFilter.MakerID = Convert.ToInt64(Session["LoginID"]);

                if ((!string.IsNullOrEmpty(txtCardNo.Value)) && (!string.IsNullOrEmpty(ObjFilter.CustomerID)))
                {
                    CustSearchFilter objSearch = new CustSearchFilter();
                    objSearch.SystemID = Session["SystemID"].ToString();
                    objSearch.BankID = Session["BankID"].ToString();
                    objSearch.PINResetFlag = 4; // FLag 4 to find out PIN Try count
                    objSearch.CardNo = txtCardNo.Value;
                    DataTable ObjDTCarddetailsTOResetPin = new DataTable();
                    ObjDTCarddetailsTOResetPin = new ClsCardMasterBAL().FunGetSetCardDetailsForPinRepin(objSearch);


                    ObjFilter.CardNo = txtCardNo.Value;
                    ObjFilter.Remark = txtUpdateRemark.Text;
                    ObjFilter.RequestTypeID = Convert.ToInt32(ddlRequestType.SelectedValue);
                    ObjFilter.SystemID = Session["SystemID"].ToString();
                    ObjFilter.BankID = Session["BankID"].ToString();
                    if (Convert.ToInt32(ddlRequestType.SelectedValue) == 7)
                    {
                        if (ObjDTCarddetailsTOResetPin.Rows.Count > 0)
                        {
                            if (Convert.ToInt64(ObjDTCarddetailsTOResetPin.Rows[0]["pin_try_count"]) > 2)
                            {
                                ObjReturnStatus = new ClsCardMasterBAL().FunSaveCardOpsReq(ObjFilter);
                            }
                            else
                            {
                                ObjReturnStatus.Description = "Card pin try count is not exceeded.";
                            }
                        }
                        else
                        {
                            ObjReturnStatus.Description = "Card pin try count is not exceeded.";
                        }
                    }
                    else
                    {
                        ObjReturnStatus = new ClsCardMasterBAL().FunSaveCardOpsReq(ObjFilter);
                    }



                }
                else
                {
                    ObjReturnStatus.Description = "Data is not proper";
                }


                LblMsg.InnerText = ObjReturnStatus.Description.ToString();

                if (ObjReturnStatus.Code == 0)
                {
                    hdnResultStatus.Value = "1";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ObjEx)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperations, btnSave_Click()", ObjEx.Message, "");
            }
        }

        public void FillDropDown()
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(19, "1");
                ddlRequestType.DataSource = ObjDTOutPut;
                ddlRequestType.DataTextField = "RequestType";
                ddlRequestType.DataValueField = "ID";
                ddlRequestType.DataBind();
                ddlRequestType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception Ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, CardOperations, FillDropDown()", Ex.Message, "");
            }

        }
    }
}