using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class SalaryStructureHeaderTblVm
    {
        public int StructureID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TotalGross { get; set; }
        public Nullable<decimal> TotalDeductions { get; set; }
        public Nullable<int> BranchId { get; set; }
       
        public string BranchName { get; set; }
    }
}