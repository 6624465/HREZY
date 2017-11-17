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

        [HttpPost]
        public ActionResult SaveEmployeeLeaveForm(EmployeeLeaveList EmployeeLeaveList)
        {

            using (var dbCntx = new HrDataContext())
            {
                EmployeeLeaveList obj = new EmployeeLeaveList();

                obj.FromDate = EmployeeLeaveList.FromDate;
                obj.ToDate = EmployeeLeaveList.ToDate;
                obj.Days = EmployeeLeaveList.Days;
                obj.EmployeeId = EmployeeLeaveList.EmployeeId;
                obj.LeaveTypeId = EmployeeLeaveList.LeaveTypeId;
                obj.Remarks = EmployeeLeaveList.Remarks;
                obj.Reason = EmployeeLeaveList.Reason;
                obj.CreatedBy = USERID;
                obj.CreatedOn = UTILITY.SINGAPORETIME;
                dbCntx.EmployeeLeaveLists.Add(obj);
                dbCntx.SaveChanges();
                return View("EmployeeRequestFrom");
            }
        }
        public ActionResult GrantLeaveForm()
        {
            return View(new LeaveVm { });
        }
        
    }
}