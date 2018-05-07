using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;
namespace HR.Web.Services.Payroll
{
    public class VariablePaymentHeaderRepository : IRepository<VariablePaymentHeader>
    {
        public void Add(VariablePaymentHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    VariablePaymentHeader variablepaymentheader = dbContext.VariablePaymentHeaders
                        .Where(x => x.HeaderID == entity.HeaderID).FirstOrDefault();
                    if (variablepaymentheader == null)
                    {

                        dbContext.VariablePaymentHeaders.Add(entity);
                    }
                    else
                    {
                        variablepaymentheader.TransactionNo = entity.TransactionNo;
                        variablepaymentheader.Month = entity.Month;
                        variablepaymentheader.Year = entity.Year;
                        variablepaymentheader.Status = entity.Status;
                        variablepaymentheader.ModifiedBy = entity.ModifiedBy;
                        variablepaymentheader.ModifiedOn = entity.ModifiedOn;
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(VariablePaymentHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    VariablePaymentHeader vpHeader = dbContext.VariablePaymentHeaders
                                                   .Where(x => x.HeaderID == entity.HeaderID).FirstOrDefault();

                    if (vpHeader != null)
                    {
                        dbContext.VariablePaymentHeaders.Remove(vpHeader);
                    }

                    dbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<VariablePaymentHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.VariablePaymentHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public VariablePaymentHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.VariablePaymentHeaders.Where(x => x.HeaderID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal int GetCount(int branchId)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.VariablePaymentHeaders.Count();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}