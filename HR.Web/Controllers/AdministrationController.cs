using HR.Web.BusinessObjects.LeaveMaster;
using HR.Web.BusinessObjects.Operation;
using HR.Web.BusinessObjects.Payroll;
using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;


namespace HR.Web.Controllers
{
    [SessionFilter]
    [ErrorHandler]
    public class AdministrationController : BaseController
    {
        AddressBO addressBO = null;
        CompanyBO companyBO = null;
        BranchBO branchBO = null;
        ContributionBO contributionBO = null;
        public AdministrationController()
        {
            addressBO = new AddressBO(SESSIONOBJ);
            companyBO = new CompanyBO(SESSIONOBJ);
            branchBO = new BranchBO(SESSIONOBJ);
            contributionBO = new ContributionBO(SESSIONOBJ);
        }
        // GET: Administration
        [HttpGet]
        public ActionResult Company(int companyId = 0)
        {
            CompanyVm companyvm = new CompanyVm();
            using (HrDataContext dbContext = new HrDataContext())
            {
                //companyId = dbContext.Branches.Where(x => x.BranchID == SESSIONOBJ.BRANCHID).FirstOrDefault()==null?0
                //    : dbContext.Branches.Where(x => x.BranchID == SESSIONOBJ.BRANCHID).FirstOrDefault().CompanyId.Value;
                companyId = 1000;
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
                    companyvm.company.SSFNo = company.SSFNumber;
                    companyvm.company.TaxIdNo = company.TaxIdNumber;
                    companyvm.company.BranchCode = company.BranchCode;


                }
                companyvm.companyAddress = new AddressVm();

                Address address = dbContext.Addresses.Where(x => x.LinkID == companyId && x.AddressType == UTILITY.COMPANY).FirstOrDefault();
                if (address != null)
                {
                    companyvm.companyAddress.Address1 = address.Address1;
                    companyvm.companyAddress.Address2 = address.Address2;
                    companyvm.companyAddress.Address3 = address.Address3;
                    companyvm.companyAddress.Address4 = address.Address4;
                    companyvm.companyAddress.Address5 = address.Address5;
                    companyvm.companyAddress.Address6 = address.Address6;
                    companyvm.companyAddress.Address7 = address.Address7;
                    companyvm.companyAddress.Address8 = address.Address8;
                    companyvm.companyAddress.Address9 = address.Address9;
                    companyvm.companyAddress.Address10 = address.Address10;
                    companyvm.companyAddress.Address11 = address.Address11;
                    companyvm.companyAddress.Address12 = address.Address12;
                    companyvm.companyAddress.Address13 = address.Address13;
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
                companyBO.SaveCompany(companyVM, addressVm);
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
            branchVm.address = new AddressVm();
            using (HrDataContext dbContext = new HrDataContext())
            {
                Branch branch = dbContext.Branches.Where(x => x.BranchID == branchId).FirstOrDefault();
                branchVm.branch = branch;
                Address address = dbContext.Addresses.Where(x => x.LinkID == branchId && x.AddressType == UTILITY.BRANCH).FirstOrDefault();

                if (branchId !=-1 && address != null)
                {
                    branchVm.address.Address1 = address.Address1;
                    branchVm.address.Address2 = address.Address2;
                    branchVm.address.Address3 = address.Address3;
                    branchVm.address.Address4 = address.Address4;
                    branchVm.address.Address5 = address.Address5;
                    branchVm.address.Address6 = address.Address6;
                    branchVm.address.Address7 = address.Address7;
                    branchVm.address.Address8 = address.Address8;
                    branchVm.address.Address9 = address.Address9;
                    branchVm.address.Address10 = address.Address10;
                    branchVm.address.Address11 = address.Address11;
                    branchVm.address.Address12 = address.Address12;
                    branchVm.address.Address13 = address.Address13;
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
                branchBO.SaveBranch(branchVm, addressVm);
                //var list = contributionBO.GetListByProperty(x => (x.BranchId == branchVm.branch.BranchID) && (x.Name==UTILITY.BASICSALARYCOMPONENT)).ToList();
                //var count = list.Count();
                //if (count == 0)
                //{
                //    Contribution contribution = new Contribution
                //    {
                //        Name = UTILITY.BASICSALARYCOMPONENT,
                //        Description = UTILITY.BASICSALARYCOMPONENT,
                //        IsActive = true,
                //        CreatedBy = SESSIONOBJ.USERID,
                //        CreatedOn = UTILITY.SINGAPORETIME,
                //        RegisterCode = "PAYMENTS",
                //        BranchId = branchVm.branch.BranchID,
                //        SortBy = -1
                //    };
                //    contributionBO.Add(contribution);

                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Company");
        }


    }
}