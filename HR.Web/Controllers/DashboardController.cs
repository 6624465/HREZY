using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{
    [SessionFilter]
    [ErrorHandler]
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            try
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
                                     .Where(x => x.Gender == 101 && x.BranchId == branch.BranchID).Count();
                                malecount.custom = malecount.y;
                                employeeDataVm.genderCount.Add(malecount);

                                GenderCount femalecount = new GenderCount();
                                femalecount.name = "Female";
                                femalecount.y = dbCntx.EmployeePersonalDetails
                                     .Where(x => x.Gender == 102 && x.BranchId == branch.BranchID).Count();
                                femalecount.custom = femalecount.y;
                                employeeDataVm.genderCount.Add(femalecount);

                                superadminGenderCount.Add(employeeDataVm);
                            }
                        }
                        ViewData["BranchId"] = BRANCHID;
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
                        ViewData["BranchId"] = BRANCHID;
                        return View("admindashboard", obj);
                    }
                }
                else if (ROLECODE == UTILITY.ROLE_EMPLOYEE)
                {
                    using (var dbCntx = new HrDataContext())
                    {
                        EmployeeDashBoardVm obj = new EmployeeDashBoardVm();
                        var empheader = dbCntx.EmployeeHeaders.Where(x => x.EmployeeId == EMPLOYEEID).FirstOrDefault();
                        var managername = dbCntx.EmployeeHeaders.Where(x => x.EmployeeId == empheader.ManagerId).FirstOrDefault();
                        if (empheader != null)
                        {
                            int managerId = empheader.ManagerId == null ? 0 : empheader.ManagerId.Value;
                            if (managerId == 0)
                            {
                                ViewData["message"] = "You Don't Have Reporting Manger";
                            }
                            else
                            {
                                ViewData["message"] = "Your Reporting manager is" + " " + managername.FirstName + " " + managername.LastName;
                            }
                        }
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
                                                    Days = x.Days.Value,
                                                    Reason = x.Reason
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
                        LeaveMaster lMaster = new LeaveMaster();
                        if (leaveStartTransactions != null)
                        {
                            var paidLeave = lMaster.ANNUALLEAVE(BRANCHID);
                            var casualLeave = lMaster.CASUALLEAVE(BRANCHID);
                            var sickLeave = lMaster.MEDICALLEAVE(BRANCHID);

                            List<OtherLeave> leavepolicy = dbCntx.OtherLeaves.Where(x => x.BranchId == BRANCHID && x.IsActive == true).ToList();
                            if (leavepolicy.Count != 0)
                            {

                                //var totalcasualLeaves = dbCntx.OtherLeaves.Where(x => x.LeaveTypeId == casualLeave && x.BranchId == BRANCHID && x.IsActive == true).FirstOrDefault().LeavesPerYear;
                                var totalpaidleaves = dbCntx.OtherLeaves.Where(x => x.LeaveTypeId == paidLeave && x.BranchId == BRANCHID && x.IsActive == true).FirstOrDefault().LeavesPerYear;
                                var totalsickLeaves = dbCntx.OtherLeaves.Where(x => x.LeaveTypeId == sickLeave && x.BranchId == BRANCHID && x.IsActive == true).FirstOrDefault().LeavesPerYear;


                                List<LeaveTran> leavetranscount = leaveStartTransactions.Where(x => x.EmployeeId == EMPLOYEEID).ToList();
                                if (leavetranscount.Count != 0)
                                {

                                    LeaveTran PreveLeaveTran = leaveStartTransactions.Where(x => x.LeaveType == paidLeave).OrderBy(x => x.TransactionId).FirstOrDefault();
                                    decimal totalPaidLeaves = 0;
                                    if (PreveLeaveTran != null)
                                        totalPaidLeaves = PreveLeaveTran.PreviousLeaves;

                                    var currentLeaveTrans = leaveStartTransactions.Where(x => x.LeaveType == paidLeave).OrderBy(x => x.TransactionId)
                                        .OrderByDescending(x => x.TransactionId).FirstOrDefault();
                                    decimal currentPaidLeaves = 0;
                                    if (currentLeaveTrans != null)
                                        currentPaidLeaves = currentLeaveTrans.CurrentLeaves;
                                    obj.remainingpls = currentPaidLeaves;



                                    if (totalPaidLeaves != 0 && currentPaidLeaves != 0)
                                        remainingPaidLeavesPercent = (currentPaidLeaves / totalPaidLeaves) * 100;

                                    //LeaveTran prevCasualLeaves = leaveStartTransactions.Where(x => x.LeaveType == casualLeave)
                                    //	.OrderBy(x => x.TransactionId).FirstOrDefault();

                                    //LeaveTran curCasualLeaves = leaveCurrentTransactions.Where(x => x.LeaveType == casualLeave).OrderBy(x => x.TransactionId)
                                    //	.OrderByDescending(x => x.TransactionId).FirstOrDefault();


                                    //decimal totalCasualLeaves = 0;

                                    //if (prevCasualLeaves != null)
                                    //	totalCasualLeaves = prevCasualLeaves.PreviousLeaves;

                                    //decimal currentCasualLeaves = 0;
                                    //if (curCasualLeaves != null)
                                    //	currentCasualLeaves = curCasualLeaves.CurrentLeaves;

                                    //obj.remainingcls = currentCasualLeaves;
                                    //if (totalCasualLeaves != 0 && currentCasualLeaves != 0)
                                    //	remainingCasualLeavesPercent = (currentCasualLeaves / totalCasualLeaves) * 100;

                                    DateTime now = DateTime.Now;
                                    var startDate = new DateTime(now.Year, now.Month, 1);
                                    var endDate = startDate.AddMonths(1).AddDays(-1);


                                    //var otherLeaveObj = dbCntx.OtherLeaves.Where(x => x.BranchId == BRANCHID && x.LeaveTypeId == sickLeave)
                                    //    .FirstOrDefault();

                                    //var SLPerMonth = otherLeaveObj != null ? (otherLeaveObj.LeavesPerMonth != null ? otherLeaveObj.LeavesPerMonth : 0) : 0;
                                    //var CurrentMonthSLs = query.Where(x => x.FromDate >= startDate && x.ToDate <= endDate && x.LeaveTypeId == sickLeave).ToList();
                                    // foreach (var item in CurrentMonthSLs)
                                    //{
                                    //    obj.totalSLs += item.Days.Value;
                                    //}
                                    //if (obj.totalSLs >= SLPerMonth)
                                    //{
                                    //    obj.totalSLs = obj.totalSLs;
                                    //}
                                    //else
                                    //{
                                    //    if (CurrentMonthSLs.Count() != 0)
                                    //    {
                                    //        obj.totalSLs = SLPerMonth.Value - obj.totalSLs;
                                    //    }
                                    //    else
                                    //    {
                                    //        obj.totalSLs = 0;
                                    //    }

                                    //}

                                    LeaveTran PrevesickLeaveTran = leaveStartTransactions.Where(x => x.LeaveType == sickLeave).OrderBy(x => x.TransactionId).FirstOrDefault();
                                    decimal totalsickleaves = 0;
                                    if (PrevesickLeaveTran != null)
                                        totalsickleaves = PrevesickLeaveTran.PreviousLeaves;

                                    var currentsickLeaveTrans = leaveStartTransactions.Where(x => x.LeaveType == sickLeave).OrderBy(x => x.TransactionId)
                                        .OrderByDescending(x => x.TransactionId).FirstOrDefault();
                                    decimal currentsickLeaves = 0;
                                    if (currentsickLeaveTrans != null)
                                        currentsickLeaves = currentsickLeaveTrans.CurrentLeaves;
                                    obj.remainingsls = currentsickLeaves;



                                    //if (totalsickleaves != 0 && currentsickLeaves != 0)
                                    //    remainingsickLeavesPercent = (currentsickLeaves / totalsickleaves) * 100;


                                    obj.empLeaveDashBoard = empLeaveDetails.ToList();
                                    obj.clPercent = remainingCasualLeavesPercent;
                                    obj.plPercent = remainingPaidLeavesPercent;
                                    //obj.totalCLs = totalCasualLeaves;
                                    obj.totalPLs = totalPaidLeaves;
                                    obj.totalSLs = totalsickleaves;
                                    obj.currentcls = obj.totalCLs - obj.remainingcls;
                                    obj.currentpls = obj.totalPLs - obj.remainingpls;
                                    obj.currentsls = obj.totalSLs - obj.remainingsls;
                                    obj.EmployeeId = EMPLOYEEID;
                                    obj.EmployeeName = empheader.FirstName + " " + empheader.LastName;
                                    ViewData["Alert"] = "";
                                }
                                else
                                {

                                    obj.empLeaveDashBoard = empLeaveDetails.ToList();
                                    //obj.totalCLs = totalcasualLeaves.Value;
                                    obj.totalPLs = totalpaidleaves.Value;
                                    obj.totalSLs = totalsickLeaves.Value;
                                    obj.currentcls = 0;
                                    obj.currentpls = 0;
                                    obj.currentsls = 0;
                                    obj.remainingpls = totalpaidleaves.Value;
                                    obj.remainingsls = totalsickLeaves.Value;
                                    //obj.remainingcls = totalcasualLeaves.Value;
                                    obj.EmployeeId = EMPLOYEEID;
                                    obj.EmployeeName = empheader.FirstName + " " + empheader.LastName;
                                    ViewData["Alert"] = "";
                                }

                            }
                            else
                            {
                                obj.empLeaveDashBoard = empLeaveDetails.ToList();
                                obj.clPercent = 0;
                                obj.plPercent = 0;
                                obj.totalCLs = 0;
                                obj.totalPLs = 0;
                                obj.totalSLs = 0;
                                obj.currentcls = 0;
                                obj.currentpls = 0;
                                obj.currentsls = 0;
                                obj.EmployeeId = EMPLOYEEID;
                                obj.EmployeeName = empheader.FirstName + " " + empheader.LastName;
                                ViewData["Alert"] = UTILITY.MISSINGLEAVEPOLICYALERT;

                            }





                        }
                        return View("employeedashboard", obj);
                    }

                }
            }
            catch (Exception ex)
            {
                //Session["ErrMsg"] = ex.Message + " " + 
                //                    ex.StackTrace + " " + 
                //                    ex.TargetSite + " " + 
                //                    ex.Source;
                throw;
            }


            return View();
        }

        public ActionResult LeaveTransaction(LeaveListVm leaveObj)
        {

            ViewData["ROLECODE"] = ROLECODE;
            using (var dbCntx = new HrDataContext())
            {
                var leaveList = dbCntx.EmployeeLeaveLists.ToList();

                if (leaveObj.Year == 0)
                    leaveObj.Year = Convert.ToInt16(DateTime.Now.Year);
                if (leaveObj.Month == 0)
                {
                    leaveObj.Month = 1;
                }

                DateTime _FromDate = new DateTime(Convert.ToInt32(leaveObj.Year), leaveObj.Month, 1);
                DateTime _toDate = new DateTime(Convert.ToInt32(leaveObj.Year), leaveObj.Month, DateTime.DaysInMonth(leaveObj.Year, leaveObj.Month));

                List<LeaveListVm> leaveListVm = new List<LeaveListVm>();

                var BranchId = leaveObj.BranchId == 0 ? BRANCHID : leaveObj.BranchId;
                LeaveListVm leave = new LeaveListVm();
                leave.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_FromDate.Month);
                var leavesByCountry = leaveList
                    .Where(x => x.BranchId == BranchId && x.CreatedOn >= _FromDate && x.CreatedOn <= _toDate).ToList();
                leave.Count = leavesByCountry.Sum(x => x.Days).Value;

                leaveListVm.Add(leave);


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

        public ActionResult DashboardofEmployeeData(int? branchid, string role)
        {
            branchid = branchid == 0 ? BRANCHID : branchid;
            int maleCount = 0;
            int femalCount = 0;
            List<usp_EmployeeDetail_Result> employeeDataReportList = new List<usp_EmployeeDetail_Result>();
            using (HrDataContext dbcontext = new HrDataContext())
            {
                employeeDataReportList = dbcontext.usp_EmployeeDetail(branchid, role).ToList();
                foreach (var item in employeeDataReportList)
                {
                    if (item.gender == 101)
                    {
                        maleCount++;
                    }
                    else
                    {
                        femalCount++;
                    }
                }
            }

            ViewData["BranchId"] = branchid;
            ViewData["RoleCode"] = role;
            ViewData["MaleCount"] = maleCount;
            ViewData["FeMaleCount"] = femalCount;
            return View(employeeDataReportList);
        }
        public ActionResult DashboardofSalaryComponents(int? BranchId, int? Year, byte? Month, int? EmployeeId)
        {
            BranchId = BranchId == 0 ? BRANCHID : BranchId;
            Month = Month == 0 ? null : Month;
            EmployeeId = EmployeeId == 0 ? null : EmployeeId;
            SalaryComponantReportVm vm = new SalaryComponantReportVm();
            vm.SalaryComponantReport = new List<USP_SALARYCOMPONENTREPORT_Result>();
            vm.SalaryComponantReportYTD = new List<USP_SALARYCOMPONENTREPORTYTD_Result>();
            using (var dbCntx = new HrDataContext())
            {
                vm.SalaryComponantReport = dbCntx.USP_SALARYCOMPONENTREPORT(BranchId, Year, Month, EmployeeId).ToList();
                vm.SalaryComponantReportYTD = dbCntx.USP_SALARYCOMPONENTREPORTYTD(BranchId, Year, EmployeeId).ToList();

            }
            vm.BranchID = BranchId;
            vm.Year = Year;
            vm.Month = Month;
            vm.EmployeeID = EmployeeId;
            ViewData["BranchId"] = BranchId;
            ViewData["RoleCode"] = ROLECODE.ToUpper();
            return View(vm);
        }
        public ActionResult DashboardofSalaryReport()
        {
            ViewData["BranchId"] = BRANCHID;
            return View();
        }
        public ActionResult DashboardofSalaryData(int? BranchId, int? Year, byte? Month, int? EmployeeId)
        {
            BranchId = BranchId == 0 ? BRANCHID : BranchId;
            Month = Month == 0 ? null : Month;
            EmployeeId = EmployeeId == 0 ? null : EmployeeId;
            ClaimsReportVm vm = new ClaimsReportVm();
            vm.TravelClaimReport = new List<USP_TRAVELCLAIMREPORT_Result>();
            vm.TravelClaimReportYTD = new List<USP_TRAVELCLAIMREPORTYTD_Result>();
            using (var dbCntx = new HrDataContext())
            {
                vm.TravelClaimReport = dbCntx.USP_TRAVELCLAIMREPORT(BranchId, Year, Month, EmployeeId).ToList();
                vm.TravelClaimReportYTD = dbCntx.USP_TRAVELCLAIMREPORTYTD(BranchId, Year, EmployeeId).ToList();

            }
            vm.BranchID = BranchId;
            vm.Year = Year;
            vm.Month = Month;
            vm.EmployeeID = EmployeeId;
            ViewData["BranchId"] = BRANCHID;
            ViewData["RoleCode"] = ROLECODE.ToUpper();
            return View(vm);
        }
        public ActionResult DashboardofLeaveReport()
        {
            ViewData["BranchId"] = BRANCHID;
            return View();
        }
    }

    //public class DashBoard {
    //public   regionWiseEmployees
    //}

    public class EmployeeDataReport
    {
        //public int EmployeeId { get; set; }
        //public string EmployeeNo { get; set; }
        //public string EmployeeName { get; set; }
        //public Nullable<System.DateTime> JoiningDate { get; set; }
        //public string JobTitle { get; set; }
        //public Nullable<short> gender { get; set; }
        //public string ContactNo { get; set; }
        //public string PersonalEmailId { get; set; }
        //public string OfficialEmailId { get; set; }
        //public Nullable<System.DateTime> DateOfBirth { get; set; }
        //public Nullable<int> DocumentDetailID { get; set; }
        //public string ProfilePic { get; set; }
        public Int32 EmployeeId { get; set; }
        public Int32 MaleCount { get; set; }
        public Int32 FemaleCount { get; set; }
        public Int32 EmployeeNo { get; set; }
        public Int64 ContactNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public string PersonalEmailId { get; set; }
    }
}