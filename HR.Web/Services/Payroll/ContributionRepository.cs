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
                    Contribution Contribution = dbContext.Contributions
                        .Where(x => x.ContributionId == entity.ContributionId).FirstOrDefault();
                    if (Contribution == null)
                    {
                        dbContext.Contributions.Add(Contribution);
                    }
                    else
                    {
                      
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Contribution entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Contributions.Remove(entity);
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

    }
}