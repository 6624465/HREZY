﻿using HR.Web.BusinessObjects.Operation;
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
        EmpSalaryStructureHeaderBO empSalaryStructureHeaderBO = null;
        public PayRollController()
        {
            salaryRuleBO = new SalaryRuleBO(SESSIONOBJ);
            salaryRuleInputBO = new SalaryRuleInputBO(SESSIONOBJ);
            salaryRuleDetailBO = new SalaryRuleDetailBO(SESSIONOBJ);
            contributionBO = new ContributionBO(SESSIONOBJ);
            salaryStructureHeaderBO = new SalaryStructureHeaderBO(SESSIONOBJ);
            salaryStructureDetailBO = new SalaryStructureDetailBO(SESSIONOBJ);
            empSalaryStructureHeaderBO = new EmpSalaryStructureHeaderBO(SESSIONOBJ);
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
                list = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BranchId).OrderBy(x => x.BranchId).ToList();
            else
                list = contributionBO.GetListByProperty(x => x.IsActive == true && x.BranchId == BRANCHID).ToList();

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

            salaryStructureVm.structureEmployeeDeductionDetail = new List<SalaryStructureDetail>();
            salaryStructureVm.structureCompanyDeductionDetail = new List<SalaryStructureDetail>();
            if (structurId == 0)
            {
                salaryStructureVm.structureHeader = new SalaryStructureHeader();

                if (BranchId == 0 && BRANCHID != -1)
                    BranchId = BRANCHID;
                var structureCount = salaryStructureHeaderBO.GetCount(BranchId);

                string headerCode = "EZYPR";
                salaryStructureVm.structureHeader.Code = headerCode + structureCount.ToString("D4");
                if (ROLECODE != UTILITY.ROLE_SUPERADMIN)
                    salaryStructureVm.structureHeader.BranchId = BRANCHID;
                else
                    salaryStructureVm.structureHeader.BranchId = BranchId;
                salaryStructureVm.structureEmployeeDeductionDetail = contributionList.Where(x => x.RegisterCode == UTILITY.EMPLOYEEDEDUCTION).Select(
                    item => new SalaryStructureDetail()
                    {
                        Code = item.Name,
                        Description = item.Description,
                        PaymentType = item.RegisterCode,
                        //BranchId=item.BranchId
                    }).ToList();

                salaryStructureVm.structureCompanyDeductionDetail = contributionList.Where(x => x.RegisterCode == UTILITY.COMPANYDEDUCTION).Select(
                   item => new SalaryStructureDetail()
                   {
                       Code = item.Name,
                       Description = item.Description,
                       PaymentType = item.RegisterCode
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
                            PaymentType = item.PaymentType
                        };
                        if (item.PaymentType == UTILITY.COMPANYDEDUCTION)
                            salaryStructureVm.structureCompanyDeductionDetail.Add(salaryDetail);
                        else
                            salaryStructureVm.structureEmployeeDeductionDetail.Add(salaryDetail);
                    }

                    var CodeList = new List<string>();
                    var employeeCodeList = new List<string>();
                    if (salaryStructureVm != null && salaryStructureVm.structureCompanyDeductionDetail != null && salaryStructureVm.structureCompanyDeductionDetail.Count > 0)
                    {
                        CodeList = salaryStructureVm.structureCompanyDeductionDetail
                                        .Select(x => x.Code)
                                        .ToList();

                        employeeCodeList = salaryStructureVm.structureEmployeeDeductionDetail
                                        .Select(x => x.Code)
                                        .ToList();
                    }

                    var structureCompanyDeductionDetail = contributionList.Where(x => !CodeList.Contains(x.Name)).Select(
                                                   item => new SalaryStructureDetail()
                                                   {
                                                       Code = item.Name,
                                                       Description = item.Description,
                                                       PaymentType = item.RegisterCode
                                                   }).Where(x => x.PaymentType == UTILITY.COMPANYDEDUCTION).ToList();//

                    salaryStructureVm.structureCompanyDeductionDetail.AddRange(structureCompanyDeductionDetail);
                    var structureEmployeeDeductionDetail = contributionList.Where(x => !employeeCodeList.Contains(x.Name)).Select(
                                                  item => new SalaryStructureDetail()
                                                  {
                                                      Code = item.Name,
                                                      Description = item.Description,
                                                      PaymentType = item.RegisterCode
                                                  }).Where(x => x.PaymentType == UTILITY.EMPLOYEEDEDUCTION).ToList();
                    salaryStructureVm.structureEmployeeDeductionDetail.AddRange(structureEmployeeDeductionDetail);

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
            salaryStructureHeaderBO.DeleteById(structurId.Value);
            return RedirectToAction("SalaryStructureHeaderList");
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
                    .Where(x => x.PaymentType == UTILITY.COMPANYDEDUCTION).ToList()
                    ,
                        structureEmployeeDeductionDetail = b
                    .Where(x => x.PaymentType == UTILITY.EMPLOYEEDEDUCTION).ToList()
                    })
                    .Where(x => x.empSalaryStructureHeader.EmployeeId == employeeId
                    && x.empSalaryStructureHeader.BranchId == BRANCHID
                    && x.empSalaryStructureHeader.StructureID == structureId)
                    .FirstOrDefault();



                List<string> CodeList = new List<string>();
                List<string> CodeEmpList = new List<string>();
                List<Contribution> contributionList = contributionBO.GetListByProperty(x => x.IsActive == true).ToList();
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
                            })
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
                           .Where(x => x.salaryStructureHeader.StructureID == structureId)
                           .FirstOrDefault();


                    }
                    var remainingSalStructure = new EmployeeSalaryStructure();

                    remainingSalStructure.empSalaryStructureHeader = new EmpSalaryStructureHeader()
                    {
                        EmployeeId = employeeId,
                        BranchId = BRANCHID,
                        IsActive = salaryStructure.salaryStructureHeader.IsActive,
                        Remarks = salaryStructure.salaryStructureHeader.Remarks,
                        Salary = 0M,
                        StructureID = salaryStructure.salaryStructureHeader.StructureID
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
                        }).Where(x => x.PaymentType == UTILITY.COMPANYDEDUCTION).ToList();//
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
                    }).Where(x => x.PaymentType == UTILITY.EMPLOYEEDEDUCTION).ToList();//


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
                        .AddRange(structureDetail.Where(x => !CodeList.Contains(x.Code) && x.PaymentType == UTILITY.COMPANYDEDUCTION));
                    empsalaryobj.employeeSalaryStructure.structureEmployeeDeductionDetail
                        .AddRange(structureDetail.Where(x => !CodeEmpList.Contains(x.Code) && x.PaymentType == UTILITY.EMPLOYEEDEDUCTION));//(remainingSalStructure.empSalaryStructureDetail);

                    empsalaryobj.employeeSalaryStructure.empSalaryStructureHeader.StructureID = structureId;

                }
                return View(empsalaryobj);
            }


        }

        [HttpPost]
        public ActionResult EmpSalaryStructure(EmpSalaryStructureVm structureVm)
        {
            empSalaryStructureHeaderBO.SaveSalaryStructure(structureVm);
            return RedirectToAction("SalaryStructureHeaderList");
        }
        public bool IsSalaryComponentExists(string component)
        {
            var list = contributionBO.GetListByProperty(x => (x.Name.ToUpper() == component.ToUpper())&& x.IsActive == true).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
        }
    }
}