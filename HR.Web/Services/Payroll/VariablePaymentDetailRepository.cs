using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.Services.Payroll
{
    public class VariablePaymentDetailRepository : IRepository<VariablePaymentDetail>
    {
        public void Add(VariablePaymentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    VariablePaymentDetail variablepaymentdeatil = dbContext.VariablePaymentDetails
                        .Where(x => x.DetailID == entity.DetailID).FirstOrDefault();


                    if (variablepaymentdeatil == null)
                    {
                        dbContext.VariablePaymentDetails.Add(entity);
                    }
                    else
                    {
                        variablepaymentdeatil.EmployeeId = entity.EmployeeId;
                        variablepaymentdeatil.ComponentCode = entity.ComponentCode;
                        variablepaymentdeatil.Amount = entity.Amount;

                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(VariablePaymentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.VariablePaymentDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<VariablePaymentDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.VariablePaymentDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public VariablePaymentDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.VariablePaymentDetails.Where(x => x.DetailID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}