using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class BranchLeaveVm
    {
        public Leave leave { get; set; }

        public int BranchId { get; set; }
        public WeekendPolicy weekendPolicy { get; set; }
        public List<OtherLeave> otherLeave { get; set; }
    }


    public partial class OtherLeaveVm
    {
        public int LeaveId { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public Nullable<decimal> LeavesPerYear { get; set; }
        public Nullable<decimal> LeavesPerMonth { get; set; }
        public bool IsCarryForward { get; set; }
        public Nullable<decimal> CarryForward { get; set; }
        public int BranchId { get; set; }
        public string LeaveDescription { get; set; }
    }
}