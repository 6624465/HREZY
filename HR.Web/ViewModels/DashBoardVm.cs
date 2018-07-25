using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Models;

namespace HR.Web.ViewModels
{
    public class DashBoardVm
    {
        public int EmployeeCount { get; set; }
        public IEnumerable<usp_EmployeeDateOfJoiningDate_Result> lineChartData { get; set; }
        public decimal totalCLs { get; set; }
        public decimal totalPLs { get; set; }
        public decimal totalSLs { get; set; }

        public EmployeeDataVm employeeDataVm { get; set; }
    }

    public class EmployeeDashBoardVm
    {
        public List<EmpLeaveDashBoard> empLeaveDashBoard { get; set; }

        public decimal clPercent { get; set; }
        public decimal plPercent { get; set; }

        public decimal totalCLs { get; set; }
        public decimal totalPLs { get; set; }
        public decimal totalSLs { get; set; }

        public decimal remainingcls { get; set; }
        public decimal? remainingpls { get; set; }
        public decimal? remainingsls { get; set; }

       public decimal currentcls { get; set; }
        public decimal? currentpls { get; set; }
        public decimal? currentsls { get; set; }

        public decimal SLsPerMonth { get; set; }
        public decimal remainingsl { get; set; }
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }
    }

    public class EmpLeaveDashBoard
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string LeaveTypeDesc { get; set; }
        public string Status { get; set; }
        public decimal Days { get; set; }
        public string Reason { get; set; }
    }


    public class EmployeeDataVm
    {
        public string branchName { get; set; }
        public List<GenderCount> genderCount { get; set; }
        public int TotalEmployeeCount { get; set; }
    }

    public class GenderCount
    {
        public string name { get; set; }

        public int y { get; set; }
        public int custom { get; set; }
    }

    public class SalaryComponantReportVm
    {
        //public List<USP_SALARYCOMPONENTREPORT_Result> SalaryComponantReport { get; set; }
    }
}