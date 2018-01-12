using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                using (var dbCntx = new HrDataContext())
                {
                    List<Branch> branches = dbCntx.Branches.ToList();
                    List<EmployeeDataVm> superadminGenderCount = new List<EmployeeDataVm>();
                    foreach (Branch branch in branches)
                    {
                        var count = dbCntx.EmployeePersonalDetails
                               .Where(x => x.BranchId == branch.BranchID).Count();
                        if (count > 0)
                        {
                            EmployeeDataVm employeeDataVm = new EmployeeDataVm();

                            employeeDataVm.genderCount = new List<GenderCount>();
                            employeeDataVm.branchName = branch.BranchName;

                            GenderCount malecount = new GenderCount();
                            malecount.name = "Male";
                            malecount.y = dbCntx.EmployeePersonalDetails
                                 .Where(x => x.Gender == 1 && x.BranchId == branch.BranchID).Count();
                            malecount.custom = malecount.y;
                            employeeDataVm.genderCount.Add(malecount);

                            GenderCount femalecount = new GenderCount();
                            femalecount.name = "Female";
                            femalecount.y = dbCntx.EmployeePersonalDetails
                                 .Where(x => x.Gender == 0 && x.BranchId == branch.BranchID).Count();
                            femalecount.custom = femalecount.y;
                            employeeDataVm.genderCount.Add(femalecount);

                            superadminGenderCount.Add(employeeDataVm);
                        }
                    }

                    return View("index", superadminGenderCount);
                }
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

                    Branch branch = dbCntx.Branches.Where(x => x.BranchID == BRANCHID).FirstOrDefault();
                    //List<EmployeeDataVm> employeeDataVm = new List<EmployeeDataVm>();

                    obj.employeeDataVm = new EmployeeDataVm();
                    obj.employeeDataVm.genderCount = new List<GenderCount>();
                    obj.employeeDataVm.branchName = branch.BranchName;

                    GenderCount malecount = new GenderCount();
                    malecount.name = "Male";
                    malecount.y = dbCntx.EmployeePersonalDetails
                         .Where(x => x.Gender == 101 && x.BranchId == branch.BranchID).Count();
                    malecount.custom = malecount.y;
                    obj.employeeDataVm.genderCount.Add(malecount);

                    GenderCount femalecount = new GenderCount();
                    femalecount.name = "Female";
                    femalecount.y = dbCntx.EmployeePersonalDetails
                         .Where(x => x.Gender == 102 && x.BranchId == branch.BranchID).Count();
                    femalecount.custom = femalecount.y;
                    obj.employeeDataVm.genderCount.Add(femalecount);




                    //var query = dbCntx.EmployeeLeaveLists
                    //                    .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID);
                    //var leaveStartTransactions = dbCntx.LeaveTrans
                    //                          .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID && x.CreatedOn >= startDayOfYear)
                    //                          .OrderBy(x => x.TransactionId)
                    //                          .FirstOrDefault();
                    //if (leaveStartTransactions != null)
                    //{
                    //    obj.totalPLs = leaveStartTransactions.CurrentLeaves;
                    //    obj.totalCLs = leaveStartTransactions.CurrentLeaves;
                    //    DateTime now = DateTime.Now;
                    //    var startDate = new DateTime(now.Year, now.Month, 1);
                    //    var endDate = startDate.AddMonths(1).AddDays(-1);

                    //    var SLPerMonth = dbCntx.Leaves.Where(x => x.BranchId == BRANCHID).FirstOrDefault().SickLeavesPerMonth;
                    //    var CurrentMonthSLs = query.Where(x => x.FromDate >= startDate && x.ToDate <= endDate && x.LeaveTypeId == 1031).ToList();
                    //    foreach (var item in CurrentMonthSLs)
                    //    {
                    //        obj.totalSLs += item.Days.Value;
                    //    }
                    //    obj.totalSLs = SLPerMonth.Value - obj.totalSLs;
                    //}
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

                    List<LeaveTran> leaveCurrentTransactions = dbCntx.LeaveTrans
                                                .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID)
                                                .OrderByDescending(x => x.TransactionId)
                                                .ToList();


                    List<LeaveTran> leaveStartTransactions = dbCntx.LeaveTrans
                                                .Where(x => x.EmployeeId == EMPLOYEEID && x.BranchId == BRANCHID && x.CreatedOn >= startDayOfYear)
                                                .OrderBy(x => x.TransactionId)
                                                .ToList();

                    decimal remainingPaidLeavesPercent = 0.0M;
                    decimal remainingCasualLeavesPercent = 0.0M;
                    if (leaveStartTransactions != null)
                    {

                        LeaveTran PreveLeaveTran = leaveStartTransactions.Where(x => x.LeaveType == UTILITY.PAIDLEAVE).OrderBy(x => x.TransactionId).FirstOrDefault();
                        decimal totalPaidLeaves = 0;
                        if (PreveLeaveTran != null)
                            totalPaidLeaves = PreveLeaveTran.PreviousLeaves;

                        var currentLeaveTrans = leaveStartTransactions.Where(x => x.LeaveType == UTILITY.PAIDLEAVE).OrderBy(x => x.TransactionId)
                            .OrderByDescending(x => x.TransactionId).FirstOrDefault();
                        decimal currentPaidLeaves = 0;
                        if (currentLeaveTrans != null)
                            currentPaidLeaves = currentLeaveTrans.CurrentLeaves;
                        obj.totalPLs = currentPaidLeaves;


                        if (totalPaidLeaves != 0 && currentPaidLeaves != 0)
                            remainingPaidLeavesPercent = (currentPaidLeaves / totalPaidLeaves) * 100;

                        LeaveTran prevCasualLeaves = leaveStartTransactions.Where(x => x.LeaveType == UTILITY.CASUALLEAVE)
                            .OrderBy(x => x.TransactionId).FirstOrDefault();

                        LeaveTran curCasualLeaves = leaveCurrentTransactions.Where(x => x.LeaveType == UTILITY.CASUALLEAVE).OrderBy(x => x.TransactionId)
                            .OrderByDescending(x => x.TransactionId).FirstOrDefault();


                        decimal totalCasualLeaves = 0;

                        if (prevCasualLeaves != null)
                            totalCasualLeaves = prevCasualLeaves.PreviousLeaves;

                        decimal currentCasualLeaves = 0;
                        if (curCasualLeaves != null)
                            currentCasualLeaves = curCasualLeaves.CurrentLeaves;

                        obj.totalCLs = currentCasualLeaves;
                        if (totalCasualLeaves != 0 && currentCasualLeaves != 0)
                            remainingCasualLeavesPercent = (currentCasualLeaves / totalCasualLeaves) * 100;

                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var endDate = startDate.AddMonths(1).AddDays(-1);

                        var SLPerMonth = dbCntx.OtherLeaves.Where(x => x.BranchId == BRANCHID && x.LeaveTypeId == UTILITY.SICKLEAVE)
                            .FirstOrDefault().LeavesPerMonth;
                        var CurrentMonthSLs = query.Where(x => x.FromDate >= startDate && x.ToDate <= endDate && x.LeaveTypeId == UTILITY.SICKLEAVE).ToList();
                        foreach (var item in CurrentMonthSLs)
                        {
                            obj.totalSLs += item.Days.Value;
                        }
                        obj.totalSLs = SLPerMonth.Value - obj.totalSLs;
                        obj.empLeaveDashBoard = empLeaveDetails;
                        obj.clPercent = remainingCasualLeavesPercent;
                        obj.plPercent = remainingPaidLeavesPercent;
                    }
                    return View("employeedashboard", obj);
                }

            }

            return View();
        }

        public ActionResult LeaveTransaction(LeaveListVm leaveObj)
        {
            //int branchId = 0, int year = 0, int month = 1
            //leaveListVm.Year
            using (var dbCntx = new HrDataContext())
            {
                var leaveList = dbCntx.EmployeeLeaveLists.ToList();

                if (leaveObj.Year == 0)
                    leaveObj.Year = Convert.ToInt16(DateTime.Now.Year);
                if (leaveObj.Month == 0)
                {
                    leaveObj.Month = 1;
                }

                DateTime _FromDate = new DateTime(Convert.ToInt32(leaveObj.Year), 1, 1);
                DateTime _toDate = new DateTime(Convert.ToInt32(leaveObj.Year), leaveObj., DateTime.DaysInMonth(leaveObj.Year, leaveObj.));

                List<LeaveListVm> leaveListVm = new List<LeaveListVm>();
                if (ROLECODE == UTILITY.ROLE_ADMIN)
                {
                    LeaveListVm leave = new LeaveListVm();
                    leave.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_FromDate.Month);
                    var leavesByCountry = leaveList
                        .Where(x => x.BranchId == BRANCHID && x.CreatedOn >= _FromDate && x.CreatedOn <= _toDate).ToList();
                    leave.Count = leavesByCountry.Sum(x => x.Days).Value;

                    leaveListVm.Add(leave);

                }
                else if (ROLECODE == UTILITY.ROLE_ADMIN)
                {
                    LeaveListVm leave = new LeaveListVm();
                    leave.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_FromDate.Month);
                    var leavesByCountry = leaveList
                        .Where(x => x.CreatedOn >= _FromDate && x.CreatedOn <= _toDate).ToList();
                    leave.Count = leavesByCountry.Sum(x => x.Days).Value;

                    leaveListVm.Add(leave);
                }
                return View(leaveListVm);
            }
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