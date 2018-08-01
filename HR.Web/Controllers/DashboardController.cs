using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;
using System.Data.SqlClient;
using System.Data;

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

                        LeaveMaster lMaster = new LeaveMaster();

                        var paidLeave = lMaster.ANNUALLEAVE(BRANCHID);
                        var sickLeave = lMaster.MEDICALLEAVE(BRANCHID);

                        LeaveReportVm vm = new LeaveReportVm();

                        vm.consReport = LEAVEREPORTYTD(BRANCHID, DateTime.Now.Year, EMPLOYEEID);

                        List<OtherLeave> leavepolicy = dbCntx.OtherLeaves.Where(x => x.BranchId == BRANCHID && x.IsActive == true).ToList();
                        if (leavepolicy.Count != 0)
                        {
                            var annualLeaves = vm.consReport.Where(x => x.LeaveType.ToUpper() == "ANNUAL LEAVE");
                            var medicalLeaves = vm.consReport.Where(x => x.LeaveType.ToUpper() == "MEDICAL LEAVE");

                            var totalpaidleaves = annualLeaves.Sum(x=>x.TotalLeaves)+ (annualLeaves.FirstOrDefault() == null ? 0 : annualLeaves.FirstOrDefault().BalanceLeaves);
                            var totalsickLeaves = medicalLeaves.Sum(x => x.TotalLeaves) + (medicalLeaves.FirstOrDefault() == null ? 0 : medicalLeaves.FirstOrDefault().BalanceLeaves);


                            obj.empLeaveDashBoard = empLeaveDetails.ToList();
                            obj.totalPLs = totalpaidleaves;
                            obj.totalSLs = totalsickLeaves;
                            obj.currentpls = annualLeaves.Sum(x => x.TotalLeaves);
                            obj.currentsls = medicalLeaves.Sum(x => x.TotalLeaves);
                            obj.remainingpls = annualLeaves.FirstOrDefault() == null ?0: annualLeaves.FirstOrDefault().BalanceLeaves;
                            obj.remainingsls = medicalLeaves.FirstOrDefault()==null?0:medicalLeaves.FirstOrDefault().BalanceLeaves;
                            obj.EmployeeId = EMPLOYEEID;
                            obj.EmployeeName = empheader.FirstName + " " + empheader.LastName;
                            ViewData["Alert"] = "";

                        }
                        else
                        {
                            obj.empLeaveDashBoard = empLeaveDetails.ToList();
                            obj.totalPLs = 0;
                            obj.totalSLs = 0;
                            obj.currentpls = 0;
                            obj.currentsls = 0;
                            obj.EmployeeId = EMPLOYEEID;
                            obj.EmployeeName = empheader.FirstName + " " + empheader.LastName;
                            ViewData["Alert"] = UTILITY.MISSINGLEAVEPOLICYALERT;

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
            vm.SalaryComponantReport = new List<Salarycomponentreport>();
            vm.SalaryComponantReportYTD = new List<Salarycomponentreportytd>();

            using (var dbCntx = new HrDataContext())
            {
                List<Salarycomponentreportytd> salarycomponentreportytd = new List<Salarycomponentreportytd>();
                List<Salarycomponentreport> salarycomponentreport = new List<Salarycomponentreport>();
                if (BranchId == -1 || BranchId == null)
                {
                    var countryList = SelectListItemHelper.ActiveCountryList();
                    foreach (var item in countryList)
                    {
                        var list = dbCntx.USP_SALARYCOMPONENTREPORTYTD(Convert.ToInt32(item.Value), Year, EmployeeId).ToList();
                        if (list.Sum(x => x.TotalSalary) > 0)
                        {
                            foreach (var info in list)
                            {
                                Salarycomponentreportytd countryItem = new Salarycomponentreportytd();
                                countryItem.BranchID = Convert.ToInt32(item.Value);
                                countryItem.BranchName = item.Text;
                                countryItem.YTDMonth = info.YTDMonth;
                                countryItem.TotalSalary = info.TotalSalary;
                                salarycomponentreportytd.Add(countryItem);
                            }
                        }

                        var SalaryComponantReport = dbCntx.USP_SALARYCOMPONENTREPORT(Convert.ToInt32(item.Value), Year, Month, EmployeeId).ToList();
                        foreach (var info in SalaryComponantReport)
                        {
                            Salarycomponentreport countryItem = new Salarycomponentreport();
                            countryItem.BranchID = Convert.ToInt32(item.Value);
                            countryItem.BranchName = item.Text;
                            countryItem.ComponentName = info.ComponentName;
                            countryItem.Amount = info.Amount;
                            salarycomponentreport.Add(countryItem);
                        }
                    }
                    vm.SalaryComponantReportYTD = salarycomponentreportytd;
                    vm.SalaryComponantReport = salarycomponentreport.ToList();

                }
                else
                {
                    var SalaryComponantReport = dbCntx.USP_SALARYCOMPONENTREPORT(BranchId, Year, Month, EmployeeId).ToList();
                    foreach (var info in SalaryComponantReport)
                    {
                        Salarycomponentreport countryItem = new Salarycomponentreport();
                        countryItem.BranchID = BranchId.Value;
                        countryItem.ComponentName = info.ComponentName;
                        countryItem.Amount = info.Amount;
                        salarycomponentreport.Add(countryItem);
                    }
                    vm.SalaryComponantReport = salarycomponentreport.ToList();

                    var list = dbCntx.USP_SALARYCOMPONENTREPORTYTD(BranchId, Year, EmployeeId).ToList();
                    foreach (var info in list)
                    {
                        Salarycomponentreportytd countryItem = new Salarycomponentreportytd();
                        countryItem.BranchID = BranchId.Value;
                        countryItem.YTDMonth = info.YTDMonth;
                        countryItem.TotalSalary = info.TotalSalary;
                        salarycomponentreportytd.Add(countryItem);
                    }
                    vm.SalaryComponantReportYTD = salarycomponentreportytd.ToList();

                    vm.dt = SALARYCOMPONENTEMPLOYEEYTD(BranchId, Year, Convert.ToInt32(Month), EmployeeId);
                }
            }
            vm.BranchID = BranchId == null ? -1 : BranchId;
            vm.Year = Year;
            vm.Month = Month;
            vm.EmployeeID = EmployeeId;
            ViewData["RoleCode"] = ROLECODE.ToUpper();

            if (vm.dt != null && vm.dt.Columns.Count > 0)
            {
                DataRow totalsRow = vm.dt.NewRow();
                totalsRow["EMPLOYEE NAME"] = "TOTAL";
                for (int j = 1; j < vm.dt.Columns.Count; j++)
                {
                    DataColumn col = vm.dt.Columns[j];

                    decimal colTotal = 0;
                    for (int i = 0; i < col.Table.Rows.Count; i++)
                    {
                        DataRow row = col.Table.Rows[i];
                        if (row[col] == null || row[col].ToString() == "")
                        {
                            row[col] = "0.00";
                        }
                        colTotal += Convert.ToDecimal(row[col]);
                    }
                    //col.Table.Rows[j]. = Color.Red;
                    totalsRow[col.ColumnName] = colTotal;
                }

                vm.dt.Rows.Add(totalsRow);
            }

            return View(vm);
        }
        public ActionResult DashboardofSalaryReport(int? BranchId, int? Year, byte? Month, int? EmployeeId)
        {
            BranchId = BranchId == 0 ? BRANCHID : BranchId;
            Month = Month == 0 ? null : Month;
            EmployeeId = EmployeeId == 0 ? null : EmployeeId;
            SalaryReportVm vm = new SalaryReportVm();
            using (var dbcntx = new HrDataContext())
            {
                vm.dt = SALARYCOMPONENTEMPLOYEEYTD(BranchId, Year, Convert.ToInt32(Month), EmployeeId);
            }
            vm.BranchID = BranchId;
            vm.Year = Year;
            vm.Month = Month;
            vm.EmployeeID = EmployeeId;
            ViewData["BranchId"] = BranchId;
            ViewData["RoleCode"] = ROLECODE.ToUpper();

            if (vm.dt != null && vm.dt.Columns.Count > 0)
            {
                DataRow totalsRow = vm.dt.NewRow();
                totalsRow["EMPLOYEE NAME"] = "TOTAL";
                for (int j = 1; j < vm.dt.Columns.Count; j++)
                {
                    DataColumn col = vm.dt.Columns[j];

                    decimal colTotal = 0;
                    for (int i = 0; i < col.Table.Rows.Count; i++)
                    {
                        DataRow row = col.Table.Rows[i];
                        if (row[col] == null || row[col].ToString() == "")
                        {
                            row[col] = "0.00";
                        }
                        colTotal += Convert.ToDecimal(row[col]);
                    }
                    totalsRow[col.ColumnName] = colTotal;
                }

                vm.dt.Rows.Add(totalsRow);
            }

            return View(vm);
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
                vm.dt = TRAVELCLAIMEMPLOYEEYTD(BranchId, Year, Month, EmployeeId);

            }
            vm.BranchID = BranchId;
            vm.Year = Year;
            vm.Month = Month;
            vm.EmployeeID = EmployeeId;
            ViewData["RoleCode"] = ROLECODE.ToUpper();

            if (vm.dt != null && vm.dt.Columns.Count > 0)
            {
                DataRow totalsRow = vm.dt.NewRow();
                totalsRow["EMPLOYEE NAME"] = "TOTAL";
                for (int j = 1; j < vm.dt.Columns.Count; j++)
                {
                    DataColumn col = vm.dt.Columns[j];

                    decimal colTotal = 0;
                    for (int i = 0; i < col.Table.Rows.Count; i++)
                    {
                        DataRow row = col.Table.Rows[i];
                        if (row[col] == null || row[col].ToString() == "")
                        {
                            row[col] = "0.00";
                        }
                        colTotal += Convert.ToDecimal(row[col]);
                    }
                    //col.Table.Rows[j]. = Color.Red;
                    totalsRow[col.ColumnName] = colTotal;
                    if (col.ColumnName == "TAXILOCAL")
                    {
                        vm.dt.Columns[j].ColumnName = "TAXI LOCAL";
                    }
                    if (col.ColumnName == "TAXIOVERSEAS")
                    {
                        vm.dt.Columns[j].ColumnName = "TAXI OVERSEAS";
                    }
                    if (col.ColumnName == "FOODBILLSLOCAL")
                    {
                        vm.dt.Columns[j].ColumnName = "FOOD BILLS LOCAL";
                    }
                    if (col.ColumnName == "FOODBILLSOVERSEAS")
                    {
                        vm.dt.Columns[j].ColumnName = "FOOD BILLS OVERSEAS";
                    }
                    if (col.ColumnName == "OTHEREXPENSES")
                    {
                        vm.dt.Columns[j].ColumnName = "OTHER EXPENSES";
                    }
                }

                vm.dt.Rows.Add(totalsRow);
            }

            return View(vm);
        }
        public System.Data.DataTable TRAVELCLAIMEMPLOYEEYTD(Int32? BranchId, int? Year, int? Month, int? EmployeeId)
        {
            using (var dbCntx = new HrDataContext())
            using (SqlConnection Con = new
                SqlConnection(dbCntx.Database.Connection.ConnectionString))
            {
                Con.Open();
                SqlCommand Cmd = new SqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "[Reports].[USP_TRAVELCLAIMEMPLOYEEYTD]";

                Cmd.Parameters.Add("@BranchId", System.Data.SqlDbType.SmallInt);
                Cmd.Parameters.Add("@Year", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@Month", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@EmployeeID", System.Data.SqlDbType.Int);

                Cmd.Parameters["@BranchId"].Value = BranchId;
                Cmd.Parameters["@Year"].Value = Year;
                Cmd.Parameters["@Month"].Value = Month;
                Cmd.Parameters["@EmployeeID"].Value = EmployeeId;

                System.Data.DataTable dt = new System.Data.DataTable();
                var da = new SqlDataAdapter(Cmd);
                da.Fill(dt);
                Con.Close();
                return dt;
            }

        }
        public ActionResult DashboardofLeaveReport(int? BranchId, int? Year, int? EmployeeId)
        {
            BranchId = BranchId == 0 ? BRANCHID : BranchId;
            EmployeeId = EmployeeId == 0 ? null : EmployeeId;
            LeaveReportVm vm = new LeaveReportVm();

            using (var dbCntx = new HrDataContext())
            {
                vm.dtAvailed = EMPLOYEELEAVESUMMARYYTD(BranchId, Year, 0);
                vm.dtBalance = EMPLOYEELEAVESUMMARYYTD(BranchId, Year, 1);
                vm.consReport = LEAVEREPORTYTD(BranchId, Year, EmployeeId);
            }
            vm.BranchID = BranchId;
            vm.Year = Year;
            vm.EmployeeID = EmployeeId;
            ViewData["RoleCode"] = ROLECODE.ToUpper();


            return View(vm);
        }
        public System.Data.DataTable SALARYCOMPONENTEMPLOYEEYTD(Int32? BranchId, int? Year, int? Month, int? EmployeeId)
        {
            using (var dbCntx = new HrDataContext())
            using (SqlConnection Con = new
                SqlConnection(dbCntx.Database.Connection.ConnectionString))
            {
                Con.Open();
                SqlCommand Cmd = new SqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "[Reports].[USP_SALARYCOMPONENTEMPLOYEEYTD]";

                Cmd.Parameters.Add("@BranchId", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@Year", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@Month", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@EmployeeID", System.Data.SqlDbType.Int);

                Cmd.Parameters["@BranchId"].Value = BranchId;
                Cmd.Parameters["@Year"].Value = Year;
                Cmd.Parameters["@Month"].Value = Month;
                Cmd.Parameters["@EmployeeID"].Value = EmployeeId;


                System.Data.DataTable dt = new System.Data.DataTable();
                var da = new SqlDataAdapter(Cmd);

                da.Fill(dt);

                Con.Close();

                return dt;
            }

        }
        public System.Data.DataTable EMPLOYEELEAVESUMMARYYTD(Int32? BranchId, int? Year, int? IsBalance)
        {
            using (var dbCntx = new HrDataContext())
            using (SqlConnection Con = new
                SqlConnection(dbCntx.Database.Connection.ConnectionString))
            {
                Con.Open();
                SqlCommand Cmd = new SqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "[Operation].[usp_EMPLOYEELEAVESUMMARYYTD]";

                Cmd.Parameters.Add("@BranchID", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@LeaveYear", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@IsBalance", System.Data.SqlDbType.Int);

                Cmd.Parameters["@BranchID"].Value = BranchId;
                Cmd.Parameters["@LeaveYear"].Value = Year;
                Cmd.Parameters["@IsBalance"].Value = IsBalance;


                System.Data.DataTable dt = new System.Data.DataTable();
                var da = new SqlDataAdapter(Cmd);

                da.Fill(dt);

                Con.Close();

                return dt;
            }

        }
        public List<ConsReport> LEAVEREPORTYTD(Int32? BranchId, int? Year, int? EmployeeId)
        {
            using (var dbCntx = new HrDataContext())
            using (SqlConnection Con = new
                SqlConnection(dbCntx.Database.Connection.ConnectionString))
            {
                Con.Open();
                SqlCommand Cmd = new SqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "[Reports].[USP_LEAVEREPORTYTD]";

                Cmd.Parameters.Add("@BranchID", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@Year", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int);

                Cmd.Parameters["@BranchID"].Value = BranchId;
                Cmd.Parameters["@Year"].Value = Year;
                Cmd.Parameters["@EmployeeId"].Value = EmployeeId;


                System.Data.DataTable dt = new System.Data.DataTable();
                var da = new SqlDataAdapter(Cmd);

                da.Fill(dt);

                Con.Close();

                List<ConsReport> consReport = (from DataRow row in dt.Rows
                                               select new ConsReport
                                               {
                                                   YTDMonth = row["YTDMonth"].ToString(),
                                                   TotalLeaves = Convert.ToDecimal(row["TotalLeaves"]),
                                                   LeaveType = row["LeaveType"].ToString(),
                                                   BalanceLeaves = Convert.ToDecimal(row["BalanceLeaves"])

                                               }).ToList();

                return consReport;
            }

        }
    }

    //public class DashBoard {
    //public   regionWiseEmployees
    //}

}