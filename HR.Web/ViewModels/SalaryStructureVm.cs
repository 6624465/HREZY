using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class SalaryStructureVm
    {
        public SalaryStructureHeader structureHeader { get; set; }
        public  List<SalaryStructureDetail> structureCompanyDeductionDetail { get; set; }

        public List<SalaryStructureDetail> structureEmployeeDeductionDetail { get; set; }

        public List<SalaryStructureDetail> structureEmployeeTaxDetail { get; set; }
    }
}