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
    public partial class CourierMaster : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OptionNeumonic = "MCM"; //unique optionneumonic from database
                Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
                ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

                if (ObjDictRights.ContainsKey(OptionNeumonic))
                {
                    StrAccessCaption = ObjDictRights[OptionNeumonic];
                    if (!string.IsNullOrEmpty(StrAccessCaption))
                    {
                        hdnAccessCaption.Value = StrAccessCaption;

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
            catch (Exception )
            {
                //new ClsCommonBAL().FunInsertIntoErrorLog("CS, CourierMater, Page_Load()", ex.Message, "");
            }



        }

        protected void add_Click(object sender, EventArgs e)
        {

            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            DataTable ObjDTOutPut = new DataTable();
            ClsCourierDetailsBO ObjPriFeeBO = new ClsCourierDetailsBO() { CourierName = txt_couriername.Value.Trim(), OfficeName = txt_officename.Value.Trim(), MobileNo = txt_mobileno.Value.ToString().Trim(), Status = dropdown_status.SelectedValue, Courierid = txt_courierid.Value };
            if (ObjPriFeeBO.Courierid == "" || ObjPriFeeBO.Courierid == null)
            {
                try
                {
                    ObjReturnStatus = new ClsCourierDetailsBAL().FunInsertIntoCourierDetails(ObjPriFeeBO.CourierName, ObjPriFeeBO.OfficeName, ObjPriFeeBO.MobileNo, Convert.ToInt32(ObjPriFeeBO.Status), Convert.ToInt16(Session["LoginID"]),Session["SystemID"].ToString(),Session["BankID"].ToString());
                    LblMsg.InnerText = ObjReturnStatus.Description;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);

                }


                catch (Exception ex)
                {


                    new ClsCommonBAL().FunInsertIntoErrorLog("CS, CourierDetails, add_Click(Insert)", ex.Message, "");
                }

                finally
                {
                    //using (ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(18, string.Empty))
                    //{
                    //    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString();
                    //}
                    FunGetResult();

                    txt_couriername.Value = string.Empty;
                    txt_officename.Value = string.Empty;
                    txt_mobileno.Value = string.Empty;
                    txt_courierid.Value = string.Empty;

                }
            }

            else
            {
                try
                {
                    ObjReturnStatus = new ClsCourierDetailsBAL().FunUpdateIntoCourierDetails(ObjPriFeeBO.CourierName, ObjPriFeeBO.OfficeName, ObjPriFeeBO.MobileNo, Convert.ToInt32(ObjPriFeeBO.Status), Convert.ToInt16(Session["LoginID"]), Convert.ToInt32(ObjPriFeeBO.Courierid),Session["SystemID"].ToString(),Session["BankID"].ToString());
                    LblMsg.InnerText = ObjReturnStatus.Description;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                }
                catch (Exception ex)
                {

                    new ClsCommonBAL().FunInsertIntoErrorLog("CS,CourierDetails, add_Click(Edit)", ex.Message, "");

                }

                finally
                {

                    //using (ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(18, string.Empty))
                    //{
                    //    hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString();
                    //}
                    FunGetResult();

                    txt_couriername.Value = string.Empty;
                    txt_officename.Value = string.Empty;
                    txt_mobileno.Value = string.Empty;
                    txt_courierid.Value = string.Empty;


                }
            }

        }

        public void FunGetResult()
        {
            using (DataTable ObjDTOutPut = new ClsCommonBAL().FunGetCommonDataTable(18,Session["BankID"]+","+Session["SystemID"]))
            {
              
                hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString();

                if (ObjDTOutPut.Rows.Count > 0)
                {
                    string[] accessPrefix = StrAccessCaption.Split(',');
                    //For user those having Edit right
                    if (accessPrefix.Contains("E"))
                    {
                        AddedTableData[] objAdded = new AddedTableData[1];
                        objAdded[0] = new AddedTableData()
                        {
                            control = AGS.Utilities.Controls.Button,
                            buttonName = "Edit",
                            cssClass = "btn btn-primary"
                                              ,
                            hideColumnName = true,
                            events = new Event[] { new Event() { EventName = "onclick", EventValue = "funedit($(this));" } }
                                              ,
                            attributes = new AGS.Utilities.Attribute[]
                                              { new AGS.Utilities.Attribute() { AttributeName = "id", BindTableColumnValueWithAttribute = "CourierId" }

                        , new AGS.Utilities.Attribute() { AttributeName = "couriername", BindTableColumnValueWithAttribute = "CourierName" }
                        , new AGS.Utilities.Attribute() { AttributeName = "office", BindTableColumnValueWithAttribute = "Office" }
                        , new AGS.Utilities.Attribute() { AttributeName = "contact", BindTableColumnValueWithAttribute = "ContactNo" }
                        , new AGS.Utilities.Attribute() { AttributeName = "statusid", BindTableColumnValueWithAttribute = "StatusId" }

                                     }
                        };

                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CourierId,StatusId", objAdded);
                    }
                    else
                        hdnTransactionDetails.Value = ObjDTOutPut.ToHtmlTableString("CourierId,StatusId");
                }

            }

        }

    }
}