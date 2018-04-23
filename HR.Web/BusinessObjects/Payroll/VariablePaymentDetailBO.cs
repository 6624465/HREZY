using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Services.Payroll;
using HR.Web.Controllers;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.Models;

namespace HR.Web.BusinessObjects.Payroll
{
    public class VariablePaymentDetailBO : BaseBO
    {
        VariablePaymentDetailRepository variablepaymentdetailrepository = null;
        public VariablePaymentDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            variablepaymentdetailrepository = new VariablePaymentDetailRepository();
        }

        public void Add(VariablePaymentDetail input)
        {
            try
            {
                variablepaymentdetailrepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(VariablePaymentDetail entity)
        {
            try
            {
               variablepaymentdetailrepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public VariablePaymentDetail GetById(int id)
        {
            try
            {
                return variablepaymentdetailrepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<VariablePaymentDetail> GetAll()
        {
            try
            {
                return variablepaymentdetailrepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}