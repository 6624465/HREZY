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
            using (var dbcntx = new HrDataContext())
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

        [HttpGet]
        public ActionResult SalaryRulesList(int? page = 1)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appViewLeaveListOffSet"]);
                int skip = (page.Value - 1) * offset;
                List<SalaryRuleHeaderVm> salaryRules = dbContext.SalaryRuleHeaders
                    .Select(x => new SalaryRuleHeaderVm()
                    {
                        RuleId = x.RuleId,
                        Category = x.Category,
                        Code = x.Code,
                        Name = x.Name,
                        SequenceNo = x.SequenceNo
                    }).OrderByDescending(x => x.RuleId).Skip(skip).Take(offset).ToList();
                var count = salaryRules.Count();
                decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));

                HtmlTblVm<SalaryRuleHeaderVm> HtmlTblVm = new HtmlTblVm<SalaryRuleHeaderVm>();
                HtmlTblVm.TableData = salaryRules;
                HtmlTblVm.TotalRows = count;
                HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
                HtmlTblVm.CurrentPage = page.Value;
                return View(HtmlTblVm);
            }
        }
        [HttpGet]
        public ActionResult SalaryRules(int RuleId = 0)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                var salaryRule = dbContext.SalaryRuleHeaders
                    .Join(dbContext.SalaryRuleDetails,
                    a => a.RuleId, b => b.RuleId,
                    (a, b) => new { A = a, B = b })
                    .Join(dbContext.SalaryRuleInputs,
                    c => c.A.RuleId, d => d.RuleId,
                    (c, d) => new { C = c, D = d }).Where(x => x.C.A.RuleId == RuleId).FirstOrDefault();
                //SalaryRuleHeaderVm salaryRule = dbContext.SalaryRuleHeaders.Where(x => x.RuleId == RuleId).FirstOrDefault();
                SalaryRuleHeaderVm salaryRuleVm = new SalaryRuleHeaderVm()
                {
                    Category = salaryRule.C.A.Category,
                    Code = salaryRule.C.A.Code,
                    IsActive = salaryRule.C.A.IsActive ?? false,
                    IsOnPayslip = salaryRule.C.A.IsOnPayslip ?? false,
                    Name = salaryRule.C.A.Name,
                    RuleId = salaryRule.C.A.RuleId,
                    SequenceNo = salaryRule.C.A.RuleId,
                    salaryRuleDetailVm = new SalaryRuleDetailVm()
                    {
                        AmountType = salaryRule.C.B.AmountType,
                        ConditionBased = salaryRule.C.B.ConditionBased,
                        ContributionRegister = salaryRule.C.B.ContributionRegister,
                        PythonCode = salaryRule.C.B.PythonCode,
                        RuleDetailId = salaryRule.C.B.RuleDetailId,
                        RuleId = salaryRule.C.B.RuleId,
                    },
                    salaryRuleInputVm = new SalaryRuleInputVm()
                    {
                        Category = salaryRule.D.Category,
                        Code = salaryRule.D.Code,
                        Name = salaryRule.D.Name,
                        RuleId = salaryRule.D.RuleId,
                        RuleInputId = salaryRule.D.RuleInputId
                    }
                };
                return View(salaryRuleVm);
            }
        }
        [HttpPost]
        public ActionResult SalaryRules(SalaryRuleHeaderVm salaryRules)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                if (salaryRules.RuleId == 0)
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
                        RuleId = salaryRule.RuleId,
                        ContributionRegister = salaryRuleDetail.ContributionRegister
                    };
                    dbContext.SalaryRuleInputs.Add(salaryRuleInput);
                }
                else
                {

                    SalaryRuleHeader salaryRule = dbContext.SalaryRuleHeaders
                        .Where(x => x.RuleId == salaryRules.RuleId).FirstOrDefault();

                    salaryRule.RuleId = salaryRules.RuleId;
                    salaryRule.Category = salaryRules.Category;
                    salaryRule.Code = salaryRules.Code;
                    salaryRule.IsActive = salaryRules.IsActive;
                    salaryRule.IsOnPayslip = salaryRules.IsOnPayslip;
                    salaryRule.Name = salaryRules.Name;
                    salaryRule.SequenceNo = salaryRules.SequenceNo;

                    SalaryRuleDetail salaryRuleDetail = dbContext.SalaryRuleDetails
                        .Where(x => x.RuleDetailId == salaryRules.salaryRuleDetailVm.RuleDetailId).FirstOrDefault();

                    salaryRuleDetail.RuleDetailId = salaryRules.salaryRuleDetailVm.RuleDetailId;
                    salaryRuleDetail.AmountType = salaryRules.salaryRuleDetailVm.AmountType;
                    salaryRuleDetail.ConditionBased = salaryRules.salaryRuleDetailVm.ConditionBased;
                    salaryRuleDetail.ContributionRegister = salaryRules.salaryRuleDetailVm.ContributionRegister;
                    salaryRuleDetail.PythonCode = salaryRules.salaryRuleDetailVm.PythonCode;
                    salaryRuleDetail.RuleId = salaryRule.RuleId;

                    SalaryRuleInput salaryRuleInput = dbContext.SalaryRuleInputs
                        .Where(x => x.RuleInputId == salaryRules.salaryRuleInputVm.RuleInputId).FirstOrDefault();
                    if (salaryRuleInput != null)
                    {
                        salaryRuleInput.RuleInputId = salaryRuleInput.RuleInputId;
                        salaryRuleInput.Category = salaryRuleInput.Category;
                        salaryRuleInput.Code = salaryRuleInput.Code;
                        salaryRuleInput.Name = salaryRuleInput.Name;
                        salaryRuleInput.RuleId = salaryRuleInput.RuleId;
                        salaryRuleInput.ContributionRegister = salaryRuleInput.ContributionRegister;
                    }
                }
                dbContext.SaveChanges();
                return RedirectToAction("SalaryRulesList");
            }
        }

        public ActionResult SalaryStructure()
        {
            return View();
        }
    }
}