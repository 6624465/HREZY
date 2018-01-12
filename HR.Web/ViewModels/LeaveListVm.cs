using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class LeaveListVm
    {
        public decimal? Count { get; set; }
        public string MonthName { get; set; }
        public Int16 Year { get; set; }

        public short Month { get; set; }

        public short BranchId { get; set; }

        public List<EmployeeCountVm> empDataVm { get; set; }
    }

    public class EmployeeCountVm
    {
        public string BranchName { get; set; }
        public List<int> count { get; set; }
    }
}