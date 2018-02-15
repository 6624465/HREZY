using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    [SessionFilter]
    public class PayrollBatchController : Controller
    {
        HrDataContext dbContext = new HrDataContext();

        // GET: PayrollBatch
        [HttpGet]
        public ActionResult ProcessPayroll()
        {
            var list = dbContext.SalaryStructureHeaders.GroupJoin(dbContext.SalaryStructureDetails,
                a=>a.StructureID, b=>b.StructureID,
               (a,b) => new { A=a,B=b.ToList()});
            return View();
        }

    }
}