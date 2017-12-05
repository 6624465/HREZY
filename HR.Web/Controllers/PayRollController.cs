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