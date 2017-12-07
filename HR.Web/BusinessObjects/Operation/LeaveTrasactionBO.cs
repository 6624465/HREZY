using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class LeaveTrasactionBO : BaseBO
    {
        LeaveTrasactionRepository leaveTrasactionRepository = null;
        public LeaveTrasactionBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            leaveTrasactionRepository = new LeaveTrasactionRepository();

        }

        public void Add(LeaveTransaction transaction)
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
        public void Delete(LeaveTransaction transaction)
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

        public LeaveTransaction GetById(int id)
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

        public IEnumerable<LeaveTransaction> GetAll(int id)
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
                Leave leave = dbContext.Leaves.Where(x => x.BranchId == sessionObj.BRANCHID).FirstOrDefault();

                LeaveTransaction leavetrasaction = new LeaveTransaction()
                {
                    BranchId = sessionObj.BRANCHID,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    CurrentCasualLeaves = leave.CasualLeavesPerMonth.Value,
                    CurrentPaidLeaves = leave.PaidLeavesPerMonth.Value,
                    CurrentSickLeaves = leave.SickLeavesPerMonth.Value,
                    EmployeeId = empId,
                    FromDt = UTILITY.SINGAPORETIME,
                    ToDt = UTILITY.SINGAPORETIME,
                    PreviousCasualLeaves = leave.CasualLeavesPerMonth.Value,
                    PreviousPaidLeaves = leave.PaidLeavesPerMonth.Value,
                    PreviousSickLeaves = leave.SickLeavesPerMonth.Value,
                    ModifiedBy = sessionObj.USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME,
                };
                Add(leavetrasaction);
            }

        }
    }
}