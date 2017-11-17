using HR.Web.Models;
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


                ViewData["LeaveTypeList"] = lookups.Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEELEAVETYPE).ToList();
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