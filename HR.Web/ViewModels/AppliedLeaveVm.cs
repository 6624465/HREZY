using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class AppliedLeaveVm
    {
        //public LeaveHeader leaveHeader { get; set; }
        public IEnumerable<LeaveHeader> adminLeaveList { get; set; }
        public IEnumerable<LeaveHeader> SuperadminLeaveList { get; set; }
    }
}