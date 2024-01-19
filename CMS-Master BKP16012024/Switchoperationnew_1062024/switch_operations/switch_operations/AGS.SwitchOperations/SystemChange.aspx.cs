using AGS.SwitchOperations.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class SystemChange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                funBindDDl();
            }

        }

        public void funBindDDl()
        {
            DataTable ObjDTSystem = new DataTable();
            ObjDTSystem = (new ClsCommonBAL()).FunGetCommonDataTable(20, Session["LoginID"].ToString());
            DdlSystem.DataSource = ObjDTSystem;
            DdlSystem.DataTextField = "SystemName";
            DdlSystem.DataValueField = "SystemID";
            DdlSystem.DataBind();
            DdlSystem.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                string SystemID = string.Empty;
                SystemID = DdlSystem.SelectedValue;
                Session["SystemID"] = null;
                if (!string.IsNullOrEmpty(SystemID))
                {
                    Session["SystemID"] = SystemID;
                    Session["SystemName"] = DdlSystem.SelectedItem;
                    DataTable dtRights = (new ClsCommonBAL()).FunGetCommonDataTable(10, Session["LoginID"].ToString() + "," + SystemID + "," + 1);
                    Dictionary<string, string> ObjDict = new Dictionary<string, string>();

                    foreach (DataRow dr in dtRights.Rows)
                    {
                        string OptionNeumonic = dr["OptionNeumonic"].ToString();
                        string AccessCaptions = dr["AccessCaptions"].ToString();
                        if (!ObjDict.ContainsKey(OptionNeumonic))
                        {
                            ObjDict.Add(OptionNeumonic, AccessCaptions);
                        }

                    }
                    Session["UserRights"] = ObjDict;
                    DataTable dtOptions = (new ClsCommonBAL()).FunGetCommonDataTable(10, Session["LoginID"].ToString() + "," + SystemID);
                    Session["MenuList"] = dtOptions;
                    Response.Redirect("Home.aspx", false);
                }
            }
            catch (Exception ex)
            {
                new ClsCommonBAL().FunInsertIntoErrorLog("CS, UserDetails, btnProceed_Click()", ex.Message, "");
            }
        }
    }
}