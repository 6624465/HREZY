using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class EmployeeListVm
    {
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string JobTitle { get; set; }
        public string ContactNo { get; set; }
        public string PersonalEmailId { get; set; }
        public string OfficialEmailId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePic { get; set; }
        public int DocumentDetailID { get; set; }
        public int branchid { get; set; }
    }
}