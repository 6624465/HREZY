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
    
    public partial class usp_SSFSummaryDetailByMonthTH_Result
    {
        public Nullable<int> EmployeeId { get; set; }
        public string IDNumber { get; set; }
        public Nullable<int> SalutationType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SocialWelfareNo { get; set; }
        public string EPFNO { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public Nullable<byte> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> TotalSalary { get; set; }
        public string RegisterCode { get; set; }
        public string ContributionCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
