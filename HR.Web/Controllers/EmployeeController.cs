using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HR.Web.Models;
using HR.Web.ViewModels;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View(new EmployeeVm { });
        }

        [HttpPost]
        public ActionResult SaveEmployee()
        {
            return View();
        }    
      


    }
}