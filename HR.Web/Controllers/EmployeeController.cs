﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HR.Web.Models;
using HR.Web.ViewModels;
using System.IO;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Linq.Expressions;
using HR.Web.BusinessObjects.Operation;
using HR.Web.BusinessObjects.Security;
using HR.Web.BusinessObjects.LookUpMaster;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{

    [SessionFilter]
    //[ErrorHandler]
    public class EmployeeController : BaseController
    {
        EmployeeHeaderBO empHeaderBO = null;
        EmployeePersonalDetailBO empPersonalDetailBO = null;
        EmployeeDocumentDetailBO empDocDetailBO = null;
        EmployeeWorkDetailBO empWorkDetailBO = null;
        AddressBO addressBO = null;
        UserBO userBo = null;
        LeaveTrasactionBO leaveTransactionBO = null;
        LookUpBO lookUpBO = null;
        LeaveTransBO leaveTransBO = null;
        SalaryStructureHeaderBO salaryStructureHeaderBO = null;
        EmployeeBankDetailBO empbankdetailBO = null;
        public EmployeeController()
        {

            empHeaderBO = new EmployeeHeaderBO(SESSIONOBJ);
            empPersonalDetailBO = new EmployeePersonalDetailBO(SESSIONOBJ);
            empDocDetailBO = new EmployeeDocumentDetailBO(SESSIONOBJ);
            empWorkDetailBO = new EmployeeWorkDetailBO(SESSIONOBJ);
            addressBO = new AddressBO(SESSIONOBJ);
            userBo = new UserBO(SESSIONOBJ);
            leaveTransactionBO = new LeaveTrasactionBO(SESSIONOBJ);
            lookUpBO = new LookUpBO(SESSIONOBJ);
            leaveTransBO = new LeaveTransBO(SESSIONOBJ);
            salaryStructureHeaderBO = new SalaryStructureHeaderBO(SESSIONOBJ);
            empbankdetailBO = new EmployeeBankDetailBO(SESSIONOBJ);
        }
        [HttpGet]
        public ViewResult employeedirectory()
        {
            using (var dbCntx = new HrDataContext())
            {
                ViewData["BranchId"] = BRANCHID;
                var empDirectoryVm = new EmpDirectoryVm
                {

                    empSearch = new EmpSearch { }
                };

                return View("employeedirectory", empDirectoryVm);
            }
        }

        [HttpGet]
        public JsonResult GetGridTileEmployees(int pageNumber)
        {
            int offSet = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skipRows = (pageNumber - 1) * offSet;

            using (var dbCntx = new HrDataContext())
            {
                var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                var query = dbCntx.usp_EmployeeDetail(BRANCHID, ROLECODE);

                var list = query.OrderByDescending(x => x.EmployeeId)
            .Skip(skipRows)
            .Take(offSet)
            .ToList()
            .AsEnumerable();

                var totalCount = dbCntx.usp_EmployeeDetail(BRANCHID, ROLECODE).Count();

                decimal pagerLength = decimal.Divide(Convert.ToDecimal(totalCount), Convert.ToDecimal(offSet));
                decimal pagnationRound = Math.Ceiling(Convert.ToDecimal(pagerLength));

                var empDirectoryVm = new EmpDirectoryResultVm
                {
                    employeeVm = list,
                    empSearch = new EmpSearch { },
                    count = totalCount,
                    PagerLength = pagnationRound
                };


                return Json(empDirectoryVm, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult empsearchajax(EmpSearch empSearch)
        {
            int offSet = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["appTableOffSet"]);
            int skipRows = (empSearch.pageNumber - 1) * offSet;
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.EmployeeHeaders.AdvSearchEmpHeaderWhere(empSearch.EmployeeName, empSearch.EmployeeType)
                            .Join(dbCntx.EmployeePersonalDetails,
                            a => a.EmployeeId, b => b.EmployeeId,
                            (a, b) => new { A = a, B = b })
                            .Join(dbCntx.EmployeeWorkDetails.AdvSearchEmpWorkDetailWhere(empSearch.DOJ, empSearch.Designation, BRANCHID, ROLECODE),
                            c => c.A.EmployeeId, d => d.EmployeeId,
                            (c, d) => new { C = c, D = d })
                            //.Join(dbCntx.Users,
                            //e => e.C.A.EmployeeId, f => f.EmployeeId,
                            //(e, f) => new { E = e, F = f })
                            //.Join(dbCntx.EmployeeDocumentDetails,
                            //g => g.E.C.A.EmployeeId, h => h.EmployeeId,
                            //(g, h) => new { G = g, H = h })
                            .Select(x => new EmployeeListVm
                            {
                                EmployeeId = x.C.A.EmployeeId,
                                EmployeeNo = x.C.A.IDNumber,
                                EmployeeName = x.C.A.FirstName,
                                JoiningDate = x.D.JoiningDate,
                                JobTitle = dbCntx.LookUps
                                            .Where(y => y.LookUpID == x.D.DesignationId)
                                            .FirstOrDefault().LookUpDescription,
                                ContactNo = x.C.B.EmergencyContactNumber,
                                PersonalEmailId = x.C.A.UserEmailId,
                                OfficialEmailId = x.C.A.UserEmailId,
                                DocumentDetailID = dbCntx.EmployeeDocumentDetails
                                .Where(z => z.DocumentType == UTILITY.DOCUMENTTYPEID && z.EmployeeId == x.C.A.EmployeeId).FirstOrDefault() == null ? 0 :
                                dbCntx.EmployeeDocumentDetails
                                .Where(z => z.DocumentType == UTILITY.DOCUMENTTYPEID && z.EmployeeId == x.C.A.EmployeeId).FirstOrDefault().DocumentDetailID,//x.H.DocumentDetailID,
                                DateOfBirth = x.C.B.DOB,
                                branchid = x.C.A.BranchId,
                                ProfilePic = dbCntx.EmployeeDocumentDetails
                                .Where(a => a.DocumentType == UTILITY.DOCUMENTTYPEID && a.EmployeeId == x.C.A.EmployeeId).FirstOrDefault() == null ? "" :
                                dbCntx.EmployeeDocumentDetails
                                .Where(a => a.DocumentType == UTILITY.DOCUMENTTYPEID && a.EmployeeId == x.C.A.EmployeeId).FirstOrDefault().FileName
                            });
                var emplist = list.Where(x => x.branchid == BRANCHID).ToList();
                var query = emplist.OrderByDescending(x => x.EmployeeId).Skip(skipRows).Take(offSet).ToList().AsEnumerable();

                var totalCount = emplist.Count();

                decimal pagerLength = decimal.Divide(Convert.ToDecimal(totalCount), Convert.ToDecimal(offSet));
                decimal pagnationRound = Math.Ceiling(Convert.ToDecimal(pagerLength));

                var empDirectoryVm = new EmpDirectoryVm
                {
                    employeeVm = query,
                    empSearch = empSearch,
                    count = totalCount,
                    PagerLength = pagnationRound
                };
                return Json(empDirectoryVm, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult add(int? EmployeeId)
        {
            ViewData["BranchId"] = BRANCHID;
            ViewData["RoleCode"] = ROLECODE;
            var count = salaryStructureHeaderBO.GetListByProperty(x => x.BranchId == BRANCHID && x.IsActive == true).Count();
            ViewData["IsEnable"] = false;
            if (count > 0)
            {
                ViewData["IsEnable"] = true;
            }
            if (EmployeeId != null)
            {

                var empObj = new EmployeeVm();

                empObj.empHeader = empHeaderBO.GetById(EmployeeId.Value);
                empObj.empPersonalDetail = empPersonalDetailBO.GetByProperty(x => x.EmployeeId == EmployeeId.Value);
                empObj.empWorkDetail = empWorkDetailBO.GetByProperty(x => x.EmployeeId == EmployeeId.Value);
                empObj.address = addressBO.GetByProperty(x => x.LinkID == EmployeeId.Value && x.AddressType == UTILITY.EMPLOYEE);
                empObj.empBankdetail = empbankdetailBO.GetByProperty(x => x.EmployeeId == EmployeeId.Value);
                List<EmployeeDocumentDetail> empDocumentDetList = empDocDetailBO.GetAll().ToList();

                var documentTypeLookupids = lookUpBO.GetByAll()
                            .Where(x => x.LookUpCategory == UTILITY.CONFIG_DOCUMENTTYPE)
                            .Select(x => x.LookUpID)
                            .ToList();
                empDocumentDetList = empDocumentDetList
                            .Where(x => x.EmployeeId == EmployeeId.Value &&
                                        documentTypeLookupids.Contains(x.DocumentType))
                            .ToList();

                var codeList = empDocumentDetList.Select(x => x.DocumentType).ToList();

                if (empObj != null)
                {
                    if (empDocumentDetList.Count > 0)
                    {
                        List<EmployeeDocumentVm> docVmList = new List<EmployeeDocumentVm>();
                        foreach (EmployeeDocumentDetail item in empDocumentDetList)
                        {
                            EmployeeDocumentVm docVm = new EmployeeDocumentVm()
                            {
                                DocumentType = item.DocumentType,
                                DocumentDescription = lookUpBO
                                .GetByProperty(y => y.LookUpCategory == UTILITY.CONFIG_DOCUMENTTYPE && y.LookUpID == item.DocumentType)
                                .LookUpDescription,
                                fileName = item.FileName,
                                DocumentDetailId = item.DocumentDetailID
                            };
                            docVmList.Add(docVm);
                        }



                        empObj.empDocument = lookUpBO.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_DOCUMENTTYPE)
                        .Select(y => new EmployeeDocumentVm
                        {
                            DocumentType = y.LookUpID,
                            DocumentDescription = y.LookUpDescription
                        }).Where(x => !codeList.Contains(x.DocumentType)).ToList();
                        empObj.empDocument.AddRange(docVmList);

                        ViewData["empdocumentsPath"] = "Uploads/" + empObj.empHeader.EmployeeId + "/";


                    }
                    else
                    {
                        empObj.empDocument = lookUpBO.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_DOCUMENTTYPE)
                        .Select(y => new EmployeeDocumentVm
                        {
                            DocumentType = y.LookUpID,
                            DocumentDescription = y.LookUpDescription
                        }).ToList();
                    }
                }
                return View(empObj);
            }
            else
            {
                var documentTypes = lookUpBO.GetListByProperty(y => y.LookUpCategory == UTILITY.CONFIG_DOCUMENTTYPE)
                                        .Select(x => new EmployeeDocumentVm
                                        {
                                            DocumentType = x.LookUpID,
                                            DocumentDescription = x.LookUpDescription
                                        }).ToList();
                return View(new EmployeeVm
                {
                    empHeader = new EmployeeHeader
                    { EmployeeId = -1, IsActive = true },
                    empDocument = documentTypes,
                    empPersonalDetail = new EmployeePersonalDetail() { Gender = 101 }
                });
            }
        }


        [HttpPost]
        public ActionResult saveemployee(EmployeeVm empVm)
        {

            try
            {
                if (empVm.empHeader.EmployeeId == -1)
                {
                    empHeaderBO.SaveEmployeeVm(empVm);
                    userBo.AddUserVm(empVm);
                    leaveTransBO.AddLeave(empVm.empHeader.EmployeeId);
                    return RedirectToAction("employeedirectory");
                }
                else
                {
                    empHeaderBO.SaveEmployeeVm(empVm);
                    userBo.UpdateUserVm(empVm);
                    leaveTransBO.UpdateLeave(empVm.empHeader.EmployeeId);
                    return RedirectToAction("employeedirectory");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool IsEmailExists(string Email, int Employeeid)
        {

            if (Employeeid == -1 && Email != "")
            {
                var list = empHeaderBO.GetListByProperty(x => (x.UserEmailId.ToLower() == Email.ToLower())).ToList();
                int count = list.Count();
                return (count > 0 ? true : false);
            }
            else if (Email == "")
            {
                return false;
            }
            else
            {
                var list = empHeaderBO.GetListByProperty(x => (x.UserEmailId.ToLower() == Email.ToLower())).Where(x => x.EmployeeId != Employeeid).ToList();
                int count = list.Count();
                return (count > 0 ? true : false);
            }

        }
        public ActionResult DeleteEmployeeDocs(int docdetailid)
        {
            var empdetails = empDocDetailBO.GetById(docdetailid);
            var EmployeeId = empdetails.EmployeeId;
            empDocDetailBO.Delete(docdetailid);
            return RedirectToAction("add", "Employee", new { EmployeeId = EmployeeId });

        }

        public ActionResult EmployeeDelete(int EmployeeId)
        {
            using (var dbCntx = new HrDataContext())
            {
                var result = dbCntx.usp_EmployeeDelete(BRANCHID,EmployeeId);
            }
            return RedirectToAction("employeedirectory");
        }

        /*
        private EmployeePersonalDetail PrepareEmployeePersonalInfo(EmployeePersonalDetail employeePersonalInfo, EmployeeHeader employeeHeader)
        {
            EmployeePersonalDetail _employeePersonalInfo = null;
            if (employeePersonalInfo.Id > 0)
            {
                _employeePersonalInfo = employeeHeader.EmployeePersonalInfo.FirstOrDefault();
                _employeePersonalInfo.ModifiedBy = USER_OBJECT.UserID;
                _employeePersonalInfo.ModifiedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            else
            {
                _employeePersonalInfo = new EmployeePersonalInfo();
                _employeePersonalInfo.CreatedBy = USER_OBJECT.UserName;
                _employeePersonalInfo.CreatedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            _employeePersonalInfo.BranchId = employeePersonalInfo.BranchId;
            //_employeePersonalInfo.EmployeeId = employeeHeader.Id;
            _employeePersonalInfo.DOB = DateTimeConverter.SingaporeDateTimeConversion(Convert.ToDateTime(employeePersonalInfo.DOB));
            _employeePersonalInfo.Gender = employeePersonalInfo.Gender;
            _employeePersonalInfo.FatherName = employeePersonalInfo.FatherName;
            _employeePersonalInfo.BirthCountry = employeePersonalInfo.BirthCountry;
            _employeePersonalInfo.MaritalStatus = employeePersonalInfo.MaritalStatus;
            _employeePersonalInfo.SpouseName = employeePersonalInfo.SpouseName;
            _employeePersonalInfo.EmergencyContactNumber = employeePersonalInfo.EmergencyContactNumber;
            _employeePersonalInfo.EmergencyContactName = employeePersonalInfo.EmergencyContactName;
            if (employeePersonalInfo.MarriageDate.HasValue)
                _employeePersonalInfo.MarriageDate = DateTimeConverter.SingaporeDateTimeConversion(employeePersonalInfo.MarriageDate.Value);
            else
                _employeePersonalInfo.MarriageDate = null;
            _employeePersonalInfo.ResidentialStatus = employeePersonalInfo.ResidentialStatus;

            return _employeePersonalInfo;
        }

        private Address PrepareEmployeeAddress(Address address, EmployeeHeader employeeHeader = null)
        {
            Address _address = null;
            if (address.AddressId > 0)
            {
                _address = employeeHeader != null ? employeeHeader.Address.FirstOrDefault() : address;
                _address.ModifiedBy = USER_OBJECT.UserName;
                _address.ModifiedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            else
            {
                _address = new Address();
                _address.CreatedBy = USER_OBJECT.UserName;
                _address.CreatedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            _address.Address1 = address.Address1.ToUpper();
            _address.Address2 = !string.IsNullOrWhiteSpace(address.Address2) ? address.Address2.ToUpper() : string.Empty;
            //_address.AddressLinkID = !string.IsNullOrWhiteSpace(address.AddressLinkID) ? address.AddressLinkID : string.Empty;
            _address.SeqNo = 0;
            _address.CityName = address.CityName.ToUpper();
            _address.StateName = !string.IsNullOrWhiteSpace(address.StateName) ? address.StateName.ToUpper() : string.Empty;
            _address.ZipCode = address.ZipCode;
            _address.MobileNo = address.MobileNo;
            _address.CountryCode = address.CountryCode;
            _address.AddressType = "Employee";
            _address.Contact = address.MobileNo;
            _address.Email = !string.IsNullOrWhiteSpace(address.Email) ? address.Email : string.Empty;
            _address.IsActive = true;
            return _address;
        }

        private EmployeeWorkDetail PrepareEmployeeWorkDetail(EmployeeWorkDetail employeeWorkDetail, EmployeeHeader employeeHeader)
        {
            EmployeeWorkDetail _employeeWorkDetail = null;
            if (employeeWorkDetail.Id > 0)
            {
                _employeeWorkDetail = employeeHeader.EmployeeWorkDetail.FirstOrDefault();
                _employeeWorkDetail.ModifiedBy = USER_OBJECT.UserName;
                _employeeWorkDetail.ModifiedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            else
            {
                _employeeWorkDetail = new EmployeeWorkDetail();
                _employeeWorkDetail.CreatedBy = USER_OBJECT.UserName;
                _employeeWorkDetail.CreatedOn = DateTimeConverter.SingaporeDateTimeConversion(DateTime.Now);
            }
            _employeeWorkDetail.BranchId = employeeWorkDetail.BranchId;
            //_employeeWorkDetail.EmployeeId = employeeHeader.Id;
            _employeeWorkDetail.JoiningDate = employeeWorkDetail.JoiningDate.HasValue ? DateTimeConverter.SingaporeDateTimeConversion(employeeWorkDetail.JoiningDate.Value) : DateTime.Now;
            _employeeWorkDetail.ConfirmationDate = employeeWorkDetail.ConfirmationDate.HasValue ? DateTimeConverter.SingaporeDateTimeConversion(employeeWorkDetail.ConfirmationDate.Value) : DateTime.Now;
            _employeeWorkDetail.ProbationPeriod = employeeWorkDetail.ProbationPeriod;
            _employeeWorkDetail.NoticePeriod = employeeWorkDetail.NoticePeriod;
            _employeeWorkDetail.DesignationId = employeeWorkDetail.DesignationId;
            _employeeWorkDetail.DepartmentId = employeeWorkDetail.DepartmentId;
            _employeeWorkDetail.ResignationDate = employeeWorkDetail.ResignationDate;
            return _employeeWorkDetail;
        }
        */


    }
}