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

        public void Update(LeaveTran transaction)
        {
            try
            {
                leaveTrasactionRepository.Update(transaction);
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

            /*
                --------------------------- START --------------------------------
               1. get the list of leaves for the given branch.
               2. get the joining date of the employee.
               3. compute the total remaining months out of the whole year from joiningdate
                    pendingMnths = (employee joining month) - ( total months in year)
               4. loop through the list of leaves.
               5. get the leaves per month for the particular leave type
               6. compute the total elegible leaves for the employee from the date of joining till the year end
                    total eligible leaves = pendingMnths  * leaves per month 
               7. push the values to the employees leave object.
               8. push the values to the database.

                --------------------------- END --------------------------------
            
            */

            using (HrDataContext dbContext = new HrDataContext())
            {
                List<OtherLeave> leavelist = dbContext.OtherLeaves.Where(x => x.BranchId ==sessionObj.BRANCHID).ToList();
                var JoiningDate = dbContext.EmployeeWorkDetails.Where(x => x.EmployeeId == empId).Select(x => x.JoiningDate).FirstOrDefault();
                int month = JoiningDate.Month;
                int remainingmonths = 12 - (month - 1);

                foreach (OtherLeave leave in leavelist)
                {
                    decimal leavespermonth = 0;
                    decimal totalleavesperyear = 0;

                    try
                    {
                        leavespermonth = leavelist.Where(x=> x.LeaveTypeId == leave.LeaveTypeId).Select(x => x.LeavesPerMonth.Value).FirstOrDefault();
                        totalleavesperyear = remainingmonths * leavespermonth;
                    }
                    catch(Exception ex)
                    {
                       
                    }




                    LeaveTran leavetrasaction = new LeaveTran()
                    {
                        BranchId =  sessionObj.BRANCHID,
                        CreatedBy = sessionObj.USERID,
                        CreatedOn = UTILITY.SINGAPORETIME,
                        CurrentLeaves = leave.LeavesPerYear == null ? 0 : totalleavesperyear,
                        PreviousLeaves = leave.LeavesPerYear == null ? 0 : totalleavesperyear,
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


        internal void UpdateLeave(int empId)
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
                        LeaveType = leave.LeaveTypeId
                    };
                    Update(leavetrasaction);
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