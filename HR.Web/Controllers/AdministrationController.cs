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
        public ActionResult Company(int companyId = 0)
        {
            CompanyVm companyvm = new CompanyVm();
            using (HrDataContext dbContext = new HrDataContext())
            {
                Company company = dbContext.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();

                if (company != null)
                {
                    companyvm.company = new CompanyVmObj();
                    companyvm.company.CompanyCode = company.CompanyCode;
                    companyvm.company.CompanyId = company.CompanyId;
                    companyvm.company.CompanyName = company.CompanyName;
                    companyvm.company.CreatedBy = company.CreatedBy;
                    companyvm.company.CreatedOn = company.CreatedOn;
                    companyvm.company.InCorporationDate = company.InCorporationDate;
                    companyvm.company.IsActive = company.IsActive;
                    companyvm.company.ModifiedBy = company.ModifiedBy;
                    companyvm.company.ModifiedOn = company.ModifiedOn;
                    companyvm.company.RegNo = company.RegNo;


                }
                companyvm.companyAddress = new AddressVm();

                Address address = dbContext.Addresses.Where(x => x.LinkID == companyId).FirstOrDefault();
                if (address != null)
                {
                    companyvm.companyAddress.Address1 = address.Address1;
                    companyvm.companyAddress.Address2 = address.Address2;
                    companyvm.companyAddress.AddressId = address.AddressId;
                    companyvm.companyAddress.AddressType = address.AddressType;
                    companyvm.companyAddress.CityName = address.CityName;
                    companyvm.companyAddress.Contact = address.Contact;
                    companyvm.companyAddress.CountryCode = address.CountryCode;
                    companyvm.companyAddress.CreatedBy = address.CreatedBy;
                    companyvm.companyAddress.CreatedOn = address.CreatedOn;
                    companyvm.companyAddress.Email = address.Email;
                    companyvm.companyAddress.FaxNo = address.FaxNo;
                    companyvm.companyAddress.IsActive = address.IsActive;
                    companyvm.companyAddress.LinkID = address.LinkID;
                    companyvm.companyAddress.MobileNo = address.MobileNo;
                    companyvm.companyAddress.ModifiedBy = address.ModifiedBy;
                    companyvm.companyAddress.ModifiedOn = address.ModifiedOn;
                    companyvm.companyAddress.SeqNo = address.SeqNo;
                    companyvm.companyAddress.StateName = address.StateName;
                    companyvm.companyAddress.TelNo = address.TelNo;
                    companyvm.companyAddress.WebSite = address.WebSite;
                    companyvm.companyAddress.ZipCode = address.ZipCode;
                }

                var result = dbContext.Companies.GroupJoin(dbContext.Branches,
                    a => a.CompanyId, b => b.CompanyId,
                            (a, b) => new { A = a, B = b }).ToList();
                companyvm.companyTreeVm = new List<companyTreeVm>();
                companyTreeVm companyTreeVm = new companyTreeVm();
                foreach (var item in result)
                {
                    companyTreeVm.href = item.A.CompanyId.ToString();
                    companyTreeVm.text = item.A.CompanyName;
                    companyTreeVm.nodes = new List<branchTreeVm>();
                    foreach (var subitem in item.B)
                    {
                        branchTreeVm branchVM = new branchTreeVm()
                        {
                            href = subitem.BranchID.ToString(),
                            text = subitem.BranchName,
                        };
                        companyTreeVm.nodes.Add(branchVM);
                    }
                }
                companyvm.companyTreeVm.Add(companyTreeVm);
            }

            return View(companyvm);
        }
        [HttpPost]
        public ActionResult Company(CompanyVm companyVM, AddressVm addressVm)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    if (companyVM.company.CompanyId == 0)
                    {

                        Company company = new Company();

                        company.CompanyCode = companyVM.company.CompanyCode;
                        company.CompanyName = companyVM.company.CompanyName;
                        company.CreatedBy = "Admin";
                        company.CreatedOn = DateTime.Now;
                        company.InCorporationDate = companyVM.company.InCorporationDate;
                        company.IsActive = true;

                        if (companyVM.company.Logo != null && companyVM.company.Logo.ContentLength > 0)
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
                    }
                    else
                    {

                        Company company = dbContext.Companies.
                            Where(x => x.CompanyId == companyVM.company.CompanyId).FirstOrDefault();

                        company.CompanyCode = companyVM.company.CompanyCode;
                        company.CompanyName = companyVM.company.CompanyName;
                        company.CreatedBy = "Admin";
                        company.CreatedOn = DateTime.Now;
                        company.InCorporationDate = companyVM.company.InCorporationDate;
                        company.IsActive = true;

                        if (companyVM.company.Logo != null && companyVM.company.Logo.ContentLength > 0)
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

                        Address branchAddress = dbContext.Addresses.
                            Where(x => x.LinkID == companyVM.company.CompanyId).FirstOrDefault();

                        branchAddress.Address1 = addressVm.Address1;
                        branchAddress.Address2 = addressVm.Address2;

                        branchAddress.AddressType = "Company";
                        branchAddress.CityName = addressVm.CityName;
                        branchAddress.Contact = addressVm.Contact;
                        branchAddress.CountryCode = addressVm.CountryCode;
                        branchAddress.LinkID = company.CompanyId;
                        branchAddress.CreatedBy = "Admin";
                        branchAddress.CreatedOn = DateTime.Now;
                        branchAddress.Email = addressVm.Email;
                        branchAddress.FaxNo = addressVm.FaxNo;
                        branchAddress.MobileNo = addressVm.MobileNo;
                        branchAddress.IsActive = true;
                        branchAddress.ModifiedBy = "Admin";
                        branchAddress.ModifiedOn = DateTime.Now;
                        branchAddress.SeqNo = addressVm.SeqNo;
                        branchAddress.StateName = addressVm.StateName;
                        branchAddress.TelNo = addressVm.TelNo;
                        branchAddress.WebSite = addressVm.WebSite;
                        branchAddress.ZipCode = addressVm.ZipCode;
                    }

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return RedirectToAction("Company");
        }

        public ActionResult AddBranch(int branchId = 0)
        {
            BranchVm branchVm = new BranchVm();
            using (HrDataContext dbContext = new HrDataContext())
            {

                Branch branch = dbContext.Branches.Where(x => x.BranchID == branchId).FirstOrDefault();
                if (branch != null)
                {
                    branchVm.branch = new Branch();
                    branchVm.branch.AddressID = branch.AddressID;
                    branchVm.branch.BranchCode = branch.BranchCode;
                    branchVm.branch.BranchID = branch.BranchID;
                    branchVm.branch.BranchName = branch.BranchName;
                    branchVm.branch.CompanyCode = branch.CompanyCode;
                    branchVm.branch.CompanyId = branch.CompanyId;
                    branchVm.branch.CreatedBy = branch.CreatedBy;
                    branchVm.branch.CreatedOn = branch.CreatedOn;
                    branchVm.branch.IsActive = branch.IsActive;
                    branchVm.branch.ModifiedBy = branch.ModifiedBy;
                    branchVm.branch.ModifiedOn = branch.ModifiedOn;
                    branchVm.branch.RegNo = branch.RegNo;

                }
                branchVm.address = new AddressVm();
                Address address = dbContext.Addresses.Where(x => x.LinkID == branchId).FirstOrDefault();
                if (address != null)
                {
                    branchVm.address.Address1 = address.Address1;
                    branchVm.address.Address2 = address.Address2;
                    branchVm.address.AddressId = address.AddressId;
                    branchVm.address.AddressType = address.AddressType;
                    branchVm.address.CityName = address.CityName;
                    branchVm.address.Contact = address.Contact;
                    branchVm.address.CountryCode = address.CountryCode;
                    branchVm.address.CreatedBy = address.CreatedBy;
                    branchVm.address.CreatedOn = address.CreatedOn;
                    branchVm.address.Email = address.Email;
                    branchVm.address.FaxNo = address.FaxNo;
                    branchVm.address.IsActive = address.IsActive;
                    branchVm.address.LinkID = address.LinkID;
                    branchVm.address.MobileNo = address.MobileNo;
                    branchVm.address.ModifiedBy = address.ModifiedBy;
                    branchVm.address.ModifiedOn = address.ModifiedOn;
                    branchVm.address.SeqNo = address.SeqNo;
                    branchVm.address.StateName = address.StateName;
                    branchVm.address.TelNo = address.TelNo;
                    branchVm.address.WebSite = address.WebSite;
                    branchVm.address.ZipCode = address.ZipCode;
                }


                var result = dbContext.Companies.GroupJoin(dbContext.Branches,
                    a => a.CompanyId, b => b.CompanyId,
                            (a, b) => new { A = a, B = b }).ToList();
                branchVm.companyTreeVm = new List<companyTreeVm>();
                companyTreeVm companyTreeVm = new companyTreeVm();
                foreach (var item in result)
                {
                    companyTreeVm.href = item.A.CompanyId.ToString();
                    companyTreeVm.text = item.A.CompanyName;
                    companyTreeVm.nodes = new List<branchTreeVm>();
                    foreach (var subitem in item.B)
                    {
                        branchTreeVm branchVM = new branchTreeVm()
                        {
                            href = subitem.BranchID.ToString(),
                            text = subitem.BranchName,
                        };
                        companyTreeVm.nodes.Add(branchVM);
                    }
                }
                branchVm.companyTreeVm.Add(companyTreeVm);
            }
            return View(branchVm);
        }

        [HttpPost]
        public ActionResult AddBranch(BranchVm branchVm, AddressVm addressVm)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    if (branchVm.branch.BranchID == 0)
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
                            ZipCode = addressVm.ZipCode,
                        };
                        dbContext.Branches.Add(branch);
                        dbContext.Addresses.Add(branchAddress);
                    }
                    else
                    {
                        Branch branch = dbContext.Branches.
                            Where(x => x.BranchID == branchVm.branch.BranchID).FirstOrDefault();
                        branch.BranchCode = branchVm.branch.BranchCode;
                        branch.BranchName = branchVm.branch.BranchName;
                        branch.RegNo = branchVm.branch.RegNo;
                        branch.CreatedBy = "Admin";
                        branch.CreatedOn = DateTime.Now;
                        branch.ModifiedBy = "Admin";
                        branch.ModifiedOn = DateTime.Now;
                        branch.CompanyCode = "EZY";
                        branch.CompanyId = 1000;


                        Address branchAddress = dbContext.Addresses.
                            Where(x => x.LinkID == branchVm.branch.BranchID).FirstOrDefault();

                        branchAddress.Address1 = addressVm.Address1;
                        branchAddress.Address2 = addressVm.Address2;

                        branchAddress.AddressType = "Company";
                        branchAddress.CityName = addressVm.CityName;
                        branchAddress.Contact = addressVm.Contact;
                        branchAddress.CountryCode = addressVm.CountryCode;
                        branchAddress.LinkID = branch.BranchID;
                        branchAddress.CreatedBy = "Admin";
                        branchAddress.CreatedOn = DateTime.Now;
                        branchAddress.Email = addressVm.Email;
                        branchAddress.FaxNo = addressVm.FaxNo;
                        branchAddress.MobileNo = addressVm.MobileNo;
                        branchAddress.IsActive = true;
                        branchAddress.ModifiedBy = "Admin";
                        branchAddress.ModifiedOn = DateTime.Now;
                        branchAddress.SeqNo = addressVm.SeqNo;
                        branchAddress.StateName = addressVm.StateName;
                        branchAddress.TelNo = addressVm.TelNo;
                        branchAddress.WebSite = addressVm.WebSite;
                        branchAddress.ZipCode = addressVm.ZipCode;

                    }
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