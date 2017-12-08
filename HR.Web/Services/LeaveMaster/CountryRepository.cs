using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class CountryRepository : IRepository<Country>
    {
        public void Add(Country entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Country country = dbContext.Countries
                        .Where(x => x.CountryId == entity.CountryId).FirstOrDefault();
                    if (country == null)
                    {
                        dbContext.Countries.Add(entity);
                    }
                    else
                    {
                        country.CountryCode = entity.CountryCode;
                        country.CountryName = entity.CountryName;
                        country.Description = entity.Description;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Country entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Countries.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Country> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Countries.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Country GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Countries.Where(x => x.CountryId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}