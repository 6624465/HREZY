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
    }

    public class EmployeeDashBoardVm
    {
        public IEnumerable<EmpLeaveDashBoard> empLeaveDashBoard { get; set; }

        public decimal clPercent { get; set; }
        public decimal plPercent { get; set; }

        public decimal totalCLs { get; set; }
        public decimal totalPLs { get; set; }
        public decimal totalSLs { get; set; }
    }

    public class EmpLeaveDashBoard
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string LeaveTypeDesc { get; set; }
        public string Status { get; set; }
        public decimal Days { get; set; }
    }
}