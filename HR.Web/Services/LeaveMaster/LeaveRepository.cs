using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class LeaveRepository : IRepository<Leave>
    {
        public LeaveRepository()
        {

        }
        public void Add(Leave entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Leave leave = dbContext.Leaves
                        .Where(x => x.LeaveId == entity.LeaveId).FirstOrDefault();
                    if (leave == null)
                    {
                        dbContext.Leaves.Add(leave);
                    }
                    else
                    {
                        leave.BranchId = entity.BranchId;
                        leave.CarryForwardCasualLeave = entity.CarryForwardCasualLeave;
                        leave.CarryForwardPerYear = entity.CarryForwardPerYear;
                        leave.CarryForwardSickLeaves = entity.CarryForwardSickLeaves;
                        leave.CasualLeavesPerMonth = entity.CasualLeavesPerMonth;
                        leave.CasualLeavesPerYear = entity.CasualLeavesPerYear;
                        leave.IsCasualLeaveCarryForward = entity.IsCasualLeaveCarryForward;
                        leave.IsPaidLeaveCarryForward = entity.IsPaidLeaveCarryForward;
                        leave.IsSickLeaveCarryForward = entity.IsSickLeaveCarryForward;
                        leave.PaidLeavesPerMonth = entity.PaidLeavesPerMonth;
                        leave.PaidLeavesPerYear = entity.PaidLeavesPerYear;
                        leave.SickLeavesPerMonth = entity.SickLeavesPerMonth;
                        leave.SickLeavesPerYear = entity.SickLeavesPerYear;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Leave entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.Leaves.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<Leave> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Leaves.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Leave GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Leaves.Where(x => x.LeaveId == id).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}