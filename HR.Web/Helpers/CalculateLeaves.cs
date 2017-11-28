using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Helpers
{
    public static class CalculateLeaves
    {
        public static void CalculateLeaveFromTransaction(LeaveTransaction LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc, bool flag)
        {
            if (obj.LeaveTypeId == 1030)
            {
                if (flag)
                {
                    leaveListCalc.previousCasualLeaves = leaveListCalc.currentCasualLeaves;
                    leaveListCalc.currentCasualLeaves = leaveListCalc.currentCasualLeaves - obj.Days;
                }
                else
                {
                    leaveListCalc.currentCasualLeaves = leaveListCalc.currentCasualLeaves + obj.Days;
                    leaveListCalc.previousCasualLeaves = leaveListCalc.currentCasualLeaves;

                }
            }
            else if (obj.LeaveTypeId == 1031)
            {
                if (flag)
                {
                    leaveListCalc.previousSickLeaves = leaveListCalc.currentSickLeaves;
                    leaveListCalc.currentSickLeaves = leaveListCalc.currentSickLeaves - obj.Days;
                }
                else
                {
                    leaveListCalc.currentSickLeaves = leaveListCalc.currentSickLeaves + obj.Days;
                    leaveListCalc.previousSickLeaves = leaveListCalc.currentSickLeaves;

                }
            }

        }
        public static void CalculateLeave(Leave LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc)
        {
            if (obj.LeaveTypeId == 1030)
                leaveListCalc.currentCasualLeaves = LeaveTransaction.CasualLeavesPerYear.Value - obj.Days;
            else if (obj.LeaveTypeId == 1031)
                leaveListCalc.currentSickLeaves = LeaveTransaction.SickLeavesPerYear.Value - obj.Days;
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