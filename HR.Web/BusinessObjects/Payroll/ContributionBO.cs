using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class ContributionBO : BaseBO
    {

        ContributionRepository contributionRepository = null;
        public ContributionBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            contributionRepository = new ContributionRepository();
        }

        public void Add(Contribution input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                contributionRepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                contributionRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Contribution GetById(int id)
        {
            try
            {
                return contributionRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Contribution> GetAll()
        {
            try
            {
                return contributionRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Contribution GetByProperty(bool isActive)
        {
            try
            {
                return contributionRepository.GetByProperty(x=>x.IsActive == isActive);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Contribution> GetListByProperty(bool isActive)
        {
            try
            {
                return contributionRepository.GetListByProperty(x => x.IsActive == isActive);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}