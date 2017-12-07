using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.ViewModels
{
    public class WeekendPolicyVm 
    {
        // GET: WeekendPolicyVm
       public int BranchId { get; set; }
       public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }
        public String BranchName { get; set; }
    }
}