using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeLeavePolicyBO :BaseBO
    {
        EmployeeLeavePolicyService employeeleavepolicyservice = null;
        public EmployeeLeavePolicyBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            employeeleavepolicyservice = new EmployeeLeavePolicyService();
        }
        public void Add(EmployeeLeavePolicy entity)
        {
            try
            {
                employeeleavepolicyservice.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeeLeavePolicy entity)
        {
            try
            {
                employeeleavepolicyservice.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeeLeavePolicy GetById(int id)
        {
            try
            {
                return employeeleavepolicyservice.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}