using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Models;

namespace HR.Web.Controllers
{
    [Authorize]
    public class LeaveController : BaseController
    {
        // GET: Leave
        public ActionResult HolidayList()
        {
            return View();
        }

        public PartialViewResult GetHolidayList()
        {
                return PartialView(new HolidayList
                {
                    Date = DateTime.Now,
                });
            
        }
      
        public ActionResult AppliedLeaveList()
        {
            return View();
        }
        public ActionResult EmployeeRequestFrom()
        {
            return View(new EmployeeLeaveList {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            });
        }
        public ActionResult GrantLeaveForm()
        {
            return View(new LeaveVm { });
        }
        
    }
}