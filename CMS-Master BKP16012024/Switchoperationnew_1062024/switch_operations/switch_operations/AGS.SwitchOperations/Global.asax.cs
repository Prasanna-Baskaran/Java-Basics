using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AGS.SwitchOperations
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
          
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           // for security changes
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
            Response.Headers.Set("Server", "");
            Response.Headers.Set("X-AspNet-Version", "");
            Response.Headers.Set("X-AspNetMvc-Version", "");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoStore();
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 30));
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }


        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
           // Response.Headers.Remove("Server");
          //  Remove the "Server" HTTP Header from response

           HttpApplication app = sender as HttpApplication;

            if (null != app && null != app.Request && //app.Request.IsLocal &&

                null != app.Context && null != app.Context.Response)

            {

                NameValueCollection headers = app.Context.Response.Headers;

                if (null != headers)

                {

                    headers.Remove("Server");
                    headers.Remove("X-AspNet-Version");
                    headers.Remove("X-AspNetMvc-Version");
                    headers.Remove("Cache-Control");
                }
            }

        }



    }
}
