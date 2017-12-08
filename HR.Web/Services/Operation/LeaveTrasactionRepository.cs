using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class LeaveTrasactionRepository : IRepository<LeaveTransaction>
    {
        public LeaveTrasactionRepository()
        {

        }

        public void Add(LeaveTransaction entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    LeaveTransaction leaveTransaction = dbContext.LeaveTransactions
                        .Where(x => x.TransactionId == entity.TransactionId).FirstOrDefault();
                    if (leaveTransaction == null)
                    {
                        dbContext.LeaveTransactions.Add(entity);
                    }
                    else
                    {
                        leaveTransaction.BranchId = entity.BranchId;
                        leaveTransaction.CreatedBy = UTILITY.SSN_USERID;
                        leaveTransaction.CreatedOn = UTILITY.SINGAPORETIME;
                        leaveTransaction.CurrentCasualLeaves = entity.CurrentCasualLeaves;
                        leaveTransaction.CurrentPaidLeaves = entity.CurrentPaidLeaves;
                        leaveTransaction.CurrentSickLeaves = entity.CurrentSickLeaves;
                        leaveTransaction.EmployeeId = entity.EmployeeId;
                        leaveTransaction.FromDt = entity.FromDt;
                        leaveTransaction.LeaveType = entity.LeaveType;
                        leaveTransaction.ModifiedBy = UTILITY.SSN_USERID;
                        leaveTransaction.ModifiedOn = UTILITY.SINGAPORETIME;
                        leaveTransaction.PreviousCasualLeaves = entity.PreviousCasualLeaves;
                        leaveTransaction.PreviousPaidLeaves = entity.PreviousPaidLeaves;
                        leaveTransaction.PreviousSickLeaves = entity.PreviousSickLeaves;
                        leaveTransaction.ToDt = entity.ToDt;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LeaveTransaction entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.LeaveTransactions.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<LeaveTransaction> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTransactions.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveTransaction GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTransactions
                        .Where(x => x.TransactionId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveTransaction GetByProperty(Func<LeaveTransaction, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveTransactions.Where(predicate).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}