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
        public EmployeePersonalDetail empPersonalDetail { get; set; }
        public EmployeeWorkDetail empWorkDetail { get; set; }
        public EmployeeDocumentDetail empDocumentDetail { get; set; }
        public Address address { get; set; }
    }
}