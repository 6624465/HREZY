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
    
    public partial class EmployeeHeader
    {
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public Nullable<int> IDType { get; set; }
        public string IDNumber { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserEmailId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ManagerId { get; set; }
    }
}
