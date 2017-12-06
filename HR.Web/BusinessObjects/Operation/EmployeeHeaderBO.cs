using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeHeaderBO : BaseBO
    {
        EmployeeHeaderService employeeHeaderService = null;

        public EmployeeHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeHeaderService = new EmployeeHeaderService();
        }

        public void Add(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Update(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeeHeader entity)
        {
            try
            {
                employeeHeaderService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeeHeader GetById(int id)
        {
            try
            {
                return employeeHeaderService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeHeader> GetAll()
        {
            try
            {
                return employeeHeaderService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}