using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.ViewModels;
using HR.Web.BusinessObjects.Operation;




namespace HR.Web.Controllers
{
    [SessionFilter]
    public class PayrollBatchController : BaseController
    {
        PayslipBatchHeaderBo PayslipbatchheaderBo = null;
        EmployeeHeaderBO empheaderBo = null;
        SalaryStructureDetailBO salarystructuredetailBo = null;
        SalaryStructureHeaderBO salarystructureheaderBo = null;
        VariablePaymentDetailBO variabledetailBo = null;
        public PayrollBatchController()
        {
            PayslipbatchheaderBo = new PayslipBatchHeaderBo(SESSIONOBJ);
            empheaderBo = new EmployeeHeaderBO(SESSIONOBJ);
            salarystructuredetailBo = new SalaryStructureDetailBO(SESSIONOBJ);
            salarystructureheaderBo = new SalaryStructureHeaderBO(SESSIONOBJ);
            variabledetailBo = new VariablePaymentDetailBO(SESSIONOBJ);
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


        public ActionResult UpdateVariablePay()
        {
            try
            {
                
               
                using (var dbCntx = new HrDataContext())
                {
                var list = dbCntx.EmployeeHeaders.Join(dbCntx.EmployeeWorkDetails,
                                            a => a.EmployeeId, b => b.EmployeeId, (a, b) => new { A = a, B = b }).
                                            Where(x=>x.A.BranchId==BRANCHID).
                                            Select(x => new EmployeeTable
                                            {
                                                EmployeeName = x.A.FirstName + " " + x.A.LastName,
                                                EmployeeDesignation  = dbCntx.LookUps.Where(y=>y.LookUpID==x.B.DesignationId).FirstOrDefault().LookUpDescription,
                                                ManagerName = dbCntx.EmployeeHeaders.Where(y=>y.EmployeeId==x.A.ManagerId).FirstOrDefault().FirstName,
                                                EmployeeId=x.A.EmployeeId,
                                            }).ToList();


                    var updatevariablepayvm = new UpdateVariablePayVm
                    {
                        Employeetable = list,
                        variablepaymentheader = new VariablePaymentHeader(),
                       
                    };
                    return View(updatevariablepayvm);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public PartialViewResult EditVariablePay(int Employeeid)
        {
            var updatevariablevm = new UpdateVariablePayVm();
            var structurelist = salarystructureheaderBo.GetListByProperty(x => x.EmployeeId == Employeeid).ToList();
            if (structurelist.Count()!=0)
            {
                var structureID = salarystructureheaderBo.GetByProperty(x => x.EmployeeId == Employeeid && x.IsActive == true).StructureID;
                var ComponentsList = salarystructuredetailBo.GetListByProperty(x => x.StructureID == structureID && x.IsActive == true && x.IsVariablePay == true).ToList();
                foreach (var item in ComponentsList)
                {
                    var variablepaymentdetail = new VariablePaymentDetail()
                    {
                        Amount = item.Amount,
                        ComponentCode = item.Description,
                        EmployeeId = salarystructureheaderBo.GetByProperty(x => x.StructureID == item.StructureID).EmployeeId,

                    };
                    if (updatevariablevm.variablepaymentdetail == null)
                        updatevariablevm.variablepaymentdetail = new List<VariablePaymentDetail>();

                    updatevariablevm.variablepaymentdetail.Add(variablepaymentdetail);
                }
            }
            
            return PartialView(updatevariablevm);
        }


        public ActionResult SaveVariablePay(UpdateVariablePayVm updatevariablepay)
        {
            List<VariablePaymentDetail> variablelist = new List<VariablePaymentDetail>();
            foreach(var item in updatevariablepay.variablepaymentdetail)
            {
                var variablepaymentdetail = new VariablePaymentDetail()
                {
                    Amount = item.Amount,
                    ComponentCode = item.ComponentCode,
                    EmployeeId = item.EmployeeId,
                };
               
                    variablelist.Add(variablepaymentdetail);
            }

           

            return RedirectToAction("UpdateVariablePay", variablelist);
        }


        //public ActionResult SaveVariablePaytransaction(UpdateVariablePayVm updatevariablepay)
        //{
        //    return View();

        //}

    }
}