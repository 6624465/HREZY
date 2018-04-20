using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.ViewModels;



namespace HR.Web.Controllers
{
    [SessionFilter]
    public class PayrollBatchController : BaseController
    {
        PayslipBatchHeaderBo PayslipbatchheaderBo = null;
        public PayrollBatchController()
        {
            PayslipbatchheaderBo = new PayslipBatchHeaderBo(SESSIONOBJ);
        }
        // GET: PayrollBatch
        [HttpGet]
        public ActionResult ProcessPayroll(int? currentmonth,int? currentyear)
        {
            using (var dbContext=new HrDataContext())
            {
                var list = dbContext.SalaryStructureHeaders.GroupJoin(dbContext.SalaryStructureDetails,
                    a => a.StructureID, b => b.StructureID,
                   (a, b) => new { A = a, B = b.ToList() });
                PayrollBatchVm vm = new PayrollBatchVm();

                if (currentmonth!=null && currentyear != null)
                {
                    vm.dt = PayslipbatchheaderBo.GeneratePayslip(Convert.ToInt16(BRANCHID), currentmonth.Value, currentyear.Value);
                }
                   
               
                //var structureList = dbContext.SalaryStructureDetails.Where(x => x.BranchId == 10006).ToList();
             
                return View(vm);
            }
                
            
        }
        [HttpPost]
        public ActionResult ProcessPayrollGeneration(int? currentmonth, int? currentyear)
        {
            using (var dbContext = new HrDataContext())
            {
                //var list = dbContext.SalaryStructureHeaders.GroupJoin(dbContext.SalaryStructureDetails,
                //    a => a.StructureID, b => b.StructureID,
                //   (a, b) => new { A = a, B = b.ToList() });


                //var structureList = dbContext.SalaryStructureDetails.Where(x => x.BranchId == 10006).ToList();
                PayrollBatchVm vm = new PayrollBatchVm();
               // vm.dt = PayslipbatchheaderBo.GeneratePayslip(Convert.ToInt16(BRANCHID), 4, 2018);
                return RedirectToAction("ProcessPayroll", new { currentmonth, currentyear });
            }


        }

    }
}