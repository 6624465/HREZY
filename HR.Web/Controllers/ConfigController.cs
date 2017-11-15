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
        // GET: Config
        public ActionResult EmployeeDesignation()
        {
         var  list = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeDesignation").Select(m => new
            {
                LookUpCode = m.LookUpCode,
                LookUpDescription = m.LookUpDescription,
                LookUpID = m.LookUpID,
                IsActive = m.IsActive
            }).AsQueryable();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public  ActionResult Index()
        {

            return View();
        }
    }
}