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
    }
}