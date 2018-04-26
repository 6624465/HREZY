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
    public class TaxAssessmentDetailBO : BaseBO
    {
        TaxAssessmentDetailRepository taxassessmentdetailrepository = null;
        public TaxAssessmentDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            taxassessmentdetailrepository = new TaxAssessmentDetailRepository();
        }

        public void Add(TaxAssessmentDetail input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                taxassessmentdetailrepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(TaxAssessmentDetail entity)
        {
            try
            {
                taxassessmentdetailrepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TaxAssessmentDetail GetById(int id)
        {
            try
            {
                return taxassessmentdetailrepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TaxAssessmentDetail> GetAll()
        {
            try
            {
                return taxassessmentdetailrepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}