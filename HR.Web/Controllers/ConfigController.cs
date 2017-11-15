using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HR.Web.Controllers
{

    [SessionFilter]
    public class ConfigController : BaseController
    {
        HrDataContext dbContext = new HrDataContext();

        public ActionResult GetDesignation()
        {
            var EmployeeData = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeDesignation").AsQueryable();
            return View(EmployeeData);
        }
        [HttpPost]
        public ActionResult GetDesignationById(int lookupId)
        {
            var id = dbContext.LookUps.Where(x => x.LookUpID == lookupId).FirstOrDefault();
            if (id == null)
            {
      
            }
            else
            { }
            return View();
        }
        public ActionResult EmployeeTypeList()
        {
            using (var dbContext = new HrDataContext())
            {
                var list = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeType").ToList().AsEnumerable();
                return View(list);
            }
        }
        public ActionResult EmpTypepaging(int page)
        {
            
                int pageSize = 5;
                int totalpages = 0;
                int totalRecords = 0;
            
            using (var dbctx = new HrDataContext())
            {

                    totalRecords = dbctx.LookUps.ToList().Count();
        
                    totalpages = (totalRecords / pageSize) + ((totalRecords % pageSize) > 0 ? 1 : 0);
   
                   var allList = dbctx.LookUps.OrderBy(a => a.LookUpID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return View(allList);
            }
           
        }
       
         
       
        public PartialViewResult GetEmployeeType(int lookupID)
        {
            using (var dbCntx = new HrDataContext())
            {
                var employeeType = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                return PartialView(employeeType);
            }
        }
        public ActionResult EmployeeDepartmentList()
        {
            var list = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeDepartment").AsQueryable();
            return View(list);
        }
    }
}