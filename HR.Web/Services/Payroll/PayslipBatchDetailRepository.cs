using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.Services.Payroll
{
    public class PayslipBatchDetailRepository : IRepository<PayslipBatchDetail>
    {
        public void Add(PayslipBatchDetail entity)
        {
            try
            {
                using (var dbcntx = new HrDataContext())
                {
                    PayslipBatchDetail payslipbatchdetail = dbcntx.PayslipBatchDetails
                        .Where(x => x.BatchDetailId == entity.BatchDetailId).FirstOrDefault();
                    if (payslipbatchdetail == null)
                    {
                        dbcntx.PayslipBatchDetails.Add(entity);
                        dbcntx.SaveChanges();
                    }
                    else
                    {
                        payslipbatchdetail.BatchDetailId = entity.BatchDetailId;
                        payslipbatchdetail.BatchHeaderId = entity.BatchHeaderId;
                        payslipbatchdetail.BatchNo = entity.BatchNo;
                        payslipbatchdetail.RegisterCode = entity.RegisterCode;
                        payslipbatchdetail.ContributionCode = entity.ContributionCode;
                        payslipbatchdetail.Amount = entity.Amount;
                        dbcntx.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

         public void Delete(PayslipBatchDetail entity)
        {
            try
            {
                using(var dbcntx=new HrDataContext())
                {
                    dbcntx.PayslipBatchDetails.Attach(entity);
                    dbcntx.PayslipBatchDetails.Remove(entity);
                    dbcntx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<PayslipBatchDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchDetails.Where(x => x.BatchDetailId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IEnumerable<PayslipBatchDetail> GetListByProperty(Func<PayslipBatchDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchDetails.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchDetail GetByProperty(Func<PayslipBatchDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchDetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}