using HR.Web.BusinessObjects.LeaveMaster;
using HR.Web.BusinessObjects.Operation;
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
    [SessionFilter]
    public class AdministrationController : BaseController
    {
        AddressBO addressBO = null;
        CompanyBO companyBO = null;
        BranchBO branchBO = null;
        public AdministrationController()
        {
            addressBO = new AddressBO(SESSIONOBJ);
            companyBO = new CompanyBO(SESSIONOBJ);
            branchBO = new BranchBO(SESSIONOBJ);
        }
        // GET: Administration
        [HttpGet]
        public ActionResult Company(int companyId = 0)
        {
            CompanyVm companyvm = new CompanyVm();
            using (HrDataContext dbContext = new HrDataContext())
            {
                companyId = dbContext.Branches.Where(x => x.BranchID == SESSIONOBJ.BRANCHID).FirstOrDefault()==null?0
                    : dbContext.Branches.Where(x => x.BranchID == SESSIONOBJ.BRANCHID).FirstOrDefault().CompanyId.Value;
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
            using (HrDataContext dbContext = new HrDataContext())
            {
                
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
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("AddBranch");
        }

      
    }
}