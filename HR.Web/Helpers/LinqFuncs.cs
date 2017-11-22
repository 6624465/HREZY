using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Models;
using System.Data.Entity;

namespace HR.Web
{
    public static class LinqFuncs
    {
        public static IQueryable<EmployeeHeader> AdvSearchEmpHeaderWhere(this IQueryable<EmployeeHeader> empHeader, string EmployeeName, int? EmployeeType)
        {
            if (!string.IsNullOrWhiteSpace(EmployeeName))
            {
                empHeader = empHeader.Where(x => x.FirstName == EmployeeName || x.LastName == EmployeeName || x.MiddleName == EmployeeName);
            }

            if (EmployeeType != null)
                empHeader = empHeader.Where(x => x.IDType == EmployeeType);

            return empHeader;
        }

        public static IQueryable<EmployeeWorkDetail> AdvSearchEmpWorkDetailWhere(this IQueryable<EmployeeWorkDetail> empWorkDetail, DateTime? DOJ, int? Designation)
        {
            if (DOJ.HasValue)
            {
                empWorkDetail = empWorkDetail.Where(x => x.JoiningDate.Date == DOJ.Value.Date);
            }

            if (Designation != null)
                empWorkDetail = empWorkDetail.Where(x => x.DesignationId == Designation);

            return empWorkDetail;
        }

        public static IQueryable<EmployeeLeaveList> empLeaveListWhere(this IQueryable<EmployeeLeaveList> empLeaveList, string RoleCode, int BranchID, int EmployeeID, ref string viewName)
        {
            if (RoleCode == "SuperAdmin")
            {
                viewName = "AppliedLeaveList";
                return empLeaveList;
            }
            else if (RoleCode == "Admin")
            {
                viewName = "AppliedLeaveListAdmin";
                return empLeaveList.Where(x => x.BranchId == BranchID);
            }
            else if (RoleCode == "Employee")
            {
                viewName = "EmployeeLeaveList";
                return empLeaveList.Where(x => x.BranchId == BranchID && x.EmployeeId == EmployeeID);
            }

            return empLeaveList;
        }


    }
}