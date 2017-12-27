using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class LeaveTransBO : BaseBO
    {
        LeaveTransRepository leaveTrasactionRepository = null;
        public LeaveTransBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            leaveTrasactionRepository = new LeaveTransRepository();

        }

        public void Add(LeaveTran transaction)
        {
            try
            {
                leaveTrasactionRepository.Add(transaction);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Delete(LeaveTran transaction)
        {
            try
            {
                leaveTrasactionRepository.Delete(transaction);
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
                return leaveTrasactionRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<LeaveTran> GetAll(int id)
        {
            try
            {
                return leaveTrasactionRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        internal void AddLeave(int empId)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                List<OtherLeave> leavelist = dbContext.OtherLeaves.Where(x => x.BranchId == sessionObj.BRANCHID).ToList();

                foreach (OtherLeave leave in leavelist)
                {
                    LeaveTran leavetrasaction = new LeaveTran()
                    {
                        BranchId = sessionObj.BRANCHID,
                        CreatedBy = sessionObj.USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CurrentLeaves = leave.LeavesPerYear == null ? 0 : leave.LeavesPerYear.Value,
                        PreviousLeaves = leave.LeavesPerYear == null ? 0 : leave.LeavesPerYear.Value,
                        EmployeeId = empId,
                        FromDt = UTILITY.SINGAPORETIME,
                        ToDt = UTILITY.SINGAPORETIME,
                        ModifiedBy = sessionObj.USERID,
                        ModifiedOn = UTILITY.SINGAPORETIME,
                        LeaveType= leave.LeaveTypeId
                    };
                    Add(leavetrasaction);
                }
            }

        }

        public LeaveTran GetByProperty(Func<LeaveTran,bool> predicate)
        {
            try
            {
                return leaveTrasactionRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}