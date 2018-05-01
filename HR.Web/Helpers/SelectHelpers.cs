using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Controllers;
using HR.Web.Models;
using System.Web.Mvc;
using System.Globalization;

namespace HR.Web.Helpers
{
    public static class SelectListItemHelper
    {
        public static IEnumerable<System.Web.Mvc.SelectListItem> ConfigData(string key)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == key && x.IsActive == true)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEETYPE && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEMARITALSTATUS && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDESIGNATION && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDEPARTMENT && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_NOTICEPERIOD && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_PROHIBATIONPERIOD && x.IsActive == true)
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
                return dbCntx.Countries.OrderBy(x=>x.CountryName)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEPERIODICITY && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEYEAR && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVEPERIODTYPE && x.IsActive == true)
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
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_GRANTLEAVESCHEMETYPE && x.IsActive == true)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
        public static IEnumerable<SelectListItem> EmployeeLeaveType(int BranchID)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEELEAVETYPE && x.IsActive == true && x.BranchId == BranchID)
                                .Select(x => new SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> EmployeeList(int branchId, bool isReportAuth = false)
        {
            using (var dbCntx = new HrDataContext())
            {
                if (isReportAuth)
                {
                    return dbCntx.EmployeeHeaders.Where(x => x.IsReportingAuthority == isReportAuth &&
                                    x.IsActive == true && x.BranchId == branchId)
                        .Select(x => new SelectListItem
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.EmployeeId.ToString()
                        }).ToList();
                }
                else
                    return dbCntx.EmployeeHeaders.Where(x => x.BranchId == branchId)
                            .Select(x => new SelectListItem
                            {
                                Text = x.FirstName + " " + x.LastName,
                                Value = x.EmployeeId.ToString()
                            }).ToList();
            }
        }


        public static IEnumerable<SelectListItem> CountryListById()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.Branches.Select(x => new SelectListItem
                {
                    Value = x.BranchID.ToString(),
                    Text = x.BranchName
                }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> Branches()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.Branches.Select(x => new SelectListItem
                {
                    Value = x.BranchID.ToString(),
                    Text = x.BranchName
                }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> EmployeeList(int BranchId)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmployeeHeaders.Where(x => x.BranchId == BranchId && x.IsActive == true)
                    .Select(x => new SelectListItem
                    {
                        Value = x.EmployeeId.ToString(),
                        Text = x.FirstName
                    }).OrderBy(x => x.Text).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> PayRollCategory()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.PAYROLLCATEGORY)
                    .Select(x => new SelectListItem
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> TravelExpenses()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.TRAVELCLAIM && x.IsActive == true)
                    .Select(x => new SelectListItem
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> CurrnyList()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.CURRENCY && x.IsActive == true)
                    .Select(x => new SelectListItem
                    {
                        Value = x.LookUpDescription,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> PayRollCondition()
        {

            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.PAYROLLCONDITION)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> PayRollAmount()
        {

            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.PAYROLLAMOUNT)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> PayRollContribution()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.PAYROLLCONTRIBUTION)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }

        public static IEnumerable<SelectListItem> ContributionRegister()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.CONTRIBUTIONREGISTER && x.IsActive == true)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable().OrderBy(x => x.Text);
            }
        }


        public static IEnumerable<SelectListItem> Computation()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.LookUps.Where(x => x.LookUpCategory == UTILITY.COMPUTATION)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.LookUpCode,
                        Text = x.LookUpDescription
                    }).ToList().AsEnumerable();
            }
        }
        public static IEnumerable<SelectListItem> SalaryStructure(int branchId)
        {
            using (HrDataContext dbcntx = new HrDataContext())
            {
                return dbcntx.SalaryStructureHeaders.Where(x => x.IsActive == true && x.BranchId == branchId).Select(x => new SelectListItem
                {
                    Value = x.StructureID.ToString(),
                    Text = x.Code
                }).ToList().AsEnumerable();
            }
        }
        public static IEnumerable<SelectListItem> LeaveSession()
        {

            List<SelectListItem> listItem = new List<SelectListItem>() {
                                            new SelectListItem() {
                                                Text="AM",
                                                Value="AM"
                                            },
                                             new SelectListItem() {
                                                Text="PM",
                                                Value="PM"
                                            },
                                        };

            return listItem;
        }
        public static IEnumerable<SelectListItem> AccountType()
        {

            List<SelectListItem> listItem = new List<SelectListItem>() {
                                            new SelectListItem() {
                                                Text="SAVINGS",
                                                Value="SAVINGS"
                                            },
                                             new SelectListItem() {
                                                Text="CURRENT",
                                                Value="CURRENT"
                                            },
                                        };

            return listItem;
        }
        public static IEnumerable<SelectListItem> Deductions()
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                List<SelectListItem> listItem = new List<SelectListItem>() {
                                            new SelectListItem() {
                                                Text="BASIC SALARY",
                                                Value="BASIC SALARY"
                                            },
                                             new SelectListItem() {
                                                Text="EMPLOYER CONTRIBUTION",
                                                Value="EMPLOYER CONTRIBUTION"
                                            },
                                             new SelectListItem() {
                                                 Text="EMPLOYEE CONTRIBUTION",
                                                 Value="EMPLOYEE CONTRIBUTION"
                                             }
                                        };
                return listItem;
            }
        }

        public static IEnumerable<SelectListItem> GetMonths()
        {
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            string currentMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month);            

            List<SelectListItem> listItem = new List<SelectListItem>();
            SelectListItem sItem = null;
            int loopCount = 0;
            for(var i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(names[i]))
                    continue;

                loopCount++;
                sItem = new SelectListItem {
                    Text = names[i],
                    Value = loopCount.ToString(),
                    Selected = names[i] == currentMonth ? true : false
                };

                listItem.Add(sItem);
            }            

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetYears()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            var currentYear = DateTime.Now.Year;

            var obj2 = new SelectListItem
            {
                Value = (currentYear - 1).ToString(),
                Text = (currentYear - 1).ToString(),
                Selected = false
            };
            listItem.Add(obj2);

            var obj1 = new SelectListItem {
                Value = currentYear.ToString(),
                Text = currentYear.ToString(),
                Selected = true
            };
            listItem.Add(obj1);
            

            return listItem;
        }

        public static IEnumerable<SelectListItem> PayslipDownload()
        {
            List<SelectListItem> listItem = new List<SelectListItem>() {
                                            new SelectListItem() {
                                                Text="Download Pay Slip",
                                                Value="Download Pay Slip"
                                            },
                                             new SelectListItem() {
                                                Text="Download Pay Slip",
                                                Value="Download Pay Slip"
                                            },
                                        };

            return listItem;
        }
        public static IEnumerable<System.Web.Mvc.SelectListItem> SalutationType()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPSALUTATIONTYPE && x.IsActive == true)
                                .Select(x => new System.Web.Mvc.SelectListItem
                                {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpID.ToString()
                                })
                                .OrderBy(x => x.Text)
                                .ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
    }
}