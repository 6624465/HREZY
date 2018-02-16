using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
                a => a.StructureID, b => b.StructureID,
               (a, b) => new { A = a, B = b.ToList() });
          
            //var structureList = dbContext.SalaryStructureDetails.Where(x => x.BranchId == 10006).ToList();

            return View();
        }
    }
}