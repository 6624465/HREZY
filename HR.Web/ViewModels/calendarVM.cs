using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class calendarVM
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public string url { get; set; }


    }

    public class holidayVm {
        public List<calendarVM> calendarVM { get; set; }
        public HolidayList HolidayList { get; set; }

        public int BranchId { get; set; }
    }



}