using HR.Web.BusinessObjects.LookUpMaster;
using HR.Web.BusinessObjects.LeaveMaster;
using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{

    [SessionFilter]
    [ErrorHandler]
    public class ConfigController : BaseController
    {
        LookUpBO lookUpBO = null;
        OtherLeaveBO otherLeaveBo = null;
        public ConfigController()
        {
            lookUpBO = new LookUpBO(SESSIONOBJ);
            otherLeaveBo = new OtherLeaveBO(SESSIONOBJ);
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
            lookup.LookUpCategory = UTILITY.CONFIG_EMPLOYEEDESIGNATION;
            
            lookUpBO.Add(lookup);
            return RedirectToAction("EmployeeDesignationList");
        }

        public ActionResult DesignationDelete(int lookupid)
        {
            
            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeDesignationList");
        }
        public bool IsEmployeeDesignationExist(string Designation)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == Designation.ToUpper()) && x.IsActive==true && (x.LookUpCategory==UTILITY.CONFIG_EMPLOYEEDESIGNATION)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
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
    
        public ActionResult EmployeeTypeDelete(int lookupid)
        {

            lookUpBO.Delete(lookupid);
            return RedirectToAction("EmployeeTypeList");
        }
        public bool IsEmployeeTypeExist(string EmployeeType)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == EmployeeType.ToUpper()) && x.IsActive==true &&(x.LookUpCategory==UTILITY.CONFIG_EMPLOYEETYPE)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
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
        public bool IsEmployeeDepartmentExist(string Department)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == Department.ToUpper())&& x.IsActive==true && (x.LookUpCategory==UTILITY.CONFIG_EMPLOYEEDEPARTMENT)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
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
        public bool IsEmployeeStatusExist(string Status)
        {
            var list = lookUpBO.GetListByProperty(x =>( x.LookUpCode.ToUpper() == Status.ToUpper()) && x.IsActive==true && (x.LookUpCategory==UTILITY.CONFIG_EMPLOYEESTATUS)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
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
        public bool IsMaritalStatusExist(string MaritalStatus)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == MaritalStatus.ToUpper())&& x.IsActive==true&&(x.LookUpCategory== UTILITY.CONFIG_EMPLOYEEMARITALSTATUS)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
        }
        #endregion
        #region LeaveList
        public ActionResult LeaveList()
        {
            var leaveType = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_EMPLOYEELEAVETYPE && x.IsActive == true && x.BranchId==BRANCHID).ToList().AsEnumerable();
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
            lookup.BranchId = BRANCHID;
            lookUpBO.Add(lookup);
            otherLeaveBo.AddLookUp(lookup);
            return RedirectToAction("LeaveList");
        }
        
        public ActionResult LeaveListDelete(LookUp lookup)
        {

            lookUpBO.Delete(lookup.LookUpID);
            otherLeaveBo.DeleteLookUp(lookup);
            return RedirectToAction("LeaveList");
        }
        public bool IsLeaveTypeExist(string LeaveType)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == LeaveType.ToUpper()) && x.IsActive==true && (x.LookUpCategory==UTILITY.CONFIG_EMPLOYEELEAVETYPE) && x.BranchId==BRANCHID).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
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
        public bool IsPaymentTypeExist(string PaymentType)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == PaymentType.ToUpper()) && x.IsActive==true && (x.LookUpCategory==UTILITY.CONFIG_EMPLOYEEPAYMENTTYPE)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
        }
        #endregion

        #region Travel Claim
        
        public ViewResult TravelClaimList()
        {
            var TravelClaim = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_TRAVELCLAIM && x.IsActive == true).ToList().AsEnumerable();
            return View(TravelClaim);
        }

        public PartialViewResult GetTravelClaim(int lookUpID)
        {
            if (lookUpID == -1)
            {
                return PartialView(new LookUp { LookUpID = -1});
            }
            else
            {
                var tarvelclaim_data = lookUpBO.GetByProperty(x => x.LookUpID == lookUpID && x.IsActive == true);
                return PartialView(tarvelclaim_data);
            }
               
        }
        [HttpPost]
        public ActionResult SaveTravelClaim(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_TRAVELCLAIM;
            lookUpBO.Add(lookup);
            return RedirectToAction("TravelClaimList");
        }
        public ActionResult TravelClaimDelete(int lookupid)
        {
            lookUpBO.Delete(lookupid);
            return RedirectToAction("TravelClaimList");
        }
        public bool IsTravelClaimExist(string travelclaim)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == travelclaim.ToUpper()) && x.IsActive == true && (x.LookUpCategory == UTILITY.CONFIG_TRAVELCLAIM)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
        }
        #endregion

            #region Currency
            public ViewResult CurrencyList()
        {
            var currencylist = lookUpBO.GetListByProperty(x => x.LookUpCategory == UTILITY.CONFIG_CURRENCY && x.IsActive == true).ToList().AsEnumerable();
            return View(currencylist);
        }
        public PartialViewResult GetCurrency(int lookupID)
        {
            if (lookupID == -1)
            {
                return PartialView(new LookUp { LookUpID = -1 });
            }
            else
            {
                var currency_data = lookUpBO.GetByProperty(x => x.LookUpID == lookupID && x.IsActive == true);
                return PartialView(currency_data);
            }
        }

        [HttpPost]
        public ActionResult SaveCurrency(LookUp lookup)
        {
            lookup.LookUpCategory = UTILITY.CONFIG_CURRENCY;
            lookUpBO.Add(lookup);
            return RedirectToAction("CurrencyList");
        }
        public ActionResult CurrencyDelete(int lookupid)
        {
            lookUpBO.Delete(lookupid);
            return RedirectToAction("CurrencyList");
        }
        public bool IsCurrencyExists(string currency)
        {
            var list = lookUpBO.GetListByProperty(x => (x.LookUpCode.ToUpper() == currency.ToUpper()) && x.IsActive == true && (x.LookUpCategory == UTILITY.CONFIG_CURRENCY)).ToList();
            int count = list.Count();
            return (count > 0 ? true : false);
        }

        #endregion
    }
}