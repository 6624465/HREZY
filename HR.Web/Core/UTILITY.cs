using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

using System.Configuration;

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
        public static string SSN_EMPLOYEEID = "SSN_EMPLOYEEID";
        public static string SSN_BRANCHID = "SSN_BRANCHID";
        public static string SSN_OBJECT = "SSN_OBJECT";

        public static string ROLE_SUPERADMIN = "SuperAdmin";
        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_EMPLOYEE = "Employee";
        public static string SSN_FILENAME = "SSN_FILENAME";
        public static string SSN_DOCUMENTDETAILID = "SSN_DOCUMENTDETAILID";
        public static string SSN_FIRSTNAME = "SSN_FIRSTNAME";




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
        public static string CONFIG_MANAGER = "MANAGER";
        public static string CONFIG_TRAVELCLAIM = "TravelClaim";
        public static string CONFIG_CURRENCY = "Currency";
        public static string CONFIG_TRAVELCLAIMTYPE = "TravelClaimType";
        public static string CONFIG_EMPSALUTATIONTYPE = "salutationtype";



        public static string LEAVEAPPROVED = "Approved";
        public static string LEAVEREJECTED = "Rejected";
        public static string LEAVECANCELLED = "Cancelled";
        public static string LEAVEPENDING = "Pending";

        public static string PAYROLLCONTRIBUTION = "PayRoll Contribution";
        public static string PAYROLLAMOUNT = "PayRoll Amount";
        public static string PAYROLLCONDITION = "PayRoll Condition";
        public static string PAYROLLCATEGORY = "PayRoll Category";
        public static string CONTRIBUTIONREGISTER = "ContributionRegister";
        public static string COMPUTATION = "COMPUTATION";
        public static string PERCENTAGE = "PERCENTAGE";
        public static string TRAVELCLAIM = "TRAVELCLAIM";
        public static string CURRENCY = "Currency";
        public static string AMOUNT = "AMOUNT";

        public static string EMPLOYEECONTRIBUTION = "EMPLOYEE CONTRIBUTION";
        public static string SALARYPAYMENTS = "BASIC SALARY";
        public static string EMPLOYERCONTRIBUTION = "EMPLOYER CONTRIBUTION";     

        public static string LOOKUPCATEGORY = "LeaveType";

        public static int PRIVILEGELEAVE = 1029;
        //public static int CASUALLEAVE = 1030;
        //public static int SICKLEAVE = 1031;
        public static int MATERNITYLEAVE = 1032;
        public static int HALFPAYLEAVE = 1033;
        public static int QUARANTINELEAVE = 1034;
        public static int STUDYLEAVE = 1035;
        public static int PATERNITYLEAVE = 1036;
        public static int COMPENSATORYLEAVE = 1037;
        //public static int PAIDLEAVE = 1049;
        public static int EARNEDLEVAE = 1113;
        public static int RESTRICTEDHOLIDAY = 1114;
        public static int HALFDAYLEAVE = 2440;
        public static int LOSSOFPAY = 2464;
        public static int MEDICALTREATMENTLEAVE = 2489;
        public static int DOCUMENTTYPEID = 1112;
        public static string BASICSALARYCOMPONENT = "BASIC";

        // Travel Claim Categories
        public const string AIRFARE = "AIRFARE";
        public const string VISA = "VISA";
        public const string ACCOMMODATION = "ACCOMMODATION";
        public const string TAXILOCAL = "TAXILOCAL";
        public const string TAXIOVERSEAS = "TAXIOVERSEAS";
        public const string FOODBILLSLOCAL = "FOODBILLSLOCAL";
        public const string FOODBILLSOVERSEAS = "FOODBILLSOVERSEAS";
        public const string OTHEREXPENSES = "OTHEREXPENSES";

        /* Config Keys End */

        public static string TRAVELCLAIMSAVED = "SAVED";
        public static string TRAVELCLAIMSUBMITTED = "SUBMITTED";
        public static string TRAVELCLAIMAPPROVED = "APPROVED";
        public static string TRAVELCLAIMREJECTED = "REJECTED";

        /* Travel Claim DocumentTypeId */

        public static int TRAVELCLAIMDOCUMENTID = 2581;




        //public static int PAIDLEAVE(int BRANCHID)
        //{
        //    return Convert.ToInt32(ConfigurationManager.AppSettings["PAIDLEAVE_" + BRANCHID.ToString()]);
        //}

        //public static int SICKLEAVE(int BRANCHID)
        //{
        //    return Convert.ToInt32(ConfigurationManager.AppSettings["SICKLEAVE_" + BRANCHID.ToString()]);
        //}

        //public static int CASUALLEAVE(int BRANCHID)
        //{
        //    return Convert.ToInt32(ConfigurationManager.AppSettings["CASUALLEAVE_" + BRANCHID.ToString()]);
        //}

        /* for generating app keys
        Set nocount On;
        Select * into #TempLookup From Config.Lookup 
        Where LookupCode in ('CASUALLEAVE','PAIDLEAVE','SICKLEAVE')
        Select * From #TempLookup


        Declare @lookupId smallint;
        Declare @lookupCode nvarchar(20);
        Declare @branchId int;
        While (Select Count(0) From #TempLookup) > 0
        Begin
            Select Top(1) @lookupId = LookUpID, @lookupCode = LookUpCode, @branchId = BranchId From #TempLookup Order By LookupCategory asc
            --print 'public static string ' + UPPER(@lookupCode) + '_' + convert(nvarchar(20), @branchId) + ' = "' + convert(nvarchar(20), @lookupId) + '";'
	        print '<add key="' + UPPER(@lookupCode) + '_' + convert(nvarchar(20), @branchId) + '" value="' + convert(nvarchar(20), @lookupId) + '" />'

            Delete From #TempLookup Where LookUpID = @lookupId;
        End

        Drop table #TempLookup
        Set nocount Off; 
        */

        public static DateTime SINGAPORETIME
        {
            get
            {
                string nzTimeZoneKey = "SE Asia Standard Time";
                TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, nzTimeZone);
            }

        }

        public static string RAZORDATEFORMAT = "{0:dd/MM/yyyy}";



    }
}