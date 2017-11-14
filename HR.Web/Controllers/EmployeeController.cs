using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class EmployeeController : Controller
    {
               // GET: Employee
        public ActionResult GetEmployees()
        {
            return View();
        }


        public ActionResult SaveEmployee()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult SaveEmployee(EmployeeHeader empHeader)
        //{
        //    return View();
        //}


    }
}