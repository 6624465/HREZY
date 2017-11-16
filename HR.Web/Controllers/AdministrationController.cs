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
    [Authorize]
    public class AdministrationController : Controller
    {
        // GET: Administration
        [HttpGet]
        public ActionResult Company()
        {
            CompanyVm companyvm = new CompanyVm();
            companyvm.companyAddress = new AddressVm();
            return View(companyvm);
        }
        [HttpPost]
        public ActionResult Company(CompanyVm companyVM, AddressVm addressVm)
        {
            try
            {

                using (HrDataContext dbContext = new HrDataContext())
                {
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
                        companyVM.company.Logo.SaveAs(path + company.CompanyLogo);
                    }

                    company.RegNo = companyVM.company.RegNo;
                    company.ModifiedBy = "Admin";
                    company.ModifiedOn = DateTime.Now;


                    Address companyAddress = new Address()
                    {
                        Address1 = addressVm.Address1,
                        Address2 = addressVm.Address2,

                        AddressType = "Company",
                        CityName = addressVm.CityName,
                        Contact = addressVm.Contact,
                        CountryCode = addressVm.CountryCode,
                        LinkID = company.CompanyId,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now,
                        Email = addressVm.Email,
                        FaxNo = addressVm.FaxNo,
                        MobileNo = addressVm.MobileNo,
                        IsActive = true,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        SeqNo = addressVm.SeqNo,
                        StateName = addressVm.StateName,
                        TelNo = addressVm.TelNo,
                        WebSite = addressVm.WebSite,
                        ZipCode = addressVm.WebSite,
                    };

                    dbContext.Companies.Add(company);
                    dbContext.Addresses.Add(companyAddress);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        public ActionResult AddBranch()
        {
            BranchVm branchVm = new BranchVm();
            branchVm.address = new AddressVm();
            return View(branchVm);
        }

        [HttpPost]
        public ActionResult AddBranch(BranchVm branchVm, AddressVm addressVm)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Branch branch = new Branch()
                    {
                        BranchCode = branchVm.branch.BranchCode,
                        BranchName = branchVm.branch.BranchName,
                        RegNo = branchVm.branch.RegNo,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        CompanyCode = "EZY",
                        CompanyId = 1000
                    };

                    Address branchAddress = new Address()
                    {
                        Address1 = addressVm.Address1,
                        Address2 = addressVm.Address2,

                        AddressType = "Company",
                        CityName = addressVm.CityName,
                        Contact = addressVm.Contact,
                        CountryCode = addressVm.CountryCode,
                        LinkID = branch.BranchID,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now,
                        Email = addressVm.Email,
                        FaxNo = addressVm.FaxNo,
                        MobileNo = addressVm.MobileNo,
                        IsActive = true,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        SeqNo = addressVm.SeqNo,
                        StateName = addressVm.StateName,
                        TelNo = addressVm.TelNo,
                        WebSite = addressVm.WebSite,
                        ZipCode = addressVm.WebSite,
                    };
                    dbContext.Branches.Add(branch);
                    dbContext.Addresses.Add(branchAddress);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("AddBranch");
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<AuthorViewModel> model = new List<AuthorViewModel>();

            AuthorViewModel firstAuthor = new AuthorViewModel
            {
                Id = 1,
                Name = "John",
                BookViewModel = new List<BookViewModel>{
                    new BookViewModel{
                        Id=1,
                        Title = "JQuery",
                        IsWritten = false
                    }, new BookViewModel{
                        Id=1,
                        Title = "JavaScript",
                        IsWritten = false
                    }
                }
            };

            AuthorViewModel secondAuthor = new AuthorViewModel
            {
                Id = 2,
                Name = "Deo",
                BookViewModel = new List<BookViewModel>{
                    new BookViewModel{
                        Id=3,
                        Title = "C#",
                        IsWritten = false
                    }, new BookViewModel{
                        Id=4,
                        Title = "Entity Framework",
                        IsWritten = false
                    }
                }
            };
            model.Add(firstAuthor);
            model.Add(secondAuthor);
            return View("Index", model);
        }
        public class AuthorViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<BookViewModel> BookViewModel { get; set; }
        }
        public class BookViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public bool IsWritten { get; set; }
        }
    }
}