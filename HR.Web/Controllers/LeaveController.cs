using HR.Web.Helpers;
using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class LeaveController : BaseController
    {


        // GET: Leave
        #region HolidayList
        public ActionResult HolidayList()
        {

            using (HrDataContext dbContext = new HrDataContext())
            {
                var obj = dbContext.HolidayLists.ToList();
                List<calendarVM> holidayList = new List<calendarVM>();
                foreach (HolidayList item in obj)
                {
                    calendarVM list = new calendarVM();
                    list.title = item.Description;
                    list.date = item.Date;

                    var strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

                    var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                    string hrefUrl = UrlHelper.GenerateContentUrl(strHref, context);
                    list.url = hrefUrl;
                    holidayList.Add(list);
                }
                holidayVm holidayVm = new holidayVm()
                {
                    calendarVM = holidayList,
                    HolidayList = new HolidayList()
                };
                return View("HolidayList", holidayVm);
            }

        }

        public ActionResult AddHoliday(int HolidayId)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                var obj = dbContext.HolidayLists.ToList();
                holidayVm holidayVm = new holidayVm();
                List<calendarVM> holidayList = new List<calendarVM>();
                foreach (HolidayList item in obj)
                {
                    calendarVM list = new calendarVM();
                    list.title = item.Description;
                    list.date = item.Date;

                    var strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

                    var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                    string hrefUrl = UrlHelper.GenerateContentUrl(strHref, context);
                    list.url = hrefUrl;
                    holidayList.Add(list);



                    holidayVm.calendarVM = holidayList;
                    holidayVm.HolidayList = dbContext.HolidayLists.Where(x => x.HolidayId == HolidayId).FirstOrDefault();
                }
                return View("HolidayList", holidayVm);
            }

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
                var dt = DateTime.Now;
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

                    //_holidaylistObj.HolidayId = holidaylist.HolidayId;
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





        #endregion


        public ViewResult AppliedLeaveList()
        {
            string viewName = string.Empty;
            using (var dbCntx = new HrDataContext())
            {
                var empLeaveList = dbCntx.Branches.GroupJoin(dbCntx.EmployeeLeaveLists.
                    empLeaveListWhere(ROLECODE, BRANCHID, EMPLOYEEID, ref viewName),
                        a => a.BranchID, b => b.BranchId, (a, b) => new { A = a, B = b.AsEnumerable() })

                        .Select(x => new AppliedLeaveListVm
                        {
                            BranchID = x.A.BranchID,
                            BranchName = x.A.BranchName,
                            employeeLeaveList = x.B.Select(y => new EmpLeaveListVm
                            {
                                EmployeeName = dbCntx.EmployeeHeaders.Where(m => m.EmployeeId == y.EmployeeId).Select(z => new
                                {
                                    EmployeeFullName = z.FirstName + " " + z.LastName
                                }).FirstOrDefault().EmployeeFullName,
                                EmployeeId = y.EmployeeId,
                                FromDate = y.FromDate,
                                ToDate = y.ToDate
                            })
                        }).ToList();

                return View(viewName, empLeaveList);
            }
        }

        public JsonResult GetBranchLeaveData(int branchId, int pageNo)
        {

            int offSet = 10;
            int skipRows = (pageNo - 1) * offSet;
            using (var dbCntx = new HrDataContext())
            {
                var Query = dbCntx.EmployeeHeaders
                                        .Join(dbCntx.EmployeeLeaveLists,
                                        a => a.BranchId, b => b.BranchId, (a, b) => new { A = a, B = b })
                                        .Where(x => x.A.BranchId == branchId);

                var empLeaveList = Query
                                        .OrderBy(x => x.A.EmployeeId)
                                        .Skip(skipRows)
                                        .Take(offSet)
                                        .Select(x => new EmpLeaveListVm
                                        {
                                            EmployeeName = x.A.FirstName + " " + x.A.LastName,
                                            EmployeeId = x.B.EmployeeId,
                                            FromDate = x.B.FromDate,
                                            ToDate = x.B.ToDate
                                        }).ToList();

                return Json(new
                {
                    empLeaveList = empLeaveList,
                    pagerLength = Query.Count()
                }, JsonRequestBehavior.AllowGet);
            }
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
            string ishalfday = Request.Form["chkhalfday"];
            using (var dbCntx = new HrDataContext())
            {
                EmployeeLeaveList obj = new EmployeeLeaveList();

                var isValid = dbCntx.EmployeeLeaveLists
                                .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID)
                                .Between(EmployeeLeaveList.FromDate, EmployeeLeaveList.ToDate)
                                .Count();


                if (isValid == 0)
                {
                    obj.BranchId = BRANCHID;
                    obj.FromDate = EmployeeLeaveList.FromDate;
                    obj.ToDate = EmployeeLeaveList.ToDate;
                    obj.Days = EmployeeLeaveList.Days;
                    obj.EmployeeId = EMPLOYEEID;
                    obj.LeaveTypeId = EmployeeLeaveList.LeaveTypeId;
                    obj.Remarks = EmployeeLeaveList.Remarks;
                    obj.Reason = EmployeeLeaveList.Reason;
                    obj.CreatedBy = USERID;
                    obj.CreatedOn = UTILITY.SINGAPORETIME;
                    dbCntx.EmployeeLeaveLists.Add(obj);


                    LeaveTransaction leavetransaction = dbCntx.LeaveTransactions
                         .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    LeaveListCalc leaveListCalc = null;
                    if (leavetransaction != null)
                    {


                        leaveListCalc = new LeaveListCalc(leavetransaction.CurrentCasualLeaves, leavetransaction.CurrentPaidLeaves, leavetransaction.CurrentSickLeaves,
                            leavetransaction.PreviousCasualLeaves, leavetransaction.PreviousPaidLeaves, leavetransaction.PreviousSickLeaves
                            );
                        CalculateLeaves.CalculateLeaveFromTransaction(leavetransaction, EmployeeLeaveList, leaveListCalc, true);
                    }
                    else
                    {
                        Leave leave = dbCntx.Leaves
                             .Where(x => x.BranchId == BRANCHID).FirstOrDefault();
                        leaveListCalc = new LeaveListCalc(leave.CasualLeavesPerYear.Value, leave.SickLeavesPerYear.Value, leave.PaidLeavesPerYear.Value, leave.CasualLeavesPerYear.Value, leave.SickLeavesPerYear.Value, leave.PaidLeavesPerYear.Value);
                        CalculateLeaves.CalculateLeave(leave, EmployeeLeaveList, leaveListCalc);


                    }
                    LeaveTransaction leaveTransaction = new LeaveTransaction()
                    {
                        BranchId = BRANCHID,
                        CreatedBy = USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CurrentCasualLeaves = leaveListCalc.currentCasualLeaves,
                        CurrentPaidLeaves = leaveListCalc.currentPaidLeaves,
                        CurrentSickLeaves = leaveListCalc.currentSickLeaves,
                        EmployeeId = EMPLOYEEID,
                        FromDt = obj.FromDate,
                        ToDt = obj.ToDate,
                        PreviousCasualLeaves = leaveListCalc.previousCasualLeaves,
                        PreviousPaidLeaves = leaveListCalc.previousPaidLeaves,
                        PreviousSickLeaves = leaveListCalc.previousSickLeaves,
                    };
                    dbCntx.LeaveTransactions.Add(leaveTransaction);

                    dbCntx.SaveChanges();

                    return View("EmployeeRequestFrom");
                }
                else
                {
                    ViewData["Message"] = "You have already applied a leave within this date range. Please check.";
                    return View("EmployeeRequestFrom");
                }

            }
        }

        [HttpGet]
        public ActionResult GrantLeaveFormList()
        {
            using (var dbcntx = new HrDataContext())
            {
                var grantleaveform = dbcntx.EmployeeHeaders.
                     Join(dbcntx.EmployeeLeaveLists,
                     a => a.EmployeeId, b => b.EmployeeId,
                     (a, b) => new { A = a, B = b })
                     .Where(x => x.A.ManagerId == EMPLOYEEID)
                     .Select(x => new GrantLeaveListVm
                     {
                         ToDate = x.B.ToDate,
                         FromDate = x.B.FromDate,
                         Name = x.A.FirstName + " " + x.A.LastName,
                         EmployeeId = x.A.EmployeeId,
                         EmployeeLeaveID = x.B.EmployeeLeaveID,
                         Status = x.B.Status
                     })
                     .ToList()
                     .AsEnumerable();

                return View("GrantLeaveForm", grantleaveform);
            }
        }

        [HttpGet]
        public ActionResult AppliedGrantLeaveStatus(int? EmployeeLeaveID)
        {
            if (EmployeeLeaveID != null)
            {
                using (var dbcntx = new HrDataContext())
                {
                    var Leavestatus = dbcntx.EmployeeHeaders.Join(dbcntx.EmployeeLeaveLists,
                        a => a.EmployeeId, b => b.EmployeeId,
                        (a, b) => new { A = a, B = b })
                        .Where(x => x.A.ManagerId == EMPLOYEEID && x.B.EmployeeLeaveID == EmployeeLeaveID)
                     .Select(x => new GrantLeaveListVm
                     {
                         ToDate = x.B.ToDate,
                         FromDate = x.B.FromDate,
                         Name = x.A.FirstName + " " + x.A.LastName,
                         EmployeeId = x.A.EmployeeId,
                         Remarks = x.B.Remarks,
                         Reason = x.B.Reason,
                         EmployeeLeaveID = x.B.EmployeeLeaveID,
                         Status = x.B.Status
                     }).FirstOrDefault();

                    return View(Leavestatus);

                }
            }
            else
            {
                return RedirectToAction("GrantLeaveForm");
            }
        }

        [HttpPost]
        public ActionResult ApproveLeave(GrantLeaveListVm grantLeaveVm)
        {
            using (var dbcntx = new HrDataContext())
            {
                var empLeaveObj = dbcntx.EmployeeLeaveLists
                                    .Where(x => x.EmployeeLeaveID == grantLeaveVm.EmployeeLeaveID)
                                    .FirstOrDefault();


                empLeaveObj.Status = "Approved";
                empLeaveObj.Remarks = "";
                dbcntx.SaveChanges();

                return RedirectToAction("AppliedGrantLeaveStatus", new { EmployeeLeaveID = grantLeaveVm.EmployeeLeaveID });
            }
        }

        [HttpPost]
        public ActionResult RejectLeave(GrantLeaveListVm grantLeaveVm)
        {
            using (var dbCntx = new HrDataContext())
            {
                var empLeaveObj = dbCntx.EmployeeLeaveLists
                                    .Where(x => x.EmployeeLeaveID == grantLeaveVm.EmployeeLeaveID)
                                    .FirstOrDefault();

                empLeaveObj.Status = "Rejected";
                empLeaveObj.Remarks = grantLeaveVm.Remarks;

                dbCntx.SaveChanges();

                return RedirectToAction("AppliedGrantLeaveStatus", new { EmployeeLeaveID = grantLeaveVm.EmployeeLeaveID });
            }
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
        public ActionResult LeaveList()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                List<Leave> leaves = dbContext.Leaves.ToList();
                //leaves.ForEach(x => x.BranchId = dbContext.Branches.Where(y => y.BranchID == x.BranchId).FirstOrDefault().BranchName);
                return View(leaves);
            }
        }

        [HttpGet]
        public ActionResult Leave(int leaveId = 0)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                ViewBag.RoleCode = ROLECODE;

                Leave leave = null;
                if (leaveId != 0)
                {
                    leave = dbContext.Leaves
                                .leaveWhere(BRANCHID, ROLECODE)
                                .FirstOrDefault();
                }
                else
                    leave = dbContext.Leaves
                                .Where(x => x.BranchId == BRANCHID)
                                .FirstOrDefault();
                return View(leave);
            }
        }
        [HttpPost]
        public ActionResult Leave(Leave leave)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                if (leave.LeaveId == 0)
                {
                    leave.BranchId = BRANCHID;
                    dbContext.Leaves.Add(leave);
                }
                else
                {
                    Leave updateLeave = dbContext.Leaves
                                           .leaveWhere(BRANCHID, ROLECODE)
                                           .FirstOrDefault();

                    updateLeave.CarryForwardPerYear = leave.CarryForwardPerYear;
                    updateLeave.CarryForwardSickLeaves = leave.CarryForwardSickLeaves;
                    updateLeave.CasualLeavesPerMonth = leave.CasualLeavesPerMonth;
                    updateLeave.CasualLeavesPerYear = leave.CasualLeavesPerYear;
                    updateLeave.IsCasualLeaveCarryForward = leave.IsCasualLeaveCarryForward;
                    updateLeave.IsPaidLeaveCarryForward = leave.IsPaidLeaveCarryForward;
                    updateLeave.IsSickLeaveCarryForward = leave.IsSickLeaveCarryForward;
                    updateLeave.PaidLeavesPerMonth = leave.PaidLeavesPerMonth;
                    updateLeave.PaidLeavesPerYear = leave.PaidLeavesPerYear;
                    updateLeave.SickLeavesPerMonth = leave.SickLeavesPerMonth;
                    updateLeave.SickLeavesPerYear = leave.SickLeavesPerYear;
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("Leave");
        }

        public ActionResult ViewLeavesList()
        {
            using (var dbcntx = new HrDataContext())
            {
                var viewleavelist = dbcntx.EmployeeLeaveLists.Where(x => x.EmployeeId == EMPLOYEEID).ToList().AsEnumerable();
                return View(viewleavelist);
            }

        }
        [HttpPost]
        public ActionResult CancelLeave(FormCollection collection)
        {
            int employeeLeaveID = Convert.ToInt32(collection["employeeLeaveID"]);
            string remarks = collection["employeeRemarks"];

            using (HrDataContext dbContext = new HrDataContext())
            {
                try
                {

                    EmployeeLeaveList empLeaveObj = dbContext.EmployeeLeaveLists.Where(x => x.BranchId == BRANCHID && x.EmployeeLeaveID == employeeLeaveID).FirstOrDefault();
                    empLeaveObj.Status = "Cancelled";
                    empLeaveObj.Remarks = remarks;

                    LeaveTransaction leavetransaction = dbContext.LeaveTransactions
                             .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    LeaveListCalc leaveListCalc = null;
                    if (leavetransaction != null)
                    {


                        leaveListCalc = new LeaveListCalc(leavetransaction.CurrentCasualLeaves, leavetransaction.CurrentPaidLeaves, leavetransaction.CurrentSickLeaves,
                            leavetransaction.PreviousCasualLeaves, leavetransaction.PreviousPaidLeaves, leavetransaction.PreviousSickLeaves
                            );
                        CalculateLeaves.CalculateLeaveFromTransaction(leavetransaction, empLeaveObj, leaveListCalc, false);
                    }

                    LeaveTransaction leaveTransaction = new LeaveTransaction()
                    {
                        BranchId = BRANCHID,
                        CreatedBy = USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CurrentCasualLeaves = leaveListCalc.currentCasualLeaves,
                        CurrentPaidLeaves = leaveListCalc.currentPaidLeaves,
                        CurrentSickLeaves = leaveListCalc.currentSickLeaves,
                        EmployeeId = EMPLOYEEID,
                        FromDt = empLeaveObj.FromDate,
                        ToDt = empLeaveObj.ToDate,
                        PreviousCasualLeaves = leaveListCalc.previousCasualLeaves,
                        PreviousPaidLeaves = leaveListCalc.previousPaidLeaves,
                        PreviousSickLeaves = leaveListCalc.previousSickLeaves,
                    };
                    dbContext.LeaveTransactions.Add(leaveTransaction);


                    dbContext.SaveChanges();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }


            return RedirectToAction("ViewLeavesList");
        }
    }

}