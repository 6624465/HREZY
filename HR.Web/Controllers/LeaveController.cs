using HR.Web.BusinessObjects;
using HR.Web.BusinessObjects.LeaveMaster;
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
        HolidayListBO holidayListBO = null;
        EmployeeLeaveListBO employeeLeaveListBO = null;
        LeaveTrasactionBO leaveTransactionBO = null;
        public LeaveController()
        {
            weekendPolicyBO = new WeekendPolicyBO(SESSIONOBJ);
            leaveHeaderBO = new LeaveHeaderBO(SESSIONOBJ);
            leaveBO = new LeaveBO(SESSIONOBJ);
            holidayListBO = new HolidayListBO(SESSIONOBJ);
            employeeLeaveListBO = new EmployeeLeaveListBO(SESSIONOBJ);
            leaveTransactionBO = new LeaveTrasactionBO(SESSIONOBJ);
        }

        // GET: Leave
        #region HolidayList
        public ActionResult HolidayList()
        {
            ViewData["RoleCode"] = ROLECODE;

            holidayVm holidayVm = new holidayVm()
            {
                calendarVM = holidayListBO.GetHolidayList(),
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
            List<HolidayList> holidayList = holidayListBO.GetListByProperty(x => x.CountryId == BRANCHID).ToList();
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
            return View(new EmployeeLeaveList
            {
                // FromDate = DateTime.Now,
                //  ToDate = DateTime.Now
            });
        }

        [HttpPost]
        public ActionResult SaveEmployeeLeaveForm(EmployeeLeaveList EmployeeLeaveList)
        {

            GetHolidayWeekends();
            WeekendPolicy weekendPolicy = weekendPolicyBO.GetById(BRANCHID);
            List<HolidayList> holidayList = holidayListBO.GetListByProperty(x => x.CountryId == BRANCHID).ToList();
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
                LeaveTransaction leavetransaction = dbCntx.LeaveTransactions
                        .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn)
                        .FirstOrDefault();
                if (leavetransaction == null)
                {
                    Leave leave = dbCntx.Leaves.Where(x => x.BranchId == BRANCHID).FirstOrDefault();

                    leavetransaction = new LeaveTransaction()
                    {
                        BranchId = BRANCHID,
                        CreatedBy = USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CurrentCasualLeaves = leave.CasualLeavesPerMonth.Value,
                        CurrentPaidLeaves = leave.PaidLeavesPerMonth.Value,
                        CurrentSickLeaves = leave.SickLeavesPerMonth.Value,
                        EmployeeId = EMPLOYEEID,
                        FromDt = UTILITY.SINGAPORETIME,
                        ToDt = UTILITY.SINGAPORETIME,
                        PreviousCasualLeaves = leave.CasualLeavesPerMonth.Value,
                        PreviousPaidLeaves = leave.PaidLeavesPerMonth.Value,
                        PreviousSickLeaves = leave.SickLeavesPerMonth.Value,
                        ModifiedBy = USERID,
                        ModifiedOn = UTILITY.SINGAPORETIME,
                    };

                    dbCntx.LeaveTransactions.Add(leavetransaction);
                    dbCntx.SaveChanges();
                }
                if (!isPreviousLeaveExists)
                {

                    if (EmployeeLeaveList.LeaveTypeId == 1030 && leavetransaction.CurrentCasualLeaves == 0)
                    {
                        ViewData["Message"] = "You do not have enough casual leaves,other leaves will be LOP";
                        ViewData["IsLop"] = true;
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                    else if (EmployeeLeaveList.LeaveTypeId == 1031 && leavetransaction.CurrentPaidLeaves == 0)
                    {

                        ViewData["Message"] = "You do not have enough paid leaves,other leaves will be LOP";
                        ViewData["IsLop"] = true;
                        return View("EmployeeRequestFrom", EmployeeLeaveList);
                    }
                    else if (EmployeeLeaveList.LeaveTypeId == 1049 && leavetransaction.CurrentSickLeaves == 0)
                    {
                        ViewData["Message"] = "You do not have enough paid leaves,other leaves will be LOP";
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
                    obj.Session = EmployeeLeaveList.Session;

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

                LeaveTransaction leavetransaction = dbCntx.LeaveTransactions
                            .Where(x => x.BranchId == BRANCHID && x.EmployeeId == EMPLOYEEID).OrderByDescending(x => x.CreatedOn)
                            .FirstOrDefault();
                obj.IsLossOfPay = true;
                obj.LossOfPayDays = EmployeeLeaveList.Days - leavetransaction.CurrentCasualLeaves;

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
            LeaveTransaction leavetransaction = leaveTransactionBO.GetByProperty(empLeaveObj.BranchId, empLeaveObj.EmployeeId);
            LeaveListCalc leaveListCalc = null;
            if (leavetransaction != null)
            {
                leaveListCalc = new LeaveListCalc(leavetransaction.CurrentCasualLeaves, leavetransaction.CurrentPaidLeaves, leavetransaction.CurrentSickLeaves,
leavetransaction.PreviousCasualLeaves, leavetransaction.PreviousPaidLeaves, leavetransaction.PreviousSickLeaves
);
                CalculateLeavesTransaction.CalculateLeaveFromTransaction(leavetransaction, empLeaveObj, leaveListCalc, false);
            }

            LeaveTransaction _leaveTransaction = new LeaveTransaction()
            {
                BranchId = BRANCHID,
                CreatedBy = USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                CurrentCasualLeaves = leaveListCalc.currentCasualLeaves,
                CurrentPaidLeaves = leaveListCalc.currentPaidLeaves,
                CurrentSickLeaves = leaveListCalc.currentSickLeaves,
                EmployeeId = grantLeaveVm.EmployeeId,
                FromDt = empLeaveObj.FromDate,
                ToDt = empLeaveObj.ToDate,
                PreviousCasualLeaves = leaveListCalc.previousCasualLeaves,
                PreviousPaidLeaves = leaveListCalc.previousPaidLeaves,
                PreviousSickLeaves = leaveListCalc.previousSickLeaves,
                LeaveType = empLeaveObj.LeaveTypeId
            };
            leaveTransactionBO.Add(_leaveTransaction);
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
                var leaves = dbContext.Leaves.Join(dbContext.Branches,
                    a => a.BranchId, b => b.BranchID,
                    (a, b) => new { A = a, B = b }).Select(x => new LeavepolicyVm
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
        }

        [HttpPost]
        public ActionResult GetLeaves(Leave leave)
        {
            ViewBag.RoleCode = ROLECODE;
            BranchLeaveVm leaveVm = new BranchLeaveVm();
            leaveVm.leave = new Leave();
            if (leave.BranchId == -1)
            {
                if (leaveVm.leave == null)
                    leaveVm.leave = new Leave();

                leaveVm.weekendPolicy = new WeekendPolicy
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
                };
            }
            else
            {
                leaveVm.leave = leaveBO.GetById(Convert.ToInt32(leave.BranchId));
                leaveVm.weekendPolicy = weekendPolicyBO.GetById(Convert.ToInt32(leave.BranchId));
                if (leaveVm.weekendPolicy == null)
                {
                    leaveVm.weekendPolicy = new WeekendPolicy
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
                    };
                }
                return View("Leave", leaveVm);
            }
            return View("Leave", leaveVm);
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [HttpGet]
        public ActionResult Leave(int leaveId = 0, int branchid = -1)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                ViewBag.RoleCode = ROLECODE;
                BranchLeaveVm leaveVm = new BranchLeaveVm();
                leaveVm.leave = new Models.Leave();
                if (ROLECODE != UTILITY.ROLE_SUPERADMIN && ROLECODE != UTILITY.ROLE_EMPLOYEE)
                {
                    leaveVm.leave = dbContext.Leaves
                                .leaveWhere(BRANCHID, ROLECODE)
                                .FirstOrDefault();
                    leaveVm.weekendPolicy = weekendPolicyBO.GetById(BRANCHID);
                    return View(leaveVm);
                }

                if (leaveId == 0 && branchid == -1)
                {
                    leaveVm.leave = new Models.Leave();
                    leaveVm.weekendPolicy = new WeekendPolicy
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
                    };
                    return View(leaveVm);
                }
                else
                {
                    leaveVm.leave = dbContext.Leaves.Where(x => x.BranchId == branchid)
                        .FirstOrDefault();
                    leaveVm.weekendPolicy = weekendPolicyBO.GetById(branchid);
                }
                                
                return View(leaveVm);
            }
        }
        [HttpPost]
        public ActionResult Leave(BranchLeaveVm vm)
        {
            Leave leave = vm.leave;
            WeekendPolicy wekendPolicy = vm.weekendPolicy;
            //Leave leave, WeekendPolicy wekendPolicy
            wekendPolicy.BranchId = Convert.ToInt32(leave.BranchId);
            if (leave.LeaveId == 0)
            {
                leaveBO.Add(leave);
                weekendPolicyBO.Add(wekendPolicy);
            }
            else
            {
                leaveBO.Add(leave);
                weekendPolicyBO.Add(wekendPolicy);
            }
            if (ROLECODE == UTILITY.ROLE_ADMIN)
            {
                return RedirectToAction("Leave", new { leaveId = leave.LeaveId, branchid = wekendPolicy.BranchId });
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
                var list = dbcntx.Branches
                    .Join(dbcntx.WeekendPolicies,
                    a => a.BranchID, b => b.BranchId,
                    (a, b) => new { A = a, B = b })
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
                    }).ToList();
                return View("WeekendPolicyList", list);
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
                var _weekendPolicy = weekendPolicyBO.GetById(branchId);
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
    }



}