using HR.Web.BusinessObjects;
using HR.Web.BusinessObjects.LeaveMaster;
using HR.Web.BusinessObjects.LookUpMaster;
using HR.Web.BusinessObjects.Operation;
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
        WeekendPolicyBO weekendPolicyBO = null;
        LeaveHeaderBO leaveHeaderBO = null;
        LeaveBO leaveBO = null;
        OtherLeaveBO otherLeaveBO = null;
        HolidayListBO holidayListBO = null;
        EmployeeLeaveListBO employeeLeaveListBO = null;
        LeaveTrasactionBO leaveTransactionBO = null;
        LookUpBO lookUpBo = null;
        LeaveTransBO leaveTransBO = null;
        EmployeeHeaderBO empHeaderBO = null;
        public LeaveController()
        {
            weekendPolicyBO = new WeekendPolicyBO(SESSIONOBJ);
            leaveHeaderBO = new LeaveHeaderBO(SESSIONOBJ);
            leaveBO = new LeaveBO(SESSIONOBJ);
            holidayListBO = new HolidayListBO(SESSIONOBJ);
            employeeLeaveListBO = new EmployeeLeaveListBO(SESSIONOBJ);
            leaveTransactionBO = new LeaveTrasactionBO(SESSIONOBJ);
            otherLeaveBO = new OtherLeaveBO(SESSIONOBJ);
            lookUpBo = new LookUpBO(SESSIONOBJ);
            leaveTransBO = new LeaveTransBO(SESSIONOBJ);
            empHeaderBO = new EmployeeHeaderBO(SESSIONOBJ);

        }

        // GET: Leave
        #region HolidayList
        public ActionResult HolidayList()
       {
            ViewData["RoleCode"] = ROLECODE;

            if (ROLECODE == UTILITY.ROLE_ADMIN)
            {
                holidayVm holidayVm = new holidayVm()
                {
                    calendarVM = holidayListBO.GetHolidayListByBranch(BRANCHID),
                    HolidayList = new HolidayList()
                }; return View("HolidayList", holidayVm);
            }
            else
            {
                holidayVm holidayVm = new holidayVm()
                {
                    calendarVM = holidayListBO.GetHolidayListByBranch(BRANCHID),
                    //holidayListBO.GetListByProperty(x => x.CountryId == BRANCHID).ToList()
                    HolidayList = new HolidayList()
                }; return View("HolidayList", holidayVm);
            }
            
        }

        public ActionResult HolidayListByBranch(int branchID=0)
        {
            ViewData["RoleCode"] = ROLECODE;

            holidayVm holidayVm = new holidayVm()
            {
                calendarVM = holidayListBO.GetHolidayListByBranch(branchID),
                HolidayList = new HolidayList()
            };
            return View("HolidayList", holidayVm);
        }


        public ActionResult AddHoliday(int HolidayId)
        {
            ViewData["RoleCode"] = ROLECODE;
            return View("HolidayList", holidayListBO.SaveHolidayList(HolidayId));


        }

        public PartialViewResult GetHolidayList(int holidayId)
        {
            ViewData["RoleCode"] = ROLECODE;
            if (holidayId != -1)
            {
                using (var dbntx = new HrDataContext())
                {
                    var holidaylist = holidayListBO.GetById(holidayId);
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
            holidaylist.BranchID = holidaylist.CountryId;
            holidayListBO.Add(holidaylist);


            return RedirectToAction("HolidayList");

        }

        #endregion
        public ViewResult AppliedLeaveList()
        {
            string viewName = string.Empty;
            using (var dbCntx = new HrDataContext())
            {
                IQueryable<Branch> branchList = null;
                if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
                {
                    branchList = dbCntx.Branches;
                }
                else if (ROLECODE == UTILITY.ROLE_ADMIN)
                {
                    branchList = dbCntx.Branches.Where(x => x.BranchID == BRANCHID);
                }

                var empLeaveList = branchList.GroupJoin(dbCntx.EmployeeLeaveLists.
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
                                    EmployeeFullName = z.FirstName + " " + z.LastName,

                                }).FirstOrDefault().EmployeeFullName,
                                EmployeeId = y.EmployeeId,
                                FromDate = y.FromDate,
                                ToDate = y.ToDate,
                                Status = y.Status
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

        public void GetHolidayWeekends()
        {
            List<int> weekEnd = new List<int>();

            WeekendPolicy weekendPolicy = weekendPolicyBO.GetById(BRANCHID);
            List<HolidayList> holidayList = holidayListBO.GetListByProperty(x => x.BranchID == BRANCHID).ToList();
            string[] holidaysList = new string[holidayList.Count];
            for (int i = 0; i < holidayList.Count; i++)
            {
                DateTime date = holidayList[i].Date;
                holidaysList[i] = new DateTime(date.Year, date.Month, date.Day).ToString("MM/dd/yyyy");
            }
            ViewBag.HoliDayList = holidaysList;
            if (weekendPolicy != null)
            {
                if ((!weekendPolicy.Monday.Value && !weekendPolicy.IsMondayHalfDay.Value) || !weekendPolicy.Monday.Value || weekendPolicy.IsMondayHalfDay.Value)
                {
                    weekEnd.Add(1);
                }
                if ((!weekendPolicy.Tuesday.Value && !weekendPolicy.IsTuesdayHalfDay.Value) || !weekendPolicy.Tuesday.Value || weekendPolicy.IsTuesdayHalfDay.Value)
                {
                    weekEnd.Add(2);
                }
                if ((!weekendPolicy.Wednesday.Value && !weekendPolicy.IsWednesdayHalfDay.Value) || !weekendPolicy.Wednesday.Value || weekendPolicy.IsWednesdayHalfDay.Value)
                {
                    weekEnd.Add(3);
                }
                if ((!weekendPolicy.Thursday.Value && !weekendPolicy.IsThursdayHalfDay.Value) || !weekendPolicy.Thursday.Value || weekendPolicy.IsThursdayHalfDay.Value)
                {
                    weekEnd.Add(4);
                }
                if ((!weekendPolicy.Friday.Value && !weekendPolicy.IsFridayHalfDay.Value) || !weekendPolicy.Friday.Value && weekendPolicy.IsFridayHalfDay.Value)
                {
                    weekEnd.Add(5);
                }
                if ((!weekendPolicy.Saturday.Value && !weekendPolicy.IsSaturdayHalfDay.Value) || !weekendPolicy.Saturday.Value || weekendPolicy.IsSaturdayHalfDay.Value)
                {
                    weekEnd.Add(6);
                }
                if ((!weekendPolicy.Sunday.Value && !weekendPolicy.IsSundayHalfDay.Value) || !weekendPolicy.Sunday.Value || weekendPolicy.IsSundayHalfDay.Value)
                {
                    weekEnd.Add(7);
                }
                ViewBag.weekend = weekEnd;
            }

        }

        public ActionResult EmployeeRequestFrom()
        {
            GetHolidayWeekends();

            var empHeader = empHeaderBO.GetByProperty(x => x.EmployeeId == EMPLOYEEID);
            if (empHeader != null)
            {
                int managerId = empHeader.ManagerId == null ? 0 : empHeader.ManagerId.Value;
                if (managerId == 0)
                {
                    return View("Error");
                }
            }

            return View(new EmployeeLeaveList
            {
            });
        }

        [HttpPost]
        public ActionResult SaveEmployeeLeaveForm(EmployeeLeaveList EmployeeLeaveList)
        {

            GetHolidayWeekends();
            WeekendPolicy weekendPolicy = weekendPolicyBO.GetById(BRANCHID);
            List<HolidayList> holidayList = holidayListBO.GetListByProperty(x => x.BranchID == BRANCHID).ToList();
            EmployeeLeaveList.Days = (decimal)CalculateLeavesTransaction
                .GetBusinessDays(EmployeeLeaveList.FromDate, EmployeeLeaveList.ToDate, weekendPolicy, holidayList);
            bool ishalfday = false;
            if (Request.Form["isChecked"] != null && Request.Form["isChecked"] != "")
            {
                ishalfday = Request.Form["isChecked"] == "on";
                if (ishalfday)
                {
                    EmployeeLeaveList.Days = 0.5m;

                }
                else
                    EmployeeLeaveList.Session = "";
            }
            using (var dbCntx = new HrDataContext())
            {
                EmployeeLeaveList obj = new EmployeeLeaveList();

                var isPreviousLeaveExists = dbCntx.EmployeeLeaveLists
                                            .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID && x.Status != UTILITY.LEAVECANCELLED)
                                            .Between(EmployeeLeaveList.FromDate, EmployeeLeaveList.ToDate)
                                            .Count() > 0;

                if (isPreviousLeaveExists)
                {
                    ViewData["Message"] = "You have already applied a leave within this date range. Please check.";
                    ViewData["IsLop"] = false;
                    return View("EmployeeRequestFrom", EmployeeLeaveList);
                }
                //LeaveTransaction leavetransaction = dbCntx.LeaveTransactions
                //        .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn)
                //        .FirstOrDefault();

                LeaveTran leavetransaction = dbCntx.LeaveTrans
                      .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID && x.LeaveType == EmployeeLeaveList.LeaveTypeId)
                      .OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if (leavetransaction == null)
                {
                    List<OtherLeave> leaveList = dbCntx.OtherLeaves.Where(x => x.BranchId == BRANCHID).ToList();

                    foreach (OtherLeave leave in leaveList)
                    {
                        leavetransaction = new LeaveTran()
                        {
                            BranchId = BRANCHID,
                            CreatedBy = USERID,
                            CreatedOn = UTILITY.SINGAPORETIME,
                            CurrentLeaves = leave.LeavesPerYear == null ? 0 : leave.LeavesPerYear.Value,
                            PreviousLeaves = leave.LeavesPerYear == null ? 0 : leave.LeavesPerYear.Value,
                            EmployeeId = EMPLOYEEID,
                            FromDt = UTILITY.SINGAPORETIME,
                            ToDt = UTILITY.SINGAPORETIME,
                            ModifiedBy = USERID,
                            ModifiedOn = UTILITY.SINGAPORETIME,
                            LeaveType = leave.LeaveTypeId
                        };

                        dbCntx.LeaveTrans.Add(leavetransaction);
                        dbCntx.SaveChanges();
                    }
                }
                if (!isPreviousLeaveExists)
                {

                    if (EmployeeLeaveList.LeaveTypeId == UTILITY.CASUALLEAVE && leavetransaction.CurrentLeaves == 0)
                    {
                        ViewData["Message"] = "You do not have enough casual leaves or applied leave,other leaves will be LOP";
                        ViewData["IsLop"] = true;
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                    else if (EmployeeLeaveList.LeaveTypeId == UTILITY.SICKLEAVE && leavetransaction.CurrentLeaves == 0)
                    {

                        ViewData["Message"] = "You do not have enough paid leaves or applied leave,other leaves will be LOP";
                        ViewData["IsLop"] = true;
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                    else if (EmployeeLeaveList.LeaveTypeId == UTILITY.PAIDLEAVE && leavetransaction.CurrentLeaves == 0)
                    {
                        ViewData["Message"] = "You do not have enough paid leaves or applied leave,other leaves will be LOP";
                        ViewData["IsLop"] = true;
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                }
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

                if (!isPreviousLeaveExists)
                {
                    if (eligibleLeaves >= EmployeeLeaveList.Days)
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
                        obj.ModifiedBy = USERID;
                        obj.ModifiedOn = UTILITY.SINGAPORETIME;
                        obj.ApplyDate = UTILITY.SINGAPORETIME;
                        //obj.ManagerId = 0;
                        obj.ManagerId = dbCntx.EmployeeHeaders.Where(x => x.EmployeeId == EMPLOYEEID)
                                            .FirstOrDefault() == null ? 0 : dbCntx.EmployeeHeaders
                                            .Where(x => x.EmployeeId == EMPLOYEEID).FirstOrDefault()
                                            .ManagerId.Value;

                        obj.Status = UTILITY.LEAVEPENDING;
                        obj.Session = EmployeeLeaveList.Session;

                        dbCntx.EmployeeLeaveLists.Add(obj);


                        LeaveListCalc leaveListCalc = null;
                        if (leavetransaction != null)
                        {
                            leaveListCalc = new LeaveListCalc(leavetransaction.CurrentLeaves,
                                                                leavetransaction.PreviousLeaves
                             );
                            CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, EmployeeLeaveList, leaveListCalc, true);
                        }
                        else
                        {
                            OtherLeave leave = dbCntx.OtherLeaves
                                 .Where(x => x.BranchId == BRANCHID && x.LeaveTypeId == EmployeeLeaveList.LeaveTypeId).FirstOrDefault();
                            leaveListCalc = new LeaveListCalc(leave.LeavesPerYear.Value,
                                                                leave.LeavesPerYear.Value
                                                              );
                            CalculateLeavesTransaction.CalculateLeave(leave, EmployeeLeaveList, leaveListCalc);


                        }
                        LeaveTran leaveTransaction = new LeaveTran()
                        {
                            BranchId = BRANCHID,
                            CreatedBy = USERID,
                            CreatedOn = UTILITY.SINGAPORETIME,
                            CurrentLeaves = leaveListCalc.currentLeaves,
                            PreviousLeaves = leaveListCalc.previousLeaves,

                            EmployeeId = EMPLOYEEID,
                            FromDt = obj.FromDate,
                            ToDt = obj.ToDate,
                            LeaveType = EmployeeLeaveList.LeaveTypeId,
                            ModifiedBy = USERID,
                            ModifiedOn = UTILITY.SINGAPORETIME
                        };
                        dbCntx.LeaveTrans.Add(leaveTransaction);

                        dbCntx.SaveChanges();

                        return RedirectToAction("ViewLeavesList");
                    }
                    else
                    {
                        ViewData["IsLop"] = true;
                        ViewData["Message"] = "You are not eligible for applied number of leaves or applied leave,other leaves will be LOP";
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                }
                else
                {
                    if (isPreviousLeaveExists)
                        ViewData["Message"] = "You have already applied a leave within this date range. Please check.";
                    if (eligibleLeaves >= EmployeeLeaveList.Days)
                        ViewData["Message"] = "You are not eligible for applied number of leaves";

                    return View("EmployeeRequestFrom", EmployeeLeaveList);
                }
                // return View("EmployeeRequestFrom", EmployeeLeaveList);
            }
        }


        [HttpPost]
        public ActionResult SaveLOP(EmployeeLeaveList EmployeeLeaveList)
        {
            using (var dbCntx = new HrDataContext())
            {
                var val = Request["isLOP"];

                EmployeeLeaveList obj = new EmployeeLeaveList();

                var isPreviousLeaveExists = dbCntx.EmployeeLeaveLists
                                            .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID)
                                            .Between(EmployeeLeaveList.FromDate, EmployeeLeaveList.ToDate)
                                            .Count() > 0;

                LeaveTran leavetransaction = dbCntx.LeaveTrans
                            .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID
                            && x.LeaveType == EmployeeLeaveList.LeaveTypeId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                LeaveCalculator leaveCal = new LeaveCalculator();
                decimal eligibleLeaves = leaveCal.GetLeavesCount(BRANCHID, EMPLOYEEID, EmployeeLeaveList.LeaveTypeId.Value,
                                            EmployeeLeaveList.FromDate);

                obj.IsLossOfPay = true;
                // if (eligibleLeaves > 0)
                obj.LossOfPayDays = EmployeeLeaveList.Days - eligibleLeaves;
                //else
                //    obj.LossOfPayDays = EmployeeLeaveList.Days - leavetransaction.CurrentLeaves;
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
                                    .FirstOrDefault() == null ? 0 : dbCntx.EmployeeHeaders
                                    .Where(x => x.EmployeeId == EMPLOYEEID)
                                    .FirstOrDefault()
                                    .ManagerId.Value;
                obj.Status = UTILITY.LEAVEPENDING;

                dbCntx.EmployeeLeaveLists.Add(obj);

                LeaveListCalc leaveListCalc = null;
                if (leavetransaction != null)
                {
                    leaveListCalc = new LeaveListCalc(leavetransaction.CurrentLeaves,
                                    leavetransaction.PreviousLeaves);
                    CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, EmployeeLeaveList, leaveListCalc, true);
                }
                else
                {
                    OtherLeave leave = dbCntx.OtherLeaves
                         .Where(x => x.BranchId == BRANCHID && x.LeaveTypeId == EmployeeLeaveList.LeaveTypeId).FirstOrDefault();
                    leaveListCalc = new LeaveListCalc(leave.LeavesPerYear.Value,
                                                        leave.LeavesPerYear.Value);
                    CalculateLeavesTransaction.CalculateLeave(leave, EmployeeLeaveList, leaveListCalc);


                }
                LeaveTran leaveTransaction = new LeaveTran()
                {
                    BranchId = BRANCHID,
                    CreatedBy = USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    CurrentLeaves = leaveListCalc.currentLeaves,
                    PreviousLeaves = leaveListCalc.currentLeaves,
                    EmployeeId = EMPLOYEEID,
                    FromDt = obj.FromDate,
                    ToDt = obj.ToDate,
                    LeaveType = EmployeeLeaveList.LeaveTypeId,
                    ModifiedBy = USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME
                };
                dbCntx.LeaveTrans.Add(leaveTransaction);

                dbCntx.SaveChanges();

                return RedirectToAction("ViewLeavesList");
            }
        }

        [HttpGet]
        public ActionResult GrantLeaveFormList(int? page = 1)
        {

            using (var dbcntx = new HrDataContext())
            {
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
                var skip = (page - 1) * offset;

                var grantleaveform = dbcntx.EmployeeHeaders.
                    Join(dbcntx.EmployeeLeaveLists,
                    a => a.EmployeeId, b => b.EmployeeId,
                    (a, b) => new { A = a, B = b })
                    .Where(x => x.B.ManagerId == EMPLOYEEID && x.B.Status != UTILITY.LEAVECANCELLED)
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

                var count = grantleaveform.Count();
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
            employeeLeaveListBO.ApproveLeave(grantLeaveVm);
            return RedirectToAction("AppliedGrantLeaveStatus", new { EmployeeLeaveID = grantLeaveVm.EmployeeLeaveID });
        }

        [HttpPost]
        public ActionResult RejectLeave(GrantLeaveListVm grantLeaveVm)
        {
            try
            {
                var empLeaveObj = employeeLeaveListBO.RejectLeave(grantLeaveVm);
                Save(grantLeaveVm, empLeaveObj);
                return RedirectToAction("AppliedGrantLeaveStatus", new { EmployeeLeaveID = grantLeaveVm.EmployeeLeaveID });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Save(GrantLeaveListVm grantLeaveVm, EmployeeLeaveList empLeaveObj)
        {
            LeaveTran leavetransaction = leaveTransBO.GetByProperty(x => x.BranchId == empLeaveObj.BranchId &&
            x.EmployeeId == empLeaveObj.EmployeeId && x.LeaveType == empLeaveObj.LeaveTypeId);
            LeaveListCalc leaveListCalc = null;
            if (leavetransaction != null)
            {
                leaveListCalc = new LeaveListCalc(leavetransaction.CurrentLeaves, leavetransaction.PreviousLeaves);
                CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, empLeaveObj, leaveListCalc, false);
            }

            LeaveTran _leaveTransaction = new LeaveTran()
            {
                BranchId = BRANCHID,
                CreatedBy = USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                CurrentLeaves = leaveListCalc.currentLeaves,
                PreviousLeaves = leaveListCalc.previousLeaves,
                EmployeeId = grantLeaveVm.EmployeeId,
                FromDt = empLeaveObj.FromDate,
                ToDt = empLeaveObj.ToDate,
                LeaveType = empLeaveObj.LeaveTypeId
            };
            leaveTransBO.Add(_leaveTransaction);
        }

        [HttpPost]
        public ActionResult SaveGrantLeave(LeaveVm lvm)
        {
            try
            {
                leaveHeaderBO.SaveLeave(lvm);
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
                //var leaves = dbContext.Leaves.Join(dbContext.Branches,
                //    a => a.BranchId, b => b.BranchID,
                //    (a, b) => new { A = a, B = b }).Select(x => new LeavepolicyVm
                //    {
                //        LeaveId = x.A.LeaveId,
                //        PaidLeavesPerYear = x.A.PaidLeavesPerYear,
                //        PaidLeavesPerMonth = x.A.PaidLeavesPerMonth,
                //        IsPaidLeaveCarryForward = x.A.IsPaidLeaveCarryForward,
                //        CarryFarwardPerYear = x.A.CarryForwardPerYear,
                //        SickLeavesPerYear = x.A.SickLeavesPerYear,
                //        SickLeavesPerMonth = x.A.SickLeavesPerMonth,
                //        IsSickLeaveCarryFarward = x.A.IsSickLeaveCarryForward,
                //        CarryFarwardSickLeaves = x.A.CarryForwardSickLeaves,
                //        CasualLeavesPerYear = x.A.CasualLeavesPerYear,
                //        CasualLeavesPerMonth = x.A.CasualLeavesPerMonth,
                //        IsCasualLeaveCarryFarward = x.A.IsCasualLeaveCarryForward,
                //        BranchId = x.A.BranchId,
                //        BranchName = x.B.BranchName
                //    }).ToList();
                List<Branch> Branches = dbContext.Branches.ToList();
                List<LeavepolicyVm> leave = new List<LeavepolicyVm>();
                foreach (Branch item in Branches)
                {
                    LeavepolicyVm policy = new LeavepolicyVm()
                    {
                        BranchId = item.BranchID,
                        BranchName = item.BranchName,
                    };
                    leave.Add(policy);
                }
                return View(leave);
            }
        }

        [HttpPost]
        public ActionResult GetLeaves(Leave leave)
        {
            ViewBag.RoleCode = ROLECODE;
            BranchLeaveVm leaveVm = new BranchLeaveVm();
            leaveVm.BranchId = Convert.ToInt32(leave.BranchId.Value);


            List<LookUp> lookUpList = lookUpBo.GetListByProperty(x => x.LookUpCategory == UTILITY.LOOKUPCATEGORY && x.IsActive == true).ToList();

            List<int> lookupIdList = otherLeaveBO.GetByAll().Select(x => x.LeaveTypeId.Value).ToList();


            leaveVm.otherLeave = otherLeaveBO.GetListById(Convert.ToInt32(leave.BranchId));

            if (leaveVm.otherLeave.Count == 0)
            {
                foreach (LookUp lookUp in lookUpList)
                {
                    OtherLeave otherleaves = new OtherLeave()
                    {
                        LeaveTypeId = lookUp.LookUpID,
                        Description = lookUp.LookUpDescription,
                        IsCarryForward = lookUp.IsCarryForward,
                        //BranchId = BRANCHID
                    };
                    leaveVm.otherLeave.Add(otherleaves);
                }
            }
            else
            {

                lookUpList = lookUpList.Where(x => !lookupIdList.Contains(x.LookUpID)).ToList();
                foreach (LookUp lookUp in lookUpList)
                {

                    OtherLeave otherleaves = new OtherLeave()
                    {
                        LeaveTypeId = lookUp.LookUpID,
                        Description = lookUp.LookUpDescription,
                        IsCarryForward = lookUp.IsCarryForward,
                        BranchId = BRANCHID
                    };
                    leaveVm.otherLeave.Add(otherleaves);
                }
            }



            return View("Leave", leaveVm);
        }



        [HttpGet]
        public ActionResult Leave(int leaveId = 0, int branchid = -1)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                ViewBag.RoleCode = ROLECODE;
                BranchLeaveVm leaveVm = new BranchLeaveVm();

                leaveVm.otherLeave = new List<OtherLeave>();

                List<LookUp> lookUpList = dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.LOOKUPCATEGORY && x.IsActive == true).ToList();

                List<int> lookupIdList = dbContext.OtherLeaves
                           .Select(x => x.LeaveTypeId.Value).ToList();

                if (ROLECODE != UTILITY.ROLE_SUPERADMIN && ROLECODE != UTILITY.ROLE_EMPLOYEE)
                {


                    leaveVm.otherLeave = dbContext.OtherLeaves.Where(x => x.BranchId == BRANCHID).ToList();
                    if (leaveVm.otherLeave.Count == 0)
                    {
                        foreach (LookUp lookUp in lookUpList)
                        {
                            OtherLeave otherleaves = new OtherLeave()
                            {
                                LeaveTypeId = lookUp.LookUpID,
                                Description = lookUp.LookUpDescription,
                                IsCarryForward = lookUp.IsCarryForward,
                                BranchId = BRANCHID
                            };
                            leaveVm.otherLeave.Add(otherleaves);
                        }
                    }
                    else
                    {

                        lookUpList = lookUpList.Where(x => !lookupIdList.Contains(x.LookUpID)).ToList();
                        foreach (LookUp lookUp in lookUpList)
                        {

                            OtherLeave otherleaves = new OtherLeave()
                            {
                                LeaveTypeId = lookUp.LookUpID,
                                Description = lookUp.LookUpDescription,
                                IsCarryForward = lookUp.IsCarryForward,
                                BranchId = BRANCHID
                            };
                            leaveVm.otherLeave.Add(otherleaves);
                        }
                    }
                    return View(leaveVm);
                }
                else
                {

                    if (leaveId == 0 && branchid == -1)
                    {
                        foreach (LookUp lookUp in lookUpList)
                        {
                            OtherLeave otherleaves = new OtherLeave()
                            {
                                LeaveTypeId = lookUp.LookUpID,
                                Description = lookUp.LookUpDescription,
                                IsCarryForward = lookUp.IsCarryForward,
                                BranchId = BRANCHID
                            };
                            leaveVm.otherLeave.Add(otherleaves);
                        }

                    }
                    else
                    {
                        leaveVm.otherLeave = dbContext.OtherLeaves.Where(x => x.BranchId == branchid).ToList();

                        if (leaveVm.otherLeave.Count == 0)
                        {
                            foreach (LookUp lookUp in lookUpList)
                            {
                                OtherLeave otherleaves = new OtherLeave()
                                {
                                    LeaveTypeId = lookUp.LookUpID,
                                    Description = lookUp.LookUpDescription,
                                    IsCarryForward = lookUp.IsCarryForward,
                                    BranchId = BRANCHID
                                };
                                leaveVm.otherLeave.Add(otherleaves);
                            }
                        }
                        else
                        {

                            lookUpList = lookUpList.Where(x => !lookupIdList.Contains(x.LookUpID)).ToList();
                            foreach (LookUp lookUp in lookUpList)
                            {

                                OtherLeave otherleaves = new OtherLeave()
                                {
                                    LeaveTypeId = lookUp.LookUpID,
                                    Description = lookUp.LookUpDescription,
                                    IsCarryForward = lookUp.IsCarryForward,
                                    BranchId = BRANCHID
                                };
                                leaveVm.otherLeave.Add(otherleaves);
                            }
                        }
                    }
                    return View(leaveVm);
                }

            }
        }
        [HttpPost]
        public ActionResult Leave(BranchLeaveVm vm)
        {
            List<OtherLeave> leaveList = new List<OtherLeave>();
            if (ROLECODE == UTILITY.ROLE_ADMIN)
            {
                vm.BranchId = BRANCHID;
            }
            foreach (OtherLeave leaveVm in vm.otherLeave)
            {
                OtherLeave OtherLeave = new OtherLeave()
                {
                    LeaveId = leaveVm.LeaveId,
                    CarryForward = leaveVm.CarryForward,
                    IsCarryForward = leaveVm.IsCarryForward,
                    LeaveTypeId = leaveVm.LeaveTypeId,
                    LeavesPerMonth = leaveVm.LeavesPerMonth,
                    LeavesPerYear = leaveVm.LeavesPerYear,
                    BranchId = vm.BranchId,
                    Description = leaveVm.Description
                };
                otherLeaveBO.Add(OtherLeave);
            }


            if (ROLECODE == UTILITY.ROLE_ADMIN)
            {
                return RedirectToAction("Leave", new { leaveId = 0, branchid = vm.BranchId });
            }
            return RedirectToAction("LeaveList");
            // return RedirectToAction("Leave", new { leaveId = leave.LeaveId, branchid = wekendPolicy.BranchId });
        }

        public ActionResult ViewLeavesList(int? page = 1)
        {

            using (var dbcntx = new HrDataContext())
            {
                ViewData["RoleCode"] = ROLECODE;
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
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

            employeeLeaveListBO.CancelLeave(employeeLeaveID, remarks);
            return RedirectToAction("ViewLeavesList");
        }

        [HttpGet]
        public ViewResult WeekendPolicyList()
        {
            //var list = weekendPolicy.GetAll();
            using (var dbcntx = new HrDataContext())
            {
                ViewBag.ROLECODE = ROLECODE;
                if (BRANCHID != -1)
                {
                    var list = dbcntx.Branches
                        .Join(dbcntx.WeekendPolicies,
                        a => a.BranchID, b => b.BranchId,
                        (a, b) => new { A = a, B = b }).Where(x => x.A.BranchID == BRANCHID)
                        .Select(x => new WeekendPolicyVm
                        {
                            BranchId = x.B.BranchId,
                            BranchName = x.A.BranchName,
                            Monday = x.B.Monday,
                            Tuesday = x.B.Tuesday,
                            Wednesday = x.B.Wednesday,
                            Thursday = x.B.Thursday,
                            Friday = x.B.Friday,
                            Saturday = x.B.Saturday,
                            Sunday = x.B.Sunday,
                            IsMondayHalfDay = x.B.IsMondayHalfDay,
                            IsTuesdayHalfDay = x.B.IsTuesdayHalfDay,
                            IsWednesdayHalfDay = x.B.IsWednesdayHalfDay,
                            IsThursdayHalfDay = x.B.IsThursdayHalfDay,
                            IsFridayHalfDay = x.B.IsFridayHalfDay,
                            IsSaturdayHalfDay = x.B.IsSaturdayHalfDay,
                            IsSundayHalfDay = x.B.IsSundayHalfDay
                        }).ToList(); return View("WeekendPolicyList", list);
                }
                else
                {
                    var list = dbcntx.Branches
                        .Join(dbcntx.WeekendPolicies,
                        a => a.BranchID, b => b.BranchId,
                        (a, b) => new { A = a, B = b }).Where(x => x.A.BranchID == BRANCHID)
                        .Select(x => new WeekendPolicyVm
                        {
                            BranchId = x.B.BranchId,
                            BranchName = x.A.BranchName,
                            Monday = x.B.Monday,
                            Tuesday = x.B.Tuesday,
                            Wednesday = x.B.Wednesday,
                            Thursday = x.B.Thursday,
                            Friday = x.B.Friday,
                            Saturday = x.B.Saturday,
                            Sunday = x.B.Sunday,
                            IsMondayHalfDay = x.B.IsMondayHalfDay,
                            IsTuesdayHalfDay = x.B.IsTuesdayHalfDay,
                            IsWednesdayHalfDay = x.B.IsWednesdayHalfDay,
                            IsThursdayHalfDay = x.B.IsThursdayHalfDay,
                            IsFridayHalfDay = x.B.IsFridayHalfDay,
                            IsSaturdayHalfDay = x.B.IsSaturdayHalfDay,
                            IsSundayHalfDay = x.B.IsSundayHalfDay
                        }).ToList(); return View("WeekendPolicyList", list);
                }

            }

        }

        [HttpGet]
        public ViewResult WeekEndPolicy(int branchId)
        {
            if (branchId == -1)
            {
                return View(new WeekendPolicy
                {
                    Monday = false,
                    Tuesday = false,
                    Wednesday = false,
                    Thursday = false,
                    Friday = false,
                    Saturday = false,
                    Sunday = false,
                    IsMondayHalfDay = false,
                    IsTuesdayHalfDay = false,
                    IsWednesdayHalfDay = false,
                    IsThursdayHalfDay = false,
                    IsFridayHalfDay = false,
                    IsSaturdayHalfDay = false,
                    IsSundayHalfDay = false
                });
            }
            else
            {
                branchId = BRANCHID;
                var _weekendPolicy = weekendPolicyBO.GetById(branchId);
                if (_weekendPolicy == null)
                {
                    _weekendPolicy = new WeekendPolicy
                    {
                        Monday = false,
                        Tuesday = false,
                        Wednesday = false,
                        Thursday = false,
                        Friday = false,
                        Saturday = false,
                        Sunday = false,
                        IsMondayHalfDay = false,
                        IsTuesdayHalfDay = false,
                        IsWednesdayHalfDay = false,
                        IsThursdayHalfDay = false,
                        IsFridayHalfDay = false,
                        IsSaturdayHalfDay = false,
                        IsSundayHalfDay = false,
                        BranchId = BRANCHID
                    };
                }
                return View(_weekendPolicy);
            }

        }

        [HttpPost]
        public ViewResult SaveWeekendPolicy(WeekendPolicy weekendpolicy)
        {
            if (weekendpolicy.BranchId == -1)
            {

                weekendPolicyBO.Add(weekendpolicy);

                return WeekendPolicyList();
            }
            else
            {
                weekendpolicy.BranchId = BRANCHID;
                weekendPolicyBO.Update(weekendpolicy);
                return WeekendPolicyList();
            }


        }
        public ActionResult GetBranchPolicy(WeekendPolicy weekendpolicy)
        {
            WeekendPolicy WeekEndPolicy = new WeekendPolicy();
            using (var dbcntx = new HrDataContext())
            {
                WeekEndPolicy = dbcntx.WeekendPolicies.Where(x => x.BranchId == weekendpolicy.BranchId).FirstOrDefault();
                return View("WeekEndPolicy", WeekEndPolicy ?? weekendpolicy);
            }

        }
        public ActionResult DeleteHoliday(int holidayid)
        {
            holidayListBO.Delete(holidayid);
            return RedirectToAction("HolidayList");

        }
    }



}