using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{

    public class AppliedLeaveListVm
    {
        public string BranchName { get; set; }
        public int BranchID { get; set; }
        public IEnumerable<EmpLeaveListVm> employeeLeaveList { get; set; }
    }
    public class LeaveVm
    {
        public LookUp lookup { get; set; }        
        public LeaveHeader leaveHeader { get; set; }
        public IEnumerable<LeaveHeaderVm> lvmList { get; set; }
       

    }

    public class EmpLeaveListVm
    {
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class LeaveHeaderVm
    {
        public int LeaveHeaderID { get; set; }
        public int BranchID { get; set; }
        public Nullable<short> LeaveYear { get; set; }
        public string LeaveYearDescription { get; set; }
        
        public Nullable<int> PeriodicityType { get; set; }
        public string PeriodicityTypeDescription { get; set; }
        public Nullable<int> PeriodType { get; set; }
        public string PeriodTypeDescription { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> LeaveSchemeType { get; set; }
      
       public string LeaveSchemeTypeDescription { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

    }
    
    public class GrantLeaveListVm
    {
        public DateTime ToDate { get; set; }

        public DateTime FromDate { get; set; }

        public string Name { get; set; }

        public int EmployeeId { get; set; }
        public string Remarks { get; set; }
        public string Reason { get; set; }

        public int EmployeeLeaveID { get; set; }

        public string Status { get; set; }

    }
    


}