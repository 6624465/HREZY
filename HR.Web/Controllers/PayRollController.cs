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
    public class PayRollController : BaseController
    {
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }
        public ActionResult ContributionRegister(int ContributionId)
        {
            if (ContributionId != -1)
            {
                using (var dbcntx = new HrDataContext())
                {
                    var contributionregisterobj = dbcntx.Contributions.Where(x => x.ContributionId == ContributionId).FirstOrDefault();
                    return View(contributionregisterobj);

                }
            }
            else
            {
                return View(new Contribution());
            }
          
        }
        public ActionResult ContributionRegisterList()
        {
            using (var dbcntx=new HrDataContext())
            {
                var list = dbcntx.Contributions.ToList();
                return View(list);
            }
           
        }
        public ActionResult ContributionSave(Contribution contribution)
        {
            if (contribution.ContributionId == -1)
            {
                using (var dbcntx = new HrDataContext())
                {
                    var contributionregistersave = new Contribution
                    {
                        ContributionId = contribution.ContributionId,
                        Name = contribution.Name,
                        Description = contribution.Description,
                        IsActive = contribution.IsActive,
                        CreatedBy = USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        ModifiedBy = USERID,
                        ModifiedOn = UTILITY.SINGAPORETIME
                    };
                    dbcntx.Contributions.Add(contributionregistersave);
                    dbcntx.SaveChanges();

                }
            }
            else
            {
                using (var dbcntx = new HrDataContext())
                {
                    var contributionregister = dbcntx.Contributions.Where(x => x.ContributionId == contribution.ContributionId).FirstOrDefault();
                    contributionregister.ContributionId = contribution.ContributionId;
                    contributionregister.Name = contribution.Name;
                    contributionregister.Description = contribution.Description;
                    contributionregister.IsActive = contribution.IsActive;
                    contributionregister.CreatedBy = USERID;
                    contributionregister.CreatedOn = UTILITY.SINGAPORETIME;
                    contributionregister.ModifiedBy = USERID;
                    contributionregister.ModifiedOn = UTILITY.SINGAPORETIME;
                    dbcntx.SaveChanges();
                };
               
            }
           
            return RedirectToAction("ContributionRegisterList");
        }

        [HttpPost]
        public JsonResult ContributionDelete(int contributionid)
        {
            using (var dbcntx = new HrDataContext())
            {
                var record = dbcntx.Contributions.Where(x => x.ContributionId == contributionid).FirstOrDefault();
                dbcntx.Contributions.Remove(record);
                dbcntx.SaveChanges();
            }
            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SalaryRulesList()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                List<SalaryRuleHeaderVm> salaryRules = dbContext.SalaryRuleHeaders
                    .Select(x=>new SalaryRuleHeaderVm() {
                        Category=x.Category,
                        Code=x.Code,
                        Name=x.Name,
                        SequenceNo=x.SequenceNo
                    }).ToList();
                return View(salaryRules);
            }
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
                dbContext.SaveChanges();
                return View();
            }
        }

        public ActionResult SalaryStructure() {
            return View();
        }
    }
}