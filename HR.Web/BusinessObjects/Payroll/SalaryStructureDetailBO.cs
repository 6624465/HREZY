using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryStructureDetailBO : BaseBO
    {
        SalaryStructureDetailRepository salaryStructureDetailRepository = null;
        public SalaryStructureDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryStructureDetailRepository = new SalaryStructureDetailRepository();
        }


        public void Add(SalaryStructureDetail entity)
        {
            try
            {
                entity.CreatedBy = sessionObj.USERID;
                entity.CreatedOn = UTILITY.SINGAPORETIME;
                //Commented by SKD
                //if (entity.PaymentType == UTILITY.SALARYPAYMENTS)
                //{
                //    entity.ComputationCode = entity.ComputationCode;
                //}
                //else
                //{
                //    entity.ComputationCode = UTILITY.AMOUNT;
                //}
                salaryStructureDetailRepository.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryStructureDetail entity)
        {
            try
            {
                salaryStructureDetailRepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryStructureDetail GetById(int id)
        {
            try
            {
                return salaryStructureDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureDetail> GetAll()
        {
            try
            {
                return salaryStructureDetailRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public SalaryStructureDetail GetByProperty(Func<SalaryStructureDetail, bool> predicate)
        {
            try
            {
                return salaryStructureDetailRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureDetail> GetListByProperty(Func<SalaryStructureDetail,bool> predicate)
        {
            try
            {
                return salaryStructureDetailRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
