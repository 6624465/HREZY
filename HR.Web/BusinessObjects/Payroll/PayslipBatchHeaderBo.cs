using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;

namespace HR.Web.BusinessObjects.Payroll
{
    public class PayslipBatchHeaderBo:BaseBO
    {
        PayslipBatchHeaderRepository PayslipBatchHeaderRepository = null;
        public PayslipBatchHeaderBo(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            PayslipBatchHeaderRepository = new PayslipBatchHeaderRepository();
        }
        public void Add(PayslipBatchHeader input)
        {
            try
            {
                input.CreateBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                input.BranchId = sessionObj.BRANCHID;
                PayslipBatchHeaderRepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Delete(PayslipBatchHeader entity)
        {
            try
            {
                PayslipBatchHeaderRepository.Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PayslipBatchHeader GetById(int id)
        {
            try
            {
                return PayslipBatchHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<PayslipBatchHeader> GetAll()
        {
            try
            {
                return PayslipBatchHeaderRepository.GetAll();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public PayslipBatchHeader GetByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                return PayslipBatchHeaderRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PayslipBatchHeader> GetListByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                return PayslipBatchHeaderRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.Data.DataTable GeneratePayslip(Int16 BranchId, int CurrentMonth, int CurrentYear)
        {
            return PayslipBatchHeaderRepository.GeneratePayslip(BranchId, CurrentMonth, CurrentYear);
        }

    }
}