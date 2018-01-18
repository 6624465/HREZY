using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class TravelClaimDetailRepository : IRepository<TravelClaimDetail>
    {
        public void Add(TravelClaimDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    TravelClaimDetail travelClaimDetail = dbContext.TravelClaimDetails
                        .Where(x => x.TravelClaimId == entity.TravelClaimId).FirstOrDefault();
                    if (travelClaimDetail == null)
                    {
                        travelClaimDetail.CreatedBy = entity.CreatedBy;
                        travelClaimDetail.CreatedOn = entity.CreatedOn;
                        dbContext.TravelClaimDetails.Add(entity);
                    }
                    else
                    {
                        travelClaimDetail.Amount = entity.Amount;
                        travelClaimDetail.Category = entity.Category;
                        travelClaimDetail.CreatedBy = entity.CreatedBy;
                        travelClaimDetail.CreatedOn = entity.CreatedOn;
                        travelClaimDetail.Currency = entity.Currency;
                        travelClaimDetail.Description = entity.Description;
                        travelClaimDetail.ExchangeRate = entity.ExchangeRate;
                        travelClaimDetail.ModifiedBy = entity.ModifiedBy;
                        travelClaimDetail.ModifiedOn = entity.ModifiedOn;
                        travelClaimDetail.Receipts = entity.Receipts;
                        travelClaimDetail.TotalInSGD = entity.TotalInSGD;
                        travelClaimDetail.TravelClaimId = entity.TravelClaimId;
                        travelClaimDetail.TravelDate = entity.TravelDate;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(TravelClaimDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    TravelClaimDetail travelClaimDetail = dbContext.TravelClaimDetails
                        .Where(x => x.TravelClaimDetailId == entity.TravelClaimDetailId).FirstOrDefault();
                    // travelClaimHeader.IsActive = false;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TravelClaimDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TravelClaimDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimDetails.Where(x => x.TravelClaimDetailId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        internal IEnumerable<TravelClaimDetail> GetListByProperty(Func<TravelClaimDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimDetails.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal TravelClaimDetail GetByProperty(Func<TravelClaimDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimDetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}