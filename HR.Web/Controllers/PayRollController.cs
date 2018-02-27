using HR.Web.BusinessObjects.Operation;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{
    [SessionFilter]
    //[ErrorHandler]
    public class PayRollController : BaseController
    {
        SalaryRuleBO salaryRuleBO = null;
        SalaryRuleInputBO salaryRuleInputBO = null;
        SalaryRuleDetailBO salaryRuleDetailBO = null;
        ContributionBO contributionBO = null;
        SalaryStructureHeaderBO salaryStructureHeaderBO = null;
        SalaryStructureDetailBO salaryStructureDetailBO = null;
        EmpSalaryStructureHeaderBO empSalaryStructureHeaderBO = null;
        TravelClaimHeaderBO travelClaimHeaderBO = null;
        TravelClaimDetailBO travelClaimDetailBO = null;
        public PayRollController()
        {
            salaryRuleBO = new SalaryRuleBO(SESSIONOBJ);
            salaryRuleInputBO = new SalaryRuleInputBO(SESSIONOBJ);
            salaryRuleDetailBO = new SalaryRuleDetailBO(SESSIONOBJ);
            contributionBO = new ContributionBO(SESSIONOBJ);
            salaryStructureHeaderBO = new SalaryStructureHeaderBO(SESSIONOBJ);
            salaryStructureDetailBO = new SalaryStructureDetailBO(SESSIONOBJ);
            empSalaryStructureHeaderBO = new EmpSalaryStructureHeaderBO(SESSIONOBJ);
            travelClaimHeaderBO = new TravelClaimHeaderBO(SESSIONOBJ);
            travelClaimDetailBO = new TravelClaimDetailBO(SESSIONOBJ);
        }
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }
        public ActionResult ContributionRegister(int ContributionId)
        {
            ViewData["RoleCode"] = ROLECODE;
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
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
            else
            {
                if (ContributionId != -1)
                {
                    var contributionregisterobj = contributionBO.GetById(ContributionId);
                    contributionregisterobj.BranchId = BRANCHID;
                    return View(contributionregisterobj);
                }
                else
                {
                    return View(new Contribution { BranchId = BRANCHID });
                }
            }
        }
        public ActionResult ContributionRegisterList(int? BranchId = 0, int? page = 1)
        {
            ViewData["RoleCode"] = ROLECODE;
            var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skip = (page.Value - 1) * offset;
            List<Contribution> list = new List<Contribution>();
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
                list = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BranchId && x.SortBy != -1).OrderBy(x => x.BranchId).ToList();
            else
                list = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BRANCHID && x.SortBy != -1).ToList();

            var count = list.Count();
            decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));
            HtmlTblVm<Contribution> HtmlTblVm = new HtmlTblVm<Contribution>();
            HtmlTblVm.TableData = list.Skip(skip).Take(offset).ToList();
            HtmlTblVm.TotalRows = count;
            HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
            HtmlTblVm.CurrentPage = page.Value;
            return View(HtmlTblVm);
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
            var list = contributionBO.GetListByProperty(x => (x.BranchId == contribution.BranchId) && (x.Name == UTILITY.BASICSALARYCOMPONENT)).ToList();
            var count = list.Count();
            if (count == 0)
            {
                Contribution contributionBasic = new Contribution
                {
                    Name = UTILITY.BASICSALARYCOMPONENT,
                    Description = UTILITY.BASICSALARYCOMPONENT,
                    IsActive = true,
                    CreatedBy = SESSIONOBJ.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    RegisterCode = "PAYMENTS",
                    BranchId = contribution.BranchId,
                    SortBy = -1
                };
                contributionBO.Add(contributionBasic);

            }
            return RedirectToAction("ContributionRegisterList");
        }

        //[HttpPost]
        //public JsonResult ContributionDelete(int contributionid)
        //{
        //    contributionBO.Delete(contributionid);
        //    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult ContributionDelete(int contributionid)
        {
            contributionBO.Delete(contributionid);
            return RedirectToAction("ContributionRegisterList");
        }
        [HttpGet]
        public ActionResult SalaryRulesList(int? page = 1)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
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
        public ActionResult SalaryStructure(int structurId = 0, int BranchId = 0)
        {
            ViewData["RoleCode"] = ROLECODE;

            SalaryStructureVm salaryStructureVm = new SalaryStructureVm();
            List<Contribution> contributionList = new List<Contribution>();
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
            {
                contributionList = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BranchId)
                    .OrderBy(x => x.SortBy).ToList();
            }
            else
                contributionList = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BRANCHID)
               .OrderBy(x => x.SortBy).ToList();

            salaryStructureVm.structureSalaryPaymentDetail = new List<SalaryStructureDetail>();
            salaryStructureVm.structureEmployerContributionDetail = new List<SalaryStructureDetail>();
            salaryStructureVm.structureEmployeeContributionDetail = new List<SalaryStructureDetail>();
            if (structurId == 0)
            {
                salaryStructureVm.structureHeader = new SalaryStructureHeader();

                if (BranchId == 0 && BRANCHID != -1)
                    BranchId = BRANCHID;
                var structureCount = salaryStructureHeaderBO.GetCount(BranchId);
                structureCount = structureCount + 1;
                salaryStructureVm.structureHeader.Code = "EZYPR" + structureCount.ToString("D4");
                if (ROLECODE != UTILITY.ROLE_SUPERADMIN)
                    salaryStructureVm.structureHeader.BranchId = BRANCHID;
                else
                    salaryStructureVm.structureHeader.BranchId = BranchId;
                salaryStructureVm.structureSalaryPaymentDetail = contributionList
                    .Where(x => x.RegisterCode == UTILITY.SALARYPAYMENTS).Select(
                    item => new SalaryStructureDetail()
                    {
                        Code = item.Name,
                        Description = item.Description,
                        PaymentType = item.RegisterCode,
                        //BranchId=item.BranchId
                        RegisterCode = UTILITY.SALARYPAYMENTS,
                    }).ToList();

                salaryStructureVm.structureEmployerContributionDetail = contributionList
                    .Where(x => x.RegisterCode == UTILITY.EMPLOYERCONTRIBUTION).Select(
                   item => new SalaryStructureDetail()
                   {
                       Code = item.Name,
                       Description = item.Description,
                       PaymentType = item.RegisterCode,
                       RegisterCode = UTILITY.EMPLOYERCONTRIBUTION
                   }).ToList();
                salaryStructureVm.structureEmployeeContributionDetail = contributionList.Where(x => x.RegisterCode == UTILITY.EMPLOYEECONTRIBUTION).Select(
                 item => new SalaryStructureDetail()
                 {
                     Code = item.Name,
                     Description = item.Description,
                     PaymentType = item.RegisterCode,
                     RegisterCode = UTILITY.EMPLOYEECONTRIBUTION
                 }).ToList();
            }
            else
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    var salaryStructure = dbContext.SalaryStructureHeaders
                        .GroupJoin(dbContext.SalaryStructureDetails,
                        a => a.StructureID, b => b.StructureID,
                        (a, b) => new { A = a, B = b.AsEnumerable() })
                        .Where(x => x.A.StructureID == structurId).FirstOrDefault();

                    salaryStructureVm.structureHeader = new SalaryStructureHeader()
                    {
                        Code = salaryStructure.A.Code,
                        CreatedBy = salaryStructure.A.CreatedBy,
                        CreatedOn = salaryStructure.A.CreatedOn,
                        EffectiveDate = salaryStructure.A.EffectiveDate.Value,
                        IsActive = salaryStructure.A.IsActive,
                        ModifiedBy = salaryStructure.A.ModifiedBy,
                        ModifiedOn = salaryStructure.A.ModifiedOn,
                        Remarks = salaryStructure.A.Remarks,
                        StructureID = salaryStructure.A.StructureID,
                        NetAmount = salaryStructure.A.NetAmount,
                        BranchId = salaryStructure.A.BranchId,
                        TotalDeductions = salaryStructure.A.TotalDeductions,
                        TotalGross = salaryStructure.A.TotalGross,
                        Description = salaryStructure.A.Description,
                        EmployeeId = salaryStructure.A.EmployeeId
                    };

                    foreach (SalaryStructureDetail item in salaryStructure.B)
                    {
                        SalaryStructureDetail salaryDetail = new SalaryStructureDetail()
                        {

                            Amount = item.Amount,
                            Code = item.Code,
                            CreatedBy = item.CreatedBy,
                            ComputationCode = item.ComputationCode,
                            CreatedOn = item.CreatedOn,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            ModifiedBy = item.ModifiedBy,
                            ModifiedOn = item.ModifiedOn,
                            RegisterCode = item.RegisterCode,
                            StructureDetailID = item.StructureDetailID,
                            StructureID = item.StructureID,
                            Total = item.Total,
                            PaymentType = item.PaymentType,
                            IsVariablePay = item.IsVariablePay,
                        };
                        if (item.PaymentType == UTILITY.SALARYPAYMENTS)
                            salaryStructureVm.structureSalaryPaymentDetail.Add(salaryDetail);
                        else if (item.PaymentType == UTILITY.EMPLOYERCONTRIBUTION)
                            salaryStructureVm.structureEmployerContributionDetail.Add(salaryDetail);
                        else if (item.PaymentType == UTILITY.EMPLOYEECONTRIBUTION)
                            salaryStructureVm.structureEmployeeContributionDetail.Add(salaryDetail);
                    }

                    var CodeList = new List<string>();
                    var employeeCodeList = new List<string>();
                    var employerCodeList = new List<string>();
                    if (salaryStructureVm != null && salaryStructureVm.structureSalaryPaymentDetail != null
                        && salaryStructureVm.structureSalaryPaymentDetail.Count > 0)
                    {
                        CodeList = salaryStructureVm.structureSalaryPaymentDetail
                                        .Select(x => x.Code)
                                        .ToList();

                        employerCodeList = salaryStructureVm.structureEmployerContributionDetail
                                       .Select(x => x.Code)
                                       .ToList();
                        employeeCodeList = salaryStructureVm.structureEmployeeContributionDetail
                                       .Select(x => x.Code)
                                       .ToList();
                    }

                    var structureCompanyDeductionDetail = contributionList.Where(x => !CodeList.Contains(x.Name.ToUpper())).Select(
                                                   item => new SalaryStructureDetail()
                                                   {
                                                       Code = item.Name,
                                                       Description = item.Description,
                                                       PaymentType = item.RegisterCode
                                                   }).Where(x => x.PaymentType == UTILITY.SALARYPAYMENTS).ToList();//

                    salaryStructureVm.structureSalaryPaymentDetail.AddRange(structureCompanyDeductionDetail);
                    var structureEmployeeDeductionDetail = contributionList.Where(x => !employeeCodeList.Contains(x.Name.ToUpper())).Select(
                                                  item => new SalaryStructureDetail()
                                                  {
                                                      Code = item.Name,
                                                      Description = item.Description,
                                                      PaymentType = item.RegisterCode
                                                  }).Where(x => x.PaymentType == UTILITY.EMPLOYEECONTRIBUTION).ToList();
                    salaryStructureVm.structureEmployeeContributionDetail.AddRange(structureEmployeeDeductionDetail);
                    var structureEmployerDeductionDetail = contributionList.Where(x => !employerCodeList.Contains(x.Name.ToUpper())).Select(
                                                  item => new SalaryStructureDetail()
                                                  {
                                                      Code = item.Name,
                                                      Description = item.Description,
                                                      PaymentType = item.RegisterCode
                                                  }).Where(x => x.PaymentType == UTILITY.EMPLOYERCONTRIBUTION).ToList();
                    salaryStructureVm.structureEmployerContributionDetail.AddRange(structureEmployerDeductionDetail);

                };

            }

            return View(salaryStructureVm);
        }

        [HttpPost]
        public ActionResult SalaryStructure(SalaryStructureVm salaryStructureVm)
        {
            salaryStructureHeaderBO.SaveSalaryStructure(salaryStructureVm);
            return RedirectToAction("SalaryStructureHeaderList");
        }

        [HttpGet]
        public ActionResult DeleteSalaryStructure(int? structurId)
        {
            var result = empSalaryStructureHeaderBO.GetByPropertyFunc(x => x.StructureID == structurId.Value);
            if (result == null)
            {
                salaryStructureHeaderBO.DeleteById(structurId.Value);
            }

            return RedirectToAction("SalaryStructureHeaderList");
        }

        public JsonResult CheckSalaryStructure(int structurId)
        {
            bool isPresent = true;
            var result = empSalaryStructureHeaderBO.GetByPropertyFunc(x => x.StructureID == structurId);
            if (result != null)
            {
                isPresent = true;
            }
            else
                isPresent = false;
            return Json(isPresent, JsonRequestBehavior.AllowGet);
        }
        public ViewResult SalaryStructureHeaderList(int? BranchId = 0, int? page = 1)
        {
            ViewData["RoleCode"] = ROLECODE;
            var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skip = (page.Value - 1) * offset;
            List<SalaryStructureHeader> list = new List<SalaryStructureHeader>();
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
                list = salaryStructureHeaderBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BranchId).OrderBy(x => x.BranchId).ToList();
            else
                list = salaryStructureHeaderBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BRANCHID).ToList();

            var count = list.Count();
            decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));
            HtmlTblVm<SalaryStructureHeader> HtmlTblVm = new HtmlTblVm<SalaryStructureHeader>();
            HtmlTblVm.TableData = list.Skip(skip).Take(offset).ToList();
            HtmlTblVm.TotalRows = count;
            HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
            HtmlTblVm.CurrentPage = page.Value;
            return View(HtmlTblVm);
        }

        [HttpGet]
        public ViewResult EmpSalaryStructure(int employeeId, int structureId = 0)
        {
            using (var dbcntx = new HrDataContext())
            {
                if (structureId == 0)
                {
                    var salarystructureDetail = dbcntx.EmpSalaryStructureHeaders.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
                    if (salarystructureDetail != null)
                        structureId = salarystructureDetail.StructureID;
                }
                var empsalaryobj = dbcntx.EmployeeHeaders
                    .Join(dbcntx.EmployeeWorkDetails,
                    a => a.EmployeeId, b => b.EmployeeId,
                    (a, b) => new { A = a, B = b }).Where(x => x.A.EmployeeId == employeeId)
                    .Select(x => new EmpSalaryStructureVm
                    {
                        EmployeeName = x.A.FirstName + " " + x.A.LastName,
                        Designation = dbcntx.LookUps
                                            .Where(y => y.LookUpID == x.B.DesignationId)
                                            .FirstOrDefault().LookUpDescription,
                        Department = dbcntx.LookUps
                                            .Where(y => y.LookUpID == x.B.DepartmentId)
                                            .FirstOrDefault().LookUpDescription,
                    }).FirstOrDefault();

                empsalaryobj.employeeSalaryStructure = dbcntx.EmpSalaryStructureHeaders
                    .GroupJoin(dbcntx.EmpSalaryStructureDetails,
                    a => new { a.EmployeeId, a.BranchId }, b => new { b.EmployeeId, b.BranchId },
                    (a, b) => new EmployeeSalaryStructure
                    {
                        empSalaryStructureHeader = a,
                        structureCompanyDeductionDetail = b
                    .Where(x => x.PaymentType == UTILITY.SALARYPAYMENTS).ToList()
                    ,
                        structureEmployeeDeductionDetail = b
                    .Where(x => x.PaymentType == UTILITY.EMPLOYEECONTRIBUTION).ToList()
                    })
                    .Where(x => x.empSalaryStructureHeader.EmployeeId == employeeId
                    && x.empSalaryStructureHeader.BranchId == BRANCHID
                    && x.empSalaryStructureHeader.StructureID == structureId
                    && x.empSalaryStructureHeader.IsActive == true)
                    .FirstOrDefault();



                List<string> CodeList = new List<string>();
                List<string> CodeEmpList = new List<string>();
                List<Contribution> contributionList = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BRANCHID).OrderBy(x => x.SortBy).ToList();
                var structureDetail = contributionList.Select(
                                              y => new EmpSalaryStructureDetail()
                                              {
                                                  Code = y.Name,
                                                  EmployeeId = employeeId,
                                                  BranchId = BRANCHID,
                                                  IsActive = false,
                                                  PaymentType = y.RegisterCode
                                              }).ToList();

                if (true)//empsalaryobj.employeeSalaryStructure == null
                {
                    SalaryStructure salaryStructure = null;
                    if (structureId == 0)
                    {
                        salaryStructure = dbcntx.SalaryStructureHeaders
                            .GroupJoin(dbcntx.SalaryStructureDetails,
                            a => a.StructureID, b => b.StructureID,
                            (a, b) => new SalaryStructure
                            {
                                salaryStructureHeader = a,
                                salaryStructureDetail = b.ToList() //b.Where(y => !CodeList.Contains(y.Code)).ToList()
                            }).Where(x => x.salaryStructureHeader.IsActive == true && x.salaryStructureHeader.BranchId == BRANCHID)
                            .FirstOrDefault();
                    }
                    else
                    {
                        salaryStructure = dbcntx.SalaryStructureHeaders
                           .GroupJoin(dbcntx.SalaryStructureDetails,
                           a => a.StructureID, b => b.StructureID,
                           (a, b) => new SalaryStructure
                           {
                               salaryStructureHeader = a,
                               salaryStructureDetail = b.Where(y => !CodeList.Contains(y.Code)).ToList()
                           })
                           .Where(x => x.salaryStructureHeader.StructureID == structureId && x.salaryStructureHeader.IsActive == true
                           && x.salaryStructureHeader.BranchId == BRANCHID)
                           .FirstOrDefault();


                    }
                    if (salaryStructure != null)
                    {
                        var remainingSalStructure = new EmployeeSalaryStructure();

                        remainingSalStructure.empSalaryStructureHeader = new EmpSalaryStructureHeader()
                        {
                            EmployeeId = employeeId,
                            BranchId = BRANCHID,
                            IsActive = salaryStructure.salaryStructureHeader.IsActive,
                            Remarks = salaryStructure.salaryStructureHeader.Remarks,
                            Salary = 0M,
                            StructureID = salaryStructure.salaryStructureHeader.StructureID,

                        };
                        remainingSalStructure.structureCompanyDeductionDetail = salaryStructure.salaryStructureDetail
                            .Select(y => new EmpSalaryStructureDetail
                            {
                                EmployeeId = employeeId,
                                BranchId = BRANCHID,
                                Code = y.Code,
                                Amount = y.Amount,
                                Computation = y.ComputationCode,
                                ContributionRegister = y.RegisterCode,
                                Total = y.Total,
                                IsActive = y.IsActive,
                                PaymentType = y.PaymentType
                            }).Where(x => x.PaymentType == UTILITY.SALARYPAYMENTS).ToList();//
                        remainingSalStructure.structureEmployeeDeductionDetail = salaryStructure.salaryStructureDetail
                        .Select(y => new EmpSalaryStructureDetail
                        {
                            EmployeeId = employeeId,
                            BranchId = BRANCHID,
                            Code = y.Code,
                            Amount = y.Amount,
                            Computation = y.ComputationCode,
                            ContributionRegister = y.RegisterCode,
                            Total = y.Total,
                            IsActive = y.IsActive,
                            PaymentType = y.PaymentType
                        }).Where(x => x.PaymentType == UTILITY.EMPLOYEECONTRIBUTION).ToList();//


                        if (empsalaryobj.employeeSalaryStructure == null)
                        {
                            empsalaryobj.employeeSalaryStructure = remainingSalStructure;
                            empsalaryobj.employeeSalaryStructure.empSalaryStructureHeader = remainingSalStructure.empSalaryStructureHeader;
                            empsalaryobj.employeeSalaryStructure.structureCompanyDeductionDetail = remainingSalStructure.structureCompanyDeductionDetail;
                            empsalaryobj.employeeSalaryStructure.structureEmployeeDeductionDetail = remainingSalStructure.structureEmployeeDeductionDetail;
                            // .Where(x => x.ContributionRegister == UTILITY.EMPLOYEEDEDUCTION).ToList();
                        }

                        CodeList = empsalaryobj.employeeSalaryStructure.structureCompanyDeductionDetail.Select(x => x.Code).ToList();
                        CodeEmpList = empsalaryobj.employeeSalaryStructure.structureEmployeeDeductionDetail.Select(x => x.Code).ToList();

                        empsalaryobj.employeeSalaryStructure.structureCompanyDeductionDetail
                            .AddRange(structureDetail.Where(x => !CodeList.Contains(x.Code.ToUpper()) && x.PaymentType == UTILITY.SALARYPAYMENTS));
                        empsalaryobj.employeeSalaryStructure.structureEmployeeDeductionDetail
                            .AddRange(structureDetail.Where(x => !CodeEmpList.Contains(x.Code.ToUpper()) && x.PaymentType == UTILITY.EMPLOYEECONTRIBUTION));//(remainingSalStructure.empSalaryStructureDetail);
                        if (structureId != 0)
                            empsalaryobj.employeeSalaryStructure.empSalaryStructureHeader.StructureID = structureId;

                    }
                }
                return View(empsalaryobj);
            }


        }

        [HttpPost]
        public ActionResult EmpSalaryStructure(EmpSalaryStructureVm structureVm)
        {
            empSalaryStructureHeaderBO.SaveSalaryStructure(structureVm);
            return RedirectToAction("employeedirectory", "Employee");
        }
        public bool IsSalaryComponentExists(string component, string regCode, int BranchId)
        {
            List<Contribution> List = new List<Contribution>();
            ViewData["RoleCode"] = ROLECODE;
            if (ROLECODE == UTILITY.ROLE_SUPERADMIN)
            {
                List = contributionBO.GetListByProperty(x => (x.Name.ToUpper() == component.ToUpper()) && x.IsActive == true && (x.RegisterCode.ToUpper() == regCode.ToUpper()) && (x.BranchId == BranchId)).ToList();

            }
            else
            {
                List = contributionBO.GetListByProperty(x => (x.Name.ToUpper() == component.ToUpper()) && x.IsActive == true && (x.BranchId == BRANCHID) && (x.RegisterCode.ToUpper() == regCode.ToUpper())).ToList();
            }
            int count = List.Count();
            return (count > 0 ? true : false);

        }

        [HttpGet]
        public ActionResult TravelClaimList(int? page = 1)
        {
            ViewData["RoleCode"] = ROLECODE;
            var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skip = (page.Value - 1) * offset;
            List<TravelClaimHeader> claimHeaderList = travelClaimHeaderBO
               .GetListByProperty(x => x.BranchId == BRANCHID && x.IsActive == true && x.EmployeeId == EMPLOYEEID)
               .OrderByDescending(x => x.CreatedOn).ToList();
            var count = claimHeaderList.Count();
            decimal pagerLength = decimal.Divide(Convert.ToDecimal(count), Convert.ToDecimal(offset));
            HtmlTblVm<TravelClaimHeader> HtmlTblVm = new HtmlTblVm<TravelClaimHeader>();
            HtmlTblVm.TableData = claimHeaderList.Skip(skip).Take(offset).ToList();
            HtmlTblVm.TotalRows = count;
            HtmlTblVm.PageLength = Math.Ceiling(Convert.ToDecimal(pagerLength));
            HtmlTblVm.CurrentPage = page.Value;
            return View(HtmlTblVm);
        }

        [HttpGet]
        public ActionResult TravelClaim(int travelClaimId = 0)
        {
            if (travelClaimId == 0)
            {
                TravelClaimVm travelClaimVm = new TravelClaimVm();
                travelClaimVm.claimHeader = new TravelClaimHeaderVm();
                travelClaimVm.claimHeader.Name = FIRSTNAME;
                travelClaimVm.claimHeader.EmployeeId = EMPLOYEEID;
                //travelClaimVm.claimDetail = new List<TravelClaimDetail>();

                travelClaimVm.claimDetailAccomdationVm = new List<TravelDetailAccomdationVm>()
                {
                    new TravelDetailAccomdationVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailAirfareVm = new List<TravelDetailAirfareVm>()
                {
                    new TravelDetailAirfareVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailFoodLocalVm = new List<TravelDetailFoodLocalVm>()
                {
                    new TravelDetailFoodLocalVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailFoodOverseasVm = new List<TravelDetailFoodOverseasVm>()
                {
                    new TravelDetailFoodOverseasVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailOtherExpensesVm = new List<TravelDetailOtherExpensesVm>()
                {
                    new TravelDetailOtherExpensesVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailTaxiLocalVm = new List<TravelDetailTaxiLocalVm>()
                {
                    new TravelDetailTaxiLocalVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailTaxiOverseasVm = new List<TravelDetailTaxiOverseasVm>()
                {
                    new TravelDetailTaxiOverseasVm()
                    {
                        Receipts = false
                    }
                };
                travelClaimVm.claimDetailVisaVm = new List<TravelDetailVisaVm>() {
                    new TravelDetailVisaVm()
                    {
                        Receipts = false
                    }
                };
                using (var dbcntx = new HrDataContext())
                {
                    //List<LookUp> lookUpList = new List<LookUp>();
                    //lookUpList = dbcntx.LookUps.Where(x => x.LookUpCategory == UTILITY.TRAVELCLAIM && x.IsActive == true).ToList();
                    //foreach (var item in lookUpList)
                    //{
                    //    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    //    {
                    //        Category = item.LookUpDescription,
                    //        Receipts = false
                    //    };
                    //    // travelClaimVm.claimDetail.Add(travelClaimDetail);
                    //}

                    return View(travelClaimVm);
                }
            }
            else
            {
                TravelClaimVm travelClaimNewObj = new TravelClaimVm();
                //travelClaimNewObj.claimHeader = travelClaimHeaderBO
                //   .GetByProperty(x => x.TravelClaimId == travelClaimId);

                // travelClaimNewObj.claimDetail = travelClaimDetailBO.GetListByProperty(x => x.TravelClaimId == travelClaimId).ToList();

                //if (travelClaimNewObj.claimDetail != null && travelClaimNewObj.claimDetail.Count > 0)
                //{
                //    for (var i = 0; i < travelClaimNewObj.claimDetail.Count; i++)
                //    {
                //        if (travelClaimNewObj.claimDetail[i].Amount != null)
                //        {
                //            decimal? amount = travelClaimNewObj.claimDetail[i].Amount;
                //            decimal? exrate = travelClaimNewObj.claimDetail[i].ExchangeRate;
                //            decimal total = (amount.Value * exrate.Value);

                //            travelClaimNewObj.claimDetail[i].TotalInSGD = total;
                //            travelClaimNewObj.claimDetail[i].TravelClaimId = travelClaimNewObj.claimHeader.TravelClaimId;
                //            //travelClaimDetailBO.Add(travelClaimNewObj.claimDetail[i]);
                //        }
                //    }

                //    travelClaimNewObj.claimHeader.GrossTotal = travelClaimNewObj.claimDetail.Sum(x => x.TotalInSGD);


                // }
                //return View(travelClaimNewObj);
                return View();
            }

        }

        [HttpPost]
        public ActionResult TravelClaim(TravelClaimVm travelClaimVm)
        {

            var addNewClaim = Request["addNewClaim"];
            var addOrDelVal = Request["addOrDelete"];
            switch (addNewClaim)
            {
                case UTILITY.AIRFARE:
                    TravelDetailAirfareVm travelDetailAirfareVm = new TravelDetailAirfareVm()
                    {
                        Receipts = false                    
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailAirfareVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailAirfareVm.Add(travelDetailAirfareVm);
                    travelClaimVm.claimHeader.claimDetailAirfareTotal = travelClaimVm.claimDetailAirfareVm.Sum(x => x.TotalInSGD);
                    break;
                case UTILITY.VISA:
                    TravelDetailVisaVm travelDetailVisaVm = new TravelDetailVisaVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailVisaVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailVisaVm.Add(travelDetailVisaVm);
                    break;

                case UTILITY.ACCOMMODATION:
                    TravelDetailAccomdationVm travelDetailAccomdationVm = new TravelDetailAccomdationVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailAccomdationVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailAccomdationVm.Add(travelDetailAccomdationVm);
                    break;
                case UTILITY.TAXILOCAL:
                    TravelDetailTaxiLocalVm travelDetailTaxiLocalVm = new TravelDetailTaxiLocalVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailTaxiLocalVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailTaxiLocalVm.Add(travelDetailTaxiLocalVm);
                    break;

                case UTILITY.TAXIOVERSEAS:

                    TravelDetailTaxiOverseasVm travelDetailTaxiOverseasVm = new TravelDetailTaxiOverseasVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailTaxiOverseasVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailTaxiOverseasVm.Add(travelDetailTaxiOverseasVm);
                    break;

                case UTILITY.FOODBILLSLOCAL:
                    TravelDetailFoodLocalVm travelDetailFoodLocalVm = new TravelDetailFoodLocalVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailFoodLocalVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailFoodLocalVm.Add(travelDetailFoodLocalVm);
                    break;

                case UTILITY.FOODBILLSOVERSEAS:
                    TravelDetailFoodOverseasVm travelDetailFoodOverseasVm = new TravelDetailFoodOverseasVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailFoodOverseasVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailFoodOverseasVm.Add(travelDetailFoodOverseasVm);
                    break;
                case UTILITY.OTHEREXPENSES:
                    TravelDetailOtherExpensesVm travelDetailOtherExpensesVm = new TravelDetailOtherExpensesVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                        travelClaimVm.claimDetailOtherExpensesVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    else
                        travelClaimVm.claimDetailOtherExpensesVm.Add(travelDetailOtherExpensesVm);
                    break;
            }

            //travelClaimHeaderBO.Add(travelClaimVm.claimHeader);
            //if (travelClaimVm.claimDetail != null && travelClaimVm.claimDetail.Count > 0)
            //{
            //    for (var i = 0; i < travelClaimVm.claimDetail.Count; i++)
            //    {
            //        if (travelClaimVm.claimDetail[i].Amount != null)
            //        {
            //            //CalculateAmount(ref travelClaimVm.claimDetail[i]);

            //            var amount = travelClaimVm.claimDetail[i].Amount;
            //            var exrate = travelClaimVm.claimDetail[i].ExchangeRate;
            //            var total = (amount * exrate);

            //            travelClaimVm.claimDetail[i].TotalInSGD = total;
            //            travelClaimVm.claimDetail[i].TravelClaimId = travelClaimVm.claimHeader.TravelClaimId;
            //            //travelClaimDetailBO.Add(travelClaimVm.claimDetail[i]);
            //        }
            //    }


            //}


            //if (!string.IsNullOrEmpty(addOrDelVal))
            //{
            //    travelClaimVm.claimDetail.RemoveAt(Convert.ToInt32(addOrDelVal));
            //}
            //else
            //{
            //    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
            //    {
            //        Receipts = false
            //    };

            //    travelClaimVm.claimDetail.Add(travelClaimDetail);
            //}
            //travelClaimVm.claimHeader.GrossTotal = travelClaimVm.claimDetail.Sum(x => x.TotalInSGD);
            ModelState.Clear();
            return View("TravelClaim", travelClaimVm);
        }
        [HttpPost]
        public ActionResult TravelClaimSave(TravelClaimVm travelClaimVm)
        {

            TravelClaimHeader travelClaimHeader = new TravelClaimHeader()
            {
                BranchId = BRANCHID,
                CountryVisited = travelClaimVm.claimHeader.CountryVisited,
                CreatedBy = SESSIONOBJ.USERID,
                CreatedOn = DateTime.Now,
                EmployeeId = travelClaimVm.claimHeader.EmployeeId,
                FromDate = travelClaimVm.claimHeader.FromDate,
                IsActive = true,
                Name = travelClaimVm.claimHeader.Name,
                NoOfDays = travelClaimVm.claimHeader.NoOfDays,
                Status = UTILITY.TRAVELCLAIMSAVED,
                ToDate = travelClaimVm.claimHeader.ToDate
            };
            travelClaimHeaderBO.Add(travelClaimHeader);

            if (travelClaimVm.claimDetailAirfareVm != null && travelClaimVm.claimDetailAirfareVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailAirfareVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailAirfareVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.AIRFARE,
                        Currency = travelClaimVm.claimDetailAirfareVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailAirfareVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailAirfareVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailAirfareVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailAirfareVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailAirfareVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailAirfareVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailAirfareVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailAccomdationVm != null && travelClaimVm.claimDetailAccomdationVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailAccomdationVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailAccomdationVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.ACCOMMODATION,
                        Currency = travelClaimVm.claimDetailAccomdationVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailAccomdationVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailAccomdationVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailAccomdationVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailAccomdationVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailAccomdationVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailAccomdationVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailAccomdationVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }


            if (travelClaimVm.claimDetailVisaVm != null && travelClaimVm.claimDetailVisaVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailVisaVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailVisaVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.VISA,
                        Currency = travelClaimVm.claimDetailVisaVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailVisaVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailVisaVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailVisaVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailVisaVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailVisaVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailVisaVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailVisaVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailTaxiLocalVm != null && travelClaimVm.claimDetailTaxiLocalVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailTaxiLocalVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailTaxiLocalVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.TAXILOCAL,
                        Currency = travelClaimVm.claimDetailTaxiLocalVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailTaxiLocalVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailTaxiLocalVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailTaxiLocalVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailTaxiLocalVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailTaxiLocalVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailTaxiLocalVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailTaxiLocalVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailTaxiOverseasVm != null && travelClaimVm.claimDetailTaxiOverseasVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailTaxiOverseasVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailTaxiOverseasVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.TAXIOVERSEAS,
                        Currency = travelClaimVm.claimDetailTaxiOverseasVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailTaxiOverseasVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailTaxiOverseasVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailTaxiOverseasVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailTaxiOverseasVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailTaxiOverseasVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailTaxiOverseasVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailTaxiOverseasVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailFoodLocalVm != null && travelClaimVm.claimDetailFoodLocalVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailFoodLocalVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailFoodLocalVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.FOODBILLSLOCAL,
                        Currency = travelClaimVm.claimDetailFoodLocalVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailFoodLocalVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailFoodLocalVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailFoodLocalVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailFoodLocalVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailFoodLocalVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailFoodLocalVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailFoodLocalVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailFoodOverseasVm != null && travelClaimVm.claimDetailFoodOverseasVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailFoodOverseasVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailFoodOverseasVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.FOODBILLSOVERSEAS,
                        Currency = travelClaimVm.claimDetailFoodOverseasVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailFoodOverseasVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailFoodOverseasVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailFoodOverseasVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailFoodOverseasVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailFoodOverseasVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailFoodOverseasVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailFoodOverseasVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            if (travelClaimVm.claimDetailOtherExpensesVm != null && travelClaimVm.claimDetailOtherExpensesVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailOtherExpensesVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        Amount = travelClaimVm.claimDetailOtherExpensesVm[i].Amount,
                        BranchId = 0,
                        Category = UTILITY.OTHEREXPENSES,
                        Currency = travelClaimVm.claimDetailOtherExpensesVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailOtherExpensesVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailOtherExpensesVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailOtherExpensesVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailOtherExpensesVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailOtherExpensesVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailOtherExpensesVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailOtherExpensesVm[i].TravelDate,
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }

            //travelClaimVm.claimHeader.GrossTotal = travelClaimVm.claimDetail.Sum(x => x.TotalInSGD);
            //travelClaimHeaderBO.Add(travelClaimVm.claimHeader);
            //travelClaimDetailBO.DeleteAll(travelClaimVm.claimHeader.TravelClaimId);
            //if (travelClaimVm.claimDetail != null && travelClaimVm.claimDetail.Count > 0)
            //{
            //    for (var i = 0; i < travelClaimVm.claimDetail.Count; i++)
            //    {
            //        if (travelClaimVm.claimDetail[i].Amount != null)
            //        {
            //            var amount = travelClaimVm.claimDetail[i].Amount;
            //            var exrate = travelClaimVm.claimDetail[i].ExchangeRate;
            //            var total = (amount * exrate);

            //            travelClaimVm.claimDetail[i].TotalInSGD = total;
            //            travelClaimVm.claimDetail[i].TravelClaimId = travelClaimVm.claimHeader.TravelClaimId;

            //            travelClaimDetailBO.Add(travelClaimVm.claimDetail[i]);
            //        }
            //    }
            //}


            return RedirectToAction("TravelClaimList", "Payroll");
        }


        public void CalculateAmount(ref TravelClaimDetail item)
        {
            var amount = item.Amount;
            var exrate = item.ExchangeRate;
            var total = (amount * exrate);
            item.TotalInSGD = total;
        }
        public ActionResult DeleteTravelClaim(int detailId = 0, int headerId = 0)
        {

            TravelClaimDetail detail = travelClaimDetailBO.GetByProperty(x => x.TravelClaimDetailId == detailId);
            travelClaimDetailBO.Delete(detail);

            return RedirectToAction("TravelClaim", "Payroll", new { travelClaimId = headerId });

        }

        public ActionResult ProcessTravelClaim()
        {
            var travelobj = travelClaimHeaderBO.GetListByProperty(x => x.BranchId == BRANCHID);
            return View(travelobj);
        }
    }
}