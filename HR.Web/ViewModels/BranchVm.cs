using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class BranchVm
    {
        public Branch branch { get; set; }
        public AddressVm address { get; set; }
    }
}