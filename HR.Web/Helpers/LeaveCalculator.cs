using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Helpers
{
    public class LeaveCalculator
    {
        public decimal GetLeavesCount(int BranchID, int EmployeeID, int LeaveType, DateTime fromDate)
        {
            DateTime date = fromDate;
            int currentMonth = date.Month;
            decimal eligibleLeaves = 0;
            decimal appliedLeave = 0;
            using (HrDataContext dbContext = new HrDataContext())
            {

                OtherLeave leave = dbContext.OtherLeaves.Where(x => x.BranchId == BranchID && x.LeaveTypeId == LeaveType).FirstOrDefault();
                LeaveTran leaveTransaction = dbContext.LeaveTrans.Where(x => x.EmployeeId == EmployeeID && x.BranchId == BranchID && x.LeaveType == LeaveType)
                                                    .OrderByDescending(x => x.TransactionId)
                                              .ThenByDescending(x => x.CreatedOn).FirstOrDefault();

                DateTime now = date;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                //EmployeeLeaveList leaveList = dbContext.EmployeeLeaveLists
                //    .Where(x => x.EmployeeId == EmployeeID && x.BranchId == BranchID && x.LeaveTypeId == LeaveType).FirstOrDefault();
                //leaveList = leaveList.Between(startDate, endDate).FirstOrDefault();

                if (leaveTransaction != null)
                {

                    if (LeaveType == UTILITY.SICKLEAVE)
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentMonth * leave.LeavesPerMonth.Value) - appliedLeave;
                        else
                            eligibleLeaves = leave.LeavesPerMonth.Value - appliedLeave;

                    }
                    if (LeaveType == UTILITY.CASUALLEAVE)
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentMonth * leave.LeavesPerMonth.Value) - appliedLeave;
                        else
                            eligibleLeaves = leave.LeavesPerMonth.Value - appliedLeave;

                    }
                    if (LeaveType == UTILITY.PAIDLEAVE)
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentMonth * leave.LeavesPerMonth.Value) - appliedLeave;
                        else
                            eligibleLeaves = leave.LeavesPerMonth.Value - appliedLeave;

                    }
                    //else if (LeaveType == UTILITY.PAIDLEAVE)
                    //{
                    //    appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                    //    eligibleLeaves = (currentMonth * leave.LeavesPerMonth.Value) - appliedLeave;

                    //}
                    //else if (LeaveType == UTILITY.PAIDLEAVE)
                    //{
                    //    appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                    //    eligibleLeaves = (currentMonth * leave.LeavesPerMonth.Value) - appliedLeave;

                    //}
                }
                return eligibleLeaves;

            }
        }
    }
}