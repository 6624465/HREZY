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
                empHeader = empHeader.Where(x => x.FirstName.Contains(EmployeeName) || x.LastName.Contains(EmployeeName) || x.MiddleName.Contains(EmployeeName));
            }

            if (EmployeeType != null)
                empHeader = empHeader.Where(x => x.IDType == EmployeeType);

            return empHeader;
        }

        public static IQueryable<EmployeeWorkDetail> AdvSearchEmpWorkDetailWhere(this IQueryable<EmployeeWorkDetail> empWorkDetail, DateTime? DOJ, int? Designation, int BranchId, string RoleCode)
        {
            if (DOJ.HasValue)
            {
                empWorkDetail = empWorkDetail.Where(x => DbFunctions.TruncateTime(x.JoiningDate) == DbFunctions.TruncateTime(DOJ.Value));
            }
            if (RoleCode == UTILITY.ROLE_SUPERADMIN)
            {
                empWorkDetail = empWorkDetail.Where(x => x.DesignationId == Designation);
            }
            if (Designation != null)
                empWorkDetail = empWorkDetail.Where(x => x.DesignationId == Designation && x.BranchId == BranchId);

            return empWorkDetail;
        }

        public static IQueryable<EmployeeLeaveList> empLeaveListWhere(this IQueryable<EmployeeLeaveList> empLeaveList, string RoleCode, int BranchID, int EmployeeID, ref string viewName)
        {
            if (RoleCode == UTILITY.ROLE_SUPERADMIN)
            {
                viewName = "AppliedLeaveList";
                return empLeaveList;
            }
            else if (RoleCode == UTILITY.ROLE_ADMIN)
            {
                viewName = "AppliedLeaveList";
                return empLeaveList.Where(x => x.BranchId == BranchID);
            }
            else if (RoleCode == UTILITY.ROLE_EMPLOYEE)
            {
                viewName = "EmployeeLeaveList";
                return empLeaveList.Where(x => x.BranchId == BranchID && x.EmployeeId == EmployeeID);
            }

            return empLeaveList;
        }

        public static IQueryable<Leave> leaveWhere(this IQueryable<Leave> leaves, int BranchID, string RoleCode)
        {
            if (RoleCode == UTILITY.ROLE_SUPERADMIN)
                return leaves;
            else if (RoleCode == UTILITY.ROLE_ADMIN)
                return leaves.Where(x => x.BranchId == BranchID);

            return leaves;
        }

        public static string leaveWhereBy(this DateTime dateTime, DateTime DateTime)
        {

            return "";
        }

        public static IQueryable<EmployeeLeaveList> Between(this IQueryable<EmployeeLeaveList> empLeaveLists, DateTime FromDt, DateTime ToDt)
        {
            return empLeaveLists.Where(x => (DbFunctions.TruncateTime(x.FromDate) <= FromDt.Date && DbFunctions.TruncateTime(x.ToDate) >= FromDt.Date) ||
                                            (DbFunctions.TruncateTime(x.FromDate) >= ToDt.Date && DbFunctions.TruncateTime(x.ToDate) <= ToDt.Date));
        }


    }
}