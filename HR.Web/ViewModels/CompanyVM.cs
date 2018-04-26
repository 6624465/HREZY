using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace HR.Web.ViewModels
{
    public class CompanyVm
    {
        public CompanyVmObj company { get; set; }
        public AddressVm companyAddress { get; set; }

        public List<companyTreeVm> companyTreeVm { get; set; }
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
        public string SSFNo { get; set; }
        public string TaxIdNo { get; set; }
        public string BranchCode { get; set; }
    }


    public class companyTreeVm
    {
        public string text { get; set; }
        public string href { get; set; }
        public List<branchTreeVm> nodes { get; set; }
    }
    public class branchTreeVm
    {
        public string text { get; set; }
        public string href { get; set; }

    }
}