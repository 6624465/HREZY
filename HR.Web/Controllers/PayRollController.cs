using HR.Web.BusinessObjects.Operation;
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
        public ActionResult ContributionRegisterList()
        {

            var list = contributionBO.GetListByProperty(x => x.IsActive == true);
            return View(list);
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

        [HttpPost]
        public JsonResult ContributionDelete(int contributionid)
        {
            contributionBO.Delete(contributionid);
            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SalaryStructure(int structurId = 0)
        {
            SalaryStructureVm salaryStructureVm = new SalaryStructureVm();            

            List<Contribution> contributionList = contributionBO.GetListByProperty(x => x.IsActive == true).ToList();
            salaryStructureVm.structureDetail = new List<SalaryStructureDetail>();
            if (structurId == 0)
            {
                salaryStructureVm.structureHeader = new SalaryStructureHeader();
                foreach (Contribution item in contributionList)
                {
                    SalaryStructureDetail salaryStructureDetail = new SalaryStructureDetail()
                    {
                        Code = item.Name,
                        Description = item.Description
                    };
                    salaryStructureVm.structureDetail.Add(salaryStructureDetail);
                }
            }
            else
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    //var salaryStructure = dbContext.SalaryStructureHeaders
                    //    .GroupJoin(dbContext.SalaryStructureDetails,
                    //    a => a.StructureID, b => b.StructureID,
                    //    (a, b) => new { A = a, B = b.AsEnumerable() })
                    //    .Where(x => x.A.StructureID == structurId)
                    //    .FirstOrDefault();


                    salaryStructureVm = dbContext.SalaryStructureHeaders
                        .GroupJoin(dbContext.SalaryStructureDetails,
                        a => a.StructureID, b => b.StructureID,
                        (a, b) => new { A = a, B = b.AsEnumerable() })
                        .Where(x => x.A.StructureID == structurId)
                        .Select(x => new SalaryStructureVm{
                            structureHeader = new SalaryStructureHeader()
                            {
                                Code = x.A.Code,
                                CreatedBy = x.A.CreatedBy,
                                CreatedOn = x.A.CreatedOn,
                                EffectiveDate = x.A.EffectiveDate.Value,
                                IsActive = x.A.IsActive,
                                ModifiedBy = x.A.ModifiedBy,
                                ModifiedOn = x.A.ModifiedOn,
                                Remarks = x.A.Remarks,
                                StructureID = x.A.StructureID,
                                NetAmount = x.A.NetAmount
                            },
                            structureDetail = x.B.Select(item => new SalaryStructureDetail()
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
                                Total = item.Total
                            }).ToList()
                        }).FirstOrDefault();

                    var CodeList = new List<string>();
                    if (salaryStructureVm != null && salaryStructureVm.structureDetail != null && salaryStructureVm.structureDetail.Count > 0)
                    {
                        CodeList = salaryStructureVm.structureDetail
                                        .Select(x => x.Code)
                                        .ToList();
                    }

                    foreach (Contribution item in contributionList.Where(x => !CodeList.Contains(x.Name)))
                    {
                        SalaryStructureDetail salaryStructureDetail = new SalaryStructureDetail()
                        {
                            Code = item.Name,
                            Description = item.Description
                        };
                        salaryStructureVm.structureDetail.Add(salaryStructureDetail);
                    }                 

                    
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
        public ViewResult SalaryStructureHeaderList(int? page = 1)
        {
            var offset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skip = (page.Value - 1) * offset;
            var list = salaryStructureHeaderBO.GetListByProperty(x => x.IsActive == true);
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
                var empsalaryobj = dbcntx.EmployeeHeaders
                    .Join(dbcntx.EmployeeWorkDetails,
                    a => a.EmployeeId, b => b.EmployeeId,
                    (a, b) => new { A = a, B = b })
                    .Select(x => new EmpSalaryStructureVm
                    {
                        EmployeeName = x.A.FirstName + "" + x.A.LastName,
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
                    (a, b) => new EmployeeSalaryStructure { empSalaryStructureHeader = a, empSalaryStructureDetail = b.ToList() }).FirstOrDefault();

                List<string> CodeList = new List<string>();
                if(empsalaryobj != null && empsalaryobj.employeeSalaryStructure != null && empsalaryobj.employeeSalaryStructure.empSalaryStructureDetail.Count > 0)
                {
                    CodeList = empsalaryobj.employeeSalaryStructure
                                    .empSalaryStructureDetail
                                    .Select(x => x.Code)
                                    .ToList();
                }
                

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
                                salaryStructureDetail = b.Where(y => !CodeList.Contains(y.Code)).ToList()
                            })
                            //.Where(x => x.salaryStructureDetail.Where(y => !CodeList.Contains(y.Code)))
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
                    var remainingSalStructure = new EmployeeSalaryStructure
                    {
                        empSalaryStructureHeader = new EmpSalaryStructureHeader
                        {
                            EmployeeId = employeeId,
                            BranchId = BRANCHID,
                            Salary = 0M,
                            Remarks = "",
                            StructureID = structureId
                        },
                        empSalaryStructureDetail = salaryStructure.salaryStructureDetail.Select(y => new EmpSalaryStructureDetail
                        {
                            EmployeeId = employeeId,
                            BranchId = BRANCHID,
                            Code = y.Code,
                            Amount = y.Amount,
                            Computation = y.ComputationCode,
                            ContributionRegister = y.RegisterCode,
                            Total = y.Total,
                            IsActive = y.IsActive,
                        }).ToList()
                    };

                    empsalaryobj.employeeSalaryStructure.empSalaryStructureDetail
                        .AddRange(remainingSalStructure.empSalaryStructureDetail);


                }


                return View(empsalaryobj);
            }


        }

        [HttpPost]
        public ViewResult EmpSalaryStructure(EmpSalaryStructureVm structureVm)
        {
            empSalaryStructureHeaderBO.SaveSalaryStructure(structureVm);
            return View();
        }
    }
}