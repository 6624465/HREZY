using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Controllers;

namespace HR.Web.Helpers
{
    public class LeaveCalculator
    {
        public decimal GetLeavesCount(int BranchID, int EmployeeID, int LeaveType, DateTime fromDate)
        {
            DateTime date = fromDate;
            //int currentMonth = date.Month;
            int currentYear = date.Year;
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

                List<EmployeeLeaveList> leaveList = dbContext.EmployeeLeaveLists
                    .Where(x => x.EmployeeId == EmployeeID && x.BranchId == BranchID && x.LeaveTypeId == LeaveType &&
                    x.FromDate >= startDate && x.ToDate <= endDate && x.Status != UTILITY.LEAVECANCELLED).ToList();
                decimal? DaysCount = 0;
                foreach (EmployeeLeaveList item in leaveList)
                {
                    DaysCount += item.Days;
                }

                if (leaveTransaction != null)
                {
                    LeaveMaster lMaster = new LeaveMaster();

                    if (LeaveType == lMaster.SICKLEAVE(BranchID))
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentYear * leave.LeavesPerYear.Value) - appliedLeave;
                        else
                            eligibleLeaves = leave.LeavesPerYear.Value - appliedLeave;

                    }
                    if (LeaveType == lMaster.CASUALLEAVE(BranchID))
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentYear * leave.LeavesPerYear.Value) - appliedLeave;
                        else if(DaysCount==0)
                            eligibleLeaves = leave.LeavesPerYear.Value;
                        else
                            eligibleLeaves = leave.LeavesPerYear.Value - appliedLeave;

                    }
                    if (LeaveType == lMaster.PAIDLEAVE(BranchID))
                    {
                        appliedLeave = leave.LeavesPerYear.Value - leaveTransaction.CurrentLeaves;
                        if (leave.IsCarryForward)
                            eligibleLeaves = (currentYear * leave.LeavesPerYear.Value) - appliedLeave;
                        else
                            eligibleLeaves = leave.LeavesPerYear.Value - appliedLeave;

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