using HR.Web.BusinessObjects.Payroll;
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
        SalaryRuleBO salaryRuleBO = null;
        SalaryRuleInputBO salaryRuleInputBO = null;
        SalaryRuleDetailBO salaryRuleDetailBO = null;
        ContributionBO contributionBO = null;
        SalaryStructureHeaderBO salaryStructureHeaderBO = null;
        SalaryStructureDetailBO salaryStructureDetailBO = null;
        public PayRollController()
        {
            salaryRuleBO = new SalaryRuleBO(SESSIONOBJ);
            salaryRuleInputBO = new SalaryRuleInputBO(SESSIONOBJ);
            salaryRuleDetailBO = new SalaryRuleDetailBO(SESSIONOBJ);
            contributionBO = new ContributionBO(SESSIONOBJ);
            salaryStructureHeaderBO = new SalaryStructureHeaderBO(SESSIONOBJ);
            salaryStructureDetailBO = new SalaryStructureDetailBO(SESSIONOBJ);
        }
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }
        public ActionResult ContributionRegister(int ContributionId)
        {
            if (ContributionId != -1)
            {
                var contributionregisterobj = contributionBO.GetById(ContributionId);
                return View(contributionregisterobj);
            }
            else
            {
                return View(new Contribution());
            }

        }
        public ActionResult ContributionRegisterList()
        {

            var list = contributionBO.GetListByProperty(x => x.IsActive == true);
            return View(list);
        }
        public ActionResult ContributionSave(Contribution contribution)
        {
            if (contribution.ContributionId == -1)
            {
                contributionBO.Add(contribution);
            }
            else
            {
                contributionBO.Add(contribution);
            }

            return RedirectToAction("ContributionRegisterList");
        }

        [HttpPost]
        public JsonResult ContributionDelete(int contributionid)
        {
            contributionBO.Delete(contributionid);
            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
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
                if (RuleId != 0)
                {
                    var salaryRule = dbContext.SalaryRuleHeaders
                        .Join(dbContext.SalaryRuleDetails,
                        a => a.RuleId, b => b.RuleId,
                        (a, b) => new { A = a, B = b })
                        .Join(dbContext.Contributions,
                        c => c.A.RuleId, d => d.ContributionId,
                        (c, d) => new { C = c, D = d }).Where(x => x.C.A.RuleId == RuleId).FirstOrDefault();
                    //SalaryRuleHeader salaryRule = dbContext.SalaryRuleHeaders.Where(x => x.RuleId == RuleId).FirstOrDefault();
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
                        contributionVm = new ContributionVm()
                        {
                            ContributionId = salaryRule.D.ContributionId,
                            IsActive = salaryRule.D.IsActive,
                            Name = salaryRule.D.Name,
                            Description = salaryRule.D.Description
                        }
                    };
                    return View(salaryRuleVm);
                }
                else
                {
                    return View(new SalaryRuleHeaderVm() { IsActive = false, IsOnPayslip = false });
                }
            }
        }
        [HttpPost]
        public ActionResult SalaryRules(SalaryRuleHeaderVm salaryRules)
        {
            salaryRuleBO.SavePayroll(salaryRules);
            return RedirectToAction("SalaryRulesList");
        }

        [HttpGet]
        public ActionResult SalaryStructure()
        {
            SalaryStructureVm salaryStructureVm = new SalaryStructureVm();
            salaryStructureVm.structureHeader = new SalaryStructureHeader();

            List<Contribution> contributionList = contributionBO.GetListByProperty(x => x.IsActive == true).ToList();
            salaryStructureVm.structureDetail = new List<SalaryStructureDetail>();
            foreach (Contribution item in contributionList)
            {
                SalaryStructureDetail salaryStructureDetail = new SalaryStructureDetail()
                {
                    Code = item.Name,
                    Description = item.Description,

                };
                salaryStructureVm.structureDetail.Add(salaryStructureDetail);
            }

            return View(salaryStructureVm);
        }

        [HttpPost]
        public ActionResult SalaryStructure(SalaryStructureVm salaryStructureVm)
        {
            return View();
        }
        public ViewResult SalaryStructureHeaderList(int? page=1)
        {
            var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appSalaryStructureHeaderOffset"]);
            int skip = (page.Value - 1) * offset;
            var list = salaryStructureHeaderBO.GetListByProperty( x => x.IsActive == true);
            var count = list.Count();
            decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));
            HtmlTblVm<SalaryStructureHeader> HtmlTblVm = new HtmlTblVm<SalaryStructureHeader>();
            HtmlTblVm.TableData = list.Skip(skip).Take(offset).ToList();
            HtmlTblVm.TotalRows = count;
            HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
            HtmlTblVm.CurrentPage = page.Value;
            return View(HtmlTblVm);
        }
    }
}