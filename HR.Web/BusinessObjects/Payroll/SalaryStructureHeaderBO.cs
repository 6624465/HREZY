using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryStructureHeaderBO : BaseBO
    {
        SalaryStructureHeaderRepository salaryStructureHeaderRepository = null;
        public SalaryStructureHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryStructureHeaderRepository = new SalaryStructureHeaderRepository();
        }


        public void Add(SalaryStructureHeader input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                salaryStructureHeaderRepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryStructureHeader entity)
        {
            try
            {
                salaryStructureHeaderRepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryStructureHeader GetById(int id)
        {
            try
            {
                return salaryStructureHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureHeader> GetAll()
        {
            try
            {
                return salaryStructureHeaderRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public SalaryStructureHeader GetByProperty(Func<SalaryStructureHeader, bool> predicate)
        {
            try
            {
                return salaryStructureHeaderRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureHeader> GetListByProperty(Func<SalaryStructureHeader,bool> predicate)
        {
            try
            {
                return salaryStructureHeaderRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
