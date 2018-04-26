using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;
namespace HR.Web.Services.Payroll
{
    public class TaxAssessmentHeaderRepository : IRepository<TaxAssessmentHeader>
    {
        public void Add(TaxAssessmentHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    TaxAssessmentHeader taxassessmentheader = dbContext.TaxAssessmentHeaders
                        .Where(x => x.HeaderID == entity.HeaderID).FirstOrDefault();
                    if (taxassessmentheader == null)
                    {

                        dbContext.TaxAssessmentHeaders.Add(entity);
                    }
                    else
                    {
                        taxassessmentheader.HeaderID = entity.HeaderID;
                        taxassessmentheader.BranchID = entity.BranchID;
                        taxassessmentheader.AssessmentNo = entity.AssessmentNo;
                        taxassessmentheader.Year = entity.Year;
                        taxassessmentheader.SocialContributionRate = entity.SocialContributionRate;
                        taxassessmentheader.MaximumAmount = entity.MaximumAmount;
                        taxassessmentheader.Status = entity.Status;
                        taxassessmentheader.CreatedBy = entity.CreatedBy;
                        taxassessmentheader.CreatedOn = entity.CreatedOn;
                        taxassessmentheader.ModifiedBy = entity.ModifiedBy;
                        taxassessmentheader.ModifiedOn = entity.ModifiedOn;
                    }

                    dbContext.SaveChanges();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.TaxAssessmentHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TaxAssessmentHeaders.ToList();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TaxAssessmentHeaders.Where(x => x.HeaderID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}