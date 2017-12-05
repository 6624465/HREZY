using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class SalaryRuleHeaderVm
    {
        public int RuleId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public int? SequenceNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsOnPayslip { get; set; }

        public SalaryRuleDetailVm salaryRuleDetailVm { get; set; }
        public SalaryRuleInputVm salaryRuleInputVm { get; set; }


    }



    public class SalaryRuleInputVm
    {
        public int RuleInputId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string ContributionRegister { get; set; }
        public int RuleId { get; set; }
    }
    public class SalaryRuleDetailVm
    {
        public int RuleDetailId { get; set; }
        public string ConditionBased { get; set; }
        public string AmountType { get; set; }
        public string PythonCode { get; set; }
        public string ContributionRegister { get; set; }
        public int RuleId { get; set; }
    }


}