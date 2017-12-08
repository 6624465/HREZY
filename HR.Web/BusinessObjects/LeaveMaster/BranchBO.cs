using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class BranchBO : BaseBO
    {
        BranchRepository branchRepository = null;

        public BranchBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            branchRepository = new BranchRepository();
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

        public  IEnumerable<Branch> GetAll(int id)
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
    }
}