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
        public Address address { get; set; }

        public List<EmployeeDocumentVm> empDocument { get; set; }

        public List<Documents> documents { get; set; }
        //public HttpPostedFileBase UIDCard { get; set; }
        //public HttpPostedFileBase EducationDocument { get; set; }
        //public HttpPostedFileBase ExperienceLetters { get; set; }
        //public HttpPostedFileBase ProjectDocuments { get; set; }
        //public HttpPostedFileBase OtherDocuments { get; set; }        
    }


    public class EmpDirectoryResultVm
    {
        public IEnumerable<usp_EmployeeDetail_Result3> employeeVm { get; set; }
        public EmpSearch empSearch { get; set; }
        public int count { get; set; }

        public decimal PagerLength { get; set; }
    }


    public class Documents
    {
       public string fileName { get; set; }
    }

    public class EmpDirectoryVm
    {
        public IEnumerable<EmployeeListVm> employeeVm { get; set; }
        public EmpSearch empSearch { get; set; }
        public int count { get; set; }

        public decimal PagerLength { get; set; }
    }

    public class EmployeeDocumentVm
    {
        public int DocumentDetailId { get; set; }
        public int DocumentType { get; set; } 
        public string DocumentDescription { get; set; }
        public HttpPostedFileBase Document { get; set; }
        public string fileName { get; set; }
    }

    public class EmpSearch
    {
        public string EmployeeName { get; set; }
        public DateTime? DOJ { get; set; }
        public string EmployeeNumber { get; set; }
        public int? Designation { get; set; }
        public int? EmployeeType { get; set; }

        public int pageNumber { get; set; }
    }
}