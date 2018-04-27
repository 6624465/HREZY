using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class YearlyReportsController : BaseController
    {
        // GET: YearlyReports
       public ActionResult YearlyReportsTDS()
        {
            return View();
        }
        public ActionResult YearlyReportsSSC()
        {
            return View();
        }
        public ActionResult YearlyReportsOne()
        {
            return View();
        }
        public ActionResult YearlyReportsTwo()
        {
            return View();
        }
    }
}