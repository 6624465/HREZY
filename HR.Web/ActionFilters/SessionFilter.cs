using HR.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web
{
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var SSN_USERID = ((SessionObj)HttpContext.Current.Session[UTILITY.SSN_USERID]).USERID;
            if (string.IsNullOrWhiteSpace(SSN_USERID))
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
}