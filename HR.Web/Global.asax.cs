using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using HR.Web.Helpers;
using System.Threading;
using System.Globalization;

namespace HR.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(DateTime), new MyDateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new MyDateTimeModelBinder());

            ModelBinders.Binders.Add(typeof(string), new MyStringModelBinder());
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();
        //    Response.Redirect("~/Error/NotFound");
        //}

        /*
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            /*
            try
            {
                string cultureName = HttpContext.Current.Request.UserLanguages[0];
                if (cultureName != null && cultureName != "en-US")
                {
                    // Unable to pick culture from browser setting, need to review this
                    // <globalization enableClientBasedCulture="true" culture="auto:en-US" uiCulture="auto:en"/>
                    var culture = new CultureInfo(cultureName);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
            catch
            {
            }*/
        //HttpApplication application = (HttpApplication)source;
        //HttpContext context = application.Context;

        //string culture = null;
        //if (context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
        //{
        //    culture = Request.UserLanguages[0];
        //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
        //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        //}
        //}

    }
}
