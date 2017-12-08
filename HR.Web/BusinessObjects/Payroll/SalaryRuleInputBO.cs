using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryRuleInputBO : BaseBO
    {

        SalaryRuleInputRepository salaryRuleInputRepository = null;
        public SalaryRuleInputBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryRuleInputRepository = new SalaryRuleInputRepository();
        }

        public void Add(SalaryRuleInput input)
        {
            try
            {
                salaryRuleInputRepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryRuleInput input)
        {
            try
            {
                salaryRuleInputRepository.Delete(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryRuleInput GetById(int id)
        {
            try
            {
                return salaryRuleInputRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryRuleInput> GetAll()
        {
            try
            {
                return salaryRuleInputRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}