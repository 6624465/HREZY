using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class PayrollBatchVm
    {
        public PayslipBatchHeader payslipBatchHeader { get; set; }
        public List<PayslipBatchDetail> payslipBatchDetail { get; set; }
    }
}