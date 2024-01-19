using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class BankConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
            }
        }
       // protected void Save_Click(object sender, EventArgs e)
       // {
       // //    try
       // //    {
       // //        if (string.IsNullOrEmpty(TxtIssuerNo.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter  Issueer No');</script>");
       // //        }

       // //        if (string.IsNullOrEmpty(TxtBank.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter Bank Name');</script>");
       // //        }
       // //        if (string.IsNullOrEmpty(TxtBankFolder.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter BankFolder Name');</script>");
       // //        }
       // //        if (string.IsNullOrEmpty(TxtSwitchInstitutionID.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter SwitchInstitutionID ');</script>");
       // //        }
       // //        if (string.IsNullOrEmpty(TxtCardPrefix.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter CardPrefix ');</script>");
       // //        }
       // //        if (string.IsNullOrEmpty(TxtCardProgram.Text))
       // //        {
       // //            Response.Write("<script>alert('Please enter CardProgram No');</script>");
       // //        }

       // //        ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
       // //        ClsBankConfigureBO obj = new ClsBankConfigureBO();
       // //       // obj.IssuerNo = Convert.ToInt32(TxtIssuerNo.Text);
       // //        obj.Bank = TxtBank.Text;
       // //        obj.BankFolder = TxtBankFolder.Text;
       // //        obj.SwitchInstitutionID = TxtSwitchInstitutionID.Text;
       // //        obj.CardPrefix = TxtCardPrefix.Text;
       // //        obj.CardProgram = TxtCardProgram.Text;

       // //        ObjReturnStatus = new ClsBankConfigurationBAL().FunConfigureBank(obj);
       // //   // LblMsg.InnerText = ObjReturnStatus.Description.ToString();

       // // if (ObjReturnStatus.Code == 0)
       // //    {

       // //        //hdnResultStatus.Value = "1";
       // //        //Get Result
       // //        //FunGetResult();
       // //    }

       // //   // ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
       // //}
       // //    catch (Exception ObjEx)
       // //    {
       // //        new ClsCommonBAL().FunInsertIntoErrorLog("CS, BankConfiguration, Save_Click()", ObjEx.Message, "");
       // //     }
       //}
    }
}