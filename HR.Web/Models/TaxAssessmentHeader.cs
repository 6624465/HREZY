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
    
    public partial class TaxAssessmentHeader
    {
        public int HeaderID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public string AssessmentNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> SocialContributionRate { get; set; }
        public Nullable<decimal> MaximumAmount { get; set; }
        public Nullable<bool> Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
