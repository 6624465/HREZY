using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web;
namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeHeaderBO : BaseBO
    {
        EmployeeHeaderService employeeHeaderService = null;
        EmployeePersonalDetailBO empPersonalDetailBO = null;
        EmployeeDocumentDetailBO empDocDetailBO = null;
        EmployeeWorkDetailBO empWorkDetailBO = null;
        AddressBO addressBO = null;
        EmployeeBankDetailBO empbankdetailBO = null;
        public EmployeeHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeHeaderService = new EmployeeHeaderService();
            empPersonalDetailBO = new EmployeePersonalDetailBO(sessionObj);
            empDocDetailBO = new EmployeeDocumentDetailBO(sessionObj);
            empWorkDetailBO = new EmployeeWorkDetailBO(sessionObj);
            addressBO = new AddressBO(sessionObj);
            empbankdetailBO = new EmployeeBankDetailBO(sessionObj);
        }

        public void SaveEmployeeVm(EmployeeVm empVm)
        {
            if (empVm.empHeader.EmployeeId == -1)
            {
                var empHeader = new EmployeeHeader()
                {
                    BranchId = sessionObj.BRANCHID,
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
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = sessionObj.USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME,
                    ManagerId = empVm.empHeader.ManagerId,
                    IsReportingAuthority = empVm.empHeader.IsReportingAuthority,
                   

                };

                Add(empHeader);
                empVm.empHeader.EmployeeId = empHeader.EmployeeId;
                var empPersonalDetail = new EmployeePersonalDetail
                {
                    EmployeeId = empVm.empHeader.EmployeeId,
                    BranchId = sessionObj.BRANCHID,
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
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = sessionObj.USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME,
                    EPFNO = empVm.empPersonalDetail.EPFNO,
                    PasspostNo = empVm.empPersonalDetail.PasspostNo
                };
                empPersonalDetailBO.Add(empPersonalDetail);
                var empWorkDetail = new EmployeeWorkDetail
                {
                    BranchId = sessionObj.BRANCHID,
                    JoiningDate = empVm.empWorkDetail.JoiningDate,
                    ConfirmationDate = empVm.empWorkDetail.ConfirmationDate,
                    ProbationPeriod = empVm.empWorkDetail.ProbationPeriod,
                    NoticePeriod = empVm.empWorkDetail.NoticePeriod,
                    DesignationId = empVm.empWorkDetail.DesignationId,
                    DepartmentId = empVm.empWorkDetail.DepartmentId,
                    ResignationDate = empVm.empWorkDetail.ResignationDate,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = sessionObj.USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME,
                    EmployeeId = empVm.empHeader.EmployeeId,
                    SendMailsTo=empVm.empWorkDetail.SendMailsTo
                };
                empWorkDetailBO.Add(empWorkDetail);

                var empAddress = new Address
                {
                    LinkID = empHeader.EmployeeId,
                    BranchId = sessionObj.BRANCHID,
                    Address1 = empVm.address.Address1,
                    Address2 = empVm.address.Address2,
                    SeqNo = 0, //
                    CityName = empVm.address.CityName,
                    StateName = empVm.address.StateName,
                    ZipCode = empVm.address.ZipCode,
                    MobileNo = empVm.address.MobileNo,
                    CountryCode = empHeader.Nationality,
                    AddressType = "Employee",
                    Contact = empVm.address.MobileNo,
                    Email = empHeader.UserEmailId,
                    IsActive = true,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    ModifiedBy = sessionObj.USERID,
                    ModifiedOn = UTILITY.SINGAPORETIME
                };
                addressBO.Add(empAddress);
                var empbankdetail = new EmployeeBankdetail
                {
                    EmployeeId =empHeader.EmployeeId,
                    BranchId = sessionObj.BRANCHID,
                    BankName = empVm.empBankdetail.BankName,
                    AccountNo = empVm.empBankdetail.AccountNo,
                    AccountType = empVm.empBankdetail.AccountType,
                    BankBranchCode = empVm.empBankdetail.BankBranchCode,
                    SwiftCode = empVm.empBankdetail.SwiftCode
                };
                empbankdetailBO.Add(empbankdetail);


                foreach (var item in empVm.empDocument)
                {
                    if (item.Document != null && item.Document.ContentLength > 0)
                    {
                        var uidDocument = new EmployeeDocumentDetail
                        {
                            EmployeeId = empHeader.EmployeeId,
                            BranchId = sessionObj.BRANCHID,
                            DocumentType = item.DocumentType,
                            FileName = item.Document.FileName,
                            CreatedBy = sessionObj.USERID,
                            CreatedOn = UTILITY.SINGAPORETIME
                        };

                        empDocDetailBO.Add(uidDocument);

                        string path = HttpContext.Current.Server.MapPath("~/Uploads/" + empHeader.EmployeeId + "/" + uidDocument.DocumentDetailID + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        item.Document.SaveAs(path + item.Document.FileName);

                    }
                }
            }
            else
            {
                empVm.empHeader.IsActive = true;
                empVm.empHeader.CreatedBy = sessionObj.USERID;
                empVm.empHeader.CreatedOn = UTILITY.SINGAPORETIME;
                empVm.empHeader.ModifiedBy = sessionObj.USERID;
                empVm.empHeader.ModifiedOn = UTILITY.SINGAPORETIME;
                Add(empVm.empHeader);

                empVm.empPersonalDetail.CreatedBy = sessionObj.USERID;
                empVm.empPersonalDetail.CreatedOn = UTILITY.SINGAPORETIME;
                empVm.empPersonalDetail.ModifiedBy = sessionObj.USERID;
                empVm.empPersonalDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                empVm.empPersonalDetail.BranchId = empVm.empHeader.BranchId;
                empPersonalDetailBO.Add(empVm.empPersonalDetail);

                empVm.empWorkDetail.CreatedBy = sessionObj.USERID;
                empVm.empWorkDetail.CreatedOn = UTILITY.SINGAPORETIME;
                empVm.empWorkDetail.ModifiedBy = sessionObj.USERID;
                empVm.empWorkDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                empVm.empWorkDetail.BranchId = empVm.empHeader.BranchId;
                empWorkDetailBO.Add(empVm.empWorkDetail);

                empVm.address.CreatedBy = sessionObj.USERID;
                empVm.address.CreatedOn = UTILITY.SINGAPORETIME;
                empVm.address.ModifiedBy = sessionObj.USERID;
                empVm.address.ModifiedOn = UTILITY.SINGAPORETIME;
                empVm.address.LinkID = empVm.empHeader.EmployeeId;
                empVm.address.AddressType = "Employee";
                empVm.address.Contact = empVm.address.MobileNo;
                empVm.address.Email = empVm.empHeader.UserEmailId;
                empVm.address.BranchId = empVm.empHeader.BranchId;
                addressBO.Add(empVm.address);

                empVm.empBankdetail.AccountNo = empVm.empBankdetail.AccountNo;
                empVm.empBankdetail.AccountType = empVm.empBankdetail.AccountType;
                empVm.empBankdetail.BankBranchCode = empVm.empBankdetail.BankBranchCode;
                empVm.empBankdetail.BankName = empVm.empBankdetail.BankName;
                empVm.empBankdetail.SwiftCode = empVm.empBankdetail.SwiftCode;
                empVm.empBankdetail.EmployeeId = empVm.empHeader.EmployeeId;
                empVm.empBankdetail.BranchId = empVm.empHeader.BranchId;
                empbankdetailBO.Add(empVm.empBankdetail);


                foreach (var item in empVm.empDocument)
                {
                    if (item.Document != null && item.Document.ContentLength > 0)
                    {
                        var uidDocument = new EmployeeDocumentDetail
                        {
                            EmployeeId = empVm.empHeader.EmployeeId,
                            BranchId = sessionObj.BRANCHID,
                            DocumentDetailID = item.DocumentDetailId,
                            DocumentType = item.DocumentType,
                            FileName = item.Document.FileName,
                            CreatedBy = sessionObj.USERID,
                            CreatedOn = UTILITY.SINGAPORETIME
                        };

                        empDocDetailBO.Add(uidDocument);

                        string path = HttpContext.Current.Server.MapPath("~/Uploads/" + empVm.empHeader.EmployeeId + "/" + uidDocument.DocumentDetailID + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        item.Document.SaveAs(path + item.Document.FileName);

                    }
                }
            }

        }

        public void Add(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Update(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeeHeader GetById(int id)
        {
            try
            {
                return employeeHeaderService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeHeader> GetAll()
        {
            try
            {
                return employeeHeaderService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IEnumerable<EmployeeHeader> GetListByProperty(Func<EmployeeHeader, bool> predicate)
        {
            try
            {
                return employeeHeaderService.GetListByProperty(predicate).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeHeader GetByProperty(Func<EmployeeHeader, bool> predicate)
        {
            try
            {
                return employeeHeaderService.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}