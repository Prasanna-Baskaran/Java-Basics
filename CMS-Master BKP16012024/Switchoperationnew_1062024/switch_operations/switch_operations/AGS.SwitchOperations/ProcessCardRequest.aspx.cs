using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Renci.SshNet;
using AGS.SwitchOperations.Common;
using System.Configuration;

namespace AGS.SwitchOperations
{
    public partial class ProcessCardRequest : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FunGetResult(int RequestType)
        {
            try
            {
                DataTable ObjDTOutPut = new DataTable();

                ClsProcessCardRequest _ClsProcessCardRequest = new ClsProcessCardRequest();
                _ClsProcessCardRequest.BankId = Session["BankID"].ToString();
                _ClsProcessCardRequest.Code = "";
                _ClsProcessCardRequest.RequestType = RequestType;
                _ClsProcessCardRequest.ActionType = 1;
                _ClsProcessCardRequest.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                _ClsProcessCardRequest.UserBranchCode = Session["BranchCode"].ToString();
                _ClsProcessCardRequest.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                _ClsProcessCardRequest.UserID = Session["UserName"].ToString();

                ObjDTOutPut = new ClsCommonBAL().FunAcceptRejectCardRequestDetails(_ClsProcessCardRequest);

                //if (ObjDTOutPut.Rows.Count > 0)
                //{
                //    string[] accessPrefix = StrAccessCaption.Split(',');
                //    //if user has accept right
                //    if (accessPrefix.Contains("P"))
                //    {

                AddedTableData[] objAdded = new AddedTableData[1];
                objAdded[0] = new AddedTableData() { control = AGS.Utilities.Controls.Checkbox, columnName = "Select", cssClass = "checkbox", index = 0, hideColumnName = true, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "custid", BindTableColumnValueWithAttribute = "CustomerID" } } };
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustomerID", objAdded);

                if (ObjDTOutPut.Rows.Count == 0)
                {

                    LblMsg.InnerText = "No records found";
                    LblMsgFailuer.InnerText = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                //    }
                //    else
                //        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CustomerID");
                //}
            }
            catch (Exception ex)
            {

                (new ClsCommonBAL()).FunInsertIntoErrorLog("ProcessCardRequest, FunGetResult()", ex.Message, "");
            }


        }

        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            FunGetResult(Convert.ToInt32(DdlRequestType.SelectedValue));
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                ClsProcessCardRequest _ClsCardReq = new ClsProcessCardRequest();

                _ClsCardReq.BankId = Session["BankID"].ToString();
                _ClsCardReq.Code = hdnCustomerID.Value;
                _ClsCardReq.RequestType = Convert.ToInt32(DdlRequestType.SelectedValue);
                _ClsCardReq.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                _ClsCardReq.UserID= Session["UserName"].ToString();
                _ClsCardReq.UserBranchCode = Session["BranchCode"].ToString();
                _ClsCardReq.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);

                DataTable _dtCardRequest = new DataTable();
                DataTable _dtSFTPDetails = new DataTable();
                _ClsCardReq.ActionType = 2; //to fetch txt format card records or Edit ISO records
                _dtCardRequest = new ClsCommonBAL().FunAcceptRejectCardRequestDetails(_ClsCardReq);

                if (_ClsCardReq.RequestType == 1 && _dtCardRequest.Rows.Count > 0) //For Issuance request
                {
                    SFTPDetails _clsSFTPDet = new SFTPDetails();
                    _dtSFTPDetails = new ClsCommonBAL().FunGetSFTPDetailsToPlaceCIF(Session["IssuerNo"].ToString(), 11); //ProcessId 3 for Issueance

                    _clsSFTPDet = ClsCommonMethods.BindDatatableToClass<SFTPDetails>(_dtSFTPDetails);

                    
                    //_SFTPDetails.SwitchPortal_Local_Input = "D:\\MyGit\\Card Processing\\switch_operations\\AGS.SwitchOperations\\CIF_Input";
                    string InputFilePath = _clsSFTPDet.SwitchPortal_Local_Input + "CIF_Portal_" + Session["IssuerNo"].ToString() + "_" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".txt";
                    if (WriteDataToFile(_dtCardRequest, InputFilePath))
                    {
                        //_SFTPDetails.SwitchPortal_Local_Archive = "D:\\MyGit\\Card Processing\\switch_operations\\AGS.SwitchOperations\\CIF_Archive";
                        if (ClsCommonMethods.UploadFile(_clsSFTPDet))
                        {
                            //Update card request status
                            _ClsCardReq.ActionType = 3;
                            _ClsCardReq.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                            _ClsCardReq.LoginId = Convert.ToInt32(Session["LoginID"]);
                            _dtCardRequest = new ClsCommonBAL().FunAcceptRejectCardRequestDetails(_ClsCardReq);
                            LblMsg.InnerText = "Card requests processed successfully.";
                        }
                        else {

                            LblMsg.InnerText = "Error while SFTP file upload";

                        }
                    }
                    else
                    {
                        LblMsg.InnerText = "Unexpected error occured";
                    }
                }
                else if (_ClsCardReq.RequestType == 2)//For Edit request
                {
                    string _SuccessCIF = string.Empty;
                    string _FailuerCIF = string.Empty;
                    CustSearchFilter ObjCustomer = new CustSearchFilter();
                    DataTable _dtCardAPISourceIdDetails = new DataTable();
                    DataTable _dtUpdateStatus = new DataTable();
                    if (Session["iInstaEdit"].ToString() == "3")
                    { ObjCustomer.APIRequest = "CustomerDataUpdateInsta"; }
                    else { ObjCustomer.APIRequest = "CustomerDataUpdate"; }
                  
                    ObjCustomer.IssuerNo = Session["IssuerNo"].ToString();
                    _dtCardAPISourceIdDetails = new ClsCardMasterBAL().GetCardAPISourceIdDetails(ObjCustomer);

                    for (int i = 0; i < _dtCardRequest.Rows.Count; i++)
                    {
                        GenerateCardAPIRequest _GenerateCardAPIRequest = new GenerateCardAPIRequest();
                        APIResponseObject ObjAPIResponse = new APIResponseObject();
                        ConfigDataObject ObjData = new ConfigDataObject();
                        ObjData.IssuerNo = Session["IssuerNo"].ToString();
                        ObjData.APIRequestType = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][0]);
                        ObjData.CardAPIURL = ConfigurationManager.AppSettings["CardAPIURL"].ToString();
                        ObjData.SourceID = Convert.ToString(_dtCardAPISourceIdDetails.Rows[0][1]);

                        _GenerateCardAPIRequest.CallCardAPIService(_dtCardRequest.Rows[i], _dtCardRequest, ObjData, ObjAPIResponse);

                        if (ObjAPIResponse.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            _SuccessCIF = _SuccessCIF + _dtCardRequest.Rows[i][1].ToString() + ",";
                        }
                        else
                        {
                            _FailuerCIF = _FailuerCIF + _dtCardRequest.Rows[i][1].ToString() + ",";
                        }
                        //Update customer details into tblauth table 
                        _ClsCardReq.ActionType = 3;
                        _ClsCardReq.Code = _dtCardRequest.Rows[i][0].ToString();
                        _ClsCardReq.ISORSPCode = ObjAPIResponse.Status;
                        _ClsCardReq.ISORSPDesc = ObjAPIResponse.StatusDesc;
                        _ClsCardReq.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                        _ClsCardReq.LoginId = Convert.ToInt32(Session["LoginID"]);
                        _ClsCardReq.iInstaEdit = Session["iInstaEdit"].ToString();
                        _dtUpdateStatus = new ClsCommonBAL().FunAcceptRejectCardRequestDetails(_ClsCardReq);
                    }
                    if (!string.IsNullOrEmpty(_SuccessCIF))
                    {
                        LblMsg.InnerText = "Success CIF Id: " + _SuccessCIF.Substring(0,_SuccessCIF.Length-1);
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Process Card Request", "Accepted successfully", "", LblMsg.InnerText.Replace("Success CIF Id: ",""), "", "", "", "", "Accept", "1");
                    }
                    if (!string.IsNullOrEmpty(_FailuerCIF))
                    {
                        LblMsgFailuer.InnerText = "Failed CIF Id: " + _FailuerCIF.Substring(0, _FailuerCIF.Length - 1);
                        ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Process Card Request", "Failed to accept", "", LblMsgFailuer.InnerText.Replace("Failed CIF Id: ", ""), "", "", "", "", "Accept", "0");
                    }

                }
                else
                {
                    LblMsg.InnerText = "Unexpected error occured.";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
            catch (Exception ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ProcessCardRequest, btnAccept_Click()", ex.Message, "");
                LblMsg.InnerText = "Unexpected error occured.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }


        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                ClsProcessCardRequest _ClsCardReq = new ClsProcessCardRequest();

                _ClsCardReq.BankId = Session["BankID"].ToString();
                _ClsCardReq.Code = hdnCustomerID.Value;
                _ClsCardReq.RequestType = Convert.ToInt32(DdlRequestType.SelectedValue);
                _ClsCardReq.LoginId = Convert.ToInt32(Session["LoginID"]);
                _ClsCardReq.IssuerNo = Convert.ToInt32(Session["IssuerNo"]);
                _ClsCardReq.ActionType = 4;
                _ClsCardReq.UserBranchCode = Session["BranchCode"].ToString();
                _ClsCardReq.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);
                _ClsCardReq.UserID = Session["UserName"].ToString();


                DataTable _dtCardRequest = new DataTable();
                _dtCardRequest = new ClsCommonBAL().FunAcceptRejectCardRequestDetails(_ClsCardReq);
                if (_ClsCardReq.ErrorFlag)
                {
                    LblMsg.InnerText = "Rejected successfully.";
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Process Card Request", "Rejected request of request type :"+ DdlRequestType.SelectedValue, "", "", "", "", "", "", "Reject", "1");
                }
                else
                {
                    LblMsg.InnerText = "Unexpected error occured.";
                    ClsCommonDAL.FunSOAL(Convert.ToString(HttpContext.Current.Session["LoginID"]), Convert.ToString(HttpContext.Current.Session["IssuerNo"]), "Process Card Request", "Failed to reject request of request type :" + DdlRequestType.SelectedValue, "_ClsCardReq.ErrorFlag", "", "", "", "", "", "Reject", "0");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }
            catch (Exception ex)
            {

                (new ClsCommonBAL()).FunInsertIntoErrorLog("ProcessCardRequest, btnReject_Click", ex.Message, "");
                LblMsg.InnerText = "Unexpected error occured.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }
        public bool WriteDataToFile(DataTable submittedDataTable, string submittedFilePath)
        {
            try
            {
                int i = 0;
                StreamWriter sw = null;
                sw = new StreamWriter(submittedFilePath, false);
                foreach (DataRow row in submittedDataTable.Rows)
                {
                    object[] array = row.ItemArray;

                    for (i = 0; i < array.Length - 1; i++)
                    {
                        sw.Write(array[i].ToString() + ";");
                    }
                    sw.Write(array[i].ToString());
                    sw.WriteLine();
                }
                sw.Close();
                return true;
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("ProcessCardRequest, WriteDataToFile", Ex.Message, "");
                return false;
               

            }
        }
    }
}