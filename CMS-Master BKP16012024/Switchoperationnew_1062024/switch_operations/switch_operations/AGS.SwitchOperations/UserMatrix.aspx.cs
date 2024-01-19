using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;

namespace AGS.SwitchOperations
{
    public partial class UserMatrix : System.Web.UI.Page
    {
        string StrAccessCaption = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string OptionNeumonic = "MUM"; //unique optionneumonic from database

            Dictionary<string, string> ObjDictRights = new Dictionary<string, string>();
            ObjDictRights = (Dictionary<string, string>)Session["UserRights"];

            if (ObjDictRights.ContainsKey(OptionNeumonic))
            {
                StrAccessCaption = ObjDictRights[OptionNeumonic];


                if (!string.IsNullOrEmpty(StrAccessCaption))
                {

                    userBtns.AccessButtons = StrAccessCaption;
                    userBtns.SaveClick += new EventHandler(btnSave_Click);
                    if (!IsPostBack)
                    {

                        using (DataTable dtRoles = (new ClsCommonBAL()).FunGetCommonDataTable(11, Session["SystemID"].ToString()+","+Session["BankID"].ToString()+","+Session["LoginID"].ToString()))
                        {
                            if (dtRoles.Rows.Count > 0)
                            {
                                ddlRole.DataSource = dtRoles;
                                ddlRole.DataTextField = "FirstName";
                                ddlRole.DataValueField = "UserID";
                                ddlRole.DataBind();
                                ddlRole.Items.Insert(0, new ListItem() { Text = "--Select User--", Value = "0", Selected = true });
                            }
                        }
                    }
                }
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

        private void createRightTable(string role,string SystemID)
        {
            try
            {
                DataSet dsMatrix = (new ClsRightsBAL()).GetOptionRights(role,SystemID);
                DataTable dtMatrix = new DataTable(), dtAccessCaption = new DataTable();
                if (dsMatrix != null)
                {
                    if (dsMatrix.Tables.Count == 2)
                    {
                        dtMatrix = dsMatrix.Tables[0];
                        dtAccessCaption = dsMatrix.Tables[1];
                    }
                }
                string strtable = "<table width='100%' border=1>";
                if (dtMatrix != null)
                {
                    strtable += "<thead><tr>";
                    foreach (DataColumn dcCols in dtMatrix.Columns)
                    {
                        if (!dcCols.ColumnName.Contains('#'))
                            strtable += "<td>" + dcCols.ColumnName + "</td>";
                    }
                    strtable += "</tr></thead>";

                    if (dtMatrix.Rows.Count > 0)
                    {
                        strtable += "<tbody>";
                        foreach (DataRow drRows in dtMatrix.Rows)
                        {
                            strtable += "<tr optionneumonic='" + Convert.ToString(drRows["OptionNeumonic#"]).Trim() + "'  parentneumonic='" + Convert.ToString(drRows["ParentNeumonic#"]).Trim() + "' optionid='" + Convert.ToString(drRows["OptionID#"]).Trim() + "'>";
                            int intCount = 1;
                            foreach (DataColumn dcCols in dtMatrix.Columns)
                            {
                                bool isCheckboxField = true;
                                if (!dcCols.ColumnName.Contains('#'))
                                {
                                    string strAccessCode = string.Empty;
                                    try
                                    {
                                        strAccessCode = dtAccessCaption.AsEnumerable().Where(p => p.Field<string>("CaptionName").ToUpper() == dcCols.ColumnName.ToUpper()).FirstOrDefault().Field<string>("AccessCode");
                                    }
                                    catch
                                    {
                                        strAccessCode = intCount.ToString(); intCount++;
                                        if (!dcCols.ColumnName.Equals("ALL"))
                                            isCheckboxField = false;
                                    }

                                    //var abc = from ac in dtAccessCaption.AsEnumerable() where ac.Field<string>("CaptionName") ==    dcCols.ColumnName select ac.Field<string>("AccessCode");
                                    string id = Convert.ToString(drRows["OptionNeumonic#"]).Trim() + "_" + strAccessCode, strTd = string.Empty;
                                    if (Convert.ToString(drRows[dcCols.ColumnName]) != "2")
                                    {
                                        strTd += "<td id='td_" + id + "'>" + ((!isCheckboxField) ? Convert.ToString(drRows[dcCols.ColumnName]) : "<input type='checkbox' id ='chk_" + id + "' " + (Convert.ToString(drRows[dcCols.ColumnName]) == "1" ? "checked" : "") + " accesscode='" + strAccessCode + "' />") + "</td>";
                                    }
                                    else
                                    {
                                        strTd += "<td id='td_" + id + "'>" + ((!isCheckboxField) ? Convert.ToString(drRows[dcCols.ColumnName]) : "<input type='checkbox' disabled id ='chk_" + id + "' accesscode='" + strAccessCode + "' />") + "</td>";
                                    }
                                    strtable += strTd;
                                }
                            }
                            strtable += "</tr>";
                        }
                        strtable += "</tbody>";

                        strtable += "<tbody>";



                        strtable += "</tbody>";
                    }
                }
                strtable += "</table>";
                //dvRights.InnerHtml = strtable;

                hdnData.Value = strtable;
            }
            catch { }
        }

        private string getInnerMenus(DataTable dtOptions, string strParentNeumonic, System.Drawing.Color colr)
        {
            try
            {
                string strListItems = string.Empty, strSubMenuStart = string.Empty, strSubMenuEnd = string.Empty;
                DataRow[] drParents = dtOptions.Select("OptionParentNeumonic='" + strParentNeumonic + "'");
                if (drParents.Length > 0)
                {

                    foreach (DataRow drOptions in drParents)
                    {
                        strListItems += "<li class=\"effect7\"> <a href=\"" + (Convert.ToString(drOptions["URL"]).Trim() == "" ? "#" : Convert.ToString(drOptions["URL"])) + "\">" + Convert.ToString(drOptions["OptionName"]) + "</a>";
                        strListItems += getInnerMenus(dtOptions, Convert.ToString(drOptions["OptionNeumonic"]), System.Drawing.Color.Red);
                        //strListItems += (!string.IsNullOrEmpty(strInnerMenus) ? strSubMenuStart + strInnerMenus + strSubMenuEnd : string.Empty);
                        strListItems += "</li>";
                    }
                }
                else
                {

                }
                return strListItems;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlRole.SelectedValue) == 0)
            {

            }
            else
            {
                //Start 18/04/2017
                createRightTable(ddlRole.SelectedValue,Session["SystemID"].ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                string UserID = ddlRole.SelectedValue;

                List<JObject> objData = (List<JObject>)JsonConvert.DeserializeObject(hdnData.Value, typeof(List<JObject>));

                DataTable dtRights = new DataTable();
                dtRights.Columns.AddRange((new DataColumn[]{
                    (new DataColumn() { ColumnName = "RoleId", DataType = typeof(string), DefaultValue=ddlRole.SelectedValue }),
                    (new DataColumn() { ColumnName = "OptionNeumonic", DataType = typeof(string) }),
                    (new DataColumn() { ColumnName = "AccessCaption", DataType = typeof(string) })
                }));

                foreach (JObject jobject in objData)
                {
                    DataRow drRights = dtRights.NewRow();
                    drRights["OptionNeumonic"] = Convert.ToString(jobject["optionneumonic"]).Trim();
                    drRights["AccessCaption"] = Convert.ToString(jobject["accessrights"]).TrimEnd(',');
                    dtRights.Rows.Add(drRights);
                }
                ObjReturnStatus = (new ClsRightsBAL()).updateUserRights(dtRights, UserID,Session["SystemID"].ToString());
                strresult.InnerText = ObjReturnStatus.Description;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
                createRightTable(ddlRole.SelectedValue,Session["SystemID"].ToString());
            }
            catch
            {
          
            }

        }


    }
}