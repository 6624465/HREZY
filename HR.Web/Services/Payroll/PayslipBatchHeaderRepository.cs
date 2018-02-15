using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class PayslipBatchHeaderRepository : IRepository<PayslipBatchHeader>
    {
        public void Add(PayslipBatchHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    PayslipBatchHeader payslipBatchHeader = dbContext.PayslipBatchHeaders
                        .Where(x => x.BatchHeaderId == entity.BatchHeaderId).FirstOrDefault();
                    if (payslipBatchHeader == null)
                    {
                        dbContext.PayslipBatchHeaders.Add(entity);
                        dbContext.SaveChanges();
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Delete(PayslipBatchHeader entity)
        {
            try
            {
                using (var dbcntx = new HrDataContext())
                {
                    dbcntx.PayslipBatchHeaders.Remove(entity);
                    dbcntx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PayslipBatchHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<PayslipBatchHeader> GetListByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchHeader GetByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}