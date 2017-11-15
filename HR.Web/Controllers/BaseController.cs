using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class BaseController : Controller
    {

        public string USERID
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session[UTILITY.SSN_USERID];
            }
            set
            {
                Session[UTILITY.SSN_USERID] = value;
            }
        }
    }
}