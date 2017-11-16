using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
                Company company = new Company();

                company.CompanyCode = companyVM.company.CompanyCode;
                company.CompanyName = companyVM.company.CompanyName;
                company.CreatedBy = "Admin";
                company.CreatedOn = DateTime.Now;
                company.InCorporationDate = companyVM.company.InCorporationDate;
                company.IsActive = true;

                if (companyVM.company.Logo.ContentLength > 0)
                {
                    company.CompanyLogo = companyVM.company.Logo.FileName;
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    companyVM.company.Logo.SaveAs(path+ company.CompanyLogo);
                }
              
                company.RegNo = companyVM.company.RegNo;
                company.ModifiedBy = "Admin";
                company.ModifiedOn = DateTime.Now;
            

            Address companyAddress = new Address()
            {
                Address1 = companyVM.companyAddress.Address1,
                Address2 = companyVM.companyAddress.Address2,
              
                AddressType = "",
                CityName= companyVM.companyAddress.CityName,
                Contact = companyVM.companyAddress.Contact,
                CountryCode = companyVM.companyAddress.CountryCode,
                LinkID = company.CompanyId,
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
                dbContext.Addresses.Add(companyAddress);
                dbContext.SaveChanges();
            }

            return View();
        }
    }
}