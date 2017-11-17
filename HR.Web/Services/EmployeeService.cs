using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HR.Web.Models;
using HR.Web.ViewModels;
using HR.Web;

namespace HR.Web.Services
{
    //public class EmployeeService
    //{
    //    private void Save(HrDataContext dbCntx, EmployeeVm empVm)
    //    {
    //        if (empVm.empHeader.EmployeeId == -1)
    //        {
                
    //                var empHdr = new EmployeeHeader
    //                {
    //                    BranchId = BRANCHID,
    //                    FirstName = empVm.empHeader.FirstName,
    //                    MiddleName = empVm.empHeader.MiddleName,
    //                    LastName = empVm.empHeader.LastName,
    //                    Nationality = empVm.empHeader.Nationality,
    //                    IDType = empVm.empHeader.IDType,
    //                    IDNumber = "",
    //                    UserEmailId = empVm.empHeader.UserEmailId,
    //                    Password = empVm.empHeader.Password,
    //                    ConfirmPassword = empVm.empHeader.ConfirmPassword,
    //                    IsActive = true,
    //                    CreatedBy = USERID,
    //                    CreatedOn = UTILITY.SINGAPORETIME,
    //                    //ModifiedBy = USERID,
    //                    //ModifiedOn = UTILITY.SINGAPORETIME,
    //                    //ManagerId = 0,
    //                    //UserId = 0

    //                };
    //                dbCntx.EmployeeHeaders.Add(empHdr);
    //                var empPerDetail = new EmployeePersonalDetail
    //                {
    //                    EmployeeId = empHdr.EmployeeId,
    //                    BranchId = BRANCHID,
    //                    DOB = empVm.empPersonalDetail.DOB,
    //                    Gender = empVm.empPersonalDetail.Gender, //
    //                    BirthCountry = empVm.empPersonalDetail.BirthCountry, //
    //                    CitizenshipCountry = "", //
    //                    FatherName = empVm.empPersonalDetail.FatherName,
    //                    MaritalStatus = empVm.empPersonalDetail.MaritalStatus,
    //                    SpouseName = empVm.empPersonalDetail.SpouseName, //
    //                    EmergencyContactName = empVm.empPersonalDetail.EmergencyContactName,
    //                    EmergencyContactNumber = empVm.empPersonalDetail.EmergencyContactNumber,
    //                    MarriageDate = empVm.empPersonalDetail.MarriageDate, //
    //                    ResidentialStatus = empVm.empPersonalDetail.ResidentialStatus, //
    //                    CreatedBy = USERID,
    //                    CreatedOn = UTILITY.SINGAPORETIME,
    //                    //ModifiedBy = USERID,
    //                    //ModifiedOn = UTILITY.SINGAPORETIME
    //                };
    //                dbCntx.EmployeePersonalDetails.Add(empPerDetail);
    //                var empWorkDetail = new EmployeeWorkDetail
    //                {
    //                    BranchId = BRANCHID,
    //                    JoiningDate = empVm.empWorkDetail.JoiningDate,
    //                    ConfirmationDate = empVm.empWorkDetail.ConfirmationDate,
    //                    ProbationPeriod = empVm.empWorkDetail.ProbationPeriod,
    //                    NoticePeriod = empVm.empWorkDetail.NoticePeriod,
    //                    DesignationId = empVm.empWorkDetail.DesignationId,
    //                    DepartmentId = empVm.empWorkDetail.DepartmentId,
    //                    ResignationDate = empVm.empWorkDetail.ResignationDate,
    //                    CreatedBy = USERID,
    //                    CreatedOn = UTILITY.SINGAPORETIME,
    //                    //ModifiedBy = USERID,
    //                    //ModifiedOn = UTILITY.SINGAPORETIME
    //                };
    //                dbCntx.EmployeeWorkDetails.Add(empWorkDetail);
    //                var empAddress = new EmployeeAddress
    //                {
    //                    EmployeeId = empHdr.EmployeeId,
    //                    BranchId = BRANCHID,
    //                    Address1 = empVm.address.Address1,
    //                    Address2 = empVm.address.Address2,
    //                    SeqNo = 0, //
    //                    CityName = empVm.address.CityName,
    //                    StateName = empVm.address.StateName,
    //                    ZipCode = empVm.address.ZipCode,
    //                    MobileNo = empVm.address.MobileNo,
    //                    CountryCode = empHdr.Nationality,
    //                    AddressType = "Employee",
    //                    Contact = empVm.address.MobileNo,
    //                    Email = empHdr.UserEmailId,
    //                    IsActive = true,
    //                    CreatedBy = USERID,
    //                    CreatedOn = UTILITY.SINGAPORETIME,
    //                    //ModifiedBy = USERID,
    //                    //ModifiedOn = UTILITY.SINGAPORETIME
    //                };
    //                dbCntx.EmployeeAddresses.Add(empAddress);

    //                dbCntx.SaveChanges();
    //            }
    //        }
    //        else
    //        {

    //        }
                
    //}
}