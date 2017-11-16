using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        [HttpGet]
        public ActionResult Company()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Company(CompanyVm companyVM)
        {
            using(HrDataContext dbContext=new HrDataContext()) { 
            Company company = new Company()
            {
                CompanyCode = companyVM.company.CompanyCode,
                CompanyName = companyVM.company.CompanyName,
                CreatedBy = "Admin",
                CreatedOn = DateTime.Now,
                InCorporationDate = companyVM.company.InCorporationDate,
                IsActive = true,
                CompanyLogo = companyVM.company.CompanyLogo,
                RegNo = companyVM.company.RegNo,
                ModifiedBy = "Admin",
                ModifiedOn = DateTime.Now
            };

            CompanyAddress companyAddress = new CompanyAddress()
            {
                Address1 = companyVM.companyAddress.Address1,
                Address2 = companyVM.companyAddress.Address2,
                BranchID = 1,
                AddressType = "",
                CityName= companyVM.companyAddress.CityName,
                Contact = companyVM.companyAddress.Contact,
                CountryCode = companyVM.companyAddress.CountryCode,
                CompanyId = company.CompanyId,
                CreatedBy="Admin",
                CreatedOn=DateTime.Now,
                Email= companyVM.companyAddress.Email,
                FaxNo = companyVM.companyAddress.FaxNo,
                MobileNo = companyVM.companyAddress.MobileNo,
                IsActive=true,
                ModifiedBy = "Admin",
                ModifiedOn = DateTime.Now,
                SeqNo= companyVM.companyAddress.SeqNo,
                StateName=companyVM.companyAddress.StateName,
                TelNo = companyVM.companyAddress.TelNo,
                WebSite = companyVM.companyAddress.WebSite,
                ZipCode = companyVM.companyAddress.WebSite,
            };

                dbContext.Companies.Add(company);
                dbContext.CompanyAddresses.Add(companyAddress);
                dbContext.SaveChanges();
            }

            return View();
        }
    }
}