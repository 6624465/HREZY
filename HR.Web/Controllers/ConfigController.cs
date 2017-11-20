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


        #region Designation
        public ActionResult EmployeeDesignationList()
        {
            using (var dbContext = new HrDataContext())
            {
                var EmployeeData = dbContext.LookUps.Where(x => x.LookUpCategory == "EmployeeDesignation").ToList().AsEnumerable();
                return View(EmployeeData);
            }
        }
        [HttpGet]
        public PartialViewResult GetEmployeeDesignation(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var employeeDesign = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView("GetEmployeeDesignation",employeeDesign);
                }

            }
            else
                return PartialView("GetEmployeeDesignation", new LookUp { LookUpID = -1 });
        }

        [HttpPost]
        public ActionResult SaveEmployeeDesignation(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.IsActive = lookup.IsActive;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEEDESIGNATION,
                        IsActive = lookup.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("EmployeeDesignationList");
        }

        #endregion

        

        #region EmployeeType
        [HttpGet]
        public ActionResult EmployeeTypeList()
        {
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.LookUps.Where(x => x.LookUpCategory == "EmployeeType").ToList().AsEnumerable();
                return View(list);
            }
        }

        [HttpGet]

        public PartialViewResult GetEmployeeType(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var employeeType = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(employeeType);
                }
             
            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }

        [HttpPost]
        public ActionResult SaveEmployeeType(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.IsActive = lookup.IsActive;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEETYPE,
                        IsActive = lookup.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("EmployeeTypeList");
        }

        #endregion
        #region department
        public ActionResult EmployeeDepartmentList()
        {
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.LookUps.Where(x => x.LookUpCategory == "EmployeeDepartment").ToList().AsEnumerable();
                return View(list);
            }
                
        }
        [HttpGet]
        public PartialViewResult GetEmployeeDepartment(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var employeeDepartment = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(employeeDepartment);
                }

            }
            else
                return PartialView( new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeDepartment(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.IsActive = lookup.IsActive;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEEDEPARTMENT,
                        IsActive = lookup.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("EmployeeDepartmentList");
        }
        #endregion
        #region EmployeeStatus
        public ActionResult EmployeeStatusList()
        {
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.LookUps.Where(x => x.LookUpCategory == "EmployeeStatus").ToList().AsEnumerable();
                return View(list);
            }

        }
        [HttpGet]
        public PartialViewResult GetEmployeeStatus(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var employeeStatus = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(employeeStatus);
                }

            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeStatus(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.IsActive = lookup.IsActive;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEESTATUS,
                        IsActive = lookup.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("EmployeeStatusList");
        }
        #endregion

        #region Marital Status
        public ActionResult MaritalStatusList()
        {
            using (var dbContext = new HrDataContext())
            {
                var marital_status = dbContext.LookUps.Where(x => x.LookUpCategory == "MartialSatus").ToList().AsEnumerable();
                return View(marital_status);
            }
            
        }
        [HttpGet]
        public PartialViewResult GetEmployeeMaritalStatus(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var marital_status_data = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(marital_status_data);
                }

            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveEmployeeMaritalStatus(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.IsActive = lookup.IsActive;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEEMARITALSTATUS,
                        IsActive = lookup.IsActive,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("MaritalStatusList");
        }
        #endregion
        #region LeaveList
        public ActionResult LeaveList()
        {
            using (var dbContext = new HrDataContext())
            {
                var leaveType = dbContext.LookUps.Where(x => x.LookUpCategory == "LeaveType").ToList().AsEnumerable();
                return View(leaveType);
            }

        }
        [HttpGet]
        public PartialViewResult GetLeaveType(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var leave_data = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(leave_data);
                }

            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SaveLeaveType(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEELEAVETYPE,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("LeaveList");
        }
        #endregion
        #region PaymentType
        public ActionResult PaymentTypeList()
        {
            using (var dbContext = new HrDataContext())
            {
                var paymentType = dbContext.LookUps.Where(x => x.LookUpCategory == "PaymentType").ToList().AsEnumerable();
                return View(paymentType);
            }

        }
        [HttpGet]
        public PartialViewResult GetPaymentType(int lookupID)
        {
            if (lookupID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var payment_data = dbCntx.LookUps.Where(x => x.LookUpID == lookupID).FirstOrDefault();
                    return PartialView(payment_data);
                }

            }
            else
                return PartialView(new LookUp { LookUpID = -1 });
        }
        [HttpPost]
        public ActionResult SavePaymentType(LookUp lookup)
        {
            if (lookup.LookUpID != -1)
            {
                using (var dbCntx = new HrDataContext())
                {
                    var _lookupObj = dbCntx.LookUps.Where(x => x.LookUpID == lookup.LookUpID).FirstOrDefault();

                    _lookupObj.LookUpCode = lookup.LookUpCode;
                    _lookupObj.LookUpDescription = lookup.LookUpDescription;
                    _lookupObj.ModifiedBy = USERID;
                    _lookupObj.ModifiedOn = DateTime.Now;

                    dbCntx.SaveChanges();
                }
            }
            else
            {
                using (var dbCntx = new HrDataContext())
                {
                    var lookupObj = new LookUp
                    {
                        LookUpCode = lookup.LookUpCode,
                        LookUpDescription = lookup.LookUpDescription,
                        LookUpCategory = UTILITY.CONFIG_EMPLOYEEPAYMENTTYPE,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = USERID,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = USERID
                    };

                    dbCntx.LookUps.Add(lookupObj);
                    dbCntx.SaveChanges();
                }

            }
            return RedirectToAction("PaymentTypeList");
        }
        #endregion
    }


}