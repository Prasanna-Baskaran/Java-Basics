using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;

namespace AGS.SwitchOperations
{
    public partial class FrmCardProduction : System.Web.UI.Page
    {
        enum CurrentUser
        {
            Maker,
            Checker
        }

        Nullable<CurrentUser> priCurUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajax.Utility.RegisterTypeForAjax(this.GetType());

            Session["UserRole"] = "A";
            if (!String.IsNullOrEmpty(Convert.ToString(Session["UserRole"])))
            {
                if (Convert.ToString(Session["UserRole"]).Split(',').Contains("T"))
                {
                    priCurUser = CurrentUser.Checker;

                }
                else
                {
                    priCurUser = CurrentUser.Maker;
                }
            }

            if (!IsPostBack)
            {
                SetUserwiseDetails();
            }
        }

        protected void btnProcess1_Click(object sender, EventArgs e)
        {
            try
            {
                dvCardGrid.InnerHtml = (new ClsCardProductionBAL()).ProcessPendingCardDetails(new ClsCardProductionBO() { UserId = "123", CardNo = hdnSelectedCustomers.Value });
                BindBatchNo();
                btnProcess1.Enabled = false;
            }
            catch
            {

            }

        }

        protected void btnDownload1_Click(object sender, EventArgs e)
        {
            string strRarFileName = (new ClsCardProductionBAL()).DownloadFiles((new ClsCardProductionBO() { ReDownload = false, BatchNo = "BATCH20170119195654" }));

            if (!string.IsNullOrEmpty(strRarFileName))
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "text/plain";
                response.AddHeader("Content-Disposition",
                                   "attachment; filename=" + strRarFileName + ";");
                response.TransmitFile(Server.MapPath("tempOutputs") + "\\" + strRarFileName);
                response.Flush();
                response.End();
            }
            else
            {
                //display message
            }
        }

        protected void btnAuthorise_Click(object sender, EventArgs e)
        {
            try
            {
                dvCardGrid.InnerHtml = (new ClsCardProductionBAL()).AuthoriseeCardDetails(new ClsCardProductionBO() { UserId = "123", CardNo = hdnSelectedCustomers.Value });
                btnAuthorise.Enabled = false;
            }
            catch
            {

            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                SetUserwiseDetails();
                btnProcess1.Enabled = true;
                btnAuthorise.Enabled = true;
            }
            catch
            {

            }
        }

        private void BindBatchNo()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDate.Value))
                {
                    ddlBatch.DataSource = (new ClsCardProductionBAL()).GetBatchNos((new ClsCardProductionBO() { ProcessDate = DateTime.ParseExact(txtDate.Value, "dd/MM/yyyy", null) }));
                    ddlBatch.DataTextField = "BatchNo";
                    ddlBatch.DataValueField = "BatchNo";
                    ddlBatch.DataBind();
                }
                else
                {
                    foreach(ListItem li in ddlBatch.Items){
                        ddlBatch.Items.Remove(li);
                    }
                }
                ddlBatch.Items.Insert(0, new ListItem() { Selected = true, Text = "--Select--", Value = "0" });
            }
            catch
            {

            }
        }

        protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlBatch.SelectedValue != "0")
                {
                    dvCardGrid.InnerHtml = (new ClsCardProductionBAL()).GetBatchHistory(new ClsCardProductionBO() { ProcessDate = Convert.ToDateTime(txtDate.Value), BatchNo = ddlBatch.SelectedItem.Text });
                    btnAuthorise.Enabled = false;
                    btnProcess1.Enabled = false;
                }
                else
                    dvCardGrid.InnerHtml = string.Empty;
            }
            catch
            {

            }
        }

        private void SetUserwiseDetails()
        {
            switch (priCurUser)
            {
                case CurrentUser.Maker:
                    dvCardGrid.InnerHtml = (new ClsCardProductionBAL()).GetPendingCardDetails();
                    btnProcess1.Visible = true; btnAuthorise.Visible = false;
                    break;
                case CurrentUser.Checker:
                    dvCardGrid.InnerHtml = (new ClsCardProductionBAL()).GetUnauthorisedCardDetails();
                    btnProcess1.Visible = false; btnAuthorise.Visible = true;
                    break;
            }
            txtDate.Value = "";
            BindBatchNo();
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            BindBatchNo();
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            BindBatchNo();
        }

    }
}
