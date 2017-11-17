using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Controllers;
using HR.Web.Models;

namespace HR.Web.Helpers
{
    public static class SelectListItemHelper
    {
        public static IEnumerable<System.Web.Mvc.SelectListItem> ConfigData(string key)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == key)
                                .Select(x => new System.Web.Mvc.SelectListItem {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpCode,
                                    
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> EmployeeType()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEETYPE)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> MaritalStatus()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEMARITALSTATUS)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> Designation()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDESIGNATION)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> Department()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDEPARTMENT)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> NoticePeriod()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_NOTICEPERIOD)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> ProhibationPeriod()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_PROHIBATIONPERIOD)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> CountryList()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.Countries
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.CountryName,
                                    Value = x.CountryCode
                                }).ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }   
        public static IEnumerable<System.Web.Mvc.SelectListItem> PeriodicityTypeList()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEPERIODICITY)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        public static IEnumerable<System.Web.Mvc.SelectListItem> LeaveYearList()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEYEAR)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        public static IEnumerable<System.Web.Mvc.SelectListItem> PeriodTypeList()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEPERIODTYPE)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        public static IEnumerable<System.Web.Mvc.SelectListItem> grantSchemeType()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVESCHEMETYPE)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        public static IEnumerable<System.Web.Mvc.SelectListItem> EmployeeLeaveType()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEELEAVETYPE)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        
        public static IEnumerable<System.Web.Mvc.SelectListItem> EmployeeList()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmployeeHeaders
                        .Select(x => new System.Web.Mvc.SelectListItem {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.EmployeeId.ToString()
                        }).ToList();
            }
        }     
        public static IEnumerable<System.Web.Mvc.SelectListItem> CountryListById()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.Countries
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.CountryName,
                                    Value = x.CountryId.ToString()
                                }).ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
    }
}