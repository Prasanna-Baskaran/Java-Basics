using AGS.SwitchOperations.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class frmTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["LoginID"] = "TestUser";
            string strLoginId = Convert.ToString(Session["LoginID"]);
            new ClsCommonBAL().FunInsertIntoErrorLog("CS, SwitchOperationSite, Page_Load()", "strLoginId : " + strLoginId, "");
        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Equals("test"))
            {
                Session["LoginID"] = txtName.Text;
                Response.Redirect("frmTest2.aspx",false);
            }
        }
    }
}