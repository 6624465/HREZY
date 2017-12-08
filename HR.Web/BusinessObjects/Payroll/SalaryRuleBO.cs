using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryRuleBO : BaseBO
    {
        SalaryRuleHeaderRepository salaryRuleHeaderRepository = null;
        public SalaryRuleBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryRuleHeaderRepository = new SalaryRuleHeaderRepository();
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


    }
}