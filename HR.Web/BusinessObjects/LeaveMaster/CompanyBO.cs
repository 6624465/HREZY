using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class CompanyBO : BaseBO
    {
        CompanyRepository companyRepository = null;
        public CompanyBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            companyRepository = new CompanyRepository();
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

        public Company GetById(int id) {
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
    }
}