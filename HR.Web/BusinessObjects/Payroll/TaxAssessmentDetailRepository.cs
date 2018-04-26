using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.Services.Payroll
{
    public class TaxAssessmentDetailRepository : IRepository<TaxAssessmentDetail>
    {
        public void Add(TaxAssessmentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    TaxAssessmentDetail taxassessmentdetail = dbContext.TaxAssessmentDetails
                        .Where(x => x.ID == entity.ID).FirstOrDefault();
                    if (taxassessmentdetail == null)
                    {
                        dbContext.TaxAssessmentDetails.Add(entity);
                    }
                    else
                    {
                        taxassessmentdetail.ID = entity.ID;
                        taxassessmentdetail.HeaderID = entity.HeaderID;
                        taxassessmentdetail.SalaryFrom = entity.SalaryFrom;
                        taxassessmentdetail.SalaryTo = entity.SalaryTo;
                        taxassessmentdetail.Rate = entity.Rate;
                        taxassessmentdetail.CreatedBy = entity.CreatedBy;
                        taxassessmentdetail.CreatedOn = entity.CreatedOn;
                        taxassessmentdetail.ModifiedBy = entity.ModifiedBy;
                        taxassessmentdetail.ModifiedOn = entity.ModifiedOn;


                    }

                    dbContext.SaveChanges();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.TaxAssessmentDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TaxAssessmentDetails.ToList();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TaxAssessmentDetails.Where(x => x.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}