using HR.Web.Models;
using System;
using System.Web;

namespace HR.Web.ViewModels
{
    public class CompanyVm
    {
        public CompanyVmObj company { get; set; }
        public Address companyAddress { get; set; }
    }

    public class CompanyVmObj
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string RegNo { get; set; }
        public string CompanyLogo { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? InCorporationDate { get; set; }
    }
}