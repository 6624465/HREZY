using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HR.Web.Models;
using HR.Web.ViewModels;

namespace HR.Web.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public ViewResult employeedirectory()
        {
            using (var dbCntx = new HrDataContext())
            {
                var list = dbCntx.EmployeeHeaders
                            .Join(dbCntx.EmployeePersonalDetails,
                            a => a.EmployeeId, b => b.EmployeeId,
                            (a, b) => new { A = a, B = b })
                            .Join(dbCntx.EmployeeWorkDetails,
                            c => c.A.EmployeeId, d => d.EmployeeId,
                            (c, d) => new { C = c, D = d })
                            .Join(dbCntx.EmployeeAddresses,
                            e => e.C.A.EmployeeId, f => f.EmployeeId,
                            (e, f) => new { E = e, F = f })
                            .Select(x => new EmployeeListVm {
                                EmployeeNo = x.E.C.A.IDNumber,
                                EmployeeName = x.E.C.A.FirstName + " " + x.E.C.A.LastName + " " + x.E.C.A.MiddleName,
                                JoiningDate = x.E.D.JoiningDate,
                                JobTitle = dbCntx.LookUps
                                            .Where(y => y.LookUpID == x.E.D.DesignationId)
                                            .FirstOrDefault().LookUpDescription,
                                ContactNo = x.F.Contact,
                                PersonalEmailId = x.F.Email,
                                OfficialEmailId = x.F.Email,
                                DateOfBirth = x.E.C.B.DOB
                            }).ToList().AsEnumerable();

                return View(list);
            }
        }

        [HttpGet]
        public ActionResult add()
        {
            return View(new EmployeeVm { });
        }       

        [HttpPost]
        public ActionResult saveemployee(EmployeeVm empVm)
        {
            using (var dbCntx = new HrDataContext())
            {
                var empHdr = new EmployeeHeader {
                    BranchId = BRANCHID,
                    FirstName = empVm.empHeader.FirstName,
                    MiddleName = empVm.empHeader.MiddleName,
                    LastName = empVm.empHeader.LastName,
                    Nationality = empVm.empHeader.Nationality,
                    IDType = empVm.empHeader.IDType,
                    IDNumber = "",
                    UserEmailId = empVm.empHeader.UserEmailId,
                    Password = empVm.empHeader.Password,
                    ConfirmPassword = empVm.empHeader.ConfirmPassword,
                    IsActive = true,
                    CreatedBy = USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME,
                    //ManagerId = 0,
                    //UserId = 0
                    
                };
                dbCntx.EmployeeHeaders.Add(empHdr);
                var empPerDetail = new EmployeePersonalDetail {
                    EmployeeId = empHdr.EmployeeId,
                    BranchId = BRANCHID,
                    DOB = empVm.empPersonalDetail.DOB,
                    Gender = empVm.empPersonalDetail.Gender, //
                    BirthCountry = empVm.empPersonalDetail.BirthCountry, //
                    CitizenshipCountry = "", //
                    FatherName = empVm.empPersonalDetail.FatherName,
                    MaritalStatus = empVm.empPersonalDetail.MaritalStatus,
                    SpouseName = empVm.empPersonalDetail.SpouseName, //
                    EmergencyContactName = empVm.empPersonalDetail.EmergencyContactName,
                    EmergencyContactNumber = empVm.empPersonalDetail.EmergencyContactNumber,
                    MarriageDate = empVm.empPersonalDetail.MarriageDate, //
                    ResidentialStatus = empVm.empPersonalDetail.ResidentialStatus, //
                    CreatedBy = USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME
                };
                dbCntx.EmployeePersonalDetails.Add(empPerDetail);
                var empWorkDetail = new EmployeeWorkDetail {
                    BranchId = BRANCHID,
                    JoiningDate = empVm.empWorkDetail.JoiningDate,
                    ConfirmationDate = empVm.empWorkDetail.ConfirmationDate,
                    ProbationPeriod = empVm.empWorkDetail.ProbationPeriod,
                    NoticePeriod = empVm.empWorkDetail.NoticePeriod,
                    DesignationId = empVm.empWorkDetail.DesignationId,
                    DepartmentId = empVm.empWorkDetail.DepartmentId,
                    ResignationDate = empVm.empWorkDetail.ResignationDate,
                    CreatedBy = USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME
                };
                dbCntx.EmployeeWorkDetails.Add(empWorkDetail);
                var empAddress = new EmployeeAddress {
                    EmployeeId = empHdr.EmployeeId,
                    BranchId = BRANCHID,
                    Address1 = empVm.address.Address1,
                    Address2 = empVm.address.Address2,
                    SeqNo = 0, //
                    CityName = empVm.address.CityName,
                    StateName = empVm.address.StateName,
                    ZipCode = empVm.address.ZipCode,
                    MobileNo = empVm.address.MobileNo,
                    CountryCode = empHdr.Nationality,
                    AddressType = "Employee",
                    Contact = empVm.address.MobileNo,
                    Email = empHdr.UserEmailId,
                    IsActive = true,
                    CreatedBy = USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME
                };
                dbCntx.EmployeeAddresses.Add(empAddress);

                dbCntx.SaveChanges();
                return View();
            }                
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