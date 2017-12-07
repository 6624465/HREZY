using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class LookUpRepository : IRepository<LookUp>
    {
        public void Add(LookUp entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    LookUp lookUp = dbContext.LookUps
                        .Where(x => x.LookUpID == entity.LookUpID).FirstOrDefault();
                    if (lookUp == null)
                    {
                        dbContext.LookUps.Add(lookUp);
                    }
                    else
                    {
                        lookUp.IsActive = entity.IsActive;
                        lookUp.LookUpCategory = entity.LookUpCategory;
                        lookUp.LookUpCode = entity.LookUpCode;
                        lookUp.LookUpDescription = entity.LookUpDescription;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LookUp entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.LookUps.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<LookUp> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LookUps.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LookUp GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LookUps.Where(x=>x.LookUpID ==  id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}