using AGS.SwitchOperations.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["LoginID"])))
                    {
                        AGS.SwitchOperations.Common.ClsCommonDAL.UserActivity_DBLog(
                                                            Convert.ToString(HttpContext.Current.Session["LoginID"]),
                                                            Convert.ToString(Session["UserName"]),
                                                            System.IO.Path.GetFileName(Request.CurrentExecutionFilePath), "Page_Load"
                                                            );
                    }
                }
            }
            catch(Exception ex)
            {
                //(new ClsCommonBAL()).FunInsertIntoErrorLog("CS, Home, Page_Load()", ex.Message, "");
            }
        }
    }
}