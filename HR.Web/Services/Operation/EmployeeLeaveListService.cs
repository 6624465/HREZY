﻿using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeeLeaveListService : IRepository<EmployeeLeaveList>
    {
        public EmployeeLeaveListService()
        {

        }
        public void Add(EmployeeLeaveList entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    EmployeeLeaveList leaveList = dbContext.EmployeeLeaveLists
                        .Where(x => x.EmployeeLeaveID == entity.EmployeeLeaveID).FirstOrDefault();
                    if (leaveList == null)
                    {
                        dbContext.EmployeeLeaveLists.Add(entity);
                    }
                    else
                    {
                        leaveList.BranchId = entity.BranchId;
                        leaveList.FromDate = entity.FromDate;
                        leaveList.ToDate = entity.ToDate;
                        leaveList.Days = entity.Days;
                        leaveList.EmployeeId = entity.EmployeeId; //Convert.ToInt32(UTILITY.SSN_EMPLOYEEID);
                        leaveList.LeaveTypeId = entity.LeaveTypeId;
                        leaveList.Remarks = entity.Remarks;
                        leaveList.Reason = entity.Reason;
                        leaveList.CreatedBy =entity.CreatedBy;
                        leaveList.CreatedOn = UTILITY.SINGAPORETIME;
                        leaveList.ModifiedBy = entity.ModifiedBy;
                        leaveList.ModifiedOn = UTILITY.SINGAPORETIME;
                        leaveList.ApplyDate = UTILITY.SINGAPORETIME;
                        if (entity.ManagerId == 0)
                        {
                            leaveList.ManagerId = 0;
                        }
                        else
                        {
                            leaveList.ManagerId = dbContext.EmployeeHeaders
                                       .Where(x => x.EmployeeId == leaveList.EmployeeId)
                                       .FirstOrDefault()
                                       .ManagerId.Value;
                        }
                   
                        leaveList.Status =entity.Status;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeLeaveList entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeLeaveLists.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeLeaveList> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeLeaveLists.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeLeaveList GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeLeaveLists
                        .Where(x => x.EmployeeLeaveID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       public EmployeeLeaveList GetByProperty(Func<EmployeeLeaveList, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeLeaveLists.Where(predicate).OrderByDescending(x=>x.EmployeeLeaveID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}