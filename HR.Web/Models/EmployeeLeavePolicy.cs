//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HR.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeLeavePolicy
    {
        public int BranchID { get; set; }
        public short LeaveYear { get; set; }
        public int EmployeeID { get; set; }
        public int LeaveTypeID { get; set; }
        public Nullable<decimal> LeavesPerYear { get; set; }
        public Nullable<decimal> CarryForwardLeaves { get; set; }
        public Nullable<decimal> TotalLeaves { get; set; }
        public Nullable<decimal> BalanceLeaves { get; set; }
    }
}