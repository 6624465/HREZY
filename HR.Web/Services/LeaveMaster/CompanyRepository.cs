using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class CompanyRepository : IRepository<Company>
    {
        public CompanyRepository()
        {

        }

        public void Add(Company entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Company company = dbContext.Companies
                        .Where(x => x.CompanyId == entity.CompanyId).FirstOrDefault();
                    if (company == null)
                    {
                        dbContext.Companies.Add(entity);
                    }
                    else
                    {
                        company.CompanyCode = entity.CompanyCode;
                        company.CompanyName = entity.CompanyName;
                        company.CreatedBy = entity.CreatedBy;
                        company.CreatedOn = entity.CreatedOn;
                        company.InCorporationDate = entity.InCorporationDate;
                        company.IsActive = true;
                        company.RegNo = entity.RegNo;
                        company.ModifiedBy = entity.ModifiedBy;
                        company.ModifiedOn = entity.ModifiedOn;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Company entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Companies.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<Company> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Companies.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Company GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Companies.Where(x => x.CompanyId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}