using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class LeaveAdminVM
    {
         
        public LeaveAdminVM()
        {

        }
        public int LeaveId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int? PaidLeavesPerYear { get; set; }
        public decimal? PaidLeavesPerMonth { get; set; }
        public bool? IsPaidLeaveCarryForward { get; set; }
        public string CarryForwardPerYear { get; set; }
        public int? SickLeavesPerYear { get; set; }
        public decimal? SickLeavesPerMonth { get; set; }
        public Nullable<bool> IsSickLeaveCarryForward { get; set; }
        public Nullable<decimal> CarryForwardSickLeaves { get; set; }
        public Nullable<int> CasualLeavesPerYear { get; set; }
        public Nullable<decimal> CasualLeavesPerMonth { get; set; }
        public Nullable<bool> IsCasualLeaveCarryForward { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string RolCode { get; set; }
    }
}