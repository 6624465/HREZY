using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryRuleDetailBO : BaseBO
    {
        SalaryRuleDetailRepository salaryRuleDetailRepository = null;
        public SalaryRuleDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryRuleDetailRepository = new SalaryRuleDetailRepository();
        }

        public void Add(SalaryRuleDetail detail)
        {
            try
            {
                salaryRuleDetailRepository.Add(detail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryRuleDetail detail)
        {
            try
            {
                salaryRuleDetailRepository.Delete(detail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryRuleDetail GetById(int id)
        {
            try
            {
                return salaryRuleDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryRuleDetail> GetAll()
        {
            try
            {
                return salaryRuleDetailRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}