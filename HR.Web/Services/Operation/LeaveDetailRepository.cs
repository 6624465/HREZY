using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class LeaveDetailRepository : IRepository<LeaveDetail>
    {
        public LeaveDetailRepository()
        {

        }
        public void Add(LeaveDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    LeaveDetail leaveDetail = dbContext.LeaveDetails
                        .Where(x => x.LeaveDetailID == entity.LeaveDetailID).FirstOrDefault();
                    if (leaveDetail == null)
                    {
                        leaveDetail.CreatedBy = UTILITY.SSN_USERID;
                        leaveDetail.CreatedOn = UTILITY.SINGAPORETIME;
                        dbContext.LeaveDetails.Add(leaveDetail);
                    }
                    else
                    {
                        leaveDetail.CreatedBy = UTILITY.SSN_USERID;
                        leaveDetail.CreatedOn = UTILITY.SINGAPORETIME;
                        leaveDetail.LeaveHeaderId = entity.LeaveHeaderId;
                        leaveDetail.LeaveType = entity.LeaveType;
                        leaveDetail.ModifiedBy = UTILITY.SSN_USERID;
                        leaveDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                        leaveDetail.TotalLeaves = entity.TotalLeaves;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Delete(LeaveDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.LeaveDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<LeaveDetail> GetAll()
        {
            throw new NotImplementedException(); try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.LeaveDetails.Where(x => x.LeaveDetailID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}