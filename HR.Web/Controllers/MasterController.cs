using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class MasterController : BaseController
    {
        // GET: Master
        public ActionResult List()
        {
            return View();
        }
    }
}