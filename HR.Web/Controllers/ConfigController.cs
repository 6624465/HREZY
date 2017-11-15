using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class ConfigController : Controller
    {
        HrDataContext dbContext = new HrDataContext();
       
        public  ActionResult Index()
        {

            return View();
        }
        public ActionResult EmployeeType()
        {

            return View();
        }
    }
}