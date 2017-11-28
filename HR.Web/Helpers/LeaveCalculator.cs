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

                Leave leave = dbContext.Leaves.Where(x => x.BranchId == BranchID).FirstOrDefault();
                LeaveTransaction leaveTransaction = dbContext.LeaveTransactions.Where(x => x.EmployeeId == EmployeeID && x.BranchId == BranchID)
                                                    .OrderByDescending(x => x.TransactionId)
                                              .ThenByDescending(x => x.CreatedOn).FirstOrDefault();
                if (leaveTransaction != null)
                {
                    if (LeaveType == 1030)
                    {
                        appliedLeave = leave.CasualLeavesPerYear.Value - leaveTransaction.CurrentCasualLeaves;
                        eligibleLeaves = (currentMonth * leave.CasualLeavesPerMonth.Value) - appliedLeave;
                    }
                    else if (LeaveType == 1049)
                    {
                        appliedLeave = leave.PaidLeavesPerYear.Value - leaveTransaction.CurrentPaidLeaves;
                        eligibleLeaves = (currentMonth * leave.PaidLeavesPerMonth.Value) - appliedLeave;

                    }
                }
                return eligibleLeaves;

            }
        }
    }
}