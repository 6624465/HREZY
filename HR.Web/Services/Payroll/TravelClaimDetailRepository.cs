using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class TravelClaimDetailRepository : IRepository<TravelClaimDetail>
    {
        public void Add(TravelClaimDetail entity)
        {

            using (HrDataContext dbContext = new HrDataContext())
            {
                using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        

                        TravelClaimDetail travelClaimDetail = dbContext.TravelClaimDetails
                        .Where(x => x.TravelClaimDetailId == entity.TravelClaimDetailId).FirstOrDefault();
                        if (travelClaimDetail == null)
                        {
                            //travelClaimDetail.CreatedBy = entity.CreatedBy;
                            //travelClaimDetail.CreatedOn = entity.CreatedOn;
                            entity.CurrencyCode = entity.Currency;
                            dbContext.TravelClaimDetails.Add(entity);
                        }
                        else
                        {
                            travelClaimDetail.Amount = entity.Amount;
                            travelClaimDetail.Category = entity.Category;
                            //travelClaimDetail.CreatedBy = entity.CreatedBy;
                            //travelClaimDetail.CreatedOn = entity.CreatedOn;
                            travelClaimDetail.Currency = entity.Currency;
                            travelClaimDetail.Perticulars = entity.Perticulars;
                            travelClaimDetail.ExchangeRate = entity.ExchangeRate;
                            travelClaimDetail.ModifiedBy = entity.CreatedBy;
                            travelClaimDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                            travelClaimDetail.Receipts = entity.Receipts;
                            travelClaimDetail.TotalInSGD = entity.TotalInSGD;
                            travelClaimDetail.TravelClaimId = entity.TravelClaimId;
                            travelClaimDetail.TravelDate = entity.TravelDate;
                            travelClaimDetail.FromDate = entity.FromDate;
                            travelClaimDetail.TODate = entity.TODate;
                            travelClaimDetail.DepartureTime = entity.DepartureTime;
                            travelClaimDetail.CurrencyCode = entity.Currency;

                        }
                        dbContext.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
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
                    dbContext.TravelClaimDetails.Remove(travelClaimDetail);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Delete(int  id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    TravelClaimDetail travelClaimDetail = dbContext.TravelClaimDetails
                        .Where(x => x.TravelClaimDetailId == id).FirstOrDefault();
                    // travelClaimHeader.IsActive = false;
                    dbContext.TravelClaimDetails.Remove(travelClaimDetail);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteAll(int? TravelClaimId)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    var travelClaimDetails = dbContext.TravelClaimDetails
                        .Where(x => x.TravelClaimId == TravelClaimId).ToList();
                    foreach (var item in travelClaimDetails)
                    {
                        dbContext.TravelClaimDetails.Remove(item);
                        dbContext.SaveChanges();
                    }
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