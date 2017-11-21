﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace HR.Web
{

    public static class DbDateHelper
    {
        /// <summary>
        /// Replaces any date before 01.01.1753 with a Nullable of 
        /// DateTime with a value of null.
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>Input date if valid in the DB, or Null if date is 
        /// too early to be DB compatible.</returns>
      
        public static DateTime? ToNullIfTooEarlyForDb(this DateTime date)
        {
            return (date >= (DateTime)SqlDateTime.MinValue) ? date : (DateTime?)null;
        }

    }
    public static class UTILITY
    {
        public static string SSN_USERID = "SSN_USERID";
        public static string SSN_BRANCHID = "SSN_BRANCHID";


        /* Config Keys Start */
        public static string CONFIG_EMPLOYEETYPE = "EmployeeType";
        public static string CONFIG_EMPLOYEESTATUS = "EmployeeStatus";
        public static string CONFIG_EMPLOYEEDEPARTMENT = "EmployeeDepartment";
        public static string CONFIG_EMPLOYEEDESIGNATION = "EmployeeDesignation";
        public static string CONFIG_EMPLOYEEMARITALSTATUS = "MartialSatus";
        public static string CONFIG_EMPLOYEELEAVETYPE = "LeaveType";
        public static string CONFIG_EMPLOYEEPAYMENTTYPE = "PaymentType";
        public static string CONFIG_NOTICEPERIOD = "NoticePeriod";
        public static string CONFIG_PROHIBATIONPERIOD = "ProbabtonPeriod";
        public static string CONFIG_GRANTLEAVEYEAR = "Year";
        public static string CONFIG_GRANTLEAVEPERIODICITY = "PeriodicityType";
        public static string CONFIG_GRANTLEAVEPERIODTYPE = "PeriodType";
        public static string CONFIG_GRANTLEAVESCHEMETYPE = "LeaveSchemeType";
        public static string CONFIG_DOCUMENTTYPE = "DocumentType";
        public static string CONFIG_ROLECODE = "ROLECODE";


        /* Config Keys End */

        public static DateTime SINGAPORETIME
        {
            get
            {
                string nzTimeZoneKey = "SE Asia Standard Time";
                TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, nzTimeZone);
            }

        }
       
    }
}