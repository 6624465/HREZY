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
                    {
                        entity.CreateBy = sessionobj.USERID;
                        entity.CreatedOn = UTILITY.SINGAPORETIME;
                        dbContext.WeekendPolicies.Add(entity);
                    }
                    else
                    {

                        weekendPolicy.ModifiedBy = sessionobj.USERID;
                        weekendPolicy.ModifiedOn = UTILITY.SINGAPORETIME;
                        weekendPolicy.Monday = entity.Monday;
                        weekendPolicy.Tuesday = entity.Tuesday;
                        weekendPolicy.Wednesday = entity.Wednesday;
                        weekendPolicy.Thursday = entity.Thursday;
                        weekendPolicy.Friday = entity.Friday;
                        weekendPolicy.Saturday = entity.Saturday;
                        weekendPolicy.Sunday = entity.Sunday;
                        weekendPolicy.IsMondayHalfDay = entity.IsMondayHalfDay;
                        weekendPolicy.IsTuesdayHalfDay = entity.IsTuesdayHalfDay;
                        weekendPolicy.IsWednesdayHalfDay = entity.IsWednesdayHalfDay;
                        weekendPolicy.IsThursdayHalfDay = entity.IsThursdayHalfDay;
                        weekendPolicy.IsFridayHalfDay = entity.IsFridayHalfDay;
                        weekendPolicy.IsSaturdayHalfDay = entity.IsSaturdayHalfDay;
                        weekendPolicy.IsSundayHalfDay = entity.IsSundayHalfDay;
                        weekendPolicy.FridaySession = entity.FridaySession;
                        weekendPolicy.SaturdaySession = entity.SaturdaySession;
                        weekendPolicy.SundaySession = entity.SundaySession;
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