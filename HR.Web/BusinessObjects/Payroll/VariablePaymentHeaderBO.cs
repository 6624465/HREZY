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
    public class VariablePaymentHeaderBO : BaseBO
    {
        VariablePaymentHeaderRepository variablepaymentheaderrepository = null;
        VariablePaymentDetailBO variablepaymentdetailBo = null;
        public VariablePaymentHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            variablepaymentheaderrepository = new VariablePaymentHeaderRepository();
            variablepaymentdetailBo = new VariablePaymentDetailBO(_sessionObj);
        }

        public void Add(VariablePaymentHeader input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                variablepaymentheaderrepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(VariablePaymentHeader entity)
        {
            try
            {
                variablepaymentheaderrepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public VariablePaymentHeader GetById(int id)
        {
            try
            {
                return variablepaymentheaderrepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<VariablePaymentHeader> GetAll()
        {
            try
            {
                return variablepaymentheaderrepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal int GetCount(int branchId)
        {
            return variablepaymentheaderrepository.GetCount(branchId);
        }
    }
}