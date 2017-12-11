using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class BranchLeaveVm
    {
        public Leave leave { get; set; }
        public WeekendPolicy weekendPolicy { get; set; }
    }
}