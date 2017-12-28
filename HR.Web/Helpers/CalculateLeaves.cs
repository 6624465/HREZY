using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Helpers
{
    public static class CalculateLeavesTransaction
    {
        public static void CalculateLeaveFromTransaction(LeaveTran LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc, bool flag)
        {

            if (flag)
            {
                leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
                leaveListCalc.currentLeaves = leaveListCalc.currentLeaves != 0 ? leaveListCalc.currentLeaves - obj.Days.Value : leaveListCalc.currentLeaves;
            }
            else
            {
                leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
                leaveListCalc.currentLeaves = leaveListCalc.currentLeaves + obj.Days.Value;
                //leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
            }

            /*No need to check for each leave*/
            //else if (obj.LeaveTypeId == UTILITY.PAIDLEAVE)
            //{
            //    if (flag)
            //    {
            //        leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
            //        leaveListCalc.currentLeaves = leaveListCalc.currentLeaves != 0 ?
            //            leaveListCalc.currentLeaves - obj.Days.Value : leaveListCalc.currentLeaves;
            //    }
            //    else
            //    {
            //        leaveListCalc.currentLeaves = leaveListCalc.currentLeaves + obj.Days.Value;
            //        leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;

            //    }
            //}
            //if (obj.LeaveTypeId == UTILITY.SICKLEAVE)
            //{
            //    if (flag)
            //    {
            //        leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
            //        leaveListCalc.currentLeaves = leaveListCalc.currentLeaves != 0 ? leaveListCalc.currentLeaves - obj.Days.Value : leaveListCalc.currentLeaves;
            //    }
            //    else
            //    {
            //        leaveListCalc.currentLeaves = leaveListCalc.currentLeaves + obj.Days.Value;
            //        leaveListCalc.previousLeaves = leaveListCalc.currentLeaves;
            //    }
            //}

        }
        public static void CalculateLeave(OtherLeave LeaveTransaction, EmployeeLeaveList obj, LeaveListCalc leaveListCalc)
        {
            if (obj.LeaveTypeId == UTILITY.CASUALLEAVE)
                leaveListCalc.currentLeaves = leaveListCalc.currentLeaves - obj.Days.Value;
            else if (obj.LeaveTypeId == UTILITY.PAIDLEAVE)
                leaveListCalc.currentLeaves = leaveListCalc.currentLeaves - obj.Days.Value;
            if (obj.LeaveTypeId == UTILITY.SICKLEAVE)
                leaveListCalc.currentLeaves = leaveListCalc.currentLeaves - obj.Days.Value;
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
                    else if (weekendPolicy.Monday.Value)
                        calCBusinessDays--;
                if (date.DayOfWeek == DayOfWeek.Tuesday)
                    if (weekendPolicy.IsTuesdayHalfDay.Value)
                        calCBusinessDays = calCBusinessDays - 0.5;
                    else if (weekendPolicy.Tuesday.Value)
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
    public decimal currentLeaves = 0;
    public decimal previousLeaves = 0;

    public LeaveListCalc(decimal _currentLeaves, decimal _previousLeaves)
    {
        currentLeaves = _currentLeaves;
        previousLeaves = _previousLeaves;
    }

}