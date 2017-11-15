using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}