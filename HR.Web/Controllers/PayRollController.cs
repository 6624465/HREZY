                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               using HR.Web.BusinessObjects.Operation;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.BusinessObjects.Security;
using HR.Web.BusinessObjects.LeaveMaster;
using HR.Web.BusinessObjects.LookUpMaster;
using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HR.Web.Helpers;
using System.IO;
using System.Web;
   

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
        UserBO userBo = null;
        EmployeeHeaderBO employeeHeaderBo = null;
        BranchBO branchBO = null;
        EmployeeDocumentDetailBO empDocDetailBO = null;
        LookUpBO lookUpBo = null;
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
            userBo = new UserBO(SESSIONOBJ);
            employeeHeaderBo = new EmployeeHeaderBO(SESSIONOBJ);
            branchBO = new BranchBO(SESSIONOBJ);
            lookUpBo = new LookUpBO(SESSIONOBJ);
            empDocDetailBO = new EmployeeDocumentDetailBO(SESSIONOBJ);
        }
        // GET: PayRoll
        public ActionResult PayRollInfo()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetExRateByCurrencyCode(string currencyCode)
        {
            var lookupObj = lookUpBo
                                .GetByProperty(x => x.LookUpCategory == UTILITY.CURRENCY && x.LookUpDescription == currencyCode);

            if (lookupObj == null)
                lookupObj = new LookUp { LookUpCode = "1" };
                                
            return Json(lookupObj, JsonRequestBehavior.AllowGet);
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
                var travelCount = travelClaimHeaderBO.GetCount(BRANCHID);
                travelCount = travelCount + 1;
                travelClaimVm.claimHeader.ClaimNo = "TRC" + travelCount.ToString("D4");
                var documentTypes = lookUpBo.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_TRAVELCLAIMTYPE && y.LookUpID == UTILITY.TRAVELCLAIMDOCUMENTID)
                                        .Select(x => new TravelClaimDocumentVm
                                        {
                                            DocumentType=x.LookUpID,
                                            DocumentDescription = x.LookUpDescription
                                        }).ToList();
                travelClaimVm.claimDocumentVm = documentTypes;

                var travelClaimDocument = new List<TravelClaimDocumentVm>();
                for(var i = 0; i < 10; i++)
                {
                    var tcdObj = new TravelClaimDocumentVm();
                    tcdObj.DocumentType = 2581 + i;
                    travelClaimDocument.Add(tcdObj);
                }
                travelClaimVm.claimDocumentVm = travelClaimDocument;
                return View(travelClaimVm);

            }
            else
            {
               TravelClaimVm travelclaimobj= gettravelclaimobj(travelClaimId);
                ViewData["documentsPath"] = 
                    "TravlClaimDocs/" + 
                    BRANCHID + "/" + 
                    EMPLOYEEID + "/" + 
                    travelclaimobj.claimHeader.ClaimNo;

                return View("TravelClaim", travelclaimobj);
            }
        }



        [HttpPost]
        public ActionResult TravelClaim(TravelClaimVm travelClaimVm)
        {

            var addNewClaim = Request["addNewClaim"];
            var addOrDelVal = Request["addOrDelete"];
            var travelclaimdetailid = Request["DetailId"];


            ViewData["SelectedTab"] = Request["selectedTab"];
            switch (addNewClaim)
            {
                case UTILITY.AIRFARE:
                    TravelDetailAirfareVm travelDetailAirfareVm = new TravelDetailAirfareVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {
                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailAirfareVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailAirfareVm == null)
                            travelClaimVm.claimDetailAirfareVm = new List<TravelDetailAirfareVm>();

                        travelClaimVm.claimDetailAirfareVm.Add(travelDetailAirfareVm);
                    }

                    travelClaimVm.claimHeader.claimDetailAirfareTotal = travelClaimVm.claimDetailAirfareVm.Sum(x => x.TotalInSGD);
                    break;
                case UTILITY.VISA:
                    TravelDetailVisaVm travelDetailVisaVm = new TravelDetailVisaVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {
                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailVisaVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailVisaVm == null)
                            travelClaimVm.claimDetailVisaVm = new List<TravelDetailVisaVm>();

                        travelClaimVm.claimDetailVisaVm.Add(travelDetailVisaVm);
                    }
                    travelClaimVm.claimHeader.claimDetailVisaTotal = travelClaimVm.claimDetailVisaVm.Sum(x => x.TotalInSGD);
                    break;

                case UTILITY.ACCOMMODATION:
                    TravelDetailAccomdationVm travelDetailAccomdationVm = new TravelDetailAccomdationVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {

                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailAccomdationVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailAccomdationVm == null)
                            travelClaimVm.claimDetailAccomdationVm = new List<TravelDetailAccomdationVm>();
                        travelClaimVm.claimDetailAccomdationVm.Add(travelDetailAccomdationVm);
                    }
                    travelClaimVm.claimHeader.claimDetailAccomdationTotal = travelClaimVm.claimDetailAccomdationVm.Sum(x => x.TotalInSGD);
                    break;
                case UTILITY.TAXILOCAL:
                    TravelDetailTaxiLocalVm travelDetailTaxiLocalVm = new TravelDetailTaxiLocalVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {

                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailTaxiLocalVm.RemoveAt(Convert.ToInt32(addOrDelVal));

                    }
                    else
                    {
                        if (travelClaimVm.claimDetailTaxiLocalVm == null)
                            travelClaimVm.claimDetailTaxiLocalVm = new List<TravelDetailTaxiLocalVm>();
                        travelClaimVm.claimDetailTaxiLocalVm.Add(travelDetailTaxiLocalVm);
                    }
                        
                    travelClaimVm.claimHeader.claimDetailTaxiLocalTotal = travelClaimVm.claimDetailTaxiLocalVm.Sum(x => x.TotalInSGD);
                    break;

                case UTILITY.TAXIOVERSEAS:

                    TravelDetailTaxiOverseasVm travelDetailTaxiOverseasVm = new TravelDetailTaxiOverseasVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {

                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));

                        }
                        travelClaimVm.claimDetailTaxiOverseasVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {

                        if (travelClaimVm.claimDetailTaxiOverseasVm == null)
                            travelClaimVm.claimDetailTaxiOverseasVm = new List<TravelDetailTaxiOverseasVm>();
                        travelClaimVm.claimDetailTaxiOverseasVm.Add(travelDetailTaxiOverseasVm);
                    }
                      
                    travelClaimVm.claimHeader.claimDetailTaxiOverseasTotal = travelClaimVm.claimDetailTaxiOverseasVm.Sum(x => x.TotalInSGD);
                    break;

                case UTILITY.FOODBILLSLOCAL:
                    TravelDetailFoodLocalVm travelDetailFoodLocalVm = new TravelDetailFoodLocalVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {

                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailFoodLocalVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailFoodLocalVm == null)
                            travelClaimVm.claimDetailFoodLocalVm = new List<TravelDetailFoodLocalVm>();
                        travelClaimVm.claimDetailFoodLocalVm.Add(travelDetailFoodLocalVm);
                    }
                       
                    travelClaimVm.claimHeader.claimDetailFoodLocalTotal = travelClaimVm.claimDetailFoodLocalVm.Sum(x => x.TotalInSGD);
                    break;

                case UTILITY.FOODBILLSOVERSEAS:
                    TravelDetailFoodOverseasVm travelDetailFoodOverseasVm = new TravelDetailFoodOverseasVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {

                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));

                        }
                        travelClaimVm.claimDetailFoodOverseasVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailFoodOverseasVm == null)
                            travelClaimVm.claimDetailFoodOverseasVm = new List<TravelDetailFoodOverseasVm>();
                        travelClaimVm.claimDetailFoodOverseasVm.Add(travelDetailFoodOverseasVm);
                    }
                        
                    travelClaimVm.claimHeader.claimDetailFoodOverseasTotal = travelClaimVm.claimDetailFoodOverseasVm.Sum(x => x.TotalInSGD);
                    break;
                case UTILITY.OTHEREXPENSES:
                    TravelDetailOtherExpensesVm travelDetailOtherExpensesVm = new TravelDetailOtherExpensesVm()
                    {
                        Receipts = false
                    };
                    if (!string.IsNullOrEmpty(addOrDelVal))
                    {
                        if (travelclaimdetailid != "0")
                        {
                            travelClaimDetailBO.Delete(Convert.ToInt32(travelclaimdetailid));
                        }
                        travelClaimVm.claimDetailOtherExpensesVm.RemoveAt(Convert.ToInt32(addOrDelVal));
                    }
                    else
                    {
                        if (travelClaimVm.claimDetailOtherExpensesVm == null)
                            travelClaimVm.claimDetailOtherExpensesVm = new List<TravelDetailOtherExpensesVm>();
                        travelClaimVm.claimDetailOtherExpensesVm.Add(travelDetailOtherExpensesVm);
                    }
                       
                    travelClaimVm.claimHeader.claimDetailOtherExpensesTotal = travelClaimVm.claimDetailOtherExpensesVm.Sum(x => x.TotalInSGD);
                    break;
            }


            //travelClaimVm.claimHeader.GrossTotal = travelClaimVm.claimHeader.claimDetailAirfareTotal + travelClaimVm.claimHeader.claimDetailVisaTotal + travelClaimVm.claimHeader.claimDetailAccomdationTotal + travelClaimVm.claimHeader.claimDetailTaxiLocalTotal
            //    + travelClaimVm.claimHeader.claimDetailTaxiOverseasTotal + travelClaimVm.claimHeader.claimDetailFoodLocalTotal + travelClaimVm.claimHeader.claimDetailFoodOverseasTotal + travelClaimVm.claimHeader.claimDetailOtherExpensesTotal;
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
                ClaimNo = travelClaimVm.claimHeader.ClaimNo,
                CreatedBy = SESSIONOBJ.USERID,
                CreatedOn = DateTime.Now,
                EmployeeId = travelClaimVm.claimHeader.EmployeeId,
                FromDate = travelClaimVm.claimHeader.FromDate,
                IsActive = true,
                Name = travelClaimVm.claimHeader.Name,
                NoOfDays = travelClaimVm.claimHeader.NoOfDays,
                Status = UTILITY.TRAVELCLAIMSAVED,
                ToDate = travelClaimVm.claimHeader.ToDate,
                TravelClaimId = travelClaimVm.claimHeader.TravelClaimId,
                GrossTotal = GetGrossTotal(travelClaimVm)

            };
            travelClaimHeaderBO.Add(travelClaimHeader);

            if (travelClaimVm.claimDetailAirfareVm != null && travelClaimVm.claimDetailAirfareVm.Count > 0)
            {
                for (var i = 0; i < travelClaimVm.claimDetailAirfareVm.Count; i++)
                {
                    TravelClaimDetail travelClaimDetail = new TravelClaimDetail()
                    {
                        TravelClaimId = travelClaimHeader.TravelClaimId,
                        TravelClaimDetailId = travelClaimVm.claimDetailAirfareVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailAirfareVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailAccomdationVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailAccomdationVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
                        Category = UTILITY.ACCOMMODATION,
                        Currency = travelClaimVm.claimDetailAccomdationVm[i].Currency,
                        DepartureTime = null,
                        ExchangeRate = travelClaimVm.claimDetailAccomdationVm[i].ExchangeRate,
                        FromDate = travelClaimVm.claimDetailAccomdationVm[i].FromDate == null ? DateTime.Now :
                         travelClaimVm.claimDetailAccomdationVm[i].FromDate,
                        Perticulars = travelClaimVm.claimDetailAccomdationVm[i].Perticulars,
                        Receipts = travelClaimVm.claimDetailAccomdationVm[i].Receipts,
                        TODate = travelClaimVm.claimDetailAccomdationVm[i].ToDate == null ?
                        DateTime.Now : travelClaimVm.claimDetailAccomdationVm[i].ToDate,
                        TotalInSGD = travelClaimVm.claimDetailAccomdationVm[i].TotalInSGD,
                        TravelDate = travelClaimVm.claimDetailAccomdationVm[i].TravelDate,
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailVisaVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailVisaVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailTaxiLocalVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailTaxiLocalVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailTaxiOverseasVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailTaxiOverseasVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailFoodLocalVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailFoodLocalVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailFoodOverseasVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailFoodOverseasVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
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
                        TravelClaimDetailId = travelClaimVm.claimDetailOtherExpensesVm[i].TravelClaimDetailId,
                        Amount = travelClaimVm.claimDetailOtherExpensesVm[i].Amount,
                        BranchId = travelClaimHeader.BranchId,
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
                        ClaimNo = travelClaimHeader.ClaimNo
                    };
                    travelClaimDetailBO.Add(travelClaimDetail);
                }
            }
            foreach (var item in travelClaimVm.claimDocumentVm)
            {
                if (item.Document != null && item.Document.ContentLength > 0)
                {
                    var uidDocument = new EmployeeDocumentDetail
                    {
                        EmployeeId = travelClaimHeader.TravelClaimId,
                        BranchId = SESSIONOBJ.BRANCHID,
                        DocumentType = item.DocumentType,
                        FileName = item.Document.FileName,
                        CreatedBy = SESSIONOBJ.USERID,
                        CreatedOn = UTILITY.SINGAPORETIME
                    };

                    empDocDetailBO.AddClaimDocuments(uidDocument);
                    
                    string path = 
                        System.Web.HttpContext.Current.Server.MapPath("~/TravlClaimDocs/" +
                        BRANCHID + "/" +
                        travelClaimVm.claimHeader.EmployeeId + "/" + 
                        travelClaimVm.claimHeader.ClaimNo + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    item.Document.SaveAs(path + item.Document.FileName);

                }
            }

            return RedirectToAction("TravelClaimList", "Payroll");
        }

        private decimal? GetGrossTotal(TravelClaimVm travelClaimVm)
        {
            decimal? GrossTotal = null;
            if (travelClaimVm.claimDetailAirfareVm != null && travelClaimVm.claimDetailAirfareVm.Count > 0)
                GrossTotal = travelClaimVm.claimDetailAirfareVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailVisaVm != null && travelClaimVm.claimDetailVisaVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailVisaVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailAccomdationVm != null && travelClaimVm.claimDetailAccomdationVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailAccomdationVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailTaxiLocalVm != null && travelClaimVm.claimDetailTaxiLocalVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailTaxiLocalVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailTaxiOverseasVm != null && travelClaimVm.claimDetailTaxiOverseasVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailTaxiOverseasVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailFoodLocalVm != null && travelClaimVm.claimDetailFoodLocalVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailFoodLocalVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailFoodOverseasVm != null && travelClaimVm.claimDetailFoodOverseasVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailFoodOverseasVm.Sum(x => x.TotalInSGD);

            if (travelClaimVm.claimDetailOtherExpensesVm != null && travelClaimVm.claimDetailOtherExpensesVm.Count > 0)
                GrossTotal += travelClaimVm.claimDetailOtherExpensesVm.Sum(x => x.TotalInSGD);

            return GrossTotal;
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
            var travelobj = travelClaimHeaderBO.GetListByProperty(x => x.BranchId == BRANCHID && x.IsActive == true && x.Status == UTILITY.TRAVELCLAIMSUBMITTED).ToList();
            return View(travelobj);
        }

        public ActionResult ApproveTravelClaim(List<SelectedTCVm> selectedTCVm)
        {
            for(var i = 0; i < selectedTCVm.Count; i++)
            {
                if (!selectedTCVm[i].IsChecked)
                    continue;

                var travelclaimid = selectedTCVm[i].TravelClaimId;
                var travelclaim = travelClaimHeaderBO.GetById(travelclaimid);
                travelClaimHeaderBO.ApproveTravelClaim(travelclaim);
                TravelClaimHeader travelobj = travelClaimHeaderBO.GetById(travelclaim.TravelClaimId);
                var employeeid = travelobj.EmployeeId;
                EmployeeHeader empheaderobj = employeeHeaderBo.GetByProperty(x => x.EmployeeId == employeeid);
                var email = empheaderobj.UserEmailId;
                var subject = string.Empty;
                subject = "Travel claim Approval";
                var strbody = string.Empty;
                strbody = "Travel Claim Approved:" + "<BR>" + "Your Travelclaim Expenses are Approved";
                EmailGenerator emailgenerator = new EmailGenerator();
                emailgenerator.ConfigMail(email, true, subject, strbody);
            }            
            return RedirectToAction("ProcessTravelClaim");
        }

        public ActionResult RejectTravelClaim(List<SelectedTCVm> selectedTCVm)
        {
            for (var i = 0; i < selectedTCVm.Count; i++)
            {
                if (!selectedTCVm[i].IsChecked)
                    continue;

                var travelclaimid = selectedTCVm[i].TravelClaimId;
                var travelclaim = travelClaimHeaderBO.GetById(travelclaimid);
                travelClaimHeaderBO.RejectTravelClaim(travelclaim);
                TravelClaimHeader travelobj = travelClaimHeaderBO.GetById(travelclaim.TravelClaimId);
                var employeeid = travelobj.EmployeeId;
                EmployeeHeader empheaderobj = employeeHeaderBo.GetByProperty(x => x.EmployeeId == employeeid);
                var email = empheaderobj.UserEmailId;
                var subject = string.Empty;
                subject = "Travel claim rejected";
                var strbody = string.Empty;
                strbody = "Travel Claim Rejected:" + "<BR>" + "Your Travelclaim Expenses are Rejected";
                EmailGenerator emailgenerator = new EmailGenerator();
                emailgenerator.ConfigMail(email, true, subject, strbody);
            }
            return RedirectToAction("ProcessTravelClaim");
        }
        [HttpPost]
        public ActionResult SubmittravelClaim(TravelClaimHeader TravelClaim)
        {
            var travelclaimid = Request["claimHeader.TravelClaimId"];
            TravelClaim.TravelClaimId = Convert.ToInt32(travelclaimid);
            travelClaimHeaderBO.SubmitTravelClaim(TravelClaim);

            var empObj = employeeHeaderBo.GetByProperty(x => x.EmployeeId == SESSIONOBJ.EMPLOYEEID);
            var employeename = empObj.FirstName + " " + empObj.LastName;
            var employeeemail = empObj.UserEmailId;
            var managerEmail = employeeHeaderBo.GetByProperty(x => x.EmployeeId == empObj.ManagerId).UserEmailId;
            //if (string.IsNullOrWhiteSpace(managerEmail))
            //{
            //    return View("Error");
            //}
            var hrAdminEmail = userBo.GetByProperty(x => x.BranchId == empObj.BranchId && x.RoleCode == UTILITY.ROLE_ADMIN).Email;

            var subject = string.Empty;
            subject = "Travel claim submission";
            var strbody = string.Empty;



            strbody = employeename + " " + "submitted travel expenses claim.Please review and approve.";
            var body = string.Empty;
            body = "Dear" + employeename + "<BR>";
            body += "Your expenses claim submitted successfully.";

            EmailGenerator emailgenerator = new EmailGenerator();
            emailgenerator.ConfigMail(managerEmail, hrAdminEmail, true, subject, strbody);
            emailgenerator.ConfigMail(employeeemail, true, subject, body);



            return RedirectToAction("TravelClaimList");
        }

        public ActionResult DeleteTravelRecord(int travelclaimid)
        {
            travelClaimHeaderBO.Delete(travelclaimid);
            return RedirectToAction("TravelClaimList");
        }
        public PartialViewResult PreviewTravelClaim(int TravelClaimId)
        {

            TravelClaimVm travelobj = gettravelclaimobj(TravelClaimId);
            ViewData["documentsPath"] =
                   "TravlClaimDocs/" +
                   BRANCHID + "/" +
                   EMPLOYEEID + "/" +
                  travelobj.claimHeader.ClaimNo;
            return PartialView("_PreviewTravelClaim", travelobj);
        }
        private TravelClaimVm gettravelclaimobj(int travelClaimId)
        {
            TravelClaimVm travelClaimNewObj = new TravelClaimVm();
            TravelClaimHeader header = travelClaimHeaderBO
             .GetByProperty(x => x.TravelClaimId == travelClaimId);
            travelClaimNewObj.claimHeader = new TravelClaimHeaderVm()
            {
                BranchId = header.BranchId,
                ClaimNo = header.ClaimNo,
                CountryVisited = header.CountryVisited,
                EmployeeId = header.EmployeeId,
                FromDate = header.FromDate,
                GrossTotal = header.GrossTotal,
                Name = header.Name,
                NoOfDays = header.NoOfDays,
                Status = header.Status,
                ToDate = header.ToDate,
                TravelClaimId = header.TravelClaimId
            };


            var details = travelClaimDetailBO.GetListByProperty(x => x.TravelClaimId == travelClaimId).ToList();
            travelClaimNewObj.claimDetailAirfareVm = new List<TravelDetailAirfareVm>();
            travelClaimNewObj.claimDetailAccomdationVm = new List<TravelDetailAccomdationVm>();
            travelClaimNewObj.claimDetailVisaVm = new List<TravelDetailVisaVm>();
            travelClaimNewObj.claimDetailTaxiLocalVm = new List<TravelDetailTaxiLocalVm>();
            travelClaimNewObj.claimDetailTaxiOverseasVm = new List<TravelDetailTaxiOverseasVm>();
            travelClaimNewObj.claimDetailFoodLocalVm = new List<TravelDetailFoodLocalVm>();
            travelClaimNewObj.claimDetailFoodOverseasVm = new List<TravelDetailFoodOverseasVm>();
            travelClaimNewObj.claimDetailOtherExpensesVm = new List<TravelDetailOtherExpensesVm>();
            

            foreach (var item in details)
            {
                switch (item.Category)
                {
                    case UTILITY.AIRFARE:
                        TravelDetailAirfareVm travelClaimDetailAirfare = new TravelDetailAirfareVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailAirfareVm.Add(travelClaimDetailAirfare);
                        break;


                    case UTILITY.ACCOMMODATION:
                        TravelDetailAccomdationVm travelClaimDetailAccomdation = new TravelDetailAccomdationVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailAccomdationVm.Add(travelClaimDetailAccomdation);
                        break;

                    case UTILITY.VISA:
                        TravelDetailVisaVm travelClaimDetailVisa = new TravelDetailVisaVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailVisaVm.Add(travelClaimDetailVisa);
                        break;

                    case UTILITY.TAXILOCAL:
                        TravelDetailTaxiLocalVm travelClaimDetailTaxiLocal = new TravelDetailTaxiLocalVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailTaxiLocalVm.Add(travelClaimDetailTaxiLocal);
                        break;

                    case UTILITY.TAXIOVERSEAS:
                        TravelDetailTaxiOverseasVm travelClaimDetailTaxiOverseas = new TravelDetailTaxiOverseasVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailTaxiOverseasVm.Add(travelClaimDetailTaxiOverseas);
                        break;

                    case UTILITY.FOODBILLSLOCAL:
                        TravelDetailFoodLocalVm travelClaimDetailFoodLocal = new TravelDetailFoodLocalVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailFoodLocalVm.Add(travelClaimDetailFoodLocal);
                        break;

                    case UTILITY.FOODBILLSOVERSEAS:
                        TravelDetailFoodOverseasVm travelClaimDetailFoodOverseas = new TravelDetailFoodOverseasVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            Amount = item.Amount,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailFoodOverseasVm.Add(travelClaimDetailFoodOverseas);
                        break;

                    case UTILITY.OTHEREXPENSES:
                        TravelDetailOtherExpensesVm travelClaimDetailOtherExpenses = new TravelDetailOtherExpensesVm()
                        {
                            TravelClaimId = item.TravelClaimId,
                            Amount = item.Amount,
                            TravelClaimDetailId = item.TravelClaimDetailId,
                            BranchId = item.BranchId,
                            Category = UTILITY.AIRFARE,
                            Currency = item.Currency,
                            DepartureTime = null,
                            ExchangeRate = item.ExchangeRate,
                            FromDate = item.FromDate,
                            Perticulars = item.Perticulars,
                            Receipts = item.Receipts,
                            ToDate = item.TODate,
                            TotalInSGD = item.TotalInSGD,
                            TravelDate = item.TravelDate,
                            ClaimNo = item.ClaimNo
                        };
                        travelClaimNewObj.claimDetailOtherExpensesVm.Add(travelClaimDetailOtherExpenses);
                        break;
                }

            }
            travelClaimNewObj.claimHeader.claimDetailAirfareTotal = travelClaimNewObj.claimDetailAirfareVm
                .Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailVisaTotal = travelClaimNewObj.claimDetailVisaVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailAccomdationTotal = travelClaimNewObj.claimDetailAccomdationVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailTaxiLocalTotal = travelClaimNewObj.claimDetailTaxiLocalVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailTaxiOverseasTotal = travelClaimNewObj.claimDetailTaxiOverseasVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailFoodLocalTotal = travelClaimNewObj.claimDetailFoodLocalVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailFoodOverseasTotal = travelClaimNewObj.claimDetailFoodOverseasVm.Sum(x => x.TotalInSGD);
            travelClaimNewObj.claimHeader.claimDetailOtherExpensesTotal = travelClaimNewObj.claimDetailOtherExpensesVm.Sum(x => x.TotalInSGD);
            List<EmployeeDocumentDetail> empDocumentDetList = new List<EmployeeDocumentDetail>();

           empDocumentDetList = empDocDetailBO.
                GetListByProperty(x => x.EmployeeId == travelClaimId && x.BranchId == BRANCHID).ToList();

            var codeList = empDocumentDetList.Select(x => x.DocumentType).ToList();

            List<TravelClaimDocumentVm> docVmList = new List<TravelClaimDocumentVm>();
            if (empDocumentDetList.Count > 0)
            {
                //Int16 loopCount = 0;
                //foreach (EmployeeDocumentDetail item in empDocumentDetList)
                //{
                //    TravelClaimDocumentVm docVm = new TravelClaimDocumentVm()
                //    {
                //        DocumentType = item.DocumentType,
                //        DocumentDescription = "",
                //        fileName = item.FileName,
                //        DocumentDetailId = item.DocumentDetailID
                //    };
                //    docVmList.Add(docVm);
                //    loopCount++;
                //}

                for(var i = 0; i < 10; i++)
                {
                    var documentType = 2581 + i;
                    var tdcObj = empDocumentDetList
                            .Where(x => x.DocumentType == documentType)
                            .Select(x => new TravelClaimDocumentVm {
                                DocumentType = documentType,
                                DocumentDescription = "",
                                fileName = x.FileName,
                                DocumentDetailId = x.DocumentDetailID,
                                EmployeeId=x.EmployeeId
                            })
                            .FirstOrDefault();

                    if(tdcObj == null)
                    {
                        tdcObj = new TravelClaimDocumentVm
                        {
                            DocumentType = documentType,
                            DocumentDescription = "",
                            fileName = ""
                        };
                    }


                    docVmList.Add(tdcObj);
                }


                travelClaimNewObj.claimDocumentVm = docVmList;
                //travelClaimNewObj.claimDocumentVm = lookUpBo.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_TRAVELCLAIMTYPE && y.LookUpID==UTILITY.TRAVELCLAIMDOCUMENTID)
                //    .Select(y => new TravelClaimDocumentVm
                //    {
                //        DocumentType = y.LookUpID,
                //        DocumentDescription = y.LookUpDescription
                //    }).Where(x => !codeList.Contains(x.DocumentType)).ToList();
                //travelClaimNewObj.claimDocumentVm.AddRange(docVmList);
            }
            else
            {
                //travelClaimNewObj.claimDocumentVm = lookUpBo.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_TRAVELCLAIMTYPE &&
                //y.LookUpID == UTILITY.TRAVELCLAIMDOCUMENTID)
                //    .Select(y => new TravelClaimDocumentVm
                //    {
                //        DocumentType = y.LookUpID,
                //        DocumentDescription = y.LookUpDescription
                //    }).ToList();
                for(var i = 0; i < 10; i++)
                {
                    var tdcObj = new TravelClaimDocumentVm();
                    docVmList.Add(tdcObj);
                }

                travelClaimNewObj.claimDocumentVm = docVmList;
            }


            return travelClaimNewObj;

        }
        public ActionResult DeleteTravelclaimDocs(int docdetailid, int travelclaimid, string claimNo)
        {
            EmployeeDocumentDetail empdocdetail = empDocDetailBO.GetById(docdetailid);
            string path =
                        System.Web.HttpContext.Current.Server.MapPath("~/TravlClaimDocs/" +
                        BRANCHID + "/" +
                        EMPLOYEEID + "/" +
                        claimNo + "/");

            FileInfo file = new FileInfo(path + empdocdetail.FileName);
            if (file.Exists)
                file.Delete();

            empDocDetailBO.Delete(docdetailid);
            return RedirectToAction("TravelClaim","PayRoll",new { travelClaimId = travelclaimid });
        }
    }
}
   
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           