using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                                    Value = x.LookUpCode
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
    }
}