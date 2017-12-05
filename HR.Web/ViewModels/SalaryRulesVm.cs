using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class SalaryRulesVm
    {
        public string Name { get; set; }

        public string Category { get; set; }
        public string Code { get; set; }

        public int Sequence { get; set; }

        public bool IsActive { get; set; }


        public string IsOnPayslip { get; set; }

        public string ConditionBased { get; set; }
        public string AmountType { get; set; }
        public string PythonCode { get; set; }
        public string ContributionRegister { get; set; }
        /*Childrens*/
        public string ChildrenName { get; set; }

        public string ChildrenCategory { get; set; }
        public string ChildrenCode { get; set; }
        public string ChildrenContributionRegister { get; set; }
    }
}