using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class TravelClaimHeaderRepository : IRepository<TravelClaimHeader>
    {
        public void Add(TravelClaimHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    TravelClaimHeader travelClaimHeader = dbContext.TravelClaimHeaders
                        .Where(x => x.TravelClaimId == entity.TravelClaimId).FirstOrDefault();
                    if (travelClaimHeader == null)
                    {
                        //travelClaimHeader.CreatedBy = entity.CreatedBy;
                        //travelClaimHeader.CreatedOn = entity.CreatedOn;
                        dbContext.TravelClaimHeaders.Add(entity);
                    }
                    else
                    {
                        travelClaimHeader.CountryVisited = entity.CountryVisited;
                        //travelClaimHeader.CreatedBy = entity.CreatedBy;
                        //travelClaimHeader.CreatedOn = entity.CreatedOn;
                        travelClaimHeader.FromDate = entity.FromDate;
                        travelClaimHeader.GrossTotal = entity.GrossTotal;
                        travelClaimHeader.ModifiedBy = entity.CreatedBy;
                        travelClaimHeader.ModifiedOn = UTILITY.SINGAPORETIME;
                        travelClaimHeader.Name = entity.Name;
                        travelClaimHeader.NoOfDays = entity.NoOfDays;
                        travelClaimHeader.ToDate = entity.ToDate;
                        travelClaimHeader.Status = entity.Status;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public void Delete(TravelClaimHeader entity)
        //{
        //    try
        //    {
        //        using (HrDataContext dbContext = new HrDataContext())
        //        {
        //            TravelClaimHeader travelClaimHeader = dbContext.TravelClaimHeaders
        //                .Where(x => x.TravelClaimId == entity.TravelClaimId).FirstOrDefault();
        //            travelClaimHeader.IsActive = false;
        //            dbContext.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public void Delete(TravelClaimHeader entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    var record = dbContext.TravelClaimHeaders.Where(x => x.TravelClaimId == id).FirstOrDefault();
                    record.IsActive = false;

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<TravelClaimHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TravelClaimHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimHeaders.Where(x => x.TravelClaimId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal int GetCount(int branchId)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimHeaders.Count();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal IEnumerable<TravelClaimHeader> GetListByProperty(Func<TravelClaimHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimHeaders.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal TravelClaimHeader GetByProperty(Func<TravelClaimHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.TravelClaimHeaders.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}