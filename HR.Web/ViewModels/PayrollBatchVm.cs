using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class PayrollBatchVm
    {
        public PayslipBatchHeader payslipBatchHeader { get; set; }
        public List<PayslipBatchDetail> payslipBatchDetail { get; set; }

        public System.Data.DataTable dt { get; set; }
    }

    public class UpdateVariablePayVm
    {
        public VariablePaymentHeader variablepaymentheader { get; set; }

        public List<EmployeeTable> Employeetable { get; set; }
        
    }

    public class EmployeeTable
    {
        public string EmployeeName { get; set; }

        public string EmployeeDesignation { get; set; }

        public string ManagerName { get; set; }

        public int Branchid { get; set; }

        public int EmployeeId { get; set; }
    }

    public class variablepayEditVm
    {
        public List<SalaryStructureDetail> salarystructuredetail { get; set; }

        public VariablePaymentDetail variablepaymentdetail { get; set; }
    }
}