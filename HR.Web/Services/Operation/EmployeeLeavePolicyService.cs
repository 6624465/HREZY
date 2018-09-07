using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.Services.Operation
{
    public class EmployeeLeavePolicyService : IRepository<EmployeeLeavePolicy>
    {
        public EmployeeLeavePolicyService()
        {

        }
        public void Add(EmployeeLeavePolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    EmployeeLeavePolicy leaveTransaction = dbContext.EmployeeLeavePolicies
                        .Where(x => x.LeaveTypeID == entity.LeaveTypeID && x.BranchID == entity.BranchID && x.EmployeeID==entity.EmployeeID).FirstOrDefault();
                    if (leaveTransaction == null)
                    {
                        dbContext.EmployeeLeavePolicies.Add(entity);
                    }
                  
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeLeavePolicy entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {

                    dbContext.EmployeeLeavePolicies.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<EmployeeLeavePolicy> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeLeavePolicies.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeLeavePolicy GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeLeavePolicies
                        .Where(x => x.LeaveTypeID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}