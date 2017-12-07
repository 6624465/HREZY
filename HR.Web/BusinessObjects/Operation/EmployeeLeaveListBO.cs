using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeLeaveListBO : BaseBO
    {
        EmployeeLeaveListService employeeLeaveListService = null;
        public EmployeeLeaveListBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeLeaveListService = new EmployeeLeaveListService();
        }

        public void Add(EmployeeLeaveList empLeaveList)
        {
            try
            {
                employeeLeaveListService.Add(empLeaveList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeLeaveList empLeaveList)
        {
            try
            {
                employeeLeaveListService.Delete(empLeaveList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeLeaveList GetById(int id)
        {
            try
            {
                return employeeLeaveListService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeLeaveList> GetAll(int id)
        {
            try
            {
                return employeeLeaveListService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}