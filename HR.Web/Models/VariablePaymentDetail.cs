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
    
    public partial class VariablePaymentDetail
    {
        public int DetailID { get; set; }
        public Nullable<int> HeaderId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string ComponentCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
