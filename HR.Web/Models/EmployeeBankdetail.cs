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
    
    public partial class EmployeeBankdetail
    {
        public int BankDetailId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public string BankBranchCode { get; set; }
        public string SwiftCode { get; set; }
    }
}
