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

        public ActionResult GetDesignation()
        {
            var EmployeeData = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeDesignation").AsQueryable();
            return View(EmployeeData);
        }
        [HttpPost]
        public ActionResult GetDesignationById(int lookupId)
        {
            var id = dbContext.LookUps.Where(x => x.LookUpID == lookupId).FirstOrDefault();
            if (id == null)
            {

            }
            else
            {

            }
            dbContext.SaveChanges();
            return RedirectToAction("GetDesignation");
        }
    }
}
