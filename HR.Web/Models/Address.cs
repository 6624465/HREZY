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
    
    public partial class Address
    {
        public int AddressId { get; set; }
        public int LinkID { get; set; }
        public short SeqNo { get; set; }
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public int BranchId { get; set; }
    }
}
