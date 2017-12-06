using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services
{
    public class WeekendPolicyService : IRepository<WeekendPolicy>
    {
        public WeekendPolicyService()
        {

        }
        public void Add(WeekendPolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.WeekendPolicies.Add(entity);
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