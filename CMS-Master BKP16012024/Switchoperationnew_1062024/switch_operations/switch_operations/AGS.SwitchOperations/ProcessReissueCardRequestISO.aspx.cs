using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class ProcessReissueCardRequestISO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {//
                FunGetResult();

                DataTable ObjDTOutPut = new DataTable();
                ObjDTOutPut = new ClsCardCheckerDetailsBAL().FunGetProcessType();
                ddlProcessType.DataSource = ObjDTOutPut;
                ddlProcessType.DataTextField = "ProcessType";
                ddlProcessType.DataValueField = "ProcessID";
                ddlProcessType.DataBind();

            }
        }
        protected void FunGetResult()
        {
            ProcessCardReissueRequest ObjChecker = new ProcessCardReissueRequest();
            ObjChecker.LoginId = Convert.ToString(Session["LoginID"]);
            ObjChecker.ActionType = 1;
            ObjChecker.BankID = Convert.ToString(Session["BankID"]);
            ObjChecker.SystemID = Convert.ToString(Session["SystemID"]);
            ObjChecker.IssuerNo = Convert.ToString(Session["IssuerNo"]);

            DataTable ObjDTOutPut = new ClsCardCheckerDetailsBAL().ValidateReissueCardRequest(ObjChecker);

            if (ObjDTOutPut.Rows.Count > 0)
            {
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { index = 0, control = Utilities.Controls.Checkbox, hideColumnName = true, cssClass = "CHECK" } });
            }
            else
            {
                hdnTransactionDetails.Value = "";
               
                LblResult.InnerHtml = "Please make Card Reissue request to Approve.";
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckedIDs = hdnAllSelectedValues.Value;
            ProcessCardReissueRequest ObjChecker = new ProcessCardReissueRequest();
            ObjChecker.IDs = CheckedIDs;
            ObjChecker.LoginId = Convert.ToString(Session["LoginID"]);
            ObjChecker.BankID = Convert.ToString(Session["BankID"]);
            ObjChecker.IssuerNo = Convert.ToString(Session["IssuerNo"]);
            ObjChecker.ActionType = 2;

            DataTable ObjDTOutPut = new ClsCardCheckerDetailsBAL().ValidateReissueCardRequest(ObjChecker);

            if (ObjDTOutPut.Rows.Count > 0)
            {
                DataTable _dtSFTPDetails = new DataTable();
                SFTPDetails _SFTPDetails = new SFTPDetails();
                _dtSFTPDetails = new ClsCommonBAL().FunGetSFTPDetailsToPlaceCIF(Session["IssuerNo"].ToString(), 12); //ProcessId 6 for Reissuance

                _SFTPDetails = ClsCommonMethods.BindDatatableToClass<SFTPDetails>(_dtSFTPDetails);

                //_SFTPDetails.SwitchPortal_Local_Input = "D:\\MyGit\\Card Processing\\switch_operations\\AGS.SwitchOperations\\CIF_Input";
                string InputFilePath = _SFTPDetails.SwitchPortal_Local_Input + "CardReissue_" + Session["IssuerNo"].ToString() + "_" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".txt";
                if (WriteDataToFile(ObjDTOutPut, InputFilePath))
                {
                    if (ClsCommonMethods.UploadFile(_SFTPDetails))
                    {
                        //Update card request status
                        ObjChecker.ActionType = 3;
                        ObjDTOutPut = new ClsCardCheckerDetailsBAL().ValidateReissueCardRequest(ObjChecker);
                        LblMessage.Text = "PRE File will be generated on SFTP.";
                    }
                }
                else
                {
                    LblMessage.Text = "Unexpected error occured.";
                }
                FunGetResult();

                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(
                           Convert.ToString(HttpContext.Current.Session["LoginID"]),
                           Convert.ToString(Session["UserName"]),
                            "Checker.aspx", "PRE File will be generated on SFTP", ""
                           );

                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

            }
            else
            {
                AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(
                           Convert.ToString(HttpContext.Current.Session["LoginID"]),
                           Convert.ToString(Session["UserName"]),
                            "Checker.aspx", "PRE File request failed", ""
                           );

                LblMessage.Text = "Failed";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        protected void ProcessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunGetResult();
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
                return false;
            }
        }
    }
}