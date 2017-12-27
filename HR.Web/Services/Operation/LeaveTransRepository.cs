using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class LeaveTransRepository : IRepository<LeaveTran>
    {
        public LeaveTransRepository()
        {

        }

        public void Add(LeaveTran entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    LeaveTran LeaveTransaction = dbContext.LeaveTrans
                        .Where(x => x.TransactionId == entity.TransactionId).FirstOrDefault();
                    if (LeaveTransaction == null)
                    {
                        dbContext.LeaveTrans.Add(entity);
                    }
                    else
                    {
                        LeaveTransaction.BranchId = entity.BranchId;
                        LeaveTransaction.CreatedBy = UTILITY.SSN_USERID;
                        LeaveTransaction.CreatedOn = UTILITY.SINGAPORETIME;
                        LeaveTransaction.CurrentLeaves = entity.CurrentLeaves;                      
                        LeaveTransaction.PreviousLeaves = entity.PreviousLeaves;
                        LeaveTransaction.EmployeeId = entity.EmployeeId;
                        LeaveTransaction.FromDt = entity.FromDt;
                        LeaveTransaction.LeaveType = entity.LeaveType;
                        LeaveTransaction.ModifiedBy = UTILITY.SSN_USERID;
                        LeaveTransaction.ModifiedOn = UTILITY.SINGAPORETIME;
                        LeaveTransaction.ToDt = entity.ToDt;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LeaveTran entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.LeaveTrans.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<LeaveTran> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTrans.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveTran GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTrans
                        .Where(x => x.TransactionId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveTran GetByProperty(Func<LeaveTran, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTrans.Where(predicate).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}