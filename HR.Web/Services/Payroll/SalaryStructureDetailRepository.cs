using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class SalaryStructureDetailRepository : IRepository<SalaryStructureDetail>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(SalaryStructureDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    SalaryStructureDetail SalaryStructureDetail = dbContext.SalaryStructureDetails
                        .Where(x => x.StructureDetailID == entity.StructureDetailID).FirstOrDefault();
                  

                    if (SalaryStructureDetail == null)
                    {
                        dbContext.SalaryStructureDetails.Add(entity);
                    }
                    else
                    {
                            SalaryStructureDetail.Code = entity.Code;
                            SalaryStructureDetail.ComputationCode = entity.ComputationCode;
                            SalaryStructureDetail.Description = entity.Description;
                            SalaryStructureDetail.RegisterCode = entity.RegisterCode;
                            SalaryStructureDetail.ModifiedBy = entity.ModifiedBy;
                            SalaryStructureDetail.ModifiedOn = entity.ModifiedOn;
                            SalaryStructureDetail.IsActive = entity.IsActive;
                            SalaryStructureDetail.Total = entity.Total;
                            SalaryStructureDetail.IsVariablePay = entity.IsVariablePay;
                        SalaryStructureDetail.IsSatuatoryPay = entity.IsSatuatoryPay;
                        
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryStructureDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    SalaryStructureDetail SalaryStructureDetail = dbContext.SalaryStructureDetails
                        .Where(x => x.StructureDetailID == entity.StructureDetailID).FirstOrDefault();
                    dbContext.SalaryStructureDetails.Remove(SalaryStructureDetail);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal SalaryStructureDetail GetByProperty(Func<SalaryStructureDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureDetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal IEnumerable<SalaryStructureDetail> GetListByProperty(Func<SalaryStructureDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureDetails.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryStructureDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryStructureDetails.Where(x => x.StructureDetailID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}