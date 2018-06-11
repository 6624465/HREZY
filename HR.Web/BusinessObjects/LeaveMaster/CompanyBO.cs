using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.ViewModels;
using System.IO;
using HR.Web.BusinessObjects.Operation;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class CompanyBO : BaseBO
    {
        CompanyRepository companyRepository = null;
        AddressBO addressBO = null;
        public CompanyBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            companyRepository = new CompanyRepository();
            addressBO = new AddressBO(sessionObj);
        }

        public void Add(Company company)
        {
            try
            {
                companyRepository.Add(company);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Company company)
        {
            try
            {
                companyRepository.Delete(company);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Company GetById(int id)
        {
            try
            {
                return companyRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Company> GetAll(int id)
        {
            try
            {
                return companyRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void SaveCompany(CompanyVm companyVM, AddressVm addressVm)
        {
            Company company = new Company();
            company.CompanyId = companyVM.company.CompanyId;
            company.CompanyCode = companyVM.company.CompanyCode;
            company.CompanyName = companyVM.company.CompanyName;
            company.CreatedBy = sessionObj.USERID;
            company.CreatedOn = UTILITY.SINGAPORETIME;
            company.InCorporationDate = companyVM.company.InCorporationDate;
            company.IsActive = true;
            company.SSFNumber = companyVM.company.SSFNo;
            company.TaxIdNumber = companyVM.company.TaxIdNo;
            company.BranchCode = companyVM.company.BranchCode;

            if (companyVM.company.Logo != null && companyVM.company.Logo.ContentLength > 0)
            {
                company.CompanyLogo = companyVM.company.Logo.FileName;
                string path = HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                companyVM.company.Logo.SaveAs(path + company.CompanyLogo);
            }

            company.RegNo = companyVM.company.RegNo;
            company.ModifiedBy = sessionObj.USERID;
            company.ModifiedOn = UTILITY.SINGAPORETIME;

            Add(company);

            Address companyAddress = new Address()
            {
                Address1 = addressVm.Address1,
                Address2 = addressVm.Address2,
            Address3 = addressVm.Address3,
            Address4 = addressVm.Address4,
            Address5 = addressVm.Address5,
            Address6 = addressVm.Address6,
            Address7 = addressVm.Address7,
            Address8 = addressVm.Address8,
            Address9 = addressVm.Address9,
            Address10 = addressVm.Address10,
            Address11 = addressVm.Address11,
            Address12 = addressVm.Address12,
            Address13 = addressVm.Address13,
            AddressType = UTILITY.COMPANY,
                CityName = addressVm.CityName,
                Contact = addressVm.Contact,
                CountryCode = addressVm.CountryCode,
                LinkID = company.CompanyId,
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                Email = addressVm.Email,
                FaxNo = addressVm.FaxNo,
                MobileNo = addressVm.MobileNo,
                IsActive = true,
                ModifiedBy = sessionObj.USERID,
                ModifiedOn = UTILITY.SINGAPORETIME,
                SeqNo = addressVm.SeqNo,
                StateName = addressVm.StateName,
                TelNo = addressVm.TelNo,
                WebSite = addressVm.WebSite,
                ZipCode = addressVm.ZipCode,
            };

            addressBO.Add(companyAddress);
        }
    }
}