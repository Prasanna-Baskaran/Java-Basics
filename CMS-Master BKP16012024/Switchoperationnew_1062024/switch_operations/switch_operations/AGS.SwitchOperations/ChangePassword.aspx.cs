using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using AGS.SwitchOperations.Validator;

namespace AGS.SwitchOperations
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        ClsCommonDAL ClsCommonDAL = new ClsCommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ClsCahngePass objChangePass = new ClsCahngePass();
                objChangePass.LoginID = Session["LoginID"].ToString();
                objChangePass.Role = Session["UserRoleID"].ToString();
                objChangePass.ChangedBy = Convert.ToInt32(Session["LoginID"]);
                objChangePass.BankId = Session["BankID"].ToString();
                objChangePass.UserName = Session["UserName"].ToString();
                objChangePass.mode = 0;
                //------------------------------------------//
                if (!IsPostBack)
                {
                    hfPublicKey.Value = new RSABuilder().InitiateRSA();
                }
                //--------------------------------------------//
                new ClsCommonBAL().FunChangepass(objChangePass);
                if (objChangePass.Result == 1)
                {
                    ddlUserID.Enabled = true;
                    FillDropDown(objChangePass);
                }
                else
                {
                    ListItem li = new ListItem(objChangePass.UserName, objChangePass.UserName);
                    ddlUserID.Items.Insert(0, li);
                }
            }
            //loginErrorMsg.Attributes.Add("class", "alert alert-error hide");
            //loginErrorMsg.InnerText = "";

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsCahngePass objChangePass = new ClsCahngePass();
                if ((!string.IsNullOrEmpty(ddlUserID.SelectedValue)) && (!string.IsNullOrEmpty(txtCurrentPassword.Value)) && (!string.IsNullOrEmpty(txtNewPassword.Value)) && (!string.IsNullOrEmpty(txtConfirmNewPassword.Value)))
                {
                    if (new RSABuilder().Decrypt(txtNewPassword.Value.Trim()).Equals(new RSABuilder().Decrypt(txtConfirmNewPassword.Value.Trim())))
                    {
                        string msg = string.Empty;

                        List<ValidatorAttr> ListValid = new List<ValidatorAttr>()
                        {
                            new ValidatorAttr { Name="Username", Value= ddlUserID.SelectedItem.Text.Trim(), MinLength = 1, MaxLength = 16, AlphaNumeric=true },
                            new ValidatorAttr { Name="Current Password", Value= new RSABuilder().Decrypt( txtCurrentPassword.Value.Trim()), MinLength = 0, MaxLength = 20},
                            new ValidatorAttr { Name="New Password", Value= new RSABuilder().Decrypt(txtNewPassword.Value.Trim()), MinLength = 8, MaxLength = 20},
                            new ValidatorAttr { Name="Confirm Password", Value=new RSABuilder().Decrypt( txtConfirmNewPassword.Value.Trim()), MinLength = 8, MaxLength = 20},
                        };

                        if (!ListValid.Validate(out msg))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "validateserver('SpnErrorMsg','errormsgDiv','" + msg + "')", true);
                        }
                        else
                        {
                            objChangePass.UserName = ddlUserID.SelectedItem.Text.Trim();
                            objChangePass.CurrentPass = new RSABuilder().Decrypt(txtCurrentPassword.Value.Trim());
                            /************************************************************************/
                            byte[] bytesToEncode01 = System.Text.Encoding.UTF8.GetBytes(txtCurrentPassword.Value.Trim());
                            // Encode the bytes to Base64
                            string base64Encoded01 = Convert.ToBase64String(bytesToEncode01);
                            txtCurrentPassword.Value = base64Encoded01;
                            /************************************************************************/
                            // objChangePass.CurrentPass =  txtCurrentPassword.Value.Trim();
                            /************************************************************************/
                            objChangePass.NewPass = new RSABuilder().Decrypt(txtNewPassword.Value.Trim());
                            /************************************************************************/
                            byte[] bytesToEncode02 = System.Text.Encoding.UTF8.GetBytes(txtNewPassword.Value.Trim());

                            // Encode the bytes to Base64
                            string base64Encoded02 = Convert.ToBase64String(bytesToEncode02);
                            txtNewPassword.Value = base64Encoded02;
                            /************************************************************************/
                            //  objChangePass.NewPass = txtNewPassword.Value.Trim();
                            /************************************************************************/
                            objChangePass.ConfirmNewPass = new RSABuilder().Decrypt(txtConfirmNewPassword.Value.Trim());
                            /************************************************************************/
                            byte[] bytesToEncode03 = System.Text.Encoding.UTF8.GetBytes(txtConfirmNewPassword.Value.Trim());

                            // Encode the bytes to Base64
                            string base64Encoded03 = Convert.ToBase64String(bytesToEncode03);
                            txtConfirmNewPassword.Value = base64Encoded03;
                            /************************************************************************/
                            // objChangePass.ConfirmNewPass = txtConfirmNewPassword.Value.Trim();
                            /************************************************************************/
                            objChangePass.LoginID = Session["LoginID"].ToString();
                            objChangePass.Role = Session["UserRoleID"].ToString();
                            objChangePass.ChangedBy = Convert.ToInt32(Session["LoginID"]);
                            objChangePass.BankId = Session["BankID"].ToString();
                            objChangePass.mode = 1;

                            new ClsCommonBAL().FunChangepass(objChangePass);
                        }
                    }
                    else
                    {
                        objChangePass.Description = "New password field and Confirm new password field does not match.";
                    }
                    if (objChangePass.Result == 1)
                    {
                        string issuerno = Session["IssuerNo"] != null ? Session["IssuerNo"].ToString() : "";
                        ClsCommonDAL.FunUMAL(objChangePass.LoginID, issuerno, "Change Password", objChangePass.Description, ddlUserID.SelectedValue, "Save", "1");
                    }
                    //loginErrorMsg.InnerText = objChangePass.Description;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg('" + objChangePass.Description + "')", true);
                }
            }
            catch (Exception Ex)
            {
                (new ClsCommonBAL()).FunInsertIntoErrorLog("CS, Change Password, btnSave_Click()", Ex.Message, Ex.StackTrace);
            }
        }

        public string EncryptPassword(string text)
        {
            string g = "";
            if (!(text == null))
            {
                g = ClsRSA.EncryptRSA(text);
            }
            return g;
        }


        public void FillDropDown(ClsCahngePass objChangePass)
        {
            ddlUserID.DataSource = new ClsCardMasterBAL().FunGetUserIdActivityReports(objChangePass.BankId);
            ddlUserID.DataTextField = "Text";
            ddlUserID.DataValueField = "Value";
            ddlUserID.DataBind();
        }
    }
}