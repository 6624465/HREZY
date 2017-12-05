using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class PayRollController : Controller
    {
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }
        public ViewResult ContributionRegister()
        {
            return View();
        }
        public ViewResult ContributionRegisterList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SalaryRulesList()
        {
            List<SalaryRuleHeaderVm> salaryRules = new List<SalaryRuleHeaderVm>();
            return View(salaryRules);
        }
        [HttpGet]
        public ActionResult SalaryRules()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SalaryRules(SalaryRuleHeaderVm salaryRules)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                SalaryRuleHeader salaryRule = new SalaryRuleHeader()
                {
                    Category = salaryRules.Category,
                    Code = salaryRules.Code,
                    IsActive = salaryRules.IsActive,
                    IsOnPayslip = salaryRules.IsOnPayslip,
                    Name = salaryRules.Name,
                    SequenceNo = salaryRules.SequenceNo
                };
                dbContext.SalaryRuleHeaders.Add(salaryRule);

                SalaryRuleDetail salaryRuleDetail = new SalaryRuleDetail()
                {
                    AmountType = salaryRules.salaryRuleDetailVm.AmountType,
                    ConditionBased = salaryRules.salaryRuleDetailVm.ConditionBased,
                    ContributionRegister = salaryRules.salaryRuleDetailVm.ContributionRegister,
                    PythonCode = salaryRules.salaryRuleDetailVm.PythonCode,
                    RuleId = salaryRule.RuleId

                };
                dbContext.SalaryRuleDetails.Add(salaryRuleDetail);

                SalaryRuleInput salaryRuleInput = new SalaryRuleInput()
                {
                    Category = salaryRules.Category,
                    Code = salaryRules.Code,
                    Name = salaryRules.Name,
                    RuleId = salaryRule.RuleId
                };
                dbContext.SalaryRuleInputs.Add(salaryRuleInput);
                return View();
            }
        }
    }
}