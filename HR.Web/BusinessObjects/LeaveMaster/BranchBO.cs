using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using HR.Web.ViewModels;
using HR.Web.BusinessObjects.Operation;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class BranchBO : BaseBO
    {
        BranchRepository branchRepository = null;
        AddressBO addressBO = null;
        public BranchBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            branchRepository = new BranchRepository();
            addressBO = new AddressBO(sessionObj);
        }

        public void Add(Branch branch)
        {
            try
            {
                branchRepository.Add(branch);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Branch branch)
        {
            try
            {
                branchRepository.Delete(branch);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Branch GetById(int id)
        {
            try
            {
                return branchRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Branch> GetAll(int id)
        {
            try
            {
                return branchRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void SaveBranch(BranchVm branchVm, AddressVm addressVm)
        {
            Branch branch = new Branch()
            {
                BranchCode = branchVm.branch.BranchCode,
                BranchName = branchVm.branch.BranchName,
                RegNo = branchVm.branch.RegNo,
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                ModifiedBy = sessionObj.USERID,
                ModifiedOn = UTILITY.SINGAPORETIME,
                CompanyCode = "EZY",
                CompanyId = 1000
            };
            Add(branch);
            Address branchAddress = new Address()
            {
                Address1 = addressVm.Address1,
                Address2 = addressVm.Address2,

                AddressType = "Company",
                CityName = addressVm.CityName,
                Contact = addressVm.Contact,
                CountryCode = addressVm.CountryCode,
                LinkID = branch.BranchID,
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
            addressBO.Add(branchAddress);

        }
    }
}