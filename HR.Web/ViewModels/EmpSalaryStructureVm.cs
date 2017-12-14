using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.ViewModels
{
    public class EmpSalaryStructureVm
    {
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }

        public EmployeeSalaryStructure employeeSalaryStructure { get; set; }
    }


    public class EmployeeSalaryStructure
    {
        public EmpSalaryStructureHeader empSalaryStructureHeader { get; set; }
        public IEnumerable<EmpSalaryStructureDetail> empSalaryStructureDetail { get; set; }
    }

    public class SalaryStructure
    {
        public SalaryStructureHeader salaryStructureHeader { get; set; }
        public IEnumerable<SalaryStructureDetail> salaryStructureDetail { get; set; }
    }
}