using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryRuleBO : BaseBO
    {
        SalaryRuleHeaderRepository salaryRuleHeaderRepository = null;
        SalaryRuleInputBO salaryRuleInputBO = null;
        SalaryRuleDetailBO salaryRuleDetailBO = null;
        ContributionBO contributionBO = null;
        public SalaryRuleBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryRuleHeaderRepository = new SalaryRuleHeaderRepository();
            salaryRuleInputBO = new SalaryRuleInputBO(sessionObj);
            salaryRuleDetailBO = new SalaryRuleDetailBO(sessionObj);
            contributionBO = new ContributionBO(sessionObj);
        }

        public void Add(SalaryRuleHeader header)
        {
            try
            {
                salaryRuleHeaderRepository.Add(header);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(SalaryRuleHeader header)
        {
            try
            {
                salaryRuleHeaderRepository.Delete(header);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public SalaryRuleHeader GetById(int id)
        {
            try
            {
                return salaryRuleHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IEnumerable<SalaryRuleHeader> GetByAll()
        {
            try
            {
                return salaryRuleHeaderRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void SavePayroll(SalaryRuleHeaderVm salaryRules)
        {
            if (salaryRules.RuleId == 0)
            {
                SalaryRuleHeader salaryRule = new SalaryRuleHeader()
                {
                    Category = salaryRules.Category,
                    Code = salaryRules.Code,
                    IsActive = salaryRules.IsActive,
                    IsOnPayslip = salaryRules.IsOnPayslip,
                    Name = salaryRules.Name,
                    SequenceNo = salaryRules.SequenceNo,
                    
                };
                Add(salaryRule);

                SalaryRuleDetail salaryRuleDetail = new SalaryRuleDetail()
                {
                    AmountType = salaryRules.salaryRuleDetailVm.AmountType,
                    ConditionBased = salaryRules.salaryRuleDetailVm.ConditionBased,
                    ContributionRegister = salaryRules.salaryRuleDetailVm.ContributionRegister,
                    PythonCode = salaryRules.salaryRuleDetailVm.PythonCode,
                    RuleId = salaryRule.RuleId

                };
                salaryRuleDetailBO.Add(salaryRuleDetail);

                Contribution Contribution = new Contribution()
                {
                    Description = salaryRules.contributionVm.Description,
                    IsActive = salaryRules.contributionVm.IsActive,
                    Name = salaryRules.Name
                };
                contributionBO.Add(Contribution);
            }
        }


    }
}