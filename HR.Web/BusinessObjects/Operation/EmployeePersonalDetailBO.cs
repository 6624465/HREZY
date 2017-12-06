using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{

    public class EmployeePersonalDetailBO : BaseBO
    {
        EmployeePersonalDetailService empPersonalDetailService = null;
        public EmployeePersonalDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            empPersonalDetailService = new EmployeePersonalDetailService();
        }

        public void Add(EmployeePersonalDetail entity)
        {
            try
            {
                empPersonalDetailService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeePersonalDetail entity)
        {
            try
            {
                empPersonalDetailService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeePersonalDetail GetById(int id)
        {
            try
            {
                return empPersonalDetailService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeePersonalDetail> GetAll()
        {
            try
            {
                return empPersonalDetailService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}