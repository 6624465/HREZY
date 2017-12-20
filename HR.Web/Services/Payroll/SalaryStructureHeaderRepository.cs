using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class SalaryStructureHeaderRepository : IRepository<SalaryStructureHeader>
    {
        public void Add(SalaryStructureHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    SalaryStructureHeader salaryStructureHeader = dbContext.SalaryStructureHeaders
                        .Where(x => x.StructureID == entity.StructureID).FirstOrDefault();
                    if (salaryStructureHeader == null)
                    {

                        dbContext.SalaryStructureHeaders.Add(entity);
                    }
                    else
                    {
                        salaryStructureHeader.Code = entity.Code;
                        salaryStructureHeader.Remarks = entity.Remarks;
                        salaryStructureHeader.EffectiveDate = entity.EffectiveDate;
                        salaryStructureHeader.IsActive = entity.IsActive;
                        salaryStructureHeader.ModifiedBy = entity.ModifiedBy;
                        salaryStructureHeader.ModifiedOn = entity.ModifiedOn;
                        salaryStructureHeader.NetAmount = entity.NetAmount;
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryStructureHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.SalaryStructureHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal SalaryStructureHeader GetByProperty(Func<SalaryStructureHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureHeaders.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal IEnumerable<SalaryStructureHeader> GetListByProperty(Func<SalaryStructureHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureHeaders.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryStructureHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureHeaders.Where(x => x.StructureID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void DeleteById(int structurId)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    SalaryStructureHeader structureHeader= dbContext.SalaryStructureHeaders.Where(x=>x.StructureID== structurId).FirstOrDefault();
                    structureHeader.IsActive = false;
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