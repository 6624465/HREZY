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
        //public IEnumerable<EmployeeDocumentVm> empDocumentDetail { get; set; }
        public EmployeeAddress address { get; set; }

        public HttpPostedFileBase UIDCard { get; set; }
        public HttpPostedFileBase EducationDocument { get; set; }
        public HttpPostedFileBase ExperienceLetters { get; set; }
        public HttpPostedFileBase ProjectDocuments { get; set; }
        public HttpPostedFileBase OtherDocuments { get; set; }
    }

    public class EmployeeDocumentVm
    {                
        public int DocumentType { get; set; } 
        public string DocumentDescription { get; set; }
        public HttpPostedFileBase Document { get; set; }
    }
}