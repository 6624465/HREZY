using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Models;

namespace HR.Web.Helpers
{
    public static class SelectListItemHelper
    {
        public static IEnumerable<System.Web.Mvc.SelectListItem> EmployeeType()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.LookUps
                                .Where(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEETYPE)
                                .Select(x => new System.Web.Mvc.SelectListItem {
                                    Text = x.LookUpDescription,
                                    Value = x.LookUpCode
                                }).ToList<System.Web.Mvc.SelectListItem>().AsEnumerable();
            }
        }
    }
}