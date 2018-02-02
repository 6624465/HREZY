using System;
using HR.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;
using HR.Web.Services.Operation;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeBankDetailBO : BaseBO
    {
        EmployeeBankDetailService empbankdetailservice = null;
        public EmployeeBankDetailBO(SessionObj _sessionobj)
        {
            sessionObj = _sessionobj;
            empbankdetailservice = new EmployeeBankDetailService();
        }
        public void Add(EmployeeBankdetail entity)
        {
            try
            {
                empbankdetailservice.Add(entity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(EmployeeBankdetail entity)
        {
            try
            {
                empbankdetailservice.Delete(entity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<EmployeeBankdetail> GetAll()
        {
            try
            {
                return empbankdetailservice.GetAll();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeBankdetail GetById(int id)
        {
            try
            {
                return empbankdetailservice.GetById(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
} 