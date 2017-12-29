using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.OtherLeaveMaster
{
    public class OtherLeaveRepository : IRepository<OtherLeave>
    {
        public OtherLeaveRepository()
        {
           
        }
        public void Add(OtherLeave entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    OtherLeave OtherLeave = dbContext.OtherLeaves
                        .Where(x => x.BranchId == entity.BranchId && x.LeaveId == entity.LeaveId).FirstOrDefault();
                    if (OtherLeave == null)
                    {
                        dbContext.OtherLeaves.Add(entity);
                    }
                    else
                    {
                        OtherLeave.BranchId = entity.BranchId;
                        OtherLeave.CarryForward = entity.CarryForward;
                        OtherLeave.IsCarryForward = entity.IsCarryForward;
                        OtherLeave.LeavesPerMonth = entity.LeavesPerMonth;
                        OtherLeave.LeavesPerYear = entity.LeavesPerYear;
                        OtherLeave.LeaveTypeId = entity.LeaveTypeId;
                      //  OtherLeave.
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(OtherLeave entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.OtherLeaves.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<OtherLeave> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.OtherLeaves.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<OtherLeave> GetlistById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.OtherLeaves.Where(x => x.BranchId == id).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public OtherLeave GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.OtherLeaves.Where(x => x.BranchId == id).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddLookUP(LookUp entity)
        {
            try
            {
                using (var dbcntx = new HrDataContext())
                {

                    OtherLeave Leave = new OtherLeave
                    {
                        LeaveTypeId = entity.LookUpID,
                        Description = entity.LookUpCode,
                        IsCarryForward = entity.IsCarryForward,
                    };
                    dbcntx.OtherLeaves.Add(Leave);
                    dbcntx.SaveChanges();

                }

            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}