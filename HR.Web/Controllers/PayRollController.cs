using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class PayRollController : Controller
    {
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }

    }
}