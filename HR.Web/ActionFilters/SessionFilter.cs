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
            var SSN_OBJ = ((SessionObj)HttpContext.Current.Session[UTILITY.SSN_OBJECT]);
            if (SSN_OBJ == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
}