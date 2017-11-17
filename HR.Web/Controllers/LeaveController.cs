using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class LeaveController : BaseController
    {
        // GET: Leave
        public ActionResult HolidayList()
        {
            return View();
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
            using (HrDataContext dbContext = new HrDataContext())
            {
                LeaveVm leavevm = new LeaveVm();
                leavevm.lookup = new LookUp();
                leavevm.leaveHeader = new LeaveHeader();
                var lookups = dbContext.LookUps;
                var lvmList = dbContext.LeaveHeaders
                                 .Select(x => new LeaveHeaderVm {
                                     LeaveHeaderID = x.LeaveHeaderID,
                                     BranchID = x.BranchID,
                                     LeaveSchemeType = x.LeaveSchemeType,
                                     LeaveSchemeTypeDescription = lookups.Where(y => y.LookUpID == x.LeaveSchemeType).FirstOrDefault().LookUpDescription,
                                     LeaveYear=x.LeaveYear,
                                     LeaveYearDescription=lookups.Where(y=>y.LookUpID==x.LeaveYear).FirstOrDefault().LookUpDescription,
                                     PeriodicityType= x.PeriodicityType,
                                     PeriodicityTypeDescription= lookups.Where(y=>y.LookUpID==x.PeriodicityType).FirstOrDefault().LookUpDescription,
                                     PeriodType=x.PeriodType,
                                     PeriodTypeDescription=lookups.Where(y=>y.LookUpID==x.PeriodType).FirstOrDefault().LookUpDescription
                                 }).ToList().AsEnumerable();
                leavevm.lvmList = lvmList;
                return View(leavevm);
            }
           // return View(new LeaveHeader());
        }
        [HttpPost]
        public ActionResult SaveGrantLeave(LeaveVm lvm)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext()) {
                    var leaveheader = new LeaveHeader {
                        BranchID = BRANCHID,
                        LeaveHeaderID = lvm.leaveHeader.LeaveHeaderID,
                        LeaveYear = lvm.leaveHeader.LeaveYear,
                        PeriodicityType = lvm.leaveHeader.PeriodicityType,
                        PeriodType = lvm.leaveHeader.PeriodType,
                        LeaveSchemeType = lvm.leaveHeader.LeaveSchemeType,
                        CreatedBy = USERID,
                        ModifiedBy = USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        ModifiedOn = UTILITY.SINGAPORETIME,
                };
                    dbContext.LeaveHeaders.Add(leaveheader);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GrantLeaveForm");
        }


    }
}