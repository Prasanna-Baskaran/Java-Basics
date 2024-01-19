using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;
using AGS.Utilities;

namespace AGS.SwitchOperations
{
    public partial class AccountLinkingRequest : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        string Status = string.Empty;
        string Msg = string.Empty;
        string Description = string.Empty;
        string SessionId = string.Empty;
        string BankId = string.Empty;
        Boolean SkipDialogBox;
        bool LinkDelinkFlag;
        ClsGeneratePrepaidCardBO ObjPrepaid;
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BtnSave.Visible = false;
                BtnDelink.Visible = false;
                select_all.Visible = false;
                LBLselect_all.Visible = false;
                txtSearchCardNo.Disabled = true;
                BankId = Session["BankID"].ToString();
                if (!Page.IsPostBack)
                {
                    string OptionNeumonic = "ALSR"; //unique optionneumonic from database
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

                    /*Hiding memberModal DIV*/
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                    Session["CustomerDetail"] = null;
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, AccountLinkingRequest, Page_Load()", ex.Message, BankId);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SkipDialogBox = true;
            string msg = string.Empty;
            List<ValidatorAttr> ListValid = new List<ValidatorAttr>();

            if ((txtSearchCardNo.Value == null || txtSearchCardNo.Value == "") && (txtCIF.Value == null || txtCIF.Value == ""))
            //&& (txtNIC.Value == null || txtNIC.Value == ""))
            {
                msg = "All fields cannot be empty!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
            }
            else
            {
                new ValidatorAttr { Name = "Card No", Value = txtSearchCardNo.Value, MinLength = 16, MaxLength = 16, Numeric = true };
                new ValidatorAttr { Name = "CIF No", Value = txtCIF.Value, MinLength = 3, MaxLength = 20 };
                //new ValidatorAttr { Name = "NIC No", Value = txtNIC.Value, MinLength = 3, MaxLength = 20 };

                if (!ListValid.Validate(out msg))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                }
                else
                {
                    FunSearchDetails();
                }
            }
        }

        public void FunSearchDetails()
        {
            try
            {
                LblResult.InnerHtml = "";
                LblMessage.Text = "";

                if (string.IsNullOrEmpty(txtCIF.Value) /*&& !string.IsNullOrEmpty(txtNIC.Value)*/)
                {
                    LblMessage.Text = "Please enter either CIF No!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "";
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "Please enter either CIF No!";
                    return;
                }

                CustSearchFilter objSearch = new CustSearchFilter();
                DataTable ObjDTOutPut = new DataTable();
                DataTable DtAccDet = new DataTable();

                objSearch.CardNo = txtSearchCardNo.Value;
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                objSearch.IssuerNo = Session["IssuerNo"].ToString();

                objSearch.CIF = Convert.ToString(txtCIF.Value);
                //objSearch.NIC = Convert.ToString(txtNIC.Value);
                if (!string.IsNullOrEmpty(txtSearchCardNo.Value))
                {
                    objSearch.tranType = "GetAccountList"; // "SDBACCDETAILS";
                    //objSearch.tranType = "GetCustomerDetail"; 
                    objSearch.APIRequest = "AccountDetails";

                }
                else
                {
                    objSearch.tranType = "GetCustomerDetail"; // "SDBNICDETAILS";
                    objSearch.APIRequest = "CustomerDetail";
                    Session["CustomerDetail"] = null;
                }

                objSearch.LoginId = Convert.ToInt32(Session["LoginID"]);
                //DtAccDet = new ClsCustomerMasterBAL().FunGetCardAPIdata(objSearch);
                //objSearch.SDBAPIURL = DtAccDet.Rows[0]["URL"].ToString();
                //objSearch.PARA = DtAccDet.Rows[0]["PARA"].ToString();
                //objSearch.USER = DtAccDet.Rows[0]["USER"].ToString();
                //objSearch.TOKEN = DtAccDet.Rows[0]["TOKEN"].ToString();


                //APIResponseObjectSansa objAPIResponseObjectSansa = new APIResponseObjectSansa();
                //(new CallCardAPISansa()).Process(objSearch, objAPIResponseObjectSansa);

                GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();

                DataTable _dtRequest = new DataTable();
                _dtRequest.Columns.Add("CardNo", typeof(string));
                _dtRequest.Columns.Add("CifId", typeof(string));
                _dtRequest.Rows.Add(new Object[] { txtSearchCardNo.Value, txtCIF.Value });
                //_dtRequest.Rows.Add(new Object[] { hdnCardNo.Value, hdnCifId.Value });

                ConfigDataObject ObjData = new ConfigDataObject();


                DataTable _dtCardAPISourceIdDetails = new DataTable();
                //CustSearchFilter objSearch = new CustSearchFilter();

                //objSearch.APIRequest = "AccountDetails";
                objSearch.IntPara = 0;
                objSearch.SystemID = Session["SystemID"].ToString();
                objSearch.BankID = Session["BankID"].ToString();
                if (!string.IsNullOrEmpty(txtSearchCardNo.Value))
                {
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();

                }
                else
                {
                    //objSearch.IssuerNo = "7";
                    objSearch.IssuerNo = Session["IssuerNo"].ToString();
                }

                _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(objSearch);


                ObjData.IssuerNo = Session["IssuerNo"].ToString();
                ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);
                if (txtSearchCardNo.Value == "")
                {
                    ObjData.IsCustomerDetailsSearch = true;
                }


                APIResponseObject ObjAPIResponse = new APIResponseObject();

                _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

                if ((DataTable)Session["CustomerDetail"] != null)
                {
                    DtAccDet = (DataTable)Session["CustomerDetail"];

                    if (!DtAccDet.Columns.Contains("AccountNumber"))
                        DtAccDet.Columns.Add(new DataColumn("AccountNumber", typeof(string)));

                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber1))
                        DtAccDet.Rows[0]["AccountNumber"] = ObjAPIResponse.AccountNumber1;

                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber2))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        //dataRow["CustomerName"] = DtAccDet.Rows[0]["CustomerName"];
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber2;
                        DtAccDet.Rows.Add(dataRow);
                    }

                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber3))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"];  //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber3;
                        DtAccDet.Rows.Add(dataRow);
                    }

                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber4))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber4;
                        DtAccDet.Rows.Add(dataRow);
                    }

                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber5))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber5;
                        DtAccDet.Rows.Add(dataRow);
                    }
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber6))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber6;
                        DtAccDet.Rows.Add(dataRow);
                    }
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber7))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber7;
                        DtAccDet.Rows.Add(dataRow);
                    }
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNumber8))
                    {
                        DataRow dataRow = DtAccDet.NewRow();
                        dataRow["FirstName"] = DtAccDet.Rows[0]["FirstName"];
                        dataRow["LastName"] = DtAccDet.Rows[0]["LastName"];
                        dataRow["FullName"] = DtAccDet.Rows[0]["FullName"];
                        dataRow["EmailId"] = DtAccDet.Rows[0]["EmailId"];
                        dataRow["MobileNo"] = DtAccDet.Rows[0]["MobileNo"];
                        dataRow["NICNumber"] = DtAccDet.Rows[0]["NICNumber"];
                        dataRow["MotherName"] = DtAccDet.Rows[0]["MotherName"];
                        dataRow["Address1"] = DtAccDet.Rows[0]["Address1"];
                        dataRow["Address2"] = DtAccDet.Rows[0]["Address2"];
                        dataRow["DateOfBirth"] = DtAccDet.Rows[0]["DateOfBirth"]; //Mani
                        //dataRow["Address3"] = DtAccDet.Rows[0]["Address3"];
                        //dataRow["Address4"] = DtAccDet.Rows[0]["Address4"];
                        //dataRow["Address5"] = DtAccDet.Rows[0]["Address5"];
                        dataRow["AccountNumber"] = ObjAPIResponse.AccountNumber8;
                        DtAccDet.Rows.Add(dataRow);
                    }
                }
                else
                {


                    DtAccDet.Columns.Add(new DataColumn("FirstName", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("LastName", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("FullName", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("EmailId", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("MobileNo", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("NICNumber", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("MotherName", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("Address1", typeof(string)));
                    DtAccDet.Columns.Add(new DataColumn("Address2", typeof(string)));
                    //DtAccDet.Columns.Add(new DataColumn("Address3", typeof(string)));
                    //DtAccDet.Columns.Add(new DataColumn("Address4", typeof(string)));
                    //DtAccDet.Columns.Add(new DataColumn("Address5", typeof(string)));
                    //Edited by Mangesh 25Feb2023
                    DtAccDet.Columns.Add(new DataColumn("DateOfBirth", typeof(string)));
                    //Edited by Mangesh 25Feb2023 DOB at end of string
                    DtAccDet.Rows.Add(ObjAPIResponse.FirstName, ObjAPIResponse.LastName, ObjAPIResponse.FullName, ObjAPIResponse.EmailID, ObjAPIResponse.MobileNo, ObjAPIResponse.NICNumber, ObjAPIResponse.MotherName, ObjAPIResponse.Address1, ObjAPIResponse.Address2, ObjAPIResponse.DateOfBirth);
                    //DtAccDet.Rows.Add(ObjAPIResponse.FirstName, ObjAPIResponse.FullName, ObjAPIResponse.EmailID, ObjAPIResponse.MobileNo, ObjAPIResponse.NICNumber, ObjAPIResponse.MotherName, ObjAPIResponse.Address1, ObjAPIResponse.Address2, ObjAPIResponse.Address3, ObjAPIResponse.Address4, ObjAPIResponse.Address5, ObjAPIResponse.DateOfBirth);
                    Session["CustomerDetail"] = DtAccDet;
                    //DtAccDet = objAPIResponseObjectSansa.dtdetails;
                    //DtAccDet = ObjAPIResponse.dtdetails;
                }

                //if (objAPIResponseObjectSansa.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                {
                    txtSearchCardNo.Disabled = false;
                    txtCIF.Disabled = true;
                    if (DtAccDet == null)
                    {
                        System.Data.DataColumn newColumn = new System.Data.DataColumn("Custnm", typeof(System.String));
                        newColumn.DefaultValue = "test";
                        DtAccDet.Columns.Add(newColumn);

                        DtAccDet.Columns.Add(new DataColumn("FirstName", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("LastName", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("FullName", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("EmailId", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("MobileNo", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("NICNumber", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("MotherName", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("Address1", typeof(string)));
                        DtAccDet.Columns.Add(new DataColumn("Address2", typeof(string)));
                        //DtAccDet.Columns.Add(new DataColumn("Address3", typeof(string)));
                        //DtAccDet.Columns.Add(new DataColumn("Address4", typeof(string)));
                        //DtAccDet.Columns.Add(new DataColumn("Address5", typeof(string)));

                        DataRow dr = DtAccDet.NewRow();
                        dr[3] = ObjAPIResponse.CustomerName;

                        //dr[0]["EmailId"] = ObjAPIResponse.EmailID;
                        //dr[0]["MobileNo"] = ObjAPIResponse.MobileNo;
                        DtAccDet.Rows.Add(dr);

                        //foreach (DataRow dr in DtAccDet.Rows)
                        //{
                        //    dr["CustomerName"] = ObjAPIResponse.CustomerName;
                        //    dr["EmailId"] = ObjAPIResponse.EmailID;
                        //    dr["MobileNo"] = ObjAPIResponse.MobileNo;
                        //}
                    }


                    if (DtAccDet.Rows.Count > 0)
                    {

                        DtAccDet.Columns["FirstName"].ColumnName = "FirstName";
                        DtAccDet.Columns["FirstName"].SetOrdinal(0);

                        DtAccDet.Columns["LastName"].ColumnName = "LastName";
                        DtAccDet.Columns["LastName"].SetOrdinal(1);

                        DtAccDet.Columns["FullName"].ColumnName = "FullName";
                        DtAccDet.Columns["FullName"].SetOrdinal(2);

                        DtAccDet.Columns["EmailId"].ColumnName = "EmailId";
                        DtAccDet.Columns["EmailId"].SetOrdinal(3);

                        DtAccDet.Columns["MobileNo"].ColumnName = "MobileNo";
                        DtAccDet.Columns["MobileNo"].SetOrdinal(4);

                        DtAccDet.Columns["NICNumber"].ColumnName = "NICNumber";
                        DtAccDet.Columns["NICNumber"].SetOrdinal(5);

                        DtAccDet.Columns["MotherName"].ColumnName = "MotherName";
                        DtAccDet.Columns["MotherName"].SetOrdinal(6);

                        DtAccDet.Columns["Address1"].ColumnName = "Address1";
                        DtAccDet.Columns["Address1"].SetOrdinal(7);

                        DtAccDet.Columns["Address2"].ColumnName = "Address2";
                        DtAccDet.Columns["Address2"].SetOrdinal(8);

                        //DtAccDet.Columns["Address3"].ColumnName = "Address3";
                        //DtAccDet.Columns["Address3"].SetOrdinal(8);

                        //DtAccDet.Columns["Address4"].ColumnName = "Address4";
                        //DtAccDet.Columns["Address4"].SetOrdinal(9);

                        //DtAccDet.Columns["Address5"].ColumnName = "Address5";
                        //DtAccDet.Columns["Address5"].SetOrdinal(10);

                        //Edited by Mangesh on 25 Feb 2023
                        // DtAccDet.Columns["DateOfBirth"].ColumnName = "DateOfBirth";
                        // DtAccDet.Columns["DateOfBirth"].SetOrdinal(11); 

                        if (DtAccDet.Columns.Contains("AccountNumber"))
                            DtAccDet.Columns["AccountNumber"].SetOrdinal(3);

                        if (SkipDialogBox)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                        //hdnTransactionDetails.Value = createTable(DtAccDet);

                        //malar AccQualifier
                        if (txtSearchCardNo.Value != "" && txtCIF.Value != null)
                        {
                            if (!DtAccDet.Columns.Contains("AccQualifier"))
                                DtAccDet.Columns.Add(new DataColumn("AccQualifier"));

                            foreach (DataRow row in DtAccDet.Rows)
                            {
                                row["AccQualifier"] = "<select class='form-control'><option value='1'>Primary</option><option value='2'>Secondary</option><option value='3'>Tertiary</option><option value='4'>Quaternary</option></select>";
                                ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "Search result", txtSearchCardNo.Value, txtCIF.Value, "", "", row["FirstName"].ToString(), "", /*txtNIC.Value,*/ "Searched", "1");
                                //ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "Search result", txtSearchCardNo.Value, txtCIF.Value, "", "", row["CustomerName"].ToString(), "", /*txtNIC.Value,*/ "Searched", "1");
                            }
                        }

                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ACCOUNTNO" }, new AGS.Utilities.Attribute() { AttributeName = "ACCOUNTNO", BindTableColumnValueWithAttribute = "ACCOUNTNO" } } };

                        string columndata = "[FirstName],[LastName],[FullName],[EmailId],[MobileNo],[NICNumber],[MotherName],[Address1],[Address2],[AccQualifier]";
                        //string columndata = "[FirstName],[FullName],[EmailId],[MobileNo],[NICNumber],[MotherName],[Address1],[Address2],[Address3],[Address4],[Address5],[AccQualifier]";
                        if ((DataTable)Session["CustomerDetail"] != null)
                        {
                            //columndata += "[FirstName],[EmailId],[MobileNo],[NICNumber],[MotherName],[Address1],[Address2],[Address3],[Address4],[Address5],[AccountNumber],[AccQualifier]";
                            columndata += "[FirstName],[LastName],[EmailId],[MobileNo],[NICNumber],[MotherName],[Address1],[Address2],[AccountNumber],[AccQualifier]";
                        }

                        hdnTransactionDetails.Value = DtAccDet.ToHtmlTableString(columndata, objAdded);





                        //DtAccDet.Columns["DMACCT"].ColumnName = "ACCOUNTNO";
                        //    DtAccDet.Columns["ACCOUNTNO"].SetOrdinal(0);

                        //    if (SkipDialogBox)
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
                        //    //hdnTransactionDetails.Value = createTable(DtAccDet);
                        //    DtAccDet.Columns.Add(new DataColumn("AccQualifier"));
                        //    foreach (DataRow row in DtAccDet.Rows)
                        //    {
                        //        row["AccQualifier"] = "<select class='form-control'><option value='1'>Primary</option><option value='2'>Secondary</option><option value='3'>Tertiary</option><option value='4'>Quaternary</option></select>";
                        //        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "Search result", txtSearchCardNo.Value, txtCIF.Value, "", "", row["ACCOUNTNO"].ToString(), txtNIC.Value, "Searched", "1");
                        //    }

                        //    AddedTableData[] objAdded = new AddedTableData[1];
                        //    objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "FormStatus", BindTableColumnValueWithAttribute = "ACCOUNTNO" }, new AGS.Utilities.Attribute() { AttributeName = "ACCOUNTNO", BindTableColumnValueWithAttribute = "ACCOUNTNO" } } };
                        //    hdnTransactionDetails.Value = DtAccDet.ToHtmlTableString("[ACCOUNTNO],[CFPRNM],[MEMOBAL],[DMDOPN],[CURBAL],[DMBRCH],[DMTYPE],[AccQualifier]", objAdded);

                        if (!string.IsNullOrEmpty(txtSearchCardNo.Value))
                        {
                            BtnSave.Visible = true;
                            BtnDelink.Visible = true;

                        }

                        select_all.Visible = true;
                        LBLselect_all.Visible = true;
                    }
                    else
                    {
                        LblMessage.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                        hdnFlag.Value = "";
                        hdnTransactionDetails.Value = "";
                        LblResult.InnerHtml = "No Record Found";
                        BtnSave.Visible = false;
                        BtnDelink.Visible = false;
                        select_all.Visible = false;
                        LBLselect_all.Visible = false;
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "No Record Found for", txtSearchCardNo.Value, txtCIF.Value, "", "", txtAccountNo.Value, "",/*txtNIC.Value,*/ "Searched", "1");
                    }


                }
                else if (ObjAPIResponse.Status.Equals("140", StringComparison.OrdinalIgnoreCase))
                {
                    LblMessage.Text = "No Response!";
                }
                else
                {
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), "", "Search", "No Record Found for", txtSearchCardNo.Value, txtCIF.Value, "", "", txtAccountNo.Value, "",/*txtNIC.Value,*/ "Searched", "1");
                    LblMessage.Text = "No Record Found!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                    hdnFlag.Value = "";
                    hdnTransactionDetails.Value = "";
                    LblResult.InnerHtml = "No Record Found!";
                    BtnSave.Visible = false;
                    BtnDelink.Visible = false;
                    select_all.Visible = false;
                    LBLselect_all.Visible = false;
                }

                ObjDTOutPut = new ClsCardMasterBAL().FunSearchCardDtlISO(objSearch);

                #region //not used
                //if (ObjDTOutPut.Rows.Count > 0 && ObjDTOutPut.Rows[0]["Code"].ToString() == "0")
                //{
                //    APIResponseObject ObjAPIResponse1 = new APIResponseObject();
                //    ObjAPIResponse1 = CallISOForAccountOperation("AccountDetails", "3");

                //    if (ObjAPIResponse1.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                //    {
                //        if (!string.IsNullOrEmpty(ObjAPIResponse1.AccountNo))
                //        {
                //            DataTable _dtAccountDetails = new DataTable();
                //            _dtAccountDetails = DataTableToView(ObjAPIResponse1.AccountNo, objSearch.CardNo);


                //            if (SkipDialogBox)
                //                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);

                //            if (_dtAccountDetails.Rows.Count > 0)
                //            {
                //                hdnAccLinkDetails.Value = createTable(_dtAccountDetails);
                //            }
                //            else
                //            {
                //                hdnAccLinkDetails.Value = "";
                //                LblResult.InnerHtml = "No Linking Records Found";
                //            }
                //        }
                //        else
                //        {
                //            LblMessage.Text = "No Linking Records Found";
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //            hdnFlag.Value = "";
                //            hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("");
                //            hdnAccLinkDetails.Value = "";
                //            LblResult.InnerHtml = "No Linking Records Found";
                //        }
                //    }
                //    else
                //    {
                //        LblMessage.Text = "No Linking Records Found!";
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //        hdnFlag.Value = "";
                //        hdnAccLinkDetails.Value = "";
                //        LblResult.InnerHtml = "No Linking Records Found!";
                //    }
                //}
                //else
                //{
                //    LblMessage.Text = "Invalid card number!";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                //    hdnFlag.Value = "";
                //    hdnAccLinkDetails.Value = "";
                //    LblResult.InnerHtml = "Invalid card number!";
                //}
                #endregion

            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, AccountLinkingRequest, " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            FunSetResult("Insert", "1");
        }
        protected void BtnDelink_Click(object sender, EventArgs e)
        {
            FunSetResult("Insert", "2");
        }

        public void FunSetResult(String mode, string Linkingflag)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["IssuerNo"])))
                {
                    LblMessage.Text = "";
                    ObjPrepaid = new ClsGeneratePrepaidCardBO();
                    ObjPrepaid.RequestIDs = (hdnAllSelectedValues.Value).Split(',');
                    ObjPrepaid.UserID = Convert.ToString(Session["UserName"]);
                    ObjPrepaid.IssuerNo = Convert.ToString(Session["IssuerNo"]);
                    ObjPrepaid.Mode = mode;
                    ObjPrepaid.UserBranchCode = Session["BranchCode"].ToString();
                    ObjPrepaid.CardREqID = 0;
                    DataTable _dtRequest = new DataTable();
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("CustomerId", typeof(string));
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("NICNo", typeof(string));
                    _dtRequest.Columns.Add("Linkingflag", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));

                    _dtRequest.Columns.Add("CustomerName", typeof(string));
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
                    _dtRequest.Columns.Add("dateofbirth", typeof(string));
                    _dtRequest.Columns.Add("LastName", typeof(string));

                    foreach (string sr in ObjPrepaid.RequestIDs)
                    {

                        //_dtRequest.Rows.Add(new Object[] { sr.Split('|')[1], txtCIF.Value, txtSearchCardNo.Value, "",/*txtNIC.Value,*/ Linkingflag, "10", sr.Split('|')[2] });
                        _dtRequest.Rows.Add(new Object[] { sr.Split('|')[1], txtCIF.Value, txtSearchCardNo.Value, "", Linkingflag, "10", sr.Split('|')[2], sr.Split('|')[3], sr.Split('|')[4], sr.Split('|')[5], sr.Split('|')[6], sr.Split('|')[7], sr.Split('|')[8], sr.Split('|')[9], sr.Split('|')[10], sr.Split('|')[11], sr.Split('|')[12] });
                        //_dtRequest.Rows.Add(new Object[] { sr.Split('|')[1], txtCIF.Value, txtSearchCardNo.Value, "",Linkingflag, "10", sr.Split('|')[2], sr.Split('|')[3], sr.Split('|')[4], sr.Split('|')[5], sr.Split('|')[6], sr.Split('|')[7], sr.Split('|')[8], sr.Split('|')[9], sr.Split('|')[10], sr.Split('|')[11], sr.Split('|')[12], sr.Split('|')[13], sr.Split('|')[14] });
                    }

                    //var duplicates = _dtRequest.AsEnumerable().GroupBy(r => r["AccountQualifier"]).Where(gr => gr.Count() > 1).ToList().Any();

                    ObjPrepaid.DtBulkData = _dtRequest;
                    DataSet ObjDSOutPut = new DataSet();
                    ObjDSOutPut = new ClsGeneratePrepaidCardsBAL().GetSetAccountLinkrequest(ObjPrepaid);

                    if (ObjDSOutPut.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ObjDSOutPut.Tables[0].Rows[0][0]) == "1")
                        {

                            LblResult.InnerHtml = "<span style='color:green'>Account " + ((Linkingflag == "1") ? "linking" : "delinking") + " request send successfully!</span>";

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
                new ClsCommonBAL().FunInsertIntoErrorLog("AGS.SwitchOperations.AccountLinkingRequest,FunSetResult()", ex.Message, Convert.ToString(Session["IssuerNo"]));
                LblResult.InnerHtml = "Error Occured";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "Hidemodel()", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowinfo()", true);

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

                        //Clear old Linked/Delinked account list
                        hdnLinkedAccList.Value = hdnDeLinkedAccList.Value = string.Empty;

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
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='2' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' CifId='" + currentRow[5].ToString() + "' > Delink </ asp:Button >" + strTDEnd);
                                // Convert.ToInt32(Session["Bank"])
                                hdnLinkedAccList.Value = hdnLinkedAccList.Value + "," + currentRow[2].ToString();
                            }
                            if (currentRow[columnNames[0]].ToString().ToUpper() == "DELINK")
                            {
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Delink'  class='btn btn-primary btn-xs' OnClick='FunAcceptRejectOpsReq($(this))' requesttypeid='1' statusid='1' userid='" + currentRow[0].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                //strHTML.Append(strTDStart + " <asp:Button runat='server' ID='btn_Link'  class='btn btn-primary btn-xs ' OnClick='FunLinkDelinkAccount($(this))' Linkingflag='1' Linkingflg='" + currentRow[0].ToString() + "' CardNo='" + currentRow[1].ToString() + "' AccountNo='" + currentRow[2].ToString() + "' AccountType='" + currentRow[3].ToString() + "' AccountQuilifier='" + currentRow[4].ToString() + "' CifId='" + currentRow[5].ToString() + "' > Link </ asp:Button >" + strTDEnd);
                                hdnDeLinkedAccList.Value = hdnDeLinkedAccList.Value + "," + currentRow[2].ToString();
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
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinkingRequest " + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
                return "";
            }
        }

        protected void hdnLinkBtn_Click(object sender, EventArgs e)
        {
            APIResponseObject ObjAPIResponse = new APIResponseObject();
            ObjAPIResponse = CallISOForAccountOperation("Acclinking", "1");

            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
            {
                if (hdnLinkingflag.Value == "1")
                {
                    LblMessage.Text = "Account Linked Successfully.";
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully.";
                }


                ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3");
                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNo))
                    {

                        DataTable _dtAccountLlinkingDetails = new DataTable();
                        _dtAccountLlinkingDetails = CardAccountDataTable(ObjAPIResponse.AccountNo);
                        if (_dtAccountLlinkingDetails.Rows.Count > 0)
                        {
                            ClsAccountLinkingDelinkingBAL.FunSyncCardAccountLinkingDetails(hdnCardNo.Value, Session["IssuerNo"].ToString(), _dtAccountLlinkingDetails);
                        }

                        DataTable _dtAccountDetails = new DataTable();
                        _dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, hdnCardNo.Value);
                        if (_dtAccountDetails.Rows.Count > 0)
                        {
                            hdnTransactionDetails.Value = createTable(_dtAccountDetails);
                        }
                        else
                        {
                            hdnTransactionDetails.Value = "";
                            LblResult.InnerHtml = "No Record Found";
                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully but failed to sync details. Please try again.";

                }
            }
            else
            {
                LblMessage.Text = "Failed";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

        }

        protected void AddAccount_Click(object sender, EventArgs e)
        {
            //
            if (CheckAccountQuantifier.Checked)
            {
                hdnCheckAccQuntifr.Value = "1";
            }
            else
            {
                hdnCheckAccQuntifr.Value = "2";
            }

            APIResponseObject ObjAPIResponse = new APIResponseObject();
            ObjAPIResponse = CallISOForAccountOperation("AddAccount", "2");

            if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
            {
                ClsAccountLinkingDelinkingBAL.FunSyncAccountEnc(BankId, ObjAPIResponse.AccountNo, txtAccountNo.Value, ddlAccountType.SelectedValue, ddlCurrencyCode.SelectedValue);

                LblMessage.Text = "Account No. added successfully.";
                //FunSearchDetails();

                ObjAPIResponse = CallISOForAccountOperation("AccountDetails", "3");
                if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(ObjAPIResponse.AccountNo))
                    {

                        DataTable _dtAccountLlinkingDetails = new DataTable();
                        _dtAccountLlinkingDetails = CardAccountDataTable(ObjAPIResponse.AccountNo);
                        if (_dtAccountLlinkingDetails.Rows.Count > 0)
                        {
                            ClsAccountLinkingDelinkingBAL.FunSyncCardAccountLinkingDetails(txtSearchCardNo.Value, Session["IssuerNo"].ToString(), _dtAccountLlinkingDetails);
                        }
                        //hdnCardNo.Value
                        DataTable _dtAccountDetails = new DataTable();
                        _dtAccountDetails = DataTableToView(ObjAPIResponse.AccountNo, txtSearchCardNo.Value);
                        if (_dtAccountDetails.Rows.Count > 0)
                        {
                            hdnTransactionDetails.Value = createTable(_dtAccountDetails);
                        }
                        else
                        {
                            hdnTransactionDetails.Value = "";
                            LblResult.InnerHtml = "No Record Found";
                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Account De-Linked Successfully but failed to sync details. Please try again.";

                }

            }
            else
            {
                LblMessage.Text = "Failed";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
        }
        public APIResponseObject CallISOForAccountOperation(string APIRequest, string RequestType)
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
                if (RequestType == "1")//AccountLinkDelink
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    _dtRequest.Columns.Add("LinkingFlag", typeof(string));
                    _dtRequest.Columns.Add("CifId", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { hdnCardNo.Value, hdnAccountNo.Value, hdnAccountType.Value, hdnAccountQuilifier.Value, hdnLinkingflag.Value, hdnCifId.Value });
                }
                else if (RequestType == "2")//Add New Account
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Columns.Add("AccountNo", typeof(string));
                    _dtRequest.Columns.Add("AccountType", typeof(string));
                    _dtRequest.Columns.Add("AccountQualifier", typeof(string));
                    _dtRequest.Columns.Add("Currency", typeof(string));
                    _dtRequest.Columns.Add("CifId", typeof(string));

                    _dtRequest.Rows.Add(new Object[] { txtSearchCardNo.Value, txtAccountNo.Value, ddlAccountType.SelectedValue, hdnCheckAccQuntifr.Value, ddlCurrencyCode.SelectedValue, hdnCifId.Value });
                    ObjData.IsAccountDetailsSearch = true;
                }
                else
                {
                    _dtRequest.Columns.Add("CardNo", typeof(string));
                    _dtRequest.Rows.Add(new Object[] { txtSearchCardNo.Value });
                    ObjData.IsAccountDetailsSearch = true;
                }
                _GenerateCardAPIRequest.CallCardAPIService(_dtRequest.Rows[0], _dtRequest, ObjData, ObjAPIResponse);

            }
            catch (Exception Ex)
            {
                ObjAPIResponse.Status = "140";
                ObjAPIResponse.StatusDesc = "Unexpected error";
                //(new ClsCommonBAL()).SaveErrorLogDetails("AcManagement.aspx.cs, createTable()", Ex.Message, Ex.StackTrace);
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, AccountLinkingRequest>>CallISOForAccountOperation" + System.Reflection.MethodBase.GetCurrentMethod().Name, Ex.Message, BankId);
            }
            return ObjAPIResponse;
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
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingRequest CS, DataTableToView()", ex.Message, BankId);
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
                new ClsCommonBAL().FunInsertIntoErrorLog("AccountLinkingRequest CS, CardAccountDataTable()", ex.Message, BankId);
            }
            return _dtAccountDetails;
        }
    }
}