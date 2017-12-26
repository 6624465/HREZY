using HR.Web.BusinessObjects.LookUpMaster;
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
        LookUpBO lookUpBO = null;
        public ConfigController()
        {
            lookUpBO = new LookUpBO(SESSIONOBJ);
        }

        #region Designation
        public ActionResult EmployeeDesignationList()
        {
            var EmployeeData = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDESIGNATION && x.IsActive == true)
       .ToList().AsEnumerable();
            return View(EmployeeData);
        }
        [HttpGet]
        public PartialViewResult GetEmployeeDesignation(int lookupID)
        {
            if (lookupID != -1)
            {
                var employeeDesign = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView("GetEmployeeDesignation", employeeDesign);

            }
            else
                return PartialView("GetEmployeeDesignation", new LookUp { LookUpID = -1 });
        }

        [HttpPost]
        public ActionResult SaveEmployeeDesignation(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                
                var _lookupObj = lookUpBO.GetByProperty(x => x.LookUpID == lookup.LookUpID && x.IsActive == true);
                lookUpBO.Add(_lookupObj);
            }
            else { 
                lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEEDESIGNATION;
            lookUpBO.Add(lookup);
            }
            return RedirectToAction("EmployeeDesignationList");
        }
    
        public ActionResult DesignationDelete(int lookupid)
        {
            
            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeDesignationList");
        }

        #endregion



        #region EmployeeType
        [HttpGet]
        public ActionResult EmployeeTypeList()
        {
            var list = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEETYPE && x.IsActive == true).ToList().AsEnumerable();
             return View(list);
        }

        [HttpGet]

        public PartialViewResult GetEmployeeType(int lookupID)
        {
            if (lookupID != -1)
            {
                var employeeType = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(employeeType);

            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }

        [HttpPost]
        public ActionResult SaveEmployeeType(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEETYPE;
            lookUpBO.Add(lookup);
            return RedirectToAction("EmployeeTypeList");
        }
        [HttpPost]
        public ActionResult EmployeeTypeDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeTypeList");
        }
        #endregion
        #region department
        public ActionResult EmployeeDepartmentList()
        {
            var list = lookUpBO.GetListByProperty
            (x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEDEPARTMENT && x.IsActive == true).ToList().AsEnumerable();
            return View(list);

        }
        [HttpGet]
        public PartialViewResult GetEmployeeDepartment(int lookupID)
        {
            if (lookupID != -1)
            {
                var employeeDepartment = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(employeeDepartment);
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeDepartment(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEEDEPARTMENT;
            lookUpBO.Add(lookup);
            return RedirectToAction("EmployeeDepartmentList");
        }
       
        public ActionResult DepartmentDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeDepartmentList");
        }

        #endregion
        #region EmployeeStatus
        public ActionResult EmployeeStatusList()
        {
            var list = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEESTATUS && x.IsActive == true).ToList().AsEnumerable();
            return View(list);
        }
        [HttpGet]
        public PartialViewResult GetEmployeeStatus(int lookupID)
        {
            if (lookupID != -1)
            {
                var employeeStatus = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(employeeStatus);
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeStatus(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEESTATUS;
            lookUpBO.Add(lookup);
            return RedirectToAction("EmployeeStatusList");
        }
      
        public ActionResult EmployeeStatusDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeStatusList");
        }
        #endregion

        #region Marital Status
        public ActionResult MaritalStatusList()
        {
            var marital_status = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEMARITALSTATUS && x.IsActive == true).ToList().AsEnumerable();
            return View(marital_status);

        }
        [HttpGet]
        public PartialViewResult GetEmployeeMaritalStatus(int lookupID)
        {
            if (lookupID != -1)
            {
                var marital_status_data = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(marital_status_data);
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeMaritalStatus(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEEMARITALSTATUS;
            lookUpBO.Add(lookup);
            return RedirectToAction("MaritalStatusList");
        }
       
        public ActionResult MaritalStatusDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("MaritalStatusList");
        }
        #endregion
        #region LeaveList
        public ActionResult LeaveList()
        {
            var leaveType = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEELEAVETYPE && x.IsActive == true).ToList().AsEnumerable();
            return View(leaveType);
        }
        [HttpGet]
        public PartialViewResult GetLeaveType(int lookupID)
        {
            if (lookupID != -1)
            {
                var leave_data = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(leave_data);
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveLeaveType(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEELEAVETYPE;
            lookUpBO.Add(lookup);
            return RedirectToAction("LeaveList");
        }
        
        public ActionResult LeaveListDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("LeaveList");
        }
        #endregion
        #region PaymentType
        public ActionResult PaymentTypeList()
        {
            var paymentType = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEEPAYMENTTYPE && x.IsActive == true).ToList().AsEnumerable();
            return View(paymentType);
        }
        [HttpGet]
        public PartialViewResult GetPaymentType(int lookupID)
        {
            if (lookupID != -1)
            {
                var payment_data = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(payment_data);
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SavePaymentType(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEEPAYMENTTYPE;
            lookUpBO.Add(lookup);
            return RedirectToAction("PaymentTypeList");
        }
     
        public ActionResult PaymentTypeDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("PaymentTypeList");
        }
        #endregion
    }


}