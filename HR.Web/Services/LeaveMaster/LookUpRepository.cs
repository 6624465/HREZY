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
                       
                        entity.IsActive = true;
                        dbContext.LookUps.Add(entity);
                    }
                    else
                    {
                        lookUp.IsCarryForward = entity.IsCarryForward;
                        lookUp.IsWeekendPolicy = entity.IsWeekendPolicy;
                        lookUp.LookUpCategory = entity.LookUpCategory;
                        lookUp.LookUpCode = entity.LookUpCode;
                        lookUp.LookUpDescription = entity.LookUpDescription;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public void Delete(LookUp entity)
        //{
        //    try
        //    {
        //        using (HrDataContext dbContext = new HrDataContext())
        //        {
        //            LookUp lookUp = dbContext.LookUps
        //                .Where(x => x.LookUpID == entity.LookUpID).FirstOrDefault();
        //            lookUp.IsActive = false;
        //            //dbContext.LookUps.Remove(entity);
        //            dbContext.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public void Delete(LookUp entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    var record = dbContext.LookUps.Where(x => x.LookUpID == id).FirstOrDefault();
                    record.IsActive = false;

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
                    return dbContext.LookUps.Where(x => x.LookUpID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LookUp GetByProperty(Func<LookUp, bool> predicate)
        {

            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LookUps.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<LookUp> GetListByProperty(Func<LookUp, bool> predicate)
        {

            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LookUps.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}