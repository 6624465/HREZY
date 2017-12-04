using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            DateTime startDayOfYear = new DateTime(UTILITY.SINGAPORETIME.Year, 01, 01);
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
            {
                return View("index");
            }
            else if (ROLECODE == UTILITY.ROLE_ADMIN)
            {
                using (var dbCntx = new HrDataContext())
                {
                    DashBoardVm obj = new DashBoardVm();
                    /*
                    obj.EmployeeCount = dbCntx.EmployeeHeaders
                                        .Where(x => x.BranchId == BRANCHID)
                                        .Count();*/
                    obj.EmployeeCount = dbCntx.usp_EmployeeDetail(BRANCHID, ROLECODE).Count();

                    obj.lineChartData = dbCntx.usp_EmployeeDateOfJoiningDate(UTILITY.SINGAPORETIME, BRANCHID)
                                            .ToList()
                                            .AsEnumerable();

                    var query = dbCntx.EmployeeLeaveLists
                                        .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID);
                    var leaveStartTransactions = dbCntx.LeaveTransactions
                                              .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID && x.CreatedOn >= startDayOfYear)
                                              .OrderBy(x => x.TransactionId)
                                              .FirstOrDefault();
                    if (leaveStartTransactions != null)
                    {
                        obj.totalPLs = leaveStartTransactions.CurrentPaidLeaves;
                        obj.totalCLs = leaveStartTransactions.CurrentCasualLeaves;
                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var endDate = startDate.AddMonths(1).AddDays(-1);

                        var SLPerMonth = dbCntx.Leaves.Where(x => x.BranchId == BRANCHID).FirstOrDefault().SickLeavesPerMonth;
                        var CurrentMonthSLs = query.Where(x => x.FromDate >= startDate && x.ToDate <= endDate && x.LeaveTypeId == 1031).ToList();
                        foreach (var item in CurrentMonthSLs)
                        {
                            obj.totalSLs += item.Days.Value;
                        }
                        obj.totalSLs = SLPerMonth.Value - obj.totalSLs;
                    }
                    return View("admindashboard", obj);
                }
            }
            else if (ROLECODE == UTILITY.ROLE_EMPLOYEE)
            {
                using (var dbCntx = new HrDataContext())
                {
                    EmployeeDashBoardVm obj = new EmployeeDashBoardVm();
                    var query = dbCntx.EmployeeLeaveLists
                                            .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID);
                    var empLeaveDetails = query.OrderByDescending(x => x.EmployeeLeaveID)
                                            .ThenByDescending(x => x.ApplyDate)
                                            .Take(5)
                                            .Select(x => new EmpLeaveDashBoard
                                            {
                                                FromDate = x.FromDate,
                                                ToDate = x.ToDate,
                                                ApplyDate = x.ApplyDate,
                                                LeaveTypeDesc = dbCntx.LookUps.Where(y => y.LookUpID == x.LeaveTypeId).FirstOrDefault().LookUpDescription,
                                                Status = x.Status,
                                                Days = x.Days.Value
                                            })
                                            .ToList()
                                            .AsEnumerable();

                    var leaveCurrentTransactions = dbCntx.LeaveTransactions
                                                .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID)
                                                .OrderByDescending(x => x.TransactionId)
                                                .FirstOrDefault();


                    var leaveStartTransactions = dbCntx.LeaveTransactions
                                                .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID && x.CreatedOn >= startDayOfYear)
                                                .OrderBy(x => x.TransactionId)
                                                .FirstOrDefault();

                    decimal remainingPaidLeavesPercent = 0.0M;
                    decimal remainingCasualLeavesPercent = 0.0M;
                    if (leaveStartTransactions != null)
                    {

                        var totalPaidLeaves = leaveStartTransactions.PreviousPaidLeaves;
                        var currentPaidLeaves = leaveCurrentTransactions.CurrentPaidLeaves;
                        obj.totalPLs = currentPaidLeaves;
                        if (totalPaidLeaves != 0 && currentPaidLeaves != 0)
                            remainingPaidLeavesPercent = (currentPaidLeaves / totalPaidLeaves) * 100;

                        var totalCasualLeaves = leaveStartTransactions.PreviousCasualLeaves;
                        var currentCasualLeaves = leaveCurrentTransactions.CurrentCasualLeaves;
                        obj.totalCLs = currentCasualLeaves;
                        if (totalCasualLeaves != 0 && currentCasualLeaves != 0)
                            remainingCasualLeavesPercent = (currentCasualLeaves / totalCasualLeaves) * 100;
                    }
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var SLPerMonth = dbCntx.Leaves.Where(x => x.BranchId == BRANCHID).FirstOrDefault().SickLeavesPerMonth;
                    var CurrentMonthSLs = query.Where(x => x.FromDate >= startDate && x.ToDate <= endDate && x.LeaveTypeId == 1031).ToList();
                    foreach (var item in CurrentMonthSLs)
                    {
                        obj.totalSLs += item.Days.Value;
                    }
                    obj.totalSLs = SLPerMonth.Value - obj.totalSLs;
                    obj.empLeaveDashBoard = empLeaveDetails;
                    obj.clPercent = remainingCasualLeavesPercent;
                    obj.plPercent = remainingPaidLeavesPercent;

                    return View("employeedashboard", obj);
                }

            }

            return View();
        }

        public JsonResult GetData()
        {
            //JsonResult result = null;

            using (HrDataContext dbContext = new HrDataContext())
            {
                var list = dbContext.EmployeeHeaders
                         .Join(dbContext.EmployeePersonalDetails,
                         a => a.EmployeeId, b => b.EmployeeId,
                         (a, b) => new { A = a, B = b })
                         .Join(dbContext.EmployeeWorkDetails,
                         c => c.A.EmployeeId, d => d.EmployeeId,
                         (c, d) => new { C = c, D = d })
                         .Join(dbContext.EmployeeAddresses,
                         e => e.C.A.EmployeeId, f => f.EmployeeId,
                         (e, f) => new { E = e, F = f })
                         .Join(dbContext.Branches, x => x.E.C.A.BranchId, y => y.BranchID,
                         (x, y) => new { X = x, Y = y })
                         .ToList().AsEnumerable();
                var regionWiseEmployees = list.GroupBy(g => g.Y.BranchName).ToList()
                 .Select(x => new
                 {
                     y = x.Count(),
                     name = x.Key
                 }).OrderByDescending(o => o.y);

                var genderWiseEmployees = list.GroupBy(g => g.X.E.C.B.Gender).ToList()
                                      .Select(n => new
                                      {
                                          y = n.Count(),
                                          name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                                      });

                var indiawiseGenders = list.Where(s => s.Y.BranchName == "INDIA")
                    .GroupBy(g => g.X.E.C.B.Gender).ToList()
                     .Select(n => new
                     {
                         y = n.Count(),
                         name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                     }).OrderByDescending(n => n.name);


                var singaporewiseGenders = list.Where(s => s.Y.BranchName == "SINGAPORE")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var hongkongwiseGenders = list.Where(s => s.Y.BranchName == "HONGKONG")
                         .GroupBy(x => x.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var mayanmarwiseGenders = list.Where(s => s.Y.BranchName == "MAYANMAR")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var pakistanwiseGenders = list.Where(s => s.Y.BranchName == "PAKISTAN")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var srilankawiseGenders = list.Where(s => s.Y.BranchName == "SRILANKA")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var cambodiawiseGenders = list.Where(s => s.Y.BranchName == "CAMBODIA")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var bangladeshwiseGenders = list.Where(s => s.Y.BranchName == "BANGLADESH")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);


                return Json(
                    new
                    {
                        sucess = true,
                        regionWiseEmployees = regionWiseEmployees,
                        genderWiseEmployees = genderWiseEmployees,
                        indiawiseGenders = indiawiseGenders,
                        bangladeshwiseGenders = bangladeshwiseGenders,
                        cambodiawiseGenders = cambodiawiseGenders,
                        srilankawiseGenders = srilankawiseGenders,
                        pakistanwiseGenders = pakistanwiseGenders,
                        mayanmarwiseGenders = mayanmarwiseGenders,
                        hongkongwiseGenders = hongkongwiseGenders,
                        singaporewiseGenders = singaporewiseGenders
                    }, JsonRequestBehavior.AllowGet
                );
            }
        }
    }

    //public class DashBoard {
    //public   regionWiseEmployees
    //}
}