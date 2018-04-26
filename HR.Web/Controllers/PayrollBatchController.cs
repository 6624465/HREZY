﻿using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.ViewModels;
using HR.Web.BusinessObjects.Operation;
using System.Net.Http;


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
        VariablePaymentHeaderBO variablepaymentheaderBo = null;
        public PayrollBatchController()
        {
            PayslipbatchheaderBo = new PayslipBatchHeaderBo(SESSIONOBJ);
            empheaderBo = new EmployeeHeaderBO(SESSIONOBJ);
            salarystructuredetailBo = new SalaryStructureDetailBO(SESSIONOBJ);
            salarystructureheaderBo = new SalaryStructureHeaderBO(SESSIONOBJ);
            variabledetailBo = new VariablePaymentDetailBO(SESSIONOBJ);
            variablepaymentheaderBo = new VariablePaymentHeaderBO(SESSIONOBJ);
        }
        // GET: PayrollBatch
        [HttpGet]
        public ActionResult ProcessPayroll(int? currentmonth, int? currentyear)
        {
            using (var dbContext = new HrDataContext())
            {
                var list = dbContext.SalaryStructureHeaders.GroupJoin(dbContext.SalaryStructureDetails,
                    a => a.StructureID, b => b.StructureID,
                   (a, b) => new { A = a, B = b.ToList() });
                PayrollBatchVm vm = new PayrollBatchVm();

                if (currentmonth != null && currentyear != null)
                {
                    vm.dt = PayslipbatchheaderBo.GeneratePayslip(Convert.ToInt16(BRANCHID), currentmonth.Value, currentyear.Value);
                }
                decimal? netamount = 0;
                var headerlist = salarystructureheaderBo.GetListByProperty(x => x.BranchId == BRANCHID && x.EffectiveDate.Value.Month == currentmonth && x.EffectiveDate.Value.Year == currentyear && x.IsActive==true);
                if (headerlist != null && headerlist.Count() != 0)
                {
                    
                //       foreach(var item in headerlist)
                //    {
                //        decimal sum = 0;
                //        sum = item.NetAmount.Value;
                       
                //        netamount = netamount + sum;
                //    }
                   netamount = headerlist.Sum(x=>x.NetAmount).Value;
                    if (vm.payslipBatchHeader == null)
                        vm.payslipBatchHeader = new PayslipBatchHeader();
                    vm.payslipBatchHeader.TotalSalary = netamount;

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
                //var netsalarytotal = salarystructureheaderBo.GetAll();
                //if (netsalarytotal != null)
                //{
                //    vm.payslipBatchHeader.TotalSalary = salarystructureheaderBo.GetListByProperty(x => x.BranchId == BRANCHID && x.IsActive==true).Sum(x => x.NetAmount);
                //}
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
                        Where(x => x.A.BranchId == BRANCHID && x.A.IsActive == true).
                        Select(x => new EmployeeTable
                        {
                            EmployeeName = x.A.FirstName + " " + x.A.LastName,
                            EmployeeDesignation = dbCntx.LookUps.Where(y => y.LookUpID == x.B.DesignationId).FirstOrDefault().LookUpDescription,
                            ManagerName = dbCntx.EmployeeHeaders.Where(y => y.EmployeeId == x.A.ManagerId).FirstOrDefault().FirstName,
                            EmployeeId = x.A.EmployeeId,
                        }).ToList();

                    var updatevariablepayvm = new UpdateVariablePayVm
                    {
                        Employeetable = list,
                        variablepaymentheader = new VariablePaymentHeader(),
                        CevpdVm = null
                    };
                    return View(updatevariablepayvm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult EditVariablePay(int Employeeid)
        {
            var structurelist = salarystructureheaderBo
                                   .GetListByProperty(x => x.EmployeeId == Employeeid)
                                   .ToList();

            if(structurelist != null && structurelist.Count > 0)
            {
                var structureID = salarystructureheaderBo
                                        .GetByProperty(x => x.EmployeeId == Employeeid && x.IsActive == true)
                                        .StructureID;
                var ComponentsList = salarystructuredetailBo
                                        .GetListByProperty(x => x.StructureID == structureID && x.IsActive == true && x.IsVariablePay == true)
                                        .ToList();

                var variablepaymentdetailList = new List<VariablePaymentDetail>();
                foreach (var item in ComponentsList)
                {
                    var variablepaymentdetail = new VariablePaymentDetail()
                    {
                        Amount = item.Amount,
                        ComponentCode = item.Description,
                        EmployeeId = salarystructureheaderBo
                                        .GetByProperty(x => x.StructureID == item.StructureID)
                                        .EmployeeId,

                    };

                    variablepaymentdetailList.Add(variablepaymentdetail);
                }

                return Json(variablepaymentdetailList, JsonRequestBehavior.AllowGet);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        /*
        [HttpPost]
        public PartialViewResult EditVariablePay(UpdateVariablePayVm updatevariablepay, int Employeeid)
        {
            ViewBag.TempEmployeeID = Employeeid;
            var updatevariablevm = new UpdateVariablePayVm();
           
            if (updatevariablepay.variablepaymentdetail == null)
            {

                var structurelist = salarystructureheaderBo
                                   .GetListByProperty(x => x.EmployeeId == Employeeid)
                                   .ToList();
                if (structurelist.Count != 0)
                {
                    var structureID = salarystructureheaderBo
                                        .GetByProperty(x => x.EmployeeId == Employeeid && x.IsActive == true)
                                        .StructureID;
                    var ComponentsList = salarystructuredetailBo
                                            .GetListByProperty(x => x.StructureID == structureID && x.IsActive == true && x.IsVariablePay == true)
                                            .ToList();
                    foreach (var item in ComponentsList)
                    {
                        var variablepaymentdetail = new VariablePaymentDetail()
                        {
                            Amount = item.Amount,
                            ComponentCode = item.Description,
                            EmployeeId = salarystructureheaderBo
                                            .GetByProperty(x => x.StructureID == item.StructureID)
                                            .EmployeeId,

                        };
                        if (updatevariablevm.variablepaymentdetail == null)
                            updatevariablevm.variablepaymentdetail = new List<VariablePaymentDetail>();

                        updatevariablevm.variablepaymentdetail.Add(variablepaymentdetail);
                    }
                }

                }
            else
            {

                foreach (var item in updatevariablepay.variablepaymentdetail)
                {
                    var variablepaymentdetail = new VariablePaymentDetail()
                    {
                        Amount = item.Amount,
                        ComponentCode = item.ComponentCode,
                        EmployeeId = item.EmployeeId

                    };
                    if (updatevariablevm.variablepaymentdetail == null)
                        updatevariablevm.variablepaymentdetail = new List<VariablePaymentDetail>();

                    updatevariablevm.variablepaymentdetail.Add(variablepaymentdetail);
                }

            }

            return PartialView(updatevariablevm);
        }
        */
        /*
        [HttpPost]
        public ActionResult SaveVariablePay(UpdateVariablePayVm updatevariablepay)
        {
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.EmployeeHeaders.Join(dbCntx.EmployeeWorkDetails,
                        a => a.EmployeeId, b => b.EmployeeId, (a, b) => new { A = a, B = b }).
                        Where(x => x.A.BranchId == BRANCHID && x.A.IsActive == true).
                        Select(x => new EmployeeTable
                        {
                            EmployeeName = x.A.FirstName + " " + x.A.LastName,
                            EmployeeDesignation = dbCntx.LookUps.Where(y => y.LookUpID == x.B.DesignationId).FirstOrDefault().LookUpDescription,
                            ManagerName = dbCntx.EmployeeHeaders.Where(y => y.EmployeeId == x.A.ManagerId).FirstOrDefault().FirstName,
                            EmployeeId = x.A.EmployeeId,
                        }).ToList();

                if (updatevariablepay.variablepaymentdetail == null)
                    updatevariablepay.variablepaymentdetail = new List<VariablePaymentDetail>();

                var tempEmployeeId = Convert.ToInt32(Request.Form["tempEmployeeId"]);

                var empList = updatevariablepay.variablepaymentdetail.Where(x => x.EmployeeId == tempEmployeeId).ToList();
                if (empList != null)
                {
                    empList.ForEach(x =>
                    {
                        updatevariablepay.variablepaymentdetail.Remove(x);
                    });
                }

                updatevariablepay.variablepaymentdetail.AddRange(updatevariablepay.CevpdVm);
                updatevariablepay.CevpdVm = null;
                updatevariablepay.Employeetable = list;

                return View("UpdateVariablePay", updatevariablepay);
            }

        }
        */
        [HttpPost]
        public ActionResult SaveVariablePaytransaction(UpdateVariablePayVm updatevariablepay)
        {
            VariablePaymentHeader variablepaymentheader = new VariablePaymentHeader()
            {
                TransactionNo = updatevariablepay.variablepaymentheader.TransactionNo,
                Month = updatevariablepay.variablepaymentheader.Month,
                Year = updatevariablepay.variablepaymentheader.Year,
                BranchID = BRANCHID,
                Status = true,
            };
            variablepaymentheaderBo.Add(variablepaymentheader);
            if (updatevariablepay.variablepaymentdetail != null && updatevariablepay.variablepaymentdetail.Count() > 0)
            {
                for (var i = 0; i < updatevariablepay.CevpdVm.Count; i++)
                {
                    VariablePaymentDetail variabledetail = new VariablePaymentDetail()
                    {
                        HeaderId = variablepaymentheader.HeaderID,
                        Amount = updatevariablepay.CevpdVm[i].Amount,
                        ComponentCode = updatevariablepay.CevpdVm[i].ComponentCode,
                        EmployeeId = updatevariablepay.CevpdVm[i].EmployeeId
                    };
                    variabledetailBo.Add(variabledetail);
                }
            }
            return RedirectToAction("UpdateVariablePay");

        }

    }
}