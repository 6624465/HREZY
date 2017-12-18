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

        public static double GetBusinessDays(DateTime StartDate, DateTime EndDate, WeekendPolicy weekendPolicy, List<HolidayList> holidayList)
        {
            DateTime[] holidaysList = new DateTime[holidayList.Count];
            for (int i = 0; i < holidayList.Count; i++)
            {
                DateTime date = holidayList[i].Date;
                holidaysList[i] = new DateTime(date.Year, date.Month, date.Day);
            }


            double calCBusinessDays = calcWeekendDays(StartDate, EndDate, weekendPolicy);//1 + ((EndDate - StartDate).TotalDays);
            foreach (var holiday in holidaysList)
            {
                DateTime _holiday = holiday.Date;
                if (StartDate <= _holiday && _holiday <= EndDate)
                    calCBusinessDays--;
            }

            return calCBusinessDays;
        }

        public static double calcWeekendDays(DateTime StartDate, DateTime EndDate, WeekendPolicy weekendPolicy)
        {
            double calCBusinessDays = 1 + ((EndDate - StartDate).TotalDays);
            for (DateTime date = StartDate; date.Date <= EndDate.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday)
                    if (weekendPolicy.IsMondayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if(weekendPolicy.Monday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Tuesday)
                    if (weekendPolicy.IsTuesdayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                else if(weekendPolicy.Tuesday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Wednesday)
                    if (weekendPolicy.IsWednesdayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if (weekendPolicy.Wednesday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Thursday)
                    if (weekendPolicy.IsThursdayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if (weekendPolicy.Thursday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Friday)
                    if (weekendPolicy.IsFridayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if (weekendPolicy.Friday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    if (weekendPolicy.IsSaturdayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if (weekendPolicy.Saturday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Sunday)
                   
                        if (weekendPolicy.IsSundayHalfDay.Value)
                            calCBusinessDays = calCBusinessDays - 0.5;
                        else if (weekendPolicy.Sunday.Value)
                            calCBusinessDays--;
            }

            return calCBusinessDays;
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