using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public PartialViewResult GetHolidayList(int holidayId)
        {
            if (holidayId != -1)
            {
                using (var dbntx = new HrDataContext())
                {
                    var holidaylist = dbntx.HolidayLists.Where(x => x.HolidayId == holidayId).FirstOrDefault();
                    return PartialView(holidaylist);
                }
            }
            else
            {
                return PartialView(new HolidayList
                {
                    Date = DateTime.Now,
                    HolidayId = -1
                });
            }

        }

        public ActionResult SaveHoliday(HolidayList holidaylist)
        {
            if (holidaylist.HolidayId != -1)
            {
                using (var dbnctx = new HrDataContext())
                {
                    var _holidaylistObj = dbnctx.HolidayLists.Where(x => x.HolidayId == holidaylist.HolidayId).FirstOrDefault();

                    _holidaylistObj.HolidayId = holidaylist.HolidayId;
                    _holidaylistObj.Date = holidaylist.Date;
                    _holidaylistObj.Description = holidaylist.Description;
                    _holidaylistObj.CountryId = holidaylist.CountryId;
                    _holidaylistObj.ModifiedBy = USERID;
                    _holidaylistObj.ModifiedOn = UTILITY.SINGAPORETIME;

                    dbnctx.SaveChanges();
                }
            }
            else
            {

                using (var dbntcx = new HrDataContext())
                {
                    var holidaylistobj = new HolidayList
                    {
                        BranchID = BRANCHID,
                        HolidayId = holidaylist.HolidayId,
                        Date = holidaylist.Date,
                        Description = holidaylist.Description,
                        CountryId = holidaylist.CountryId,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CreatedBy = USERID,
                        ModifiedOn = UTILITY.SINGAPORETIME,
                        ModifiedBy = USERID
                    };
                    dbntcx.HolidayLists.Add(holidaylistobj);
                    dbntcx.SaveChanges();
                }
            }
            return RedirectToAction("HolidayList");

        }


        public ActionResult AppliedLeaveList()
        {
            return View();
        }
        public ActionResult EmployeeRequestFrom()
        {
            return View(new EmployeeLeaveList
            {
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
                                 .Select(x => new LeaveHeaderVm
                                 {
                                     LeaveHeaderID = x.LeaveHeaderID,
                                     BranchID = x.BranchID,
                                     LeaveSchemeType = x.LeaveSchemeType,
                                     LeaveSchemeTypeDescription = lookups.Where(y => y.LookUpID == x.LeaveSchemeType).FirstOrDefault().LookUpDescription,
                                     LeaveYear = x.LeaveYear,
                                     LeaveYearDescription = lookups.Where(y => y.LookUpID == x.LeaveYear).FirstOrDefault().LookUpDescription,
                                     PeriodicityType = x.PeriodicityType,
                                     PeriodicityTypeDescription = lookups.Where(y => y.LookUpID == x.PeriodicityType).FirstOrDefault().LookUpDescription,
                                     PeriodType = x.PeriodType,
                                     PeriodTypeDescription = lookups.Where(y => y.LookUpID == x.PeriodType).FirstOrDefault().LookUpDescription
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    var leaveheader = new LeaveHeader
                    {
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

        [HttpGet]
        public ActionResult Leave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Leave(Leave leave)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                dbContext.Leaves.Add(leave);
                dbContext.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public JsonResult events()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

    }
}