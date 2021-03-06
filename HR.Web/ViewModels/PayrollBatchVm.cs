﻿using HR.Web.Models;
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
        public List<VariablePaymentDetail> variablepaymentdetail { get; set; }

        public List<VariablePaymentDetail> CevpdVm { get; set; }
                                         //Current employee variable payment detail vm
    }
    public class TaxAssessmentVm
    {
        public TaxAssessmentHeader taxassessmentheader { get; set; }
        public List<TaxAssessmentDetail> TaxAssessmentDetailList { get; set; }


    }
    public class EmployeeTable
    {
        public string EmployeeName { get; set; }

        public string EmployeeDesignation { get; set; }

        public string ManagerName { get; set; }

        public int Branchid { get; set; }

        public int EmployeeId { get; set; }
    }


    //public class ConfirmProcessVm
    //{
    //    public PayslipBatchHeader payslipBatchHeader { get; set; }
    //    public List<PayslipBatchDetail> payslipBatchDetail { get; set; }

    //    public List<ProcessTable> ProcessTable { get; set; }

    //}

    public class ProcessTable
    {
        public int EmployeeId { get; set; }
        public string RegisterCode { get; set; }
        public string ContributionCode { get; set; }
        public decimal? Amount { get; set; }
    }
  

    //public class variablepayEditVm
    //{



    //}
}