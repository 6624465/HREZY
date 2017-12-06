using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeeHeaderService : IRepository<EmployeeHeader>
    {
        public EmployeeHeaderService()
        {

        }
        public void Add(EmployeeHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeHeaders.Add(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeHeaders.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeHeaders.Where(x => x.EmployeeId == id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(EmployeeHeader entity)
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