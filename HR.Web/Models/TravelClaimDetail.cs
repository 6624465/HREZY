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
    
    public partial class TravelClaimDetail
    {
        public int TravelClaimDetailId { get; set; }
        public Nullable<int> TravelClaimId { get; set; }
        public string Category { get; set; }
        public Nullable<System.DateTime> TravelDate { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Receipts { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
