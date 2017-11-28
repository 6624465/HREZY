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
    
    public partial class LeaveTransaction
    {
        public int TransactionId { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime FromDt { get; set; }
        public System.DateTime ToDt { get; set; }
        public decimal PreviousPaidLeaves { get; set; }
        public decimal CurrentPaidLeaves { get; set; }
        public decimal PreviousSickLeaves { get; set; }
        public decimal CurrentSickLeaves { get; set; }
        public decimal PreviousCasualLeaves { get; set; }
        public decimal CurrentCasualLeaves { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> LeaveType { get; set; }
    }
}
