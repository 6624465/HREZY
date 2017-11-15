using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Models;

namespace HR.Web.ViewModels
{
    public class EmployeeVm
    {
        public EmployeeHeader empHeader { get; set; }
        public List<EmployeePersonalDetail> empPersonalDetail { get; set; }
        public List<EmployeeWorkDetail> empWorkDetail { get; set; }
        public List<EmployeeDocumentDetail> empDocumentDetail { get; set; }
    }
}