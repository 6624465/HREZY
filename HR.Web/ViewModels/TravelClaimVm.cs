using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class TravelClaimVm
    {
        public TravelClaimHeader claimHeader { get; set; }
        public List<TravelClaimDetail> claimDetail { get; set; }
    }
}