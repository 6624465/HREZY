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
    
    public partial class PayslipBatchDetail
    {
        public int BatchDetailId { get; set; }
        public Nullable<int> BatchHeaderId { get; set; }
        public string BatchNo { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string RegisterCode { get; set; }
        public string ContributionCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
