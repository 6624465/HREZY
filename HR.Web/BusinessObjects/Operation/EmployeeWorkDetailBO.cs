using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeWorkDetailBO : BaseBO
    {
        EmployeeWorkDetailService employeeWorkDetailService = null;

        public EmployeeWorkDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeWorkDetailService = new EmployeeWorkDetailService();
        }

        public void Add(EmployeeWorkDetail entity)
        {
            try
            {
                employeeWorkDetailService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeeWorkDetail entity)
        {
            try
            {
                employeeWorkDetailService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeeWorkDetail GetById(int id)
        {
            try
            {
                return employeeWorkDetailService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeWorkDetail> GetAll()
        {
            try
            {
                return employeeWorkDetailService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}