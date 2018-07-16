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
using System.Net.Http;
using System.Drawing;

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
        PayslipBatchDetailBO payslipbatchdetailBo = null;
        TaxAssessmentDetailBO taxassessmentdetailBo = null;
        TaxAssessmentHeaderBO taxassessmentheaderBo = null;
        public PayrollBatchController()
        {
            PayslipbatchheaderBo = new PayslipBatchHeaderBo(SESSIONOBJ);
            empheaderBo = new EmployeeHeaderBO(SESSIONOBJ);
            salarystructuredetailBo = new SalaryStructureDetailBO(SESSIONOBJ);
            salarystructureheaderBo = new SalaryStructureHeaderBO(SESSIONOBJ);
            variabledetailBo = new VariablePaymentDetailBO(SESSIONOBJ);
            variablepaymentheaderBo = new VariablePaymentHeaderBO(SESSIONOBJ);
            payslipbatchdetailBo = new PayslipBatchDetailBO(SESSIONOBJ);
            taxassessmentheaderBo = new TaxAssessmentHeaderBO(SESSIONOBJ);
            taxassessmentdetailBo = new TaxAssessmentDetailBO(SESSIONOBJ);

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
                var headerlist = salarystructureheaderBo.GetListByProperty(x => x.BranchId == BRANCHID && x.EffectiveDate.Value.Month <= currentmonth && x.EffectiveDate.Value.Year == currentyear && x.IsActive == true);
                if (headerlist != null && headerlist.Count() != 0)
                {

                    //       foreach(var item in headerlist)
                    //    {
                    //        decimal sum = 0;
                    //        sum = item.NetAmount.Value;

                    //        netamount = netamount + sum;
                    //    }
                    netamount = headerlist.Sum(x => x.NetAmount).Value;
                    if (vm.payslipBatchHeader == null)
                        vm.payslipBatchHeader = new PayslipBatchHeader();
                    vm.payslipBatchHeader.TotalSalary = netamount;
                    vm.payslipBatchHeader.Month = Convert.ToByte(currentmonth.Value);
                    vm.payslipBatchHeader.Year = currentyear.Value;

                    var header = PayslipbatchheaderBo.GetByProperty(x => x.Month == Convert.ToByte(currentmonth.Value) && x.Year == currentyear && x.BranchId == BRANCHID);
                    if (header != null)
                    {
                        vm.payslipBatchHeader.BatchNo = PayslipbatchheaderBo.GetByProperty(x => x.Month == Convert.ToByte(currentmonth.Value) && x.Year == currentyear && x.BranchId == BRANCHID).BatchNo;
                    }
                    else
                    {

                        var batchcount = PayslipbatchheaderBo.GetCount(BRANCHID);
                        batchcount = batchcount + 1;
                        vm.payslipBatchHeader.BatchNo = "BATCH" + batchcount.ToString("D4");
                    }
                }
                else
                {
                    vm.payslipBatchHeader = new PayslipBatchHeader();
                    vm.payslipBatchHeader.Month = Convert.ToByte(currentmonth.Value);
                    vm.payslipBatchHeader.Year = currentyear.Value;

                }
                ViewData["ConfirmError"] = "";
                if (Session["IsError"] != null && Convert.ToBoolean(Session["IsError"]))
                {
                    ViewData["ConfirmError"] = "Please Generate The Previous Months Payslip";
                    Session.Remove("IsError");
                }

                //var structureList = dbContext.SalaryStructureDetails.Where(x => x.BranchId == 10006).ToList();
                DataRow totalsRow = vm.dt.NewRow();
                totalsRow["EmployeeName"] = "Total";
                for (int j = 5; j < vm.dt.Columns.Count; j++)
                {
                    DataColumn col = vm.dt.Columns[j];

                    decimal colTotal = 0;
                    for (int i = 0; i < col.Table.Rows.Count; i++)
                    {
                        DataRow row = col.Table.Rows[i];
                        colTotal += Convert.ToDecimal(row[col]);
                    }
                    //col.Table.Rows[j]. = Color.Red;
                    totalsRow[col.ColumnName] = colTotal;
                }
            
                vm.dt.Rows.Add(totalsRow);

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
                //Session["ConfirmError"] = "";
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

                    var transactioncount = variablepaymentheaderBo.GetCount(BRANCHID);
                    transactioncount = transactioncount + 1;

                    var updatevariablepayvm = new UpdateVariablePayVm
                    {
                        Employeetable = list,
                        variablepaymentheader = new VariablePaymentHeader(),
                        CevpdVm = null
                    };

                    updatevariablepayvm.variablepaymentheader.TransactionNo = "TRSAC" + transactioncount.ToString("D4");
                    return View(updatevariablepayvm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult EditVariablePay(int Employeeid, int Month, int Year)
        {
            var structurelist = salarystructureheaderBo
                                   .GetListByProperty(x => x.EmployeeId == Employeeid)
                                   .ToList();

            if (structurelist != null && structurelist.Count > 0)
            {
                var structureID = salarystructureheaderBo
                                        .GetByProperty(x => x.EmployeeId == Employeeid && x.IsActive == true)
                                        .StructureID;
                var ComponentsList = salarystructuredetailBo
                                        .GetListByProperty(x => x.StructureID == structureID && x.IsActive == true && x.IsVariablePay == true)
                                        .ToList();

                VariablePaymentHeader vpHeader = variablepaymentheaderBo.GetAll().Where(x => x.BranchID == BRANCHID && x.Month == Month && x.Year == Year).FirstOrDefault();

                List<VariablePaymentDetail> vpDetails = new List<VariablePaymentDetail>();
                if (vpHeader != null)
                    vpDetails = variabledetailBo.GetAll().Where(x => x.HeaderId == vpHeader.HeaderID && x.EmployeeId == Employeeid).ToList();

                var variablepaymentdetailList = new List<VariablePaymentDetail>();

                foreach (var item in ComponentsList)
                {
                    VariablePaymentDetail vpDtl = vpDetails.Where(x => x.ComponentCode == item.Code).FirstOrDefault();
                    var variablepaymentdetail = new VariablePaymentDetail()
                    {
                        Amount = vpDtl != null ? vpDtl.Amount : item.Amount,
                        ComponentCode = item.Code,
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
            VariablePaymentHeader vpHeader = variablepaymentheaderBo.GetAll().Where(x => x.BranchID == BRANCHID && x.Month == updatevariablepay.variablepaymentheader.Month && x.Year == updatevariablepay.variablepaymentheader.Year).FirstOrDefault();
            if (vpHeader != null)
            {
                //variablepaymentheaderBo.Delete(vpHeader);
                List<VariablePaymentDetail> vpDetails = variabledetailBo.GetAll().Where(x => x.HeaderId == vpHeader.HeaderID && x.EmployeeId == updatevariablepay.variablepaymentdetail[0].EmployeeId).ToList();
                for (var i = 0; i < vpDetails.Count; i++)
                {
                    VariablePaymentDetail vpDtl = variabledetailBo.GetById(vpDetails[i].DetailID);
                    variabledetailBo.Delete(vpDtl);
                }
            }

            VariablePaymentHeader variablepaymentheader = new VariablePaymentHeader()
            {
                HeaderID = vpHeader != null ? vpHeader.HeaderID : 0,
                TransactionNo = updatevariablepay.variablepaymentheader.TransactionNo,
                Month = updatevariablepay.variablepaymentheader.Month,
                Year = updatevariablepay.variablepaymentheader.Year,
                BranchID = BRANCHID,
                Status = true,
            };
            variablepaymentheaderBo.Add(variablepaymentheader);
            if (updatevariablepay.variablepaymentdetail != null && updatevariablepay.variablepaymentdetail.Count() > 0)
            {
                for (var i = 0; i < updatevariablepay.variablepaymentdetail.Count; i++)
                {
                    VariablePaymentDetail variabledetail = new VariablePaymentDetail()
                    {
                        HeaderId = variablepaymentheader.HeaderID,
                        Amount = updatevariablepay.variablepaymentdetail[i].Amount,
                        ComponentCode = updatevariablepay.variablepaymentdetail[i].ComponentCode,
                        EmployeeId = updatevariablepay.variablepaymentdetail[i].EmployeeId
                    };
                    variabledetailBo.Add(variabledetail);

                }

            }
            return RedirectToAction("UpdateVariablePay");

        }

        public ActionResult confirmprocesspayrollBySP(int year, int month)
        {
            var nullcheck = PayslipbatchheaderBo.GetAll().Where(x => x.BranchId == BRANCHID).ToList();
            if (nullcheck.Count() != 0)
            {
                var confirmMonth = PayslipbatchheaderBo.GetLatestRecord(x => x.BranchId == BRANCHID).Month;
                if (confirmMonth == (month - 1))
                {
                    using (var dbCntx = new HrDataContext())
                    {
                        var result = dbCntx.CommitPayslip(Convert.ToInt16(BRANCHID), month, year);
                    }
                    //Session["IsError"] = false;
                }
                else
                {
                    //Session["IsError"] = true;// "Please Generate The Previous Months Payslip";
                }

            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var result = dbCntx.CommitPayslip(Convert.ToInt16(BRANCHID), month, year);
                }
            }

            return RedirectToAction("ProcessPayroll", new { currentmonth = month, currentyear = year });
        }


        [HttpPost]
        public ActionResult IsValidProcessPayrollGeneration(int year, int month)
        {
            bool success = false;
            string message = "";
            var nullcheck = PayslipbatchheaderBo.GetAll().Where(x => x.BranchId == BRANCHID).ToList();
            if (nullcheck.Count() != 0)
            {
                var confirmMonth = PayslipbatchheaderBo.GetLatestRecord(x => x.BranchId == BRANCHID).Month;
                if (confirmMonth != (month - 1))
                {
                    success = true;
                    message = "Please Generate The Previous Months Payslip";
                }
                else
                {
                    success = false;
                    message = "";
                }

            }
            else
            {
                if (month == 1)
                {
                    success = false;
                    message = "";
                }
                else
                {
                    success = true;
                    message = "Please Generate The Previous Months Payslip";
                }

            }

            return Json(new { success, message });
        }

        [HttpPost]
        public ActionResult confirmprocesspayroll(PayslipBatchHeader payslipbatchheader)
        {
            using (var dbcntx = new HrDataContext())
            {
                var salaryprocesslist = dbcntx.SalaryStructureHeaders
                                        .Join(dbcntx.SalaryStructureDetails,
                                        a => a.StructureID, b => b.StructureID,
                                        (a, b) => new { A = a, B = b }).
                                        Where(x => x.A.BranchId.Value == BRANCHID && x.A.EffectiveDate.Value.Month == payslipbatchheader.Month &&
                                        x.A.EffectiveDate.Value.Year == payslipbatchheader.Year && x.A.IsActive == true).
                                        Select(x => new ProcessTable
                                        {
                                            EmployeeId = x.A.EmployeeId.Value,
                                            RegisterCode = x.B.PaymentType,
                                            ContributionCode = x.B.Code,
                                            Amount = x.B.Amount,
                                        }).ToList();

                var payslipheader = new PayslipBatchHeader()
                {
                    BatchNo = payslipbatchheader.BatchNo,
                    BranchId = BRANCHID,
                    Month = payslipbatchheader.Month,
                    Year = payslipbatchheader.Year,
                    ProcessDate = payslipbatchheader.ProcessDate,
                    TotalSalary = payslipbatchheader.TotalSalary,
                };
                //if (payrollvm.payslipBatchHeader == null)
                //   payrollvm.payslipBatchHeader = new PayslipBatchHeader();
                PayslipbatchheaderBo.Add(payslipheader);


                if (salaryprocesslist != null && salaryprocesslist.Count != 0)
                {
                    for (var i = 0; i < salaryprocesslist.Count; i++)
                    {
                        var payslipbatchdetail = new PayslipBatchDetail()
                        {
                            BatchHeaderId = payslipheader.BatchHeaderId,
                            BatchNo = payslipheader.BatchNo,
                            BranchId = payslipheader.BranchId,
                            EmployeeId = salaryprocesslist[i].EmployeeId,
                            RegisterCode = salaryprocesslist[i].RegisterCode,
                            ContributionCode = salaryprocesslist[i].ContributionCode,
                            Amount = salaryprocesslist[i].Amount,
                        };


                        payslipbatchdetailBo.Add(payslipbatchdetail);

                    }

                }

            }

            return RedirectToAction("ProcessPayroll");
        }


        [HttpGet]
        public ActionResult TaxAssessment(int year)
        {
            int branchID = BRANCHID;
            TaxAssessmentVm taxassessmentvm = new TaxAssessmentVm();
            taxassessmentvm.taxassessmentheader = taxassessmentheaderBo.GetByBranchId(branchID, year);
            taxassessmentvm.taxassessmentheader = taxassessmentvm.taxassessmentheader == null ? new TaxAssessmentHeader() : taxassessmentvm.taxassessmentheader;
            taxassessmentvm.TaxAssessmentDetailList = taxassessmentdetailBo.GetAll().Where(x => x.HeaderID == taxassessmentvm.taxassessmentheader.HeaderID).ToList();
            //var batchcount = taxassessmentheaderBo.GetCount(BRANCHID);
            //batchcount = batchcount + 1;
            //taxassessmentheaderBo.GetCount= "TAX-HR" + batchcount.ToString("D4");     
            var batchcount = taxassessmentheaderBo.GetCount(BRANCHID);
            batchcount = batchcount + 1;
            taxassessmentvm.taxassessmentheader.AssessmentNo = "TAX - HR" + batchcount.ToString("D4");
            taxassessmentvm.taxassessmentheader.Year = year;
            return View("TaxAssessment", taxassessmentvm);
        }

        [HttpPost]
        public ActionResult TaxAssessment(TaxAssessmentVm taxassessmentvm)
        {
            var addOrDelVal = Request["addOrDelete"];
            var taxAssessmentDetailid = Request["DetailId"];

            TaxAssessmentDetail taxAssessmentDetail = new TaxAssessmentDetail();
            if (!string.IsNullOrEmpty(addOrDelVal))
            {
                if (taxAssessmentDetailid != "0")
                {
                    TaxAssessmentDetail tad = taxassessmentdetailBo.GetById(Convert.ToInt32(taxAssessmentDetailid));
                    taxassessmentdetailBo.Delete(tad);
                }
                taxassessmentvm.TaxAssessmentDetailList.RemoveAt(Convert.ToInt32(addOrDelVal));
            }
            else
            {
                if (taxassessmentvm.TaxAssessmentDetailList == null)
                    taxassessmentvm.TaxAssessmentDetailList = new List<TaxAssessmentDetail>();
                taxassessmentvm.TaxAssessmentDetailList.Add(taxAssessmentDetail);
            }
            ModelState.Clear();
            return View("TaxAssessment", taxassessmentvm);
        }

        [HttpPost]
        public ActionResult SaveTaxAssessment(TaxAssessmentVm taxassessmentvm)
        {
            var taxassessmentheader = new TaxAssessmentHeader()
            {
                HeaderID = taxassessmentvm.taxassessmentheader.HeaderID,
                BranchID = BRANCHID,
                AssessmentNo = taxassessmentvm.taxassessmentheader.AssessmentNo,
                Year = taxassessmentvm.taxassessmentheader.Year,
                SocialContributionRate = taxassessmentvm.taxassessmentheader.SocialContributionRate,
                MaximumAmount = taxassessmentvm.taxassessmentheader.MaximumAmount,
                Status = true,
            };
            taxassessmentheaderBo.Add(taxassessmentheader);
            foreach (var item in taxassessmentvm.TaxAssessmentDetailList)
            {
                var taxassessmentdetail = new TaxAssessmentDetail()
                {
                    HeaderID = taxassessmentheader.HeaderID,
                    ID = item.ID,
                    SalaryFrom = item.SalaryFrom,
                    SalaryTo = item.SalaryTo,
                    Rate = item.Rate,
                    Maxamount = item.Maxamount
                };
                taxassessmentdetailBo.Add(taxassessmentdetail);
            }
            return RedirectToAction("TaxAssessment", new { taxassessmentheader.Year });
        }

        [HttpGet]
        public ActionResult DeleteProcessedPayroll(int month, int year)
        {

            bool success = false;
            string message = "";
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            var checkpreviousmonths = PayslipbatchheaderBo.GetListByProperty(x => x.BranchId == BRANCHID && x.Month > month && x.Year == year).ToList();
            if (checkpreviousmonths.Count() > 0)
            {
                success = true;
                message = "Please delete the next months Payroll first.";
            }
            else
            {
                PayslipBatchHeader DeletePayrollObjheader = PayslipbatchheaderBo.GetByProperty(x => x.BranchId == BRANCHID && x.Month == month && x.Year == year);
                if (DeletePayrollObjheader != null)
                {
                    List<PayslipBatchDetail> DeletePayrollObjdetail = payslipbatchdetailBo.GetListByProperty(x => x.BatchHeaderId == DeletePayrollObjheader.BatchHeaderId).ToList();
                    if (DeletePayrollObjheader != null)
                    {
                        PayslipbatchheaderBo.Delete(DeletePayrollObjheader);
                        foreach (var item in DeletePayrollObjdetail)
                        {
                            payslipbatchdetailBo.Delete(item);
                        }
                        success = false;
                        message = "";
                    }
                }

                else
                {
                    success = true;
                    message = "Deletion Is Not Possible Because Payroll is not generated yet For this Month.Please check.";
                    //ViewData["message"] = "Payroll is not generated yet For this Month.Please check.";
                }
            }
            return Json(new { success, message }, JsonRequestBehavior.AllowGet);

        }
    }
}