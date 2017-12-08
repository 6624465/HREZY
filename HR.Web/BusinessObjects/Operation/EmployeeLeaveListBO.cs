using HR.Web.Controllers;
using HR.Web.Helpers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeLeaveListBO : BaseBO
    {
        EmployeeLeaveListService employeeLeaveListService = null;
        LeaveTrasactionBO leaveTransactionBO = null;
        public EmployeeLeaveListBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeLeaveListService = new EmployeeLeaveListService();
            leaveTransactionBO = new LeaveTrasactionBO(_sessionObj);
        }

        public EmployeeLeaveList ApproveLeave(GrantLeaveListVm grantLeaveListVm)
        {

            var empLeaveObj = GetById(grantLeaveListVm.EmployeeLeaveID);
            empLeaveObj.Status = "Approved";
            empLeaveObj.Remarks = "";
            Add(empLeaveObj);
            return empLeaveObj;
        }

        public EmployeeLeaveList RejectLeave(GrantLeaveListVm grantLeaveListVm)
        {

            var empLeaveObj = GetById(grantLeaveListVm.EmployeeLeaveID);
            empLeaveObj.Status = "Rejected";
            empLeaveObj.Remarks = "";
            Add(empLeaveObj);
            return empLeaveObj;

        }
        public void CancelLeave(int employeeLeaveID, string remarks)
        {

            EmployeeLeaveList empLeaveObj = GetById(employeeLeaveID);

            empLeaveObj.Status = "Cancelled";
            empLeaveObj.Remarks = remarks;
            Add(empLeaveObj);
            LeaveTransaction leavetransaction = leaveTransactionBO.GetByProperty(sessionObj.BRANCHID, sessionObj.EMPLOYEEID);

            LeaveListCalc leaveListCalc = null;
            if (leavetransaction != null)
            {


                leaveListCalc = new LeaveListCalc(
                    leavetransaction.CurrentCasualLeaves,
                    leavetransaction.CurrentPaidLeaves,
                    leavetransaction.CurrentSickLeaves,
                    leavetransaction.PreviousCasualLeaves,
                    leavetransaction.PreviousPaidLeaves,
                    leavetransaction.PreviousSickLeaves);

                CalculateLeavesTransaction.CalculateLeaveFromTransaction(
                    leavetransaction,
                    empLeaveObj,
                    leaveListCalc,
                    false);
            }

            LeaveTransaction leaveTransaction = new LeaveTransaction()
            {
                BranchId = sessionObj.BRANCHID,
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                CurrentCasualLeaves = leaveListCalc.currentCasualLeaves,
                CurrentPaidLeaves = leaveListCalc.currentPaidLeaves,
                CurrentSickLeaves = leaveListCalc.currentSickLeaves,
                EmployeeId = sessionObj.EMPLOYEEID,
                FromDt = empLeaveObj.FromDate,
                ToDt = empLeaveObj.ToDate,
                PreviousCasualLeaves = leaveListCalc.previousCasualLeaves,
                PreviousPaidLeaves = leaveListCalc.previousPaidLeaves,
                PreviousSickLeaves = leaveListCalc.previousSickLeaves,
                LeaveType = empLeaveObj.LeaveTypeId
            };
            leaveTransactionBO.Add(leaveTransaction);

        }

        public void Add(EmployeeLeaveList empLeaveList)
        {
            try
            {
                employeeLeaveListService.Add(empLeaveList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeLeaveList empLeaveList)
        {
            try
            {
                employeeLeaveListService.Delete(empLeaveList);
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
                return employeeLeaveListService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeLeaveList> GetAll(int id)
        {
            try
            {
                return employeeLeaveListService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}