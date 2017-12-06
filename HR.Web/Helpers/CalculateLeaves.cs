using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Helpers
{
    public static class CalculateLeavesTransaction
    {
        public static void CalculateLeaveFromTransaction(LeaveTransaction LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc, bool flag)
        {
            if (obj.LeaveTypeId == 1030)
            {
                if (flag)
                {
                    leaveListCalc.previousCasualLeaves = leaveListCalc.currentCasualLeaves;

                    leaveListCalc.currentCasualLeaves = leaveListCalc.currentCasualLeaves != 0 ? leaveListCalc.currentCasualLeaves - obj.Days.Value : leaveListCalc.currentCasualLeaves;
                }
                else
                {
                    leaveListCalc.currentCasualLeaves = leaveListCalc.currentCasualLeaves + obj.Days.Value;
                    leaveListCalc.previousCasualLeaves = leaveListCalc.currentCasualLeaves;

                }
            }
            else if (obj.LeaveTypeId == 1049)
            {
                if (flag)
                {
                    leaveListCalc.previousPaidLeaves = leaveListCalc.currentPaidLeaves;
                    leaveListCalc.currentPaidLeaves = leaveListCalc.currentPaidLeaves != 0 ? leaveListCalc.currentPaidLeaves - obj.Days.Value : leaveListCalc.currentPaidLeaves;
                }
                else
                {
                    leaveListCalc.currentPaidLeaves = leaveListCalc.currentPaidLeaves + obj.Days.Value;
                    leaveListCalc.previousPaidLeaves = leaveListCalc.currentPaidLeaves;

                }
            }

        }
        public static void CalculateLeave(Leave LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc)
        {
            if (obj.LeaveTypeId == 1030)
                leaveListCalc.currentCasualLeaves = leaveListCalc.currentCasualLeaves - obj.Days.Value;
            else if (obj.LeaveTypeId == 1049)
                leaveListCalc.currentPaidLeaves = leaveListCalc.currentPaidLeaves - obj.Days.Value;
        }

        public static double GetBusinessDays(DateTime startDate, DateTime endDate)
        {
            DateTime[] holidaysList = new DateTime[] {
                new DateTime(2017,12,25)
            };

            double? calCBusinessDays = 1 + ((endDate - startDate).TotalDays * 5 -
                                     (startDate.DayOfWeek - endDate.DayOfWeek) * 2) / 7;

            foreach (var holiday in holidaysList)
            {
                DateTime _holiday = holiday.Date;
                if (startDate <= _holiday && _holiday <= endDate)
                    calCBusinessDays--;
            }
            //if (startDate.DayOfWeek == DayOfWeek.Saturday)
            //    calCBusinessDays--;
            //if (endDate.DayOfWeek == DayOfWeek.Sunday)
            //    calCBusinessDays--;

            

            return calCBusinessDays.Value;
        }

    }
}

public class LeaveListCalc
{
    public decimal currentCasualLeaves = 0;
    public decimal currentPaidLeaves = 0;
    public decimal currentSickLeaves = 0;
    public decimal previousCasualLeaves = 0;
    public decimal previousPaidLeaves = 0;
    public decimal previousSickLeaves = 0;
    public LeaveListCalc(decimal _currentCasualLeaves, decimal _currentPaidLeaves, decimal _currentSickLeaves,
        decimal _previousCasualLeaves, decimal _previousPaidLeaves, decimal _previousSickLeaves)
    {
        currentCasualLeaves = _currentCasualLeaves;
        currentPaidLeaves = _currentPaidLeaves;
        currentSickLeaves = _currentSickLeaves;
        previousCasualLeaves = _previousCasualLeaves;
        previousPaidLeaves = _previousPaidLeaves;
        previousSickLeaves = _previousSickLeaves;
    }

}