using HR.Web.Controllers;
using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services
{
    public class WeekendPolicyService : IRepository<WeekendPolicy>
    {
        SessionObj sessionobj;
        public WeekendPolicyService(SessionObj _sessionobj)
        {
            sessionobj = _sessionobj;
        }
        public void Add(WeekendPolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    WeekendPolicy weekendPolicy = dbContext.WeekendPolicies
                                        .Where(x => x.BranchId == entity.BranchId).FirstOrDefault();
                    if (weekendPolicy == null)
                        dbContext.WeekendPolicies.Add(entity);
                    else
                    {
                        weekendPolicy.CreateBy = entity.CreateBy;
                        weekendPolicy.CreatedOn = entity.CreatedOn;
                        weekendPolicy.ModifiedBy = entity.ModifiedBy;
                        weekendPolicy.ModifiedOn = entity.ModifiedOn;
                        weekendPolicy.Monday = entity.Monday;
                        weekendPolicy.Tuesday = entity.Tuesday;
                        weekendPolicy.Wednesday = entity.Wednesday;
                        weekendPolicy.Thursday = entity.Thursday;
                        weekendPolicy.Friday = entity.Friday;
                        weekendPolicy.Saturday = entity.Saturday;
                        weekendPolicy.Sunday = entity.Sunday;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(WeekendPolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.WeekendPolicies.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<WeekendPolicy> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.WeekendPolicies.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public WeekendPolicy GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.WeekendPolicies.Where(x => x.BranchId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(WeekendPolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}