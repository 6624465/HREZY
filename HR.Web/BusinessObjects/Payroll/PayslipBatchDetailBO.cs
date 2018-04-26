using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Services.Payroll;
using HR.Web.Models;
using HR.Web.Controllers;


namespace HR.Web.BusinessObjects.Payroll
{

    public class PayslipBatchDetailBO : BaseBO
    {
        PayslipBatchDetailRepository payslipbatchdetailrepository = null;
        public PayslipBatchDetailBO(SessionObj _sessionobj)
        {
            sessionObj = _sessionobj;
            payslipbatchdetailrepository = new PayslipBatchDetailRepository();
        }
        public void Add(PayslipBatchDetail entity)
        {
            try
            {
                entity.CreatedBy = sessionObj.USERID;
                entity.CreatedOn = UTILITY.SINGAPORETIME;
                //entity.BranchId = sessionObj.BRANCHID;
                //entity.EmployeeId = sessionObj.EMPLOYEEID;
                payslipbatchdetailrepository.Add(entity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(PayslipBatchDetail entity)
        {
            try
            {
                payslipbatchdetailrepository.Delete(entity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PayslipBatchDetail> GetAll()
        {
            try
            {
                return payslipbatchdetailrepository.GetAll();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public PayslipBatchDetail GetById(int id)
        {
            try
            {
                return payslipbatchdetailrepository.GetById(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public PayslipBatchDetail GetByProperty(Func<PayslipBatchDetail,bool> predicate)
        {
            try
            {
                return payslipbatchdetailrepository.GetByProperty(predicate);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PayslipBatchDetail> GetListByProperty(Func<PayslipBatchDetail,bool> predicate)
        {
            try
            {
                return payslipbatchdetailrepository.GetListByProperty(predicate);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}