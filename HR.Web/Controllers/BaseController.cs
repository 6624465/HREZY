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

        public int EMPLOYEEID
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[UTILITY.SSN_EMPLOYEEID];
            }
            set
            {
                Session[UTILITY.SSN_EMPLOYEEID] = value;
            }
        }

        public int BRANCHID
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[UTILITY.SSN_BRANCHID];
            }
            set
            {
                Session[UTILITY.SSN_BRANCHID] = value;
            }
        }

        public string ROLECODE
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session[UTILITY.CONFIG_ROLECODE];
            }
            set
            {
                Session[UTILITY.CONFIG_ROLECODE] = value;
            }
        }
    }
}