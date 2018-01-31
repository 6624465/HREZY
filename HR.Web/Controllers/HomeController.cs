using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{
    [SessionFilter]
    [ErrorHandler]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}