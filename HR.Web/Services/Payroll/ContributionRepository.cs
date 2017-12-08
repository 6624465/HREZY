using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class ContributionRepository : IRepository<Contribution>
    {
        public void Add(Contribution entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Contribution contribution = dbContext.Contributions
                        .Where(x => x.ContributionId == entity.ContributionId).FirstOrDefault();
                    if (contribution == null)
                    {
                      
                        dbContext.Contributions.Add(entity);
                    }
                    else
                    {
                        contribution.Description = entity.Description;
                        contribution.IsActive = entity.IsActive;
                        contribution.ModifiedBy = entity.ModifiedBy;
                        contribution.ModifiedOn = entity.ModifiedOn;
                        contribution.Name = entity.Name;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Contribution entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    var record = dbContext.Contributions.Where(x => x.ContributionId == id).FirstOrDefault();
                    record.IsActive = false;
                    
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Contribution> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Contributions.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Contribution GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Contributions.Where(x => x.ContributionId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Contribution GetByProperty(Func<Contribution, bool> predicate) {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Contributions.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<Contribution> GetListByProperty(Func<Contribution, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Contributions.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}