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
            ViewData["RoleCode"] = ROLECODE;
            using (HrDataContext dbContext = new HrDataContext())
            {
                var obj = dbContext.HolidayLists.ToList();
                List<calendarVM> holidayList = new List<calendarVM>();
                foreach (HolidayList item in obj)
                {
                    calendarVM list = new calendarVM();
                    list.title = item.Description;
                    list.date = item.Date;
                    var strHref = "";
                    if (ROLECODE == UTILITY.ROLE_EMPLOYEE)
                    {
                        strHref = "#";
                    }
                    else
                        strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

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
            ViewData["RoleCode"] = ROLECODE;
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
                // FromDate = DateTime.Now,
                //  ToDate = DateTime.Now
            });
        }

        [HttpPost]
        public ActionResult SaveEmployeeLeaveForm(EmployeeLeaveList EmployeeLeaveList)
        {
            bool ishalfday = false;
            if (Request.Form["isChecked"] != null && Request.Form["isChecked"] != "")
            {
                ishalfday = Request.Form["isChecked"] == "on";
                if (ishalfday)
                    EmployeeLeaveList.Days = 0.5m;
            }
            using (var dbCntx = new HrDataContext())
            {
                EmployeeLeaveList obj = new EmployeeLeaveList();

                var isPreviousLeaveExists = dbCntx.EmployeeLeaveLists
                                            .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID)
                                            .Between(EmployeeLeaveList.FromDate, EmployeeLeaveList.ToDate)
                                            .Count() > 0;

                LeaveTransaction leavetransaction = dbCntx.LeaveTransactions
                            .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn)
                            .FirstOrDefault();

                decimal eligibleLeaves = 0;
                if (leavetransaction == null)
                {
                    eligibleLeaves = 1;
                }
                else
                {
                    LeaveCalculator leaveCal = new LeaveCalculator();
                    eligibleLeaves = leaveCal.GetLeavesCount(BRANCHID, EMPLOYEEID, EmployeeLeaveList.LeaveTypeId.Value,
                                                EmployeeLeaveList.FromDate);
                }
                if (eligibleLeaves != 0)
                {
                    //if (isValid ==0 && eligibleLeaves == EmployeeLeaveList.Days || ishalfday)
                    //{
                    if (!isPreviousLeaveExists || eligibleLeaves >= EmployeeLeaveList.Days)
                    {
                        //eligibleLeaves >= EmployeeLeaveList.Days -- need to check this condition
                        //|| ishalfday 
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
                        obj.ModifiedBy = USERID;
                        obj.ModifiedOn = UTILITY.SINGAPORETIME;
                        obj.ApplyDate = UTILITY.SINGAPORETIME;
                        obj.ManagerId = dbCntx.EmployeeHeaders
                                            .Where(x => x.EmployeeId == EMPLOYEEID)
                                            .FirstOrDefault()
                                            .ManagerId.Value;
                        obj.Status = UTILITY.LEAVEPENDING;

                        dbCntx.EmployeeLeaveLists.Add(obj);



                        LeaveListCalc leaveListCalc = null;
                        if (leavetransaction != null)
                        {


                            leaveListCalc = new LeaveListCalc(leavetransaction.CurrentCasualLeaves,
                                                                leavetransaction.CurrentPaidLeaves,
                                                                leavetransaction.CurrentSickLeaves,
                                                                leavetransaction.PreviousCasualLeaves,
                                                                leavetransaction.PreviousPaidLeaves,
                                                                leavetransaction.PreviousSickLeaves
                                );
                            CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, EmployeeLeaveList, leaveListCalc, true);
                        }
                        else
                        {
                            Leave leave = dbCntx.Leaves
                                 .Where(x => x.BranchId == BRANCHID).FirstOrDefault();
                            leaveListCalc = new LeaveListCalc(leave.CasualLeavesPerYear.Value,
                                                                leave.PaidLeavesPerYear.Value,
                                                                leave.SickLeavesPerYear.Value,
                                                                leave.CasualLeavesPerYear.Value,
                                                                leave.PaidLeavesPerYear.Value,
                                                                leave.SickLeavesPerYear.Value);
                            CalculateLeavesTransaction.CalculateLeave(leave, EmployeeLeaveList, leaveListCalc);


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
                            LeaveType = EmployeeLeaveList.LeaveTypeId,
                            ModifiedBy = USERID,
                            ModifiedOn = UTILITY.SINGAPORETIME
                        };
                        dbCntx.LeaveTransactions.Add(leaveTransaction);

                        dbCntx.SaveChanges();

                        return RedirectToAction("ViewLeavesList");
                    }
                    else
                    {
                        if (isPreviousLeaveExists)
                            ViewData["Message"] = "You have already applied a leave within this date range. Please check.";
                        if (eligibleLeaves >= EmployeeLeaveList.Days)
                            ViewData["Message"] = "You are not eligible for applied number of leaves";

                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                }
                else
                {
                    ViewData["Message"] = "You dont have leaves.";
                    return View("EmployeeRequestFrom", EmployeeLeaveList);
                }

            }
        }

        [HttpGet]
        public ActionResult GrantLeaveFormList(int? page = 1)
        {

            using (var dbcntx = new HrDataContext())
            {
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appViewLeaveListOffSet"]);
                var skip = (page - 1) * offset;

                var grantleaveform = dbcntx.EmployeeHeaders.
                    Join(dbcntx.EmployeeLeaveLists,
                    a => a.EmployeeId, b => b.EmployeeId,
                    (a, b) => new { A = a, B = b })
                    .Where(x => x.B.ManagerId == EMPLOYEEID && x.B.Status!=UTILITY.LEAVECANCELLED)
                    .Select(x => new GrantLeaveListVm
                    {
                        ToDate = x.B.ToDate,
                        FromDate = x.B.FromDate,
                        Name = x.A.FirstName + " " + x.A.LastName,
                        EmployeeId = x.A.EmployeeId,
                        EmployeeLeaveID = x.B.EmployeeLeaveID,
                        Status = x.B.Status,
                        ApplyDate = x.B.ApplyDate,
                        Reason = x.B.Reason,
                        Remarks = x.B.Remarks,
                        LeaveTypeDesc = dbcntx.LookUps
                                            .Where(y => y.LookUpID == x.B.LeaveTypeId)
                                            .FirstOrDefault()
                                            .LookUpDescription,
                        TotalDays = x.B.Days
                    });
                var grantleaveformlist = grantleaveform.OrderByDescending(x => x.ApplyDate)
                     .Skip(skip.Value)
                     .Take(offset)
                     .ToList();

                var count = grantleaveformlist.Count();
                decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));

                HtmlTblVm<GrantLeaveListVm> HtmlTblVm = new HtmlTblVm<GrantLeaveListVm>();
                HtmlTblVm.TableData = grantleaveformlist;
                HtmlTblVm.TotalRows = count;
                HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
                HtmlTblVm.CurrentPage = page.Value;
                return View(HtmlTblVm);
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
                return RedirectToAction("GrantLeaveFormList");
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
                var leaves = dbContext.Leaves.Join(dbContext.Branches,
                    a => a.BranchId, b => b.BranchID,
                    (a, b) => new { A = a, B = b }).Select(x=>new LeavepolicyVm
                    {
                        LeaveId = x.A.LeaveId,
                        PaidLeavesPerYear = x.A.PaidLeavesPerYear,
                        PaidLeavesPerMonth = x.A.PaidLeavesPerMonth,
                        IsPaidLeaveCarryForward = x.A.IsPaidLeaveCarryForward,
                        CarryFarwardPerYear = x.A.CarryForwardPerYear,
                        SickLeavesPerYear = x.A.SickLeavesPerYear,
                        SickLeavesPerMonth = x.A.SickLeavesPerMonth,
                        IsSickLeaveCarryFarward = x.A.IsSickLeaveCarryForward,
                        CarryFarwardSickLeaves = x.A.CarryForwardSickLeaves,
                        CasualLeavesPerYear = x.A.CasualLeavesPerYear,
                        CasualLeavesPerMonth = x.A.CasualLeavesPerMonth,
                        IsCasualLeaveCarryFarward = x.A.IsCasualLeaveCarryForward,
                        BranchId = x.A.BranchId,
                        BranchName = x.B.BranchName
                    }).ToList();
                return View(leaves);
                }

                //List<Leave> leaves = dbContext.Leaves.ToList();
                ////leaves.ForEach(x => x.BranchId = dbContext.Branches.Where(y => y.BranchID == x.BranchId).FirstOrDefault().BranchName);
                //return View(leaves);
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
                    leave = new Leave();
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
                    updateLeave.CarryForwardCasualLeave = leave.CarryForwardCasualLeave;
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("Leave");
        }

        public ActionResult ViewLeavesList(int? page = 1)
        {

            using (var dbcntx = new HrDataContext())
            {
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appViewLeaveListOffSet"]);
                var skip = (page - 1) * offset;

                var Query = dbcntx.EmployeeLeaveLists
                                        .Join(dbcntx.EmployeeHeaders,
                                        a => a.ManagerId, b => b.EmployeeId,
                                        (a, b) => new { A = a, B = b })
                                        .Where(x => x.A.EmployeeId == EMPLOYEEID)
                                        .Select(x => new ViewLeaveVm
                                        {
                                            EmployeeLeaveID = x.A.EmployeeLeaveID,
                                            EmployeeId = x.A.EmployeeId,
                                            LeaveTypeId = x.A.LeaveTypeId,
                                            LeaveTypeDesc = dbcntx.LookUps
                                                                .Where(y => y.LookUpID == x.A.LeaveTypeId)
                                                                .Select(y => y.LookUpDescription)
                                                                .FirstOrDefault(),
                                            BranchId = x.A.BranchId,
                                            FromDate = x.A.FromDate,
                                            ToDate = x.A.ToDate,
                                            Days = x.A.Days,
                                            Reason = x.A.Reason,
                                            Remarks = x.A.Remarks,
                                            Status = x.A.Status,
                                            ApplyDate = x.A.ApplyDate,
                                            ManagerId = x.A.ManagerId,
                                            ManagerName = x.B.FirstName + " " + x.B.LastName
                                        });

                var viewleavelist = Query
                                        .OrderByDescending(x => x.EmployeeLeaveID)
                                        .Skip(skip.Value)
                                        .Take(offset)
                                        .ToList();

                var totalRows = Query.Count();

                decimal pagerLength = decimal.Divide(Convert.ToDecimal(totalRows), Convert.ToDecimal(offset));

                HtmlTblVm<ViewLeaveVm> HtmlTblVm = new HtmlTblVm<ViewLeaveVm>();
                HtmlTblVm.TableData = viewleavelist;
                HtmlTblVm.TotalRows = totalRows;
                HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
                HtmlTblVm.CurrentPage = page.Value;
                return View(HtmlTblVm);
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
                        CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, empLeaveObj, leaveListCalc, false);
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
                        LeaveType = empLeaveObj.LeaveTypeId
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