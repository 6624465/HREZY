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
        public int StructureId { get; set; }
    }
}