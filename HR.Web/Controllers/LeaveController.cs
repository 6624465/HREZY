using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class LeaveController : Controller
    {
        // GET: Leave
        public ActionResult HolidayList()
        {
            return View();
        }

        public PartialViewResult GetHolidayList()
        {
                return PartialView();
            
        }
        public ActionResult AppliedLeaveList()
        {
            return View();
        }
        public ActionResult EmployeeRequestFrom()
        {
            return View();
        }
        public ActionResult GrantLeaveForm()
        {
            return View(new LeaveVm { });
        }
        
    }
}