using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class LeaveHeaderRepository : IRepository<LeaveHeader>
    {
        public LeaveHeaderRepository()
        {

        }

        public void Add(LeaveHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    LeaveHeader leaveHeader = dbContext.LeaveHeaders
                        .Where(x => x.LeaveHeaderID == entity.LeaveHeaderID).FirstOrDefault();
                    if (leaveHeader == null)
                    {
                        dbContext.LeaveHeaders.Add(entity);
                    }
                    else
                    {
                        leaveHeader.BranchID = entity.BranchID;
                        leaveHeader.CreatedBy = UTILITY.SSN_USERID;
                        leaveHeader.CreatedOn = UTILITY.SINGAPORETIME;
                        leaveHeader.EndDate = entity.EndDate;
                        leaveHeader.LeaveSchemeType = entity.LeaveSchemeType;
                        leaveHeader.LeaveYear = entity.LeaveYear;
                        leaveHeader.ModifiedBy = UTILITY.SSN_USERID;
                        leaveHeader.ModifiedOn = UTILITY.SINGAPORETIME;
                        leaveHeader.PeriodicityType = entity.PeriodicityType;
                        leaveHeader.PeriodType = entity.PeriodType;
                        leaveHeader.StartDate = entity.StartDate;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LeaveHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.LeaveHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<LeaveHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public LeaveHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveHeaders.Where(x => x.LeaveHeaderID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}