using HR.Web.ViewModels;
using HR.Web.Models;
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
        public ActionResult ContributionRegister()
        {
            return View();
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
            return RedirectToAction("ContributionRegisterList");
        }

        [HttpGet]
        public ActionResult SalaryRulesList()
        {
            List<SalaryRulesVm> salaryRules = new List<SalaryRulesVm>();
            return View(salaryRules);
        }
        [HttpGet]
        public ActionResult SalaryRules()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SalaryRules(SalaryRulesVm salaryRules)
        {
            return View();
        }
    }
}