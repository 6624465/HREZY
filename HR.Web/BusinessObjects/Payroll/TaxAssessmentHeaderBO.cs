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
    public class TaxAssessmentHeaderBO : BaseBO
    {
        TaxAssessmentHeaderRepository taxassessmentheaderrepository = null;
        TaxAssessmentHeaderBO taxassessmentheaderbo = null;
        public TaxAssessmentHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            taxassessmentheaderrepository = new TaxAssessmentHeaderRepository();
        }

        public void Add(TaxAssessmentHeader input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                taxassessmentheaderrepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(TaxAssessmentHeader entity)
        {
            try
            {
                taxassessmentheaderrepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TaxAssessmentHeader GetById(int id)
        {
            try
            {
                return taxassessmentheaderrepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TaxAssessmentHeader GetByEmployeeId(int id)
        {
            try
            {
                return taxassessmentheaderrepository.GetByEmployeeId(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TaxAssessmentHeader> GetAll()
        {
            try
            {
                return taxassessmentheaderrepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}